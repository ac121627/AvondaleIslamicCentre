using System.ComponentModel.DataAnnotations;

namespace AvondaleIslamicCentre.Models
{
    public class Hall
    {
        [Key]
        public int HallId { get; set; } // Unique identifier for the hall

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty; // Name of the hall

        [Required(ErrorMessage = ".")] //Capacity field that indicates how many animals may fit inside the enclosure.
        [Range(1, 200)]
        public int Capacity { get; set; } // Maximum number of people the hall can accommodate

        [Required]
        public DateTime AvailableFrom { get; set; } // Date and time when the hall becomes available for booking

        [Required]
        public DateTime AvailableTo { get; set; } // Date and time when the hall is no longer available for booking

        [Required]
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>(); // Navigation property to Booking, representing all bookings for this hall
    }
}