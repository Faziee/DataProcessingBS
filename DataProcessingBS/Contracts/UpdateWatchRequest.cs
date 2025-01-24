namespace DataProcessingBS.Contracts;

public record UpdateWatchRequest(int Watch_Id, int Profile_Id, int Media_Id, DateOnly Watch_Date, string Status, DateTime? Pause_Time);