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
    public class TransferToProjectMasterController : Controller
    {

        public JsonResult CheckCode(string Trans_Code)
        {
            if (!TransferToProjectMaster.containCode(Trans_Code))
            {
                var msg = "TTPRO- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Checkdate(string Trans_Date)
        {
            if (TransferToProjectMaster.Checkdate(Trans_Date) == false)
            {
                var msg = "يجب ادخال تاريخ خلال السنة المالية الحالية";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: TransferToProjectMaster
        public ActionResult Index(string mesg,int ?all)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfPoject(user.Id);
            if (pack.Item2 == true)
            {
                ViewBag.Message = mesg;
                var models = TransferToProjectMaster.GetAllSalesReceipt();
                if (all == null)
                {
                    models.Reverse();
                    models = models.Take(100).ToList();
                }
                return View(models);
            }
            else
            {
                var msgo = Resources.validate.YourpackagedoesnotsupporttheProdepartment;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }
        public ActionResult IndexoftranstoProject(int?all)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfPoject(user.Id);
            if (pack.Item2 == true)
            {
                var models = TransferToProjectMaster.GetAlltransFromProject();
                if (all == null)
                {
                    models.Reverse();
                    models = models.Take(100).ToList();
                }
                return View(models);
            }
            else
            {
                var msgo = Resources.validate.YourpackagedoesnotsupporttheProdepartment;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }

        // GET: TransferToProjectMaster/Details/5
        public ActionResult Details(int id)
        {
            var models = TransferToProjectMaster.GetTransferProjectById(id);
            return View(models);
        }
        public ActionResult Detailstrans(int id)
        {
            var models = TransferToProjectMaster.GetTransferProjectById(id);
            return View(models);
        }

        // GET: TransferToProjectMaster/Create
        public ActionResult addnewTransToProj()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء تحويل من مخزن الى مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var Invoice = TransferToProjectMaster.AddNewTransferForCreation();
            return View(Invoice);
        }

        public ActionResult addnewTransToStor()
        {
            var Invoice = TransferToProjectMaster.AddNewTransferForCreationtoStore();
            return View(Invoice);
        }
        [HttpPost]
        public ActionResult addnewTransToProj(TransferToProjectMasterVM mastervm)
        {
            try
            {
                // TODO: Add insert logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var result = TransferToProjectMaster.addnewSaleReciept(mastervm , user.Id);
                if (result.Item2 == 0)
                {
                    return RedirectToAction("Index", new {msg=result.Item1 });
                }
                else
                {
                    return RedirectToAction("Details", new { id = result.Item2 });
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult PrepareInvoiceDetails(int storeId)
        {
            var invoiceDetailVm = TransferToProjectMaster.PrepareInvoiceMasterDetail(storeId);
            return PartialView("PrepareInvoiceDetails", invoiceDetailVm);
        }
        public virtual JsonResult GetProductAvailableQty(int batchId)
        {

            try
            {
                decimal availableQty = TransferToProjectMaster.GetAvailbleProductQty(batchId);
                return Json(availableQty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {


                return Json(ex, JsonRequestBehavior.AllowGet);
            }


        }
        public virtual JsonResult AddInvoiceDetailsToMaster(TransferToProjectDetailVM invoiceMasterDetail)
        {
            string msg = "";
            try
            {
                msg = TransferToProjectMaster.AddInvoiceDetailsTemp(invoiceMasterDetail);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                 var mol = ex.Data;
                throw;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllTempInvoiceDetails(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = TransferToProjectMaster.GetAllTempTransferDetails(tempInvoiceMasterId);
            return PartialView("GetAllTempInvoiceDetails", invoiceDetailVm);
        }
        public async System.Threading.Tasks.Task<ActionResult> Edit(TransferToProjectMasterVM salesinvoiceMasterVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد تحويل من مخزن الى مشروع");



                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }
                
                await Task.Delay(2000);
                string result = TransferToProjectMaster.UpdateInvoiceMaster(salesinvoiceMasterVM);

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
        public async System.Threading.Tasks.Task<ActionResult> Edittrans(TransferToProjectMasterVM salesinvoiceMasterVM)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد تحويل من مشروع الى مخزن");



            if (employeeAction == false)
            {
                return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
            }
            try
            {
                await Task.Delay(2000);
                string result = TransferToProjectMaster.UpdateInvoiceMasterTrans(salesinvoiceMasterVM);

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
        public ActionResult PrepareTransDetails(int masterId)
        {
            var invoic = TransferToProjectMaster.PrepareTransDetail2(masterId);
            return PartialView("TransDetailsrCreationEdit", invoic);

        }
        public ActionResult GetAllTempTransDetailsEdit(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = TransferToProjectMaster.GetAllTempTransferDetails2(tempInvoiceMasterId);
            return PartialView("TransDetailsForEdit", invoiceDetailVm);
        }
        public ActionResult GetAllTempInvoiceDetailsresult(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = TransferToProjectMaster.GetAllTempTransDetailsresult2(tempInvoiceMasterId);
            return PartialView("TransDetailResultList", invoiceDetailVm);
        }
        public ActionResult EditForPending(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل تحويل من مخزن الى مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var editTrans = TransferToProjectMaster.fillForEditingTransfer(id);
            return View(editTrans);
        }
        [HttpPost]
        public ActionResult EditForPending(TransferToProjectMasterVM masterVM)
        {
          

            if (masterVM.Trans_Code.Contains("TTPRO"))
            {
                var editTrans = TransferToProjectMaster.editinvoice(masterVM);
                if (editTrans.Item2 == 0)
                {
                    return RedirectToAction("Index",new {msg= editTrans.Item1 });
                }
                else
                {
                    return RedirectToAction("Details", new { id = editTrans.Item2 });
                }
            }
            else
            {
                var editTranstostroe = TransferToProjectMaster.editinvoicetostore(masterVM);

                if (editTranstostroe.Item2 == 0)
                {
                    return RedirectToAction("IndexoftranstoProject", new { msg = editTranstostroe.Item1 });
                }
                else
                {
                    return RedirectToAction("Detailstrans", new { id = editTranstostroe.Item2 });
                }
              
            }
          
           
        }
        public ActionResult GetAllFiles(int invoiceMasterId)
        {
            var model = TransferToProjectMaster.GetAllFiles(invoiceMasterId);
            return View(model);
        }
        public FileResult ReturnInvoicetFile(int fileId)
        {

            var document = TransferToProjectMaster.getFileById(fileId);
            var cd = new System.Net.Mime.ContentDisposition
            {

                FileName = document.FileName,

                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.FileContent, document.FileType);
        }
        //////////// This  Code  is  to  Transfare  from  Project  to  Store

        public ActionResult CreateForTFPS()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء تحويل من مشروع الى مخزن");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var thisList = TransferToProjectMaster.AddNewTransferForCreationtoStore();
            return View(thisList);
        }
        [HttpPost]
        public ActionResult CreateForTFPS(TransferToProjectMasterVM TVM)
        {
       
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var msg = TransferToProjectMaster.saveTransfaretoprogectMaster(TVM , user.Id);
                if (msg.Item2 == 0)
                {
                    return RedirectToAction("IndexoftranstoProject", new { msg = msg.Item1 });
                }
                else
                {
                    return RedirectToAction("Detailstrans", new { id = msg.Item2 });
                }

            }
            catch (Exception ex)
            {
                var a = ex.Message;
                throw;
            }
      
        }
        public ActionResult GetListOFAllTransDetails(int projectID)
        {
            var invoiceDetailVm = TransferToProjectMaster.getListProjectTransDetail(projectID);
            return PartialView("GetListOFAllTransDetails", invoiceDetailVm);
        }

        public ActionResult PrepareTheQuantityOFProduct(int DetailID)
        {
            var invoiceDetailVm = TransferToProjectMaster.PrepareTransDetailtoBatch(DetailID);
            return PartialView("PrepareTheQuantityOFProduct", invoiceDetailVm);
        }
        public ActionResult IndexOFSavedTemp()
        {
            var models = TransferToProjectMaster.getAllSavedData();
            return View(models);
        }
        [HttpPost]
        public virtual JsonResult SaveDatatoPermTemp(TransferToProjectDetailVM TVM)
        {
            string msg = "";
            try
            {
                msg = TransferToProjectMaster.SaveDataToPermenantTemp(TVM);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public virtual JsonResult SaveData(TransferToProjectMasterVM TVM)
        {
            var msg = "";
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var a = TransferToProjectMaster.saveTransfaretoprogectMaster(TVM , user.Id);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getlistofPermtemp(int Master_Id)
        {
            var invoiceDetailVm = TransferToProjectMaster.GetListOFProjectsintemp(Master_Id);
            return PartialView("getlistofPermtemp", invoiceDetailVm);
        }
        [HttpPost]
        public virtual JsonResult DeleteDatatoPermTemp(int ID)
        {
            string msg = "";
            try
            {
                msg = TransferToProjectMaster.DeleteFromTempofPermenant(ID);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual JsonResult DeleteDataFromTemp(int ID)
        {
            string msg = "";
            try
            {
                msg = TransferToProjectMaster.DeleteFromTempProjectToStore(ID);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDetails(int id)
        {
            var models = TransferToProjectMaster.gettempbyID(id);
            return View(models);
        }
        [HttpPost]
        public JsonResult UpdateDetails(TransfareFromPorjectToStoretemp TFPS)
        {
            string msg = "";
            try
            {
                TransferToProjectMaster.UpdateInvoicePermtem(TFPS);
                msg = "تم التحويل الي المخزن";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteTemps(int id)
        {
            var models = TransferToProjectMaster.gettempbyID(id);
            return View(models);
        }
        [HttpPost]
        public ActionResult DeleteTemps(int id, TransfareFromPorjectToStoretemp ids)
        {

            var models = TransferToProjectMaster.DeleteFromTempofPermenant(id);
            return RedirectToAction("IndexOFSavedTemp");
        }

        public JsonResult DeleteTempDetail(int iD)
        {
            var temp = TransferToProjectMaster.DeleteInvoiceFromTemp(iD);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
        //////////////////////////////////Edit for Transfer from project to store

        public ActionResult PrepareTransDetailsToStore(int masterId)
        {
            var invoic = TransferToProjectMaster.PrepareTransDetailToStore(masterId);
            return PartialView("TransDetailsrCreationEdit", invoic);

        }
        public ActionResult GetAllTempTransDetailsEditToStore(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = TransferToProjectMaster.GetAllTempTransferDetailsToStore(tempInvoiceMasterId);
            return PartialView("TransDetailsForEdit", invoiceDetailVm);
        }
        public ActionResult GetAllTempInvoiceDetailsresultToStore(int tempInvoiceMasterId)
        {
            var invoiceDetailVm = TransferToProjectMaster.GetAllTempTransDetailsresultToStore(tempInvoiceMasterId);
            return PartialView("TransDetailResultList", invoiceDetailVm);
        }
        public ActionResult EditForPendingToStore(int id)
        {
            var editTrans = TransferToProjectMaster.fillForEditingTransferToStore(id);
            return View(editTrans);
        }
        [HttpPost]
        public ActionResult EditForPendingToStore(TransferToProjectMasterVM masterVM)
        {
            var editTrans = TransferToProjectMaster.editinvoice(masterVM);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
           var trans= TransferToProjectMaster.GetTransferProjectById(id);
            var employeeAction = true;
            if (trans.Trans_Code.StartsWith("TTPRO-"))
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف تحويل من مخزن الى مشروع");
            }
            else
            {
                 employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف تحويل من مشروع الى مخزن");
            }
            



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = TransferToProjectMaster.DeleteTransferToProject(id);
            return RedirectToAction("Index", new { msg = msg });
        }

        public virtual JsonResult GetDepartment(int departId)
        {
            var my = TransferToProjectMaster.GetDepartment(departId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetBussines(int BussinesId)
        {
            var my = TransferToProjectMaster.GetBussines(BussinesId);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
    }
}