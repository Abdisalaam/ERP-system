using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Accountingsystem.Utilites;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        public JsonResult CheckCode(string Project_Code)
        {
            if (Project.containCode(Project_Code))
            {
                var msg = "PRO- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: Project
        public ActionResult Index(string msg,int ?all)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfPoject(user.Id);
            if (pack.Item2 == true)
            {
                var proj = Project.getAllDataWorking();
                ViewBag.Message = msg;
                if (all == null)
                {
                    proj.Reverse();
                    proj = proj.Take(100).ToList();
                }
                return View(proj);
            }
            else
            {
                var msgo = Resources.validate.YourpackagedoesnotsupporttheProdepartment;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }
        public ActionResult IndexFinshed(string msg,int ?all)
        {
            var proj = Project.getAllDataFinshed();
            if (all == null)
            {
                proj.Reverse();
                proj = proj.Take(100).ToList();
            }
            ViewBag.Message = msg;
            return View(proj);
        }
        public ActionResult IndexNotWorking(string msg,int ?all)
        {
            var proj = Project.getAllDataNotWokring();
            if (all == null)
            {
                proj.Reverse();
                proj = proj.Take(100).ToList();
            }
            ViewBag.Message = msg;
            return View(proj);
        }
        // GET: Project/Details/5
        public ActionResult Details(int id)
        {
            var proj = Project.getDetail(id);
            return View(proj);
        }

        public ActionResult PrepareDetail()
        {
            var DetailVM = Project.CreateDetailOfProjectVM();
            return PartialView(DetailVM);

        }
        public JsonResult AddToListDetail(ProjectDetailVM ProVM)
        {
            var message = Project.AddToList(ProVM);

            return Json(message, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ListTempDetail(int master_ID)
        {
            var projectDetailTemps = Project.GetDataTempForCreation(master_ID);
            return PartialView(projectDetailTemps);

        }
        public ActionResult ListTempDetailForEdit(int master_ID)
        {
            var projectDetailTemps = Project.GetAllTempPRojectForEdit(master_ID);
            return PartialView(projectDetailTemps);

        }
        public JsonResult DeleteTemp(int ids)
        {
            var PaymentVM = Project.DeleteFromTemp(ids);

            return Json(PaymentVM, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var prvm = Project.fillDataForCreate();

            return View(prvm);
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(ProjectVM prvm)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var proj = Project.createProject(prvm, user.Id);
                if (proj.Item2 == 0)
                {
                    return RedirectToAction("Index", new { msg = proj.Item1 });
                }
                else
                {
                    return RedirectToAction("Details", new { id = proj.Item2 });
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return View();
            }
        }






        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var proj = Project.fillDataForEditing(id);
            return View(proj);
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(ProjectVM prvm)
        {
            try
            {
                var proj = Project.editTheData(prvm);
                if (proj.Project_Id == 0)
                {
                    return RedirectToAction("Index", new { msg = "يوجد خطأ بالتعديل" });
                }
                else
                {
                    return RedirectToAction("Details", new { id = proj.Project_Id });
                }
            }



            catch
            {
                return View();
            }
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف فاتورة مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = Project.DeleteProjectInvoiceMaster(id);
            return RedirectToAction("IndexOfInvoice", new { msg = msg });
        }

        public FileResult ReturnproFile(int fileId)
        {

            var document = Project.getFileById(fileId);
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

        public ActionResult GetAllFiles(int projId)
        {
            var model = Project.GetAllFiles(projId);
            return View(model);
        }



        [HttpGet]
        public ActionResult PartialDialog()
        {
            var custvm = Customer.AddCustomerVM();
            return PartialView("CSTprojectPartialView", custvm);
        }

        [HttpPost]
        public ActionResult AddNewCustomerPartial(CustomerVM masterVM)
        {

            HttpFileCollectionBase files = Request.Files;
            if (files.Count > 0)
            {
                masterVM.customerFiles = files;
            }
            var result = Customer.AddNewCustomer(masterVM);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpGet]
        public ActionResult GetCustomerDropdownList()
        {
            var customer = Project.GetCustomerList();
            return new JsonResult { Data = customer, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult PartialDialog2()
        {
            var custvm = PaymentMethod.getListForCreation();
            return PartialView("PaymentPartialView", custvm);
        }


        [HttpPost]
        public ActionResult AddNewPaymentPartial(PaymentMethodVM masterVM)
        {

            var result = PaymentMethod.createNewPaymentMethod(masterVM);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        [HttpGet]
        public ActionResult GetMthodDropdownList()
        {
            var customer = Project.GetPaymentList();
            return PartialView("MthodtDropDownList", customer);
        }
        //////start  the  Invice  of  the  Project  From Here //////

        public ActionResult IndexOfInvoice(String msg,int ?all)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfPoject(user.Id);
            if (pack.Item2 == true)
            {
                var pp = Project.GetAllInvoices();
                if (all == null)
                {
                    pp.Reverse();
                    pp = pp.Take(100).ToList();
                }
                ViewBag.Message = msg;
                return View(pp);
            }
            else
            {
                var msgo = Resources.validate.YourpackagedoesnotsupporttheProdepartment;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }

        public ActionResult CreateProjectInvs(int project_Id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء فاتورة مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pp = Project.fillDataForCreateinvoice(project_Id,user.Id);

            return View(pp);
        }
        [HttpPost]
        public ActionResult CreateProjectInvs(ProjectInvoiceMasterVM PVM)
        {
            string msg = "";
            try
            {
                var a = Project.SaveAllData(PVM);

                return RedirectToAction("DetailsofInvoice", "Project", new { id = a });

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }



        }

        [HttpGet]
        public JsonResult GetPurchInvoiceJson(int proid)
        {
            var pro = Project.getDetail(proid);
            //var db = new AccountingContainer();
            //var curr = db.Customers.Find(pro.CustomerCustomer_Id);
            //var deadline = pro.DeadLine.ToString();
            //var starttime = pro.StartTime.ToString();
            //    var paM = db.PaymentMethods.Find(pro.Payment_Method);
            // var xx = new
            //{
            //    pro.Project_Code,
            //    pro.Project_Name,
            //    curr.Customer_Name,
            //    pro.Project_Description,
            //    pro.Project_Location,
            //     deadline,
            //     pro.StartTime,
            //     pro.Estimate_Amount,
            //    pro.CustomerCustomer_Id,
            //    pro.Emp_Id,
            //     starttime,
            //    pro.Payment_Method,
            //    pro.Revenu_Id,
            //    pro.Project_State,
            //    paM.Payment_Type,
            // };
            var xx = Project.GetPurchInvoiceJsonModel(pro);
            return Json(xx, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetPartialForDetail()
        {
            var DetailVm = Project.getPartialforDetail();
            return PartialView("GetPartialForDetail", DetailVm);
        }
        [HttpPost]
        public JsonResult AddPartialDatatoTemp(ProjectInvoiceDetailsVM PVM)
        {
            string msg = "";
            try
            {
                msg = Project.addDetailstoTemp(PVM);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public JsonResult updateDataDebendOnDiscount(decimal discount, int id)
        {
            var result = Project.changeDetailOfInvoice(discount, id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updateDataDebendOnAlowDiscount(decimal discount, int id)
        {
            var result = Project.changeDetailOfInvoice(discount, id);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult GetListOfDetailTemp(int MasterID)
        {
            var DetailVm = Project.getListofDetailTemp(MasterID);
            return PartialView("GetListOfDetailTemp", DetailVm);
        }
        [HttpGet]
        public ActionResult calculatealltotals(decimal fixeddiscount, int masterid)
        {
            var DetailVm = Project.calculatealltotals(fixeddiscount, masterid);
            return PartialView("GetListOfDetailTemp", DetailVm);
        }

        public ActionResult DetailsofInvoice(int id)
        {
            var invoiceMaster = Project.GetProjectInvoice(id);
            return View(invoiceMaster);
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> EditInvoice(ProjectInvoiceMasterVM invoiceMasterVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد فاتورة مشروع");



                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }
                await Task.Delay(2000);
                var inv = Project.GetProjectInvoice(invoiceMasterVM.PInvoiceMaster_ID);


                string result = Project.UpdateProjectInvoiceMaster(invoiceMasterVM);




                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return View();
            }
        }

        public virtual JsonResult GetDepartments(int departId)
        {
            var my = Project.GetDepartment(departId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetBussiness(int bussinId)
        {
            var my = Project.GetBussines(bussinId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteTempDetail(int iD)
        {
            var temp = Project.DeleteInvoiceFromTemp(iD);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditInvoicebeforPost(int id)
        {
            try
            {

                var inv = Project.EditProjInvBeforPostforCreation(id);
                return View(inv);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public ActionResult EditInvoicebeforPost(ProjectInvoiceMasterVM PVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل فاتورة مشروع");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var a = Project.EditDataofInv(PVM);

                return RedirectToAction("DetailsofInvoice", "Project", new { id = a });

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return View(PVM);
            }
        }
        [HttpGet]
        public ActionResult GetListOfDetailTempfirst(int masterID)
        {
            var DetailVm = Project.getListofDetailTempfirstLoad(masterID);
            return PartialView("GetListOfDetailTemp", DetailVm);
        }
        [HttpGet]
        public ActionResult GetListOfDetailTempsec(int masterID)
        {
            var DetailVm = Project.getListofDetailTempsecondLoad(masterID);
            return PartialView("GetListOfDetailTemp", DetailVm);
        }
        [HttpPost]
        public JsonResult AddPartialDatatoTempEdit(ProjectInvoiceDetailsVM PVM)
        {
            string msg = "";
            try
            {
                msg = Project.addDetailstoTempEdit(PVM);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);

        }

       
        //public JsonResult EditProjectInvs(ProjectInvoiceMasterVM PVM)
        //{
        //    string msg = "";
        //    try
        //    {
        //        msg = Project.EditDataofInv(PVM);

        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ex.Message;
        //        throw;
        //    }

        //    return Json(msg, JsonRequestBehavior.AllowGet);

        //}

    }
}