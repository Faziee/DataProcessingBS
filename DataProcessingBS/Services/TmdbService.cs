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
           _tmdbApiKey = configuration["TMDB_API_KEY"];
            _httpClient = httpClient;
        }
        public async Task<List<Movie>> GetMoviesAsync(string category)
        {
            // Construct the TMDB API URL using the category (e.g., 'now_playing', 'popular', etc.)
            var url = $"https://api.themoviedb.org/3/movie/{category}?api_key={_tmdbApiKey}&language=en-US&page=1";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(content);
                var movies = jsonResponse["results"];
                var movieList = new List<Movie>();
                foreach (var movie in movies)
                {
                    var title = movie["title"]?.ToString();
                    var originalTitle = movie["original_title"]?.ToString();
                    var backdropPath = movie["backdrop_path"]?.ToString();
                    var overview = movie["overview"]?.ToString();
                    var posterUrl = backdropPath != null ? $"https://image.tmdb.org/t/p/w500{backdropPath}" : "";
                    movieList.Add(new Movie
                    {
                        Title = title,
                        OriginalTitle = originalTitle,
                        BackdropPath = backdropPath,
                        Overview = overview,
                        PosterUrl = posterUrl
                    });
                }
                return movieList;
            }
            return new List<Movie>();  // Return an empty list if TMDB API call fails
        }
    }
    // Movie model to hold the necessary data
    public class Movie
    {
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string BackdropPath { get; set; }
        public string Overview { get; set; }
        public string PosterUrl { get; set; }
    }
}