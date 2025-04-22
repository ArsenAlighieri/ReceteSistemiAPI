using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceteSistemiAPI;

namespace ReceteSistemiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Bu satır ile sadece yetkilendirilmiş kullanıcılar bu controller'a erişebilecek
    public class ReceteController : ControllerBase
    {
        private readonly MySqlDbContext _context;

        public ReceteController(MySqlDbContext context)
        {
            _context = context;
        }

        // GET: api/Recete
        [HttpGet]
        public async Task<IActionResult> GetReceteler()
        {
            var receteler = await _context.Receteler
                                          .Include(r => r.Veteriner)
                                          .Include(r => r.ReceteIlaclar)
                                          .ThenInclude(ri => ri.Ilac)
                                          .ToListAsync();
            return Ok(receteler);
        }

        // POST: api/Recete
        [HttpPost]
        public async Task<IActionResult> PostRecete(Recete recete)
        {
            // Stok güncelleme işlemi yapılacak
            foreach (var receteIlac in recete.ReceteIlaclar)
            {
                var ilac = await _context.Ilaclar.FindAsync(receteIlac.IlacId);

                if (ilac == null)
                {
                    return NotFound($"İlaç {receteIlac.IlacId} bulunamadı.");
                }

                if (ilac.Stok < receteIlac.Miktar)
                {
                    return BadRequest($"Yetersiz stok: {ilac.Ad}");
                }

                // Stok güncelleme
                ilac.Stok -= receteIlac.Miktar;
            }

            // Reçeteyi ekle
            _context.Receteler.Add(recete);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReceteler), new { id = recete.Id }, recete);
        }

        // PUT: api/Recete/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecete(int id, Recete recete)
        {
            if (id != recete.Id)
            {
                return BadRequest("Reçete ID'si eşleşmiyor.");
            }

            // Stok güncellemeleri
            foreach (var receteIlac in recete.ReceteIlaclar)
            {
                var ilac = await _context.Ilaclar.FindAsync(receteIlac.IlacId);

                if (ilac == null)
                {
                    return NotFound($"İlaç {receteIlac.IlacId} bulunamadı.");
                }

                if (ilac.Stok < receteIlac.Miktar)
                {
                    return BadRequest($"Yetersiz stok: {ilac.Ad}");
                }

                // Stok güncelleme
                ilac.Stok -= receteIlac.Miktar;
            }

            _context.Entry(recete).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceteExists(id))
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

        // DELETE: api/Recete/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecete(int id)
        {
            var recete = await _context.Receteler.FindAsync(id);
            if (recete == null)
            {
                return NotFound();
            }

            // Stokları geri eklemek isteyebilirsiniz, ancak bu isteğe bağlıdır.
            foreach (var receteIlac in recete.ReceteIlaclar)
            {
                var ilac = await _context.Ilaclar.FindAsync(receteIlac.IlacId);

                if (ilac != null)
                {
                    // Reçete iptal edilirse, ilaçların stoklarını geri ekleyebiliriz
                    ilac.Stok += receteIlac.Miktar;
                }
            }

            _context.Receteler.Remove(recete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Yardımcı metod: Reçete var mı diye kontrol etmek için
        private bool ReceteExists(int id)
        {
            return _context.Receteler.Any(e => e.Id == id);
        }
    }
}
