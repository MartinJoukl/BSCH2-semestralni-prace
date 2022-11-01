namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Zavazek
    {
        public int ZavazekId { get; set; }
        public DateTime Vznik { get; set; }
        public DateTime DatumSplatnosti { get; set; }
        public int Vyse { get; set; }
        public DateTime DatumSplaceni { get; set; }
        public string Popis { get; set; }
        public PojistnaUdalost PojistnaUdalost { get; set; };
    }
}