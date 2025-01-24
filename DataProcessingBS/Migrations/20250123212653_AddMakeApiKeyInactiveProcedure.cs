using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class AddMakeApiKeyInactiveProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the stored procedure for making API key inactive
            migrationBuilder.Sql(@"
            CREATE PROCEDURE UpdateApiKeyStatus
                @ApiKeyId INT
            AS
            BEGIN
                UPDATE ApiKeys
                SET is_active = 0
                WHERE apikey_id = @ApiKeyId;
            END;
            GO
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the stored procedure if migration is rolled back
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UpdateApiKeyStatus;");
        }
    }
}
