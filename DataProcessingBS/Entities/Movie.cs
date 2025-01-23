using System.ComponentModel.DataAnnotations;

namespace DataProcessingBS.Entities;

public class Movie
{
    [Key]
    public int Movie_Id { get; set; }
    
    public int Media_Id { get; set; }
    public bool? Has_Subtitles { get; set; }
    
    public Media Media { get; set; }
}      