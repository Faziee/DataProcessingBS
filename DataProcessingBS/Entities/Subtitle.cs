using System.ComponentModel.DataAnnotations;

namespace DataProcessingBS.Entities;

public class Subtitle
{
    [Key] 
    public int Subtitle_Id { get; set; }

    public int Media_Id { get; set; }
    public string Language { get; set; }
}