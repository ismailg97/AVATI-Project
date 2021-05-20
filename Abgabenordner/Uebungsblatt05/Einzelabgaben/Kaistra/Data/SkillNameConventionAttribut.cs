using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization.Infrastructure;


namespace Team12.Data           // source.: https://www.youtube.com/watch?v=o_AH2MGti0A
{                               //source1.: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-5.0  | Custom attributes
    public class SkillNameConventionAttribut : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Skill caster = (Skill) validationContext.ObjectInstance;
            List<string> invalidChars = new List<string>() {"!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-",};

            foreach (string x in invalidChars)
            {
                if (caster.Name.Contains(x))
                {
                    return new ValidationResult("Error | enter valid string with valid chars");
                }
            }

            var result = Validator.TryValidateObject(caster, null, null);
            if (result is false)
            {
                return new ValidationResult("Error trying to validate");
            }

            return ValidationResult.Success;
        }
    }
}