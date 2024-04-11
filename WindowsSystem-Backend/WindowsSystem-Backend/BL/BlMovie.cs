using WindowsSystem_Backend.BL.DTO;
using WindowsSystem_Backend.DO;
using WindowsSystem_Backend.Services;

namespace WindowsSystem_Backend.BL
{
    public class BlMovie
    {

        public MediaDto GetMediaFromMovie(Movie movie)
        {
            return new MediaDto { 
                Title = movie.Title,
                ImdbID = movie.ImdbID,
                Poster = movie.PosterURL,
                Year = $"{movie.Year}",
                Type = "movie",
            };
        }

        public GetMovieDto GetMovieDtoFromMovie(Movie movie)
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
        public async Task<Movie?> GetMovieByImdbID(string imdbID) {
            var movieDetails = await OmdbApiService.GetMovieByIDAsync(imdbID);

            var jsonConvertor = new BL.BlJsonConversion();
            return jsonConvertor.GetMovieFromJson(movieDetails);
        }

        public async Task<IEnumerable<MediaDto>?> GetMoviesBySearch(string s, int? y = null) {
            var str = await OmdbApiService.GetMoviesBySearchAsync(s, y);

            var jsonConvertor = new BL.BlJsonConversion();
            return jsonConvertor.GetMovieObjFromJson(str);
        }
    }
}
