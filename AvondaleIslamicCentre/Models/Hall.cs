using AvondaleIslamicCentre.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleIslamicCentre.Models
{
    public class Hall
    {
        [Key]
        public int HallId { get; set; } // Unique identifier for the hall

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Hall name must be between 2 and 50 characters.")]
        public string Name { get; set; } = string.Empty; // Name of the hall

        [Required]
        [Range(10, 200, ErrorMessage = "Capacity must be between 10 and 200.")]
        public int Capacity { get; set; } // Maximum number of people the hall can accommodate

        [Required]
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>(); // Navigation property to Booking, representing all bookings for this hall
    }
}