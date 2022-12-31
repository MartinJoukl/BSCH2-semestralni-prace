using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model.ZavazekModels;
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
        public List<PojistnaUdalost> ListPojistnaUdalost(int id = 0)
        {
            Db db = new Db();
            List<PojistnaUdalost> list = new List<PojistnaUdalost>();
            OracleDataReader dr;

            if (id == 0) { 
                dr = db.ExecuteRetrievingCommand("select * from pojistne_udalosti_view JOIN VIEW_VSECHNY_OSOBY ON klient_id = klient_klient_id");
            } else
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(":id", id);
                dr = db.ExecuteRetrievingCommand("select * from pojistne_udalosti_view JOIN VIEW_VSECHNY_OSOBY ON klient_id = klient_klient_id where klient_klient_id = :id", parameters);
            }

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Klient klient = ReadKlient(dr);
                    //map data
                    PojistnaUdalost pojistnaUdalost = new PojistnaUdalost()
                    {
                        Klient = klient,
                        NarokovanaVysePojistky = long.Parse(dr["NAROKOVANA_VYSE_POJISTKY"].ToString()),
                        Vznik = DateTime.Parse(dr["vznik"].ToString()),
                        Popis = dr["popis"].ToString(),
                        PojistnaUdalostId = long.Parse(dr["pojistna_Udalost_Id"].ToString()),
                    };
                    list.Add(pojistnaUdalost);
                }
            }
            db.Dispose();
            return list;
        }
        public List<PojistnaUdalost> ListPojistnaUdalost(PageInfo pageInfo, string currentFilter)
        {
            List<PojistnaUdalost> list = new List<PojistnaUdalost>();
            int pageStart = pageInfo.PageIndex * pageInfo.PageSize;
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":pageStart", pageStart },
                { ":pageSize", pageInfo.PageSize }
            };

            string sql;

            if (currentFilter != null)
            {
                sql = "select * from pojistne_udalosti_view JOIN VIEW_VSECHNY_OSOBY ON klient_id = klient_klient_id where popis like '%' || :currentFilter || '%' OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
                parameters.Add(":currentFilter", currentFilter);
            }
            else
            {
                sql = "select * from pojistne_udalosti_view JOIN VIEW_VSECHNY_OSOBY ON klient_id = klient_klient_id  OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
            }

            OracleDataReader dr = db.ExecuteRetrievingCommand(sql, parameters, true);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Klient klient = ReadKlient(dr);
                    //map data
                    PojistnaUdalost pojistnaUdalost = new PojistnaUdalost()
                    {
                        Klient = klient,
                        NarokovanaVysePojistky = long.Parse(dr["NAROKOVANA_VYSE_POJISTKY"].ToString()),
                        Vznik = DateTime.Parse(dr["vznik"].ToString()),
                        Popis = dr["popis"].ToString(),
                        PojistnaUdalostId = long.Parse(dr["pojistna_Udalost_Id"].ToString()),
                    };
                    list.Add(pojistnaUdalost);
                }
            }
            db.Dispose();
            return list;
        }

        public long GetCount(string currentFilter)
        {
            long count = 0;
            Db db = new Db();

            OracleDataReader dr;
            if (currentFilter != null)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object> {
                    { ":currentFilter", currentFilter }
                };
                dr = db.ExecuteRetrievingCommand("select count(*) as count from pojistne_udalosti_view where popis like '%' || :currentFilter || '%'", parameters, true);
            }
            else
            {
                dr = db.ExecuteRetrievingCommand("select count(*) as count from pojistne_udalosti_view");
            }

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    count = long.Parse(dr["count"].ToString());
                }
            }
            dr.Close();
            db.Dispose();
            return count;
        }

        public void CreatePojistnaUdalost(PojistnaUdalost pojistnaUdalost)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "p_klientId", pojistnaUdalost.Klient.KlientId },
                { "p_vznik", pojistnaUdalost.Vznik.ToString("dd-MM-yyyy")},
                { "p_popis", pojistnaUdalost.Popis },
                { "p_narokovana_vyse_pojistky", pojistnaUdalost.NarokovanaVysePojistky }
            };

            db.ExecuteNonQuery("CREATE_POJISTNA_UDALOST", parameters, false, true);
            db.Dispose();
        }

        public void UpdatePojistnaUdalost(int id, PojistnaUdalostEditModel pojistnaUdalost)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "p_udalost_id", id },
                { "p_klient_id", pojistnaUdalost.KlientId },
                { "p_vznik", pojistnaUdalost.Vznik.ToString("dd-MM-yyyy")},
                { "p_popis", pojistnaUdalost.Popis },
                { "p_narokovana_vyse_pojistky", pojistnaUdalost.NarokovanaVysePojistky }
            };

            db.ExecuteNonQuery("UPDATE_POJISTNA_UDALOST", parameters, false, true);
            db.Dispose();
        }

        public void DeletePojistnaUdalost(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "p_udalost_id", id },
            };

            db.ExecuteNonQuery("DELETE_POJISTNA_UDALOST", parameters, false, true);
            db.Dispose();
        }

        public PojistnaUdalost GetPojistnaUdalost(int id)
        {
            ZavazekModel zavazekModel = new ZavazekModel(new Db());
            PojistnaUdalost pojistnaUdalost = null;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters[":id"] = id;
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
                        NarokovanaVysePojistky = long.Parse(dr["NAROKOVANA_VYSE_POJISTKY"].ToString()),
                        Vznik = DateTime.Parse(dr["vznik"].ToString()),
                        Popis = dr["popis"].ToString(),
                        PojistnaUdalostId = long.Parse(dr["pojistna_Udalost_Id"].ToString()),
                        Zavazky = zavazekModel.ListZavazek(id)
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
