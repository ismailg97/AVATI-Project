using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Team12.Data
{
    public enum Skilltype
    {
        Hardskill,
        Softskill
    }

    [SkillNameConventionAttribut]
    public class Skill 
    {
        public int ID { get; set; }

        [Required] public string Name { get; set; }

        [Required] public Skilltype type { get; set; }
    }

    internal interface ISkillService 
    {
        public Skill GetSkill(int skillID);
        public List<Skill> GetAllSkills();
        public bool UpdateSkill(Skill skill);
        public bool DeleteSkill(int skillID);
    }

    public class SkillServiceSimple : ISkillService 
    {
        private readonly List<Skill> skills = new();

        public bool DeleteSkill(int skillID)
        {
            var nullORone = skills.RemoveAll(x => x.ID == skillID);
            if (nullORone == 1)
                return true;
            return false;
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
            var old = skills.Find(index => index.ID == skill.ID);
            if (old.ID == 1) return false;

            skills.Add(skill);
            skills.Remove(old);
            return true;
        }
    }
}