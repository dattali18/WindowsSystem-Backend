﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using WindowsSystem_Backend.BL.DTO;
using WindowsSystem_Backend.DAL;
using WindowsSystem_Backend.DO;

namespace WindowsSystem_Backend.BL
{
    public class BlLibrary
    {
        private readonly Bl bl = Factory.GetBL();
        
        public GetLibraryDto getLibraryDTOs(DO.Library library, List<Movie> movies, List<TvSeries> tvSeries)
        {
            var movieMedia = from movie in movies
                                select bl.BlMovie.getMediaFromMovie(movie);

            var seriesMedia = from series in tvSeries
                              select bl.BlTvSeries.getMediaFromTvSeries(series);

            List<MediaDto> media = movieMedia.Concat(seriesMedia).ToList();

            return new GetLibraryDto { 
                Id = library.Id,
                Name = library.Name,
                Keywords = library.Keywords,
                Media = media
            };
        }
        
    }
}
