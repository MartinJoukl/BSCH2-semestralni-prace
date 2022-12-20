using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Informacni_System_Pojistovny.Controllers
{
    public class PobockaController : Controller
    {
        private readonly Db _db;

        public PobockaController(Db db)
        {
            _db = db;
        }
        // GET: PobockaController
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Index()
        {
            PobockaModel pobockaModel = new PobockaModel(_db);
            List<Pobocka> pobockas = pobockaModel.ReadBranches();
            return View(pobockas);
        }

        // GET: PobockaController/Details/5
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Details(int id)
        {
            PobockaModel pobockaModel = new PobockaModel(_db);
            Pobocka pobocka = pobockaModel.ReadBranch(id);
            pobocka.Adresa = pobockaModel.GetBranchAddress(id);
            return View(pobocka);
        }

        // GET: PobockaController/Create
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PobockaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Create(PobockaEditModel pobockaEditModel)
        {
            if (ModelState.IsValid) {
                try
                {
                    PobockaModel pobockaModel = new PobockaModel(_db);
                    pobockaModel.CreateBranch(pobockaEditModel);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            } else { return View(); }
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
            if (ModelState.IsValid)
            {
                PobockaModel pobockaEditModel = new PobockaModel(_db);
                pobockaEditModel.AddAddressesToBranch(id, adresa);

                return RedirectToAction(nameof(Index));
            }
            else return View(id);
        }


        // Post: PobockaController/Edit/5
        [HttpPost]
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Edit(int id, PobockaEditModel pobockaEditModel)
        {
            if (ModelState.IsValid) {
                PobockaModel pobockaModel = new PobockaModel(_db);
                try
                {
                    pobockaModel.RealizePobockaEdit(pobockaEditModel, id);
                    return RedirectToAction(nameof(Index));
                } catch (Exception ex)
                {
                    return View(id);
                }
            } else return View(id);
        }

        // GET: PobockaController/Edit/5
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Edit(int id)
        {
            PobockaModel pobockaModel = new PobockaModel(_db);
            PobockaEditModel pobockaEdit = pobockaModel.ReadBranchAsPobockaEdit(id);
            if (pobockaEdit == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(pobockaEdit);
            }
        }

        // GET: PobockaController/Delete/5
        [Authorize(Roles = nameof(UzivateleRole.User))]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PobockaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UzivateleRole.User))]
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
