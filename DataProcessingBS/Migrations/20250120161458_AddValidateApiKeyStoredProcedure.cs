using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class AddValidateApiKeyStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE PROCEDURE ValidateApiKey
                @apiKey NVARCHAR(100)
            AS
            BEGIN
                SELECT CAST(
                    CASE 
                        WHEN EXISTS (
                            SELECT 1 
                            FROM ApiKeys 
                            WHERE [Key] = @apiKey 
                            AND Is_Active = 1
                        ) 
                        THEN 1 
                        ELSE 0 
                    END AS BIT
                ) AS IsValid;
            END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE ValidateApiKey");
        }
    }
}
