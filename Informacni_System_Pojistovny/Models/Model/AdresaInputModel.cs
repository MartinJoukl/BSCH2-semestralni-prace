using Informacni_System_Pojistovny.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class AdresaInputModel
    {
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
