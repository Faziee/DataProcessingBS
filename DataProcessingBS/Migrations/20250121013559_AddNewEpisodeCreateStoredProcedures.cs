using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEpisodeCreateStoredProcedures : Migration
    {
            protected override void Up(MigrationBuilder migrationBuilder)
            {
                // Creating the 'GetLatestMediaId' stored procedure
                migrationBuilder.Sql(@"
            CREATE PROCEDURE GetLatestMediaId
            AS
            BEGIN
                SELECT MAX(Media_Id) AS MediaId FROM Media;
            END;
        ");

                // Creating the 'GetSeriesIdByTitle' stored procedure
                migrationBuilder.Sql(@"
            CREATE PROCEDURE GetSeriesIdByTitle
                @SeriesTitle NVARCHAR(255)
            AS
            BEGIN
                SELECT Series_Id FROM Series WHERE Title = @SeriesTitle;
            END;
        ");
            }

            protected override void Down(MigrationBuilder migrationBuilder)
            {
                // Dropping the 'GetLatestMediaId' stored procedure if the migration is rolled back
                migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetLatestMediaId");

                // Dropping the 'GetSeriesIdByTitle' stored procedure if the migration is rolled back
                migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetSeriesIdByTitle");
            }
    }
}
