using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class NewUpdateGetAllMoviesStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetAllMovies
                AS
                BEGIN
                    SELECT 
                        m.Movie_Id AS Movie_Id,
                        m.Media_Id AS Media_Id,
                        m.Has_Subtitles AS Has_Subtitles,
                        me.Title AS Title
                    FROM 
                        Movies m
                    INNER JOIN 
                        Media me ON m.Media_Id = me.Media_Id;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAllMovies");
        }
    }
}