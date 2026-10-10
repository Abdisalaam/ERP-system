using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class MatchingController : Controller
    {
        // GET: Matching
        public ActionResult Index(string Msg)
        {
            ViewBag.msg = Msg;
            var Mat = Matching.GetAllMaching();
            return View(Mat);
        }
        public JsonResult CheckCode(string Matching_Code)
        {
            if (Matching.containCode(Matching_Code))
            {
                var msg = "MATCH- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: Matching/Details/5
        public ActionResult Details(int id)
        {
            var model = Matching.GetMatchingById(id);
            return View(model);
        }

        // GET: Matching/Create
        public ActionResult Create()
        {
            var model = Matching.CreatForCreation();
            return View(model);
        }

        public ActionResult GetMatchingInvoiceDetails(int InvoiceDetailsID)
        {
            var model = Matching.GetInvoiceDetailsVM(InvoiceDetailsID);
            return PartialView("GetMatchingInvoiceDetails", model);
        }

        public PartialViewResult GetCusAllInvoiceDetails(int CusId)
        {
            List<CusInvoiceDetailsVM> model = Matching.GetCusAllInvoiceDetailsVM(CusId);
            return PartialView(model);
        }

        public ActionResult GetMatchingReceiptDetails(int receiptDetailsID)
        {
            var model = Matching.GetReceiptDetailsVM(receiptDetailsID);
            return PartialView("GetMatchingReceiptDetails", model);
        }

        // POST: Matching/Create
        [HttpPost]
        public ActionResult Create(MatchingVM MatchVM)
        {
            try
            {
                // TODO: Add insert logic here


               
                Matching.CreateMatching(MatchVM);
                string msg = "matching add successfil";
                return RedirectToAction("Index", new  { msg = msg });
            }
            catch
            {
                return View();
            }
        }

        // GET: Matching/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Matching/Edit/5
        [HttpPost]
        public ActionResult Edit(MatchingVM MatVM)
        {
            try
            {
                // TODO: Add update logic here

                string result = "Success! The GLs are created";
                Matching.UpdateMatching(MatVM);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

        // GET: Matching/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpGet]
        public virtual JsonResult GetResult(int CustomerId)
        {
            var my = Matching.GetCusromerInvoise(CustomerId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetRecipt(int CustomerId)
        {
            var my = Matching.GetCustomerRecipt(CustomerId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }


        // POST: Matching/Delete/5
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