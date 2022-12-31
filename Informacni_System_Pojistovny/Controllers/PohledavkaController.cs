using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.PohledavkaModels;
using Informacni_System_Pojistovny.Models.Model.Pojistka;
using Informacni_System_Pojistovny.Models.Model.PojistnaUdalostModels;
using Informacni_System_Pojistovny.Models.Model.ZavazekModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Informacni_System_Pojistovny.Controllers
{
    public class PohledavkaController : Controller
    {
        private readonly Db _db;
        public PohledavkaController(Db db)
        {
            _db = db;
        }
        // GET: ZavazkyController
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Index(PageInfo pageInfo, string CurrentFilter)
        {
            PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
            List<Pohledavka> pohledavky = pohledavkaModel.ListPohledavka(pageInfo, CurrentFilter);
            long count = pohledavkaModel.GetCount(CurrentFilter);
            ViewBag.count = count;
            ViewBag.CurrentFilter = CurrentFilter;

            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;
            return View(pohledavky);
        }

        // GET: ZavazkyController/Details/5
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Details(int id)
        {
            PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
            Pohledavka pohledavka = pohledavkaModel.GetPohledavkaPojistka(id);
            if (pohledavka == null)
            {
                ViewBag.errorMessage = "Pohledávka nebyla nalezena";
            }
            return View(pohledavka);
        }

        // GET: PohledavkyController/Create
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create(int pojistkaId)
        {
            ViewBag.pojistkaId = pojistkaId;
            return View();
        }

        // POST: PohledavkyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create(PohledavkaCreateModel pohledavkaCreateModel)
        {
            if (!ModelState.IsValid || pohledavkaCreateModel.Vznik > pohledavkaCreateModel.DatumSplatnosti)
            {
                ViewBag.pojistkaId = pohledavkaCreateModel.PojistkaId;
                return View();
            }
            try
            {
                PojistkaModel pojistkaModel = new PojistkaModel(_db);
                Pojistka pojistka = pojistkaModel.ReadInsurance((int)pohledavkaCreateModel.PojistkaId);
                if (pojistka != null)
                {
                    PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
                    pohledavkaModel.CreatePohledavka(pohledavkaCreateModel);
                }
                else
                {
                    ViewBag.errorMessage = "Pojistka k pohledávce nebyla nalezena";
                }
                return RedirectToAction("Details", "Pojistka", new { id = pohledavkaCreateModel.PojistkaId });
            }
            catch(Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                ViewBag.pojistkaId = pohledavkaCreateModel.PojistkaId;
                return View();
            }
        }

        // GET: ZavazkyController/Edit/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(int id, string redirectedFrom, string klientId = null)
        {
            PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
            Pohledavka pohledavka = pohledavkaModel.GetPohledavkaPojistka(id);
            if (pohledavka == null)
            {
                ViewBag.errorMessage = "Pohledávka nebyla nalezena";
                //return RedirectToAction(nameof(Index));
                return View(new RedirectablePohledavka());
            }
            return View(new RedirectablePohledavka() { ID = pohledavka.ID, Vznik = pohledavka.Vznik, DatumSplaceni=pohledavka.DatumSplaceni, DatumSplatnosti= pohledavka.DatumSplatnosti, PojistkaId = pohledavka.ID, Popis = pohledavka.Popis, Vyse = pohledavka.Vyse, RedirectedFrom = redirectedFrom, KlientId = klientId });
        }

        // POST: ZavazkyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(int id, RedirectablePohledavka model)
        {
            try
            {
                if (!ModelState.IsValid || model.Vznik > model.DatumSplatnosti)
                {
                    ViewBag.errorMessage = "Formulář není validní";
                    return View(model);
                }
                PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
                Pohledavka original = pohledavkaModel.GetPohledavkaPojistka(id);
                try
                {
                    pohledavkaModel.UpdatePohledavka(id, model);
                }catch(Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                }
                _db.Dispose();
                if (model.RedirectedFrom == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Details", "Pojistka", new { id = original.Pojistka.ID });
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
            PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
            Pohledavka pohledavka = pohledavkaModel.GetPohledavkaPojistka(id);
            _db.Dispose();
            if (pohledavka == null)
            {
                ViewBag.errorMessage = "Pohledávka již byla smazána";
                //return RedirectToAction("Index");
                return View(new RedirectablePohledavka());
            }
            return View(new RedirectablePohledavka() { ID = pohledavka.ID, Vznik = pohledavka.Vznik, DatumSplaceni = pohledavka.DatumSplaceni, DatumSplatnosti = pohledavka.DatumSplatnosti, PojistkaId = pohledavka.ID, Popis = pohledavka.Popis, Vyse = pohledavka.Vyse, RedirectedFrom = redirectedFrom, KlientId = klientId });
        }

        // POST: ZavazkyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(int id, RedirectablePohledavka model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
                Pohledavka original = pohledavkaModel.GetPohledavkaPojistka(id);
                pohledavkaModel.DeletePohledavka(id);
                _db.Dispose();
                if (model.RedirectedFrom == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Details", "Pojistka", new { id = original.Pojistka.ID });
                }
            }
            catch
            {
                _db.Dispose();
                return View(model);
            }
        }
    }
}
