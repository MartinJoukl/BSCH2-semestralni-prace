using Informacni_System_Pojistovny.Controllers;
using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Oracle.ManagedDataAccess.Client;

namespace Informacni_System_Pojistovny.Models.Model.UserObjectsModels
{
    public class UserObjectsModel
    {
        private readonly Db db;
        public UserObjectsModel(Db db)
        {
            this.db = db;
        }
        public List<UserObjects> ReadUserObjects() {
            List<UserObjects> userObjects= new List<UserObjects>();
            OracleDataReader dr = db.ExecuteRetrievingCommand("SELECT object_name, object_type FROM USER_OBJECTS");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    UserObjects userObject = new UserObjects();
                    userObject.Name = dr["object_name"].ToString();
                    userObject.Type = dr["object_type"].ToString();
                    userObjects.Add(userObject);
                }
            }
            dr.Close();
            db.Dispose();
            return userObjects;
        }
    }
}
