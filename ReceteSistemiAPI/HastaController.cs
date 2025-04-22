using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ReceteSistemiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HastaController : ControllerBase
    {
        private readonly MySqlDbContext _context;

        public HastaController(MySqlDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetHastalar()
        {
            var hastalar = await _context.Hastalar.ToListAsync();
            return Ok(hastalar);
        }

        [HttpPost]
        public async Task<IActionResult> PostHasta(Hasta hasta)
        {
            _context.Hastalar.Add(hasta);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetHastalar), new { hastaID = hasta.HastaID }, hasta);
        }

        // PUT: api/Hasta/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHasta(int id, Hasta hasta)
        {
            if (id != hasta.HastaID)
            {
                return BadRequest("Hasta ID eşleşmiyor.");
            }

            _context.Entry(hasta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Hastalar.Any(e => e.HastaID == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Hasta/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHasta(int id)
        {
            var hasta = await _context.Hastalar.FindAsync(id);
            if (hasta == null)
            {
                return NotFound();
            }

            _context.Hastalar.Remove(hasta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
