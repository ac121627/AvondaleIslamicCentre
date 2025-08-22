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

        [StringLength(50), Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter Teacher's First Name")]
        [MinLength(3, ErrorMessage = "First Name must be at least 3 characters.")]
        [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string FirstName { get; set; } = string.Empty; // First name of the teacher

        [StringLength(50), Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter Teacher's Last Name")]
        [MinLength(3, ErrorMessage = "Last Name must be at least 3 characters.")]
        [MaxLength(25, ErrorMessage = "Last Name cannot exceed 25 characters.")]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string LastName { get; set; } = string.Empty; // Last name of the teacher

        [Required(ErrorMessage = "Please enter an email address")]
        [MaxLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty; // Email address of the teacher

        [Required(ErrorMessage = "Please enter a phone number")]
        [Phone]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(17, ErrorMessage = "Phone number cannot exceed 17 characters.")]
        [RegularExpression(@"^\+((64 (\b(2[0-6])\b)-\d{3,4}-\d{4,5})|(91 \d{5}-\d{5}))$", ErrorMessage = "Phone Number is not valid.")]
        public string PhoneNumber { get; set; } = string.Empty; // Phone number of the teacher

        public ICollection<Class> Classes { get; set; } = new List<Class>();// Navigation property to Class   
        public ICollection<Student> Students { get; set; } = new List<Student>(); // Navigation property to multiple Students
    }
}