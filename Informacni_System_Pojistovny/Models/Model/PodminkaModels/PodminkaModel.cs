using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model.Pojistka;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Informacni_System_Pojistovny.Models.Model.PodminkaModels
{
    public class PodminkaModel
    {
        private readonly Db db;
        public PodminkaModel(Db db)
        {
            this.db = db;
        }
        public bool CreateCondition(PodminkaCreateModel podminkaCreateModel)
        {
            Dictionary<string, object> podminkaParametry = new Dictionary<string, object>();
            podminkaParametry.Add(":v_popis", podminkaCreateModel.Popis);

            db.ExecuteNonQuery("vytvor_podminku", podminkaParametry, false, true);
            return true;
        }

        public bool RemoveConditionFromInsurance(int conditionId, int insuranceId) {
            Dictionary<string, object> podminkaParametry = new Dictionary<string, object>();
            podminkaParametry.Add(":v_podminka_id", conditionId);
            podminkaParametry.Add(":v_pojistka_id", insuranceId);

            db.ExecuteNonQuery("odeber_podminku", podminkaParametry, false, true);
            return true;
        }

        public bool ChangeCondition(int id, PodminkaCreateModel podminkaCreateModel)
        {
            Dictionary<string, object> podminkaParametry = new Dictionary<string, object>();
            podminkaParametry.Add(":v_popis", podminkaCreateModel.Popis);
            podminkaParametry.Add("v_id", id);

            db.ExecuteNonQuery("zmenit_podminku", podminkaParametry, false, true);
            return true;
        }
        public bool DeleteCondition(int id)
        {
            Dictionary<string, object> podminkaParametry = new Dictionary<string, object>();
            podminkaParametry.Add("v_id", id);

            db.ExecuteNonQuery("smazat_podminku", podminkaParametry, false, true);
            return true;
        }

        public Podminka ReadCondition(int id)
        {
            Dictionary<string, object> podminkaParametry = new Dictionary<string, object>();
            podminkaParametry.Add(":id", id);

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from view_podminky where podminka_id = :id", podminkaParametry);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Podminka podminka = new Podminka();
                    podminka.ID = int.Parse(dr["podminka_id"].ToString());
                    podminka.Popis = dr["popis"].ToString();
                    dr.Close();
                    db.Dispose();
                    return podminka;
                }
            }
            dr.Close();
            db.Dispose();
            return null;
        }

        public List<SelectListItem> ReadConditionsAsSelectListItems(int insuranceId = 0) {
            List<Podminka> podminky = ReadConditions(insuranceId);
            List<SelectListItem> podminkySelectListItems = new List<SelectListItem>();
            podminky.ForEach(p => { podminkySelectListItems.Add(new SelectListItem { Value = p.ID.ToString(), Text = p.Popis }); });
            return podminkySelectListItems;
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
                dr = db.ExecuteRetrievingCommand("select count(*) as count from view_podminky where popis like '%' || :currentFilter || '%'", parameters, true);
            }
            else
            {
                dr = db.ExecuteRetrievingCommand("select count(*) as count from view_podminky");
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

        public List<Podminka> ReadConditions(PageInfo pageInfo, string currentFilter = null)
        {
            string sql;

            int pageStart = pageInfo.PageIndex * pageInfo.PageSize;
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":pageStart", pageStart },
                { ":pageSize", pageInfo.PageSize }
            };

            if (currentFilter != null)
            {
                sql = "select * from view_podminky where popis like '%' || :currentFilter || '%' order by podminka_id OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
                parameters.Add(":currentFilter", currentFilter);
            }
            else
            {
                sql = "select * from view_podminky order by podminka_id OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY";
            }

            OracleDataReader dr = db.ExecuteRetrievingCommand(sql, parameters, true);
            
            List<Podminka> podminky = new List<Podminka>();

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
            dr.Close();
            db.Dispose();
            return podminky;
        }

        public List<Podminka> ReadConditions(int insuranceId = 0)
        {
            string sql;
            OracleDataReader dr;
            if (insuranceId == 0)
            {
                sql = "select * from view_podminky";
                dr = db.ExecuteRetrievingCommand(sql);
            }
            else {
                Dictionary<string, object> podminkaParametry = new Dictionary<string, object>();
                podminkaParametry.Add(":id", insuranceId);
                sql = "select * from view_podminky where podminka_id not in (SELECT podminka_podminka_id from pojistne_podminky where pojistka_produkt_id = :id)";
                dr = db.ExecuteRetrievingCommand(sql, podminkaParametry);
            }
            List<Podminka> podminky = new List<Podminka>();

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
            dr.Close();
            db.Dispose();
            return podminky;
        }

        public PodminkaCreateModel ReadConditionAsCreateModel(int id)
        {
            Podminka podminka = ReadCondition(id);
            if (podminka == null) return null;
            else
            {
                PodminkaCreateModel podminkaCreateModel = new PodminkaCreateModel();
                podminkaCreateModel.Popis = podminka.Popis;
                return podminkaCreateModel;
            }
        }

    }
}
