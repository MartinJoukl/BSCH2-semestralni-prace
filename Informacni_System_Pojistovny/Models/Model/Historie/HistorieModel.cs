using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Model.Uzivatele;
using Oracle.ManagedDataAccess.Client;

namespace Informacni_System_Pojistovny.Models.Model.Historie
{
    public class HistorieModel
    {
        private readonly Db db;
        public HistorieModel(Db db)
        {
            this.db = db;
        }
        public List<Entity.Historie> ListHistorie(PageInfo pageInfo)
        {
            List<Entity.Historie> list = new List<Entity.Historie>();

            int pageStart = pageInfo.PageIndex * pageInfo.PageSize;
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":pageStart", pageStart },
                { ":pageSize", pageInfo.PageSize }
            };

            OracleDataReader dr = db.ExecuteRetrievingCommand("select * from historie_view order by cas_zmeny DESC OFFSET :pageStart ROWS FETCH NEXT :pageSize ROWS ONLY", parameters);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string idHistorie = dr["historie_id"]?.ToString();

                    string jmenoPredmetu = dr["Jmeno_Predmetu"]?.ToString();
                    string item_idString = dr["item_id"]?.ToString();

                    DMLTyp dmlTyp = DMLTypeRetriever.GetByName(dr["DDL_TYP"]?.ToString());

                    DateTime.TryParse(dr["cas_zmeny"]?.ToString(), out DateTime casHistorie);
                    int.TryParse(idHistorie, out var idHistorieInt);
                    int.TryParse(item_idString, out var item_idStringInt);
                    list.Add(new Entity.Historie
                    {
                        Id = idHistorieInt,
                        CasZmeny = casHistorie,
                        DML = dmlTyp,
                        ItemId = item_idStringInt,
                        JmenoPredmetu = jmenoPredmetu
                    });
                }
            }
            db.Dispose();
            return list;
        }

        public long GetCount()
        {
            long count = 0;
            Db db = new Db();
            OracleDataReader dr = db.ExecuteRetrievingCommand("select count(*) as count from historie_view");
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
    }
}
