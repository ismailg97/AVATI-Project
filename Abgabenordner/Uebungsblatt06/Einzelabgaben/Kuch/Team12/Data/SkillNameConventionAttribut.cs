using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Team12.Data
{
    public class SkillNameConventionAttribut : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is Skill)
            {
                var skill = (Skill)validationContext.ObjectInstance;

                if (skill.Skilltype == Skilltype.Softskill)
                {
                    if (!Regex.IsMatch(skill.Name, @"^[a-zA-ZäüöÄÜÖ\s]+$"))
                    {

                        return new ValidationResult("Für den Name eines Softskills sind nur die Zeichen von A-Z sowie Umlaute und Leerzeichen erlaubt");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
