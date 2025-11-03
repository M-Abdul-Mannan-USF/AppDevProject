using System;
using System.Collections.Generic;               // ✅ needed for List<> and ICollection<>
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsBiasChecker.Models                  // ✅ match what views expect
{
    public class Roundup
    {
        public int Id { get; set; }

        [Required, StringLength(250)]
        public string Title { get; set; } = "";

        public string? Topics { get; set; }

        public DateTime Date { get; set; }

        [Column(TypeName = "nvarchar(max)")]   // ✅ was StringLength(1024) or 2048
        public string? StoryUrl { get; set; }

        // Allow long descriptions
        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }

        public ICollection<Story> Stories { get; set; } = new List<Story>();
    }
}
