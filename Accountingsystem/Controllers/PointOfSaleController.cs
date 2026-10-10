using Accountingsystem.Models;
using Accountingsystem.Utilites;
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
    public class PointOfSaleController : Controller
    {
        // GET: PointOfSale
        public ActionResult Index(string Message)
        {
            try
            {

                var PointList = PointOfSale.GetAllDataForPointOfSale();
                ViewBag.Message = Message;
                return View(PointList);
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


                return View();
            }
        }

        // GET: PointOfSale/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var Point = PointOfSale.GetDateById(id);
                return View(Point);
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


                return View();
            }
        }
        public ActionResult PreparePayment()
        {
            var PaymentVM = PointOfSale.CreatePOSMethod();
            return PartialView(PaymentVM);

        }
        public JsonResult AddPayemnt(POSDetailPaymentVM PayVM )
        {
            var PaymentVM = PointOfSale.AddToList(PayVM);
        
            return Json(PaymentVM, JsonRequestBehavior.AllowGet);

        }
        public ActionResult TempPayment(int POSTemp) {
            var PaymentDetail = PointOfSale.GetDataTempForCreation(POSTemp);
            return PartialView(PaymentDetail);

        }

        // GET: PointOfSale/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء نقطة مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
           // ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfPointOfSale(user.Id);
            if (pack.Item2 == true)
            {
                var Point = PointOfSale.CreatePOS();
                return View(Point);
            }

            else
            {
                var Message =Resources.validate.YourpackagedoesnotsupportthatNumberOfPOS;
                return RedirectToAction("Packagevalidate", "Reports", new { Message = Message });
            }
        }

        // POST: PointOfSale/Create
        [HttpPost]
        public ActionResult Create(PointOfSaleVM POSVM)
        {
            try
            {
                var PoS = PointOfSale.CreatePOS(POSVM);

                return RedirectToAction("Index",new {Message= PoS });
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


                return View();
            }
        }

        // GET: PointOfSale/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل نقطة مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var PoSVM = PointOfSale.EditVM(id);
            return View(PoSVM);
        }

        // POST: PointOfSale/Edit/5
        [HttpPost]
        public ActionResult Edit(PointOfSaleVM POSVM)
        {
            try
            {
                // TODO: Add update logic here
                var PoSVM = PointOfSale.EditPost(POSVM);
                return RedirectToAction("Index", new { Message = PoSVM });
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


                return View();
            }
        }

        // GET: PointOfSale/Delete/5
        public ActionResult Delete(int id)
        {
            var PointList = PointOfSale.GetDateById(id);
            return View(PointList);
        }

        // POST: PointOfSale/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var PointList = PointOfSale.DeletePOS(id);
                return RedirectToAction("Index",new { Message = PointList });
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


                return View();
            }
        }
        public ActionResult ALLTempForEdit(int POSTemp)
        {
            var PaymentDetail = PointOfSale.GetAllTempPointForEdit(POSTemp);
            return PartialView(PaymentDetail);

        }
        public JsonResult DeleteTemp(int ids)
        {
            var PaymentVM = PointOfSale.DeleteFromTemp(ids);

            return Json(PaymentVM, JsonRequestBehavior.AllowGet);

        }
        public static bool checkValidate()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var a = PointOfSale.checkValidateOfSaler(user.Id);
            return a;

        }

    }
}
