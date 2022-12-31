using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class PscModel
    {
        private readonly Db db;
        public PscModel(Db db)
        {
            this.db = db;
        }

        public List<Psc> ReadPscs()
        {
            List<Psc> pscs = new List<Psc>();
            Db db = new Db();
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from view_psc");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Psc psc = new Psc();
                    psc.PscCislo = dr["PSC"].ToString();
                    psc.Mesto = dr["mesto"].ToString();
                    pscs.Add(psc);
                }
            }
            dr.Close();
            db.Dispose();
            return pscs;
        }

        public List<Psc> ReadPscs(PageInfo pageInfo, string currentFilter = null)
        {
            List<Psc> pscs = new List<Psc>();
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
                sql = "select * from view_psc where psc like '%' || :currentFilter || '%' or mesto like '%' || :currentFilter || '%' order by PSC OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
                parameters.Add(":currentFilter", currentFilter);
            }
            else
            {
                sql = "select * from view_psc order by PSC OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
            }

            OracleDataReader dr = db.ExecuteRetrievingCommand(sql, parameters, true);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Psc psc = new Psc();
                    psc.PscCislo = dr["PSC"].ToString();
                    psc.Mesto = dr["mesto"].ToString();
                    pscs.Add(psc);
                }
            }
            dr.Close();
            db.Dispose();
            return pscs;
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
                dr = db.ExecuteRetrievingCommand("select count(*) as count from view_psc where PSC like '%' || :currentFilter || '%' or mesto like '%' || :currentFilter || '%'", parameters, true);
            }
            else
            {
                dr = db.ExecuteRetrievingCommand("select count(*) as count from view_psc");
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

        public bool DeletePSC(string pscCislo) {
            Dictionary<string, object> pscParametry = new Dictionary<string, object>();
            pscParametry.Add(":v_psc", pscCislo);
            db.ExecuteNonQuery("smazat_psc", pscParametry, false, true);
            return true;
        }
        public Psc ReadPsc(string pscString)
        {
            Dictionary<string, object> pscParametry = new Dictionary<string, object>();
            pscParametry.Add(":psc", pscString);

            Db db = new Db();
            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from view_psc where psc = :psc", pscParametry);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Psc psc = new Psc();
                    psc.PscCislo = dr["PSC"].ToString();
                    psc.Mesto = dr["mesto"].ToString();
                    dr.Close();
                    db.Dispose();
                    return psc;
                }
            }
            dr.Close();
            db.Dispose();
            return null;
        }

        public PscEditModel ReadPscAsEditModel(string pscString)
        {
            Psc psc = ReadPsc(pscString);
            if (psc != null) {
                PscEditModel pscEditModel = new PscEditModel();
                pscEditModel.Mesto = psc.Mesto;
                return pscEditModel;
            }
            return null;
        }

        public List<SelectListItem> ReadPscsAsSelectListItems()
        {
            List<Psc> pscs = ReadPscs();
            List<SelectListItem> pscsAsSelectList = new List<SelectListItem>();
            pscs.ForEach(p => { pscsAsSelectList.Add(new SelectListItem { Value = p.PscCislo, Text = p.PscCislo});});
            return pscsAsSelectList;
        }

        public bool CreatePsc(Psc psc) {
            Dictionary<string, object> pscParametry = new Dictionary<string, object>();
            pscParametry.Add(":v_psc", psc.PscCislo);
            pscParametry.Add(":v_mesto", psc.Mesto);

            try
            {
                db.ExecuteNonQuery("vytvorit_psc", pscParametry, false, true);
                return true;
            }
            catch(Exception e) {
                return false;
            }
        }

        public bool EditPsc(string pscCislo, PscEditModel pscEditModel)
        {
            Dictionary<string, object> pscParametry = new Dictionary<string, object>();
            pscParametry.Add(":v_psc", pscCislo);
            pscParametry.Add(":v_mesto", pscEditModel.Mesto);

            try
            {
                db.ExecuteNonQuery("zmenit_psc", pscParametry, false, true);
                db.Dispose();
                return true;
            }
            catch (Exception e)
            {
                db.Dispose();
                return false;
            }
        }
    }
}
