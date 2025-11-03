namespace NewsBiasChecker.Models.ViewModels
{
    public class RoundupEditViewModel
    {
        public int? Id { get; set; }               // null on Create
        public string Title { get; set; } = "";
        public DateTime Date { get; set; }
        public string? Topics { get; set; }
        public string? StoryUrl { get; set; }
        public string? Description { get; set; }

        public List<StoryInput> Stories { get; set; } = new()
        {
            new StoryInput { Side = "Left" },
            new StoryInput { Side = "Center" },
            new StoryInput { Side = "Right" }
        };
    }

    public class StoryInput
    {
        public int? Id { get; set; }               // filled on Edit
        public string Side { get; set; } = "";     // Left / Center / Right
        public string? Title { get; set; } = "";
        public string? Url { get; set; }
        public string? Text { get; set; }
        public string? Outlet { get; set; }
    }
}
