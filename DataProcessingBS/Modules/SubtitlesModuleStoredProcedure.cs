using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class SubtitlesModuleStoredProcedure
{
    public static void AddSubtitlesStoredProcedureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/stored-procedure-create-subtitle",
            async ([FromBody] CreatSubtitleRequest creatSubtitleRequest, [FromServices] AppDbcontext dbContext) =>
            {
                // Validate input
                if (creatSubtitleRequest == null || creatSubtitleRequest.Media_Id <= 0 || string.IsNullOrWhiteSpace(creatSubtitleRequest.Language))
                {
                    return Results.BadRequest(new { Message = "Invalid input: Media_Id and Language are required." });
                }

                try
                {
                    // Define parameters
                    var mediaIdParameter = new SqlParameter("@GMedia_id", creatSubtitleRequest.Media_Id);
                    var languageParameter = new SqlParameter("@Language", creatSubtitleRequest.Language);

                    // Execute the stored procedure
                    await dbContext.Database.ExecuteSqlRawAsync(
                        "EXEC CreateSubtitle @GMedia_id, @Language", mediaIdParameter, languageParameter);

                    return Results.Ok(new { Message = "Subtitle created successfully." });
                }
                catch (Exception ex)
                {
                    // Return an error response
                    return Results.Problem($"An error occurred while creating the subtitle: {ex.Message}", statusCode: 500);
                }
            });
        
        // Get all subtitles
        app.MapGet("/stored-procedure-get-subtitles", async ([FromServices] AppDbcontext dbContext) =>
        {
            var subtitles = await dbContext.Subtitles
                .FromSqlRaw("EXEC GetAllSubtitles")
                .ToListAsync();
            return Results.Ok(subtitles);
        });
        
        app.MapGet("/stored-procedure-get-subtitle-by-id/{subtitleId:int}", async (int subtitleId, [FromServices] AppDbcontext dbContext) =>
        {
            var subtitleDto = await dbContext.Subtitles
                .FromSqlInterpolated($"EXEC GetSubtitleById @Subtitle_Id={subtitleId}")
                .ToListAsync()
                .ContinueWith(task => task.Result.Select(s => new SubtitleDto()
                {
                    SubtitleId = s.Subtitle_Id,
                    MediaId = s.Media_Id,
                    Language = s.Language
                }).FirstOrDefault());

            return subtitleDto == null 
                ? Results.NotFound() 
                : Results.Ok(subtitleDto);
        });

        
        /*// Get subtitles by media ID
        app.MapGet("/stored-procedure-get-subtitles-by-media/{mediaId:int}", async (int mediaId, [FromServices] AppDbcontext dbContext) =>
        {
            var subtitles = await dbContext.Subtitles
                .FromSqlInterpolated($"EXEC GetSubtitlesByMedia @MediaId={mediaId}")
                .ToListAsync();

            return Results.Ok(subtitles);
        });*/

        // Delete subtitle by ID
        app.MapDelete("/stored-procedure-delete-subtitle-by-id/{subtitleId}", async (int subtitleId,[FromServices] AppDbcontext dbContext) =>
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteSubtitleById @Subtitle_Id={subtitleId}");
            return Results.Ok();
        });
    }
}