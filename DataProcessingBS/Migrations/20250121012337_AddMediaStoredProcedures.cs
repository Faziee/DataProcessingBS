using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    public partial class AddMediaStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[CreateMedia]
                    @GenreId INT,
                    @Title VARCHAR(255),
                    @AgeRating VARCHAR(10),
                    @Quality VARCHAR(10)
                AS
                BEGIN
                    SET NOCOUNT ON;

                    INSERT INTO [dbo].[Media] (genre_id, title, age_rating, quality)
                    VALUES (@GenreId, @Title, @AgeRating, @Quality);
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[GetAllMedia]
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT 
                        media_id,
                        genre_id,
                        title,
                        age_rating,
                        quality
                    FROM [dbo].[Media];
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[GetMediaById]
                    @MediaId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT 
                        media_id,
                        genre_id,
                        title,
                        age_rating,
                        quality
                    FROM [dbo].[Media]
                    WHERE media_id = @MediaId;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[UpdateMediaById]
                    @MediaId INT,
                    @GenreId INT = NULL,
                    @Title VARCHAR(255) = NULL,
                    @AgeRating VARCHAR(10) = NULL,
                    @Quality VARCHAR(10) = NULL
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE [dbo].[Media]
                    SET 
                        genre_id = ISNULL(@GenreId, genre_id),
                        title = ISNULL(@Title, title),
                        age_rating = ISNULL(@AgeRating, age_rating),
                        quality = ISNULL(@Quality, quality)
                    WHERE media_id = @MediaId;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[DeleteMediaById]
                    @MediaId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DELETE FROM [dbo].[Media]
                    WHERE media_id = @MediaId;
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[CreateMedia];");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[GetAllMedia];");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[GetMediaById];");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[UpdateMediaById];");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[DeleteMediaById];");
        }
    }
}
