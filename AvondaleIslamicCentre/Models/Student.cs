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

    public enum Ethnicity
    {
        [Display(Name = "Indian")] Indian,
        [Display(Name = "Pakistani")] Pakistani,
        [Display(Name = "Sri Lankan")] Sri_Lankan,
        [Display(Name = "Bangladeshi")] Bangladeshi,
        [Display(Name = "Arab")] Arab,
        [Display(Name = "Malaysian")] Malaysian,
        [Display(Name = "Indonesian")] Indonesian,
        [Display(Name = "Other Asian")] Other_Asian,
        [Display(Name = "African")] African,
        [Display(Name = "European")] European,
        [Display(Name = "North American")] North_American,
        [Display(Name = "South American")] South_American,
        [Display(Name = "Kiwi / New Zealander")] Kiwi,
        [Display(Name = "Fijian")] Fijian
    }

    public enum QuranLevel
    {
        [Display(Name = "Not Started")] Not_Started = 0,
        [Display(Name = "Juz Amma")] Juz_Amma = 1,
        [Display(Name = "Juz 1")] Juz_1,
        [Display(Name = "Juz 2")] Juz_2,
        [Display(Name = "Juz 3")] Juz_3,
        [Display(Name = "Juz 4")] Juz_4,
        [Display(Name = "Juz 5")] Juz_5,
        [Display(Name = "Juz 6")] Juz_6,
        [Display(Name = "Juz 7")] Juz_7,
        [Display(Name = "Juz 8")] Juz_8,
        [Display(Name = "Juz 9")] Juz_9,
        [Display(Name = "Juz 10")] Juz_10,
        [Display(Name = "Juz 11")] Juz_11,
        [Display(Name = "Juz 12")] Juz_12,
        [Display(Name = "Juz 13")] Juz_13,
        [Display(Name = "Juz 14")] Juz_14,
        [Display(Name = "Juz 15")] Juz_15,
        [Display(Name = "Juz 16")] Juz_16,
        [Display(Name = "Juz 17")] Juz_17,
        [Display(Name = "Juz 18")] Juz_18,
        [Display(Name = "Juz 19")] Juz_19,
        [Display(Name = "Juz 20")] Juz_20,
        [Display(Name = "Juz 21")] Juz_21,
        [Display(Name = "Juz 22")] Juz_22,
        [Display(Name = "Juz 23")] Juz_23,
        [Display(Name = "Juz 24")] Juz_24,
        [Display(Name = "Juz 25")] Juz_25,
        [Display(Name = "Juz 26")] Juz_26,
        [Display(Name = "Juz 27")] Juz_27,
        [Display(Name = "Juz 28")] Juz_28,
        [Display(Name = "Juz 29")] Juz_29,
        [Display(Name = "Juz 30")] Juz_30
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
        public string GuardianFirstName { get; set; }  // First name of the guardian

        [StringLength(100)]
        [Required(ErrorMessage = "Please enter Guardian Last Name")]
        [MinLength(2)]
        [MaxLength(50)]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        [Display(Name = "Guardian Last Name")]
        public string GuardianLastName { get; set; }  // Last name of the guardian

        [StringLength(100)]
        [Required(ErrorMessage = "Please enter Student First Name")]
        [MinLength(3)]
        [MaxLength(25)]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        [Display(Name = "Student First Name")]
        public string FirstName { get; set; }  // First name of the student

        [StringLength(100)]
        [Required(ErrorMessage = "Please enter Student Last Name")]
        [MinLength(3)]
        [MaxLength(25)]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        [Display(Name = "Student Last Name")]
        public string LastName { get; set; }  // Last name of the student

        [Required]
        [EmailAddress]
        [Display(Name = "Guardian Email")]
        [StringLength(100, ErrorMessage = "Email must be under 100 characters.")]
        public string Email { get; set; } // Email of the guardian

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Address must be 5–100 characters.")]
        public string Address { get; set; }// Address of the guardian

        [Required]
        [Display(Name = "Student Date of Birth")]
        //[DOBValidator]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } // Date of birth of the student

        [Required, Display(Name = "Guardian Phone Number")]
        [DataType(DataType.PhoneNumber), MaxLength(17)]
        [RegularExpression(@"^\+((64 (\b(2[0-6])\b)-\d{3,4}-\d{4,5})|(91 \d{5}-\d{5}))$",
        ErrorMessage = "Phone Number is not valid.\n\n" +
               "In New Zealand:\n" +
               "+64 followed by a 2-digit area code (20-26),\n" +
               "a 3- or 4-digit local number,\n" +
               "and a 4- or 5-digit subscriber number.\n" +
               "(e.g., +64 20-345-6789 or +64 22-1234-5678).")]
        public string PhoneNumber { get; set; } // Phone number of the guardian

        [Required]
        [Display(Name = "Student Gender")]
        public Gender Gender { get; set; }

        [Required]
        [Display(Name = "Student Ethnicity")]
        public Ethnicity Ethnicity { get; set; }

        [Required]
        [Display(Name = "Quran Nazira")]
        public QuranLevel QuranNazira { get; set; }

        [Required]
        [Display(Name = "Quran Hifz")]
        public QuranLevel QuranHifz { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Class.")]
        [Display(Name = "Class")]
        public int ClassId { get; set; }
        public Class? Class { get; set; } // Navigation property to Class

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Teacher.")]
        [Display(Name = "Teacher")]
        public int TeacherId { get; set; } // Foreign key to Teacher
        public Teacher? Teacher { get; set; } // Navigation property to Teacher
    }
}