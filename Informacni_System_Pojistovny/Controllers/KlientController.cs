using Informacni_System_Pojistovny.Models.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Informacni_System_Pojistovny.Controllers
{
    public class KlientController : Controller
    {
        // GET: KlientController
        public ActionResult Index()
        {
            KlientModel klientModel = new KlientModel();
            return View(klientModel.klients());
        }

        // GET: KlientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: KlientController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KlientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                String s = collection["jmeno"];
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: KlientController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: KlientController/Edit/5
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

        // GET: KlientController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: KlientController/Delete/5
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
