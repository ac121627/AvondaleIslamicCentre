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

        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty; // Title of the notice

        [Required, StringLength(1000)]
        public string Message { get; set; } = string.Empty; // Full content of the notice

        [Required]
        public DateTime PostedAt { get; set; } = DateTime.Now; // When the notice was posted

        public DateTime? UpdatedAt { get; set; } // Optional: if the notice gets edited later

        [Required]
        public string AICUserId { get; set; } = string.Empty; // FK to the user who posted it

        [ForeignKey("AICUserId")]
        public AICUser AICUser { get; set; } = new AICUser(); // Navigation to user who posted it
    }
}
