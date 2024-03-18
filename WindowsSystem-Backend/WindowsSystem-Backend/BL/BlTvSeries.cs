using WindowsSystem_Backend.BL.BO;
using WindowsSystem_Backend.DO;

namespace WindowsSystem_Backend.BL
{
    public class BlTvSeries
    {
        public static Media getMediaFromMovie(TvSeries series)
        {
            return new Media
            {
                Title = series.Title,
                ImdbID = series.ImdbID,
                Poster = series.PosterURL,
                Year = $"{series.StartingYear} - {series.EndingYear}"
            };
        }
    }
}
