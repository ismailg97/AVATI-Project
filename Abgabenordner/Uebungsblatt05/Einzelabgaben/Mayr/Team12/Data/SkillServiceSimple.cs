using System.Collections.Generic;
namespace Team12.Data {
    public class SkillServiceSimple : ISkillService
    {
        private List<Skill> SkillList = new List<Skill>();
        public Skill GetSkill(int skillId) {
            foreach (var s in SkillList) {
                if (s.Id == skillId)
                    return s;
            }
            return null;
        }
        public List<Skill> GetAllSkills() {
            return SkillList;
        }
        public bool UpdateSkill(Skill skill) {
            var temp = GetSkill(skill.Id);
            if (temp == null) {
                skill.Id = SkillList.Count;
                SkillList.Add(skill);
                return false;
            }
            temp.Name = skill.Name;
            temp.SkillCategory = skill.SkillCategory;
            return true;
        }
        public bool DeleteSkill(int skillId) {
            return SkillList.Remove(GetSkill(skillId));
        }
    }
}