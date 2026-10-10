using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class SystemRoleController : Controller
    {
        // GET: SystemRole
        public ActionResult Index()
        {
            var list = SystemRole.getAllRole();
            return View(list);
        }
        public ActionResult IndexOfUser()
        {
            var list = SystemRole.getAllUser();
            return View(list);
        }

        // GET: SystemRole/Details/5
        public ActionResult Details(int id)
        {
            var vm = SystemRole.GetSystemRolById(id);
            return View(vm);
        }

        // GET: SystemRole/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء صلاحية");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var vm = SystemRole.prepareForeCreation();
            return View(vm);
        }

        // POST: SystemRole/Create
        [HttpPost]
        public ActionResult Create(string data, string name)
        {
            dynamic batchList = JsonConvert.DeserializeObject<dynamic>(data);

            int msg = SystemRole.Create(batchList,name);
            var fullUrl = this.Url.Action("Details", "SystemRole", new { id = msg }, this.Request.Url.Scheme);
            return Json(fullUrl, JsonRequestBehavior.AllowGet);
          
        }
       
        // GET: SystemRole/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل صلاحية");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var vm = SystemRole.prepareForEdit(id);
            return View(vm);
        }

        // POST: SystemRole/Edit/5
        [HttpPost]
        public ActionResult Edit(string data, string name,int id)
        {
            try
            {
                dynamic batchList = JsonConvert.DeserializeObject<dynamic>(data);

                int msg = SystemRole.Edit(batchList, name,id);
                var fullUrl = this.Url.Action("Details", "SystemRole", new { id = msg }, this.Request.Url.Scheme);
                return Json(fullUrl, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

        // GET: SystemRole/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SystemRole/Delete/5
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

        public ActionResult NotAuthorize()
        {
            
                return View();
            
        }
    }
}
