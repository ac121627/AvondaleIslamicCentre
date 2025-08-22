using System;
using System.ComponentModel.DataAnnotations;

namespace AvondaleIslamicCentre.Models
{
    public class DateWithinRangeAttribute : ValidationAttribute
    {
        public int MaxDaysAhead { get; set; } = 28;

        public override bool IsValid(object? value)
        {
            if (value is not DateTime date)
                return false;

            var today = DateTime.Today;
            var maxDate = today.AddDays(MaxDaysAhead);
            if (date < today)
            {
                ErrorMessage = "Date cannot be before today.";
                return false;
            }
            if (date > maxDate)
            {
                ErrorMessage = $"Date cannot be more than {MaxDaysAhead} days ahead.";
                return false;
            }
            return true;
        }
    }
}
