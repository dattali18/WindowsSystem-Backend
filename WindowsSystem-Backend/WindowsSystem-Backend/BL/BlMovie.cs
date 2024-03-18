using WindowsSystem_Backend.BL.BO;
using WindowsSystem_Backend.DO;

namespace WindowsSystem_Backend.BL
{
    public class BlMovie
    {
        public static Media getMediaFromMovie(Movie movie)
        {
            return new Media { 
                Title = movie.Title,
                ImdbID = movie.ImdbID,
                Poster = movie.PosterURL,
                Year = $"{movie.Year}"
            };
        }
    }
}
