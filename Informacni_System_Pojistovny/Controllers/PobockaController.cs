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
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Index(PageInfo pageInfo)
        {
            PobockaModel pobockaModel = new PobockaModel(_db);
            List<Pobocka> pobockas = pobockaModel.ReadBranches(pageInfo);
            long count = pobockaModel.GetCount();
            ViewBag.count = count;

            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;
            return View(pobockas);
        }

        // GET: PobockaController/Details/5
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Details(int id)
        {
            PobockaModel pobockaModel = new PobockaModel(_db);
            Pobocka pobocka = pobockaModel.ReadBranch(id);
            pobocka.Adresa = pobockaModel.GetBranchAddress(id);
            return View(pobocka);
        }

        // GET: PobockaController/Create
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create()
        {
            PscModel pscModel = new PscModel(_db);
            List<SelectListItem> pscs = pscModel.ReadPscsAsSelectListItems();
            ViewBag.pscs = pscs;
            return View();
        }

        // POST: PobockaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create(PobockaCreateModel pobockaEditModel)
        {
            if (ModelState.IsValid)
            {
                PobockaModel pobockaModel = new PobockaModel(_db);
                pobockaModel.CreateBranch(pobockaEditModel);
                return RedirectToAction(nameof(Index));
            }
            else { return View(); }
        }

        // GET: KlientController/AddAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult AddAddress(int id)
        {
            PscModel pscModel = new PscModel(_db);
            List<SelectListItem> pscs = pscModel.ReadPscsAsSelectListItems();
            ViewBag.pscs = pscs;
            return View();
        }

        // GET: KlientController/AddAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        [HttpPost]
        public ActionResult AddAddress(AdresaInputModel adresa, int id)
        {
            if (ModelState.IsValid)
            {
                PobockaModel pobockaEditModel = new PobockaModel(_db);
                pobockaEditModel.AddAddressesToBranch(id, adresa);

                return RedirectToAction(nameof(Details), new { id });
            }
            else return View(id);
        }


        // Post: PobockaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(int id, PobockaEditModel pobockaEditModel)
        {
            if (ModelState.IsValid)
            {
                PobockaModel pobockaModel = new PobockaModel(_db);
                try
                {
                    pobockaModel.RealizePobockaEdit(pobockaEditModel, id);
                    return RedirectToAction(nameof(Details), new { id });
                } catch (Exception ex)
                {
                    return View();
                }
            }
            else return View();
        }

        // GET: PobockaController/Edit/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
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
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(int id)
        {
            PobockaModel pobockaModel = new PobockaModel(_db);
            PobockaEditModel pobocka = pobockaModel.ReadBranchAsPobockaEdit(id);
            return View(pobocka);
        }

        // POST: PobockaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(int id, PobockaEditModel pobockaEditModel)
        {
            PobockaModel pobockaModel = new PobockaModel(_db);
            pobockaModel.DeleteBranch(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: PobockaController/EditAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult EditAddress(int id, int redirectTo)
        {
            PscModel pscModel = new PscModel(_db);
            List<SelectListItem> pscs = pscModel.ReadPscsAsSelectListItems();
            AdresaModel adresaModel = new AdresaModel(_db);
            AdresaInputModel adresaInputModel = adresaModel.ReadAddressAsEditModel(id);
            ViewBag.pscs = pscs;
            ViewBag.redirectTo = redirectTo;
            return View(adresaInputModel);
        }

        // ´POST: PobockaController/AddAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        [HttpPost]
        public ActionResult EditAddress(AdresaInputModel adresa, int id, int redirectTo, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                AdresaModel adresaModel = new AdresaModel(_db);
                adresaModel.EditAddress(id, adresa);

                return RedirectToAction(nameof(Details), new { id = collection["redirectTo"] });
            }
            else return View();
        }

        // GET: PobockaController/DeleteAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult DeleteAddress(int id, int redirectTo)
        {
            PscModel pscModel = new PscModel(_db);
            List<SelectListItem> pscs = pscModel.ReadPscsAsSelectListItems();
            AdresaModel adresaModel = new AdresaModel(_db);
            AdresaInputModel adresaInputModel = adresaModel.ReadAddressAsEditModel(id);
            ViewBag.redirectTo = redirectTo;
            ViewBag.pscs = pscs;
            return View(adresaInputModel);
        }

        // POST: PobockaController/AddAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        [HttpPost]
        public ActionResult DeleteAddress(AdresaInputModel adresa, int id, IFormCollection collection)
        {
            AdresaModel adresaModel = new AdresaModel(_db);
            adresaModel.DeleteAddress(id);

            return RedirectToAction(nameof(Details), new { id = collection["redirectTo"] });
        }
    }
}
