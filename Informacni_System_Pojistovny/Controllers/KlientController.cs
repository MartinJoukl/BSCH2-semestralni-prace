using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.DokumentModels;
using Informacni_System_Pojistovny.Models.Model.PojistnyProduktModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;

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
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)} ")]
        public ActionResult Index(PageInfo pageInfo, string CurrentFilter)
        {
            KlientModel klientModel = new KlientModel(_db);

            long count = klientModel.GetCount(CurrentFilter);
            ViewBag.count = count;
            ViewBag.CurrentFilter = CurrentFilter;

            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;

            return View(klientModel.ReadClients(pageInfo, CurrentFilter));
        }

        // GET: KlientController/Details/5
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Details(int id)
        {
            Klient klient = new();
            try
            {
                KlientModel klientDb = new KlientModel(_db);
                klient = klientDb.GetClient(id);
                klient.Adresy = klientDb.GetClientAddresses(id);
                klient.Dokumenty = klientDb.ReadClientDocuments(id);
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
            }
            return View(klient);
        }

        // GET: KlientController/Create
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create()
        {
            PscModel pscModel = new PscModel(_db);
            List<SelectListItem> pscs = pscModel.ReadPscsAsSelectListItems();
            ViewBag.pscs = pscs;
            return View();
        }

        // GET: KlientController/AddAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult AddAddress(int id)
        {
            PscModel pscModel = new PscModel(_db);
            List<SelectListItem> pscs = pscModel.ReadPscsAsSelectListItems();
            ViewBag.pscs = pscs;
            return View();
        }

        // POST: KlientController/AddAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        [HttpPost]
        public ActionResult AddAddress(AdresaInputModel adresa, int id)
        {
            if (ModelState.IsValid)
            {
                KlientModel klientModel = new KlientModel(_db);
                try
                {
                    klientModel.AddAddressToClient(id, adresa);
                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                }


                return RedirectToAction(nameof(Details), new { id });
            }
            else return View();
        }

        // GET: KlientController/EditAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult EditAddress(int id, int redirectTo)
        {
            AdresaInputModel adresaInputModel = new();
            try
            {
                PscModel pscModel = new PscModel(_db);
                List<SelectListItem> pscs = pscModel.ReadPscsAsSelectListItems();
                AdresaModel adresaModel = new AdresaModel(_db);
                adresaInputModel = adresaModel.ReadAddressAsEditModel(id);
                ViewBag.redirectTo = redirectTo;
                ViewBag.pscs = pscs;
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
            }
            return View(adresaInputModel);
        }

        // ´POST: KlientController/AddAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        [HttpPost]
        public ActionResult EditAddress(AdresaInputModel adresa, int id, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var x = RouteData.Values["redirectTo"];
                AdresaModel adresaModel = new AdresaModel(_db);
                adresaModel.EditAddress(id, adresa);
                return RedirectToAction(nameof(Details), new { id = collection["redirectTo"] });
            }
            else return View();
        }

        // GET: KlientController/DeleteAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult DeleteAddress(int id, int redirectTo)
        {
            PscModel pscModel = new PscModel(_db);
            List<SelectListItem> pscs = pscModel.ReadPscsAsSelectListItems();
            AdresaModel adresaModel = new AdresaModel(_db);
            AdresaInputModel adresaInputModel = adresaModel.ReadAddressAsEditModel(id);
            ViewBag.pscs = pscs;
            ViewBag.redirectTo = redirectTo;
            return View(adresaInputModel);
        }

        // ´POST: KlientController/AddAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        [HttpPost]
        public ActionResult DeleteAddress(AdresaInputModel adresa, int id, IFormCollection collection)
        {
            AdresaModel adresaModel = new AdresaModel(_db);
            adresaModel.DeleteAddress(id);

            return RedirectToAction(nameof(Details), new { id = collection["redirectTo"] });
        }

        // POST: KlientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Create(KlientCreateModel model, IFormCollection collection)
        {
            if (collection["zvolenyTypOsoby"].Equals("F"))
            {
                ModelState.Remove("Ico");
                ModelState.Remove("Nazev");
            }
            else
            {
                ModelState.Remove("Jmeno");
                ModelState.Remove("Prijmeni");
                ModelState.Remove("Telefon");
                ModelState.Remove("RodneCislo");
                ModelState.Remove("Email");
            }
            if (ModelState.IsValid)
            {
                KlientModel klientDb = new KlientModel(_db);
                klientDb.CreateClient(collection);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        // GET: KlientController/Edit/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(int id)
        {
            KlientModel klientDb = new KlientModel(_db);
            KlientEditModel klient = klientDb.GetEditClient(id);
            if (klient == null)
            {
                ViewBag.errorMessage = "Klient nebyl nalezen";
                return RedirectToAction(nameof(Details), new { id });
            }
            else
            {
                return View(klient);
            }
        }

        // POST: KlientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Edit(int id, KlientEditModel model, IFormCollection collection)
        {
            if (collection["zvolenyTypOsoby"].Equals("F"))
            {
                ModelState.Remove("Ico");
                ModelState.Remove("Nazev");
            }
            else
            {
                ModelState.Remove("Jmeno");
                ModelState.Remove("Prijmeni");
                ModelState.Remove("Telefon");
                ModelState.Remove("RodneCislo");
                ModelState.Remove("Email");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    KlientModel klientDb = new KlientModel(_db);
                    klientDb.RealizeEditClient(model, id, collection["zvolenyTypOsoby"]);
                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                }
                return RedirectToAction(nameof(Details), new { id });
            }
            else
            {
                return View();
            }
        }

        // GET: KlientController/Delete/5
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(int id)
        {

            KlientModel klientDb = new KlientModel(_db);
            Klient klient = klientDb.GetClient(id);

            if (klient == null)
            {
                ViewBag.errorMessage = "Klient nebyl nalezen";
                return RedirectToAction(nameof(Index));
            }

            return View(klient);
        }

        // POST: KlientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult Delete(int id, KlientEditModel klient)
        {
            try
            {
                KlientModel klientDb = new KlientModel(_db);
                klientDb.ChangeClientStatus(id);
                return RedirectToAction(nameof(Details), new { id });
            }
            catch(Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View();
            }
        }

        // GET: KlientController/AddAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult AddDocument(int id)
        {
            KlientModel klientModel = new KlientModel(_db);
            List<SelectListItem> klienti = klientModel.ReadClientsAsSelectList();
            ViewBag.klienti = klienti;
            return View();
        }

        // GET: KlientController/AddAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.User)},{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult DownloadDocument(int documentId)
        {
            KlientModel klientModel = new KlientModel(_db);
            Dokument dokument = klientModel.ReadDocument(documentId);
            if(dokument == null)
            {
                ViewBag.errorMessage = "Soubor nebyl nalezen"; //nefunguje, nemělo by ale normálně nastat
                return NoContent();
            }
            byte[] bytes = dokument.Data;
            return File(bytes, dokument.Typ, dokument.Nazev + dokument.Pripona);
        }

        // POST: KlientController/AddAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        [HttpPost]
        public ActionResult AddDocument(DokumentUploadModel dokumentUploadModel, int id)
        {
            if (ModelState.IsValid)
            {
                IFormFile nahranySoubor = dokumentUploadModel.Data;
                KlientModel klientModel = new KlientModel(_db);
                try
                {
                    klientModel.AddDocumentToClient(id, dokumentUploadModel);
                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                }
                return RedirectToAction(nameof(Details), new { id });
            }
            else return View();
        }

        // GET: KlientController/DeleteAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        public ActionResult DeleteDocument(int id, int redirectTo)
        {
            KlientModel klientModel = new KlientModel(_db);
            Dokument dokument = new();
            try
            {
                dokument = klientModel.ReadDocument(id);
                dokument.Klient = klientModel.GetClient(redirectTo);
            }catch(Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
            }
            ViewBag.redirectTo = redirectTo;
            return View(dokument);
        }

        // ´POST: KlientController/AddAddress
        [Authorize(Roles = $"{nameof(UzivateleRole.PriviledgedUser)},{nameof(UzivateleRole.Admin)}")]
        [HttpPost]
        public ActionResult DeleteDocument(Dokument dokument, int id, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                KlientModel klientModel = new KlientModel(_db);
                klientModel.DeleteDocument(id);

                return RedirectToAction(nameof(Details), new { id = collection["redirectTo"] });
            }
            else return View();
        }
    }
}
