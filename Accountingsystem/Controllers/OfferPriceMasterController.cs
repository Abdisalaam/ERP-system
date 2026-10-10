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
 
    public class OfferPriceMasterController : Controller
    {
        public JsonResult CheckCode(string Serial_Number)
        {
            if (OfferPriceMaster.containCode(Serial_Number))
            {
                var msg = Resources.OfferPriceMasterCreateOffer.OfferCodeExisit;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependOnquantity(int detailId, decimal quantity, decimal masterDiscount)
        {


            var msg = OfferPriceMaster.changeSpecificValueinPOs(detailId, quantity, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependOnPrice(int detailId, decimal Price, decimal masterDiscount)
        {


            var msg = OfferPriceMaster.changeSpecificPrice(detailId, Price, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddingTax(List<string> LisOfTaxa, int MasterIda)
        {


            var msg = OfferPriceMaster.AddmuchTax(LisOfTaxa, MasterIda);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updatePOSDependTax(int detailId, int Tax, decimal masterDiscount)
        {


            var msg = OfferPriceMaster.changeSpecificTax(detailId, Tax, masterDiscount);


            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        // GET: OfferPriceMaster
        public ActionResult Index(string msg,int ?all)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var offer = OfferPriceMaster.getALLOfferMasr(user,all);
            ViewBag.Message = msg;
            return View(offer);

        }
        public ActionResult IndexOfTrueState(int ?all)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var offer = OfferPriceMaster.getALLOfferMasrTrueState(user,all);
            return View(offer);
        }
        // GET: OfferPriceMaster/Details/5
        public ActionResult Details(int id,string msg)
        {
            var offer = OfferPriceMaster.getDetailById(id);
           
            if (offer == null)
            {
                 msg = Resources.OfferPriceMasterDelete.OfferIsNotFoundControllerMessage + " " + id.ToString();
                return RedirectToAction("Index", new { msg = msg });
            }
            else
            {
                ViewBag.Message = msg;
                return View(offer);
            }
          
        }

       
        public ActionResult Create()
        {
        
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء عرض سعر");
            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            var offer =OfferPriceMaster.AddNewForCreation(user.Id);
            return View(offer);
        }

        // POST: OfferPriceMaster/Create
        [HttpPost]
        public ActionResult Create(OfferPriceMasterVM offerVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                string result = "Success! offer is added sucessfully";
                OfferPriceMaster.AddNewOffer(offerVM , user.Id);
                return Json(result, JsonRequestBehavior.AllowGet);

             
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]        
        public ActionResult CreateOffer()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء عرض سعر");
            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var offer = OfferPriceMaster.AddNewForCreation(user.Id);
            if (offer != null)
            {
                return View(offer);
            }
            else
            {
                var msg = Resources.OfferPriceMasterCreateOffer.OfferTechErrorControllerMessage;
                return RedirectToAction("Index", new { msg = msg });
            }
          
        }

        // POST: OfferPriceMaster/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOffer(OfferPriceMasterVM offerVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

               var id=  OfferPriceMaster.AddNewOffer(offerVM , user.Id);
                if (id == 0)
                {
                    
                   var msg = Resources.OfferPriceMasterCreateOffer.OfferNotCreatedControllerMessage;
                    return RedirectToAction("Index", new { msg = msg });
                }
                else
                {
                    return RedirectToAction("Details", new {id=id });
                }


            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message + "____" + Resources.OfferPriceMasterCreateOffer.OfferDataNotCorrectControllerMessage;
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                offerVM = OfferPriceMaster.AddNewForCreation(user.Id);
                return View(offerVM);
            }
        }

        [HttpGet]
        public ActionResult PrepareInvoiceDetails(int storeId, int offerMasterId)
        {
            var invoiceDetailVm = OfferPriceMaster.PrepareInvoiceMasterDetail(storeId, offerMasterId);
            return PartialView("InvoiceDetailsPartial", invoiceDetailVm);
        }
        [HttpPost]
        public virtual JsonResult AddInvoiceDetailsToMaster(InvoiceMasterDetailsVM invoiceMasterDetail)
        {
            string msg = "";
            try
            {
                msg = OfferPriceMaster.AddInvoiceDetailsTemp(invoiceMasterDetail);
                return Json(msg, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAllTempInvoiceDetails(int offerMasterId)
        {
            var invoiceDetailVm = OfferPriceMaster.GetAllTempInvoiceDetails(offerMasterId);
            return PartialView(invoiceDetailVm);
        }

      //  InvoiceMasterDetailsVM
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
              
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: OfferPriceMaster/Delete/5
        public ActionResult Delete(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف عرض سعر");
            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var offer = OfferPriceMaster.getDetailById(id);
             if (offer == null)
             {
                var msg = Resources.OfferPriceMasterDelete.OfferIsNotFoundControllerMessage;
               return RedirectToAction("Index", new { msg = msg });
             }
           else
             {
                 return View(offer);
             }
        }

        // POST: OfferPriceMaster/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف عرض سعر");
                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var offer = OfferPriceMaster.deleteOffer(id);
                var msg = "";
                if (offer == 1)
                {
                    msg = Resources.OfferPriceMasterDelete.OfferIsDeletedControllerMessage;

                    return RedirectToAction("Index", new { msg = msg });
                }
                else
                {
                    msg = Resources.OfferPriceMasterDelete.OfferCanNotBeDeletedControllerMessage;


                    return RedirectToAction("Index", new { msg = msg });
                }

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message + "__" + Resources.OfferPriceMasterDelete.OfferTechErrorControllerMessage;
                var offer = OfferPriceMaster.getDetailById(id);
                return View(offer);
            }
        }

    


        [HttpGet]
        public virtual JsonResult GetProducts(int storeId)
        {
            var products = OfferPriceMaster.getStoreProducts(storeId);
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditForOffer(int id,int isCopy)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string name = "تعديل عرض سعر";
            if(isCopy!=0&& isCopy != null)
            {
                name = "نسخ عرض سعر";
            }
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, name);
            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var editOffer = OfferPriceMaster.FillTheOfferViewModel(id,isCopy);
            var msg = "";
            if (editOffer != null)
            {
                return View(editOffer);
            }
            else
            {
                msg = Resources.OfferPriceMasterEditForOffer.OfferIsNotFoundControllerMessage;
                return RedirectToAction("Index", new { msg = msg });
            }
           
        }
        public ActionResult EditForOffer(OfferPriceMasterVM masterVM)
        {
            var msg = "";
            try
            {
                if (masterVM.isCopy == 0)
                {
                    var id = OfferPriceMaster.editOffer(masterVM);
                   
                    if (id == 0)
                    {
                        msg = Resources.OfferPriceMasterEditForOffer.OfferIsNotBeenEditedControllerMessage;
                        return RedirectToAction("Index", new { msg = msg });
                       
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = id });
                    }
                }
                else
                {
                    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                    var id = OfferPriceMaster.AddNewOffer(masterVM, user.Id);
                    if (id == 0)
                    {
                        msg = Resources.OfferPriceMasterEditForOffer.OfferIsNotBeenEditedControllerMessage;
                        return RedirectToAction("Index", new { msg = msg });

                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = id });
                    }
                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message + "___" + Resources.OfferPriceMasterEditForOffer.OfferTechErrorControllerMessage;



                masterVM = OfferPriceMaster.FillTheOfferViewModel(masterVM.OfferMas_Id, masterVM.isCopy);
                return View(masterVM);
            }

        }
        [HttpGet]
        public ActionResult GetAllOFFerDetails(int masterOfferID)
        {
            var invoiceDetailVm = OfferPriceMaster.GetAllofferDetails2(masterOfferID);
            return PartialView("GetAllOFFerDetails", invoiceDetailVm);
        }
        [HttpGet]
        public ActionResult GetAllTempOfferresult(int OFferId)
        {
            var invoiceDetailVm = OfferPriceMaster.GetAllTempDetailsresult2(OFferId);
            return PartialView("GetAllTempOfferresult", invoiceDetailVm);
        }
        [HttpGet]
        public ActionResult PreparofferDetails(int serial)
        {
            var invoic = OfferPriceMaster.PreparOfferPriceDetailVMEdit(serial);
            return PartialView("PreparofferDetails", invoic);

        }
        [HttpPost]
        public virtual JsonResult AddOfferDetailsToMaster(OfferPriceDetailVM offerDetail)
        {
            string msg = "";
            try
            {
                msg = OfferPriceMaster.AddofferDetailsTemp(offerDetail);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }



            return Json(msg, JsonRequestBehavior.AllowGet);

        }



        [HttpGet]
        public ActionResult PartialDialog()
        {
            var custvm = Customer.AddCustomerVM();
            return PartialView("CSTpartialView", custvm);
        }

        [HttpGet]
        public ActionResult GetProductPartialDialog()
        {
            var productVM = Product.CreateVMForCreation();           
            return PartialView("ProductPartialView", productVM);
        }

        [HttpGet]
        public ActionResult GetProductPartialDialogNoModel()
        {
            var productVM = Product.CreateVMForCreation();
            ViewBag.Model = productVM;
            ViewBag.ProdID = productVM.Prod_ID;
            ViewBag.ProdCode = productVM.Prod_Code;
            return PartialView("ProductPartialView", productVM);
        }

        [HttpPost]        
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

        [HttpPost]       
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
        [HttpGet]
        public ActionResult GetCustomerDropdownList()
        {
            var customer = OfferPriceMaster.GetCustomerList();
            return new JsonResult { Data = customer, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetProductDrobDownList()
        {
            var customer = OfferPriceMaster.GetProductList();
            return new JsonResult { Data = customer, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetCustomerDropdownListbyid(int offerMasterId)
        {
            var customer = OfferPriceMaster.GetCustomerListbYid(offerMasterId);
            return PartialView("CSTdropDownList", customer);
        }


        [HttpPost]
        public ActionResult DeleteITem(int ids)
        {

            var result = OfferPriceMaster.deleteItemFomDetails(ids);
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public ActionResult makeDeadLine(int pay,String StartDate)
        {
            var date = OfferPriceMaster.MakeDeadLineDate(pay, StartDate);
            return Json(date, JsonRequestBehavior.AllowGet);


        }

        public JsonResult updateDataDebendOnDiscount(decimal discount, int id)
        {
            var result = OfferPriceMaster.changeDetailOfInvoice(discount, id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getProductBatchJsondrobDown()
        {
            
            List<dynamic> result = new List<dynamic>();
           
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
