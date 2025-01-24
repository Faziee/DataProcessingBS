using System.ComponentModel.DataAnnotations;

namespace DataProcessingBS.Entities;

public class Watch
{
    [Key] 
    public int Watch_Id { get; set; }

    public int Profile_Id { get; set; }
    public int Media_Id { get; set; }
    public DateOnly Watch_Date { get; set; }
    public string Status { get; set; }
    public DateTime? Pause_Time { get; set; }
}