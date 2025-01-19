using System.ComponentModel.DataAnnotations;

namespace DataProcessingBS.Entities;

public class Profile
{
    [Key]
    public int Profile_Id { get; set; }
    
    public int Account_Id { get; set; }
    public string? Profile_Image { get; set; }
    public bool Child_Profile { get; set; }
    public int? User_Age { get; set; }
    public string? Language { get; set; }
    
    public Account? Account { get; set; }
    
}