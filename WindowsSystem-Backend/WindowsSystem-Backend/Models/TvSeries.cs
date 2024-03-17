namespace WindowsSystem_Backend.Models
{
    public class TvSeries
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Genre { get; set; }

        public string? PosterURL { get; set; }

        public double Rating { get; set; }

        public int? StartingYear { get; set; }

        public int? EndingYear { get; set;}

        public int totalSeasons { get; set; }
    }
}
