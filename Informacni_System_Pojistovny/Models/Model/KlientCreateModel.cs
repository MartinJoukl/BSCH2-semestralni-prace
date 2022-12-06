namespace Informacni_System_Pojistovny.Models.Model
{
    using Informacni_System_Pojistovny.Models.Entity;
    using System.ComponentModel.DataAnnotations;
    //DAO
    public class KlientCreateModel
    {
        [Display(Name = "Název"), StringLength(200), Required(ErrorMessage = "Název musí být vyplněn", AllowEmptyStrings = false)]
        public string Nazev { get; set; }
        [Display(Name = "Ičo"), StringLength(8, MinimumLength = 8, ErrorMessage = "Ičo má 8 znaků"), Required(ErrorMessage = "Ičo musí být vyplněno", AllowEmptyStrings = false)]
        public string Ico { get; set; }
        [Display(Name = "Jméno"), Required(ErrorMessage = "Jméno musí být vyplněno", AllowEmptyStrings = false)]
        public string Jmeno { get; set; }
        [Display(Name = "Příjmení"), StringLength(200), Required(ErrorMessage = "Příjmení číslo musí být vyplněno", AllowEmptyStrings = false)]
        public string Prijmeni { get; set; }
        [Required(ErrorMessage = "Rodné číslo musí být vyplněno", AllowEmptyStrings = false), StringLength(10, MinimumLength = 9, ErrorMessage = "Rodné číslo má 9 nebo 10 znaků")]
        [Display(Name = "Rodné číslo")]
        public string RodneCislo { get; set; }
        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Telefon musí být vyplněn", AllowEmptyStrings = false), RegularExpression(@"^([0-9]{9})$", ErrorMessage = "Telefonní číslo není správně vyplněno")]
        public string Telefon { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email musí být vyplněn", AllowEmptyStrings = false), StringLength(200)]
        [EmailAddress(ErrorMessage = "Emailová adresa není validní")]
        public string Email { get; set; }

        public KlientCreateModel LegalPersonToKlientCreateModel(PravnickaOsoba pravnickaOsoba) { 
            KlientCreateModel klientCreateModel= new KlientCreateModel();
            //TODO
            return klientCreateModel;
        }
    }

}
