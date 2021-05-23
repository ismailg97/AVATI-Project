
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Team12.Data
{
    public interface ISkillService
    {
        
        public Skill GetSkill(int id);


        public List<Skill> GetAllSkills();


        public bool UpdateSkill(Skill skill);


        public bool DeleteSkill(int id);

    }
}