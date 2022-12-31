using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Informacni_System_Pojistovny.Models.Model.Uzivatele;
using Informacni_System_Pojistovny.Models.Model.PojistnyProduktModels;
using Informacni_System_Pojistovny.Models.Model;

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
        [Authorize(Roles = nameof(UzivateleRole.Admin))]
        public ActionResult Index(PageInfo pageInfo)
        {
            UzivatelModel uzivatelModel = new UzivatelModel(_db);
            long count = uzivatelModel.GetCount();
            ViewBag.count = count;

            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;

            return View("Index", uzivatelModel.ListUzivatel(pageInfo));
        }

        // GET: UzivatelController/Details/5
        [Authorize(Roles = nameof(UzivateleRole.Admin))]
        public ActionResult Details(int id)
        {
            UzivatelModel uzivatelModel = new UzivatelModel(_db);
            Uzivatel uzivatel = uzivatelModel.GetUzivatel(id);
            if (uzivatel != null)
            {
                return View(uzivatel);
            }
            else
            {
                return Index(new PageInfo());
            }
        }

        // GET: UzivatelController/Register
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: UzivatelController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Register")]
        public async Task<ActionResult> RegisterAsync(UzivatelRegisterFormModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            Uzivatel uzivatel;
            if (ModelState.IsValid)
            {
                try
                {
                    UzivatelModel uzivatelModel = new UzivatelModel(_db);
                    uzivatelModel.Create(model);
                    return await LoginAsync(new UzivatelLoginFormModel() { Heslo = model.Heslo, Mail = model.Mail });
                }
                catch
                {
                    ViewData["error"] = "Registrace selhala";
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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            UzivatelModel uzivatelModel = new UzivatelModel(_db);
            Uzivatel uzivatel = null;
            if (ModelState.IsValid)
            {
                uzivatel = uzivatelModel.Login(model.Mail, model.Heslo);
            }

            if (uzivatel != null)
            {
                await LoginUser(uzivatel);

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

        private async Task LoginUser(Uzivatel uzivatel)
        {
            var claims = new List<Claim>
                 {
                 new Claim(ClaimTypes.Email, uzivatel.Email),
                 new Claim(ClaimTypes.Name, uzivatel.Jmeno),
                 new Claim(ClaimTypes.Surname, uzivatel.Prijmeni),
                 new Claim(ClaimTypes.Role, uzivatel.Role.ToString()),
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
        }

        // POST: UzivatelController/Login
        [HttpGet]
        [ActionName("Login")]
        public ActionResult LoginGet(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
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

        [Authorize(Roles = nameof(UzivateleRole.Admin))]
        public async Task<ActionResult> ImpersonifikaceAsync(int id)
        {
            UzivatelModel uzivatelModel = new UzivatelModel(_db);
            Uzivatel impersonifikovany = uzivatelModel.GetUzivatel(id);
            if (impersonifikovany == null)
            {
                throw new Exception("Impersonifikace selhala!");
            }


            var claims = new List<Claim>
                 {
                 new Claim(ClaimTypes.Email, impersonifikovany.Email),
                 new Claim(ClaimTypes.Name, impersonifikovany.Jmeno),
                 new Claim(ClaimTypes.Surname, impersonifikovany.Prijmeni),
                 new Claim(ClaimTypes.Role, impersonifikovany.Role.ToString()),
                 new Claim("CasZmeny", impersonifikovany.casZmeny.ToString()),
                 new Claim("Id", impersonifikovany.Id.ToString())
                 };

            //check if impersonification is already active
            if (HttpContext.User.Claims.Where((claim) => claim.Type == "originalId").FirstOrDefault()?.Value != null)
            {
                //default values of new Claims dont change
                string originalMail = HttpContext.User.Claims.Where((claim) => claim.Type == "originalMail").First().Value;
                string originalRole = HttpContext.User.Claims.Where((claim) => claim.Type == "originalRole").First().Value;
                string originalCasZmeny = HttpContext.User.Claims.Where((claim) => claim.Type == "originalCasZmeny").First().Value;
                string originalId = HttpContext.User.Claims.Where((claim) => claim.Type == "originalId").First().Value;

                claims.Add(new Claim("originalMail", originalMail));
                claims.Add(new Claim("originalRole", originalRole));
                claims.Add(new Claim("originalCasZmeny", originalCasZmeny));
                claims.Add(new Claim("originalId", originalId));
            }
            else
            {
                //keep originnal claims
                string originalMail = HttpContext.User.Claims.Where((claim) => claim.Type == ClaimTypes.Email).First().Value;
                string originalRole = HttpContext.User.Claims.Where((claim) => claim.Type == ClaimTypes.Role).First().Value;
                string originalCasZmeny = HttpContext.User.Claims.Where((claim) => claim.Type == "CasZmeny").First().Value;
                string originalId = HttpContext.User.Claims.Where((claim) => claim.Type == "Id").First().Value;

                claims.Add(new Claim("originalMail", originalMail));
                claims.Add(new Claim("originalRole", originalRole));
                claims.Add(new Claim("originalCasZmeny", originalCasZmeny));
                claims.Add(new Claim("originalId", originalId));
            }

            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult ZrusImpersonifikaci()
        {
            string originalId = HttpContext.User.Claims.Where((claim) => claim.Type == "originalId").First().Value;
            UzivatelModel uzivatelModel = new UzivatelModel(_db);
            Uzivatel uzivatel = uzivatelModel.ForceLogin(int.Parse(originalId));
            if (uzivatel == null)
            {
                throw new Exception("Zrušení impersonifikace selhalo!");
            }

            LoginUser(uzivatel);

            return RedirectToAction(nameof(Index), "Home");
        }


        // GET: UzivatelController/Delete/5
        [Authorize(Roles = nameof(UzivateleRole.Admin))]
        public ActionResult Delete(int id)
        {
            UzivatelModel uzivatelModel = new UzivatelModel(_db);
            Uzivatel uzivatel = uzivatelModel.GetUzivatel(id);
            if (uzivatel != null)
            {
                return View(uzivatel);
            }
            else
            {
                return Index(new PageInfo());
            }
        }

        // GET: UzivatelController/EditOwnProfile
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult EditOwnProfile()
        {
            int id = int.Parse(HttpContext.User.Claims.Where((claim) => claim.Type == "Id").First().Value);
            UzivatelModel uzivatelModel = new UzivatelModel(_db);
            Uzivatel uzivatel = uzivatelModel.GetUzivatel(id);
            return View(new EditOwnProfileModel() { Jmeno = uzivatel.Jmeno, Mail = uzivatel.Email, Prijmeni = uzivatel.Prijmeni });
        }

        // GET: UzivatelController/Edit
        [Authorize(Roles = nameof(UzivateleRole.Admin))]
        public ActionResult Edit(int id)
        {
            UzivatelModel uzivatelModel = new UzivatelModel(_db);
            Uzivatel editovany = uzivatelModel.GetUzivatel(id);
            return View(new EditUserModel() { Id = id, Jmeno = editovany.Jmeno, Role = editovany.Role, Mail = editovany.Email, Prijmeni = editovany.Prijmeni });
        }

        // GET: UzivatelController/Edit
        [Authorize(Roles = nameof(UzivateleRole.Admin))]
        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditPost(EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                UzivatelModel uzivatelModel = new UzivatelModel(_db);
                Uzivatel uzivatel = uzivatelModel.EditUzivatel(model, model.Id);
                return Index(new PageInfo());
            }
            else
            {
                return View();
            }
        }

        // GET: UzivatelController/EditOwnProfile
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        [HttpPost]
        [ActionName("EditOwnProfile")]
        public ActionResult EditOwnProfilePost(EditOwnProfileModel model)
        {
            int id = int.Parse(HttpContext.User.Claims.Where((claim) => claim.Type == "Id").First().Value);
            UzivatelModel uzivatelModel = new UzivatelModel(_db);
            Uzivatel uzivatelOriginal = uzivatelModel.GetUzivatel(id);
            model.Role = uzivatelOriginal.Role;
            Uzivatel uzivatel = uzivatelModel.EditUzivatel(model, id);

            return RedirectToAction(nameof(Index), "Home");
        }

        // POST: UzivatelController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(UzivateleRole.Admin))]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                UzivatelModel uzivatelModel = new UzivatelModel(_db);
                uzivatelModel.DeleteUzivatel(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
