using System.ComponentModel.DataAnnotations;

namespace AvondaleIslamicCentre.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required, StringLength(50)]
        public string GuardianFirstName { get; set; } = string.Empty; // First name of the guardian

        [Required, StringLength(50)]
        public string GuardianLastName { get; set; } = string.Empty; // Last name of the guardian

        [Required, StringLength(50)]
        public string FirstName { get; set; } = string.Empty; // First name of the student

        [Required, StringLength(50)]
        public string LastName { get; set; } = string.Empty; // Last name of the student

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty; // Email address of the student

        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty; // Phone number of the student

        [Required, StringLength(10)]
        public string Gender { get; set; } = string.Empty; // Gender of the student

        [Required, StringLength(10)]
        public string Ethnicity { get; set; } = string.Empty; // Ethnicity of the student

        [Required, StringLength(20)]
        public string QuranNazira { get; set; } = string.Empty; // Quran Nazira level of the student

        [Required, StringLength(20)]
        public string QuranHifz { get; set; } = string.Empty; // Quran Hifz level of the student

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required, StringLength(100)]
        public string Address { get; set; } = string.Empty; // Address of the student

        public int ClassId { get; set; }
        public Class? Class { get; set; } // Navigation property to Class
    }
}