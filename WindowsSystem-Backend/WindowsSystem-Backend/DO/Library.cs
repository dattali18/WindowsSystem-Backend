namespace WindowsSystem_Backend.DO
{
    public class Library
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Keywords { get; set; }
        public List<Movie> Movies { get; set; } = new List<Movie>();
        public List<TvSeries> TvSeries { get; set; } = new List<TvSeries>();
    }
}
