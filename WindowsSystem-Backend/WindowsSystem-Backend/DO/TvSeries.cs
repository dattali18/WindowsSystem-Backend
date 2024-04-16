using Microsoft.EntityFrameworkCore;

namespace WindowsSystem_Backend.DO
{
    [Index(nameof(ImdbID), IsUnique = true)]
    public class TvSeries
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Genre { get; set; }

        public string? PosterURL { get; set; }

        public double Rating { get; set; }

        public string? Years { get; set; }
        
        public int? TotalSeasons { get; set; }

        public string? ImdbID { get; set; }

        public int? Time {  get; set; }

        public List<Library> Libraries { get; } = new List<Library>();
    }
}
