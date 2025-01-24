namespace DataProcessingBS.Contracts;

public class MediaDto
{
    public int Media_Id { get; set; }
    public int Genre_Id { get; set; }
    public string Title { get; set; }
    public string? Age_Rating { get; set; }
    public string? Quality { get; set; }
}