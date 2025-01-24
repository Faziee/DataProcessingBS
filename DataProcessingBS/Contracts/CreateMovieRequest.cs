namespace DataProcessingBS.Contracts;

public record CreateMovieRequest(int Genre_Id, string Title, string Age_Rating, string Quality, bool? Has_Subtitles);