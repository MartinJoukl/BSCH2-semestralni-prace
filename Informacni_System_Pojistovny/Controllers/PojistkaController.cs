using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.PodminkaModels;
using Informacni_System_Pojistovny.Models.Model.PohledavkaModels;
using Informacni_System_Pojistovny.Models.Model.Pojistka;
using Informacni_System_Pojistovny.Models.Model.PojistnyProduktModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Informacni_System_Pojistovny.Controllers
{
    public class PojistkaController : Controller
    {
        private readonly Db _db;

        public PojistkaController(Db db)
        {
            _db = db;
        }
        // GET: PojistkaController
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Index(PageInfo pageInfo, string CurrentFilter)
        {
            PojistkaModel pojistkaModel = new PojistkaModel(_db);
            List<Pojistka> pojistky = pojistkaModel.ReadInsurances(pageInfo, CurrentFilter);

            long count = pojistkaModel.GetCount(CurrentFilter);
            ViewBag.count = count;
            ViewBag.CurrentFilter = CurrentFilter;

            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;
            _db.Dispose();
            return View(pojistky);
        }

        // GET: PojistkaController/Details/5
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Details(int id)
        {
            PojistkaModel pojistkaModel = new PojistkaModel(_db);
            PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
            Pojistka pojistka = pojistkaModel.ReadInsurance(id);
            if (pojistka == null)
            {
                ViewBag.errorMessage = "Pojistka nebyla nalezena";
            }
            else
            {
                pojistka.Pohledavky = pohledavkaModel.ListPohledavka(id);
            }
            _db.Dispose();
            return View(pojistka);
        }

        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult AddCondition(int id)
        {
            PodminkaModel podminkaModel = new PodminkaModel(_db);
            List<SelectListItem> conditions = podminkaModel.ReadConditionsAsSelectListItems(id);
            ViewBag.conditions = conditions;
            _db.Dispose();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult AddCondition(int id, PojistkaAddConditionModel pojistkaAddConditionModel)
        {
            if (ModelState.IsValid)
            {
                PojistkaModel pojistkaModel = new PojistkaModel(_db);
                pojistkaModel.AddConditionToInsurance(id, pojistkaAddConditionModel);
                _db.Dispose();
                return RedirectToAction(nameof(Details), new { id });
            }
            else return View();
        }

        // GET: PojistkaController/Create
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create()
        {
            PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
            List<SelectListItem> produkty = pojistnyProduktModel.ReadInsuranceProductsAsSelectListItems(false);
            ViewBag.produkty = produkty;
            KlientModel klientModel = new KlientModel(_db);
            List<SelectListItem> klienti = klientModel.ReadClientsAsSelectList();
            ViewBag.klienti = klienti;
            ViewBag.produkty = produkty;
            _db.Dispose();
            return View();
        }
        // GET: PojistkaController/RemoveCondition
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult RemoveCondition(int id, int redirectTo) { 
            PodminkaModel podminkaModel = new PodminkaModel(_db);
            try
            {
                podminkaModel.RemoveConditionFromInsurance(id, redirectTo);
            }catch(Exception ex)
            {
                _db.Dispose();
                ViewBag.errorMessage = ex.Message;
            }
            _db.Dispose();
            return RedirectToAction(nameof(Details), new { id = redirectTo });
        }

        // POST: PojistkaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create(PojistkaCreateModel pojistkaCreateModel)
        {
            if(ModelState.IsValid)
            {
                PojistkaModel pojistkaModel = new PojistkaModel(_db);
                pojistkaModel.CreateInsurance(pojistkaCreateModel);
                _db.Dispose();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _db.Dispose();
                return View();
            }
        }

        // GET: PojistkaController/Edit/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(int id)
        {
            PojistkaModel pojistkaModel = new PojistkaModel(_db);
            PojistkaEditModel pojistkaEditModel = pojistkaModel.ReadInsuranceAsEditModel(id);
            _db.Dispose();
            return View(pojistkaEditModel);
        }

        // POST: PojistkaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(int id, PojistkaEditModel pojistkaEditModel)
        {
            if(ModelState.IsValid)
            {
                PojistkaModel pojistkaModel = new PojistkaModel(_db);
                pojistkaModel.ChangeInsurance(id, pojistkaEditModel);
                _db.Dispose();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _db.Dispose();
                return View(pojistkaEditModel);
            }
        }

        // GET: PojistkaController/Delete/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(int id)
        {
            PojistkaModel pojistkaModel = new PojistkaModel(_db);
            Pojistka pojistka = pojistkaModel.ReadInsurance(id);
            _db.Dispose();
            if (pojistka == null)
            {
                ViewBag.errorMessage = "Pojistka nebyla nalezena";
            }
            return View(pojistka);
        }

        // POST: PojistkaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                PojistkaModel pojistkaModel = new PojistkaModel(_db);
                pojistkaModel.ChangeInsuranceStatus(id);
                _db.Dispose();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.errorMessage = "Nastala chyba při změně stavu pojistky";
                return View();
            }
        }


        // GET: PojistkaController/PermanentDelete/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult PermanentDelete(int id)
        {
            PojistkaModel pojistkaModel = new PojistkaModel(_db);
            Pojistka pojistka = pojistkaModel.ReadInsurance(id);
            _db.Dispose();
            if (pojistka == null)
            {
                ViewBag.errorMessage = "Pojistka nebyla nalezena";
            }
            return View(pojistka);
        }

        // POST: PojistkaController/PermanentDelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult PermanentDelete(int id, IFormCollection collection)
        {
            try
            {
                PojistkaModel pojistkaModel = new PojistkaModel(_db);
                pojistkaModel.DeleteInsurance(id);
                _db.Dispose();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
            ViewBag.errorMessage = ex.Message;
            return View();
            }
        }
    }
}
