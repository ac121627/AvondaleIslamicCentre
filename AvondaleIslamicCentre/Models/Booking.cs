using AvondaleIslamicCentre.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace AvondaleIslamicCentre.Models
{

    public class Booking
    {
        [Key]
        public int BookingId { get; set; }  // Primary key (unique ID for each booking)

        [Required]
        [Display(Name = "Start Date and Time")]
        [StartDateTime]
        public DateTime StartDateTime { get; set; }  // When the booking starts (date + time)

        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        [EndDateTime]
        public TimeSpan EndDateTime { get; set; }  // When the booking ends (time only)

        [Required]
        [Display(Name = "Hall")]
        public int? HallId { get; set; }  // FK to the Hall being booked
        [ForeignKey("HallId")]
        public Hall? Hall { get; set; }  // Navigation property to the Hall

        [Display(Name = "Username")]
        public string? AICUserId { get; set; }  // FK to the user who made the booking (nullable)
        [ForeignKey("AICUserId")]
        public AICUser? AICUser { get; set; }  // Navigation property to the user
    }
}
