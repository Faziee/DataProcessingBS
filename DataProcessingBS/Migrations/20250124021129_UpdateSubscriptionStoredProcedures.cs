using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubscriptionStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Creating stored procedures
            migrationBuilder.Sql(@"
        CREATE PROCEDURE [dbo].[GetAllSubscriptions]
        AS
        BEGIN
            SET NOCOUNT ON;
            SELECT 
                subscription_id,
                account_id,
                subscription_price,
                type,
                start_date,
                renewal_date
            FROM [dbo].[Subscriptions];
        END;");

            migrationBuilder.Sql(@"
        CREATE PROCEDURE [dbo].[GetSubscriptionById]
            @Subscription_Id INT
        AS
        BEGIN
            SET NOCOUNT ON;
            SELECT 
                subscription_id,
                account_id,
                subscription_price,
                type,
                start_date,
                renewal_date
            FROM [dbo].[Subscriptions]
            WHERE subscription_id = @Subscription_Id;
        END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop stored procedures if rolling back migration
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetAllSubscriptions]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetSubscriptionById]");
        }

    }
}
