using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Accountingsystem.Utilites;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class SubLedgerController : Controller
    {


        public JsonResult CheckCode(string SubLed_Code)
        {
            if (SubLedger.containCode(SubLed_Code))
            {
                var msg = "SUBLD- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: SubLedger
        public ActionResult Index(string msg)
        {
            var sub = SubLedger.getAllData();
           


            ViewBag.Message = msg;
            return View(sub);
        }
        public ActionResult IndexNotActive( String msg)
        {
            var models = SubLedger.GetAllNotActive();
            ViewBag.message = msg;
            return View(models);
        }
        // GET: SubLedger/Details/5
        public ActionResult Details(int id)
        {
            var sub = SubLedger.getDetail(id);
            return View(sub);
        }
        [AllowAnonymous]
        public JsonResult getName(int id)
        {
            var results = SubLedger.getName(id);
            return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: SubLedger/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء مركز تكلفة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var pack = PackageRecognize.checkValiditeOfSubLedger(user.Id);
            if (pack.Item2 == true)
            {
                var sub = SubLedger.fillForCreateSubLedger();
                return View(sub);
            }
            else
            {
                var msgo = Resources.validate.YourpackagedoesnotsupportthatNumberOfSub;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });

            }
        }

        // POST: SubLedger/Create
        [HttpPost]
        public ActionResult Create(SubLedgerVM subvm)
        {
            try
            {
                var sub = SubLedger.createSubLedgerManual(subvm);
                if (sub != null)
                {
                    return RedirectToAction("Details", new { id = sub.SubLed_Id });
                }
                else
                {
                    string msg = Resources.validate.Error;
                    return RedirectToAction("Index", new { msg = msg });
                }


                
            }
            catch
            {
                return View();
            }
        }

        // GET: SubLedger/Edit/5
        public ActionResult Edit(int id)

        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل مركز تكلفة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var sub = SubLedger.fillDataForEditing(id);
            return View(sub);
        }

        // POST: SubLedger/Edit/5
        [HttpPost]
        public ActionResult Edit(SubLedgerVM subvm)
        {
            try
            {
                var sub = SubLedger.editTheData(subvm);
                if (sub != null)
                {
                    return RedirectToAction("Details", new { id = sub.SubLed_Id });
                }
                else
                {
                    string msg = Resources.validate.Error;
                    return RedirectToAction("Index", new { msg = msg });
                }

               
            }
            catch
            {
                return View();
            }
        }

        // GET: SubLedger/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubLedger/Delete/5
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



        /// <summary>
        /// Gets all subledgers in the form of Json array in the form specified by 
        /// Select2 (https://select2.org/data-sources/arrays) to populate 
        /// the drop-down list
        /// </summary>
        /// <returns> A Json array in the form specified by Select2  </returns>
        [HttpGet]
        public ActionResult getSubledgersJson()
        {
            
            List<SubLedger> subledgers = SubLedger.getAllData();

            List<dynamic> result = new List<dynamic>();
            foreach (var subledger in subledgers)
            {
                var data = new
                {
                    // the value of each select item
                    id = subledger.SubLed_Id,
                    // showing both code and name in the drop-down list 
                    text = subledger.SubLed_Name + "\t" + subledger.SubLed_Code
                };
                result.Add(data);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getSubledgersBalanceJson()
        {
            List<SubLedger> subledgers = SubLedger.getAllData().Where(a=>a.SubLedger_Balance > 0 ).ToList();
            List<dynamic> result = new List<dynamic>();
            foreach (var subledger in subledgers)
            {
                var data = new
                {
                    // the value of each select item
                    id = subledger.SubLed_Id,
                    // showing both code and name in the drop-down list 
                    text = subledger.SubLed_Name + "\t" + subledger.SubLed_Code
                };
                result.Add(data);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public FileResult ReturncustFile(int fileId)
        {

            var document = SubLedger.getFileById(fileId);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = document.FileName,
                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.ProImg, document.File_Type);
        }
        public ActionResult GetAllFiles(int subId)
        {
            var model = SubLedger.GetAllFiles(subId);
            return View(model);
        }
        // GET: SubLedger/CreateOpenBalance
        public ActionResult CreateOpenBalance()
        {
            var sub = SubLedger.fillForCreateSubLedger();
            return View(sub);
        }

        // POST: SubLedger/CreateOpenBalance
        [HttpPost]
        public ActionResult CreateOpenBalance(SubLedgerVM subvm)
        {
            try
            {
                var sub = SubLedger.createSubLedgerOpenBalance(subvm);



                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
        }

        public JsonResult CheckCodeOpen(string OpenBalance_Code)
        {
            if (SubLedger.containCodeOpen(OpenBalance_Code))
            {
                var msg = "OPS- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTreeJson()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "عرض شجرة مراكز التكلفة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var root = SubLedger.getDetail(1);
            if (root == null)
            {
                var msg = Resources.validate.ThereIsNoSubledgerToShow;
                return RedirectToAction("Index", new { msg = msg });
            }
            else
            {
                SubLedger.MakeCalculateOfSubLedgerForTree();
                JObject json = SubLedger.traverseChildren(root);
                JArray jArr = new JArray();
                jArr.Add(json);
                return View(jArr);
            }
        }

        [HttpGet]
        public ActionResult GetTree()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "عرض شجرة مراكز التكلفة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var root = SubLedger.getDetail(1);
            if (root == null)
            {
                var msg = Resources.validate.ThereIsNoSubledgerToShow;
                return Json(msg, JsonRequestBehavior.AllowGet);
               
            }
            else
            {
                SubLedger.MakeCalculateOfSubLedgerForTree();
                JObject json = SubLedger.traverseChildren(root);
                JArray jArr = new JArray();
                jArr.Add(json);
                return Json(JsonConvert.SerializeObject(jArr), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteTrans(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ت غير نشط لمركز تكلفة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = SubLedger.DeleteTransfer(id,0);
            return RedirectToAction("Details", new { id = id });
        }
        /// <summary>
        /// Gets all accounts in the form of Json array in the form specified by 
        /// Select2 (https://select2.org/data-sources/arrays) to populate 
        /// the drop-down list
        /// </summary>
        /// <returns> A Json array in the form specified by Select2  </returns>


    }
}
