using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
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
            using var db = GetConnection();
            var query = "INSERT INTO Softskill (Description) VALUES (@softskill)";
            var rowsAffected = db.Execute(query, new {softskill = description});
            return rowsAffected == 1;
        }

        public bool UpdateSoftSkill(string newDescription, string oldDescription)
        {
            using var db = GetConnection();
            var query = "UPDATE Softskill SET Description = @newSoftskill WHERE Description = @oldSoftskill";
            var rowsAffected = db.Execute(query, new {newSoftskill = newDescription, oldSoftskill = oldDescription});
            return rowsAffected == 1;
        }

        public bool DeleteSoftSkill(string description)
        {
            using var db = GetConnection();
            var query = "DELETE FROM Softskill WHERE Description = @softskill";
            var rowsAffected = db.Execute(query, new { softskill = description });
            return rowsAffected == 1;
        }

        public List<string> GetAllSoftSkills()
        {
            using var db = GetConnection();
            return db.Query<string>("SELECT Description FROM Softskill").ToList();
        }

        public bool CreateRole(string description)
        {
            using var db = GetConnection();
            var query = "INSERT INTO dbo.Role (Description) VALUES (@role)";
            var rowsAffected = db.Execute(query, new {role = description});
            return rowsAffected == 1;
        }

        public bool UpdateRole(string newDescription, string oldDescription)
        {
            using var db = GetConnection();
            var query = "UPDATE dbo.Role SET Description = @newRole WHERE Description = @oldRole";
            var rowsAffected = db.Execute(query, new {newRole = newDescription, oldRole = oldDescription});
            return rowsAffected == 1;
        }

        public bool DeleteRole(string description)
        {
            using var db = GetConnection();
            var query = "DELETE FROM dbo.Role WHERE Description = @role";
            var rowsAffected = db.Execute(query, new { role = description });
            return rowsAffected == 1;
        }

        public List<string> GetAllRoles()
        {
            using var db = GetConnection();
            return db.Query<string>("SELECT Description FROM dbo.Role").ToList();
        }

        public bool CreateField(string description)
        {
            using var db = GetConnection();
            var query = "INSERT INTO Field (Description) VALUES (@field)";
            var rowsAffected = db.Execute(query, new {field = description});
            return rowsAffected == 1;
        }

        public bool UpdateField(string newDescription, string oldDescription)
        {
            using var db = GetConnection();
            var query = "UPDATE Field SET Description = @newField WHERE Description = @oldField";
            var rowsAffected = db.Execute(query, new {newField = newDescription, oldField = oldDescription});
            return rowsAffected == 1;
        }

        public bool DeleteField(string description)
        {
            using var db = GetConnection();
            var query = "DELETE FROM Field WHERE Description = @field";
            var rowsAffected = db.Execute(query, new { field = description });
            return rowsAffected == 1;
        }

        public List<string> GetAllFields()
        {
            using var db = GetConnection();
            return db.Query<string>("SELECT Description FROM Field").ToList();
        }

        public List<string> GetAllLanguages()
        {
            using var db = GetConnection();
            return db.Query<string>("SELECT Description FROM dbo.Language").ToList();
        }
        
        public bool CheckDescriptionSoftskill(string description)
        {
            using var db = GetConnection();
            var trimDesc = description.Replace(" ", "");
            var allSoftSkills = db.Query<string>("SELECT Description FROM Softskill").ToList();
            foreach (var softSkill in allSoftSkills)
            {
                if (trimDesc.Equals(softSkill.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }
        
        public bool CheckDescriptionField(string description)
        {
            using var db = GetConnection();
            var trimDesc = description.Replace(" ", "");
            var allFields = db.Query<string>("SELECT Description FROM Field").ToList();
            foreach (var field in allFields)
            {
                if (trimDesc.Equals(field.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }
        
        public bool CheckDescriptionRole(string description)
        {
            using var db = GetConnection();
            var trimDesc = description.Replace(" ", "");
            var allRoles = db.Query<string>("SELECT Description FROM Role").ToList();
            foreach (var role in allRoles)
            {
                if (trimDesc.Equals(role.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }
        
        public bool CheckEmptyBasicData(string description)
        {
            return !(string.IsNullOrEmpty(description) || string.IsNullOrWhiteSpace(description));
        }

        public bool CheckLengthBasicData(string description)
        {
            return description.Length <= 150;
        }
    }
}