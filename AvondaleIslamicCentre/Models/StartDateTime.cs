using System;
using System.ComponentModel.DataAnnotations;

public class StartDateTime : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime startDate)
        {
            var today = DateTime.Now;
            var maxDate = today.AddMonths(2);

            // 1️ Check if start date is in the past
            if (startDate < today)
            {
                return new ValidationResult("Start date and time must not be in the past.");
            }

            // 2️ Check if start date is more than 2 months ahead
            if (startDate > maxDate)
            {
                return new ValidationResult("Start date cannot be more than 2 months ahead.");
            }

            // 3️ Time validation (6 AM – 11 PM)
            var earliest = new TimeSpan(6, 0, 0);   // 6:00 AM
            var latest = new TimeSpan(19, 0, 0);    // 11:00 PM

            if (startDate.TimeOfDay < earliest || startDate.TimeOfDay > latest)
            {
                return new ValidationResult("Bookings can only start between 6:00 AM and 7:00 PM.");
            }

            return ValidationResult.Success;
        }

        return new ValidationResult("Invalid date format.");
    }
}
