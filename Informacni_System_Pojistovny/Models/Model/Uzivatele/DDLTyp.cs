using System.ComponentModel;

namespace Informacni_System_Pojistovny.Models.Model.Uzivatele
{
    public class DMLTypeRetriever
    {
        public static DMLTyp GetByName(string name)
        {
            DMLTyp ddlTyp;

            switch (name.ToUpper())
            {
                case "INSERT":
                    ddlTyp = DMLTyp.Insert;
                    break;
                case "UPDATE":
                    ddlTyp = DMLTyp.Update;
                    break;
                case "DELETE":
                    ddlTyp = DMLTyp.Delete;
                    break;
                default: throw new Exception("This DDL doesn't exist");
            }
            return ddlTyp;
        }
    }
}
public enum DMLTyp
{
    [Description("Insert")]
    Insert,
    [Description("Update")]
    Update,
    [Description("Delete")]
    Delete
}
