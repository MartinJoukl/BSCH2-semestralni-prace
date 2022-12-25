using Informacni_System_Pojistovny.Models.Entity;
using Microsoft.Build.Framework;

namespace Informacni_System_Pojistovny.Models.Model.PojistnaUdalostModels
{
    public class PojistnaUdalostCreateEditModel
    {
        [Required]
        public PojistnaUdalost PojistnaUdalost { get; set; }
        [Required]
        public List<Klient> Klients { get; set; }
    }
}
