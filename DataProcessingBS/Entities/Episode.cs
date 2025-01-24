using System.ComponentModel.DataAnnotations;

namespace DataProcessingBS.Entities;

public class Episode
{
    [Key] 
    public int Episode_Id { get; set; }

    public int Media_Id { get; set; }
    public int Series_Id { get; set; }
    public int Season_Number { get; set; }
    public int Episode_Number { get; set; }
    public string Title { get; set; }
    public int? Duration { get; set; }
}