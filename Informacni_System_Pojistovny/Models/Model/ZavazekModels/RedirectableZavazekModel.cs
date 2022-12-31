using Informacni_System_Pojistovny.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Model.ZavazekModels
{
    public class RedirectableZavazekModel
    {
        [Required]
        public long ZavazekId { get; set; }
        [Display(Name = "Vznik"), Required]
        [DataType(DataType.Date)]
        public DateTime Vznik { get; set; } = DateTime.Now;
        [Display(Name = "Datum splatnosti"), Required]
        [DataType(DataType.Date)]
        public DateTime DatumSplatnosti { get; set; } = DateTime.Now;
        [Display(Name = "Výše")]
        [DataType(DataType.Currency)]
        public long Vyse { get; set; }
        [Display(Name = "Datum splacení")]
        [DataType(DataType.Date)]
        public DateTime? DatumSplaceni { get; set; } = DateTime.Now;
        [Display(Name = "Popis")]
        public string Popis { get; set; }
        [Display(Name = "Pojistná událost"), Required]
        public long PojistnaUdalostId { get; set; }
        public string? RedirectedFrom { get; set; }
        public string? KlientId { get; set; }
    }
}
