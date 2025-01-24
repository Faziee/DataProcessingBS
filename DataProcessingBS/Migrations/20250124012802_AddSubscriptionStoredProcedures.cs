using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[CreateSubscription]
                    @AccountId INT,
                    @SubscriptionPrice DECIMAL(5, 2),
                    @Type VARCHAR(10),
                    @StartDate DATE,
                    @RenewalDate DATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    -- Default StartDate to GETDATE() if NULL
                    IF @StartDate IS NULL
                        SET @StartDate = GETDATE();

                    INSERT INTO [dbo].[Subscriptions] (account_id, subscription_price, type, start_date, renewal_date)
                    VALUES (@AccountId, @SubscriptionPrice, @Type, @StartDate, @RenewalDate);
                END;
            ");

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
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[GetSubscriptionById]
                    @SubscriptionId INT
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
                    WHERE subscription_id = @SubscriptionId;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[UpdateSubscriptionById]
                    @SubscriptionId INT,
                    @AccountId INT = NULL,
                    @SubscriptionPrice DECIMAL(5, 2) = NULL,
                    @Type VARCHAR(10) = NULL,
                    @StartDate DATE = NULL,
                    @RenewalDate DATE = NULL
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE [dbo].[Subscriptions]
                    SET 
                        account_id = ISNULL(@AccountId, account_id),
                        subscription_price = ISNULL(@SubscriptionPrice, subscription_price),
                        type = ISNULL(@Type, type),
                        start_date = ISNULL(@StartDate, start_date),
                        renewal_date = ISNULL(@RenewalDate, renewal_date)
                    WHERE subscription_id = @SubscriptionId;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[DeleteSubscriptionById]
                    @SubscriptionId INT
                AS
                BEGIN
                    SET NOCOUNT ON;
                    DELETE FROM [dbo].[Subscriptions]
                    WHERE subscription_id = @SubscriptionId;
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[CreateSubscription];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetAllSubscriptions];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetSubscriptionById];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[UpdateSubscriptionById];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[DeleteSubscriptionById];");
        }
    }
}
