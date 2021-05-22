using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization.Infrastructure;


namespace Team12.Data   // source.: https://www.youtube.com/watch?v=o_AH2MGti0A
{                       //source1.: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-5.0  | Custom attributes
    public class SkillNameConventionAttribut : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Skill caster = (Skill) validationContext.ObjectInstance;
            string data = " äöü$§%!&/()=?";     //chars that arent allowed
            List<char> falseChar = new List<char>();
            falseChar.AddRange(data);

            if (caster.type == Skilltype.Softskill)
            {
                if (caster.Name.Contains('ä') || caster.Name.Contains('ö') || caster.Name.Contains('!') ||
                    caster.Name.Contains('ö') || caster.Name.Contains(')') || caster.Name.Contains('§') ||
                    caster.Name.Contains('&') || caster.Name.Contains('(') || caster.Name.Contains('-') ||
                    caster.Name.Contains('%') || caster.Name.Contains('/') || caster.Name.Contains('=') ||
                    caster.Name.Contains('`') || caster.Name.Contains('´') || caster.Name.Contains('^'))        //gibts safe ne andere einfachere möglichkeit
                {
                    return new ValidationResult("Error.: Fehler bei der Eingabe");
                }
            }
            return ValidationResult.Success;
        }
    }
}