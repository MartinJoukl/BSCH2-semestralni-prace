using Informacni_System_Pojistovny.Models;
using Informacni_System_Pojistovny.Models.Dao.BlogPostApi;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Informacni_System_Pojistovny.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Db db = new Db();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}