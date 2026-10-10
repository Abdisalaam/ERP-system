using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Models;
using Accountingsystem.Utilites;
using Accountingsystem.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class TransferToMaintenanceController : Controller
    {


        public JsonResult CheckCode(string Trans_Code)
        {
            if (TransferToMaintenance.ContainCode(Trans_Code))
            {
                var msg = "TMain- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);

        }

        // GET: TransferToMaintenance
        public ActionResult Index(string Msg,int ?all)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfSubscribtion(user.Id);
            if (pack.Item2 == true)
            {
                ViewBag.Msg = Msg;
                var tran = TransferToMaintenance.GetAllTransferToMaintenance();
                if (all == null)
                {
                    tran.Reverse();
                    tran = tran.Take(100).ToList();
                }
                return View(tran);
            }
            else
            {
                var msgo = Resources.validate.Yourpackagedoesnotsupportthesubdepartment1;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }

        // GET: TransferToMaintenance/Details/5
        public ActionResult Details(int id)
        {
            var trans = TransferToMaintenance.GetAllTransferToMaintenanceById(id);
            return View(trans);
        }

        // GET: TransferToMaintenance/Create
        public ActionResult AddNewTranfer()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء تحويل من مخزن لاشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var transfer = TransferToMaintenance.CreateForCreation();
            return View(transfer);
        }

        // POST: TransferToMaintenance/Create
        [HttpPost]
        public ActionResult AddTransferTOmain(TransferToMaintenanceVM transVM)
        {
            try
            {
                // TODO: Add insert logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var result = TransferToMaintenance.CreateNewTransfer(transVM , user.Id);
                if (result.Item2 != 0)
                {
                    return RedirectToAction("Details", new { id = result.Item2 });
                }
                else
                {
                    return RedirectToAction("Index", new { Msg = result.Item1 });
                }
            }
            catch(Exception  x)
            {
              var  msg = x.Message;
              var  mg = x.Data;
                return View();
            }
        }


        public virtual JsonResult GetProductAvailableQty(int batchId)
        {
            try
            {
                int availableQty = TransferToMaintenance.GetAvailbleProductQty(batchId);
                return Json(availableQty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception s)
            {

                return Json(s, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PreperInvoiceDetails(string storId)
        {
            var preper = TransferToMaintenance.PreperInvoiceMasterDetail(storId);
            return PartialView("PreperInvoiceDetails", preper);
        }


      
        public virtual JsonResult AddInvoiceDetailToMaster(TransferToMaintenanceDetailsVM invoiceMasterDetail)
        {
            string msg = "";

            try
            {
                msg = TransferToMaintenance.AddInvoiceDetailTemp(invoiceMasterDetail);
            }
            catch (Exception m)
            {

                msg = m.Message;
                var mol = m.Data;
                throw;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetAllTempInvoiceDetails(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = TransferToMaintenance.GetAllTransferDetails(tempInvoiceMasterId);
            return PartialView("GetAllTempInvoiceDetails", invoiceDetailVm);
        }
        public virtual ActionResult DeleteTemp(int id)
        {
            var temp = TransferToMaintenance.delete_Temp_Record(id);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllTempInvoiceDetailsForEditing(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = TransferToMaintenance.GetAllDetailForEditing(tempInvoiceMasterId);
            return PartialView("GetAllTempInvoiceDetailsForEditing", invoiceDetailVm);
        }

        [HttpGet]
        public virtual JsonResult GetDepart(int BranchID)
        {
            var my = TransferToMaintenance.GetDepartment(BranchID);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

         [HttpGet]
        public virtual JsonResult GetBesniss(int Besniss)
        {
            var my = TransferToMaintenance.GetBesniessUnet(Besniss);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        // GET: TransferToMaintenance/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TransferToMaintenance/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(TransferToMaintenanceVM MaintenanceVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد تحويل من مخزن لاشتراك");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                // TODO: Add update logic here
                await Task.Delay(2000);
                string result = TransferToMaintenance.UpdateInvoiceMaster(MaintenanceVM);
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

        public ActionResult EditForPending(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل تحويل من مخزن لاشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var editTrans = TransferToMaintenance.fillForCreation(id);
            return View(editTrans);
        }




        public ActionResult PrepareTransDetails(int masterId)
        {
            var invoic = TransferToMaintenance.PreperTransferMain2(masterId);
            return PartialView("TransDetailsrCreationEdit", invoic);

        }


        [HttpGet]
        public ActionResult GetAllTempTransDetailsEdit(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = TransferToMaintenance.GetAllTempTransferDetails2(tempInvoiceMasterId);
            return PartialView("TransDetailsForEdit", invoiceDetailVm);
        }



        public ActionResult GetAllTempInvoiceDetailsresult(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = TransferToMaintenance.GetAllTempTransDetailsresult(tempInvoiceMasterId);
            return PartialView("TransDetailResultList", invoiceDetailVm);
        }



        [HttpPost]
        public ActionResult EditForPending(TransferToMaintenanceVM masterVM)
        {
            var editTrans = TransferToMaintenance.EditMaintanens(masterVM);
            if (editTrans == null)
            {
                return RedirectToAction("Index", new { Msg = Resources.validate.Error });
            }
            return RedirectToAction("Details", new { id = editTrans.Master_Id });
        }

        //delete method 
        public ActionResult Delete(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف تحويل من مخزن لاشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            TransferToMaintenance.DeleteMaintaince(id);
            string mesg = Resources.validate.DeleteSuccessful;
            return RedirectToAction("Index", new { mesg = mesg });
        }
    }
}
