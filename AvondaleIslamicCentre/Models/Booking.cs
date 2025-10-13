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
        //[StartDateTime]
        public DateTime StartDateTime { get; set; } 

        [Required]
        [Display(Name = "End Time")]
        //[EndDateTime]
        public DateTime EndDateTime { get; set; } 

        [Required]
        [Display(Name ="Hall")]
        public int HallId { get; set; }
        [ForeignKey("HallId")]
        public Hall? Hall { get; set; } // Navigation property to Hall

        [Required]
        [Display(Name = "Username")]
        public string? AICUserId { get; set; }// Default value to ensure it is not null
        [ForeignKey("AICUserId")]
        public AICUser? AICUser { get; set; }  // Navigation property to AICUser

    }
} 