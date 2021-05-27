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
                    var regexItem = new Regex("^[a-zA-Z]*$");
                    if(!regexItem.IsMatch(test.Name))
                    {
                        return new ValidationResult("Hardskill hat falsche Bezeichnung.");
                    }
                }
            }

            return ValidationResult.Success;
        }

    }
}