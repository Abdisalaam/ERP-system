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
    public class DepartmentBussinesUnitController : Controller
    {
        // GET: DepartmentBussinesUnit
        public ActionResult Index(int depId)
        {
            var depart= DepartmentBussinesUnit.GetAllBussiness(depId);
            if (depart.Count() == 0)
            {
                ViewBag.message = "الرجاء ادخل نشاط لانه لا يوجد نشاط فى هذا القسم";

                // ViewBag.Id = depId;
                return RedirectToAction("Create", new { deptId = depId });
            }
            else
            {
                return View(depart);
            }
         
        }
        public ActionResult IndexNotActive(int id, String msg)
        {
            var models = DepartmentBussinesUnit.GetAllNotActive(id);
            ViewBag.message = msg;
            return View(models);
        }

        // GET: DepartmentBussinesUnit/Details/5
        public ActionResult Details(int id)
        {
            var depart = DepartmentBussinesUnit.GetBussinesById(id);
            return View(depart);
        }

        // GET: DepartmentBussinesUnit/Create
        public ActionResult Create(int deptId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء نشاط");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var depart = DepartmentBussinesUnit.CreateVM(deptId);
            return View(depart);
        }

        // POST: DepartmentBussinesUnit/Create
        [HttpPost]
        public ActionResult Create(DepartmentBussinesUnitVM vm)
        {
            try
            {
                var depart = DepartmentBussinesUnit.addNewBusinessUnit(vm);

                return RedirectToAction("Index" , new { depId = vm.BranchDepartmentDept_Id });
            }
            catch
            {
                return View();
            }
        }

        // GET: DepartmentBussinesUnit/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل نشاط");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var depart = DepartmentBussinesUnit.getEditBussines(id);

            return View(depart);
        }

        // POST: DepartmentBussinesUnit/Edit/5
        [HttpPost]
        public ActionResult Edit( DepartmentBussinesUnitVM dept)
        {
            try
            {
                DepartmentBussinesUnit.PostEditBussines(dept);

                return RedirectToAction("Index", new { depId = dept.BranchDepartmentDept_Id });
            }
            catch
            {
                return View();
            }
        }

        // GET: DepartmentBussinesUnit/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DepartmentBussinesUnit/Delete/5
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
        public ActionResult DeleteTrans(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ت غير نشط لنشاط");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = DepartmentBussinesUnit.DeleteTransfer(id, 0);
            var a = DepartmentBussinesUnit.GetBussinesById(id);
            return RedirectToAction("Index", new { depId = a.BranchDepartmentDept_Id });
        }
    }
}
