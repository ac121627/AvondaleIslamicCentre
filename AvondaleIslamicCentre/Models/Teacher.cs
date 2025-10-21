using AvondaleIslamicCentre.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleIslamicCentre.Models
{
    public class Teacher
    {
        [Key]
        [Display(Name = "Teacher ID")]
        public int TeacherId { get; set; }

        [Required]
        [Display(Name ="First Name")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First name must be 1–25 characters.")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*(?:[ '\-][A-Za-z][a-zA-Z]*)*$", ErrorMessage = "Name must start with a capital letter and may only contain letters, spaces, hyphens, or apostrophes.")]
        public string FirstName { get; set; }  // First name of the teacher

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "Last name must be 1–25 characters.")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*(?:[ '\-][A-Za-z][a-zA-Z]*)*$", ErrorMessage = "Name must start with a capital letter and may only contain letters, spaces, hyphens, or apostrophes.")]
        public string LastName { get; set; }  // Last name of the teacher

        [Required]
        [EmailAddress]
        [StringLength(50, ErrorMessage = "Email must be under 50 characters.")]
        public string Email { get; set; }  // Email of the teacher

        [Required, Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber), MaxLength(15)]
        [RegularExpression(@"^(\+64\s?\d{1,2}\s?\d{3,4}\s?\d{3,4}|0\d{1,2}\s?\d{3,4}\s?\d{3,4})$",
        ErrorMessage = "Please enter a valid New Zealand phone number (e.g., +64 21 234 5678 or 021 234 5678).")]
        public string PhoneNumber { get; set; }  // Phone number of the guardian

        public ICollection<Class>? Classes { get; set; } = new List<Class>(); // Navigation property to Class   

        public ICollection<Student>? Students { get; set; } = new List<Student>(); // Navigation property to multiple Students
    }
}