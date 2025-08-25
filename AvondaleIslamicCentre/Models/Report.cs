using Microsoft.AspNetCore.Identity;
using AvondaleIslamicCentre.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleIslamicCentre.Models
{
    public class Report
    {
        [Required, Key]
        public int ReportId { get; set; }

        [Required(ErrorMessage = "Please provide a valid First Name.")]
        [PersonalData] // Marks this property as personal data for GDPR purposes
        [RegularExpression(@"^[A-Z][a-z\s]*$", ErrorMessage = "Name must begin with a capital letter and must not include special characters or numbers.")]
        [Column(TypeName = "varchar(100)"), Display(Name = "First Name")] // Specifies database column type
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide a valid Last Name.")]
        [PersonalData] // Marks this property as personal data for GDPR purposes
        [RegularExpression(@"^[A-Z][a-z\s]*$", ErrorMessage = "Name must begin with a capital letter and must not include special characters or numbers.")]
        [Column(TypeName = "varchar(100)"), Display(Name = "Last Name")] // Specifies database column type
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 100 characters.")]
        public string Title { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 1000 characters.")]
        public string Message { get; set; }

        [Required, StringLength(50), Display(Name = "Created By")]
        public string CreatedBy { get; set; } = string.Empty;

        [Required, StringLength(50), Display(Name = "Updated By")]
        public string UpdatedBy { get; set; } = string.Empty;

        [Required]
        public required string AICUserId { get; set; } // Foreign key to AICUser
        [ForeignKey("AICUserId")]
        public required AICUser AICUser { get; set; } // Navigation property to AICUser
    } 
}