using System;
using System.ComponentModel.DataAnnotations;

namespace AVATI.Data.ValidationAttributes
{
    public class ProjectDateTimeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime time)
            {

                if (DateTime.Compare(time, DateTime.Now.AddDays(-1)) < 0)
                {

                    return new ValidationResult("Das Datum kann nicht vor dem aktuellen Datum liegen! (" +
                                                DateTime.Now.ToString("dd.MM.yy") + ")");
                }
            }



            return ValidationResult.Success;
        }
    }
}