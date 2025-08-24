using System.ComponentModel.DataAnnotations;

namespace AvondaleIslamicCentre.Models
{
    public class DateValidation : ValidationAttribute
    {
        // Override the IsValid method to implement custom date validation logic
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Check if a date value is provided
            if (value != null)
            {
                // Cast the input value to a DateTime object
                var date = (DateTime)value;

                // Get the current date and time
                var currentDate = DateTime.Now;

                // Get the name of the object being validated (usually the model's class name)
                var controllerName = validationContext.ObjectType.Name;

                // Logic for validation when the context is in a "Create" operation
                if (controllerName.Contains("Create"))
                {
                    // Set the maximum allowable appointment date to 14 days from the current date
                    var maxDate = currentDate.AddDays(14);

                    // Check if the provided date is before the current date or after the maximum date
                    if (date < currentDate.Date || date > maxDate.Date)
                    {
                        // Return an error if the date is out of range, providing a helpful message to the user
                        return new ValidationResult($"The appointment date must be between {currentDate:d/MM/yyyy} and {maxDate:d/MM/yyyy}.");
                    }
                }
                // Logic for validation when the context is in an "Edit" operation
                else if (controllerName.Contains("Edit"))
                {
                    // Set the minimum allowable date to 1 year before and the maximum to 1 year after the current date
                    var minDate = currentDate.AddYears(-1);
                    var maxDate = currentDate.AddYears(1);

                    // Check if the provided date is out of the allowable range
                    if (date < minDate.Date || date > maxDate.Date)
                    {
                        // Return an error if the date is out of range, providing a helpful message to the user
                        return new ValidationResult($"The appointment date must be between {minDate:d/MM/yyyy} and {maxDate:d/MM/yyyy}.");
                    }
                }
            }

            // If the date is valid or no date is provided, return success
            return ValidationResult.Success;
        }
    }
}
