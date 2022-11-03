namespace Informacni_System_Pojistovny.Models.Entity
{
    public class FyzickaOsoba : Klient
    {
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string RodneCislo { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
    }
}
