using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using DataProcessingBS.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly AppDbcontext _context;

        public ProfileController(AppDbcontext context)
        {
            _context = context;
        }

        // CREATE
        [HttpPost]
        public async Task<ActionResult<Profile>> CreateProfile(Profile profile)
        {
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProfileById), new { id = profile.Profile_Id }, profile);
        }

        // READ ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profile>>> GetProfiles()
        {
            return await _context.Profiles.ToListAsync();
        }

        // READ ONE
        [HttpGet("{id}")]
        public async Task<ActionResult<Profile>> GetProfileById(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(int id, Profile profile)
        {
            if (id != profile.Profile_Id)
            {
                return BadRequest();
            }

            _context.Entry(profile).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}