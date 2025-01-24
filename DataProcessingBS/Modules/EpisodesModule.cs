using System.Data;
using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class EpisodesModule
{
    public static void AddEpisodeEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/episodes",
            async ([FromBody] CreateEpisodeRequest createEpisodeRequest, [FromServices] AppDbcontext dbContext) =>
            {
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

                Series series;

                if (createEpisodeRequest.Series_Id == 0)
                {
                    series = new Series
                    {
                        Genre_Id = createEpisodeRequest.Genre_Id,
                        Title = createEpisodeRequest.Series_Title,
                        Age_Rating = createEpisodeRequest.Age_Rating
                    };

                    await dbContext.Series.AddAsync(series);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    series = await dbContext.Series.FirstOrDefaultAsync(s =>
                        s.Series_Id == createEpisodeRequest.Series_Id);

                    if (series == null) return Results.NotFound("Series not found.");
                }

                var episode = new Episode
                {
                    Media_Id = mediaId,
                    Series_Id = series.Series_Id,
                    Title = createEpisodeRequest.Title,
                    Season_Number = createEpisodeRequest.Season_Number,
                    Episode_Number = createEpisodeRequest.Episode_Number,
                    Duration = createEpisodeRequest.Duration
                };

                await dbContext.Episodes.AddAsync(episode);
                await dbContext.SaveChangesAsync();

                return Results.Ok(episode);
            });

        app.MapGet("/episodes", async (AppDbcontext dbContext) =>
        {
            var episodes = await dbContext.Episodes.ToListAsync();
            return Results.Ok(episodes);
        });

        app.MapGet("/episodes/{episodeId:int}", async (int episodeId, [FromServices] AppDbcontext dbContext) =>
        {
            var episode = await dbContext.Episodes
                .FirstOrDefaultAsync(x => x.Episode_Id == episodeId);

            return episode == null
                ? Results.NotFound()
                : Results.Ok(episode);
        });

        app.MapPut("/episodes/{episodeId}", async (int episodeId, [FromBody] UpdateEpisodeRequest updateEpisodeRequest,
            [FromServices] AppDbcontext dbContext) =>
        {
            var episode = await dbContext.Episodes.FirstOrDefaultAsync(x => x.Episode_Id == episodeId);

            if (episode != null)
            {
                episode.Media_Id = updateEpisodeRequest.Media_Id;
                episode.Series_Id = updateEpisodeRequest.Series_Id;
                episode.Season_Number = updateEpisodeRequest.Season_Number;
                episode.Episode_Number = updateEpisodeRequest.Episode_Number;
                episode.Title = updateEpisodeRequest.Title;
                episode.Duration = updateEpisodeRequest.Duration;

                await dbContext.SaveChangesAsync();
                return Results.Ok(episode);
            }

            return Results.NotFound();
        });

        app.MapDelete("/episodes/{episodeId}", async (int episodeId, AppDbcontext dbContext) =>
        {
            var episode = await dbContext.Episodes.FirstOrDefaultAsync(x => x.Episode_Id == episodeId);

            if (episode != null)
            {
                dbContext.Episodes.Remove(episode);
                await dbContext.SaveChangesAsync();
                return Results.Ok();
            }

            return Results.NotFound();
        });
    }
}