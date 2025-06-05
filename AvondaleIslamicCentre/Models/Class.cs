using System.ComponentModel.DataAnnotations;

namespace AvondaleIslamicCentre.Models
{
    public class Class
    {
        [Key]
        public int ClassId { get; set; }

        [Required, StringLength(20)]
        public string ClassName { get; set; }

        [Required, StringLength(100)]
        public string Description { get; set; }

        [Required]
        public int CurrentStudents { get; set; }

        [Required]
        public int TeacherId { get; set; } // Foreign key to Teacher
        public Teacher Teacher { get; set; } // Navigation property to Teacher

        [Required]
        public int studentId { get; set; } // Foreign key to Student
        public Student Student { get; set; } // Navigation property to Student
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}