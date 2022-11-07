using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class UzivatelLoginFormModel
    {
        [Required(ErrorMessage = "Mail musí být vyplněn")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "Heslo musí být vyplněno")]
        [DataType(DataType.Password)]
        public string Heslo { get; set; }
    }
}
