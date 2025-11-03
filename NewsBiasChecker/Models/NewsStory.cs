using System;
using System.ComponentModel.DataAnnotations;

namespace NewsBiasChecker.Models
{
    public class NewsStory
    {
        public int Id { get; set; }

        [MaxLength(128)]
        public string Category { get; set; } = "";

        [MaxLength(512)]
        public string Headline { get; set; } = "";

        [MaxLength(4000)]
        public string? Summary { get; set; }

        [MaxLength(512)]
        public string? Authors { get; set; }

        [MaxLength(1024)]
        public string? Url { get; set; }

        public DateTime? PublishedOn { get; set; }
    }
}

