using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    public partial class GetApiKeySLAYYYStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE CreateApiKey
                    @accountId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @apiKey NVARCHAR(50);

                    SET @apiKey = NEWID();  

                    INSERT INTO ApiKeys ([Key], Account_Id, Create_Date, Is_Active)
                    VALUES (@apiKey, @accountId, GETDATE(), 1);

                    SELECT @apiKey AS ApiKey;
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CreateApiKey;");
        }
    }
}