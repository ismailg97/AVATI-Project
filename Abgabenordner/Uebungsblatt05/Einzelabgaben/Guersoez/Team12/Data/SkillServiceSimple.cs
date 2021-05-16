using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;


namespace Team12.Data
{





    public class SkillServiceSimple : ISkillService
    {
        //private List<Skill> SkillList;

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