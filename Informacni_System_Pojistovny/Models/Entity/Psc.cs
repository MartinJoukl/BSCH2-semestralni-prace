using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Psc
    {
        [Display(Name = "PSČ"), StringLength(5, MinimumLength = 5, ErrorMessage = "PSČ musí obsahovat 5 znaků"), Required(ErrorMessage = "PSČ musí být vyplněno", AllowEmptyStrings = false)]
        [RegularExpression(@"^([0-9]{5})$", ErrorMessage = "PSČ není správně vyplněno")]
        public string PscCislo { get; set; }
        [StringLength(200, ErrorMessage = "Maximální délka je 200 znaků")]
        [Display(Name = "Město")]
        [Required(ErrorMessage = "Město musí být vyplněno", AllowEmptyStrings = false)]
        public string Mesto { get; set; }
        public Adresa? Adresa { get; set; }
    }
}