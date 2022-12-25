using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class PscEditModel
    {
        [StringLength(200, ErrorMessage = "Maximální délka je 200 znaků")]
        [Display(Name = "Město")]
        public string Mesto { get; set; }
    }
}
