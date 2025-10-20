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

        //  Minimum 1-hour difference
        if ((endDateTime - start).TotalHours < 1)
        {
            return new ValidationResult("End time must be at least 1 hour after the start time.");
        }

        //  4-hour max limit
        if ((endDateTime - start).TotalHours > 4)
        {
            return new ValidationResult("End time must be within 4 hours after the start time.");
        }

        // Valid booking window (6:00 AM – 7:00 PM)
        var earliest = new TimeSpan(7, 0, 0);
        var latest = new TimeSpan(23, 0, 0);

        if (endTime < earliest || endTime > latest)
        {
            return new ValidationResult("Bookings can only end between 7:00 AM and 11:00 PM.");
        }

        return ValidationResult.Success;
    }
}
