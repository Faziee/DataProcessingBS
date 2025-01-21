using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class AddNewCreateMediaStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[CreateNewMedia]
                @GenreId INT,
                @Title VARCHAR(255),
                @AgeRating VARCHAR(10),
                @Quality VARCHAR(10),
                @MediaId INT OUTPUT
            AS
            BEGIN
                SET NOCOUNT ON;

                -- Insert into the Media table
                INSERT INTO [dbo].[Media] (genre_id, title, age_rating, quality)
                VALUES (@GenreId, @Title, @AgeRating, @Quality);

                -- Get the auto-generated Media_Id
                SET @MediaId = SCOPE_IDENTITY();
            END;
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE [dbo].[CreateNewMedia];");
        }
    }
}