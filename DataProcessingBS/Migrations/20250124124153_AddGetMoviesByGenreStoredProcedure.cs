using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    public partial class AddGetMoviesByGenreStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetMoviesByGenre
                    @GenreType NVARCHAR(100)
                AS
                BEGIN
                    SELECT 
                        m.Movie_Id,
                        m.Media_Id,
                        m.Has_Subtitles,
                        me.Title
                    FROM 
                        Movies m
                    INNER JOIN 
                        Media me ON m.Media_Id = me.Media_Id
                    INNER JOIN 
                        Genre g ON me.Genre_Id = g.Genre_Id
                    WHERE 
                        g.Type = @GenreType;
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE GetMoviesByGenre;");
        }
    }
}
