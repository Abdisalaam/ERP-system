using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.ViewModel;
using Accountingsystem.Models;
using Accountingsystem.Utilites;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Reflection;

namespace Accountingsystem.Controllers
{
    [Authorize]

    public class VacationsController : Controller
    {
        public JsonResult CheckTotals(int empid, string start, string end)
        {
            var listofExpGl = Vacations.checkDateIsSuitable(empid, start, end);
            return Json(listofExpGl, JsonRequestBehavior.AllowGet);

        }
        
        // GET: Vacations
        public ActionResult Index()
        {
            var allVacations = Vacations.getAllVacForEmployees();
            return View(allVacations);
        }
        [AllowAnonymous]
        public ActionResult IndexOfEmployee(int id, string msg)
        {
            var allVacations = Vacations.getAllVacForThatEmployee(id);
            ViewBag.msg = msg;
            return View(allVacations);
        }
        public ActionResult IndexOfPenddingVac(string msg)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfHR(user.Id);
            if (pack.Item2 == true)
            {
                var allVacations = Vacations.getAllVacInpenddingStatus();
                ViewBag.msg = msg;
                return View(allVacations);
            }
            else
            {
                var msgo =Resources.validate.YourpackagedoesnotsupporttheHRdepartment;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
           
        }
        
        public ActionResult EmgratThVacation()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ترحيل الاجازات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = Vacations.makeEmgrationOfVacation();

            return RedirectToAction("IndexOfPenddingVac", "Vacations", new {  msg = msg });
        }
        // GET: Vacations/Details/5
        public ActionResult Details(int id)
        {
            var thisvacation = Vacations.GetVacationByID(id);
            return View(thisvacation);
        }
        [AllowAnonymous]
        // GET: Vacations/Create
        public ActionResult Create(int id)
        {
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            //var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء اجازة لموظف");



            //if (employeeAction == false)
            //{
            //    return RedirectToAction("NotAuthorize", "SystemRole");
            //}
            var forcreation = Vacations.PrepareVacForcreation(id);
            return View(forcreation);
        }
        [AllowAnonymous]
        // POST: Vacations/Create
        [HttpPost]
        public async Task< ActionResult> Create(VacationsVM VVM)
        {
            try
            {
                var vac = Vacations.AddNewVacationForEmployee(VVM);
                var admin = Employee.getEmployees().FirstOrDefault();
                var emp = Employee.getEmpById(VVM.Employee_Id).Emp_Name;
                var Message = new List<string> { };
                var name = " تم طلب اجازة من الموظف " + emp;
                var from = VVM.Start_Date_Vacations;
                var to = "الي" + VVM.End_Date_Vacations;
                var title = " من الفترة ";
                var AllTitle = title + from + to;
                var date = AllTitle;
                var resaon = VVM.Reason;
                //Message.Add(name);
                //Message.Add(AllTitle);
                //Message.Add(resaon);
                var sb = new System.Text.StringBuilder();
                 sb.AppendLine(name);
               sb.AppendLine(AllTitle);
                sb.AppendLine(resaon);
                
                var email= sb.ToString();
               // var emailmessage = string.Join("</br></br>", Message.ToArray());
                var msg = Utilites.MailManager.SendEmail(admin.Emp_Mail, "طلب اجازه ", email);
                
                

                return RedirectToAction("IndexOfEmployee", "Vacations", new { id = VVM.Employee_Id, msg = vac });
                
                  
            }
            catch (Exception e)
            {
                return View(e.Message);
            }
        }

        // GET: Vacations/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل اجازة لموظف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var thisvacation = Vacations.FillTheViewModel(id);
            return View(thisvacation);
        }

        // POST: Vacations/Edit/5
        [HttpPost]
        public ActionResult Edit(VacationsVM VVM)
        {
            try
            {
                var vac = Vacations.EditVacation(VVM);

                return RedirectToAction("IndexOfEmployee", "Vacations", new { id = VVM.Employee_Id, msg = vac });
            }
            catch
            {
                return View();
            }
        }


        // GET: Vacations/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Vacations/Delete/5
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
   
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
        public class MultipleButtonAttribute : ActionNameSelectorAttribute
        {
            public string Name { get; set; }
            public string Argument { get; set; }

            public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
            {
                var isValidName = false;
                var keyValue = string.Format("{0}:{1}", Name, Argument);
                var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

                if (value != null)
                {
                    controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                    isValidName = true;
                }

                return isValidName;
            }
        }
        [Authorize]
        

        public ActionResult ApprovfoVacation(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد اجازة او رفضها");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var forcreation = Vacations.FillTheViewModelForAproveManger(id);
            return View(forcreation);
        }

        // POST: Vacations/Create

        [HttpPost]
       

        public async Task <ActionResult> ApprovfoVacation(VacationsVM VVM)
        {
            try
            {
               
                
                var path= Request.FilePath;
                if(path.Contains("ApprovfoVacation/RefusefoVacation"))
                {
                    VVM.Status = false;
                }
                if (path.Contains("ApprovfoVacation/ApprovfoVacation"))
                {
                    VVM.Status = true;
                }
               
                var vac = Vacations.ApprovornotMangerVacation(VVM);
                var emp = Employee.getEmpById(VVM.Employee_Id);
                if (emp.Emp_Mail != null && emp.Emp_Mail != "")
                {
                    var from = VVM.Start_Date_Vacations;
                    var to = "الي" + VVM.End_Date_Vacations;
                    var title = "بخصوص الاجازة المقدمة من";
                    var AllTitle = title + from + to;
                    if (VVM.Status == true)
                    {
                        var msg = Utilites.MailManager.SendEmail(emp.Emp_Mail, AllTitle, "تم الموافقة على الاجازه");
                    }
                    else
                    {
                        var msg = Utilites.MailManager.SendEmail(emp.Emp_Mail, AllTitle, "تم رفض الاجازه " + "  " + VVM.MangerDescription);
                    }
                }
                return RedirectToAction("IndexOfPenddingVac", "Vacations", new { msg = vac });
            }
            catch (Exception e)
            {
                return View(e.Message);
            }
        }
        
      

       
        public FileResult ReturnVacationFile(int id)
        {

            var document = Vacations.GetVacationByID(id);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = document.Attachname,
                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.Attachment, "txt");
        }
        [AllowAnonymous]

        public ActionResult EmployeeLogin(string msg)
        {
            var forcreation = Vacations.makevmforEmployee();
            ViewBag.Message = msg;
            return View(forcreation);
        }
        [AllowAnonymous]
        [HttpPost]

 

        public ActionResult EmployeeLogin(EmpVM vm)
        {
            var id = Vacations.IsEmployeeOrNot(vm);
            if (id == 0)
            {
                return RedirectToAction("EmployeeLogin", "Vacations", new { msg = Resources.validate.passportWrong });
            }
            if (vm.Branch_Id == 1)
            {
                return RedirectToAction("Create", "Vacations", new { id = id });
            }
            else
            {
                return RedirectToAction("Create", "SpentReciptOrder", new { id = id });
            }
        }
    }
}
