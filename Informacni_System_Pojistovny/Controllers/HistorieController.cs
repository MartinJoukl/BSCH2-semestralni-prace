using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model.UzivatelHistorie;
using Informacni_System_Pojistovny.Models.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Informacni_System_Pojistovny.Models.Model.Historie;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Informacni_System_Pojistovny.Controllers
{
    public class HistorieController : Controller
    {
        private readonly Db _db;
        public HistorieController(Db db)
        {
            _db = db;
        }
        // GET: Historie
        [Authorize(Roles = nameof(UzivateleRole.Admin))]
        public ActionResult Index(PageInfo pageInfo)
        {
            HistorieModel historieModel = new HistorieModel(_db);
            List<Historie> historie = historieModel.ListHistorie(pageInfo);
            long count = historieModel.GetCount();
            ViewBag.count = count;

            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;
            _db.Dispose();
            return View(historie);
        }
    }
}
