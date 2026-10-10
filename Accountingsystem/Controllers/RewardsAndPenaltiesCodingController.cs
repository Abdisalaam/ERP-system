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
    public class RewardsAndPenaltiesCodingController : Controller
    {
        // GET: RewardsAndPenaltiesCoding
        public ActionResult Index()
        {

     
            var List =  RewardsAndPenaltiesCoding.getListOfRewardAndPenalties();
            return View( List);
        }

        // GET: RewardsAndPenaltiesCoding/Details/5
        public ActionResult Details(int id)
        {

            var model = RewardsAndPenaltiesCoding.getReward(id);
            return View(model);
        }

        // GET: RewardsAndPenaltiesCoding/Create
        public ActionResult Create()
        {
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            //var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء تصنيف");



            //if (employeeAction == false)
            //{
            //    return RedirectToAction("NotAuthorize", "SystemRole");
            //}
            var model = RewardsAndPenaltiesCoding.CreateVMForCreation();
            return View(model);
        }

        // POST: RewardsAndPenaltiesCoding/Create
        [HttpPost]
        public ActionResult Create(RewardsAndPenaltiesCodingVM vm)
        {
            try
            {
                // TODO: Add insert logic here

                var reward = RewardsAndPenaltiesCoding.CreateRewardAndPenalties(vm);
                if (reward != null)
                {
                    return RedirectToAction("Details", new { id = reward.Id });
                }
                else
                {
                    string msg = Resources.validate.Error;
                    return RedirectToAction("Index", new { Msg = msg });
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: RewardsAndPenaltiesCoding/Edit/5
        public ActionResult Edit(int id)
        {
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            //var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء تصنيف");



            //if (employeeAction == false)
            //{
            //    return RedirectToAction("NotAuthorize", "SystemRole");
            //}
            var model = RewardsAndPenaltiesCoding.FillTheViewModel(id);
            return View(model);
        }

        // POST: RewardsAndPenaltiesCoding/Edit/5
        [HttpPost]
        public ActionResult Edit(RewardsAndPenaltiesCodingVM vm)
        {
            try
            {
                // TODO: Add update logic here
                var reward = RewardsAndPenaltiesCoding.EditForRewardsAndPenaltiesCoding(vm);
                if (reward != null)
                {
                    return RedirectToAction("Details", new { id = reward.Id });
                }
                else
                {
                    string msg = Resources.validate.Error;
                    return RedirectToAction("Index", new { Msg = msg });
                }

            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            //var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تغير حالة التفعيل");



            //if (employeeAction == false)
            //{
            //    return RedirectToAction("NotAuthorize", "SystemRole");
            //}
            var rewardsAndPenaltiesCoding = RewardsAndPenaltiesCoding.getReward(id);
            return View(rewardsAndPenaltiesCoding);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var rewardsAndPenaltiesCoding = RewardsAndPenaltiesCoding.DeleteReward(id);
                if (rewardsAndPenaltiesCoding == 1)
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
        // GET: RewardsAndPenaltiesCoding/Delete/5

        public virtual JsonResult GetSalaryType(int PenaltyAndReward)
        {
            var my = RewardsAndPenaltiesCoding.GetAllTypeOfSalaryDependToThat(PenaltyAndReward);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
    }
}
