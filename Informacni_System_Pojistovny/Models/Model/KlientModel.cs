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
            Db db = new Db();
            OracleDataReader dr = db.executeRetrievingCommand("select * from Fyzicke_osoby f JOIN klienti k on f.klient_id = k.klient_id");
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
            dr.Close();
            //SELECT pravnickych osob
            OracleDataReader dr2 = db.executeRetrievingCommand("select * from Pravnicke_osoby f JOIN klienti k on f.klient_id = k.klient_id");
            if (dr2.HasRows)
            {
                while (dr2.Read())
                {
                    PravnickaOsoba pravnickaOsoba = new PravnickaOsoba();
                    pravnickaOsoba.KlientId = int.Parse(dr2["KLIENT_ID"].ToString());
                    pravnickaOsoba.Nazev = dr2["NAZEV"].ToString();
                    string stav = dr2["STAV"].ToString();
                    if (stav != null && stav.Equals("a"))
                    {
                        pravnickaOsoba.Stav = true;
                    }
                    else
                    {
                        pravnickaOsoba.Stav = false;
                    }
                    pravnickaOsoba.Ico = dr2["ICO"].ToString();
                    klients.Add(pravnickaOsoba);
                }
            }
            dr2.Close();

            db.Dispose();
            return klients;
        }

        public int CreateClient(IFormCollection collection) {
            Dictionary<string, object> klientParametry = new Dictionary<string, object>();
            klientParametry.Add(":typKlienta", collection["zvolenyTypOsoby"]);

            int klientId = db.executeNonQuery("INSERT INTO KLIENTI (STAV, TYP_KLIENTA) VALUES ('a', :typKlienta) returning klient_id into :id", klientParametry);

            Dictionary<string, object> typOsobyParametry = new Dictionary<string, object>();
            typOsobyParametry.Add(":klient_id", klientId);

            if (collection["zvolenyTypOsoby"].Equals("F"))
            {
                typOsobyParametry.Add(":jmeno", collection["jmeno"]);
                typOsobyParametry.Add(":prijmeni", collection["prijmeni"]);
                typOsobyParametry.Add(":rc", collection["rodneCislo"]);
                typOsobyParametry.Add(":telefon", collection["telefon"]);
                typOsobyParametry.Add(":email", collection["email"]);
                db.executeNonQuery("INSERT into fyzicke_osoby (klient_id, jmeno, prijmeni, rodne_cislo, telefon, email) values (:klient_id, :jmeno, :prijmeni, :rc, :telefon, :email) returning klient_id into :id", typOsobyParametry);
            }
            else {
                typOsobyParametry.Add(":nazev", collection["nazev"]);
                typOsobyParametry.Add(":ico", collection["ico"]);
                db.executeNonQuery("INSERT into pravnicke_osoby (klient_id, nazev, ico) values (:klient_id, :nazev, :ico) returning klient_id into :id", typOsobyParametry);
            }

            db.Dispose();

            return klientId;
        }
    }
}
