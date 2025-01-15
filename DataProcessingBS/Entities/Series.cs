using System.ComponentModel.DataAnnotations;

namespace DataProcessingBS.Entities;

public class Series
{
    [Key]
    public int Series_Id { get; set; }
    
    public int Genre_Id { get; set; }
    public string Title { get; set; }
    public string? Age_Rating { get; set; }
}