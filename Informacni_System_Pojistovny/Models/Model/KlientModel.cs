using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model.DokumentModels;
using Informacni_System_Pojistovny.Models.Model.HistorieClenstviModels;
using Informacni_System_Pojistovny.Models.Model.Pojistka;
using Informacni_System_Pojistovny.Models.Model.PojistnaUdalostModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

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

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from View_vsechny_osoby o where o.klient_id = :klient_id", clientIdBinding);
            if (dr.HasRows)
            {
                PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(db);
                PojistkaModel pojistkaModel = new PojistkaModel(db);
                HistorieClenstviModel historieClenstviModel = new HistorieClenstviModel(db);

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
                        fyzickaOsoba.PohledavkyNad20k = int.Parse(dr["pohledavky_nad_20k"].ToString());
                        fyzickaOsoba.NesplacenePohledavkyPoTerminu = int.Parse(dr["nesplacene_pohledavky"].ToString());
                        fyzickaOsoba.NesplaceneZavazkyPoTerminu = int.Parse(dr["nesplacene_zavazky"].ToString());
                        fyzickaOsoba.PojistneUdalosti = pojistnaUdalostModel.ListPojistnaUdalost(fyzickaOsoba.KlientId);
                        fyzickaOsoba.Pojistky = pojistkaModel.ReadInsurances(fyzickaOsoba.KlientId);
                        fyzickaOsoba.HistorieClenstvi = historieClenstviModel.ReadMembershipHistories(fyzickaOsoba.KlientId);
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
                        pravnickaOsoba.PohledavkyNad20k = int.Parse(dr["pohledavky_nad_20k"].ToString());
                        pravnickaOsoba.NesplacenePohledavkyPoTerminu = int.Parse(dr["nesplacene_pohledavky"].ToString());
                        pravnickaOsoba.NesplaceneZavazkyPoTerminu = int.Parse(dr["nesplacene_zavazky"].ToString());
                        pravnickaOsoba.PojistneUdalosti = pojistnaUdalostModel.ListPojistnaUdalost(pravnickaOsoba.KlientId);
                        pravnickaOsoba.Pojistky = pojistkaModel.ReadInsurances(pravnickaOsoba.KlientId);
                        pravnickaOsoba.HistorieClenstvi = historieClenstviModel.ReadMembershipHistories(pravnickaOsoba.KlientId);
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
        //KlientModel klientModel = new KlientModel(db);

        public List<Klient> ReadClients()
        {
            HistorieClenstviModel historieClenstviModel = new HistorieClenstviModel(db);
            List<Klient> klients = new List<Klient>();
            //SELECT fyzickych osob
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from View_vsechny_osoby order by KLIENT_ID");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (dr["typ_klienta"].Equals("F"))
                    {
                        FyzickaOsoba fyzickaOsoba = new FyzickaOsoba();
                        fyzickaOsoba.KlientId = int.Parse(dr["KLIENT_ID"].ToString());
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
                        fyzickaOsoba.HistorieClenstvi = historieClenstviModel.ReadMembershipHistories(fyzickaOsoba.KlientId);
                        klients.Add(fyzickaOsoba);
                    }
                    else {
                        PravnickaOsoba pravnickaOsoba = new PravnickaOsoba();
                        pravnickaOsoba.KlientId = int.Parse(dr["KLIENT_ID"].ToString());
                        pravnickaOsoba.Nazev = dr["NAZEV"].ToString();
                        string stav = dr["STAV"].ToString();
                        if (stav != null && stav.ToUpper().Equals("a".ToUpper()))
                        {
                            pravnickaOsoba.Stav = true;
                        }
                        else
                        {
                            pravnickaOsoba.Stav = false;
                        }
                        pravnickaOsoba.Ico = dr["ICO"].ToString();
                        pravnickaOsoba.HistorieClenstvi = historieClenstviModel.ReadMembershipHistories(pravnickaOsoba.KlientId);
                        klients.Add(pravnickaOsoba);
                    }
                }
            }
            dr.Close();

            return klients;
        }

        public List<Klient> ReadClients(PageInfo pageInfo, string currentFilter = null)
        {
            HistorieClenstviModel historieClenstviModel = new HistorieClenstviModel(db);
            List<Klient> klients = new List<Klient>();

            int pageStart = pageInfo.PageIndex * pageInfo.PageSize;
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":pageStart", pageStart },
                { ":pageSize", pageInfo.PageSize }
            };

            string sql;
            if (currentFilter != null)
            {
                sql = "select * from View_vsechny_osoby where nazev like '%' || :currentFilter || '%' or jmeno like '%' || :currentFilter || '%' or prijmeni like '%' || :currentFilter || '%' order by KLIENT_ID OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
                parameters.Add(":currentFilter", currentFilter);
                //sql = "select * from View_vsechny_osoby order by KLIENT_ID OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
            }
            else
            {
                sql = "select * from View_vsechny_osoby order by KLIENT_ID OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
            }

            OracleDataReader dr = db.ExecuteRetrievingCommand(sql, parameters, true);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (dr["typ_klienta"].Equals("F"))
                    {
                        FyzickaOsoba fyzickaOsoba = new FyzickaOsoba();
                        fyzickaOsoba.KlientId = int.Parse(dr["KLIENT_ID"].ToString());
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
                        fyzickaOsoba.HistorieClenstvi = historieClenstviModel.ReadMembershipHistories(fyzickaOsoba.KlientId);
                        klients.Add(fyzickaOsoba);
                    }
                    else
                    {
                        PravnickaOsoba pravnickaOsoba = new PravnickaOsoba();
                        pravnickaOsoba.KlientId = int.Parse(dr["KLIENT_ID"].ToString());
                        pravnickaOsoba.Nazev = dr["NAZEV"].ToString();
                        string stav = dr["STAV"].ToString();
                        if (stav != null && stav.ToUpper().Equals("a".ToUpper()))
                        {
                            pravnickaOsoba.Stav = true;
                        }
                        else
                        {
                            pravnickaOsoba.Stav = false;
                        }
                        pravnickaOsoba.Ico = dr["ICO"].ToString();
                        pravnickaOsoba.HistorieClenstvi = historieClenstviModel.ReadMembershipHistories(pravnickaOsoba.KlientId);
                        klients.Add(pravnickaOsoba);
                    }
                }
            }
            dr.Close();

            return klients;
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
                dr = db.ExecuteRetrievingCommand("select count(*) as count from View_vsechny_osoby where nazev like '%' || :currentFilter || '%' or jmeno like '%' || :currentFilter || '%' or prijmeni like '%' || :currentFilter || '%' ", parameters, true);
            }
            else
            {
                dr = db.ExecuteRetrievingCommand("select count(*) as count from View_vsechny_osoby");
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

        public bool AddDocumentToClient(int id, DokumentUploadModel dokumentUploadModel) {
            Dictionary<string, object> dokumentParametry = new Dictionary<string, object>();
            string koncovka = Path.GetExtension(dokumentUploadModel.Data.FileName);
            string typ = dokumentUploadModel.Data.ContentType;
            Stream dataStream = dokumentUploadModel.Data.OpenReadStream();
            BinaryReader binaryReader = new BinaryReader(dataStream);
            byte[] binaryData = binaryReader.ReadBytes((Int32)dataStream.Length);

            dokumentParametry.Add(":v_nazev", dokumentUploadModel.Nazev);
            dokumentParametry.Add(":v_typ", typ);
            dokumentParametry.Add(":v_pripona", koncovka);

            //dokumentParametry.Add(":v_data", binaryData); presunuto do parametru
            dokumentParametry.Add(":v_klient_id", id);

            db.ExecuteNonQuery("nahraj_dokument_klienta", dokumentParametry, false, true, binaryData);
            return true;
        }

        public List<Dokument> ReadClientDocuments(int klientId)
        {
            List<Dokument> dokumenty = new List<Dokument>();
            Dictionary<string, object> dokumentParametry = new Dictionary<string, object>();
            dokumentParametry.Add(":klient_id", klientId);

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from view_dokumenty where klient_id = :klient_id", dokumentParametry);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    OracleBinary binaryData = dr.GetOracleBinary(5);
                    byte[] imgBytes = binaryData.IsNull ? null : binaryData.Value;
                    Dokument dokument = new Dokument();
                    dokument.DokumentId = int.Parse(dr["dokument_id"].ToString());
                    dokument.DatumNahrani = DateTime.Parse(dr["datum_nahrani"].ToString());
                    dokument.Typ = dr["typ"].ToString();
                    dokument.Pripona = dr["pripona"].ToString();
                    dokument.Data = imgBytes;
                    dokument.Nazev = dr["nazev"].ToString();
                    dokumenty.Add(dokument);
                }
            }

            dr.Close();
            db.Dispose();
            return dokumenty;
        }
        public Dokument ReadDocument(int document)
        {
            Dictionary<string, object> dokumentParametry = new Dictionary<string, object>();
            dokumentParametry.Add(":dokument_id", document);

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from view_dokumenty where dokument_id = :dokument_id", dokumentParametry);
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    OracleBinary binaryData = dr.GetOracleBinary(5);
                    byte[] imgBytes = binaryData.IsNull ? null : binaryData.Value;
                    Dokument dokument = new Dokument();
                    dokument.DokumentId = int.Parse(dr["dokument_id"].ToString());
                    dokument.DatumNahrani = DateTime.Parse(dr["datum_nahrani"].ToString());
                    dokument.Typ = dr["typ"].ToString();
                    dokument.Pripona = dr["pripona"].ToString();
                    dokument.Data = imgBytes;
                    dokument.Nazev = dr["nazev"].ToString();
                    dr.Close();
                    db.Dispose();
                    return dokument;
                }
            }

            dr.Close();
            db.Dispose();
            return null;
        }
        public bool DeleteDocument(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(":v_dokument_id", id);
            db.ExecuteNonQuery("smazat_dokument", parameters, false, true);
            db.Dispose();
            return true;
        }
    }
}
