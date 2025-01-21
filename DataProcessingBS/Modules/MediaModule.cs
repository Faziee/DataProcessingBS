using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class MediaModule
{
    public static void AddMediaEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/media", async ([FromBody] CreateMediaRequest createMediaRequest, [FromServices] AppDbcontext dbContext) =>
        {
            var media = new Media()
            {
                Genre_Id = createMediaRequest.Genre_Id,
                Title = createMediaRequest.Title,
                Age_Rating = createMediaRequest.Age_Rating,
                Quality = createMediaRequest.Quality
            };

            await dbContext.Media.AddAsync(media);
            await dbContext.SaveChangesAsync();
            return Results.Ok(media);
        });
        
        app.MapPut("/media/{mediaId}", async (int mediaId, [FromBody] UpdateMediaRequest updateMediaRequest, [FromServices] AppDbcontext dbContext) =>
        {
            var media = await dbContext.Media.FirstOrDefaultAsync(x => x.Media_Id == mediaId);

            if (media != null)
            {
                media.Genre_Id = updateMediaRequest.Genre_Id;
                media.Title = updateMediaRequest.Title;
                media.Age_Rating = updateMediaRequest.Age_Rating;
                media.Quality = updateMediaRequest.Quality;

                await dbContext.SaveChangesAsync();
                return Results.Ok(media);
            }
            else
            {
                return Results.NotFound();
            }
        });
        
        app.MapGet("/media", async (AppDbcontext dbContext) =>
        {
            var media = await dbContext.Media.ToListAsync();
            return Results.Ok(media);
        });
        
        app.MapGet("/media/{mediaId:int}", async (int mediaId, [FromServices] AppDbcontext dbContext) =>
        {
            var media = await dbContext.Media
                .FirstOrDefaultAsync(x => x.Media_Id == mediaId);

            return media == null
                ? Results.NotFound()
                : Results.Ok(media);
        });
        
        app.MapDelete("/media/{mediaId}", async (int mediaId, AppDbcontext dbContext) =>
        {
            var media = await dbContext.Media.FirstOrDefaultAsync(x => x.Media_Id == mediaId);

            if (media != null)
            {
                dbContext.Media.Remove(media);
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
