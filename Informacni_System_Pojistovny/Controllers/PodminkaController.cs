using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.PodminkaModels;
using Informacni_System_Pojistovny.Models.Model.Pojistka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult Index(PageInfo pageInfo)
        {
            PodminkaModel podminkaModel = new PodminkaModel(_db);

            long count = podminkaModel.GetCount();
            ViewBag.count = count;

            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;

            List<Podminka> podminky = podminkaModel.ReadConditions(pageInfo);
            return View(podminky);
        }

        // GET: PodminkyController/Details/5
        public ActionResult Details(int id)
        {
            PodminkaModel podminkaModel = new PodminkaModel(_db);
            Podminka podminka = podminkaModel.ReadCondition(id);
            return View(podminka);
        }

        // GET: PodminkyController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PodminkyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, PodminkaCreateModel podminkaCreateModel)
        {
            if (ModelState.IsValid)
            {
                PodminkaModel podminkaModel = new PodminkaModel(_db);
                podminkaModel.CreateCondition(podminkaCreateModel);

                return RedirectToAction(nameof(Details), new { id });
            }
            else return View();
        }

        // GET: PodminkyController/Edit/5
        public ActionResult Edit(int id)
        {
            PodminkaModel podminkaModel = new PodminkaModel(_db);
            PodminkaCreateModel podminkaCreateModel = podminkaModel.ReadConditionAsCreateModel(id);
            return View(podminkaCreateModel);
        }

        // POST: PodminkyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PodminkaCreateModel podminkaCreateModel)
        {
            try
            {
                PodminkaModel podminkaModel = new PodminkaModel(_db);
                podminkaModel.ChangeCondition(id, podminkaCreateModel);
                return RedirectToAction(nameof(Details), new { id });
            }
            catch
            {
                return View();
            }
        }

        // GET: PodminkyController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PodminkyController/Delete/5
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
