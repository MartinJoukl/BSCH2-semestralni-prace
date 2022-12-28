﻿using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.PodminkaModels;
using Informacni_System_Pojistovny.Models.Model.PohledavkaModels;
using Informacni_System_Pojistovny.Models.Model.Pojistka;
using Informacni_System_Pojistovny.Models.Model.PojistnyProduktModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Informacni_System_Pojistovny.Controllers
{
    public class PojistkaController : Controller
    {
        private readonly Db _db;

        public PojistkaController(Db db)
        {
            _db = db;
        }
        // GET: PojistkaController
        public ActionResult Index()
        {
            PojistkaModel pojistkaModel = new PojistkaModel(_db);
            List<Pojistka> pojistky = pojistkaModel.ReadInsurances();
            return View(pojistky);
        }

        // GET: PojistkaController/Details/5
        public ActionResult Details(int id)
        {
            PojistkaModel pojistkaModel = new PojistkaModel(_db);
            PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
            Pojistka pojistka = pojistkaModel.ReadInsurance(id);
            pojistka.Pohledavky = pohledavkaModel.ListPohledavka(id);
            return View(pojistka);
        }

        public ActionResult AddCondition(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCondition(int id, PodminkaCreateModel podminkaCreateModel)
        {
            if (ModelState.IsValid)
            {
                PojistkaModel pojistkaModel = new PojistkaModel(_db);
                pojistkaModel.AddConditionToInsurance(id, podminkaCreateModel);

                return RedirectToAction(nameof(Index));
            }
            else return View();
        }

        // GET: PojistkaController/Create
        public ActionResult Create()
        {
            PojistnyProduktModel pojistnyProduktModel = new PojistnyProduktModel(_db);
            List<SelectListItem> produkty = pojistnyProduktModel.ReadInsuranceProductsAsSelectListItems(false);
            ViewBag.produkty = produkty;
            KlientModel klientModel = new KlientModel(_db);
            List<SelectListItem> klienti = klientModel.ReadClientsAsSelectList();
            ViewBag.klienti = klienti;
            ViewBag.produkty = produkty;
            return View();
        }

        // POST: PojistkaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PojistkaCreateModel pojistkaCreateModel)
        {
            if(ModelState.IsValid)
            {
                PojistkaModel pojistkaModel = new PojistkaModel(_db);
                pojistkaModel.CreateInsurance(pojistkaCreateModel);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        // GET: PojistkaController/Edit/5
        public ActionResult Edit(int id)
        {
            PojistkaModel pojistkaModel = new PojistkaModel(_db);
            PojistkaEditModel pojistkaEditModel = pojistkaModel.ReadInsuranceAsEditModel(id);
            return View(pojistkaEditModel);
        }

        // POST: PojistkaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PojistkaEditModel pojistkaEditModel)
        {
            if(ModelState.IsValid)
            {
                PojistkaModel pojistkaModel = new PojistkaModel(_db);
                pojistkaModel.ChangeInsurance(id, pojistkaEditModel);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(pojistkaEditModel);
            }
        }

        // GET: PojistkaController/Delete/5
        public ActionResult Delete(int id)
        {
            PojistkaModel pojistkaModel = new PojistkaModel(_db);
            Pojistka pojistka = pojistkaModel.ReadInsurance(id);
            return View(pojistka);
        }

        // POST: PojistkaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                PojistkaModel pojistkaModel = new PojistkaModel(_db);
                pojistkaModel.ChangeInsuranceStatus(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: PojistkaController/PermanentDelete/5
        public ActionResult PermanentDelete(int id)
        {
            PojistkaModel pojistkaModel = new PojistkaModel(_db);
            Pojistka pojistka = pojistkaModel.ReadInsurance(id);
            return View(pojistka);
        }

        // POST: PojistkaController/PermanentDelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PermanentDelete(int id, IFormCollection collection)
        {
            try
            {
                PojistkaModel pojistkaModel = new PojistkaModel(_db);
                pojistkaModel.DeleteInsurance(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
