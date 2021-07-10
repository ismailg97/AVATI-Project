using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Dapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AVATI.Data
{
    public class ProjectActivityService : IProjectActivityService
    {
        private IConfiguration _config;

        private IDbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("AVATI-Database"));
        }

        public ProjectActivityService(IConfiguration config)
        {
            _config = config;
        }

        public bool SetProjectActivity(int EmpId,int ProjId, ProjectActivity activity)
        {
            using IDbConnection db = GetConnection();
            var returnVal =
                db.Query<string>(
                    "Select ProjectActivity from ProjectActivity_Project_Employee WHERE ProjectId = @pro AND EmployeeId = @emp AND ProjectActivity = @desc",
                    new
                    {
                        emp = EmpId, pro = ProjId, desc = activity.Description
                    });
            //if (returnVal.FirstOrDefault() != null)
            //{
            //    return false;
            //}

            db.Execute("INSERT INTO ProjectActivity_Project_Employee VALUES(@pro, @emp, @desc)",
                new {emp = EmpId, pro = ProjId, desc = activity.Description});

            int ProjectActivityID = db.QuerySingle<int>("SELECT ProjectActivityID FROM ProjectActivity_Project_Employee WHERE ProjectId = @pro AND EmployeeId = @emp AND ProjectActivity = @desc", new{ emp = EmpId, pro = ProjId, desc = activity.Description});

            
            List<string> HardSkillList = new List<string>(db.Query<string>(
                "SELECT Hardskill FROM ProjectActivity_Hardskill WHERE ProjectActivityID=@projActivityID", new { projActivityID= ProjectActivityID}));
            foreach (var hard in HardSkillList)
            {
                if (activity.HardSkills.Find(x => x.Equals(hard)) == null)
                {
                    db.Query("DELETE FROM Employee_Hardskill WHERE Hardskill = @hardsk AND ProjectActivityID=@projActivityID",
                        new {hardsk = hard, projActivityID = activity.ProjectActivityID});
                } 
            }

            foreach (var hardskill in activity.HardSkills)
            {
                if (HardSkillList.Find(x => x.Equals(hardskill)) == null)
                {
                    db.Query(
                        "INSERT INTO ProjectActivity_Hardskill VALUES (@projActivityID, @EmployeeID, @hardskillDesc)",
                        new
                        {
                            projActivityID = ProjectActivityID, EmployeeID = EmpId, hardskillDesc = hardskill
                        });
                }
            }
            
            
            
            List<string> SoftSkillList = new List<string>(db.Query<string>(
                "SELECT Softskill FROM ProjectActivity_Softskill WHERE ProjectActivityID=@projActivityID", new { projActivityID = ProjectActivityID}));
            foreach (var soft in SoftSkillList)
            {
                if (activity.SoftSkills.Find(x => x.Equals(soft)) == null)
                {
                    db.Query("DELETE FROM ProjectActivity_Softskill WHERE Softskill = @softskill AND ProjectActivityID=@projActivityID",
                        new {softskill = soft, projActivityID = activity.ProjectActivityID});
                } 
            }
            
            foreach (var softskill in activity.SoftSkills)
            {
                if (SoftSkillList.Find(x => x.Equals(softskill)) == null)
                {
                    db.Query(
                        "INSERT INTO ProjectActivity_Softskill VALUES (@projActivityID, @EmployeeID, @softskillDesc)",
                        new
                        {
                            projActivityID = ProjectActivityID, EmployeeID = EmpId, softskillDesc = softskill
                        });
                }
            }
            
            return true;
        }

        public bool DeleteProjectActivityEmployee(int ProjectActivityID)
        {
            using IDbConnection db = GetConnection();
            var returnVal =
                db.Query<string>(
                    "Select ProjectActivity from ProjectActivity_Project_Employee WHERE ProjectActivityID = @projActivityID",
                    new
                    {
                        projActivityID = ProjectActivityID
                    });
            if (returnVal.FirstOrDefault() == null)
            {
                return false;
            }
            
            db.Execute("Delete from ProjectActivity_Hardskill WHERE ProjectActivityID = @ProjectActivityId",new
            {
                ProjectActivityId = ProjectActivityID
            });

            db.Execute("Delete from ProjectActivity_Softskill WHERE ProjectActivityID = @ProjectActivityId",new
            {
                ProjectActivityId = ProjectActivityID
            });
            
            db.Execute(
                "Delete from ProjectActivity_Project_Employee WHERE ProjectActivityID = @ProjectActivityId",
                new
                {
                    ProjectActivityId = ProjectActivityID
                });
            return true;
        }

        public bool DeleteProjectActivity(string Description)
        {
            using IDbConnection db = GetConnection();
            var returnVal =
                db.Query<string>(
                    "Select * from ProjectActivity WHERE Description = @desc",
                    new
                    {
                        desc = Description
                    });
            if (returnVal.FirstOrDefault() == null)
            {
                return false;
            }

            db.Execute("Delete from ProjectActivity WHERE Description = @desc",
                new {desc = Description});
            return true;
        }

        public List<ProjectActivity> GetEmployeeProjectActivities(int EmployeeId, int ProjectId)
        {
            using IDbConnection db = GetConnection();
            List<ProjectActivity> tempList = new List<ProjectActivity>(db.Query<ProjectActivity>(
                "SELECT * FROM ProjectActivity_Project_Employee WHERE EmployeeId = @emp AND ProjectId = @pro",
                new {emp = EmployeeId, pro = ProjectId}));
            return tempList;
        }

        public List<ProjectActivity> GetProjectActivitiesProject(int ProjectID)
        {
            using IDbConnection db = GetConnection();
            List<ProjectActivity> toReturn = new List<ProjectActivity>();
            List<string> tempList = new List<string>(db.Query<string>(
                "SELECT ProjectActivity FROM ProjectActivity_Project_Employee WHERE ProjectId = @pro AND ProjectActivity IS NOT NULL",
                new {pro = ProjectID}));
            foreach (var act in tempList)
            {
                toReturn.Add(new ProjectActivity() {Description = act});
            }
            return toReturn;
        }

        public List<ProjectActivity> GetAllProjectActivities()
        {
            using IDbConnection db = GetConnection();
            List<ProjectActivity> tempList = new List<ProjectActivity>(db.Query<ProjectActivity>(
                "SELECT * FROM ProjectActivity"));
            return tempList;
        }

        public List<ProjectActivity> GetProjectActivitiesEmployee(int EmployeeId)
        {
            List<ProjectActivity> projALIst = new List<ProjectActivity>();
            using IDbConnection db = GetConnection();
            List<int> tempList = db.Query<int>(
                "SELECT ProjectActivityID FROM ProjectActivity_Project_Employee WHERE EmployeeID = @id", new
                {
                    id = EmployeeId
                }).ToList();
            foreach (var projA in tempList)
            {
                projALIst.Add(new ProjectActivity()
                {
                    EmployeeID = EmployeeId, 
                    Description = db.QuerySingle<string>(
                        "SELECT ProjectActivity FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @pr ", new
                        {
                            pr = projA
                        }),
                    ProjectID = db.QuerySingle<int>("SELECT ProjectID FROM ProjectActivity_Project_Employee WHERE ProjectActivityID = @pr", new
                    {
                        pr = projA
                    }),
                    HardSkills = db.Query<string>("SELECT Hardskill FROM ProjectActivity_Hardskill WHERE ProjectActivityID = @pr ", new
                    {
                        pr = projA
                    }).ToList(),
                    SoftSkills = db.Query<string>("SELECT Softskill FROM ProjectActivity_Softskill WHERE ProjectActivityID = @pr ", new
                    {
                        pr = projA
                    }).ToList(),
                    ProjectActivityID = projA
                });
                
            }
            return projALIst;
        }

        public bool UpdateActivity(string oldDescription, string newDescription)
        {
            using IDbConnection db = GetConnection();
            var returnVal =
                db.Query<string>(
                    "Select Description from ProjectActivity WHERE Description = @desc",
                    new
                    {
                        desc = oldDescription
                    });
            if (returnVal.FirstOrDefault() == null)
            {
                return false;
            }

            db.Execute(
                "Update ProjectActivity SET Description = @newDes WHERE Description = @old",
                new {newDes = newDescription, old = oldDescription});
            return true;
        }

        public bool AddActivity(string description)
        {
            using IDbConnection db = GetConnection();
            var returnVal =
                db.Query<string>(
                    "Select Description from ProjectActivity WHERE Description = @desc",
                    new
                    {
                        desc = description
                    });
            if (returnVal.FirstOrDefault() != null)
            {
                return false;
            }
            db.Execute(
                "INSERT INTO ProjectActivity VALUES(@newDes)",
                new {newDes = description});
            return true;
        }

        public bool UpdateProjectActivity(int proposalId, List<ProjectActivity> activities)
        {
            using IDbConnection db = GetConnection();
            foreach (var activity in activities)
            {
                Console.WriteLine("Do we enter here1");
                var returnVal =
                    db.Query<string>(
                        "Select ProjectActivity from ProjectActivity_Project_Employee WHERE ProjectActivity = @desc AND ProjectID = @proId",
                        new {desc = activity.Description, proId = proposalId});
                if (returnVal.FirstOrDefault() == null)
                {
                    Console.WriteLine("Do we enter here2");
                    db.Execute(
                        "INSERT INTO ProjectActivity_Project_Employee VALUES(@proId, NULL, @newDes)",
                        new {newDes = activity.Description, proId = proposalId});
                }
            }
            var tempList = new List<string>(db.Query<string>("SELECT ProjectActivity from ProjectActivity_Project_Employee WHERE  ProjectID = @proId",
                new { proId = proposalId}));
            foreach (var description in tempList)
            {
                Console.WriteLine("Do we enter here3");

                if (activities.Find(e => e.Description == description) == null)
                {
                    Console.WriteLine("Do we enter here4");

                    db.Execute("DELETE FROM ProjectActivity_Project_Employee WHERE ProjectActivity = @desc AND ProjectID = @proId",
                        new {desc = description,  proId = proposalId});
                }
            }

            return true;
        }

        public bool UpdateProjectActivityEmployee(int EmpId,int ProjId, ProjectActivity activity)
        {
            using IDbConnection db = GetConnection();
            db.Query("UPDATE ProjectActivity_Project_Employee SET ProjectActivity=@newDesc WHERE EmployeeID=@empID AND ProjectID=@projID AND ProjectActivityID=@projActivityID ",
                new { projID=ProjId, empID=EmpId, newDesc = activity.Description ,projActivityID=activity.ProjectActivityID});
            
            
            
            List<string> HardSkillList = new List<string>(db.Query<string>(
                "SELECT Hardskill FROM ProjectActivity_Hardskill WHERE EmployeeID =@id AND ProjectActivityID=@projActivityID", new {id = EmpId, projActivityID=activity.ProjectActivityID}));
            foreach (var hard in HardSkillList)
            {
                if (activity.HardSkills.Find(x => x.Equals(hard)) == null)
                {
                    db.Query("DELETE FROM Employee_Hardskill WHERE Hardskill = @hardsk AND EmployeeID=@id AND ProjectActivityID=@projActivityID",
                        new {hardsk = hard, id = EmpId, projActivityID = activity.ProjectActivityID});
                } 
            }

            foreach (var hardskill in activity.HardSkills)
            {
                if (HardSkillList.Find(x => x.Equals(hardskill)) == null)
                {
                    db.Query(
                        "INSERT INTO ProjectActivity_Hardskill VALUES (@projActivityID, @EmployeeID, @hardskillDesc)",
                        new
                        {
                            projActivityID = activity.ProjectActivityID, EmployeeID = EmpId, hardskillDesc = hardskill
                        });
                }
            }
            
            
            
            List<string> SoftSkillList = new List<string>(db.Query<string>(
                "SELECT Softskill FROM ProjectActivity_Softskill WHERE EmployeeID =@id AND ProjectActivityID=@projActivityID", new {id = EmpId, projActivityID = activity.ProjectActivityID}));
            foreach (var soft in SoftSkillList)
            {
                if (activity.SoftSkills.Find(x => x.Equals(soft)) == null)
                {
                    db.Query("DELETE FROM ProjectActivity_Softskill WHERE Softskill = @softskill AND EmployeeID=@id AND ProjectActivityID=@projActivityID",
                        new {softskill = soft, id = EmpId, projActivityID = activity.ProjectActivityID});
                } 
            }
            
            foreach (var softskill in activity.SoftSkills)
            {
                if (SoftSkillList.Find(x => x.Equals(softskill)) == null)
                {
                    db.Query(
                        "INSERT INTO ProjectActivity_Softskill VALUES (@projActivityID, @EmployeeID, @softskillDesc)",
                        new
                        {
                            projActivityID = activity.ProjectActivityID, EmployeeID = EmpId, softskillDesc = softskill
                        });
                }
            }
            
            return true;
        }

    }
}