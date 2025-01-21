namespace DataProcessingBS.Contracts;

//public record CreateEpisodeRequest(int Media_Id, int Series_Id, int Season_Number, int Episode_Number, string Title, int? Duration);

/*
public record CreateEpisodeRequest(
    int Media_Id,
    int Series_Id,      
    int Season_Number,
    int Episode_Number,
    string Title,
    int? Duration
);
*/
public record CreateEpisodeRequest(
    string Media_Title,   // Title for the new Media
    int Genre_Id,         // Genre for the new Media
    string? Age_Rating,   // Optional Age Rating for the new Media
    string? Quality,      // Optional Quality for the new Media
    int Series_Id,        // Existing Series Id, or 0 for new Series
    string Series_Title,  // Only used when creating a new Series
    int Season_Number,    // Season number for the episode
    int Episode_Number,   // Episode number for the episode
    string Title,         // Title for the episode
    int? Duration   
);