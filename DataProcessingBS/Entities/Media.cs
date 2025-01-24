using System.ComponentModel.DataAnnotations;

namespace DataProcessingBS.Entities;

public class Media
{
    [Key]
    public int Media_Id { get; set; }
    public int Genre_Id { get; set; }
    public string Title { get; set; }
    public string? Age_Rating { get; set; }
    public string? Quality { get; set; }
    
    public Genre Genre { get; set; }
}