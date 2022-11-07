namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Uzivatel
    {
        public string Mail { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public int UrovenOpravneni { get; set; }

        public string HesloHash { get; set; }
        public string Salt { get; set; }
        public int ID { get; set; }
    }
}
