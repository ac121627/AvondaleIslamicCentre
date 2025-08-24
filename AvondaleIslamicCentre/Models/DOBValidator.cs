using System.ComponentModel.DataAnnotations;

namespace AvondaleIslamicCentre.Models
{
    //public class DOBValidator
    //{
        /*public static bool IsValidStartDate(DateTime startDate)
        {
            // Ensure the start date is not in the past
            return startDate.Date >= DateTime.Now.Date;
        }
        public static bool IsValidEndDate(DateTime endDate) {
            // Ensure the end date is not in the past
            return endDate.Date >= DateTime.Now.Date;
        }
        public static bool IsEndDateAfterStartDate(DateTime startDate, DateTime endDate) {
            // Ensure the end date is after the start date
            return endDate > startDate;
        }
        public static bool IsWithinHallAvailability(DateTime startDate, DateTime endDate, DateTime hallAvailableFrom, DateTime hallAvailableTo) {
            // Ensure the booking dates are within the hall's availability
            return startDate >= hallAvailableFrom && endDate <= hallAvailableTo;
        }*/

        // Custom validation attribute for validating a date of birth range
        public class DOBValidator : ValidationAttribute
        {
            // Override the IsValid method to implement custom date validation logic
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                // Check if a value is provided (not null)
                if (value != null)
                {
                    // Cast the value to a DateTime object
                    var date = (DateTime)value;

                    // Calculate the minimum allowable date (100 years ago from the current date)
                    var minDate = DateTime.Now.AddYears(-100);

                    // Calculate the maximum allowable date (16 years ago from the current date)
                    var maxDate = DateTime.Now.AddYears(-16);

                    // Check if the provided date is outside the valid range
                    if (date < minDate || date > maxDate)
                    {
                        // Return a validation error with a message if the date is out of range
                        return new ValidationResult($"The date of birth must be between {minDate:dd/MM/yyyy} and {maxDate:dd/MM/yyyy}.");
                    }
                }

                // Return success if the date is valid or no date is provided
                return ValidationResult.Success;
            }
        }
    //}
}
