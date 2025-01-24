using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    public partial class AddSeriesStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[CreateSeries]
                    @GenreId INT,
                    @Title VARCHAR(255),
                    @AgeRating VARCHAR(10)
                AS
                BEGIN
                    SET NOCOUNT ON;
                    INSERT INTO [dbo].[Series] (genre_id, title, age_rating)
                    VALUES (@GenreId, @Title, @AgeRating);
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[GetAllSeries]
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT 
                        series_id,
                        genre_id,
                        title,
                        age_rating
                    FROM [dbo].[Series];
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[GetSeriesById]
                    @SeriesId INT
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT 
                        series_id,
                        genre_id,
                        title,
                        age_rating
                    FROM [dbo].[Series]
                    WHERE series_id = @SeriesId;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[UpdateSeriesById]
                    @SeriesId INT,
                    @GenreId INT = NULL,
                    @Title VARCHAR(255) = NULL,
                    @AgeRating VARCHAR(10) = NULL
                AS
                BEGIN
                    SET NOCOUNT ON;
                    UPDATE [dbo].[Series]
                    SET 
                        genre_id = ISNULL(@GenreId, genre_id),
                        title = ISNULL(@Title, title),
                        age_rating = ISNULL(@AgeRating, age_rating)
                    WHERE series_id = @SeriesId;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[DeleteSeriesById]
                    @SeriesId INT
                AS
                BEGIN
                    SET NOCOUNT ON;
                    DELETE FROM [dbo].[Series]
                    WHERE series_id = @SeriesId;
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the stored procedures in case of rollback
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[CreateSeries]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetAllSeries]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetSeriesById]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[UpdateSeriesById]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[DeleteSeriesById]");
        }
    }
}
