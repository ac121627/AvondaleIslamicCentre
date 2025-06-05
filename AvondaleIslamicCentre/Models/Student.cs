using System.ComponentModel.DataAnnotations;

namespace AvondaleIslamicCentre.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string ParentEmail { get; set; }

        [Required, Phone]
        public string ParentPhoneNumber { get; set; }

        [Required]
        public int Age { get; set; }

        [Required, StringLength(100)]
        public string Address { get; set; }


        public ICollection<Class> Classes { get; set; } = new List<Class>(); // Navigation property to Class
    }
}