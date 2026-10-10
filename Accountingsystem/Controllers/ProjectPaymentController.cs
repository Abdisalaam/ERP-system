using Accountingsystem.Models;
using Accountingsystem.Utilites;
using Accountingsystem.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class ProjectPaymentController : Controller
    {

        public JsonResult CheckCode(string ProjectPay_Code)
        {
            if (ProjectPayment.containCode(ProjectPay_Code))
            {
                var msg = "PROPAY- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: ProjectPayment
        public JsonResult Checkdate(string Date)
        {
            if (ProjectPayment.Checkdate(Date) == false)
            {
                var msg = "يجب ادخال تاريخ خلال السنة المالية الحالية";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index(string mesg,int ?all)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfPoject(user.Id);
            if (pack.Item2 == true)
            {
                ViewBag.Message = mesg;

                var models = ProjectPayment.GetAllProjectPayment();
                if (all == null)
                {
                    models.Reverse();
                    models = models.Take(100).ToList();
                }
                return View(models);
            }
            else
            {
                var msgo = Resources.validate.YourpackagedoesnotsupporttheProdepartment;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }

        // GET: ProjectPayment/Details/5
        public ActionResult Details(int id)
        {
            var model = ProjectPayment.GetProjectPaymentById(id);
            return View(model);
        }


        public FileResult ReturnRecFile(int fileId)
        {

            var document = ProjectPayment.FindFileByID(fileId);
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
        public ActionResult GetAllFiles(int RecId)
        {
            var model = ProjectPayment.GetAllFiles(RecId);
            return View(model);
        }
        // GET: ProjectPayment/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء سند صرف مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ProjectPayment.AddProjectPaymentVM();
            return View(model);
        }

        // POST: ProjectPayment/Create
        [HttpPost]
        public ActionResult Create(ProjectPaymentVM projectPaymentVM)
        {
            try
            {
                // TODO: Add insert logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var model = ProjectPayment.AddNewProjectPayment(projectPaymentVM , user.Id);
                if (model == 0 || model == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Details",new {id= model });
                }
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                return View();
            }
        }

        // GET: ProjectPayment/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل سند صرف مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ProjectPayment.FillForEditPayment(id);
            return View(model);
        }

        // POST: ProjectPayment/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ProjectPaymentVM projectPaymentVM)
        {
            try
            {
                // TODO: Add update logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var model = ProjectPayment.EditProjectPayment(projectPaymentVM , user.Id);
                if (model == 0 || model == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Details", new { id = model });
                }
            }
            catch
            {
                return View();
            }
        }



        public async System.Threading.Tasks.Task<ActionResult> UpdateGL(ProjectPaymentVM projectPayVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد سند صرف مشروع");



                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }
                // TODO: Add update logic here
                await Task.Delay(2000);
                string result = ProjectPayment.UpdateProjectPayment(projectPayVM);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف سند صرف مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            ProjectPayment.DeleteProjectPayment(id);
            string mesg = Resources.validate.DeleteSuccessful;
            return RedirectToAction("Index", new { mesg = mesg });
        }
        public ActionResult DeleteActive(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف سند صرف مشروع معتمد");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            try
            {
                var msg = ProjectPayment.deletionactiveGL(id);
                return RedirectToAction("Index", new { mesg = msg });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
                if (cookie != null && cookie.Value != null)
                {
                    if (cookie.Value == "en")
                    {
                        ViewBag.Message = ex.Message + "___" + "Please review this error and try to correct it";
                    }
                    else
                    {
                        ViewBag.Message = ex.Message + "__" + "هناك خطأ في البيانات. الرجاء التاكد من صحتها";
                    }
                }
                else
                {

                    ViewBag.Message = ex.Message + "__" + "هناك خطأ في البيانات. الرجاء التاكد من صحتها";
                }

                return RedirectToAction("Index", new { mesg = ViewBag.Message });
            }

        }


        public virtual JsonResult GetDepartments(int departId)
        {
            var my = ProjectPayment.GetDepartment(departId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetBussiness(int bussinId)
        {
            var my = ProjectPayment.GetBussines(bussinId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PartialDialog()
        {
            var supplier = Supplier.AddSupplierVM();
            return PartialView("SupplierPartialView", supplier);
        }
        //  AddNewSupplierPartial
        //SupplierPartialView
        public ActionResult AddNewSupplierPartial(SupplierVM masterVM)
        {
            HttpFileCollectionBase files = Request.Files;
            if (files.Count > 0)
            {
                masterVM.SuppFiles = files;
            }
            var result = Supplier.AddNewSupplier(masterVM);

            return new JsonResult { Data = result.Item2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        [HttpGet]
        public ActionResult GetSupplierDropdownList()
        {
            var supplier = PurInvoiceMaster.GetSupplierList();
            return new JsonResult { Data = supplier, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public virtual JsonResult GetSubledger(int projectId)
        {
            var my = ProjectPayment.GetSubledger(projectId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetClause(int projectId)
        {
            var my = ProjectPayment.GetClause(projectId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult GetSubsubledger(int subledgerid)
        {
            var my = ProjectPayment.GetSubsubledger(subledgerid);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
    }
}
