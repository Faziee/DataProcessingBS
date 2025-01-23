namespace DataProcessingBS.Contracts;

public record UpdateEpisodeRequest(int Episode_Id, int Media_Id, int Series_Id, int Season_Number, int Episode_Number, string Title, int? Duration);