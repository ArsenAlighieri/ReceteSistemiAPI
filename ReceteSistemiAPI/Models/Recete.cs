using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceteSistemiAPI
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Recete
    {
        [Key]
        [Column("ReceteID")]
        public int Id { get; set; }
        public int HastaID { get; set; }
        public int VeterinerID { get; set; }
        public DateTime ReceteTarihi { get; set; }

        public Hasta Hasta { get; set; }
        public Veteriner Veteriner { get; set; }
        public List<ReceteIlac> ReceteIlaclar { get; set; }
    }

}