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
        [Range(0.01, double.MaxValue, ErrorMessage = "Donation amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Date Donated")]
        public DateTime DateDonated { get; set; }

        [StringLength(100, ErrorMessage = "Donor name must be under 100 characters.")]
        [Display(Name = "Donor Name")]
        public string? DonorName { get; set; }

        [Required]
        [Display(Name = "Donation Type")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Donation type must be 3 to 100 characters.")]
        public string? DonationType { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Payment method must be 3 to 50 characters.")]
        public string? PaymentMethod { get; set; }

        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
        public string? Description { get; set; }

        [Required]
        [StringLength(450, ErrorMessage = "User ID is invalid.")]
        public string? AICUserId { get; set; }
        [ForeignKey("AICUserId")]
        public AICUser? AICUser { get; set; } // Navigation property
    }
}
