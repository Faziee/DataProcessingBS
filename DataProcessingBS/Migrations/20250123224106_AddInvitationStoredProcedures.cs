using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessingBS.Migrations
{
    /// <inheritdoc />
    public partial class AddInvitationStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Create CreateInvitation stored procedure
        migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[CreateInvitation]
                @InviterId INT,
                @InviteeId INT
            AS
            BEGIN
                SET NOCOUNT ON;
                INSERT INTO [dbo].[Invitations] (inviter_id, invitee_id)
                VALUES (@InviterId, @InviteeId);
            END;
        ");

        // Create GetAllInvitations stored procedure
        migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[GetAllInvitations]
            AS
            BEGIN
                SET NOCOUNT ON;
                SELECT invitation_id, inviter_id, invitee_id
                FROM [dbo].[Invitations];
            END;
        ");

        // Create GetInvitationById stored procedure
        migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[GetInvitationById]
                @InvitationId INT
            AS
            BEGIN
                SET NOCOUNT ON;
                SELECT invitation_id, inviter_id, invitee_id
                FROM [dbo].[Invitations]
                WHERE invitation_id = @InvitationId;
            END;
        ");

        // Create UpdateInvitationById stored procedure
        migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[UpdateInvitationById]
                @InvitationId INT,
                @InviterId INT = NULL,
                @InviteeId INT = NULL
            AS
            BEGIN
                SET NOCOUNT ON;
                UPDATE [dbo].[Invitations]
                SET 
                    inviter_id = ISNULL(@InviterId, inviter_id),
                    invitee_id = ISNULL(@InviteeId, invitee_id)
                WHERE invitation_id = @InvitationId;
            END;
        ");

        // Create DeleteInvitationById stored procedure
        migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[DeleteInvitationById]
                @InvitationId INT
            AS
            BEGIN
                SET NOCOUNT ON;
                DELETE FROM [dbo].[Invitations]
                WHERE invitation_id = @InvitationId;
            END;
        ");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Drop the stored procedures if rolling back migration
        migrationBuilder.Sql("DROP PROCEDURE [dbo].[CreateInvitation];");
        migrationBuilder.Sql("DROP PROCEDURE [dbo].[GetAllInvitations];");
        migrationBuilder.Sql("DROP PROCEDURE [dbo].[GetInvitationById];");
        migrationBuilder.Sql("DROP PROCEDURE [dbo].[UpdateInvitationById];");
        migrationBuilder.Sql("DROP PROCEDURE [dbo].[DeleteInvitationById];");
    }
    }
}
