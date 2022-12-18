using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class EditOwnProfileModel : EditUserModel
    {
        [StringLength(60, MinimumLength = 0), Display(Name = "Heslo"), DataType(DataType.Password)]
        public new string Heslo { get; set; }
    }
}
