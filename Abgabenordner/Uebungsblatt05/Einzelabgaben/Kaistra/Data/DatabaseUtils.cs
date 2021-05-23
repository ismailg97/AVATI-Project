using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Team12.Data
{
    public class DatabaseUtils
    {
        private readonly IConfiguration _config;

        public DatabaseUtils(IConfiguration config)
        {
            _config = config;
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        } //alles notwendige zur Verbindung

        public void CreatingEmptyTable() //
        {
            using DbConnection db = GetConnection();

            if (db.State == ConnectionState.Closed)
            {
                db.Open();
            }

            var result = db.Query<int>("SELECT CASE WHEN OBJECT_ID('dbo.skill', 'U') IS NOT NULL THEN 1 ELSE 0 END");
            if (result.First() == 0)
            {
                var erg = db.Query(
                    "Create table Skill (id int identity(1, 1) primary key not null, name varchar(50) not null, skilltype bit not null)");
                //same Query as used in ssms to create a table -> not sure bout identity(1, 1) aus autoincrement
            }
        }
    }
}