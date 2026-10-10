using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class ReceiptController : Controller
    {
        public JsonResult CheckCode(string Rec_Number)
        {
            if (Receipt.containCode(Rec_Number))
            {
                var Message = "REC- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Checkdate(string Rec_Date)
        {
            if (Receipt.Checkdate(Rec_Date) == false)
            {
                var Message = "يجب ادخال تاريخ خلال السنة المالية الحالية";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getCustomerBalance(int ID)
        {
            decimal balance=Receipt.BalanceOFCustomer(ID);
            return Json(balance, JsonRequestBehavior.AllowGet);
        }
        // GET: Receipt
        public ActionResult Index(string Message,int ?all)
        {
            try
            {
                ViewBag.Message = Message;
                var receipts = Receipt.GetAllReceipt(all);
                return View(receipts);
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


                return View();
            }
        }

        // GET: Receipt/Details/5
        public ActionResult Details(int id,string Message)
        {
            try
            {
                var receipt = Receipt.FindReceiptByID(id);
                ViewBag.Message = Message;
                return View(receipt);
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


                return View();
            }
        }

        // GET: Receipt/Create
        public ActionResult Create(int? cust_ID,int ? invoice_Id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء سند قبض مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            var receipt = Receipt.CreateVMForCreation(cust_ID, invoice_Id);
            return View(receipt);
        }

        // POST: Receipt/Create
        [HttpPost]
        public ActionResult Create(ReceiptVM bnm)
        {
            try
            {
                // TODO: Add insert logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var NewRecipt = Receipt.AddNewReceipt(bnm , user.Id);
                var Message = "";
                if (NewRecipt == 0)
                {
                    Message = "لم يتم قبول السند";
                    return RedirectToAction("Index", new { Message = Message });
                }
                else
                { Message = "تم اضافة السند بنجاح"; }
               
                return RedirectToAction("Details", new { id = NewRecipt });
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


                return View();
            }
        }

        // GET: Receipt/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل سند قبض مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            if (id < 1)
            {
                string Message = Resources.ReceiptCreate.FailureMessage;
                return RedirectToAction("Index", new { Message = Message });
            }
            var receipt = Receipt.FillReseipt(id);
            return View(receipt);
        }

        // POST: Receipt/Edit/5
        [HttpPost]
        public async Task< ActionResult> Edit(ReceiptVM bnv)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد سند قبض مبيعات");



                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }
                // TODO: Add update logic here

                await Task.Delay(2000);
                string result = Receipt.UpdateRecipt(bnv);

                return Json(result, JsonRequestBehavior.AllowGet);
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


                return View();
            }
        }


        public ActionResult Delete(int id)
        {
          
            try
            {
                // TODO: Add delete logic here

                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف سند قبض مبيعات");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                Receipt.DeleteReceipt(id);
                string mesg = Resources.ReceiptCreate.delete_Success;
                return RedirectToAction("Index" , new { Message = mesg});
                         
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


                return View();
            }
        }

        public ActionResult Update(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل سند قبض مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var rece = Receipt.FillEditTheReceipt(id);
            return View(rece);
        }

        [HttpPost]
        public ActionResult Update(int id , ReceiptVM receVM)
        {
            try
            {
                // TODO: Add update logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var rec = Receipt.EditReceipt(receVM , user.Id);
                return RedirectToAction("Details", new { id = rec });
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


                return View();
            }
        }
        public ActionResult deleteFile(int fileId)
        {

            var document = Receipt.FindFileByID(fileId);
            var delete = Receipt.deleteFile(fileId);
            return RedirectToAction("Details", new { id = document.ReceiptRec_ID });
        }

        public FileResult ReturnRecFile(int fileId)
        {

            var document = Receipt.FindFileByID(fileId);
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
        public ActionResult GetAllFiles(int RecId)
        {
            var model = Receipt.GetAllFiles(RecId);
            return View(model);
        }

        [HttpGet]
        public ActionResult PartialDialog()
        {
            var custvm = Customer.AddCustomerVM();
            return PartialView("CustomerPartialView", custvm);
        }



        public ActionResult AddNewCustomerPartial(CustomerVM masterVM)
        {

            var result = Customer.AddNewCustomer(masterVM);
            //ViewBag.res = result;
            //return PartialView("CustomerPartialView", ViewBag.res);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        [HttpGet]
        public ActionResult GetCustomerDropdownList()
        {
            var customer = Receipt.GetCustomerList();
            return PartialView("CustomerDropDownList", customer);
        }
        [HttpGet]
        public virtual JsonResult GetInvoice(int CustID)
        {
            var my = Receipt.GetPayInvoiceMaster(CustID);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInvoiceDetails(int InvoiceDetailsID)
        {
            var model = Receipt.GetInvoiceDetailsVM(InvoiceDetailsID);
            return PartialView("GetInvoiceDetails", model);
        }
        public ActionResult GetinvoiceRecalls(int InvoiceDetailsID)
        {
            var model = Receipt.getAllRecallOfInvoiceSales(InvoiceDetailsID);
            if (model == null)
            {
                return null;
            }
            else
            {
                return PartialView("GetinvoiceRecalls", model);
            }
         
        }
        public ActionResult DeleteActive(int id)
        {

            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف سند قبض مبيعات معتمد ");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var msg = Receipt.deletionactiveGL(id);
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
