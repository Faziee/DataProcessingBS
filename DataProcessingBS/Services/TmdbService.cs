using Newtonsoft.Json.Linq;
namespace DataProcessingBS.Services
{
    public class TmdbService
    {
        private readonly string _tmdbApiKey; 
        private readonly HttpClient _httpClient;

        // Constructor with HttpClient dependency injection
        public TmdbService(IConfiguration configuration, HttpClient httpClient)
        {
            _tmdbApiKey = configuration["TMDB_API_KEY"]; // Note the colon, not asterisk
            _httpClient = httpClient;
        }

        public async Task<List<Movie>> GetMoviesAsync(string category)
        {
            var url = $"https://api.themoviedb.org/3/movie/{category}?api_key={_tmdbApiKey}&language=en-US&page=1";
            
            try 
            {
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
    
                Console.WriteLine($"TMDB API Response: {content}");
    
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API call failed with status: {response.StatusCode}");
                    return new List<Movie>();
                }
    
                var jsonResponse = JObject.Parse(content);
                var movies = jsonResponse["results"] as JArray;
    
                if (movies == null || !movies.Any())
                {
                    Console.WriteLine("No movies found in the response");
                    return new List<Movie>();
                }
    
                return movies.Select(movie => new Movie
                {
                    Title = movie["title"]?.ToString(),
                    OriginalTitle = movie["original_title"]?.ToString(),
                    BackdropPath = movie["backdrop_path"]?.ToString(),
                    Overview = movie["overview"]?.ToString(),
                    PosterUrl = movie["backdrop_path"] != null 
                        ? $"https://image.tmdb.org/t/p/w500{movie["backdrop_path"]}"
                        : ""
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Movie>();
            }
        }
    }

    public class Movie
    {
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string BackdropPath { get; set; }
        public string Overview { get; set; }
        public string PosterUrl { get; set; }
    }
}