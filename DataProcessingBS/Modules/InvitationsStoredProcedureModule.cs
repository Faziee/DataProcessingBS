using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class InvitationsStoredProcedureModule
{
    public static void AddInvitationsStoredProcedureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/stored-procedure-create-invitation",
            async ([FromBody] CreateInvitationRequest createInvitationRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC CreateInvitation @InviterId={createInvitationRequest.Inviter_Id}, @InviteeId={createInvitationRequest.Invitee_Id}");
                return Results.Ok();
            });

        app.MapGet("/stored-procedure-get-invitations", async (AppDbcontext dbContext) =>
        {
            var invitations = await dbContext.Invitations.FromSqlRaw("EXEC GetAllInvitations").ToListAsync();
            return Results.Ok(invitations);
        });

        app.MapGet("/stored-procedure-get-invitation-by-id/{invitationId:int}",
            async (int invitationId, [FromServices] AppDbcontext dbContext) =>
            {
                var invitation = await dbContext.Invitations
                    .FromSqlInterpolated($"EXEC GetInvitationById @InvitationId={invitationId}")
                    .ToListAsync()
                    .ContinueWith(task => Enumerable.Select(task.Result, i => new InvitationDto
                    {
                        Invitation_Id = i.Invitation_Id,
                        Inviter_Id = i.Inviter_Id,
                        Invitee_Id = i.Invitee_Id
                    }).FirstOrDefault());

                return invitation == null
                    ? Results.NotFound()
                    : Results.Ok(invitation);
            });

        app.MapPut("/stored-procedure-update-invitation-by-id",
            async ([FromBody] UpdateInvitationRequest updateInvitationRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC UpdateInvitationById @InvitationId={updateInvitationRequest.Invitation_Id}, @InviterId={updateInvitationRequest.Inviter_Id}, @InviteeId={updateInvitationRequest.Invitee_Id}");
                return Results.Ok();
            });

        app.MapDelete("/stored-procedure-delete-invitation-by-id/{invitationId}",
            async (int invitationId, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC DeleteInvitationById @InvitationId={invitationId}");
                return Results.Ok();
            });
    }
}