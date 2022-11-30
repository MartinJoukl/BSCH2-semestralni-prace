using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Informacni_System_Pojistovny.Controllers
{
    public class UzivatelController : Controller
    {
        private readonly Db _db;
        public UzivatelController(Db db)
        {
            _db = db;
        }
        // GET: UzivatelController
        [Authorize(Roles = "0")]
        public ActionResult Index()
        {
            UzivatelModel uzivatelModel = new UzivatelModel(_db);
            return View("Index", uzivatelModel.ListUzivatel());
        }

        // GET: UzivatelController/Details/5
        [Authorize(Roles = "0")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UzivatelController/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: UzivatelController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Register")]
        public async Task<ActionResult> RegisterAsync(UzivatelRegisterFormModel model)
        {
            Uzivatel uzivatel;
            if (ModelState.IsValid)
            {
                try
                {
                    UzivatelModel uzivatelModel = new UzivatelModel(_db);
                    uzivatelModel.Register(model);
                    return await LoginAsync(new UzivatelLoginFormModel() { Heslo = model.Heslo, Mail = model.Mail });
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        // POST: UzivatelController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Login")]
        public async Task<ActionResult> LoginAsync(UzivatelLoginFormModel model)
        {
            UzivatelModel uzivatelModel = new UzivatelModel(_db);
            Uzivatel uzivatel = null;
            if (ModelState.IsValid)
            {
                uzivatel = uzivatelModel.Login(model.Mail, model.Heslo);
            }
            if (uzivatel != null)
            {
                var claims = new List<Claim>
                 {
                 new Claim(ClaimTypes.Email, uzivatel.Email),
                 new Claim(ClaimTypes.Name, uzivatel.Jmeno),
                 new Claim(ClaimTypes.Surname, uzivatel.Prijmeni),
                 new Claim(ClaimTypes.Role, uzivatel.UrovenOpravneni.ToString()),
                 new Claim("CasZmeny", uzivatel.casZmeny.ToString()),
                 new Claim("Id", uzivatel.Id.ToString())
                 };

                var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties();
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (model.RedirectToUrl != null)
                {
                    return Redirect(model.RedirectToUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return LoginGet(model.RedirectToUrl);
            }
        }
        // POST: UzivatelController/Login
        [HttpGet]
        [ActionName("Login")]
        public ActionResult LoginGet(string returnUrl)
        {
            return View("Login", new UzivatelLoginFormModel { RedirectToUrl = returnUrl });
        }
        // POST: UzivatelController/Logout
        [HttpPost]
        public async Task<ActionResult> LogoutAsync()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(
    CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index), "Home");
        }

        // GET: UzivatelController/Edit/5
        [Authorize(Roles = "0")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UzivatelController/Edit/5
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
        [Authorize(Roles = "0")]
        public ActionResult Impersonifikace(int id)
        {
            UzivatelModel uzivatelModel = new UzivatelModel(_db);
            Uzivatel impersonifikovany = uzivatelModel.Impersonifikuj(id);
            if (impersonifikovany == null)
            {
                throw new Exception("Impersonifikace selhala!");
            }
            HttpContext.Session.SetString("ImpJmeno", impersonifikovany.Jmeno);
            HttpContext.Session.SetString("ImpPrijmeni", impersonifikovany.Prijmeni);
            HttpContext.Session.SetInt32("ImpId", impersonifikovany.Id);
            HttpContext.Session.SetString("ImpMail", impersonifikovany.Email);
            HttpContext.Session.SetInt32("ImpOpravneni", impersonifikovany.UrovenOpravneni);

            return RedirectToAction(nameof(Index), "Home");
        }
        [HttpGet]
        [Authorize(Roles = "0")]
        public ActionResult ZrusImpersonifikaci()
        {
            HttpContext.Session.Remove("ImpJmeno");
            HttpContext.Session.Remove("ImpPrijmeni");
            HttpContext.Session.Remove("ImpId");
            HttpContext.Session.Remove("ImpMail");
            HttpContext.Session.Remove("ImpOpravneni");

            return RedirectToAction(nameof(Index), "Home");
        }


        // GET: UzivatelController/Delete/5
        [Authorize(Roles = "0")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UzivatelController/Delete/5
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
