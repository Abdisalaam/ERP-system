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
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult Index(string Message,int ?all)
        {
            ViewBag.Message = Message;
            var payment = Payment.GetAllPayment(all);
            return View(payment);
        }
        public JsonResult Checkdate(string Pay_Date)
        {
            if (Payment.Checkdate(Pay_Date) == false)
            {
                var Message = "يجب ادخال تاريخ خلال السنة المالية الحالية";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: Payment/Details/5
        public ActionResult Details(int id)
        {
            var pay = Payment.GetPaymentById(id);
            return View(pay);
        }

        // GET: Payment/Create
        public ActionResult Create(int? suppId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء سند صرف مورد");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            var paym = Payment.AddPaymentVM(suppId);
            return View(paym);
        }

        // POST: Payment/Create
        [HttpPost]
        public ActionResult Create(PaymentVM PaymentVM)
        {
            try
            {
                // TODO: Add insert logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var NewPayment = Payment.AddNewPayment(PaymentVM , user.Id);
                string Message = NewPayment.Item1;
                if (NewPayment.Item2==0)
                {
                    return RedirectToAction("Index", new { Message = Message });
                }
                else
                {
                    return RedirectToAction("Details", new { id = NewPayment.Item2 });
                }
            }
            catch
            {
                return View(PaymentVM);
            }
        }

        [HttpGet]
        public virtual JsonResult GetInvoice(int SuppId)
        {
            var my = Payment.GetPayInvoiceMaster(SuppId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMatchingPurInvoiceDetails(int InvoiceDetailsID)
        {
            var model = Payment.GetPurInvoiceDetailsVM(InvoiceDetailsID);
            return PartialView("GetPurInvoiceDetails", model);
        }
        public ActionResult GetMatchingRecalls(int InvoiceDetailsID)
        {
            var model = Payment.getAllRecalls(InvoiceDetailsID);
            if (model == null)
            {
                return null;
            }
            else
            {
                return PartialView("PurchasRecallView", model);
            }
        }
        // GET: Payment/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل سند صرف مورد");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            if (id < 1)
            {
                string Message = "error id is empty";
                return RedirectToAction("Index", new { Message = Message });
            }
            var pay = Payment.FillViewModelForPayment(id);
            return View(pay);
        }

        // POST: Payment/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(PaymentVM ccVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, " اعتماد سند صرف مورد");



                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }
                // TODO: Add update logic here
                await Task.Delay(2000);
                string result = Payment.UpdatePayment(ccVM);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                var Message = ex.Message;
                return View();
            }
        }

        public ActionResult Update(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل سند صرف مورد");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var pay = Payment.FilltheEditPayment(id);
            return View(pay);
        }

        // POST: Payment/Edit/5
        [HttpPost]
        public ActionResult Update(int id , PaymentVM paymVM)
        {
            try
            {
                // TODO: Add update logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
               
                var payment = Payment.EditPayment(paymVM , user.Id);
              
                    return RedirectToAction("Details", new { id = payment });
               
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                return View();
            }
        }
        // GET: Payment/Delete/5
        //public ActionResult Delete(int id)
        //{

        //    var payment = Payment.GetPaymentById(id);
        //    return View(payment);
        //}

        // POST: Payment/Delete/5
     
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف سند صرف مورد");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var payment = Payment.Deletepayment(id);
                string Message = Resources.validate.DeleteSuccessful;
                return RedirectToAction("Index", new { Message = Message });


            }
            catch
            {
                return View();
            }
        }


        public FileResult ReturnPayFile(int fileId)
        {

            var document = Payment.FindFileByID(fileId);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = document.FileName,
                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.ProImg, "txt");
        }
        public ActionResult deleteFile(int fileId)
        {

            var document = Payment.FindFileByID(fileId);
            var delete = Payment.deleteFile(fileId);
            return RedirectToAction("Details", new { id = document.PaymentPay_ID });
        }
        public ActionResult GetAllFiles(int PayId)
        {
            var model = Payment.GetAllFiles(PayId);
            return View(model);
        }

        [HttpGet]
        public ActionResult PartialDialog()
        {
            var Suplliertvm = Supplier.AddSupplierVM();
            return PartialView("CustomerPartialView", Suplliertvm);
        }


        public ActionResult AddNewSupplierPartial(SupplierVM SupVM)
        {

            var result = Supplier.AddNewSupplier(SupVM);
            //ViewBag.res = result;
            //return PartialView("CustomerPartialView", ViewBag.res);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        [HttpGet]
        public ActionResult GetSupplierDropdownList()
        {
            var supplier = Payment.GetSupplierList();
            return PartialView("SupplierDropDownList", supplier);
        }
        [HttpGet]
        public virtual JsonResult GetPurchaseInvoices(int supID)
        {
            var my = Payment.GetPurchaseInvoice(supID);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteActive(int id)
        {

            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف سند صرف مورد معتمد");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var msg = Payment.deletionactiveGL(id);
                return RedirectToAction("Index", new { Message = msg });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
                if (cookie != null && cookie.Value != null)
                {
                    if (cookie.Value == "en")
                    {
                        ViewBag.Message = ex.Message + "___" + "Please review this error and try to correct it";
                    }
                    else
                    {
                        ViewBag.Message = ex.Message + "__" + "هناك خطأ في البيانات. الرجاء التاكد من صحتها";
                    }
                }
                else
                {

                    ViewBag.Message = ex.Message + "__" + "هناك خطأ في البيانات. الرجاء التاكد من صحتها";
                }

                return RedirectToAction("Index", new { Message = ViewBag.Message });
            }

        }
    }
}