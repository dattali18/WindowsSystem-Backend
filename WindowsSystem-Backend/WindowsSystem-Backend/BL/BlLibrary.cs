using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using WindowsSystem_Backend.BL.DTO;
using WindowsSystem_Backend.DAL;
using WindowsSystem_Backend.DO;

namespace WindowsSystem_Backend.BL
{
    public class BlLibrary
    {
        public static GetLibreryDto getLibreryDTOs(DO.Library library, List<Movie> movies, List<TvSeries> tvSeries)
        {
            var movieMedia = from movie in movies
                                select BlMovie.getMediaFromMovie(movie);

            var seriesMedia = from series in tvSeries
                              select BlTvSeries.getMediaFromMovie(series);

            List<MediaDto> media = movieMedia.Concat(seriesMedia).ToList();

            return new GetLibreryDto { 
                Name = library.Name,
                Keywords = library.Keywords,
                Media = media
            };
        }
        
    }
}
