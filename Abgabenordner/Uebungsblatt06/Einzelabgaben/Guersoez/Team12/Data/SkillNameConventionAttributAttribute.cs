using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Team12.Data
{
    public class SkillNameConventionAttributAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is Skill)
            {
                Skill test = (Skill)validationContext.ObjectInstance;
                if (test.SkillType == Skill.Category.Softskill)
                {
                    if (test.Name.Any(j => (Char.IsLetter(j) || j == 'ö' || j == 'ü' || j == 'ä' || j == 'Ä' || j == 'Ö' || j == 'Ü')))
                    {
                        return new ValidationResult("Hardskill hat falsche Bezeichnung.");
                    }
                }
            }

            return ValidationResult.Success;
        }

    }
}