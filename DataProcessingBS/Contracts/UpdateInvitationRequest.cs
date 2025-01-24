namespace DataProcessingBS.Contracts;

public record UpdateInvitationRequest(int Invitation_Id, int? Inviter_Id, int? Invitee_Id);