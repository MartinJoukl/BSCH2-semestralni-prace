using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Informacni_System_Pojistovny.Controllers
{
    public class UzivatelController : Controller
    {
        // GET: UzivatelController
        public ActionResult Index()
        {
            UzivatelModel uzivatelModel = new UzivatelModel();
            return View("Index", uzivatelModel.ListUzivatel());
        }

        // GET: UzivatelController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UzivatelController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UzivatelController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            Uzivatel uzivatel;
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: UzivatelController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UzivatelLoginFormModel model)
        {
            UzivatelModel uzivatelModel = new UzivatelModel();
            Uzivatel uzivatel = null;
            if (ModelState.IsValid)
            {
                uzivatel = uzivatelModel.Login(model.Mail, model.Heslo);
            }
            //ASP VALIDACE
            if (uzivatel != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        // POST: UzivatelController/Login
        [HttpGet]
        [ActionName("Login")]
        public ActionResult LoginGet()
        {
            return View();
        }

        // GET: UzivatelController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UzivatelController/Edit/5
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

        // GET: UzivatelController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UzivatelController/Delete/5
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
