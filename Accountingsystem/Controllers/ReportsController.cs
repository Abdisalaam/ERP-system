using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using Accountingsystem.ViewModel.ReportsVM;
using Accountingsystem.ViewModel.ReportsVM.SubscriptionReportVM;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Accountingsystem.Utilites;
using System.IO;

namespace Accountingsystem.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        string footer = "--footer-right \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";
        // GET: Reports
        int bottomWhite = 43;
        //int TopWhite = 40;
        int TopWhite = 40;
        string protocol = "https";
        public ActionResult GetAllReports()
        {
            return View();
        }
        public ActionResult GetAllReportfinancial()
        {
            return View();
        }
        public ActionResult GetAllReportAsset()
        {
            return View();
        }
        public ActionResult Packagevalidate(string msg)
        {
            ViewBag.message = msg;
            return View();
        }
        public ActionResult GetAllReportsPurchases()
        {
            return View();
        }
        public ActionResult GetAllReportsInvoiceSales()
        {
            return View();
        }
        public ActionResult GetAllReportsForEmployee()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfHR(user.Id);
            if (pack.Item2 == true)
            {
                return View();
            }
            else
            {
                var msgo = "باقتك لا تدعم قسم الموارد البشرية";
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }
        public ActionResult GetAllReportsForProjects()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfPoject(user.Id);
            if (pack.Item2 == true)
            {
                return View();
            }
            else
            {
                var msgo = "باقتك لا تدعم قسم المشاريع";
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }
        public ActionResult PrintCostDistribution(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة توزيع التكلفة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintCostDestribution(id);
            return new ViewAsPdf("PrintCostDistribution", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 30 } };
        }
        // For print
        public ActionResult PrintPOSInvoice(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل فاتورة نقطة مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintPOSInvoice(id);
            

            return new ViewAsPdf("PrintPOSInvoice", model) { MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 5 } };
        }
        //public ActionResult SendPOSInvoice(int id)
        //{
        //    var model = Reports.PrintPOSInvoice(id);
        //    var action = new Rotativa.ViewAsPdf("PrintPOSInvoice", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        //    var bytearr = action.BuildFile(ControllerContext);
        //    string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + DateTime.Now.ToBinary() + ".pdf");

        //    var file = new FileStream(path, FileMode.Create, FileAccess.Write);
        //    file.Write(bytearr, 0, bytearr.Length);
        //    file.Close();
        //    var a = Utilites.MailManager.SendEmailWithAttachment(model.customerEmail, "فاتورة خزنية سريعة", "فاتورة خزنية سريعة", path);
        //    return new ViewAsPdf("PrintPOSInvoice", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        //}
        public ActionResult PrintPOSInvoiceWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة فاتورة نقطة مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintPOSInvoice(id);
            return new ViewAsPdf("PrintPOSInvoiceWhite", model) {  MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult PrintReversePOSInvoice(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل  مردود فاتورة نقطة مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintReversePOSInvoice(id);
            return new ViewAsPdf("PrintReversePOSInvoice", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult PrintSales(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل فاتورة اجل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintSalesInvoice(id);
            //return View(model);
            return new ViewAsPdf("PrintSales", model) {  MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 30, Right = 5, Top = 5 } } ;
        }
        public ActionResult PrintMaintenanceOrderWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة امر استلام صيانة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintMaintenanceOrder(id);
            return new ViewAsPdf("PrintMaintenanceOrderWhite", model) {  MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite} };
        }
        public ActionResult PrintMaintenanceOrder(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل  امر استلام صيانة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintMaintenanceOrder(id);
            return new ViewAsPdf("PrintMaintenanceOrder", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 30, Right = 5, Top = 5 } };
        }
        public ActionResult SendSalesEmail(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ارسال ايميل فاتورة اجل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintSalesInvoice(id);
            ViewAsPdf action = null;
            if (model.CustomerTax != null)
            {
                action = new ViewAsPdf("PrintSales", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
            }
            else
            {
                var modelnew = MasterQRInvoice.GetQRMasterByGLCode(model.Serial_Number);
                action = new ViewAsPdf("DetailsimpleQR", modelnew) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 30, Right = 15, Top = 10 } }; 
            }
            
            var bytearr = action.BuildFile(ControllerContext);
            
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + model.Serial_Number+" " + DateTime.Now.ToString("yyyy-MM-dd") + "  "+DateTime.Now.Second + ".pdf");

            var file = new FileStream(path, FileMode.Create, FileAccess.Write);
            file.Write(bytearr, 0, bytearr.Length);
            file.Close();
           
            var isSend = Utilites.MailManager.SendEmailWithAttachment(model.customerEmail, "فاتورة اجل", "اليك بيانات الفاتورة", path);
            ////System.IO.File.Delete(path);
            if (isSend == true)
            {
               // System.IO.File.Delete(path);
                return RedirectToAction("Details", "InvoiceMaster", new { id = id ,msg="تم ارسال الفاتورة للعميل"});
            }
            return RedirectToAction("Details", "InvoiceMaster", new { id = id, msg = "لم يتم ارسال الفاتورة" });
        }
        public ActionResult DetailsimpleQR(string code)
        {
            
            var model = MasterQRInvoice.GetQRMasterByGLCode(code);
            //return View(model);
            return new ViewAsPdf("DetailsimpleQR", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 30, Right = 5, Top = 5 } };
        }
        public ActionResult PrintSalesDelivery(int id)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "سند توريد فاتورة اجل");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var model = Reports.PrintSalesInvoice(id);
                return new ViewAsPdf("PrintSalesDelivery", model) {  MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 15 } };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index","InvoiceMaster", new {msg=ex.Message});
            }
        }
     
        public ActionResult PrintSalesWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة فاتورة اجل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            var model = Reports.PrintSalesInvoice(id);
            return new ViewAsPdf("PrintSalesWhite", model) { MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
            //return View(model);
        }
        //Print offer price
        [AllowAnonymous]
        public ActionResult PrintOfferPrice(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة عرض سعر ");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintOfferPriceNew(id);
            return new ViewAsPdf("PrintOfferPrice", model) {  MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult PrintOfferPricePackage(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة عرض سعر بكج");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintOfferPriceNew(id);
            return new ViewAsPdf("PrintOfferPricePackage", model) {  MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
        }
        public ActionResult SendOfferPriceEmail(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ارسال عرض سعر");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintOfferPriceNew(id);
    

            var action = new ViewAsPdf("PrintOfferPriceNew", model) { MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
            var bytearr = action.BuildFile(ControllerContext);
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + model.Serial_Number + " " + DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.Second + ".pdf");

            var file = new FileStream(path, FileMode.Create, FileAccess.Write);
            file.Write(bytearr, 0, bytearr.Length);
            file.Close();
            var isSend = Utilites.MailManager.SendEmailWithAttachment(model.customerEmail, "عرض السعر", "اليك بيانات عرض السعر", path);

            if (isSend == true)
            {
                // System.IO.File.Delete(path);
                return RedirectToAction("Details", "OfferPriceMaster", new { id = id, msg = "تم ارسال عرض السعر للعميل" });
            }
            return RedirectToAction("Details", "OfferPriceMaster", new { id = id, msg = "لم يتم ارسال عرض السعر" });
        }
        public ActionResult SendEmailPurchaseOrder(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ارسال امر مشتريات ايميل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintPurchOrder(id);

          
            var action = new ViewAsPdf("PrintPurchaseOrder", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 5 } }; ;
            var bytearr = action.BuildFile(ControllerContext);
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + model.Serial_Number + " " + DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.Second + ".pdf");

            var file = new FileStream(path, FileMode.Create, FileAccess.Write);
            file.Write(bytearr, 0, bytearr.Length);
            file.Close();
            var isSend = Utilites.MailManager.SendEmailWithAttachment(model.supplierEmail, " امر الشراء", "اليك بيانات امر الشراء", path);

            if (isSend == true)
            {
                // System.IO.File.Delete(path);
                return RedirectToAction("Details", "PurchaseOrderMaster", new { id = id, msg = "تم ارسال أمر الشراء للمورد" });
            }
            return RedirectToAction("Details", "PurchaseOrderMaster", new { id = id, msg = "لم يتم ارسال أمر الشراء" });
        }
        public ActionResult SendEmailRecipt(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ارسال ايميل سند قبض مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintReceipt(id);
            var action = new ViewAsPdf("PrintReceipt", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } }; 
            var bytearr = action.BuildFile(ControllerContext);
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + model.Recepit_Code + " " + DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.Second + ".pdf");

            var file = new FileStream(path, FileMode.Create, FileAccess.Write);
            file.Write(bytearr, 0, bytearr.Length);
            file.Close();
            var isSend = Utilites.MailManager.SendEmailWithAttachment(model.customer.Customer_Mail, " سند قبض مبيعات", "اليك بيانات سند قبض مبيعات", path);

            if (isSend == true)
            {
                // System.IO.File.Delete(path);
                return RedirectToAction("Details", "Receipt", new { id = id, msg = "تم ارسال سند القبض للعميل" });
            }
            return RedirectToAction("Details", "Receipt", new { id = id, msg = "لم يتم سند القبض" });
          
        }
        public ActionResult PrintOfferPriceNew(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة عرض سعر ايميل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
        
            var model = Reports.PrintOfferPriceNew(id);
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
Url.Action("Header", "InvoiceQRExternalUseres", new { Branch_Id = model.branchId }, protocol));

            return new ViewAsPdf(model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;
        }
        public ActionResult proformaInvoiceEmail(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة عرض سعر ايميل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            var model = Reports.PrintOfferPriceNew(id);
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
Url.Action("Header", "InvoiceQRExternalUseres", new { Branch_Id = model.branchId }, protocol));

            return new ViewAsPdf(model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;
        }
        public ActionResult proformaInvoice(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة عرض سعر ايميل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            var model = Reports.PrintOfferPriceNew(id);
            return new ViewAsPdf("proformaInvoice", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult PrintOfferPriceCustmize(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة عرض سعر ايميل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            var model = Reports.PrintOfferPriceNew(id);
            string customSwitchesfooter = string.Format(" --print-media-type --page-offset 2 --footer-center [page] --allow {0} --footer-html {0} --footer-spacing 0 ",
          Url.Action("Footer1", "InvoiceQRExternalUseres", new { Branch_Id = model.branchId }, protocol));
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
    Url.Action("Header1", "InvoiceQRExternalUseres", new { Branch_Id = model.branchId }, protocol));

            return new ViewAsPdf(model)
            {

                CustomSwitches = customSwitchesfooter + customSwitchesheader

            }
            ;
        }

        public ActionResult PrintSalesRecipt(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل فاتورة نقدي");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintSalesRecipt(id);
            return new ViewAsPdf("PrintSalesRecipt", model) {  MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }
        public ActionResult PrintSalesReciptDelivery(int id)
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "سند توريد فاتورة نقدي");



                if (employeeAction == false)
                {
                    return RedirectToAction("NotAuthorize", "SystemRole");
                }
                var model = Reports.PrintSalesRecipt(id);
                return new ViewAsPdf("PrintSalesReciptDelivery", model) {  MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "SalesReceiptMaster", new { msg = ex.Message });
            }
        }
        public ActionResult PrintSalesReciptWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة فاتورة نقدي");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintSalesRecipt(id);
            return new ViewAsPdf("PrintSalesRecipWhite", model) {  MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }

        public ActionResult PrintOrder(int id)
        {
           
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل فاتورة كاشير");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintOrder(id);
            //return View(model);
            return new ViewAsPdf(model) {  MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }
     
        public ActionResult PrintOrderWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة فاتورة كاشير");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintOrder(id);
            return new ViewAsPdf(model) {  MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult SendOrderEmail(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ارسال فاتورة كاشير");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintOrder(id);
            ViewAsPdf action = null;
            if (model.CustomerTax != null)
            {
                 action = new ViewAsPdf(model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
            }
            else
            {
                var modelnew = MasterQRInvoice.GetQRMasterByGLCode(model.Serial_Number);
                action = new ViewAsPdf("DetailsimpleQR", modelnew) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 30, Right = 15, Top = 10 } };
            }
            var bytearr = action.BuildFile(ControllerContext);

            string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + model.Serial_Number + " " + DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.Second + ".pdf");

            var file = new FileStream(path, FileMode.Create, FileAccess.Write);
            file.Write(bytearr, 0, bytearr.Length);
            file.Close();

            var isSend = Utilites.MailManager.SendEmailWithAttachment(model.customerEmail, "فاتورة نقدي", "اليك بيانات الفاتورة", path);
            ////System.IO.File.Delete(path);
            if (isSend == true)
            {
                // System.IO.File.Delete(path);
                return RedirectToAction("Details", "Cachier", new { id = id, msg = "تم ارسال الفاتورة للعميل" });
            }
            return RedirectToAction("Details", "Cachier", new { id = id, msg = "لم يتم ارسال الفاتورة" });
        }

        public ActionResult PrintRevInvoiceMaster(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل مردود اجل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintReverseSalesInvoice(id);
            return new ViewAsPdf("PrintRevInvoiceMaster", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult PrintRevInvoiceMasterWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة مردود اجل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintReverseSalesInvoice(id);
            return new ViewAsPdf("PrintRevInvoiceMasterWhite", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult PrintRevSalesRecipt(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل مردود نقدي");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintReverseSalesRecipt(id);
            return new ViewAsPdf("PrintRevSalesRecipt", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult PrintRevSalesReciptWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة مردود نقدي");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintReverseSalesRecipt(id);
            return new ViewAsPdf("PrintRevSalesReciptWhite", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }

        public ActionResult PrintRevOrder(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل  مردود فاتورة كاشير");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintReverseOrder(id);
            return new ViewAsPdf(model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult PrintRevOrderWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة مردود فاتورة كاشير");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintReverseOrder(id);
            return new ViewAsPdf(model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult PrintReceipt(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل سند قبض مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintReceipt(id);
            return new ViewAsPdf("PrintReceipt", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 5 } };
        }
        public ActionResult PrintReceiptWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة سند قبض مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintReceipt(id);
            return new ViewAsPdf("PrintReceiptWhite", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult PrintReceiptSpecial(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة سند قبض مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintReceipt(id);
            string customSwitchesfooter = string.Format(" --print-media-type --page-offset 2 --footer-center [page] --allow {0} --footer-html {0} --footer-spacing 0 ",
                Url.Action("Footer1", "InvoiceQRExternalUseres", new { Branch_Id = model.branchId }, protocol));
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
    Url.Action("Header1", "InvoiceQRExternalUseres", new { Branch_Id = model.branchId }, protocol));

            return new ViewAsPdf(model)
            {
                PageMargins = { Left = 5, Right = 5 },
                CustomSwitches = customSwitchesfooter + customSwitchesheader

            }
            ;
        }
        public ActionResult PrintPOSReceipt(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل سند قبض نقطة بيع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintPOSReceipt(id);
            return new ViewAsPdf( model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 5 } };
        }
        public ActionResult PrintPOSReceiptWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة سند قبض نقطة بيع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintPOSReceipt(id);
            return new ViewAsPdf( model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }

        public ActionResult SendEmailPOSRecipt(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ارسال ايميل سند قبض نقطة بيع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintPOSReceipt(id);
            var action = new ViewAsPdf("PrintPOSReceipt", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
            var bytearr = action.BuildFile(ControllerContext);
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + model.Recepit_Code + " " + DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.Second + ".pdf");

            var file = new FileStream(path, FileMode.Create, FileAccess.Write);
            file.Write(bytearr, 0, bytearr.Length);
            file.Close();
            var isSend = Utilites.MailManager.SendEmailWithAttachment(model.customer.Customer_Mail, " سند قبض مبيعات", "اليك بيانات سند قبض مبيعات", path);

            if (isSend == true)
            {
                // System.IO.File.Delete(path);
                return RedirectToAction("Details", "Receipt", new { id = id, msg = "تم ارسال سند القبض للعميل" });
            }
            return RedirectToAction("Details", "Receipt", new { id = id, msg = "لم يتم سند القبض" });

        }
        public ActionResult PrintPayment(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل صرف مورد");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintPayment(id);
            return new ViewAsPdf("PrintPayment", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 5, Right = 15, Top = 5 } };
        }
        public ActionResult PrintPaymentWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل صرف مورد");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintPayment(id);
            return new ViewAsPdf("PrintPaymentWhite", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = TopWhite } };
        }
        // For Profits and loss
        public ActionResult ProfitLoss()
        {
            var model = Reports.ProfitsAndLoss();
            return new ViewAsPdf("ProfitLoss", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        // For Customer Balance Details
        public ActionResult CustomerBalanceDetails(int id)
        {
            var model = Reports.CustomerBalanceDetails(id);
            return new ViewAsPdf("CustomerBalanceDetails", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult getDetailsofCustomer(int id)
        {
            var model = Reports.getDetailsofCustomer(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult getDetailsofCustomer(CustomerTransactionGettingDateVM CVM) 
        {
            var model = Reports.CustomerTransactionDetails(CVM);
            return new ViewAsPdf("ViewListofCustomerTransactionsPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult getDetailsofSupplier(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مورد");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.getDetailsofCustomer(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult getDetailsofSupplier(CustomerTransactionGettingDateVM CVM)
        {
            var model = Reports.SupplierTransactionDetails(CVM);
            return new ViewAsPdf("ViewListOFSupplierTransactionPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult GetPurList(int id)
        {
            var model = Reports.PrintPurInvoice(id);
            return new ViewAsPdf("GetPurList", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult GetRecallPurList(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل مردود مشتريات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.GetAllRecallPurshace(id);
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
Url.Action("Header", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

            return new ViewAsPdf("GetRecallPurList", model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;
            //return new ViewAsPdf("GetRecallPurList", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult GetRecallPurListWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل مردود مشتريات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.GetAllRecallPurshace(id);
            return new ViewAsPdf("GetRecallPurListWhite", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = TopWhite } };
        }
        public ActionResult GetAssetPurInvoiceList(int id)
        {
            var model = Reports.PrintPurInvoiceAsset(id);
            return new ViewAsPdf("GetAssetPurInvoiceList", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult GetDateOfIncome()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "قائمة الدخل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = new IncomeStatementVM();
            return View(model);
        }

        [HttpPost]
        public ActionResult GetDateOfIncome(IncomeStatementVM incomeVM)
        {
            var model = Reports.IncomeStatement(incomeVM);
     
            TempData["IncomeData"] = model;
            TempData.Keep("IncomeData");

            return View("GetDateOfIncomeList", model);
        }
        public ActionResult GetDateOfIncomePdf()
        {

            IncomeStatementVM model = (IncomeStatementVM)TempData["IncomeData"];
            TempData.Keep("IncomeData");
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
Url.Action("HeaderReport", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

            return new ViewAsPdf(model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;
        }
        public ActionResult GetDateOfIncomeExcel()
        {
            IncomeStatementVM model = (IncomeStatementVM)TempData["IncomeData"];
            TempData.Keep("IncomeData");
            var excel = new ExcelSheetTemplate();
            Response.ClearContent();
            Response.BinaryWrite(excel.GetListOfIncomeExcel(model));
            Response.AddHeader("content-disposition", "attachment; filename=/ClassfiySales.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return View();
            // return new ViewAsPdf("TrialBalanceNewPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }
        [HttpGet]
        public ActionResult MakeTrilBallane()
        {
            var model = Reports.getDatesToMake();
            return View(model);
        }
        [HttpPost]
        public ActionResult MakeTrilBallane(TrialBalncePostVM vm)
        {
            var model = Reports.MakeTrailBalance(vm);
            return new ViewAsPdf("MakeTrilBallanepdf", model) { CustomSwitches = footer, MinimumFontSize =15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
          //  return View("MakeTrilBallanepdf", model);
            
        }

        public ActionResult GetReportGeneral()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "قائمة المركز المالي");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var gen = Reports.CreateForeReport();
            return View(gen);
        }

        [HttpPost]
        public ActionResult GetReportGeneral(ReportMasterGeneralBudgetVM VM)
        {
            var model = Reports.ReportGeneralBadget(VM);
            TempData["ReportMasterGeneral"] = model;
            TempData.Keep("ReportMasterGeneral");

            return View("GetReportGeneralList", model);
        }
        public ActionResult GetReportGeneralPdf()
        {

            ReportMasterGeneralBudgetVM model = (ReportMasterGeneralBudgetVM)TempData["ReportMasterGeneral"];
            TempData.Keep("ReportMasterGeneral");
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
Url.Action("HeaderReport", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

            return new ViewAsPdf("ReportBalancChet", model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;

        }
        public ActionResult GetReportGeneralExcel()
        {
            ReportMasterGeneralBudgetVM model = (ReportMasterGeneralBudgetVM)TempData["ReportMasterGeneral"];
            TempData.Keep("ReportMasterGeneral");
            var excel = new ExcelSheetTemplate();
            Response.ClearContent();
            Response.BinaryWrite(excel.GetListOfGeneralReportExcel(model));
            Response.AddHeader("content-disposition", "attachment; filename=Trial.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return View();
            // return new ViewAsPdf("TrialBalanceNewPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }

        public ActionResult getListofLedger()
        {
            var model = Reports.getListForLedger();
            return View(model);
        }
        [HttpPost]
        public ActionResult getListofLedger(LedgerPickerVM vm)
        {
            var model = Reports.AddingDataInLedger(vm);
            return new ViewAsPdf("ViewListofLedgerPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult getLedger(int id)
        {
            LedgerPickerVM vm = new LedgerPickerVM();
            vm.SubAccount_ID= id;
            var model = Reports.AddingDataInLedger(vm);
            return new ViewAsPdf("getLedger", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        public ActionResult SupplierBalanceDetail(int id)
        {
            var model = Reports.SupplierPalanceDetails(id);
            return new ViewAsPdf("SupplierBalanceDetail", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        ////////subscription
        public ActionResult getListAllSubscriptionPDF()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير كل الاشتراكات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.GetAllSubscriptions();
            return new ViewAsPdf("getListAllSubscriptionPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }
        public ActionResult SubSciptionTypeView()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "معلومات تقرير الاشتراكات بنوع الاشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.GetTypleForCreateReport();
            return View(model);
        }
        [HttpPost]
        public ActionResult SubSciptionTypeView(SubscriptionViewPageVM LVM)
        {
            var model = Reports.getSubscriptionBytype(LVM);
            TempData["SubscribtionData"] = model;
            TempData.Keep("SubscribtionData");

            return View("SubSciptionTypeViewList", model);
            
        }
        public ActionResult SubSciptionTypeViewPDF()
        {
            AllSubscriptionVM model = (AllSubscriptionVM)TempData["SubscribtionData"];
            TempData.Keep("SubscribtionData");

           
            return new ViewAsPdf("ViewExactSubsByTypePDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }
        public ActionResult SubSciptionTypeViewExcel()
        {
            AllSubscriptionVM model = (AllSubscriptionVM)TempData["SubscribtionData"];
            TempData.Keep("SubscribtionData");
            var excel = new ExcelSheetTemplate();
            Response.ClearContent();
            Response.BinaryWrite(excel.GetListSubscribtion(model));
            Response.AddHeader("content-disposition", "attachment; filename=/Subscribtion.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return View();
            // return new ViewAsPdf("TrialBalanceNewPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }
        public ActionResult GetsubTeam()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير اشتراك الصيانة لفرقة معينة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var subb = Reports.SubscriptionTeam();
            return View(subb);
        }

        [HttpPost]
        public ActionResult GetsubTeam(SubscriptionTeamVM sub)
        {
            var model = Reports.ReportSubscriptionteam(sub);
            return new ViewAsPdf("GetSubscriptionTReam", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }

        public ActionResult SubscriptionStop()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "نقرير حالات الاشتراكات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var substop = Reports.CreateSubscriptionForCreation();
            return View(substop);
        }
        [HttpPost]
        public ActionResult SubscriptionStop(SubscriptionStopVM subVM)
        {
            var model = Reports.subscriptionStop(subVM);
            TempData["SubscriptionStop"] = model;
            TempData.Keep("SubscriptionStop");

            return View("SubscriptionStopList", model);
   
        }
        public ActionResult SubscriptionStopPDF()
        {


            SubscriptionStopVM model = (SubscriptionStopVM)TempData["SubscriptionStop"];
            TempData.Keep("SubscriptionStop");
            return new ViewAsPdf("StopSubscriptionReport", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult SubscriptionStopExcel()
        {
            SubscriptionStopVM model = (SubscriptionStopVM)TempData["SubscriptionStop"];
            TempData.Keep("SubscribtionData");
            var excel = new ExcelSheetTemplate();
            Response.ClearContent();
            Response.BinaryWrite(excel.GetTypeSubscribtion(model));
            Response.AddHeader("content-disposition", "attachment; filename=/Subscribtion.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return View();
            // return new ViewAsPdf("TrialBalanceNewPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }
        public ActionResult makeSubledgerReport()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مركز التكلفة الاجمالي");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var substop = Reports.getDataToMakeReportOfSubLedger();
            return View(substop);
        }
        [HttpPost]
        public ActionResult makeSubledgerReport(SubLedgerReportVM subVM)
        {
            var model = Reports.postDataToMakeReportOfSubLedger(subVM);
            TempData["SubledgerReport"] = model;
            TempData.Keep("SubledgerReport");

            return View("SubledgerReportList", model);
           // return new ViewAsPdf("SubledgerReport", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }
    
        public ActionResult SubledgerReport()
        {

            SubLedgerReportVM model = (SubLedgerReportVM)TempData["SubledgerReport"];
            TempData.Keep("SubledgerReport");
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
     Url.Action("HeaderReport", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

            return new ViewAsPdf(model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;
        }
        public ActionResult SubledgerReportExcel()
        {
            SubLedgerReportVM model = (SubLedgerReportVM)TempData["SubledgerReport"];
            TempData.Keep("SubledgerReport");
            var excel = new ExcelSheetTemplate();
            Response.ClearContent();
            Response.BinaryWrite(excel.GetListTotalSubledger(model));
            Response.AddHeader("content-disposition", "attachment; filename=Subledger.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return View();
            // return new ViewAsPdf("TrialBalanceNewPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }



        public ActionResult GetSubsNonPaid()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير اشتراكات لم يدفع او جزئيا");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = new SubscriptionNonPaidVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult GetSubsNonPaid(SubscriptionNonPaidVM LVM)
        {
            var model = Reports.GetSubscriptionNonPaid(LVM);
       
            return new ViewAsPdf("GetSubsNonPaidPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }
        public ActionResult GetCustomerSubScribtion()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير عملاء الاشتراكات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            var model = Reports.getAllCustomerSub();
            TempData["Model"] = model;
            return View(model);
        }
        public ActionResult GetCustomerSubScribtionPDF()
        {
          
            var model = TempData.Peek("Model");
           
            return new ViewAsPdf( model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }


      
        public ActionResult PrintcatchRecipt(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة سند قبض");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintCatchRecipt(id);
            
            return new ViewAsPdf("PrintcatchRecipt", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult PrintcatchReciptEmail(int id)
        {
            var model = Reports.PrintCatchRecipt(id);
            return new ViewAsPdf("PrintcatchReciptEmail", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult PrintSpentRecipt(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة سند صرف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintSpentRecipt(id);
            return new ViewAsPdf("PrintSpentRecipt", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult PrintSpentReciptEmail(int id)
        {
            var model = Reports.PrintSpentRecipt(id);
            return new ViewAsPdf("PrintSpentReciptEmail", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult prientExpen(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة مصروف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.printExpen(id);
            return new ViewAsPdf("Printexpense", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult GetglMaster(int id)
        {
            var model = Reports.PrintGlMaster(id);
            return new ViewAsPdf("PrintGlMasterDetails", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult PrintExpencesNew(int id)
        {
            var model = Reports.MakeGLExpences(id);
            return new ViewAsPdf("PrintExpencesNew", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult Transfertomentainas(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة تحويل من مخزن لاشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.TransferToMentainans(id);
            return new ViewAsPdf("TransferTomentainan", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        public ActionResult InvenrotyTransfer(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة تحويل مخزني");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.TansferToInventory(id);
            return new ViewAsPdf("InvenrotyTransferMaster", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        public ActionResult ProjectTransfer(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل فاتورة مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.ProjectInvoice(id);
            return new ViewAsPdf("ProjectTransferMaster", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }
        public ActionResult SendEmailProjectTransfer(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ارسال ايميل فاتورة مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.ProjectInvoice(id);

                 ViewAsPdf action = null;
            if (model.CustomerTax != null)
            {
                action = new ViewAsPdf("ProjectTransferMaster", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
            }
            else
            {
                var modelnew = MasterQRInvoice.GetQRMasterByGLCode(model.Project_Code);
                action = new ViewAsPdf("DetailsimpleQR", modelnew) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 30, Right = 15, Top = 10 } };
            }
            var bytearr = action.BuildFile(ControllerContext);
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + model.Project_Code + " " + DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.Second + ".pdf");

            var file = new FileStream(path, FileMode.Create, FileAccess.Write);
            file.Write(bytearr, 0, bytearr.Length);
            file.Close();
          
            var isSend = Utilites.MailManager.SendEmailWithAttachment(model.CustomerEmail, " فاتورة مشروع", "اليك بيانات فاتورة مشروع", path);

            if (isSend == true)
            {
                // System.IO.File.Delete(path);
                return RedirectToAction("DetailsofInvoice", "Project", new { id = id, msg = "تم ارسال فاتورة مشروع للعميل" });
            }
            return RedirectToAction("DetailsofInvoice", "Project", new { id = id, msg = "لم يتم فاتورة مشروع" });

        }

        public ActionResult ProjectTransferDelivery(int id)
        {
            try { 
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "سند توريد فاتورة مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            
                var model = Reports.ProjectInvoice(id);
                return new ViewAsPdf("ProjectTransferDelivery", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
        }
        public ActionResult ProjectTransferWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة فاتورة مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.ProjectInvoice(id);
            return new ViewAsPdf("ProjectTransferWhite", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult SubscripInvoice(int id)
        {
            var model = Reports.SupscriptionInvoic(id);
            return new ViewAsPdf("InvoceSubscription", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        public ActionResult AssetsReportPDF()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير الاصول");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var pack = PackageRecognize.checkValiditeOfAsset(user.Id);
            if (pack.Item2 == true)
            {
                var model = Reports.AllAssetReport();
                return new ViewAsPdf("AssetsReportPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
            }
            else {
                var msgo = "باقتك لا تدعم قسم الاصول";
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });
            }
        }
        public ActionResult ReportOfLoanEmployee()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير سلف الموظفيين");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.getLoanOfEmployee();
            return new ViewAsPdf("ReportOfLoanEmployee", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        public ActionResult GetAllPurchaseWithTax()
        {
            var models = new AllPurchaseInvoicesWithTaxVM();
            return View(models);
        }
        [HttpPost]
        public ActionResult GetAllPurchaseWithTax(AllPurchaseInvoicesWithTaxVM purchase)
        {
            var models = Reports.GetAllPurchaseInvoicesWithTax(purchase);
            return new ViewAsPdf("AllPurchaseWithTax", models) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult GetAllPurchaseWithOutTax()
        {
            var models = new AllPurchaseInvoicesWithTaxVM();
            return View(models);
        }
        [HttpPost]
        public ActionResult GetAllPurchaseWithOutTax(AllPurchaseInvoicesWithTaxVM purchase)
        {
            var models = Reports.GetAllPurchaseInvoicesWithoutTax(purchase);
            return new ViewAsPdf("AllPurchaseWithOutTax", models) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult GetAllSalesWithOutTax()
        {
            var models = new AllSalseInvoicesWithOutTaxVM();
            return View(models);
        }

        [HttpPost]
        public ActionResult GetAllSalesWithOutTax(AllSalseInvoicesWithOutTaxVM invoices)
        {
            var models = Reports.GetAllInvoiceSalseWithoutTax(invoices);
            return new ViewAsPdf("AllSalesWithOutTax", models) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        /// <summary>
        /// print sallary of employee all of them
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PrintSallary(int id)
        {
            var model = Reports.PrintAllEmployee(id);
            return new ViewAsPdf("PrintSallary", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        /// <summary>
        /// print Loan Of Employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PrintLoan(int id)
        {
            var model = Reports.PrintAllEmployeeLoan(id);
            return new ViewAsPdf("PrintLoan", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult Printsub(int subIDid)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة اشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.SubscriptionPrint(subIDid);
            return new ViewAsPdf("Printsub", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult Printsubcolor(int subIDid)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل اشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.SubscriptionPrint(subIDid);
            return new ViewAsPdf("Printsubcolor", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }
        public ActionResult SendSUbEmail(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ارسال ايميل اشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.SubscriptionPrint(id);
            ViewAsPdf action = null;
            if (model.customer_Tax != null)
            {
                 action = new ViewAsPdf("Printsubcolor", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
            }
            else
            {
                var modelnew = MasterQRInvoice.GetQRMasterByGLCode(model.Sub_Code);
                action = new ViewAsPdf("DetailsimpleQR", modelnew) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 30, Right = 15, Top = 10 } };
            }
           
            var bytearr = action.BuildFile(ControllerContext);

            string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + model.Sub_Code + " " + DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.Second + ".pdf");

            var file = new FileStream(path, FileMode.Create, FileAccess.Write);
            file.Write(bytearr, 0, bytearr.Length);
            file.Close();

            var isSend = Utilites.MailManager.SendEmailWithAttachment(model.Customer_Email, "فاتورة اشتراك", "اليك بيانات الفاتورة", path);
            ////System.IO.File.Delete(path);
            if (isSend == true)
            {
                // System.IO.File.Delete(path);
                return RedirectToAction("DetailofSubscription", "Subscription", new { id = id, msg = "تم ارسال الفاتورة للعميل" });
            }
            return RedirectToAction("DetailofSubscription", "Subscription", new { id = id, msg = "لم يتم ارسال الفاتورة" });
        }


        public ActionResult PrintAllSallary(int id)
        {
            var model = Reports.PrintAllEmployee(id);
            return new ViewAsPdf("PrintAllSallary", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult GLReportAndDetails(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة قيد ");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.ReportOfEachGLMaster(id);
            return new ViewAsPdf("GLReportPrintPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
        }
        public ActionResult SubInvReport(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل سند قبض اشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.reportofSubInvoice(id);
            return new ViewAsPdf("SubInvReportPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
        }
        public ActionResult SendEmailSubRecipt(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ارسال ايميل سند قبض اشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.reportofSubInvoice(id);
            var action= new ViewAsPdf("SubInvReportPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
            var bytearr = action.BuildFile(ControllerContext);
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + model.InvoiceCode + " " + DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.Second + ".pdf");

            var file = new FileStream(path, FileMode.Create, FileAccess.Write);
            file.Write(bytearr, 0, bytearr.Length);
            file.Close();
            var customer = Customer.GetCustomerByCode(model.CustomerCode);
            var isSend = Utilites.MailManager.SendEmailWithAttachment(customer.Customer_Mail, " سند قبض اشتراك", "اليك بيانات سند قبض اشتراك", path);

            if (isSend == true)
            {
                // System.IO.File.Delete(path);
                return RedirectToAction("DetailOFSubInv", "Subscription", new { id = id, msg = "تم ارسال سند القبض للعميل" });
            }
            return RedirectToAction("DetailOFSubInv", "Subscription", new { id = id, msg = "لم يتم سند القبض" });

        }
        public ActionResult SubInvReportWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة سند قبض اشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.reportofSubInvoice(id);
            return new ViewAsPdf("SubInvReportWhite", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult SubInvReportSpecial(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة سند قبض اشتراك");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.reportofSubInvoice(id);
            string customSwitchesfooter = string.Format(" --print-media-type --page-offset 2 --footer-center [page] --allow {0} --footer-html {0} --footer-spacing 0 ",
      Url.Action("Footer1", "InvoiceQRExternalUseres", new { Branch_Id = model.branchId }, protocol));
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
    Url.Action("Header1", "InvoiceQRExternalUseres", new { Branch_Id = model.branchId }, protocol));

            return new ViewAsPdf(model)
            {
                PageMargins = { Left = 5, Right = 5 },
                CustomSwitches = customSwitchesfooter + customSwitchesheader

            }
            ;
        }
        public ActionResult StoreReport(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة منتجات مخزن");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.ReportofStor(id);
            return new ViewAsPdf("StoreReportPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
        }


        public ActionResult GetEmployeeRecivable()
        {
            var models = Reports.EmployeeRecivableForCreation();
            return View(models);
        }
        [HttpPost]
        public ActionResult GetEmployeeRecivable(EmployeesrecivableVM ERVM)
        {
            var model = Reports.EmployeeRecivableFor(ERVM);
            return new ViewAsPdf("EmployeeRecivablePDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
        }
        public ActionResult MakeWareHouseGet()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقارير تكاليف المخازن");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var models = Reports.MakeWareHouseGet();
            return View(models);
        }
        [HttpPost]
        public ActionResult MakeWareHouseGet(WarehousesReportVM Stores)
        {
            var model = Reports.MakeWareHousePost(Stores);
            return new ViewAsPdf("MakeWareHouseGetPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
        }

        public ActionResult GetAllReportsOfStore()
        {
            return View();
        }

        public ActionResult GetAllSalesWithBranch()
        {
            var models = Reports.GetAllInvoiceSalseWithBranchForCreation();
            return View(models);
        }

        [HttpPost]
        public ActionResult GetAllSalesWithBranch(AllSalseInvoicesWithOutTaxVM invoices)
        {
            var models = Reports.GetAllInvoiceSalseWithBranch(invoices);
            return new ViewAsPdf("GetAllSalesWithBranchPDF", models) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        public ActionResult transfaretoproject(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var trans = TransferToProjectMaster.GetTransferProjectById(id);
            var employeeAction = true;
            if (trans.Trans_Code.StartsWith("TTPRO-"))
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة تحويل من مخزن الى مشروع");
            }
            else
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة تحويل من مشروع الى مخزن");
            }



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.trnasfarePrint(id);
            return new ViewAsPdf("transfaretoproject", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
        }

        public ActionResult PrintProPayment(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل سند صرف مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.printProPayment(id);
            return new ViewAsPdf("PrintProPayment", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
        }
        public ActionResult PrintProPaymentWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة سند صرف مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.printProPayment(id);
            return new ViewAsPdf("PrintProPaymentWhite", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }

        public ActionResult printProRecipt(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل سند قبض مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.printProRecipt(id);
            return new ViewAsPdf("printProRecipt", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
        }
        public ActionResult SendEmailProRecipt(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ارسال ايميل سند قبض مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.printProRecipt(id);
             
            var action = new ViewAsPdf("printProRecipt", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
            var bytearr = action.BuildFile(ControllerContext);
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + model.ReciptCode + " " + DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.Second + ".pdf");

            var file = new FileStream(path, FileMode.Create, FileAccess.Write);
            file.Write(bytearr, 0, bytearr.Length);
            file.Close();
            var customer = Customer.GetCustomerByCode(model.Customer_Code);
            var isSend = Utilites.MailManager.SendEmailWithAttachment(customer.Customer_Mail, " سند قبض مشروع", "اليك بيانات سند قبض مشروع", path);

            if (isSend == true)
            {
                // System.IO.File.Delete(path);
                return RedirectToAction("Details", "ProjectRecipt", new { id = id, msg = "تم ارسال سند القبض للعميل" });
            }
            return RedirectToAction("Details", "ProjectRecipt", new { id = id, msg = "لم يتم سند القبض" });

        }
        public ActionResult printProReciptWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة سند قبض مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.printProRecipt(id);
            return new ViewAsPdf("printProReciptWhite", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult printProReciptSpecial(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة سند قبض مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.printProRecipt(id);
            string customSwitchesfooter = string.Format(" --print-media-type --page-offset 2 --footer-center [page] --allow {0} --footer-html {0} --footer-spacing 0 ",
           Url.Action("Footer1", "InvoiceQRExternalUseres", new { Branch_Id = model.branchId }, protocol));
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
    Url.Action("Header1", "InvoiceQRExternalUseres", new { Branch_Id = model.branchId }, protocol));

            return new ViewAsPdf(model)
            {
                PageMargins = { Left = 5, Right = 5 },
                CustomSwitches = customSwitchesfooter + customSwitchesheader

            }
            ;
        }
        public ActionResult printSubTemp(int id)
        {
            var model = Reports.printNonPaidSubs(id);
            return new ViewAsPdf("printSubTemp", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult printSubTempEmail(int id)
        {
            var model = Reports.printNonPaidSubs(id);
            return new ViewAsPdf("printSubTempEmail", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
        }
        public ActionResult SendSUbTempEmail(int id)
        {
            var model = Reports.printNonPaidSubs(id);

            var action = new ViewAsPdf("printSubTempEmail", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
            var bytearr = action.BuildFile(ControllerContext);

            string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + model.Sub_Code + " " + DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.Second + ".pdf");

            var file = new FileStream(path, FileMode.Create, FileAccess.Write);
            file.Write(bytearr, 0, bytearr.Length);
            file.Close();

            var isSend = Utilites.MailManager.SendEmailWithAttachment(model.Customer_Email, "اشعار اشتراك", "اليك بيانات الاشعار", path);
            ////System.IO.File.Delete(path);
            if (isSend == true)
            {
                // System.IO.File.Delete(path);
                return RedirectToAction("DetailofSubscription", "Subscription", new { id = id, msg = "تم ارسال الفاتورة للعميل" });
            }
            return RedirectToAction("DetailofSubscription", "Subscription", new { id = id, msg = "لم يتم ارسال الفاتورة" });
        }

        public ActionResult printSubTemps()
        {
            var model = Reports.printNonPaidSubss();
            return new ViewAsPdf("printSubTemps", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
        }

        [HttpGet]
        public ActionResult GetAllDoneOfEmployee(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfHR(user.Id);
            if (pack.Item2 == true)
            {
                var model = new DoingOfEmployeeVM();
                model.Emp_Id = id;
                return View(model);
            }
            else
            {
                var msgo = "باقتك لا تدعم قسم الموارد البشرية";
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });

            }
        }
        public ActionResult GetAllDoneOfEmployee(DoingOfEmployeeVM doingOfEmployeeVM )
        {
            var model = Reports.DoingOfEmployee(doingOfEmployeeVM );
            return new ViewAsPdf("GetAllDoneOfEmployeePdf", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
        }
        public ActionResult PrintPurchaseInvoiceNew(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var model = Reports.PrintPurchInvoiceNew(id);
            var employeeAction = false;
            if (model.storeId != null && model.storeId > 0)
            {
                 employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة فاتورة مشتريات");
            }
            else
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة فاتورة مشتريات اصل");
            }

            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
          

            return new ViewAsPdf("PrintPurchaseInvoiceNew", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult PrintPurchaseInvoiceNewEmail(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var model = Reports.PrintPurchInvoiceNew(id);

            
            var employeeAction = false;
            if (model.storeId != null && model.storeId > 0)
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل فاتورة مشتريات");
            }
            else
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة ايميل فاتورة مشتريات اصل");
            }

            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
Url.Action("Header", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

            return new ViewAsPdf("PrintPurchaseInvoiceNewEmail", model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;
            //return new ViewAsPdf("PrintPurchaseInvoiceNewEmail", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }
        public ActionResult PrintPurchaseOrder(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة امر شراء ايميل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintPurchOrder(id);
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
Url.Action("Header", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

            return new ViewAsPdf("PrintPurchaseOrder", model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;
            //return new ViewAsPdf("PrintPurchaseOrder", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 5} };
        }
        public ActionResult PrintPurchaseOrderWhite(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة امر شراء ايميل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintPurchOrder(id);

            return new ViewAsPdf("PrintPurchaseOrderWhite", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite } };
        }
        public ActionResult PrintPurchaseOrderWithoutPrice(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة امر شراء بكج");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = Reports.PrintPurchOrder(id);
            return new ViewAsPdf("PrintPurchaseOrderWithoutPrice", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 5 } };
        }
        public ActionResult GetTreasuryReport(int id)
        {
            var model = Reports.GetLedgerTreasuryVM(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult GetTreasuryReport(LedgerPickerVM vm)
        {
            var model = Reports.TreasuryReport(vm);
            return new ViewAsPdf("GetTreasuryReportPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        public ActionResult GetBankReport(int id)
        {
            var model = Reports.GetLedgerBankVM(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult GetBankReport(LedgerPickerVM vm)
        {
            var model = Reports.BankReport(vm);
            return new ViewAsPdf("GetBankReportPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        public ActionResult getDetailsofEmployee(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var pack = PackageRecognize.checkValiditeOfHR(user.Id);
            if (pack.Item2 == true)
            {
                var model = Reports.getDetailsofCustomer(id);
                return View(model);
            }
            else
            {
                var msgo = "باقتك لا تدعم قسم الموارد البشرية";
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });

            }
        }
        [HttpPost]
        public ActionResult getDetailsofEmployee(CustomerTransactionGettingDateVM CVM)
        {
            var model = Reports.EmployeeTransactionDetails(CVM);
            return new ViewAsPdf("ViewListOFEmployeeTransactionPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult getBankReportSalary()
        {
           var Salary = (List<EmployeeSalaryMonth>)TempData.Peek("EmployeeData");
           
            List<SalaryReportForBankVM> model = Reports.GetSalaryReportForBank(Salary);
            TempData["BankReportSal"] = model;
            return View(model);
        }
        public ActionResult getBankReportSalaryPDF()
        {
            var Model = (List<SalaryReportForBankVM>)TempData.Peek("BankReportSal");

         
            return new ViewAsPdf("getBankReportSalaryPDF", Model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }


    }
}