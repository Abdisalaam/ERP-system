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
    public class SettlementOfStoresController : Controller
    {
        public JsonResult CheckCode(string Serial_Number)
        {
            if (SettlementOfStores.containCode(Serial_Number))
            {
                var msg = "SETSTO- هذا الكود موجود علي النظام  او لا يحتوي علي ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: SettlementOfStores
        public ActionResult Index(string msg,int ?all)
        {
            var models = SettlementOfStores.GetAllISettlementStore();

            if (all == null)
            {
                models.Reverse();
                models = models.Take(100).ToList();
            }
            ViewBag.Message = msg;
            return View(models);
        }

        // GET: SettlementOfStores/Details/5
        public ActionResult Details(int id)
        {
            var settlment = SettlementOfStores.GetSettlemementsById(id);
            return View(settlment);
           
        }
        public ActionResult PreparerecallDetailsInStore(int prodID, int storID)
        {
            var btachQan = SettlementOfStores.PrepareBatchQuantityInStore(prodID, storID);
            return PartialView("PreparerecallDetailsInStore", btachQan);

        }
        public ActionResult PreparerecallDetailsOutStore(int prodID, int storID)
        {
            var productOut = SettlementOfStores.PrepareProductOutStore(prodID, storID);
            return PartialView("PreparerecallDetailsInStore", productOut);

        }
        //Add To Temp details
        [HttpPost]
        public virtual JsonResult AddDetailsToMaster(SettlementOfStoresDetailsVM ofStoresDetailsVM)
        {
            var products = SettlementOfStores.AddDetailsToTemp(ofStoresDetailsVM);
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetAllTempSettlementDetails(int tempSettlementId)
        {
            var detailsVM = SettlementOfStores.GetAllTempSettlementDetails(tempSettlementId);
            return PartialView("GetAllTempSettlementDetails", detailsVM);
        }
        // GET: SettlementOfStores/Create
        public ActionResult Create(int storeId,int ? productINId,int? productout)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء تسوية");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = SettlementOfStores.AddForCreation(storeId, productINId, productout);
            return View(model);
        }

        // POST: SettlementOfStores/Create
        [HttpPost]
        public ActionResult Create(SettlementOfStoresVM settlementOfStoresVM)
        {
            try
            {
                // TODO: Add insert logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var  result = SettlementOfStores.AddForCreate(settlementOfStoresVM , user.Id);
                if (result.Item2 == 0) {
                    return RedirectToAction("Index",new {msg=result.Item1 });
                }
                else
                {
                    return RedirectToAction("Details", new { id = result.Item2 });
                }
               
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                return View();
            }
        }

        // Delete the DeleteSettlementOfStore in state pending from db
        [HttpGet]
        public ActionResult Delete(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف تسوية");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var msg = SettlementOfStores.DeleteSettlementOfStore(id);
            return RedirectToAction("Index", new { msg = msg });
        }
        // POST: settlement/UpdateGl/
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> UpdateGL(SettlementOfStoresVM settlementOfStoresVM)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "اعتماد تسوية");



                if (employeeAction == false)
                {
                    return Json(Resources.validate.yourAuth, JsonRequestBehavior.AllowGet);
                }
               
                await Task.Delay(2000);
                string result = SettlementOfStores.UpdateSettlementOfStores(settlementOfStoresVM);

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

        public JsonResult DeleteTempDetail(int iD)
        {
            var temp = SettlementOfStores.DeleteTemp(iD);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// For Edit Settlement store
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult PreparerecallDetailsInStoreForEdit(int prodID, int storID , int serial)
        {
            var btachQan = SettlementOfStores.PrepareBatchQuantityInStoreForEdit(prodID, storID , serial);
            return PartialView("PreparerecallDetailsInStore", btachQan);

        }
        public ActionResult PreparerecallDetailsOutStoreForEdit(int prodID, int storID, int serial)
        {
            var productOut = SettlementOfStores.PrepareProductOutStoreForEdit(prodID, storID , serial);
            return PartialView("PreparerecallDetailsInStore", productOut);

        }

        [HttpGet]
        public ActionResult GetAllOfSettlmentDetails(int settlementId)
        {
            var settlemtnt = SettlementOfStores.GetAllSettlementDetails2(settlementId);
            return PartialView("GetAllOfSettlmentDetails", settlemtnt);
        }

        [HttpGet]
        public ActionResult GetAllTempOfferresult(int settlementId)
        {
            var settlemtnt = SettlementOfStores.GetAllTempDetailsresult2(settlementId);
            return PartialView("GetAllTempSettlementDetails", settlemtnt);
        }
        //Add To Temp details
        [HttpPost]
        public virtual JsonResult AddDetailsToMasterForEdit(SettlementOfStoresDetailsVM ofStoresDetailsVM)
        {
            var products = SettlementOfStores.AddDetailsToTempForEdit(ofStoresDetailsVM);
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        // GET: SettlementOfStores/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل تسوية");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = SettlementOfStores.EditForCreation(id);
            return View(model);
        }

        // POST: SettlementOfStores/Edit/5
        [HttpPost]
        public ActionResult Edit(SettlementOfStoresVM settlementOfStoresVM)
        {
            try
            {
                // TODO: Add update logic here
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var model = SettlementOfStores.Edit(settlementOfStoresVM , user.Id);
                if (model.Item2 == 0)
                {
                    return RedirectToAction("Index", new { msg = model.Item1 });
                }
                else
                {
                    return RedirectToAction("Details", new { id = model.Item2 });
                }
                
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                return View();
            }
        }
     
    }
}
