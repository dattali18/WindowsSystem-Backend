namespace WindowsSystem_Backend.DO
{
    public class Library
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public ICollection<Movie>? Movies { get; set; }

        public ICollection<TvSeries>? TvSeries { get; set; }

        public string? keywords { get; set; }
    }
}
