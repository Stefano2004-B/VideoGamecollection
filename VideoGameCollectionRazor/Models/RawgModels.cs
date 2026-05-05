namespace VideoGameCollection.Models
{
    // Modelli per RAWG API (https://rawg.io/apidocs)
    public class RawgSearchResult
    {
        public int Count { get; set; }
        public List<RawgGame> Results { get; set; } = new();
    }

    public class RawgGame
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Background_Image { get; set; }
        public double? Rating { get; set; }
        public List<RawgPlatformWrapper>? Platforms { get; set; }
        public List<RawgGenre>? Genres { get; set; }
        public string? Released { get; set; }
    }

    public class RawgPlatformWrapper
    {
        public RawgPlatform? Platform { get; set; }
    }

    public class RawgPlatform
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class RawgGenre
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
