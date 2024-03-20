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
                Year = $"{movie.Year}",
                Type = "movie"
            };
        }

        public static GetMovieDto getMovieDtoFromMovie(Movie movie)
        {
            return new GetMovieDto
            {
                Title = movie.Title,
                ImdbID = movie.ImdbID,
                Time = movie.Time,
                Genre = movie.Genre,
                Year = movie.Year,
                Rating = movie.Rating,
                Id = movie.Id,
                PosterURL = movie.PosterURL
            };
        }
    }
}
