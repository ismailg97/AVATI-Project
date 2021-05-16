using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Data.Common;
using Microsoft.AspNetCore.Components;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Team12.Data;


namespace Team12.Data
{
    public class DatabaseUtils
    {
        IConfiguration _configuration;

        public DatabaseUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection GetDbConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("SoPro2021"));
        }

        public bool ErzeugeTabelle()
        {
            DbConnection db = GetDbConnection();
            db.Open();
            if (db.Query<int>("SELECT CASE WHEN OBJECT_ID('dbo.Skill', 'U') IS NOT NULL THEN 1 ELSE 0 END").First() == 1)
            {
                Console.WriteLine("Test");
                return false;
            }
            else { db.Query("create table Skill( Id int not null identity(1, 1) primary key, Name varchar(50) not null, Skilltype bit not null)");
                return true;
            }

        }

    }
}
