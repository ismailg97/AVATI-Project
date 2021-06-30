using System;
using System.ComponentModel.DataAnnotations;

namespace AVATI.Data.ValidationAttributes
{
    public class DateTimeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is Proposal proposal)
            {

                if (proposal.Start.CompareTo(proposal.End) >= 0)
                {
                    return new ValidationResult("Startdatum kann nicht vor Enddatum liegen!");
                }
            } else if (value is Project project)
            {
                if (project.Projectbeginning.CompareTo(project.Projectend) >= 0)
                {
                    return new ValidationResult("Startdatum kann nicht vor Enddatum liegen!");
                }
            }



            return ValidationResult.Success;
        }
    }
}