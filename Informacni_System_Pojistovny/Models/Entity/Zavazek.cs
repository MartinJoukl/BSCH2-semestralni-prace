using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Zavazek
    {
        public int ZavazekId { get; set; }
        [Display(Name = "Vznik")]
        [DataType(DataType.Date)]
        public DateTime Vznik { get; set; }
        [Display(Name = "Datum splatnosti")]
        [DataType(DataType.Date)]
        public DateTime DatumSplatnosti { get; set; }
        [Display(Name = "Výše")]
        [DataType(DataType.Currency)]
        public int Vyse { get; set; }
        [Display(Name = "Datum splacení")]
        [DataType(DataType.Date)]
        public DateTime? DatumSplaceni { get; set; }
        [Display(Name = "Popis")]
        public string Popis { get; set; }
        [Display(Name = "Pojistná událost")]
        public PojistnaUdalost PojistnaUdalost { get; set; }
    }
}
