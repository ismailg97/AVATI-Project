using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Team12.Data;

namespace Team12.Entities{
    public class SkillService:ISkillService
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Skilltyp { get; set; }

        public List<Skill> AllSkills = new List<Skill>();
        public bool DeleteSkill(int skillId)
        {
            int ausgabe = AllSkills.RemoveAll(x => x.Id == skillId);
            if (ausgabe == 1) return true;
            else return false;
        }

        public List<Skill> GetAllSkills()
        {
            return AllSkills;
        }

        public Skill GetSkill(int skillId)
        {
            return AllSkills.Find(x => x.Id == skillId);
        }

        public bool UpdateSkill(Skill skill)
        {
            Skill oldskill = AllSkills.Find(x => x.Id == skill.Id);
            if (oldskill.Equals(null)) return false;
            oldskill = skill;
            return true;
        }

    }



}