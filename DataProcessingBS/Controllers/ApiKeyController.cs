using Microsoft.AspNetCore.Mvc;
using DataProcessingBS.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataProcessingBS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiKeyController : ControllerBase
    {
        private readonly AppDbcontext _context;

        public ApiKeyController(AppDbcontext context)
        {
            _context = context;
        }

        // CREATE API KEY (after account creation)
        [HttpPost("generate/{accountId}")]
        public async Task<ActionResult> CreateApiKey(int accountId)
        {
            try
            {
                // Check if account exists
                var account = await _context.Accounts
                    .FromSqlRaw("EXEC GetAccountById @p0", accountId)
                    .FirstOrDefaultAsync();

                if (account == null)
                {
                    return NotFound("Account not found.");
                }

                // Generate API Key using stored procedure
                var apiKey = await _context.ApiKeys
                    .FromSqlRaw("EXEC GenerateApiKey @p0", accountId)
                    .FirstOrDefaultAsync();

                if (apiKey == null)
                {
                    return StatusCode(500, "Failed to generate API Key.");
                }

                return CreatedAtAction(nameof(GetApiKey), new { accountId }, new { ApiKey = apiKey.Key });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while generating the API key: {ex.Message}");
            }
        }

        // READ API KEY by AccountId
        [HttpGet("{accountId}")]
        public async Task<ActionResult> GetApiKey(int accountId)
        {
            try
            {
                // Get API Key for the account
                var apiKey = await _context.ApiKeys
                    .FromSqlRaw("EXEC GetApiKeyByAccountId @p0", accountId)
                    .FirstOrDefaultAsync();

                if (apiKey == null)
                {
                    return NotFound("API key not found for the given account.");
                }

                return Ok(new { ApiKey = apiKey.Key });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching the API key: {ex.Message}");
            }
        }

        // REGENERATE API KEY
        [HttpPut("regenerate/{accountId}")]
        public async Task<ActionResult> RegenerateApiKey(int accountId)
        {
            try
            {
                // Check if account exists
                var account = await _context.Accounts
                    .FromSqlRaw("EXEC GetAccountById @p0", accountId)
                    .FirstOrDefaultAsync();

                if (account == null)
                {
                    return NotFound("Account not found.");
                }

                // Regenerate API Key using stored procedure
                var result = await _context.Database.ExecuteSqlRawAsync(
                    "EXEC RegenerateApiKey @p0",
                    parameters: new object[] { accountId });

                if (result == 0)
                {
                    return StatusCode(500, "Failed to regenerate API Key.");
                }

                return NoContent(); // API Key successfully regenerated
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while regenerating the API key: {ex.Message}");
            }
        }

        // DELETE API KEY
        [HttpDelete("{accountId}")]
        public async Task<ActionResult> DeleteApiKey(int accountId)
        {
            try
            {
                // Check if account exists
                var account = await _context.Accounts
                    .FromSqlRaw("EXEC GetAccountById @p0", accountId)
                    .FirstOrDefaultAsync();

                if (account == null)
                {
                    return NotFound("Account not found.");
                }

                // Delete the API key using stored procedure
                var result = await _context.Database.ExecuteSqlRawAsync(
                    "EXEC DeleteApiKey @p0",
                    parameters: new object[] { accountId });

                if (result == 0)
                {
                    return NotFound("API key not found for the account.");
                }

                return NoContent(); // API Key successfully deleted
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the API key: {ex.Message}");
            }
        }
    }
}
