using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Model.Pojistka
{
    public class PojistkaAddConditionModel
    {
        [Display(Name = "Podmínka")]
        public int PodminkaId { get; set; }
    }
}
