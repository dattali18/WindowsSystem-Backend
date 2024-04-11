using Newtonsoft.Json;
using WindowsSystem_Backend.BL.DTO;

namespace WindowsSystem_Backend.BL
{
    public class BlJsonConversion
    {
        public  IEnumerable<MediaDto>? GetMovieObjFromJson(string json)
        {
            var jsonMovieDeserialization = new JsonMediaDeserialization();
            return jsonMovieDeserialization.GetSearchResults(json);
        }

        public IEnumerable<MediaDto>? GetTvSeriesObjFromJson(string json) 
        {
            var jsonTvSeriesDeserialization = new JsonMediaDeserialization();
            return jsonTvSeriesDeserialization.GetSearchResults(json);
        }

        public  DO.Movie? GetMovieFromJson(string json)
        {
            var jsonMovieDeserialization = new JsonMediaDeserialization();
            return jsonMovieDeserialization.GetMovieResult(json);
        }

        public  DO.TvSeries? GetTvSeriesFromJson(string json)
        {
            var jsonSeriesDeserialization = new JsonMediaDeserialization();
            return jsonSeriesDeserialization.GetSeriesResult(json);
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
        public string? Year { get; set; }
        public string? Genre { get; set; }
        public string? ImdbID { get; set; }
        public string? ImdbRating { get; set; }
        public string? Poster { get; set;}
        public string? Runtime { get; set; }
        public string? TotalSeasons { get; set; }
    }

    internal class JsonMediaDeserialization
    {
        public IEnumerable<MediaDto>? GetSearchResults(string json)
        {
            var searchResponse = JsonConvert.DeserializeObject<SearchResponse>(json);

            return (
                from response in searchResponse?.Search
                select new MediaDto { 
                    Title = response.Title,
                    Year = response.Year,
                    ImdbID = response.ImdbID,
                    Type = response.Type,
                    Poster = response.Poster
                }
                ).ToList();
        }

        public DO.Movie? GetMovieResult(string json)
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
                Rating = rating,
                Time = time
            };
        }

        public DO.TvSeries? GetSeriesResult(string json)
        {
            var seriesResult = JsonConvert.DeserializeObject<SeriesResult>(json);

            if (seriesResult == null || seriesResult.Title == null)
            {
                return null;
            }

            // parsing the year for movie
            // Split the string at the hyphen and store results in an array
            string years = seriesResult.Year ?? "";
            string[] numbers = years.Split('-');


            int startYear;
            int endYear = 0;
            if (!int.TryParse(numbers[0], out startYear))
            {
                startYear = 0;
            }

            if(numbers.Length > 1)
            {
                if (!int.TryParse(numbers[1], out endYear)) {
                    endYear = 0;
                }
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

            int totalSeasons;
            if (!int.TryParse(seriesResult.TotalSeasons, out totalSeasons)) 
            { 
                totalSeasons = 0;
            }

            return new DO.TvSeries
            {
                Title = seriesResult.Title,
                Genre = seriesResult.Genre,
                PosterURL = seriesResult.Poster,
                StartingYear = startYear,
                EndingYear = endYear,
                TotalSeasons = totalSeasons,
                ImdbID = seriesResult.ImdbID,
                Rating = rating,
                Time = time
            };
        }
    }
}
