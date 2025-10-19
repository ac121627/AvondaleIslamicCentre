using AvondaleIslamicCentre.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleIslamicCentre.Models
{
    public enum DonationType
    {
        [Display(Name = "Sadaqah")] Sadaqah,
        [Display(Name = "Zakat")] Zakat,
        [Display(Name = "Masjid Donation")] Masjid_Donation,
        [Display(Name = "Madrasah Donation")] Madrasah_Donation,
        [Display(Name = "Palestine")] Palestine,
    }

    public enum PaymentMethod
    {
        [Display(Name = "Cash")] Cash,
        [Display(Name = "Card")] Card
    }
    public class Donation
    {
        [Key]
        public int DonationId { get; set; }

        [Required]
        [Range(0.01, 1000.00, ErrorMessage = "Donation amount must be between $0.01 and $1000.")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Date Donated")]
        [DataType(DataType.Date)]
        public DateTime DateDonated { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Donation Type")]
        public DonationType DonationType { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }

        [StringLength(100, ErrorMessage = "Description must not exceed 100 characters.")]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string? Description { get; set; }

        [Display(Name = "Username")]
        public string? AICUserId { get; set; } // nullable FK
        [ForeignKey("AICUserId")]
        public AICUser? AICUser { get; set; } // Navigation property (nullable)
    }
}
