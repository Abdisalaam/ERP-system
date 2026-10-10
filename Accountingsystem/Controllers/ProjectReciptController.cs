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
    public class ProjectReciptController : Controller
    {
        // GET: ProjectRecipt
        public ActionResult Index(string msg,int ?all)
        {
            ViewBag.msg = msg;
            var listofRec = ProjectRecipt.GetListOFRecipt();
            if (all == null)
            {
                listofRec.Reverse();
                listofRec = listofRec.Take(100).ToList();
            }
            return View(listofRec);
        }
        public JsonResult Checkdate(string Recipt_Date)
        {
            if (ProjectRecipt.Checkdate(Recipt_Date) == false)
            {
                var msg = "يجب ادخال تاريخ خلال السنة المالية الحالية";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: ProjectRecipt/Details/5
        public ActionResult Details(int id)
        {
            var thisrec = ProjectRecipt.getdetailofRecipt(id);
            return View(thisrec);
        }
        public FileResult ReturnRecFile(int fileId)
        {

            var document = ProjectRecipt.FindFileByID(fileId);
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
            var model = ProjectRecipt.GetAllFiles(RecId);
            return View(model);
        }
        // GET: ProjectRecipt/Create
        public ActionResult Create(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء سند قبض مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var thisrec = ProjectRecipt.projectReciptForCreation(id);
            return View(thisrec);
        }

        // POST: ProjectRecipt/Create
        [HttpPost]
        public ActionResult Create(ProjectReciptVM PRVM)
        {
            try
            {
                // TODO: Add insert logic here
                var thisrec = ProjectRecipt.SaveRecipte(PRVM);
                var msg = thisrec;
                return RedirectToAction("Details", new { id = thisrec });
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectRecipt/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل سند قبض مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var thisrec = ProjectRecipt.EditforCreation(id);
            return View();
        }

        // POST: ProjectRecipt/Edit/5
        [HttpPost]
        public ActionResult Edit(ProjectReciptVM PRVM)
        {
            try
            {
                // TODO: Add update logic here
                var thisrec = ProjectRecipt.EditProjectRecipt(PRVM);
                var msg = Resources.validate.Editsucc;
                return RedirectToAction("Index",new { msg=msg});
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectRecipt/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    var thisrec = ProjectRecipt.getdetailofRecipt(id);
        //    return View(thisrec);
        //}

        // POST: ProjectRecipt/Delete/5
      
        public ActionResult Delete(int id)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف سند قبض مشروع");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var thisrec = ProjectRecipt.Deletethisrec(id);
                var msg = Resources.validate.DeleteSuccessful;
                return RedirectToAction("Index",new { msg=msg});
            }
            catch
            {
                return View();
            }
        }
      
        public async System.Threading.Tasks.Task<ActionResult> VoidRecipt (int id)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد سند قبض مشروع");



                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }
                await Task.Delay(2000);
                var thisrec = ProjectRecipt.voidPorjectRecipt(id);
                var msg = thisrec;
                return RedirectToAction("Details", new { id = id });

            }
            catch
            {
                return View();
            }
        }
        public ActionResult DeleteActive(int id)
        {

            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف سند قبض مشروع معتمد");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var msg = ProjectRecipt.deletionactiveGL(id);
                return RedirectToAction("Index", new { msg = msg });
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

                return RedirectToAction("Index", new { Message = ViewBag.Message });
            }

        }
    }
}
