using System.Data.Common;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;

namespace AVATI.Data.DatabaseConnection
{
    public class DatabaseUtils
    {
        private readonly IConfiguration _configuration;

        public DatabaseUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public DbConnection GetConnection()
        {
            //Uses the connection string stored in appsettings.json (Jede Person hat ihren eigenen Connection-String!!)
            return new SqlConnection
                (_configuration.GetConnectionString("TestDBAnton"));
        }

        public bool CreateTables()
        {
            using DbConnection db = GetConnection();
            db.Open();
            string sqlToCheck = "SELECT CASE WHEN OBJECT_ID('dbo." + "Testtabelle" + "', 'U') IS NOT NULL THEN 1 ELSE 0 END";
            var result = db.Query<int>(sqlToCheck);
            if (result.First() == 1)
            {
                //Happens, when table with this name already exists;
                return false;
            }
            else
            {
                db.Execute("create table Testtabelle ( Id int not null identity(1, 1) primary key, Name varchar(50) not null, Skilltype bit not null)");
                //The tables does not exist yet
                
            }
                    

            return true;
        }


    }
}