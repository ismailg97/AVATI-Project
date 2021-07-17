using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace AVATI.Data
{
    public class ProjectActivityService2 : IProjectActivityService
    {
        private IConfiguration _config;
        private string _connection;

        public ProjectActivityService2(IConfiguration config)
        {
            _config = config;
        }

        public ProjectActivityService2(string connection)
        {
            _connection = connection;
        }

        private IDbConnection GetConnection()
        {
            if (_config != null) return new SqlConnection(_config.GetConnectionString("AVATI-Database"));
            return new SqlConnection(_connection);
        }

        private int GetProjectActivityId(int projectId, int empId, string activity)
        {
            using var db = GetConnection();
            return db.Query<int>(
                "SELECT ProjectActivityID FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND EmployeeID = @emp AND ProjectActivity = @description",
                new {project = projectId, emp = empId, description = activity}).Single();
        }

        /// <summary>
        /// Returns Specific ProjectActivity - Object of given Employee, Project and Activity-Description - Combination (Including Hard- and Softskills)
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="empId">Employee identifier</param>
        /// <param name="activity">Activity identifier</param>
        /// <returns></returns>
        public ProjectActivity GetProjectActivity(int projectId, int empId, string activity)
        {
            using var db = GetConnection();
            var projectActivityId = GetProjectActivityId(projectId, empId, activity);

            var projectActivity = new ProjectActivity()
            {
                ProjectActivityID = projectActivityId,
                ProjectID = projectId,
                EmployeeID = empId,
                Description = activity,
                HardSkills = db.Query<string>(
                    "SELECT Hardskill FROM ProjectActivity_Hardskill WHERE ProjectActivityID = @pr ",
                    new {pr = projectActivityId}).ToList(),
                SoftSkills = db.Query<string>(
                    "SELECT Softskill FROM ProjectActivity_Softskill WHERE ProjectActivityID = @pr ",
                    new {pr = projectActivityId}).ToList(),
            };
            return projectActivity;
        }

        /// <summary>
        /// Checks whether specified Employee is currently in DB of specified Project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="empId"></param>
        /// <returns></returns>
        private bool ExistEmployeeInProject(int projectId, int empId)
        {
            using var db = GetConnection();
            return db.Query<int>(
                "SELECT COUNT(*) FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND EmployeeID = @emp",
                new {project = projectId, emp = empId}).Single() > 0;
        }

        /// <summary>
        /// Checks whether activity exists in Project given ProjectId and Description of activity
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        private bool ExistActivityInProject(int projectId, string activity)
        {
            using var db = GetConnection();
            return db.Query<int>(
                "SELECT COUNT(*) FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND ProjectActivity = @description",
                new {project = projectId, description = activity}).Single() > 0;
        }

        /// <summary>
        /// Deletes entry of Employee with specified EmployeeId and ProjectId in ProjectActivity-Table Where ProjectActivity is null and
        /// entry of ProjectActivity with matching description where EmployeeId is null
        /// adds ProjectActivity to specified Employee and Project in ProjectActivity-Object (including Hard- and Softskills)
        /// </summary>
        /// <param name="activity"></param>
        /// <returns>true if activity was set and other entries were deleted</returns>
        public bool SetProjectActivityToEmployee(ProjectActivity activity)
        {
            using var db = GetConnection();
            int deleteRows;
            deleteRows = db.Execute(
                "DELETE FROM ProjectActivity_Project_Employee WHERE ProjectID = @projectId AND EmployeeID = @empId AND ProjectActivity IS NULL",
                new {projectId = activity.ProjectID, empId = activity.EmployeeID});
            if (deleteRows > 1) return false;

            deleteRows = db.Execute(
                "DELETE FROM ProjectActivity_Project_Employee WHERE ProjectID = @projectId AND EmployeeID IS NULL AND ProjectActivity = @description",
                new {projectId = activity.ProjectID, description = activity.Description});
            if (deleteRows > 1) return false;

            var insertRows = db.Execute(
                "INSERT INTO ProjectActivity_Project_Employee VALUES(@projectId, @empId, @description)",
                new {projectId = activity.ProjectID, empId = activity.EmployeeID, description = activity.Description});
            if (insertRows != 1) return false;
            var projectActivityId = GetProjectActivityId(activity.ProjectID, activity.EmployeeID, activity.Description);

            foreach (var hardSkill in activity.HardSkills)
            {
                insertRows = db.Execute("INSERT INTO ProjectActivity_Hardskill VALUES(@projectActID, @empId, @skill)",
                    new {projectActID = projectActivityId, empId = activity.EmployeeID, skill = hardSkill});
                if (insertRows != 1) return false;
            }

            foreach (var softSkill in activity.SoftSkills)
            {
                insertRows = db.Execute("INSERT INTO ProjectActivity_Softskill VALUES(@projectActID, @empId, @skill)",
                    new {projectActID = projectActivityId, empId = activity.EmployeeID, skill = softSkill});
                if (insertRows != 1) return false;
            }

            return true;
        }

        /// <summary>
        /// Updates ProjectActivity with specific Id (Compares Hard-/Softskills from DB with given Lists and deletes
        /// Hard-/Softskills from Db that are not in Lists and Adds Hard-/Softskills that are in List)
        /// </summary>
        /// <param name="projectActivityId">Identifier ProjectActivity</param>
        /// <param name="hardSkills">Hardskills to add/keep</param>
        /// <param name="softSkills">Softskills to add/keep</param>
        /// <returns></returns>
        public bool UpdateSkillsToActivity(int projectActivityId, List<string> hardSkills, List<string> softSkills)
        {
            using var db = GetConnection();
            var empId = db.Query<int>(
                "SELECT EmployeeID FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @projectActId",
                new {projectActId = projectActivityId}).Single();

            var addingHardSkills = new List<string>(hardSkills);
            var addingSoftSkills = new List<string>(softSkills);

            var actualHardSkills = db.Query<string>(
                "SELECT Hardskill FROM ProjectActivity_Hardskill WHERE ProjectActivityID = @pr ",
                new {pr = projectActivityId}).ToList();
            var actualSoftSkills = db.Query<string>(
                "SELECT Softskill FROM ProjectActivity_Softskill WHERE ProjectActivityID = @pr ",
                new {pr = projectActivityId}).ToList();

            int insertRows;
            int deleteRows;

            foreach (var hardSkill in actualHardSkills)
            {
                if (!hardSkills.Contains(hardSkill))
                {
                    deleteRows = db.Execute(
                        "DELETE FROM ProjectActivity_Hardskill WHERE ProjectActivityID = @pr AND Hardskill = @skill",
                        new {pr = projectActivityId, skill = hardSkill});
                    if (deleteRows != 1) return false;
                    continue;
                }

                addingHardSkills.Remove(hardSkill);
            }

            foreach (var softSkill in actualSoftSkills)
            {
                if (!softSkills.Contains(softSkill))
                {
                    deleteRows = db.Execute(
                        "DELETE FROM ProjectActivity_Softskill WHERE ProjectActivityID = @pr AND Softskill = @skill",
                        new {pr = projectActivityId, skill = softSkill});
                    if (deleteRows != 1) return false;
                    continue;
                }

                addingSoftSkills.Remove(softSkill);
            }

            foreach (var hardSkill in addingHardSkills)
            {
                insertRows = db.Execute("INSERT INTO ProjectActivity_Hardskill VALUES(@projectActID, @emp, @skill)",
                    new {projectActID = projectActivityId, emp = empId, skill = hardSkill});
                if (insertRows != 1) return false;
            }

            foreach (var softSkill in addingSoftSkills)
            {
                insertRows = db.Execute("INSERT INTO ProjectActivity_Softskill VALUES(@projectActID, @emp, @skill)",
                    new {projectActID = projectActivityId, emp = empId, skill = softSkill});
                if (insertRows != 1) return false;
            }

            return true;
        }
        
        /// <summary>
        /// Deletes specific ProjectActivity including Hard- and Softskills from ProjectActivity_Employee-Table
        /// </summary>
        /// <param name="projectActivityId">Identifier of ProjectActivity</param>
        /// <returns>true if deletion was successful</returns>
        public bool DeleteProjectActivityToEmployee(int projectActivityId)
        {
            using var db = GetConnection();
            db.Execute("DELETE FROM ProjectActivity_Hardskill WHERE ProjectActivityID = @projectActId",
                new {projectActId = projectActivityId});
            db.Execute("DELETE FROM ProjectActivity_Softskill WHERE ProjectActivityID = @projectActId",
                new {projectActId = projectActivityId});
            var projectId = db.Query<int>(
                "SELECT ProjectID FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @projectActId",
                new {projectActId = projectActivityId}).Single();
            var activity = db.Query<string>(
                "SELECT ProjectActivity FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @projectActId",
                new {projectActId = projectActivityId}).Single();
            var empId = db.Query<int>(
                "SELECT EmployeeID FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @projectActId",
                new {projectActId = projectActivityId}).Single();
            var deleteRows = db.Execute(
                "DELETE FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @projectActId",
                new {projectActId = projectActivityId});
            if (deleteRows != 1) return false;
            int insertRows;
            if (!ExistEmployeeInProject(projectId, empId))
            {
                insertRows = db.Execute("INSERT INTO ProjectActivity_Project_Employee VALUES(@project, @emp, null)",
                    new {project = projectId, emp = empId});
                if (insertRows != 1) return false;
            }

            if (!ExistActivityInProject(projectId, activity))
            {
                insertRows = db.Execute(
                    "INSERT INTO ProjectActivity_Project_Employee VALUES(@project, null, @description)",
                    new {project = projectId, description = activity});
                if (insertRows != 1) return false;
            }

            return true;
        }

        /// <summary>
        /// Returns all Activities associated with specified Employee
        /// </summary>
        /// <param name="employeeId">Identifier Employee</param>
        /// <returns>List of Project Activities</returns>
        public List<ProjectActivity> GetProjectActivitiesOfEmployee(int employeeId)
        {
            using var db = GetConnection();

            List<ProjectActivity> activities = new List<ProjectActivity>();
            List<int> tempList = db.Query<int>(
                "SELECT ProjectActivityID FROM ProjectActivity_Project_Employee WHERE EmployeeID = @empId ",
                new {empId = employeeId}).ToList();

            foreach (var activityId in tempList)
            {
                var projectActivity = new ProjectActivity()
                {
                    EmployeeID = employeeId,
                    Description = db.QuerySingle<string>(
                        "SELECT ProjectActivity FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @pr ",
                        new {pr = activityId}),
                    ProjectID = db.QuerySingle<int>(
                        "SELECT ProjectID FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @pr ",
                        new {pr = activityId}),
                    HardSkills = db.Query<string>(
                        "SELECT Hardskill FROM ProjectActivity_Hardskill WHERE ProjectActivityID = @pr ",
                        new {pr = activityId}).ToList(),
                    SoftSkills = db.Query<string>(
                        "SELECT Softskill FROM ProjectActivity_Softskill WHERE ProjectActivityID = @pr ",
                        new {pr = activityId}).ToList(),
                    ProjectActivityID = activityId,
                };
                activities.Add(projectActivity);
            }

            return activities;
        }

        /// <summary>
        /// Groups all ProjectActivities from specified Employee with the respective ProjectID
        /// </summary>
        /// <param name="employeeId">Identifier Employee</param>
        /// <returns>Dictionary of List of ProjectActivities with ProjectID as Keys </returns>
        public Dictionary<int, List<ProjectActivity>> GetActivitiesWithProjectsGrouped(int employeeId)
        {
            using var db = GetConnection();
            var activities = GetProjectActivitiesOfEmployee(employeeId);
            var result = new Dictionary<int, List<ProjectActivity>>();

            foreach (var activity in activities)
            {
                if (result.ContainsKey(activity.ProjectID))
                {
                    if (activity.Description == null) continue;
                    result[activity.ProjectID].Add(activity);
                }
                else
                {
                    result.Add(activity.ProjectID,
                        activity.Description == null
                            ? new List<ProjectActivity>()
                            : new List<ProjectActivity>() {activity});
                }
            }

            return result;
        }

        /// <summary>
        /// Returns just the Description of all Activities associated with specified ProjectID
        /// </summary>
        /// <param name="projectId">Identifier Project</param>
        /// <returns>List of strings</returns>
        public List<string> GetActivitiesDesOfProject(int projectId)
        {
            using var db = GetConnection();
            var result = new List<string>();
            var listWithNull = db.Query<string>(
                "SELECT ProjectActivity FROM ProjectActivity_Project_Employee WHERE ProjectID = @proj ",
                new {proj = projectId}).ToList();
            foreach (var activity in listWithNull.Where(x => x != null && !result.Contains(x)))
            {
                result.Add(activity);
            }

            return result;
        }

        /// <summary>
        /// Groups Employees of specified Project by their respective Activities 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Dictionary of Ids where Activity Description is Key</returns>
        public Dictionary<string, List<int>> GetActivitiesWithEmployeesGrouped(int projectId)
        {
            using var db = GetConnection();
            var result = new Dictionary<string, List<int>>();
            var activities = GetActivitiesDesOfProject(projectId);

            foreach (var activity in activities)
            {
                var count = db.Query<int>(
                    "SELECT COUNT(*) FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND EmployeeID Is Null AND ProjectActivity = @description",
                    new {project = projectId, description = activity}).Single();
                if (count > 0)
                {
                    result.Add(activity, new List<int>());
                }
                else
                {
                    var empList = db.Query<int>(
                        "SELECT EmployeeID FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND ProjectActivity = @description",
                        new {project = projectId, description = activity}).ToList();
                    result.Add(activity, empList);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns List of Activities associated with specific Employee in specific Project
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<ProjectActivity> GetEmployeeProjectActivities(int employeeId, int projectId)
        {
            using var db = GetConnection();

            List<ProjectActivity> activities = new List<ProjectActivity>();
            List<int> tempList = db.Query<int>(
                "SELECT ProjectActivityID FROM ProjectActivity_Project_Employee WHERE EmployeeID = @id AND ProjectID = @proj ",
                new {id = employeeId, proj = projectId}).ToList();

            foreach (var activityId in tempList)
            {
                var projectActivity = new ProjectActivity()
                {
                    EmployeeID = employeeId,
                    Description = db.QuerySingle<string>(
                        "SELECT ProjectActivity FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @pr ",
                        new {pr = activityId}),
                    ProjectID = projectId,
                    HardSkills = db.Query<string>(
                        "SELECT Hardskill FROM ProjectActivity_Hardskill WHERE ProjectActivityID = @pr ",
                        new {pr = activityId}).ToList(),
                    SoftSkills = db.Query<string>(
                        "SELECT Softskill FROM ProjectActivity_Softskill WHERE ProjectActivityID = @pr ",
                        new {pr = activityId}).ToList(),
                    ProjectActivityID = activityId,
                };
                //gewollt?
                if (projectActivity.Description != null)
                    activities.Add(projectActivity);
            }

            return activities;
        }

        /// <summary>
        /// Adds List of ProjectActivities to Project (No relation to Employees given!) with specified Id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="activities"></param>
        /// <returns>true if Update was successful</returns>
        public bool SetProjectActivitiesToProject(int projectId, List<string> activities)
        {
            using var db = GetConnection();
            var copyActivities = new List<string>(activities);
            var actualActivities = GetActivitiesDesOfProject(projectId);
            foreach (var activity in actualActivities)
            {
                if (activities.Exists(x => x == activity))
                {
                    copyActivities.Remove(activity);
                    continue;
                }

                var empList = db.Query<int>(
                    "SELECT EmployeeID FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND ProjectActivity = @description AND EmployeeID IS NOT NULL",
                    new {project = projectId, description = activity}).ToList();

                db.Execute(
                    "DELETE FROM ProjectActivity_Softskill WHERE ProjectActivityID IN (SELECT ProjectActivityID FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND ProjectActivity = @description)",
                    new {project = projectId, description = activity});

                db.Execute(
                    "DELETE FROM ProjectActivity_Hardskill WHERE ProjectActivityID IN (SELECT ProjectActivityID FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND ProjectActivity = @description)",
                    new {project = projectId, description = activity});

                var deleteRows = db.Execute(
                    "DELETE FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND ProjectActivity = @description",
                    new {project = projectId, description = activity});

                if (deleteRows == 0) return false;

                foreach (var employeeId in empList)
                {
                    if (!ExistEmployeeInProject(projectId, employeeId))
                    {
                        db.Execute("INSERT INTO ProjectActivity_Project_Employee VALUES (@project, @emp, null)",
                            new {project = projectId, emp = employeeId});
                    }
                }
            }

            foreach (var activity in copyActivities)
            {
                var insertRows = db.Execute(
                    "INSERT INTO ProjectActivity_Project_Employee VALUES(@project, null, @description)",
                    new {project = projectId, description = activity});
                if (insertRows != 1) return false;
            }

            return true;
        }

        /// <summary>
        /// Creates Activity that is accessible to all Projects
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool AddGlobalProjectActivity(string description)
        {
            using var db = GetConnection();
            return db.Execute("INSERT INTO ProjectActivity VALUES (@activity)",
                new {activity = description}) == 1;
        }

        /// <summary>
        /// Updates ProjectActivity in Global List (Does not alter Description of Activity in Project!)
        /// </summary>
        /// <param name="oldDescription"></param>
        /// <param name="newDescription"></param>
        /// <returns></returns>
        public bool UpdateGlobalProjectActivity(string oldDescription, string newDescription)
        {
            using var db = GetConnection();
            return db.Execute("UPDATE ProjectActivity SET Description = @newD WHERE Description = @oldD",
                new {oldD = oldDescription, newD = newDescription}) == 1;
        }

        /// <summary>
        /// Deletes Activity accessible to all Projects (Does not remove Activity from specific Projects!)
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool DeleteGlobalProjectActivity(string description)
        {
            using var db = GetConnection();
            return db.Execute("DELETE FROM ProjectActivity WHERE Description = @activity",
                new {activity = @description}) == 1;
        }

        public List<string> GetAllGlobalProjectActivities()
        {
            using var db = GetConnection();
            return db.Query<string>("SELECT Description FROM ProjectActivity").ToList();
        }

        public bool AlreadyExistsGlobalActivity(string activity)
        {
            using var db = GetConnection();
            return db.Query<string>("SELECT Description FROM ProjectActivity WHERE Description = @description",
                new {description = activity}).SingleOrDefault() != null;
        }

        public bool AlreadyExistsActivityInProject(int projectId, string activity)
        {
            using var db = GetConnection();
            return db.Query<string>(
                "SELECT ProjectActivity FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND ProjectActivity = @description",
                new {project = projectId, description = activity}).SingleOrDefault() != null;
        }
    }
}