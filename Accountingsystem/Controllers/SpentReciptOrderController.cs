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
    public class SpentReciptOrderController : Controller
    {
        // GET: SpentReciptOrder
        public ActionResult Index()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "قوائم طلبات السلف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var spentList = SpentReciptOrder.getAllSpentReciptOrders();
            return View(spentList);
        }
        public ActionResult IndexForType(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "قوائم طلبات السلف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var spentList = SpentReciptOrder.getAllSpentReciptOrdersByType(id);
            return View(spentList);
        }
        // GET: SpentReciptOrder/Details/5
        public ActionResult Details(int id)
        {
            var spent = SpentReciptOrder.getSpentReciptOrderByItsId(id);
            return View(spent);
        }

        // GET: SpentReciptOrder/Create
        public ActionResult Create(int?id)
        {
            var spent = SpentReciptOrder.FillForCreate(id);
            return View(spent);
        }

        // POST: SpentReciptOrder/Create
        [HttpPost]
        public ActionResult Create(SpentReciptOrderVM vm)
        {
            try
            {
                var spent = SpentReciptOrder.Create(vm);
                return RedirectToAction("Details",new {id= spent });
            }
            catch
            {
                return View();
            }
        }

        // GET: SpentReciptOrder/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد طلب سلفه او رفضها");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var spent = SpentReciptOrder.managerAcceptForFill(id);
            return View(spent);
        }

        // POST: SpentReciptOrder/Edit/5
        [HttpPost]
        public ActionResult Edit(SpentReciptOrderVM vm)
        {
            try
            {
                var path = Request.FilePath;
                if (path.Contains("ApprovfoRecipt"))
                {
                    vm.SpentReciptOrderTypeId = 2;
                }
                if (path.Contains("RefusefoRecipt"))
                {
                    vm.SpentReciptOrderTypeId = 3;
                }
                var spent = SpentReciptOrder.mangerApprovePost(vm);
                return RedirectToAction("Details", new { id = vm.Id });
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: SpentReciptOrder/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SpentReciptOrder/Delete/5
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
    }
}
