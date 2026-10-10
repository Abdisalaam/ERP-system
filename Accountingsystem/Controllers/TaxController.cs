using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using Accountingsystem.ViewModel.ReportsNewVM;
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
    public class TaxController : Controller
    {
        // GET: Tax
        public ActionResult Index(string msg)
        {
            ViewBag.msg = msg;
            var models = Tax.GetAlltaxes();
            return View(models);
        }

        // GET: Tax/Details/5
        public ActionResult Details(int id)
        {
            var model = Tax.GetTaxesbyId(id);
            return View(model);
        }

        // GET: Tax/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء ضريبة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var taxes = Tax.Create();
            return View(taxes);
        }

        // POST: Tax/Create
        [HttpPost]
        public ActionResult Create(TaxesVM taxVM)
        {
            try
            {
                // TODO: Add insert logic here

                var Newtaxes = Tax.AddNewTaxes(taxVM);
                if (Newtaxes != null)
                {
                    return RedirectToAction("Details", new { id = Newtaxes.Tax_ID });
                }
                else
                {
                    string msg = Resources.validate.Error;
                    return RedirectToAction("index", new { msg = msg });
                }
            }
            catch
            {
                return View(taxVM);
            }
        }

        // GET: Tax/Edit/5
        public ActionResult Edit(int id)
        {
            if (id < 1)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل ضريبة");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                string Msg = Resources.validate.Error;
                return RedirectToAction("Index", new { msg = Msg });
            }
            var tax = Tax.FillTax(id);
            return View(tax);
        }

        // POST: Tax/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, TaxesVM cuuVM)
        {
            try
            {
                // TODO: Add update logic here
                var tax = Tax.EditTaxes(cuuVM);
                if (tax != null)
                {
                    return RedirectToAction("Details", new { id = tax.Tax_ID });
                }
                else
                {
                    string msg = Resources.validate.Error;
                    return RedirectToAction("index", new { msg = msg });
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Tax/Delete/5
        public ActionResult Delete(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تغير حالة التفعيل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var tax = Tax.findForDelete(id);
            return View(tax);
        }

        // POST: Tax/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var tax = Tax.DeleteTaxes(id);
                if (tax == 1)
                {
                    return RedirectToAction("Index");
                }
                return View(collection);
            }
            catch
            {
                return View();
            }
        }
        public ActionResult makeGLTax()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "قيد التسوية الضريبية");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            var vm = Tax.FillForMakeTaxGLFRomReport();
            return View(vm);

        }
        [HttpPost]
        public ActionResult makeGLTax(TaxReportVM vm )
        {

            var message = Tax.makeGlTaxFromReport(vm);
           
            return RedirectToAction("index", new { msg = message });

        }
        public ActionResult makePayGLTax()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "قيد الدفع للتسوية الضريبية");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var vm = Tax.PreparePaymentTax();
            return View(vm);

        }
        [HttpPost]
        public ActionResult makePayGLTax(TaxPaymentVM vm)
        {

            var message = Tax.makePaymentTaxGL(vm);

            return RedirectToAction("index", new { msg = message });

        }
        public ActionResult ReportTaxSales(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير الضرائب التفصيل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = new TaxSalesandPurchase();
            model.tax_Id = id;
            return View(model);
        }
        [HttpPost]
        public ActionResult ReportTaxSales(TaxSalesandPurchase PDVM)
        {
            //var model = Tax.MakeReportTaxSalesAndPurch(PDVM);
            PDVM.ActionName = "TaxSalesList";
            //TempData["data"] = model;
         
            return RedirectToAction(PDVM.ActionName, new { taxid = PDVM.tax_Id,to= PDVM.to,from= PDVM.from });
        }
        public ActionResult TaxSalesList(int taxid ,string to,string from)
        {
            TaxSalesandPurchase tax = new TaxSalesandPurchase();
            tax.tax_Id = taxid;
            tax.to = to;
            tax.from = from;
            var model = Tax.MakeReportTaxSalesAndPurch(tax);


            
            return View(model);


        }
    }
}