using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Rotativa;

namespace Accountingsystem.Controllers
{
    public class InvoiceQRExternalUseresController : Controller
    {
        //string footer = "--footer-center \"Name: " + "XYZ" + "  DOS: " +
        //    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  Page: [page]/[toPage]\"" +
        //    " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
        //string footer = "--footer-right \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";
      string footer = "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";
        int bottomWhite = 43;     
        int TopWhite = 40;
        string protocol = "http";
        // GET: InvoiceQRExternalUseres
        public ActionResult Index()
        {
            return View();
        }

        // GET: InvoiceQRExternalUseres/Details/5
        public ActionResult Details(string id)
        {
            var invoice = MasterQRInvoice.GetMasterQRInvoiceById(id);
            return new ViewAsPdf(invoice) {   MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 30, Right = 5, Top = 10 } }; ;

        }
        public ActionResult DetailSpecial(int id)
        {
            var invoice = MasterQRInvoice.GetMasterQRInvoiceById(id);

            string customSwitchesfooter = string.Format(" --print-media-type --page-offset 2 --footer-center [page] --allow {0} --footer-html {0} --footer-spacing 0 ",
          Url.Action("Footer1", "InvoiceQRExternalUseres", new { Branch_Id = invoice.Branch_Id }, protocol));
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
    Url.Action("Header1", "InvoiceQRExternalUseres", new { Branch_Id = invoice.Branch_Id }, protocol));

            return new ViewAsPdf(invoice)
            {
                PageMargins = { Left = 5, Right = 5 },
                CustomSwitches = customSwitchesfooter + customSwitchesheader
            
            }
            ;
           // return new ViewAsPdf(invoice) {   MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 30, Right = 5, Top = 10 } }; ;

        }
        public ActionResult DetailsimpleSpecial(int id)
        {
            var invoice = MasterQRInvoice.GetMasterQRInvoiceById(id);

            string customSwitchesfooter = string.Format(" --print-media-type --page-offset 2 --footer-center [page] --allow {0} --footer-html {0} --footer-spacing 0 ",
          Url.Action("Footer1", "InvoiceQRExternalUseres", new { Branch_Id = invoice.Branch_Id }, protocol));
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
    Url.Action("Header1", "InvoiceQRExternalUseres", new { Branch_Id = invoice.Branch_Id }, protocol));

            return new ViewAsPdf(invoice)
            {
                PageMargins = { Left = 5, Right = 5 },
                CustomSwitches = customSwitchesfooter + customSwitchesheader

            }
            ;
            // return new ViewAsPdf(invoice) {   MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 30, Right = 5, Top = 10 } }; ;

        }
        public ActionResult Detailsimple(int id)
        {
            var invoice = MasterQRInvoice.GetMasterQRInvoiceById(id);
            

          //  string customSwitchesfooter = string.Format(" --print-media-type --page-offset 2 --footer-center [page] --allow {0} --footer-html {0} --footer-spacing 0 ",
          //Url.Action("Footer", "InvoiceQRExternalUseres", new { Branch_Id = invoice.Branch_Id }, "http"));
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
    Url.Action("Header", "InvoiceQRExternalUseres", new { Branch_Id = invoice.Branch_Id }, protocol));

            return new ViewAsPdf(invoice)
            {
                PageMargins = { Left = 5, Right = 5 },
                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader+footer
            }
            ; 

        }
        [AllowAnonymous]
     
        public ActionResult Footer1(int? Branch_Id)
        {
            var branch =Branch.getAddressForInvoice(Branch_Id);
            return View(branch);
        }
        [AllowAnonymous]

        public ActionResult Header1(int? Branch_Id)
        {
            var branch = Branch.getAddressForInvoice(Branch_Id);
            return View(branch);
        }
        [AllowAnonymous]

        public ActionResult HeaderReport(int? Branch_Id)
        {
            var branch = Branch.getAddressForInvoice(Branch_Id);
            return View(branch);
        }
        [AllowAnonymous]
       
        public ActionResult Header(int? Branch_Id)
        {
            var branch = Branch.getAddressForInvoice(Branch_Id);
            return View(branch);
        }
        public ActionResult DetailsimpleWhite(int id)
        {
            var invoice = MasterQRInvoice.GetMasterQRInvoiceById(id);
            return new ViewAsPdf(invoice) {   MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 40, Right = 5, Top = 40 } }; ;

        }

        public ActionResult DetailReverse(string serial)
        {
            var invoice = MasterQRInvoice.GetReversQRInvoiceById(serial);
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
     Url.Action("Header", "InvoiceQRExternalUseres", new { Branch_Id = invoice.MasterQRInvoice.Branch_Id }, protocol));

            return new ViewAsPdf(invoice)
            {
                PageMargins = { Left = 5, Right = 5 },
                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader + footer
            }
            ;

        }
        public ActionResult DetailReverseWhite(string serial)
        {
            var invoice = MasterQRInvoice.GetReversQRInvoiceById(serial);
            return new ViewAsPdf(invoice) {   MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 40, Right = 5, Top = 40 } }; ;

        }
        public ActionResult DetailSimpleReverse(string serial)
        {
            var invoice = MasterQRInvoice.GetReversQRInvoiceById(serial);
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
    Url.Action("Header", "InvoiceQRExternalUseres", new { Branch_Id = invoice.MasterQRInvoice.Branch_Id }, protocol));

            return new ViewAsPdf(invoice)
            {
                PageMargins = { Left = 5, Right = 5 },
                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader + footer
            }
            ;

        }
        public ActionResult DetailSimpleReverseWhite(string serial)
        {
            var invoice = MasterQRInvoice.GetReversQRInvoiceById(serial);
            return new ViewAsPdf(invoice) {   MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 40, Right = 5, Top = 40 } }; ;

        }
        public ActionResult DetailReverseTemparure(string serial)
        {
            var invoice = MasterQRInvoice.GetReversQRInvoiceById(serial);
            return  View(invoice);

        }

        public ActionResult PrintEmail1(int id)
        {
            var invoice = MasterQRInvoice.GetMasterQRInvoiceById(id);
    //        string customSwitchesfooter = string.Format(" --print-media-type  --page-offset 2 --footer-center [page] --allow {0} --footer-html {0} --footer-spacing 0 ",
    //Url.Action("Footer1", "InvoiceQRExternalUseres", new { Branch_Id = invoice.Branch_Id }, protocol) ) + footer;
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
   Url.Action("Header", "InvoiceQRExternalUseres", new { Branch_Id = invoice.Branch_Id }, protocol))+footer;
            //string customSwitchesfooter = string.Format(footer);
            return new ViewAsPdf(invoice)
            {
                PageMargins = { Left = 5,  Right = 5},


                CustomSwitches = footer+ customSwitchesheader
            }
            ;


        }
        public ActionResult PrintWhite1(int id)
        {
            var invoice = MasterQRInvoice.GetMasterQRInvoiceById(id);
            return new ViewAsPdf(invoice) {  MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite }, CustomSwitches = footer };

        }
        public ActionResult PrintEmail2(int id)
        {
            var invoice = MasterQRInvoice.GetMasterQRInvoiceById(id);
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
 Url.Action("Header", "InvoiceQRExternalUseres", new { Branch_Id = invoice.Branch_Id }, protocol));

            return new ViewAsPdf(invoice)
            {
                PageMargins = { Left = 5, Right = 5 },
                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader +  footer
            }
            ;


        }
        public ActionResult PrintWhite2(int id)
        {
            var invoice = MasterQRInvoice.GetMasterQRInvoiceById(id);
            return new ViewAsPdf(invoice) { MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = bottomWhite, Right = 5, Top = TopWhite }, CustomSwitches = footer };

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
                var companyPrint = InvoicePrintSetting.getCompanyPrint(1);
                var modelnew = MasterQRInvoice.GetQRMasterByGLCode(model.Serial_Number);
                action = new ViewAsPdf(companyPrint.PrintLogo, modelnew) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 } };
            }
            else
            {
                var companyPrint = InvoicePrintSetting.getCompanyPrint(2);
                var modelnew = MasterQRInvoice.GetQRMasterByGLCode(model.Serial_Number);
                action = new ViewAsPdf(companyPrint.printWhite, modelnew) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 30, Right = 15, Top = 10 } };
            }

            var bytearr = action.BuildFile(ControllerContext);

            string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + model.Serial_Number + " " + DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.Second + ".pdf");

            var file = new FileStream(path, FileMode.Create, FileAccess.Write);
            file.Write(bytearr, 0, bytearr.Length);
            file.Close();

            var isSend = Utilites.MailManager.SendEmailWithAttachment(model.customerEmail, "فاتورة اجل", "اليك بيانات الفاتورة", path);
            ////System.IO.File.Delete(path);
            if (isSend == true)
            {
                // System.IO.File.Delete(path);
                return RedirectToAction("Details", "InvoiceMaster", new { id = id, msg = "تم ارسال الفاتورة للعميل" });
            }
            return RedirectToAction("Details", "InvoiceMaster", new { id = id, msg = "لم يتم ارسال الفاتورة" });
        }
    }
}
