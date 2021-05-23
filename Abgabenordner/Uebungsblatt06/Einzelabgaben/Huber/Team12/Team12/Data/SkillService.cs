using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Team12.Data
{
    public class SkillService : ISkillService
    {
        private readonly IConfiguration _configuration;

        public SkillService()
        {
            
        }
        public SkillService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection
                (_configuration.GetConnectionString("SoPro2021Database"));
        }
        
        public Skill GetSkill(int skillId)
        {
            using DbConnection db = GetConnection();
            db.Open();
            var result = db.Query<Skill>("select * from Skill where Id = @id", 
                new {id = skillId}).ToList();
            if (!result.Any()) return null;
            var skill = result[0];
            return skill;
        }

        public List<Skill> GetAllSkills()
        {
            using DbConnection db = GetConnection();
            db.Open();
            var result = db.Query<Skill>("select * from Skill").ToList();
            var categoryResult = db.Query<int>("select Skilltype from Skill").ToList();
            for (int i = 0; i < result.Count; ++i)
            {
                if (categoryResult[i] == 0)
                {
                    result[i].SkillType = Skill.SkillCategory.Hardskill;
                }
                else
                {
                    result[i].SkillType = Skill.SkillCategory.Softskill;
                }
            }

            return result;
        }

        public bool UpdateSkill(Skill skill)
        {
            using DbConnection db = GetConnection();
            int i;
            db.Open();
            if (skill.SkillType == Skill.SkillCategory.Hardskill)
            {
                i = 0;
            }
            else
            {
                i = 1;
            }

            if (skill.Id == 0)
            {
                db.Query("insert into Skill values(@name, @skilltype)",
                    new {name = skill.Name, skilltype = i});                    //Skilltype wird über 1 und 0 realisiert
            }
            else
            {
                db.Query("update Skill set Name = @name, Skilltype = @skilltype where Id = @id",
                    new {name = skill.Name, skilltype = i, id = skill.Id});
            }

            return true;
        }

        public bool DeleteSkill(int skillID)
        {
            using DbConnection db = GetConnection();
            db.Open();
            db.Query<Skill>("delete from Skill where Id = @id", new {id = skillID});
            return true;
        }
    }
}