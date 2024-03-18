using Newtonsoft.Json;
using WindowsSystem_Backend.BL.DTO;

namespace WindowsSystem_Backend.BL
{
    public class BlJsonConversion
    {
        public static IEnumerable<MediaDto>? GetMovieObjFromJson(string json)
        {
            return JsonMovieDeserialization.GetSearchResults(json);
        }

        public static DO.Movie? GetMovieFromJson(string json)
        {
            return JsonMovieDeserialization.GetMovieResult(json);
        }

        public static DO.TvSeries? GetTvSeriesFromJson(string json)
        {
            return JsonMovieDeserialization.GetSeriesResult(json);
        }
    }

    internal class SearchResponse
    {
        public List<MediaDto>? Search { get; set; }
        public string? TotalResult { get; set; }
        public bool Response { get; set; }
    }

    internal class MovieResult
    {
        public string? Title { get; set; }
        public string? Year { get; set; }
        public string? Genre { get; set; }
        public string? ImdbID { get; set; }
        public string? ImdbRating { get; set; }
        public string? Poster { get; set; }
        public string? Runtime { get; set; }
    }

    internal class SeriesResult
    {
        public string? Title { get; set; }
        public string? Years { get; set; }
        public string? Genre { get; set; }
        public string? ImdbID { get; set; }
        public string? ImdbRating { get; set; }
        public string? Poster { get; set;}
        public string? Runtime { get; set; }
        public string? TotalSeasons { get; set; }
    }

    internal class JsonMovieDeserialization
    {
        public static IEnumerable<MediaDto>? GetSearchResults(string json)
        {
            var searchResponse = JsonConvert.DeserializeObject<SearchResponse>(json);

            return searchResponse?.Search;
        }

        public static DO.Movie? GetMovieResult(string json)
        {
            var movieResult = JsonConvert.DeserializeObject<MovieResult>(json);

            if (movieResult == null || movieResult.Title == null)
            {
                return null;
            }

            // parsing the year for movie
            int year;
            if(!int.TryParse(movieResult.Year, out year))
            {
                year = 1900;
            } 
  

            // parsing the rating for movie
            double rating;
            if(!double.TryParse(movieResult.ImdbRating, out rating))
            {
                rating = 0;
            }

            int time;

            if(!int.TryParse(movieResult.Runtime?.Split()[0], out time))
            {
                time = 0;
            }

            return new DO.Movie
            { 
                Title = movieResult.Title,
                Genre = movieResult.Genre,
                PosterURL = movieResult.Poster,
                Year = year,
                ImdbID = movieResult.ImdbID,
                Rating = rating, Time = time
            };
        }

        public static DO.TvSeries? GetSeriesResult(string json)
        {
            var seriesResult = JsonConvert.DeserializeObject<SeriesResult>(json);

            if (seriesResult == null || seriesResult.Title == null)
            {
                return null;
            }

            // parsing the year for movie
            // Split the string at the hyphen and store results in an array
            string years = seriesResult.Years ?? "";
            string[] numbers = years.Split('-');

            int? startYear = int.Parse(numbers[0]);
            int? endYear = null;


            if(numbers.Length > 1)
            {
                endYear = int.Parse(numbers[1]);
            }


            // parsing the rating for movie
            double rating;
            if (!double.TryParse(seriesResult.ImdbRating, out rating))
            {
                rating = 0;
            }

            int time;

            if (!int.TryParse(seriesResult.Runtime?.Split()[0], out time))
            {
                time = 0;
            }

            return new DO.TvSeries
            {
                Title = seriesResult.Title,
                Genre = seriesResult.Genre,
                PosterURL = seriesResult.Poster,
                StartingYear = startYear,
                EndingYear = endYear,
                ImdbID = seriesResult.ImdbID,
                Rating = rating,
                Time = time
            };
        }
    }
}
