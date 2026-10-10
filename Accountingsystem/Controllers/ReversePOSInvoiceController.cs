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
    public class ReversePOSInvoiceController : Controller
    {
        // GET: ReversePOSInvoice
        public ActionResult Index(String Message,int ?all)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var list = ReversePOSInvoice.getAllDataForReverseInvoice(user.Id,all);
                ViewBag.message = Message;
                return View(list);
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
        public JsonResult Checkdate(string Invoice_Date)
        {
            if (ReversePOSInvoice.Checkdate(Invoice_Date) == false)
            {
                var Message = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.dateTrue;
                return Json(Message, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: ReversePOSInvoice/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var Invoice = ReversePOSInvoice.getinvoiceByid(id);
                return View(Invoice);
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
        public JsonResult GetSalesInvoiceJson(int Invoice_Id)
        {
            var db = new AccountingContainer();
            var salsInvoice = ReversePOSInvoice.GetDataForReverseInvoice(Invoice_Id);


            var xx = new
            {
                salsInvoice.Customer_Id,
                salsInvoice.Serial_Number,
                salsInvoice.PointOfSalePOS_Id,
                salsInvoice.Company_Id,
                
                salsInvoice.Master_Discount,
                details = salsInvoice.DetailPOSInvoices.Select(detail => new { detail.Detail_Id, detail.Product_Id, detail.Product_Quantity, detail.Product_Price, detail.Product_Tax }).ToList()
            };
            return Json(xx, JsonRequestBehavior.AllowGet);
        }
        // GET: ReversePOSInvoice/Create
        public ActionResult Create(string Message)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء مردود فاتورة نقطة مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            Message = Resources.MasterPOSInvoice.donothavePOS;
            var POSVM = ReversePOSInvoice.AddNewForCreation(user.Id);
            if (POSVM != null)
            {
                return View(POSVM);
            }
            else
            {
                return RedirectToAction("Index", new { Message = Message });
            }
            
        }

        // POST: ReversePOSInvoice/Create
        [HttpPost]
        public ActionResult Create(ReversePOSInvoiceVM RevVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var id = ReversePOSInvoice.AddNewRecallsalesInvoice(RevVM , user.Id);

                return RedirectToAction("Details", new {id=id });
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

        
      
        // GET: ReversePOSInvoice/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReversePOSInvoice/Delete/5
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
        public virtual JsonResult AddInvoiceDetailsToMaster(ReversePOSDetailVM invDetail)
        {
            string Message = "";
            try
            {
                Message = ReversePOSInvoice.AddReversSalesDetailsTemp(invDetail);

            }
            catch (Exception ex)
            {
                Message = ex.Message;
                throw;
            }



            return Json(Message, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetDetailOfInvoiceForReversing(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = ReversePOSInvoice.getDetailOfInvoice(tempInvoiceMasterId);
            return PartialView( invoiceDetailVm);
        }
        [HttpGet]
        public ActionResult GetAllTempOfReverseInvoice(int tempInvoiceMasterId, int delete)
        {
            var invoiceDetailVm = ReversePOSInvoice.GetTempOfInvoice(tempInvoiceMasterId,delete);
            return PartialView(invoiceDetailVm);
        }
        public ActionResult PrepareReverseDetails(int Detail_id)
        {
            var invoicrecall = ReversePOSInvoice.PrepareDetailForMakingReverse(Detail_id);
            return PartialView( invoicrecall);

        }
        public ActionResult DeleteITem(int ids)
        {

            var result = ReversePOSInvoice.deleteItemFomDetails(ids);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public async System.Threading.Tasks.Task<ActionResult> Edit(ReversePOSInvoiceVM  salesinvoiceMasterVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "استبعاد مردود فاتورة نقطة مبيعات");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                await Task.Delay(2000);
                string result = ReversePOSInvoice.UpdateInvoiceMaster(salesinvoiceMasterVM);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                var erorr = Message;
                var data = ex.Data;
                return View();

            }
        }
    }
}
