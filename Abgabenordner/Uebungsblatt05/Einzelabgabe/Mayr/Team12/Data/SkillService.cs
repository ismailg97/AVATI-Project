using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Team12.Data {
    public class SkillService : ISkillService {
        private static IConfiguration _configuration;
        public SkillService(IConfiguration configuration) {
            _configuration = configuration;
        }
        public Skill GetSkill(int skillId) {
            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("SkillConnectionString"))) {
                IEnumerable<Skill> result = connection.Query<Skill>("SELECT * FROM Skill WHERE id = @id", new {id = skillId});
                return result.SingleOrDefault();
            }
        }
        public List<Skill> GetAllSkills() {
            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("SkillConnectionString"))) {
                IEnumerable<Skill> result = connection.Query<Skill>("SELECT * FROM Skill");
                return result.ToList();
            }
        }
        public bool UpdateSkill(Skill skill) {
            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("SkillConnectionString"))) {
                var temp = GetSkill(skill.Id);
                if (temp == null) {
                    IEnumerable<Skill> result1 = connection.Query<Skill>(
                        "INSERT INTO Skill (Name, Skilltyp) VALUES (@namme, @typ)",
                        new {namme = skill.Name, typ = skill.SkillCategory});
                    return false; //anlegne
                }
                IEnumerable<Skill> result2 = connection.Query<Skill>(
                    "UPDATE Skill SET Name = @namme, Skilltyp = @typ WHERE id = @id",
                    new {namme = skill.Name, typ = skill.SkillCategory, id = skill.Id});
                return true;//editne
            } 
        }
        public bool DeleteSkill(int skillId)
        {
            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("SkillConnectionString")))
            {
                IEnumerable<Skill> result =
                    connection.Query<Skill>("DELETE FROM Skill WHERE id = @id", new {id = skillId});
            }
            return true;
        }
    }
}