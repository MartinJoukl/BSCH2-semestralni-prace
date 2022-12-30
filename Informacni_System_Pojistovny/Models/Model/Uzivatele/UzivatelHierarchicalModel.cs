using Informacni_System_Pojistovny.Models.Entity;
using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Model.Uzivatele
{
    public class UzivatelHierarchicalModel
    {
        [Display(Name = "Email"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Jméno"), DataType(DataType.Text)]
        public string Jmeno { get; set; }
        [Display(Name = "Příjmení"), DataType(DataType.Text)]
        public string Prijmeni { get; set; }
        [Display(Name = "Role"), DataType(DataType.Text)]
        public UzivateleRole Role { get; set; }
        [Display(Name = "Manažer"), DataType(DataType.Text)]
        public Uzivatel? Manazer { get; set; }
        public int Id { get; set; }
        [Display(Name = "Úroveň")]
        public int? Uroven { get; set; }
    }
}
