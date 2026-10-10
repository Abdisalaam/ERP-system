using Accountingsystem.Models;
using Accountingsystem.Utilites;
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
    [Authorize]
    public class GLMasterController : Controller
    {
        // GET: GLMaster
        public ActionResult Index(int ?all,string msg)
        {
            var Taxgl = GLMaster.getALLGLMaster().Where(z => z.GLCode.StartsWith("TAX")).Select(a=>a.GLMaster_Id);
            var mas = GLMaster.getALLGLMaster().Where(f=>!Taxgl.Contains(f.GLMaster_Id)).OrderBy(a => a.GL_Date).ToList();
            //var gl = GLMaster.getALLGLMasterAll().Where(a => a.GL_Date.Year == 2022).Where(q => q.GLDetails.Select(b => b.GLDetail_Credit).Sum() == 0).ToList();
// var gl = GLMaster.getALLGLMasterAll().Where(a=>a.GL_Date.Year==2022).Where(q => Math.Round(q.GLDetails.Select(b => b.GLDetail_Credit).Sum(), 4, MidpointRounding.AwayFromZero) != Math.Round(q.GLDetails.Select(r => r.GLDetail_Debit).Sum(), 4, MidpointRounding.AwayFromZero)).ToList();
            if (all == null)
            {
                mas.Reverse();
                mas = mas.Take(100).ToList();
            }
            ViewBag.Message = msg;
            return View(mas);
        }
        public JsonResult CheckStartdate(string date)
        {
            var msg = GLMaster.Checkdate(date);

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult IndexAuto(int ?all)
        {
            var Taxgl = GLMaster.getALLGLMasterAuto().Where(z => z.GLCode.StartsWith("TAX")).ToList();

            var mas = GLMaster.getALLGLMasterAuto().Except(Taxgl).OrderBy(a=>a.GL_Date).ToList();

            //var mo = mas.Where(a => a.GLState == TransactionState.Post && a.GL_Date.Month == 8);
            // var query = mo.GroupBy(x => x.GLCode)
            //.Where(g => g.Count() > 1)
            //.Select(y => y.Key)
            //.ToList();
            //var db = new AccountingContainer();
            //var value = db.StoreCards.Where(a => a.Store_Id == 3 && a.IncreaseQuantity == true).Select(a => a.TotalCost).Sum();
            //var value2 = db.StoreCards.Where(a => a.Store_Id == 3 && a.IncreaseQuantity == false).Select(a => a.TotalCost).Sum();
            //var final = value - value2;
            if (all == null)
            {
                mas.Reverse();
                mas = mas.Take(100).ToList();
            }
            return View(mas);
        }
        public ActionResult IndexAutoOpen(int ?all,string msg)
        {
            var glMaster = GLMaster.getALLGLMasterAutoOpen().OrderBy(a => a.GL_Date).ToList(); 
            if (all == null)
            {
                glMaster.Reverse();
                glMaster = glMaster.Take(100).ToList();
            }
            ViewBag.Message = msg;
            return View(glMaster);
        }
        public ActionResult IndexTaxGL(int? all)
        {
            var glMaster = GLMaster.getALLTAXGL().OrderBy(a => a.GL_Date).ToList(); 
            if (all == null)
            {
                glMaster.Reverse();
                glMaster = glMaster.Take(100).ToList();
            }
            return View(glMaster);
        }
        public ActionResult IndexEMP(int? all, string msg)
        {
            var glMaster = GLMaster.getALLEMPGL().OrderBy(a => a.GL_Date).ToList();
            if (all == null)
            {
                glMaster.Reverse();
                glMaster = glMaster.Take(100).ToList();
            }
            ViewBag.Message = msg;
            return View(glMaster);
        }
        [HttpGet]
        public ActionResult FillGlForCreate()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء قيد يومي");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var glMaster = GLMaster.fillToCreate();
            return View(glMaster);
        }
        [HttpPost]
        public ActionResult FillGlForCreate(GLMasterVM glvm)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var a = GLMaster.createGL(glvm , user.Id);
            var fullUrl = this.Url.Action("detail", "GLMaster", new { id = a }, this.Request.Url.Scheme);
            return Json(fullUrl, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillGlForEdit(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تعديل قيد");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var glMaster = GLMaster.fillToEdit(id);
            if (glMaster == null)
            {
                return RedirectToAction("detail", new { id = id });
            }
            return View(glMaster);
        }
        [HttpPost]
        public ActionResult FillGlForEditPost(GLMasterVM glvm)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var a = GLMaster.EditPost(glvm);
            var fullUrl = this.Url.Action("detail", "GLMaster", new { id = a }, this.Request.Url.Scheme);
            return Json(fullUrl, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FillGlForCreateSettelment(string code )
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء قيد يومي");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var glMaster = GLMaster.fillToCreateForSettlement(code);
            return View(glMaster);
        }
        [HttpPost]
        public ActionResult FillGlForCreateSettelment(GLMasterVM vm)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var model = GLMaster.createGLStellement(vm, user.Id);
            return RedirectToAction("detail", new { id = model });
        }
        public ActionResult detail(int id,string msg)
        {
            var glMaster = GLMaster.getDetailById(id);
            ViewBag.msg = msg;
            return View(glMaster);


        }
        public ActionResult DetailByCode(string code)

        {
            var glmas = GLMaster.GetGlByCode(code);
            return RedirectToAction("detail",new {id= glmas });

        }
        public ActionResult Delete(int id)

        {
            var glmas = GLMaster.getDetailById(id);
            return View(glmas);

        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                int glmas = GLMaster.deleteGl(id);
                if (glmas == 1)
                {
                    return RedirectToAction("Index");

                }
                return View(collection);
            }
            catch
            {
                return View();
            }
        }
        public ActionResult FillGlForCreateStartGL()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء قيد افتتاحي");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var glMaster = GLMaster.fillToCreateStartingGL();
            return View(glMaster);
        }

        [HttpPost]
        public ActionResult FillGlForCreateStartGL(GLMasterVM glvm)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            //string result = "تم انشاء القيد";
            var a= GLMaster.createGL(glvm , user.Id);
            var fullUrl = this.Url.Action("detail", "GLMaster", new { id = a }, this.Request.Url.Scheme);
            return Json(fullUrl, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getdetailForAutoGL(String code)
        {
            var a = GLMaster.SendToDetailView(code);
            if (a.Name == "GL")
            {
                return RedirectToAction("detail", new { id = a.id });

            }
            if (a.Name == "SAL")
            {
                return RedirectToAction("Details", "InvoiceMaster", new { id = a.id });

            }
            if (a.Name == "PRCT")
            {
                return RedirectToAction("Details", "ProjectRecipt", new { id = a.id });

            }

            if (a.Name == "SALRC")
            {
                return RedirectToAction("Details", "SalesReceiptMaster", new { id = a.id });

            }
            if (a.Name == "PUR")
            {
                return RedirectToAction("Details", "PurchaseInvoice", new { id = a.id });

            }
            if (a.Name == "PAY")
            {
                return RedirectToAction("Details", "Payment", new { id = a.id });

            }
            if (a.Name == "REC")
            {
                return RedirectToAction("Details", "Receipt", new { id = a.id });

            }
            if (a.Name == "REVP")
            {
                return RedirectToAction("Details", "RecalPurchInvoiceMaster", new { id = a.id });

            }
            if (a.Name == "REVS")
            {
                return RedirectToAction("Details", "ReverseSalesInvoiceMaster", new { id = a.id });

            }
            if (a.Name == "EXP")
            {
                var exp = Expense.getDetail(a.id).RandomValue;
                if (exp == null)
                {
                    return RedirectToAction("DetailsNew", "Expense", new { id = a.id });
                }
                return RedirectToAction("Details", "Expense", new { id = a.id });
            }
            if (a.Name == "REVSa")
            {
                return RedirectToAction("Details", "ReverseSalesReceiptMaster", new { id = a.id });

            }
     
            if (a.Name == "SUBINV")
            {
                return RedirectToAction("DetailOFSubInv", "Subscription", new { id = a.id });

            }
            if (a.Name == "PROPAY")
            {
                return RedirectToAction("Details", "ProjectPayment", new { id = a.id });

            }
            if (a.Name == "PINV")
            {
                return RedirectToAction("DetailsofInvoice", "Project", new { id = a.id });

            }
            if (a.Name == "COSDS")
            {
                return RedirectToAction("Details", "CostDistribution", new { id = a.id });

            }
            if (a.Name == "SPRE")
            {
                return RedirectToAction("Details", "SpentRecipt", new { id = a.id });

            }
            if (a.Name == "MATPAY")
            {
                return RedirectToAction("Details", "MatchingPayment", new { id = a.id });

            }
            if (a.Name == "CARECI")
            {
                return RedirectToAction("Details", "CatchReceipt", new { id = a.id });

            }
            if (a.Name == "TMain")
            {
                return RedirectToAction("Details", "TransferToMaintenance", new { id = a.id });

            }
            if (a.Name == "TTPRO")
            {
                return RedirectToAction("Details", "TransferToProjectMaster", new { id = a.id });

            }
            if (a.Name == "AS")
            {
                return RedirectToAction("Details", "FixedAsset", new { id = a.id });

            }
            if (a.Name == "PRO")
            {
                return RedirectToAction("Details", "Product", new { id = a.id });

            }
            if (a.Name == "TRS")
            {
                return RedirectToAction("Details", "InvTreansMaster", new { id = a.id });

            }

            if (a.Name == "SETSTO")
            {
                return RedirectToAction("Details", "SettlementOfStores", new { id = a.id });

            }
            if (a.Name == "POS")
            {
                return RedirectToAction("Detail", "MasterPOSInvoice", new { id = a.id });

            }
            if (a.Name == "REVPOS")
            {
                return RedirectToAction("Details", "ReversePOSInvoice", new { id = a.id });

            }
            if (a.Name == "SUBS")
            {
                return RedirectToAction("DetailofSubscription", "Subscription", new { id = a.id });

            }
            if (a.Name == "PRCT")
            {
                return RedirectToAction("Details", "ProjectRecipt", new { id = a.id });

            }
            if (a.Name == "ORD")
            {
                return RedirectToAction("Details", "Cachier", new { id = a.id });

            }
            if (a.Name == "RORD")
            {
                return RedirectToAction("GetReversdeOrderDetails", "Cachier", new { reversdeOrderId = a.id });

            }
            else 
            {
                return RedirectToAction("detail", "GLMaster", new { id = a.id });

            }

           
        }
        [HttpGet]
        public ActionResult getdetailForAutoGLOpen(String code)
        {
            var a = GLMaster.SendToDetailViewOpenBalance(code);
            if (a.Name == "GLST")
            {
                return RedirectToAction("detail", new { id = a.id });

            }
            if (a.Name == "OPA")
            {
                return RedirectToAction("Details", "FixedAsset", new { id = a.id });

            }
            if (a.Name == "PRO")
            {
                return RedirectToAction("Details", "Product", new { id = a.id });

            }
            if (a.Name == "OPB")
            {
                return RedirectToAction("Details", "Bank", new { id = a.id });

            }
            return null;
        }
        [HttpGet]
        public virtual JsonResult GetAccountLedger(int Acc_ID)
        {
            var my = GLMaster.GetAccountLedger(Acc_ID);
            return Json(my, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetAccountParent(int Acc_ID)
        {
            var my = GLMaster.GetChildOfParent(Acc_ID);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public virtual JsonResult GetAccountSubCredit(int Acc_ID)
        {
            var my = GLMaster.GetsubCredit(Acc_ID);
            return Json(my, JsonRequestBehavior.AllowGet);
        }
        public ActionResult makeOffGL()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "انشاء قيد اقفال");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var pack = PackageRecognize.checkValiditeOfGLOff(user.Id);
            if (pack.Item2 == true)
            {
                var glMaster = GLMaster.MakingOffGlGet();
                return View(glMaster);
            }
            else
            {
                var msgo = "باقتك لا تدعم عمل قيد اقفال";
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }
        [HttpPost]
        public ActionResult makeOffGL(OffGlMasterVM offGl)
        {
           var msg= GLMaster.makingOffglPost(offGl);
            return RedirectToAction("getAllOffGL", new { msg = msg });
        }
        public ActionResult getAllOffGL(String msg)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfGLOff(user.Id);
            if (pack.Item2 == true)
            {
                var glMaster = GLMaster.getallOffGL();

                ViewBag.Message = msg;
                return View(glMaster);
            }
            else
            {
                var msgo = "باقتك لا تدعم عمل قيد اقفال";
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }
       public ActionResult MakePostGLVoid(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "استبعاد قيد");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var glMaster = GLMaster.MakeVoidOfPostingGL(id);
            return RedirectToAction("detail", new { id = id, msg= glMaster });

        }
        public ActionResult MakeEvaluationOFStore()
        {
            var glMaster = GLMaster.makeEvaluationOfStore();
            return RedirectToAction("Index");

        }
        public ActionResult DeleteActive(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "حذف قيد");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            try
            {
                var gl = GLMaster.getDetailById(id);
                var msg = GLMaster.deletionactiveGL(id);
              if( gl.GLCode.StartsWith("GLST") )
                {
                    return RedirectToAction("IndexAutoOpen", new { msg = msg });
                }
                if (gl.GLCode.StartsWith("GLOF"))
                {
                    return RedirectToAction("getAllOffGL", new { msg = msg });
                }
                if (gl.GLCode.StartsWith("EmpT-"))
                {
                    return RedirectToAction("IndexEMP", new { msg = msg });
                }
                return RedirectToAction("Index", new { msg = msg });
            }
            catch (Exception ex)
            {
                var gl = GLMaster.getDetailById(id);
                ViewBag.Message = ex.Message;
                HttpCookie cookie = HttpContext.Request.Cookies.Get("Language");
                if (cookie != null && cookie.Value != null)
                {
                    if (cookie.Value == "en")
                    {
                        ViewBag.Message = ex.Message + "___" + "Please review this error and try to correct it";
                    }
                    else
                    {
                        ViewBag.Message = ex.Message + "__" + "هناك خطأ في البيانات. الرجاء التاكد من صحتها";
                    }
                }
                else
                {

                    ViewBag.Message = ex.Message + "__" + "هناك خطأ في البيانات. الرجاء التاكد من صحتها";
                }

                if (gl.Auto == true && gl.GLCode.StartsWith("GLST") || gl.GL_Desc.Contains("افتتاحي"))
                {
                    return RedirectToAction("IndexAutoOpen", new { msg = ViewBag.Message });
                }
                return RedirectToAction("Index", new { msg = ViewBag.Message });
            }

        }
        public JsonResult getbalanceAccount(int AccountId)
        {
            var balance = GLMaster.getbalanceAccount(AccountId);
          

            return Json(balance, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getbalanceAccountChild(int ChildId)
        {
            var balance = GLMaster.getbalanceChildAccount(ChildId);


            return Json(balance, JsonRequestBehavior.AllowGet);
        }
    }
}