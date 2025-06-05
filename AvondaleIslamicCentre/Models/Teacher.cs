using System.ComponentModel.DataAnnotations;

namespace AvondaleIslamicCentre.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }


        public ICollection<Class> Classes { get; set; } = new List<Class>();// Navigation property to Class
        
    }
}