using WindowsSystem_Backend.BL.DTO;
using WindowsSystem_Backend.DO;
using WindowsSystem_Backend.Services;

namespace WindowsSystem_Backend.BL
{
    public class BlTvSeries
    {
        public MediaDto GetMediaFromTvSeries(TvSeries series)
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

            var jsonConvertor = new BL.BlJsonConversion();
            return jsonConvertor.GetTvSeriesFromJson(seriesDetails);
        }
    }
}
