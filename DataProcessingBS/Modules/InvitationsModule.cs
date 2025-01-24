using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class InvitationsModule
{
    public static void AddInvitationEndpoints(this IEndpointRouteBuilder app)
    {
        // Create Invitation
        app.MapPost("/invitations",
            async ([FromBody] CreateInvitationRequest createInvitationRequest, [FromServices] AppDbcontext dbContext) =>
            {
                var invitation = new Invitation()
                {
                    Inviter_Id = createInvitationRequest.Inviter_Id,
                    Invitee_Id = createInvitationRequest.Invitee_Id
                };

                await dbContext.Invitations.AddAsync(invitation);
                await dbContext.SaveChangesAsync();
                return Results.Ok(invitation);
            });
        
        app.MapPut("/invitations/{invitationId}", async (int invitationId, [FromBody] UpdateInvitationRequest updateInvitationRequest, [FromServices] AppDbcontext dbContext) =>
        {
            var invitation = await dbContext.Invitations.FirstOrDefaultAsync(x => x.Invitation_Id == invitationId);

            if (invitation != null)
            {
                // Only update if the new value is not null
                if (updateInvitationRequest.Inviter_Id.HasValue)
                {
                    invitation.Inviter_Id = updateInvitationRequest.Inviter_Id.Value;
                }

                if (updateInvitationRequest.Invitee_Id.HasValue)
                {
                    invitation.Invitee_Id = updateInvitationRequest.Invitee_Id.Value;
                }

                await dbContext.SaveChangesAsync();
                return Results.Ok(invitation);
            }
            else
            {
                return Results.NotFound();
            }
        });


        // Get All Invitations
        app.MapGet("/invitations", async (AppDbcontext dbContext) =>
        {
            var invitations = await dbContext.Invitations.ToListAsync();
            return Results.Ok(invitations);
        });
        
        // Delete Invitation by ID
        app.MapDelete("/invitations/{id:int}", async (int id, [FromServices] AppDbcontext dbContext) =>
        {
            var invitation = await dbContext.Invitations.FirstOrDefaultAsync(s => s.Invitation_Id == id);

            if (invitation == null)
                return Results.NotFound("Invitation not found.");

            dbContext.Invitations.Remove(invitation);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new { Message = "Invitation deleted successfully." });
        });
    }


}