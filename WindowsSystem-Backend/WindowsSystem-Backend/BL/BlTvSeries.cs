using WindowsSystem_Backend.BL.DTO;
using WindowsSystem_Backend.DO;

namespace WindowsSystem_Backend.BL
{
    public class BlTvSeries
    {
        public static MediaDto getMediaFromTvSeries(TvSeries series)
        {
            return new MediaDto
            {
                Title = series.Title,
                ImdbID = series.ImdbID,
                Poster = series.PosterURL,
                Year = $"{series.StartingYear}-{series.EndingYear}"
            };
        }
    }
}
