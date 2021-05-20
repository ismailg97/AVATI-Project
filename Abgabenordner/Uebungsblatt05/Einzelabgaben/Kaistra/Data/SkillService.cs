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
                DatabaseUtils util = new DatabaseUtils(_config); 
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
                HardSkills[n].type = Skilltype.Hardskill;
                combined.Add(HardSkills[n]);
            }

            for (int n = 0; n < Softskills.Count; ++n)
            {
                Softskills[n].type = Skilltype.Softskill;
                combined.Add(Softskills[n]);
            }
            return combined;
        }

        public bool UpdateSkill(Skill skill)
        {
            using DbConnection db = GetConnection();
            int i;
            db.Open();
            if (skill.type == Skilltype.Hardskill)
            {
                i = 1;
            }
            else
            {
                i = 0;
            }

            if (skill.ID== 0)
            {
                db.Query("insert into Skill values(@name, @bit)",
                    new {name = skill.Name, bit = i});
            }
            else
            {
                db.Query("update Skill set Name = @name, Skilltype = @skilltype where Id = @id",
                    new {name = skill.Name, skilltype = i, id = skill.ID});
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