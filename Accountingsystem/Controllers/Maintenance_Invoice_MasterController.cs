using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class Maintenance_Invoice_MasterController : Controller
    {
        public JsonResult CheckCode(string Main_Invoice_Code)
        {
            if (Maintenance_Invoice_Master.containCode(Main_Invoice_Code))
            {
                var msg = "MIV- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: Maintenance_Invoice_Master
        public ActionResult Index()
        {

            var models = Maintenance_Invoice_Master.GetAllInvoices();
            return View(models);
        }

        // GET: Maintenance_Invoice_Master/Details/5
        public ActionResult Details(int id)
        {
            var invoiceMaster = Maintenance_Invoice_Master.GetInvoiceDetailsById(id);
            return View(invoiceMaster);
        }
        public ActionResult GetAllTempPurchaseInvoiceDetails(int tempPurchaseInvoiceMasterId)
        {
            var purchaseInvoiceDetailVm = Maintenance_Invoice_Master.GetAllTempInvoiceDetails(tempPurchaseInvoiceMasterId);
            return PartialView("MasterDetailsList", purchaseInvoiceDetailVm);
        }
        public ActionResult PreparePurchaseMasterDetails()
        {
            var purchaseMaster = Maintenance_Invoice_Master.prepareInvoiceDetail();
            return PartialView("DetailCreationVM", purchaseMaster);
        }
        public virtual JsonResult AddPurchaseInvoiceDetailsToMaster(Maintenance_Invoice_DetailVM InvoiceMasterDetail)
        {
            string msg = "";
            try
            {
                msg = Maintenance_Invoice_Master.addDetailToMaster(InvoiceMasterDetail);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(int id)
        {
            var model = Maintenance_Invoice_Master.fillVMForCreation(id);
            return View(model);
        }

        // POST: Maintenance_Invoice_Master/Create
        [HttpPost]
        public ActionResult Create(Maintenance_Invoice_MasterVM masterVM)
        {
            try
            {
                Maintenance_Invoice_Master.addNewInvoice(masterVM);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Maintenance_Invoice_Master/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Maintenance_Invoice_Master/Edit/5
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

        // GET: Maintenance_Invoice_Master/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Maintenance_Invoice_Master/Delete/5
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
        public ActionResult getDetailFromOrder(int Order)
        {
            var orderdetail = Maintenance_Invoice_Master.getAllSubscription(Order);
            return PartialView("getDetailFromOrder", orderdetail);
        }
    }
}
