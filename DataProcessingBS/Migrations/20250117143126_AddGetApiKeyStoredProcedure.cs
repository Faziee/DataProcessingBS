using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    public partial class AddGetApiKeyStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create stored procedure for API key validation
            migrationBuilder.Sql(@"
            CREATE PROCEDURE GetApiKeyByKey
                @ApiKey NVARCHAR(255)
            AS
            BEGIN
                SELECT 
                    Apikey_Id,
                    [key],
                    Account_Id,
                    Create_Date,
                    Is_Active
                FROM 
                    ApiKeys
                WHERE 
                    [key] = @ApiKey;
            END;
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the stored procedure if we rollback the migration
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetApiKeyByKey;");
        }
    }
}
