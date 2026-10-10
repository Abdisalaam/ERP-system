using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Models;
using Accountingsystem.ViewModel;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class TransactionBatchController : Controller
    {
        // GET: TransactionBatch
        public ActionResult Index()
            
        {
            var trans = TransactionBatch.GetAllTransactions();
            return View(trans);
        }

        // GET: TransactionBatch/Details/5
        public ActionResult Details(int id)
        {
            var trans = TransactionBatch.GetTransactionById(id);
            
            return View(id);
        }

        // GET: TransactionBatch/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TransactionBatch/Create
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

        // GET: TransactionBatch/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TransactionBatch/Edit/5
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

        // GET: TransactionBatch/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TransactionBatch/Delete/5
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
