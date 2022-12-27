using Informacni_System_Pojistovny.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Model.Pojistka
{
    public class PojistkaCreateModel
    {
        [Required(ErrorMessage = "Je nutné vybrat pojistný produkt")]
        [Display(Name = "Pojistný produkt")]
        public int? PojistnyProduktId { get; set; }
        [Required(ErrorMessage = "Je nutné vybrat klienta")]
        [Display(Name = "Klient")]
        public int? KlientId { get; set; }
        [Display(Name = "Datum uzavření pojistky")]
        [Required(ErrorMessage = "Datum uzavření pojistky je požadováno")]
        [DataType(DataType.Date)]
        public DateTime Sjednano { get; set; }
        [Display(Name = "Výše poplatku")]
        [Required(ErrorMessage = "Výše poplatku musí být vyplněna")]
        [Range(1, int.MaxValue, ErrorMessage = "Výše poplatku musí být vyšší než 0")]
        public int Poplatek { get; set; }
        [Display(Name = "Sjednaná plnění")]
        [Required(ErrorMessage = "Sjednaná výše plnění musí být vyplněna")]
        [Range(1, int.MaxValue, ErrorMessage = "Sjednaná výše plnění musí být vyšší než 0")]
        public int SjednanaVyse { get; set; }
    }
}
