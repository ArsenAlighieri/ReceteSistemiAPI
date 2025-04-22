using System.ComponentModel.DataAnnotations.Schema;

namespace ReceteSistemiAPI
{
    public class Ilac
    {
        [Column("IlacID")]
        public int Id { get; set; }
        public string Ad { get; set; }
        
        public string Aciklama { get; set; }
        public int Stok { get; set; }

        public ICollection<ReceteIlac> ReceteIlaclar { get; set; }
    }
}