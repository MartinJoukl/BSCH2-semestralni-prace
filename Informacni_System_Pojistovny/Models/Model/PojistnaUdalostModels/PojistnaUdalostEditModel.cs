using Informacni_System_Pojistovny.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Model.PojistnaUdalostModels
{
    public class PojistnaUdalostEditModel
    {
        [Display(Name = "Id")]
        public int PojistnaUdalostId { get; set; }
        [Required]
        public string KlientId { get; set; }
        [Display(Name = "Vznik pojistky"), DataType(DataType.Date)]
        public DateTime Vznik { get; set; } = DateTime.Now;
        [Display(Name = "Popis"), DataType(DataType.MultilineText)]
        public string Popis { get; set; }
        [Display(Name = "Nárokovaná výše pojistky"), Required, DataType(DataType.Currency), Range(1, long.MaxValue, ErrorMessage = "Minimální nárokovaná hodnota musí být vetší než 0")]
        public long NarokovanaVysePojistky { get; set; }
    }
}
