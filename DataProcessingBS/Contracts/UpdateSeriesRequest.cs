namespace DataProcessingBS.Contracts;

public record UpdateSeriesRequest(int Series_Id, int Genre_Id, string Title, string? Age_Rating);