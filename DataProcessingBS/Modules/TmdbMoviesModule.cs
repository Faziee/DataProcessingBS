using Microsoft.AspNetCore.Mvc;
using DataProcessingBS.Services;
namespace DataProcessingBS.Modules
{
    public static class TmdbMoviesModule
    {
        public static void AddExternalMovieEndpoints(this IEndpointRouteBuilder app)
        {
            // Endpoint to fetch movies, which the frontend will call
            app.MapGet("/movies", async (string category, [FromServices] TmdbService tmdbService) =>
            {
                var movieDetails = await tmdbService.GetMoviesAsync(category);
                return Results.Ok(movieDetails);  // Return movie details to frontend
            });
        }
    }
}