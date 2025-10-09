using AvondaleIslamicCentre.Models;
using System;
using System.ComponentModel.DataAnnotations;

public class EndDateTime : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var booking = (Booking)validationContext.ObjectInstance;

        if (booking.EndDateTime <= booking.StartDateTime)
        {
            return new ValidationResult("End time must be after the start time.");
        }

        if ((booking.EndDateTime - booking.StartDateTime).TotalHours > 4)
        {
            return new ValidationResult("End time must be within 4 hours after the start time.");
        }

        var earliest = new TimeSpan(6, 0, 0);  // 6:00 AM
        var latest = new TimeSpan(23, 0, 0);   // 11:00 PM

        if (booking.EndDateTime.TimeOfDay < earliest || booking.EndDateTime.TimeOfDay > latest)
        {
            return new ValidationResult("Bookings can only end between 6:00 AM and 11:00 PM.");
        }

        return ValidationResult.Success;
    }
}
