using System.ComponentModel.DataAnnotations;

namespace DataProcessingBS.Entities;

public class ApiKey
{
    [Key] public int ApiKey_Id { get; set; }

    public string Key { get; set; }
    public int Account_Id { get; set; }
    public DateTime Create_Date { get; set; }
    public bool? Is_Active { get; set; }
}