using System.ComponentModel.DataAnnotations.Schema;

namespace ReceteSistemiAPI
{
    public class ReceteIlac
    {
        [Column("ReceteID")]
        public int ReceteId { get; set; }
        public Recete Recete { get; set; }

        [Column("IlacID")]
        public int IlacId { get; set; }
        public Ilac Ilac { get; set; }

        public int Miktar { get; set; }
    }
}