using System.ComponentModel.DataAnnotations;

namespace AvondaleIslamicCentre.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; } = string.Empty; // First name of the teacher

        [Required, StringLength(50)]
        public string LastName { get; set; } = string.Empty; // Last name of the teacher

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty; // Email address of the teacher

        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty; // Phone number of the teacher


        public ICollection<Class> Classes { get; set; } = new List<Class>();// Navigation property to Class   
    }
}