using Informacni_System_Pojistovny.Models.Dao.BlogPostApi;
using Informacni_System_Pojistovny.Models.Entity;
using Oracle.ManagedDataAccess.Client;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class KlientModel
    {
        private Db db = new Db();
        public List<Klient> klients() {
            List<Klient> klients = new List<Klient>();
            //SELECT fyzickych osob
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = "select * from Fyzicke_osoby f JOIN klienti k on f.klient_id = k.klient_id";
            cmd.Connection = db.Connection;
            cmd.Connection.Open();
            OracleDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    FyzickaOsoba fyzickaOsoba = new FyzickaOsoba();
                    fyzickaOsoba.KlientId = int.Parse(dr["KLIENT_ID"].ToString());
                    fyzickaOsoba.Jmeno = dr["JMENO"].ToString();
                    fyzickaOsoba.Prijmeni = dr["PRIJMENI"].ToString();
                    string stav = dr["STAV"].ToString();
                    if (stav != null && stav.Equals("a"))
                    {
                        fyzickaOsoba.Stav = true;
                    }
                    else
                    {
                        fyzickaOsoba.Stav = false;
                    }
                    fyzickaOsoba.Email = dr["EMAIL"].ToString();
                    fyzickaOsoba.Telefon = dr["TELEFON"].ToString();
                    fyzickaOsoba.RodneCislo = dr["RODNE_CISLO"].ToString();
                    klients.Add(fyzickaOsoba);
                }
            }
            else
            {

            }
            db.Dispose();
            //SELECT pravnickych osob
            return klients;
        }
    }
}
