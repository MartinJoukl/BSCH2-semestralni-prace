using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class PojistnaUdalost
    {
        [Display(Name = "Id")]
        public int PojistnaUdalostId { get; set; }
        public Klient Klient { get; set; }
        [Display(Name = "Vznik pojistky"), DataType(DataType.Date)]
        public DateTime Vznik { get; set; }
        [Display(Name = "Popis")]
        public string Popis { get; set; }
        [Display(Name = "Nárokovaná výše pojistky")]
        public int NarokovanaVysePojistky { get; set; }
        public List<Zavazek> Zavazky { get; set; } = new List<Zavazek>();
    }
}