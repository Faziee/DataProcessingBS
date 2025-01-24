using System.ComponentModel.DataAnnotations;

namespace DataProcessingBS.Entities;

public class Invitation
{
    [Key] 
    public int Invitation_Id { get; set; }

    public int Inviter_Id { get; set; }
    public int Invitee_Id { get; set; }
}