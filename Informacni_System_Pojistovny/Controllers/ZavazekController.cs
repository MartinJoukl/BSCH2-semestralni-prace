using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.PojistnaUdalostModels;
using Informacni_System_Pojistovny.Models.Model.Uzivatele;
using Informacni_System_Pojistovny.Models.Model.ZavazekModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Index(PageInfo pageInfo, string CurrentFilter)
        {
            ZavazekModel zavazekModel = new ZavazekModel(_db);

            long count = zavazekModel.GetCount(CurrentFilter);
            ViewBag.count = count;

            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;
            ViewBag.CurrentFilter = CurrentFilter;

            List<Zavazek> zavazky = zavazekModel.ListZavazek(pageInfo, CurrentFilter);
            _db.Dispose();
            return View(zavazky);
        }

        // GET: ZavazkyController/Details/5
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Details(int id)
        {
            ZavazekModel zavazekModel = new ZavazekModel(_db);
            Zavazek zavazek = zavazekModel.GetZavazekUdalost(id);
            if (zavazek == null)
            {
                ViewBag.errorMessage = "Závazek nebyl nalezen";
                _db.Dispose();
                return View(zavazek);
            }
            return View(zavazek);
        }

        // GET: ZavazkyController/Create
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create(int pojistnaUdalostId)
        {
            ViewBag.pojistnaUdalostId = pojistnaUdalostId;
            _db.Dispose();
            return View();
        }

        // POST: ZavazkyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create(ZavazekCreateModel zavazekCreateModel)
        {
            if (!ModelState.IsValid || zavazekCreateModel.Vznik > zavazekCreateModel.DatumSplatnosti)
            {
                ViewBag.pojistnaUdalostId = zavazekCreateModel.PojistnaUdalostId;
                _db.Dispose();
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
                _db.Dispose();
                return RedirectToAction("Details", "PojistnaUdalost", new { id = zavazekCreateModel.PojistnaUdalostId });
            }
            catch
            {
                ViewBag.pojistnaUdalostId = zavazekCreateModel.PojistnaUdalostId;
                ViewBag.errorMessage = "Založení závazku se nezdařilo";
                return View();
            }
        }

        // GET: ZavazkyController/Edit/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(int id, string redirectedFrom, string klientId = null)
        {
            ZavazekModel zavazekModel = new ZavazekModel(_db);
            Zavazek zavazek = zavazekModel.GetZavazekUdalost(id);
            if (zavazek == null)
            {
                ViewBag.errorMessage = "Závazek nebyl nalezen";
                _db.Dispose();
                return View(new RedirectableZavazekModel());
            }
            _db.Dispose();
            return View(new RedirectableZavazekModel() { Vznik= zavazek.Vznik, Vyse = zavazek.Vyse, Popis = zavazek.Popis, DatumSplaceni= zavazek.DatumSplaceni, DatumSplatnosti = zavazek.DatumSplatnosti, PojistnaUdalostId = zavazek.PojistnaUdalost.PojistnaUdalostId, ZavazekId = zavazek.ZavazekId, RedirectedFrom = redirectedFrom, KlientId = klientId });
        }

        // POST: ZavazkyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(int id, RedirectableZavazekModel model)
        {
            try
            {
                if (!ModelState.IsValid || model.Vznik > model.DatumSplatnosti)
                {
                    ViewBag.errorMessage = "Formulář není validní";
                    _db.Dispose();
                    return View(model);
                }
                ZavazekModel zavazekModel = new ZavazekModel(_db);
                Zavazek original = zavazekModel.GetZavazekUdalost(id);
                zavazekModel.UpdateZavazek(id, model);

                if (model.RedirectedFrom == null)
                {
                    _db.Dispose();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _db.Dispose();
                    return RedirectToAction("Details", "PojistnaUdalost", new { id = original.PojistnaUdalost.PojistnaUdalostId });
                }
            }
            catch
            {
                _db.Dispose();
                return View(model);
            }
        }

        // GET: ZavazkyController/Delete/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(int id, string redirectedFrom, string klientId = null)
        {
            ZavazekModel zavazekModel = new ZavazekModel(_db);
            Zavazek zavazek = zavazekModel.GetZavazekUdalost(id);
            if (zavazek == null)
            {
                ViewBag.errorMessage = "Závazek nebyl nalezen";
                _db.Dispose();
                return View(new RedirectableZavazekModel());
            }
            _db.Dispose();
            return View(new RedirectableZavazekModel() { Vznik = zavazek.Vznik, Vyse = zavazek.Vyse, Popis = zavazek.Popis, DatumSplaceni = zavazek.DatumSplaceni, DatumSplatnosti = zavazek.DatumSplatnosti, PojistnaUdalostId = zavazek.PojistnaUdalost.PojistnaUdalostId, ZavazekId = zavazek.ZavazekId, RedirectedFrom = redirectedFrom, KlientId = klientId });
        }

        // POST: ZavazkyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(int id, RedirectableZavazekModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _db.Dispose();
                    return View(model);
                }
                ZavazekModel zavazekModel = new ZavazekModel(_db);
                Zavazek original = zavazekModel.GetZavazekUdalost(id);
                zavazekModel.DeleteZavazek(id);

                if (model.RedirectedFrom == null)
                {
                    _db.Dispose();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _db.Dispose();
                    return RedirectToAction("Details", "PojistnaUdalost", new { id = original.PojistnaUdalost.PojistnaUdalostId });
                }
            }
            catch(Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                _db.Dispose();
                return View(model);
            }
        }
    }
}
