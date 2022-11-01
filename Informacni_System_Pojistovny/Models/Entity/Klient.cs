namespace Informacni_System_Pojistovny.Models.Entity
{
    public abstract class Klient
    {
        public int KlientId { get; set; }
        public bool Stav { get; set; }
        public List<HistorieClenstvi> HistorieClenstvi { get; set; } = new List<HistorieClenstvi>();
        public List<Adresa> Adresy { get; set; } = new List<Adresa>();
        public List<PojistnaUdalost> PojistneUdalosti { get; set; } = new List<PojistnaUdalost>();
        public List<Pojistka> Pojistky { get; set; }
    }
}
