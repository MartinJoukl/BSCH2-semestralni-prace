using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Oracle.ManagedDataAccess.Client;

namespace Informacni_System_Pojistovny.Models.Model.PojistnaUdalostModels
{
    public class PojistnaUdalostModel
    {
        private readonly Db db;
        public PojistnaUdalostModel(Db db)
        {
            this.db = db;
        }
        public List<PojistnaUdalost> ListPojistnaUdalost()
        {
            List<PojistnaUdalost> list = new List<PojistnaUdalost>();

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from pojistne_udalosti_view JOIN VIEW_VSECHNY_OSOBY ON klient_id = klient_klient_id");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Klient klient = ReadKlient(dr);
                    //map data
                    PojistnaUdalost pojistnaUdalost = new PojistnaUdalost()
                    {
                        Klient = klient,
                        NarokovanaVysePojistky = int.Parse(dr["NAROKOVANA_VYSE_POJISTKY"].ToString()),
                        Vznik = DateTime.Parse(dr["vznik"].ToString()),
                        Popis = dr["popis"].ToString(),
                        PojistnaUdalostId = int.Parse(dr["pojistna_Udalost_Id"].ToString()),
                    };
                    list.Add(pojistnaUdalost);
                }
            }
            db.Dispose();
            return list;
        }

        public PojistnaUdalost GetPojistnaUdalost(int id)
        {
            PojistnaUdalost pojistnaUdalost = null;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["id"] = id;
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from pojistne_udalosti_view JOIN VIEW_VSECHNY_OSOBY ON klient_id = klient_klient_id where pojistna_Udalost_Id = :id", parameters);
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    Klient klient = ReadKlient(dr);
                    //map data
                    pojistnaUdalost = new PojistnaUdalost()
                    {
                        Klient = klient,
                        NarokovanaVysePojistky = int.Parse(dr["NAROKOVANA_VYSE_POJISTKY"].ToString()),
                        Vznik = DateTime.Parse(dr["vznik"].ToString()),
                        Popis = dr["popis"].ToString(),
                        PojistnaUdalostId = int.Parse(dr["pojistna_Udalost_Id"].ToString()),
                    };
                }
            }
            db.Dispose();
            return pojistnaUdalost;
        }

        private Klient ReadKlient(OracleDataReader dr)
        {
            string typKlienta = dr["typ_klienta"].ToString();
            Klient klient;
            if (typKlienta.Equals("F"))
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
                klient = fyzickaOsoba;
            }
            else
            {
                PravnickaOsoba pravnickaOsoba = new PravnickaOsoba();
                pravnickaOsoba.KlientId = int.Parse(dr["KLIENT_ID"].ToString());
                pravnickaOsoba.Nazev = dr["NAZEV"].ToString();
                string stav = dr["STAV"].ToString();
                if (stav != null && stav.Equals("a"))
                {
                    pravnickaOsoba.Stav = true;
                }
                else
                {
                    pravnickaOsoba.Stav = false;
                }
                pravnickaOsoba.Ico = dr["ICO"].ToString();
                klient = pravnickaOsoba;
            }
            return klient;
        }
    }
}
