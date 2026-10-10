using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

using Microsoft.Owin.Security;

using System.Web.Security;
using Accountingsystem.Utilites;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private ApplicationUserManager _userManager;
        // GET: Emp
      
        public JsonResult CheckCode(string Emp_Code)
        {
            if (Employee.containCode(Emp_Code))
            {
                var msg = "EMP- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult makeEmpExcel()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "بيانات الموظف اكسيل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var excelProduct = new ExcelSheetTemplate();
            Response.ClearContent();
            Response.BinaryWrite(excelProduct.GenerateExcelForEmployee());
            Response.AddHeader("content-disposition", "attachment; filename=Employee.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");
        }
        public ActionResult Getexcel()
        {
            return File("~/FiLeDownLoad/Employee.xlsx", "application/xlsx", Server.UrlEncode("~/FiLeDownLoad/Employee.xlsx"));
        }
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            try
            {
                var msg = Employee.Import(excelfile);
                return RedirectToAction("Index", new { msg = msg });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { msg = ex.Message });

            }
        }
        public JsonResult Checkdate(string date)
        {
            if (Employee.Checkdate(date) == false)
            {
                var msg = Resources.validate.Payrollprocessingcannotbedoneforafuturedate;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(string msg)
        {

            var models = Employee.getEmployees();
            ViewBag.employee = msg;
            ViewBag.Message = msg;
            return View(models);
        }

        public ActionResult IndexOfNotActiVeEmployee()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            //var pack = PackageRecognize.checkValiditeOfHR(user.Id);
            //if (pack.Item2 == true)
            //{
                var Empolyee = Employee.getEmployeesNotActive();
                return View(Empolyee);
            //}
            //else
            //{
            //    var msgo = Resources.validate.YourpackagedoesnotsupporttheHRdepartment;
            //    return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            //}

        }
        public ActionResult ChangeValueOfEmployeeActiveOrNot(int id)
        {
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تحويل الموظف لغير نشط");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            //var pack = PackageRecognize.checkValiditeOfHR(user.Id);
            //if (pack.Item2 == true)
            //{

                var Active = Employee.ChangeStatusOfEmployeeToActiveOrNot(id);
                if (Active == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("IndexOfNotActiVeEmployee");
                }
            //}
            //else
            //{
            //    var msgo = Resources.validate.YourpackagedoesnotsupporttheHRdepartment; 
            //    return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            //}
        }
        // GET: Emp/Details/5
        public ActionResult Details(int id)
        {
            var model = Employee.getEmpById(id);
            return View(model);
        }
        public ActionResult GetAllTempSalaryDetails(int tempPurchaseInvoiceMasterId)
        {
            var purchaseInvoiceDetailVm = Employee.GetAllTempSalaryDetails(tempPurchaseInvoiceMasterId);
            return PartialView("SalaryDetailsList", purchaseInvoiceDetailVm);
        }
        public ActionResult RemoveFile(int Fileid)
        {
            var Empid = Employee.deleteFile(Fileid);
            return RedirectToAction("Details", new { id = Empid });
        }
        public ActionResult RemoveImage(int Emp_Id)
        {
            var Empid = Employee.deleteImage(Emp_Id);
            return RedirectToAction("Details", new { id = Empid });
        }
        public virtual JsonResult AddSalaryDetailsToMaster(Salary_ComponentsVM purchaseInvoiceMasterDetail)
        {
            string msg = "";
            try
            {
                msg = Employee.addSalaryDetailTemp(purchaseInvoiceMasterDetail);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }



            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public ActionResult PrepareSalaryDetails()
        {
            var purchaseMaster = Employee.prepareSalaryComponant();
            return PartialView("SalaryDetailCreation", purchaseMaster);
        }


        public virtual JsonResult AddVacationDetailsToMaster(EmpVacationVM vacationVM)
        {
            string msg = "";
            try
            {
                msg = Employee.addVacationTemp(vacationVM);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }



            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public ActionResult PrepareVacationDetails()
        {
            var vm = Employee.prepareVacation();
            return PartialView(vm);
        }
        public ActionResult GetAllTempVacationDetails(int Empid)
        {
            var purchaseInvoiceDetailVm = Employee.GetAllEmpVacationDetails(Empid);
            return PartialView( purchaseInvoiceDetailVm);
        }

        public ActionResult Create()
        {
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء موظف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var pack = PackageRecognize.checkValiditeOfEmployee(user.Id);
            if (pack.Item2 == true)
            {
                var model = Employee.getViewForCreation();
                return View(model);
            }
            else
            {
                var msgo = Resources.validate.YourpackagedoesnotsupporttheHRdepartment; 
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });

            }
        }

        // POST: Emp/Create
        [HttpPost]
        public ActionResult Create(EmpVM empVM)
        {
            try
            {
                //string NewEmployee = "تم اضافة الموظف بنجاح";
                var id = Employee.CreateEmployee(empVM);
                // return Json(NewEmployee, JsonRequestBehavior.AllowGet);
                if (id == 0)
                {
                    return RedirectToAction("Index", new { msg = "يوجد خطأ فى الادخال" });
                }
                else
                {
                    return RedirectToAction("Details", new { id = id });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { msg = "يوجد خطأ فى الادخال" });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public FileResult ReturnEmpFile(EmployeeFileDetail empFileDetail)
        //{
        //    var document = empFileDetail;
        //    var cd = new System.Net.Mime.ContentDisposition
        //    {
        //        // for example foo.bak
        //        FileName = document.FileName,
        //        // always prompt the user for downloading, set to true if you want 
        //        // the browser to try to show the file inline
        //        Inline = false,
        //    };
        //    Response.AppendHeader("Content-Disposition", cd.ToString());
        //    return File(document.ProImg, "txt");
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empId"></param>
        /// <returns>Action</returns>
        //public ActionResult DownloadEmpFiles(int empId)
        //{
        //    var emp = Employee.getEmpById(empId);
        //    List<EmployeeFileDetail> emFileDetails = new List<EmployeeFileDetail>();
        //    foreach (var empFileDetails in emp.EmployeeFileDetails)
        //    {
        //        ReturnEmpFile(empFileDetails);
        //    }
        //    return RedirectToAction("Details", new { id = empId });
        //}

        // GET: Emp/Edit/5
        public FileResult ReturnEmpFile(int fileId)
        {

            var document = Employee.getFileById(fileId);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = document.FileName,
                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.ProImg, "txt");
        }

        public ActionResult GetAllFiles(int empId)
        {
            var model = Employee.GetAllFiles(empId);
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل موظف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Employee.FillViewForEditting(id);
            return View(model);
        }

        // POST: Emp/Edit/5
        [HttpPost]
        public ActionResult Edit(EmpVM empVM)
        {
            try
            {
                string msg = "يوجد خطأ في التعديل";
                var Emp = Employee.Edit(empVM);
                if (Emp == null)
                {
                    return RedirectToAction("Index", new { msg = msg });
                }
                else
                {
                    return RedirectToAction("Details", new { id = Emp.Emp_Id });
                }

                // return Json(NewEmployee, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View("Index");
            }
        }



        public ActionResult EditAdmin()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل موظف");

            //if (user.Id != Client.getAllClient().FirstOrDefault().User_Id)
            //{
            //    return RedirectToAction("NotAuthorize", "SystemRole");
            //}

            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Employee.FillViewForEdittingForAdmin(user.Id);
            return View(model);
        }

        // POST: Emp/Edit/5
        [HttpPost]
        public ActionResult EditAdmin(EmpVM empVM)
        {
            try
            {
                string msg = "يوجد خطأ في التعديل";
                var Emp = Employee.EditAdmin(empVM);
                if (Emp == null)
                {
                    return RedirectToAction("Index","Company", new { msg = msg });
                }
                else
                {
                    return RedirectToAction("Index", "Company", new { msg = msg });
                }

                // return Json(NewEmployee, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View("Index");
            }
        }


        public JsonResult DeleteTempDetail(int iD)
        {
            var temp = Employee.DeleteEmployeeFromTemp(iD);
            var msg = Resources.validate.DeleteSuccessful;


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteTempVacation(int iD)
        {
            var temp = Employee.DeleteEmpVacation(iD);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
        // GET: Emp/Delete/5
      

        // POST: Emp/Delete/5
    
        public ActionResult Delete(int id)
        {
            try
            {

                var emp = Employee.deleteEmplyee(id);
                return RedirectToAction("Index", new { msg = emp });
            }
            catch
            {
                return View();
            }
        }
        public ActionResult EditStatus(int id)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تغيير حالة صرف الراتب");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }

                Employee.UpdateEmployeeSalary(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var erorr = msg;
                var data = ex.Data;
                return View();

            }
        }
        public ActionResult makeGL()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "قيد استحقاق الرواتب");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfHR(user.Id);
            if (pack.Item2 == true)
            {

            
                var sal = Employee.FillForMakeGLForSalary();

                return View(sal);
            }
            else
            {
                var msgo = Resources.validate.YourpackagedoesnotsupporttheHRdepartment;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }

        }
      
        public ActionResult PreparemakeGL(SalGLDate salvm)
        {




            var sal = Employee.GetlListOfEmployeeSalary(salvm);

            return View(sal);



        }
        [HttpPost]
        public  ActionResult makeGL(SalGLDate salvm)
        {



            var sal = Employee.GetlListOfEmployeeSalary(salvm);
            var msg = Employee.MakeGLForSalary(salvm);
            ViewBag.Message = msg;
            return RedirectToAction("Index", new { msg = msg });



        }

        public async Task<ActionResult> makevoidGL()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "قيد الغاء استحقاق الرواتب");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var pack = PackageRecognize.checkValiditeOfHR(user.Id);
            if (pack.Item2 == true)
            {
                await Task.Delay(2000);
                var voidgl = Employee.FillForMakeVoidOfGLSalary();
              
                return View(voidgl);
            }
            else
            {
                var msgo = Resources.validate.YourpackagedoesnotsupporttheHRdepartment;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });

            }
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> makevoidGL(SalGLDate vm)
        {
          
                await Task.Delay(2000);
                var msg = Employee.MakeVoidOfGLSalary(vm);
                ViewBag.Message = msg;
                return RedirectToAction("Index", new { msg = msg });
            
            
        }

        public ActionResult makeTransferSalaryToAccount(string msg)
        {


            var model = Employee.transferToBank();
            ViewBag.Message = msg;
            return View(model);
        }
        [HttpPost]
        public ActionResult makeTransferSalaryToAccount(TransferSalaryToAccountVM trans)
        {
            var msg = Employee.MakeGLForTransferToBank(trans);
            ViewBag.Message = msg;
            return RedirectToAction("makeTransferSalaryToAccount", new { msg = msg });
        }


        public virtual JsonResult GetDepartments(int departId)
        {
            var my = Employee.GetDepartment(departId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetBussiness(int bussinId)
        {
            var my = Employee.GetBussines(bussinId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        //////////////////////////////////////////EMployee Settings///////////////////
        public ActionResult CreateSetting(int id)
        {
            var model = Employee.SalessettingforCreation(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult CreateSetting(Sales_SettingsVM Svm)
        {
            var model = Employee.AddSettings(Svm);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult EditSettings(int id)
        {
            var model = Employee.SalessettingforEdditing(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult EditSettings(Sales_SettingsVM SVM)
        {
            var model = Employee.EditSettings(SVM);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult AddSliceTotemp(int id)
        {
            var model = Employee.addSlicetoTemp(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult AddAllSlices()
        {
            var model = Employee.AddAllSlicestoTemp();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewListOFTemp()
        {
            var model = Employee.AlltempList();
            return PartialView(model);
        }
        [HttpGet]
        public ActionResult makManualGL()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "صرف الرواتب واستلام السلف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var pack = PackageRecognize.checkValiditeOfHR(user.Id);
            if (pack.Item2 == true)
            {
                var model = Employee.manualGLSalary();
                return View(model);
            }
            else
            {
                var msgo = Resources.validate.YourpackagedoesnotsupporttheHRdepartment;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> makManualGL(EmployeeSalaryVM vm)
        {
            await Task.Delay(2000);
            var model = Employee.PostManualGLSalary(vm);
            return RedirectToAction("makManualGL");
        }
        public ActionResult GetAllSallaryThatNotExpense()
        {
            var model = Employee.getAllsallaryForThatMounth();
            return PartialView(model);
        }
        public ActionResult EditStatusSalary(int id)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var pack = PackageRecognize.checkValiditeOfHR(user.Id);
                if (pack.Item2 == true)
                {

                    Employee.UpdateSalaryMounth(id);

                    return RedirectToAction("makManualGL");
                }
                else
                {
                    var msgo = Resources.validate.YourpackagedoesnotsupporttheHRdepartment;
                    return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var erorr = msg;
                var data = ex.Data;
                return View();

            }

        }
        public ActionResult EditStatusSalaryforAll()
        {
            try
            {

                Employee.UpdateSalaryMounthForAllEmp();

                return RedirectToAction("makManualGL");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var erorr = msg;
                var data = ex.Data;
                return View();

            }

        }
        public ActionResult getAllSalForThatMounth()
        {
            var empsal = Employee.getAllSalaryInThatMounth();
            return View(empsal);
        }
        /////////////////// Employee Sales and  Collectors////////////////
        public ActionResult ShowDetails(int id)
        {
            var model = Employee.FillEmplyeeSales(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult AddDetailsOrUpdate(Employee_SalesVM Svm)
        {
            var model = Employee.AddDetailsToEMployeeSales(Svm);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        ///////////////////////// Employee Adding  And  Removing  Roles./////////////////
        [HttpGet]
        public ActionResult RoleView(int id ,string msg)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل صلاحية موظف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Employee.EditRoleForEditting(id);
            ViewBag.Message = msg;
            return View(model);
        }
        [HttpPost]
        public ActionResult RoleView(EmployeeRoleEditVM svm)
        {
            var id = svm.EMployeeID;
            if (svm.roleIDMain == "Admin" || svm.roleIDMain == "مالك نظام")
            {
                svm.roleIDMain = "عميل";
            }
            //var value = svm.roleIDMain;
            //var old = UserManager.FindById(id);
            var msg = Resources.validate.Editsucc;
            //if (old.Roles.FirstOrDefault()!=null)
            //{
            //    var roles = await UserManager.GetRolesAsync(id);
            //    await UserManager.RemoveFromRolesAsync(id, roles.ToArray());
            //    //DeleteConfirmed(id);
            //    UserManager.AddToRole(id, value);
            //    msg = Resources.validate.Editsucc;

            //    return RedirectToAction("RoleView", new { msg = msg });
            //}
            //else
            //{
            //    UserManager.AddToRole(id, value);
            var a = Employee.EditRoleForEditting(svm);

            return RedirectToAction("Index", new { msg = msg });
            //}

        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [HttpGet]
        public ActionResult RoleList(string msg)
        {

         

                var result = (from user in UserManager.Users
                              from role in user.Roles
                              where role.UserId == user.Id
                              select new UserRoleVM
                              {
                                  fname = user.UserName,
                                  fname2 = user.UserName,
                                  rname = role.RoleId,
                                  rname2 = role.RoleId,
                                  UserID = role.UserId

                              }).ToList();
                var result2 = Employee.GetRoleANdUserNames(result);
                ViewBag.Message = msg;
                return View(result2);
        
        }
        public ActionResult DeleteR(string Userid, string RName)
        {
            var RegID = Employee.GetEmployeeRegID(Userid);
            //var result = await UserManager.RemoveFromRoleAsync(Userid,RName);
            var msg = "";
            //if (result.Succeeded)
            //{
                var ROleList = UserManager.RemoveFromRole(RegID, RName);

               
                if (ROleList != null)
                {
                    msg = Resources.validate.DeleteSuccessful;
                return RedirectToAction("RoleList", new { msg = msg });
            }
                return RedirectToAction("RoleList", new { msg = msg });
            //}
            //else
            //{
            //    msg = "لم يتم حذف الصلاحية";
            //    return RedirectToAction("RoleList", new { msg = msg });
            //}
        }

        public ActionResult AddRole()
        {
            var newrole = Employee.RoleCreateVM();
            return View(newrole);
        }
        [HttpPost]
        public ActionResult AddRole(RoleCreateVM RCVM)
        {
            var newrole = Employee.addRole(RCVM);
            return RedirectToAction("AllRoleList", new { msg = newrole });
        }
        public ActionResult AllRoleList()
        {
          
                var allROles = Employee.ListofRole();

                return View(allROles);

        }
        //ForEdit Sallary
        public ActionResult PrepareSallaryDetails(int ComId)
        {
            var model = Employee.perpareSalaruEmp(ComId);
            return PartialView("SalaryComDetailCreationForEdit", model);

        }
        public ActionResult GetAllTempComDetailsEdit(int EmpId)
        {
            var sliceProduct = Employee.GetAllEmpSalaryDetails(EmpId);
            return PartialView("SalListForEdit", sliceProduct);
        }
        public ActionResult GetAllTempEmpTempSalDetailsResult(int EmpId)
        {
            var _SliceProductTemp = Employee.GetallEmpDetailsTemp(EmpId);
            return PartialView("SalResultListTemp", _SliceProductTemp);
        }

        public ActionResult GetAllTempForEditVacation(int EmpId)
        {
            var _SliceProductTemp = Employee.GetallEmpDetailsVacationTemp(EmpId);
            return PartialView(_SliceProductTemp);
        }
        public ActionResult GetTheSallaryForAllEmployeeForThatMounth(string msg)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مرتبات الموظفيين");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var empSal = Employee.GetAllSallryDate();
            ViewBag.Message = msg;
            empSal.ActionName = "AllSallaryForThatMounth";
            return View(empSal);


        }
        [HttpPost]
        public ActionResult GetTheSallaryForAllEmployeeForThatMounth(EmployeeSalVM vm)
        {
            return RedirectToAction(vm.ActionName, new { VM = vm.Date });
        }

       
        public ActionResult AllSallaryForThatMounth(string VM)
        {
            var empSal = Employee.getAllSallaryforThatMounth(VM);
            TempData["EmployeeData"] = empSal;
            if (empSal.Count()>0) {
                return View(empSal);
            }
            else
            {
                var msg =Resources.validate.Nosalarydataforthismonth;
                return RedirectToAction("GetTheSallaryForAllEmployeeForThatMounth", new { msg = msg });
            }
           


        }
        public ActionResult allDetailForThatSallary(int Id)
        {
            var empSal = Employee.getDetailOfThatSallary(Id);
            return View(empSal);
        }

    
        public ActionResult EditOfSalaryMounth(int Id)
        {
            var emp = Employee.EditInTypeTrims(Id);
            return View(emp);
        }
        [HttpPost]
        public ActionResult EditOfSalaryMounth(EmployeeSalaryVM empVM)
        {

            var result = Employee.EditInTypeTrimsPost(empVM);
            return RedirectToAction("makManualGL");

        }
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {


                var user = await _userManager.FindByIdAsync(id);
                var logins = user.Logins;
                var rolesForUser = await _userManager.GetRolesAsync(id);


                foreach (var login in logins.ToList())
                {
                    await _userManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var result = await _userManager.RemoveFromRoleAsync(user.Id, item);
                    }
                }

                await _userManager.DeleteAsync(user);



                return View();
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public ActionResult RestEmployeePassword(string Userid)
        {

            var RegID = Employee.GetEmployeeRegIDID(Userid);
            EmployeeResetPasswordVM EMVM = new EmployeeResetPasswordVM();
            EMVM.EmployeeID = RegID.Emp_Id;
            EMVM.UserID = RegID.Reg_ID;
            return View(EMVM);

        }
        [HttpPost]
        public ActionResult RestEmployeePassword(EmployeeResetPasswordVM EMVM)
        {
            try
            {

                UserManager<IdentityUser> userManager =
            new UserManager<IdentityUser>(new UserStore<IdentityUser>());
                var RegID = Employee.GetEmployeeRegIDID(EMVM.UserID);
                userManager.RemovePassword(RegID.Reg_ID);

                userManager.AddPassword(RegID.Reg_ID, EMVM.Password);
                var msg = Resources.validate.Error;
                return RedirectToAction("RoleList", new { msg = msg });
            }
            catch(Exception ex)
            {
                var msg =ex.Message;
                return RedirectToAction("RoleList", new { msg = msg });
            }

        }
        public ActionResult RmoveAccount(int id)
        {

            ApplicationUser usero = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(usero.Id, "حذف صلاحية موظف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var Emp = Employee.removeUserAccount(id);
            UserManager<IdentityUser> userManager =
       new UserManager<IdentityUser>(new UserStore<IdentityUser>());
          var user=  userManager.FindById(Emp);
           var a= userManager.DeleteAsync(user);
            object msg = Accountingsystem.Resources.validate.RemoveAutherize;
            return RedirectToAction("Index", new { msg = msg });


        }
       

    }
}
