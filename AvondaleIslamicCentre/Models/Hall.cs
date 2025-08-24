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

        [Required]
        [Range(10, 200)]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; } // Maximum number of people the hall can accommodate

        [Required]
        [Display(Name = "Available From")]
        public DateTime AvailableFrom { get; set; } = DateTime.Now; // Date and time when the hall becomes available for booking

        [Required]
        [Display(Name = "Available To")]
        public DateTime AvailableTo { get; set; } = DateTime.Now; // Date and time when the hall is no longer available for booking

        [Required]
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>(); // Navigation property to Booking, representing all bookings for this hall
    }
}