using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Uzivatel
    {
        [Display(Name = "Email"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Jméno"), DataType(DataType.Text)]
        public string Jmeno { get; set; }
        [Display(Name = "Příjmení"), DataType(DataType.Text)]
        public string Prijmeni { get; set; }
        [Display(Name = "Role"), DataType(DataType.Text)]
        public UzivateleRole Role { get; set; }

        public string HesloHash { get; set; }
        public string Salt { get; set; }
        public int Id { get; set; }
        [Display(Name = "Čas změny"), DataType(DataType.Date)]
        public DateTime casZmeny { get; set; }
    }
}
