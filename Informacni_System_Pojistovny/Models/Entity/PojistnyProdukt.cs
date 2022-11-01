namespace Informacni_System_Pojistovny.Models.Entity
{
    public class PojistnyProdukt
    {
        public int ID { get; set; }
        public string Nazev { get; set; }
        public string Popis { get; set; }
        public string MaximalniVysePlneni { get; set; }
        public bool Status { get; set; }
    }
}
