using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace PersonalBlazor.Data
{
    public class SkillNameConventionAttributAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is Skill)
            {
                Skill test = (Skill)validationContext.ObjectInstance;
                if (test.Skilltyp == Skill.Category.Softskill)
                {
                    if (!test.Name.All(i => (Char.IsLetter(i) || i == 'ö' || i == 'ü' || i == 'ä' || i == 'Ä' || i == 'Ö' || i == ' ' || i == 'Ü')))
                    {
                        return new ValidationResult("Hardskill hat falsche Bezeichnung.");
                    }
                }
            }

            return ValidationResult.Success;
        }

    }
}