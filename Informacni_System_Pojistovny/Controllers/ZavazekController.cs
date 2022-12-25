using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model.ZavazekModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Informacni_System_Pojistovny.Controllers
{
    public class ZavazekController : Controller
    {
        private readonly Db _db;
        public ZavazekController(Db db)
        {
            _db = db;
        }
        // GET: ZavazkyController
        public ActionResult Index()
        {
            ZavazekModel zavazekModel = new ZavazekModel(_db);
            List<Zavazek> zavazky = zavazekModel.ListZavazek();
            return View(zavazky);
        }

        // GET: ZavazkyController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ZavazkyController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ZavazkyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ZavazkyController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ZavazkyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ZavazkyController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ZavazkyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
