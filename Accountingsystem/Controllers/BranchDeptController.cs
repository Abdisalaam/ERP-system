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
    public class BranchDeptController : Controller
    {
        // GET: BranchDept
        public ActionResult Index(int id)
        {
            var alldepartments = BranchDepartment.GetBranchDepartment(id);
            //if (alldepartments.Count != 0)
            //{
                ViewBag.branchid = id;

                return View(alldepartments);

            //}
            //else
            //{
            //    ViewBag.branchid = id;

            //    return RedirectToAction("Create", "BranchDept", new { id = id });
            //}




        }
        public ActionResult IndexNotActive(int id,String msg)
        {
            var models = BranchDepartment.GetAllNotActive(id);
            ViewBag.message = msg;
            return View(models);
        }

        // GET: BranchDept/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BranchDept/Create
        public ActionResult Create(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء قسم لفرع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var bd = BranchDepartment.CreateVMForbranchDept(id);
            return View(bd);
        }

        // POST: BranchDept/Create
        [HttpPost]
        public ActionResult Create(BranchDeptVM bdvm)
        {
            try
            {
                // TODO: Add insert logic here
                var bdmodel = BranchDepartment.CreateBranchDept(bdvm);
                return RedirectToAction("Index", "BranchDept", new { id = bdmodel.BranchBranch_Id });
                //   return RedirectToAction("Create", "BranchDept", new { id = id });

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return View();
            }
        }

        // GET: BranchDept/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل قسم لفرع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var branchdeptvm = BranchDepartment.EditCompDeptFill(id);

            return View(branchdeptvm);
        }

        // POST: BranchDept/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, BranchDeptVM bdvm)
        {
            try
            {
                // TODO: Add update logic here
                var editpost = BranchDepartment.EditBranchPost(bdvm);
                return RedirectToAction("Index", "BranchDept", new { id = editpost.BranchBranch_Id });

                //    return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: BranchDept/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BranchDept/Delete/5
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

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ت غير نشط قسم لفرع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = BranchDepartment.DeleteTransfer(id,0);
            var a = BranchDepartment.GetBranchDepartmentByID(id);
            return RedirectToAction("Index", new { id = a.BranchBranch_Id });
        }
    }
}