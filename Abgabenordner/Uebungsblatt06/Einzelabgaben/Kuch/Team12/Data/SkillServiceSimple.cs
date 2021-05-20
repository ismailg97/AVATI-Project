using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Team12.Data
{
    public class SkillServiceSimple: ISkillService
    {
        public static int idcount = 0;
        private List<Skill> allSkills = new List<Skill>()
            {
                new Skill { Id = ++idcount, Name = "Teamfähigkeit", Skilltype = Skilltype.Softskill},
                new Skill { Id = ++idcount, Name = "Emotionale Intelligenz", Skilltype = Skilltype.Softskill},
                new Skill { Id = ++idcount, Name = "Webentwicklung", Skilltype = Skilltype.Hardskill }
            };
        public bool DeleteSkill(int skillId)
        {
            int ausgabe = allSkills.RemoveAll(x => x.Id == skillId);
            if (ausgabe == 1) return true;
            else return false;
        }

        public List<Skill> GetAllSkills()
        {
            return allSkills;
        }

        public Skill GetSkill(int skillId)
        {
            return allSkills.Find(x => x.Id == skillId);
        }

        public bool UpdateSkill(Skill skill)
        {
            Skill oldskill = allSkills.Find(x => x.Id == skill.Id);
            if (oldskill == null)
            {
                allSkills.Add(skill);
                return true;
            }
            else if (allSkills.Remove(oldskill)) {
                allSkills.Add(skill);
                return true;
            }
            return false;
        }
    }
}
