namespace DataProcessingBS.Contracts;

public record UpdateSubscriptionRequest(int Subscription_Id, decimal? Subscription_Price, string? Type, DateOnly Start_Date, DateOnly Renewal_Date);
