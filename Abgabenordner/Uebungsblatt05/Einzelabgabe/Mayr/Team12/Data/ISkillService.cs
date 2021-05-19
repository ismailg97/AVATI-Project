using System.Collections.Generic;
namespace Team12.Data {
    public interface ISkillService {
        Skill GetSkill(int skillId);
        List<Skill> GetAllSkills();
        bool UpdateSkill(Skill skill);
        bool DeleteSkill(int skillId);
    }
}