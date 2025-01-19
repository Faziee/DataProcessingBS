namespace DataProcessingBS.Contracts;

public record UpdateAccountRequest(int Account_Id, string Email, string Password, string Payment_Method, bool? Blocked, bool? Is_Invited, DateOnly? Trial_End_Date);
