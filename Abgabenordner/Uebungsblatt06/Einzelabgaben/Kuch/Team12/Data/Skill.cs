using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;


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
        
        [Required]
        [NotNull]
        public string Name { get; set; }
        
        [Required]
        public Skilltype Skilltype { get; set; }
    }
}
