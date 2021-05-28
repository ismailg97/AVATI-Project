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
        public Skill GetSkill(int skillId)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("SkillConnectionString"));
            var result = connection.Query<Skill>("SELECT * FROM Skill WHERE id = @id", new {id = skillId});
            return result.SingleOrDefault();
        }
        public List<Skill> GetAllSkills()
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("SkillConnectionString"));
            var result = connection.Query<Skill>("SELECT * FROM Skill").ToList();
           
            var categoryResult = connection.Query<int>("select Skilltype from Skill").ToList();
            for (int i = 0; i < result.Count; ++i)
            {
                if (categoryResult[i] == 0)
                {
                    result[i].SkillCategory = Skill.Category.Hardskill;
                }
                else
                {
                    result[i].SkillCategory = Skill.Category.Softskill;
                }
            }
            return result;
        }
        public bool UpdateSkill(Skill skill)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("SkillConnectionString"));
            var i = skill.SkillCategory == Skill.Category.Hardskill ? 0 : 1;
            if (GetSkill(skill.Id) == null) {
                connection.Execute(
                    "INSERT INTO Skill (Name, Skilltype) VALUES (@namme, @bit)",
                    new {namme = skill.Name, bit = i});
                return false; //anlegne
            }
            connection.Execute(
                "UPDATE Skill SET Name = @namme, Skilltype = @typ WHERE id = @id",
                new {namme = skill.Name, typ = i, id = skill.Id});
            return true;//editne
                
            //Execute statt Query wenn man nichts returned 
        }
        public bool DeleteSkill(int skillId)
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("SkillConnectionString"));
            var result =
                connection.Query<Skill>("DELETE FROM Skill WHERE id = @id", new {id = skillId});
            return true;
        }
    }
}