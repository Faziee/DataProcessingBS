using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    public partial class AddGetSeriesIdByTitleProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add stored procedure creation
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetSeriesIdByTitle
                    @Series_Title NVARCHAR(255),
                    @Series_Id INT OUTPUT
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    -- Retrieve the SeriesId from the Series table
                    SELECT @Series_Id = Series_Id
                    FROM Series
                    WHERE Title = @Series_Title;
                    
                    -- Return a result
                    SELECT @Series_Id AS Series_Id;
                END;
            ");
        }
    
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the stored procedure in case of rollback
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetSeriesIdByTitle;");
        }
    }

}
