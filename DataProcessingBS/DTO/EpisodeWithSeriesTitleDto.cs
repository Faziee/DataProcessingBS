using System.ComponentModel.DataAnnotations.Schema;

namespace DataProcessingBS.Contracts;

public class EpisodeWithSeriesTitleDto
{
    public string SeriesTitle { get; set; }

    [Column("Episode_Id")] 
    public int EpisodeId { get; set; }
    public int Series_Id { get; set; }
    public string Title { get; set; }
    public int Duration { get; set; }

    [Column("Season_Number")] 
    public int SeasonNumber { get; set; }

    [Column("Episode_Number")] 
    public int EpisodeNumber { get; set; }
}