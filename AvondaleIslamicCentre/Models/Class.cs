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

        [Required(ErrorMessage = "Please enter a class name.")]
        [StringLength(20, ErrorMessage = "Class name cannot exceed 20 characters.")]
        [Display(Name = "Class Name")]
        public string ClassName { get; set; } = string.Empty; // Class name with default value

        [Required(ErrorMessage = "Please enter a class description.")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        [Display(Name = "Class Description")]
        public string Description { get; set; } = string.Empty; // Class description

        [Required(ErrorMessage = "Please enter the number of current students.")]
        [Range(0, 1000, ErrorMessage = "Current students must be between 0 and 1000.")]
        [Display(Name = "Current Students")]
        public int CurrentStudents { get; set; } // Current number of students in the class    

        [Required]
        public int TeacherId { get; set; } // Foreign key to Teacher
        public Teacher? Teacher { get; set; } // Navigation property to Teacher

        public ICollection<Student> Students { get; set; } = new List<Student>(); // Navigation property to multiple Students
    }
}