namespace DataProcessingBS.Contracts;

public record CreateEpisodeRequest(string Media_Title, int Genre_Id, string? Age_Rating, string? Quality, int Series_Id, string Series_Title, int Season_Number, int Episode_Number, string Title, int? Duration);