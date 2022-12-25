namespace Informacni_System_Pojistovny.Models.Model.ZavazekModels
{
    using Informacni_System_Pojistovny.Models.Dao;
    using Informacni_System_Pojistovny.Models.Entity;
    using Oracle.ManagedDataAccess.Client;
    public class ZavazekModel
    {
        private readonly Db db;
        public ZavazekModel(Db db)
        {
            this.db = db;
        }
        public List<Zavazek> ListZavazek()
        {
            List<Zavazek> list = new List<Zavazek>();

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from zavazky_view JOIN pojistne_udalosti_view using (POJISTNA_UDALOST_ID)");
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    DateTime.TryParse(dr["Datum_splaceni"].ToString(), out DateTime DatumSplaceni);
                    DateTime? nullableDatumSplaceni = DatumSplaceni;
                    if(nullableDatumSplaceni.Equals(DateTime.MinValue) ) {
                        nullableDatumSplaceni = null;
                    }
                    //map data
                    Zavazek zavazek = new Zavazek()
                    {
                        ZavazekId = int.Parse(dr["Zavazek_id"].ToString()),
                        Vyse = int.Parse(dr["Vyse"].ToString()),
                        Vznik = DateTime.Parse(dr["vznik"].ToString()),
                        DatumSplatnosti = DateTime.Parse(dr["Datum_splatnosti"].ToString()),
                        DatumSplaceni = nullableDatumSplaceni,
                        Popis = dr["popis"].ToString(),
                        PojistnaUdalost = new()
                        {
                            Klient = null, //TODO klient
                            NarokovanaVysePojistky = int.Parse(dr["NAROKOVANA_VYSE_POJISTKY"].ToString()),
                            Vznik = DateTime.Parse(dr["vznik"].ToString()),
                            Popis = dr["popis"].ToString(),
                            PojistnaUdalostId = int.Parse(dr["pojistna_Udalost_Id"].ToString()),
                        }
                    };
                    list.Add(zavazek);
                }
            }
            db.Dispose();
            return list;
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

        public void UpdatePojistnaUdalost(int id, PojistnaUdalost pojistnaUdalost)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "p_udalost_id", id },
                { "p_klient_id", pojistnaUdalost.Klient.KlientId },
                { "p_vznik", pojistnaUdalost.Vznik.ToString("dd-MM-yyyy")},
                { "p_popis", pojistnaUdalost.Popis },
                { "p_narokovana_vyse_pojistky", pojistnaUdalost.NarokovanaVysePojistky }
            };

            db.ExecuteNonQuery("UPDATE_POJISTNA_UDALOST", parameters, false, true);
            db.Dispose();
        }

        public Zavazek? GetZavatekUdalost(int id)
        {
            Zavazek? zavazek = null;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["id"] = id;
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from zavazky_view JOIN pojistne_udalosti_view using (POJISTNA_UDALOST_ID) where zavazek_id");
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    zavazek = new Zavazek()
                    {
                        ZavazekId = int.Parse(dr["Zavazek_id"].ToString()),
                        Vyse = int.Parse(dr["Vyse"].ToString()),
                        Vznik = DateTime.Parse(dr["vznik"].ToString()),
                        DatumSplatnosti = DateTime.Parse(dr["Datum_splatnosti"].ToString()),
                        DatumSplaceni = DateTime.Parse(dr["Datum_splaceni"].ToString()),
                        Popis = dr["popis"].ToString(),
                        PojistnaUdalost = new()
                        {
                            Klient = null, //TODO klient
                            NarokovanaVysePojistky = int.Parse(dr["NAROKOVANA_VYSE_POJISTKY"].ToString()),
                            Vznik = DateTime.Parse(dr["vznik"].ToString()),
                            Popis = dr["popis"].ToString(),
                            PojistnaUdalostId = int.Parse(dr["pojistna_Udalost_Id"].ToString()),
                        }
                    };
                }
            }
            db.Dispose();
            return zavazek;
        }
    }
}

