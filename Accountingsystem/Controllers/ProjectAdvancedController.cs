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
    public class ProjectAdvancedController : Controller
    {
        // GET: ProjectAdvanced
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProjectAdvanced/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProjectAdvanced/Create
        public ActionResult Create(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء مراكز تكلفة لمشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var vm = ProjectAdvanced.CreateSubledgerForProject(id);
            return View(vm);
        }
        public ActionResult prepareSubledger()
        {
            var vm = new ProjectSubledgerVM();
            return PartialView(vm);
        }
        public ActionResult prepareSubSubledger()
        {
            var vm = new ProjectSubSubledgerVM();
            vm.projectSubledgerList = new SelectList(Enumerable.Empty<SelectListItem>());
            return PartialView(vm);
        }
        public ActionResult prepareClause()
        {
            var vm = new ProjectClauseVM();
            return PartialView(vm);
        }
        public ActionResult GetProjectSubLedgertempsByProId(int protemp_Id)
        {
            var vm = ProjectAdvanced.GetProjectSubLedgertempsByProId(protemp_Id);
            return PartialView(vm);
        }
        public ActionResult GetProjectSubSubtempsByProId(int protemp_Id)
        {
            var vm = ProjectAdvanced.GetProjectSubSubLedgertempsByProId(protemp_Id);
            return PartialView(vm);
        }
        public ActionResult GetProjectClausetempsByProId(int protemp_Id)
        {
            var vm = ProjectAdvanced.GetProjectClausetempsByProId(protemp_Id);
            return PartialView(vm);
        }
        public virtual JsonResult AddSubledgerToMaster(ProjectSubledgerVM vm)
        {
            string msg = "";
            try
            {
                msg = ProjectAdvanced.AddSubledgerList(vm);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }



            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public virtual JsonResult AddSubSubledgerToMaster(ProjectSubSubledgerVM vm)
        {
            string msg = "";
            try
            {
                msg = ProjectAdvanced.AddSubSubledgerToMaster(vm);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }



            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public virtual JsonResult AddClauseToMaster(ProjectClauseVM vm)
        {
            string msg = "";
            try
            {
                msg = ProjectAdvanced.AddClauseList(vm);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }



            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult GetSubList(int Pro_Id)
        {
            var sub = ProjectAdvanced.GetSubledgerList(Pro_Id);
            return new JsonResult { Data = sub, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult deleteSubtemp(int ids)
        {
            var sub = ProjectAdvanced.deleteSubledgerTemp(ids);
            return new JsonResult { Data = sub, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult deleteClause(int ids)
        {
            var sub = ProjectAdvanced.deleteClauseTemp(ids);
            return new JsonResult { Data = sub, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        
        // POST: ProjectAdvanced/Create
        [HttpPost]
        public ActionResult Create(ProjectVM vm)
        {
            try
            {
                var sub = ProjectAdvanced.addSubToProject(vm);

                return RedirectToAction("Details","Project",new {id=vm.Project_Id });
            
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectAdvanced/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProjectAdvanced/Edit/5
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

        // GET: ProjectAdvanced/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProjectAdvanced/Delete/5
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
