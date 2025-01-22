using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class MediaStoredProcedureModule
{
    public static void AddMediaStoredProcedureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPut("/stored-procedure-update-media-by-id", async ([FromBody] UpdateMediaRequest updateMediaRequest, [FromServices] AppDbcontext dbContext) =>
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC UpdateMediaById @MediaId={updateMediaRequest.Media_Id}, @GenreId={updateMediaRequest.Genre_Id}, @Title={updateMediaRequest.Title}, @AgeRating={updateMediaRequest.Age_Rating}, @Quality={updateMediaRequest.Quality}");
            return Results.Ok();
        });
        
        app.MapGet("/stored-procedure-get-media", async (AppDbcontext dbContext) =>
        {
            var media = await dbContext.Media.FromSqlRaw("EXEC GetAllMedia").ToListAsync();
            return Results.Ok(media);
        });
        
        app.MapGet("/stored-procedure-get-media-by-id/{mediaId:int}", async (int mediaId, [FromServices] AppDbcontext dbContext) =>
        {
            var media = await dbContext.Media
                .FromSqlInterpolated($"EXEC GetMediaById @MediaId={mediaId}")
                .ToListAsync()
                .ContinueWith(task => task.Result.Select(m => new MediaDto
                {
                    Media_Id = m.Media_Id,
                    Genre_Id = m.Genre_Id,
                    Title = m.Title,
                    Age_Rating = m.Age_Rating,
                    Quality = m.Quality
                }).FirstOrDefault());

            return media == null
                ? Results.NotFound()
                : Results.Ok(media);
        });
    }
}
