using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Zavazek
    {
        [Required]
        public int ZavazekId { get; set; }
        [Display(Name = "Vznik"), Required]
        [DataType(DataType.Date)]
        public DateTime Vznik { get; set; } = DateTime.Now;
        [Display(Name = "Datum splatnosti"), Required]
        [DataType(DataType.Date)]
        public DateTime DatumSplatnosti { get; set; } = DateTime.Now;
        [Display(Name = "Výše")]
        [DataType(DataType.Currency)]
        public int Vyse { get; set; }
        [Display(Name = "Datum splacení")]
        [DataType(DataType.Date)]
        public DateTime? DatumSplaceni { get; set; } = DateTime.Now;
        [Display(Name = "Popis")]
        public string Popis { get; set; }
        [Display(Name = "Pojistná událost"), Required]
        public PojistnaUdalost PojistnaUdalost { get; set; }
    }
}
