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
    public class SubscriptionController : Controller
    {
        public ActionResult GetAllReports()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfSubscribtion(user.Id);
            if (pack.Item2 == true)
            {
                return View();
            }
            else
            {
                var msgo = "باقتك لا تدعم قسم الاشتركات";
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }
        public JsonResult CheckCode(string Subscrip_Code)
        {
            if (Subscription.containCode(Subscrip_Code))
            {
                var msg = "SUBS- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckEndDate(string startdate,int TypeSub)
        {

            var models = Subscription.GetEndDateSub(startdate, TypeSub);


            return Json(models, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckCodeInvoice(string Serial_Number)
        {
            if (Subscription.containCodes(Serial_Number))
            {
                var msg = "SUBINV- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: Subscription
        public ActionResult Index(string Msg,int ?all)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfSubscribtion(user.Id);
            if (pack.Item2 == true)
            {
                ViewBag.Msg = Msg;
                Subscription.changestate();
                var models = Subscription.GetAllSubscription();
                if (all == null)
                {
                    models.Reverse();
                    models = models.Take(100).ToList();
                }

                return View(models);
            }
            else
            {
                var msgo = Resources.validate.Yourpackagedoesnotsupportthesubdepartment1;
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }
        public ActionResult IndexSubPending(string Msg,int ?all)
        {
            ViewBag.Msg = Msg;
            Subscription.changestate();
            var models = Subscription.GetAllSubscriptionPending();
            if (all == null)
            {
                models.Reverse();
                models = models.Take(100).ToList();
            }
            return View(models);
        }
        public ActionResult IndexSubVoid(string Msg,int?all)
        {
            ViewBag.Msg = Msg;
            Subscription.changestate();
            var models = Subscription.GetAllSubscriptionVoid();
            if (all == null)
            {
                models.Reverse();
                models = models.Take(100).ToList();
            }
            return View(models);
        }
        public ActionResult IndexOfInvoices(int ?all,string msg)
        {

            var models = Subscription.getallInvoices();
            if (all == null)
            {
                models.Reverse();
                models = models.Take(100).ToList();
            }
            ViewBag.Message = msg;
            return View(models);
        }
        public ActionResult DeleteInvoiceSub(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف سند قبض اشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var models = Subscription.DeleteSupInvoiceWithGL(id);
            return RedirectToAction("IndexOfInvoices", new { msg = models });
        }

        public ActionResult IndexFinshedSub(int ?all)
        {
            Subscription.changestate();
            var models = Subscription.GetAllSubscriptionFinshed();
            if (all == null)
            {
                models.Reverse();
                models = models.Take(100).ToList();
            }
            return View(models);
        }
        public ActionResult IndexIsCanceld(string msg,int ?all)
        {
            //Subscription.changestate();
            ViewBag.Msg = msg;
            var models = Subscription.GetSubscriptionsIsCanceled();
            if (all == null)
            {
                models.Reverse();
                models = models.Take(100).ToList();
            }
            return View(models);
        }
        public ActionResult MakeSubscriptionCanceled(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "الغاء اشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var models = Subscription.MakeSubscriptionIsCanceled(id);
            return RedirectToAction("IndexIsCanceld", new { msg = models });
        }
        // GET: Subscription/Details/5
        public ActionResult Details(int id)
        {
            var subscription = Subscription.GetAllSubscriptionById(id);
            return View(subscription);
        }
        [HttpGet]
        public JsonResult GetProjectJson(int ID)
        {
            var subType = Subscription.GetAllTypeInfo(ID);
            var xx = new
            {
                subType.Subscrip_Company,
                subType.Subscrip_BU,
                subType.Subscrip_Description,
            };
            return Json(xx, JsonRequestBehavior.AllowGet);
        }
   
        // GET: Subscription/Create
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء اشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var subscription = Subscription.CreateVMFoCreation(user.Id);
            return View(subscription);
        }
        // POST: Subscription/Create
        [HttpPost]

        public ActionResult Create(SubscriptionVM supscripeVM)
        {
            try
            {
                // TODO: Add insert logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var sup = Subscription.createNewSubscription(supscripeVM , user.Id);
                string msg = "حدث خطأ عند الانشاء";
                if (sup == null)
                {
                    return RedirectToAction("IndexSubPending", new { msg = Resources.validate.Error });
                }
                else
                {
                    return RedirectToAction("DetailofSubscription", new { id = sup.Subscrip_Id });
                }
               
            }
            catch
            {
                return View();
            }
        }
        // GET: Subscription/Edit/5
        public ActionResult Edit(int id)
        {

            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل اشتراك");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                if (id < 1)
                {
                    string Msg = "error id is empty";
                    return RedirectToAction("Index", new { Msg = Msg });
                }
                var subscr = Subscription.FillSubscription(id);
                return View(subscr);
            }
            catch(Exception ex)
            {
                var m = ex.Message;
                var w = ex.Data;
                return View();
            }
        }
        // POST: Subscription/Edit/5
        [HttpPost]
        public ActionResult Edit(SubscriptionVM Bvm)
        {
            try
            {
                // TODO: Add update logic here
                var sup = Subscription.EditSubscription(Bvm);
                string msg = "حدث خطأ عند الانشاء";
                if (sup == null)
                {
                    return RedirectToAction("IndexSubPending", new { msg =Resources.validate.Error });
                }
                else
                {
                    return RedirectToAction("DetailofSubscription", new { id = sup.Subscrip_Id });
                }
              
            }
            catch(Exception ex)
            {
                var ms = ex.Message;
                return View();
            }
        }
        // GET: Subscription/Delete/5
    
        // POST: Subscription/Delete/5
      
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                var subscriptio = Subscription.DeleteSubscription(id);
                if (subscriptio == 1)
                {
                    return RedirectToAction("Index",new { Msg = Resources.validate.DeleteSuccessful });
                }
                return RedirectToAction("Index", new { Msg = Resources.validate.Error });
            }
            catch
            {

                return View();
            }
        }
        public ActionResult Update(SubscriptionVM SupVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد اشتراك");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                // TODO: Add update logic here

                string result = "Success! The GLs are created";
                Subscription.UpdateSubscription(SupVM);

                return Json(Resources.validate.GLDone, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult PartialDialog()
        {
            var custvm = Customer.AddCustomerVM();
            return PartialView("CustomerPartialView", custvm);
        }
        [HttpPost]
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
            var customer = Subscription.GetCustomerList();
            return PartialView("CustomerDropDownList", customer);
        }
        [HttpGet]
        public ActionResult GetPhone(int ID)
        {
            var result = Subscription.getCustPhone(ID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetFinalDate(int typeid,string starttime)
        {
            var result = Subscription.getdate(typeid, starttime);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult GetFinalDateofSub(string  date,int types)
        {
            var result = Subscription.getdateofsub(date, types);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Date(int id)
        {

            try
            {
                if (id < 1)
                {
                    string Msg = "error id is empty";
                    return RedirectToAction("Index", new { Msg = Msg });
                }
                var subscr = Subscription.FillSubscrip(id);
                return View(subscr);
            }
            catch (Exception ex)
            {
                var m = ex.Message;
                var w = ex.Data;
                return View();
            }
        }

        [HttpPost]              
        public ActionResult Date(SubscriptionVM vm)
        {
            try
            {
                // TODO: Add update logic here
                var sup = Subscription.EditSubscrip(vm);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var ms = ex.Message;
                return View();
            }
        }

       
        public ActionResult getSupsJson1(int id)
        {
            var result = Subscription.StopSub(id);

            return RedirectToAction("getSupsJson", "Subscription");
        }
        

        public ActionResult getSupsJson()
        {
            
            var result2 = Subscription.stoppedsub();

            return View(result2);  //RedirectToAction("getSupsJson");
        }


        public FileResult ReturnRecFile(int fileId)
        {

            var document = Subscription.FindFileByID(fileId);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = document.FileName,
                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.ProImg, "txt");
        }
        public ActionResult GetAllFiles(int RecId)
        {
            var model = Subscription.GetAllFiles(RecId);
            return View(model);
        }

        public ActionResult CreateSubInv(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء سند قبض اشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var subscription = Subscription.FillinvForcreation(id);
            return View(subscription);
        }

        // POST: Subscription/Create
        [HttpPost]
        public ActionResult CreateSubInv(SubscriptionInvoiceVM supscripeVM)
        {
            try
            {
                // TODO: Add insert logic here 
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var sup = Subscription.CreateInvoice(supscripeVM , user.Id);
                var thissub = Subscription.GetsubByCode1(supscripeVM.Subscrip_Code);
                
                    return RedirectToAction("DetailOFSubInv", new { id = sup.Inv_ID });
              
              
                
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                return View();
            }
        }

        public ActionResult DetailOFSubInv(int id)
        {
            var subscription = Subscription.GetThisINvoiceDetail(id);
            return View(subscription);
        }
      
        public async System.Threading.Tasks.Task<ActionResult> Voidtheinvoice(int ID)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "استبعاد سند قبض اشتراك");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                // TODO: Add insert logic here
                await Task.Delay(2000);
                var sup = Subscription.VoidInvoice(ID);

                return RedirectToAction("DetailofSubscription", new { id = ID });
            }
            catch
            {
                return View();
            }
        }
       
        public ActionResult CreateNew(int id)
        {
           
                var sup = Subscription.FillSubscripForNew(id,false);
               
                
          
                return View(sup);
          
        }

        [HttpPost]
        public ActionResult CreateNew(SubscriptionVM supscripeVM)
        {
            try
            {
                // TODO: Add insert logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var sup = Subscription.createNewSubscription(supscripeVM , user.Id);
           
                //return RedirectToAction("Index", new { msg = Resources.validate.CreateSucc });
                if (sup == null)
                {
                    return RedirectToAction("IndexSubPending", new { msg = Resources.validate.Error });
                }
                else
                {
                    return RedirectToAction("DetailofSubscription", new { id = sup.Subscrip_Id });
                }
            }
            catch
            {
                return View();
            }
        }


        public ActionResult EditForPendding(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل بيانات كامل اشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var sup = Subscription.FillSubscripForNew(id,true);



            return View(sup);

        }
       [ HttpPost]
        public ActionResult EditForPendding(SubscriptionVM supscripeVM)
        {
            try
            {
                // TODO: Add insert logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var sup = Subscription.EditPenddingSubscription(supscripeVM, user.Id);
                string msg = "Subscription Add Successfuly";
                return RedirectToAction("DetailofSubscription", new { id = sup.Subscrip_Id });
            }
            catch
            {
                return View();
            }
        }





        public ActionResult StopSupscription(int id)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ايقاف اشتراك");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                // TODO: Add insert logic here
                var msg = Subscription.StopSubscription(id);
              
                return RedirectToAction("Index", new { msg = msg });
            }
            catch
            {
                return View();
            }
        }
     
        public ActionResult StartSupscription(int id)
        {
            try
            {
                // TODO: Add insert logic here
                var msg = Subscription.StartSubscription(id);

                return RedirectToAction("Index", new { msg = msg });
            }
            catch
            {
                return View();
            }
        }
        public ActionResult IndexStopedSup(string Msg)
        {
            ViewBag.Msg = Msg;
            var models = Subscription.GetAllSubscriptionstoped();
            return View(models);
        }
        public ActionResult EditSub(int id)
        {
            var sup = Subscription.FillSubscripForNew(id,true);
            return View(sup);

        }
        public ActionResult EditSub(SubscriptionVM supscripeVM)
        {



            var msg = Subscription.EditSub(supscripeVM);

            return RedirectToAction("Index", new { msg = msg });
        }



        [HttpGet]
        public virtual JsonResult GetDepart(int BranchID)
        {
            var my = Subscription.GetDepartment(BranchID);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public virtual JsonResult GetBesniss(int Besniss)
        {
            var my = Subscription.GetBesniessUnet(Besniss);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DetailofSubscription(int id)
        {
            var sup = Subscription.GetAllSubscriptionById(id);
            return View(sup);

        }
        public JsonResult GetAmountwithTaxorNot(int? taxID,decimal AMount,decimal?Discount,decimal ?AllowDis,int? SuptempID)
        {
            var sup = Subscription.CalculateAmount(taxID, AMount, Discount.Value, AllowDis, SuptempID);
            return Json(sup, JsonRequestBehavior.AllowGet);

        }
  
        public async System.Threading.Tasks.Task<JsonResult> PostSubscription(int SubID)
        {
            await Task.Delay(2000);
            var sup = Subscription.PostSubscription(SubID);
            return Json(sup, JsonRequestBehavior.AllowGet);

        }

        public async System.Threading.Tasks.Task<JsonResult> VoidSubscription(int SubID)
        {
            await Task.Delay(2000);
            var sup = Subscription.VoidSubscription(SubID);
            return Json(sup, JsonRequestBehavior.AllowGet);

        }

        public ActionResult IndexNonPaidSubs(string Msg,int ?all)
        {
            ViewBag.Message = Msg;
            Subscription.changestate();
            var models = Subscription.GetsubsofTemp();
            if (all == null)
            {
                models.Reverse();
                models = models.Take(100).ToList();
            }

            return View(models);
        }
        [HttpGet]
        public ActionResult EditSubTemp(int  Id)
        {
           
            var subTemp = Subscription.EditSubTempForCreation(Id);
            return View(subTemp);
        }
        [HttpPost]
        public ActionResult EditSubTemp(SubscriptionTempVM subvm)
        {
            var subTemp = Subscription.EditSubscriptionTemp(subvm);
            return RedirectToAction("DetailsofTemp", new { id= subvm.Id });
        }
        [HttpGet]
        public ActionResult GetPartialForDetailTemp()
        {
            return PartialView("GetPartialForDetailTemp");
        }
        [HttpGet]
        public ActionResult GetListOfDetailTemp(int tempId)
        {
            var DetailVm = Subscription.getListofTemp(tempId);
            return PartialView("GetListOfDetailTemp", DetailVm);
        }

        [HttpPost]
        public JsonResult AddPartialDatatoTemp(SubscriptionItemsTempVM PVM)
        {
            string msg = "";
            try
            {
                 msg = Subscription.addingItemsinTemp(PVM);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);

        }
      
        public JsonResult DeleteFromTemp(int iD)
        {
            string msg = "";
            try
            {
                msg = Subscription.deletefromListofTemp(iD);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
           
        }
        [HttpGet]
        public ActionResult DetailsofTemp(int Id)
        {
            var subTemp = Subscription.getDetailsofSubTemp(Id);
            return View(subTemp);
        }
        [HttpGet]
        public ActionResult GetListOfDetailTempItems(int tempId)
        {
            var DetailVm = Subscription.getListofTemp(tempId);
            return PartialView("GetListOfDetailTempItems", DetailVm);
        }
    }
}
