using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class EditUserModel
    {
        [StringLength(60, MinimumLength = 1), Display(Name = "Mail"), DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Emailová adresa není validní")]
        public string Mail { get; set; }
        [StringLength(60, MinimumLength = 1), Display(Name = "Jméno"), DataType(DataType.Text)]
        public string Jmeno { get; set; }
        [StringLength(60, MinimumLength = 1), Display(Name = "Příjmení"), DataType(DataType.Text)]
        public string Prijmeni { get; set; }
        public int Id { get; set; }
        public virtual string Heslo { get; private set; }
    }
}
