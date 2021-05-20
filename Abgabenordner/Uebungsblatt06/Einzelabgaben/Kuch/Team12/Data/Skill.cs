using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public int Id { get; set; }
        public string Name { get; set; }
        public Skilltype Skilltype { get; set; }
    }
}
