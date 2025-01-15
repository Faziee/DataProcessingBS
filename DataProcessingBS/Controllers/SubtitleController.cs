using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using DataProcessingBS.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubtitleController : ControllerBase
    {
        private readonly AppDbcontext _context;

        public SubtitleController(AppDbcontext context)
        {
            _context = context;
        }

        // CREATE
        [HttpPost]
        public async Task<ActionResult<Subtitle>> CreateSubtitle(Subtitle subtitle)
        {
            _context.Subtitles.Add(subtitle);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSubtitleById), new { id = subtitle.Subtitle_Id }, subtitle);
        }

        // READ ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subtitle>>> GetSubtitles()
        {
            return await _context.Subtitles.ToListAsync();
        }

        // READ ONE
        [HttpGet("{id}")]
        public async Task<ActionResult<Subtitle>> GetSubtitleById(int id)
        {
            var subtitle = await _context.Subtitles.FindAsync(id);

            if (subtitle == null)
            {
                return NotFound();
            }

            return subtitle;
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubtitle(int id, Subtitle subtitle)
        {
            if (id != subtitle.Subtitle_Id)
            {
                return BadRequest();
            }

            _context.Entry(subtitle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubtitle(int id)
        {
            var subtitle = await _context.Subtitles.FindAsync(id);
            if (subtitle == null)
            {
                return NotFound();
            }

            _context.Subtitles.Remove(subtitle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}