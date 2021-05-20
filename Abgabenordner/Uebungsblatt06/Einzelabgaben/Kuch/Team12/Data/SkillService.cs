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
    public class SkillService : ISkillService
    {
        private readonly IConfiguration _config;

        public SkillService(IConfiguration config)
        {
            _config = config;
        }
        public bool DeleteSkill(int skillId)
        {
            string connString = _config.GetConnectionString("SoPro2021Connection");
            string query = "DELETE FROM dbo.Skill WHERE Id = @Id";

            using (IDbConnection connection = new SqlConnection(connString))
            {
                int rowsAffected = connection.Execute(query, new { Id = skillId });
                if (rowsAffected != 1) return false;
            }
            return true;
        }

        public List<Skill> GetAllSkills()
        {
            string connString = _config.GetConnectionString("SoPro2021Connection");
            int count = 0;
            List<Skill> allSkills;
            List<int> Skilltypes;
            string queryall = "SELECT Id, Name, Skilltype FROM dbo.Skill";
            string queryskilltyp = "SELECT Skilltype FROM dbo.Skill";

            using (IDbConnection connection = new SqlConnection(connString))
            {
                allSkills = connection.Query<Skill>(queryall).ToList();
                Skilltypes = connection.Query<int>(queryskilltyp).ToList();

                foreach (int Skilltyp in Skilltypes) {
                    Skill skill = allSkills[count];
                    if (Skilltyp == 0)
                    {
                        skill.Skilltype = Skilltype.Hardskill;
                        allSkills[count] = skill;
                    }
                    else {
                        skill.Skilltype = Skilltype.Softskill;
                    }
                    allSkills[count] = skill;
                    ++count;
                }
            }
            return allSkills;
        }

        public Skill GetSkill(int skillId)
        {
            string connString = _config.GetConnectionString("SoPro2021Connection");
            Skill skill;
            string query = "SELECT Id, Name, Skilltype FROM dbo.Skill WHERE Id = @skillId";

            using (IDbConnection connection = new SqlConnection(connString))
            {
                return connection.Query<Skill>(query, new { skillId }).SingleOrDefault();

                /*skill = connection.Query<Skill>(query, new { skillId }).SingleOrDefault();

                if (skill == null) return null;
                else if(skill)*/
            }
        }

        public bool UpdateSkill(Skill skill)
        {
            string connString = _config.GetConnectionString("SoPro2021Connection");
            int bit = 0;
            string updatequery = "UPDATE dbo.Skill SET Name = @Name, Skilltype = @Bit WHERE Id = @Id";
            string insertquery = "INSERT INTO dbo.Skill (Name, Skilltype) VALUES (@Name, @Bit)";

            if (skill.Skilltype == Skilltype.Softskill)
            {
                bit = 1;
            }

            using (IDbConnection connection = new SqlConnection(connString))
            {
                int rowsAffected;
                if (skill.Id == 0)
                {
                    rowsAffected = connection.Execute(insertquery, new { Name = skill.Name, Bit = bit});
                    if (rowsAffected != 1) return false;
                }
                else
                {
                    rowsAffected = connection.Execute(updatequery, new { Name = skill.Name, Bit = bit, Id = skill.Id});
                    if (rowsAffected != 1) return false;
                }
            }
            return true;
        }
    }
}
