using AvondaleIslamicCentre.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleIslamicCentre.Models
{
    public class Donation
    {
        [Key]
        public int DonationId { get; set; }

        [Required]
        [Display(Name = "Donation Amount")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Donation amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Date Donated")]
        public DateTime DateDonated { get; set; } = DateTime.Now;

        [MaxLength(100)]
        [Display(Name = "Donor Name")]
        public string? DonorName { get; set; }  // Optional for anonymous donations

        [Required, MaxLength(100)]
        [Display(Name = "Donation Type")]
        public string? DonationType { get; set; } // e.g. Zakat, Sadaqah, General, Building Fund

        [Required, MaxLength(50)]
        [Display(Name = "Payment Method")]
        public string? PaymentMethod { get; set; } // e.g. Cash, Credit Card, Bank Transfer

        [MaxLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; } // For admin or donor comments

        // Foreign key to AICUser
        [Required]
        public string? AICUserId { get; set; } 
        [ForeignKey("AICUserId")]
        public AICUser? AICUser { get; set; } // Navigation property
    }
}
