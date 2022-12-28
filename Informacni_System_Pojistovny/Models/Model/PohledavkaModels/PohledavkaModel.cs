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
                { "p_datum_splaceni", pohledavka.DatumSplatnosti.ToString("dd-MM-yyyy") },
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

