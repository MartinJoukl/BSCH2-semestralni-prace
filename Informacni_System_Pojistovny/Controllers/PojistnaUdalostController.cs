using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.Pojistka;
using Informacni_System_Pojistovny.Models.Model.PojistnaUdalostModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Data;

namespace Informacni_System_Pojistovny.Controllers
{
    public class PojistnaUdalostController : Controller
    {
        private readonly Db _db;
        public PojistnaUdalostController(Db db)
        {
            _db = db;
        }
        // GET: PojistnaUdalostController
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Index(PageInfo pageInfo, string CurrentFilter)
        {
            PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
            List<PojistnaUdalost> pojistneUdalosti = pojistnaUdalostModel.ListPojistnaUdalost(pageInfo, CurrentFilter);

            long count = pojistnaUdalostModel.GetCount(CurrentFilter);
            ViewBag.count = count;
            ViewBag.CurrentFilter = CurrentFilter;

            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;
            _db.Dispose();
            return View(pojistneUdalosti);
        }

        // GET: PojistnaUdalostController/Details/5
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Details(int id, string exceptionMessage)
        {
            PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
            PojistnaUdalost pojistnaUdalost = pojistnaUdalostModel.GetPojistnaUdalost(id);
            if (pojistnaUdalost == null)
            {
                ViewBag.errorMessage = "Pojistná událost nebyla nalezena";

                //return RedirectToAction(nameof(Index));
            }
            else if(exceptionMessage != null)
            {
                ViewBag.errorMessage = exceptionMessage;
            }
            _db.Dispose();
            return View(pojistnaUdalost);
        }

        // GET: PojistnaUdalostController/Create
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create(int id = 0)
        {
            PojistnaUdalost pojistneUdalosti = new PojistnaUdalost();
            KlientModel klientModel = new KlientModel(_db);
            List<Klient> klients = klientModel.ReadClients();

            _db.Dispose();
            return View(new PojistnaUdalostCreateEditModel() { Klients = klients, PojistnaUdalost = pojistneUdalosti });
        }

        // POST: PojistnaUdalostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create(PojistnaUdalostCreateModel model)
        {
            ModelState.Remove("PojistnaUdalost.Klient");
            if (!ModelState.IsValid)
            {
                PojistnaUdalost pojistneUdalosti = new PojistnaUdalost();
                KlientModel klientModel = new KlientModel(_db);
                List<Klient> klients = klientModel.ReadClients();
                _db.Dispose();
                return View(new PojistnaUdalostCreateEditModel() { Klients = klients, PojistnaUdalost = pojistneUdalosti });
            }
            try
            {
                KlientModel klientModel = new KlientModel(_db);
                Klient klient = klientModel.GetClient(model.KlientId);
                if (klient == null)
                {
                    ViewBag.errorMessage = "Zvolený klient nebyl nalezen";
                    return View();
                }
                PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
                pojistnaUdalostModel.CreatePojistnaUdalost(new PojistnaUdalost() { Klient = klient, NarokovanaVysePojistky = model.PojistnaUdalost.NarokovanaVysePojistky, Popis = model.PojistnaUdalost.Popis, Vznik = model.PojistnaUdalost.Vznik });
                _db.Dispose();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                PojistnaUdalost pojistneUdalosti = new PojistnaUdalost();
                KlientModel klientModel = new KlientModel(_db);
                List<Klient> klients = klientModel.ReadClients();
                ViewBag.errorMessage = ex.Message;
                _db.Dispose();
                return View(new PojistnaUdalostCreateEditModel() { Klients = klients, PojistnaUdalost = pojistneUdalosti });
            }
        }

        // GET: PojistnaUdalostController/Edit/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(int id)
        {
            PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
            PojistnaUdalost pojistnaUdalost = pojistnaUdalostModel.GetPojistnaUdalost(id);
            if (pojistnaUdalost == null)
            {
                ViewBag.errorMessage = "Pojistná událost nebyla nalezena";
            }
            KlientModel klientModel = new KlientModel(_db);
            List<Klient> klients = klientModel.ReadClients();
            _db.Dispose();
            return View(new PojistnaUdalostCreateEditModel() { Klients = klients, PojistnaUdalost = pojistnaUdalost });
        }

        // POST: PojistnaUdalostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(int id, PojistnaUdalostEditModel model)
        {
            string klientId = model.KlientId;
            if (!ModelState.IsValid)
            {
                PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
                PojistnaUdalost pojistneUdalosti = pojistnaUdalostModel.GetPojistnaUdalost(id);
                KlientModel klientModel = new KlientModel(_db);
                List<Klient> klients = klientModel.ReadClients();
                _db.Dispose();
                return View(new PojistnaUdalostCreateEditModel() { Klients = klients, PojistnaUdalost = pojistneUdalosti });
            }
            try
            {
                KlientModel klientModel = new KlientModel(_db);
                Klient klient = klientModel.GetClient(int.Parse(klientId));
                _db.Dispose();
                if (klient == null)
                {
                    PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
                    PojistnaUdalost pojistneUdalosti = pojistnaUdalostModel.GetPojistnaUdalost(id);
                    List<Klient> klients = klientModel.ReadClients();
                    _db.Dispose();
                    return View(new PojistnaUdalostCreateEditModel() { Klients = klients, PojistnaUdalost = pojistneUdalosti });
                }
                else
                {
                    PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
                    pojistnaUdalostModel.UpdatePojistnaUdalost(id, model);
                    _db.Dispose();
                    return RedirectToAction(nameof(Details), new { id });
                }
            }
            catch
            {
                PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
                PojistnaUdalost pojistneUdalosti = pojistnaUdalostModel.GetPojistnaUdalost(id);
                KlientModel klientModel = new KlientModel(_db);
                List<Klient> klients = klientModel.ReadClients();
                _db.Dispose();
                return View(new PojistnaUdalostCreateEditModel() { Klients = klients, PojistnaUdalost = pojistneUdalosti });
            }
        }

        // GET: PojistnaUdalostController/Delete/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(int id)
        {
            PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
            PojistnaUdalost pojistnaUdalost = pojistnaUdalostModel.GetPojistnaUdalost(id);
            if (pojistnaUdalost == null)
            {
                ViewBag.errorMessage = "Pojistná událost nebyla nalezena";
                // return RedirectToAction(nameof(Index));
            }
            _db.Dispose();
            return View(pojistnaUdalost);
        }

        // POST: PojistnaUdalostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
            try
            {
                pojistnaUdalostModel.DeletePojistnaUdalost(id);
                _db.Dispose();
                return RedirectToAction(nameof(Index), new { id });
            }
            catch (Exception ex)
            {
                _db.Dispose();
                ViewBag.errorMessage = ex.Message;
                return RedirectToAction(nameof(Details), new { id, exceptionMessage = ex.Message });
            }
        }
    }
}
