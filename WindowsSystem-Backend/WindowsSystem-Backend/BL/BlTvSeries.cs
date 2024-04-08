using WindowsSystem_Backend.BL.DTO;
using WindowsSystem_Backend.DO;
using WindowsSystem_Backend.Services;

namespace WindowsSystem_Backend.BL
{
    public class BlTvSeries
    {
        private readonly Bl bl = Factory.GetBL();
        public MediaDto getMediaFromTvSeries(TvSeries series)
        {
            return new MediaDto
            {
                Title = series.Title,
                ImdbID = series.ImdbID,
                Poster = series.PosterURL,
                Year = $"{series.StartingYear}-{series.EndingYear}"
            };
        }

        public async Task<TvSeries?> GetTvSeriesByImdbID(string imdbID) {
            var seriesDetails = await OmdbApiService.GetSeriesByIDAsync(imdbID);
            return bl.BlJsonConversion.GetTvSeriesFromJson(seriesDetails);
        }
    }
}
