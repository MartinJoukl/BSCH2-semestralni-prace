using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.Pojistka;
using Informacni_System_Pojistovny.Models.Model.PojistnyProduktModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public ActionResult Index()
        {
            PojistkaModel pojistkaModel = new PojistkaModel(_db);
            List<Pojistka> pojisky = pojistkaModel.ReadInsurances();
            return View(pojisky);
        }

        // GET: PojistkaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PojistkaController/Create
        public ActionResult Create()
        {
            PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
            List<SelectListItem> produkty = pojistnyProduktModel.ReadInsuranceProductsAsSelectListItems(false);
            ViewBag.produkty = produkty;
            KlientModel klientModel = new KlientModel(_db);
            List<SelectListItem> klienti = klientModel.ReadClientsAsSelectList();
            ViewBag.klienti = klienti;
            ViewBag.produkty = produkty;
            return View();
        }

        // POST: PojistkaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PojistkaCreateModel pojistkaCreateModel)
        {
            if(ModelState.IsValid)
            {
                PojistkaModel pojistkaModel = new PojistkaModel(_db);
                pojistkaModel.CreateInsurance(pojistkaCreateModel);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        // GET: PojistkaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PojistkaController/Edit/5
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

        // GET: PojistkaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PojistkaController/Delete/5
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
