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
    public class MaintenanceOrderOperationalController : Controller
    {
        // GET: MaintenanceOrderOperational
        public ActionResult Index(string msg)
        {
            var model = MaintenanceOrderOperational.GetAllOrder();
            if (User.IsInRole("فني"))
            {
                model = model.Where(a => a.Status == 1).ToList();
            }
            ViewBag.Message = msg;
            return View(model);
        }

        // GET: MaintenanceOrderOperational/Details/5
        public ActionResult Details(int id)
        {
            var model = MaintenanceOrderOperational.GetOrderById(id);
            return View(model);
        }
        public ActionResult PrepareDetailForCreate()
        {
            var vm = MaintenanceOrderOperational.prepareDetailOfOrder();
            return PartialView(vm);
        }
        public ActionResult GetListOfOrderProduct(int master_Id)
        {
            var vm = MaintenanceOrderOperational.DetaildatatempList(master_Id);
            return PartialView(vm);
        }
        public virtual JsonResult AddInvoiceDetailsToMaster(MaintenanceOrderDetailVM detail)
        {
            string msg = "";
            try
            {
                msg = MaintenanceOrderOperational.Adddetailpostodetailtemp(detail);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }



            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public JsonResult updatePOSDependOnquantity(int detailId, decimal quantity, decimal masterDiscount)
        {


            var msg = MaintenanceOrderOperational.changeSpecificValueinPOs(detailId, quantity, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependOnPrice(int detailId, decimal Price, decimal masterDiscount)
        {


            var msg = MaintenanceOrderOperational.changeSpecificPrice(detailId, Price, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependTax(int detailId, int Tax, decimal masterDiscount)
        {


            var msg = MaintenanceOrderOperational.changeSpecificTax(detailId, Tax, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updateDataDebendOnDiscount(decimal discount, int id)
        {
            var result = MaintenanceOrderOperational.changeDetailOfInvoice(discount, id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateDataDebendOnAlowDiscount(decimal discount, int id)
        {
            var result = MaintenanceOrderOperational.changeDetailOfInvoiceForAllDiscount(discount, id);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DeleteTempDetail(int iD)
        {
            var temp = MaintenanceOrderOperational.DeleteTemp(iD);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
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

        // GET: MaintenanceOrderOperational/Create
        public ActionResult Create()
        {
            var vm = MaintenanceOrderOperational.PrepareForCreate();
            return View(vm);
        }

        // POST: MaintenanceOrderOperational/Create
        [HttpPost]
        public ActionResult Create(MaintenanceOrderOperationalVM vm)
        {
            try
            {
                //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء امر استلام");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }

                var result = MaintenanceOrderOperational.addnewSaleReciept(vm, user.Id);
                if (result.Item2 == 0)
                {
                    var msg = Resources.InvoiceMasterAddNewInvoice.InvoiceIsNotCreatedControllerrMessage;
                    return RedirectToAction("Index", new { msg = msg });
                }
                return RedirectToAction("Details", new { id = result.Item2 });

            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeliverToCustomer(int id,int status)
        {
            try
            {
                //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var employeeAction = true;
                if (status == 4)
                {
                    employeeAction = Employee.getAllFunctionForEmployee(user.Id, "جاهز لاستلام");
                }
                else
                {
                    employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تسليم الجهاز");
                }
                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var result = MaintenanceOrderOperational.DeliverToCustomer(id,user.Id,status);
               
                return RedirectToAction("Details", new { id = id });

            }
            catch
            {
                return View();
            }
        }
        // GET: MaintenanceOrderOperational/Edit/5
        public ActionResult Edit(int id,int status)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = true;
            if (status == 2)
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "معاينة فنية");
            }
            else
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقدير مبلغ الاصلاح");
            }
            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            var model = MaintenanceOrderOperational.PrepareForEdit(id,status);
            return View(model);
        }
        public ActionResult GetAllTempinvoiceDetailForEditing(int MasterId)

        {
            var TempEditing = MaintenanceOrderOperational.GetAllDetailForEditing(MasterId);
            return PartialView(TempEditing);

        }
        // POST: MaintenanceOrderOperational/Edit/5
        [HttpPost]
        public ActionResult Edit(MaintenanceOrderOperationalVM vm)
        {
            try
            {
                // TODO: Add update logic here

                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var result = MaintenanceOrderOperational.editinvoice(vm, user.Id);
               
                return RedirectToAction("Details", new { id = vm.Id });
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // GET: MaintenanceOrderOperational/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MaintenanceOrderOperational/Delete/5
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
