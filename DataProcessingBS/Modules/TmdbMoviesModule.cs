using DataProcessingBS.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataProcessingBS.Modules;

public static class TmdbMoviesModule
{
    public static void AddExternalMovieEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/movies", async (string category, [FromServices] TmdbService tmdbService) =>
        {
            var movieDetails = await tmdbService.GetMoviesAsync(category);
            return Results.Ok(movieDetails);
        });
    }
}