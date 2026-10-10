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
    public class ShiftCodingController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            var models = ShiftCoding.GetAllShiftCoding();
            return View(models);
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            var model = ShiftCoding.GetShiftCodingsById(id);
            return View(model);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء تصنيف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ShiftCoding.CreateVMForCreation();
            return View(model);
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(ShiftCodingVM VM)
        {
            try
            {
                // TODO: Add insert logic here
                var model = ShiftCoding.CreateShiftCoding(VM);
                var fullUrl = this.Url.Action("Details", "ShiftCoding", new { id = model }, this.Request.Url.Scheme);
                return Json(fullUrl, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: ShiftCoding/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل تصنيف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ShiftCoding.FillTheViewModel(id);
            return View(model);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(ShiftCodingVM VM)
        {
            try
            {
                var model = ShiftCoding.EditeCatgery(VM);

                var fullUrl = this.Url.Action("Details", "ShiftCoding", new { id = model.Id }, this.Request.Url.Scheme);
                return Json(fullUrl, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }
        public ActionResult AddingEmployeeToShift(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل تصنيف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ShiftCoding.GetEmployeeShift(id);
            return View(model);
        }
        public ActionResult AddingEmployeeToShiftPost(string btachQan, int hh)
        {
            dynamic batchList = JsonConvert.DeserializeObject<dynamic>(btachQan);

            int msg = Store.PostNewSalesForStore(batchList);
            var fullUrl = this.Url.Action("Details", "Store", new { id = msg }, this.Request.Url.Scheme);
            return Json(fullUrl, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index", new { msg = msg });
        }
        // GET: ShiftCoding/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ShiftCoding/Delete/5
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
        public ActionResult DeleteTrans(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ت غير نشط صنف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = ShiftCoding.DeleteShiftTransfer(id);
            return RedirectToAction("Details", new { id = id });
        }
    }
}
