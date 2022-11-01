namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Pojistka
    {
        public PojistnyProdukt PojistnyProdukt { get; set; }
        public DateTime Sjednano { get; set; }
        public int Poplatek { get; set; }
        public bool Status { get; set; }
        public int SjednanaVyse { get; set; }
        public int ID { get; set; }
        public List<Podminky> Podminky { get; set; } = new List<Podminky>();

        public Klient Klient { get; set; }
    }
}
