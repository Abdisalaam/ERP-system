using Accountingsystem.Models;
using Accountingsystem.Utilites;
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
    public class PurchaseInvoiceController : Controller
    {

        public ActionResult IndexPendingPurchSection()
        {
            var models = PurInvoiceMaster.GetAllPendings();
            return View(models);

        }
        public JsonResult CheckCode(string Serial_Number)
        {
            if (PurInvoiceMaster.containCode(Serial_Number))
            {
                var msg = "PUR- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Checkdate(string Invoice_Date)
        {
            if (PurInvoiceMaster.Checkdate(Invoice_Date) == false)
            {
                var msg = "يجب ادخال تاريخ خلال السنة المالية الحالية";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: InvoiceMaster
        public ActionResult Index(string msg,int ?all)
        {
            ViewBag.Message = msg;
            var models = PurInvoiceMaster.GetAllInvoicesProducts(all);
            return View(models);

        }
        public ActionResult IndexAssetPurchase(int ?all)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfAsset(user.Id);
            if (pack.Item2 == true)
            {

                var models = PurInvoiceMaster.GetAllInvoicesAssets(all);
                return View(models);
            }
            else
            {
                var msgo = "باقتك لا تدعم قسم الاصول";
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }
        // GET: InvoiceMaster/Details/5
        public ActionResult Details(int id)
        {
            var invoiceMaster = PurInvoiceMaster.GetInvoiceDetailsById(id);
            return View(invoiceMaster);
        }
        [HttpGet]
        public ActionResult GetAllTempPurchaseInvoiceDetails(int tempPurchaseInvoiceMasterId)
        {
            var purchaseInvoiceDetailVm = PurInvoiceMaster.GetAllTempInvoiceDetails(tempPurchaseInvoiceMasterId);
            return PartialView(purchaseInvoiceDetailVm);
            //return PartialView("PurchaseMasterDetailsList", purchaseInvoiceDetailVm);
        }
        public ActionResult GetAllTempForOrder(string code,int MasterId)
        {
            var purchaseInvoiceDetailVm = PurInvoiceMaster.GetAllTempInvoiceDetailsForOrder(code, MasterId);
            return PartialView(purchaseInvoiceDetailVm);
            //return PartialView("PurchaseMasterDetailsList", purchaseInvoiceDetailVm);
        }
        [HttpPost]
        public virtual JsonResult AddPurchaseInvoiceDetailsToMaster(PurchaseDetailsVM purchaseInvoiceMasterDetail)
        {
            string msg = "";
            try
            {
                msg = PurInvoiceMaster.AddInvoiceDetailsTemp(purchaseInvoiceMasterDetail);

            }
            catch (Exception ex) 
            {
                msg = ex.Message;
                throw;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
            // return PartialView("InvoiceMasterDetailsList", invoiceDetailVm);
        }
        // GET: InvoiceMaster/Create
        public ActionResult CreatePurchaseInv(int? supId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء فاتورة مشتريات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = PurInvoiceMaster.AddNewForCreation(supId);
            return View(model);
        }

       
        [HttpGet]
        public ActionResult PreparePurchaseMasterDetails()
        {
            var purchaseMaster = PurInvoiceMaster.PrepareInvoiceMasterDetail();
            return PartialView("PurchaseDetailCreation", purchaseMaster);
        }
        [HttpGet]
        public JsonResult GetProductPrice(int id)
        {
            var result = PurInvoiceMaster.GetProductPrice(id);
            var Price = new
            {
                result.Prod_Cost_Price,
                result.Tax_Id_Purch
               

            };
            return Json(Price, JsonRequestBehavior.AllowGet);
        }
       // POST: InvoiceMaster/Create
       [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreatePurchaseInv(PurMasterVM masterVM)
        {

            // TODO: Add insert logic here
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

               var msg= PurInvoiceMaster.AddNewInvoice(masterVM , user.Id);
                var id = msg.Item2;
               
                if (id == 0)
                {
                    return RedirectToAction("Index",new {msg=msg.Item1 });
                }
                else
                {
                    return RedirectToAction("Details", new { id = id });
                }

            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                return View();

            }
        }
        [HttpGet]
        public ActionResult GetAllTempPurchaseInvoiceForAssetDetails(int tempPurchaseInvoiceMasterId)
        {
            var purchaseInvoiceForAssetDetailVm = PurInvoiceMaster.GetAllTempInvoiceDetails(tempPurchaseInvoiceMasterId);
            return PartialView(purchaseInvoiceForAssetDetailVm);
            //return PartialView("PurchaseInvoiceForAssetList", purchaseInvoiceForAssetDetailVm);
        }
        [HttpPost]
        public virtual JsonResult AddPurchaseInvoiceForAssetDetailsToMaster(PurchaseDetailsVM purchaseInvoiceMasterDetail)
        {
            string msg = "";
            try
            {
                msg = PurInvoiceMaster.AddInvoiceDetailsTempForAsset(purchaseInvoiceMasterDetail);

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
        public ActionResult PreparePurchaseMasterDetailsForAsset()
        {
            var purchaseMasterForAsset = PurInvoiceMaster.PrepareInvoiceMasterDetailForAsset();
            return PartialView("PurchaseInvoiceDetailsForAsset", purchaseMasterForAsset);
        }
        // GET: InvoiceMaster/Create
        public ActionResult CreateAssPurchaseInv(string msg)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء فاتورة مشتريات اصل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            ViewBag.Message = msg;
            var model = PurInvoiceMaster.AssetAddNewForCreation();
            return View(model);

        }
        // POST: InvoiceMaster/Create
        [HttpPost]
        public ActionResult CreateAssPurchaseInv(PurMasterVM masterVM)
        {
            try
            {

                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

              var boo=  PurInvoiceMaster.AssetOrItemAddNewInvoice(masterVM , user.Id);
                if (boo!=0)
                {
                    return RedirectToAction("Details",new {id=boo });
                }
                else
                {
                    var msg = "يجب اختيار الاصل وادخال كميته و سعرة";
                    return RedirectToAction("CreateAssPurchaseInv",new { msg=msg});
                }
                      
             }
            catch(Exception ex)
            {
                var msg = ex.Message;
                return View();
            }
         }
        // POST: InvoiceMaster/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(PurMasterVM invoiceMasterVM)
        {
            try
            {
                var inv = PurInvoiceMaster.GetInvoiceDetailsById(invoiceMasterVM.Master_Id);
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var employeeAction = true;
                if (inv.Store_ID > 0)
                {
                    employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد فاتورة مشتريات");
                }
                else
                {
                    employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد فاتورة مشتريات اصل");
                }
                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }

                await Task.Delay(2000);
            
                string result = "";
                if(inv.Store_ID !=0)
                {
                    result=  PurInvoiceMaster.UpdateInvoiceMaster(invoiceMasterVM);


                }
                else
                {
                    result= PurInvoiceMaster.UpdateRegularInvoiceMaster(invoiceMasterVM);


                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = false;
            if (PurInvoiceMaster.GetpurchInvByID(id).Store_ID > 0)
            {
                 employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف فاتورة مشتريات");

            }
            else
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف فاتورة مشتريات اصل");
            }

            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            PurInvoiceMaster.DeletePurchaseInvoice(id);

            string mesg = Resources.validate.DeleteSuccessful;
            return RedirectToAction("Index", new { mesg = mesg });
        }
        public JsonResult CheckSerialNum(int supId, string serialNum)
        {
            var model = PurInvoiceMaster.CheckTheSerialNum(supId, serialNum);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPurchInvoiceJson(int proid)
        {
            var purchaseInvoice = PurInvoiceMaster.GetpurchInvByID(proid);
            var xx = new
            {
                purchaseInvoice.Supplier_Id,
                purchaseInvoice.Serial_Number,
                purchaseInvoice.Store_ID,
                purchaseInvoice.Master_Discount,
                purchaseInvoice.Company_Id,
                purchaseInvoice.Invoice_Currency,

            };
            return Json(xx, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult PreparepurchDetails(int serial,decimal discount)
        {
            var invoic = PurInvoiceMaster.PreparpurchMasterDetail2(serial,discount);
            return PartialView("PurchaseDetailCreationEdit", invoic);

        }
        [HttpPost]
        public virtual JsonResult AddInvoiceDetailsToMaster(PurchaseDetailsVM invoiceMasterDetail)
        {
            string msg = "";
            try
            {
                msg = PurInvoiceMaster.AddPurchDetailsTemp(invoiceMasterDetail);

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
            var invoiceDetailVm = PurInvoiceMaster.GetAllTempPURCHInvoiceDetails2(tempInvoiceMasterId);
            return PartialView("PurchMasterDetailList", invoiceDetailVm);
        }
        [HttpGet]
        public ActionResult GetAllTempInvoiceDetailsresult(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = PurInvoiceMaster.GetAllTempPurchInvoiceDetailsresult2(tempInvoiceMasterId);
            return PartialView("PurchaseDetailResultList", invoiceDetailVm);
        }
        [HttpGet]
        public ActionResult EditForPending(int id,int isCopy )
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = true;
            if (isCopy > 0)
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "نسخ فاتورة مشتريات");
            }
            else
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل فاتورة مشتريات");
            }
            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            var editFAsst = PurInvoiceMaster.FillTheViewModel(id,isCopy);
            return View(editFAsst);
        }
        public ActionResult EditForPending(PurMasterVM masterVM)
        {
            if (masterVM.isCopy == 0)
            {
                var editFAsst = PurInvoiceMaster.editinvoice(masterVM);
                var id = editFAsst.Item2;

                if (id == 0)
                {
                    return RedirectToAction("Index", new { msg = editFAsst.Item1 });
                }
                else
                {
                    return RedirectToAction("Details", new { id = id });
                }
            }
            else
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var msg = PurInvoiceMaster.AddNewInvoice(masterVM, user.Id);
                var id = msg.Item2;

                if (id == 0)
                {
                    return RedirectToAction("Index", new { msg = msg.Item1 });
                }
                else
                {
                    return RedirectToAction("Details", new { id = id });
                }

            }
           
        }
        [HttpGet]
        public ActionResult EditForPendingForAsset(int id, int isCopy)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = true;
            if (isCopy > 0)
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "نسخ فاتورة مشتريات اصل");
            }
            else
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل فاتورة مشتريات اصل");
            }
            var editFAsst = PurInvoiceMaster.FillTheViewModel(id, isCopy);
            return View(editFAsst);
        }
        [HttpPost]
        public ActionResult EditForPendingForAsset(PurMasterVM masterVM)
        {
            if (masterVM.isCopy == 0)
            {
                var editFAsst = PurInvoiceMaster.editinvoice(masterVM);
                var id = editFAsst.Item2;

                if (id == 0)
                {
                    return RedirectToAction("Index", new { msg = editFAsst.Item1 });
                }
                else
                {
                    return RedirectToAction("Details", new { id = id });
                }
            }
            else
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var msg = PurInvoiceMaster.AddNewInvoice(masterVM, user.Id);
                var id = msg.Item2;

                if (id == 0)
                {
                    return RedirectToAction("Index", new { msg = msg.Item1 });
                }
                else
                {
                    return RedirectToAction("Details", new { id = id });
                }

            }

        }
        [HttpGet]
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
        [HttpGet]
        public ActionResult GetSupplierDropdownList()
        {
            var supplier = PurInvoiceMaster.GetSupplierList();
            return new JsonResult { Data = supplier, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpGet]
        public ActionResult GetCurrencyDropdownList()
        {
            var cur = PurInvoiceMaster.GetCurrencyList();
            return PartialView("CurrencyDropDownlist", cur);
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
           
            var Prod = PurInvoiceMaster.GetProudectList();
            return new JsonResult { Data = Prod, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpGet]
        public ActionResult AddNewOrderFromOffer(int offerMasterId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تحويل امر شراء الى فاتورة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = PurInvoiceMaster.AddNewOfferForCreation(offerMasterId);
            return View(model);

        }
        [HttpPost]
        public ActionResult AddNewOrderFromOffer(PurMasterVM purVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var msg = PurInvoiceMaster.AddNewInvoice(purVM, user.Id);
                var id = msg.Item2;

                if (id == 0)
                {
                    return RedirectToAction("Index", new { msg = msg.Item1 });
                }
                else
                {
                    return RedirectToAction("Details", new { id = id });
                }

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return View();

            }

        }
        public ActionResult getDetailFromOrder(int Order)
        {
            var orderdetail = PurInvoiceMaster.getDetailFromOrder(Order);
            return PartialView("OrderDetailList", orderdetail);
        }
        public ActionResult PartialDialog3()
        {
            var Fixvm = FixedAsset.createVMForCreation();
            return PartialView("AssetPartialView",Fixvm);
        }
        public ActionResult PartialDialogCurrency()
        {
            var cur = Currency.CreateCurrencyVM();
            return PartialView("PartialDialogCurrency", cur);
        }
        public JsonResult AddNewFixedAssetPartial(FixedAssetVM AssetVM)
        {

            var result = FixedAsset.AddNewAsset(AssetVM);
            var msg = "تم انشاء الاصل";
            //ViewBag.res = result;
            //return PartialView("CustomerPartialView", ViewBag.res);

            return  Json( msg, JsonRequestBehavior.AllowGet );

        }
        public JsonResult AddNewcurrencyPartial(CurrencyVM curVM)
        {

            var result = Currency.CreateCurrency(curVM);
            var msg = "تم انشاء العملة";
            //ViewBag.res = result;
            //return PartialView("CustomerPartialView", ViewBag.res);

            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult GetAssettDropdownList()
        {
            var Prod = PurInvoiceMaster.GetAssetctList();
            return PartialView("AssetDropDownList", Prod);
        }
        [HttpGet]
        public JsonResult DeleteTempDetail(int iD)
        {
            var temp = PurInvoiceMaster.DeleteInvoiceFromTemp(iD);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDueDate(int pay, String StartDate)
        {
            var date = PurInvoiceMaster.MakeDeadLineDate(pay, StartDate);
            return Json(date, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getduedue(string Invoice_Date,int Payment_Policy)
        {
            var Prod = PurInvoiceMaster.GetDueDate1(Invoice_Date, Payment_Policy);
            return PartialView("getduedue", Prod);
            ;
        }
        /// <summary>
        /// Gets all subledgers in the form of Json array in the form specified by 
        /// Select2 (https://select2.org/data-sources/arrays) to populate 
        /// the drop-down list
        /// </summary>
        /// <returns> A Json array in the form specified by Select2  </returns>
        [HttpGet]
        public ActionResult getPurchaseJson()
        {
            List<PurInvoiceMaster> purchases = PurInvoiceMaster.GetAllInvoicesCostDistribution();



            List<dynamic> result = new List<dynamic>();
            foreach (var purchase in purchases)
            {
                var data = new
                {
                    // the value of each select item
                    id = purchase.Master_Id,
                    // showing both code and name in the drop-down list 
                    text = purchase.Serial_Number 
                };
                result.Add(data);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllFiles(int invoiceMasterId)
        {
            var model = PurInvoiceMaster.GetAllFiles(invoiceMasterId);
            return View(model);
        }
        public ActionResult deleteFile(int fileId)
        {

            var document = PurInvoiceMaster.getFileById(fileId);
            var delete = PurInvoiceMaster.deleteFile(fileId);
            return RedirectToAction("Details", new { id = document.PurInvoiceMasterMaster_Id });
        }
        public FileResult ReturnInvoicetFile(int fileId)
        {

            var document = PurInvoiceMaster.getFileById(fileId);
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
        [HttpGet]
        public ActionResult getDetailTtempFirst(int MasterID)
        {
            var supplier = PurInvoiceMaster.GetOneListofDetailsFirst(MasterID);
            return PartialView("PurchaseDetailResultList", supplier);
        }
        public ActionResult getDetailTtempFirstForAsset(int MasterID)
        {
            var supplier = PurInvoiceMaster.GetOneListofDetailsFirst(MasterID);
            return PartialView( supplier);
        }
        [HttpGet]
        public ActionResult getDetailTtempsecond(int MasterID)
        {
            var supplier = PurInvoiceMaster.GetOneListofDetailssecond(MasterID);
            return PartialView("PurchaseDetailResultList", supplier);
        }
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

        public JsonResult updateDataDebendOnDiscount(decimal discount, int id)
        {
            var result = PurInvoiceMaster.changeDetailOfInvoice(discount, id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateDataDebendOnAlowDiscount(decimal discount, int id)
        {
            var result = PurInvoiceMaster.changeDetailOfInvoiceForAllDiscount(discount, id);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

    }
}