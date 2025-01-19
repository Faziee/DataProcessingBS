using DataProcessingBS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using DataProcessingBS.Data;

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
            // Call the stored procedure to generate and store the API key
            var apiKey = await _context.ApiKeys
                .FromSqlInterpolated($"EXEC CreateApiKey @accountId={accountId}")
                .Select(a => a.Key)  // Select only the generated API key
                .FirstOrDefaultAsync();

            return apiKey;  // Return the generated API key
        }

        public async Task<bool> IsApiKeyValidAsync(string apiKey)
        { 
            var apiKeyEntity = await _context.ApiKeys
                .FromSqlRaw("EXEC GetApiKeyByKey @p0", apiKey)
                .ToListAsync(); 

            // Check if the entity is valid and the key is active
            return apiKeyEntity.FirstOrDefault()?.Is_Active == true;;
        }
        
        // Helper method to validate IsActive
        private bool IsApiKeyActive(string isActive)
        {
            return isActive.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }
    }
}