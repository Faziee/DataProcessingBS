using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class WatchesModule
{
    public static void AddWatchesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/watches", async ([FromBody] CreateWatchRequest createWatchRequest, [FromServices] AppDbcontext dbContext) =>
        {
            var watch = new Watch()
            {
                Profile_Id = createWatchRequest.Profile_Id,
                Media_Id = createWatchRequest.Media_Id,
                Watch_Date = createWatchRequest.Watch_Date,
                Status = createWatchRequest.Status,
                Pause_Time = createWatchRequest.Pause_Time
            };

            await dbContext.Watches.AddAsync(watch);
            await dbContext.SaveChangesAsync();
            return Results.Ok(watch);
        });

        app.MapPut("/watches/{watchId}", async (int watchId, [FromBody] UpdateWatchRequest updateWatchRequest, [FromServices] AppDbcontext dbContext) =>
        {
            var watch = await dbContext.Watches.FirstOrDefaultAsync(w => w.Watch_Id == watchId);

            if (watch == null) return Results.NotFound();

            watch.Profile_Id = updateWatchRequest.Profile_Id;
            watch.Media_Id = updateWatchRequest.Media_Id;
            watch.Watch_Date = updateWatchRequest.Watch_Date;
            watch.Status = updateWatchRequest.Status;
            watch.Pause_Time = updateWatchRequest.Pause_Time;

            await dbContext.SaveChangesAsync();
            return Results.Ok(watch);
        });

        app.MapGet("/watches", async (AppDbcontext dbContext) =>
        {
            var watches = await dbContext.Watches.ToListAsync();
            return Results.Ok(watches);
        });

        app.MapGet("/watches/{watchId:int}", async (int watchId, [FromServices] AppDbcontext dbContext) =>
        {
            var watch = await dbContext.Watches.FindAsync(watchId);
            return watch == null ? Results.NotFound() : Results.Ok(watch);
        });

        app.MapDelete("/watches/{watchId:int}", async (int watchId, [FromServices] AppDbcontext dbContext) =>
        {
            var watch = await dbContext.Watches.FindAsync(watchId);

            if (watch == null) return Results.NotFound("Watch not found.");

            dbContext.Watches.Remove(watch);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new { Message = "Watch deleted successfully." });
        });
    }
}
