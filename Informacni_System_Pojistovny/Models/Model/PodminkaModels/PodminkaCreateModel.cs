using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Informacni_System_Pojistovny.Models.Model.PodminkaModels
{
    public class PodminkaCreateModel
    {
        [Display(Name = "Popis")]
        [Required(ErrorMessage = "Popis musí být vyplněn", AllowEmptyStrings = false)]
        public string Popis { get; set; }
    }
}
