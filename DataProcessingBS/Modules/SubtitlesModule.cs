using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class SubtitlesModule
{
    public static void AddSubtitlesEndpoints(this IEndpointRouteBuilder app)
    {
        // Create a new subtitle
        app.MapPost("/subtitles", async ([FromBody] CreatSubtitleRequest request, [FromServices] AppDbcontext dbContext) =>
        {
            var subtitle = new Subtitle
            {
                Media_Id = request.Media_Id,
                Language = request.Language
            };

            await dbContext.Subtitles.AddAsync(subtitle);
            await dbContext.SaveChangesAsync();

            return Results.Ok(subtitle);
        });

        // Get all subtitles
        app.MapGet("/subtitles", async ([FromServices] AppDbcontext dbContext) =>
        {
            var subtitles = await dbContext.Subtitles.ToListAsync();
            return Results.Ok(subtitles);
        });

        // Get subtitle by ID
        app.MapGet("/subtitles/{id:int}", async (int id, [FromServices] AppDbcontext dbContext) =>
        {
            var subtitle = await dbContext.Subtitles.FirstOrDefaultAsync(s => s.Subtitle_Id == id);

            return subtitle == null
                ? Results.NotFound()
                : Results.Ok(subtitle);
        });

        // Delete subtitle by ID
        app.MapDelete("/subtitles/{id:int}", async (int id, [FromServices] AppDbcontext dbContext) =>
        {
            var subtitle = await dbContext.Subtitles.FirstOrDefaultAsync(s => s.Subtitle_Id == id);

            if (subtitle == null)
                return Results.NotFound("Subtitle not found.");

            dbContext.Subtitles.Remove(subtitle);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new { Message = "Subtitle deleted successfully." });
        });
    }
}