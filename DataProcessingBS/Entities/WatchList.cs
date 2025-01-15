using System.ComponentModel.DataAnnotations;

namespace DataProcessingBS.Entities;

public class WatchList
{
    [Key]
    public int WatchList_Id { get; set; }
    
    public int Profile_Id { get; set; }
    public int Media_Id { get; set; }
    public DateOnly Added_Date { get; set; }
}