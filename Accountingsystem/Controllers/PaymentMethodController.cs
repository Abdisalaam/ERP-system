using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.ViewModel;
using System.Data.Entity;
using Accountingsystem.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class PaymentMethodController : Controller
    {
        // GET: PaymentMethod
        public ActionResult Index()
        {
            var paymentMethod = PaymentMethod.GetAllPaymentMethods();
            return View(paymentMethod);
        }
        public ActionResult IndexNotActive(String msg)
        {
            var models = PaymentMethod.GetAllNotActive();
            ViewBag.message = msg;
            return View(models);
        }
        // GET: PaymentMethod/Details/5
        public ActionResult Details(int id)
        {
            var paymentMethod = PaymentMethod.GetPaymentMethodById(id);
            return View(paymentMethod);
        }

        // GET: PaymentMethod/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء طريقة دفع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var listofBank = PaymentMethod.getListForCreation();
            return View(listofBank);
        }

        // POST: PaymentMethod/Create
        [HttpPost]
        public ActionResult Create(PaymentMethodVM paymentMethodvm)
        {
            try
            {
                // TODO: Add insert logic here
                var result = PaymentMethod.createNewPaymentMethod(paymentMethodvm);
                if (result != null)
                {
                    return RedirectToAction("Details", new { id = result.PaymentMethod_Id });
                }
                else
                {
                    string msg = "يوجد خطأ عند الحفظ";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: PaymentMethod/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل طريقة دفع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var paymentMethod = PaymentMethod.fillThePayment(id);
            return View(paymentMethod);
        }

        // POST: PaymentMethod/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PaymentMethodVM paymentMethodvm)
        {
            try
            {
                id = paymentMethodvm.PaymentMethod_Id;
                // TODO: Add update logic here
                var paymentMethod = PaymentMethod.editPaymentMethod(paymentMethodvm);
                if (paymentMethod != null)
                {
                    return RedirectToAction("Details", new { id = paymentMethod.PaymentMethod_Id });
                }
                else
                {
                    string msg = "يوجد خطأ عند الحفظ";
                    return RedirectToAction("Index");
                }
                
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: PaymentMethod/Delete/5
        public ActionResult Delete(int id)
        {
            var paymentMethod = PaymentMethod.GetPaymentMethodById(id);
            return View(paymentMethod);
        }

        // POST: PaymentMethod/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var paymentMethod = PaymentMethod.deletePaymentMethod(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(collection);
            }
        }

        [HttpGet]
        public ActionResult PartialDialog()
        {
            var bank = Bank.AddBankVM();
            return PartialView("BankPaymentPartialView", bank);
        }


        [HttpPost]
        public ActionResult AddNewBankPartial(BankVM bankVM)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var result = Bank.AddNewBank(bankVM , user.Id);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpGet]
        public ActionResult GetBankDropDownList()
        {
            var account = PaymentMethod.GetBankList();
            return PartialView("BankPaymentDropDownList", account);
        }
        public ActionResult DeleteTrans(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ت غير نشط طريقة دفع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = PaymentMethod.DeleteTransfer(id);
            return RedirectToAction("Details", new { id = id });
        }

    }
}
