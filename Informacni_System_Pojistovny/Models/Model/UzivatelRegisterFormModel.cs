using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class UzivatelRegisterFormModel
    {
        [Required(ErrorMessage = "Mail musí být vyplněn"), StringLength(60, MinimumLength = 1), Display(Name = "Mail"), DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Emailová adresa není validní")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "Heslo musí být vyplněno"), StringLength(60, MinimumLength = 1), Display(Name = "Heslo"), DataType(DataType.Password)]
        public string Heslo { get; set; }
        [Required(ErrorMessage = "Kontrola hesla musí být vyplněna"), StringLength(60, MinimumLength = 1), Display(Name = "Heslo znovu"), DataType(DataType.Password)]
        public string HesloZnovu { get; set; }
        [Required(ErrorMessage = "Jméno musí být vyplněno"), StringLength(60, MinimumLength = 1), Display(Name = "Jméno"), DataType(DataType.Text)]

        public string Jmeno { get; set; }
        [Required(ErrorMessage = "Příjmení"), StringLength(60, MinimumLength = 1), Display(Name = "Příjmení"), DataType(DataType.Text)]
        public string Prijmeni { get; set; }
    }
}
