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
    public class AttendingAndLeavingController : Controller
    {
        // GET: AttendingAndLeaving
        public ActionResult Index()
        {
            return View();
        }

        // GET: AttendingAndLeaving/Details/5
     

        // GET: AttendingAndLeaving/Create
        public ActionResult ChooseDateAndShift(string msg)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء تصنيف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = AttendingAndLeaving.PrepareattendingAndLeavingVM();
            ViewBag.Message = msg;
            return View(model);
        }

        // POST: AttendingAndLeaving/Create
        [HttpPost]
        public ActionResult prepareEmployeeForShift(AttendingAndLeavingVM vm)
        {
            try
            {
                // TODO: Add insert logic here

                var model = AttendingAndLeaving.GetAllEmpolyeeForThatDay(vm);
                if (model == null)
                {
                  var  Message = "هذا اليوم اجازة عن وقت العمل المحدد";
                    return RedirectToAction("ChooseDateAndShift", "AttendingAndLeaving",new{msg=Message });
                }
                return View(model);
            }
            catch(Exception ex)
            {
                throw ex;
                //return View();
            }
        }
        public ActionResult PostNewAttendanceEdit(string AttendData,int ShiftId,int branchId,string Date)
        {
            dynamic batchList = JsonConvert.DeserializeObject<dynamic>(AttendData);

            int msg = AttendingAndLeaving.PostNewAttendanceEdit(batchList);
            var fullUrl = this.Url.Action("Details", "AttendingAndLeaving",new { ShiftId= ShiftId, branchId= branchId, Date= Date }, this.Request.Url.Scheme);
            return Json(fullUrl, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index", new { msg = msg });
        }
       public ActionResult Details( int ShiftId, int branchId, string Date)
        {
            var vm = new AttendingAndLeavingVM();
            vm.BranchId = branchId;
            vm.ShiftCodingId = ShiftId;
            vm.Date = Date;
            var model = AttendingAndLeaving.GetAllEmpolyeeForThatDay(vm);
            return View(model);

       
        }

        // GET: AttendingAndLeaving/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AttendingAndLeaving/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AttendingAndLeaving/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AttendingAndLeaving/Delete/5
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

        public ActionResult GetEmployeePenalties(int EmpId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء تصنيف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = AttendingAndLeaving.prepareAttending(EmpId);
            return View(model);
        }
        [HttpPost]
        public ActionResult GetAllPenaltyForThisEmployee(EmployeePenalitiesVM vm)
        {
            try
            {
                // TODO: Add insert logic here

                var model = AttendingAndLeaving.prepareAttending(vm);
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
                //return View();
            }
        }
        public ActionResult RenewPenalityHours(string Penalities,int EmpId)
        {
            dynamic penalitiList = JsonConvert.DeserializeObject<dynamic>(Penalities);

            int msg = AttendingAndLeaving.PostEditPenaltiy(penalitiList, EmpId);
            var fullUrl = this.Url.Action("Details", "Employee", new { id = EmpId }, this.Request.Url.Scheme);
            return Json(fullUrl, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index", new { msg = msg });
        }
    }
}
