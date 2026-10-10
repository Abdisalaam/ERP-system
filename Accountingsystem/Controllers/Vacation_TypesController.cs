using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Models;
using Accountingsystem.Utilites;
using Accountingsystem.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Accountingsystem.Controllers
{
  
    public class Vacation_TypesController : Controller
    {
        // GET: Vacation_Types
        public ActionResult Index(string msg)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfHR(user.Id);
            ViewBag.msg = msg;
            if (pack.Item2 == true)
            {
                var allvac = Vacation_Types.getAllVacationTypes();
                return View(allvac);
            }
            else
            {
                var msgo = "باقتك لا تدعم قسم الموارد البشرية";
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
           
        }

        // GET: Vacation_Types/Details/5
        public ActionResult Details(int id)
        {
            var findVacType = Vacation_Types.GetVacationTypeByID(id);
            return View(findVacType);
        }

        // GET: Vacation_Types/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء نوع اجازة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var VacVM = new Vacation_TypesVM();
            return View(VacVM);
          
        }

        // POST: Vacation_Types/Create
        [HttpPost]
        public ActionResult Create(Vacation_TypesVM vm)
        {
            try
            {
                Vacation_Types.AddNewVacationType(vm);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: Vacation_Types/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل نوع اجازة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var vacType = Vacation_Types.FillTheViewModel(id);
            if (vacType.Id != id)
            {
                string msg = "لا يوجد هذا النوع الان";
                return RedirectToAction("Index", new { msg = msg });
            }

            return View(vacType);
        }

        // POST: Vacation_Types/Edit/5
        [HttpPost]
        public ActionResult Edit(Vacation_TypesVM VVM)
        {
            try
            {
                Vacation_Types.EditVacationType(VVM);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(VVM);
            }
        }

        // GET: Vacation_Types/Delete/5
        public ActionResult Delete(int id)
        {
            var myvac = Vacation_Types.GetVacationTypeByID(id);
            return View(myvac);
        }

        // POST: Vacation_Types/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Vacation_TypesVM VVM)
        {
            try
            {
                VVM.Id = id;
                // TODO: Add delete logic here
                int vacdelete = Vacation_Types.DeleteVacationType(id);
                var msg = "تم الحذف بنجاح";
                if (vacdelete == 1)

                {

                    return RedirectToAction("Index", new { msg = msg });

                }
                msg = "الاجازة تم استخدامها لايمكنك حذفها";
                    
                return RedirectToAction("Index", new { msg = msg });
            }
            catch
            {
                return View();
            }
        }
    }
}
