using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Informacni_System_Pojistovny.Models.Model.PojistnyProduktModels
{
    public class PojistnyProduktModel
    {
        private readonly Db db;
        public PojistnyProduktModel(Db db)
        {
            this.db = db;
        }

        /* pri false se ctou jen active */
        public List<PojistnyProdukt> ReadInsuranceProducts(bool activeInactive = true) {
            List<PojistnyProdukt> pojistnyProdukts = new List<PojistnyProdukt>();

            string sql;
            if (activeInactive)
            {
                sql = "select * from view_pojistne_produkty";
            }
            else {
                sql = "select * from view_pojistne_produkty where status = 'a'";
            }

            OracleDataReader dr = db.ExecuteRetrievingCommand(sql);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    PojistnyProdukt pojistnyProdukt = new PojistnyProdukt();
                    pojistnyProdukt.ID = int.Parse(dr["Produkt_id"].ToString());
                    pojistnyProdukt.Status = true;
                    pojistnyProdukt.MaximalniVysePlneni = int.Parse(dr["MAX_VYSE_PLNENI"].ToString());
                    pojistnyProdukt.Popis = dr["popis"].ToString();
                    pojistnyProdukt.Nazev = dr["nazev"].ToString();
                    string status = dr["status"].ToString();
                    if (status != null && status.Equals("a"))
                    {
                        pojistnyProdukt.Status = true;
                    }
                    else
                    {
                        pojistnyProdukt.Status = false;
                    }
                    pojistnyProdukts.Add(pojistnyProdukt);
                }
            }
            dr.Close();
            db.Dispose();
            return pojistnyProdukts;
        }

        public List<PojistnyProdukt> ReadInsuranceProducts(PageInfo pageInfo,bool activeInactive = true, string currentFilter = null)
        {
            List<PojistnyProdukt> pojistnyProdukts = new List<PojistnyProdukt>();

            int pageStart = pageInfo.PageIndex * pageInfo.PageSize;
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":pageStart", pageStart },
                { ":pageSize", pageInfo.PageSize }
            };

            string sql;
            if (currentFilter != null)
            {
                if (activeInactive)
                {
                    sql = "select * from view_pojistne_produkty where nazev like '%' || :currentFilter || '%' or popis like '%' || :currentFilter || '%' OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
                    parameters.Add(":currentFilter", currentFilter);
                }
                else
                {
                    sql = "select * from view_pojistne_produkty where status = 'a' and (nazev like '%' || :currentFilter || '%' or popis like '%' || :currentFilter || '%') OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
                    parameters.Add(":currentFilter", currentFilter);
                }
            }
            else {
                if (activeInactive)
                {
                    sql = "select * from view_pojistne_produkty OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
                }
                else
                {
                    sql = "select * from view_pojistne_produkty where status = 'a'  OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
                }
            }

            OracleDataReader dr = db.ExecuteRetrievingCommand(sql,parameters, true);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    PojistnyProdukt pojistnyProdukt = new PojistnyProdukt();
                    pojistnyProdukt.ID = int.Parse(dr["Produkt_id"].ToString());
                    pojistnyProdukt.Status = true;
                    pojistnyProdukt.MaximalniVysePlneni = int.Parse(dr["MAX_VYSE_PLNENI"].ToString());
                    pojistnyProdukt.Popis = dr["popis"].ToString();
                    pojistnyProdukt.Nazev = dr["nazev"].ToString();
                    pojistnyProdukt.pocetPravnickychOsob = long.Parse(dr["pocet_Pravnickych_Osob"].ToString());
                    pojistnyProdukt.pocetFyzickychOsob = long.Parse(dr["pocet_fyzickych_osob"].ToString());
                    string status = dr["status"].ToString();
                    if (status != null && status.Equals("a"))
                    {
                        pojistnyProdukt.Status = true;
                    }
                    else
                    {
                        pojistnyProdukt.Status = false;
                    }
                    pojistnyProdukts.Add(pojistnyProdukt);
                }
            }
            dr.Close();
            db.Dispose();
            return pojistnyProdukts;
        }

        public long GetCount(bool activeInactive = true, string currentFilter = null)
        {
            long count = 0;
            Db db = new Db();
            string sql;
            OracleDataReader dr;

            if (currentFilter != null)
            {
                if (activeInactive)
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object> {
                        { ":currentFilter", currentFilter }
                    };
                    sql = "select count(*) as count from view_pojistne_produkty where nazev like '%' || :currentFilter || '%' or popis like '%' || :currentFilter || '%'";
                    dr = db.ExecuteRetrievingCommand(sql, parameters, true);
                }
                else
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object> {
                    { ":currentFilter", currentFilter }
                    };
                    sql = "select count(*) as count from view_pojistne_produkty where status = 'a' and (nazev like '%' || :currentFilter || '%' or popis like '%' || :currentFilter || '%')";
                    dr = db.ExecuteRetrievingCommand(sql, parameters, true);
                }
            }
            else {
                if (activeInactive)
                {
                    sql = "select count(*) as count from view_pojistne_produkty";
                    dr = db.ExecuteRetrievingCommand(sql);
                }
                else
                {
                    sql = "select count(*) as count from view_pojistne_produkty where status = 'a'";
                    dr = db.ExecuteRetrievingCommand(sql);
                }
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
        public List<SelectListItem> ReadInsuranceProductsAsSelectListItems(bool activePassive)
        {
            List<PojistnyProdukt> pojistneProdukty = ReadInsuranceProducts(activePassive);
            List<SelectListItem> pojistneProduktySelectList = new List<SelectListItem>();
            pojistneProdukty.ForEach(p => { pojistneProduktySelectList.Add(new SelectListItem { Value = p.ID.ToString(), Text = p.Nazev + ", max. plnění " + p.MaximalniVysePlneni }); });
            return pojistneProduktySelectList;
        }

        public bool CreateInsuranceProduct(PojistnyProduktInputModel pojistnyProduktInputModel) {
            Dictionary<string, object> pojistnyProduktParametry = new Dictionary<string, object>();
            pojistnyProduktParametry.Add(":v_nazev", pojistnyProduktInputModel.Nazev);
            pojistnyProduktParametry.Add(":v_popis", pojistnyProduktInputModel.Popis);
            pojistnyProduktParametry.Add(":v_max_vyse_plneni", pojistnyProduktInputModel.MaximalniVysePlneni);

            int pobockaId = db.ExecuteNonQuery("vytvorit_pojistny_produkt", pojistnyProduktParametry, false, true);
            db.Dispose();
            return true;
        }

        public bool EditInsuranceProduct(PojistnyProduktInputModel pojistnyProduktInputModel, int id)
        {
            Dictionary<string, object> pojistnyProduktParametry = new Dictionary<string, object>();
            pojistnyProduktParametry.Add(":v_id", id);
            pojistnyProduktParametry.Add(":v_nazev", pojistnyProduktInputModel.Nazev);
            pojistnyProduktParametry.Add(":v_popis", pojistnyProduktInputModel.Popis);
            pojistnyProduktParametry.Add(":v_max_vyse_plneni", pojistnyProduktInputModel.MaximalniVysePlneni);

            int pobockaId = db.ExecuteNonQuery("zmenit_pojistny_produkt", pojistnyProduktParametry, false, true);
            db.Dispose();
            return true;
        }

        public bool ChangeInsuranceProductStatus(int id)
        {
            Dictionary<string, object> pojistnyProduktParametry = new Dictionary<string, object>();
            pojistnyProduktParametry.Add(":v_id", id);

            db.ExecuteNonQuery("zmenit_stav_pojistny_produkt", pojistnyProduktParametry, false, true);
            db.Dispose();
            return true;
        }

        public PojistnyProduktInputModel ReadInsuranceProductAsInputModel(int id) {
            Dictionary<string, object> pojistnyProduktParametry = new Dictionary<string, object>();
            pojistnyProduktParametry.Add(":v_id", id);
            PojistnyProduktInputModel pojistnyProdukt = null;
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from view_pojistne_produkty where produkt_id = :v_id", pojistnyProduktParametry);
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    pojistnyProdukt = new PojistnyProduktInputModel();
                    pojistnyProdukt.MaximalniVysePlneni = int.Parse(dr["MAX_VYSE_PLNENI"].ToString());
                    pojistnyProdukt.Popis = dr["popis"].ToString();
                    pojistnyProdukt.Nazev = dr["nazev"].ToString();
                }
            }
            dr.Close();
            db.Dispose();
            return pojistnyProdukt;
        }

        public PojistnyProdukt ReadInsuranceProduct(int id)
        {
            Dictionary<string, object> pojistnyProduktParametry = new Dictionary<string, object>();
            pojistnyProduktParametry.Add(":v_id", id);
            PojistnyProdukt pojistnyProdukt = null;
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from view_pojistne_produkty where produkt_id = :v_id", pojistnyProduktParametry);
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    pojistnyProdukt = new PojistnyProdukt();
                    pojistnyProdukt.ID = int.Parse(dr["Produkt_id"].ToString());
                    string status = dr["status"].ToString();
                    if (status != null && status.Equals("a"))
                    {
                        pojistnyProdukt.Status = true;
                    }
                    else
                    {
                        pojistnyProdukt.Status = false;
                    }
                    pojistnyProdukt.MaximalniVysePlneni = int.Parse(dr["MAX_VYSE_PLNENI"].ToString());
                    pojistnyProdukt.Popis = dr["popis"].ToString();
                    pojistnyProdukt.Nazev = dr["nazev"].ToString();
                    pojistnyProdukt.pocetPravnickychOsob = long.Parse(dr["pocet_Pravnickych_Osob"].ToString());
                    pojistnyProdukt.pocetFyzickychOsob = long.Parse(dr["pocet_fyzickych_osob"].ToString());
                }
            }
            dr.Close();
            db.Dispose();
            return pojistnyProdukt;
        }
    }
}
