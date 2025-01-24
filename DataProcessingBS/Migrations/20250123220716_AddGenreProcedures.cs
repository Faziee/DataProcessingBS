using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class AddGenreProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Create the stored procedure for inserting a new genre
        migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[CreateGenre]
                @Type VARCHAR(50)
            AS
            BEGIN
                SET NOCOUNT ON;
                INSERT INTO [dbo].[Genres] (type)
                VALUES (@Type);
            END;
            GO
        ");

        // Create the stored procedure to get all genres
        migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[GetAllGenres]
            AS
            BEGIN
                SET NOCOUNT ON;
                SELECT 
                    genre_id,
                    type
                FROM [dbo].[Genres];
            END;
            GO
        ");

        // Create the stored procedure to get a genre by ID
        migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[GetGenreById]
                @GenreId INT
            AS
            BEGIN
                SET NOCOUNT ON;
                SELECT 
                    genre_id,
                    type
                FROM [dbo].[Genres]
                WHERE genre_id = @GenreId;
            END;
            GO
        ");

        // Create the stored procedure to update a genre by ID
        migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[UpdateGenreById]
                @GenreId INT,
                @Type VARCHAR(50) = NULL
            AS
            BEGIN
                SET NOCOUNT ON;
                UPDATE [dbo].[Genres]
                SET 
                    type = ISNULL(@Type, type)
                WHERE genre_id = @GenreId;
            END;
            GO
        ");

        // Create the stored procedure to delete a genre by ID
        migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[DeleteGenreById]
                @GenreId INT
            AS
            BEGIN
                SET NOCOUNT ON;
                DELETE FROM [dbo].[Genres]
                WHERE genre_id = @GenreId;
            END;
            GO
        ");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Drop all the stored procedures if migration is rolled back
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[CreateGenre];");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetAllGenres];");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetGenreById];");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[UpdateGenreById];");
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[DeleteGenreById];");
    }
    }
}
