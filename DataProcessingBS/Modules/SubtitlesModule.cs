using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DataProcessingBS.Modules;

public static class SubtitlesModule
{
    public static void AddSubtitlesEndpoints(this IEndpointRouteBuilder app)
    {
        // Create Series
        app.MapPost("/subtitle/",
            async ([FromBody] CreatSubtitleRequest creatSubtitleRequest, [FromServices] AppDbcontext dbContext) =>
            {
                var subtitle = new Subtitle()
                {
                    Media_Id = creatSubtitleRequest.Media_Id,
                    Language = creatSubtitleRequest.Language,
                };

                await dbContext.Subtitles.AddAsync(subtitle);
                await dbContext.SaveChangesAsync();
                return Results.Ok(subtitle);
            });
    }
}