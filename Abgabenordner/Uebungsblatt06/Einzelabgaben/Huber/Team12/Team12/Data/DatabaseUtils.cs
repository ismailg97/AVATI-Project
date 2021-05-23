using System;
using System.Data.Common;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Team12.Data
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
                    return new SqlConnection
                        (_configuration.GetConnectionString("SoPro2021Database"));
                }
        
                
                public bool CreateSkillTable()
                {
                    using DbConnection db = GetConnection();
                    db.Open();
                    var result = db.Query<int>("SELECT CASE WHEN OBJECT_ID('dbo.Skill', 'U') IS NOT NULL THEN 1 ELSE 0 END");
                    if (result.First() == 1)
                    {
                        return false;
                    }
                    
                    db.Query("create table Skill ( Id int not null identity(1, 1) primary key, Name varchar(50) not null, Skilltype bit not null)");
                    

                    return true;
                }
                
    }
}