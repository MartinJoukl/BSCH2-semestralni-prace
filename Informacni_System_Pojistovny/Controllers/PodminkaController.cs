using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.PodminkaModels;
using Informacni_System_Pojistovny.Models.Model.Pojistka;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Informacni_System_Pojistovny.Controllers
{
    public class PodminkaController : Controller
    {
        private readonly Db _db;

        public PodminkaController(Db db)
        {
            _db = db;
        }

        // GET: PodminkyController
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Index(PageInfo pageInfo, string CurrentFilter)
        {
            PodminkaModel podminkaModel = new PodminkaModel(_db);

            long count = podminkaModel.GetCount(CurrentFilter);
            ViewBag.count = count;

            ViewBag.CurrentFilter = CurrentFilter;
            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;

            List<Podminka> podminky = podminkaModel.ReadConditions(pageInfo, CurrentFilter);
            return View(podminky);
        }

        // GET: PodminkyController/Details/5
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Details(int id)
        {
            PodminkaModel podminkaModel = new PodminkaModel(_db);
            Podminka podminka = podminkaModel.ReadCondition(id);
            if (podminka == null) {
              ViewBag.errorMessage = "Podmínka nebyla nalezena";
            }
            return View(podminka);
        }

        // GET: PodminkyController/Create
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PodminkyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create(int id, PodminkaCreateModel podminkaCreateModel)
        {
            if (ModelState.IsValid)
            {
                PodminkaModel podminkaModel = new PodminkaModel(_db);
                podminkaModel.CreateCondition(podminkaCreateModel);

                return RedirectToAction(nameof(Index));
            }
            else return View();
        }

        // GET: PodminkyController/Edit/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(int id)
        {
            PodminkaModel podminkaModel = new PodminkaModel(_db);
            PodminkaCreateModel podminkaCreateModel = podminkaModel.ReadConditionAsCreateModel(id);
            return View(podminkaCreateModel);
        }

        // POST: PodminkyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(int id, PodminkaCreateModel podminkaCreateModel)
        {
            try
            {
                PodminkaModel podminkaModel = new PodminkaModel(_db);
                podminkaModel.ChangeCondition(id, podminkaCreateModel);
                return RedirectToAction(nameof(Details), new { id });
            }
            catch(Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View();
            }
        }

        // GET: PodminkyController/Delete/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(int id)
        {
            PodminkaModel podminkaModel = new PodminkaModel(_db);
            Podminka podminka = podminkaModel.ReadCondition(id);
            if(podminka == null)
            {
               ViewBag.errorMessage = "Podmínka nebyla nalezena";
            }
            return View(podminka);
        }

        // POST: PodminkyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                PodminkaModel podminkaModel = new PodminkaModel(_db);
                podminkaModel.DeleteCondition(id);
                _db.Dispose();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                _db.Dispose();
                return View();
            }
        }
    }
}
