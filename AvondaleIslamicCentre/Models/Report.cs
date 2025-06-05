
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
        [Column(TypeName = "varchar(100)")] // Specifies database column type
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide a valid Last Name.")]
        [PersonalData] // Marks this property as personal data for GDPR purposes
        [RegularExpression(@"^[A-Z][a-z\s]*$", ErrorMessage = "Name must begin with a capital letter and must not include special characters or numbers.")]
        [Column(TypeName = "varchar(100)")] // Specifies database column type
        public string LastName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty; // Description of the report

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Automatically set to current time when created

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Automatically set to current time when created

        public string CreatedBy { get; set; } = string.Empty;

        public string UpdatedBy { get; set; } = string.Empty; 

        public string AICUserId { get; set; } // Foreign key to AICUser
        [ForeignKey("AICUserId")]
        public AICUser AICUser { get; set; } // Navigation property to AICUser
    } 
}