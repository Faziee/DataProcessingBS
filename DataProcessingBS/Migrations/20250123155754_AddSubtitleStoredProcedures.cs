using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    public partial class AddSubtitleStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // CreateSubtitle Stored Procedure
            migrationBuilder.Sql(@"
                CREATE PROCEDURE CreateSubtitle
                    @GMedia_id INT,
                    @Language NVARCHAR(50)
                AS
                BEGIN
                    SET NOCOUNT ON;

                    INSERT INTO Subtitles (Media_Id, Language)
                    VALUES (@GMedia_id, @Language);
                END
            ");

            // GetAllSubtitles Stored Procedure
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetAllSubtitles
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT Subtitle_Id, Media_Id, Language
                    FROM Subtitles;
                END
            ");

            // GetSubtitleById Stored Procedure
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetSubtitleById
                    @Subtitle_Id INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT Subtitle_Id, Media_Id, Language
                    FROM Subtitles
                    WHERE Subtitle_Id = @Subtitle_Id;
                END
            ");

            // DeleteSubtitleById Stored Procedure
            migrationBuilder.Sql(@"
                CREATE PROCEDURE DeleteSubtitleById
                    @Subtitle_Id INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DELETE FROM Subtitles
                    WHERE Subtitle_Id = @Subtitle_Id;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop procedures if they exist
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CreateSubtitle");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAllSubtitles");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetSubtitleById");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS DeleteSubtitleById");
        }
    }
}
