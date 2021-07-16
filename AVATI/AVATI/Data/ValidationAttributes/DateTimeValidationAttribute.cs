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

                if (proposal.Start.CompareTo(proposal.End.AddDays(1)) >= 0)
                {
                    return new ValidationResult("Startdatum kann nicht vor Enddatum liegen!");
                }
                if (proposal.ProposalTitle is null or "" || proposal.ProposalTitle.Length > 70)
                {
                    return new ValidationResult("Der Titel ist ungültig!");
                }

                if (proposal.AdditionalInfo is not null && proposal.AdditionalInfo.Length > 150)
                {
                    return new ValidationResult("Die Beschreibung ist zu lang!");
                }
                    
                    
            } 
            else if (value is Project project)
            {
                if (project.Projectbeginning.CompareTo(project.Projectend.AddDays(1)) >= 0)
                {
                    return new ValidationResult("Startdatum kann nicht vor Enddatum liegen!");
                }
                if (project.Projecttitel is null or "" || project.Projecttitel.Length > 70)
                {
                    return new ValidationResult("Der Titel ist ungültig!");
                }

                if (project.Projectdescription is not null && project.Projectdescription.Length > 150)
                {
                    return new ValidationResult("Die Beschreibung ist zu lang!");
                }
                
            }
            return ValidationResult.Success;
        }
    }
}