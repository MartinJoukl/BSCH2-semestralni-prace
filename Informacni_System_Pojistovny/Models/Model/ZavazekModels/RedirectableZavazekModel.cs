using Informacni_System_Pojistovny.Models.Entity;

namespace Informacni_System_Pojistovny.Models.Model.ZavazekModels
{
    public class RedirectableZavazek
    {
        public Zavazek Zavazek { get; set; }
        public string? RedirectedFrom { get; set; }
    }
}
