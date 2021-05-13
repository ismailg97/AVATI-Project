using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Team12.Data
{
    public class SkillService : ISkillService
    {
        private readonly IConfiguration _configuration;

        public SkillService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool DeleteSkill(int skillID)
        {
            using DbConnection db = GetConnection();
            db.Open();
            db.Query<Skill>("delete from Skill where ID = @id", new {id = skillID});
            return true;
        }

        public List<Skill> GetAllSkills()
        {
            using (DbConnection db = GetConnection())
            {
                db.Open();

                var result = db.Query<Skill>("select * from Skill").ToList();
                var catRes = db.Query<int>("select * from Skill").ToList();
                for (int n = 0; n < result.Count; ++n)
                {
                    if (catRes[n] == 0)
                    {
                        Skill copy = result[n];
                        copy.type = Skilltype.Hardskill;
                        result[n] = copy;

                    }
                    else
                    {
                        Skill copy = result[n];
                        copy.type = Skilltype.Softskill;
                        result[n] = copy;
                    }
                }
                return result;
            }
        }

        public Skill GetSkill(int skillID)
        {
            using DbConnection db = GetConnection();
            db.Open();
            var result = db.Query<Skill>("select * from Skill from ID = @ID", new {id = skillID}).ToList();
            if (!result.Any()) return default;
            var skill = result[0];
            return skill;
        }

        public bool UpdateSkill(Skill skill)
        {
            using DbConnection db = GetConnection();
            int n;
            db.Open();
            if (skill.type == Skilltype.Hardskill)
            {
                n = 0; 
            }
            else
            {
                n = 1;
            }

            if (skill.ID == 0)
            {
                db.Query("insert into Skill values(@name, @bit)", new {Name = skill.Name, bit = 0});
            }
            return true;
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("SoPro2021Database"));
        }
    }
    
}


