
using AvondaleIslamicCentre.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleIslamicCentre.Models
{
    public class Booking
    {
        [Key] public int BookingId { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        [ForeignKey("HallId")]
        public int HallId { get; set; }
        public Hall Hall { get; set; }

        [Required]
        public string AICUserId { get; set; }  
        [ForeignKey("AICUserId")]
        public AICUser AICUser { get; set; }

    }
}