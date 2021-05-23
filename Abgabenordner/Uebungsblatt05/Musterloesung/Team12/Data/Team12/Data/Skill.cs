using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Team12.Data
{
    public class Skill
    {
        public int Id { get; set; }
        
        [Required]
        [NotNull]
        public string Name { get; set; }
        
        [Required]
        public Category SkillType { get; set; }
        public enum Category
        {
            Hardskill,
            Softskill
        }

        
    }
}