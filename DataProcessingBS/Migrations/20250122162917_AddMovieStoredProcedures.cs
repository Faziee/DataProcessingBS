using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE CreateMovie
                    @MediaId INT,
                    @HasSubtitles BIT
                AS
                BEGIN
                    INSERT INTO Movies (Media_Id, Has_Subtitles)
                    VALUES (@MediaId, @HasSubtitles)
                END
            ");

            // Create stored procedure for updating a Movie
            migrationBuilder.Sql(@"
                CREATE PROCEDURE UpdateMovieById
                    @MovieId INT,
                    @HasSubtitles BIT
                AS
                BEGIN
                    UPDATE Movies
                    SET Has_Subtitles = @HasSubtitles
                    WHERE Movie_Id = @MovieId
                END
            ");

            // Create stored procedure for retrieving all Movies
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetAllMovies
                AS
                BEGIN
                    SELECT 
                        m.Movie_Id,
                        m.Media_Id,
                        m.Has_Subtitles,
                        me.Title -- Add Title from the Media table
                    FROM 
                        Movies m
                    INNER JOIN 
                        Media me ON m.Media_Id = me.Media_Id;
                END
            ");

            // Create stored procedure for retrieving a Movie by ID
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetMovieById
                    @MovieId INT
                AS
                BEGIN
                    SELECT * FROM Movies
                    WHERE Movie_Id = @MovieId
                END
            ");

            // Create stored procedure for deleting a Movie
            migrationBuilder.Sql(@"
                CREATE PROCEDURE DeleteMovieById
                    @MovieId INT
                AS
                BEGIN
                    DELETE FROM Movies
                    WHERE Movie_Id = @MovieId
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CreateMovie");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UpdateMovieById");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAllMovies");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetMovieById");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS DeleteMovieById");
        }
    }
}
