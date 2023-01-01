using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model.UserObjectsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Informacni_System_Pojistovny.Controllers
{
    public class UserObjectsController : Controller
    {
        private readonly Db _db;

        public UserObjectsController(Db db)
        {
            _db = db;
        }
        // GET: UserObjects
        [Authorize(Roles = nameof(UzivateleRole.Admin))]
        public ActionResult Index()
        {
            UserObjectsModel userObjectsModel = new UserObjectsModel(_db);
            List<UserObjects> userObjects = userObjectsModel.ReadUserObjects();
            _db.Dispose();
            return View(userObjects);
        }
    }
}
