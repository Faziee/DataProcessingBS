using System.Data;
using DataProcessingBS.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Services;

public class ApiKeyService
{
    private readonly IConfiguration _configuration;
    private readonly AppDbcontext _context;

    public ApiKeyService(AppDbcontext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> CreateApiKeyAsync(int accountId)
    {
        using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = "EXEC CreateApiKey @accountId";
        command.CommandType = CommandType.Text;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@accountId";
        parameter.Value = accountId;
        command.Parameters.Add(parameter);

        await _context.Database.OpenConnectionAsync();

        try
        {
            using var result = await command.ExecuteReaderAsync();
            if (await result.ReadAsync())
                return result.GetString(0); 

            return null;
        }
        finally
        {
            await _context.Database.CloseConnectionAsync();
        }
    }

    public async Task<bool> IsApiKeyValidAsync(string apiKey)
    {
        using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = "EXEC ValidateApiKey @apiKey";
        command.CommandType = CommandType.Text;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@apiKey";
        parameter.Value = apiKey;
        command.Parameters.Add(parameter);

        await _context.Database.OpenConnectionAsync();

        try
        {
            using var result = await command.ExecuteReaderAsync();
            if (await result.ReadAsync())
                return result.GetBoolean(0);

            return false;
        }
        finally
        {
            await _context.Database.CloseConnectionAsync();
        }
    }

    public static async Task UpdateApiKeyStatus(int apiKeyId, bool isActive, AppDbcontext dbContext)
    {
        var apiKeyIdParameter = new SqlParameter("@ApiKeyId", apiKeyId);
        var isActiveParameter = new SqlParameter("@IsActive", isActive);

        await dbContext.Database.ExecuteSqlRawAsync("EXEC UpdateApiKeyStatus @ApiKeyId, @IsActive", apiKeyIdParameter,
            isActiveParameter);
    }
}