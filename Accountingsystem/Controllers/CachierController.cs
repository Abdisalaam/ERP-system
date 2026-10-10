using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Models;
using Accountingsystem.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class CachierController : Controller
    {
        public JsonResult updateDiscountDebendOnNet(decimal net, int id)
        {
            var result = Order.updateDiscountDebendOnNet(net, id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllOrdersIndex()
        {
            var orders = Order.GetAllOrdersNoExeption();
            return View(orders);
        }
        // GET: Cachier
     
        public ActionResult Index(string msg, int? all)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var differentInvoice = false;
            //if (User.IsInRole("مالك النظام") || User.IsInRole("عميل"))
            //{
            //    differentInvoice = true;
            //}

            var list = Order.GetAllOrders(user.Id, differentInvoice);
            if (all == null)
            {
                list.Reverse();
                list = list.Take(100).ToList();
            }
            ViewBag.message = msg;
            return View(list);
        }

        public ActionResult ReverseOrdersIndex()
        {
            var reverseOrders = Order.GetAllReverseOrders();
            return View(reverseOrders);
        }

        // GET: Cachier/Details/5
        public ActionResult Details(int id,string msg)
        {
            var order = Order.GetOrderVMById(id);
            ViewBag.Message = msg;
            return View(order);
        }
        [HttpGet]
        public ActionResult StartSelling()
        {
            var OpenForSale = Order.StartSelling();
            var msg = Resources.CachierCreate.StartSelling;
            return RedirectToAction("Create", new { msg});

        }
        [HttpGet]
        public ActionResult CloseCachier()
        {
            var ClosedForSale = Order.EndSelling();
            var msg = Resources.CachierCreate.EndSelling;
            return RedirectToAction("Index", new { msg });
        }
        // GET: Cachier/Create
        [HttpGet]
        public ActionResult Create(string msg, int? orderId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء فاتورة كاشير");
            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            ViewBag.Message = msg;
            ViewBag.ModelOrderId = orderId;
            var userID = User.Identity.GetUserId();
            var empolyeeExisit = Order.CheckIfEmpolyeeExisit(userID);
            if (empolyeeExisit)
            {
                var model = Order.PrepareOrderVM(userID);
                return View(model);
            }
            else
            {
                return RedirectToAction("WelcomePage", "Company");
            }
          
        }

        // POST: Cachier/Create
        [HttpPost]
        public ActionResult Create(OrderVM orderVM)
        {
            var userID = User.Identity.GetUserId();
            try
            {
                // TODO: Add insert logic here

                var order = Order.AddOrderNewOrder(orderVM);
                if (order !=null)
                {
                    var msg = order.OrderNumber;
                    var orderId = order.Id;                   
                    //var model = Order.PrepareOrderVM(userID);
                    //return View(model);
                    return RedirectToAction("Create", new { msg, orderId });
                }
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                throw ex;
                //return View();
            }
        }
        public ActionResult UpdateOrderTypeAndComment(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء فاتورة كاشير");
            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
           
            var userID = User.Identity.GetUserId();
            var empolyeeExisit = Order.CheckIfEmpolyeeExisit(userID);
            if (empolyeeExisit)
            {
                var model = Order.PrepareOrderVMFoUpdate(id);
                if (model == null)
                {
                    return RedirectToAction("Create");

                }

                return View(model);
            }
            else
            {
                return RedirectToAction("WelcomePage", "Company");
            }
        }


        // POST: Cachier/Delete/5
        [HttpPost]
        public ActionResult UpdateOrderTypeAndComment(OrderVM orderVM)
        {
            try
            {

                var model = Order.UpdateOrderType(orderVM);
                var msg = "";
                if (model == 0)
                {
                    return RedirectToAction("Create");

                }
                else
                {
                    return RedirectToAction("Create", new { msg, orderId = model });
                }

            }
            catch (Exception)
            {
                var order = Order.GetOrderVMByIdForReverse(orderVM.OrderId);
                return View(order);
            }
        }
        // GET: Cachier/Edit/5
        public ActionResult searchView()
        {
            return PartialView("searchView");
        }
        public JsonResult HoldOrder(int orderId, int employeeId, int customerId, decimal discountPrecentag, int extraTaxId, int paymentMethodId, string comment)
        {
            OrderVM orderVM = new OrderVM();
            orderVM.Id = orderId;
            orderVM.EmployeeID = employeeId;
            orderVM.SelectedCustomer_Id = customerId;
            orderVM.DicountPrecentage = discountPrecentag;
            orderVM.SelectedTaxID = extraTaxId;
            orderVM.SelectedPaymentMethodeID = paymentMethodId;
            orderVM.Comment = comment;
            var holdOldersCount = Order.HoldOrder(orderVM);
            return Json(holdOldersCount, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddProductToInvoiceDetails(decimal qty, int orderId, int patchId,int customer)
        {
            if (qty < 1)
            {
                qty = 1;
            }
            var modelId = Order.AddOrderDetailsVM(qty, orderId, patchId, customer);
            return Json(modelId, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddProductToInvoiceDetailsFromScanner(decimal qty, int orderId, string productBarcode, int customer)
        {
            if (qty < 1)
            {
                qty = 1;
            }
            var modelId = Order.AddOrderDetailsVMFromScanner(qty, orderId, productBarcode, customer);
            return Json(modelId, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBatchIdByBarcode(string barcode, int orderId)
        {
            var result = Order.GetBatchidByBarcode(barcode, orderId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteInvoiceDetails(int orderDetailId)
        {
            var modelId = Order.DeleteOrderInvoiceDetails(orderDetailId);
            return Json(modelId, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddInvoiceDetailsToReverse(int orderDetailId, int tempOrderId, int qty)
        {
            var modelId = Order.AddInvoiceDetailsToRevrse(orderDetailId, tempOrderId, qty);
            return Json(modelId, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReverseInvoiceDetails(int orderDetailId, int qty)
        {
            var modelId = Order.DeleteOrderInvoiceDetails(orderDetailId);
            return Json(modelId, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllOrderDetailsForReverse(int orderId)
        {
            var orderDetails = Order.GetAllOrderDetailsForReverse(orderId);
            return PartialView("AllOrderDetailsForReverse", orderDetails);
        }
        public JsonResult DeleteHoldedOrder(int holdedOrderId)
        {
            var holdedOrdersCount = Order.DeleteHoldedOrder(holdedOrderId);
            return Json(holdedOrdersCount, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHoldedOrder(int holdedOrderId)
        {
            var holdedOrdersId = Order.GetHoldedOrder(holdedOrderId);
            return Json(holdedOrdersId, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateOrderPrecentage(decimal precentageValue, int orderId)
        {
            var modelId = Order.AddOrderDiscountPrecentageToOrder(precentageValue,orderId);
            return Json(modelId, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTaxById(int taxId, int tempOrderId)
        {
            var tax = Order.GetTaxById(taxId, tempOrderId);
            return Json(tax, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddCustomerToRder(int customerId, int tempOrderId)
        {
            var customer = Order.AddCustomerToOrder(customerId, tempOrderId);
            return Json(customer, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllTempOrderDetails(int orderId)
        {
            var orderDetails = Order.GetAllOrderDetails(orderId);
            return PartialView("OrderDetailsVMView", orderDetails);
        }
        [HttpGet]
        public ActionResult GetAllTempOrderDetailsToPrint(int orderId)
        {
            var orderDetails = Order.GetAllOrderDetails(orderId);
            return PartialView("PrintInvoiceTemplate", orderDetails);
        }
        [HttpGet]
        public ActionResult GetAllOrderDetailsToPrint(int orderId)
        {
            var orderDetails = Order.GetAllOrderDetailsToPrint(orderId);
            return PartialView("PrintInvoiceTemplate", orderDetails);
        }

        //[HttpGet]
        //public ActionResult GetAllOrderDetailsToPrintNewWindow(int orderId)
        //{
        //    var orderDetails = Order.GetAllOrderDetailsToPrint(orderId);
        //    return View(orderDetails);
        //}
        [HttpGet]
        public ActionResult GetAllOrderDetailsToPrintNewWindow(int orderId)
        {
            var orderDetails = Order.GetAllOrderDetailsToPrint(orderId);
            return View(orderDetails);
        }
        [HttpGet]
        public ActionResult GetAllReversdOrderDetailsToPrint(int reversdeOrderId)
        {
            var orderDetails = Order.GetAllRevesdOrderDetails(reversdeOrderId);
            return PartialView("PrintInvoiceTemplate", orderDetails);
        }

        [HttpGet]
        public ActionResult GetAllReversdeOrderDetails(int orderId)
        {
            var orderDetails = Order.GetAllOrderDetails(orderId);
            return PartialView("AllReversdeOrderDetails", orderDetails);
        }
        [HttpGet]
        public ActionResult GetHoldedOrdersPartialDialog()
        {
            var holdedOrders = Order.GetAllHoldedOrders();
            return PartialView("HoldedOrdersPartialView", holdedOrders);
        }
        
        [HttpGet]
        public ActionResult GetAllOrdersPartialDialog()
        {
            var allOrders = Order.GetLastHunderdOrders();
            return PartialView("AllOrdersOfTheDay", allOrders);
        }
        //[HttpGet]
        //public ActionResult GetAllOrdersPartialDialog()
        //{
        //    var allOrders = Order.GetLastHunderdOrders();
        //    return PartialView("AllOrdersPartialView", allOrders);
        //}
        [HttpGet]
        public ActionResult GetAllReversdeOrderItemsPartialDialog(int orderId)
        {
            var allReversdeItemsOfOrders = Order.GetAllRevesdOrderofOrder(orderId);
            return PartialView("AllReversdeItemsOfOrder", allReversdeItemsOfOrders);
        }
        [HttpGet]
        public ActionResult GetAllCategoeryProducts(int categoeryId)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                var categoeryProductsVMList = Order.GetAllCategoeryProducts(categoeryId,userID);
                var result = categoeryProductsVMList;
                return PartialView("CategoeryProductListView", categoeryProductsVMList);
            }
            catch (Exception ex)
            {
                var tt = ex.Message;
                var dd = ex.InnerException.Message;
                var mesg = "It error";
               return View(mesg) ;
            }
           
            //return PartialView("Tset");
        }
        [HttpPost]
        public ActionResult VoidOrderById(OrderVM data)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "استبعاد فاتورة كاشير");



                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }
                var ordervM = Order.GetOrderVMById(data.Id);
                var result = Order.MakeVoidGL(ordervM.Id);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return RedirectToAction("Create");
            }
           
        }
     
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء مردود فاتورة كاشير");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var order = Order.GetOrderVMByIdForReverse(id);
            return View(order);
        }

       
        // POST: Cachier/Delete/5
        [HttpPost]
        public ActionResult Edit(OrderVM orderVM)
        {
            try
            {

                var model = Order.ReversOrder(orderVM);
                return RedirectToAction("GetReversdeOrderDetails", new { reversdeOrderId = model });

            }
            catch(Exception)
            {
                var order = Order.GetOrderVMByIdForReverse(orderVM.OrderId);
                return View(order);
            }
        }
        [HttpGet]
        public ActionResult GetReversdeOrderDetails(int reversdeOrderId)
        {
            try
            {
                var model = Order.GetReverseOrderVMById(reversdeOrderId);
                return View(model);
            }
            catch (Exception ex)
            {
                var tt = ex.Message;
                var dd = ex.InnerException.Message;
                var mesg = "It error";
                return View(mesg);
            }

            //return PartialView("Tset");
        }
        [HttpGet]
        public ActionResult CustomerPartialView()
        {
            var custvm = Customer.AddCustomerVM();
            return PartialView(custvm);
        }

        [HttpGet]
        public ActionResult AdditionalOrderDetailPartialView(int orderDetailId)
        {
            var additionalDetal = Order.PrepareAdditionalOrderDetail(orderDetailId);
            return PartialView(additionalDetal);
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

        public ActionResult AddNewAdditional(int selectedAdditionalDetailId, int orderDetailId)
        {

            var result = Order.AddAdditionalDetailToOrderDetail(selectedAdditionalDetailId, orderDetailId);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}
