using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


//es kein sein,dass der Code ähnlich dem von Anton ausschaut -> Er hat eine Dokumentation gefunden, die er uns allen geteilt hat. 
//um drüberzugehem und zu beweisen, dass der Code eigenhändig geschrieben ist.: https://medium.com/informatics/blazor-server-project-1-d77b0c3926d4
//dort ist der Code in der Dokumentation gleich although man den Stoff ja selber durchgeganen ist und somit (hoffentlich) dann auch verstanden hat
//aber nach der Ansprache am Freitag hat wohl keiner Bock auf Plagiate #woBleibtVonUndZuGutenberg xD
//habs mit try and catch versucht, hat aber nicht so ganz gefunzt.. sollte man damit arbeiten? oder erledigt dapper das für einen?

namespace Team12.Data
{
    public class SkillService : ISkillService
    {
        private readonly IConfiguration _config;
    
        public SkillService(IConfiguration config)
        {
            _config = config;
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("SoPro2021Database"));
        }

        public Skill GetSkill(int skillID) //using public T get() von medium.com #doubleChecking
        {
            Skill result;
            using DbConnection db = GetConnection();
            try
            {
                if (db.State == ConnectionState.Closed)
                {
                    DatabaseUtils util = new DatabaseUtils(_config);    //riiiichtig unsicher tho
                    util.CreatingEmptyTable();
                    db.Open();
                }

                var abfrage =
                    db.Query<Skill>("Select * from Skill where id = @ID", new {ID = skillID})
                        .ToList(); //zetcode.com/csharp/dapper/ (C# Dapper parameterized query
                result = abfrage[0]; //list -> array[]
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                {
                    db.Close();
                }
            }
        }

        public List<Skill> GetAllSkills()
        {
            List<Skill> Allskills = new List<Skill>();
            using DbConnection db = GetConnection();
            try
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }

                var abfrage =
                    db.Query<Skill>("select * from Skill")
                        .ToList(); //wieso muss hier ein var sein? ist das C# spezifisch?
                int n = abfrage.Count;
                int i = 0;
                while (i != n)
                {
                    Allskills.Add(abfrage[i]);
                    ++i;
                }
            }
            catch (Exception e)
            {
            }

            return Allskills;
        }

        public bool UpdateSkill(Skill skill)
        {
            using DbConnection db = GetConnection();

            if (db.State == ConnectionState.Closed)
            {
                
                db.Open();
            }

            Skill old = new Skill();
            var asdf = db.Query<Skill>("select * from Skill where id = @oldID", new {id = skill.ID}).ToList();
            old = asdf[0];
            //umschreiben Code Skill_datatype
            if (old.ID == 0)
            {
                db.Query<Skill>("Insert Into SKill values(@name, @type)", new {name = skill.Name, type = skill.type});
                return false;
            }
            else
            {
                db.Query<Skill>("Update Skill set name = @Name, type = @Type where id = @ID",
                    new {Name = skill.Name, Type = skill.type, ID = skill.ID});
            }

            return true;
        }

        public bool DeleteSkill(int skillID)
        {
            using DbConnection db = GetConnection();
            if (db.State == ConnectionState.Closed)
                db.Open();
            var result = db.Query<Skill>("Delete from Skill where id = @ID", new
            {
                ID = skillID
            });

            return true;
        }
    }
}