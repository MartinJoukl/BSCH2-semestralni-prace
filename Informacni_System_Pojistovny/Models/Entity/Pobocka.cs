namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Pobocka
    {
        public int PobockaId { get; set; }
        public string Nazev { get; set; }
        public Adresa Adresa { get; set; }
    }
}