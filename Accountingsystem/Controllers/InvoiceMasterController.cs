using Accountingsystem.Models;
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
    //[Authorize(Roles = "SalesEmployee , Admin")]

    [Authorize]
    public class InvoiceMasterController : Controller
    {
        //public InvoiceMasterController()
        //{

        //    var actionName = this.ControllerContext.RouteData.Values["action"].ToString();


        //}
        public JsonResult updateDiscountDebendOnNet(decimal net, int id)
        {
            var result = InvoiceMaster.updateDiscountDebendOnNet(net, id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckCode(string Serial_Number)
        {
            if (InvoiceMaster.containCode(Serial_Number))
            {
                var msg = Resources.InvoiceMasterAddNewInvoice.OfferCodeExisit;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependOnDetailDiscount(int detailId, decimal detaildiscount)
        {


            var msg = InvoiceMaster.changeSpecificDiscount(detailId, detaildiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updatePOSDependOnDetailDiscountAmount(int detailId, decimal detaildiscountAmount)
        {


            var msg = InvoiceMaster.changeSpecificDiscountAmount(detailId, detaildiscountAmount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependOnDetailTotalAmount(int detailId, decimal detailTotalAmount)
        {


            var msg = InvoiceMaster.changeSpecificTotalAmount(detailId, detailTotalAmount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependOnquantity(int detailId, decimal quantity, decimal masterDiscount)
        {


            var msg = InvoiceMaster.changeSpecificValueinPOs(detailId, quantity, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependOnPrice(int detailId, decimal Price, decimal masterDiscount)
        {


            var msg = InvoiceMaster.changeSpecificPrice(detailId, Price, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependTax(int detailId, int Tax, decimal masterDiscount)
        {


            var msg = InvoiceMaster.changeSpecificTax(detailId, Tax, masterDiscount);


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

        public JsonResult AddingTax(List<string> LisOfTaxa,int MasterIda)
        {
            

            var msg = InvoiceMaster.AddmuchTax(LisOfTaxa, MasterIda);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        // GET: InvoiceMaster
        public JsonResult Checkdate(string Invoice_Date)
        {
            if (InvoiceMaster.Checkdate(Invoice_Date) ==false)
            {
                var msg = Resources.InvoiceMasterAddNewInvoice.FinancialYearOfTheCompanyControllerMessage;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index(string msg,int? all)
        {
            var models =new List<InvoiceMaster>{ };
            if (Request.IsAuthenticated && User.IsInRole("بائع"))
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                 models = InvoiceMaster.GetAllInvoiceForThatUser(user.Id,all);
            }
            else
            {
                models = InvoiceMaster.GetAllInvoices( all);
            }
            ViewBag.Message = msg;
            return View(models);
        }
        public ActionResult GetAllFiles(int invoiceMasterId)
        {
            var model = InvoiceMaster.GetAllFiles(invoiceMasterId);
            return View(model);
        }
        public ActionResult deleteFile(int fileId)
        {

            var document = InvoiceMaster.getFileById(fileId);
            var delete = InvoiceMaster.deleteFile(fileId);
            return RedirectToAction("Details", new { id = document.InvoiceMasterMaster_Id });
        }

        public FileResult ReturnInvoicetFile(int fileId)
        {

            var document = InvoiceMaster.getFileById(fileId);
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
        // GET: InvoiceMaster/Details/5
        public ActionResult Details(int id,string msg)
        {
            var invoiceMaster = InvoiceMaster.GetInvoiceDetailsById(id);
         
            if (invoiceMaster == null)
            {
                msg = Resources.InvoiceMasterAddNewInvoice.InvoiceIsNotFounControllerMessage + " " + id.ToString();
                return RedirectToAction("Index", new { msg = msg });
            }
            else
            {
                ViewBag.Message = msg;
                return View(invoiceMaster);
            }
          
           
            
        }
       



        public ActionResult AddNewInvoice(int orderido , int customerId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = true;
            if (orderido == 0)
            {
                 employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء فاتورة اجل");
            }
            else
            {
                 employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تحويل عرض سعر الى اجل");
            }
           
            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            var Invoice = InvoiceMaster.AddNewForCreation(orderido, user.Id, customerId);
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



        // POST: InvoiceMaster/Create
        [HttpPost]
        public ActionResult AddNewInvoice(MasterVM masterVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                int result = InvoiceMaster.AddNewInvoice(masterVM, user.Id);
                if (result == 0)
                {
                    var msg = Resources.InvoiceMasterAddNewInvoice.InvoiceIsNotCreatedControllerrMessage;
                    return RedirectToAction("Index", new { msg = msg});

                }
                return RedirectToAction("Details", new { id = result });
            }
            catch (Exception ex)
            {

                var msg = Resources.InvoiceMasterAddNewInvoice.InvoiceBackEndErrorController + " " +  ex.Message;
                return RedirectToAction("Index", new { msg = msg });
            }
           
           
        }
        [HttpGet]
        public ActionResult PrepareofferDetails(int ID)
        {
            var invoiceDetailVm = InvoiceMaster.PrepareOfferDetail(ID);
            return PartialView("PrepareofferDetails", invoiceDetailVm);
        }

        [HttpGet]
        public ActionResult PrepareInvoiceDetails(int storeId)
        {
            var invoiceDetailVm = InvoiceMaster.PrepareInvoiceMasterDetail(storeId);
            return PartialView("InvoiceDetailsPartial", invoiceDetailVm);
        }
        [HttpGet]
        public virtual JsonResult GetProductAvailableQty(int batchId)
        {

            try
            {
                int availableQty = InvoiceMaster.GetAvailbleProductQty(batchId);
                return Json(availableQty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {


                return Json(ex, JsonRequestBehavior.AllowGet);
            }





        }
        [HttpPost]
        //for add details to master
        public virtual JsonResult AddInvoiceDetailsToMaster(InvoiceMasterDetailsVM invoiceMasterDetail)
        {
            string msg = "";
            try
            {
                msg = InvoiceMaster.AddInvoiceDetailsTemp(invoiceMasterDetail);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }


           
            return Json(msg, JsonRequestBehavior.AllowGet);
           
        }
        [HttpPost]
        //for add details to master
        public virtual JsonResult AddInvoiceDetailsToMasters(InvoiceMasterDetailsVM invoiceMasterDetail)
        {
            string msg = "";
            try
            {
                msg = InvoiceMaster.AddInvoiceDetailsTempOffer(invoiceMasterDetail);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }         
            return Json(msg, JsonRequestBehavior.AllowGet);
          
        }
        [HttpGet]
        public ActionResult GetAllTempInvoiceDetails(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = InvoiceMaster.GetAllTempInvoiceDetails(tempInvoiceMasterId);
            return PartialView(invoiceDetailVm);           
        }
        public ActionResult GetAllTempInvoiceDetailsForOffer(int tempInvoiceMasterId,int Orderid)
        {
            var invoiceDetailVm = InvoiceMaster.PrepareListdetailOfInvoiceOffer(tempInvoiceMasterId, Orderid);
            return PartialView(invoiceDetailVm);
            //return PartialView("InvoiceMasterDetailsList", invoiceDetailVm);
        }
        // POST: InvoiceMaster/Edit/5
        [HttpPost]
        public ActionResult Edit(MasterVM invoiceMasterVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد فاتورة اجل");



                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }
                string result = InvoiceMaster.UpdateInvoiceMaster(invoiceMasterVM);

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

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
              
                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف فاتورة اجل");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }

                var msg = InvoiceMaster.DeleteInvoiceMaster(id);
                return RedirectToAction("ShowDeleteData", new { serial = msg.Serial_Number, value = msg.TotaL_After_Fixed });
            }
            catch (Exception ex)
            {

                var msg = Accountingsystem.Resources.InvoiceMasterAddNewInvoice.InvoiceBackEndErrorController + " " + ex.Message;
                return RedirectToAction("Index", new { msg = msg });
            }
          
        }
        public ActionResult ShowDeleteData(string serial, decimal value)
        {
            //ViewBag.Sales = (SalesReceiptMaster)TempData["sales"];
            ViewBag.serial = serial;
            ViewBag.value = value;
            return View();
        }
        [HttpGet]
        public virtual JsonResult GetProducts(int storeId)
        {
            var products = InvoiceMaster.getStoreProducts(storeId);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

      

        [HttpGet]
        //for list from details
        public ActionResult PrepareSalesDetails(int masterId)
        {
            var invoic = InvoiceMaster.PreparpurchMasterDetail2(masterId);
            return PartialView("SalesDetailsrCreationEdit", invoic);

        }


        [HttpGet]
        public ActionResult GetAllTempInvoiceDetailsEdit(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = InvoiceMaster.GetAllTempPURCHInvoiceDetails2(tempInvoiceMasterId);
            return PartialView("SalesDetailsForEdit", invoiceDetailVm);
        }
        [HttpGet]
        public ActionResult GetAllTempInvoiceDetailsresult(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = InvoiceMaster.GetAllTempPurchInvoiceDetailsresult2(tempInvoiceMasterId);
            return PartialView("SalesDetailResultList", invoiceDetailVm);
        }
        [HttpGet]
        public ActionResult EditForPending(int id ,int isCopy)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = true;
            if (isCopy == 0)
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل فاتورة اجل");
            }
            else
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "نسخ فاتورة اجل");
            }
            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var invoice = InvoiceMaster.fillForEditingInvoice(id,isCopy);
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
        public ActionResult EditForPending(MasterVM masterVM)
        {
            try
            {
                if (masterVM.isCopy == 0)
                {
                    var editFAsst = InvoiceMaster.editinvoice(masterVM);
                    return RedirectToAction("Details", new { id = masterVM.Master_Id });
                }
                else
                {
                    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                    int result = InvoiceMaster.AddNewInvoice(masterVM, user.Id);
                    if (result == 0)
                    {
                        var msg = Resources.InvoiceMasterAddNewInvoice.InvoiceTechnicalErrorControllerMessage;
                        return RedirectToAction("Index", new { msg = msg });
                    }
                    return RedirectToAction("Details", new { id = result });
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
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpGet]
        public ActionResult GetCustomerDropdownList()
        {
            var customer = InvoiceMaster.GetCustomerList();
            return new JsonResult { Data = customer, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetProductPrice(int proId, int customer)
        {
            var result = InvoiceMaster.getProductPrice(proId, customer);
            var price = new
            {
                result.price,
                result.Tax,
            };
            return Json(price, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetProductPriceNotPatched(int proId, int customer)
        {
            var result = InvoiceMaster.getProductPricenotPatched(proId, customer);
            var price = new
            {
                result.price,
                result.Tax,
            };
            return Json(price, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDetailInfo(int proId, int masterID)
        {
            var result = InvoiceMaster.getProductInfo(proId, masterID);
            var price = new
            {
                result.Product_Tax,
                result.Product_Price,
                result.Product_Quantity,

            };
            return Json(price, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBranchJson(int ID)
        {
            var branch = InvoiceMaster.GetBranchFroStoreInfo(ID);
            var xx = branch.Branch_Id;

            return Json(xx, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getDetailFromOrder(int Order)
        {
            var orderdetail = InvoiceMaster.getDetailFromOrder(Order);
            return PartialView("OrderDetailList", orderdetail);
        }

        public ActionResult GetFinalDate(int typeid, string startdate)
        {
            var result = InvoiceMaster.getdate(startdate, typeid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOfferJson(int proid)
        {
            var offer = OfferPriceMaster.getDetailById(proid);
            var xx = new
            {
                offer.Branch_Id, offer.BranchSubledger_Id, offer.DepartSubledger_Id, offer.BussinesSubledger_Id, 
                offer.Company_Id,
                offer.Emp_Id,
                offer.Store_ID,
                offer.Master_Discount,
                offer.RevenueAccount_Id,
                offer.Reference,
                offer.Currency_Id,
                offer.Customer_Id,
                offer.FixedDiscount,

            };
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
        public JsonResult DeleteTempDetail(int iD)
        {
            var temp = InvoiceMaster.DeleteTemp(iD);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public virtual JsonResult GetCustomers(int EmpID)
        {
            var my = InvoiceMaster.GetCustomerUsingEmployee(EmpID);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Getmessage(int batchId, decimal price)
        {
            var result = InvoiceMaster.CreateAlertIfPriceLowPriceCostForSales(batchId, price);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetmessageForPOS(int batchId, decimal price)
        {
            var result = InvoiceMaster.CreateAlertIfPriceLowPriceCostForPOS(batchId, price);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetnetValue(decimal total, decimal fixedDiscount)
        {
            var result = InvoiceMaster.getTheNetTotal(total, fixedDiscount);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllTempinvoiceDetailForEditing(int MasterId)

        {
            var TempEditing = InvoiceMaster.GetAllDetailForEditing(MasterId);
            return PartialView(TempEditing);

        }

        public JsonResult updateDataDebendOnDiscount(decimal discount, int id)
        {
            var result = InvoiceMaster.changeDetailOfInvoice(discount, id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updateDiscountDebendOnDiscountAmount(decimal discount, int id)
        {
            var result = InvoiceMaster.updateDiscountDebendOnDiscountAmount(discount, id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateDataDebendOnAlowDiscount(decimal discount, int id)
        {
            var result = InvoiceMaster.changeDetailOfInvoiceForAllDiscount(discount, id);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
     
        public virtual JsonResult GetAllCustomerForEmployee(int empId)
        {
            var my = InvoiceMaster.GetAllCustomerForEmp(empId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
    }
}