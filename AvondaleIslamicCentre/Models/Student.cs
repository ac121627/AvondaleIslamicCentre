using AvondaleIslamicCentre.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleIslamicCentre.Models
{
    // Gender options for students
    public enum Gender { Male, Female }

    // Ethnicity options for students
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

    // Quran progress levels
    public enum QuranLevel
    {
        [Display(Name = "Not Started")] Not_Started = 0,
        [Display(Name = "Juz Amma")] Juz_Amma = 1,
        [Display(Name = "Juz 1")] Juz_1, [Display(Name = "Juz 2")] Juz_2, [Display(Name = "Juz 3")] Juz_3,
        [Display(Name = "Juz 4")] Juz_4, [Display(Name = "Juz 5")] Juz_5, [Display(Name = "Juz 6")] Juz_6,
        [Display(Name = "Juz 7")] Juz_7, [Display(Name = "Juz 8")] Juz_8, [Display(Name = "Juz 9")] Juz_9,
        [Display(Name = "Juz 10")] Juz_10, [Display(Name = "Juz 11")] Juz_11, [Display(Name = "Juz 12")] Juz_12,
        [Display(Name = "Juz 13")] Juz_13, [Display(Name = "Juz 14")] Juz_14, [Display(Name = "Juz 15")] Juz_15,
        [Display(Name = "Juz 16")] Juz_16, [Display(Name = "Juz 17")] Juz_17, [Display(Name = "Juz 18")] Juz_18,
        [Display(Name = "Juz 19")] Juz_19, [Display(Name = "Juz 20")] Juz_20, [Display(Name = "Juz 21")] Juz_21,
        [Display(Name = "Juz 22")] Juz_22, [Display(Name = "Juz 23")] Juz_23, [Display(Name = "Juz 24")] Juz_24,
        [Display(Name = "Juz 25")] Juz_25, [Display(Name = "Juz 26")] Juz_26, [Display(Name = "Juz 27")] Juz_27,
        [Display(Name = "Juz 28")] Juz_28, [Display(Name = "Juz 29")] Juz_29, [Display(Name = "Juz 30")] Juz_30
    }

    // Represents a student enrolled in the Madrasah
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Student ID")]
        public int StudentId { get; set; }  // Primary key

        [Required, StringLength(25)]
        [Display(Name = "Guardian First Name")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*(?:[ '\-][A-Za-z][a-zA-Z]*)*$",
    ErrorMessage = "Name must start with a capital letter and may only contain letters, spaces, hyphens, or apostrophes.")]
        public string GuardianFirstName { get; set; }  // Guardian’s first name

        [Required, StringLength(25)]
        [Display(Name = "Guardian Last Name")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*(?:[ '\-][A-Za-z][a-zA-Z]*)*$",
    ErrorMessage = "Name must start with a capital letter and may only contain letters, spaces, hyphens, or apostrophes.")]
        public string GuardianLastName { get; set; }  // Guardian’s last name

        [Required, StringLength(25)]
        [Display(Name = "Student First Name")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*(?:[ '\-][A-Za-z][a-zA-Z]*)*$",
    ErrorMessage = "Name must start with a capital letter and may only contain letters, spaces, hyphens, or apostrophes.")]
        public string FirstName { get; set; }  // Student’s first name

        [Required, StringLength(25)]
        [Display(Name = "Student Last Name")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*(?:[ '\-][A-Za-z][a-zA-Z]*)*$",
    ErrorMessage = "Name must start with a capital letter and may only contain letters, spaces, hyphens, or apostrophes.")]
        public string LastName { get; set; }  // Student’s last name

        [Required, EmailAddress, StringLength(50)]
        [Display(Name = "Guardian Email")]
        public string Email { get; set; }  // Guardian’s email address

        [Required, Display(Name = "Address")]
        [RegularExpression(@"^[a-zA-Z0-9\s,'\-\/\.#]+$", ErrorMessage = "Please enter a valid address.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Address must be 5–100 characters.")]
        public string Address { get; set; }  // Guardian’s address

        [Required, DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }  // Student’s date of birth

        [Required, MaxLength(15)]
        [Display(Name = "Guardian Phone Number")]
        [RegularExpression(@"^(\+64\s?\d{1,2}\s?\d{3,4}\s?\d{3,4}|0\d{1,2}\s?\d{3,4}\s?\d{3,4})$",
            ErrorMessage = "Please enter a valid New Zealand phone number.")]
        public string PhoneNumber { get; set; }  // Guardian’s contact number

        [Required]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }  // Student’s gender

        [Required]
        [Display(Name = "Ethnicity")]
        public Ethnicity Ethnicity { get; set; }  // Student’s ethnicity

        [Required]
        [Display(Name = "Quran Nazira")]
        public QuranLevel QuranNazira { get; set; }  // Current Nazira progress

        [Required]
        [Display(Name = "Quran Hifz")]
        public QuranLevel QuranHifz { get; set; }  // Current Hifz progress

        [Display(Name = "Class")]
        public int? ClassId { get; set; }  // FK to Class (nullable)
        public Class? Class { get; set; }  // Navigation property

        [Display(Name = "Teacher")]
        public int? TeacherId { get; set; }  // FK to Teacher 
        public Teacher? Teacher { get; set; }  // Navigation property
    }
}
