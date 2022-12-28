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
            PojistnaUdalost pojistnaUdalost = pojistnaUdalostModel.GetPojistnaUdalost(id);
            if (pojistnaUdalost == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(pojistnaUdalost);
        }

        // GET: PojistnaUdalostController/Create
        public ActionResult Create()
        {
            PojistnaUdalost pojistneUdalosti = new PojistnaUdalost();
            KlientModel klientModel = new KlientModel(_db);
            List<Klient> klients = klientModel.ReadClients();

            return View(new PojistnaUdalostCreateEditModel() { Klients = klients, PojistnaUdalost = pojistneUdalosti });
        }

        // POST: PojistnaUdalostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection form)
        {
            string klientId = form["PojistnaUdalost.Klient.KlientId"];
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                KlientModel klientModel = new KlientModel(_db);
                Klient klient = klientModel.GetClient(int.Parse(klientId));
                if (klient == null)
                {
                    return View();
                }
                PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
                pojistnaUdalostModel.CreatePojistnaUdalost(new PojistnaUdalost() { Klient = klient, NarokovanaVysePojistky = int.Parse(form["PojistnaUdalost.NarokovanaVysePojistky"]), Popis = form["PojistnaUdalost.Popis"], Vznik = DateTime.Parse(form["PojistnaUdalost.Vznik"]) });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                PojistnaUdalost pojistneUdalosti = new PojistnaUdalost();
                KlientModel klientModel = new KlientModel(_db);
                List<Klient> klients = klientModel.ReadClients();

                return View(new PojistnaUdalostCreateEditModel() { Klients = klients, PojistnaUdalost = pojistneUdalosti });
            }
        }

        // GET: PojistnaUdalostController/Edit/5
        public ActionResult Edit(int id)
        {
            PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
            PojistnaUdalost pojistneUdalosti = pojistnaUdalostModel.GetPojistnaUdalost(id);
            KlientModel klientModel = new KlientModel(_db);
            List<Klient> klients = klientModel.ReadClients();

            return View(new PojistnaUdalostCreateEditModel() { Klients = klients, PojistnaUdalost = pojistneUdalosti });
        }

        // POST: PojistnaUdalostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PojistnaUdalostEditModel model)
        {
            string klientId = model.KlientId;
            if (!ModelState.IsValid)
            {
                PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
                PojistnaUdalost pojistneUdalosti = pojistnaUdalostModel.GetPojistnaUdalost(id);
                KlientModel klientModel = new KlientModel(_db);
                List<Klient> klients = klientModel.ReadClients();
                return View(new PojistnaUdalostCreateEditModel() { Klients = klients, PojistnaUdalost = pojistneUdalosti });
            }
            try
            {
                KlientModel klientModel = new KlientModel(_db);
                Klient klient = klientModel.GetClient(int.Parse(klientId));
                if (klient == null)
                {
                    PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
                    PojistnaUdalost pojistneUdalosti = pojistnaUdalostModel.GetPojistnaUdalost(id);
                    List<Klient> klients = klientModel.ReadClients();
                    return View(new PojistnaUdalostCreateEditModel() { Klients = klients, PojistnaUdalost = pojistneUdalosti });
                }
                else
                {
                    PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
                    pojistnaUdalostModel.UpdatePojistnaUdalost(id, model);
                    return RedirectToAction(nameof(Details), new { id });
                }
            }
            catch
            {
                PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
                PojistnaUdalost pojistneUdalosti = pojistnaUdalostModel.GetPojistnaUdalost(id);
                KlientModel klientModel = new KlientModel(_db);
                List<Klient> klients = klientModel.ReadClients();

                return View(new PojistnaUdalostCreateEditModel() { Klients = klients, PojistnaUdalost = pojistneUdalosti });
            }
        }

        // GET: PojistnaUdalostController/Delete/5
        public ActionResult Delete(int id)
        {
            PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
            PojistnaUdalost pojistnaUdalost = pojistnaUdalostModel.GetPojistnaUdalost(id);
            if (pojistnaUdalost == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(pojistnaUdalost);
        }

        // POST: PojistnaUdalostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            PojistnaUdalostModel pojistnaUdalostModel = new PojistnaUdalostModel(_db);
            try
            {
                pojistnaUdalostModel.DeletePojistnaUdalost(id);
                return RedirectToAction(nameof(Details), new { id });
            }
            catch
            {
                return RedirectToAction(nameof(Delete), new { id });
            }
        }
    }
}
