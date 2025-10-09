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
        [Range(0.01, double.MaxValue, ErrorMessage = "Donation amount must be greater than zero.")]
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

        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string? AICUserId { get; set; }
        [ForeignKey("AICUserId")]
        public AICUser? AICUser { get; set; } // Navigation property
    }
}
