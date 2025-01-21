using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    public partial class AddEpisodeStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[CreateEpisode]
                    @MediaId INT,
                    @SeriesId INT,
                    @SeasonNumber INT,
                    @EpisodeNumber INT,
                    @Title VARCHAR(255),
                    @Duration INT = NULL
                AS
                BEGIN
                    SET NOCOUNT ON;

                    INSERT INTO [dbo].[Episodes] (media_id, series_id, season_number, episode_number, title, duration)
                    VALUES (@MediaId, @SeriesId, @SeasonNumber, @EpisodeNumber, @Title, @Duration);
                END;
            ");
            
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[GetAllEpisodes]
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT 
                        episode_id,
                        media_id,
                        series_id,
                        season_number,
                        episode_number,
                        title,
                        duration
                    FROM [dbo].[Episodes];
                END;
            ");
            
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[GetEpisodeById]
                    @EpisodeId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT 
                        episode_id,
                        media_id,
                        series_id,
                        season_number,
                        episode_number,
                        title,
                        duration
                    FROM [dbo].[Episodes]
                    WHERE episode_id = @EpisodeId;
                END;
            ");
            
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[UpdateEpisodeById]
                    @EpisodeId INT,
                    @MediaId INT = NULL,
                    @SeriesId INT = NULL,
                    @SeasonNumber INT = NULL,
                    @EpisodeNumber INT = NULL,
                    @Title VARCHAR(255) = NULL,
                    @Duration INT = NULL
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE [dbo].[Episodes]
                    SET 
                        media_id = ISNULL(@MediaId, media_id),
                        series_id = ISNULL(@SeriesId, series_id),
                        season_number = ISNULL(@SeasonNumber, season_number),
                        episode_number = ISNULL(@EpisodeNumber, episode_number),
                        title = ISNULL(@Title, title),
                        duration = ISNULL(@Duration, duration)
                    WHERE episode_id = @EpisodeId;
                END;
            ");
            
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[DeleteEpisodeById]
                    @EpisodeId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DELETE FROM [dbo].[Episodes]
                    WHERE episode_id = @EpisodeId;
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[CreateEpisode]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetAllEpisodes]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetEpisodeById]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[UpdateEpisodeById]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[DeleteEpisodeById]");
        }
    }

}
