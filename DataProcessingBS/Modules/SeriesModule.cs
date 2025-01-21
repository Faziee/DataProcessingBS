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
        // Create Series
        app.MapPost("/series", async ([FromBody] CreateSeriesRequest createSeriesRequest, [FromServices] AppDbcontext dbContext) =>
        {
            var series = new Series()
            {
                Genre_Id = createSeriesRequest.Genre_Id,
                Title = createSeriesRequest.Title,
                Age_Rating = createSeriesRequest.Age_Rating
            };

            await dbContext.Series.AddAsync(series);
            await dbContext.SaveChangesAsync();
            return Results.Ok(series);
        });

        // Update Series by Series_Id
        app.MapPut("/series/{seriesId}", async (int seriesId, [FromBody] UpdateSeriesRequest updateSeriesRequest, [FromServices] AppDbcontext dbContext) =>
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
            else
            {
                return Results.NotFound();
            }
        });

        // Get All Series
        app.MapGet("/series", async (AppDbcontext dbContext) =>
        {
            var series = await dbContext.Series.ToListAsync();
            return Results.Ok(series);
        });

        // Get Series by Series_Id
        app.MapGet("/series/{seriesId:int}", async (int seriesId, [FromServices] AppDbcontext dbContext) =>
        {
            var series = await dbContext.Series
                .FirstOrDefaultAsync(x => x.Series_Id == seriesId);

            return series == null
                ? Results.NotFound()
                : Results.Ok(series);
        });

        // Delete Series by Series_Id
        app.MapDelete("/series/{seriesId}", async (int seriesId, AppDbcontext dbContext) =>
        {
            var series = await dbContext.Series.FirstOrDefaultAsync(x => x.Series_Id == seriesId);

            if (series != null)
            {
                dbContext.Series.Remove(series);
                await dbContext.SaveChangesAsync();
                return Results.Ok();
            }
            else
            {
                return Results.NotFound();
            }
        });
    }
}
