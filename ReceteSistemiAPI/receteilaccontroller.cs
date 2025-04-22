using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceteSistemiAPI;

namespace ReceteSistemiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceteIlacController : ControllerBase
    {
        private readonly MySqlDbContext _context;

        public ReceteIlacController(MySqlDbContext context)
        {
            _context = context;
        }

        // GET: api/ReceteIlac
        [HttpGet]
        public async Task<IActionResult> GetReceteIlaclar()
        {
            var receteIlaclar = await _context.ReceteIlaclar
                .Include(r => r.Recete)  // Recete ilişkisini dahil ediyoruz
                .Include(i => i.Ilac)    // Ilac ilişkisini dahil ediyoruz
                .ToListAsync();

            return Ok(receteIlaclar);
        }

        // POST: api/ReceteIlac
        [HttpPost]
        public async Task<IActionResult> PostReceteIlac(ReceteIlac receteIlac)
        {
            _context.ReceteIlaclar.Add(receteIlac);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReceteIlaclar), new { receteId = receteIlac.ReceteId, ilacId = receteIlac.IlacId }, receteIlac);
        }

        // DELETE: api/ReceteIlac/{receteId}/{ilacId}
        [HttpDelete("{receteId}/{ilacId}")]
        public async Task<IActionResult> DeleteReceteIlac(int receteId, int ilacId)
        {
            var receteIlac = await _context.ReceteIlaclar
                .FirstOrDefaultAsync(r => r.ReceteId == receteId && r.IlacId == ilacId);

            if (receteIlac == null)
            {
                return NotFound();
            }

            _context.ReceteIlaclar.Remove(receteIlac);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}