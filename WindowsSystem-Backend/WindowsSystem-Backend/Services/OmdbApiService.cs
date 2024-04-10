namespace WindowsSystem_Backend.Services
{

    public class OmdbApiService
    {
        private static readonly string _apiKey = "130ade15";
        private static readonly string _baseUrl = "http://www.omdbapi.com/";

        public static async Task<string> GetMovieAsync(string title)
        {
            using (var httpClient = new HttpClient())
            {
                var query = $"?apikey={_apiKey}&t={title}&type=movie";
                var url = _baseUrl + query;

                Console.WriteLine(url);

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var movieData = await response.Content.ReadAsStringAsync();

                return movieData;
            }
        }

        public static async Task<string> GetMovieByIDAsync(string id)
        {
            using (var httpClient = new HttpClient())
            {
                var query = $"?apikey={_apiKey}&i={id}&type=movie";
                var url = _baseUrl + query;

                Console.WriteLine(url);

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var movieData = await response.Content.ReadAsStringAsync();

                return movieData;
            }
        }

        public static async Task<string> GetMoviesBySearchAsync(string s, int? y = null, int page = 0)
        {
            using (var httpClient = new HttpClient())
            {
                var query = $"?apikey={_apiKey}&type=movie&s=\"{s}\"&page={page}";
                
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

        public static async Task<string> GetSeriesAsync(string title)
        {
            using (var httpClient = new HttpClient())
            {
                var query = $"?apikey={_apiKey}&t={title}&type=series";
                var url = _baseUrl + query;

                Console.WriteLine(url);

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var movieData = await response.Content.ReadAsStringAsync();

                return movieData;
            }
        }

        public static async Task<string> GetSeriesByIDAsync(string id)
        {
            using (var httpClient = new HttpClient())
            {
                var query = $"?apikey={_apiKey}&i={id}&type=series";
                var url = _baseUrl + query;

                Console.WriteLine(url);

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var movieData = await response.Content.ReadAsStringAsync();

                return movieData;
            }
        }

        public static async Task<string> GetSeriesBySearchAsync(string s, int? y = null, int page = 0)
        {
            using (var httpClient = new HttpClient())
            {
                var query = $"?apikey={_apiKey}&type=series&s=\"{s}\"&page={page}";

                if (y != null)
                {
                    query += $"&y={y}";
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
