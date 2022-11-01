namespace Informacni_System_Pojistovny.Models.Entity
{
    public class HistorieClenstvi
    {
        public int HistorieClenstviId { get; set; }
        public DateTime Od { get; set; }
        public DateTime Do { get; set; }
        public Klient Klient { get; set; }
    }
}