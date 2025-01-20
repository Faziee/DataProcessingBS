using Microsoft.EntityFrameworkCore;
using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataProcessingBS.Modules;

public static class AccountsStoredProcedureModule
{
    public static void AddAccountStoredProcedureEndpoints(this IEndpointRouteBuilder app)
        {
            /*// Create Account
            app.MapPost("/stored-procedure-create-account", async ([FromBody] CreateAccountRequest createAccountRequest,
                [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC CreateAccount @email={createAccountRequest.Email}, @password={createAccountRequest.Password}, @paymentMethod={createAccountRequest.Payment_Method}, @blocked={createAccountRequest.Blocked}, @isInvited={createAccountRequest.Is_Invited}, @trialEndDate={createAccountRequest.Trial_End_Date}");
                return Results.Ok();
            });*/
            
            // Create Account using Stored Procedure
            app.MapPost("/stored-procedure-create-account", async ([FromBody] CreateAccountRequest createAccountRequest,
                [FromServices] AppDbcontext dbContext,
                [FromServices] ApiKeyService apiKeyService) =>
            {
                // Execute the stored procedure to create the account
                await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC CreateAccount @email={createAccountRequest.Email}, @password={createAccountRequest.Password}, @paymentMethod={createAccountRequest.Payment_Method}, @blocked={createAccountRequest.Blocked}, @isInvited={createAccountRequest.Is_Invited}, @trialEndDate={createAccountRequest.Trial_End_Date}");
            
                // Retrieve the newly created account's AccountId (assuming it’s auto-generated)
                var account = await dbContext.Accounts
                    .FirstOrDefaultAsync(a => a.Email == createAccountRequest.Email);
            
                if (account != null)
                {
                    // Generate and store the API key using the stored procedure
                    var apiKey = await apiKeyService.CreateApiKeyAsync(account.Account_Id);
            
                    // Return the account and the API key
                    return Results.Ok(new { Account = account, ApiKey = apiKey });
                }
            
                return Results.BadRequest("Account creation failed.");
            });
            
            // Update Account
            app.MapPut("/stored-procedure-update-account-by-id", async ([FromBody] UpdateAccountRequest updateAccountRequest,
                [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC UpdateAccountById @accountId={updateAccountRequest.Account_Id}, @email={updateAccountRequest.Email}, @password={updateAccountRequest.Password}, @paymentMethod={updateAccountRequest.Payment_Method}, @blocked={updateAccountRequest.Blocked}, @isInvited={updateAccountRequest.Is_Invited}, @trialEndDate={updateAccountRequest.Trial_End_Date}");
                return Results.Ok();
            });

            // Get Accounts
            app.MapGet("/stored-procedure-get-accounts", async (AppDbcontext dbContext) =>
            {
                var accounts = await dbContext.Accounts.FromSqlRaw("EXEC GetAccounts").ToListAsync();
                return accounts;
            });
            
            // Get Account by Account_Id using Stored Procedure
            app.MapGet("/stored-procedure-get-account-by-id/{accountId:int}", async (int accountId, [FromServices] AppDbcontext dbContext) =>
            {
                var accountDto = await dbContext.Accounts
                    .FromSqlInterpolated($"EXEC GetAccountById @AccountId={accountId}")
                    .ToListAsync()
                    .ContinueWith(task => task.Result.Select(a => new AccountDto
                    {
                        Account_Id = a.Account_Id,
                        Email = a.Email,
                        Payment_Method = a.Payment_Method,
                        Blocked = a.Blocked,
                        Is_Invited = a.Is_Invited,
                        Trial_End_Date = a.Trial_End_Date
                    }).FirstOrDefault());

                return accountDto == null 
                    ? Results.NotFound() 
                    : Results.Ok(accountDto);
            });

            // Get Account by Account_Email using Stored Procedure
            app.MapGet("/stored-procedure-get-account-by-email/{email}", async (string email, [FromServices] AppDbcontext dbContext) =>
            {
                var accountDto = await dbContext.Accounts
                    .FromSqlInterpolated($"EXEC GetAccountByEmail @Email={email}")
                    .ToListAsync()
                    .ContinueWith(task => task.Result.Select(a => new AccountDto
                    {
                        Account_Id = a.Account_Id,
                        Email = a.Email,
                        Payment_Method = a.Payment_Method,
                        Blocked = a.Blocked,
                        Is_Invited = a.Is_Invited,
                        Trial_End_Date = a.Trial_End_Date
                    }).FirstOrDefault());

                return accountDto == null 
                    ? Results.NotFound() 
                    : Results.Ok(accountDto);
            });
            
            // Stored Procedure for Deleting Account
            app.MapDelete("/stored-procedure-delete-account", async (int accountId,[FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteAccountById @accountId={accountId}");
                return Results.Ok();
            });
        }
    }


