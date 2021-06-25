using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace AVATI.Data
{
    public class BasicDataService: IBasicDataService
    {
        private readonly IConfiguration _config;
        
        public BasicDataService(IConfiguration config)
        {
            _config = config;
        }

        private IDbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("AVATI-Database"));
        }

        public bool CreateSoftSkill(string description)
        {
            var query = "INSERT INTO Softskill (Description) VALUES (@softskill)";
            var rowsAffected = GetConnection().Execute(query, new {softskill = description});
            return rowsAffected == 1;
        }

        public bool UpdateSoftSkill(string newDescription, string oldDescription)
        {
            var query = "UPDATE Softskill SET Description = @newSoftskill WHERE Description = @oldSoftskill";
            var rowsAffected = GetConnection().Execute(query, new {newSoftskill = newDescription, oldSoftskill = oldDescription});
            return rowsAffected == 1;
        }

        public bool DeleteSoftSkill(string description)
        {
            var query = "DELETE FROM Softskill WHERE Description = @softskill";
            var rowsAffected = GetConnection().Execute(query, new { softskill = description });
            return rowsAffected == 1;
        }

        public List<string> GetAllSoftSkills()
        {
            return GetConnection().Query<string>("SELECT Description FROM Softskill").ToList();
        }

        public bool CreateRole(string description)
        {
            var query = "INSERT INTO dbo.Role (Description) VALUES (@role)";
            var rowsAffected = GetConnection().Execute(query, new {role = description});
            return rowsAffected == 1;
        }

        public bool UpdateRole(string newDescription, string oldDescription)
        {
            var query = "UPDATE dbo.Role SET Description = @newRole WHERE Description = @oldRole";
            var rowsAffected = GetConnection().Execute(query, new {newRole = newDescription, oldRole = oldDescription});
            return rowsAffected == 1;
        }

        public bool DeleteRole(string description)
        {
            var query = "DELETE FROM dbo.Role WHERE Description = @role";
            var rowsAffected = GetConnection().Execute(query, new { role = description });
            return rowsAffected == 1;
        }

        public List<string> GetAllRoles()
        {
            return GetConnection().Query<string>("SELECT Description FROM dbo.Role").ToList();
        }

        public bool CreateField(string description)
        {
            var query = "INSERT INTO Field (Description) VALUES (@field)";
            var rowsAffected = GetConnection().Execute(query, new {field = description});
            return rowsAffected == 1;
        }

        public bool UpdateField(string newDescription, string oldDescription)
        {
            var query = "UPDATE Field SET Description = @newField WHERE Description = @oldField";
            var rowsAffected = GetConnection().Execute(query, new {newField = newDescription, oldField = oldDescription});
            return rowsAffected == 1;
        }

        public bool DeleteField(string description)
        {
            var query = "DELETE FROM Field WHERE Description = @field";
            var rowsAffected = GetConnection().Execute(query, new { field = description });
            return rowsAffected == 1;
        }

        public List<string> GetAllFields()
        {
            return GetConnection().Query<string>("SELECT Description FROM Field").ToList();
        }

        public List<string> GetAllLanguages()
        {
            return GetConnection().Query<string>("SELECT Description FROM dbo.Language").ToList();
        }
    }
}