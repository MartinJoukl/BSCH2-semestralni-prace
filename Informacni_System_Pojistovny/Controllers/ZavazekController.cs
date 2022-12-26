using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model.PojistnaUdalostModels;
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
            ZavazekModel zavazekModel = new ZavazekModel(_db);
            Zavazek zavazek = zavazekModel.GetZavazekUdalost(id);
            if (zavazek == null)
            {
                return RedirectToAction("Index");
            }
            return View(zavazek);
        }

        // GET: ZavazkyController/Create
        public ActionResult Create(int pojistnaUdalostId)
        {
            ViewBag.pojistnaUdalostId = pojistnaUdalostId;
            return View();
        }

        // POST: ZavazkyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ZavazekCreateModel zavazekCreateModel)
        {
            if (!ModelState.IsValid)
            {

                return View();
            }
            try
            {
                PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
                PojistnaUdalost pojistnaUdalost = pojistnaUdalostModel.GetPojistnaUdalost((int)zavazekCreateModel.PojistnaUdalostId);
                if (pojistnaUdalost != null)
                {
                    ZavazekModel zavazekModel = new ZavazekModel(_db);
                    zavazekModel.CreateZavazek(zavazekCreateModel);
                }
                return RedirectToAction("Details", "PojistnaUdalost", new { id = zavazekCreateModel.PojistnaUdalostId });
            }
            catch
            {
                return View();
            }
        }

        // GET: ZavazkyController/Edit/5
        public ActionResult Edit(int id, string redirectedFrom)
        {
            ZavazekModel zavazekModel = new ZavazekModel(_db);
            Zavazek zavazek = zavazekModel.GetZavazekUdalost(id);
            if (zavazek == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(new RedirectableZavazek() { Zavazek = zavazek, RedirectedFrom = redirectedFrom });
        }

        // POST: ZavazkyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, RedirectableZavazek model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(id);
                }
                ZavazekModel zavazekModel = new ZavazekModel(_db);
                Zavazek original = zavazekModel.GetZavazekUdalost(id);
                zavazekModel.UpdateZavazek(id, model.Zavazek);

                if (model.RedirectedFrom == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Details", "PojistnaUdalost", new { id = original.PojistnaUdalost.PojistnaUdalostId });
                }
            }
            catch
            {

                return View(id);
            }
        }

        // GET: ZavazkyController/Delete/5
        public ActionResult Delete(int id, string redirectedFrom)
        {
            ZavazekModel zavazekModel = new ZavazekModel(_db);
            Zavazek zavazek = zavazekModel.GetZavazekUdalost(id);
            if (zavazek == null)
            {
                return RedirectToAction("Index");
            }
            return View(new RedirectableZavazek() { Zavazek = zavazek, RedirectedFrom = redirectedFrom });
        }

        // POST: ZavazkyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, RedirectableZavazek model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(id);
                }
                ZavazekModel zavazekModel = new ZavazekModel(_db);
                Zavazek original = zavazekModel.GetZavazekUdalost(id);
                zavazekModel.DeleteZavazek(id);

                if (model.RedirectedFrom == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Details", "PojistnaUdalost", new { id = original.PojistnaUdalost.PojistnaUdalostId });
                }
            }
            catch
            {

                return View(id);
            }
        }
    }
}
