using AvondaleIslamicCentre.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleIslamicCentre.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required, StringLength(50), Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty; // First name of the teacher

        [Required, StringLength(50), Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty; // Last name of the teacher

        [Required, EmailAddress, Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty; // Email address of the teacher

        [Required, Phone, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty; // Phone number of the teacher

        public ICollection<Class> Classes { get; set; } = new List<Class>();// Navigation property to Class   

        public ICollection<Student> Students { get; set; } = new List<Student>(); // Navigation property to multiple Students
    }
}