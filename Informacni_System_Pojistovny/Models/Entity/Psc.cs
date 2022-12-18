using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Psc
    {
        [Display(Name = "PSČ"), StringLength(5, MinimumLength = 5, ErrorMessage = "PSČ musí obsahovat 5 znaků"), Required(ErrorMessage = "PSČ musí být vyplněno", AllowEmptyStrings = false)]
        [RegularExpression(@"^([0-9]{5})$", ErrorMessage = "PSČ není správně vyplněno")]
        public string PscCislo { get; set; }
        [StringLength(200, ErrorMessage = "Maximální délka je 5 znaků")]
        [Display(Name = "Město")]
        public string Mesto { get; set; }
        public Adresa? Adresa { get; set; }
    }
}