namespace DataProcessingBS.Contracts;

public record UpdateMediaRequest(int Media_Id, int Genre_Id, string Title, string? Age_Rating, string? Quality);