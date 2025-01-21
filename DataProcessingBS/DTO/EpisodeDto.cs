namespace DataProcessingBS.Contracts;

public class EpisodeDto
{
    public int Episode_Id { get; set; }
    public int Media_Id { get; set; }
    public int Series_Id { get; set; }
    public int Season_Number { get; set; }
    public int Episode_Number { get; set; }
    public string Title { get; set; }
    public int? Duration { get; set; }

}