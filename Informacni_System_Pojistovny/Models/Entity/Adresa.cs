namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Adresa
    {
        public int AdresaId { get; set; }
        public int CisloPopisne { get; set; }
        public string Ulice { get; set; }
        public Pobocka? Pobocka { get; set; }
        public Psc Psc { get; set; }
        public Klient? Klient { get; set; }
    }
}