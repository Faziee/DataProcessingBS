namespace DataProcessingBS.Contracts;

public record CreateWatchRequest(int Profile_Id, int Media_Id, string Status, DateTime? Pause_Time);