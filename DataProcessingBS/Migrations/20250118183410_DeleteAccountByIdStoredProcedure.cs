using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class DeleteAccountByIdStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createProcedure = @"
                CREATE PROCEDURE [dbo].[DeleteAccountById]
                    @AccountId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DELETE FROM [dbo].[Accounts]
                    WHERE account_id = @AccountId;
                END;
            ";
            migrationBuilder.Sql(createProcedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[DeleteAccountById]");
        }
    }
}
