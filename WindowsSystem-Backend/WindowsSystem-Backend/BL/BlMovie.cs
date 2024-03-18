using WindowsSystem_Backend.BL.DTO;
using WindowsSystem_Backend.DO;

namespace WindowsSystem_Backend.BL
{
    public class BlMovie
    {
        public static MediaDto getMediaFromMovie(Movie movie)
        {
            return new MediaDto { 
                Title = movie.Title,
                ImdbID = movie.ImdbID,
                Poster = movie.PosterURL,
                Year = $"{movie.Year}"
            };
        }
    }
}
