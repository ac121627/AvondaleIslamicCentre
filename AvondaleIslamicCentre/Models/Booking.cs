using AvondaleIslamicCentre.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;


namespace AvondaleIslamicCentre.Models
{
    public class Booking 
    {
        [Key] public int BookingId { get; set; }

        [Required]
        [Display(Name = "Start Date and Time")]
        [StartDateTime]
        public DateTime StartDateTime { get; set; } 

        [Required]
        [Display(Name = "End Time")]
        [EndDateTime]
        public DateTime EndDateTime { get; set; } 

        [Required]
        [ForeignKey("HallId")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a hall.")]
        public int HallId { get; set; }
        public Hall Hall { get; set; } = new Hall(); // Navigation property to Hall

        [Required]
        public string AICUserId { get; set; } = string.Empty; // Default value to ensure it is not null
        [ForeignKey("AICUserId")]
        public AICUser AICUser { get; set; } = new AICUser(); // Navigation property to AICUser

    }
} 