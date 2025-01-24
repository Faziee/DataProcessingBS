namespace DataProcessingBS.Contracts;

public record CreateSubscriptionRequest(int Account_Id, decimal? Subscription_Price, string? Type, DateOnly Start_Date, DateOnly Renewal_Date);