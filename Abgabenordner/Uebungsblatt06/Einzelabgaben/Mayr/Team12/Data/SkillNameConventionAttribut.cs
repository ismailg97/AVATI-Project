using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Team12.Data
{
    public class SkillNameConventionAttribut : ValidationAttribute
    {
        private Regex regex = new Regex(@"^[a-zA-Z0-9\s]+$");
        private string GetErrorMessage() => $"Invalid Name for Skill of type 'Softskill'";
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var skill = (Skill) validationContext.ObjectInstance;
            if (skill.SkillType.Equals(Skill.Category.Softskill))
            {
                if (regex.IsMatch(skill.Name))
                    return ValidationResult.Success;
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
    }
}

//° ^ ! " § $ % & / ( ) = ? \ ` ´ + * ~ # ' - _ . : , ; > | < { [ ] } 