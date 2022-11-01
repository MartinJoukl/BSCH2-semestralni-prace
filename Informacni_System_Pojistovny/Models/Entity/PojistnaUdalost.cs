using System.Collections;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class PojistnaUdalost
    {
        public int PojistnaUdalostId { get; set; }
        public Klient Klient { get; set; }
        public DateTime Vznik { get; set; }
        public string Popis { get; set; }
        public int NarokovanaVysePojistky { get; set; }
        public List<Zavazek> Zavazky { get; set; } = new List<Zavazek>();
    }
}