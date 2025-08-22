using AvondaleIslamicCentre.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleIslamicCentre.Models
{
    public class Hall
    {
        [Key]
        public int HallId { get; set; } // Unique identifier for the hall

        [Required, StringLength(50)]
        [Display(Name = "Hall Name")]
        public string Name { get; set; } = string.Empty; // Name of the hall

        [Required(ErrorMessage = "Please enter a capacity.")]
        [Range(10, 200, ErrorMessage = "Capacity must be between 10 and 200.")]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; } // Maximum number of people the hall can accommodate

        [Required]
        [Display(Name = "Available From")]
        [DateWithinRange(MaxDaysAhead = 28, ErrorMessage = "Available From must be today or within 4 weeks ahead.")]
        public DateTime AvailableFrom { get; set; } = DateTime.Now; // Date and time when the hall becomes available for booking

        [Required]
        [Display(Name = "Available To")]
        [DateWithinRange(MaxDaysAhead = 28, ErrorMessage = "Available To must be today or within 4 weeks ahead.")]
        public DateTime AvailableTo { get; set; } = DateTime.Now; // Date and time when the hall is no longer available for booking

        [Required]
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>(); // Navigation property to Booking, representing all bookings for this hall
    }
}