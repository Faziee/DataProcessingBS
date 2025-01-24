using Microsoft.EntityFrameworkCore;
using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Entities;
using DataProcessingBS.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataProcessingBS.Modules
{
    public static class AccountsModule
    {
        public static void AddAccountEndpoints(this IEndpointRouteBuilder app)
        {
            /*// Create Account
            app.MapPost("/accounts", async ([FromBody] CreateAccountRequest createAccountRequest, [FromServices] AppDbcontext dbContext) =>
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
                return Results.Ok(account);
            });*/
            
            // // Create Account
            app.MapPost("/accounts", async ([FromBody] CreateAccountRequest createAccountRequest, 
                [FromServices] AppDbcontext dbContext,
                [FromServices] ApiKeyService apiKeyService) =>
            {
                // Create the new account
                var account = new Account
                {
                    Email = createAccountRequest.Email,
                    Password = createAccountRequest.Password,
                    Payment_Method = createAccountRequest.Payment_Method,
                    Blocked = createAccountRequest.Blocked,
                    Is_Invited = createAccountRequest.Is_Invited,
                    Trial_End_Date = createAccountRequest.Trial_End_Date
                };
            
                // Save account to the database
                await dbContext.Accounts.AddAsync(account);
                await dbContext.SaveChangesAsync();
            
                // Generate and store API key
                var apiKey = await apiKeyService.CreateApiKeyAsync(account.Account_Id);
            
                // Return the account and generated API key
                return Results.Ok(new { Account = account, ApiKey = apiKey });
            });
            
            
            // Update Account by Account_Id
            app.MapPut("/accounts/{accountId}", async (int accountId,[FromBody] UpdateAccountRequest updateAccountRequest, [FromServices] AppDbcontext dbContext) =>
            {
                // Fetch the account by its Account_Id
                var account = await dbContext.Accounts.FirstOrDefaultAsync(x => x.Account_Id == accountId);

                if (account != null)
                {
                    // Update the account details
                    account.Email = updateAccountRequest.Email;
                    account.Password = updateAccountRequest.Password;
                    account.Payment_Method = updateAccountRequest.Payment_Method;

                    // Save changes to the database
                    await dbContext.SaveChangesAsync();
                    return Results.Ok(account);  // Return 200 OK with the updated account
                }
                else
                {
                    return Results.NotFound();  // Return 404 if the account is not found
                }
            });
            
            // Get All Accounts
            app.MapGet("/accounts", async (AppDbcontext dbContext) =>
            {
                var accounts = await dbContext.Accounts.ToListAsync();
                return Results.Ok(accounts);  // Return 200 OK with the list of accounts
            });
            
            // Get Account by Account_Email
            app.MapGet("/accounts/{email}", async (string email, [FromServices] AppDbcontext dbContext) =>
            {
                var account = await dbContext.Accounts
                    .FirstOrDefaultAsync(x => x.Email == email);

                return account == null
                    ? Results.NotFound()
                    : Results.Ok(account);
            });
            
            // Delete Account by Account_Id
            app.MapDelete("/accounts/{accountId}", async (int accountId, AppDbcontext dbContext) =>
            {
                // Find the account by Account_Id
                var account = await dbContext.Accounts.FirstOrDefaultAsync(x => x.Account_Id == accountId);

                if (account != null)
                {
                    // Remove the account from the database
                    dbContext.Accounts.Remove(account);
                    await dbContext.SaveChangesAsync();
                    return Results.Ok();  // Return 200 OK after successful deletion
                }
                else
                {
                    return Results.NotFound();  // Return 404 if the account is not found
                }
            });
        }
    }
}
