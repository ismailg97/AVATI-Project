using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using System.IO;
using static PersonalBlazor.Data.Skill;

namespace PersonalBlazor.Data
{
    public class SkillService : ISkillService
    {
        IConfiguration config;
        public List<Skill> SkillList = new List<Skill>();

        public SkillService(IConfiguration _config)
        {
            config = _config;
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection("data source=DESKTOP-15GNDRC\\SQLEXPRESS;initial catalog=SoPro2021;trusted_connection=true");
        }




        public Skill GetSkill(int skillId)
        {
            DbConnection db = GetConnection();
            db.Open();
            var result = db.Query<Skill>("select * from Skill where Id =@id", new { id = skillId }).ToList();
            if (!result.Any()) return null;
            Skill skill = result[0];
            return skill;

        }
        public List<Skill> GetAllSkills()
        {
            DbConnection db = GetConnection();
            db.Open();
            var result = db.Query<Skill>("select * from Skill").ToList();
            var categoryResult = db.Query<int>("select Skilltype from Skill").ToList();
            for (int i = 0; i < result.Count; ++i)
            {
                if (categoryResult[i] == 0)
                {
                    result[i].Skilltyp= Skill.Category.Hardskill;
                }
                else
                {
                    result[i].Skilltyp = Skill.Category.Softskill;
                }
            }

            return result;

        }

        public bool UpdateSkill(Skill skill)
        {
            DbConnection db = GetConnection();
            db.Open();
            int i;
            if (skill.Skilltyp == Skill.Category.Hardskill)
            {
                i = 0;
            }
            else
            {
                i = 1;
            }

            if (skill.Id == 0)
            {
                db.Query("insert into Skill values(@name, @bit)",
                    new { name = skill.Name, bit = i });
            }
            else
            {
                db.Query("update Skill set Name = @name, Skilltype = @skilltype where Id = @id",
                    new { name = skill.Name, skilltype = i, id = skill.Id });
            }

            return true;
        }

        public bool DeleteSkill(int skillId)
        {
            DbConnection db = GetConnection();
            db.Open();
            db.Query<bool>("delete from Skill where Id =@id", new { id = skillId });
            return true;
        }
    }
}
