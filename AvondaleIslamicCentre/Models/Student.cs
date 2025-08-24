using AvondaleIslamicCentre.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
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

        [StringLength(50), Display(Name = "Guardian First Name")]
        [Required(ErrorMessage = "Please enter Guardian First Name")]
        [MinLength(2)]
        [MaxLength(50)]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string GuardianFirstName { get; set; } = string.Empty; // First name of the guardian

        [StringLength(50), Display(Name = "Guardian Last Name")]
        [Required(ErrorMessage = "Please enter Guardian Last Name")]
        [MinLength(2)]
        [MaxLength(50)]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string GuardianLastName { get; set; } = string.Empty; // Last name of the guardian

        [StringLength(50), Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter Student First Name")]
        [MinLength(3)]
        [MaxLength(25)]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string FirstName { get; set; } = string.Empty; // First name of the student

        [StringLength(50), Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter Student Last Name")]
        [MinLength(3)]
        [MaxLength(25)]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string LastName { get; set; } = string.Empty; // Last name of the student

        [Required(ErrorMessage = "Please enter an email address")]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty; // Email address of the student

        [Required, Phone, Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty; // Phone number of the student

        [Required, StringLength(10), Display(Name ="Gender")]
        public string Gender { get; set; } = string.Empty; // Gender of the student

        [Required, StringLength(10)]
        public string Ethnicity { get; set; } = string.Empty; // Ethnicity of the student

        [Required, StringLength(20), Display(Name = "Quran Nazira")]
        public string QuranNazira { get; set; } = string.Empty; // Quran Nazira level of the student

        [Required, StringLength(20), Display(Name = "Quran Hifz")]
        public string QuranHifz { get; set; } = string.Empty; // Quran Hifz level of the student

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$", ErrorMessage = "Date of Birth must be in the format dd/MM/yyyy.")]
        [Required(ErrorMessage = "Please enter Date of Birth")]
        [Column(TypeName = "date")]
        [Display(Name = "Date of Birth")]
        [DOBValidator(ErrorMessage = "Date of Birth cannot be in the future and age must be less than 100 years.")]
        public DateTime DateOfBirth { get; set; }

        [Required, StringLength(100)]
        public string Address { get; set; } = string.Empty; // Address of the student

        public int ClassId { get; set; }
        public Class? Class { get; set; } // Navigation property to Class

        [Required]
        public int TeacherId { get; set; } // Foreign key to Teacher
        public Teacher? Teacher { get; set; } // Navigation property to Teacher
    }
}