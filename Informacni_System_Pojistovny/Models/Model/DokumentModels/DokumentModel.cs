using Informacni_System_Pojistovny.Models.Dao;
using Microsoft.CodeAnalysis;

namespace Informacni_System_Pojistovny.Models.Model.DokumentModels
{
    public class DokumentModel
    {
        private readonly Db db;
        public DokumentModel(Db db)
        {
            this.db = db;
        }
    }
}
