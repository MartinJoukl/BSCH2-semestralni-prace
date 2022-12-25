using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class FyzickaOsoba : Klient
    {
        [Display(Name = "Jméno")]
        public string Jmeno { get; set; }
        [Display(Name = "Příjmení")]
        public string Prijmeni { get; set; }
        [Display(Name = "Rodné číslo")]
        public string RodneCislo { get; set; }
        [Display(Name = "Telefon")]
        public string Telefon { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }

        public override string ToString()
        {
            return $"{Jmeno} {Prijmeni}";
        }
    }
}
