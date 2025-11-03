using System.Text.Json.Serialization;

namespace NewsBiasChecker.Models.ImportDtos
{
    public class AllSidesDto
    {
        // ← maps the spaced key: "Title of Headline Roundup"
        [JsonPropertyName("Title of Headline Roundup")]
        public string? Title_of_Headline_Roundup { get; set; }

        public string? description { get; set; }
        public string? Topics { get; set; }
        public string? Date { get; set; }
        public string? url_story { get; set; }

        public string? left_story_title { get; set; }
        public string? left_story_url { get; set; }
        public string? left_story_text { get; set; }

        public string? center_story_title { get; set; }
        public string? center_story_url { get; set; }
        public string? center_story_text { get; set; }

        public string? right_story_title { get; set; }
        public string? right_story_url { get; set; }
        public string? right_story_text { get; set; }
    }
}

