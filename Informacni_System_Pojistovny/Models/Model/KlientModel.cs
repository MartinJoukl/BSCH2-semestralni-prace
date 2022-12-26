using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class KlientModel
    {
        private readonly Db db;
        public KlientModel(Db db)
        {
            this.db = db;
        }

        //TODO cist adresy klienta
        public Klient GetClient(int klientId) {
            Dictionary<string, object> clientIdBinding = new Dictionary<string, object>();
            clientIdBinding.Add(":klient_id", klientId);

            Db db = new Db();
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from View_vsechny_osoby o where o.klient_id = :klient_id", clientIdBinding);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
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
                        return fyzickaOsoba;
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
                        return pravnickaOsoba;
                    }
                }
            }
            throw new Exception("Person not found!");
        }

        public List<SelectListItem> ReadClientsAsSelectList()
        {
            List<Klient> klienti = ReadClients();
            List<SelectListItem> klientiAsSelectList = new List<SelectListItem>();
            klienti.ForEach(p => { klientiAsSelectList.Add(new SelectListItem { Value = p.KlientId.ToString(), Text = p.CeleJmeno }); });
            return klientiAsSelectList;
        }

        public List<Klient> ReadClients()
        {
            List<Klient> klients = new List<Klient>();
            //SELECT fyzickych osob
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from View_fyzicke_osoby");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    FyzickaOsoba fyzickaOsoba = new FyzickaOsoba();
                    fyzickaOsoba.KlientId = int.Parse(dr["KLIENT_ID_0"].ToString());
                    fyzickaOsoba.Jmeno = dr["JMENO"].ToString();
                    fyzickaOsoba.Prijmeni = dr["PRIJMENI"].ToString();
                    string stav = dr["STAV"].ToString();
                    if (stav != null && stav.ToUpper().Equals("a".ToUpper()))
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
            OracleDataReader dr2 = db.ExecuteRetrievingCommand("select * from view_pravnicke_osoby");
            if (dr2.HasRows)
            {
                while (dr2.Read())
                {
                    PravnickaOsoba pravnickaOsoba = new PravnickaOsoba();
                    pravnickaOsoba.KlientId = int.Parse(dr2["KLIENT_ID"].ToString());
                    pravnickaOsoba.Nazev = dr2["NAZEV"].ToString();
                    string stav = dr2["STAV"].ToString();
                    if (stav != null && stav.ToUpper().Equals("a".ToUpper()))
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

            return klients;
        }

        public int CreateClient(IFormCollection collection)
        {
            Dictionary<string, object> klientParametry = new Dictionary<string, object>();
            Dictionary<string, object> typOsobyParametry = new Dictionary<string, object>();
            typOsobyParametry.Add(":v_cislo_popisne", collection["cisloPopisne"]);
            typOsobyParametry.Add(":v_ulice", collection["ulice"]);
            typOsobyParametry.Add(":v_psc", collection["psc"]);

            if (collection["zvolenyTypOsoby"].Equals("F"))
            {
                typOsobyParametry.Add(":v_jmeno", collection["jmeno"]);
                typOsobyParametry.Add(":v_prijmeni", collection["prijmeni"]);
                typOsobyParametry.Add(":v_rc", collection["rodneCislo"]);
                typOsobyParametry.Add(":v_telefon", collection["telefon"]);
                typOsobyParametry.Add(":v_email", collection["email"]);
                db.ExecuteNonQuery("vytvor_fyz_o", typOsobyParametry, false, true);
            }
            else
            {
                typOsobyParametry.Add(":v_nazev", collection["nazev"]);
                typOsobyParametry.Add(":v_ico", collection["ico"]);
                db.ExecuteNonQuery("vytvor_prav_o", typOsobyParametry, false, true);
            }

            return 0;
        }

        public void ChangeClientStatus(int id)
        {
            Dictionary<string, object> klientParametry = new Dictionary<string, object>();
            klientParametry.Add(":id", id);
            db.ExecuteNonQuery("zmenStavKlienta", klientParametry, false, true);
            db.Dispose();
        }

        public void RealizeEditClient(KlientEditModel klient, int id, string typKlienta)
        {
            Dictionary<string, object> klientParametry = new Dictionary<string, object>();
            klientParametry.Add(":v_id", id);

            if (typKlienta.Equals("F"))
            {
                klientParametry.Add(":v_jmeno", klient.Jmeno);
                klientParametry.Add(":v_prijmeni", klient.Prijmeni);
                klientParametry.Add(":v_rodne_cislo", klient.RodneCislo);
                klientParametry.Add(":v_telefon", klient.Telefon);
                klientParametry.Add(":v_email", klient.Email);
                db.ExecuteNonQuery("zmenitKlientaFO", klientParametry, false, true);
                db.Dispose();
            }
            else {
                klientParametry.Add(":v_nazev", klient.Nazev);
                klientParametry.Add(":v_ico", klient.Ico);
                db.ExecuteNonQuery("zmenitKlientaPO", klientParametry, false, true);
                db.Dispose();
            }
        }

        public KlientEditModel GetEditClient(int id) {
            Dictionary<string, object> klientParametry = new Dictionary<string, object>();
            klientParametry.Add(":id", id);

            OracleDataReader dr = db.ExecuteRetrievingCommand("SELECT * FROM view_vsechny_osoby where klient_id = :id", klientParametry);

            KlientEditModel klientCreateModel = null;
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    klientCreateModel = new KlientEditModel();
                    string typKlienta = dr["typ_klienta"].ToString();
                    if (typKlienta.Equals("F"))
                    {
                        klientCreateModel.Jmeno = dr["JMENO"].ToString();
                        klientCreateModel.Prijmeni = dr["PRIJMENI"].ToString();
                        klientCreateModel.Email = dr["EMAIL"].ToString();
                        klientCreateModel.Telefon = dr["TELEFON"].ToString();
                        klientCreateModel.RodneCislo = dr["RODNE_CISLO"].ToString();
                    }
                    else
                    {
                        klientCreateModel.Nazev = dr["NAZEV"].ToString();
                        klientCreateModel.Ico = dr["ICO"].ToString();
                    }
                }
            }
            db.Dispose();
            return klientCreateModel;
        }

        public bool AddAddressToClient(int klientId, AdresaInputModel adresa) {
            //PRIDEJ_ADRESU_KLIENTA
            //v_cislo_popisne NUMBER, v_ulice varchar2, v_psc varchar2, v_klient_id varchar2

            Dictionary<string, object> adresaParametry = new Dictionary<string, object>();
            adresaParametry.Add(":v_cislo_popisne", adresa.CisloPopisne);
            adresaParametry.Add(":v_ulice", adresa.Ulice);
            adresaParametry.Add(":v_psc", adresa.Psc);
            adresaParametry.Add(":v_klient_id", klientId);
            db.ExecuteNonQuery("PRIDEJ_ADRESU_KLIENTA", adresaParametry, false, true);
            return true;
        }

        public bool EditClientAddress(int addressId, AdresaInputModel adresa) {
            Dictionary<string, object> adresaParametry = new Dictionary<string, object>();
            adresaParametry.Add(":v_cislo_popisne", adresa.CisloPopisne);
            adresaParametry.Add(":v_ulice", adresa.Ulice);
            adresaParametry.Add(":v_psc", adresa.Psc);
            adresaParametry.Add(":v_adresa_id", addressId);
            db.ExecuteNonQuery("zmen_adresu", adresaParametry, false, true);
            return true;
        }

        public List<Adresa> GetClientAddresses(int KlientId) {
            PscModel pscModel = new PscModel(db);
            List<Adresa> adresas = new List<Adresa>();
            Dictionary<string, object> adresaParametry = new Dictionary<string, object>();
            adresaParametry.Add("klient_id", KlientId);

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from View_adresy where klient_klient_id = :klient_id", adresaParametry);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    // string stav = dr2["STAV"].ToString();
                    Adresa adresa = new Adresa();
                    adresa.AdresaId = int.Parse(dr["adresa_id"].ToString());
                    adresa.CisloPopisne = int.Parse(dr["cislo_popisne"].ToString());
                    adresa.Ulice = dr["ulice"].ToString();
                    adresa.Psc = pscModel.ReadPsc(dr["psc_psc"].ToString());
                    adresas.Add(adresa);
                }
            }

            dr.Close();
            db.Dispose();

            return adresas;
        }
    }
}
