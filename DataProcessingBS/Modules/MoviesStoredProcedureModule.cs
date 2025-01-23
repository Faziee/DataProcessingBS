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
            // Step 1: Create the Media using the CreateNewMedia stored procedure
            var mediaIdParameter = new SqlParameter
            {
                ParameterName = "@MediaId",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                EXEC CreateNewMedia 
                @GenreId={createMovieRequest.Genre_Id}, 
                @Title={createMovieRequest.Title}, 
                @AgeRating={createMovieRequest.Age_Rating}, 
                @Quality={createMovieRequest.Quality}, 
                @MediaId={mediaIdParameter} OUTPUT");

            int mediaId = (int)mediaIdParameter.Value;

            // Step 2: Create the Movie using the stored procedure
            await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                EXEC CreateMovie 
                @MediaId={mediaId}, 
                @HasSubtitles={createMovieRequest.Has_Subtitles}");

            return Results.Ok(new { Message = "Movie created successfully" });
        });

        app.MapPut("/stored-procedure-update-movie-by-id",
            async ([FromBody] UpdateMovieRequest updateMovieRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC UpdateMovieById @MovieId={updateMovieRequest.Movie_Id}, @HasSubtitles={updateMovieRequest.Has_Subtitles}");
                return Results.Ok();
            });
        
        
        app.MapGet("/stored-procedure-get-movies", async (AppDbcontext dbContext) =>
        {
            var movies = await dbContext.Set<MovieWithMediaTitleDto>()
                .FromSqlRaw("EXEC GetAllMovies")
                .ToListAsync();

            return Results.Ok(movies);
        });

        

        app.MapGet("/stored-procedure-get-movie-by-id/{id:int}", async (AppDbcontext dbContext, int id) =>
        {
            var movies = await dbContext.Set<MovieWithMediaTitleDto>()
                .FromSqlRaw("EXEC GetMovieById @MovieId = {0}", id)
                .ToListAsync();

            var movie = movies.FirstOrDefault();

            if (movie == null)
            {
                return Results.NotFound(new { Message = $"Movie with ID {id} not found." });
            }

            return Results.Ok(movie);
        });



        app.MapDelete("/stored-procedure-delete-movie/{movieId}", async (int movieId, [FromServices] AppDbcontext dbContext) =>
        {
            // Get the Media_Id associated with the movie
            var mediaId = await dbContext.Movies
                .Where(m => m.Movie_Id == movieId)
                .Select(m => m.Media_Id)
                .FirstOrDefaultAsync();

            if (mediaId == 0)
            {
                return Results.NotFound("Movie not found.");
            }

            // Delete the movie using the stored procedure
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteMovieById @MovieId={movieId}");

            // Delete the associated media
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteMediaById @MediaId={mediaId}");

            return Results.Ok();
        });
    }
}