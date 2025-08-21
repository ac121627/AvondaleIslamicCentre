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

        [Required, StringLength(20)]
        [Display(Name = "Class Name")]
        public string ClassName { get; set; } = string.Empty; // Class name with default value

        [Required, StringLength(100)]
        [Display(Name = "Class Description")]
        public string Description { get; set; } = string.Empty; // Class description

        [Required]
        [Display(Name = "Start Date")]
        public int CurrentStudents { get; set; } // Current number of students in the class    

        [Required]
        public int TeacherId { get; set; } // Foreign key to Teacher
        public Teacher? Teacher { get; set; } // Navigation property to Teacher

        public ICollection<Student> Students { get; set; } = new List<Student>(); // Navigation property to multiple Students
    }
}