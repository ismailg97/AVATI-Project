using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization.Infrastructure;


namespace Team12.Data // source.: https://www.youtube.com/watch?v=o_AH2MGti0A
{ //source1.: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-5.0  | Custom attributes
    public class SkillNameConventionAttribut : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Skill caster = (Skill) validationContext.ObjectInstance;
            
            if (caster.type == Skilltype.Softskill)
            {
                if (!caster.Name.All(c=>(Char.IsLetter(c) || c ==' ' || c=='ä' ||  c=='ö' || c=='ü' || c=='Ä' || c=='Ö' || c=='Ü' || c == '!' || c == '"' || c == '§' || c == '$'  || c == '&')))
                    {
                        return new ValidationResult("Der uebergebene SoftSkill wurde nicht richtig bennant");
                    }       
                
            }
            return ValidationResult.Success;
        }
    }
}