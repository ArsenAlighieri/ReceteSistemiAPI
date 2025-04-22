using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Hasta
{
    [Key]
    public int HastaID { get; set; }
    public string Ad { get; set; }
    public string Soyad { get; set; }
    public string Telefon { get; set; }
    public string Email { get; set; }
    public string Tedavi { get; set; }
    public string Teshis { get; set; }
    public string HayvanAd { get; set; }
    public string HayvanTur { get; set; }
}