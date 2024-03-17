namespace WindowsSystem_Backend.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Genre { get; set; }

        public string? PosterURL { get; set; }

        public double Rating { get; set; }

        public int Year { get; set; }
    }
}
