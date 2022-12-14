namespace Informacni_System_Pojistovny.Models.Model.PohledavkaModels
{
    using Informacni_System_Pojistovny.Models.Dao;
    using Informacni_System_Pojistovny.Models.Entity;
    using Oracle.ManagedDataAccess.Client;
    public class PohledavkaModel
    {
        private readonly Db db;
        public PohledavkaModel(Db db)
        {
            this.db = db;
        }
        public List<Pohledavka> ListPohledavka()
        {
            List<Pohledavka> list = new List<Pohledavka>();

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from pohledavky_view JOIN VIEW_POJISTKY using (pojistka_id)");
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
                    Pohledavka pohledavka = new Pohledavka()
                    {
                        ID = int.Parse(dr["Pohledavka_id"].ToString()),
                        Vyse = int.Parse(dr["Vyse"].ToString()),
                        Vznik = DateTime.Parse(dr["vznik"].ToString()),
                        DatumSplatnosti = DateTime.Parse(dr["Datum_splatnosti"].ToString()),
                        DatumSplaceni = nullableDatumSplaceni,
                        Popis = dr["popis"].ToString(),
                        Pojistka = new()
                        {
                            Klient = null, //TODO klient
                            ID = int.Parse(dr["pojistka_id"].ToString()),
                            Podminky = null, //TODO podminky
                            PojistnyProdukt = null,
                            Poplatek = int.Parse(dr["poplatek"].ToString()),
                            SjednanaVyse = int.Parse(dr["sjednana_vyse"].ToString()),
                            Status = dr["pojistka_id"].ToString().Equals("a"),
                            Sjednano = DateTime.Parse(dr["sjednano"].ToString()),
                        }
                    };
                    list.Add(pohledavka);
                }
            }
            db.Dispose();
            return list;
        }

        public List<Pohledavka> ListPohledavka(PageInfo pageInfo, string currentFilter)
        {
            List<Pohledavka> list = new List<Pohledavka>();
            string sql;

            int pageStart = pageInfo.PageIndex * pageInfo.PageSize;
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":pageStart", pageStart },
                { ":pageSize", pageInfo.PageSize }
            };

            if (currentFilter != null)
            {
                sql = "select * from pohledavky_view JOIN VIEW_POJISTKY using (pojistka_id) where pohledavky_view.popis like '%' || :currentFilter || '%' order by pohledavka_id OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
                parameters.Add(":currentFilter", currentFilter);
            }
            else
            {
                sql = "select * from pohledavky_view JOIN VIEW_POJISTKY using (pojistka_id) order by pohledavka_id OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
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
                    Pohledavka pohledavka = new Pohledavka()
                    {
                        ID = int.Parse(dr["Pohledavka_id"].ToString()),
                        Vyse = int.Parse(dr["Vyse"].ToString()),
                        Vznik = DateTime.Parse(dr["vznik"].ToString()),
                        DatumSplatnosti = DateTime.Parse(dr["Datum_splatnosti"].ToString()),
                        DatumSplaceni = nullableDatumSplaceni,
                        Popis = dr["popis"].ToString(),
                        Pojistka = new()
                        {
                            Klient = null, //TODO klient
                            ID = int.Parse(dr["pojistka_id"].ToString()),
                            Podminky = null, //TODO podminky
                            PojistnyProdukt = null,
                            Poplatek = int.Parse(dr["poplatek"].ToString()),
                            SjednanaVyse = int.Parse(dr["sjednana_vyse"].ToString()),
                            Status = dr["pojistka_id"].ToString().Equals("a"),
                            Sjednano = DateTime.Parse(dr["sjednano"].ToString()),
                        }
                    };
                    list.Add(pohledavka);
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
                dr = db.ExecuteRetrievingCommand("select count(*) as count from pohledavky_view where popis like '%' || :currentFilter || '%'", parameters, true);
            }
            else
            {
                dr = db.ExecuteRetrievingCommand("select count(*) as count from pohledavky_view");
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

        public List<Pohledavka> ListPohledavka(int idPojistky)
        {
            List<Pohledavka> list = new List<Pohledavka>();
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":id", idPojistky }
            };
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from pohledavky_view JOIN VIEW_POJISTKY using (pojistka_id) where pojistka_id = :id", parameters);
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
                    Pohledavka pohledavka = new Pohledavka()
                    {
                        ID = int.Parse(dr["Pohledavka_id"].ToString()),
                        Vyse = int.Parse(dr["Vyse"].ToString()),
                        Vznik = DateTime.Parse(dr["vznik"].ToString()),
                        DatumSplatnosti = DateTime.Parse(dr["Datum_splatnosti"].ToString()),
                        DatumSplaceni = nullableDatumSplaceni,
                        Popis = dr["popis"].ToString(),
                        Pojistka = new()
                        {
                            Klient = null, //TODO klient
                            ID = int.Parse(dr["pojistka_id"].ToString()),
                            Podminky = null, //TODO podminky
                            PojistnyProdukt = null,
                            Poplatek = int.Parse(dr["poplatek"].ToString()),
                            SjednanaVyse = int.Parse(dr["sjednana_vyse"].ToString()),
                            Status = dr["pojistka_id"].ToString().Equals("a"),
                            Sjednano = DateTime.Parse(dr["sjednano"].ToString()),
                        }
                    };
                    list.Add(pohledavka);
                }
            }
            db.Dispose();
            return list;
        }

        public void CreatePohledavka(PohledavkaCreateModel pohledavka)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "p_vznik", pohledavka.Vznik.ToString("dd-MM-yyyy") },
                { "p_datum_splatnosti", pohledavka.DatumSplatnosti.ToString("dd-MM-yyyy")},
                { "p_vyse", pohledavka.Vyse },
                { "p_datum_splaceni", pohledavka.DatumSplaceni?.ToString("dd-MM-yyyy") },
                { "p_popis", pohledavka.Popis },
                { "p_pojistka_id", pohledavka.PojistkaId }
            };

            db.ExecuteNonQuery("CREATE_POHLEDAVKA", parameters, false, true);
            db.Dispose();
        }

        public void UpdatePohledavka(int id, RedirectablePohledavka pohledavka)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "p_pohledavka_id", id },
                { "p_vznik", pohledavka.Vznik.ToString("dd-MM-yyyy") },
                { "p_datum_splatnosti", pohledavka.DatumSplatnosti.ToString("dd-MM-yyyy")},
                { "p_vyse", pohledavka.Vyse },
                { "p_datum_splaceni", pohledavka.DatumSplaceni != null ? ((DateTime)pohledavka.DatumSplaceni).ToString("dd-MM-yyyy"): null },
                { "p_popis", pohledavka.Popis }
            };

            db.ExecuteNonQuery("UPDATE_POHLEDAVKA", parameters, false, true);
            db.Dispose();
        }

        public void DeletePohledavka(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "p_pohledavka_id", id },
            };

            db.ExecuteNonQuery("DELETE_POHLEDAVKA", parameters, false, true);
            db.Dispose();
        }

        public Pohledavka? GetPohledavkaPojistka(int id)
        {
            Pohledavka? pohledavka = null;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters[":id"] = id;
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from pohledavky_view JOIN VIEW_POJISTKY using (pojistka_id) where pohledavka_id = :id", parameters);
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

                    pohledavka = new Pohledavka()
                    {
                        ID = int.Parse(dr["Pohledavka_id"].ToString()),
                        Vyse = int.Parse(dr["Vyse"].ToString()),
                        Vznik = DateTime.Parse(dr["vznik"].ToString()),
                        DatumSplatnosti = DateTime.Parse(dr["Datum_splatnosti"].ToString()),
                        DatumSplaceni = nullableDatumSplaceni,
                        Popis = dr["popis"].ToString(),
                        Pojistka = new()
                        {
                            Klient = null, //TODO klient
                            ID = int.Parse(dr["pojistka_id"].ToString()),
                            Podminky = null, //TODO podminky
                            PojistnyProdukt = null,
                            Poplatek = int.Parse(dr["poplatek"].ToString()),
                            SjednanaVyse = int.Parse(dr["sjednana_vyse"].ToString()),
                            Status = dr["pojistka_id"].ToString().Equals("a"),
                            Sjednano = DateTime.Parse(dr["sjednano"].ToString()),
                        }
                    };
                }
            }
            db.Dispose();
            return pohledavka;
        }
    }
}

