using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class SubtitlesModuleStoredProcedure
{
    public static void AddSeriesStoredProcedureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/stored-procedure-create-series",
            async ([FromBody] CreatSubtitleRequest creatSubtitleRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC CreateSubtitle @GMedia_id={creatSubtitleRequest.Media_Id}, @Language={creatSubtitleRequest.Language}");
                return Results.Ok();
            });
    }
}