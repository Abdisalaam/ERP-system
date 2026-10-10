using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class ProductBatchesController : Controller
    {
        // GET: ProductBatches
        public ActionResult Index()
        {
            var product = ProductBatches.getAllProduct();
            return View(product);
        }

        // GET: ProductBatches/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductBatches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductBatches/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductBatches/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductBatches/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductBatches/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductBatches/Delete/5
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
