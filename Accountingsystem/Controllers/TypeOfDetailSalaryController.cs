using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.ViewModel;
using Accountingsystem.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class TypeOfDetailSalaryController : Controller
    {
        // GET: TypeOfDetailSalary
        public ActionResult Index()
        {
            var Type = TypeOfDetailSalary.getDetailOfTypeSalary();
            return View(Type);
        }

        // GET: TypeOfDetailSalary/Details/5
        public ActionResult Details(int id)
        {
            var Type = TypeOfDetailSalary.getDetailOfTypeSalaryById(id);
            return View(Type);
        }

        // GET: TypeOfDetailSalary/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء مفرد مرتب");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var Type = TypeOfDetailSalary.fillForCreate();
            return View(Type);
        }

        // POST: TypeOfDetailSalary/Create
        [HttpPost]
        public ActionResult Create(TypeOfDetailSalaryVM TypeVM)
        {
            try
            {
                var Type = TypeOfDetailSalary.createTypeOfDetailSalary(TypeVM);

                return RedirectToAction("Details",new {id= Type });
            }
            catch
            {
                return View();
            }
        }

        // GET: TypeOfDetailSalary/Edit/5
        public ActionResult Edit(int id)
        {
            var Type = TypeOfDetailSalary.fillForEditing(id);
            return View(Type);
        }

        // POST: TypeOfDetailSalary/Edit/5
        [HttpPost]
        public ActionResult Edit(TypeOfDetailSalaryVM VM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل مفرد مرتب");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var Type = TypeOfDetailSalary.EditingTypeOfSalary(VM);

                return RedirectToAction("Details", new { id = Type });
            }
            catch
            {
                return View();
            }
        }

        // GET: TypeOfDetailSalary/Delete/5
        public ActionResult Delete(int id)
        {
            var Type = TypeOfDetailSalary.getDetailOfTypeSalaryById(id);
            return View(Type);
        }

        // POST: TypeOfDetailSalary/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
               TypeOfDetailSalary.delete(id);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }
    }
}
