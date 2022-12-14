using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class PobockaModel
    {
        private readonly Db db;
        public PobockaModel(Db db)
        {
            this.db = db;
        }
        public bool CreateBranch(PobockaCreateModel pobockaCreateModel)
        {
            Dictionary<string, object> pobockaParametry = new Dictionary<string, object>();
            pobockaParametry.Add("v_nazev", pobockaCreateModel.Nazev);
            pobockaParametry.Add("v_cislo_popisne", pobockaCreateModel.CisloPopisne);
            pobockaParametry.Add("v_ulice", pobockaCreateModel.Ulice);
            pobockaParametry.Add("v_psc", pobockaCreateModel.Psc);

            db.ExecuteNonQuery("vytvorit_pobocku", pobockaParametry, false, true);
            return true;
        }

        public bool DeleteBranch(int id)
        {
            Db db = new Db();
            Dictionary<string, object> pobockaParametry = new Dictionary<string, object>();
            pobockaParametry.Add(":v_id", id);
            db.ExecuteNonQuery("smazat_pobocku", pobockaParametry, false, true);
            return true;
        }

        public List<Pobocka> ReadBranches()
        {
            List<Pobocka> pobockas = new List<Pobocka>();
            Db db = new Db();
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from VIEW_POBOCKY order by pobocka_id");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Pobocka pobocka = new Pobocka();
                    pobocka.Nazev = dr["nazev"].ToString();
                    pobocka.PobockaId = int.Parse(dr["pobocka_id"].ToString());
                    pobockas.Add(pobocka);
                }
            }
            dr.Close();
            db.Dispose();
            return pobockas;
        }

        public List<Pobocka> ReadBranches(PageInfo pageInfo, string currentFilter = null)
        {
            List<Pobocka> pobockas = new List<Pobocka>();
            Db db = new Db();
            int pageStart = pageInfo.PageIndex * pageInfo.PageSize;
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":pageStart", pageStart },
                { ":pageSize", pageInfo.PageSize }
            };

            string sql;
            if (currentFilter != null)
            {
                sql = "select * from VIEW_POBOCKY where nazev like '%' || :currentFilter || '%' order by pobocka_id OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
                parameters.Add(":currentFilter", currentFilter);
            }
            else
            {
                sql = "select * from VIEW_POBOCKY order by pobocka_id OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
            }

            OracleDataReader dr = db.ExecuteRetrievingCommand(sql, parameters, true);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Pobocka pobocka = new Pobocka();
                    pobocka.Nazev = dr["nazev"].ToString();
                    pobocka.PobockaId = int.Parse(dr["pobocka_id"].ToString());
                    pobockas.Add(pobocka);
                }
            }
            dr.Close();
            db.Dispose();
            return pobockas;
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
                dr = db.ExecuteRetrievingCommand("select count(*) as count from VIEW_POBOCKY where nazev like '%' || :currentFilter || '%'", parameters, true);
            }
            else
            {
                dr = db.ExecuteRetrievingCommand("select count(*) as count from VIEW_POBOCKY");
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

        public Pobocka ReadBranch(int id)
        {
            Dictionary<string, object> branchParameters = new Dictionary<string, object>();
            branchParameters.Add("id", id);

            Db db = new Db();
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from VIEW_POBOCKY where pobocka_id = :id", branchParameters);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Pobocka pobocka = new Pobocka();
                    pobocka.Nazev = dr["nazev"].ToString();
                    pobocka.PobockaId = int.Parse(dr["pobocka_id"].ToString());
                    dr.Close();
                    db.Dispose();
                    return pobocka;
                }
            }
            dr.Close();
            db.Dispose();
            return null;
        }

        public PobockaEditModel ReadBranchAsPobockaEdit(int id)
        {
            Pobocka pobocka = ReadBranch(id);
            if (pobocka == null)
                return null;
            else return new PobockaEditModel { Nazev = pobocka.Nazev };
        }

        public bool RealizePobockaEdit(PobockaEditModel pobockaEditModel, int id)
        {
            Dictionary<string, object> pobockaParametry = new Dictionary<string, object>();
            pobockaParametry.Add(":v_nazev", pobockaEditModel.Nazev);
            pobockaParametry.Add(":v_id", id);
            db.ExecuteNonQuery("zmenit_Pobocku", pobockaParametry, false, true);
            db.Dispose();

            return true;
        }

        public bool AddAddressesToBranch(int pobockaId, AdresaInputModel adresa)
        {
            //PRIDEJ_ADRESU_KLIENTA
            //v_cislo_popisne NUMBER, v_ulice varchar2, v_psc varchar2, v_klient_id varchar2

            Adresa branchAddress = GetBranchAddress(pobockaId);
            if (branchAddress != null)
            {
                throw new Exception("Příliš mnoho adres pobočky, po přidání by bylo více než očekávaná 1");
            }

            Dictionary<string, object> adresaParametry = new Dictionary<string, object>();
            adresaParametry.Add(":v_cislo_popisne", adresa.CisloPopisne);
            adresaParametry.Add(":v_ulice", adresa.Ulice);
            adresaParametry.Add(":v_psc", adresa.Psc);
            adresaParametry.Add(":v_pobocka_id", pobockaId);
            db.ExecuteNonQuery("PRIDEJ_ADRESU_POBOCKY", adresaParametry, false, true);
            return true;
        }

        public bool EditBranchAddress(int addressId, AdresaInputModel adresa)
        {
            Dictionary<string, object> adresaParametry = new Dictionary<string, object>();
            adresaParametry.Add(":v_cislo_popisne", adresa.CisloPopisne);
            adresaParametry.Add(":v_ulice", adresa.Ulice);
            adresaParametry.Add(":v_psc", adresa.Psc);
            adresaParametry.Add(":v_adresa_id", addressId);
            db.ExecuteNonQuery("zmen_adresu", adresaParametry, false, true);
            return true;
        }

        public Adresa GetBranchAddress(int pobockaId)
        {
            PscModel pscModel = new PscModel(db);
            List<Adresa> adresas = new List<Adresa>();
            Dictionary<string, object> adresaParametry = new Dictionary<string, object>();
            adresaParametry.Add("pobocka_id", pobockaId);

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from View_adresy where POBOCKA_POBOCKA_ID = :pobocka_id", adresaParametry);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    // string stav = dr2["STAV"].ToString();
                    Adresa adresa = new Adresa();
                    adresa.CisloPopisne = int.Parse(dr["cislo_popisne"].ToString());
                    adresa.Ulice = dr["ulice"].ToString();
                    adresa.Psc = pscModel.ReadPsc(dr["psc_psc"].ToString());
                    adresa.AdresaId = int.Parse(dr["adresa_id"].ToString());
                    adresas.Add(adresa);
                }
            }

            dr.Close();
            db.Dispose();

            if (adresas.Count > 1)
            {
                throw new Exception("Příliš mnoho adres pobočky, očekávána 1");
            }
            if (adresas.Count > 0)
            {
                return adresas[0];
            }
            else return null;
        }
    }
}
