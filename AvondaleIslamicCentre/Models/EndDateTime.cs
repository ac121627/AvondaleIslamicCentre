using AvondaleIslamicCentre.Models;
using System;
using System.ComponentModel.DataAnnotations;

// This custom validation checks if a booking's end time makes sense
public class EndDateTime : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var booking = (Booking)validationContext.ObjectInstance;

        // Make sure an end time is provided
        if (value == null)
            return new ValidationResult("End time is required.");

        // Make sure the time format is valid
        if (value is not TimeSpan endTime)
            return new ValidationResult("Invalid time format.");

        var start = booking.StartDateTime;
        var endDateTime = start.Date + endTime; // Combine start date with end time to compare

        // End time must come after the start time
        if (endDateTime <= start)
        {
            return new ValidationResult("End time must be after the start time.");
        }

        // Booking must last at least 1 hour
        if ((endDateTime - start).TotalHours < 1)
        {
            return new ValidationResult("End time must be at least 1 hour after the start time.");
        }

        // Booking cannot go longer than 4 hours
        if ((endDateTime - start).TotalHours > 4)
        {
            return new ValidationResult("End time must be within 4 hours after the start time.");
        }

        // Allow bookings to end only between 7:00 AM and 11:00 PM
        var earliest = new TimeSpan(7, 0, 0);
        var latest = new TimeSpan(23, 0, 0);

        if (endTime < earliest || endTime > latest)
        {
            return new ValidationResult("Bookings can only end between 7:00 AM and 11:00 PM.");
        }

        // Everything is valid
        return ValidationResult.Success;
    }
}
