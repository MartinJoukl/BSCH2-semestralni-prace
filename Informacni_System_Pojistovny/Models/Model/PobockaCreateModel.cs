using Informacni_System_Pojistovny.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class PobockaCreateModel
    {
        [StringLength(200, ErrorMessage = "Maximální délka je 200 znaků")]
        [Display(Name = "Název pobočky")]
        public string Nazev { get; set; }
        [IntegerValidator(MinValue = 0)]
        public int CisloPopisne { get; set; }
        [StringValidator(MaxLength = 200)]
        [NotNull]
        public string Ulice { get; set; }
        [StringValidator(MaxLength = 5, MinLength = 5)]
        public string Psc { get; set; }
    }
}
