using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using WindowsSystem_Backend.BL.DTO;
using WindowsSystem_Backend.DAL;
using WindowsSystem_Backend.DO;

namespace WindowsSystem_Backend.BL
{
    public class BlLibrary
    {
        
        public GetLibraryDto GetLibraryDTOs(DO.Library library, IEnumerable<Movie> movies, IEnumerable<TvSeries> tvSeries)
        {
            var blMovie = new BlMovie();
            var blTvSeries = new BlTvSeries();
            var movieMedia = from movie in movies
                                select blMovie.GetMediaFromMovie(movie);

            var seriesMedia = from series in tvSeries
                              select blTvSeries.GetMediaFromTvSeries(series);

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
