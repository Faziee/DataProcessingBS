using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Entities;
using DataProcessingBS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class AccountsModule
{
    public static void AddAccountEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/accounts", async ([FromBody] CreateAccountRequest createAccountRequest,
            [FromServices] AppDbcontext dbContext,
            [FromServices] ApiKeyService apiKeyService) =>
        {
            var account = new Account
            {
                Email = createAccountRequest.Email,
                Password = createAccountRequest.Password,
                Payment_Method = createAccountRequest.Payment_Method,
                Blocked = createAccountRequest.Blocked,
                Is_Invited = createAccountRequest.Is_Invited,
                Trial_End_Date = createAccountRequest.Trial_End_Date
            };

            await dbContext.Accounts.AddAsync(account);
            await dbContext.SaveChangesAsync();

            var apiKey = await apiKeyService.CreateApiKeyAsync(account.Account_Id);

            return Results.Ok(new { Account = account, ApiKey = apiKey });
        });

        app.MapGet("/accounts", async (AppDbcontext dbContext) =>
        {
            var accounts = await dbContext.Accounts.ToListAsync();
            return Results.Ok(accounts);
        });

        app.MapGet("/accounts/{email}", async (string email, [FromServices] AppDbcontext dbContext) =>
        {
            var account = await dbContext.Accounts
                .FirstOrDefaultAsync(x => x.Email == email);

            return account == null
                ? Results.NotFound()
                : Results.Ok(account);
        });

        app.MapPut("/accounts/{accountId}", async (int accountId, [FromBody] UpdateAccountRequest updateAccountRequest,
            [FromServices] AppDbcontext dbContext) =>
        {
            var account = await dbContext.Accounts.FirstOrDefaultAsync(x => x.Account_Id == accountId);

            if (account != null)
            {
                account.Email = updateAccountRequest.Email;
                account.Password = updateAccountRequest.Password;
                account.Payment_Method = updateAccountRequest.Payment_Method;

                await dbContext.SaveChangesAsync();
                return Results.Ok(account);
            }

            return Results.NotFound();
        });

        app.MapDelete("/accounts/{accountId}", async (int accountId, AppDbcontext dbContext) =>
        {
            var account = await dbContext.Accounts.FirstOrDefaultAsync(x => x.Account_Id == accountId);

            if (account != null)
            {
                dbContext.Accounts.Remove(account);
                await dbContext.SaveChangesAsync();
                return Results.Ok();
            }

            return Results.NotFound();
        });
    }
}