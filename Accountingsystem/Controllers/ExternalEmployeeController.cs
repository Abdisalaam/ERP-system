using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.ViewModel;
using Accountingsystem.Models;
using Accountingsystem.Utilites;

namespace Accountingsystem.Controllers
{
    public class ExternalEmployeeController : Controller
    {
        // GET: ExternalEmployee
        public ActionResult Index()
        {
            return View();
        }

        // GET: ExternalEmployee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult EmailHasbeenSended()
        {
            return View();
        }
        public ActionResult ThankYouPage()
        {
            return View();
        }
        // GET: ExternalEmployee/Create
        public ActionResult Create()
        {
            var model = new ExtrnalSalesEmployeeVM();
            return View(model);
        }
        [HttpGet]
        public ActionResult GetExternalEmployeeToCompleetRegistration(int id)
        {
            var model = ExtrnalSalesEmployee.PrepareVMToCompleetRegistration(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult GetExternalEmployeeToCompleetRegistration(ExtrnalSalesEmployeeVM extrnalSalesEmployeeVM)
        {
            if (extrnalSalesEmployeeVM.SelectedParentID > 1)
            {
                if (!ExtrnalSalesEmployee.CheckIfParentPromoCodeExist(extrnalSalesEmployeeVM.ParentPromoCode))
                {
                    ViewBag.Message = Resources.ExternalEmployee.ParentDosntExist;
                    return View(extrnalSalesEmployeeVM);
                }
            }
           
            var model = ExtrnalSalesEmployee.CompleteEmployeeRegistration(extrnalSalesEmployeeVM);
            if (model != null)
            {
                HttpCookie cookie = Request.Cookies["Language"];

                if (cookie != null && cookie.Value != null)

                {
                    if (cookie.Value == "en")
                    {
                        MailManager.SendEmail(model.Email, "Registration is completed", "<div><h4>" + model.Name + "</h4></div>" + "<div><h4>" + model.Name + "</h4></div>" + "<div><h4>Thank you for your registration with us</h4></div>");
                    }
                    else
                    {
                        MailManager.SendEmail(model.Email, "التسجيل مكتمل", "<div><h4>" + model.Name + "</h4></div>" + "<div><h4>شكرا لتسجيلك معنا</h4></div>");
                    }

                }
                else
                {
                    MailManager.SendEmail(model.Email, "التسجيل مكتمل", "<div><h4>" + model.Name + "</h4></div>" + "<div><h4>شكرا لتسجيلك معنا</h4></div>");
                }
                return RedirectToAction("ThankYouPage");
            }
            return View(extrnalSalesEmployeeVM);
        }
        // POST: ExternalEmployee/Create
        [HttpPost]
        public ActionResult Create(ExtrnalSalesEmployeeVM extrnalSalesEmployeeVM)
        {
            try
            {
                if (ExtrnalSalesEmployee.CheckIfEmailExist(extrnalSalesEmployeeVM.Email))
                {
                    ViewBag.Message = Resources.ExternalEmployee.EmailExistMessage;
                    return View(extrnalSalesEmployeeVM);
                }
                if (ExtrnalSalesEmployee.CheckIfPromoCodeExist(extrnalSalesEmployeeVM.PromoCode))
                {
                    ViewBag.Message = Resources.ExternalEmployee.PromoCodeExist;
                    return View(extrnalSalesEmployeeVM);
                }
                var model = ExtrnalSalesEmployee.RegisterFirstFase(extrnalSalesEmployeeVM);
                if (model != null)
                {
                    var callbackUrl = Url.Action("GetExternalEmployeeToCompleetRegistration", "ExternalEmployee", new { id = model.Id }, protocol: Request.Url.Scheme);
                    HttpCookie cookie = Request.Cookies["Language"];

                    if (cookie != null && cookie.Value != null)

                    {
                        if (cookie.Value == "en")
                        {
                            MailManager.SendEmail(model.Email, "confirm your email", "<div><h4>" + model.Name + "</h4></div>" + "<div>Please click <a href=\"" + callbackUrl + "\">Here</a> to activate your email</div>");
                        }
                        else
                        {
                            MailManager.SendEmail(model.Email, "أكد بريدك الإلكتروني", "<div> الرجاء.. لتأكيد بريدك الإلكتروني اظغط <a href=\"" + callbackUrl + "\">هنا</a></div>");
                        }

                    }
                    else
                    {
                        MailManager.SendEmail(model.Email, "أكد بريدك الإلكتروني", "<div><h4>" + model.Name + "</h4></div>" + "<div> الرجاء.. لتأكيد بريدك الإلكتروني اظغط <a href=\"" + callbackUrl + "\">هنا</a></div>");
                    }
                    return RedirectToAction("EmailHasbeenSended");
                }
                return View(extrnalSalesEmployeeVM);
            }
            catch
            {
                return View(extrnalSalesEmployeeVM);
            }
        }

        // GET: ExternalEmployee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ExternalEmployee/Edit/5
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

        // GET: ExternalEmployee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ExternalEmployee/Delete/5
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
