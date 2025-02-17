using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class WatchListModule
{
    public static void AddWatchListEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/watchlists",
            async ([FromBody] CreateWatchListRequest createWatchListRequest, [FromServices] AppDbcontext dbContext) =>
            {
                var watchList = new WatchList
                {
                    Profile_Id = createWatchListRequest.Profile_Id,
                    Media_Id = createWatchListRequest.Media_Id
                };

                await dbContext.WatchLists.AddAsync(watchList);
                await dbContext.SaveChangesAsync();
                return Results.Ok(watchList);
            });

        app.MapGet("/watchlists", async (AppDbcontext dbContext) =>
        {
            var watchlists = await dbContext.WatchLists.ToListAsync();
            return Results.Ok(watchlists);
        });

        app.MapGet("/watchlists/{watchListId:int}", async (int watchListId, [FromServices] AppDbcontext dbContext) =>
        {
            var watchList = await dbContext.WatchLists.FindAsync(watchListId);
            return watchList == null ? Results.NotFound() : Results.Ok(watchList);
        });

        app.MapPut("/watchlists/{watchListId}", async (int watchListId,
            [FromBody] UpdateWatchListRequest updateWatchListRequest, [FromServices] AppDbcontext dbContext) =>
        {
            var watchList = await dbContext.WatchLists.FirstOrDefaultAsync(w => w.WatchList_Id == watchListId);

            if (watchList == null) return Results.NotFound();

            watchList.Profile_Id = updateWatchListRequest.Profile_Id;
            watchList.Media_Id = updateWatchListRequest.Media_Id;
            watchList.Added_Date = updateWatchListRequest.AddedDate;

            await dbContext.SaveChangesAsync();

            return Results.Ok(watchList);
        });

        app.MapDelete("/watchlists/{watchListId:int}", async (int watchListId, [FromServices] AppDbcontext dbContext) =>
        {
            var watchList = await dbContext.WatchLists.FindAsync(watchListId);

            if (watchList == null) return Results.NotFound("WatchList not found.");

            dbContext.WatchLists.Remove(watchList);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new { Message = "WatchList deleted successfully." });
        });
    }
}