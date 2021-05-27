using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace Team12.Data
{
    public class SkillNameConventionAttribut : ValidationAttribute
    {
        private readonly Regex _regex = new(@"^[a-zA-ZäöüÄÖÜ\s]+$");
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var skill = (Skill) validationContext.ObjectInstance;
            if (!skill.SkillType.Equals(Skill.Category.Softskill)) return ValidationResult.Success;
            return _regex.IsMatch(skill.Name) ? ValidationResult.Success : new ValidationResult("Invalid Name for Skill of type 'Softskill'");
        }
    }
}