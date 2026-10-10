using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class AdminExternalEmployeeController : Controller
    {
        // GET: AdminExternalEmployee
        public ActionResult Index()
        {
            var models = ExtrnalSalesEmployee.GetAllEmployees();
            return View(models);
        }
        
        public ActionResult IndexOfAllPayment()
        {
            var models = ExtrnalSalesEmployee.GetAllPayments();
            return View(models);
        }
        public ActionResult IndexOfAllSales()
        {
            var models = ExtrnalSalesEmployee.GetAllSales();
            return View(models);
        }
        // GET: AdminExternalEmployee/Details/5
        public ActionResult Details(int id)
        {
            var model = ExtrnalSalesEmployee.GetEmployeeByID(id);
            return View(model);
        }
        public ActionResult PaymentDetails(int id)
        {
            var model = ExtrnalSalesEmployee.GetPaymentByID(id);
            return View(model);
        }
        public ActionResult SalesDetails(int id)
        {
            var model = ExtrnalSalesEmployee.GetSaleByID(id);
            return View(model);
        }
        [HttpGet]
        public ActionResult DeleteSale(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var sale = ExtrnalSalesEmployee.GetSaleByID(id);
            return View(sale);
        }
        [HttpPost]
        public ActionResult DeleteSale(EmployeeSalesDetails employeeSalesDetails)
        {
            var externalEmployee = ExtrnalSalesEmployee.DeleteSale(employeeSalesDetails.Id);
            if (externalEmployee == null)
            {              
                return View(employeeSalesDetails);
            }
            return RedirectToAction("Details", new { id = externalEmployee.Id });
        }
        [HttpGet]
        public ActionResult DeletePayment(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف دفعة لعمولة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var payment = ExtrnalSalesEmployee.FindPaymentByID(id);
            return View(payment);
        }
        [HttpPost]
        public ActionResult DeletePayment(ExternalEmployeePayments externalEmployeePayments)
        {
            var externalEmployee = ExtrnalSalesEmployee.DeletePayment(externalEmployeePayments.Id);
            if (externalEmployee == null)
            {                
                return View(externalEmployeePayments);
            }
            return RedirectToAction("Details", new { id = externalEmployee.Id });
        }
        [HttpGet]
        public ActionResult EditSale(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل مبيعات العمولات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var sale = ExtrnalSalesEmployee.PrepareSaleForEdit(id);
            return View(sale);
        }
        [HttpPost]
        public ActionResult EditSale(EmployeeSalesDetailsVM employeeSalesDetailsVM)
        {
            var sale = ExtrnalSalesEmployee.UpdateSale(employeeSalesDetailsVM);
            if (sale == null)
            {
                var model = ExtrnalSalesEmployee.PrepareSaleForEdit(employeeSalesDetailsVM.Id);
                return View(model);
            }
            return RedirectToAction("SalesDetails", new { id = sale.Id });
        }
        [HttpGet]
        public ActionResult EditPayment(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل دفعة لعمولة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var payment = ExtrnalSalesEmployee.PreparePaymentForEdit(id);
            return View(payment);
        }
        [HttpPost]
        public ActionResult EditPayment(ExternalEmployeePaymentsVM externalEmployeePaymentsVM)
        {
            var payment = ExtrnalSalesEmployee.UpdatePayment(externalEmployeePaymentsVM);
            if (payment == null)
            {
                var model = ExtrnalSalesEmployee.PreparePaymentForEdit(externalEmployeePaymentsVM.Id);
                return View(model);
            }
            return RedirectToAction("PaymentDetails", new { id = payment.Id });
        }
        [HttpGet]
        public ActionResult AddPayment()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء دفعة لعمولة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ExtrnalSalesEmployee.PreparePaymentVMForCreation();
            return View(model);
        }
        [HttpGet]
        public ActionResult AddPaymentToEmployee(int empID)
        {
            var model = ExtrnalSalesEmployee.PreparePaymentVMForCreation(empID);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddPaymentToEmployee(ExternalEmployeePaymentsVM externalEmployeePaymentsVM)
        {
            var model = ExtrnalSalesEmployee.AddPaymentToEmployee(externalEmployeePaymentsVM);
            if (model == null)
            {
               var modelVM = ExtrnalSalesEmployee.PreparePaymentVMForCreation(externalEmployeePaymentsVM.ExternalSalesEmployeeID);
                return View(modelVM);
            }
            return RedirectToAction("Details",new { id = model.Id });
        }
        [HttpGet]
        public ActionResult AddSales()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء مبيعات العمولات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ExtrnalSalesEmployee.PrepareEmployeeSalesDetailsVMForCreation();
            return View(model);
        }
       
        [HttpGet]
        public ActionResult AddSalesToEmployee(int empID)
        {
            var model = ExtrnalSalesEmployee.PrepareEmployeeSalesDetailsVMForCreation(empID);
            return View(model);
        }
        [HttpPost]
        public ActionResult AddSalesToEmployee(EmployeeSalesDetailsVM employeeSalesDetailsVM)
        {
            var model = ExtrnalSalesEmployee.AddEmployeeSalesDetailsToEmployee(employeeSalesDetailsVM);
            if (model == null)
            {
                var modelVM = ExtrnalSalesEmployee.PrepareEmployeeSalesDetailsVMForCreation(employeeSalesDetailsVM.ExternalSalesEmployeeID);
                return View(modelVM);
            }
            return RedirectToAction("Details", new { id = model.Id });
        }
        public ActionResult AllChilderingsIndex(int empID)
        {
            var models = ExtrnalSalesEmployee.GetEmployeeChilderen(empID);
            return PartialView("ChilderingsPartialView", models);
        }
        // GET: AdminExternalEmployee/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء سفراء العمولات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ExtrnalSalesEmployee.PrepareExtrnalEmployeeVMForCreation();
            return View(model);
        }

        // POST: AdminExternalEmployee/Create
        [HttpPost]
        public ActionResult Create(ExtrnalSalesEmployeeVM extrnalSalesEmployeeVM)
        {
            try
            {
                // TODO: Add insert logic here
                if (ExtrnalSalesEmployee.CheckIfEmailExist(extrnalSalesEmployeeVM.Email))
                {
                    ViewBag.Message = Resources.ExternalEmployee.EmailExistMessage;
                    extrnalSalesEmployeeVM = ExtrnalSalesEmployee.PrepareExtrnalEmployeeVMForCreation();
                    return View(extrnalSalesEmployeeVM);
                }
                if (ExtrnalSalesEmployee.CheckIfPromoCodeExist(extrnalSalesEmployeeVM.PromoCode))
                {
                    ViewBag.Message = Resources.ExternalEmployee.PromoCodeExist;
                    extrnalSalesEmployeeVM = ExtrnalSalesEmployee.PrepareExtrnalEmployeeVMForCreation();
                    return View(extrnalSalesEmployeeVM);
                }
                var model = ExtrnalSalesEmployee.AddNewExtrnalEmployee(extrnalSalesEmployeeVM);
                if (model == null)
                {
                    extrnalSalesEmployeeVM = ExtrnalSalesEmployee.PrepareExtrnalEmployeeVMForCreation();
                    return View(extrnalSalesEmployeeVM);
                }
                return RedirectToAction("Details", new { id = model.Id });
            }
            catch
            {
                extrnalSalesEmployeeVM = ExtrnalSalesEmployee.PrepareExtrnalEmployeeVMForCreation();
                return View(extrnalSalesEmployeeVM);
            }
        }

        // GET: AdminExternalEmployee/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل سفراء العمولات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ExtrnalSalesEmployee.PrepareExtrnalEmployeeVMForEdit(id);
            return View(model);
        }

        // POST: AdminExternalEmployee/Edit/5
        [HttpPost]
        public ActionResult Edit(ExtrnalSalesEmployeeVM extrnalSalesEmployeeVM)
        {
            try
            {
                // TODO: Add update logic here
                var model = ExtrnalSalesEmployee.UpdateExtrnalEmployee(extrnalSalesEmployeeVM);
                return RedirectToAction("Details", new { id = model.Id });
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult GetEmployeeOfTheMonth()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "موظف الشهر");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ExtrnalSalesEmployee.PrepareEmployeeOtheMonth();
            return View(model);
        }
        [HttpPost]
        public ActionResult GetEmployeeOfTheMonth(ExtrenlaEmployeeOfTheMonthVM extrenlaEmployeeOfTheMonthVM)
        {
            var models = ExtrnalSalesEmployee.GetEmployeeOtheMonth(extrenlaEmployeeOfTheMonthVM.StartDate, extrenlaEmployeeOfTheMonthVM.EndDate);
            return View(models);
        }
        // GET: AdminExternalEmployee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminExternalEmployee/Delete/5
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
    }
}
