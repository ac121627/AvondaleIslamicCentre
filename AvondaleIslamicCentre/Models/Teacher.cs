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
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be 3–50 characters.")]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string FirstName { get; set; } = string.Empty; // First name of the teacher

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must be 3–50 characters.")]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string LastName { get; set; } = string.Empty; // Last name of the teacher

        [Required]
        [EmailAddress]
        [StringLength(50, ErrorMessage = "Email must be under 50 characters.")]
        public string Email { get; set; } = string.Empty; // Email of the teacher

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(17, ErrorMessage = "Phone number cannot exceed 17 digits.")]
        [RegularExpression(@"^\+((64 (\b(2[0-6])\b)-\d{3,4}-\d{4,5})|(91 \d{5}-\d{5}))$", ErrorMessage = "Phone Number is not valid.")]
        public string PhoneNumber { get; set; } = string.Empty; // Phone number of the teacher

        public ICollection<Class> Classes { get; set; } = new List<Class>();// Navigation property to Class   

        public ICollection<Student> Students { get; set; } = new List<Student>(); // Navigation property to multiple Students
    }
}