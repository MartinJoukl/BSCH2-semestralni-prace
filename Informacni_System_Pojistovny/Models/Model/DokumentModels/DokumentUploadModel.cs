using Informacni_System_Pojistovny.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Model.DokumentModels
{
    public class DokumentUploadModel
    {
        [Display(Name = "Název")]
        [Required(ErrorMessage = "Název je povinné pole")]
        [StringLength(255, ErrorMessage = "Název je povinné pole")]
        public string Nazev { get; set; }
        //[Display(Name = "Klient")]
        //public int KlientId { get; set; }
        [Display(Name = "Dokument")]
        public IFormFile Data { get; set; }
    }
}
