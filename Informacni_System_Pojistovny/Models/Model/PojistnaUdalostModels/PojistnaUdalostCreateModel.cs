using Informacni_System_Pojistovny.Models.Entity;
using Microsoft.Build.Framework;

namespace Informacni_System_Pojistovny.Models.Model.PojistnaUdalostModels
{
    public class PojistnaUdalostCreateModel
    {
        [Required]
        public PojistnaUdalost PojistnaUdalost { get; set; }
        [Required]
        public int KlientId { get; set; }
    }
}
