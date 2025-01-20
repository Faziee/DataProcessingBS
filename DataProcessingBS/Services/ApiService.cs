using DataProcessingBS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using DataProcessingBS.Contracts;
using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;

namespace DataProcessingBS.Services
{
    public class ApiKeyService
    {
        private readonly AppDbcontext _context;
        private readonly IConfiguration _configuration;

        public ApiKeyService(AppDbcontext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> CreateApiKeyAsync(int accountId)
        {
            // Execute stored procedure to create API key and return the generated key
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "EXEC CreateApiKey @accountId";
            command.CommandType = System.Data.CommandType.Text;

            var parameter = command.CreateParameter();
            parameter.ParameterName = "@accountId";
            parameter.Value = accountId;
            command.Parameters.Add(parameter);

            await _context.Database.OpenConnectionAsync();

            try
            {
                using var result = await command.ExecuteReaderAsync();
                if (await result.ReadAsync())
                {
                    return result.GetString(0); // Assuming the stored procedure returns the API key in the first column
                }

                return null;
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }
        }

        public async Task<bool> IsApiKeyValidAsync(string apiKey)
        {
            // Execute stored procedure to validate API key
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "EXEC ValidateApiKey @apiKey";
            command.CommandType = System.Data.CommandType.Text;

            var parameter = command.CreateParameter();
            parameter.ParameterName = "@apiKey";
            parameter.Value = apiKey;
            command.Parameters.Add(parameter);

            await _context.Database.OpenConnectionAsync();

            try
            {
                using var result = await command.ExecuteReaderAsync();
                if (await result.ReadAsync())
                {
                    return result.GetBoolean(0); // Assuming the stored procedure returns a bit indicating validity
                }

                return false;
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }
        }
    }
}