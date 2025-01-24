using System.Data;
using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class MoviesStoredProcedureModule
{
    public static void AddMoviesStoredProcedureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/stored-procedure-create-movie", async ([FromBody] CreateMovieRequest createMovieRequest,
            [FromServices] AppDbcontext dbContext) =>
        {
            var mediaIdParameter = new SqlParameter
            {
                ParameterName = "@MediaId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                EXEC CreateNewMedia 
                @GenreId={createMovieRequest.Genre_Id}, 
                @Title={createMovieRequest.Title}, 
                @AgeRating={createMovieRequest.Age_Rating}, 
                @Quality={createMovieRequest.Quality}, 
                @MediaId={mediaIdParameter} OUTPUT");

            var mediaId = (int)mediaIdParameter.Value;

            await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                EXEC CreateMovie 
                @MediaId={mediaId}, 
                @HasSubtitles={createMovieRequest.Has_Subtitles}");

            return Results.Ok(new { Message = "Movie created successfully" });
        });

        app.MapGet("/stored-procedure-get-movies", async (AppDbcontext dbContext) =>
        {
            var movies = await dbContext.Set<MovieDto>()
                .FromSqlRaw("EXEC GetAllMovies")
                .ToListAsync();

            return Results.Ok(movies);
        });


        app.MapGet("/stored-procedure-get-movie-by-id/{id:int}", async (AppDbcontext dbContext, int id) =>
        {
            var movies = await dbContext.Set<MovieDto>()
                .FromSqlRaw("EXEC GetMovieById @MovieId = {0}", id)
                .ToListAsync();

            var movie = movies.FirstOrDefault();

            if (movie == null) return Results.NotFound(new { Message = $"Movie with ID {id} not found." });

            return Results.Ok(movie);
        });

        app.MapGet("/stored-procedure-get-movies-by-genre/{genre}", async (string genre, AppDbcontext dbContext) =>
        {
            if (string.IsNullOrWhiteSpace(genre)) return Results.BadRequest(new { Message = "Genre cannot be empty." });

            var movies = await dbContext.Set<MovieDto>()
                .FromSqlInterpolated($"EXEC GetMoviesByGenre @GenreType = {genre}")
                .ToListAsync();

            if (!movies.Any()) return Results.NotFound(new { Message = $"No movies found for genre '{genre}'." });

            return Results.Ok(movies);
        });

        app.MapPut("/stored-procedure-update-movie-by-id",
            async ([FromBody] UpdateMovieRequest updateMovieRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC UpdateMovieById @MovieId={updateMovieRequest.Movie_Id}, @HasSubtitles={updateMovieRequest.Has_Subtitles}");
                return Results.Ok();
            });

        app.MapDelete("/stored-procedure-delete-movie/{movieId}",
            async (int movieId, [FromServices] AppDbcontext dbContext) =>
            {
                // Call the stored procedure to handle the deletion
                var result = await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC DeleteMovieById @MovieId={movieId}");

                if (result == 0) return Results.NotFound("Movie not found.");

                return Results.Ok();
            });
    }
}