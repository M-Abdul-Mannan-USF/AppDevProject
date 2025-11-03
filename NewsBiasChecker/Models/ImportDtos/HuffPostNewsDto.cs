namespace NewsBiasChecker.Models.ImportDtos
{
    public class HuffPostNewsDto
    {
        public string? link { get; set; }
        public string? headline { get; set; }
        public string? category { get; set; }
        public string? short_description { get; set; }
        public string? authors { get; set; }
        public string? date { get; set; }
    }
}
