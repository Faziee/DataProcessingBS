using System.ComponentModel.DataAnnotations;

namespace DataProcessingBS.Entities;

public class Genre
{
    [Key]
    public int Genre_Id { get; set; }
    public string Type { get; set; }
    
    public ICollection<Media> Media { get; set; }
}