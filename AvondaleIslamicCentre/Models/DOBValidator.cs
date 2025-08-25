using System.ComponentModel.DataAnnotations;

public class DOBValidator : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;

            if (age < 5 || age > 20)
            {
                return new ValidationResult("Age must be between 5 and 20 years.");
            }

            return ValidationResult.Success;
        }

        return new ValidationResult("Invalid date format.");
    }
}
