using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class BussinesUnitController : Controller
    {
        // GET: BussinesUnit
        public ActionResult Index()
        {
            var bu = BussinesUnit.getAllData();
            return View(bu);
        }

        // GET: BussinesUnit/Details/5
        public ActionResult Details(int id)
        {
            var bu = BussinesUnit.getDetail(id);
            return View(bu);
        }

        // GET: BussinesUnit/Create
        public ActionResult Create()
        {
            var bu = BussinesUnit.fillVMForCreate();
            return View(bu);
        }

        // POST: BussinesUnit/Create
        [HttpPost]
        public ActionResult Create(BussinesUnitVM buvm)
        {
            try
            {
                var bu = BussinesUnit.createBussinesUnit(buvm);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: BussinesUnit/Edit/5
        public ActionResult Edit(int id)
        {
            var bu = BussinesUnit.fillDataForEditing(id);
            return View(bu);
        }

        // POST: BussinesUnit/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, BussinesUnitVM buvm)
        {
            try
            {
                var bu = BussinesUnit.editTheData(buvm);
                return RedirectToAction("Index");


            }
            catch
            {
                return View();
            }
        }

        // GET: BussinesUnit/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BussinesUnit/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}