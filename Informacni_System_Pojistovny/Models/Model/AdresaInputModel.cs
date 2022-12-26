using Informacni_System_Pojistovny.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class AdresaInputModel
    {
        [Display(Name = "Cislo Popisne")]
        [IntegerValidator(MinValue = 0)]
        public int CisloPopisne { get; set; }
        [StringValidator(MaxLength = 200)]
        [NotNull]
        [Display(Name = "Ulice")]
        public string Ulice { get; set; }
        [Display(Name = "Psč")]
        [StringValidator(MaxLength = 5, MinLength = 5)]
        public string Psc { get; set; }
    }
}
