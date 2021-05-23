using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Team12.Data // source.: https://www.youtube.com/watch?v=o_AH2MGti0A
{ //source1.: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-5.0  | Custom attributes
    public class SkillNameConventionAttribut : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var caster = (Skill) validationContext.ObjectInstance;
            var data = " äöü$§%!&/()=?"; //chars that arent allowed
            var falseChar = new List<char>();
            falseChar.AddRange(data);

            if (caster.type == Skilltype.Softskill)
                if (caster.Name.Contains('ä') || caster.Name.Contains('ö') || caster.Name.Contains('!') ||
                    caster.Name.Contains('ö') || caster.Name.Contains(')') || caster.Name.Contains('§') ||
                    caster.Name.Contains('&') || caster.Name.Contains('(') || caster.Name.Contains('-') ||
                    caster.Name.Contains('%') || caster.Name.Contains('/') || caster.Name.Contains('=') ||
                    caster.Name.Contains('`') || caster.Name.Contains('´') ||
                    caster.Name.Contains('^')) //gibts safe ne andere einfachere möglichkeit
                    return new ValidationResult("Error.: Fehler bei der Eingabe");
            return ValidationResult.Success;
        }
    }
}