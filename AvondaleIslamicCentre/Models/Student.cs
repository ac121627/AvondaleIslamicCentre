using AvondaleIslamicCentre.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace AvondaleIslamicCentre.Models
{
    public enum Gender
    {
        Male, Female
    }
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Student ID")]
        public int StudentId { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Please enter Guardian First Name")]
        [MinLength(2)]
        [MaxLength(50)]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        [Display(Name = "Guardian First Name")]
        public string GuardianFirstName { get; set; } = string.Empty; // First name of the guardian

        [StringLength(100)]
        [Required(ErrorMessage = "Please enter Guardian Last Name")]
        [MinLength(2)]
        [MaxLength(50)]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        [Display(Name = "Guardian Last Name")]
        public string GuardianLastName { get; set; } = string.Empty; // Last name of the guardian

        [StringLength(100)]
        [Required(ErrorMessage = "Please enter Student First Name")]
        [MinLength(3)]
        [MaxLength(25)]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty; // First name of the student

        [StringLength(100)]
        [Required(ErrorMessage = "Please enter Student Last Name")]
        [MinLength(3)]
        [MaxLength(25)]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty; // Last name of the student

        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "Email must be under 100 characters.")]
        public string Email { get; set; } = string.Empty; // Email of the guardian

        [Required, Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber), MaxLength(17)]
        [RegularExpression(@"^\+((64 (\b(2[0-6])\b)-\d{3,4}-\d{4,5})|(91 \d{5}-\d{5}))$",
        ErrorMessage = "Phone Number is not valid.\n\n" +
               "For New Zealand:\n" +
               "+64 followed by a 2-digit area code (20-26),\n" +
               "a 3- or 4-digit local number,\n" +
               "and a 4- or 5-digit subscriber number.\n" +
               "(e.g., +64 20-345-6789 or +64 22-1234-5678).")]
        public string PhoneNumber { get; set; } = string.Empty; // Phone number of the guardian

        [Required]
        [StringLength(10, ErrorMessage = "Gender must be under 10 characters.")]
        public string Gender { get; set; } = string.Empty; 

        [Required]
        [StringLength(10, ErrorMessage = "Ethnicity must be under 10 characters.")]
        public string Ethnicity { get; set; } = string.Empty; 

        [Required]
        [StringLength(20, ErrorMessage = "Quran Nazira must be under 20 characters.")]
        [Display(Name = "Quran Nazira")]
        public string QuranNazira { get; set; } = string.Empty;

        [Required]
        [StringLength(20, ErrorMessage = "Quran Hifz must be under 20 characters.")]
        [Display(Name = "Quran Hifz")]
        public string QuranHifz { get; set; } = string.Empty; 

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Address must be 5–100 characters.")]
        public string Address { get; set; } = string.Empty; // Address of the guardian

        [Required]
        [Display(Name = "Date of Birth")]
        [DOBValidator]
        public DateTime DateOfBirth { get; set; } // Date of birth of the student

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Class.")]
        public int ClassId { get; set; }
        public Class? Class { get; set; } // Navigation property to Class

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Teacher.")]
        public int TeacherId { get; set; } // Foreign key to Teacher
        public Teacher? Teacher { get; set; } // Navigation property to Teacher
    }
}