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
        [Required(ErrorMessage = "Please enter Teachers First Name")]
        [MinLength(3)]
        [MaxLength(50)]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string FirstName { get; set; } = string.Empty; // First name of the teacher

        [StringLength(50), Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter Teachers Last Name")]
        [MinLength(3)]
        [MaxLength(25)]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string LastName { get; set; } = string.Empty; // Last name of the teacher

        [Required(ErrorMessage = "Please enter an email address")]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty; // Email address of the teacher

        [Required, Phone, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty; // Phone number of the teacher

        public ICollection<Class> Classes { get; set; } = new List<Class>();// Navigation property to Class   

        public ICollection<Student> Students { get; set; } = new List<Student>(); // Navigation property to multiple Students
    }
}