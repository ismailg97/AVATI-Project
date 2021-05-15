using System;
using System.Collections.Generic;

namespace Team12.Data
{
    public enum Skilltype
    {
        Hardskill,
        Softskill
    }

    public struct Skill
    {
        private static int identification = 0;
        public int ID { get; set; }
        public string Name { get; set; }
        public Skilltype type { get; set; }

        public Skill(int ID, string Name, Skilltype skill)
        {
            this.ID = ID;
            this.Name = Name;
            this.type = skill;
        }

        public Skill(string Name, Skilltype skill)
        {
            ID = getID();
            this.Name = Name;
            this.type = skill;
        }

        public static int getID()
        {
            return identification++;
        }
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
            if (old.ID == null)
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