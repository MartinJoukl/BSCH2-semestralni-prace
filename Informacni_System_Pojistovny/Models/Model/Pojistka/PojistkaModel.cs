using Informacni_System_Pojistovny.Models.Dao;

namespace Informacni_System_Pojistovny.Models.Model.Pojistka
{
    public class PojistkaModel
    {
        private readonly Db db;
        public PojistkaModel(Db db)
        {
            this.db = db;
        }
    }
}
