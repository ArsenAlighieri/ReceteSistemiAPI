using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceteSistemiAPI;

namespace ReceteSistemiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IlacController : ControllerBase
    {
        private readonly MySqlDbContext _context;

        public IlacController(MySqlDbContext context)
        {
            _context = context;
        }

        // GET: api/Ilac
        [HttpGet]
        public async Task<IActionResult> GetIlaclar()
        {
            var ilaclar = await _context.Ilaclar.ToListAsync();
            return Ok(ilaclar);
        }

        // POST: api/Ilac
        [HttpPost]
        public async Task<IActionResult> PostIlac(Ilac ilac)
        {
            _context.Ilaclar.Add(ilac);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetIlaclar), new { id = ilac.Id }, ilac);
        }

        // PUT: api/Ilac/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIlac(int id, Ilac ilac)
        {
            if (id != ilac.Id)
            {
                return BadRequest("Ilac ID'si eşleşmiyor.");
            }

            _context.Entry(ilac).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IlacExists(id))
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

        // DELETE: api/Ilac/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIlac(int id)
        {
            var ilac = await _context.Ilaclar.FindAsync(id);
            if (ilac == null)
            {
                return NotFound();
            }

            _context.Ilaclar.Remove(ilac);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Yardımcı metod: Ilac var mı diye kontrol etmek için
        private bool IlacExists(int id)
        {
            return _context.Ilaclar.Any(e => e.Id == id);
        }
    }
}
