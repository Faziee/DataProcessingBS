using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules
{
    public static class WatchListsStoredProcedureModule
    {
        public static void AddWatchListsStoredProcedureEndpoints(this IEndpointRouteBuilder app)
        {
            // Create WatchList using Stored Procedure
            app.MapPost("/stored-procedure-create-watchlist", async ([FromBody] CreateWatchListRequest createWatchListRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC CreateWatchList @ProfileId={createWatchListRequest.Profile_Id}, @MediaId={createWatchListRequest.Media_Id}");
                return Results.Ok();
            });

            // Update WatchList using Stored Procedure
            app.MapPut("/stored-procedure-update-watchlist-by-id", async ([FromBody] UpdateWatchListRequest updateWatchListRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC UpdateWatchListById @WatchListId={updateWatchListRequest.WatchList_Id}, @ProfileId={updateWatchListRequest.Profile_Id}, @MediaId={updateWatchListRequest.Media_Id}, @AddedDate={updateWatchListRequest.AddedDate}");
                return Results.Ok();
            });

            // Get All WatchLists using Stored Procedure
            app.MapGet("/stored-procedure-get-watchlists", async (AppDbcontext dbContext) =>
            {
                var watchlists = await dbContext.WatchLists.FromSqlRaw("EXEC GetAllWatchLists").ToListAsync();
                return Results.Ok(watchlists);
            });

            // Get WatchList by ID using Stored Procedure
            app.MapGet("/stored-procedure-get-watchlist-by-id/{watchListId:int}", async (int watchListId, [FromServices] AppDbcontext dbContext) =>
            {
                var watchlist = await dbContext.WatchLists
                    .FromSqlInterpolated($"EXEC GetWatchListById @WatchListId={watchListId}")
                    .ToListAsync()
                    .ContinueWith(task => task.Result.Select(w => new WatchListDto()
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

            // Delete WatchList by ID using Stored Procedure
            app.MapDelete("/stored-procedure-delete-watchlist-by-id/{watchListId}", async (int watchListId, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteWatchListById @WatchListId={watchListId}");
                return Results.Ok();
            });
        }
    }
}
