using System.ComponentModel.DataAnnotations;

namespace DataProcessingBS.Entities;

public class Account
{
    [Key]
    public int Account_Id { get; set; }
    
    public string Email { get; set; }
    public string Password { get; set; }
    public string Payment_Method { get; set; }
    public string Blocked { get; set; }
    public string Is_Invited { get; set; }
    public DateOnly? Trial_End_Date { get; set; }
    
    public ICollection<Profile>? Profiles { get; set; } 
    //do we validate for 4 idk? 
}