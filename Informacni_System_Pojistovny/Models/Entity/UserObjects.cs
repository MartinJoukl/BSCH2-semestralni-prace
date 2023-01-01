using System.ComponentModel.DataAnnotations;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class UserObjects
    {
       [Display(Name = "Jméno")]
       public string Name { get; set; }
       [Display(Name = "Typ")]
       public string Type { get; set; }
    }
}
