using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class SubscriptionModule
{
    public static void AddSubscriptionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/subscriptions", async ([FromBody] CreateSubscriptionRequest createSubscriptionRequest, [FromServices] AppDbcontext dbContext) =>
        {
            var subscription = new Subscription()
            {
                Account_Id = createSubscriptionRequest.Account_Id,
                Subscription_Price = createSubscriptionRequest.Subscription_Price,
                Type = createSubscriptionRequest.Type,
                Start_Date = createSubscriptionRequest.Start_Date, // Nullable DateOnly
                Renewal_Date = createSubscriptionRequest.Renewal_Date // Nullable DateOnly
            };

            await dbContext.Subscriptions.AddAsync(subscription);
            await dbContext.SaveChangesAsync();
            return Results.Ok(subscription);
        });

        app.MapPut("/subscriptions/{subscriptionId}", async (int subscriptionId, [FromBody] UpdateSubscriptionRequest updateSubscriptionRequest, [FromServices] AppDbcontext dbContext) =>
        {
            var subscription = await dbContext.Subscriptions.FirstOrDefaultAsync(s => s.Subscription_Id == subscriptionId);

            if (subscription == null) return Results.NotFound();
            
            subscription.Subscription_Price = updateSubscriptionRequest.Subscription_Price;
            subscription.Type = updateSubscriptionRequest.Type;
            subscription.Start_Date = updateSubscriptionRequest.Start_Date;
            subscription.Renewal_Date = updateSubscriptionRequest.Renewal_Date;

            await dbContext.SaveChangesAsync();
            return Results.Ok(subscription);
        });

        app.MapGet("/subscriptions", async (AppDbcontext dbContext) =>
        {
            var subscriptions = await dbContext.Subscriptions.ToListAsync();
            return Results.Ok(subscriptions);
        });

        app.MapGet("/subscriptions/{subscriptionId:int}", async (int subscriptionId, [FromServices] AppDbcontext dbContext) =>
        {
            var subscription = await dbContext.Subscriptions.FindAsync(subscriptionId);
            return subscription == null ? Results.NotFound() : Results.Ok(subscription);
        });

        app.MapDelete("/subscriptions/{subscriptionId:int}", async (int subscriptionId, [FromServices] AppDbcontext dbContext) =>
        {
            var subscription = await dbContext.Subscriptions.FindAsync(subscriptionId);

            if (subscription == null) return Results.NotFound("Subscription not found.");

            dbContext.Subscriptions.Remove(subscription);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new { Message = "Subscription deleted successfully." });
        });
    }
}
