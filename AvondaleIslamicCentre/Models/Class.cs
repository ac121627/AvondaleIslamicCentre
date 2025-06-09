using System.ComponentModel.DataAnnotations;

namespace AvondaleIslamicCentre.Models
{
    public class Class
    {
        [Key]
        public int ClassId { get; set; } // Primary key with default value

        [Required, StringLength(20)]
        public string ClassName { get; set; } = string.Empty; // Class name with default value

        [Required, StringLength(100)]
        public string Description { get; set; } = string.Empty; // Class description

        [Required]
        public int CurrentStudents { get; set; } // Current number of students in the class    

        [Required]
        public int TeacherId { get; set; } // Foreign key to Teacher
        public Teacher? Teacher { get; set; } // Navigation property to Teacher

        [Required]
        public int StudentId { get; set; } // Foreign key to Student
        public Student? Student { get; set; } // Navigation property to Student
        public ICollection<Student> Students { get; set; } = new List<Student>(); // Navigation property to multiple Students
    }
}