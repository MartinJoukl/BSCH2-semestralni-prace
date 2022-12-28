using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Informacni_System_Pojistovny.Controllers
{
    public class PscController : Controller
    {
        private readonly Db _db;

        public PscController(Db db)
        {
            _db = db;
        }

        // GET: PscController
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Index()
        {
            PscModel pscModel = new PscModel(_db);
            return View(pscModel.ReadPscs());
        }

        // GET: PscController/Details/5
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PscController/Create
        [Authorize(Roles = nameof(UzivateleRole.User))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Psc psc)
        {
            if (ModelState.IsValid)
            {
                PscModel pscModel = new PscModel(_db);
                pscModel.CreatePsc(psc);
                return RedirectToAction(nameof(Index));
            }
            else { 
                return View();
            }
        }

        // GET: KlientController/Create
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Create()
        {
            return View();
        }

        // GET: PscController/Edit/5
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PscController/Edit/5
        [Authorize(Roles = nameof(UzivateleRole.User))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string pscCislo, PscEditModel pscEditModel)
        {
            if (ModelState.IsValid)
            {
                PscModel pscModel = new PscModel(_db);
                if (pscModel.EditPsc(pscCislo, pscEditModel)) {
                    return RedirectToAction(nameof(Index));
                } else return View();
            } else return View();
        }

        // GET: PscController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PscController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string pscCislo)
        {
            try
            {
                PscModel pscModel = new PscModel(_db);
                pscModel.DeletePSC(pscCislo);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
