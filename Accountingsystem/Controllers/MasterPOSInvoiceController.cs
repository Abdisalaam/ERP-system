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
    [Authorize]
    public class MasterPOSInvoiceController : Controller
    {
        public JsonResult updateDiscountDebendOnNet(decimal net, int id)
        {
            var result = MasterPOSInvoice.updateDiscountDebendOnNet(net, id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: MasterPOSInvoice
        public ActionResult Index(string msg ,int? all, string invoicesDate)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var differentInvoice = false;
            //if (User.IsInRole("مالك النظام") || User.IsInRole("عميل"))
            //{
            //    differentInvoice = true;
            //}

            var list = MasterPOSInvoice.getAllDataForInvoices(user.Id, differentInvoice);
            if (!string.IsNullOrEmpty(invoicesDate) && DateTime.TryParse(invoicesDate, out DateTime Temp))
            {

               // DateTime myDate = DateTime.Parse(invoicesDate);
                list = list.Where(e => e.Invoice_Date >= Temp).ToList();
                ViewBag.message = msg;
                return View(list);
            }
            if (all == null)
            {
                list.Reverse();
                list = list.Take(100).ToList();
            }
           
            ViewBag.message = msg;
            return View(list);
        }
        
        public JsonResult Checkdate(string Invoice_Date)
        {
            if (MasterPOSInvoice.Checkdate(Invoice_Date) == false)
            {
                var msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.dateTrue;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int id,string msg)
        {
            var Invoice = MasterPOSInvoice.getinvoiceByid(id);
            ViewBag.msg = msg;
            return View(Invoice);
        }
        public ActionResult GetInvoicesByDate(string invoicesDate)
        {
           
            ViewBag.msg = invoicesDate;
            return RedirectToAction("Index", new { msg = invoicesDate });
        }
        public ActionResult PrintHotInvoice(int id)
        {
            var Invoice = MasterPOSInvoice.getinvoiceByid(id);
          
            return View(Invoice);
        }

        public ActionResult Create(String msg,int?id )
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء فاتورة نقطة مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            ViewBag.msg = msg;
            var POSVM = MasterPOSInvoice.Create(user.Id,0);

            if (POSVM != null)
            {

                return View(POSVM);
            }
            else
            {
                var MSG = Resources.MasterPOSInvoice.donothavePOS;
                return RedirectToAction("Index", new { msg = MSG });
            }
            
        }
        [HttpPost]
        public ActionResult Create(MasterPOSInvoiceVM vm)
        {

            // ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());


            var POSVM = MasterPOSInvoice.CreatePost(vm, user.Id);
            if (POSVM.Item1 != 0)
            {
                var vm1 = MasterPOSInvoice.ErrorCreate(user.Id, POSVM.Item1, 0);
                ViewBag.msg = POSVM.Item2;
                return View(vm1);
            }
            else
            {
                return RedirectToAction("Detail", new { id = POSVM.Item3, msg = POSVM.Item2 });
            }

        }



        public ActionResult CreateInvoiceMaster(String msg, int? id,int ?SewingId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء فاتورة نقطة مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            ViewBag.msg = msg;
            var POSVM = MasterPOSInvoice.Create(user.Id, SewingId);

            if (POSVM != null)
            {

                return View(POSVM);
            }
            else
            {
                var MSG = Resources.MasterPOSInvoice.donothavePOS;
                return RedirectToAction("Index", new { msg = MSG });
            }

        }
        [HttpPost]
        public async Task<ActionResult> CreateInvoiceMaster(MasterPOSInvoiceVM vm)
        {

            // ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());


            var POSVM = MasterPOSInvoice.CreatePostForInvoiceMaster(vm, user.Id);
            if (POSVM.Item1 != 0)
            {
                var vm1 = MasterPOSInvoice.ErrorCreate(user.Id, POSVM.Item1, 0);
                ViewBag.msg = POSVM.Item2;
                return View(vm1);
            }
            else
            {
                return RedirectToAction("Detail", new { id = POSVM.Item3, msg = POSVM.Item2 });
            }

        }
        public ActionResult GetAllTempInvoiceDetailsForOrder(int tempInvoiceMasterId, int Orderid)
        {
            var invoiceDetailVm = MasterPOSInvoice.PrepareListdetailOfOrdertoPOS(tempInvoiceMasterId,Orderid);
            return PartialView(invoiceDetailVm);
            //return PartialView("InvoiceMasterDetailsList", invoiceDetailVm);
        }

        public ActionResult CreateForMain(String msg, int Main_Id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تحويل الى فاتورة نقطة بيع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            ViewBag.msg = msg;
            var POSVM = MasterPOSInvoice.CreateForMaintenance(user.Id,Main_Id);

            if (POSVM != null)
            {

                return View(POSVM);
            }
            else
            {
                var MSG = Resources.MasterPOSInvoice.donothavePOS;
                return RedirectToAction("Index", "MaintenanceOrderOperational", new { msg = MSG });
            }

        }




        [HttpPost]
        public async Task<ActionResult> CreateForMain(MasterPOSInvoiceVM vm)
        {

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var POSVM = MasterPOSInvoice.CreatePost(vm, user.Id);
            if (POSVM.Item1 != 0)
            {
                var vm1 = MasterPOSInvoice.ErrorCreate(user.Id, POSVM.Item1,vm.MaintenanceOrder_Id);
                ViewBag.msg = POSVM.Item2;
                return View(vm1);
            }
            else
            {
                return RedirectToAction("Detail", new { id = POSVM.Item3, msg = POSVM.Item2 });
            }

        }

        public JsonResult AddingTax(List<string> LisOfTaxa, int MasterIda)
        {


            var msg = MasterPOSInvoice.AddmuchTax(LisOfTaxa, MasterIda);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PreparePayment(int POS_ID)
        {
            var PaymentVM = MasterPOSInvoice.PreparePaymentMethod(POS_ID);
            return PartialView(PaymentVM);
        }
        public ActionResult PrepareDetail(int POS_ID)
        {
            var DetailVM = MasterPOSInvoice.prepareDetailOfInvoice(POS_ID);
            return PartialView(DetailVM);
        }


        [HttpGet]
        public ActionResult allTempPay(int POSTemp)
        {
            var poseDetailVm = MasterPOSInvoice.Getalldatapaymenttemp(POSTemp);
            return PartialView("allTempPay", poseDetailVm);
        }

        [HttpPost]
        //for add details to master
        public virtual JsonResult Addtatatotemp(DetailPOSInvoiceVM invoiceMasterDetail)
        {
            string msg = "";
            try
            {
                msg = MasterPOSInvoice.Adddetailpostodetailtemp(invoiceMasterDetail);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllTempInvoiceDetails(int tempIPosId,int ?sewingId)
        {
            MasterPOSInvoiceDetailTempVM poseDetailVm = new MasterPOSInvoiceDetailTempVM();
            if (sewingId > 0)
            {
                 poseDetailVm = MasterPOSInvoice.PrepareListdetailOfSewing(tempIPosId,sewingId);
            }
            else
            {
                 poseDetailVm = MasterPOSInvoice.DetailPosdatatemp(tempIPosId);
            }

            return PartialView(poseDetailVm);
        }


        public JsonResult AddPayemnt(PosPaymentMethodVM PayVM)
        {
            var PaymentVM = MasterPOSInvoice.CreatePayment(PayVM);

            return Json(PaymentVM, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteTemp(int ids)
        {
            var PaymentVM = MasterPOSInvoice.DeleteFromTemp(ids);

            return Json(PaymentVM, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteAllDetails(int idss)
        {
            var PaymentVM = MasterPOSInvoice.DeleteTempForPay(idss);

            return Json(PaymentVM, JsonRequestBehavior.AllowGet);

        }
        public JsonResult updateDataDebendOnDiscount(decimal discount,int id)
        {
            var result=  MasterPOSInvoice.changeDetailOfInvoice(discount, id);

            return Json(result,JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependOnDetailDiscount(int detailId, decimal detaildiscount)
        {


            var msg = MasterPOSInvoice.changeSpecificDiscount(detailId, detaildiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updateDiscountDebendOnDiscountAmount(decimal discount, int id)
        {
            var result = MasterPOSInvoice.updateDiscountDebendOnDiscountAmount(discount, id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updateDataDebendOnAlowDiscount(decimal discount, int id)
        {
            var result = MasterPOSInvoice.changeDetailOfInvoiceForAllDiscount(discount, id);

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetProductPrice(int proId, int customer)
        {
            var result = MasterPOSInvoice.getProductPrice(proId, customer);
            var price = new
            {
                result.price,
                result.Tax,
            };
            return Json(price, JsonRequestBehavior.AllowGet);
        }
        public ActionResult searchInMasterPOS()
        {
            
            List<dynamic> result = new List<dynamic>();
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async System.Threading.Tasks.Task<ActionResult> Edit(MasterPOSInvoiceVM MasterVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "استبعاد فاتورة نقطة مبيعات");



                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }
                await Task.Delay(2000);
                string result = MasterPOSInvoice.UpdateInvoiceMaster(MasterVM);

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
        public JsonResult updatePOSDependOnquantity(int detailId, decimal quantity, decimal masterDiscount)
        {


            var msg = MasterPOSInvoice.changeSpecificValueinPOs(detailId, quantity, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependOnPrice(int detailId, decimal Price, decimal masterDiscount)
        {


            var msg = MasterPOSInvoice.changeSpecificPrice(detailId, Price, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependTax(int detailId, int Tax, decimal masterDiscount)
        {


            var msg = MasterPOSInvoice.changeSpecificTax(detailId, Tax, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependOnDetailDiscountAmount(int detailId, decimal detaildiscountAmount)
        {


            var msg = MasterPOSInvoice.changeSpecificDiscountAmount(detailId, detaildiscountAmount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependOnDetailTotalAmount(int detailId, decimal detailTotalAmount)
        {


            var msg = MasterPOSInvoice.changeSpecificTotalAmount(detailId, detailTotalAmount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangePaymentMethod(int masterId,int PaymentMethod)
        {


            MasterPOSInvoice.changePaymentMethod(masterId, PaymentMethod);


            return Json("done", JsonRequestBehavior.AllowGet);
        }
        public ActionResult PartialDialog()
        {
            var custvm = Customer.AddCustomerVM();
            return PartialView("CustomerPartialView", custvm);
        }

        public ActionResult AddNewCustomerPartial(CustomerVM masterVM)
        {

            var result = Customer.AddNewCustomer(masterVM);
            //ViewBag.res = result;
            //return PartialView("CustomerPartialView", ViewBag.res);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        [HttpGet]
        public ActionResult GetCustomerDropdownList()
        {
            var customer = SalesReceiptMaster.GetCustomerList();
            return new JsonResult { Data = customer, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
            var prod = Product.GetProdectById(result);
            result = prod.ProductBatches.FirstOrDefault().Batch_ID;
            var xx = new
            {
                result,
                productVM.Prod_Name,
                prod.Prod_Code,
               

            };

            return Json(xx, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetProductDrobDownList()
        {
            var customer = OfferPriceMaster.GetProductList();
            return new JsonResult { Data = customer, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetAllPOSDetailsToPrintNewWindow(int orderId)
        {
            var orderDetails = MasterPOSInvoice.getinvoiceByid(orderId);
            return View(orderDetails);
        }
        public virtual JsonResult GetAllCustomerForEmployee()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var my = MasterPOSInvoice.GetAllCustomerForEmp(user.Id);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
    }

  
}