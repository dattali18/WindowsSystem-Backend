﻿using WindowsSystem_Backend.BL.DTO;
using WindowsSystem_Backend.DO;
using WindowsSystem_Backend.Services;

namespace WindowsSystem_Backend.BL
{
    public class BlTvSeries
    {
        public MediaDto GetMediaFromTvSeries(TvSeries series)
        {
            // TODO: convert the Year acording to the different possible case
            return new MediaDto
            {
                Title = series.Title,
                ImdbID = series.ImdbID,
                Poster = series.PosterURL,
                Year = series.Years,
                Type = "series",
            };
        }

        public GetTvSeriesDto GetTvSeriesDtoFromTvSeries(TvSeries series) 
        {
            return new GetTvSeriesDto
            {
                Id = series.Id,
                Title = series.Title,
                Genre = series.Genre,
                PosterURL = series.PosterURL,
                Rating = series.Rating,
                Years = series.Years,
                TotalSeasons = series.TotalSeasons,
                ImdbID = series.ImdbID,
                Time = series.Time
            };
        }

        public async Task<TvSeries?> GetTvSeriesByImdbID(string imdbID) 
        {
            var seriesDetails = await OmdbApiService.GetSeriesByIDAsync(imdbID);

            var jsonConvertor = new BL.BlJsonConversion();
            return jsonConvertor.GetTvSeriesFromJson(seriesDetails);
        }

        public async Task<IEnumerable<MediaDto>?> GetTvSeriesBySearch(string s, int? y = null)
        {
            var str = await OmdbApiService.GetSeriesBySearchAsync(s, y);

            var jsonConvertor = new BL.BlJsonConversion();
            return jsonConvertor.GetTvSeriesObjFromJson(str);
        }
    }
}
