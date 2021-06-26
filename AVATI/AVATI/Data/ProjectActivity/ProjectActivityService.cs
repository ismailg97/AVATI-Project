using System.Collections.Generic;
using System.Data;
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
            var returnVal = db.Query<string>("Select Description from ProjectActivity_Pro");
            return true;
        }

        public bool DeleteProjectActivityEmployee(int EmployeeId, int ProjectId)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteProjectActivity(string Description)
        {
            throw new System.NotImplementedException();
        }

        public List<ProjectActivity> GetEmployeeProjectActivities(int EmployeeId, int ProjectId)
        {
            throw new System.NotImplementedException();
        }

        public List<ProjectActivity> GetProjectActivitiesProject(int ProjectID)
        {
            throw new System.NotImplementedException();
        }

        public List<ProjectActivity> GetAllProjectActivities()
        {
            throw new System.NotImplementedException();
        }

        public List<ProjectActivity> GetProjectActivitiesEmployee(int EmployeeId)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateActivity(string oldDescription, string newDescription)
        {
            throw new System.NotImplementedException();
        }

        public bool AddActivity(string description)
        {
            throw new System.NotImplementedException();
        }
    }
}