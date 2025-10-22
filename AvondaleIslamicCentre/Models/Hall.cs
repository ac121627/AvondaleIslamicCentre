using AvondaleIslamicCentre.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleIslamicCentre.Models
{
    // Represents a hall within the mosque that can be booked
    public class Hall
    {
        [Key]
        public int HallId { get; set; }  // Primary key

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Hall name must be between 2 and 30 characters.")]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string Name { get; set; }  // Hall name (e.g., Main Hall)

        [Required]
        [Range(10, 200, ErrorMessage = "Capacity must be between 10 and 200.")]
        public int Capacity { get; set; }  // Maximum people allowed

        public ICollection<Booking>? Bookings { get; set; } = new List<Booking>();  // One hall can have many bookings
    }
}
