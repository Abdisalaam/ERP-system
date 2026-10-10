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
    public class CostDistributionController : Controller
    {

        public JsonResult CheckCode(string Cost_Distr_Code)
        {
            if (CostDistribution.containCode(Cost_Distr_Code))
            {
                 var msg = "COSDS- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: CostDitribution
        public ActionResult Index(string msg,int ?all)
        {
            ViewBag.Message = msg;
            var models = CostDistribution.GetAllCostDistr();
            if (all == null)
            {
                models.Reverse();
                models = models.Take(100).ToList();
            }
            return View(models);
        }

        // GET: CostDitribution/Details/5
        public ActionResult Details(int id)
        {
            var model = CostDistribution.GetCostDistrById(id);
            return View(model);
        }

        // GET: CostDitribution/Create
        public ActionResult Create(string msg)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء توزيع التكلفة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var costDistributionVM = CostDistribution.FillCostDistributionVM();
            ViewBag.Message = msg;
            return View(costDistributionVM);
        }

        // POST: CostDitribution/Create
        [HttpPost]
        public ActionResult Create(CostDistributionVM costDistVM)
        {
            var msg = "";
            try
            {
                // TODO: Add insert logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

              var a=  CostDistribution.CreateCostDistribution(costDistVM , user.Id);
                msg = Resources.validate.CreateSucc;
                if (a == 0)
                {
                    msg = Resources.validate.Error;
                    return RedirectToAction("Create", new { msg = msg });
                }
               return RedirectToAction("Details", new { id = a });                           
                
            }
            catch
            {
                msg = Resources.validate.Error;
                return RedirectToAction("Create", new { msg = msg });
            }
        }
    }
}
