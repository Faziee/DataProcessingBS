namespace DataProcessingBS.Contracts;

public class MovieWithMediaTitleDto
{
    public int Movie_Id { get; set; }
    public int Media_Id { get; set; }
    public bool? Has_Subtitles { get; set; }
    public string Title { get; set; }
}