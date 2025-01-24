using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    public partial class AddGetEpisodesBySeriesIdStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.Sql(@"
            CREATE PROCEDURE GetEpisodesBySeriesId
                @Series_Id INT
            AS
            BEGIN
                SELECT 
                    e.Episode_Id,
                    e.Series_Id,
                    e.Title AS Episode_Title,
                    e.Duration,
                    e.Season_Number,
                    e.Episode_Number,
                    s.Title AS Series_Title
                FROM Episodes e
                INNER JOIN Series s ON e.Series_Id = s.Series_Id
                WHERE e.Series_Id = @Series_Id
            END
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetEpisodesBySeriesId");
        }
    }
}
