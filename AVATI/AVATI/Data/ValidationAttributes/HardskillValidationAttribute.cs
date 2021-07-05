using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AVATI.Data.ValidationAttributes
{
    public class HardskillValidationAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is Hardskill skillOrCat)
            {
                if (!skillOrCat.IsHardskill)
                {
                    if(skillOrCat.Uppercat.Count > 1)
                        return new ValidationResult("Eine Hardskill-Kategorie kann wiederrum nur höchstens eine Kategorie als Oberkategorie besitzen");
                }
                else
                {
                    if (skillOrCat.Subcat != null)
                    {
                        if(skillOrCat.Subcat.Count != 0)
                            return new ValidationResult("Ein Hardskill kann keine Unterkategorien besitzen");
                    }
                }
            }
            
            return ValidationResult.Success;
        }

    }
}