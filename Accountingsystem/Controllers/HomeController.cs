using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
      
        public ActionResult Sutumatic()
        {

            return View();

        }
        public JsonResult GetExpensesAndReveniu()
        {
            var AllColumnValues = TotalDashBoard.GetAllChartColumn();
            return Json(AllColumnValues, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTotalBanksAndTreasurys()
        {
            var total = TotalDashBoard.GetAllTreasuryBankAndStores();          
            return Json(total, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTotalDailySales()
        {
            var totalDailySalesList = TotalDashBoard.getDailySales();
            return Json(totalDailySalesList, JsonRequestBehavior.AllowGet);
        }
     
        [HttpGet]
        public ActionResult GetAllPendingsDocuments()
        {
            var allPendingDocuments = AllPendingDocumanets.GetAllPendings();
            return PartialView("AllPendingDocumentsPartialView", allPendingDocuments);
        }
        public ActionResult Index(string msg, int? Recalculate)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            //var roels = Roles.GetRolesForUser();
            if (User.IsInRole("بائع"))
            {
                SystemRole.changeRoleId(user.Id, 8);
            }
            if (User.IsInRole("عميل"))
            {
                SystemRole.changeRoleId(user.Id, 6);
            }
            if (User.IsInRole("فني"))
            {
                SystemRole.changeRoleId(user.Id, 10);
            }

            if (User.IsInRole("محاسب"))
            {
                SystemRole.changeRoleId(user.Id, 7);
            }
            if (User.IsInRole("مدخل بيانات"))
            {
                SystemRole.changeRoleId(user.Id, 10);
            }
            if (User.IsInRole("مساعد محاسب"))
            {
                SystemRole.changeRoleId(user.Id, 9);
            }

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "لوحة التحكم");



            if (employeeAction == false)
            {
                return RedirectToAction("WelcomePage", "Company");
            }

            //if (User.IsInRole("بائع"))
            //{
            //    return RedirectToAction("WelcomePage", "Company");

            //}
            var total = new TotalDashBoardVM();
            var Recalculat = TotalDashBoard.RecalculateDachboard(Recalculate);
            if (Recalculat == true)
            {

                total = TotalDashBoard.GetAllDashBoardData();
                var diagram = TotalDashBoard.makeDiagrame();
            }
            else
            {
                total = TotalDashBoard.GetAllDashBoardDataNotRecalculate();
            }
            ViewBag.Message = msg;

            HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
            if (cookie != null && cookie.Value != null)
            {
                if (cookie.Value == "en")
                {
                    return View("IndexLTR", total);
                }
            }
            else
            {
                return View("Index", total);
            }

            return View("Index",total);
        }

     
        public ActionResult TotalDashboard()
        {

            var total = TotalDashBoard.GetAllDashBoardData();

            return View(total);
        }
      
    }
}