using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.PojistnaUdalostModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult Index()
        {
            PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
            List<PojistnaUdalost> pojistneUdalosti = pojistnaUdalostModel.ListPojistnaUdalost();
            return View(pojistneUdalosti);
        }

        // GET: PojistnaUdalostController/Details/5
        public ActionResult Details(int id)
        {
            PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
            PojistnaUdalost pojistneUdalosti = pojistnaUdalostModel.GetPojistnaUdalost(id);
            return View(pojistneUdalosti);
        }

        // GET: PojistnaUdalostController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PojistnaUdalostController/Create
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

        // GET: PojistnaUdalostController/Edit/5
        public ActionResult Edit(int id)
        {
            PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
            PojistnaUdalost pojistneUdalosti = pojistnaUdalostModel.GetPojistnaUdalost(id);
            KlientModel klientModel = new KlientModel(_db);
            return View();
        }

        // POST: PojistnaUdalostController/Edit/5
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

        // GET: PojistnaUdalostController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PojistnaUdalostController/Delete/5
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
