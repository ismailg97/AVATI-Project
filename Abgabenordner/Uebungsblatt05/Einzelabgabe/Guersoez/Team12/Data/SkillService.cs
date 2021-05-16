using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Team12.Data;

namespace Team12.Data{
    public class SkillService:ISkillService
    {
        private readonly IConfiguration _configuration;

        public SkillService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection GetDbConnection()
        {
            return new SqlConnection("SoPro2021");
        }


        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Skilltyp { get; set; }

        public List<Skill> AllSkills = new List<Skill>();
        public bool DeleteSkill(int skillId)
        {
            DbConnection db = GetDbConnection();
            db.Open();
            return db.Query<bool>("delete from Skill where Id = @skillId").First();
        }

        public List<Skill> GetAllSkills()
        {
            DbConnection db = GetDbConnection();
            db.Open();
            this.AllSkills = db.Query<Skill>("select * from Skill").ToList();
            return AllSkills;
        }

        public Skill GetSkill(int skillId)
        {
            DbConnection db = GetDbConnection();
            db.Open();
            List<Skill> skill = db.Query<Skill>("select * from Skill where Id == @skillId").ToList();
            Skill _skill = skill.First();
            return AllSkills.Find(x => x.Id == skillId);
        }

        public bool UpdateSkill(Skill skill)
        {
            DbConnection db = GetDbConnection();
            db.Open();
            if(db.Query<Skill>("select * from Skill where Id == @skill.Id").Any())
            {
                db.Query<Skill>("update Skill set Id=@skill.Id, Name=@skill.Name, Skilltyp=@skill.SkillTyp where Id==@skill.Id");
            }
            else { 
                db.Query<Skill>("insert int Skill values (@skill.Name, @skill.Id, @skill.Skilltyp)");

            }
            return true;
        }

    }



}