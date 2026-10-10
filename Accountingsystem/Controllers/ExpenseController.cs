using Accountingsystem.Models;
using Accountingsystem.Utilites;
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
    public class ExpenseController : Controller
    {

        public JsonResult CheckCode(string Expense_Code)
        {
            if (Expense.containCode(Expense_Code))
            {
                var msg = "EXP- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Checkdate(string Expense_date)
        {
            if (Expense.Checkdate(Expense_date) == false)
            {
                var msg = "يجب ادخال تاريخ خلال السنة المالية الحالية";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: Expense
        public ActionResult Index(string msg,int ?all)
        {
            try
            {
                var exp = Expense.getAllData(all);
                ViewBag.Message = msg;
                return View(exp);

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(ViewBag.Message);
            }
          
        }

        // GET: Expense/Details/5
        public ActionResult Details(int id)
        {
            try
            {               
                var exp = Expense.getDetail(id);
                if (exp != null)
                {
                    return View(exp);
                }
                else
                {
                    HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
                    if (cookie != null && cookie.Value != null)
                    {
                        if (cookie.Value == "en")
                        {
                            ViewBag.Message = "It couldnt find the expense with the id number: " + id;
                        }
                        else
                        {
                            ViewBag.Message = "تعذر العثور على المصروف بالرقم : " +  id ;
                        }
                    }
                    else
                    {

                        ViewBag.Message = "تعذر العثور على المصروف بالرقم : " + id;
                    }
                   
                    return RedirectToAction("Index", new { msg = ViewBag.Message });
                }
               

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;


                return RedirectToAction("Index", new { msg = ViewBag.Message });
            }
           
        }


        public ActionResult DetailsNew(int id)
        {
            try
            {
                var exp = Expense.getDetail(id);
                if (exp != null)
                {
                    return View(exp);
                }
                else
                {
                    HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
                    if (cookie != null && cookie.Value != null)
                    {
                        if (cookie.Value == "en")
                        {
                            ViewBag.Message = "It couldnt find the expense with the id number: " + id;
                        }
                        else
                        {
                            ViewBag.Message = "تعذر العثور على المصروف بالرقم : " + id;
                        }
                    }
                    else
                    {

                        ViewBag.Message = "تعذر العثور على المصروف بالرقم : " + id;
                    }

                    return RedirectToAction("Index", new { msg = ViewBag.Message });
                }


            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;


                return RedirectToAction("Index", new { msg = ViewBag.Message });
            }

        }
        // GET: Expense/Edit/5
        public ActionResult Edit(int id)
        {

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل مصروف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var exp = Expense.fillDataForEditing(id);
            return View(exp);

        }

        // POST: Expense/Edit/5
        [HttpPost]
        public ActionResult Edit(ExpenseVM expvm)
        {
            try
            {
                var exp = Expense.DataForEditing(expvm);
                if (exp == null)
                {
                    return RedirectToAction("Edit", new { id = expvm.Expense_Id });
                }
                return RedirectToAction("Details", new { id = exp.Expense_Id });
            }
            catch(Exception ex)
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
                var exp = Expense.fillDataForEditing(expvm.Expense_Id);
                return View();
            }

        }


        public ActionResult EditNew(int id)
        {

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل مصروف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var exp = Expense.fillDataForEditingNew(id);
            return View(exp);

        }

        // POST: Expense/Edit/5
        [HttpPost]
        public ActionResult EditNew(ExpenseVM expvm)
        {
            try
            {
                var exp = Expense.DataForEditingNew(expvm);
                if (exp == null)
                {
                    return RedirectToAction("EditNew", new { id = expvm.Expense_Id });
                }
                return RedirectToAction("DetailsNew", new { id = exp.Expense_Id });
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
                var exp = Expense.fillDataForEditing(expvm.Expense_Id);
                return View();
            }

        }
        // POST: Expense/Edit/5
        [HttpPost]
        public ActionResult EditState(ExpenseVM expvm)
        {
            try
            {
               

                var result = Expense.editTheData(expvm);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

        // GET: Expense/Delete/5
        public ActionResult Delete(int id)
        {
            
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف مصروف");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var msg = Expense.deletion(id);
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

        public ActionResult DeleteActiveExp(int id)
        {

            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف مصروف معتمد");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var msg = Expense.deletionactiveGL(id);
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
        // POST: Expense/Delete File/5

        public ActionResult DeleteFile(int fileId)
        {
            var document = Expense.DeleteFileById(fileId);           
            return RedirectToAction("Details", new { id = document.ExpenseExpense_Id });
        }
        public FileResult ReturnExpFile(int fileId)
        {

            var document = Expense.getFileById(fileId);
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
        public ActionResult GetAllFiles(int expId)
        {
            var model = Expense.GetAllFiles(expId);
            return View(model);
        }
        public virtual JsonResult GetProvinces(int countryId)
        {
            var my = Expense.GetDepartment(countryId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetCities(int provinceId)
        {
            var my = Expense.GetBussines(provinceId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء مصروف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var pack = PackageRecognize.checkValiditeOfExpenses(user.Id);
            if (pack.Item2 == true)
            {
                var exp = Expense.prepareForCrateNew();
                return View(exp);
            }
            else
            {
                var msgo = "باقتك لا تدعم قسم المصروفات";
                HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
                if (cookie != null && cookie.Value != null)
                {
                    if (cookie.Value == "en")
                    {
                        msgo = "Your package does not support the expense section";
                    }
                    else
                    {
                        msgo = "باقتك لا تدعم قسم المصروفات";
                    }
                }
                else
                {
                    msgo = "باقتك لا تدعم قسم المصروفات";
                }

                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }

        }

        // POST: Expense/Create
        [HttpPost]
        public ActionResult Create(ExpenseVM expvm)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var exp = Expense.createExpenseNew(expvm, user.Id);
                if (exp == null)
                {
                    var a = Request.UrlReferrer.ToString();
                    return Redirect(Request.UrlReferrer.ToString());
                }

                return RedirectToAction("Details", new { id = exp.Expense_Id });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
                if (cookie != null && cookie.Value != null)
                {
                    if (cookie.Value == "en")
                    {
                        ViewBag.Message = ex.Message + "___" + "Please review this error";
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
                expvm = Expense.prepareForCrateNew();
                return View(expvm);
            }
        }
        public ActionResult CreateNew()
        {
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء مصروف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var pack = PackageRecognize.checkValiditeOfExpenses(user.Id);
            if (pack.Item2 == true)
            {
                var exp = Expense.prepareForCrateNew();
                return View(exp);
            }
            else
            {
                var msgo = "باقتك لا تدعم قسم المصروفات";
                HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
                if (cookie != null && cookie.Value != null)
                {
                    if (cookie.Value == "en")
                    {
                        msgo = "Your package does not support the expense section";
                    }
                    else
                    {
                        msgo = "باقتك لا تدعم قسم المصروفات";
                    }
                }
                else
                {
                    msgo = "باقتك لا تدعم قسم المصروفات";
                }

                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }

        }

        // POST: Expense/Create
        [HttpPost]
        public ActionResult CreateNew(ExpenseVM expvm)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var exp = Expense.createExpense(expvm, user.Id);
                if (exp == null)
                {
                    var a = Request.UrlReferrer.ToString();
                    return Redirect(Request.UrlReferrer.ToString());
                }

                return RedirectToAction("DetailsNew", new { id = exp.Expense_Id });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
                if (cookie != null && cookie.Value != null)
                {
                    if (cookie.Value == "en")
                    {
                        ViewBag.Message = ex.Message + "___" + "Please review this error";
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
                expvm = Expense.prepareForCrateNew();
                return View(expvm);
            }
        }
        [HttpGet]
        public ActionResult GetExpenseGLList(int ExpRandNum)
        {
            var listofExpGl = Expense.getlistofExpenseAccounts(ExpRandNum);
            return PartialView(listofExpGl);
          
        }
        [HttpPost]
        public virtual JsonResult addExpenseGltoTemp(ExpenseAccountTempVM EXpTVM)
        {
            string msg = "";
            try
            {
                msg = Expense.addExpenseTemp(EXpTVM);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DeleteExpFromTemp(int ExpTempID)
        {
            var listofExpGl = Expense.DeleteExpenseTempItem(ExpTempID);
            return Json(listofExpGl, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult CheckTotals(int RandNum)
        {
            var listofExpGl = Expense.checkTotals(RandNum);
            return Json(listofExpGl, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public async System.Threading.Tasks.Task<JsonResult> PostExp(int ExpID)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد مصروف");



            if (employeeAction == false)
            {
                return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
            }
            await Task.Delay(2000);
           
            var ExpPosted = Expense.PostNewExpense(ExpID);
  
           
            return Json(ExpPosted, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult VoidExp(int ExpID)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد مصروف");



            if (employeeAction == false)
            {
                return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
            }
            var ExpPosted = Expense.VoidNewExpense(ExpID);


            return Json(ExpPosted, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult DetailTempList(int RandNum)
        {
            var EXp = Expense.getlistofExpenseAccounts(RandNum);
            return PartialView(EXp);

        }
        public ActionResult UpdateGL(ExpenseVM vm)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد مصروف");



                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }
                // TODO: Add update logic here

                string result = Expense.UpdatePaymentExpences(vm);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return View();
            }
        }
    }
}