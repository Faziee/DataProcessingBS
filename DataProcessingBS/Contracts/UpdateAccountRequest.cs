namespace DataProcessingBS.Contracts;

public record UpdateAccountRequest(int Account_Id, string Email, string Password, string Payment_Method);
