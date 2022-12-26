using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
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

        public List<Entity.Pojistka> ReadInsurances() {
            List<Entity.Pojistka> pojistky = new List<Entity.Pojistka>();
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from view_pojistky");
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
    }
}
