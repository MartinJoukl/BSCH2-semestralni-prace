using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class PravnickaOsoba : Klient
    {
        [Display(Name = "Název")]
        public string Nazev { get; set; }
        [Display(Name = "Ičo")]
        public string Ico { get; set; }

        public override string ToString()
        {
            return Nazev;
        }
    }
}
