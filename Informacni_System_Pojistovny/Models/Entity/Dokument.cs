using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Dokument
    {
        [Display(Name = "ID")]
        public int DokumentId { get; set; }
        [Display(Name = "Název")]
        public string Nazev { get; set; }
        [Display(Name = "Typ")]
        public string Typ { get; set; }
        [Display(Name = "Přípona")]
        public string Pripona { get; set; }
        [Display(Name = "Datum nahrání")]
        public DateTime DatumNahrani { get; set; }
        public Klient Klient { get; set; }
        public byte[] Data { get; set; }
    }
}
