using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class HistorieClenstvi
    {
        [Display(Name ="ID")]
        public int HistorieClenstviId { get; set; }
        [Display(Name = "Od"), DataType(DataType.Date)]
        public DateTime Od { get; set; }
        [Display(Name = "Do"), DataType(DataType.Date)]
        public DateTime? Do { get; set; }
        public Klient Klient { get; set; }
    }
}