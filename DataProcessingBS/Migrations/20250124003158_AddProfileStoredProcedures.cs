using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[CreateProfile]
                    @AccountId INT,
                    @ProfileImage NVARCHAR(MAX) = 'default.jpg',
                    @ChildProfile BIT,
                    @UserAge INT = NULL,
                    @Language VARCHAR(50) = NULL
                AS
                BEGIN
                    SET NOCOUNT ON;

                    INSERT INTO [dbo].[Profiles] (account_id, profile_image, child_profile, user_age, language)
                    VALUES (@AccountId, @ProfileImage, @ChildProfile, @UserAge, @Language);
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[GetAllProfiles]
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT 
                        profile_id,
                        account_id,
                        profile_image,
                        child_profile,
                        user_age,
                        language
                    FROM [dbo].[Profiles];
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[GetProfileById]
                    @ProfileId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT 
                        profile_id,
                        account_id,
                        profile_image,
                        child_profile,
                        user_age,
                        language
                    FROM [dbo].[Profiles]
                    WHERE profile_id = @ProfileId;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[UpdateProfileById]
                    @ProfileId INT,
                    @AccountId INT = NULL,
                    @ProfileImage NVARCHAR(MAX) = NULL,
                    @ChildProfile BIT = NULL,
                    @UserAge INT = NULL,
                    @Language VARCHAR(50) = NULL
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE [dbo].[Profiles]
                    SET 
                        account_id = ISNULL(@AccountId, account_id),
                        profile_image = ISNULL(@ProfileImage, profile_image),
                        child_profile = ISNULL(@ChildProfile, child_profile),
                        user_age = ISNULL(@UserAge, user_age),
                        language = ISNULL(@Language, language)
                    WHERE profile_id = @ProfileId;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[DeleteProfileById]
                    @ProfileId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DELETE FROM [dbo].[Profiles]
                    WHERE profile_id = @ProfileId;
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[CreateProfile]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetAllProfiles]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetProfileById]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[UpdateProfileById]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[DeleteProfileById]");
        }
    }
}
