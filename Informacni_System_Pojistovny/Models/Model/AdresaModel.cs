using Informacni_System_Pojistovny.Models.Dao;
using Oracle.ManagedDataAccess.Client;

namespace Informacni_System_Pojistovny.Models.Model
{
    public class AdresaModel
    {
        private readonly Db db;
        public AdresaModel(Db db)
        {
            this.db = db;
        }
        public bool EditAddress(int addressId, AdresaInputModel adresa)
        {
            Dictionary<string, object> adresaParametry = new Dictionary<string, object>();
            adresaParametry.Add(":v_cislo_popisne", adresa.CisloPopisne);
            adresaParametry.Add(":v_ulice", adresa.Ulice);
            adresaParametry.Add(":v_psc", adresa.Psc);
            adresaParametry.Add(":v_adresa_id", addressId);
            db.ExecuteNonQuery("zmen_adresu", adresaParametry, false, true);
            return true;
        }

        public bool DeleteAddress(int addressId)
        {
            Dictionary<string, object> adresaParametry = new Dictionary<string, object>();
            adresaParametry.Add(":v_adresa_id", addressId);
            db.ExecuteNonQuery("smaz_adresu", adresaParametry, false, true);
            return true;
        }
        public AdresaInputModel ReadAddressAsEditModel(int addressId)
        {
            Dictionary<string, object> adresaParametry = new Dictionary<string, object>();
            adresaParametry.Add(":id", addressId);

            OracleDataReader dr = db.ExecuteRetrievingCommand("SELECT * FROM view_adresy where adresa_id = :id", adresaParametry);

            AdresaInputModel adresaInputModel = null;
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    adresaInputModel = new AdresaInputModel();
                    adresaInputModel.Ulice = dr["ulice"].ToString();
                    adresaInputModel.CisloPopisne = int.Parse(dr["cislo_popisne"].ToString());
                    adresaInputModel.Psc = dr["psc_psc"].ToString();
                }
            }
            db.Dispose();
            return adresaInputModel;
        }
    }
}
