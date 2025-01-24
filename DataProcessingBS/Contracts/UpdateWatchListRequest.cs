namespace DataProcessingBS.Contracts;

public record UpdateWatchListRequest(int WatchList_Id, int Profile_Id, int Media_Id, DateOnly AddedDate);