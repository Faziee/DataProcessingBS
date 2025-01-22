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
            
            /*// Create Account using Stored Procedure
            app.MapPost("/stored-procedure-create-account", async ([FromBody] CreateAccountRequest createAccountRequest,
                [FromServices] AppDbcontext dbContext,
                [FromServices] ApiKeyService apiKeyService) =>
            {
                // Execute the stored procedure to create the account
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC CreateAccount @email={createAccountRequest.Email}, @password={createAccountRequest.Password}, @paymentMethod={createAccountRequest.Payment_Method}, @blocked={createAccountRequest.Blocked}, @isInvited={createAccountRequest.Is_Invited}, @trialEndDate={createAccountRequest.Trial_End_Date}");

                // Fetch the account by email directly
                var account = await dbContext.Accounts
                    .FirstOrDefaultAsync(a => a.Email == createAccountRequest.Email);

                if (account != null)
                {
                    // Map the account to a DTO
                    var accountDto = new AccountDto
                    {
                        Account_Id = account.Account_Id,
                        Email = account.Email,
                        Payment_Method = account.Payment_Method,
                        Blocked = account.Blocked,
                        Is_Invited = account.Is_Invited,
                        Trial_End_Date = account.Trial_End_Date
                    };

                    // Generate and store the API key
                    var apiKey = await apiKeyService.CreateApiKeyAsync(account.Account_Id);

                    // Return the account DTO and generated API key
                    return Results.Ok(new { Account = accountDto, ApiKey = apiKey });
                }

                // Return failure response if account creation or retrieval failed
                return Results.BadRequest("Account creation failed.");
            });*/
            
            
            // 2. Modified Account Creation Endpoint
            app.MapPost("/stored-procedure-create-account", async ([FromBody] CreateAccountRequest createAccountRequest,
                [FromServices] AppDbcontext dbContext,
                [FromServices] ApiKeyService apiKeyService) =>
            {
                try
                {
                    // Step 1: Create account using stored procedure
                    await dbContext.Database.ExecuteSqlInterpolatedAsync(
                        $"EXEC CreateAccount @email={createAccountRequest.Email}, @password={createAccountRequest.Password}, @Payment_Method={createAccountRequest.Payment_Method}, @blocked={createAccountRequest.Blocked}, @is_Invited={createAccountRequest.Is_Invited}, @trial_End_Date={createAccountRequest.Trial_End_Date}");

                    // Step 2: Wait a small moment to ensure the account is created
                    await Task.Delay(100);

                    // Step 3: Fetch the newly created account using direct SELECT
                    var account = await dbContext.Accounts
                        .AsNoTracking()
                        .FirstOrDefaultAsync(a => a.Email == createAccountRequest.Email);

                    if (account == null)
                    {
                        return Results.BadRequest("Account creation failed or account could not be retrieved.");
                    }

                    // Step 4: Generate API key using stored procedure
                    var apiKey = await apiKeyService.CreateApiKeyAsync(account.Account_Id);
        
                    if (string.IsNullOrEmpty(apiKey))
                    {
                        return Results.BadRequest("Failed to generate API key.");
                    }

                    // Step 5: Return the response
                    var response = new
                    {
                        Account = new
                        {
                            account.Account_Id,
                            account.Email,
                            account.Payment_Method,
                            account.Blocked,
                            account.Is_Invited,
                            account.Trial_End_Date
                        },
                        ApiKey = apiKey
                    };

                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Failed to create account: {ex.Message}");
                }
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
            
            //aciddntly removed delet get itfrom soemhwere else 
            app.MapDelete("/stored-procedure-delete-account/{accountId}", async (int accountId, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteAccountById @SeriesId={accountId}");
                return Results.Ok();
            });

        }
    }


