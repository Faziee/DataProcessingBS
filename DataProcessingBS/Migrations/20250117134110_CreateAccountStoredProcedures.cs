using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class CreateAccountStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE PROCEDURE [dbo].[CreateAccount]
            @Email NVARCHAR(MAX),
            @Password NVARCHAR(MAX),
            @Payment_Method NVARCHAR(MAX),
            @Blocked BIT NOT NULL,
            @Is_Invited BIT NOT NULL,
            @TrialEndDate DATETIME2
        AS
        BEGIN
            INSERT INTO Accounts (Email, Password, Payment_Method, Blocked, Is_Invited, Trial_End_Date)
            VALUES (@Email, @Password, @PaymentMethod, @Blocked, @IsInvited, @TrialEndDate)
        END
    ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[CreateAccount]");

        }
    }
}
