namespace WindowsSystem_Backend.Models
{
    public class OMDbApiService
    {
        private static readonly string _apiKey = "130ade15";
        private static readonly string _baseUrl = "http://www.omdbapi.com/";

        public static async Task<string> GetMovieAsync(string title)
        {
            using (var httpClient = new HttpClient())
            {
                var query = $"?apikey={_apiKey}&t={title}";
                var url = _baseUrl + query;

                Console.WriteLine(url);

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var movieData = await response.Content.ReadAsStringAsync();

                return movieData;
            }
        }

        public static async Task<string> GetMoviesBySearchAsync(string s, int? y = null)
        {
            using (var httpClient = new HttpClient())
            {
                var query = $"?apikey={_apiKey}&s=\"{s}\"";
                
                if (y != null)
                {
                    query = query + $"&y={y}";
                }

                var url = _baseUrl + query;

                var response = await httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var movieData = await response.Content.ReadAsStringAsync();

                return movieData;
            }
        }
    }
}
