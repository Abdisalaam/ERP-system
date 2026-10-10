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
    [Authorize]
    public class Subscription_TypeController : Controller
    {
        public JsonResult CheckCode(string Subscrip_Code)
        {
            if (Subscription_Type.containCode(Subscrip_Code))
            {
                var msg = "SUBT- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: Subscription_Type
        public ActionResult Index(string Msg)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfSubscribtion(user.Id);
            if (pack.Item2 == true)
            {
                ViewBag.Msg = Msg;
                var Type = Subscription_Type.GetAllSubscription_Type();
                return View(Type);
            }
            else
            {
                var msgo = "باقتك لا تدعم قسم الاشتركات";
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }

        // GET: Subscription_Type/Details/5
        public ActionResult Details(int id)
        {
            var SUbb = Subscription_Type.GetallSubscription_TypeById(id);
            return View(SUbb);
        }

        // GET: Subscription_Type/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء نوع اشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var sub = Subscription_Type.CreateforCreation();
            return View(sub);
        }

        // POST: Subscription_Type/Create
        [HttpPost]
        public ActionResult Create(Subscription_TypeVM subscVM)
        {
            try
            {
                // TODO: Add insert logic here
                var subb = Subscription_Type.CreateNewSubscription_Type(subscVM);
                string msg = "Subscription Add Successfuly";
                return RedirectToAction("Details", new  { id = subb.Subscription_Type_ID });
            }
            catch
            {
                return View();
            }
        }

        // GET: Subscription_Type/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل نوع اشتراك");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                if (id < 1)
                { 
                string Msg = "error id is empty";
                return RedirectToAction("Index", new { Msg = Msg });
                }
           
             var subscrType = Subscription_Type.FillSubscription_TypeVM(id);
            return View(subscrType);
            }
            catch (Exception w)
            {
                var s = w.Message;
                var n = w.Data;
                return View();
            }
        }
        

        // POST: Subscription_Type/Edit/5
        [HttpPost]
        public ActionResult Edit(Subscription_TypeVM TYpeeeVM)
        {
            try
            {
                // TODO: Add update logic here
                var type = Subscription_Type.EditSubscriptionType(TYpeeeVM);
                return RedirectToAction("Details", new { id = type.Subscription_Type_ID });
            }
            catch(Exception x)
            {
                var s = x.Message;
                return View();
            }
        }

        // GET: Subscription_Type/Delete/5
        public ActionResult Delete(int id)
        {
            var sutype = Subscription_Type.GetSuptypetById(id);
            return View(sutype);
        }

        // POST: Subscription_Type/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var susc = Subscription_Type.DeleteSubscriptionType(id);
                    return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }
    }
}
