using Informacni_System_Pojistovny.Models.Entity;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class AdresaInputModel
    {
        [IntegerValidator(MinValue = 0)]
        public int CisloPopisne { get; set; }
        [StringValidator(MaxLength = 200)]
        [NotNull]
        public string Ulice { get; set; }
        public string Psc { get; set; }
    }
}
