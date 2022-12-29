using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.PojistnaUdalostModels;
using Informacni_System_Pojistovny.Models.Model.Uzivatele;
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
        public ActionResult Index(PageInfo pageInfo)
        {
            ZavazekModel zavazekModel = new ZavazekModel(_db);

            long count = zavazekModel.GetCount();
            ViewBag.count = count;

            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;

            List<Zavazek> zavazky = zavazekModel.ListZavazek(pageInfo);
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
            if (!ModelState.IsValid || zavazekCreateModel.Vznik > zavazekCreateModel.DatumSplatnosti)
            {
                ViewBag.pojistnaUdalostId = zavazekCreateModel.PojistnaUdalostId;
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
                ViewBag.pojistnaUdalostId = zavazekCreateModel.PojistnaUdalostId;
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
            return View(new RedirectableZavazekModel() { Vznik= zavazek.Vznik, Vyse = zavazek.Vyse, Popis = zavazek.Popis, DatumSplaceni= zavazek.DatumSplaceni, DatumSplatnosti = zavazek.DatumSplatnosti, PojistnaUdalostId = zavazek.PojistnaUdalost.PojistnaUdalostId, ZavazekId = zavazek.ZavazekId, RedirectedFrom = redirectedFrom });
        }

        // POST: ZavazkyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, RedirectableZavazekModel model)
        {
            try
            {
                if (!ModelState.IsValid || model.Vznik > model.DatumSplatnosti)
                {
                    return View(model);
                }
                ZavazekModel zavazekModel = new ZavazekModel(_db);
                Zavazek original = zavazekModel.GetZavazekUdalost(id);
                zavazekModel.UpdateZavazek(id, model);

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

                return View(model);
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
            return View(new RedirectableZavazekModel() { Vznik = zavazek.Vznik, Vyse = zavazek.Vyse, Popis = zavazek.Popis, DatumSplaceni = zavazek.DatumSplaceni, DatumSplatnosti = zavazek.DatumSplatnosti, PojistnaUdalostId = zavazek.PojistnaUdalost.PojistnaUdalostId, ZavazekId = zavazek.ZavazekId, RedirectedFrom = redirectedFrom });
        }

        // POST: ZavazkyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, RedirectableZavazekModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
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

                return View(model);
            }
        }
    }
}
