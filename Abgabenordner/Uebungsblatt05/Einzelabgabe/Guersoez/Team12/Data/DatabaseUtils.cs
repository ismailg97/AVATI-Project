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
            return new SqlConnection("SoPro2021");
        }

        public bool ErzeugeTabelle()
        {
            DbConnection db = GetDbConnection();
            db.Open();

            return true;

        }

    }
}
