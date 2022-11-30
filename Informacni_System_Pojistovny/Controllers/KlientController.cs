using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [Authorize(Roles = "0")]
        public ActionResult Index()
        {
            KlientModel klientModel = new KlientModel(_db);
            return View(klientModel.klients());
        }

        // GET: KlientController/Details/5
        [Authorize(Roles = "0")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: KlientController/Create
        [Authorize(Roles = "0")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: KlientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "0")]
        public ActionResult Create(IFormCollection collection)
        {
            //try {
                KlientModel model = new KlientModel(_db);
                model.CreateClient(collection);
                return RedirectToAction(nameof(Index));
            //}
            /*
            catch {
                return View();
            }*/
        }

        // GET: KlientController/Edit/5
        [Authorize(Roles = "0")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: KlientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "0")]
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

        // GET: KlientController/Delete/5
        [Authorize(Roles = "0")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: KlientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "0")]
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
