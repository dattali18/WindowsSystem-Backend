using Newtonsoft.Json;

namespace WindowsSystem_Backend.BL
{
    public class BlMovie
    {
        public static IEnumerable<BO.Movie>? GetMoviesFronJson(string json)
        {
            return JsonMovieDeserialization.GetSearchResults(json);
        }
    }

    public class SearchResponse
    {
        public List<BO.Movie>? Search { get; set; }
        public string? TotalResult { get; set; }
        public bool Response { get; set; }
    }

    public class JsonMovieDeserialization
    {
        public static IEnumerable<BO.Movie>? GetSearchResults(string json)
        {
            var searchResponse = JsonConvert.DeserializeObject<SearchResponse>(json);

            return searchResponse?.Search;
        }
    }
}
