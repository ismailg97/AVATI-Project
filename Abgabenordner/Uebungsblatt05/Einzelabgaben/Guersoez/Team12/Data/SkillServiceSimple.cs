using System.Collections.Generic;
using System.Linq;

namespace Team12.Data
{
    public class SkillServiceSimple :ISkillService
    {
        private List<Skill> _skills = new List<Skill>();

        public Skill GetSkill(int id)
        {
            return _skills.Find(x => x.Id.Equals(id));
        }

        public List<Skill> GetAllSkills()
        {
            return _skills;
        }

        public bool UpdateSkill(Skill skill)
        {
            if (!_skills.Any())
            {
                skill.Id = 1;
                _skills.Add(skill);
                return true;
            } else if (skill.Id != 0)
            {
                foreach (var listSkill in _skills)
                {
                    if (listSkill.Id == skill.Id)
                    {
                        listSkill.Name = skill.Name;
                        listSkill.SkillType = skill.SkillType;
                        return true;
                    }
                }
            }

            skill.Id = _skills.Count + 1;
            _skills.Add(skill);
            return true;
        }

        public bool DeleteSkill(int id)
        {
            var tempSkill = _skills.Find(x => x.Id.Equals(id));

            return _skills.Remove(tempSkill);
        }
    }
}