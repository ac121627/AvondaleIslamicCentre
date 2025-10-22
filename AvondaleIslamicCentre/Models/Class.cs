using AvondaleIslamicCentre.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleIslamicCentre.Models
{
    // Represents a Madrasah class
    public class Class
    {
        [Key]
        [Display(Name = "Class ID")]
        public int ClassId { get; set; }  // Primary key

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Class name must be between 2 and 30 characters.")]
        [Display(Name = "Class Name")]
        public string ClassName { get; set; }  // Name of the class (e.g., Quran Beginners)

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Class description must be between 5 and 100 characters.")]
        public string Description { get; set; }  // Short description about the class

        [Required]
        [Range(1, 30, ErrorMessage = "Number of students must be between 1 and 30.")]
        [Display(Name = "Current Students")]
        public int CurrentStudents { get; set; }  // How many students are currently enrolled

        [Display(Name = "Teacher")]
        public int? TeacherId { get; set; }  // FK to Teacher (nullable)
        public Teacher? Teacher { get; set; }  // Navigation property to Teacher

        public ICollection<Student>? Students { get; set; }  // One class can have many students
    }
}
