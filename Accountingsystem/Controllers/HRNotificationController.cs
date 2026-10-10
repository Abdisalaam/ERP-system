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
    public class HRNotificationController : Controller
    {
        // GET: HRNotification
        // GET: Category
        public ActionResult Index()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfHR(user.Id);
            if (pack.Item2 == true)
            {


                var models = HRNotification.GetAllHRNotification();
                return View(models);
            }
            else
            {
                var msgo = Resources.validate.YourpackagedoesnotsupporttheHRdepartment;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
          
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            var model = HRNotification.GetHRNotificationById(id);
            return View(model);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء تنبيه موارد بشرية");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = HRNotification.CreateVMForCreation();
            return View(model);
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(HRNotificationVM vm)
        {
            try
            {
                // TODO: Add insert logic here
                var model = HRNotification.CreateHRNotification(vm);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل تنبيه موارد بشرية");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = HRNotification.FillTheViewModel(id);
            return View(model);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(HRNotificationVM vm)
        {
            try
            {
                var model = HRNotification.EditHRNotification(vm);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            var model = HRNotification.GetHRNotificationById(id);
            return View(model);
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var model = HRNotification.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public JsonResult NoOfEnd_medical_Insurance()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تلقي تنبيهات الموراد البشرية");

            string msg = null;

            if (employeeAction == false)
            {
                msg = null;
            }

            else
            {
                msg = PackageRecognize.NoOfEnd_medical_Insurance();
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NoOfEnd_Recidence()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تلقي تنبيهات الموراد البشرية");

            string msg = null;

            if (employeeAction == false)
            {
                msg = null;
            }

            else
            {
                msg = PackageRecognize.NoOfResidence();
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
