using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.Uzivatele;
using Informacni_System_Pojistovny.Models.Model.UzivatelHistorie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace Informacni_System_Pojistovny.Controllers
{
    public class UzivatelHistorieController : Controller
    {
        private readonly Db _db;
        public UzivatelHistorieController(Db db)
        {
            _db = db;
        }
        // GET: UzivatelHistorieController
        [Authorize(Roles = nameof(UzivateleRole.Admin))]
        public ActionResult Index(PageInfo pageInfo)
        {
            UzivatelHistorieModel uzivatelHistorieModel = new UzivatelHistorieModel(_db);
            List<HistorieUzivatele> historieUzivatelu = uzivatelHistorieModel.ListHistorie(pageInfo);

            long count = uzivatelHistorieModel.GetCount();
            ViewBag.count = count;

            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;

            return View(historieUzivatelu);
        }

        // GET: UzivatelHistorieController/Details/5
        [Authorize(Roles = nameof(UzivateleRole.Admin))]
        public ActionResult Details(int id)
        {
            UzivatelHistorieModel historieModel = new UzivatelHistorieModel(_db);
            HistorieUzivatele historie = historieModel.GetHistorie(id);
            if (historie != null)
            {
                return View(historie);
            }
            else
            {
                return Index(new PageInfo());
            }
        }
    }
}
