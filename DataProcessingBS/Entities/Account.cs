using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataProcessingBS.Entities;

public class Account
{
    [Key]
    [Column("account_id")]
    public int Account_Id { get; set; }
    
    public string Email { get; set; }
    public string Password { get; set; }
    public string Payment_Method { get; set; }
    public bool? Blocked { get; set; }
    public bool? Is_Invited { get; set; }
    public DateOnly? Trial_End_Date { get; set; }
    
    public List<Profile>? Profiles { get; set; } 

}