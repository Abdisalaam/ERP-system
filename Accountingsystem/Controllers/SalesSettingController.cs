using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class SalesSettingController : Controller
    {
        public ActionResult GetAllSalesSetting()
        {
            return View();
        }
        // GET: SalesSetting
        public ActionResult IndexOFNotesAndDesc()
        {
            var model = SalesSetting.getAllNotesSubledger();
            return View(model);
        }

        // GET: SalesSetting/Details/5
        public ActionResult DetailsOFNotesAndDesc(int id)
        {
            var model = SalesSetting.getSubledgerById(id);
            return View(model);
        }

        // GET: SalesSetting/Create
        public ActionResult CreateOFNotesAndDesc()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "وصف وملاحظات للطباعة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = SalesSetting.prepareSubledger();
            return View(model);
        }

        // POST: SalesSetting/Create

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateOFNotesAndDesc(SubLedgerVM vm)
        {
            try
            {
                // TODO: Add insert logic here
                var model = SalesSetting.CreateNotesForSubledger(vm);
                return RedirectToAction("DetailsOFNotesAndDesc", new { id = model.SubLed_Id });
            }
            catch
            {
                return View();
            }
        }

        // GET: SalesSetting/Edit/5
        public ActionResult EditOFNotesAndDesc(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء تصنيف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = SalesSetting.prepareSubledgerForEdit(id);
            return View(model);
        }

        // POST: SalesSetting/Edit/5
        [HttpPost, ValidateInput(false)]
        public ActionResult EditOFNotesAndDesc(SubLedgerVM vm)
        {
            try
            {
                // TODO: Add update logic here

                var model = SalesSetting.EditNotesForSubledger(vm);
                return RedirectToAction("DetailsOFNotesAndDesc", new { id = model.SubLed_Id });
            }
            catch
            {
                return View();
            }
        }

        // GET: SalesSetting/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SalesSetting/Delete/5
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
        public JsonResult GetNotesBySubledger(int SubledgerId)
        {
            var subledger = SalesSetting.getSubledgerByIdForJson(SubledgerId);
            var xx = new
            {
                subledger.InvoiceDesc,
                subledger.InvoiceNotes,
                subledger.SubLed_Type

            };
            return Json(xx, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNotesBySubledgerForOff(int SubledgerId)
        {
            var subledger = SalesSetting.getSubledgerByIdForJsonforOffer(SubledgerId);
            var xx = new
            {
                subledger.OfferDesc,
                subledger.OfferNotes,
              

            };
            return Json(xx, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditSettingWord()
        {
            
            var model = InvoiceWordPrintSetting.PrepareSetting();
            return View(model);
        }

        // POST: SalesSetting/Edit/5
        [HttpPost]
        public ActionResult EditSettingWord(InvoiceWordPrintSettingVM vm)
        {
            try
            {
                // TODO: Add update logic here

                var model = InvoiceWordPrintSetting.EditSetting(vm);
                return RedirectToAction("GetAllSalesSetting");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult EditTypeOfPrint(int TypeId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = false;
            if (TypeId == 1)
            {
                 employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تصاميم فواتير مبيعات PDF");
            }
            if (TypeId == 2)
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تصاميم فواتير مبيعات مبسطة PDF");
            }
            if (TypeId == 3)
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تصاميم عروض اسعار PDF");
            }

            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = InvoicePrintSetting.getAllInvoicePrintForCompanyClient(TypeId);
            return View(model);
        }

        // POST: SalesSetting/Edit/5
        [HttpPost]
        public ActionResult EditTypeOfPrint(string vm,int printselect)
        {
            try
            {
                // TODO: Add update logic here

                dynamic batchList = JsonConvert.DeserializeObject<dynamic>(vm);

                int msg = InvoicePrintSetting.PostNewSalesForStore(batchList, printselect);
                var fullUrl = this.Url.Action("GetAllSalesSetting", "SalesSetting", new { id = msg }, this.Request.Url.Scheme);
                return Json(fullUrl, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

    }
}
