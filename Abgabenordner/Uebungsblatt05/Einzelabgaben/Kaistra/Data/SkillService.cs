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
    public class SkillService : ISkillService
    {
        private readonly IConfiguration _config;

        public SkillService(IConfiguration config)
        {
            using DbConnection db = GetConnection();
            try
            {
                int id = db.QueryFirst<int>("select max(id) from skill");
                Skill.UpdateIdCounter(id);
            }
            catch (Exception e)
            {
            }

            _config = config;
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        public Skill GetSkill(int skillID) //using public T get() von medium.com #doubleChecking
        {
            Skill result;
            using DbConnection db = GetConnection();

            if (db.State == ConnectionState.Closed)
            {
                DatabaseUtils util = new DatabaseUtils(_config); //riiiichtig unsicher tho
                util.CreatingEmptyTable();
                db.Open();
            }

            var abfrage =
                db.Query<Skill>("Select * from Skill where id = @ID", new {ID = skillID})
                    .ToList(); //zetcode.com/csharp/dapper/ (C# Dapper parameterized query
            result = abfrage[0]; //list -> array[]
            return result;
        }

        public List<Skill> GetAllSkills()
        {
            List<Skill> HardSkills = new List<Skill>();
            List<Skill> Softskills = new List<Skill>();

            using DbConnection db = GetConnection();
            HardSkills = db.Query<Skill>("Select * from Skill where skilltype = 1").ToList();
            Softskills = db.Query<Skill>("Select * from Skill where skilltype = 0").ToList();

            List<Skill> combined = new List<Skill>();
            for (int n = 0; n < HardSkills.Count; ++n)
            {
                combined.Add(HardSkills[n]);
            }

            for (int n = 0; n < Softskills.Count; ++n)
            {
                combined.Add(Softskills[n]);
            }

            return combined;
        }

        public bool UpdateSkill(Skill skill)
        {
            using DbConnection db = GetConnection();

            if (db.State == ConnectionState.Closed)
            {
                db.Open();
            }

            try
            {
                var name = db.Query<string>("select name from Skill where @skillname = name",
                    new {skillname = skill.Name});
                string local = name.ToString();
                if (local == skill.Name)
                {
                }
                else
                {
                    db.QueryFirst<Skill>("select * from skill where id = @oldID", new {oldID = skill.ID});

                    db.Execute("Update Skill set Name = @name, skilltype = @Type where id = @ID",
                        new {ID = skill.ID, name = skill.Name, Type = skill.type == Skilltype.Hardskill ? 1 : 0});
                    // Hardskill = 1  |  Softskill = 0
                }
            }
            catch (Exception e)
            {
                db.Execute("Insert into Skill values(@name, @skilltype)",
                    new {name = skill.Name, skilltype = skill.type == Skilltype.Hardskill ? 1 : 0});
                return true;
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