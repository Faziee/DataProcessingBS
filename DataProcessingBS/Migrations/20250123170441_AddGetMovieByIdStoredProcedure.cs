using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class AddGetMovieByIdStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

                CREATE PROCEDURE [dbo].[GetMovieById]
                    @MovieId INT
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
                        Media me ON m.Media_Id = me.Media_Id
                    WHERE 
                        m.Movie_Id = @MovieId;
                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[GetMovieById]");
        }
    }
}
