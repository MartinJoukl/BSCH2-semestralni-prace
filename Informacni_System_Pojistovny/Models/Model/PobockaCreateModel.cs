using Informacni_System_Pojistovny.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class PobockaCreateModel
    {
        [StringLength(200, ErrorMessage = "Maximální délka je 200 znaků")]
        [Display(Name = "Název pobočky")]
        [Required(ErrorMessage = "Název pobočky musí být vyplněn", AllowEmptyStrings = false)]
        public string Nazev { get; set; }
        [Display(Name = "Číslo Popisné")]
        [Range(1, int.MaxValue, ErrorMessage = "Číslo popisné musí být číslo větší jak 0")]
        [Required(ErrorMessage = "Číslo popisné musí být vyplněno")]
        public int CisloPopisne { get; set; }
        [StringValidator(MaxLength = 200)]
        [Required(ErrorMessage = "Ulice musí být vyplněna")]
        [Display(Name = "Ulice")]
        public string Ulice { get; set; }
        [Display(Name = "Psč")]
        [StringValidator(MaxLength = 5, MinLength = 5)]
        [Required(ErrorMessage = "PSČ musí být vyplněno")]
        public string Psc { get; set; }
    }
}
