// using DataProcessingBS.Data;
// using Microsoft.AspNetCore.Mvc;
// using DataProcessingBS.Entities;
// using Microsoft.Data.SqlClient;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
//
// namespace DataProcessingBS.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class AccountController : ControllerBase
//     {
//         private readonly AppDbcontext _context;
//         private readonly ILogger<AccountController> _logger;
//
//         public AccountController(AppDbcontext context, ILogger<AccountController> logger)
//         {
//             _context = context;
//             _logger = logger ?? throw new ArgumentNullException(nameof(logger));  // Make sure the logger is not null
//         }
//         
//         // [HttpPost]
//         // public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request, [FromServices] AppDbcontext dbContext)
//         // {
//         //     try
//         //     {
//         //         // Execute the stored procedure with interpolated parameters
//         //         await dbContext.Database.ExecuteSqlInterpolatedAsync(
//         //             $"EXEC InsertAccount @Email={request.Email}, @Password={request.Password}, @PaymentMethod={request.PaymentMethod}, @Blocked={request.Blocked}, @IsInvited={request.IsInvited}, @TrialEndDate={request.TrialEndDate ?? (object)DBNull.Value}");
//         //
//         //         return Ok("Account created successfully!");
//         //     }
//         //     catch (Exception ex)
//         //     {
//         //         return StatusCode(500, $"Error creating account: {ex.Message}");
//         //     }
//         // }
//
//
//
//         // CREATE: POST api/account
//         [HttpPost]
//         public async Task<ActionResult<Account>> CreateAccount(Account account)
//         {
//             // Executes the stored procedure to create a new account
//             await _context.Database.ExecuteSqlRawAsync(
//                 "EXEC CreateAccount @p0, @p1, @p2",
//                 parameters: new object[] { account.Email, account.Password, account.Payment_Method }
//             );
//
//             // Return the created account as a response (without using GetAccountByEmail)
//             var createdAccount = new Account
//             {
//                 Email = account.Email,
//                 Password = account.Password,
//                 Payment_Method = account.Payment_Method
//             };
//
//             return CreatedAtAction(nameof(GetAccountById), new { id = createdAccount.Account_Id }, createdAccount);
//         }
//         
//         
//
//         // [HttpGet("account/{email}")]
//         // public async Task<IActionResult> GetAccountByEmail(string email)
//         // {
//         //     try
//         //     {
//         //         _logger.LogInformation($"Received request for account with email: {email}");
//         //
//         //         // Execute the stored procedure and retrieve the result as a list
//         //         var accounts = await _context.Accounts
//         //             .FromSqlRaw("EXECUTE GetAccountByEmail @Email = {0}", email)
//         //             .AsEnumerable() // Force client-side processing
//         //             .ToList(); // Convert to a list (this will be done on the client side)
//         //
//         //         // Select the first account or return null if none exists
//         //         var account = accounts.FirstOrDefault(); 
//         //
//         //         if (account == null)
//         //         {
//         //             _logger.LogWarning($"No account found for email: {email}");
//         //             return NotFound("Account not found.");
//         //         }
//         //
//         //         _logger.LogInformation($"Account found for email: {email}, AccountId: {account.Account_Id}");
//         //         return Ok(account);
//         //     }
//         //     catch (Exception ex)
//         //     {
//         //         _logger.LogError($"Error occurred in GetAccountByEmail method: {ex.Message}");
//         //         return StatusCode(500, "Internal server error: " + ex.Message);
//         //     }
//         // }
//
//
//
//
//
//
//     
//
//
//
//
//
//         //GET BY ID[HttpPut("{id}")]
//         [HttpGet("{id}")]
//         public async Task<ActionResult<Account>> GetAccountById(int id)
//         {
//             var account = await _context.Accounts
//                 .FromSqlRaw("EXEC GetAccountById @p0", id)
//                 .FirstOrDefaultAsync();
//
//             if (account == null)
//             {
//                 return NotFound();
//             }
//
//             return account;
//         }
//
//         
//         //GET ALL
//         //[HttpGet]
//         //public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
//         //{
//             //return await _context.Accounts
//                 //.FromSqlRaw("EXEC GetAllAccounts")
//                 //.ToListAsync();
//         //}
//         
//         // [HttpGet]
//         // public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
//         // {
//         //     try
//         //     {
//         //         var accounts = await _context.Accounts
//         //             .FromSqlRaw("EXEC GetAllAccounts")
//         //             .ToListAsync();
//         //         return accounts;
//         //     }
//         //     catch (Exception ex)
//         //     {
//         //         return StatusCode(500, $"An error occurred while fetching accounts: {ex.Message}");
//         //     }
//         // }
//     }
// }