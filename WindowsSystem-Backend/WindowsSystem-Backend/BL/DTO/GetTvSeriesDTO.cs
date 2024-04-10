namespace WindowsSystem_Backend.BL.DTO
{
    public class GetTvSeriesDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Genre { get; set; }

        public string? PosterURL { get; set; }

        public double Rating { get; set; }

        public int? StartingYear { get; set; }

        public int? EndingYear { get; set; }

        public int? TotalSeasons { get; set; }

        public string? ImdbID { get; set; }

        public int? Time { get; set; }
    }
}
