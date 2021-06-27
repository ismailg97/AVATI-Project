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

                if (DateTime.Compare(time, DateTime.Now) < 0)
                {
                    Console.WriteLine("DAMMMMM");
                    return new ValidationResult("Das Datum kann nicht vor dem aktuellen Datum liegen! (" +
                                                DateTime.Now.ToString("dd.MM.yy") + ")");
                }
            }



            return ValidationResult.Success;
        }
    }
}