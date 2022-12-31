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
                    if (nullableDatumSplaceni.Equals(DateTime.MinValue))
                    {
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

        public List<Zavazek> ListZavazek(PageInfo pageInfo, string currentFilter = null)
        {
            List<Zavazek> list = new List<Zavazek>();
            string sql;

            int pageStart = pageInfo.PageIndex * pageInfo.PageSize;
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":pageStart", pageStart },
                { ":pageSize", pageInfo.PageSize }
            };


            if (currentFilter != null)
            {
                sql = "select * from zavazky_view JOIN pojistne_udalosti_view using (POJISTNA_UDALOST_ID) where zavazky_view.popis like '%' || :currentFilter || '%' OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
                parameters.Add(":currentFilter", currentFilter);
            }
            else
            {
                sql = "select * from zavazky_view JOIN pojistne_udalosti_view using (POJISTNA_UDALOST_ID) OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
            }

            OracleDataReader dr = db.ExecuteRetrievingCommand(sql, parameters, true);
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    DateTime.TryParse(dr["Datum_splaceni"].ToString(), out DateTime DatumSplaceni);
                    DateTime? nullableDatumSplaceni = DatumSplaceni;
                    if (nullableDatumSplaceni.Equals(DateTime.MinValue))
                    {
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

        public long GetCount(string currentFilter = null)
        {
            long count = 0;
            Db db = new Db();

            OracleDataReader dr;
            if (currentFilter != null)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object> {
                    { ":currentFilter", currentFilter }
                };
                dr = db.ExecuteRetrievingCommand("select count(*) as count from zavazky_view where popis like '%' || :currentFilter || '%'", parameters, true);
            }
            else
            {
                dr = db.ExecuteRetrievingCommand("select count(*) as count from zavazky_view");
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

        public List<Zavazek> ListZavazek(int idPojistnaUdalost)
        {
            List<Zavazek> list = new List<Zavazek>();
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":id", idPojistnaUdalost }
            };
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from zavazky_view JOIN pojistne_udalosti_view using (POJISTNA_UDALOST_ID) where POJISTNA_UDALOST_ID = :id",parameters);
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    DateTime.TryParse(dr["Datum_splaceni"].ToString(), out DateTime DatumSplaceni);
                    DateTime? nullableDatumSplaceni = DatumSplaceni;
                    if (nullableDatumSplaceni.Equals(DateTime.MinValue))
                    {
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

        public void CreateZavazek(ZavazekCreateModel zavazek)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "p_vznik", zavazek.Vznik.ToString("dd-MM-yyyy") },
                { "p_datum_splatnosti", zavazek.DatumSplatnosti.ToString("dd-MM-yyyy")},
                { "p_vyse", zavazek.Vyse },
                { "p_datum_splaceni", zavazek.DatumSplaceni?.ToString("dd-MM-yyyy") },
                { "p_popis", zavazek.Popis },
                { "p_pojistna_udalost_id", zavazek.PojistnaUdalostId }
            };

            db.ExecuteNonQuery("CREATE_ZAVAZEK", parameters, false, true);
            db.Dispose();
        }

        public void UpdateZavazek(int id, RedirectableZavazekModel zavazek)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "p_zavazek_id", id },
                { "p_vznik", zavazek.Vznik.ToString("dd-MM-yyyy") },
                { "p_datum_splatnosti", zavazek.DatumSplatnosti.ToString("dd-MM-yyyy")},
                { "p_vyse", zavazek.Vyse },
                { "p_datum_splaceni", zavazek.DatumSplaceni != null ? ((DateTime)zavazek.DatumSplaceni).ToString("dd-MM-yyyy"): null },
                { "p_popis", zavazek.Popis }
            };

            db.ExecuteNonQuery("UPDATE_ZAVAZEK", parameters, false, true);
            db.Dispose();
        }

        public void DeleteZavazek(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "p_zavazek_id", id },
            };

            db.ExecuteNonQuery("DELETE_ZAVAZEK", parameters, false, true);
            db.Dispose();
        }

        public Zavazek? GetZavazekUdalost(int id)
        {
            Zavazek? zavazek = null;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters[":id"] = id;
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from zavazky_view JOIN pojistne_udalosti_view using (POJISTNA_UDALOST_ID) where zavazek_id = :id", parameters);
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    DateTime.TryParse(dr["Datum_splaceni"].ToString(), out DateTime DatumSplaceni);
                    DateTime? nullableDatumSplaceni = DatumSplaceni;
                    if (nullableDatumSplaceni.Equals(DateTime.MinValue))
                    {
                        nullableDatumSplaceni = null;
                    }

                    zavazek = new Zavazek()
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
                }
            }
            db.Dispose();
            return zavazek;
        }
    }
}

