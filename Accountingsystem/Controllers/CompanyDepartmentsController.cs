using Accountingsystem.Models;
using Accountingsystem.Utilites;
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
    public class CompanyDepartmentsController : Controller
    {



        // GET: CompanyDepartments
        public ActionResult Index(int id)
        {

            var CompanyDepartment = CompanyDepartments.GetCompanyDepartment(id).ToList();

            //if (CompanyDepartment.Count != 0)
            //{
                ViewBag.compid = id;
                return View(CompanyDepartment);


            //}
            //else
            //{
            //    ViewBag.compid = id;

            //    return RedirectToAction("Create", "CompanyDepartments", new { id = id });
            //}



        }
        public ActionResult IndexNotActive(int id, String msg)
        {
            var models = CompanyDepartments.GetAllNotActive(id);
            ViewBag.message = msg;
            return View(models);
        }


        public ActionResult IndexListString(string CompName)
        {

            var CompanyDepartment = CompanyDepartments.GetCompanyDeptByName(CompName);
            if (CompanyDepartment.Count() == 0)
            {
                return RedirectToAction("Index", "Company");
            }
            var compid = CompanyDepartment.First().CompanyComp_Id;
            ViewBag.compid = compid;

            return View(CompanyDepartment);
            

        }

        public ActionResult RedirectToCompany(string CompName)
        {
            return RedirectToAction("Index", "Company", new { ali = CompName });
        }



        [HttpGet]
        public ActionResult Create(int id)
        {
          
            
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء قسم لشركة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var pack = PackageRecognize.checkValiditeOfDepartment(user.Id);
            if (pack.Item2 == true)
            {
                var model = Company.CreateVMForCompDept(id);
                return View(model);
            }
            else
            {
                var msg = Resources.validate.YourpackagedoesnotsupportthatNumberOfBranch;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msg });
            }
        }
    


        [HttpPost]
        public ActionResult Create(CompDeptVM compdeptvm)
        {
            
            try
            {
                // TODO: Add insert logic here

                var NewComDep = Company.CreateCompDept(compdeptvm);

                //return Json(NewComDep, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index", "CompanyDepartments", new { id = createpart.CompanyComp_Id });
                return RedirectToAction("Details", "CompanyDepartments", new { id = NewComDep });
            }
            catch
            {
                return View();
            }
        }



        [HttpGet]
        public ActionResult Edit(int id)
        {
            // return RedirectToAction("Index", "CompanyDepartments", new { id = id });
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل قسم لشركة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = CompanyDepartments.EditCompDeptFill(id);
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(CompDeptVM compdeptvm)
        {
            try
            {
                // TODO: Add insert logic here
                var createpart = CompanyDepartments.EditCompDeptPost(compdeptvm);
              //  return RedirectToAction("Index", "CompanyDepartments", new { id = createpart.CompanyComp_Id });
                return RedirectToAction("Details", "CompanyDepartments", new { id = createpart.Id });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            var model = CompanyDepartments.GetCompanyDeptById(id);
            return View(model);
        }

        public ActionResult DeleteTrans(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ت غير نشط قسم");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = CompanyDepartments.DeleteTransfer(id);
            return RedirectToAction("Details", new { id = id });
        }
    }
}