using Informacni_System_Pojistovny.Models.Dao.BlogPostApi;
using Informacni_System_Pojistovny.Models.Entity;
using Oracle.ManagedDataAccess.Client;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class UzivatelModel
    {
        public static List<Uzivatel> ListUzivatel()
        {
            List<Uzivatel> list = new List<Uzivatel>();

            Db db = new Db();
            OracleDataReader dr = db.executeRetrievingCommand("select * from uzivatele");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string mail = dr["mail"]?.ToString();
                    string jmeno = dr["jmeno"]?.ToString();
                    string prijmeni = dr["prijmeni"]?.ToString();
                    string urovenOpravneniString = dr["uroven_opravneni"]?.ToString();
                    string idString = dr["id"]?.ToString();
                    int.TryParse(idString, out var id);
                    int.TryParse(urovenOpravneniString, out var urovenOpravneni);
                    list.Add(new Uzivatel { Mail = mail, Jmeno = jmeno, Prijmeni = prijmeni, UrovenOpravneni = urovenOpravneni, ID = id });
                }
            }
            db.Dispose();
            return list;
        }
    }
}
