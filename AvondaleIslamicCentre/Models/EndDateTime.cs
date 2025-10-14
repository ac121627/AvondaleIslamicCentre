using AvondaleIslamicCentre.Models;
using System;
using System.ComponentModel.DataAnnotations;

public class EndDateTime : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var booking = (Booking)validationContext.ObjectInstance;

        if (value == null)
            return new ValidationResult("End time is required.");

        if (value is not TimeSpan endTime)
            return new ValidationResult("Invalid time format.");

        var start = booking.StartDateTime;
        var endDateTime = start.Date + endTime; // combine start date with end time for comparison

        if (endDateTime <= start)
        {
            return new ValidationResult("End time must be after the start time.");
        }

        if ((endDateTime - start).TotalHours > 4)
        {
            return new ValidationResult("End time must be within 4 hours after the start time.");
        }

        var earliest = new TimeSpan(6, 0, 0); // 6:00 AM
        var latest = new TimeSpan(19, 0, 0);  // 7:00 PM

        if (endTime < earliest || endTime > latest)
        {
            return new ValidationResult("Bookings can only end between 6:00 AM and 7:00 PM.");
        }

        return ValidationResult.Success;
    }
}
