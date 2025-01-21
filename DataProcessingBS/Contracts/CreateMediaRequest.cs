namespace DataProcessingBS.Contracts;

public record CreateMediaRequest(int Genre_Id, string Title, string? Age_Rating, string? Quality);
