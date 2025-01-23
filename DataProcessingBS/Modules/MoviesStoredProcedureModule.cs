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

        app.MapPut("/stored-procedure-update-movie-by-id", async ([FromBody] UpdateMovieRequest updateMovieRequest, [FromServices] AppDbcontext dbContext) =>
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC UpdateMovieById @MovieId={updateMovieRequest.Movie_Id}, @HasSubtitles={updateMovieRequest.Has_Subtitles}");
            return Results.Ok();
        });


        /*// Get Episodes using Stored Procedure
        app.MapGet("/stored-procedure-get-moviess", async (AppDbcontext dbContext) =>
        {
            var movies = await dbContext.Movies.FromSqlRaw("EXEC GetAllMovies").ToListAsync();
            return Results.Ok(movies);
        });*/
        
        /*app.MapGet("/stored-procedure-get-movies", async (AppDbcontext dbContext, ILogger<Program> logger) =>
        {
            try
            {
                // Fetch the movies using the stored procedure
                var moviesFromProcedure = await dbContext
                    .Movies
                    .FromSqlRaw("EXEC GetAllMovies")  // Your stored procedure call
                    .Select(m => new MovieFromProcedureDto
                    {
                        Movie_Id = m.Movie_Id,
                        Media_Id = m.Media_Id,  // Map the column to your new DTO property
                        Has_Subtitles = m.Has_Subtitles,
                        Title = m.Media.Title  // Assuming you have the proper navigation
                    })
                    .ToListAsync();

                return Results.Ok(moviesFromProcedure);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while processing the request");
                return Results.Problem("An error occurred while processing your request. Please check server logs for more details.");
            }
        });*/
        

        app.MapGet("/stored-procedure-get-movie-by-id/{movieId:int}", async (int movieId, [FromServices] AppDbcontext dbContext) =>
        {
            var movie = await dbContext.Movies
                .FromSqlInterpolated($"EXEC GetMovieById @MovieId={movieId}")
                .ToListAsync()
                .ContinueWith(task => task.Result.Select(m => new MovieDto
                {
                    Movie_Id = m.Movie_Id,
                    Media_Id = m.Media_Id,
                    Has_Subtitles = m.Has_Subtitles
                }).FirstOrDefault());

            return movie == null
                ? Results.NotFound()
                : Results.Ok(movie);
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