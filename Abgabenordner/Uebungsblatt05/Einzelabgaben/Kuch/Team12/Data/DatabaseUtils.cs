using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Team12.Data
{
    public class DatabaseUtils
    {
        private readonly IConfiguration _config;

        public DatabaseUtils(IConfiguration config)
        {
            _config = config;
        }

        public void CreateTable() {
            using (IDbConnection connection = new SqlConnection(_config.GetConnectionString("SoPro2021Connection"))) {
                try
                {
                    connection.Query("CREATE TABLE [dbo].[Skill]([Id][int] IDENTITY(1, 1) NOT NULL,[Name][varchar](50),[Skilltype][bit] NOT NULL,CONSTRAINT[pkSkill] PRIMARY KEY([Id]");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fehler beim Erstellen der Tabelle");
                }
            }

        }
    }
}
