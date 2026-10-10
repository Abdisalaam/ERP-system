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
    public class InvTreansMasterController : Controller
    {
        // GET: InvTreansMaster
        public ActionResult Index(string mesg,int ?all)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfTrans(user.Id);
            if (pack.Item2 == true)
            {
                ViewBag.Message = mesg;
                var model = InventoryTransfer_Hd.getAllInvTransferes();
                if (all == null)
                {
                    model.Reverse();
                    model = model.Take(100).ToList();
                }
                return View(model);
            }
            else
            {
                var msgo = "باقتك لا تدعم هذا التحويل من مخزن لمخزن لعدم وجود اكثر من مخزن";
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }

        // GET: InvTreansMaster/Details/5
        public ActionResult Details(int id)
        {
            var model = InventoryTransfer_Hd.GetInventoryById(id);
            return View(model);
        }

        // GET: InvTreansMaster/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء تحويل مخزني");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var Inv = InventoryTransfer_Hd.CreateTransferForCreation();
            return View(Inv);
        }
        // POST: InvTreansMaster/Create
        [HttpPost]
        public ActionResult Create(InventoryTransfer_HdVM masterVM)
        {
            //try
            //{
            // TODO: Add insert logic here

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

        
          var id=  InventoryTransfer_Hd.CreateNewInvTransfere(masterVM , user.Id);
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Details", new { id = id });
            }
            //}
            //catch
            //{
            //    return View();
            //}
        }

        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل تحويل مخزني");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var Transfer = InventoryTransfer_Hd.fillTransfer(id);
            //  return RedirectToAction("Index", "CompanyDepartments", new { id = 5 });
            return View(Transfer);
        }
        // POST: InvTreansMaster/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> EditAsync(InventoryTransfer_HdVM TransVM)
        {
            
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد تحويل مخزني");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                // TODO: Add update logic here
                //string result = "Success! Transfer is updated and the GLs are created";
                await Task.Delay(2000);
               InventoryTransfer_Hd.UpdateInvoiceTransfer(TransVM);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public virtual JsonResult GetProducts(int storeId)
        {
            var products = InvoiceMaster.getStoreProducts(storeId);
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف تحويل مخزني");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            InventoryTransfer_Hd.DeleteInvTransfer(id);
            string mesg = "تم الحذف بنجاح";
            return RedirectToAction("Index", new { mesg = mesg });
        }


        [HttpGet]
        public ActionResult PrepareInventoryeH_dDetail(int storeId , int InventoryTrans)
        {
            var InventoryDetailsVm = InventoryTransfer_Hd.PrepareInventoryeH_dDetail(storeId, InventoryTrans);
            return PartialView("InventoryDetailsPartial", InventoryDetailsVm);
        }
        [HttpGet]
        public virtual JsonResult GetProductAvailableQty(int batchId)
        {

            try 
            {
                decimal availableQty = InventoryTransfer_Hd.GetAvailbleProductQty(batchId);
                return Json(availableQty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {


                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]

        //For add details
        public virtual JsonResult AddInventoryDetailsToMaster(InventorytransferHdDetailsVM inventoryH_dDetail)
        {
            string msg = "";
            try
            {
                msg = InventoryTransfer_Hd.AddTransDetailsTemp(inventoryH_dDetail);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }
            return Json(msg,JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteTempDetail(int iD)
        {
            var temp = InventoryTransfer_Hd.DeleteTemp(iD);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllTempInventoryDetails(int tempInventoryH_dId,int delete)
        {
            var inventoryDetailsVm = InventoryTransfer_Hd.GetAllTempInventoryeDetails(tempInventoryH_dId,delete);
            return PartialView("InventoryH_dDetailsList", inventoryDetailsVm);
        }

        [HttpGet]
        //for button details
        public ActionResult GetAllInventoryeDetailresult(int TemptH_dDetailes)
        {
            var H_dDetailVm = InventoryTransfer_Hd.GetAllTempInventoryeDetailresult(TemptH_dDetailes);
            return PartialView("InventoryHdList", H_dDetailVm);
        }

        public async System.Threading.Tasks.Task<ActionResult> Update(InventoryTransfer_HdVM iventoryVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد تحويل مخزني");



                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }
                await Task.Delay(2000);
                string result = InventoryTransfer_Hd.UpdateInvoiceTransfer(iventoryVM);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                var data = ex.Data;
                return View();
            }

        }
        public JsonResult GetQuantity(int Id)
          {

          var productQuantity = RecalPurchInvoiceMaster.GetQuantity(Id);

            return Json(productQuantity, JsonRequestBehavior.AllowGet);

         }
   
        //[HttpPost]
        //for add details edit
        //public virtual JsonResult AddInvoiceDetailsToMaster(InventorytransferHdDetailsVM invoiceMasterDetail)
        //{
        //    string msg = "";
        //    try
        //    {
        //        msg = InventoryTransfer_Hd.AddTransDetailsTemp(invoiceMasterDetail);

        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ex.Message;
        //        throw;
        //    }



        //    return Json(msg, JsonRequestBehavior.AllowGet);

        //}
        [HttpGet]
        //for تحرير
        public ActionResult PrepareTransDetails(int serial)
        {
            var invoic = InventoryTransfer_Hd.PrepareInventoryeH_dDetail2(serial);
            return PartialView("InventoryDetailsPartial", invoic);

        }









        /// <summary>
        /// All methods for Edit
        /// </summary>
        /// <param name="transId"></param>
        /// <returns></returns>

        [HttpGet]
        //for list from details
        public ActionResult PrepareTransferDetails(int transfId)
        {
            var transf = InventoryTransfer_Hd.PreparTransferDetail2(transfId);
            return PartialView("TransferDetailsCreationEdit", transf);

        }


        [HttpGet]
        public ActionResult GetAllTempTransferDetailsEdit(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = InventoryTransfer_Hd.GetAllTempTransferDetails2(tempInvoiceMasterId);
            return PartialView("TransferDetailsForEdit", invoiceDetailVm);
        }
        [HttpGet]
        //add details to list again after edit
        public ActionResult GetAllTempInvoiceDetailsresult(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = InventoryTransfer_Hd.GetAllTempTransfDetailsresult2(tempInvoiceMasterId);
            return PartialView("TransferDetailResultList", invoiceDetailVm);
        }
        [HttpGet]
        public ActionResult EditForPending(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل تحويل مخزني");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var editFAsst = InventoryTransfer_Hd.fillForEditingTransfer(id);
            return View(editFAsst);
        }
        public ActionResult EditForPending(InventoryTransfer_HdVM transfVM)
        {
            var transf = InventoryTransfer_Hd.EditTransfer(transfVM);
            if (transf == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Details",new { id=transf });
            }
        }

        public ActionResult GetAllTempTransfDetailsresultForEdit(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = InventoryTransfer_Hd.GetAllTempTransfDetailsresultForEdit(tempInvoiceMasterId);
            return PartialView( invoiceDetailVm);
        }
        public ActionResult getProductBatchJsondrobDown()
        {
            //var Products = Product.getAllproductbatchFordrobDownList();
            List<dynamic> result = new List<dynamic>();
            //foreach (var pro in Products)
            //{
            //    var data = new
            //    {
            //        // the value of each select item
            //        id = pro.Product_ID,
            //        // showing both code and name in the drop-down list 
            //        text = pro.Product.Prod_Name 
            //    };
            //    result.Add(data);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult GetStore(int store)
        {
            var my = InventoryTransfer_Hd.getAllExceptStore(store);
            return Json(my, JsonRequestBehavior.AllowGet);
        }


    }
}


