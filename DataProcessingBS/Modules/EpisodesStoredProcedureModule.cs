using System.Data;
using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class EpisodesStoredProcedureModule
{
    public static void AddEpisodeStoredProcedureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/stored-procedure-create-episode", async ([FromBody] CreateEpisodeRequest createEpisodeRequest,
            [FromServices] AppDbcontext dbContext) =>
        {
            if (string.IsNullOrEmpty(createEpisodeRequest.Series_Title))
                return Results.BadRequest(new { Message = "Series title is required." });

            var mediaIdParameter = new SqlParameter
            {
                ParameterName = "@MediaId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                    EXEC CreateNewMedia 
                    @GenreId={createEpisodeRequest.Genre_Id}, 
                    @Title={createEpisodeRequest.Title}, 
                    @AgeRating={createEpisodeRequest.Age_Rating}, 
                    @Quality={createEpisodeRequest.Quality}, 
                    @MediaId={mediaIdParameter} OUTPUT");

            var mediaId = (int)mediaIdParameter.Value;

            var seriesId = createEpisodeRequest.Series_Id;

            if (seriesId == 0)
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                        EXEC CreateSeries 
                        @Title={createEpisodeRequest.Series_Title}, 
                        @GenreId={createEpisodeRequest.Genre_Id},  
                        @AgeRating={createEpisodeRequest.Age_Rating}");

                var seriesIdParameter = new SqlParameter
                {
                    ParameterName = "@Series_Id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                try
                {
                    await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                        EXEC GetSeriesIdByTitle 
                        @Series_Title={createEpisodeRequest.Series_Title}, 
                        @Series_Id={seriesIdParameter} OUTPUT");

                    if (seriesIdParameter.Value == null || (int)seriesIdParameter.Value == 0)
                        return Results.BadRequest(new { Message = "Series not found." });

                    seriesId = (int)seriesIdParameter.Value;
                }
                catch (Exception ex)
                {
                    return Results.Json(new { Message = $"Error executing stored procedure: {ex.Message}" },
                        statusCode: 500);
                }
            }

            await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                    EXEC CreateEpisode 
                    @MediaId={mediaId}, 
                    @SeriesId={seriesId}, 
                    @SeasonNumber={createEpisodeRequest.Season_Number}, 
                    @EpisodeNumber={createEpisodeRequest.Episode_Number}, 
                    @Title={createEpisodeRequest.Title}, 
                    @Duration={createEpisodeRequest.Duration}");

            return Results.Ok(new { Message = "Episode created successfully" });
        });

        app.MapGet("/stored-procedure-get-episodes", async (AppDbcontext dbContext) =>
        {
            var episodes = await dbContext.Episodes.FromSqlRaw("EXEC GetAllEpisodes").ToListAsync();
            return Results.Ok(episodes);
        });

        app.MapGet("/stored-procedure-get-episode-by-id/{episodeId:int}",
            async (int episodeId, [FromServices] AppDbcontext dbContext) =>
            {
                var episode = await dbContext.Episodes
                    .FromSqlInterpolated($"EXEC GetEpisodeById @Episode_Id={episodeId}")
                    .ToListAsync()
                    .ContinueWith(task => Enumerable.Select(task.Result, e => new EpisodeDto
                    {
                        Episode_Id = e.Episode_Id,
                        Media_Id = e.Media_Id,
                        Series_Id = e.Series_Id,
                        Season_Number = e.Season_Number,
                        Episode_Number = e.Episode_Number,
                        Title = e.Title,
                        Duration = e.Duration
                    }).FirstOrDefault());

                return episode == null
                    ? Results.NotFound()
                    : Results.Ok(episode);
            });


        app.MapPut("/stored-procedure-update-episode-by-id",
            async ([FromBody] UpdateEpisodeRequest updateEpisodeRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC UpdateEpisodeById @Episode_Id={updateEpisodeRequest.Episode_Id}, @Media_Id={updateEpisodeRequest.Media_Id}, @Series_Id={updateEpisodeRequest.Series_Id}, @Season_Number={updateEpisodeRequest.Season_Number}, @Episode_Number={updateEpisodeRequest.Episode_Number}, @Title={updateEpisodeRequest.Title}, @Duration={updateEpisodeRequest.Duration}");
                return Results.Ok();
            });

        app.MapDelete("/stored-procedure-delete-episode/{episodeId}",
            async (int episodeId, [FromServices] AppDbcontext dbContext) =>
            {
                var mediaId = await dbContext.Episodes
                    .Where(e => e.Episode_Id == episodeId)
                    .Select(e => e.Media_Id)
                    .FirstOrDefaultAsync();

                if (mediaId == 0) return Results.NotFound("Episode not found.");

                await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteEpisodeById @EpisodeId={episodeId}");

                await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteMediaById @MediaId={mediaId}");

                return Results.Ok();
            });
    }
}