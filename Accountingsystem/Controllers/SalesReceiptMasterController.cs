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
    [Authorize]
    public class SalesReceiptMasterController : Controller
    {
        
      
        public JsonResult CheckCode(string Serial_Number)
        {
            if (SalesReceiptMaster.containCode(Serial_Number))
            {
                var msg = Resources.InvoiceMasterAddNewInvoice.OfferCodeExisit;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependOnquantity(int detailId, decimal quantity, decimal masterDiscount)
        {


            var msg = SalesReceiptMaster.changeSpecificValueinPOs(detailId, quantity, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependOnPrice(int detailId, decimal Price, decimal masterDiscount)
        {


            var msg = SalesReceiptMaster.changeSpecificPrice(detailId, Price, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllSalesDetailsToPrintNewWindow(int id)
        {
            var Details = SalesReceiptMaster.GetpurchInvByID(id);
            return View(Details);
        }
        public JsonResult updatePOSDependOnDetailDiscount(int detailId,  decimal detaildiscount)
        {


            var msg = SalesReceiptMaster.changeSpecificDiscount(detailId, detaildiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependTax(int detailId, int Tax, decimal masterDiscount)
        {


            var msg = SalesReceiptMaster.changeSpecificTax(detailId, Tax, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependOnDetailDiscountAmount(int detailId, decimal detaildiscountAmount)
        {


            var msg = SalesReceiptMaster.changeSpecificDiscountAmount(detailId, detaildiscountAmount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependOnDetailTotalAmount(int detailId, decimal detailTotalAmount)
        {


            var msg = SalesReceiptMaster.changeSpecificTotalAmount(detailId, detailTotalAmount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductPartialDialogNoModel()
        {
            var productVM = Product.CreateVMForCreation();
            ViewBag.Model = productVM;
            ViewBag.ProdID = productVM.Prod_ID;
            ViewBag.ProdCode = productVM.Prod_Code;
            return PartialView("ProductPartialView", productVM);
        }

        public ActionResult AddNewProductPartial(ProductVM productVM)
        {
            HttpFileCollectionBase files = Request.Files;
            if (files.Count > 0)
            {
                productVM.Pro_Img = files;
            }
            var result = Product.CreateProdect(productVM);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult GetProductDrobDownList()
        {
            var customer = OfferPriceMaster.GetProductList();
            return new JsonResult { Data = customer, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Checkdate(string Invoice_Date)
        {
            if (InvoiceMaster.Checkdate(Invoice_Date) == false)
            {
                var msg = Resources.InvoiceMasterAddNewInvoice.FinancialYearOfTheCompanyControllerMessage;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: SalesReceiptMaster
        public ActionResult Index(string msg,int ? all)
        { 
            var models = new List<SalesReceiptMaster> { };
            if (Request.IsAuthenticated && User.IsInRole("بائع"))
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                models = SalesReceiptMaster.GetAllInvoiceForThatUser(user.Id,all);
            }
            else
            {
                models = SalesReceiptMaster.GetAllSalesReceipt(all);
            }
            ViewBag.Message = msg;
            return View(models);
        }

        // GET: SalesReceiptMaster/Details/5
        public ActionResult Details(int id)
        {
            var model = SalesReceiptMaster.GetSalesReceiptMasterById(id);
            if (model == null)
            {
               var msg = Resources.InvoiceMasterAddNewInvoice.InvoiceIsNotFounControllerMessage + " " + id.ToString();
                return RedirectToAction("Index", new { msg = msg });
            }
            else
            {
                return View(model);
            }
        }
        public JsonResult GetOfferJson(int proid)
        {
            var offer = OfferPriceMaster.getDetailById(proid);
            var xx = new
            {
                offer.Branch_Id,
                offer.BranchSubledger_Id,
                offer.DepartSubledger_Id,
                offer.BussinesSubledger_Id,
                offer.Company_Id,
                offer.Emp_Id,
                offer.Store_ID,
                offer.Master_Discount,
                offer.RevenueAccount_Id,
                offer.Reference,
                offer.Currency_Id,
                offer.Customer_Id,
                offer.Description,
                offer.Notes,
                offer.FixedDiscount
            };
            return Json(xx, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllTempInvoiceDetailsForOffer(int tempInvoiceMasterId, int Orderid)
        {
            var invoiceDetailVm = SalesReceiptMaster.PrepareListdetailOfInvoiceOffer(tempInvoiceMasterId, Orderid);
            return PartialView(invoiceDetailVm);
            //return PartialView("InvoiceMasterDetailsList", invoiceDetailVm);
        }
        public JsonResult AddingTax(List<string> LisOfTaxa, int MasterIda)
        {


            var msg = SalesReceiptMaster.AddmuchTax(LisOfTaxa, MasterIda);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult addnewSaleReciept(int cust_Id,int? orderid)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = true;
            if (orderid == 0||orderid==null)
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء فاتورة نقدي");
            }
            else
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تحويل عرض سعر الى نقدي");
            }
            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var Invoice = SalesReceiptMaster.AddNewRecieptSalesForCreation(cust_Id,user.Id,orderid);
            if (Invoice == null)
            {
                var msg = Resources.InvoiceMasterAddNewInvoice.InvoiceTechnicalErrorControllerMessage;
                return RedirectToAction("Index", new { msg = msg });
            }
            else
            {
                return View(Invoice);
            }
        }
        [HttpPost]
        public ActionResult addnewSaleReciept(SalesReceiptMasterVM masterVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var result = SalesReceiptMaster.addnewSaleReciept(masterVM, user.Id);
                if (result.Item2 == 0)
                {
                    var msg = Resources.InvoiceMasterAddNewInvoice.InvoiceIsNotCreatedControllerrMessage;
                    return RedirectToAction("Index", new { msg = msg });
                }
                return RedirectToAction("Details", new { id = result.Item2 });

            }
            catch (Exception ex)
            {
                var msg = Resources.InvoiceMasterAddNewInvoice.InvoiceBackEndErrorController + " " + ex.Message;
                return RedirectToAction("Index", new { msg = msg });
            }

        }
        public ActionResult CreateAndPrint(SalesReceiptMasterVM masterVM)
        {

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var a= SalesReceiptMaster.addnewSaleReciept(masterVM, user.Id);
            string result = a.Item1;
           var id =a.Item2 ;


            return RedirectToAction("PrintSalesRecipt", "Reports" ,new {id= id});

        }
        public ActionResult PrepareInvoiceDetails(int storeId)
        {
            var invoiceDetailVm = SalesReceiptMaster.PrepareInvoiceMasterDetail(storeId);
            return PartialView("PrepareInvoiceDetails", invoiceDetailVm);
        }
        public virtual JsonResult GetProductAvailableQty(int batchId)
        {

            try
            {
                int availableQty = SalesReceiptMaster.GetAvailbleProductQty(batchId);
                return Json(availableQty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {


                return Json(ex, JsonRequestBehavior.AllowGet);
            }





        }
        public virtual JsonResult AddInvoiceDetailsToMaster(SalesReceiptDetailsVM invoiceMasterDetail)
        {
            string msg = "";
            try
            {
                msg = SalesReceiptMaster.AddInvoiceDetailsTemp(invoiceMasterDetail);

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
        public ActionResult GetAllTempInvoiceDetails(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = SalesReceiptMaster.GetAllTempSalesReceiptDetails(tempInvoiceMasterId);
            return PartialView(invoiceDetailVm);
            //return PartialView("GetAllTempInvoiceDetails", invoiceDetailVm);
        }
        public ActionResult GetAllFiles(int invoiceMasterId)
        {
            var model = SalesReceiptMaster.GetAllFiles(invoiceMasterId);
            return View(model);
        }
        public ActionResult deleteFile(int fileId)
        {

            var document = SalesReceiptMaster.getFileById(fileId);
            var delete = SalesReceiptMaster.deleteFile(fileId);
            return RedirectToAction("Details", new { id = document.Master_Id });
        }

        public FileResult ReturnInvoicetFile(int fileId)
        {

            var document = SalesReceiptMaster.getFileById(fileId);
            var cd = new System.Net.Mime.ContentDisposition
            {

                FileName = document.FileName,

                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.FileContent, document.FileType);
        }



        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(SalesReceiptMasterVM salesinvoiceMasterVM)
        {
            try { 


            await Task.Delay(2000);
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد فاتورة نقدي");



                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }
                string result = SalesReceiptMaster.UpdateInvoiceMaster(salesinvoiceMasterVM);



            return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var erorr = msg;
                var data = ex.Data;
                return View();

            }
        }

        public ActionResult PrepareSalesDetails(int masterId)
        {
            var invoic = SalesReceiptMaster.PreparpurchMasterDetail2(masterId);
            return PartialView("SalesDetailsrCreationEdit", invoic);

        }
        public ActionResult GetAllTempInvoiceDetailsEdit(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = SalesReceiptMaster.GetAllTempPURCHInvoiceDetails2(tempInvoiceMasterId);
            return PartialView("SalesDetailsForEdit", invoiceDetailVm);
        }
        public ActionResult GetAllTempInvoiceDetailsresult(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = SalesReceiptMaster.GetAllTempPurchInvoiceDetailsresult2(tempInvoiceMasterId);
            return PartialView("SalesDetailResultList", invoiceDetailVm);
        }
        public ActionResult EditForPending(int id,int isCopy)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = true;
            if (isCopy == 0)
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل فاتورة نقدي");
            }
            else
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "نسخ فاتورة نقدي");
            }
            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var invoice = SalesReceiptMaster.fillForEditingInvoice(id,isCopy);
            if (invoice == null)
            {
                var msg = Resources.InvoiceMasterAddNewInvoice.InvoiceIsNotFounControllerMessage + " " + id.ToString();
                return RedirectToAction("Index", new { msg = msg });

            }
            else
            {
                return View(invoice);
            }
        }
        [HttpPost]
        public ActionResult EditForPending(SalesReceiptMasterVM masterVM)
        {
            try
            {
                if (masterVM.isCopy == 0)
                {
                    var invoice = SalesReceiptMaster.editinvoice(masterVM);
                    return RedirectToAction("Details", new { id = masterVM.Master_Id });
                }
                else
                {
                    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                    var result = SalesReceiptMaster.addnewSaleReciept(masterVM, user.Id);
                    if (result.Item2 == 0)
                    {
                        var msg = Resources.InvoiceMasterAddNewInvoice.InvoiceTechnicalErrorControllerMessage;
                        return RedirectToAction("Index", new { msg = msg });
                    }
                    return RedirectToAction("Details", new { id = result.Item2 });
                }
            }
            catch (Exception ex)
            {
                var msg = Resources.InvoiceMasterAddNewInvoice.InvoiceBackEndErrorController + " " + ex.Message;
                return RedirectToAction("Index", new { msg = msg });
            }


        }

        [HttpGet]
        public ActionResult PartialDialog()
        {
            var custvm = Customer.AddCustomerVM();
            return PartialView("CustomerPartialView", custvm);
        }

        public ActionResult AddNewCustomerPartial(CustomerVM masterVM)
        {
            HttpFileCollectionBase files = Request.Files;
            if (files.Count > 0)
            {
                masterVM.customerFiles = files;
            }
            var result = Customer.AddNewCustomer(masterVM);
            //ViewBag.res = result;
            //return PartialView("CustomerPartialView", ViewBag.res);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        [HttpGet]
        public ActionResult GetCustomerDropdownList( )
        {
            var customer = SalesReceiptMaster.GetCustomerList();
            return new JsonResult { Data = customer, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetBranchJson(int ID)
        {
            var branch = SalesReceiptMaster.GetBranchFroStoreInfo(ID);
            var xx = branch.Branch_Id;

            return Json(xx, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public virtual JsonResult GetDepartment(int code)
        {
            var my = FixedAsset.GetDepartments(code);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public virtual JsonResult GetBussines(int code)
        {
            var my = FixedAsset.GetBussinessUnits(code);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        public virtual ActionResult DeleteTemp(int id)
        {
         var temp=  SalesReceiptMaster.delete_Temp_Record(id);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف فاتورة نقدي");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var msg = SalesReceiptMaster.DeleteInvoiceMaster(id);
                //TempData["sales"] = msg;
                return RedirectToAction("ShowDeleteData", new { serial = msg.Serial_Number,value=msg.TotaL_After_Fixed });
            }
            catch (Exception ex)
            {

                var msg = Resources.InvoiceMasterAddNewInvoice.InvoiceBackEndErrorController + " " + ex.Message;
                return RedirectToAction("Index", new { msg = msg });
            }
        }
        public ActionResult ShowDeleteData(string serial,decimal value)
        {
            //ViewBag.Sales = (SalesReceiptMaster)TempData["sales"];
            ViewBag.serial = serial;
            ViewBag.value = value;
            return View();
        }

        public ActionResult getPaymentMethodJsondrobDown()
        {
            List<dynamic> result = new List<dynamic>();
    
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllTempinvoiceDetailForEditing(int MasterId)
        {
            var TempEditing = SalesReceiptMaster.GetAllDetailForEditing(MasterId);
            return PartialView(TempEditing);
        }

        public JsonResult updateDataDebendOnDiscount(decimal discount, int id)
        {
            var result = SalesReceiptMaster.changeDetailOfInvoice(discount, id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updateDiscountDebendOnDiscountAmount(decimal discount, int id)
        {
            var result = SalesReceiptMaster.updateDiscountDebendOnDiscountAmount(discount, id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateDiscountDebendOnNet(decimal net, int id)
        {
            var result = SalesReceiptMaster.updateDiscountDebendOnNet(net, id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updateDataDebendOnAlowDiscount(decimal discount, int id)
        {
            var result = SalesReceiptMaster.changeDetailOfInvoiceForAllDiscount(discount, id);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            try
            {

                var Message = SalesReceiptMaster.Import(excelfile);
                return RedirectToAction("Index", new { msg = Message });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { msg = ex.Message });
            }

        }
    }
}
