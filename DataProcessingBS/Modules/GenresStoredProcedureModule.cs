using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class GenresStoredProcedureModule
{
    public static void AddGenresStoredProcedureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/stored-procedure-create-genre",
            async ([FromBody] CreateGenreRequest createGenreRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC CreateGenre @Type={createGenreRequest.Type}");
                return Results.Ok();
            });

        app.MapPut("/stored-procedure-update-genre-by-id",
            async ([FromBody] UpdateGenreRequest updateGenreRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC UpdateGenreById @GenreId={updateGenreRequest.Genre_Id}, @Type={updateGenreRequest.Type}");
                return Results.Ok();
            });

        app.MapGet("/stored-procedure-get-genres", async (AppDbcontext dbContext) =>
        {
            var genres = await dbContext.Genres.FromSqlRaw("EXEC GetAllGenres").ToListAsync();
            return Results.Ok(genres);
        });
        
        app.MapGet("/stored-procedure-get-genre-by-id/{genreId:int}", async (int genreId, [FromServices] AppDbcontext dbContext) =>
        {
            var genre = await dbContext.Genres
                .FromSqlInterpolated($"EXEC GetGenreById @GenreId={genreId}")
                .ToListAsync()
                .ContinueWith(task => task.Result.Select(g => new GenreDto()
                {
               
                    Genre_Id = g.Genre_Id,
                    Type = g.Type
                }).FirstOrDefault());

            return genre == null
                ? Results.NotFound()
                : Results.Ok(genre);
        });
        
        // Delete Genre by ID
        app.MapDelete("/stored-procedure-delete-genre-by-id/{genreId}", async (int genreId,[FromServices] AppDbcontext dbContext) =>
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteGenreById @GenreId={genreId}");
            return Results.Ok();
        });
    }
}