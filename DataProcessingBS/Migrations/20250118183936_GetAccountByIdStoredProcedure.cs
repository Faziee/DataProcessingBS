using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class GetAccountByIdStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createProcedure = @"
                CREATE PROCEDURE [dbo].[GetAccountById]
                    @AccountId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT 
                        account_id,
                        email,
                        password,
                        payment_method,
                        blocked,
                        is_invited,
                        trial_end_date
                    FROM [dbo].[Accounts]
                    WHERE account_id = @AccountId;
                END;
            ";
            migrationBuilder.Sql(createProcedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetAccountById]");
        }
    }
}
