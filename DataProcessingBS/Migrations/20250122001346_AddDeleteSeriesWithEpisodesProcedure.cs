using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    public partial class AddDeleteSeriesWithEpisodesProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the stored procedure
            migrationBuilder.Sql(@"
            CREATE PROCEDURE DeleteSeriesWithEpisodes
                @SeriesId INT
            AS
            BEGIN
                BEGIN TRANSACTION;

                BEGIN TRY
                    -- Delete episodes associated with the series
                    DELETE FROM Episodes
                    WHERE Series_Id = @SeriesId;

                    -- Delete the series
                    DELETE FROM Series
                    WHERE Series_Id = @SeriesId;

                    COMMIT TRANSACTION;
                END TRY
                BEGIN CATCH
                    ROLLBACK TRANSACTION;
                    THROW;
                END CATCH;
            END;
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the stored procedure
            migrationBuilder.Sql(@"
            DROP PROCEDURE IF EXISTS DeleteSeriesWithEpisodes;
        ");
        }
    }

}
