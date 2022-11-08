using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class UzivatelLoginFormModel
    {
        [Required(ErrorMessage = "Mail musí být vyplněn"), StringLength(60, MinimumLength = 1), Display(Name = "Mail")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "Heslo musí být vyplněno"), StringLength(60, MinimumLength = 1), Display(Name = "Heslo"), DataType(DataType.Password)]
        public string Heslo { get; set; }
    }
}
