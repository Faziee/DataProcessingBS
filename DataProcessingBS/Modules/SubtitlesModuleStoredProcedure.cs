using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class SubtitlesModuleStoredProcedure
{
    public static void AddSubtitlesStoredProcedureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/stored-procedure-create-subtitle",
            async ([FromBody] CreatSubtitleRequest creatSubtitleRequest, [FromServices] AppDbcontext dbContext) =>
            {
                try
                {
                    // Execute the stored procedure that creates a subtitle for a given Media_Id
                    await dbContext.Database.ExecuteSqlInterpolatedAsync(
                        $"EXEC CreateSubtitle @GMedia_id={creatSubtitleRequest.Media_Id}, @Language={creatSubtitleRequest.Language}");
                    
                    return Results.Ok(new { message = "Subtitle created successfully." });
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., invalid parameters, database issues)
                    return Results.Problem($"An error occurred: {ex.Message}", statusCode: 500);
                }
            });
        
        /*app.MapPost("/stored-procedure-create-subtile", async ([FromBody] CreatSubtitleRequest creatSubtitleRequest, [FromServices] AppDbcontext dbContext) =>
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC CreateSubtitle @GMedia_id={creatSubtitleRequest.Media_Id}, @Language={creatSubtitleRequest.Language}");
            return Results.Ok();
        });*/
    }
}