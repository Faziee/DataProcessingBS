using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class GenresModule
{
    public static void AddGenresEndpoints(this IEndpointRouteBuilder app)
    {
        // Create Genre
        app.MapPost("/genre",
            async ([FromBody] CreateGenreRequest createGenreRequest, [FromServices] AppDbcontext dbContext) =>
            {
                var genre = new Genre()
                {
                    Type = createGenreRequest.Type
                };

                await dbContext.Genres.AddAsync(genre);
                await dbContext.SaveChangesAsync();
                return Results.Ok(genre);
            });
        
        // Update
        app.MapPut("/genres/{genreId}", async (int genreId, [FromBody] UpdateGenreRequest updateGenreRequest, [FromServices] AppDbcontext dbContext) =>
        {
            var genre = await dbContext.Genres.FirstOrDefaultAsync(x => x.Genre_Id == genreId);

            if (genre != null)
            {
                genre.Genre_Id = updateGenreRequest.Genre_Id;
                genre.Type = updateGenreRequest.Type;
                
                await dbContext.SaveChangesAsync();
                return Results.Ok(genre);
            }
            else
            {
                return Results.NotFound();
            }
        });

        // Get All Genres
        app.MapGet("/genres", async (AppDbcontext dbContext) =>
        {
            var genres = await dbContext.Genres.ToListAsync();
            return Results.Ok(genres);
        });
        
        // Delete Genre by ID
        app.MapDelete("/genres/{id:int}", async (int id, [FromServices] AppDbcontext dbContext) =>
        {
            var genre = await dbContext.Genres.FirstOrDefaultAsync(s => s.Genre_Id == id);

            if (genre == null)
                return Results.NotFound("Genre not found.");

            dbContext.Genres.Remove(genre);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new { Message = "Genre deleted successfully." });
        });
    }
}