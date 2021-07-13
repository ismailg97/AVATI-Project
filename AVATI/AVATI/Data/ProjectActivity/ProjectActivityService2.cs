using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AVATI.Data
{
    public class ProjectActivityService2: IProjectActivityService
    {
        private IConfiguration _config;

        public ProjectActivityService2(IConfiguration config)
        {
            _config = config;
        }
        private IDbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("AVATI-Database"));
        }

        private int GetProjectActivityId(int projectId, int empId, string activity)
        {
            using var db = GetConnection();
            return db.Query<int>("SELECT ProjectActivityID FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND EmployeeID = @emp AND ProjectActivity = @description",
            new{ project = projectId, emp = empId, description = activity}).Single();
        }
        
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

        private bool ExistEmployeeInProject(int projectId, int empId)
        {
            using var db = GetConnection();
            return db.Query<int>("SELECT COUNT(*) FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND EmployeeID = @emp",
                new{ project = projectId, emp = empId}).Single() > 0;
        }

        private bool ExistActivityInProject(int projectId, string activity)
        {
            using var db = GetConnection();
            return db.Query<int>("SELECT COUNT(*) FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND ProjectActivity = @description",
                new{ project = projectId, description = activity}).Single() > 0;
        }

        //sollte richtig sein
        public bool SetProjectActivityToEmployee(ProjectActivity activity)
        {
            using var db = GetConnection();
            int deleteRows;
            deleteRows = db.Execute("DELETE FROM ProjectActivity_Project_Employee WHERE ProjectID = @projectId AND EmployeeID = @empId AND ProjectActivity IS NULL",
                    new {projectId = activity.ProjectID, empId = activity.EmployeeID});
            if (deleteRows > 1) return false;
            
            deleteRows = db.Execute("DELETE FROM ProjectActivity_Project_Employee WHERE ProjectID = @projectId AND EmployeeID IS NULL AND ProjectActivity = @description",
                new {projectId = activity.ProjectID, description = activity.Description});
            if (deleteRows > 1) return false;
            
            var insertRows = db.Execute("INSERT INTO ProjectActivity_Project_Employee VALUES(@projectId, @empId, @description)",
                new {projectId = activity.ProjectID, empId = activity.EmployeeID, description = activity.Description});
            if (insertRows != 1) return false;
            var projectActivityId = GetProjectActivityId(activity.ProjectID, activity.EmployeeID, activity.Description);
            
            foreach (var hardSkill in activity.HardSkills)
            {
                insertRows = db.Execute("INSERT INTO ProjectActivity_Hardskill VALUES(@projectActID, @empId, @skill)",
                    new { projectActID = projectActivityId, empId = activity.EmployeeID, skill = hardSkill});
                if (insertRows != 1) return false;
            }
            
            foreach (var softSkill in activity.SoftSkills)
            {
                insertRows = db.Execute("INSERT INTO ProjectActivity_Softskill VALUES(@projectActID, @empId, @skill)",
                    new { projectActID = projectActivityId, empId = activity.EmployeeID, skill = softSkill});
                if (insertRows != 1) return false;
            }

            return true;
        }

        //sollte richtig sein
        public bool UpdateSkillsToActivity(int projectActivityId, List<string> hardSkills, List<string> softSkills)
        {
            using var db = GetConnection();
            var empId = db.Query<int>("SELECT EmployeeID FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @projectActId",
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
                    deleteRows = db.Execute("DELETE FROM ProjectActivity_Hardskill WHERE ProjectActivityID = @pr AND Hardskill = @skill",
                        new{ pr = projectActivityId, skill = hardSkill});
                    if (deleteRows != 1) return false;
                    continue;
                }

                addingHardSkills.Remove(hardSkill);
            }
            
            foreach (var softSkill in actualSoftSkills)
            {
                if (!softSkills.Contains(softSkill))
                {
                    deleteRows = db.Execute("DELETE FROM ProjectActivity_Softskill WHERE ProjectActivityID = @pr AND Softskill = @skill",
                        new{ pr = projectActivityId, skill = softSkill});
                    if (deleteRows != 1) return false;
                    continue;
                }

                addingSoftSkills.Remove(softSkill);
            }

            foreach (var hardSkill in addingHardSkills)
            {
                insertRows = db.Execute("INSERT INTO ProjectActivity_Hardskill VALUES(@projectActID, @emp, @skill)",
                    new { projectActID = projectActivityId, emp = empId, skill = hardSkill});
                if (insertRows != 1) return false;
            }
            
            foreach (var softSkill in addingSoftSkills)
            {
                insertRows = db.Execute("INSERT INTO ProjectActivity_Softskill VALUES(@projectActID, @emp, @skill)",
                    new { projectActID = projectActivityId, emp = empId, skill = softSkill});
                if (insertRows != 1) return false;
            }

            return true;
        }

        //hoffentlich nicht mehr verwenden
        public bool UpdateProjectActivityToEmployee(ProjectActivity activity)
        {
            using var db = GetConnection();
            var oldActivity = db.Query<string>(
                "SELECT ProjectActivity FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @projectActId",
                new {projectActId = activity.ProjectActivityID}).Single();
            int insertRows;
            if (oldActivity != activity.Description)
            {
                var deleteRows = db.Execute("DELETE FROM ProjectActivity_Project_Employee WHERE ProjectID = @projectId AND EmployeeID IS NULL AND ProjectActivity = @description",
                    new {projectId = activity.ProjectID, description = activity.Description});
                if (deleteRows > 1) return false;
                
                var updateRows = db.Execute(
                    "UPDATE ProjectActivity_Project_Employee SET ProjectActivity = @newDescription WHERE ProjectActivityID = @projectActivityId",
                    new { newDescription = activity.Description, projectActivityId = activity.ProjectActivityID });
                if (updateRows != 1) return false;
                
                if (!ExistActivityInProject(activity.ProjectID, oldActivity))
                {
                    insertRows = db.Execute(
                        "INSERT INTO ProjectActivity_Project_Employee VALUES(@project, null, @description)",
                        new {project = activity.ProjectID, description = oldActivity});
                    if (insertRows != 1) return false;
                }
            }
            
            db.Execute("DELETE FROM ProjectActivity_Hardskill WHERE ProjectActivityID = @projectActId",
                new { projectActId = activity.ProjectActivityID });
            db.Execute("DELETE FROM ProjectActivity_Softskill WHERE ProjectActivityID = @projectActId",
                new { projectActId = activity.ProjectActivityID });

            foreach (var hardSkill in activity.HardSkills)
            {
                insertRows = db.Execute("INSERT INTO ProjectActivity_Hardskill VALUES(@projectActID, @empId, @skill)",
                    new { projectActID = activity.ProjectActivityID, empId = activity.EmployeeID, skill = hardSkill});
                if (insertRows != 1) return false;
            }
            
            foreach (var softSkill in activity.SoftSkills)
            {
                insertRows = db.Execute("INSERT INTO ProjectActivity_Softskill VALUES(@projectActID, @empId, @skill)",
                    new { projectActID = activity.ProjectActivityID, empId = activity.EmployeeID, skill = softSkill});
                if (insertRows != 1) return false;
            }

            return true;
        }

        public bool DeleteProjectActivityToEmployee(int projectActivityId)
        {
            using var db = GetConnection();
            db.Execute("DELETE FROM ProjectActivity_Hardskill WHERE ProjectActivityID = @projectActId",
                new { projectActId = projectActivityId });
            db.Execute("DELETE FROM ProjectActivity_Softskill WHERE ProjectActivityID = @projectActId",
                new { projectActId = projectActivityId });
            var projectId = db.Query<int>("SELECT ProjectID FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @projectActId",
                new{ projectActId = projectActivityId}).Single();
            var activity = db.Query<string>("SELECT ProjectActivity FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @projectActId",
                new{ projectActId = projectActivityId}).Single();
            var empId = db.Query<int>("SELECT EmployeeID FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @projectActId",
                new{ projectActId = projectActivityId}).Single();
            var deleteRows = db.Execute("DELETE FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @projectActId",
                new{ projectActId = projectActivityId});
            if (deleteRows != 1) return false;
            int insertRows;
            if (!ExistEmployeeInProject(projectId, empId))
            {
                insertRows = db.Execute("INSERT INTO ProjectActivity_Project_Employee VALUES(@project, @emp, null)",
                    new { project = projectId, emp = empId});
                if (insertRows != 1) return false;
            }

            if (!ExistActivityInProject(projectId, activity))
            {
                insertRows = db.Execute("INSERT INTO ProjectActivity_Project_Employee VALUES(@project, null, @description)",
                    new { project = projectId, description = activity});
                if (insertRows != 1) return false;
            }

            return true;
        }

        public List<ProjectActivity> GetProjectActivitiesOfEmployee(int employeeId)
        {
            using var db = GetConnection();
            
            List<ProjectActivity> activities = new List<ProjectActivity>();
            List<int> tempList = db.Query<int>(
                "SELECT ProjectActivityID FROM ProjectActivity_Project_Employee WHERE EmployeeID = @empId ", 
                new { empId = employeeId }).ToList();
            
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
                //gewollt?
                /*if(projectActivity.Description != null)*/
                    activities.Add(projectActivity);
            }
            
            return activities;
        }

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
                        activity.Description == null ? new List<ProjectActivity>() : new List<ProjectActivity>() {activity});
                }
            }

            return result;
        }

        public List<string> GetActivitiesDesOfProject(int projectId)
        {
            using var db = GetConnection();
            var result = new List<string>();
            var listWithNull =  db.Query<string>(
                "SELECT ProjectActivity FROM ProjectActivity_Project_Employee WHERE ProjectID = @proj ", 
                new { proj = projectId }).ToList();
            foreach (var activity in listWithNull.Where(x => x != null && !result.Contains(x)))
            {
                result.Add(activity);
            }

            return result;
        }

        public Dictionary<string, List<int>> GetActivitiesWithEmployeesGrouped(int projectId)
        {
            using var db = GetConnection();
            var result = new Dictionary<string, List<int>>();
            var activities = GetActivitiesDesOfProject(projectId);

            foreach (var activity in activities)
            {
                var count = db.Query<int>("SELECT COUNT(*) FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND EmployeeID Is Null AND ProjectActivity = @description",
                    new{ project = projectId, description = activity}).Single();
                if (count > 0)
                {
                    result.Add(activity, new List<int>());
                }
                else
                {
                    var empList = db.Query<int>("SELECT EmployeeID FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND ProjectActivity = @description",
                        new{ project = projectId, description = activity}).ToList();
                    result.Add(activity, empList);
                }
            }

            return result;
        }

        public List<ProjectActivity> GetEmployeeProjectActivities(int employeeId, int projectId)
        {
            using var db = GetConnection();
            
            List<ProjectActivity> activities = new List<ProjectActivity>();
            List<int> tempList = db.Query<int>(
                "SELECT ProjectActivityID FROM ProjectActivity_Project_Employee WHERE EmployeeID = @id AND ProjectID = @proj ", 
                new { id = employeeId, proj = projectId }).ToList();
            
            foreach (var activityId in tempList)
            {
                var projectActivity = new ProjectActivity()
                {
                    EmployeeID = employeeId, 
                    Description = db.QuerySingle<string>(
                        "SELECT ProjectActivity FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @pr ", 
                        new { pr = activityId }),
                    ProjectID = projectId,
                    HardSkills = db.Query<string>("SELECT Hardskill FROM ProjectActivity_Hardskill WHERE ProjectActivityID = @pr ", 
                        new { pr = activityId }).ToList(),
                    SoftSkills = db.Query<string>("SELECT Softskill FROM ProjectActivity_Softskill WHERE ProjectActivityID = @pr ", 
                        new { pr = activityId }).ToList(),
                    ProjectActivityID = activityId,
                };
                //gewollt?
                if(projectActivity.Description != null)
                    activities.Add(projectActivity);
            }
            
            return activities;
        }

        public string GetProjectActivityForPurpose(string purpose)
        {
            using IDbConnection db = GetConnection();
            
            string temp1 = db.QuerySingle<string>(
                "SELECT Projectactivity FROM ProjectPurpose WHERE Purpose = @desc", 
                new { desc = purpose });

            return temp1;
        }

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
                

                var deleteRows = db.Execute("DELETE FROM ProjectActivity_Project_Employee WHERE ProjectID = @project AND ProjectActivity = @description",
                    new{ project = projectId, description = activity});

                if (deleteRows == 0) return false;

                foreach (var employeeId in empList)
                {
                    if (!ExistEmployeeInProject(projectId, employeeId))
                    {
                        db.Execute("INSERT INTO ProjectActivity_Project_Employee VALUES (@project, @emp, null)",
                            new{ project = projectId, emp = employeeId});
                    }
                }
            }

            foreach (var activity in copyActivities)
            {
                var insertRows = db.Execute("INSERT INTO ProjectActivity_Project_Employee VALUES(@project, null, @description)",
                    new{ project = projectId, description = activity});
                if (insertRows != 1) return false;
            }

            return true;
        }

        public bool AddGlobalProjectActivity(string description)
        {
            using var db = GetConnection();
            return db.Execute("INSERT INTO ProjectActivity VALUES (@activity)", 
                new { activity = description }) == 1;
        }

        public bool UpdateGlobalProjectActivity(string oldDescription, string newDescription)
        {
            using var db = GetConnection();
            return db.Execute("UPDATE ProjectActivity SET Description = @oldD WHERE Description = @newD", 
                new { oldD = oldDescription, newD = newDescription}) == 1;
        }

        public bool DeleteGlobalProjectActivity(string description)
        {
            using var db = GetConnection();
            return db.Execute("DELETE FROM ProjectActivity WHERE Description = @activity", 
                new { activity = @description}) == 1;
        }

        public List<string> GetAllGlobalProjectActivities()
        {
            using var db = GetConnection();
            return db.Query<string>("SELECT Description FROM ProjectActivity").ToList();
        }

        public bool IsGlobal(string description)
        {
            using var db = GetConnection();
            return db.Query<string>("SELECT Description FROM ProjectActivity WHERE Description = @activity",
                new{ activity = description }).SingleOrDefault() != null;
        }
    }
}