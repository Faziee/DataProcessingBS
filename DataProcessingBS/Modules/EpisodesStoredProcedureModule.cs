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
            // Validate inputs
            if (string.IsNullOrEmpty(createEpisodeRequest.Series_Title))
            {
                return Results.BadRequest(new { Message = "Series title is required." });
            }
        
            // Step 1: Create the Media using the CreateNewMedia stored procedure and retrieve MediaId
            var mediaIdParameter = new SqlParameter
            {
                ParameterName = "@MediaId",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
        
            // Execute the stored procedure with the output parameter
            await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                    EXEC CreateNewMedia 
                    @GenreId={createEpisodeRequest.Genre_Id}, 
                    @Title={createEpisodeRequest.Title}, 
                    @AgeRating={createEpisodeRequest.Age_Rating}, 
                    @Quality={createEpisodeRequest.Quality}, 
                    @MediaId={mediaIdParameter} OUTPUT");
        
            // Retrieve the MediaId from the output parameter
            int mediaId = (int)mediaIdParameter.Value;
        
            var seriesId = createEpisodeRequest.Series_Id;
        
            if (seriesId == 0) // New Series
            {
                // Step 2: Create the new series
                await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                        EXEC CreateSeries 
                        @Title={createEpisodeRequest.Series_Title}, 
                        @GenreId={createEpisodeRequest.Genre_Id},  
                        @AgeRating={createEpisodeRequest.Age_Rating}");
        
                // Retrieve the new SeriesId using an output parameter
                var seriesIdParameter = new SqlParameter
                {
                    ParameterName = "@Series_Id",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };
        
                try
                {
                    // Execute the stored procedure to get the SeriesId by Series Title
                    await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
                        EXEC GetSeriesIdByTitle 
                        @Series_Title={createEpisodeRequest.Series_Title}, 
                        @Series_Id={seriesIdParameter} OUTPUT");
        
                    // Check if SeriesId is null or 0 (meaning it wasn't found)
                    if (seriesIdParameter.Value == null || (int)seriesIdParameter.Value == 0)
                    {
                        return Results.BadRequest(new { Message = "Series not found." });
                    }
        
                    // Retrieve the SeriesId from the output parameter
                    seriesId = (int)seriesIdParameter.Value;
                }
                catch (Exception ex)
                {
                    // Return a JSON response with a custom message and status code
                    return Results.Json(new { Message = $"Error executing stored procedure: {ex.Message}" }, statusCode: 500);
                }
            }
        
            // Step 3: Create the Episode using the stored procedure
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




// Update Episode using Stored Procedure
        app.MapPut("/stored-procedure-update-episode-by-id", async ([FromBody] UpdateEpisodeRequest updateEpisodeRequest, [FromServices] AppDbcontext dbContext) =>
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC UpdateEpisodeById @Episode_Id={updateEpisodeRequest.Episode_Id}, @Media_Id={updateEpisodeRequest.Media_Id}, @Series_Id={updateEpisodeRequest.Series_Id}, @Season_Number={updateEpisodeRequest.Season_Number}, @Episode_Number={updateEpisodeRequest.Episode_Number}, @Title={updateEpisodeRequest.Title}, @Duration={updateEpisodeRequest.Duration}");
            return Results.Ok();
        });

        // Get Episodes using Stored Procedure
        app.MapGet("/stored-procedure-get-episodes", async (AppDbcontext dbContext) =>
        {
            var episodes = await dbContext.Episodes.FromSqlRaw("EXEC GetAllEpisodes").ToListAsync();
            return Results.Ok(episodes);
        });

        // Get Episode by Episode_Id using Stored Procedure
        app.MapGet("/stored-procedure-get-episode-by-id/{episodeId:int}", async (int episodeId, [FromServices] AppDbcontext dbContext) =>
        {
            var episode = await dbContext.Episodes
                .FromSqlInterpolated($"EXEC GetEpisodeById @Episode_Id={episodeId}")
                .ToListAsync()
                .ContinueWith(task => task.Result.Select(e => new EpisodeDto
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

        // Delete Episode and Media if it's used only by that episode
        app.MapDelete("/stored-procedure-delete-episode/{episodeId}", async (int episodeId, [FromServices] AppDbcontext dbContext) =>
        {
            // Get the Media_Id associated with the episode
            var mediaId = await dbContext.Episodes
                .Where(e => e.Episode_Id == episodeId)
                .Select(e => e.Media_Id)
                .FirstOrDefaultAsync();

            if (mediaId == 0)
            {
                return Results.NotFound("Episode not found.");
            }

            // Delete the episode using the stored procedure
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteEpisodeById @EpisodeId={episodeId}");

            // After deleting the episode, delete the associated media
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteMediaById @MediaId={mediaId}");

            return Results.Ok();
        });

    }
}