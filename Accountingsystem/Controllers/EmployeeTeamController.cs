using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using Accountingsystem.Controllers;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class EmployeeTeamController : Controller
    {
        // GET: EmployeeTeam
        
        public ActionResult Index()
        {
            var allteams = EmployeeTeam.EMployeeList();
            return View(allteams);
        }

        // GET: EmployeeTeam/Details/5
        public ActionResult Details(int id)
        {
            var thisteam = EmployeeTeam.getEmployeeBYID(id);
            return View(thisteam);
        }
        public ActionResult getEmployeeByTeam(int id)
        {
            var thisteam = EmployeeTeam.EmployeeTeamDetails(id);
            return PartialView("getEmployeeByTeam", thisteam);
        }
        // GET: EmployeeTeam/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء فرق موظفين");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var teamView = EmployeeTeam.AddNewForCreation();
            return View(teamView);
        }

        // POST: EmployeeTeam/Create
        [HttpPost]
        public  ActionResult Create(EmployeeTeamVM EVM)
        {
          
            string msg = "";
            int masga =0;
            try
            {
                masga = EmployeeTeam.AddTeam(EVM);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }
            return RedirectToAction("Details",new {id= masga });
        }
        [HttpGet]
        public virtual ActionResult TeamButton()
        {
            
            return PartialView("TeamButton");
        }
        // GET: EmployeeTeam/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل فرق موظفين");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var thisteam = EmployeeTeam.FillViewEmployeeTeam(id);
            return View(thisteam);
        }

        // POST: EmployeeTeam/Edit/5
        [HttpPost]
        public ActionResult Edit( EmployeeTeamVM ETV)
        {
            try
            {
              int teamId=  EmployeeTeam.EditTeam(ETV);

                return RedirectToAction("Details", new { id = teamId });
            }
            catch(Exception  ex)
            {
                var a=ex.Message;
                return View();
            }
        }

        // GET: EmployeeTeam/Delete/5
        public ActionResult Delete(int id)
        {
            var team = EmployeeTeam.getEmployeeBYID(id);
            return View(team);

        }

        // POST: EmployeeTeam/Delete/5
        [HttpPost]
        public ActionResult Delete(int id,EmployeeTeam emp)
        {
            try
            {
                emp.Team_ID = id;
                EmployeeTeam.DeleteTeam(id);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpGet]

        public ActionResult CreateOnlyEmployeeList()
        {
            var teamView = EmployeeTeam.AddNewForCreationteamMeMber();
            //return View(teamView);
            return PartialView("CreateOnlyEmployeeList", teamView);
        }
    
        [HttpPost]
        public virtual JsonResult AddEmployeestoTemp(EmployeeTeamVM TeamIDs)
        {
            string msg = "";
            
            try
            {
                msg = EmployeeTeam.AddEmployeetoTemp(TeamIDs);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }



            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult ShowTempList()
        {
            var teamView = EmployeeTeam.EmployeeTempList();
            return PartialView("ShowTempList", teamView);
        }
        [HttpPost]
        public ActionResult AddTempToEmployeeTeam(string index)
        {
            var teamView = EmployeeTeam.UpdateEmployees();
            
            return Json(new { Url= index });
       }
        [HttpPost]
        public virtual JsonResult DeleteEmployeeFromList(EmployeeTeamVM ID)
        {
            string msg = "";

            try
            {
                msg = EmployeeTeam.DeleteEmployeeFromTemp(ID);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }



            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult DeleteTeamScript(EmployeeTeamVM ETV)
        {
            var teamView = EmployeeTeam.DeleteTeamscript(ETV);

            return Json(new { Url = "index" });
        }

    }
}
