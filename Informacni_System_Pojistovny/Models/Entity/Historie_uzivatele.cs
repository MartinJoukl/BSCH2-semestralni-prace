using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Informacni_System_Pojistovny.Models.Entity
{
    public class HistorieUzivatele
    {
        public int Id { get; set; }

        [Display(Name = "Starý Email"), DataType(DataType.EmailAddress)]
        public string OldEmail { get; set; }
        [Display(Name = "Staré Jméno"), DataType(DataType.Text)]
        public string OldJmeno { get; set; }
        [Display(Name = "Staré Příjmení"), DataType(DataType.Text)]
        public string OldPrijmeni { get; set; }
        [Display(Name = "Stará Role"), DataType(DataType.Text)]
        public UzivateleRole OldRole { get; set; }
        [Display(Name = "Staré Id"), DataType(DataType.Date)]
        public int OldId { get; set; }
        [Display(Name = "Starý čas změny"), DataType(DataType.Date)]
        public DateTime? OldcasZmeny { get; set; }


        [Display(Name = "Nový Email"), DataType(DataType.EmailAddress)]
        public string NewEmail { get; set; }
        [Display(Name = "Nové Jméno"), DataType(DataType.Text)]
        public string NewJmeno { get; set; }
        [Display(Name = "Nové Příjmení"), DataType(DataType.Text)]
        public string NewPrijmeni { get; set; }
        [Display(Name = "Nová role"), DataType(DataType.Text)]
        public UzivateleRole NewRole { get; set; }
        [Display(Name = "Nové Id"), DataType(DataType.Date)]
        public int NewId { get; set; }
        [Display(Name = "Nový čas změny"), DataType(DataType.Date)]
        public DateTime? NewCasZmeny { get; set; }
        [Display(Name = "Čas provedení operace"), DataType(DataType.Date)]
        public DateTime CasHistorie { get; set; }
        [Display(Name = "Typ DML")]
        public DMLTyp DDLTyp;
    }
}
