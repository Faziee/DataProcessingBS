using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class MediaStoredProcedureModule
{
    public static void AddMediaStoredProcedureEndpoints(this IEndpointRouteBuilder app)
    {
        // Media can not exist on it's own so it can not be created or deleted independently either

        app.MapPut("/stored-procedure-update-media-by-id",
            async ([FromBody] UpdateMediaRequest updateMediaRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC UpdateMediaById @MediaId={updateMediaRequest.Media_Id}, @GenreId={updateMediaRequest.Genre_Id}, @Title={updateMediaRequest.Title}, @AgeRating={updateMediaRequest.Age_Rating}, @Quality={updateMediaRequest.Quality}");
                return Results.Ok();
            });
        
        app.MapGet("/get-media", async (AppDbcontext dbContext) =>
        {
            var media = await dbContext.Media
                .Select(m => new MediaDto
                {
                    Media_Id = m.Media_Id,
                    Genre_Id = m.Genre_Id,
                    Title = m.Title,
                    Age_Rating = m.Age_Rating,
                    Quality = m.Quality
                })
                .ToListAsync();

            return Results.Ok(media);
        });

        // Get Media by ID
        app.MapGet("/get-media-by-id/{mediaId:int}", async (int mediaId, [FromServices] AppDbcontext dbContext) =>
        {
            var media = await dbContext.Media
                .Where(m => m.Media_Id == mediaId)
                .Select(m => new MediaDto
                {
                    Media_Id = m.Media_Id,
                    Genre_Id = m.Genre_Id,
                    Title = m.Title,
                    Age_Rating = m.Age_Rating,
                    Quality = m.Quality
                })
                .FirstOrDefaultAsync();

            if (media == null)
            {
                return Results.NotFound(new { Message = $"Media with ID {mediaId} not found." });
            }

            return Results.Ok(media);
        });
    }
}