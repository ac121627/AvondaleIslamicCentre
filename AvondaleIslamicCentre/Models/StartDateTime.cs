using System;
using System.ComponentModel.DataAnnotations;

// This custom validation checks if a booking's start date and time is valid
public class StartDateTime : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime startDate)
        {
            var today = DateTime.Now;
            var maxDate = today.AddMonths(2);

            // Make sure the start date is not in the past
            if (startDate < today)
            {
                return new ValidationResult("Start date and time must not be in the past.");
            }

            // Make sure the start date is not more than 2 months in the future
            if (startDate > maxDate)
            {
                return new ValidationResult("Start date cannot be more than 2 months ahead.");
            }

            // Allow bookings only between 6:00 AM and 7:00 PM
            var earliest = new TimeSpan(6, 0, 0);
            var latest = new TimeSpan(19, 0, 0);

            if (startDate.TimeOfDay < earliest || startDate.TimeOfDay > latest)
            {
                return new ValidationResult("Bookings can only start between 6:00 AM and 7:00 PM.");
            }

            // Everything looks fine
            return ValidationResult.Success;
        }

        // If the date is in the wrong format
        return new ValidationResult("Invalid date format.");
    }
}
