using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using DataProcessingBS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Modules;

public static class AccountsStoredProcedureModule
{
    public static void AddAccountStoredProcedureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/stored-procedure-create-account", async ([FromBody] CreateAccountRequest createAccountRequest,
            [FromServices] AppDbcontext dbContext,
            [FromServices] ApiKeyService apiKeyService) =>
        {
            try
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC CreateAccount @email={createAccountRequest.Email}, @password={createAccountRequest.Password}, @Payment_Method={createAccountRequest.Payment_Method}, @blocked={createAccountRequest.Blocked}, @is_Invited={createAccountRequest.Is_Invited}, @trial_End_Date={createAccountRequest.Trial_End_Date}");

                await Task.Delay(100);

                var account = await dbContext.Accounts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Email == createAccountRequest.Email);

                if (account == null)
                    return Results.BadRequest("Account creation failed or account could not be retrieved.");

                var apiKey = await apiKeyService.CreateApiKeyAsync(account.Account_Id);

                if (string.IsNullOrEmpty(apiKey)) return Results.BadRequest("Failed to generate API key.");

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

        app.MapGet("/stored-procedure-get-accounts", async (AppDbcontext dbContext) =>
        {
            var accounts = await dbContext.Accounts.FromSqlRaw("EXEC GetAccounts").ToListAsync();
            return accounts;
        });

        app.MapGet("/stored-procedure-get-account-by-id/{accountId:int}",
            async (int accountId, [FromServices] AppDbcontext dbContext) =>
            {
                var accountDto = await dbContext.Accounts
                    .FromSqlInterpolated($"EXEC GetAccountById @AccountId={accountId}")
                    .ToListAsync()
                    .ContinueWith(task => Enumerable.Select(task.Result, a => new AccountDto
                    {
                        Account_Id = a.Account_Id,
                        Email = a.Email,
                        Password = a.Password,
                        Payment_Method = a.Payment_Method,
                        Blocked = a.Blocked,
                        Is_Invited = a.Is_Invited,
                        Trial_End_Date = a.Trial_End_Date
                    }).FirstOrDefault());

                return accountDto == null
                    ? Results.NotFound()
                    : Results.Ok(accountDto);
            });

        app.MapGet("/stored-procedure-get-account-by-email/{email}",
            async (string email, [FromServices] AppDbcontext dbContext) =>
            {
                var accountDto = await dbContext.Accounts
                    .FromSqlInterpolated($"EXEC GetAccountByEmail @Email={email}")
                    .ToListAsync()
                    .ContinueWith(task => Enumerable.Select(task.Result, a => new AccountDto
                    {
                        Account_Id = a.Account_Id,
                        Email = a.Email,
                        Password = a.Password,
                        Payment_Method = a.Payment_Method,
                        Blocked = a.Blocked,
                        Is_Invited = a.Is_Invited,
                        Trial_End_Date = a.Trial_End_Date
                    }).FirstOrDefault());

                return accountDto == null
                    ? Results.NotFound()
                    : Results.Ok(accountDto);
            });

        app.MapPut("/stored-procedure-update-account-by-id", async (
            [FromBody] UpdateAccountRequest updateAccountRequest,
            [FromServices] AppDbcontext dbContext) =>
        {
            await dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC UpdateAccountById @accountId={updateAccountRequest.Account_Id}, @email={updateAccountRequest.Email}, @password={updateAccountRequest.Password}, @paymentMethod={updateAccountRequest.Payment_Method}");
            return Results.Ok();
        });

        /*app.MapDelete("/stored-procedure-delete-account/{accountId}",
            async (int accountId, [FromServices] AppDbcontext dbContext) =>
            {
                await dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteAccountById @accountId={accountId}");
                return Results.Ok();
            });*/

        app.MapDelete("/stored-procedure-delete-account/{accountId}",
            async (int accountId, [FromServices] AppDbcontext dbContext) =>
            {
                try
                {
                    await dbContext.Database.ExecuteSqlInterpolatedAsync(
                        $"EXEC DeleteAccountById @accountId={accountId}");

                    await dbContext.Database.ExecuteSqlInterpolatedAsync(
                        $"EXEC DeleteApiKeyByAccountId @AccountId={accountId}");

                    return Results.Ok(new { Message = "Account and associated API key deleted successfully." });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Failed to delete account: {ex.Message}");
                }
            });
    }
}