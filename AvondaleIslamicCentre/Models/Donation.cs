using AvondaleIslamicCentre.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleIslamicCentre.Models
{
    // Different donation types
    public enum DonationType
    {
        [Display(Name = "Sadaqah")] Sadaqah,
        [Display(Name = "Zakat")] Zakat,
        [Display(Name = "Masjid Donation")] Masjid_Donation,
        [Display(Name = "Madrasah Donation")] Madrasah_Donation,
        [Display(Name = "Palestine")] Palestine,
    }

    // Accepted payment methods
    public enum PaymentMethod
    {
        [Display(Name = "Cash")] Cash,
        [Display(Name = "Card")] Card
    }

    // Represents a donation made by a user
    public class Donation
    {
        [Key]
        public int DonationId { get; set; }  // Primary key

        [Required]
        [Range(0.01, 1000.00, ErrorMessage = "Donation amount must be between $0.01 and $1000.")]
        public decimal Amount { get; set; }  // Amount donated

        [Required]
        [Display(Name = "Date Donated")]
        [DataType(DataType.Date)]
        public DateTime DateDonated { get; set; } = DateTime.Now;  // Date donation was made

        [Required]
        [Display(Name = "Donation Type")]
        public DonationType DonationType { get; set; }  // Type of donation (e.g., Zakat)

        [Required]
        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }  // How the donation was paid (cash/card)

        [StringLength(100, ErrorMessage = "Description must not exceed 100 characters.")]
        public string? Description { get; set; }  // Optional message from the donor

        [Display(Name = "Username")]
        public string? AICUserId { get; set; }  // FK to the user who donated (nullable)
        [ForeignKey("AICUserId")]
        public AICUser? AICUser { get; set; }  // Navigation property to user
    }
}
