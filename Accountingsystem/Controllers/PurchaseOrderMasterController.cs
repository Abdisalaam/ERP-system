using Accountingsystem.Models;
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
    public class PurchaseOrderMasterController : Controller
    {
        public JsonResult updatePurchaseDependOnquantity(int detailId, decimal quantity, decimal masterDiscount)
        {


            var msg = PurchaseOrderMaster.changeSpecificValueinPOs(detailId, quantity, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePurchaseDependOnPrice(int detailId, decimal Price, decimal masterDiscount)
        {


            var msg = PurchaseOrderMaster.changeSpecificPrice(detailId, Price, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePurchaseDependOnTotalCost(int detailId, decimal TotalCost, decimal masterDiscount)
        {


            var msg = PurchaseOrderMaster.changeSpecificTotalCost(detailId, TotalCost, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePurchaseDependTax(int detailId, int Tax, decimal masterDiscount)
        {


            var msg = PurchaseOrderMaster.changeSpecificTax(detailId, Tax, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckCode(string Serial_Number)
        {
            if (PurchaseOrderMaster.containCode(Serial_Number))
            {
                var msg = "PORD- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: PurchaseOrderMaster
        public ActionResult Index(string msg,int? all)
        {

            var models = PurchaseOrderMaster.GetAllInvoices(all);
            ViewBag.Message = msg;
            return View(models);

        }
        public ActionResult IndexofPosted(string msg, int? all)
        {

            var models = PurchaseOrderMaster.GetAllInvoicesPosted(all);
            ViewBag.Message = msg;
            return View(models);

        }

        // GET: PurchaseOrderMaster/Details/5
        public ActionResult Details(int id,string msg)
        {
            var invoiceMaster = PurchaseOrderMaster.GetInvoiceDetailsById(id);
            ViewBag.msg = msg;
            return View(invoiceMaster);
        }
        [HttpGet]
        public ActionResult GetAllTempPurchaseInvoiceDetails(int tempPurchaseInvoiceMasterId)
        {
            var purchaseInvoiceDetailVm = PurchaseOrderMaster.GetAllTempInvoiceDetails(tempPurchaseInvoiceMasterId);
            return PartialView("PurchaseMasterDetailsList", purchaseInvoiceDetailVm);
        }
        public virtual JsonResult AddPurchaseInvoiceDetailsToMaster(PurchaseOrderDetailVM purchaseInvoiceMasterDetail)
        {
            string msg = "";
            try
            {
                msg = PurchaseOrderMaster.AddInvoiceDetailsTemp(purchaseInvoiceMasterDetail);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }


            //var partialViewDetailsList = PartialView("InvoiceMasterDetailsList", invoiceDetailVm);
            //var id = 0;
            //var tt = 6;
            return Json(msg, JsonRequestBehavior.AllowGet);
            // return PartialView("InvoiceMasterDetailsList", invoiceDetailVm);
        }
        public ActionResult CreatePurchaseOrd()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء امر شراء");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = PurchaseOrderMaster.AddNewForCreation();
            return View(model);
        }
        public ActionResult PreparePurchaseMasterDetails()
        {
            var purchaseMaster = PurchaseOrderMaster.PrepareInvoiceMasterDetail();
            return PartialView("PurchaseDetailCreation", purchaseMaster);
        }
        [HttpPost]
        public ActionResult CreatePurchaseOrd(PurchaseOrderMasterVM masterVM)
        {

            // TODO: Add insert logic here
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

              var a= PurchaseOrderMaster.AddNewInvoice(masterVM , user.Id);
                if (a.Item2 == 0)
                {
                    return RedirectToAction("Index",new { msg = a.Item1 });
                }
                else
                {
                    return RedirectToAction("Details", new { id = a.Item2 });
                }

                

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return View();

            }
        }
        public ActionResult GetAllFiles(int invoiceMasterId)
        {
            var model = PurchaseOrderMaster.GetAllFiles(invoiceMasterId);
            return View(model);
        }
        public FileResult ReturnInvoicetFile(int fileId)
        {

            var document = PurchaseOrderMaster.getFileById(fileId);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = document.FileName,
                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.FileContent, document.FileType);
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
        public ActionResult PartialDialog2()
        {
            var productVM = Product.CreateVMForCreation();
            ViewBag.Model = productVM;
            ViewBag.ProdID = productVM.Prod_ID;
            ViewBag.ProdCode = productVM.Prod_Code;
            return PartialView("ProductPartialView", productVM);
        }
        public ActionResult AddNewProudectPartial(ProductVM prodVM)
        {

            var result = Product.CreateProdect(prodVM);
            //ViewBag.res = result;
            //return PartialView("CustomerPartialView", ViewBag.res);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        [HttpGet]
        public ActionResult GetProudectDropdownList()
        {

            var Prod = PurchaseOrderMaster.GetProudectList();
            return new JsonResult { Data = Prod, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpGet]
        public ActionResult GetSupplierDropdownList()
        {
            var supplier = PurchaseOrderMaster.GetSupplierList();
            return new JsonResult { Data = supplier, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        //Edit in Purchase Order
        public ActionResult PrepareSalesDetails(int masterId)
        {
            var invoic = PurchaseOrderMaster.PreparpurchMasterDetail2(masterId);
            return PartialView("OrderDetailsrCreationEdit", invoic);

        }
        public ActionResult GetAllTempInvoiceDetailsEdit(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = PurchaseOrderMaster.GetAllTempPURCHInvoiceDetails2(tempInvoiceMasterId);
            return PartialView("OrderDetailsForEdit", invoiceDetailVm);
        }
        public ActionResult GetAllTempInvoiceDetailsresult(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = PurchaseOrderMaster.GetAllTempPurchInvoiceDetailsresult2(tempInvoiceMasterId);
            return PartialView("OrderDetailResultList", invoiceDetailVm);
        }
        public ActionResult EditForPending(int id,int isCopy)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = true;
            if (isCopy == 0)
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل امر شراء");
            }
            else
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "نسخ امر شراء");
            }
                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
            
                var editFAsst = PurchaseOrderMaster.fillForEditingInvoice(id,isCopy);
            return View(editFAsst);
        }
        [HttpPost]
        public ActionResult EditForPending(PurchaseOrderMasterVM masterVM)
        {
            if (masterVM.isCopy == 0)
            {
                var editFAsst = PurchaseOrderMaster.editinvoice(masterVM);
                if (editFAsst.Item2 == 0)
                {
                    return RedirectToAction("Index", new { msg = editFAsst.Item1 });
                }
                else
                {
                    return RedirectToAction("Details", new { id = editFAsst.Item2 });
                }
            }
            else
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var a = PurchaseOrderMaster.AddNewInvoice(masterVM, user.Id);
                if (a.Item2 == 0)
                {
                    return RedirectToAction("Index", new { msg = a.Item1 });
                }
                else
                {
                    return RedirectToAction("Details", new { id = a.Item2 });
                }
            }
        }
        [HttpGet]
        public ActionResult getDetailTtempFirst(int MasterID)
        {
            var supplier = PurchaseOrderMaster.GetOneListofDetailsFirst(MasterID);
            return PartialView("OrderDetailResultList", supplier);
        }
        [HttpGet]
        public ActionResult getDetailTtempsecond(int MasterID)
        {
            var supplier = PurchaseOrderMaster.GetOneListofDetailssecond(MasterID);
            return PartialView("OrderDetailResultList", supplier);
        }
        //[HttpPost]
        //public ActionResult DeleteTempDetail(int DetailID,int MasterID)
        //{
        //    var suppliers = PurchaseOrderMaster.DeleteFromORderTemp(DetailID,MasterID);
        //    var supplier = PurchaseOrderMaster.GetOneListofDetailssecond(MasterID);
        //    return PartialView("OrderDetailResultList", supplier);
        //}
        public virtual JsonResult DeleteTempDetail(int DetailID, int MasterID)
        {
            string msg = "";
            try
            {
                msg = PurchaseOrderMaster.DeleteFromORderTemp(DetailID, MasterID);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }


            //var partialViewDetailsList = PartialView("InvoiceMasterDetailsList", invoiceDetailVm);
            //var id = 0;
            //var tt = 6;
            return Json(msg, JsonRequestBehavior.AllowGet);
            // return PartialView("InvoiceMasterDetailsList", invoiceDetailVm);
        }
        [HttpGet]
        public virtual JsonResult DeleteTempDetail2(int iD)
        {
            string msg = "";
            try
            {
                msg = PurchaseOrderMaster.DeleteFromORderTemp2(iD);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }


            //var partialViewDetailsList = PartialView("InvoiceMasterDetailsList", invoiceDetailVm);
            //var id = 0;
            //var tt = 6;
            return Json(msg, JsonRequestBehavior.AllowGet);
            // return PartialView("InvoiceMasterDetailsList", invoiceDetailVm);
        }

        public ActionResult deleteFile(int fileId)
        {

            var document = PurchaseOrderMaster.getFileById(fileId);
            var delete = PurchaseOrderMaster.deleteFile(fileId);
            return RedirectToAction("Details", new { id = document.PurchaseOrderMasterMaster_Id });
        }


        // POST: Currency/Delete/5

        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف امر شراء");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                int purchaseOrder = PurchaseOrderMaster.DeletePurchaseOrder(id);
                if (purchaseOrder == 0)
                {
                    var msg = Resources.validate.Error; 
                    return RedirectToAction("Index", new { msg = msg });
                }
                else
                {
                    var msg = Resources.validate.DeleteSuccessful;
                    return RedirectToAction("Index", new { msg = msg });
                }
            }
            catch
            {
                return View();
            }
        }

        public JsonResult updateDataDebendOnDiscount(decimal discount, int id)
        {
            var result = PurchaseOrderMaster.changeDetailOfInvoice(discount, id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}
