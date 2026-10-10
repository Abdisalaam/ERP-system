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
    public class CompanyController : Controller
    {

        public JsonResult CheckCode(string Comp_Code)
        {
            if (Company.containCode(Comp_Code))
            {
                var msg = "COM- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckEndDate(string date)
        {
            var msg = Company.checkEndDate(date);

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        // GET: Company
        public ActionResult Index(string ali)
        {
            ViewBag.Cretical = ali;
            var models = Company.GetCompanies();
            return View(models);
        }
        public ActionResult WelcomePage(string ali)
        {
           
            var models = Company.getTheFirstPage();
            return View(models);
        }

        // GET: Company/Details/5
        public ActionResult Details(int id)
        {
            var model = Company.GetCompanyById(id);
            return View(model);
        }
        
        // GET: Company/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء الشركة ");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Company.CreateVMForCreation();
            return View(model);
        }

        // POST: Company/Create
        [HttpPost]
        public ActionResult Create(CompanyVM companyVM)
        {
            var model = Company.CreateVMForCreation();
            var msg =Resources.validate.CreateSucc;
            try
            {

                var result = Company.CreateCompany(companyVM);

                if (!result)
                {

                    return View(model);

                }
                return RedirectToAction("Index", new { msg = msg });

            }
            catch (Exception)
            {
                msg =Resources.validate.Error;
                return View(model);
            }
        }


        // GET: Company/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل الشركة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var compvm = Company.FillTheViewModel(id);

            //  return RedirectToAction("Index", "CompanyDepartments", new { id = 5 });
            return View(compvm);
        }

        // POST: Company/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CompanyVM compvm)
        {
            try
            {
               
                // TODO: Add update logic here
                var createcomp = Company.EditCompany(compvm);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Company/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpGet]
        public virtual JsonResult GetProvinces(int countryId)
        {
            var my = Company.GetCountryProvinces(countryId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetCities(int provinceId)
        {
            var my = Company.GetProvincesCities(provinceId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }


        // POST: Company/Delete/5
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


        [HttpGet]
        public ActionResult CreateCompDept(int id)
        {
            return RedirectToAction("Index", "CompanyDepartments", new { id = id });

            // var model = Company.CreateVMForCompDept(id);
            // return View(model);
        }
        


        [HttpGet]
        public ActionResult CreateCompBranch(int id)
        {
            return RedirectToAction("Index", "Branch", new { id = id });

            // var model = Company.CreateVMForCompDept(id);
            // return View(model);
        }

        [HttpGet]
        public ActionResult CreatePartner(int id)
        {
            return RedirectToAction("Index", "Partner", new { compid = id });

            // var model = Company.CreateVMForCompDept(id);
            // return View(model);
        }


        public FileResult ReturnproFile(int fileId)
        {

            var document = Company.getFileById(fileId);
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

        public ActionResult GetAllFiles(int compId)
        {
            var model = Company.GetAllFiles(compId);
            return View(model);
        }

        public FileResult ReturnproPhoto(int fileId)
        {
            var document = Company.getPhotoById(fileId);
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
        

        public ActionResult GetAllPhoto(int compId)
        {
            var model = Company.GetAllPhoto(compId);
            return View(model);
        }


        public ActionResult PartialDialogCurrency()
        {
            var cur = Currency.CreateCurrencyVM();
            return PartialView("PartialDialogCurrency", cur);
        }

        [HttpPost]
        public ActionResult AddNewCurrency(CurrencyVM currencyVM)
        {

            var result = Currency.CreateCurrency(currencyVM);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        [HttpGet]
        public ActionResult GetCurrencyDropdownList()
        {
            var cur = Company.GetCurrencyList();
            return PartialView("GetCurrencyDropdownList", cur);
        }
        public ActionResult DeletePhoto(int fileId)
        {

        var a= Company.DeletePhoto(fileId);
            return RedirectToAction("Details", new {id=a });
        }
        public ActionResult DeleteFile(int fileId)
        {

            var a = Company.DeleteFile(fileId);
            return RedirectToAction("Details", new { id = a });
        }
    }
}