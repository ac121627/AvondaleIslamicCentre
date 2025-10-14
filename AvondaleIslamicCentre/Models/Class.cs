using AvondaleIslamicCentre.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleIslamicCentre.Models
{
    public class Class
    {
        [Key]
        [Display(Name = "Class ID")]
        public int ClassId { get; set; } // Primary key with default value

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Class name must be between 2 and 30 characters.")]
        [Display(Name = "Class Name")]
        public string ClassName { get; set; }  // Name of the class

        [Required]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Class description must be between 5 and 1000 characters.")]
        public string Description { get; set; } // Description of the class

        [Required]
        [Range(0, 200, ErrorMessage = "Number of students must be between 0 and 200.")]
        [Display(Name = "Current Students")]
        public int CurrentStudents { get; set; } // Current number of students in the class

        [Display(Name = "Teacher")]
        public int? TeacherId { get; set; } // Foreign key to Teacher (nullable)
        public Teacher? Teacher { get; set; } // Navigation property to Teacher (nullable)

        public ICollection<Student>? Students { get; set; } // Navigation property to multiple Students (nullable)
    }
}