namespace DataProcessingBS.Contracts;

public class WatchDto
{
    public int Watch_Id { get; set; }
    public int Profile_Id { get; set; }
    public int Media_Id { get; set; }
    public DateOnly Watch_Date { get; set; }
    public string Status { get; set; }
    public DateTime? Pause_Time { get; set; }
}