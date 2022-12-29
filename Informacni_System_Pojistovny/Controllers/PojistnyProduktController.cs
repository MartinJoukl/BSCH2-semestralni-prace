using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.PojistnaUdalostModels;
using Informacni_System_Pojistovny.Models.Model.PojistnyProduktModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult Index(PageInfo pageInfo)
        {
            PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
            List<PojistnyProdukt> pojistnyProdukts = pojistnyProduktModel.ReadInsuranceProducts(pageInfo);

            long count = pojistnyProduktModel.GetCount();
            ViewBag.count = count;

            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;
            return View(pojistnyProdukts);
        }

        // GET: PojistnyProduktController/Details/5
        public ActionResult Details(int id)
        {
            PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
            PojistnyProdukt pojistnyProdukt = pojistnyProduktModel.ReadInsuranceProduct(id);
            return View(pojistnyProdukt);
        }

        // GET: PojistnyProduktController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PojistnyProduktController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PojistnyProduktInputModel pojistnyProduktInputModel)
        {
            if(ModelState.IsValid)
            {
                PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
                pojistnyProduktModel.CreateInsuranceProduct(pojistnyProduktInputModel);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        // GET: PojistnyProduktController/Edit/5
        public ActionResult Edit(int id)
        {
            PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
            PojistnyProduktInputModel pojistnyProdukt = pojistnyProduktModel.ReadInsuranceProductAsInputModel(id);
            return View(pojistnyProdukt);
        }

        // POST: PojistnyProduktController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PojistnyProduktInputModel pojistnyProduktInputModel)
        {
            if(ModelState.IsValid)
            {
                PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
                pojistnyProduktModel.EditInsuranceProduct(pojistnyProduktInputModel, id);
                return RedirectToAction(nameof(Details), new { id });
            }
            else
            {
                return View();
            }
        }

        // GET: PojistnyProduktController/Delete/5
        public ActionResult Delete(int id)
        {
            PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
            PojistnyProdukt pojistnyProdukt = pojistnyProduktModel.ReadInsuranceProduct(id);
            return View(pojistnyProdukt);
        }

        // POST: PojistnyProduktController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, PojistnyProduktInputModel pojistnyProduktInputModel)
        {
            try
            {
                PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
                pojistnyProduktModel.ChangeInsuranceProductStatus(id);
                return RedirectToAction(nameof(Details), new { id });
            }
            catch
            {
                return View();
            }
        }
    }
}
