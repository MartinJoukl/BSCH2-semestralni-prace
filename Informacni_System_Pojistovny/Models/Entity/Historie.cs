using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class Historie
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        [Display(Name = "Jméno změněné entity"), DataType(DataType.Text)]
        public string JmenoPredmetu { get; set; }
        [Display(Name = "Typ operace"), DataType(DataType.Text)]
        public DMLTyp DML { get; set; }
        [Display(Name = "Čas změny"), DataType(DataType.Date)]
        public DateTime CasZmeny { get; set; }
    }
}
