using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Pojistka
    {
        public PojistnyProdukt PojistnyProdukt { get; set; }
        [Display(Name = "Datum uzavření pojistky")]
        [DataType(DataType.Date)]
        public DateTime Sjednano { get; set; }
        [Display(Name = "Výše poplatku")]
        public int Poplatek { get; set; }
        public bool Status { get; set; }
        [Display(Name = "Maximální výše plnění")]
        public int SjednanaVyse { get; set; }
        public int ID { get; set; }
        public List<Podminky> Podminky { get; set; } = new List<Podminky>();
        public Klient Klient { get; set; }
    }
}
