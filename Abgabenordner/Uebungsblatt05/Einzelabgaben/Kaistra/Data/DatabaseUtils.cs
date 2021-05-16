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
            return new SqlConnection(_config.GetConnectionString("SoPro2021Database"));
        }   //alles notwendige zur Verbindung

        public void CreatingEmptyTable()    //Micrsoft.docs
        {
            using DbConnection db = GetConnection();
            try
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var erg = db.Query("Create table Skill (id identity(1, 1) primary key, name varchar(50) not null, type bit not null)");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}