using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using DataProcessingBS.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchController : ControllerBase
    {
        private readonly AppDbcontext _context;

        public WatchController(AppDbcontext context)
        {
            _context = context;
        }

        // CREATE
        [HttpPost]
        public async Task<ActionResult<Watch>> CreateWatch(Watch watch)
        {
            _context.Watches.Add(watch);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWatchById), new { id = watch.Watch_Id }, watch);
        }

        // READ ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Watch>>> GetWatches()
        {
            return await _context.Watches.ToListAsync();
        }

        // READ ONE
        [HttpGet("{id}")]
        public async Task<ActionResult<Watch>> GetWatchById(int id)
        {
            var watch = await _context.Watches.FindAsync(id);

            if (watch == null)
            {
                return NotFound();
            }

            return watch;
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWatch(int id, Watch watch)
        {
            if (id != watch.Watch_Id)
            {
                return BadRequest();
            }

            _context.Entry(watch).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWatch(int id)
        {
            var watch = await _context.Watches.FindAsync(id);
            if (watch == null)
            {
                return NotFound();
            }

            _context.Watches.Remove(watch);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}