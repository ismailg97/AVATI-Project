using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlazor.Data
{
    public interface ISkillService
    {
        public Skill GetSkill(int skillId);
        public List<Skill> GetAllSkills();
        public bool UpdateSkill(Skill skill);
        public bool DeleteSkill(int skillId);
    }
}
