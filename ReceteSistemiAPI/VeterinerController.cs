using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceteSistemiAPI;

namespace ReceteSistemiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeterinerController : ControllerBase
    {
        private readonly MySqlDbContext _context;

        public VeterinerController(MySqlDbContext context)
        {
            _context = context;
        }

        // GET: api/Veteriner
        [HttpGet]
        public async Task<IActionResult> GetVeterinerler()
        {
            var veterinerler = await _context.Veterinerler.ToListAsync();
            return Ok(veterinerler);
        }

        // POST: api/Veteriner
        [HttpPost]
        public async Task<IActionResult> PostVeteriner(Veteriner veteriner)
        {
            _context.Veterinerler.Add(veteriner);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVeterinerler), new { id = veteriner.Id }, veteriner);
        }

        // PUT: api/Veteriner/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVeteriner(int id, Veteriner veteriner)
        {
            if (id != veteriner.Id)
            {
                return BadRequest("Veteriner ID'si eşleşmiyor.");
            }

            _context.Entry(veteriner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeterinerExists(id))
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

        // DELETE: api/Veteriner/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeteriner(int id)
        {
            var veteriner = await _context.Veterinerler.FindAsync(id);
            if (veteriner == null)
            {
                return NotFound();
            }

            _context.Veterinerler.Remove(veteriner);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Yardımcı metod: Veteriner var mı diye kontrol etmek için
        private bool VeterinerExists(int id)
        {
            return _context.Veterinerler.Any(e => e.Id == id);
        }
    }
}
