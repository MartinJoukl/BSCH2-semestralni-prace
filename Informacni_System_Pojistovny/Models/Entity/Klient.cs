using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Klient
    {
        [Display(Name = "Id klienta"), Key]
        public int KlientId { get; set; }
        public string CeleJmeno => ToString();
        public bool Stav { get; set; }
        public List<HistorieClenstvi> HistorieClenstvi { get; set; } = new List<HistorieClenstvi>();
        public List<Adresa> Adresy { get; set; } = new List<Adresa>();
        public List<PojistnaUdalost> PojistneUdalosti { get; set; } = new List<PojistnaUdalost>();
        public List<Pojistka> Pojistky { get; set; } = new List<Pojistka>();
        public List<Dokument> Dokumenty { get; set; } = new List<Dokument>();
        [Display(Name = "Suma nesplacených pohledávek klienta k pojišťovně po termínu")]
        public int NesplacenePohledavkyPoTerminu { get; set; }
        [Display(Name = "Suma nesplacených závazků pojišťovny ke klientovi po termínu")]
        public int NesplaceneZavazkyPoTerminu { get; set; }
        [Display(Name = "Suma pohledávek klienta k pojišťovně nad 20k po termínu")]
        public int PohledavkyNad20k { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Klient klient &&
                   KlientId == klient.KlientId &&
                   CeleJmeno == klient.CeleJmeno &&
                   Stav == klient.Stav;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(KlientId, CeleJmeno, Stav);
        }
    }
}
