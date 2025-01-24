namespace DataProcessingBS.Contracts;

public record CreateAccountRequest(string Email, string Password, string Payment_Method, bool? Blocked, bool? Is_Invited, DateOnly? Trial_End_Date, DateOnly Renewal_Date);

