using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class PojistnyProdukt
    {
        public int ID { get; set; }
        [Display(Name = "Název")]
        public string Nazev { get; set; }
        [Display(Name = "Popis")]
        public string Popis { get; set; }
        [Display(Name = "Maximální výše plnění")]
        public int MaximalniVysePlneni { get; set; }
        public bool Status { get; set; }
    }
}
