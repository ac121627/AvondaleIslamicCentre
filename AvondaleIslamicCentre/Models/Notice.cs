using AvondaleIslamicCentre.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleIslamicCentre.Models
{
    public class Notice
    {
        [Key]
        public int NoticeId { get; set; } // Unique ID for the notice
        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 100 characters.")]
        public string Title { get; set; } = string.Empty; // Title of the notice

        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 1000 characters.")]
        public string Message { get; set; } = string.Empty; // Main content of the notice
        [Required]
        [Display(Name = "Posted At")]
        public DateTime PostedAt { get; set; } = DateTime.Now; // When the notice was posted

        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; } // Optional: if the notice gets edited later

        [Required]
        public string AICUserId { get; set; } = string.Empty; // FK to the user who posted it

        [ForeignKey("AICUserId")]
        public AICUser AICUser { get; set; } = new AICUser(); // Navigation to user who posted it
    }
}
