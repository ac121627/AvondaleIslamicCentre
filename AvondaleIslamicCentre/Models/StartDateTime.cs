using System.ComponentModel.DataAnnotations;

public class StartDateTime : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime startDate)
        {
            var today = DateTime.Now;
            var maxDate = today.AddMonths(2);

            if (startDate < today)
            {
                return new ValidationResult("Start date and time must not be in the past.");
            }
            else if (startDate > maxDate)
            {
                return new ValidationResult("Start date cannot be more than 2 months ahead.");
            }

            return ValidationResult.Success;
        }

        return new ValidationResult("Invalid date format.");
    }
}
