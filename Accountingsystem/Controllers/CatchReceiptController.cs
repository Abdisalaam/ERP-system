using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.ViewModel;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class CatchReceiptController : Controller
    {
        // GET: CatchReceipt
        public ActionResult Index(string msg,int ?all)
        {
            var models = CatchReceipt.GetAllCatchREceipt();
            ViewBag.Message = msg;
            if (all == null)
            {
                models.Reverse();
                models = models.Take(100).ToList();
            }
            return View(models);
        }
        public JsonResult Checkdate(string CatchReceipt_date)
        {
            if (CatchReceipt.Checkdate(CatchReceipt_date) == false)
            {
                var msg = "يجب ادخال تاريخ خلال السنة المالية الحالية";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: CatchReceipt/Details/5
        public ActionResult Details(int id)
        {
            var thiscatchrecipt = CatchReceipt.GetCatchReceiptById(id);
            return View(thiscatchrecipt);
        }

        // GET: CatchReceipt/Create
        public ActionResult Create(int child,string type)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء سند قبض");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = CatchReceipt.AddForCreation(child, type);
            return View(model);
         
        }

        // POST: CatchReceipt/Create
        [HttpPost]
        public ActionResult Create(CatchReceiptVM CVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var result = CatchReceipt.AddCatchRecipt(CVM , user.Id);

                return RedirectToAction("Details",new {id=result });
            }
            catch
            {
                return View(CVM);
            }
        }

        // GET: CatchReceipt/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل سند قبض");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = CatchReceipt.EditForCreation(id);
          
            return View(model);
        }

        // POST: CatchReceipt/Edit/5
        [HttpPost]
        public ActionResult Edit(CatchReceiptVM cvm)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var result = CatchReceipt.EditCatchRecipt(cvm , user.Id);
                return RedirectToAction("Details", new { id = cvm.CatchReceipt_Id });
            }
            catch
            {
                return View();
            }
        }
    
      [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Update(CatchReceiptVM CVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد سند قبض");



                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }
                await Task.Delay(2000);
                string result = CatchReceipt.UpdateInvoiceMaster(CVM);
               return Json(result, JsonRequestBehavior.AllowGet);              

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var data = ex.Data;
                return View();
            }
        }

        // GET: CatchReceipt/Delete/5
       
        [HttpGet]
        public virtual JsonResult GetChildCredit(int CreditID)
        {
            var my = CatchReceipt.getCreditAccountCHild(CreditID);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public virtual JsonResult GetSubChildCredit(int ChildID)
        {
            var my = GLMaster.GetAccountLedger(ChildID);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        public FileResult ReturnsupFile(int fileId)
        {

            var document = CatchReceipt.getFileById(fileId);
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
        public ActionResult Delete(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف سند قبض");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = CatchReceipt.DeleteCatch(id);
            return RedirectToAction("Index", new { msg = msg });
        }
        public ActionResult DeleteActive(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف سند قبض معتمد");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            try
            {
                var msg = CatchReceipt.deletionactiveGL(id);
                return RedirectToAction("Index", new { msg = msg });
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

                return RedirectToAction("Index", new { msg = ViewBag.Message });
            }

        }

    }
}
