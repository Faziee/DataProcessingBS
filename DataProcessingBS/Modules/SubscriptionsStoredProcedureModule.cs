using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class SubscriptionsStoredProcedureModule
{
    public static void AddSubscriptionStoredProcedureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/stored-procedure-create-subscription",
            async ([FromBody] CreateSubscriptionRequest createSubscriptionRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC CreateSubscription @AccountId={createSubscriptionRequest.Account_Id}, @SubscriptionPrice={createSubscriptionRequest.Subscription_Price}, @Type={createSubscriptionRequest.Type}, @StartDate={createSubscriptionRequest.Start_Date}, @RenewalDate={createSubscriptionRequest.Renewal_Date}");
                return Results.Ok();
            });

        app.MapPut("/stored-procedure-update-subscription-by-id",
            async ([FromBody] UpdateSubscriptionRequest updateSubscriptionRequest, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC UpdateSubscriptionById @SubscriptionId={updateSubscriptionRequest.Subscription_Id}, @SubscriptionPrice={updateSubscriptionRequest.Subscription_Price}, @Type={updateSubscriptionRequest.Type}, @StartDate={updateSubscriptionRequest.Start_Date}, @RenewalDate={updateSubscriptionRequest.Renewal_Date}");
                return Results.Ok();
            });
        
        app.MapGet("/stored-procedure-get-subscriptions", async (AppDbcontext dbContext) =>
        {
            var subscriptions = await dbContext.Set<SubscriptionDto>()
                .FromSqlRaw("EXEC GetAllSubscriptions")
                .ToListAsync();

            return Results.Ok(subscriptions);
        });

        app.MapGet("/stored-procedure-get-subscription-by-id/{subscriptionId:int}", async ([FromServices] AppDbcontext dbContext, int subscriptionId) =>
        {
            var subscriptions = await dbContext.Set<SubscriptionDto>()
                .FromSqlRaw("EXEC GetSubscriptionById @Subscription_Id = {0}", subscriptionId)
                .ToListAsync();

            var subscription = subscriptions.FirstOrDefault();

            if (subscription == null)
            {
                return Results.NotFound(new { Message = $"Subscription with ID {subscriptionId} not found." });
            }

            return Results.Ok(subscription);
        });


        

        app.MapDelete("/stored-procedure-delete-subscription-by-id/{subscriptionId}", async (int subscriptionId, [FromServices] AppDbcontext dbContext) =>
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteSubscriptionById @SubscriptionId={subscriptionId}");
            return Results.Ok();
        });
    }
}