using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.PojistnyProduktModels;
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
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Index(PageInfo pageInfo, string CurrentFilter)
        {
            PscModel pscModel = new PscModel(_db);

            long count = pscModel.GetCount(CurrentFilter);
            ViewBag.count = count;

            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;
            ViewBag.CurrentFilter = CurrentFilter;

            return View(pscModel.ReadPscs(pageInfo, CurrentFilter));
        }
        // GET: PscController/Details/5
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PscController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
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
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create()
        {
            return View();
        }

        // GET: PscController/Edit/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(string id)
        {
            PscModel pscModel = new PscModel(_db);
            PscEditModel psc = pscModel.ReadPscAsEditModel(id);
            return View(psc);
        }

        // POST: PscController/Edit/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, PscEditModel pscEditModel)
        {
            if (ModelState.IsValid)
            {
                PscModel pscModel = new PscModel(_db);
                if (pscModel.EditPsc(id, pscEditModel)) {
                    return RedirectToAction(nameof(Index));
                } else return View();
            } else return View();
        }

        // GET: PscController/Delete/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(string id)
        {
            PscModel pscModel = new PscModel(_db);
            Psc psc = pscModel.ReadPsc(id);
            return View(psc);
        }

        // POST: PscController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(string id, Psc psc)
        {
            try
            {
                PscModel pscModel = new PscModel(_db);
                pscModel.DeletePSC(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
