using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class WatchListsStoredProcedureModule
{
    public static void AddWatchListsStoredProcedureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/stored-procedure-create-watchlist",
            async ([FromBody] CreateWatchListRequest createWatchListRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC CreateWatchList @ProfileId={createWatchListRequest.Profile_Id}, @MediaId={createWatchListRequest.Media_Id}");
                return Results.Ok();
            });

        app.MapGet("/stored-procedure-get-watchlists", async (AppDbcontext dbContext) =>
        {
            var watchlists = await dbContext.WatchLists.FromSqlRaw("EXEC GetAllWatchLists").ToListAsync();
            return Results.Ok(watchlists);
        });

        app.MapGet("/stored-procedure-get-watchlist-by-id/{watchListId:int}",
            async (int watchListId, [FromServices] AppDbcontext dbContext) =>
            {
                var watchlist = await dbContext.WatchLists
                    .FromSqlInterpolated($"EXEC GetWatchListById @WatchListId={watchListId}")
                    .ToListAsync()
                    .ContinueWith(task => Enumerable.Select(task.Result, w => new WatchListDto
                    {
                        WatchList_Id = w.WatchList_Id,
                        Profile_Id = w.Profile_Id,
                        Media_Id = w.Media_Id,
                        Added_Date = w.Added_Date
                    }).FirstOrDefault());

                return watchlist == null
                    ? Results.NotFound()
                    : Results.Ok(watchlist);
            });

        app.MapPut("/stored-procedure-update-watchlist-by-id",
            async ([FromBody] UpdateWatchListRequest updateWatchListRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC UpdateWatchListById @WatchListId={updateWatchListRequest.WatchList_Id}, @ProfileId={updateWatchListRequest.Profile_Id}, @MediaId={updateWatchListRequest.Media_Id}, @AddedDate={updateWatchListRequest.AddedDate}");
                return Results.Ok();
            });

        app.MapDelete("/stored-procedure-delete-watchlist-by-id/{watchListId}",
            async (int watchListId, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC DeleteWatchListById @WatchListId={watchListId}");
                return Results.Ok();
            });
    }
}