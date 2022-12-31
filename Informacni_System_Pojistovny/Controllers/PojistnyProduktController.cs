using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.PojistnaUdalostModels;
using Informacni_System_Pojistovny.Models.Model.PojistnyProduktModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Informacni_System_Pojistovny.Controllers
{
    public class PojistnyProduktController : Controller
    {
        private readonly Db _db;
        public PojistnyProduktController(Db db)
        {
            _db = db;
        }
        // GET: PojistnyProduktController
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Index(PageInfo pageInfo, string CurrentFilter)
        {
            PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
            List<PojistnyProdukt> pojistnyProdukts = pojistnyProduktModel.ReadInsuranceProducts(pageInfo, true, CurrentFilter);

            long count = pojistnyProduktModel.GetCount(true, CurrentFilter);
            ViewBag.count = count;
            ViewBag.CurrentFilter = CurrentFilter;

            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;
            _db.Dispose();
            return View(pojistnyProdukts);
        }

        // GET: PojistnyProduktController/Details/5
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Details(int id)
        {
            PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
            PojistnyProdukt pojistnyProdukt = pojistnyProduktModel.ReadInsuranceProduct(id);
            if(pojistnyProdukt == null)
            {
                ViewBag.errorMessage = "Pojistný produkt nebyl nalezen";
            }
            _db.Dispose();
            return View(pojistnyProdukt);
        }

        // GET: PojistnyProduktController/Create
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PojistnyProduktController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create(PojistnyProduktInputModel pojistnyProduktInputModel)
        {
            if(ModelState.IsValid)
            {
                PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
                pojistnyProduktModel.CreateInsuranceProduct(pojistnyProduktInputModel);
                _db.Dispose();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        // GET: PojistnyProduktController/Edit/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(int id)
        {
            PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
            PojistnyProduktInputModel pojistnyProdukt = pojistnyProduktModel.ReadInsuranceProductAsInputModel(id);
            return View(pojistnyProdukt);
        }

        // POST: PojistnyProduktController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(int id, PojistnyProduktInputModel pojistnyProduktInputModel)
        {
            if(ModelState.IsValid)
            {
                PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
                pojistnyProduktModel.EditInsuranceProduct(pojistnyProduktInputModel, id);
                _db.Dispose();
                return RedirectToAction(nameof(Details), new { id });
            }
            else
            {
                return View();
            }
        }

        // GET: PojistnyProduktController/Delete/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(int id)
        {
            PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
            PojistnyProdukt pojistnyProdukt = pojistnyProduktModel.ReadInsuranceProduct(id);
            _db.Dispose();
            if (pojistnyProdukt == null)
            {
                ViewBag.errorMessage = "Pojistný produkt nebyl nalezen";
            }
            return View(pojistnyProdukt);
        }

        // POST: PojistnyProduktController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(int id, PojistnyProduktInputModel pojistnyProduktInputModel)
        {
            try
            {
                PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
                pojistnyProduktModel.ChangeInsuranceProductStatus(id);
                _db.Dispose();
                return RedirectToAction(nameof(Details), new { id });
            }
            catch
            {
                return View();
            }
        }
    }
}
