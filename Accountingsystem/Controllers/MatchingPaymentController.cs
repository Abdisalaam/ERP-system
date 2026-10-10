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
    public class MatchingPaymentController : Controller
    {
        // GET: MatchingPayment
        public ActionResult Index(string Msg)
        {
            ViewBag.msg = Msg;
            var Mat = MatchingPayment.GetAllMaching();
            return View(Mat);
        }

        // GET: MatchingPayment/Details/5
        public ActionResult Details(int id)
        {
            var model = MatchingPayment.GetMatchingById(id);
            return View(model);
        }

        // GET: MatchingPayment/Create
        public ActionResult Create()
        {
            try
            {
                var model = MatchingPayment.CreatForCreation();                
               
               // return RedirectToAction("DisplayData", new { msg = result, data = data });

                return View(model);
            }
            catch (Exception ex)
            {
                var result = ex.Message;
                var data = ex.Data;
                return RedirectToAction("DisplayData", new { msg = result, data });
            }
        }
        public ActionResult DisplayData(string msg, string data)
        {

            //I have to fix this: update state listItem after selecting another country
            ViewBag.message = msg;
            ViewBag.data = data;
            return View();
        }

        // POST: MatchingPayment/Create
        [HttpPost]
        public ActionResult Create(MatchingPaymentVM MatchPayVM)
        {
            try
            {
             //TODO: Add insert logic here
          
            MatchingPayment.CreateMatchingPayment(MatchPayVM);
            string Msg = "Success! Order is Complete";
                return RedirectToAction("Index", new { Msg = Msg });
            }
            catch
            {
                return View(MatchPayVM);
            }
        }

        // GET: MatchingPayment/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MatchingPayment/Edit/5
        [HttpPost]
        public ActionResult Edit(MatchingPaymentVM MatVM)
        {
            try
            {
                // TODO: Add update logic here


                string result = "Success! Transfer is updated and the GLs are created";
                MatchingPayment.UpdateMatching(MatVM);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

        // GET: MatchingPayment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpGet]
        public virtual JsonResult GetResult(int SuppId)
        {
            var my = MatchingPayment.GetPayment(SuppId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetInvoice(int SuppId)
        {
            var my = MatchingPayment.GetPayInvoiceMaster(SuppId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }


        // POST: MatchingPayment/Delete/5
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


        public ActionResult GetMatchingPurInvoiceDetails(int InvoiceDetailsID)
        {
            var model = MatchingPayment.GetPurInvoiceDetailsVM(InvoiceDetailsID);
            return PartialView("GetMatchingPurInvoiceDetails", model);
        }

        public ActionResult GetSupAllPurInvoiceDetails(int supId)
        {
            var model = MatchingPayment.GetSupAllPurInvoiceDetailsVM(supId);
            return PartialView("SupAllInvoicesPartial", model);
        }
        public ActionResult GetMatchingPaymentDetails(int paymentId)
        {
            var model = MatchingPayment.GetPaymentsDetailsVM(paymentId);
            return PartialView("GetMatchingPaymentDetails", model);
        }
        public JsonResult CheckCode(string Match_Code)
        {
            if (MatchingPayment.containCode(Match_Code))
            {
                var msg = "MATPAY- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}