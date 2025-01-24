using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class SeriesModule
{
    public static void AddSeriesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/series",
            async ([FromBody] CreateSeriesRequest createSeriesRequest, [FromServices] AppDbcontext dbContext) =>
            {
                var series = new Series
                {
                    Genre_Id = createSeriesRequest.Genre_Id,
                    Title = createSeriesRequest.Title,
                    Age_Rating = createSeriesRequest.Age_Rating
                };

                await dbContext.Series.AddAsync(series);
                await dbContext.SaveChangesAsync();
                return Results.Ok(series);
            });

        app.MapGet("/series", async (AppDbcontext dbContext) =>
        {
            var series = await dbContext.Series.ToListAsync();
            return Results.Ok(series);
        });

        app.MapGet("/series/{seriesId:int}/episodes", async (int seriesId, [FromServices] AppDbcontext dbContext) =>
        {
            var episodes = await dbContext.Episodes
                .FromSqlRaw("EXEC GetEpisodesBySeriesId @SeriesId={0}", seriesId)
                .ToListAsync();

            return episodes.Count == 0
                ? Results.NotFound(new { Message = "No episodes found for the given series." })
                : Results.Ok(episodes);
        });

        app.MapGet("/series/{seriesId:int}", async (int seriesId, [FromServices] AppDbcontext dbContext) =>
        {
            var series = await dbContext.Series
                .FirstOrDefaultAsync(x => x.Series_Id == seriesId);

            return series == null
                ? Results.NotFound()
                : Results.Ok(series);
        });

        app.MapPut("/series/{seriesId}", async (int seriesId, [FromBody] UpdateSeriesRequest updateSeriesRequest,
            [FromServices] AppDbcontext dbContext) =>
        {
            var series = await dbContext.Series.FirstOrDefaultAsync(x => x.Series_Id == seriesId);

            if (series != null)
            {
                series.Genre_Id = updateSeriesRequest.Genre_Id;
                series.Title = updateSeriesRequest.Title;
                series.Age_Rating = updateSeriesRequest.Age_Rating;

                await dbContext.SaveChangesAsync();
                return Results.Ok(series);
            }

            return Results.NotFound();
        });

        app.MapDelete("/series/{seriesId}", async (int seriesId, AppDbcontext dbContext) =>
        {
            var series = await dbContext.Series.FirstOrDefaultAsync(x => x.Series_Id == seriesId);

            if (series != null)
            {
                dbContext.Series.Remove(series);
                await dbContext.SaveChangesAsync();
                return Results.Ok();
            }

            return Results.NotFound();
        });
    }
}