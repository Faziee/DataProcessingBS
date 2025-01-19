using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    public partial class AddUpdateAccountByIdStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createProcedure = @"
                CREATE PROCEDURE [dbo].[UpdateAccountById]
                    @AccountId INT,
                    @Email NVARCHAR(255) = NULL,
                    @Password NVARCHAR(255) = NULL,
                    @PaymentMethod NVARCHAR(100) = NULL,
                    @Blocked BIT = NULL,
                    @IsInvited BIT = NULL,
                    @TrialEndDate DATE = NULL
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE [dbo].[Accounts]
                    SET
                        email = ISNULL(@Email, email),
                        password = ISNULL(@Password, password),
                        payment_method = ISNULL(@PaymentMethod, payment_method),
                        blocked = ISNULL(@Blocked, blocked),
                        is_invited = ISNULL(@IsInvited, is_invited),
                        trial_end_date = ISNULL(@TrialEndDate, trial_end_date)
                    WHERE account_id = @AccountId;
                END;
            ";
            migrationBuilder.Sql(createProcedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[UpdateAccountById]");
        }
    }
}
