using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class GetAccountByEmailStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createProcedure = @"
                CREATE PROCEDURE [dbo].[GetAccountByEmail]
                    @Email NVARCHAR(255)
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT 
                        account_id,
                        email,
                        payment_method,
                        blocked,
                        is_invited,
                        trial_end_date
                    FROM [dbo].[Accounts]
                    WHERE email = @Email;
                END;
            ";
            migrationBuilder.Sql(createProcedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetAccountByEmail]");
        }
    }
}
