using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using DataProcessingBS.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly AppDbcontext _context;

        public InvitationController(AppDbcontext context)
        {
            _context = context;
        }

        // CREATE
        [HttpPost]
        public async Task<ActionResult<Invitation>> SendInvitation(Invitation invitation)
        {
            _context.Invitations.Add(invitation);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInvitationById), new { id = invitation.Invitation_Id }, invitation);
        }

        // READ ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invitation>>> GetInvitations()
        {
            return await _context.Invitations.ToListAsync();
        }

        // READ ONE
        [HttpGet("{id}")]
        public async Task<ActionResult<Invitation>> GetInvitationById(int id)
        {
            var invitation = await _context.Invitations.FindAsync(id);

            if (invitation == null)
            {
                return NotFound();
            }

            return invitation;
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelInvitation(int id)
        {
            var invitation = await _context.Invitations.FindAsync(id);
            if (invitation == null)
            {
                return NotFound();
            }

            _context.Invitations.Remove(invitation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}