namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Pohledavka
    {
        public Pojistka Pojistka { get; set; }
        public DateTime Vznik { get; set; }
        public DateTime DatumSplatnosti { get; set; }
        public DateTime DatumSplaceni { get; set; }
        public int Vyse { get; set; }
        public int ID { get; set; }
        public string Popis { get; set; }

    }
}