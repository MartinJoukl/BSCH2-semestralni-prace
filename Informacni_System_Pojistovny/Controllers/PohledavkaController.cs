﻿using Informacni_System_Pojistovny.Models.Dao;
using Informacni_System_Pojistovny.Models.Entity;
using Informacni_System_Pojistovny.Models.Model;
using Informacni_System_Pojistovny.Models.Model.PohledavkaModels;
using Informacni_System_Pojistovny.Models.Model.Pojistka;
using Informacni_System_Pojistovny.Models.Model.PojistnaUdalostModels;
using Informacni_System_Pojistovny.Models.Model.ZavazekModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Informacni_System_Pojistovny.Controllers
{
    public class PohledavkaController : Controller
    {
        private readonly Db _db;
        public PohledavkaController(Db db)
        {
            _db = db;
        }
        // GET: ZavazkyController
        public ActionResult Index(PageInfo pageInfo)
        {
            PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
            List<Pohledavka> pohledavky = pohledavkaModel.ListPohledavka(pageInfo);
            long count = pohledavkaModel.GetCount();
            ViewBag.count = count;

            ViewBag.PageSize = pageInfo.PageSize;
            ViewBag.PageIndex = pageInfo.PageIndex;
            return View(pohledavky);
        }

        // GET: ZavazkyController/Details/5
        public ActionResult Details(int id)
        {
            PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
            Pohledavka pohledavka = pohledavkaModel.GetPohledavkaPojistka(id);
            if (pohledavka == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(pohledavka);
        }

        // GET: PohledavkyController/Create
        public ActionResult Create(int pojistkaId)
        {
            ViewBag.pojistkaId = pojistkaId;
            return View();
        }

        // POST: PohledavkyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PohledavkaCreateModel pohledavkaCreateModel)
        {
            if (!ModelState.IsValid || pohledavkaCreateModel.Vznik > pohledavkaCreateModel.DatumSplatnosti)
            {
                ViewBag.pojistkaId = pohledavkaCreateModel.PojistkaId;
                return View();
            }
            try
            {
                PojistkaModel pojistkaModel = new PojistkaModel(_db);
                Pojistka pojistka = pojistkaModel.ReadInsurance((int)pohledavkaCreateModel.PojistkaId);
                if (pojistka != null)
                {
                    PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
                    pohledavkaModel.CreatePohledavka(pohledavkaCreateModel);
                }
                return RedirectToAction("Details", "Pojistka", new { id = pohledavkaCreateModel.PojistkaId });
            }
            catch
            {
                ViewBag.pojistkaId = pohledavkaCreateModel.PojistkaId;
                return View();
            }
        }

        // GET: ZavazkyController/Edit/5
        public ActionResult Edit(int id, string redirectedFrom)
        {
            PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
            Pohledavka pohledavka = pohledavkaModel.GetPohledavkaPojistka(id);
            if (pohledavka == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(new RedirectablePohledavka() { ID = pohledavka.ID, Vznik = pohledavka.Vznik, DatumSplaceni=pohledavka.DatumSplaceni, DatumSplatnosti= pohledavka.DatumSplatnosti, PojistkaId = pohledavka.ID, Popis = pohledavka.Popis, Vyse = pohledavka.Vyse, RedirectedFrom = redirectedFrom });
        }

        // POST: ZavazkyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, RedirectablePohledavka model)
        {
            try
            {
                if (!ModelState.IsValid || model.Vznik > model.DatumSplatnosti)
                {
                    return View(model);
                }
                PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
                Pohledavka original = pohledavkaModel.GetPohledavkaPojistka(id);
                pohledavkaModel.UpdatePohledavka(id, model);

                if (model.RedirectedFrom == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Details", "Pojistka", new { id = original.Pojistka.ID });
                }
            }
            catch
            {

                return View(model);
            }
        }

        // GET: ZavazkyController/Delete/5
        public ActionResult Delete(int id, string redirectedFrom)
        {
            PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
            Pohledavka pohledavka = pohledavkaModel.GetPohledavkaPojistka(id);
            if (pohledavka == null)
            {
                return RedirectToAction("Index");
            }
            return View(new RedirectablePohledavka() { ID = pohledavka.ID, Vznik = pohledavka.Vznik, DatumSplaceni = pohledavka.DatumSplaceni, DatumSplatnosti = pohledavka.DatumSplatnosti, PojistkaId = pohledavka.ID, Popis = pohledavka.Popis, Vyse = pohledavka.Vyse, RedirectedFrom = redirectedFrom });
        }

        // POST: ZavazkyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, RedirectablePohledavka model)
        {
          //  try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                PohledavkaModel pohledavkaModel = new PohledavkaModel(_db);
                Pohledavka original = pohledavkaModel.GetPohledavkaPojistka(id);
                pohledavkaModel.DeletePohledavka(id);

                if (model.RedirectedFrom == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Details", "Pojistka", new { id = original.Pojistka.ID });
                }
            }
          //  catch
            {

                return View(model);
            }
        }
    }
}
