namespace DataProcessingBS.Contracts;

public record UpdateProfileRequest (int Profile_Id, int Account_Id, string? Profile_Image, bool Child_Profile, int? User_Age,string? Language);