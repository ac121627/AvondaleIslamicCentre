using AvondaleIslamicCentre.Models;
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

        return ValidationResult.Success;
    }
}
