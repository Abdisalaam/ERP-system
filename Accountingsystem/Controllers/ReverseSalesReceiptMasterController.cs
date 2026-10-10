using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class ReverseSalesReceiptMasterController : Controller
    {
        public JsonResult CheckCode(string Serial_Number)
        {
            if (ReverseSalesReceiptMaster.containCode(Serial_Number))
            {
                var msg = "هذا الكود موجود بالنظام";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Checkdate(string Invoice_Date)
        {
            if (InvoiceMaster.Checkdate(Invoice_Date) == false)
            {
               var msg= Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.dateTrue;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index(String msg,int ?all)
        {
            var models = ReverseSalesReceiptMaster.GetAllRevInvoices(all);
            ViewBag.Message = msg;
            return View(models);
        }

        // GET: ReverseSalesReceiptMaster/Details/5
        public ActionResult Details(int id)
        {
            var invoiceMaster = ReverseSalesReceiptMaster.GetrevInvoiceDetailsById(id);
            return View(invoiceMaster);
        }

        // GET: ReverseSalesReceiptMaster/Create
        public ActionResult AddNewRevInvoiceDetails()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء مردود نقدي");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            var model = ReverseSalesReceiptMaster.AddNewForCreation();
            return View(model);

        }

        // POST: ReverseSalesReceiptMaster/Create
        [HttpPost]
        public ActionResult AddNewRevInvoiceDetails(ReverseSalesReceiptMasterVM masterVM)
        {

            //  string result = "Success! Order is Complete";
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var  result = ReverseSalesReceiptMaster.AddNewRecallsalesInvoice(masterVM , user.Id);
            if (result.Item2 == 0)
            {
                return RedirectToAction("Index", new { msg = result.Item1 });
            }
            else
            {
                return RedirectToAction("Details", new { id = result.Item2 });
            }
           ;
            //return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetSalesInvoiceJson(int proid)
        {
            var db = new AccountingContainer();
            var salsInvoice = ReverseSalesReceiptMaster.GetrevsalesInvoice(proid);


            var xx = new
            {
                salsInvoice.Customer_Id,
                salsInvoice.Serial_Number,
                salsInvoice.Store_ID,
                salsInvoice.Company_Id,
                salsInvoice.Invoice_Currency,
                salsInvoice.PaymentMethod_Id,
                details = salsInvoice.SalesReceiptDetails.Select(detail => new { detail.Details_Id, detail.Product_Id, detail.Product_Quantity, detail.Product_Price, detail.Product_Tax }).ToList()
            };
            return Json(xx, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllFiles(int recallMasterId)
        {
            var model = ReverseSalesReceiptMaster.GetAllFiles(recallMasterId);
            return View(model);
        }
        public ActionResult deleteFile(int fileId)
        {

            var document = ReverseSalesReceiptMaster.getFileById(fileId);
            var delete = ReverseSalesReceiptMaster.deleteFile(fileId);
            return RedirectToAction("Details", new { id = document.ReverseSalesReceiptMasterMaster_Id });
        }

        public FileResult ReturnrecalltFile(int fileId)
        {

            var document = ReverseSalesReceiptMaster.getFileById(fileId);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = document.FileName,
                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.FileContent, document.FileType);
        }
        public virtual JsonResult AddInvoiceDetailsToMaster(ReverseSalesReceiptDetailsVM invDetail)
        {
            string msg = "";
            try
            {
                msg = ReverseSalesReceiptMaster.AddReversSalesDetailsTemp(invDetail);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }



            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public ActionResult DeleteITem(int ids)
        {

            var result = ReverseSalesReceiptMaster.deleteItemFomDetails(ids);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        [HttpGet]
        public ActionResult GetAllTempSalesInvoiceDetails(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = ReverseSalesReceiptMaster.GetAllTempReverseSalesInvoiceDetails(tempInvoiceMasterId);
            return PartialView("ReversMasterDetailList", invoiceDetailVm);
        }
        [HttpGet]
        public ActionResult GetAllTempSalesInvoiceDetailsresult(int tempInvoiceMasterId,int delete)
        {
            var invoiceDetailVm = ReverseSalesReceiptMaster.GetAllTempReverseSalesInvoiceDetailsresult(tempInvoiceMasterId,delete);
            return PartialView("ReversMasterDetailResultList", invoiceDetailVm);
        }
        public ActionResult PrepareReverseDetails(int serial, string tempRecallPurchMasterID)
        {
            var invoicrecall = ReverseSalesReceiptMaster.PrepareReverseMasterDetail(serial, tempRecallPurchMasterID);
            return PartialView("ReversSalesDetailPartial", invoicrecall);

        }
        public async System.Threading.Tasks.Task<ActionResult> Edit(ReverseSalesReceiptMasterVM salesinvoiceMasterVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد مردود نقدي");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                await Task.Delay(2000);
                string result = ReverseSalesReceiptMaster.UpdateInvoiceMaster(salesinvoiceMasterVM);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var erorr = msg;
                var data = ex.Data;
                return View();

            }
        }
        public ActionResult Delete(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف مردود نقدي");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = ReverseSalesReceiptMaster.DeleteInvoiceMaster(id);
            return RedirectToAction("Index", new { msg = msg });
        }


    }
}
