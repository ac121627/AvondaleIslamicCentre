using System.ComponentModel.DataAnnotations;

namespace AvondaleIslamicCentre.Models
{
    public class Hall
    {
        [Key]
        public int HallId { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = ".")] //Capacity field that indicates how many animals may fit inside the enclosure.
        [Range(1, 200)]
        public int Capacity { get; set; }

        [Required]
        public DateTime AvailableFrom { get; set; }

        [Required]
        public DateTime AvailableTo { get; set; }

        [Required]
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}