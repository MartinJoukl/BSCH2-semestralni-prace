using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class PojistnaUdalost
    {
        [Display(Name = "Id")]
        public long PojistnaUdalostId { get; set; }
        [Required]
        public Klient Klient { get; set; }
        [Display(Name = "Vznik pojistky"), DataType(DataType.Date)]
        public DateTime Vznik { get; set; } = DateTime.Now;
        [Display(Name = "Popis"),DataType(DataType.MultilineText)]
        public string Popis { get; set; }
        [Display(Name = "Nárokovaná výše pojistky"), Required, DataType(DataType.Currency),Range(1, long.MaxValue,ErrorMessage ="Minimální nárokovaná hodnota musí být vetší než 0")]
        public long NarokovanaVysePojistky { get; set; }
        public List<Zavazek> Zavazky { get; set; } = new List<Zavazek>();
    }
}