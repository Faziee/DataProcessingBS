using DataProcessingBS.Data;
using DataProcessingBS.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly AppDbcontext _context;

        public TestController(AppDbcontext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _context.Accounts.ToListAsync();
            return Ok(accounts);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddAccount(Account account)
        {
            if (account == null)
            {
                return BadRequest("Account data is required.");
            }

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllAccounts), new { id = account.Account_Id }, account);
        }
    }
}