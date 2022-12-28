using Informacni_System_Pojistovny.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Model.PohledavkaModels
{
    public class RedirectablePohledavka
    {
        public int ID { get; set; }
        [Display(Name = "Vznik"), Required]
        [DataType(DataType.Date)]
        public DateTime Vznik { get; set; } = DateTime.Now;
        [Display(Name = "Datum splatnosti"), Required]
        [DataType(DataType.Date)]
        public DateTime DatumSplatnosti { get; set; } = DateTime.Now;
        [Display(Name = "Výše"), Required]
        [DataType(DataType.Currency)]
        public int Vyse { get; set; }
        [Display(Name = "Datum splacení")]
        [DataType(DataType.Date)]
        public DateTime? DatumSplaceni { get; set; } = DateTime.Now;
        [Display(Name = "Popis")]
        public string Popis { get; set; }
        [Required]
        public int PojistkaId { get; set; }
        public string? RedirectedFrom { get; set; }
    }
}
