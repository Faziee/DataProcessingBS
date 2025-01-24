using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class WatchesStoredProcedureModule
{
    public static void AddWatchesStoredProcedureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/stored-procedure-create-watch",
            async ([FromBody] CreateWatchRequest createWatchRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC CreateWatch @ProfileId={createWatchRequest.Profile_Id}, @MediaId={createWatchRequest.Media_Id}, @WatchDate={createWatchRequest.Watch_Date}, @Status={createWatchRequest.Status}, @PauseTime={createWatchRequest.Pause_Time}");
                return Results.Ok();
            });

        app.MapPut("/stored-procedure-update-watch-by-id",
            async ([FromBody] UpdateWatchRequest updateWatchRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC UpdateWatchById @WatchId={updateWatchRequest.Watch_Id}, @ProfileId={updateWatchRequest.Profile_Id}, @MediaId={updateWatchRequest.Media_Id}, @WatchDate={updateWatchRequest.Watch_Date}, @Status={updateWatchRequest.Status}, @PauseTime={updateWatchRequest.Pause_Time}");
                return Results.Ok();
            });

        app.MapGet("/stored-procedure-get-watches", async (AppDbcontext dbContext) =>
        {
            var watches = await dbContext.Watches.FromSqlRaw("EXEC GetAllWatches").ToListAsync();
            return Results.Ok(watches);
        });

        app.MapGet("/stored-procedure-get-watch-by-id/{watchId:int}", async (int watchId, [FromServices] AppDbcontext dbContext) =>
        {
            var watch = await dbContext.Watches
                .FromSqlInterpolated($"EXEC GetWatchById @WatchId={watchId}")
                .ToListAsync()
                .ContinueWith(task => task.Result.Select(w => new WatchDto()
                {
                    Watch_Id = w.Watch_Id,
                    Profile_Id = w.Profile_Id,
                    Media_Id = w.Media_Id,
                    Watch_Date = w.Watch_Date,
                    Status = w.Status,
                    Pause_Time = w.Pause_Time
                }).FirstOrDefault());

            return watch == null
                ? Results.NotFound()
                : Results.Ok(watch);
        });

        app.MapDelete("/stored-procedure-delete-watch-by-id/{watchId}", async (int watchId, [FromServices] AppDbcontext dbContext) =>
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteWatchById @WatchId={watchId}");
            return Results.Ok();
        });
    }
}
