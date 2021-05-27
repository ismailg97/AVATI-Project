using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Team12.Data
{
    public class DatabaseUtils
    {
        private static IConfiguration _configuration;
        public DatabaseUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private static DbConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("SkillConnectionString"));
        }
        public bool CreateTable()
        {
            using var connection = GetConnection();
            connection.Open();
            connection.Execute(
                "CREATE TABLE Skill (Id int IDENTITY(1,1), Name varchar(50), Skilltype bit notnull)");
            return true;
        }
    }
}