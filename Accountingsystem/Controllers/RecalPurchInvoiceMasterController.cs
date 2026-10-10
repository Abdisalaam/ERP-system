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
    public class RecalPurchInvoiceMasterController : Controller
    {
        public JsonResult CheckCode(string Recall_MPurch_Serial)
        {
            if (RecalPurchInvoiceMaster.containCode(Recall_MPurch_Serial))
            {
                var msg = "RECP- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Checkdate(string Register_Date)
        {
            if (RecalPurchInvoiceMaster.Checkdate(Register_Date) == false)
            {
                var msg = "يجب ادخال تاريخ خلال السنة المالية الحالية";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: RecalPurchInvoiceMaster
        public ActionResult Index(string mesg,int ?all)
        {
            ViewBag.Message = mesg;
            var models = RecalPurchInvoiceMaster.GetAllInvoices(all);
            return View(models);

        }
        // GET: InvoiceMaster/Details/5
        public ActionResult Details(int id)
        {
            var invoiceMaster = RecalPurchInvoiceMaster.GetInvoiceDetailsById(id);
            return View(invoiceMaster);
        }
        public JsonResult DeleteTempDetail(int id)
        {
            var temp = RecalPurchInvoiceMaster.DeleteInvoiceFromTemp(id);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
        // GET: InvoiceMaster/Create
        public ActionResult AddRecallPurchOrder()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء مردود مشتريات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = RecalPurchInvoiceMaster.AddNewForCreation();
            return View(model);

        }

        // POST: InvoiceMaster/Create
        [HttpPost]
        public ActionResult AddRecallPurchOrder(RecalPurchInvoiceMasterVM masterVM)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

           var mesg = RecalPurchInvoiceMaster.AddNewInvoice(masterVM , user.Id);
          
            if (mesg.Item2 == 0)
            {
                return RedirectToAction("Index", new { mesg = mesg.Item1 });
            }
            else
            {
                return RedirectToAction("Details", new { id = mesg.Item2 });
            }
        }


        // POST: InvoiceMaster/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(RecalPurchInvoiceMasterVM invoiceMasterVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد مردود مشتريات");



                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }
                // string result = "Success! Invoice is updated and the GLs are created";
                await Task.Delay(2000);
                string result= RecalPurchInvoiceMaster.UpdateInvoiceMaster(invoiceMasterVM);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                var data = ex.Data;
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف مردود مشتريات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            RecalPurchInvoiceMaster.DeleteRecallPurInvoice(id);
            string mesg = Resources.validate.DeleteSuccessful;
            return RedirectToAction("Index", new { mesg = mesg });
        }

        [HttpGet]
        public JsonResult GetPrice(int Id)
        {

            var productCost = RecalPurchInvoiceMaster.getallprices(Id);

            return Json(productCost, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetQuantity(int Id)
        {

            var productQuantity = RecalPurchInvoiceMaster.GetQuantity(Id);

            return Json(productQuantity, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAllofPurchInv(int invId)
        {
            var my = RecalPurchInvoiceMaster.GetallSupplierPurchInv(invId);

            return Json(my, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllproductsInv(int invId)
        {
            var my = RecalPurchInvoiceMaster.getallProducts(invId);

            return Json(my, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPurchInvoiceJson(int proid)
        {
            var purchaseInvoice = RecalPurchInvoiceMaster.GetPurchInvoice(proid);
            var xx = new
            {
                purchaseInvoice.Supplier_Id,
                purchaseInvoice.Serial_Number,
                purchaseInvoice.Store_ID,
                purchaseInvoice.Master_Discount,
                purchaseInvoice.Company_Id,
                purchaseInvoice.Invoice_Currency,

            };
            return Json(xx, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetQuantitystore(int proid, int purchid, int storid)
        {

            var productQuantity = RecalPurchInvoiceMaster.GetStoreQuantity(proid, purchid, storid);

            return Json(productQuantity, JsonRequestBehavior.AllowGet);

        }
        //////////////////
        public ActionResult GetAllFiles(int recallMasterId)
        {
            var model = RecalPurchInvoiceMaster.GetAllFiles(recallMasterId);
            return View(model);
        }
        public FileResult ReturnrecalltFile(int fileId)
        {

            var document = RecalPurchInvoiceMaster.getFileById(fileId);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = document.FileName,
                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                CreationDate = DateTime.Now.Date,
                ModificationDate=DateTime.Now.Date,
                ReadDate=DateTime.Now.Date,
            
                Size=document.ContentLenght,
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.ProImg,"txt");
        }

        [HttpGet]
        public ActionResult PreparerecallDetails(int serial, string list)
        {
             var invoicrecall = RecalPurchInvoiceMaster.PreparerecallMasterDetail(serial, list);
             return PartialView("RecallDetailPartial", invoicrecall);
        }
   
        [HttpPost]
        public virtual JsonResult AddInvoiceDetailsToMaster(RecalPurchInvoiceDetailsVM invoiceMasterDetail)
        {
            string msg = "";
            try
            {
                msg = RecalPurchInvoiceMaster.AddRecallDetailsTemp(invoiceMasterDetail);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }


       
            return Json(msg, JsonRequestBehavior.AllowGet);
           
        }    
        [HttpGet]
        public ActionResult GetAllTempInvoiceDetails(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = RecalPurchInvoiceMaster.GetAllTemprecallInvoiceDetails(tempInvoiceMasterId);
            return PartialView("RecallMasterDetailList", invoiceDetailVm);
        }
        [HttpGet]
        public ActionResult GetAllTempInvoiceDetailsresult(int tempInvoiceMasterId, int delete)
        {
            var invoiceDetailVm = RecalPurchInvoiceMaster.GetAllTemprecallInvoiceDetailsresult(tempInvoiceMasterId, delete);
            return PartialView("RecallMasterDetailResultList", invoiceDetailVm);
        }
    }
}
