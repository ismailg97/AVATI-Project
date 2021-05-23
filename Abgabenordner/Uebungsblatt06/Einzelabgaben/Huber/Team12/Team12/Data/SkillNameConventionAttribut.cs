using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Team12.Data
{
    public class SkillNameConventionAttribut : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is Skill)
            {
                Skill model = (Skill)validationContext.ObjectInstance;
                if (model.SkillType == Skill.SkillCategory.Softskill)
                {
                    if (!model.Name.All(c=>(Char.IsLetter(c) || c ==' ' || c=='ä'|| c=='ö' || c=='ü' || c=='Ä' || c=='Ö' || c=='Ü')))
                    {
                        return new ValidationResult("Der uebergebene Hardskill wurde nicht richtig bennant");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}