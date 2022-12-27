using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Model.Pojistka
{
    public class PojistkaEditModel
    {
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
