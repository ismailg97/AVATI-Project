using System.ComponentModel.DataAnnotations;

namespace Team12.Data
{
    public class SkillNameConventionAttribut : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //validationContext.
            return ValidationResult.Success;
        }
    }
}