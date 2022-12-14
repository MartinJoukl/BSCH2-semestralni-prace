using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Pobocka
    {
        public int PobockaId { get; set; }
        [StringLength(200, ErrorMessage = "Maximální délka je 200 znaků")]
        [Display(Name = "Název pobočky")]
        public string Nazev { get; set; }
        public Adresa Adresa { get; set; }
    }
}