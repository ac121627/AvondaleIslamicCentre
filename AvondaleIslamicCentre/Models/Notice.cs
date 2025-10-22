using AvondaleIslamicCentre.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleIslamicCentre.Models
{
    // Represents a notice or announcement posted by an admin
    public class Notice
    {
        [Key]
        public int NoticeId { get; set; }  // Primary key

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters.")]
        public string Title { get; set; }  // Title of the notice

        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 1000 characters.")]
        public string Message { get; set; }  // The notice content

        [Required]
        [Display(Name = "Posted At")]
        [DataType(DataType.Date)]
        public DateTime PostedAt { get; set; } = DateTime.Now;  // When the notice was posted

        [Display(Name = "Username")]
        public string? AICUserId { get; set; }  // FK to the user who posted it (nullable)
        [ForeignKey("AICUserId")]
        public AICUser? AICUser { get; set; }  // Navigation to the user who posted
    }
}
