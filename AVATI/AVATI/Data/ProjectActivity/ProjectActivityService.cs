using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
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

        public bool SetProjectActivity(int EmployeeId, int ProjectId, string Description)
        {
            IDbConnection db = GetConnection();
            db.Open();
            var returnVal =
                db.Query<string>(
                    "Select ProjectActivity from ProjectActivity_Project_Employee WHERE ProjectId = @pro AND EmployeeId = @emp AND ProjectActivity = @desc",
                    new
                    {
                        emp = EmployeeId, pro = ProjectId, desc = Description
                    });
            if (returnVal.FirstOrDefault() != null)
            {
                return false;
            }

            db.Execute("INSERT INTO ProjectActivity_Project_Employee VALUES(@pro, @emp, @desc)",
                new {emp = EmployeeId, pro = ProjectId, desc = Description});
            return true;
        }

        public bool DeleteProjectActivityEmployee(int EmployeeId, int ProjectId, string Description)
        {
            IDbConnection db = GetConnection();
            db.Open();
            var returnVal =
                db.Query<string>(
                    "Select ProjectActivity from ProjectActivity_Project_Employee WHERE ProjectId = @pro AND EmployeeId = @emp AND ProjectActivity = @desc",
                    new
                    {
                        emp = EmployeeId, pro = ProjectId, desc = Description
                    });
            if (returnVal.FirstOrDefault() != null)
            {
                return false;
            }

            db.Execute(
                "8 from ProjectActivity_Project_Employee WHERE ProjectId = @pro AND EmployeeId = @emp AND ProjectActivity = @desc",
                new {emp = EmployeeId, pro = ProjectId, desc = Description});
            return true;
        }

        public bool DeleteProjectActivity(string Description)
        {
            IDbConnection db = GetConnection();
            db.Open();
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
            IDbConnection db = GetConnection();
            db.Open();
            List<ProjectActivity> tempList = new List<ProjectActivity>(db.Query<ProjectActivity>(
                "SELECT * FROM ProjectActivity_Project_Employee WHERE EmployeeId = @emp AND ProjectId = @pro",
                new {emp = EmployeeId, pro = ProjectId}));
            return tempList;
        }

        public List<ProjectActivity> GetProjectActivitiesProject(int ProjectID)
        {
            IDbConnection db = GetConnection();
            db.Open();
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
            IDbConnection db = GetConnection();
            db.Open();
            List<ProjectActivity> tempList = new List<ProjectActivity>(db.Query<ProjectActivity>(
                "SELECT * FROM ProjectActivity"));
            return tempList;
        }

        public List<ProjectActivity> GetProjectActivitiesEmployee(int EmployeeId)
        {
            IDbConnection db = GetConnection();
            db.Open();
            List<ProjectActivity> tempList = new List<ProjectActivity>(db.Query<ProjectActivity>(
                "SELECT ProjectActivity FROM ProjectActivity_Project_Employee WHERE EmployeeID = @emp",
                new {emp = EmployeeId}));
            return tempList;
        }

        public bool UpdateActivity(string oldDescription, string newDescription)
        {
            IDbConnection db = GetConnection();
            db.Open();
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
            IDbConnection db = GetConnection();
            db.Open();
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
            IDbConnection db = GetConnection();
            db.Open();
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
    }
}