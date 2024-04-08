using WindowsSystem_Backend.BL.DTO;
using WindowsSystem_Backend.DO;
using WindowsSystem_Backend.Services;

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
        public static async Task<Movie?> GetMovieByImdbID(string imdbID) {
            var movieDetails = await OmdbApiService.GetMovieByIDAsync(imdbID);
            return BlJsonConversion.GetMovieFromJson(movieDetails);
        }

        public static async Task<IEnumerable<MediaDto>?> GetMoviesBySearch(string s, int? y = null) {
            var str = await OmdbApiService.GetMoviesBySearchAsync(s, y);
            return BlJsonConversion.GetMovieObjFromJson(str);
        }
    }
}
