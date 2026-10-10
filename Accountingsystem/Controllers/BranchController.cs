using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Accountingsystem.Utilites;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class BranchController : Controller
    {

        public JsonResult CheckCode(string Branch_Code)
        {
            if (Branch.containCode(Branch_Code))
            {
                var msg = "BRA- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: Branch
        public ActionResult Index(int id,string msg)
        {

            var allbranches = Branch.GetBranches(id);
            ViewBag.Message = msg;
            if (allbranches.Count != 0)
            {
                ViewBag.compid = id;
                return View(allbranches);
            }
            else
            {
                ViewBag.compid = id;
                return RedirectToAction("Create", new { id = id });
            }

        }
        public ActionResult IndexNotActive(int id,String msg)
        {
            var models = Branch.GetAllNotActive(id);
            ViewBag.message = msg;
            return View(models);
        }
        public ActionResult IndexList(string compname)
        {
            var allbranches = Branch.GetBranchesByName(compname);
            if (allbranches.Count != 0)
            {
                ViewBag.compid = allbranches.FirstOrDefault().CompanyComp_Id;
                return View(allbranches);
            }
            else
            {
                ViewBag.compid = allbranches.FirstOrDefault().CompanyComp_Id;
                return RedirectToAction("Create", new { id = ViewBag.compid });
            }
        }


        public ActionResult IndexListString(string CompName)
        {

            var CompanyBranch = Branch.GetBranchesByName(CompName);
            var compid = CompanyBranch.First().CompanyComp_Id;
            ViewBag.compid = compid;

            return View(CompanyBranch);

        }
        // GET: Branch/Details/5
        public ActionResult Details(int id)
        {

            var branchdetails = Branch.GetBranchById(id);
            return View(branchdetails);
        }

        // GET: Branch/Create
        public ActionResult Create(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء فرع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var pack = PackageRecognize.checkValiditeOfBranch(user.Id);
            if (pack.Item2 == true)
            {
                var createfill = Branch.CreateVMForCreation(id);
                return View(createfill);
            }
            else
            {
                var msg = Resources.validate.YourpackagedoesnotsupportthatNumberOfBranch;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msg });
            }
        }

        // POST: Branch/Create
        [HttpPost]
        public ActionResult Create(BranchVM branchvm)
        {
            try
            {
                // TODO: Add insert logic here
                var createbra = Branch.CreateBranch(branchvm);
                //  return RedirectToAction("Index",new {id = createbra.CompanyComp_Id});
                return RedirectToAction("Index", "Branch", new { id = createbra.CompanyComp_Id });
            }
            catch
            {
                return View();
            }
        }

        // GET: Branch/Edit/5
        public ActionResult Edit(int id,string msg)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل فرع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var branchvm = Branch.EditCompDeptFill(id);
            ViewBag.Message=msg;
            return View(branchvm);
        }

        // POST: Branch/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, BranchVM bravm)
        {
            try
            {
                // TODO: Add update logic here
                var editbra = Branch.EditBranchPost(bravm);
                var msg = Resources.validate.Editsucc;
                return RedirectToAction("Index", "Branch", new { id = editbra.CompanyComp_Id,msg=msg });
                //   return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //// GET: Branch/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Branch/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        [HttpGet]
        public ActionResult CreateBranchDept(int id)
        {
            return RedirectToAction("Index", "BranchDept", new { id = id });
        }
        public ActionResult DeleteTrans(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ت غير نشط فرع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = Branch.DeleteTransfer(id);
            return RedirectToAction("Details", new { id = id });
        }
    }
}