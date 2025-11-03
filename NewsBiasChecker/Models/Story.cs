using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsBiasChecker.Models
{
    public class Story
    {
        public int Id { get; set; }

        // FK → Roundup
        [Required]
        public int RoundupId { get; set; }
        public Roundup? Roundup { get; set; }

        // Left | Center | Right
        [Required, StringLength(10)]
        public string Side { get; set; } = "Center";

        [Required, StringLength(250)]
        public string Title { get; set; } = "";

        [Column(TypeName = "nvarchar(max)")]   // ✅ was StringLength(2048)
        public string? Url { get; set; }

        public string? Text { get; set; }

        // e.g., nytimes.com, foxnews.com
        [StringLength(120)]
        public string? Outlet { get; set; }
    }
}

