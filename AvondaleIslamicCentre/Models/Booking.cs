using AvondaleIslamicCentre.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace AvondaleIslamicCentre.Models
{
    public class Booking
    {
        [Key] public int BookingId { get; set; } 

        [Required, Display(Name = "Start Date and Time")]
        [DateWithinRange(MaxDaysAhead = 28, ErrorMessage = "Start date must be today or within 4 weeks ahead.")]
        public DateTime StartDateTime { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "End Date and Time")]
        [DateWithinRange(MaxDaysAhead = 28, ErrorMessage = "End date must be today or within 4 weeks ahead.")]
        public DateTime EndDateTime { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("HallId")]
        public int HallId { get; set; } = 0; // Default value to ensure it is not null
        public Hall Hall { get; set; } = new Hall(); // Navigation property to Hall

        [Required]
        public string AICUserId { get; set; } = string.Empty; // Default value to ensure it is not null
        [ForeignKey("AICUserId")]
        public AICUser AICUser { get; set; } = new AICUser(); // Navigation property to AICUser
    }
}