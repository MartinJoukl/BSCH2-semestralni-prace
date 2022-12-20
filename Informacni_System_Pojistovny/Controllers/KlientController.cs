using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Informacni_System_Pojistovny.Controllers
{
    public class KlientController : Controller
    {
        private readonly Db _db;

        public KlientController(Db db)
        {
            _db = db;
        }


        // GET: KlientController
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Index()
        {
            KlientModel klientModel = new KlientModel(_db);
            return View(klientModel.ReadClients());
        }

        // GET: KlientController/Details/5
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Details(int id)
        {
            KlientModel klientDb = new KlientModel(_db);
            Klient klient = klientDb.GetClient(id);
            klient.Adresy = klientDb.GetClientAddresses(id);
            return View(klient);
        }

        // GET: KlientController/Create
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Create()
        {
            return View();
        }

        // GET: KlientController/AddAddress
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult AddAddress(int id)
        {
            Console.WriteLine(id);
            PscModel pscModel = new PscModel(_db);
            List<SelectListItem> pscs = pscModel.ReadPscsAsSelectListItems();
            ViewBag.pscs = pscs;
            return View();
        }

        // GET: KlientController/AddAddress
        [Authorize(Roles = nameof(UzivateleRole.User))]
        [HttpPost]
        public ActionResult AddAddress(AdresaInputModel adresa, int id)
        {
            if (ModelState.IsValid) {
                KlientModel klientModel = new KlientModel(_db);
                klientModel.AddAddressToClient(id, adresa);

                return RedirectToAction(nameof(Index));
            } else return View(id);
        }

        // POST: KlientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Create(KlientCreateModel model, IFormCollection collection)
        {
            if (collection["zvolenyTypOsoby"].Equals("F"))
            {
                ModelState.Remove("Ico");
                ModelState.Remove("Nazev");
            } else {
                ModelState.Remove("Jmeno");
                ModelState.Remove("Prijmeni");
                ModelState.Remove("Telefon");
                ModelState.Remove("RodneCislo");
                ModelState.Remove("Email");
            }
            if (ModelState.IsValid) {
                KlientModel klientDb = new KlientModel(_db);
                klientDb.CreateClient(collection);
                return RedirectToAction(nameof(Index));
            }
            else {
                return View();
            }
        }

        // GET: KlientController/Edit/5
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Edit(int id)
        {
            KlientModel klientDb = new KlientModel(_db);
            KlientCreateModel klient = klientDb.GetEditClient(id);
            if (klient == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(klient);
            }
        }

        // POST: KlientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Edit(int id, KlientCreateModel model, IFormCollection collection)
        {
            if (collection["zvolenyTypOsoby"].Equals("F"))
            {
                ModelState.Remove("Ico");
                ModelState.Remove("Nazev");
            }
            else
            {
                ModelState.Remove("Jmeno");
                ModelState.Remove("Prijmeni");
                ModelState.Remove("Telefon");
                ModelState.Remove("RodneCislo");
                ModelState.Remove("Email");
            }
            if (ModelState.IsValid)
            {
                KlientModel klientDb = new KlientModel(_db);
                klientDb.RealizeEditClient(model, id, collection["zvolenyTypOsoby"]);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        // GET: KlientController/Delete/5
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Delete(int id)
        {
            KlientModel klientDb = new KlientModel(_db);
            Klient klient = klientDb.GetClient(id);

            if(klient == null) {
                return RedirectToAction(nameof(Index));
            }

            return View(klient);
        }

        // POST: KlientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Delete(int id, KlientCreateModel klient)
        {
            try
            {
                KlientModel klientDb = new KlientModel(_db);
                klientDb.ChangeClientStatus(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
