using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Oracle.ManagedDataAccess.Client;

namespace Informacni_System_Pojistovny.Models.Model.HistorieClenstviModels
{
    public class HistorieClenstviModel
    {
        private readonly Db db;
        public HistorieClenstviModel(Db db)
        {
            this.db = db;
        }

        public List<HistorieClenstvi> ReadMembershipHistories(int id = 0) {
            Db db = new Db();
            List<HistorieClenstvi> historieClenstvis = new List<HistorieClenstvi>();

            KlientModel klientModel = new KlientModel(db);
            OracleDataReader dr;

            if (id == 0)
            {
                dr = db.ExecuteRetrievingCommand("select * from view_historie_clenstvi");
            }
            else {
                Dictionary<string, object> historieParametry = new Dictionary<string, object>();
                historieParametry.Add(":id", id);
                dr = db.ExecuteRetrievingCommand("select * from view_historie_clenstvi where klient_klient_id = :id", historieParametry);
            }
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    HistorieClenstvi historieClenstvi = new HistorieClenstvi();
                    historieClenstvi.HistorieClenstviId = int.Parse(dr["historie_clenstvi_id"].ToString());
                    historieClenstvi.Od = DateTime.Parse(dr["od"].ToString());

                    if (dr["do"].ToString() != null && !dr["do"].ToString().Equals(""))
                    {
                        historieClenstvi.Do = DateTime.Parse(dr["do"].ToString());
                    }
                    //historieClenstvi.Klient = klientModel.GetClient(int.Parse(dr["klient_klient_id"].ToString())); //Smycka

                    historieClenstvis.Add(historieClenstvi);
                }
            }
            db.Dispose();
            return historieClenstvis;
        }
    }
}
