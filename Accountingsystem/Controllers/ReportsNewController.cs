using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.ViewModel.ReportsNewVM;
using Accountingsystem.Models;
using System.Web.Mvc;
using Rotativa;
using Accountingsystem.Utilites;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Accountingsystem.ViewModel.PrintVM;
using Accountingsystem.ViewModel;

namespace Accountingsystem.Controllers
{
    public class ReportsNewController : Controller
    {
        string footer = "--footer-right \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";
        string protocol = "https";

        public ActionResult getAllAsset()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير الاصول الاجمالي");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            var model = ReportsNew.GetAllAsset(); ;
            return new ViewAsPdf("getAllAsset", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }


        /// <summary>
        /// purchuse Report 
        /// </summary>
        /// <returns></returns>
        /// 


        public ActionResult getdateofPurchase()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير المشتريات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = new PurchaseDatesVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult getdateofPurchaseList(PurchaseDatesVM vm)
        {
            var model = ReportsNew.allPurchasePrint(vm);

            TempData["dateofPurchase"] = model;
            TempData.Keep("dateofPurchase");

            return View("getdateofPurchaseList", model);
        }
        public ActionResult getdateofPurchasePdf()
        {

            PurchaseDatesVM model = (PurchaseDatesVM)TempData["dateofPurchase"];
            TempData.Keep("dateofPurchase");
            return new ViewAsPdf("NewPurchasePrint", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult getdateofPurchaseExcel()
        {
            PurchaseDatesVM model = (PurchaseDatesVM)TempData["dateofPurchase"];
            TempData.Keep("dateofPurchase");
            var excel = new ExcelSheetTemplate();
            Response.ClearContent();
           Response.BinaryWrite(excel.GetListPurchase(model));
            Response.AddHeader("content-disposition", "attachment; filename=/PurchaseReport.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return View();
            // return new ViewAsPdf("TrialBalanceNewPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }
       
     
        public ActionResult getdateofPurchaseclassification()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير تصنيف مشتريات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.purhcaseforgettingreport();
            return View(model);
        }
        [HttpPost]
        public ActionResult getdateofPurchaseclassification(ClassifyPurchaseInvoiceVM PDVM)
        {
            var model = ReportsNew.CalssifiedPurchasePrint(PDVM);
            return new ViewAsPdf("NewPurchaseWithclasification", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult getdateofRecall()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مردود مشتريات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = new PurchaseDatesVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult getdateofRecall(PurchaseDatesVM PDVM)
        {
            var model = ReportsNew.allRecallPrint(PDVM);
            return new ViewAsPdf("NewRecallPrint", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        public ActionResult AllSupplierPrint()
        {
            var model = ReportsNew.AllSuppliersPrint();
            return new ViewAsPdf("AllSupplierPrint", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 1, Bottom = 15, Right = 1, Top = 15 } };
        }
        [HttpGet]
        public ActionResult getSupplierWithDate()
        {
            var model = new SupplierGettingDateVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult getSupplierWithDate(SupplierGettingDateVM VM)
        {
            var model = ReportsNew.getSuupliersWithDays(VM);
            return new ViewAsPdf("getSupplierWithDatePrint", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        /// <summary>
        /// store Report
        /// </summary>
        /// <returns></returns>
        public ActionResult getDetailOfStore()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير كميات المخزن بالتكلفة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.getquantityForStoreGet();
            return View(model);
        }
        [HttpPost]
        public ActionResult getDetailOfStore(StoreChoose store)
        {
            var model = ReportsNew.GetQuantityOfStore(store);
            return new ViewAsPdf("getDetailOfStorePDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        [HttpGet]
        public ActionResult CardOfProductQuanity()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "كارت الصنف بالكمية");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.getDataForQuantityOfProduct();
            return View(model);
        }
        public ActionResult CardOfProductQuanity(ProductQuantityListVM VM)
        {
            var model = ReportsNew.makeReportPostOf(VM);
            return new ViewAsPdf("CardOfProductQuanityPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        public ActionResult GetALLProduct(int ?all)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير الاصناف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.getallProduct(all);
            TempData["AllProductData"] = model;
            TempData.Keep("AllProductData");

            return View("GetALLProductList", model);
           
           
        }
        public ActionResult GetALLProductPdf()
        {

            ProductListVM model = (ProductListVM)TempData["AllProductData"];
            TempData.Keep("AllProductData");
            return new ViewAsPdf("GetALLProduct", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }
        public ActionResult GetALLProductExcel()
        {
            ProductListVM model = (ProductListVM)TempData["AllProductData"];
            TempData.Keep("AllProductData");
            var excel = new ExcelSheetTemplate();
            Response.ClearContent();
            Response.BinaryWrite(excel.GetListOfAllProduct(model));
            Response.AddHeader("content-disposition", "attachment; filename=/ClassfiySales.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return View();
            // return new ViewAsPdf("TrialBalanceNewPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }
        public ActionResult CardOfStore()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "كارت الصنف فى المخزن");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.getDataForProductCard();
            return View(model);
        }
        [HttpPost]
        public ActionResult CardOfStore(ProductCardVM VM)
        {
            ProductCardVM model = new ProductCardVM();
            
                model = ReportsNew.makeReportPostOf(VM);
            
            return new ViewAsPdf("CardOfStorePDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        ///////// Project Section
        public ActionResult PrintPorjectInvoiceSales()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مبيعات المشروعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = new ProjectDateVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult PrintPorjectInvoiceSales(ProjectDateVM PDVM)
        {
           var model = ReportsNew.allProjectInvoicesForEmployee(PDVM);
            return new ViewAsPdf("ProjectInvoiceSalesPrint", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }
        public ActionResult PrintPorjectInvoiceSalesForEmployee()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مبيعات المشاريع لموظف معيين");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.ProjectsByEmployeeForCreation();
            return View(model);
        }
        [HttpPost]
        public ActionResult PrintPorjectInvoiceSalesForEmployee(ProjectDateVM PDVM)
        {
            var model = ReportsNew.allProjectInvoicesForEmployee(PDVM);
            return new ViewAsPdf("PrintPorjectInvoiceSalesForEmployeePDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }
        public ActionResult PrintPorjectBalance()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير تحليل ارصدة المشروعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = new ProjectDateVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult PrintPorjectBalance(ProjectDateVM PDVM)
        {
            var model = ReportsNew.ProjectsWithBalance(PDVM);
            return new ViewAsPdf("PorjectBalanceReport", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }
        public ActionResult PrintstoretoProject()
        {
            var model = ReportsNew.prepareTransProject();
            return View(model);
        }
        [HttpPost]
        public ActionResult PrintstoretoProject(TransfarefromtoProjectCreationVM PDVM)
        {
            var model = ReportsNew.allTransfaretoProjectstore(PDVM);
            return new ViewAsPdf("GetallFromStoretoProject", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult Printprojecttostore()
        {
            var model = ReportsNew.prepareTransProject();
            return View(model);
        }
        [HttpPost]
        public ActionResult Printprojecttostore(TransfarefromtoProjectCreationVM PDVM)
        {
            var model = ReportsNew.allTransfaretostoreProject(PDVM);
            return new ViewAsPdf("GetallFromProjecttoStore", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }


        // Reports for Sales 
        public ActionResult getdateofSales()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = new SalesDatesVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult getdateofSales(SalesDatesVM PDVM)
        {
            var model = ReportsNew.allSalesReport(PDVM);
            return new ViewAsPdf("SalesReportPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        public ActionResult getdateofRecallSales()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مردود مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = new SalesDatesVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult getdateofRecallSales(SalesDatesVM PDVM)
        {
            var model = ReportsNew.allRecallSalesPrint(PDVM);
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",

                Url.Action("HeaderReport", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

            return new ViewAsPdf("ReversSalesPDF", model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;
            //return new ViewAsPdf("ReversSalesPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult getdateofSalesclassification()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير تصنيف مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.SalesforgettingreportClassify();
            return View(model);
        }
        [HttpPost]
        public ActionResult getdateofSalesclassification(ClassifySalesInvoiceVM PDVM)
        {
            var model = ReportsNew.CalssifiedSalesPrint(PDVM);
            return new ViewAsPdf("SalesClassfiyedPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        public ActionResult SalesByEmployee()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير المبيعات بالموظف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.SalesByEmployeeForCreation();
            return View(model);
        }
        [HttpPost]
        public ActionResult SalesByEmployee(ClassifySalesInvoiceVM PDVM)
        {
            var model = ReportsNew.allSalesByEmployeeReport(PDVM);
            return new ViewAsPdf("SalesByEmployeePDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };

        }
        public ActionResult SalesByEmployeeInvoiceRecipt()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير المبيعات بالموظف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.SalesByEmployeeForCreation();
            return View(model);
        }
        [HttpPost]
        public ActionResult SalesByEmployeeInvoiceRecipt(ClassifySalesInvoiceVM PDVM)
        {
            var model = ReportsNew.allSalesInvoiceByEmployeeReport(PDVM);
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",

                Url.Action("HeaderReport", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

            return new ViewAsPdf("SalesByEmployeeInvoiceReciptPDF", model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;
            //return new ViewAsPdf("SalesByEmployeeInvoiceReciptPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };

        }
        public ActionResult SalesByEmployeeForAllSales()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مبيعات الموظف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var pack = PackageRecognize.checkValiditeOfHR(user.Id);
            if (pack.Item2 == true)
            {
                var model = ReportsNew.SalesByEmployeeForCreation();
                return View(model);
            }
            else
            {
                var msgo = "باقتك لا تدعم قسم الموارد البشرية";
                return RedirectToAction("Packagevalidate", "Reports", new { msg = msgo });

            }
        }
        [HttpPost]
        public ActionResult SalesByEmployeeForAllsales(ClassifySalesInvoiceVM PDVM)
        {
            var model = ReportsNew.allSalesByEmployeeReportForAllSystem(PDVM);
            return new ViewAsPdf("SalesByEmployeeForAllsalesPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };

        }
        public ActionResult OfferPriceReport()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير عروض الاسعار");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = new SalesDatesVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult OfferPriceReport(SalesDatesVM PDVM)
        {
            var model = ReportsNew.allOfferPriceReport(PDVM);
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",

                Url.Action("HeaderReport", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

            return new ViewAsPdf("OfferPriceReportPDF", model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;

            //return new ViewAsPdf("OfferPriceReportPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };

        }
        public ActionResult AllCustomerPrint()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير معلومات العملاء");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.AllCustomerPrint();
            return new ViewAsPdf("AllCustomerPrint", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }

        [HttpGet]
        public ActionResult getCustomerWithDate()
        {
            var model = new SupplierGettingDateVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult getCustomerWithDate(SupplierGettingDateVM VM)
        {
            var model = ReportsNew.getCustomerWithDays(VM);
            return new ViewAsPdf("getCustomerWithDatePDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult getStatisticCustomer(int ? cust)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير اعمار زمم العملاء");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = new CustomerStatisticListVM();
            model.cust_Id = cust;
            return View(model);
        }
        [HttpPost]
        public ActionResult getStatisticCustomer(CustomerStatisticListVM VM)
        {
            var model = ReportsNew.GetStatisticListVM(VM);
            return new ViewAsPdf("getStatisticCustomerPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult getStatisticSupplier(int? Supp)
        {
            var model = new CustomerStatisticListVM();
            model.Supp_Id = Supp;
            return View(model);
        }
        [HttpPost]
        public ActionResult getStatisticSupplier(CustomerStatisticListVM VM)
        {
            var model = ReportsNew.GetStatisticListSupplierVM(VM);
            return new ViewAsPdf("getStatisticSupplierPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        [HttpGet]
        public ActionResult makeSalesProductReportForAllSales(string msg)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مبيعات بالاصناف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
           
            var model = ReportsNew.prepareReportforAllSales();

            ViewBag.Message = msg;
            return View(model);
        }
        [HttpPost]
        public ActionResult makeSalesProductReportForAllSales(SalesOfProductVM VM)
        {
            var model = ReportsNew.makeSalesProductReportForAllSales(VM);
            TempData["makeSalesProduct"] = model;
            TempData.Keep("makeSalesProduct");

           
            if (model == null)
            {
                return RedirectToAction("makeSalesProductReportForAllSales", "ReportsNew", new { msg = "قم بأ ختيار اولا" });
            }
            else
            {
                return View("makeSalesProductReportForAllSalesList", model);
         
            }
        }

        public ActionResult makeSalesProductReportForAllSalesPDF()
        {

            SalesOfProductVM model = (SalesOfProductVM)TempData["makeSalesProduct"];
            TempData.Keep("makeSalesProduct");
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",

                Url.Action("HeaderReport", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

            return new ViewAsPdf("makeSalesProductReportForAllSalesPDF", model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;
            //return new ViewAsPdf("makeSalesProductReportForAllSalesPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        [HttpGet]
        public ActionResult MakeSalesProductReportForAllSalesThermal()
        {

            SalesOfProductVM model = (SalesOfProductVM)TempData["makeSalesProduct"];
            TempData.Keep("makeSalesProduct");
            return View(model);

        }

        [HttpGet]
        public ActionResult ClassifySales(string msg)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            //var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير تصنيف مبيعات");

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مبيعات");

            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.prepareClassifiySales();
            ViewBag.Message = msg;
            return View(model);
        }
        public ActionResult ClassifySales(SalesClassffied VM)
        {
          

            var model = ReportsNew.prepareClassifiySalesPOST(VM);
            if (model == null)
            {
                return RedirectToAction("ClassifySales", "ReportsNew", new { msg = "قم بأ ختيار اولا" });
            }
            else
            {
                TempData["ClassfiyData"] = model;
                TempData.Keep("ClassfiyData");

                return View("ClassifySalesList", model);
            }


            
        }
      
        public ActionResult ClassifySalesPdf()
        {

            SalesClassffied model = (SalesClassffied)TempData["ClassfiyData"];
            TempData.Keep("ClassfiyData");
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",

                Url.Action("HeaderReport", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

            return new ViewAsPdf("ClassifySalesPdf", model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;
        }
        public ActionResult ClassifySalesExcel()
        {
            SalesClassffied model = (SalesClassffied)TempData["ClassfiyData"];
            TempData.Keep("ClassfiyData");
            var excel = new ExcelSheetTemplate();
            Response.ClearContent();
            Response.BinaryWrite(excel.GetListSalesClassify(model));
            Response.AddHeader("content-disposition", "attachment; filename=/ClassfiySales.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return View();
            // return new ViewAsPdf("TrialBalanceNewPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }


        public ActionResult makeSalesProductForCustomer()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مبيعات العملاء بالتفاصيل");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.prepareReportforCustomerSales();

            return View(model);
        }
        [HttpPost]
        public ActionResult makeSalesProductForCustomer(SalesCusomerReport VM)
        {
            var model = ReportsNew.makecustomerSalesReport(VM);
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",

                Url.Action("HeaderReport", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

            return new ViewAsPdf("makeSalesProductForCustomerPDF", model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;


            //return new ViewAsPdf("makeSalesProductForCustomerPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };

        }












        /////Subscription Section/////
        public ActionResult MaintainanceReport()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير تحويلات المخزن لصيانة بالتكلفة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = new SubscriptionMaintainDateVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult MaintainanceReport(SubscriptionMaintainDateVM PDVM)
        {
            var model = ReportsNew.getSubscriptionTransfares(PDVM);
            return new ViewAsPdf("PrintMaintainanceReport", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }
        public ActionResult SubscriptionInvoiceReport()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مبيعات الاشتراكات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = new SubscriptionMaintainDateVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult SubscriptionInvoiceReport(SubscriptionMaintainDateVM PDVM)
        {
            var model = ReportsNew.getSubscriptionInvoices(PDVM);
            return new ViewAsPdf("PrintSubscriptionInvoiceReport", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }


        //Subscription for employee 
        public ActionResult SubscriptionInvoiceReportForEmployee()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مبيعات الاشتراك لموظف معيين");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.SubscriptionByEmployeeForCreation();
            return View(model);
        }
        [HttpPost]
        public ActionResult SubscriptionInvoiceReportForEmployee(SubscriptionMaintainDateVM PDVM)
        {
            var model = ReportsNew.getSubscriptionInvoicesForEmployee(PDVM);
            return new ViewAsPdf("SubscriptionInvoiceReportForEmployeePDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }

        ////////////tax section//////////
        public ActionResult getTaxReportInfo(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير الضريبة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.getTaxInfo(id);
            return new ViewAsPdf("getTaxReportInfo", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        ////HR partiation
        public ActionResult EmployeeDateWorking()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير بيانات الموظف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.GetAllEmployeeDateWork();
            return new ViewAsPdf("EmployeeDateWorking", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        public ActionResult HRNotification()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير تنبيهات موظف");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.HRNotification();
            return new ViewAsPdf("HRNotification", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        /// <summary>
        /// POS Partitiong
        /// </summary>
        /// <returns></returns>
        public ActionResult POSInvoiceGet()
        {
            var model = ReportsNew.getAllPOSInvoiceMasterGet();
            return View(model);
        }
        [HttpPost]
        public ActionResult POSInvoiceGet(POSListVM pOSVM)
        {
            var model = ReportsNew.allPosInvoiceMaster(pOSVM);
            return new ViewAsPdf("POSInvoiceGetPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult ReversePOSInvoiceGet()
        {
            var model = ReportsNew.getAllRevesePOSInvoiceMasterGet();
            return View(model);
        }
        [HttpPost]
        public ActionResult ReversePOSInvoiceGet(ReversePOSListVM pOSVM)
        {
            var model = ReportsNew.allReversePosInvoiceMaster(pOSVM);
            return new ViewAsPdf("ReversePOSInvoiceGetPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        ///// add In general Report
        ///
        public ActionResult ReportOfSubledger(int ? SubId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مركز التكلفة التفصيلي");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.getDataForMAkeReportOfSubledger(SubId);
            return View(model);
        }
        [HttpPost]
     
        public ActionResult ReportOfSubledger(SubledgerVM subVM)
        {
            var model = ReportsNew.makeReportForSubledger(subVM);
            TempData["ReportOfSubledger"] = model;
            TempData.Keep("ReportOfSubledger");

            return View("ReportOfSubledgerList", model);
            // return new ViewAsPdf("SubledgerReport", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }

        public ActionResult ReportOfSubledgerPDF()
        {


            SubledgerVM model = (SubledgerVM)TempData["ReportOfSubledger"];
            TempData.Keep("ReportOfSubledger");
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
    Url.Action("Header", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

            return new ViewAsPdf(model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;
        }
        public ActionResult ReportOfSubledgerExcel()
        {
            SubledgerVM model = (SubledgerVM)TempData["ReportOfSubledger"];
            TempData.Keep("ReportOfSubledger");
            var excel = new ExcelSheetTemplate();
            Response.ClearContent();
            Response.BinaryWrite(excel.GetListDetailSubleger(model));
            Response.AddHeader("content-disposition", "attachment; filename=/SubledgerDetails.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return View();
            // return new ViewAsPdf("TrialBalanceNewPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }
        public ActionResult ReportOfProjectProfit()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير ارباح وخسائر مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.getDataForReportForProjectProAndLoss();
            return View(model);
        }
        [HttpPost]
        public ActionResult ReportOfProjectProfit(ProjectLossAndProfitVM subVM)
        {
            var model = ReportsNew.makeReportForProjectLoss(subVM);
            return new ViewAsPdf("ReportOfProjectProfitPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }
        public ActionResult ReportOfExpencesProject()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مصروفات مشروع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.GetDataForgetAllDataForProject();
            return View(model);
        }
        [HttpPost]
        public ActionResult ReportOfExpencesProject(ProjectExpenceVM subVM)
        {
            var model = ReportsNew.makeReportOfExpencesProject(subVM);
            return new ViewAsPdf("ReportOfExpencesProjectPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }
        public ActionResult MakeSearchForAllReport()
        {

            List<dynamic> result = new List<dynamic>();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportOfCatchReciptProject()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير حساب المشاريع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.GetDataForgetAllDataForProjectReceipt();
            return View(model);
        }
        [HttpPost]
        public ActionResult ReportOfCatchReciptProject(ProjectDetailsVM subVM)
        {
            var model = ReportsNew.makeReportOfCatchReceiptProject(subVM);
            return new ViewAsPdf("ReportOfCatchReciptProjectPdf", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 50, Right = 15, Top = 15 } };
        }
        public ActionResult printbarcode()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة باركود و ضبطه");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.PrintBarcodevm();
            return View(model);
        }
        [HttpPost]
        public ActionResult printbarcode(BarCodePrint vm)
        {
            var model = ReportsNew.PrintBarcode(vm);
            return RedirectToAction("Index","Product");
        }
        public ActionResult printbarcodeTemp(int id,int ?storeId)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة الباركود الحراري");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.PrintBarcodeTemp(id, storeId);
            return View(model);
        }
        public ActionResult printPOSTest(BarCodePrint vm)
        {
            var model = ReportsNew.PrintBarcodeTest(vm);
            return new ViewAsPdf("printPOSTest", model) { PageSize = Rotativa.Options.Size.Comm10E, CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 }

            };
        }
        public ActionResult PrintChartOfAccount()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "طباعة الشجرة المحاسبية");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.PrintChartOfAccount();
            return new ViewAsPdf("PrintChartOfAccount", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult printTaxSheet()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير الاقرار الضريبي");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.getTaxOfReportDateTax();
            return View(model);
        }
        [HttpPost]
        public ActionResult printTaxSheet(TaxReportVM vm)
        {
            var model = ReportsNew.CalculateTax(vm);
            return new ViewAsPdf("printTaxSheetPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        public ActionResult dailyPOSReport()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير بائع نقطة مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.prepareForReportPOSDaily();
            return View(model);


        }
        public ActionResult CustomersOfEmployee()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير العملاء لموظف معين");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }

            var model = ReportsNew.prepareCustomersOfEmp();
            return View(model);


        }
        [HttpPost]
        public ActionResult CustomersOfEmployee(DailyPOSEmp vm)
        {


            var model = ReportsNew.getCustomersOfEmp(vm);
            return new ViewAsPdf("CustomersOfEmployeePDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };


        }

        [HttpPost]
        public ActionResult dailyPOSReport(DailyPOSEmp vm)
        {
      
          
            var model = ReportsNew.ReportPOSDaily(vm);
            return new ViewAsPdf("dailyPOSReportPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };


        }
        public ActionResult dailyPOSReportForSaller()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مبيعاتي نقطة مبيعات");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            bool hasPOS = MasterPOSInvoice.checkthatEmployeisSalesPerson(user.Id);
            if (hasPOS == false)
            {
                return RedirectToAction("Index","MasterPOSInvoice", new { msg = "ليس لديك نقطة بيع " });
            }
            var model = ReportsNew.prepareForReportPOSDaily();

            return View(model);


        }
        [HttpPost]
        public ActionResult dailyPOSReportForSaller(DailyPOSEmp vm)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            vm.Employee_Id = MasterPOSInvoice.getTheEmployeeByRegisterId(user.Id).Emp_Id;
            var model = ReportsNew.ReportPOSDaily(vm);



            return new ViewAsPdf("dailyPOSReportForSallerPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };


        }



        public ActionResult dailyCashierReport()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير بائع");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.PrepareForReportCachierDaily();
            return View(model);


        }
        [HttpPost]
        public ActionResult dailyCashierReport(DailyPOSEmp vm)
        {


            var model = ReportsNew.ReportCasherDaily(vm);
            return new ViewAsPdf("dailyCashierReportPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };


        }
        public ActionResult dailyCashierReportForSaller()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مبيعاتي");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.prepareForReportPOSDaily();

            return View(model);


        }
        [HttpPost]
        public ActionResult dailyCashierReportForSaller(DailyPOSEmp vm)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            try
            {

                vm.Employee_Id = MasterPOSInvoice.getTheEmployeeByRegisterId(user.Id).Emp_Id;

                var model = ReportsNew.ReportCasherDaily(vm);
                return new ViewAsPdf("dailyCashierReportForSallerPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
            }
            catch {
                vm.Employee_Id = 0;
                return new ViewAsPdf("dailyCashierReportForSallerPDF", vm) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };

            }
        }
        public ActionResult DailyCashierReportForSallerThermalPrinting()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مبيعاتي");

            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.prepareForReportPOSDaily();

            return View(model);


        }

        [HttpPost]
        public ActionResult DailyCashierReportForSallerThermalPrinting(DailyPOSEmp vm)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            try
            {

                vm.Employee_Id = MasterPOSInvoice.getTheEmployeeByRegisterId(user.Id).Emp_Id;
                string StartDate = vm.StartDate;
                string End_Date = vm.End_Date;
                string TimeFrom = vm.TimeFrom;
                string TimeTo = vm.TimeTo;
                int Employee_Id = vm.Employee_Id;  
                return RedirectToAction("DailyCashierReportForSallerThermalPrint", new { StartDate, End_Date, TimeFrom, TimeTo, Employee_Id });

            }
            catch
            {
                vm.Employee_Id = 0;
                return new ViewAsPdf("dailyCashierReportForSallerPDF", vm) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };

            }
        }
        [HttpGet]
        public ActionResult DailyCashierReportForSallerThermalPrint(string StartDate, string End_Date, string TimeFrom, string TimeTo, int Employee_Id)
        {
            DailyPOSEmp vm = new DailyPOSEmp();
            vm.StartDate = StartDate;
            vm.End_Date = End_Date;
            vm.TimeFrom = TimeFrom;
            vm.TimeTo = TimeTo;
            vm.Employee_Id = Employee_Id;
            var model = ReportsNew.ReportCasherDaily(vm);
            return View(model);
        }

       
        public ActionResult getTaxOfSalesAndPurch(int taxid,string from,string to)
        {
            var tax = ReportsNew.getsaleTax(taxid, from, to);
            //return View(tax);

            return new ViewAsPdf(tax) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }
        public ActionResult getListofLedger(string code)
        {

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = true;
            //if (code.StartsWith("SUP"))
            //{
            //     employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مورد");

            //}
            //if (code.StartsWith("CUS"))
            //{
            //    employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير عميل");

            //}
            //if (code.StartsWith("EMP"))
            //{
            //    employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير موظف");

            //}
            if (code == null)
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "الاستاذ العام");
            }
            else if (code.StartsWith("010120002"))
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير بنك");

            }
            else if (code.StartsWith("010120001"))
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير صندوق");

            }
            

            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.getListForLedgerget(code);
            return View(model);
        }
        [HttpPost]
        public ActionResult getListofLedger(MRGL vm)
        {
            var model = ReportsNew.makeReportForParent(vm);

         

            TempData["LedgerData"] = model;
            TempData.Keep("LedgerData");

            return View("getListofLedgerList", model);
            
        }
        public ActionResult getListofLedgerPdf()
        {

            MRGL model = (MRGL)TempData["LedgerData"];
            TempData.Keep("LedgerData");
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
       Url.Action("HeaderReport", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

            return new ViewAsPdf(model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;
        }
        public ActionResult getListofLedgerNewExcel()
        {
            MRGL model = (MRGL)TempData["LedgerData"];
            TempData.Keep("LedgerData");
            var excel = new ExcelSheetTemplate();
            Response.ClearContent();
            Response.BinaryWrite(excel.GetListLedger(model));
            Response.AddHeader("content-disposition", "attachment; filename=/Ledger.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return View();
            // return new ViewAsPdf("TrialBalanceNewPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }
        public ActionResult getListofLedgerChild(string code)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var employeeAction = true;
            if (code.StartsWith("SUP"))
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير مورد");

            }
            if (code.StartsWith("CUS"))
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير عميل");

            }
            if (code.StartsWith("EMP"))
            {
                employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير موظف");

            }

            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.getListForLedgerForChildget(code);
            return View(model);
        }
        [HttpPost]
        public ActionResult getListofLedgerChildList(ChildAccountGL vm)
        {
            var model = ReportsNew.getListForChildAccountPOSt(vm);
          
            TempData["LedgerChildData"] = model;
            TempData.Keep("LedgerChildData");

            return View("getListofLedgerChildList", model);
        }
        public ActionResult getListofLedgerChildLExcel()
        {
            ChildAccountGL model = (ChildAccountGL)TempData["LedgerChildData"];
            TempData.Keep("LedgerChildData");
            var excel = new ExcelSheetTemplate();
            Response.ClearContent();
            Response.BinaryWrite(excel.GetListOfLedgerExcel(model));
            Response.AddHeader("content-disposition", "attachment; filename=ChildList.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return View();
            // return new ViewAsPdf("TrialBalanceNewPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }

        public ActionResult getListofLedgerChildPDF()
        {

            ChildAccountGL model = (ChildAccountGL)TempData["LedgerChildData"];
            TempData.Keep("LedgerChildData");
            return new ViewAsPdf("getListofLedgerChildPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }

        public ActionResult getListofLedgerOfAsset()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
          
            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "تقرير الاصول التفصيلي");

            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = ReportsNew.getListForLedgerForAsset();
            return View(model);
        }
        [HttpPost]
        public ActionResult getListofLedgerOfAsset(ChildAccountGL vm)
        {
            var model = ReportsNew.getListForLedgerForAssetPOSt(vm);
            return new ViewAsPdf("getListofLedgerOfAssetPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult TrialBalanceNew()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employeeAction = Employee.getAllFunctionForEmployee(user.Id, "ميزان المراجعة");



            if (employeeAction == false)
            {
                return RedirectToAction("NotAuthorize", "SystemRole");
            }
            var model = new TrialBalanceNew(); 
            return View(model);
        }
        [HttpPost]
        public ActionResult TrialBalanceNew(TrialBalanceNew vm)
        {
            var model = ReportsNew.NewTrialBalance(vm);
           
            TempData["EmployeeData"] = model;
            TempData.Keep("EmployeeData");
          
            return  View("TrialBalanceNewMain", model);
        }


        public ActionResult TrialBalanceNewPdf()
        {
    
            TrialBalanceNew model = (TrialBalanceNew)TempData["EmployeeData"];
            TempData.Keep("EmployeeData");
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
Url.Action("HeaderReport", "InvoiceQRExternalUseres", new { Branch_Id = 0 }, protocol));

            return new ViewAsPdf(model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;
        }
        public ActionResult TrialBalanceNewExcel()
        {
            TrialBalanceNew model = (TrialBalanceNew)TempData["EmployeeData"];
            TempData.Keep("EmployeeData");
            var excel = new ExcelSheetTemplate();
            Response.ClearContent();
            Response.BinaryWrite(excel.TrialBalance(model));
            Response.AddHeader("content-disposition", "attachment; filename=Trial.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return View();
           // return new ViewAsPdf("TrialBalanceNewPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 5, Bottom = 15, Right = 5, Top = 15 } };
        }
        public ActionResult StoreCardAll()
        {
            var model = ReportsNew.getMovementVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult StoreCardAll(StoreMovmentMaster store)
        {
            var model = ReportsNew.getMovementVM(store);
            return new ViewAsPdf("StoreCardAllPDF", model) { CustomSwitches = footer, MinimumFontSize = 15, PageMargins = { Left = 15, Bottom = 15, Right = 15, Top = 15 } };
        }
        public ActionResult UpaidInvoices(string code)
        {
            var model = ReportsNew.GetAllUnPaidInvoice(code);

            TempData["UpaidInvoicesData"] = model;
            TempData.Keep("UpaidInvoicesData");

            return View("UpaidInvoices", model);
        }
        public ActionResult UpaidInvoicesPDF()
        {

            UnpaidInvoiceMasterVM model = (UnpaidInvoiceMasterVM)TempData["UpaidInvoicesData"];
            TempData.Keep("UpaidInvoicesData");
            string customSwitchesheader = string.Format(" --print-media-type --page-offset 2 --header-center [page] --allow {0} --header-html {0} --header-spacing 5 ",
         Url.Action("HeaderReport", "InvoiceQRExternalUseres", new { Branch_Id = model.customer.Branch_Id }, protocol));

            return new ViewAsPdf(model)
            {

                //CustomSwitches = customSwitchesfooter + customSwitchesheader
                CustomSwitches = customSwitchesheader
            }
            ;
        }
    }
}
