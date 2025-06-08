using System.ComponentModel.DataAnnotations;

namespace AvondaleIslamicCentre.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required, StringLength(50)]
        public string GuardianFirstName { get; set; }

        [Required, StringLength(50)]
        public string GuardianLastName { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required, StringLength(10)]
        public string  Gender{ get; set; }

        [Required, StringLength(10)]
        public string Ethnicity { get; set; }

        [Required, StringLength(20)]
        public string QuranNazira { get; set; }

        [Required, StringLength(20)]
        public string QuranHifz { get; set; } 

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required, StringLength(100)]
        public string Address { get; set; }

        public int ClassId { get; set; }
        public Class Class { get; set; } // Navigation property to Class
    }
}