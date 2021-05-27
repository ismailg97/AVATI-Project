using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Team12.Data
{
    public enum Skilltype               
    {
        Hardskill,
        Softskill
    }
    public class Skill                                      //removing constructors cause the class had some problems with it -> soft and hardskill error
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Skilltype type { get; set; }
        
    }

    interface ISkillService //interface erstellung
    {
        public Skill GetSkill(int skillID);
        public List<Skill> GetAllSkills();
        public bool UpdateSkill(Skill skill);
        public bool DeleteSkill(int skillID);
    }

    public class SkillServiceSimple : ISkillService //implementiert Interface SkillService
    {
        private int identification = 0;
        private List<Skill> skills = new List<Skill>();

        public bool DeleteSkill(int skillID)
        {
            int nullORone = skills.RemoveAll(x => x.ID == skillID);
            if (nullORone == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Skill> GetAllSkills()
        {
            return skills;
        }

        public Skill GetSkill(int skillID)
        {
            return skills.Find(x => x.ID == skillID);
        }

        public bool UpdateSkill(Skill skill)
        {
            Skill old = skills.Find(index => index.ID == skill.ID);
            if (old.ID == 1)
            {
                return false;
            }
            else
            {
                skills.Add(skill);
                skills.Remove(old);
                return true;
            }
        }
    }
}