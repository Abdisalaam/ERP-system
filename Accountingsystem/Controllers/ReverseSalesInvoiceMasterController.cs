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
    public class ReverseSalesInvoiceMasterController : Controller
    {
        public JsonResult CheckCode(string Serial_Number)
        {
            if (ReverseSalesInvoiceMaster.containCode(Serial_Number))
            {
                var msg = "REVS- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Checkdate(string Invoice_Date)
        {
            if (InvoiceMaster.Checkdate(Invoice_Date) == false)
            {
                var msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.dateTrue;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: InvoiceMaster
        public ActionResult Index(string msg,int ?all)
        {
            var models = ReverseSalesInvoiceMaster.GetAllRevInvoices(all);
            
            ViewBag.Message = msg;
            return View(models);

        }
        // GET: InvoiceMaster/Details/5
        public ActionResult Details(int id)
        {
            var invoiceMaster = ReverseSalesInvoiceMaster.GetrevInvoiceDetailsById(id);
            return View(invoiceMaster);
        }
        // GET: InvoiceMaster/Create
        public ActionResult AddNewRevInvoiceDetail()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء مردود اجل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReverseSalesInvoiceMaster.AddNewForCreation();
            return View(model);

        }
        [HttpPost]
        public ActionResult AddNewRevInvoiceDetail(ReverseSalesInvoiceVM masterVM)
        {

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        

            var result = ReverseSalesInvoiceMaster.AddNewRecallsalesInvoice(masterVM , user.Id);
         
            if (result.Item2 == 0)
            {
                return RedirectToAction("Index", new { msg = result.Item1 });
            }
            else
            {
                return RedirectToAction("Details", new { id = result.Item2 });
            }
           

        }
        public ActionResult DeleteITem(int ids)
        {

            var result = ReverseSalesInvoiceMaster.deleteItemFomDetails(ids);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        // POST: InvoiceMaster/Create

        //public JsonResult AddNewRevInvoiceDetails(ReverseSalesInvoiceVM masterVM)
        //{

        //    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

        //    string result = ReverseSalesInvoiceMaster.AddNewRecallsalesInvoice(masterVM , user.Id);
        //    return Json(result, JsonRequestBehavior.AllowGet);

        //}
        // POST: InvoiceMaster/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(ReverseSalesInvoiceVM invoiceMasterVM)
        {

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد مردود اجل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            //try
            //{
            await Task.Delay(2000);
            string result = ReverseSalesInvoiceMaster.UpdateInvoiceMaster(invoiceMasterVM);

                return Json(result, JsonRequestBehavior.AllowGet);
            //}
            //catch
            //{
            //    return View();
            //}
        }
        public ActionResult Delete(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف مردود اجل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg=  ReverseSalesInvoiceMaster.DeleteInvoiceMaster(id);
            return RedirectToAction("Index",new {msg= msg });
        }
        public JsonResult GetSalesInvoiceJson(int proid)
        {
            var db = new AccountingContainer();
            var salsInvoice = ReverseSalesInvoiceMaster.GetrevsalesInvoice(proid);
         
           
            var xx = new
            {
                salsInvoice.Customer_Id,
                salsInvoice.Serial_Number,
                salsInvoice.Store_ID,
                salsInvoice.Company_Id,
                salsInvoice.Invoice_Currency,
                salsInvoice.Master_Discount,
                details = salsInvoice.InvoiceDetails.Select(detail => new { detail.Details_Id, detail.Product_ID, detail.Product_Quantity, detail.Product_Price, detail.Product_Tax }).ToList()
            };
            return Json(xx, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllFiles(int recallMasterId)
        {
            var model = InvoiceMaster.GetAllFiles(recallMasterId);
            return View(model);
        }
        public ActionResult deleteFile(int fileId)
        {

            var document = ReverseSalesInvoiceMaster.getFileById(fileId);
            var delete = ReverseSalesInvoiceMaster.deleteFile(fileId);
            return RedirectToAction("Details", new { id = document.ReverseSalesInvoiceMaster_Id });
        }

        public FileResult ReturnrecalltFile(int fileId)
        {

            var document = ReverseSalesInvoiceMaster.getFileById(fileId);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = document.FileName,
                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.FileContent, "txt");
        }
        [HttpGet]
        public ActionResult PrepareReverseDetails(int serial, string tempRecallPurchMasterID)
        {
            var invoicrecall = ReverseSalesInvoiceMaster.PrepareReverseMasterDetail(serial, tempRecallPurchMasterID);
            return PartialView("ReversSalesDetailPartial", invoicrecall);

        }
        [HttpPost]
        public virtual JsonResult AddInvoiceDetailsToMaster(ReveInvoiceDetailsVM invDetail)
        {
            string msg = "";
            try
            {
                msg = ReverseSalesInvoiceMaster.AddReversSalesDetailsTemp(invDetail);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw; 
            }



            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult GetAllTempSalesInvoiceDetails(int tempInvoiceMasterId,int Rev_Id)
        {
            var invoiceDetailVm = ReverseSalesInvoiceMaster.GetAllTempReverseSalesInvoiceDetails(tempInvoiceMasterId, Rev_Id);
            return PartialView("ReversMasterDetailList", invoiceDetailVm);
        }
        [HttpGet]
        public ActionResult GetAllTempSalesInvoiceDetailsresult(int tempInvoiceMasterId,int delete)
        {
            var invoiceDetailVm = ReverseSalesInvoiceMaster.GetAllTempReverseSalesInvoiceDetailsresult(tempInvoiceMasterId,delete);
            return PartialView("ReversMasterDetailResultList", invoiceDetailVm);
        }
    }
}