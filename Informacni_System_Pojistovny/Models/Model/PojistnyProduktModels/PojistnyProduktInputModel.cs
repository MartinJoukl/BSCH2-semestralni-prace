using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Globalization;

namespace Informacni_System_Pojistovny.Models.Model.PojistnyProduktModels
{
    public class PojistnyProduktInputModel
    {
        [Display(Name = "Název")]
        [Required(ErrorMessage = "Název musí být vyplněn")]
        public string Nazev { get; set; }
        [Display(Name = "Popis")]
        [Required(ErrorMessage = "Popis musí být vyplněn")]
        public string Popis { get; set; }
        [Display(Name = "Maximální výše plnění")]
        [Required(ErrorMessage = "Maximální výše plnění musí být vyplněna")]
        [Range(1, int.MaxValue, ErrorMessage = "Maximální výše plnění musí být vyšší než 0")]
        public int MaximalniVysePlneni { get; set; }
    }
}
