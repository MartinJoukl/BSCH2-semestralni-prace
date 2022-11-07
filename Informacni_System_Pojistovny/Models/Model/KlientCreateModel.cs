namespace Informacni_System_Pojistovny.Models.Model
{
    using System.ComponentModel.DataAnnotations;
    //DAO
    public class KlientCreateModel
    {
        [Display(Name = "Název")]
        public string Nazev { get; set; }
        [Display(Name = "Ičo")]
        public string Ico { get; set; }
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
    }
}
