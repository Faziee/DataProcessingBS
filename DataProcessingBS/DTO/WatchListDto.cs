namespace DataProcessingBS.Contracts;

public class WatchListDto
{
    public int WatchList_Id { get; set; }
    public int Profile_Id { get; set; }
    public int Media_Id { get; set; }
    public DateOnly Added_Date { get; set; }
}