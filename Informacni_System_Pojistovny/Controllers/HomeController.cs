using Informacni_System_Pojistovny.Models;
using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.Uzivatele;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Informacni_System_Pojistovny.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Db _db;

        public HomeController(ILogger<HomeController> logger, Db db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            Uzivatel uzivatel = null;
            if (User.Identity.IsAuthenticated)
            {
                int id = int.Parse(HttpContext.User.Claims.Where((claim) => claim.Type == "Id").First().Value);
                UzivatelModel uzivatelModel = new UzivatelModel(_db);
                uzivatel = uzivatelModel.GetUzivatel(id);
            }
            return View(uzivatel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}