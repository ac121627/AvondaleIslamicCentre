
using Microsoft.AspNetCore.Identity;
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
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please provide a valid Last Name.")]
        [PersonalData] // Marks this property as personal data for GDPR purposes
        [RegularExpression(@"^[A-Z][a-z\s]*$", ErrorMessage = "Name must begin with a capital letter and must not include special characters or numbers.")]
        [Column(TypeName = "varchar(100)")] // Specifies database column type
        public string LastName { get; set; }

        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public string AICUserId { get; set; }
        //[ForeignKey("AICUserId")]
        //public AICUser AICUser { get; set; }

    } 
}