using DataProcessingBS.Data;
using Microsoft.AspNetCore.Mvc;
using DataProcessingBS.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataProcessingBS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchListController : ControllerBase
    {
        private readonly AppDbcontext _context;

        public WatchListController(AppDbcontext context)
        {
            _context = context;
        }

        // CREATE
        [HttpPost]
        public async Task<ActionResult<WatchList>> CreateWatchList(WatchList watchList)
        {
            _context.WatchLists.Add(watchList);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWatchListById), new { id = watchList.WatchList_Id }, watchList);
        }

        // READ ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WatchList>>> GetWatchLists()
        {
            return await _context.WatchLists.ToListAsync();
        }

        // READ ONE
        [HttpGet("{id}")]
        public async Task<ActionResult<WatchList>> GetWatchListById(int id)
        {
            var watchList = await _context.WatchLists.FindAsync(id);

            if (watchList == null)
            {
                return NotFound();
            }

            return watchList;
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWatchList(int id, WatchList watchList)
        {
            if (id != watchList.WatchList_Id)
            {
                return BadRequest();
            }

            _context.Entry(watchList).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWatchList(int id)
        {
            var watchList = await _context.WatchLists.FindAsync(id);
            if (watchList == null)
            {
                return NotFound();
            }

            _context.WatchLists.Remove(watchList);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}