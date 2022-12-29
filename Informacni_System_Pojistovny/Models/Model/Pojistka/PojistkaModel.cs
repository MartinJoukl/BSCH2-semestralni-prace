using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model.PodminkaModels;
using Informacni_System_Pojistovny.Models.Model.PojistnyProduktModels;
using Oracle.ManagedDataAccess.Client;

namespace Informacni_System_Pojistovny.Models.Model.Pojistka
{
    public class PojistkaModel
    {
        private readonly Db db;
        public PojistkaModel(Db db)
        {
            this.db = db;
        }

        public bool CreateInsurance(PojistkaCreateModel pojistkaCreateModel) {
            Dictionary<string, object> pojistkaParametry = new Dictionary<string, object>();
            pojistkaParametry.Add(":v_klient_id", pojistkaCreateModel.KlientId);
            pojistkaParametry.Add(":v_pojistny_produkt_id", pojistkaCreateModel.PojistnyProduktId);
            pojistkaParametry.Add(":v_sjednano", pojistkaCreateModel.Sjednano.ToString("dd-MM-yyyy"));
            pojistkaParametry.Add(":v_sjednana_vyse", pojistkaCreateModel.SjednanaVyse);
            pojistkaParametry.Add(":v_poplatek", pojistkaCreateModel.Poplatek);

            db.ExecuteNonQuery("VYTVOR_POJISTKU", pojistkaParametry, false, true);
            return true;
        }

        public bool AddConditionToInsurance(int id, PojistkaAddConditionModel pojistkaAddModel) {
            Dictionary<string, object> podminkaParametry = new Dictionary<string, object>();
            podminkaParametry.Add(":v_pojistka_id", id);
            podminkaParametry.Add(":v_podminka_id", pojistkaAddModel.PodminkaId);

            db.ExecuteNonQuery("pridej_podminku", podminkaParametry, false, true);
            return true;
        }

        public bool ChangeInsurance(int id, PojistkaEditModel pojistkaEditModel) {
            Dictionary<string, object> pojistkaParametry = new Dictionary<string, object>();
            pojistkaParametry.Add(":v_pojistka_id", id);
            pojistkaParametry.Add(":v_sjednana_vyse", pojistkaEditModel.SjednanaVyse);
            pojistkaParametry.Add(":v_poplatek", pojistkaEditModel.Poplatek);

            db.ExecuteNonQuery("zmenit_pojistku", pojistkaParametry, false, true);
            return true;
        }

        public List<Entity.Pojistka> ReadInsurances(int id = 0) {
            List<Entity.Pojistka> pojistky = new List<Entity.Pojistka>();

            string sql;
            OracleDataReader dr;

            if (id == 0)
            {
                dr = db.ExecuteRetrievingCommand("select * from view_pojistky");
            }
            else
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add(":id", id);
                dr = db.ExecuteRetrievingCommand("select * from view_pojistky where klient_id = :id" , parameters);
            }

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Entity.Pojistka pojistka = new Entity.Pojistka();
                    pojistka.ID = int.Parse(dr["pojistka_id"].ToString());
                    pojistka.Sjednano = DateTime.Parse(dr["Sjednano"].ToString());
                    pojistka.Poplatek = int.Parse(dr["poplatek"].ToString());
                    pojistka.SjednanaVyse = int.Parse(dr["sjednana_vyse"].ToString());
                    string status = dr["status"].ToString();
                    if (status != null && status.Equals("a"))
                    {
                        pojistka.Status = true;
                    }
                    else
                    {
                        pojistka.Status = false;
                    }
                    PojistnyProdukt pojistnyProdukt = new PojistnyProdukt();
                    pojistnyProdukt.ID = int.Parse(dr["Produkt_id"].ToString());
                    pojistnyProdukt.Status = true;
                    pojistnyProdukt.MaximalniVysePlneni = int.Parse(dr["MAX_VYSE_PLNENI"].ToString());
                    pojistnyProdukt.Popis = dr["popis"].ToString();
                    pojistnyProdukt.Nazev = dr["nazev"].ToString();
                    string statusProduktu = dr["status_produktu"].ToString();
                    if (statusProduktu != null && statusProduktu.Equals("a"))
                    {
                        pojistnyProdukt.Status = true;
                    }
                    else
                    {
                        pojistnyProdukt.Status = false;
                    }
                    pojistka.PojistnyProdukt = pojistnyProdukt;
                    pojistka.Podminky = ReadInsuranceConditions(pojistka.ID);

                    string typKlienta = dr["typ_klienta"].ToString();
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
                        pojistka.Klient = fyzickaOsoba;
                    }
                    else
                    {
                        PravnickaOsoba pravnickaOsoba = new PravnickaOsoba();
                        pravnickaOsoba.KlientId = int.Parse(dr["KLIENT_ID"].ToString());
                        pravnickaOsoba.Nazev = dr["NAZEV_PO"].ToString();
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
                        pojistka.Klient = pravnickaOsoba;
                    }

                    pojistky.Add(pojistka);
                }
            }
            dr.Close();
            db.Dispose();
            return pojistky;
        }

        public List<Entity.Pojistka> ReadInsurances(PageInfo pageInfo)
        {
            List<Entity.Pojistka> pojistky = new List<Entity.Pojistka>();

            int pageStart = pageInfo.PageIndex * pageInfo.PageSize;
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":pageStart", pageStart },
                { ":pageSize", pageInfo.PageSize }
            };

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from view_pojistky order by pojistka_id OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY", parameters);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Entity.Pojistka pojistka = new Entity.Pojistka();
                    pojistka.ID = int.Parse(dr["pojistka_id"].ToString());
                    pojistka.Sjednano = DateTime.Parse(dr["Sjednano"].ToString());
                    pojistka.Poplatek = int.Parse(dr["poplatek"].ToString());
                    pojistka.SjednanaVyse = int.Parse(dr["sjednana_vyse"].ToString());
                    string status = dr["status"].ToString();
                    if (status != null && status.Equals("a"))
                    {
                        pojistka.Status = true;
                    }
                    else
                    {
                        pojistka.Status = false;
                    }
                    PojistnyProdukt pojistnyProdukt = new PojistnyProdukt();
                    pojistnyProdukt.ID = int.Parse(dr["Produkt_id"].ToString());
                    pojistnyProdukt.Status = true;
                    pojistnyProdukt.MaximalniVysePlneni = int.Parse(dr["MAX_VYSE_PLNENI"].ToString());
                    pojistnyProdukt.Popis = dr["popis"].ToString();
                    pojistnyProdukt.Nazev = dr["nazev"].ToString();
                    string statusProduktu = dr["status_produktu"].ToString();
                    if (statusProduktu != null && statusProduktu.Equals("a"))
                    {
                        pojistnyProdukt.Status = true;
                    }
                    else
                    {
                        pojistnyProdukt.Status = false;
                    }
                    pojistka.PojistnyProdukt = pojistnyProdukt;

                    string typKlienta = dr["typ_klienta"].ToString();
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
                        pojistka.Klient = fyzickaOsoba;
                    }
                    else
                    {
                        PravnickaOsoba pravnickaOsoba = new PravnickaOsoba();
                        pravnickaOsoba.KlientId = int.Parse(dr["KLIENT_ID"].ToString());
                        pravnickaOsoba.Nazev = dr["NAZEV_PO"].ToString();
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
                        pojistka.Klient = pravnickaOsoba;
                    }

                    pojistky.Add(pojistka);
                }
            }
            dr.Close();
            db.Dispose();
            return pojistky;
        }

        public long GetCount()
        {
            long count = 0;
            Db db = new Db();
            OracleDataReader dr = db.ExecuteRetrievingCommand("select count(*) as count from view_pojistky ");
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

        public List<Podminka> ReadInsuranceConditions(int id) { 
            List<Podminka> podminky = new List<Podminka>();
            Dictionary<string, object> pojistkaParametry = new Dictionary<string, object>();
            pojistkaParametry.Add(":id", id);
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from view_pojistky_podminky where pojistka_id = :id", pojistkaParametry);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Podminka podminka = new Podminka();
                    podminka.ID = int.Parse(dr["podminka_id"].ToString());
                    podminka.Popis = dr["popis"].ToString();
                    podminky.Add(podminka);
                }
            }
            return podminky;
        }

        public PojistkaEditModel ReadInsuranceAsEditModel(int id) {
            Entity.Pojistka pojistka = ReadInsurance(id);
            if (pojistka == null) return null;
            else { 
                PojistkaEditModel pojistkaEditModel = new PojistkaEditModel();
                pojistkaEditModel.SjednanaVyse = pojistka.SjednanaVyse;
                pojistkaEditModel.Poplatek = pojistka.Poplatek;
                return pojistkaEditModel;
            }
        }
        public bool ChangeInsuranceStatus(int id) {
            Dictionary<string, object> pojistkaParametry = new Dictionary<string, object>();
            pojistkaParametry.Add(":v_pojistka_id", id);

            db.ExecuteNonQuery("zmenit_stav_pojistky", pojistkaParametry, false, true);
            return true;
        }
        public bool DeleteInsurance(int id)
        {
            Dictionary<string, object> pojistkaParametry = new Dictionary<string, object>();
            pojistkaParametry.Add(":v_pojistka_id", id);

            db.ExecuteNonQuery("smazat_pojistku", pojistkaParametry, false, true);
            return true;
        }
        public Entity.Pojistka ReadInsurance(int id) {
            Dictionary<string, object> pojistkaParametry = new Dictionary<string, object>();
            pojistkaParametry.Add(":id", id);
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from view_pojistky where pojistka_id = :id", pojistkaParametry);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Entity.Pojistka pojistka = new Entity.Pojistka();
                    pojistka.ID = int.Parse(dr["pojistka_id"].ToString());
                    pojistka.Sjednano = DateTime.Parse(dr["Sjednano"].ToString());
                    pojistka.Poplatek = int.Parse(dr["poplatek"].ToString());
                    pojistka.SjednanaVyse = int.Parse(dr["sjednana_vyse"].ToString());
                    string status = dr["status"].ToString();
                    if (status != null && status.Equals("a"))
                    {
                        pojistka.Status = true;
                    }
                    else
                    {
                        pojistka.Status = false;
                    }
                    PojistnyProdukt pojistnyProdukt = new PojistnyProdukt();
                    pojistnyProdukt.ID = int.Parse(dr["Produkt_id"].ToString());
                    pojistnyProdukt.Status = true;
                    pojistnyProdukt.MaximalniVysePlneni = int.Parse(dr["MAX_VYSE_PLNENI"].ToString());
                    pojistnyProdukt.Popis = dr["popis"].ToString();
                    pojistnyProdukt.Nazev = dr["nazev"].ToString();
                    string statusProduktu = dr["status_produktu"].ToString();
                    if (statusProduktu != null && statusProduktu.Equals("a"))
                    {
                        pojistnyProdukt.Status = true;
                    }
                    else
                    {
                        pojistnyProdukt.Status = false;
                    }
                    pojistka.PojistnyProdukt = pojistnyProdukt;
                    pojistka.Podminky = ReadInsuranceConditions(pojistka.ID);

                    string typKlienta = dr["typ_klienta"].ToString();
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
                        pojistka.Klient = fyzickaOsoba;
                    }
                    else
                    {
                        PravnickaOsoba pravnickaOsoba = new PravnickaOsoba();
                        pravnickaOsoba.KlientId = int.Parse(dr["KLIENT_ID"].ToString());
                        pravnickaOsoba.Nazev = dr["NAZEV_PO"].ToString();
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
                        pojistka.Klient = pravnickaOsoba;
                    }

                    return pojistka;
                }
            }
            dr.Close();
            db.Dispose();
            return null;
        }
    }
}
