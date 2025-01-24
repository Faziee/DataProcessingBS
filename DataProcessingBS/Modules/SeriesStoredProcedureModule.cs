using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class SeriesStoredProcedureModule
{
    public static void AddSeriesStoredProcedureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/stored-procedure-create-series", async ([FromBody] CreateSeriesRequest createSeriesRequest, [FromServices] AppDbcontext dbContext) =>
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC CreateSeries @GenreId={createSeriesRequest.Genre_Id}, @Title={createSeriesRequest.Title}, @AgeRating={createSeriesRequest.Age_Rating}");
            return Results.Ok();
        });

        app.MapPut("/stored-procedure-update-series-by-id", async ([FromBody] UpdateSeriesRequest updateSeriesRequest, [FromServices] AppDbcontext dbContext) =>
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC UpdateSeriesById @SeriesId={updateSeriesRequest.Series_Id}, @GenreId={updateSeriesRequest.Genre_Id}, @Title={updateSeriesRequest.Title}, @AgeRating={updateSeriesRequest.Age_Rating}");
            return Results.Ok();
        });

        app.MapGet("/stored-procedure-get-series", async (AppDbcontext dbContext) =>
        {
            var series = await dbContext.Series.FromSqlRaw("EXEC GetAllSeries").ToListAsync();
            return Results.Ok(series);
        });
        
        
        app.MapGet("/stored-procedure-get-episodes-by-series-id/{seriesId:int}", async (int seriesId, [FromServices] AppDbcontext dbContext) =>
        {
            var series = dbContext.Series
                .FromSqlInterpolated($"EXEC GetSeriesById @SeriesId={seriesId}")
                .AsEnumerable()
                .FirstOrDefault();

            if (series == null)
            {
                return Results.NotFound(new { message = "Series not found." });
            }

            // Get episodes by series ID using SqlQuery
            var episodes = dbContext.Database
                .SqlQuery<EpisodeWithSeriesTitleDto>($"EXEC GetEpisodesBySeriesId @Series_Id={seriesId}")
                .ToList();

            if (!episodes.Any())
            {
                return Results.NotFound(new { message = "No episodes found for the given series." });
            }

            return Results.Ok(new
            {
                SeriesTitle = series.Title,
                Episodes = episodes
            });
        });
        
        app.MapGet("/stored-procedure-get-series-by-id/{seriesId:int}", async (int seriesId, [FromServices] AppDbcontext dbContext) =>
        {
            var series = await dbContext.Series
                .FromSqlInterpolated($"EXEC GetSeriesById @SeriesId={seriesId}")
                .ToListAsync()
                .ContinueWith(task => task.Result.Select(s => new SeriesDto
                {
                    Series_Id = s.Series_Id,
                    Genre_Id = s.Genre_Id,
                    Title = s.Title,
                    Age_Rating = s.Age_Rating
                }).FirstOrDefault());

            return series == null
                ? Results.NotFound()
                : Results.Ok(series);
        });

        app.MapDelete("/stored-procedure-delete-series/{seriesId}", async (int seriesId, [FromServices] AppDbcontext dbContext) =>
        {
            try
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                    EXEC DeleteSeriesWithEpisodes @SeriesId={seriesId}");
        
                return Results.Ok(new { Message = "Series and related episodes deleted successfully." });
            }
            catch (Exception ex)
            {
                return Results.Json(new { Message = $"An error occurred: {ex.Message}" }, statusCode: 500);
            }
        });

    }
}
