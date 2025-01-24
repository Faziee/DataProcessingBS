using System.ComponentModel.DataAnnotations;

namespace DataProcessingBS.Entities;

public class Subscription
{
    [Key] 
    public int Subscription_Id { get; set; }

    public int Account_Id { get; set; }
    public decimal? Subscription_Price { get; set; }
    public string? Type { get; set; }
    public DateOnly Start_Date { get; set; }
    public DateOnly Renewal_Date { get; set; }

    public Account? Account { get; set; }
}