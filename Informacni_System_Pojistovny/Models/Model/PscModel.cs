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

        public Psc ReadPsc(string pscString)
        {
            Dictionary<string, object> pscParametry = new Dictionary<string, object>();
            pscParametry.Add("psc", pscString);

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

        public List<SelectListItem> ReadPscsAsSelectListItems()
        {
            List<Psc> pscs = ReadPscs();
            List<SelectListItem> pscsAsSelectList = new List<SelectListItem>();
            pscs.ForEach(p => { pscsAsSelectList.Add(new SelectListItem { Value = p.PscCislo, Text = p.PscCislo});});
            return pscsAsSelectList;
        }

        public bool CreatePsc(Psc psc) {
            Dictionary<string, object> pscParametry = new Dictionary<string, object>();
            pscParametry.Add("v_psc", psc.PscCislo);
            pscParametry.Add("v_mesto", psc.Mesto);

            try
            {
                db.ExecuteNonQuery("vytvorit_psc", pscParametry, false, true);
                return true;
            }
            catch(Exception e) {
                return false;
            }
        }
    }
}
