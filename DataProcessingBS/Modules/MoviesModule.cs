using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

public static class MoviesModule
{
    public static void AddMovieEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/movies", async ([FromBody] CreateMovieRequest createMovieRequest, [FromServices] AppDbcontext dbContext) =>
        {
            // Step 1: Use the CreateNewMedia stored procedure to create Media
            var mediaIdParameter = new SqlParameter()
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

            // Step 2: Create Movie
            var movie = new Movie
            {
                Media_Id = mediaId, 
                Has_Subtitles = createMovieRequest.Has_Subtitles
            };

            await dbContext.Movies.AddAsync(movie);
            await dbContext.SaveChangesAsync();

            return Results.Ok(movie);
        });

        app.MapGet("/stored-procedure-get-movie-by-id/{movieId:int}", async (int movieId, [FromServices] AppDbcontext dbContext) =>
        {
            var movie = await dbContext.Set<MovieWithMediaTitleDto>()
                .FromSqlInterpolated($"EXEC GetMovieById @MovieId={movieId}")
                .FirstOrDefaultAsync();

            return movie == null
                ? Results.NotFound()
                : Results.Ok(movie);
        });


        app.MapGet("/movies", async (AppDbcontext dbContext) =>
        {
            var movies = await dbContext.Movies.ToListAsync();
            return Results.Ok(movies);
        });

        app.MapGet("/movies/{movieId:int}", async (int movieId, [FromServices] AppDbcontext dbContext) =>
        {
            var movie = await dbContext.Movies
                .FirstOrDefaultAsync(x => x.Movie_Id == movieId);

            return movie == null
                ? Results.NotFound()
                : Results.Ok(movie);
        });

        app.MapDelete("/movies/{movieId}", async (int movieId, AppDbcontext dbContext) =>
        {
            var movie = await dbContext.Movies.FirstOrDefaultAsync(x => x.Movie_Id == movieId);

            if (movie != null)
            {
                // Delete associated media
                await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteMediaById @MediaId={movie.Media_Id}");

                dbContext.Movies.Remove(movie);
                await dbContext.SaveChangesAsync();
                return Results.Ok();
            }
            else
            {
                return Results.NotFound();
            }
        });
    }
}