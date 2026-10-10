using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class ChartOfAccountingController : Controller
    {
        // GET: ChartOfAccounting
        public ActionResult Index(string msg)
        {
            var account = ChartOfAccounting.getAllData();
            ViewBag.msg = msg;
            return View(account);
        }
        public ActionResult IndexNotActive(String msg)
        {
            var models = ChartOfAccounting.GetAllNotActive();
            ViewBag.message = msg;
            return View(models);
        }
        public ActionResult DeleteTrans(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ت غير نشط حساب");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = ChartOfAccounting.DeleteTransfer(id);
            return RedirectToAction("Details", new { id = id });
        }
        // GET: ChartOfAccounting/Details/5
        public ActionResult Details(int id)
        {
            var account = ChartOfAccounting.getDataForThatAccount(id);
            return View(account);
        }

        // GET: ChartOfAccounting/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء مركز حساب ");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var account = ChartOfAccounting.FillDataForCreate();
            return View(account);

        }

        // POST: ChartOfAccounting/Create
        [HttpPost]
        public ActionResult Create(ChartOfAccountingVM vm)
        {
            try
            {

                var account = ChartOfAccounting.CreateAccount(vm);
                if (account != null)
                {
                    var id = ChartOfAccounting.getChartbyCode(account);
                    return RedirectToAction("Details", new { id = id });
                }
                else
                {
                    string msg = Resources.validate.Error;
                    return RedirectToAction("Index",new {msg= account });
                }

               
            }
            catch
            {
                return View();
            }
        }

        // GET: ChartOfAccounting/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل مركز حساب");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var account = ChartOfAccounting.fillChartForediting(id);
            if (account==null)
            {
                 return RedirectToAction("Index",new { msg=Resources.validate.CannotEdit});
            }
            return View(account);

        }

        // POST: ChartOfAccounting/Edit/5
        [HttpPost]
        public ActionResult Edit(ChartOfAccountingVM accountVM)
        {
            try
            {
                var account = ChartOfAccounting.editForChart(accountVM);
                if (account != null)
                {
                   
                    return RedirectToAction("Details", new { id = account.Account_Id });
                }
                else
                {
                    string msg = "يوجد خطأ عند الحفظ";
                    return RedirectToAction("Index", new { msg = msg });
                }
                
            }
            catch
            {
                return View();
            }
        }

        // GET: ChartOfAccounting/Delete/5
        public ActionResult Delete(int id)
        {
            var model = ChartOfAccounting.getDataForThatAccount(id);
            return View(model);
        }

        // POST: ChartOfAccounting/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var model = ChartOfAccounting.deleteAccount(id);

                return RedirectToAction("Index",new {msg=model });
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult GetTreeJson()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "عرض شجرة الحسابات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            ChartOfAccounting.MakeLedger();
             ChartOfAccounting root = ChartOfAccounting.getDataForThatAccount(1);

            JObject json = ChartOfAccounting.traverseChildren(root);
            JArray jArr = new JArray();
            jArr.Add(json);
            return View(jArr);
        }

        [HttpGet]
        public ActionResult GetTree()
        {
           // ChartOfAccounting.MakeLedger();
            ChartOfAccounting root = ChartOfAccounting.getDataForThatAccount(1);

            JObject json = ChartOfAccounting.traverseChildren(root);
            JArray jArr = new JArray();
            jArr.Add(json);
            return Json(JsonConvert.SerializeObject(jArr), JsonRequestBehavior.AllowGet);
          
        }
        /// <summary>
        /// Gets all accounts in the form of Json array in the form specified by 
        /// Select2 (https://select2.org/data-sources/arrays) to populate 
        /// the drop-down list
        /// </summary>
        /// <returns> A Json array in the form specified by Select2  </returns>
        [HttpGet]
        public ActionResult getAccountsJson()
        {
            
            List<dynamic> result = new List<dynamic>();
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTreasuryOrBank(int AccountId)
        {

           int result = ChartOfAccounting.GeTIsThatBankOrTreasury(AccountId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getAccountsForGLJson()
        {
           //var result = ChartOfAccounting.getAlldataForGL();
            List<dynamic> result = new List<dynamic>();
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}