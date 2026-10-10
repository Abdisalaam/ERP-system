using Accountingsystem.ViewModel;
using Accountingsystem.ViewModel.PrintVM;
using Accountingsystem.ViewModel.ReportsNewVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.ViewModel.ReportsVM.SubscriptionReportVM;
using System.Data.Entity;
using Accountingsystem.Utilites;

namespace Accountingsystem.Models
{
    public class ReportsNew
    {

        public static List<FixedAsset> GetAllAsset()
        {
            var db = new AccountingContainer();
            var Asset = db.FixedAssets.ToList();
            return Asset;

        }


        /// <summary>
        /// purchase report
        /// </summary>
        /// <param name="detasofreport"></param>
        /// <returns></returns>
        public static PurchaseDatesVM allPurchasePrint(PurchaseDatesVM detasofreport)
        {
            var db = new AccountingContainer();
            detasofreport.List = new List<PurchaseReportVM>();
            if (detasofreport.from == null)
            {
                detasofreport.from = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (detasofreport.to == null)
            {
                detasofreport.to = Convert.ToString(new DateTime(2200, 12, 30));

            }
            var fromdate = Convert.ToDateTime(detasofreport.from);
            var todate = Convert.ToDateTime(detasofreport.to).AddHours(23).AddMinutes(59).AddSeconds(59);
            detasofreport.company = db.Companies.FirstOrDefault();
            List<PurInvoiceMaster> allpurchases = new List<PurInvoiceMaster>();
            if (detasofreport.from == null || detasofreport.to == null)
            {
                allpurchases = db.PurInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Store_ID > 0).ToList();
            }
            else
            {
                allpurchases = db.PurInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate && e.Store_ID > 0).ToList();
            }

            List<PurchaseReportVM> purchaselist = new List<PurchaseReportVM>();
            var counters = 1;
            decimal totalamount = 0;
            decimal totalrecall = 0;
            decimal totalofbefor = 0;
            if (allpurchases.Count > 0)
            {
                foreach (var master in allpurchases)
                {
                    PurchaseReportVM PRVM = new PurchaseReportVM();

                    PRVM.Counter = counters;
                    PRVM.InvoiceCode = master.Serial_Number;
                    PRVM.InvoiceDate = master.Invoice_Date;
                    PRVM.invoiceDeadLine = Convert.ToDateTime(master.Due_Date).Day + "-" + Convert.ToDateTime(master.Due_Date).Month + "-" + Convert.ToDateTime(master.Due_Date).Year;
                    if (master.Master_Discount > 0)
                    {
                        decimal discount = master.Total_Amount * (master.Master_Discount / 100);
                        PRVM.invoiceTotal = master.Total_Amount - discount;
                    }
                    else
                    {
                        PRVM.invoiceTotal = master.Total_Amount;
                    }



                    var thisrecall = db.RecalPurchInvoiceMasters.Where(e => e.PurInvoiceMasterMaster_Id == master.Master_Id && e.PI_State == TransactionState.Post).ToList();
                    if (thisrecall.Count > 0)
                    {
                        foreach (var rcall in thisrecall)
                        {
                            PRVM.RecallInvoicetotal = PRVM.RecallInvoicetotal + rcall.Inv_TotalAmount;
                        }
                    }
                    else
                    {
                        PRVM.RecallInvoicetotal = 0;
                    }
                    PRVM.totaltax = master.Total_Tax;
                    PRVM.TotalOFInvoicesBefor = PRVM.invoiceTotal - PRVM.RecallInvoicetotal;
                    totalamount = totalamount + PRVM.invoiceTotal;
                    totalrecall = totalrecall + PRVM.RecallInvoicetotal;
                    totalofbefor = totalofbefor + PRVM.TotalOFInvoicesBefor;
                    try
                    {
                        var thissupp = db.Suppliers.Find(master.Supplier_Id);
                        PRVM.SupplierCode = thissupp.Supp_Code;
                        PRVM.supplierName = thissupp.Supp_Name;
                        PRVM.SupplierTaxNumber = thissupp.Tax_Number;
                    }
                    catch
                    {
                        PRVM.SupplierCode = null;
                        PRVM.supplierName = null;
                        PRVM.SupplierTaxNumber = null;
                    }
                    PRVM.invoiceTotal = master.Total_Tax + PRVM.invoiceTotal;

                    counters++;
                    purchaselist.Add(PRVM);
                }
                detasofreport.List = purchaselist;
                return detasofreport;
            }
            else
            {
                PurchaseReportVM PRVM = new PurchaseReportVM();
                PRVM.company = db.Companies.FirstOrDefault();
                purchaselist.Add(PRVM);
                detasofreport.List = purchaselist;
                return detasofreport;

            }

        }

        /// <summary>
        /// Report for all purchase by branchId, productId, storeId, and categoryId
        /// </summary>
        /// <returns></returns>
        public static ClassifyPurchaseInvoiceVM purhcaseforgettingreport()
        {
            var db = new AccountingContainer();
            var thisclassify = new ClassifyPurchaseInvoiceVM();
            thisclassify.branchList = new SelectList(db.Branches.ToList(), "Branch_Id", "Branch_Name");
            thisclassify.ProductClassList = new SelectList(db.Categories.ToList(), "Cat_ID", "Cat_Descrip");
            thisclassify.ProductList = new SelectList(db.Products.ToList(), "Prod_ID", "Prod_Name");
            thisclassify.storeList = new SelectList(db.Stores.ToList(), "Store_ID", "Store_Name");
            return thisclassify;
        }
        /// <summary>
        /// The post for report for all purchase by branchId, productId, storeId, and categoryId
        /// </summary>
        /// <param name="detasofreport"></param>
        /// <returns></returns>
        public static ClassifyPurchaseInvoiceVM CalssifiedPurchasePrint(ClassifyPurchaseInvoiceVM detasofreport)
        {
            var db = new AccountingContainer();
            detasofreport.List = new List<PurchaseReportVM>();
            detasofreport.company = db.Companies.FirstOrDefault();
            if (detasofreport.from == null)
            {
                detasofreport.from = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (detasofreport.to == null)
            {
                detasofreport.to = Convert.ToString(new DateTime(2200, 12, 30));

            }
            var fromdate = Convert.ToDateTime(detasofreport.from);
            var todate = Convert.ToDateTime(detasofreport.to).AddHours(23).AddMinutes(59).AddSeconds(59);

            List<PurInvoiceMaster> allpurchases = new List<PurInvoiceMaster>();
            if (detasofreport.from == null || detasofreport.to == null)
            {

                if (detasofreport.storeID != null && detasofreport.storeID > 0)
                {
                    allpurchases = db.PurInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Store_ID == detasofreport.storeID && e.Store_ID > 0).ToList();
                }
                else if (detasofreport.branchid != null && detasofreport.branchid > 0)
                {
                    allpurchases = db.PurInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Branch_Id == detasofreport.branchid && e.Store_ID > 0).ToList();
                }
                else if (detasofreport.ProductID != null && detasofreport.ProductID > 0)
                {

                    var allpurchasesmaster = db.PurInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Store_ID > 0).ToList();
                    foreach (var detail in allpurchasesmaster)
                    {
                        foreach (var Mdetail in detail.InvoiceDetails)
                        {
                            if (Mdetail.Product_ID == detasofreport.ProductID)
                            {
                                allpurchases.Add(detail);
                                break;
                            }
                            else
                            {

                            }
                        }
                    }
                }
                else if (detasofreport.productClassify != null && detasofreport.productClassify > 0)
                {

                    var thiscategory = db.Categories.Find(detasofreport.productClassify);
                    var thisitems = db.Products.Where(e => e.Category_ID == thiscategory.Cat_ID).ToList();
                    var thismaster = db.PurInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Store_ID > 0).ToList();
                    foreach (var detail in thismaster)
                    {

                        foreach (var Mdetails in detail.InvoiceDetails)
                        {
                            foreach (var pro in thisitems)
                            {
                                if (Mdetails.Product_ID == pro.Prod_ID)
                                {
                                    allpurchases.Add(detail);
                                    break;
                                }
                                else
                                {

                                }
                            }
                        }
                    }

                }
                else
                {
                    allpurchases = db.PurInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Store_ID > 0).ToList();
                }

            }
            else
            {
                if (detasofreport.storeID != null && detasofreport.storeID > 0)
                {
                    allpurchases = db.PurInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Store_ID == detasofreport.storeID && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate && e.Store_ID > 0).ToList();
                }
                else if (detasofreport.branchid != null && detasofreport.branchid > 0)
                {
                    allpurchases = db.PurInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Branch_Id == detasofreport.branchid && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate && e.Store_ID > 0).ToList();
                }
                else if (detasofreport.ProductID != null && detasofreport.ProductID > 0)
                {

                    var allpurchasesmaster = db.PurInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate && e.Store_ID > 0).ToList();
                    foreach (var detail in allpurchasesmaster)
                    {
                        foreach (var Mdetail in detail.InvoiceDetails)
                        {
                            if (Mdetail.Product_ID == detasofreport.ProductID)
                            {
                                allpurchases.Add(detail);
                                break;
                            }
                            else
                            {

                            }
                        }
                    }
                }
                else if (detasofreport.productClassify != null && detasofreport.productClassify > 0)
                {

                    var thiscategory = db.Categories.Find(detasofreport.productClassify);
                    var thisitems = db.Products.Where(e => e.Category_ID == thiscategory.Cat_ID).ToList();
                    var thismaster = db.PurInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate).ToList();
                    foreach (var detail in thismaster)
                    {

                        foreach (var Mdetails in detail.InvoiceDetails)
                        {
                            foreach (var pro in thisitems)
                            {
                                if (Mdetails.Product_ID == pro.Prod_ID)
                                {
                                    allpurchases.Add(detail);
                                    break;
                                }
                                else
                                {

                                }
                            }
                        }
                    }

                }
                else
                {
                    allpurchases = db.PurInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate && e.Store_ID > 0).ToList();
                }
            }

            List<PurchaseReportVM> purchaselist = new List<PurchaseReportVM>();
            var counters = 1;
            decimal totalamount = 0;
            decimal totalrecall = 0;
            decimal totalofbefor = 0;
            if (allpurchases.Count > 0)
            {
                foreach (var master in allpurchases)
                {
                    PurchaseReportVM PRVM = new PurchaseReportVM();

                    PRVM.Counter = counters;
                    PRVM.InvoiceCode = master.Serial_Number;
                    PRVM.InvoiceDate = master.Invoice_Date;
                    PRVM.invoiceDeadLine = Convert.ToDateTime(master.Due_Date).Day + "-" + Convert.ToDateTime(master.Due_Date).Month + "-" + Convert.ToDateTime(master.Due_Date).Year;
                    if (master.Master_Discount > 0)
                    {
                        decimal discount = master.Total_Amount * (master.Master_Discount / 100);
                        PRVM.invoiceTotal = master.Total_Amount - discount;
                    }
                    else
                    {
                        PRVM.invoiceTotal = master.Total_Amount;
                    }



                    var thisrecall = db.RecalPurchInvoiceMasters.Where(e => e.PurInvoiceMasterMaster_Id == master.Master_Id && e.PI_State == TransactionState.Post).ToList();
                    if (thisrecall.Count > 0)
                    {
                        foreach (var rcall in thisrecall)
                        {
                            PRVM.RecallInvoicetotal = PRVM.RecallInvoicetotal + rcall.Inv_TotalCost;
                        }
                    }
                    else
                    {
                        PRVM.RecallInvoicetotal = 0;
                    }
                    if (master.Total_Tax > 0)
                    {
                        PRVM.totaltax = master.Total_Tax;
                    }
                    else
                    {
                        PRVM.totaltax = 0;
                    }
                    PRVM.TotalOFInvoicesBefor = PRVM.invoiceTotal - PRVM.RecallInvoicetotal;
                    if (master.TotaL_After_Fixed != null)
                    {
                        PRVM.totalafterfixed = (decimal)master.TotaL_After_Fixed;

                    }
                    else
                    {
                        PRVM.totalafterfixed = (decimal)master.Total_Cost;
                    }

                    totalamount = totalamount + PRVM.invoiceTotal;
                    totalrecall = totalrecall + PRVM.RecallInvoicetotal;
                    totalofbefor = totalofbefor + PRVM.TotalOFInvoicesBefor;
                    var thissupp = db.Suppliers.Find(master.Supplier_Id);
                    if (thissupp != null)
                    {
                        PRVM.SupplierCode = thissupp.Supp_Code;
                        PRVM.supplierName = thissupp.Supp_Name;
                    }

                    counters++;
                    purchaselist.Add(PRVM);
                }
                detasofreport.List = purchaselist;
                return detasofreport;
            }
            else
            {
                PurchaseReportVM PRVM = new PurchaseReportVM();

                detasofreport.List = purchaselist;
                return detasofreport;
            }

        }

        /// <summary>
        /// Report for all reverse of purchase
        /// </summary>
        /// <param name="detasofreport"></param>
        /// <returns></returns>
        public static PurchaseDatesVM allRecallPrint(PurchaseDatesVM detasofreport)
        {
            var db = new AccountingContainer();
            detasofreport.RecallList = new List<RecallPurchaseReportVM>();
            if (detasofreport.from == null)
            {
                detasofreport.from = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (detasofreport.to == null)
            {
                detasofreport.to = Convert.ToString(new DateTime(2200, 12, 30));

            }
            detasofreport.company = db.Companies.FirstOrDefault();
            var fromdate = Convert.ToDateTime(detasofreport.from);
            var todate = Convert.ToDateTime(detasofreport.to).AddHours(23).AddMinutes(59).AddSeconds(59);

            List<RecalPurchInvoiceMaster> allRecall = new List<RecalPurchInvoiceMaster>();
            if (detasofreport.from == null || detasofreport.to == null)
            {
                allRecall = db.RecalPurchInvoiceMasters.Where(e => e.PI_State == TransactionState.Post).ToList();
            }
            else
            {
                allRecall = db.RecalPurchInvoiceMasters.Where(e => e.PI_State == TransactionState.Post && e.Register_Date >= fromdate && e.Register_Date <= todate).ToList();
            }
            List<RecallPurchaseReportVM> Recalllist = new List<RecallPurchaseReportVM>();
            var counters = 1;

            if (allRecall.Count > 0)
            {
                foreach (var master in allRecall)
                {
                    RecallPurchaseReportVM PRVM = new RecallPurchaseReportVM();

                    PRVM.Counter = counters;

                    PRVM.RecallPuchaseCode = master.Recall_MPurch_Serial;
                    PRVM.PurchaseCode = db.PurInvoiceMasters.Find(master.PurInvoiceMasterMaster_Id).Serial_Number;
                    PRVM.RecallDate = master.Register_Date;
                    PRVM.RecallDeadLine = Convert.ToDateTime(master.Due_Date).Day + "-" + Convert.ToDateTime(master.Due_Date).Month + "-" + Convert.ToDateTime(master.Due_Date).Year;
                    //if (master.Master_Discount > 0)
                    //{
                    //    decimal discount = master.Inv_TotalAmount * (master.Master_Discount / 100);
                    //    PRVM.TotalOFRecallamount = master.Inv_TotalAmount - discount;
                    //}
                    //else
                    //{
                    PRVM.TotalOFRecallamount = master.Inv_TotalAmount;
                    //}
                    if (master.Inv_TotalTax > 0)
                    {
                        PRVM.RecalltotalTax = master.Inv_TotalTax;
                    }
                    else
                    {
                        PRVM.RecalltotalTax = 0;
                    }
                    PRVM.RecalltotalafterTax = master.Inv_TotalCost;




                    var thissupp = db.Suppliers.Find(master.SupplierSuppId);
                    PRVM.SupplierCode = thissupp.Supp_Code;
                    PRVM.supplierName = thissupp.Supp_Name;
                    counters++;
                    Recalllist.Add(PRVM);
                }
                detasofreport.RecallList = Recalllist;
                return detasofreport;
            }
            else
            {
                RecallPurchaseReportVM PRVM = new RecallPurchaseReportVM();
                PRVM.company = db.Companies.FirstOrDefault();
                Recalllist.Add(PRVM);
                detasofreport.RecallList = Recalllist;
                return detasofreport;
            }
        }
        /// <summary>
        /// Print all suppliers 
        /// </summary>
        /// <returns></returns>
        public static List<SupplierInfosVM> AllSuppliersPrint()
        {
            var db = new AccountingContainer();
            var allsupplier = db.Suppliers.ToList();
            List<SupplierInfosVM> supplist = new List<SupplierInfosVM>();
            foreach (var sup in allsupplier)
            {
                SupplierInfosVM SUPVM = new SupplierInfosVM();
                var thisaccount = db.Accounts.Where(e => e.Account_Code == sup.Supp_Code).FirstOrDefault();
                if (thisaccount.Account_Balance != null && thisaccount.Account_Balance > 0)
                {
                    SUPVM.Balance = @Math.Round((decimal)thisaccount.Account_Balance, 2, MidpointRounding.AwayFromZero).ToString();


                }
                else if (thisaccount.Account_Balance != null && thisaccount.Account_Balance < 0)
                {
                    decimal supafterneg = @Math.Round((decimal)thisaccount.Account_Balance, 2, MidpointRounding.AwayFromZero) * -1;
                    SUPVM.Balance = "(" + supafterneg.ToString() + ")";
                }
                else
                {
                    SUPVM.Balance = "0";
                }
                SUPVM.sup = sup;
              
                SUPVM.company = db.Companies.FirstOrDefault();
                supplist.Add(SUPVM);
            }
            return supplist;
        }
        /// <summary>
        /// Print all suppliers report analisys with days
        /// </summary>
        /// <param name="SGVM"></param>
        /// <returns></returns>
        public static List<SupplierReportWithDaysVM> getSuupliersWithDays(SupplierGettingDateVM SGVM)
        {
            var db = new AccountingContainer();
            var allsuppliers = db.Suppliers.ToList();
            DateTime thisdate = Convert.ToDateTime(SGVM.startDate);
            DateTime befor30 = Convert.ToDateTime(SGVM.startDate).AddMonths(-1);
            DateTime after30 = Convert.ToDateTime(SGVM.startDate).AddDays(29);
            DateTime after301 = after30.AddDays(1);
            DateTime after60 = after301.AddDays(29);
            DateTime after601 = after60.AddDays(1);
            DateTime after90 = after601.AddDays(29);
            DateTime after120 = after90.AddDays(1);

            List<SupplierReportWithDaysVM> SupplierList = new List<SupplierReportWithDaysVM>();
            foreach (var sup in allsuppliers)
            {
                decimal befor30balance = 0;
                decimal after30balance = 0;
                decimal after60balance = 0;
                decimal after90balance = 0;
                decimal after120balance = 0;
                SupplierReportWithDaysVM SRD = new SupplierReportWithDaysVM();
                var accountSupplier = db.Accounts.Where(a => a.Account_Code == sup.Supp_Code).FirstOrDefault();
                SRD.company = db.Companies.FirstOrDefault();
                SRD.SupplierName = sup.Supp_Name;
                SRD.SupplierCode = sup.Supp_Code;
                if (accountSupplier.Account_Balance != null && accountSupplier.Account_Balance > 0)
                {
                    SRD.Balance = @Math.Round((decimal)accountSupplier.Account_Balance, 2, MidpointRounding.AwayFromZero).ToString();


                }
                else if (accountSupplier.Account_Balance != null && accountSupplier.Account_Balance < 0)
                {
                    decimal supafterneg = @Math.Round((decimal)accountSupplier.Account_Balance, 2, MidpointRounding.AwayFromZero) * -1;
                    SRD.Balance = "(" + supafterneg.ToString() + ")";
                }
                else
                {
                    SRD.Balance = "0";
                }
                //.AddHours(23).AddMinutes(59)
                DateTime firstdate = thisdate.AddHours(23).AddMinutes(59);
                DateTime seconddate = after30;
                DateTime thirddate = after60;
                DateTime fourthdate = after90;
                DateTime Fifthdate = after120;
                var thisdetailsSup = db.GLDetails.Where(e => e.Account_Child_ID == accountSupplier.Accounts_Id).ToList();
                List<GLMaster> glmasterlist = new List<GLMaster>();
                List<GLMaster> glmasterlistInvoice = new List<GLMaster>();
                foreach (var det in thisdetailsSup)
                {
                    GLMaster gl = new GLMaster();
                    gl = db.GLMasters.Find(det.GLMasterGLMaster_Id);
                    if (gl.GLCode.StartsWith("PUR-"))
                    {
                        glmasterlistInvoice.Add(gl);
                    }
                    else
                    {
                        glmasterlist.Add(gl);
                    }

                }
                List<GLMaster> glmasterlistInvoices = glmasterlistInvoice.Distinct().ToList();
                if (glmasterlistInvoices.Count > 0)
                {
                    foreach (var purchdetail in glmasterlistInvoices)
                    {
                        var thispurchInv = db.PurInvoiceMasters.Where(e => e.Serial_Number == purchdetail.GLCode).ToList().FirstOrDefault();
                        DateTime thisDuedate = Convert.ToDateTime(thispurchInv.Due_Date);
                        if (thisDuedate > thisdate)
                        {
                            var after30ofthis = thisdate.AddDays(29);
                            var after30by1 = after30ofthis.AddDays(1);
                            var after60ofthis = after30by1.AddDays(29);
                            var after60by1 = after60ofthis.AddDays(1);
                            var after90ofthis = after60by1.AddDays(29);
                            var after90by1 = after90ofthis.AddDays(1);
                            if (thisDuedate >= thisdate && thisDuedate <= after30ofthis)
                            {

                                var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == purchdetail.GLMaster_Id && e.Account_Child_ID == accountSupplier.Accounts_Id).ToList();

                                if (thisdetailBefor.Count != 0)
                                {
                                    foreach (var GLdetail in thisdetailBefor)
                                    {
                                        if (GLdetail.GLDetail_Debit > 0)
                                        {
                                            after30balance = after30balance + GLdetail.GLDetail_Debit;
                                        }
                                        else
                                        {
                                            after30balance = after30balance - GLdetail.GLDetail_Credit;
                                        }
                                    }
                                }
                                else
                                {

                                }

                                if (after30balance > 0)
                                {
                                    SRD.afterthiry = @Math.Round((decimal)after30balance, 2, MidpointRounding.AwayFromZero).ToString();


                                }
                                else if (after30balance < 0)
                                {
                                    decimal supafterneg = @Math.Round((decimal)after30balance, 2, MidpointRounding.AwayFromZero) * -1;
                                    SRD.afterthiry = "(" + supafterneg.ToString() + ")";
                                }
                                else
                                {
                                    SRD.afterthiry = "0";
                                }
                            }
                            else if (thisDuedate > after30ofthis && thisDuedate <= after60ofthis)
                            {
                                var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == purchdetail.GLMaster_Id && e.Account_Child_ID == accountSupplier.Accounts_Id).ToList();

                                if (thisdetailBefor.Count != 0)
                                {
                                    foreach (var GLdetail in thisdetailBefor)
                                    {
                                        if (GLdetail.GLDetail_Debit > 0)
                                        {
                                            after60balance = after60balance + GLdetail.GLDetail_Debit;
                                        }
                                        else
                                        {
                                            after60balance = after60balance - GLdetail.GLDetail_Credit;
                                        }
                                    }
                                }
                                else
                                {

                                }

                                if (after60balance > 0)
                                {
                                    SRD.aftersixty = @Math.Round((decimal)after60balance, 2, MidpointRounding.AwayFromZero).ToString();


                                }
                                else if (after60balance < 0)
                                {
                                    decimal supafterneg = @Math.Round((decimal)after60balance, 2, MidpointRounding.AwayFromZero) * -1;
                                    SRD.aftersixty = "(" + supafterneg.ToString() + ")";
                                }
                                else
                                {
                                    SRD.aftersixty = "0";
                                }
                            }
                            else if (thisDuedate > after60ofthis && thisDuedate <= after90ofthis)
                            {
                                var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == purchdetail.GLMaster_Id && e.Account_Child_ID == accountSupplier.Accounts_Id).ToList();

                                if (thisdetailBefor.Count != 0)
                                {
                                    foreach (var GLdetail in thisdetailBefor)
                                    {
                                        if (GLdetail.GLDetail_Debit > 0)
                                        {
                                            after90balance = after90balance + GLdetail.GLDetail_Debit;
                                        }
                                        else
                                        {
                                            after90balance = after90balance - GLdetail.GLDetail_Credit;
                                        }
                                    }
                                }
                                else
                                {

                                }

                                if (after90balance > 0)
                                {
                                    SRD.afterninty = @Math.Round((decimal)after90balance, 2, MidpointRounding.AwayFromZero).ToString();


                                }
                                else if (after90balance < 0)
                                {
                                    decimal supafterneg = @Math.Round((decimal)after90balance, 2, MidpointRounding.AwayFromZero) * -1;
                                    SRD.afterninty = "(" + supafterneg.ToString() + ")";
                                }
                                else
                                {
                                    SRD.afterninty = "0";
                                }
                            }
                            else if (thisDuedate > after90ofthis)
                            {
                                var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == purchdetail.GLMaster_Id && e.Account_Child_ID == accountSupplier.Accounts_Id).ToList();

                                if (thisdetailBefor.Count != 0)
                                {
                                    foreach (var GLdetail in thisdetailBefor)
                                    {
                                        if (GLdetail.GLDetail_Debit > 0)
                                        {
                                            after120balance = after120balance + GLdetail.GLDetail_Debit;
                                        }
                                        else
                                        {
                                            after120balance = after120balance - GLdetail.GLDetail_Credit;
                                        }
                                    }
                                }
                                else
                                {

                                }

                                if (after120balance > 0)
                                {
                                    SRD.afteronehunderd = @Math.Round((decimal)after120balance, 2, MidpointRounding.AwayFromZero).ToString();


                                }
                                else if (after120balance < 0)
                                {
                                    decimal supafterneg = @Math.Round((decimal)after120balance, 2, MidpointRounding.AwayFromZero) * -1;
                                    SRD.afteronehunderd = "(" + supafterneg.ToString() + ")";
                                }
                                else
                                {
                                    SRD.afteronehunderd = "0";
                                }
                            }

                        }
                        else
                        {
                            var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == purchdetail.GLMaster_Id && e.Account_Child_ID == accountSupplier.Accounts_Id).ToList();

                            if (thisdetailBefor.Count != 0)
                            {
                                foreach (var GLdetail in thisdetailBefor)
                                {
                                    if (GLdetail.GLDetail_Debit > 0)
                                    {
                                        befor30balance = befor30balance + GLdetail.GLDetail_Debit;
                                    }
                                    else
                                    {
                                        befor30balance = befor30balance - GLdetail.GLDetail_Credit;
                                    }
                                }
                            }
                            else
                            {

                            }

                            if (befor30balance > 0)
                            {
                                SRD.beforthiry = @Math.Round((decimal)befor30balance, 2, MidpointRounding.AwayFromZero).ToString();


                            }
                            else if (befor30balance < 0)
                            {
                                decimal supafterneg = @Math.Round((decimal)befor30balance, 2, MidpointRounding.AwayFromZero) * -1;
                                SRD.beforthiry = "(" + supafterneg.ToString() + ")";
                            }
                            else
                            {
                                SRD.beforthiry = "0";
                            }
                        }

                    }
                }
                //////////////////
                List<GLMaster> glmasterlists = glmasterlist.Distinct().ToList();
                var allGLMbefor30 = glmasterlists.Where(e => e.GL_Date < thisdate).ToList();
                if (allGLMbefor30.Count > 0)
                {
                    foreach (var detail in allGLMbefor30)
                    {
                        var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == detail.GLMaster_Id && e.Account_Child_ID == accountSupplier.Accounts_Id).ToList();

                        if (thisdetailBefor.Count != 0)
                        {
                            foreach (var GLdetail in thisdetailBefor)
                            {
                                if (GLdetail.GLDetail_Debit > 0)
                                {
                                    befor30balance = befor30balance + GLdetail.GLDetail_Debit;
                                }
                                else
                                {
                                    befor30balance = befor30balance - GLdetail.GLDetail_Credit;
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    if (befor30balance > 0)
                    {
                        SRD.beforthiry = @Math.Round((decimal)befor30balance, 2, MidpointRounding.AwayFromZero).ToString();


                    }
                    else if (befor30balance < 0)
                    {
                        decimal supafterneg = @Math.Round((decimal)befor30balance, 2, MidpointRounding.AwayFromZero) * -1;
                        SRD.beforthiry = "(" + supafterneg.ToString() + ")";
                    }
                    else
                    {
                        SRD.beforthiry = "0";
                    }
                }

                var allGLMafter30 = glmasterlists.Where(e => e.GL_Date >= thisdate && e.GL_Date <= seconddate).ToList();
                if (allGLMafter30.Count > 0)
                {
                    foreach (var detail in allGLMafter30)
                    {
                        var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == detail.GLMaster_Id && e.Account_Child_ID == accountSupplier.Accounts_Id).ToList();

                        if (thisdetailBefor.Count != 0)
                        {
                            foreach (var GLdetail in thisdetailBefor)
                            {
                                if (GLdetail.GLDetail_Debit > 0)
                                {
                                    after30balance = after30balance + GLdetail.GLDetail_Debit;
                                }
                                else
                                {
                                    after30balance = after30balance - GLdetail.GLDetail_Credit;
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    if (after30balance > 0)
                    {
                        SRD.afterthiry = @Math.Round((decimal)after30balance, 2, MidpointRounding.AwayFromZero).ToString();


                    }
                    else if (after30balance < 0)
                    {
                        decimal supafterneg = @Math.Round((decimal)after30balance, 2, MidpointRounding.AwayFromZero) * -1;
                        SRD.afterthiry = "(" + supafterneg.ToString() + ")";
                    }
                    else
                    {
                        SRD.afterthiry = "0";
                    }
                }

                var allGLMafter60 = glmasterlists.Where(e => e.GL_Date > after30 && e.GL_Date <= thirddate).ToList();
                if (allGLMafter60.Count > 0)
                {
                    foreach (var detail in allGLMafter60)
                    {
                        var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == detail.GLMaster_Id && e.Account_Child_ID == accountSupplier.Accounts_Id).ToList();

                        if (thisdetailBefor.Count != 0)
                        {
                            foreach (var GLdetail in thisdetailBefor)
                            {
                                if (GLdetail.GLDetail_Debit > 0)
                                {
                                    after60balance = after60balance + GLdetail.GLDetail_Debit;
                                }
                                else
                                {
                                    after60balance = after60balance - GLdetail.GLDetail_Credit;
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    if (after60balance > 0)
                    {
                        SRD.aftersixty = @Math.Round((decimal)after60balance, 2, MidpointRounding.AwayFromZero).ToString();


                    }
                    else if (after60balance < 0)
                    {
                        decimal supafterneg = @Math.Round((decimal)after60balance, 2, MidpointRounding.AwayFromZero) * -1;
                        SRD.aftersixty = "(" + supafterneg.ToString() + ")";
                    }
                    else
                    {
                        SRD.aftersixty = "0";
                    }
                }

                var allGLMafter90 = glmasterlists.Where(e => e.GL_Date > after60 && e.GL_Date <= fourthdate).ToList();
                if (allGLMafter90.Count > 0)
                {
                    foreach (var detail in allGLMafter60)
                    {
                        var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == detail.GLMaster_Id && e.Account_Child_ID == accountSupplier.Accounts_Id).ToList();

                        if (thisdetailBefor.Count != 0)
                        {
                            foreach (var GLdetail in thisdetailBefor)
                            {
                                if (GLdetail.GLDetail_Debit > 0)
                                {
                                    after90balance = after90balance + GLdetail.GLDetail_Debit;
                                }
                                else
                                {
                                    after90balance = after90balance - GLdetail.GLDetail_Credit;
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    if (after90balance > 0)
                    {
                        SRD.afterninty = @Math.Round((decimal)after90balance, 2, MidpointRounding.AwayFromZero).ToString();


                    }
                    else if (after90balance < 0)
                    {
                        decimal supafterneg = @Math.Round((decimal)after90balance, 2, MidpointRounding.AwayFromZero) * -1;
                        SRD.afterninty = "(" + supafterneg.ToString() + ")";
                    }
                    else
                    {
                        SRD.afterninty = "0";
                    }
                }

                var allGLMafter120 = glmasterlists.Where(e => e.GL_Date > after90).ToList();
                if (allGLMafter120.Count > 0)
                {
                    foreach (var detail in allGLMafter120)
                    {
                        var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == detail.GLMaster_Id && e.Account_Child_ID == accountSupplier.Accounts_Id).ToList();

                        if (thisdetailBefor.Count != 0)
                        {
                            foreach (var GLdetail in thisdetailBefor)
                            {
                                if (GLdetail.GLDetail_Debit > 0)
                                {
                                    after120balance = after120balance + GLdetail.GLDetail_Debit;
                                }
                                else
                                {
                                    after120balance = after120balance - GLdetail.GLDetail_Credit;
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    if (after120balance > 0)
                    {
                        SRD.afteronehunderd = @Math.Round((decimal)after120balance, 2, MidpointRounding.AwayFromZero).ToString();


                    }
                    else if (after120balance < 0)
                    {
                        decimal supafterneg = @Math.Round((decimal)after120balance, 2, MidpointRounding.AwayFromZero) * -1;
                        SRD.afteronehunderd = "(" + supafterneg.ToString() + ")";
                    }
                    else
                    {
                        SRD.afteronehunderd = "0";
                    }
                }
                if (SRD.Balance == "0")
                {

                }
                else
                {
                    SupplierList.Add(SRD);
                }


            }
            return SupplierList;
        }

        /// <summary>
        /// Get store Report
        /// </summary>
        /// <returns></returns>

        public static StoreChoose getquantityForStoreGet()
        {

            var db = new AccountingContainer();
            StoreChoose store = new StoreChoose();
            store.StoreList = new SelectList(db.Stores, "Store_ID", "Store_Name");
            return store;


        }
        /// <summary>
        /// Post of report get quantity of store by id
        /// </summary>
        /// <param name="storeVM"></param>
        /// <returns></returns>
        public static StoreQuantityVM GetQuantityOfStore(StoreChoose storeVM)
        {

            var db = new AccountingContainer();
            StoreQuantityVM storeDetail = new StoreQuantityVM();

            var store = db.Stores.Find(storeVM.Store_Id);
            storeDetail.ProductBatch = db.ProductBatches.Where(e => e.Store_ID == store.Store_ID && e.Product_Quantity != 0).ToList();
            storeDetail.Total = 0;
            storeDetail.Total = storeDetail.ProductBatch.Select(a => a.Batch_Cost).Sum();
            //foreach (var Bat in storeDetail.ProductBatch)
            //{

            //    storeDetail.Total += Bat.Batch_Cost;
            //}
            storeDetail.Store_Name = store.Store_Name;
            storeDetail.Company = db.Companies.FirstOrDefault();

            return storeDetail;


        }

        /// <summary>
        /// Get report of quantity for product by productId
        /// </summary>
        /// <returns></returns>
        public static ProductQuantityListVM getDataForQuantityOfProduct()
        {
            var db = new AccountingContainer();
            var quantityVM = new ProductQuantityListVM();

            var newList2 = from prod in db.Products.Where(a => a.Prod_Type != Prod_Type.خدمة)
                           select new
                           {

                               Id = prod.Prod_ID,
                               Name = prod.Prod_Name + "  " + prod.Bar_Code

                           };
            //foreach (var member in members2)
            //    newList2.Add(new
            //    {
            //        Id = member.Prod_ID,
            //        Name = member.Prod_Name+"  "+member.Bar_Code
            //    });

            quantityVM.ProducSelectionList = new SelectList(newList2, "Id", "Name");
            return quantityVM;
        }

        /// <summary>
        /// Post of the report of quantity for product by productId
        /// </summary>
        /// <param name="productQuantityVM"></param>
        /// <returns></returns>
        public static ProductQuantityListVM makeReportPostOf(ProductQuantityListVM productQuantityVM)
        {
            var db = new AccountingContainer();
            var ListProductQuantity = new List<ProductQuantityVM> { };
            var counter = 1;
            if (productQuantityVM.FromTo == null)
            {
                productQuantityVM.FromTo = Convert.ToString(new DateTime(1995, 1, 1));


            }
            productQuantityVM.company = db.Companies.FirstOrDefault();
            productQuantityVM.product = db.Products.Find(productQuantityVM.Product_Id);
            var start = Convert.ToDateTime(Convert.ToDateTime(productQuantityVM.FromTo).ToShortDateString());

            var startCard = getstartValueInStoreCrd(start, productQuantityVM.Product_Id, 0);
            // var RefactoreStoreCard = Store.EditProductCardLikeStore(productQuantityVM., productQuantityVM.Product_Id);
            ProductQuantityVM startQuantity = new ProductQuantityVM();

            startQuantity.number_Of_Document = 1;
            startQuantity.Document_Serial = " ";
            startQuantity.Type_Document = "رصيد اول مدة" + " | Opening balance  ";
            startQuantity.StartQuantity = startCard.Quantity;
            startQuantity.DateOfDocument = start;
            startQuantity.store = null;


            ListProductQuantity.Add(startQuantity);

            productQuantityVM.TotalStartQuantity = startQuantity.StartQuantity;
            var ImportAndExport = db.StoreCards.Where(a => a.Product_Id == productQuantityVM.Product_Id && a.Date > start).ToList();
            foreach (var item in ImportAndExport)
            {
                ProductQuantityVM MovMent = new ProductQuantityVM();
                MovMent.number_Of_Document = counter + 1;

                MovMent.DateOfDocument = item.Date;
                MovMent.Document_Serial = item.Document_Serial;

                if (item.IncreaseQuantity == true)
                {
                    MovMent.importQuantity = item.Quantity;
                    productQuantityVM.TotalImportQuantity += MovMent.importQuantity;
                }
                else
                {
                    MovMent.ExportQuantity = item.Quantity;
                    productQuantityVM.TotalExportQuantity += MovMent.ExportQuantity;
                }
                MovMent.store = db.Stores.Find(item.Store_Id);
                MovMent.Type_Document = item.TypeOfDocument;
                ListProductQuantity.Add(MovMent);

                counter++;


            }
            productQuantityVM.ProductList = ListProductQuantity;

            productQuantityVM.TotalAll = (productQuantityVM.TotalStartQuantity + productQuantityVM.TotalImportQuantity) - productQuantityVM.TotalExportQuantity;

            return productQuantityVM;


        }
        /// <summary>
        /// Report of get start value in store cart by start, and product and store
        /// </summary>
        /// <param name="start"></param>
        /// <param name="Product"></param>
        /// <param name="Store"></param>
        /// <returns></returns>
        public static StoreCard getstartValueInStoreCrd(DateTime start, int Product, int Store)
        {
            var db = new AccountingContainer();
            var storeCard = new StoreCard();
            storeCard.Quantity = 0;
            storeCard.CostPrice = 0;
            storeCard.TotalCost = 0;



            var StorecardList = db.StoreCards.Where(a => a.Product_Id == Product && a.Date <= start).ToList();
            if (Store != 0)
            {
                StorecardList = StorecardList.Where(a => a.Store_Id == Store).ToList();
            }
            foreach (var item in StorecardList)
            {
                if (item.IncreaseQuantity == true)
                {
                    storeCard.Quantity += item.Quantity;
                    storeCard.CostPrice += item.CostPrice;
                    storeCard.TotalCost += item.TotalCost;

                }
                else
                {
                    storeCard.Quantity -= item.Quantity;
                    storeCard.CostPrice -= item.CostPrice;
                    storeCard.TotalCost -= item.TotalCost;
                }




            }
            storeCard.Product_Id = Product;

            return storeCard;


        }

        /// <summary>
        /// Get all product 
        /// </summary>
        /// <returns></returns>
        public static ProductListVM getallProduct(int? all)
        {
            var db = new AccountingContainer();
            ProductListVM ProductList = new ProductListVM();
            var product = db.Products.ToList();
            ProductList.Products = product;
            ProductList.all = all;
            ProductList.company = db.Companies.FirstOrDefault();
            return ProductList;
        }

        /// <summary>
        /// Get initailise for report get product card
        /// </summary>
        /// <returns></returns>
        public static ProductCardVM getDataForProductCard()
        {
            var db = new AccountingContainer();
            var quantityVM = new ProductCardVM();

            var newList2 = from prod in db.Products.Where(a => a.Prod_Type != Prod_Type.مجمع && a.Prod_Type != Prod_Type.خدمة)
                           select new
                           {

                               Id = prod.Prod_ID,
                               Name = prod.Prod_Name + "  " + prod.Bar_Code
                           };


            quantityVM.ProducSelectionList = new SelectList(newList2, "Id", "Name");



            quantityVM.StorList = new SelectList(db.Stores, "Store_ID", "Store_Name");
            return quantityVM;
        }
        /// <summary>
        /// Post for the report  product card
        /// </summary>
        /// <param name="productQuantityVM"></param>
        /// <returns></returns>
        public static ProductCardVM makeReportPostOf(ProductCardVM productQuantityVM)
        {
            var db = new AccountingContainer();
            var ListProductQuantity = new List<ProductQuantityVM> { };
            var counter = 1;
            if (productQuantityVM.FromTo == null)
            {
                productQuantityVM.FromTo = Convert.ToString(new DateTime(1995, 1, 1));


            }
            if (productQuantityVM.To == null)
            {
                productQuantityVM.To = Convert.ToString(new DateTime(2200, 12, 30));

            }
            productQuantityVM.company = db.Companies.FirstOrDefault();
            productQuantityVM.product = db.Products.Find(productQuantityVM.Product_Id);
            productQuantityVM.Store = db.Stores.Find(productQuantityVM.Store_Id);
            //var RefactorProduct = Store.EditProductCardLikeStore(productQuantityVM.Store_Id, productQuantityVM.Product_Id);
            var start = Convert.ToDateTime(Convert.ToDateTime(productQuantityVM.FromTo).ToShortDateString());
            var End = Convert.ToDateTime(Convert.ToDateTime(productQuantityVM.To).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);

            var startCard = getstartValueInStoreCrd(start, productQuantityVM.Product_Id, productQuantityVM.Store_Id);
            ProductQuantityVM startQuantity = new ProductQuantityVM();

            startQuantity.number_Of_Document = 1;
            startQuantity.Document_Serial = " ";
            startQuantity.Type_Document = "رصيد اول مدة" + " | Opening balance  ";
            startQuantity.StartQuantity = startCard.Quantity;
            startQuantity.StartProCost = startCard.CostPrice;
            startQuantity.StartCost = startCard.TotalCost.Value;
            startQuantity.DateOfDocument = start;
            startQuantity.store = null;


            ListProductQuantity.Add(startQuantity);

            productQuantityVM.TotalStartQuantity = startQuantity.StartQuantity;
            productQuantityVM.TotalStartCost = startQuantity.StartCost;
            productQuantityVM.TotalStartProCost = startQuantity.StartProCost;

            var ImportAndExport = db.StoreCards.Where(a => a.Product_Id == productQuantityVM.Product_Id && a.Store_Id == productQuantityVM.Store_Id && a.Date >= start && a.Date <= End).ToList();
            foreach (var item in ImportAndExport)
            {
                ProductQuantityVM MovMent = new ProductQuantityVM();
                MovMent.number_Of_Document = counter + 1;

                MovMent.DateOfDocument = item.Date;
                MovMent.Document_Serial = item.Document_Serial;

                if (item.IncreaseQuantity == true)
                {
                    MovMent.importQuantity = item.Quantity;
                    MovMent.importProCost = item.CostPrice;
                    MovMent.importCost = item.TotalCost.Value;

                    productQuantityVM.TotalImportQuantity += MovMent.importQuantity;
                    productQuantityVM.TotalImportProCost += MovMent.importProCost;
                    productQuantityVM.TotalImportCost += MovMent.importCost;
                }
                else
                {
                    MovMent.ExportQuantity = item.Quantity;
                    MovMent.ExportProCost = item.CostPrice;
                    MovMent.ExportCost = item.TotalCost.Value;
                    productQuantityVM.TotalExportQuantity += MovMent.ExportQuantity;
                    productQuantityVM.TotalExportProCost += MovMent.ExportProCost;
                    productQuantityVM.TotalExportCost += MovMent.ExportCost;


                }
                MovMent.store = db.Stores.Find(item.Store_Id);
                MovMent.Type_Document = item.TypeOfDocument;
                ListProductQuantity.Add(MovMent);

                counter++;


            }
            productQuantityVM.ProductList = ListProductQuantity.OrderBy(a => a.DateOfDocument).ToList();

            productQuantityVM.TotalAllQuantity = (productQuantityVM.TotalStartQuantity + productQuantityVM.TotalImportQuantity) - productQuantityVM.TotalExportQuantity;

            productQuantityVM.TotalAllCost = (productQuantityVM.TotalStartCost + productQuantityVM.TotalImportCost) - productQuantityVM.TotalExportCost;
            try
            {
                productQuantityVM.TotalAllProCost = productQuantityVM.TotalAllCost / productQuantityVM.TotalAllQuantity;
            }
            catch
            {
                productQuantityVM.TotalAllProCost = 0;
            }
            return productQuantityVM;


        }



        /////////////// Project Section///////////
        /// <summary>
        /// Report for all project invoices 
        /// </summary>
        /// <param name="PDVM"></param>
        /// <returns></returns>
        public static List<ProjectInvoiceReportVM> allProjectInvoices(ProjectDateVM PDVM)
        {
            var db = new AccountingContainer();
            List<ProjectInvoiceMaster> thisprojectInvs = new List<ProjectInvoiceMaster>();
            if (PDVM.from != null && PDVM.to != null)
            {
                DateTime fromdate = Convert.ToDateTime(PDVM.from).Date;
                DateTime todate = Convert.ToDateTime(PDVM.to).AddHours(23).AddMinutes(59).AddSeconds(59);
                thisprojectInvs = db.ProjectInvoiceMasters.Where(e => e.Project_State == TransactionState.Post && e.ProjectInv_Date >= fromdate && e.ProjectInv_Date <= todate).ToList();
            }
            else
            {
                thisprojectInvs = db.ProjectInvoiceMasters.Where(e => e.Project_State == TransactionState.Post).ToList();
            }

            List<ProjectInvoiceReportVM> ListProject = new List<ProjectInvoiceReportVM>();
            if (thisprojectInvs.Count > 0)
            {
                foreach (var inv in thisprojectInvs)
                {
                    decimal totalamount = 0;
                    decimal totaltax = 0;
                    decimal totalaftertax = 0;

                    ProjectInvoiceReportVM PIVM = new ProjectInvoiceReportVM();
                    var thiscust = db.Customers.Find(inv.CustomerCustomer_Id);
                    PIVM.CustomerCode = thiscust.Customer_Code;
                    PIVM.customerName = inv.Customer_Name;
                    if (inv.ProjectInv_Date == null)
                    {
                        PIVM.InvoiceDate = DateTime.Today.AddYears(-1);
                    }
                    else
                    {
                        PIVM.InvoiceDate = inv.ProjectInv_Date.Value;
                    }
                    var thisdetail = db.ProjectInvoiceDetails.Where(e => e.ProjectInvoiceMasterPInvoiceMaster_ID == inv.PInvoiceMaster_ID).ToList();
                    foreach (var detail in thisdetail)
                    {

                        totalamount = totalamount + (detail.Product_Price * detail.Product_Quantity);
                        totaltax = totaltax + (detail.Detail_Total_Tax);
                        totalaftertax = totalaftertax + (detail.Total_Amount);
                    }
                    if (inv.MasterDiscount > 0)
                    {
                        decimal discount = totalamount * (inv.MasterDiscount / 100).Value;
                        PIVM.InvoiceAMount = totalamount - discount;
                    }
                    else
                    {
                        PIVM.InvoiceAMount = totalamount;
                    }

                    PIVM.TaxAMount = totaltax;
                    if (inv.FixedDiscount > 0)
                    {
                        PIVM.totalafterTax = (decimal)(totalaftertax - inv.FixedDiscount);
                    }
                    else
                    {
                        PIVM.totalafterTax = totalaftertax;
                    }
                    PIVM.InvoiceCode = inv.Serial_Number;
                    PIVM.company = db.Companies.FirstOrDefault();
                    ListProject.Add(PIVM);

                }
                return ListProject;
            }
            else
            {
                ProjectInvoiceReportVM PIVM = new ProjectInvoiceReportVM();
                PIVM.company = db.Companies.FirstOrDefault();
                ListProject.Add(PIVM);
                return ListProject;
            }

        }
        /// <summary>
        /// Get all project by employee
        /// </summary>
        /// <returns></returns>
        public static ProjectDateVM ProjectsByEmployeeForCreation()
        {
            var db = new AccountingContainer();
            var thisclassify = new ProjectDateVM();

            thisclassify.employeeList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            return thisclassify;
        }

        /// <summary>
        /// Post report for all project by employeeId
        /// </summary>
        /// <param name="PDVM"></param>
        /// <returns></returns>
        public static ProjectDateVM allProjectInvoicesForEmployee(ProjectDateVM PDVM)
        {
            var db = new AccountingContainer();
            PDVM.List = new List<ProjectInvoiceReportVM>();
            PDVM.company = db.Companies.FirstOrDefault();

            if (PDVM.from == null)
            {
                PDVM.from = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (PDVM.to == null)
            {
                PDVM.to = Convert.ToString(new DateTime(2200, 12, 30));

            }
            DateTime fromdate = Convert.ToDateTime(PDVM.from).Date;
            DateTime todate = Convert.ToDateTime(PDVM.to).AddHours(23).AddMinutes(59).AddSeconds(59);
            List<ProjectInvoiceMaster> thisprojectInvs = new List<ProjectInvoiceMaster>();
            if (PDVM.employeeId > 0)
            {
                thisprojectInvs = db.ProjectInvoiceMasters.Where(e => e.Project_State == TransactionState.Post && e.ProjectInv_Date >= fromdate && e.ProjectInv_Date <= todate && e.Emp_Id == PDVM.employeeId).ToList();
                PDVM.employee = db.Employees.Find(PDVM.employeeId);
            }
            else
            {
                thisprojectInvs = db.ProjectInvoiceMasters.Where(e => e.Project_State == TransactionState.Post && e.ProjectInv_Date >= fromdate && e.ProjectInv_Date <= todate).ToList();
            }
            List<ProjectInvoiceReportVM> ListProject = new List<ProjectInvoiceReportVM>();
            if (thisprojectInvs.Count > 0)
            {
                foreach (var inv in thisprojectInvs)
                {
                    decimal totalamount = 0;
                    decimal totaltax = 0;
                    decimal totalaftertax = 0;

                    ProjectInvoiceReportVM PIVM = new ProjectInvoiceReportVM();
                    var thiscust = db.Customers.Find(inv.CustomerCustomer_Id);
                    PIVM.CustomerCode = thiscust.Customer_Code;
                    PIVM.customerName = inv.Customer_Name;
                    PIVM.empId = PDVM.employeeId;
                    if (inv.ProjectInv_Date == null)
                    {
                        inv.ProjectInv_Date = DateTime.Today.AddYears(-1);
                    }
                    else
                    {
                        PIVM.InvoiceDate = inv.ProjectInv_Date.Value;
                    }
                    var thisdetail = db.ProjectInvoiceDetails.Where(e => e.ProjectInvoiceMasterPInvoiceMaster_ID == inv.PInvoiceMaster_ID).ToList();
                    foreach (var detail in thisdetail)
                    {

                        totalamount = totalamount + (detail.Product_Price * detail.Product_Quantity);
                        totaltax = totaltax + (detail.Detail_Total_Tax);
                        totalaftertax = totalaftertax + (detail.Total_Amount);
                    }
                    if (inv.MasterDiscount > 0)
                    {
                        decimal discount = totalamount * (inv.MasterDiscount / 100).Value;
                        PIVM.InvoiceAMount = totalamount - discount;
                    }
                    else
                    {
                        PIVM.InvoiceAMount = totalamount;
                    }

                    PIVM.TaxAMount = totaltax;
                    if (inv.FixedDiscount > 0)
                    {
                        PIVM.totalafterTax = (decimal)(totalaftertax - inv.FixedDiscount);
                    }
                    else
                    {
                        PIVM.totalafterTax = totalaftertax;
                    }
                    PIVM.InvoiceCode = inv.Serial_Number;

                    ListProject.Add(PIVM);

                }
                PDVM.List = ListProject;
                return PDVM;
            }
            else
            {
                ProjectInvoiceReportVM PIVM = new ProjectInvoiceReportVM();
                PIVM.company = db.Companies.FirstOrDefault();
                ListProject.Add(PIVM);
                PDVM.List = ListProject;
                return PDVM;

            }

        }

        /// <summary>
        /// Report for project balance 
        /// </summary>
        /// <param name="PDVM"></param>
        /// <returns></returns>
        public static ProjectDateVM ProjectsWithBalance(ProjectDateVM PDVM)
        {
            var db = new AccountingContainer();
            var thisPorject = new List<Project>();
            PDVM.ListProjectBalance = new List<ProjectBalancesVM>();
            if (PDVM.from != null && PDVM.to != null)
            {
                DateTime fromdate = Convert.ToDateTime(PDVM.from).Date;
                DateTime todate = Convert.ToDateTime(PDVM.to).AddHours(23).AddMinutes(59).AddSeconds(59);

                thisPorject = db.Projects.Where(e => e.StartTime >= fromdate && e.StartTime <= todate).ToList();
            }
            else
            {
                thisPorject = db.Projects.ToList();

            }

            if (thisPorject.Count > 0)
            {
                foreach (var project in thisPorject)
                {
                    ProjectBalancesVM PBVM = new ProjectBalancesVM();
                    var thiscustomer = db.Customers.Find(project.CustomerCustomer_Id);
                    PBVM.CustomerCode = thiscustomer.Customer_Code;
                    PBVM.CustomerName = thiscustomer.Customer_Name;
                    var ChildAccount = db.Accounts.Where(a => a.Account_Code == PBVM.CustomerCode).FirstOrDefault().Accounts_Id;
                    decimal CustomerDepit = 0;
                    decimal CustomerCredit = 0;
                    try
                    {
                        CustomerDepit = db.GLDetails.Where(a => a.Account_Child_ID == ChildAccount).Sum(a => a.GLDetail_Debit);
                    }
                    catch
                    {
                        CustomerDepit = 0;
                    }
                    try
                    {
                        CustomerCredit = db.GLDetails.Where(a => a.Account_Child_ID == ChildAccount).Sum(a => a.GLDetail_Credit);
                    }
                    catch
                    {
                        CustomerCredit = 0;
                    }
                    PBVM.Customer_ballance = CustomerDepit - CustomerCredit;
                    PBVM.ProjectAMount = @Math.Round(project.Estimate_Amount, 2, MidpointRounding.AwayFromZero);
                    var thisdetails = db.ProjectDetails.Where(e => e.ProjectProject_Id == project.Project_Id).ToList();
                    if (thisdetails.Count > 0)
                    {
                        foreach (var detail in thisdetails)
                        {
                            if (detail.Payed)
                            {
                                PBVM.PastPayedAmount = @Math.Round((PBVM.PastPayedAmount + detail.Amount), 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                PBVM.PastPayedAmount = @Math.Round((PBVM.PastPayedAmount + 0), 2, MidpointRounding.AwayFromZero); ;
                            }

                        }
                    }
                    else
                    {
                        PBVM.PastPayedAmount = 0;
                    }
                    var invposted = Project.amountafterpost(project.Project_Id);
                    PBVM.PorjectInvAMount = @Math.Round(invposted, 2, MidpointRounding.AwayFromZero);
                    PBVM.ProjectName = project.Project_Name;
                    PBVM.Statu = project.Project_State;
                    var invPayed = Project.amountofProjectRecipt(project.Project_Id);
                    PBVM.PayedAMount = @Math.Round(invPayed, 2, MidpointRounding.AwayFromZero);
                    PBVM.Balance = @Math.Round(invposted - invPayed, 2, MidpointRounding.AwayFromZero);
                    PBVM.company = db.Companies.FirstOrDefault();
                    PDVM.ListProjectBalance.Add(PBVM);
                }

            }
            else
            {
                ProjectBalancesVM PBVM = new ProjectBalancesVM();
                PBVM.company = db.Companies.FirstOrDefault();
                PDVM.ListProjectBalance.Add(PBVM);
            }
            return PDVM;
        }

        /// <summary>
        /// Get prepare to transfer from store to project
        /// </summary>
        /// <returns></returns>
        public static TransfarefromtoProjectCreationVM prepareTransProject()
        {
            var db = new AccountingContainer();
            TransfarefromtoProjectCreationVM TFPVM = new TransfarefromtoProjectCreationVM();
            TFPVM.ProjectList = new SelectList(db.SubLedgers.Where(e => e.SubLed_Type == "مشروع").ToList(), "SubLed_Id", "SubLed_Name");
            return TFPVM;
        }

        /// <summary>
        /// Report for transfer from store to project 
        /// </summary>
        /// <param name="TFVM"></param>
        /// <returns></returns>
        public static List<TransfareFromTOProjectVM> allTransfaretoProjectstore(TransfarefromtoProjectCreationVM TFVM)
        {
            // true
            // transf from porject to  store
            var db = new AccountingContainer();
            var thistrans = new List<TransferToProjectMaster>();
            if (TFVM.from != null && TFVM.to != null)
            {
                DateTime fromdate = Convert.ToDateTime(TFVM.from).Date;
                DateTime todate = Convert.ToDateTime(TFVM.to).AddHours(23).AddMinutes(59).AddSeconds(59);

                thistrans = db.TransferToProjectMasters.Where(e => e.Trans_Code.StartsWith("TTPRO") && e.Invoice_State == TransactionState.Post && e.Trans_Date >= fromdate && e.Trans_Date <= todate && e.Project_Id == TFVM.ProjectID).ToList();
            }
            else
            {
                thistrans = db.TransferToProjectMasters.Where(e => e.Trans_Code.StartsWith("TTPRO") && e.Invoice_State == TransactionState.Post && e.Project_Id == TFVM.ProjectID).ToList();

            }
            List<TransfareFromTOProjectVM> listofTrans = new List<TransfareFromTOProjectVM>();
            if (thistrans.Count > 0)
            {
                foreach (var tran in thistrans)
                {
                    TransfareFromTOProjectVM transfVM = new TransfareFromTOProjectVM();
                    transfVM.company = db.Companies.FirstOrDefault();
                    transfVM.Cost = tran.Total_Cost;
                    var thisporject = db.SubLedgers.Find(tran.Project_Id);
                    transfVM.projectName = thisporject.SubLed_Name;
                    var thisstore = db.Stores.Find(tran.Store_ID);
                    transfVM.StoreName = thisstore.Store_Name;
                    transfVM.TransCOde = tran.Trans_Code;
                    listofTrans.Add(transfVM);

                }
            }
            else
            {
                TransfareFromTOProjectVM transfVM = new TransfareFromTOProjectVM();
                transfVM.company = db.Companies.FirstOrDefault();
                listofTrans.Add(transfVM);
            }
            return listofTrans;

        }
        /// <summary>
        /// Report for transfer from project to store 
        /// </summary>
        /// <param name="TFVM"></param>
        /// <returns></returns>
        public static List<TransfareFromTOProjectVM> allTransfaretostoreProject(TransfarefromtoProjectCreationVM TFVM)
        {
            // true
            // transf from porject to  store
            var db = new AccountingContainer();
            var thistrans = new List<TransferToProjectMaster>();
            if (TFVM.from != null && TFVM.to != null)
            {
                DateTime fromdate = Convert.ToDateTime(TFVM.from).Date;
                DateTime todate = Convert.ToDateTime(TFVM.to).AddHours(23).AddMinutes(59).AddSeconds(59);

                thistrans = db.TransferToProjectMasters.Where(e => e.Trans_Code.StartsWith("TTSTO") && e.Invoice_State == TransactionState.Post && e.Trans_Date >= fromdate && e.Trans_Date <= todate && e.Project_Id == TFVM.ProjectID).ToList();
            }
            else
            {
                thistrans = db.TransferToProjectMasters.Where(e => e.Trans_Code.StartsWith("TTSTO") && e.Invoice_State == TransactionState.Post && e.Project_Id == TFVM.ProjectID).ToList();

            }
            List<TransfareFromTOProjectVM> listofTrans = new List<TransfareFromTOProjectVM>();
            if (thistrans.Count > 0)
            {
                foreach (var tran in thistrans)
                {
                    TransfareFromTOProjectVM transfVM = new TransfareFromTOProjectVM();
                    transfVM.company = db.Companies.FirstOrDefault();
                    transfVM.Cost = tran.Total_Amount;
                    var thisporject = db.SubLedgers.Find(tran.Project_Id);
                    transfVM.projectName = thisporject.SubLed_Name;
                    var thisstore = db.Stores.Find(tran.Store_ID);
                    transfVM.StoreName = thisstore.Store_Name;
                    transfVM.TransCOde = tran.Trans_Code;
                    listofTrans.Add(transfVM);

                }
            }
            else
            {
                TransfareFromTOProjectVM transfVM = new TransfareFromTOProjectVM();
                transfVM.company = db.Companies.FirstOrDefault();
                listofTrans.Add(transfVM);
            }
            return listofTrans;

        }
        /// <summary>
        /// Reports for Sales 
        /// </summary>
        /// <param name="detasofreport"></param> 
        /// <returns></returns>
        public static SalesDatesVM allSalesReport(SalesDatesVM detasofreport)
        {

            detasofreport.List = new List<SalesReportVM>();
            var db = new AccountingContainer();
            if (detasofreport.from == null)
            {
                detasofreport.from = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (detasofreport.to == null)
            {
                detasofreport.to = Convert.ToString(new DateTime(2200, 12, 30));

            }
            var fromdate = Convert.ToDateTime(detasofreport.from);
            var todate = Convert.ToDateTime(detasofreport.to).AddHours(23).AddMinutes(59).AddSeconds(59);
            var allSales = new List<InvoiceMaster>();
            var allSalesReceipt = new List<SalesReceiptMaster>();
            var allSalesPOS = new List<MasterPOSInvoice>();
            var AllOrder = new List<Order>();





            if (detasofreport.empId == 0)
            {
                allSales = db.InvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate).ToList();


                AllOrder = db.Orders.Where(e => e.Invoice_State == TransactionState.Post && e.OrderDate >= fromdate && e.OrderDate <= todate).ToList();



                allSalesReceipt = db.SalesReceiptMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate).ToList();

                allSalesPOS = db.MasterPOSInvoices.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate).ToList();
            }
            else
            {
                allSales = db.InvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate && e.Emp_Id == detasofreport.empId).ToList();




                AllOrder = db.Orders.Where(e => e.Invoice_State == TransactionState.Post && e.OrderDate >= fromdate && e.OrderDate <= todate && e.EmployeeID == detasofreport.empId).ToList();


                allSalesReceipt = db.SalesReceiptMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate && e.Emp_Id == detasofreport.empId).ToList();

                allSalesPOS = db.MasterPOSInvoices.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate && e.Employee_Id == detasofreport.empId).ToList();
            }
            List<SalesReportVM> salesList = new List<SalesReportVM>();
            var counters = 1;
            decimal totalamount = 0;
            decimal totalrecall = 0;
            decimal totalofbefor = 0;

            foreach (var master in allSales)
            {
                SalesReportVM salesVM = new SalesReportVM();
                salesVM.company = db.Companies.FirstOrDefault();
                salesVM.Counter = counters;
                salesVM.InvoiceCode = master.Serial_Number;
                salesVM.employee = detasofreport.empId;
                salesVM.InvoiceDate = master.Invoice_Date;
                salesVM.invoiceDeadLine = master.Due_Date.Value;
                salesVM.totaltax = master.Total_Tax;
                if (master.Branch_Id > 0)
                {
                    salesVM.branchName = db.SubLedgers.Find(master.Branch_Id.Value).SubLed_Name;
                    salesVM.Branch = master.Branch_Id.Value;
                }
                salesVM.quantity = master.InvoiceDetails.Select(a => a.Reverse_Quantity).Sum();
                if (master.TotaL_After_Fixed != 0 && master.TotaL_After_Fixed != null)
                {
                    salesVM.invoiceTotal = master.TotaL_After_Fixed.Value;
                }
                else
                {
                    salesVM.invoiceTotal = master.Total_Amount;
                }
                salesVM.TotalClassFiyAmount = salesVM.invoiceTotal - salesVM.totaltax;
                salesVM.totalafterfixed = master.Total_Amount - master.FixedDiscount.Value;
                var thisrecall = db.ReverseSalesInvoiceMasters.Where(e => e.InvoiceMaster_Id == master.Master_Id && e.Invoice_State == TransactionState.Post).ToList();

                if (thisrecall.Count > 0)
                {
                    salesVM.RecallInvoicetotal = thisrecall.Select(a => a.Total_Amount).Sum() - thisrecall.Select(a => a.Total_Tax).Sum();

                }
                else
                {
                    salesVM.RecallInvoicetotal = 0;
                }
                salesVM.TotalOFInvoicesBefor = (master.Total_Amount - master.Total_Tax) - salesVM.RecallInvoicetotal;
                totalamount = totalamount + salesVM.invoiceTotal;
                totalrecall = totalrecall + salesVM.RecallInvoicetotal;
                salesVM.TotalCost = master.Total_Cost;
                salesVM.TotalClassFiyCost = master.Total_Cost;
                totalofbefor = totalofbefor + salesVM.TotalOFInvoicesBefor;
                var thisCustomer = db.Customers.Find(master.Customer_Id);
                salesVM.CustomerCode = thisCustomer.Customer_Code;
                salesVM.CustomerName = thisCustomer.Customer_Name;
                salesVM.detail = new List<DetailInvoiceVM>();
                List<DetailInvoiceVM> detailInvoiceList = new List<DetailInvoiceVM> { };
                foreach (var detail in master.InvoiceDetails)
                {
                    DetailInvoiceVM detailInvoice = new DetailInvoiceVM();
                    detailInvoice.PrdouctId = detail.Product_ID;
                    try
                    {
                        detailInvoice.TotalCost = detail.Reverse_Quantity * (detail.Detail_Total_Cost / detail.Product_Quantity);
                    }
                    catch
                    {
                        detailInvoice.TotalCost = 0;
                    }
                    detailInvoice.TotalAmount = detail.Product_Price * detail.Reverse_Quantity;
                    detailInvoice.TotalTax = detail.Reverse_Total_Tax;
                    var category = db.Products.Find(detailInvoice.PrdouctId).Category_ID;
                    if (category == null)
                    {
                        detailInvoice.CategoryID = 0;
                    }
                    else
                    {
                        detailInvoice.CategoryID = category.Value;
                    }
                    detailInvoiceList.Add(detailInvoice);


                }
                salesVM.Store = master.Store_ID;
                if (master.Branch_Id == null)
                {
                    salesVM.Branch = 0;

                }
                else
                {
                    salesVM.Branch = master.Branch_Id.Value;
                }

                salesVM.employee = master.Emp_Id.Value;
                counters++;
                salesVM.detail.AddRange(detailInvoiceList);
                salesList.Add(salesVM);
            }
            foreach (var master in allSalesReceipt)
            {
                SalesReportVM salesVM = new SalesReportVM();
                salesVM.company = db.Companies.FirstOrDefault();
                salesVM.Counter = counters;
                salesVM.InvoiceCode = master.Serial_Number;
                salesVM.employee = detasofreport.empId;
                salesVM.InvoiceDate = master.Invoice_Date;
                salesVM.invoiceDeadLine = master.Invoice_Date;
                salesVM.TotalCost = master.Total_Cost;
                salesVM.totaltax = master.Total_Tax;
                salesVM.TotalClassFiyCost = master.Total_Cost;
                if (master.Branch_Id > 0)
                {
                    salesVM.branchName = db.SubLedgers.Find(master.Branch_Id.Value).SubLed_Name;
                    salesVM.Branch = master.Branch_Id.Value;
                }
                salesVM.quantity = master.SalesReceiptDetails.Select(a => a.Reverse_Quantity).Sum();
                salesVM.paymentmethodName = db.PaymentMethods.Find(master.PaymentMethod_Id).Payment_Type;


                if (master.TotaL_After_Fixed != 0 && master.TotaL_After_Fixed != null)
                {
                    salesVM.invoiceTotal = master.TotaL_After_Fixed.Value;
                }
                else
                {
                    salesVM.invoiceTotal = master.Total_Amount;
                }
                salesVM.TotalClassFiyAmount = salesVM.invoiceTotal - salesVM.totaltax;
                salesVM.totalafterfixed = master.Total_Amount - master.FixedDiscount.Value;
                var thisrecall = db.ReverseSalesReceiptMasters.Where(e => e.SalesRecipt_Id == master.Master_Id && e.Invoice_State == TransactionState.Post).ToList();
                if (thisrecall.Count > 0)
                {
                    salesVM.RecallInvoicetotal = thisrecall.Select(a => a.Total_Amount).Sum() - thisrecall.Select(a => a.Total_Tax).Sum();
                }
                else
                {
                    salesVM.RecallInvoicetotal = 0;
                }
                salesVM.TotalOFInvoicesBefor = (master.Total_Amount - master.Total_Tax) - salesVM.RecallInvoicetotal;
                totalamount = totalamount + salesVM.invoiceTotal;
                totalrecall = totalrecall + salesVM.RecallInvoicetotal;
                totalofbefor = totalofbefor + salesVM.TotalOFInvoicesBefor;

                var thisCustomer = db.Customers.Find(master.Customer_Id);
                if (thisCustomer == null)
                {
                    salesVM.CustomerCode = null;
                    salesVM.CustomerName = null;

                }
                else
                {
                    salesVM.CustomerCode = thisCustomer.Customer_Code;
                    salesVM.CustomerName = thisCustomer.Customer_Name;
                }
                salesVM.detail = new List<DetailInvoiceVM>();

                foreach (var detail in master.SalesReceiptDetails)
                {
                    DetailInvoiceVM detailInvoice = new DetailInvoiceVM();
                    detailInvoice.PrdouctId = detail.Product_Id;
                    detailInvoice.TotalCost = detail.Reverse_Quantity * (detail.Detail_Total_Cost / detail.Product_Quantity);
                    detailInvoice.TotalAmount = detail.Product_Price * detail.Reverse_Quantity;
                    var category = db.Products.Find(detailInvoice.PrdouctId).Category_ID;
                    detailInvoice.TotalTax = detail.Reverse_Total_Tax;
                    if (category == null)
                    {
                        detailInvoice.CategoryID = 0;
                    }
                    else
                    {
                        detailInvoice.CategoryID = category.Value;
                    }
                    salesVM.detail.Add(detailInvoice);

                }
                counters++;
                salesVM.Store = master.Store_ID;
                if (master.Branch_Id == null)
                {
                    salesVM.Branch = 0;

                }
                else
                {
                    salesVM.Branch = master.Branch_Id.Value;
                }
                if (master.Emp_Id == null)
                {
                    salesVM.employee = 0;

                }
                else
                {
                    salesVM.employee = master.Emp_Id.Value;
                }

                salesList.Add(salesVM);
            }
            foreach (var master in allSalesPOS)
            {
                SalesReportVM salesVM = new SalesReportVM();
                salesVM.company = db.Companies.FirstOrDefault();
                salesVM.Counter = counters;
                salesVM.InvoiceCode = master.Serial_Number;
                salesVM.employee = detasofreport.empId;
                salesVM.InvoiceDate = master.Invoice_Date.Value;
                salesVM.invoiceDeadLine = master.Invoice_Date.Value;
                salesVM.TotalCost = master.Total_Cost.Value;
                salesVM.totaltax = master.Total_Tax.Value;
                // salesVM.invoiceTotal = (master.Total_Amount.Value+master.AllowDiscount.Value);
                salesVM.invoiceTotal = master.Total_Amount.Value;
                salesVM.TotalClassFiyAmount = salesVM.invoiceTotal - salesVM.totaltax;
                salesVM.TotalClassFiyCost = master.Total_Cost.Value;
                salesVM.totalafterfixed = master.Total_Amount.Value;
                if (master.PointOfSale.BranchSubledger_Id > 0)
                {
                    salesVM.branchName = db.SubLedgers.Find(master.PointOfSale.BranchSubledger_Id.Value).SubLed_Name;
                    salesVM.Branch = master.PointOfSale.BranchSubledger_Id.Value;
                }
                salesVM.quantity = master.DetailPOSInvoices.Select(a => a.Reverse_Quantity).Sum();
                if (master.PosPaymentMethods.Count > 0)
                {
                    salesVM.paymentmethodName = db.PaymentMethods.Find(master.PosPaymentMethods.FirstOrDefault().PaymentMethod_Id).Payment_Type;
                }
                var thisrecall = db.ReversePOSInvoices.Where(e => e.MasterPOSInvoiceMasterPOSInvoiceId == master.MasterPOSInvoiceId && e.Invoice_State == TransactionState.Post).ToList();
                if (thisrecall.Count > 0)
                {
                    salesVM.RecallInvoicetotal = thisrecall.Select(a => a.Total_Amount).Sum() - thisrecall.Select(a => a.Total_Tax).Sum();
                }
                else
                {
                    salesVM.RecallInvoicetotal = 0;
                }
                salesVM.TotalOFInvoicesBefor = (master.Total_Amount.Value - master.Total_Tax.Value) - salesVM.RecallInvoicetotal;
                totalamount = totalamount + salesVM.invoiceTotal;
                totalrecall = totalrecall + salesVM.RecallInvoicetotal;
                totalofbefor = totalofbefor + salesVM.TotalOFInvoicesBefor;

                var thisCustomer = db.Customers.Find(master.Customer_Id);
                if (thisCustomer == null)
                {
                    salesVM.CustomerCode = null;
                    salesVM.CustomerName = null;
                }
                else
                {

                    salesVM.CustomerCode = thisCustomer.Customer_Code;
                    salesVM.CustomerName = thisCustomer.Customer_Name;
                }
                salesVM.detail = new List<DetailInvoiceVM>();
                foreach (var detail in master.DetailPOSInvoices)
                {
                    DetailInvoiceVM detailInvoice = new DetailInvoiceVM();
                    detailInvoice.PrdouctId = detail.Product_Id;
                    if (detail.Product_Quantity != 0)
                    {
                        detailInvoice.TotalCost = detail.Reverse_Quantity * (detail.Detail_Total_Cost / detail.Product_Quantity);


                    }

                    detailInvoice.TotalAmount = detail.Product_Price * detail.Reverse_Quantity;
                    detailInvoice.TotalTax = detail.Reverse_Total_Tax;
                    var category = db.Products.Find(detailInvoice.PrdouctId).Category_ID;
                    if (category == null)
                    {
                        detailInvoice.CategoryID = 0;
                    }
                    else
                    {
                        detailInvoice.CategoryID = category.Value;
                    }

                    salesVM.detail.Add(detailInvoice);

                }
                counters++;
                salesVM.Store = master.Store_ID.Value;
                if (master.PointOfSale.BranchSubledger_Id == null)
                {
                    salesVM.Branch = 0;

                }
                else
                {
                    salesVM.Branch = master.PointOfSale.BranchSubledger_Id.Value;
                }

                salesVM.employee = master.Employee_Id.Value;
                salesList.Add(salesVM);
            }
            foreach (var master in AllOrder)
            {
                SalesReportVM salesVM = new SalesReportVM();
                salesVM.company = db.Companies.FirstOrDefault();
                salesVM.Counter = counters;
                salesVM.InvoiceCode = master.OrderNumber;
                salesVM.employee = detasofreport.empId;
                salesVM.InvoiceDate = master.OrderDate;
                salesVM.invoiceDeadLine = master.OrderDate;
                salesVM.TotalCost = master.Total_Cost;
                salesVM.totaltax = master.Total_Tax;

                salesVM.TotalClassFiyCost = master.Total_Cost;
                if (master.Branch_Id > 0)
                {
                    salesVM.branchName = db.SubLedgers.Find(master.Branch_Id.Value).SubLed_Name;
                    salesVM.Branch = master.Branch_Id.Value;
                }
                salesVM.quantity = master.OrderDetails.Select(a => a.Reverse_Quantity).Sum();
                salesVM.paymentmethodName = db.PaymentMethods.Find(master.PaymentMethodeID).Payment_Type;


                salesVM.invoiceTotal = master.Total_Amount;

                salesVM.TotalClassFiyAmount = salesVM.invoiceTotal - salesVM.totaltax;
                salesVM.totalafterfixed = 0;
                var thisrecall = db.ReverseOrders.Where(e => e.OrderId == master.Id && e.Invoice_State == TransactionState.Post).ToList();
                if (thisrecall.Count > 0)
                {
                    salesVM.RecallInvoicetotal = thisrecall.Select(a => a.Total_Amount).Sum() - thisrecall.Select(a => a.Total_Tax).Sum();
                }
                else
                {
                    salesVM.RecallInvoicetotal = 0;
                }
                salesVM.TotalOFInvoicesBefor = (master.Total_Amount - master.Total_Tax) - salesVM.RecallInvoicetotal;
                totalamount = totalamount + salesVM.invoiceTotal;
                totalrecall = totalrecall + salesVM.RecallInvoicetotal;
                totalofbefor = totalofbefor + salesVM.TotalOFInvoicesBefor;

                var thisCustomer = db.Customers.Find(master.CustomerID);
                if (thisCustomer == null)
                {
                    salesVM.CustomerCode = null;
                    salesVM.CustomerName = null;

                }
                else
                {
                    salesVM.CustomerCode = thisCustomer.Customer_Code;
                    salesVM.CustomerName = thisCustomer.Customer_Name;
                }
                salesVM.detail = new List<DetailInvoiceVM>();

                foreach (var detail in master.OrderDetails)
                {
                    DetailInvoiceVM detailInvoice = new DetailInvoiceVM();
                    detailInvoice.PrdouctId = detail.ProductID;
                    detailInvoice.TotalCost = detail.Reverse_Quantity * (detail.Detail_Total_Cost / detail.Quantity);
                    detailInvoice.TotalAmount = detail.Product_Price * detail.Reverse_Quantity;
                    var category = db.Products.Find(detailInvoice.PrdouctId).Category_ID;
                    detailInvoice.TotalTax = detail.Reverse_Total_Tax;
                    if (category == null)
                    {
                        detailInvoice.CategoryID = 0;
                    }
                    else
                    {
                        detailInvoice.CategoryID = category.Value;
                    }
                    salesVM.detail.Add(detailInvoice);

                }
                counters++;
                salesVM.Store = db.Stores.FirstOrDefault().Store_ID;





                salesVM.employee = master.EmployeeID;


                salesList.Add(salesVM);
            }

            if (salesList.Count == 0)
            {
                SalesReportVM vm = new SalesReportVM();
                vm.company = db.Companies.FirstOrDefault();
                salesList.Add(vm);
            }
            detasofreport.List = salesList.OrderBy(a => a.InvoiceDate).ToList();



            return detasofreport;
        }

        /// <summary>
        /// For get all sales invoice in system
        /// </summary>
        /// <param name="detasofreport"></param>
        /// <returns></returns>
        public static List<SalesReportVM> allSalesReportForAllSystem(SalesDatesVM detasofreport)
        {
            var db = new AccountingContainer();
            if (detasofreport.from == null)
            {
                detasofreport.from = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (detasofreport.to == null)
            {
                detasofreport.to = Convert.ToString(new DateTime(2200, 12, 30));

            }
            var fromdate = Convert.ToDateTime(detasofreport.from);
            var todate = Convert.ToDateTime(detasofreport.to).AddHours(23).AddMinutes(59).AddSeconds(59);



            var allSales = db.InvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate).ToList();

            var allSalesReceipt = db.SalesReceiptMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate).ToList();
            var AllOrder = db.Orders.Where(e => e.Invoice_State == TransactionState.Post && e.OrderDate >= fromdate && e.OrderDate <= todate).ToList();

            var allSalesPOS = db.MasterPOSInvoices.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate).ToList();
            var allSubScription = db.Subscriptions.Where(e => e.Subscrip_State == TransactionState.Post && e.StartTime >= fromdate && e.StartTime <= todate && e.Emp_Id != null).ToList();
            var AllProject = db.Projects.Where(e => e.StartTime >= fromdate && e.StartTime <= todate && e.Emp_Id != null).ToList();

            List<SalesReportVM> salesList = new List<SalesReportVM>();
            var counters = 1;
            decimal totalamount = 0;
            decimal totalrecall = 0;
            decimal totalofbefor = 0;

            foreach (var master in allSales)
            {
                SalesReportVM salesVM = new SalesReportVM();
                salesVM.company = db.Companies.FirstOrDefault();
                salesVM.Counter = counters;
                salesVM.InvoiceCode = master.Serial_Number;
                salesVM.employee = detasofreport.empId;
                salesVM.InvoiceDate = master.Invoice_Date;
                salesVM.invoiceDeadLine = master.Due_Date.Value;
                salesVM.totaltax = master.Total_Tax;
                salesVM.invoiceTotal = master.Total_Amount - master.Total_Tax;
                salesVM.totalafterfixed = master.Total_Amount - master.FixedDiscount.Value;
                var thisrecall = db.ReverseSalesInvoiceMasters.Where(e => e.InvoiceMaster_Id == master.Master_Id && e.Invoice_State == TransactionState.Post).ToList();
                if (thisrecall.Count > 0)
                {
                    salesVM.RecallInvoicetotal = 0;
                    foreach (var rcall in thisrecall)
                    {
                        var Sum = rcall.Total_Amount - rcall.Total_Tax;
                        salesVM.RecallInvoicetotal += Sum;
                    }
                }
                else
                {
                    salesVM.RecallInvoicetotal = 0;
                }
                salesVM.TotalOFInvoicesBefor = salesVM.invoiceTotal - salesVM.RecallInvoicetotal;
                totalamount = totalamount + salesVM.invoiceTotal;
                totalrecall = totalrecall + salesVM.RecallInvoicetotal;
                salesVM.TotalCost = master.Total_Cost;
                totalofbefor = totalofbefor + salesVM.TotalOFInvoicesBefor;
                var thisCustomer = db.Customers.Find(master.Customer_Id);
                salesVM.CustomerCode = thisCustomer.Customer_Code;
                salesVM.CustomerName = thisCustomer.Customer_Name;
                salesVM.detail = new List<DetailInvoiceVM>();
                List<DetailInvoiceVM> detailInvoiceList = new List<DetailInvoiceVM> { };
                foreach (var detail in master.InvoiceDetails)
                {
                    DetailInvoiceVM detailInvoice = new DetailInvoiceVM();
                    detailInvoice.PrdouctId = detail.Product_ID;
                    var category = db.Products.Find(detailInvoice.PrdouctId).Category_ID;
                    if (category == null)
                    {
                        detailInvoice.CategoryID = 0;
                    }
                    else
                    {
                        detailInvoice.CategoryID = category.Value;
                    }
                    detailInvoiceList.Add(detailInvoice);


                }
                salesVM.Store = master.Store_ID;
                if (master.Branch_Id == null)
                {
                    salesVM.Branch = 0;

                }
                else
                {
                    salesVM.Branch = master.Branch_Id.Value;
                }

                salesVM.employee = master.Emp_Id.Value;
                counters++;
                salesVM.detail.AddRange(detailInvoiceList);
                salesList.Add(salesVM);
            }
            foreach (var master in allSalesReceipt)
            {
                SalesReportVM salesVM = new SalesReportVM();
                salesVM.company = db.Companies.FirstOrDefault();
                salesVM.Counter = counters;
                salesVM.InvoiceCode = master.Serial_Number;
                salesVM.employee = detasofreport.empId;
                salesVM.InvoiceDate = master.Invoice_Date;
                salesVM.invoiceDeadLine = master.Invoice_Date;
                salesVM.TotalCost = master.Total_Cost;
                salesVM.totaltax = master.Total_Tax;
                salesVM.invoiceTotal = master.Total_Amount - master.Total_Tax;
                salesVM.totalafterfixed = master.Total_Amount - master.FixedDiscount.Value;
                var thisrecall = db.ReverseSalesReceiptMasters.Where(e => e.SalesRecipt_Id == master.Master_Id && e.Invoice_State == TransactionState.Post).ToList();
                if (thisrecall.Count > 0)
                {
                    salesVM.RecallInvoicetotal = 0;
                    foreach (var rcall in thisrecall)
                    {
                        var Sum = rcall.Total_Amount - rcall.Total_Tax;
                        salesVM.RecallInvoicetotal += Sum;
                    }
                }
                else
                {
                    salesVM.RecallInvoicetotal = 0;
                }
                salesVM.TotalOFInvoicesBefor = salesVM.invoiceTotal - salesVM.RecallInvoicetotal;
                totalamount = totalamount + salesVM.invoiceTotal;
                totalrecall = totalrecall + salesVM.RecallInvoicetotal;
                totalofbefor = totalofbefor + salesVM.TotalOFInvoicesBefor;

                var thisCustomer = db.Customers.Find(master.Customer_Id);
                if (thisCustomer == null)
                {
                    salesVM.CustomerCode = null;
                    salesVM.CustomerName = null;

                }
                else
                {
                    salesVM.CustomerCode = thisCustomer.Customer_Code;
                    salesVM.CustomerName = thisCustomer.Customer_Name;
                }
                salesVM.detail = new List<DetailInvoiceVM>();

                foreach (var detail in master.SalesReceiptDetails)
                {
                    DetailInvoiceVM detailInvoice = new DetailInvoiceVM();
                    detailInvoice.PrdouctId = detail.Product_Id;
                    var category = db.Products.Find(detailInvoice.PrdouctId).Category_ID;
                    if (category == null)
                    {
                        detailInvoice.CategoryID = 0;
                    }
                    else
                    {
                        detailInvoice.CategoryID = category.Value;
                    }
                    salesVM.detail.Add(detailInvoice);

                }
                counters++;
                salesVM.Store = master.Store_ID;
                if (master.Branch_Id == null)
                {
                    salesVM.Branch = 0;

                }
                else
                {
                    salesVM.Branch = master.Branch_Id.Value;
                }
                if (master.Emp_Id == null)
                {
                    salesVM.employee = 0;

                }
                else
                {
                    salesVM.employee = master.Emp_Id.Value;
                }

                salesList.Add(salesVM);
            }
            foreach (var master in AllOrder)
            {
                SalesReportVM salesVM = new SalesReportVM();
                salesVM.company = db.Companies.FirstOrDefault();
                salesVM.Counter = counters;
                salesVM.InvoiceCode = master.OrderNumber;
                salesVM.employee = detasofreport.empId;
                salesVM.InvoiceDate = master.OrderDate;
                salesVM.invoiceDeadLine = master.OrderDate;
                salesVM.TotalCost = master.Total_Cost;
                salesVM.totaltax = master.Total_Tax;
                salesVM.invoiceTotal = master.Total_Amount - master.Total_Tax;
                salesVM.totalafterfixed = master.Total_Amount;
                var thisrecall = db.ReverseOrders.Where(e => e.OrderId == master.Id && e.Invoice_State == TransactionState.Post).ToList();
                if (thisrecall.Count > 0)
                {
                    salesVM.RecallInvoicetotal = 0;
                    foreach (var rcall in thisrecall)
                    {
                        var Sum = rcall.Total_Amount - rcall.Total_Tax;
                        salesVM.RecallInvoicetotal += Sum;
                    }
                }
                else
                {
                    salesVM.RecallInvoicetotal = 0;
                }
                salesVM.TotalOFInvoicesBefor = salesVM.invoiceTotal - salesVM.RecallInvoicetotal;
                totalamount = totalamount + salesVM.invoiceTotal;
                totalrecall = totalrecall + salesVM.RecallInvoicetotal;
                totalofbefor = totalofbefor + salesVM.TotalOFInvoicesBefor;

                var thisCustomer = db.Customers.Find(master.CustomerID);
                if (thisCustomer == null)
                {
                    salesVM.CustomerCode = null;
                    salesVM.CustomerName = null;

                }
                else
                {
                    salesVM.CustomerCode = thisCustomer.Customer_Code;
                    salesVM.CustomerName = thisCustomer.Customer_Name;
                }
                salesVM.detail = new List<DetailInvoiceVM>();

                foreach (var detail in master.OrderDetails)
                {
                    DetailInvoiceVM detailInvoice = new DetailInvoiceVM();
                    detailInvoice.PrdouctId = detail.ProductID;
                    var category = db.Products.Find(detailInvoice.PrdouctId).Category_ID;
                    if (category == null)
                    {
                        detailInvoice.CategoryID = 0;
                    }
                    else
                    {
                        detailInvoice.CategoryID = category.Value;
                    }
                    salesVM.detail.Add(detailInvoice);

                }
                counters++;
                salesVM.Store = db.Stores.FirstOrDefault().Store_ID;

                salesVM.Branch = 0;



                salesVM.employee = master.EmployeeID;


                salesList.Add(salesVM);
            }

            foreach (var master in allSalesPOS)
            {
                SalesReportVM salesVM = new SalesReportVM();
                salesVM.company = db.Companies.FirstOrDefault();
                salesVM.Counter = counters;
                salesVM.InvoiceCode = master.Serial_Number;
                salesVM.employee = detasofreport.empId;
                salesVM.InvoiceDate = master.Invoice_Date.Value;
                salesVM.invoiceDeadLine = master.Invoice_Date.Value;
                salesVM.TotalCost = master.Total_Cost.Value;
                salesVM.totaltax = master.Total_Tax.Value;
                salesVM.invoiceTotal = (master.Total_Amount.Value + master.AllowDiscount.Value) - master.Total_Tax.Value;
                salesVM.totalafterfixed = master.Total_Amount.Value;
                var thisrecall = db.ReversePOSInvoices.Where(e => e.MasterPOSInvoiceMasterPOSInvoiceId == master.MasterPOSInvoiceId && e.Invoice_State == TransactionState.Post).ToList();
                if (thisrecall.Count > 0)
                {
                    salesVM.RecallInvoicetotal = 0;
                    foreach (var rcall in thisrecall)
                    {
                        var Sum = rcall.Total_Amount - rcall.Total_Tax;
                        salesVM.RecallInvoicetotal += Sum;
                    }
                }
                else
                {
                    salesVM.RecallInvoicetotal = 0;
                }
                salesVM.TotalOFInvoicesBefor = salesVM.invoiceTotal - salesVM.RecallInvoicetotal;
                totalamount = totalamount + salesVM.invoiceTotal;
                totalrecall = totalrecall + salesVM.RecallInvoicetotal;
                totalofbefor = totalofbefor + salesVM.TotalOFInvoicesBefor;

                var thisCustomer = db.Customers.Find(master.Customer_Id);
                if (thisCustomer == null)
                {
                    salesVM.CustomerCode = null;
                    salesVM.CustomerName = null;
                }
                else
                {

                    salesVM.CustomerCode = thisCustomer.Customer_Code;
                    salesVM.CustomerName = thisCustomer.Customer_Name;
                }
                salesVM.detail = new List<DetailInvoiceVM>();
                foreach (var detail in master.DetailPOSInvoices)
                {
                    DetailInvoiceVM detailInvoice = new DetailInvoiceVM();
                    detailInvoice.PrdouctId = detail.Product_Id;
                    var category = db.Products.Find(detailInvoice.PrdouctId).Category_ID;
                    if (category == null)
                    {
                        detailInvoice.CategoryID = 0;
                    }
                    else
                    {
                        detailInvoice.CategoryID = category.Value;
                    }

                    salesVM.detail.Add(detailInvoice);

                }
                counters++;
                salesVM.Store = master.Store_ID.Value;
                if (master.PointOfSale.BranchSubledger_Id == null)
                {
                    salesVM.Branch = 0;

                }
                else
                {
                    salesVM.Branch = master.PointOfSale.BranchSubledger_Id.Value;
                }

                salesVM.employee = master.Employee_Id.Value;
                salesList.Add(salesVM);
            }
            foreach (var master in AllProject)
            {
                SalesReportVM salesVM = new SalesReportVM();
                salesVM.company = db.Companies.FirstOrDefault();
                salesVM.Counter = counters;
                salesVM.InvoiceCode = master.Project_Code;
                salesVM.employee = detasofreport.empId;
                salesVM.InvoiceDate = master.StartTime;
                salesVM.invoiceDeadLine = master.StartTime;
                salesVM.TotalCost = 0;
                salesVM.totaltax = 0;
                salesVM.invoiceTotal = master.Estimate_Amount;
                salesVM.totalafterfixed = master.Estimate_Amount;
                totalamount = totalamount + salesVM.invoiceTotal;

                var thisCustomer = db.Customers.Find(master.CustomerCustomer_Id);
                if (thisCustomer == null)
                {
                    salesVM.CustomerCode = null;
                    salesVM.CustomerName = null;
                }
                else
                {

                    salesVM.CustomerCode = thisCustomer.Customer_Code;
                    salesVM.CustomerName = thisCustomer.Customer_Name;
                }


                counters++;
                salesVM.Store = 0;
                if (master.BranchSubledger_Id == null)
                {
                    salesVM.Branch = 0;

                }
                else
                {
                    salesVM.Branch = master.BranchSubledger_Id.Value;
                }

                salesVM.employee = master.Emp_Id.Value;
                salesList.Add(salesVM);
            }
            foreach (var master in allSubScription)
            {
                SalesReportVM salesVM = new SalesReportVM();
                salesVM.company = db.Companies.FirstOrDefault();
                salesVM.Counter = counters;
                salesVM.InvoiceCode = master.Subscrip_Code;
                salesVM.employee = detasofreport.empId;
                salesVM.InvoiceDate = master.StartTime;
                salesVM.invoiceDeadLine = master.StartTime;
                salesVM.TotalCost = 0;

                salesVM.invoiceTotal = master.AmountAfterTax;
                salesVM.totalafterfixed = master.AmountAfterTax;


                totalamount = totalamount + salesVM.invoiceTotal;

                totalofbefor = totalofbefor + salesVM.TotalOFInvoicesBefor;

                var thisCustomer = db.Customers.Find(master.Customer_Id);
                if (thisCustomer == null)
                {
                    salesVM.CustomerCode = null;
                    salesVM.CustomerName = null;
                }
                else
                {

                    salesVM.CustomerCode = thisCustomer.Customer_Code;
                    salesVM.CustomerName = thisCustomer.Customer_Name;
                }

                counters++;
                salesVM.Store = 0;
                if (master.Subscrip_Pranch == null)
                {
                    salesVM.Branch = 0;

                }
                else
                {
                    salesVM.Branch = master.Subscrip_Pranch.Value;
                }

                salesVM.employee = master.Emp_Id.Value;
                salesList.Add(salesVM);
            }

            if (salesList.Count == 0)
            {
                SalesReportVM vm = new SalesReportVM();
                vm.company = db.Companies.FirstOrDefault();
                salesList.Add(vm);
            }
            return salesList;
        }
        /// <summary>
        /// For all revers Sales 
        /// </summary>
        /// <param name="detasofreport"></param>
        /// <returns></returns>
        public static SalesDatesVM allRecallSalesPrint(SalesDatesVM detasofreport)
        {
            var db = new AccountingContainer();
            detasofreport.ReversList = new List<RecallSalesReportVM>();
            var fromdate = Convert.ToDateTime(detasofreport.from);
            var todate = Convert.ToDateTime(detasofreport.to).AddHours(23).AddMinutes(59).AddSeconds(59);
            List<ReverseSalesInvoiceMaster> allRecall = new List<ReverseSalesInvoiceMaster>();
            List<ReversePOSInvoice> allRecallPos = new List<ReversePOSInvoice>();
            List<ReverseSalesReceiptMaster> allRecallRecipt = new List<ReverseSalesReceiptMaster>();
            List<ReverseOrder> allReVOrD = new List<ReverseOrder>();

            if (detasofreport.from == null || detasofreport.to == null)
            {
                allRecall = db.ReverseSalesInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post).ToList();
                allRecallPos = db.ReversePOSInvoices.Where(e => e.Invoice_State == TransactionState.Post).ToList();
                allRecallRecipt = db.ReverseSalesReceiptMasters.Where(e => e.Invoice_State == TransactionState.Post).ToList();
                allReVOrD = db.ReverseOrders.Where(e => e.Invoice_State == TransactionState.Post).ToList();
            }
            else
            {
                allRecall = db.ReverseSalesInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate).ToList();
                allRecallPos = db.ReversePOSInvoices.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate).ToList();
                allRecallRecipt = db.ReverseSalesReceiptMasters.Where(e => e.Invoice_State == TransactionState.Post && e.Invoice_Date >= fromdate && e.Invoice_Date <= todate).ToList();
                allReVOrD = db.ReverseOrders.Where(e => e.Invoice_State == TransactionState.Post && e.ReverseOrderDate >= fromdate && e.ReverseOrderDate <= todate).ToList();
            }

            List<RecallSalesReportVM> Recalllist = new List<RecallSalesReportVM>();
            var counters = 1;

            if (allRecall.Count > 0)
            {
                foreach (var master in allRecall)
                {
                    RecallSalesReportVM recallSalesReportVM = new RecallSalesReportVM();
                    recallSalesReportVM.company = db.Companies.FirstOrDefault();
                    recallSalesReportVM.Counter = counters;
                    recallSalesReportVM.InvoiceCode = master.Serial_Number;
                    recallSalesReportVM.RecallDate = master.Invoice_Date;
                    recallSalesReportVM.RecallDeadLine = Convert.ToDateTime(master.Due_Date).Day + "-" + Convert.ToDateTime(master.Due_Date).Month + "-" + Convert.ToDateTime(master.Due_Date).Year;
                    var invoiceSales = db.InvoiceMasters.Where(i => i.Master_Id == master.InvoiceMaster_Id).FirstOrDefault();
                    recallSalesReportVM.Invoice_Serial = invoiceSales.Serial_Number;
                    var employee = db.Employees.Where(e => e.Emp_Id == invoiceSales.Emp_Id).FirstOrDefault();
                    if (employee != null)
                    {
                        recallSalesReportVM.Emp_Name = employee.Emp_Name;
                    }
                    else
                    {
                        recallSalesReportVM.Emp_Name = "";
                    }
                    recallSalesReportVM.TotalOFRecallamount = master.Total_Amount - master.Total_Tax;

                    if (master.Total_Tax > 0)
                    {
                        recallSalesReportVM.RecalltotalTax = master.Total_Tax;
                    }
                    else
                    {
                        recallSalesReportVM.RecalltotalTax = 0;
                    }
                    recallSalesReportVM.RecalltotalafterTax = master.Total_Amount;


                    var thissupp = db.Customers.Find(master.Customer_Id);
                    recallSalesReportVM.CustomerCode = thissupp.Customer_Code;
                    recallSalesReportVM.CustomerName = thissupp.Customer_Name;
                    counters++;
                    Recalllist.Add(recallSalesReportVM);
                }
                //return Recalllist;
            }


            if (allRecallPos.Count > 0)
            {
                foreach (var master in allRecallPos)
                {
                    RecallSalesReportVM recallSalesReportVM = new RecallSalesReportVM();
                    recallSalesReportVM.company = db.Companies.FirstOrDefault();
                    recallSalesReportVM.Counter = counters;
                    recallSalesReportVM.InvoiceCode = master.Serial_Number;
                    recallSalesReportVM.RecallDate = master.Invoice_Date;
                    var invoiceSales = db.MasterPOSInvoices.Where(i => i.MasterPOSInvoiceId == master.MasterPOSInvoiceMasterPOSInvoiceId).FirstOrDefault();
                    recallSalesReportVM.Invoice_Serial = invoiceSales.Serial_Number;
                    var employee = db.Employees.Where(e => e.Emp_Id == invoiceSales.Employee_Id).FirstOrDefault();
                    if (employee != null)
                    {
                        recallSalesReportVM.Emp_Name = employee.Emp_Name;
                    }
                    else
                    {
                        recallSalesReportVM.Emp_Name = "";
                    }

                    recallSalesReportVM.TotalOFRecallamount = master.Total_Amount - master.Total_Tax;

                    if (master.Total_Tax > 0)
                    {
                        recallSalesReportVM.RecalltotalTax = master.Total_Tax;
                    }
                    else
                    {
                        recallSalesReportVM.RecalltotalTax = 0;
                    }
                    recallSalesReportVM.RecalltotalafterTax = master.Total_Amount;


                    if (master.Customer_Id > 0)
                    {
                        var thissupp = db.Customers.Find(master.Customer_Id);
                        recallSalesReportVM.CustomerCode = thissupp.Customer_Code;
                        recallSalesReportVM.CustomerName = thissupp.Customer_Name;
                    }
                    counters++;
                    Recalllist.Add(recallSalesReportVM);
                }
                //return Recalllist;
            }

            if (allRecallRecipt.Count > 0)
            {
                foreach (var master in allRecallRecipt)
                {
                    RecallSalesReportVM recallSalesReportVM = new RecallSalesReportVM();
                    recallSalesReportVM.company = db.Companies.FirstOrDefault();
                    recallSalesReportVM.Counter = counters;
                    recallSalesReportVM.InvoiceCode = master.Serial_Number;
                    recallSalesReportVM.RecallDate = master.Invoice_Date;

                    var invoiceSales = db.SalesReceiptMasters.Where(i => i.Master_Id == master.SalesRecipt_Id).FirstOrDefault();
                    recallSalesReportVM.Invoice_Serial = invoiceSales.Serial_Number;
                    var employee = db.Employees.Where(e => e.Emp_Id == invoiceSales.Emp_Id).FirstOrDefault();
                    if (employee != null)
                    {
                        recallSalesReportVM.Emp_Name = employee.Emp_Name;
                    }
                    else
                    {
                        recallSalesReportVM.Emp_Name = "";
                    }


                    recallSalesReportVM.TotalOFRecallamount = master.Total_Amount - master.Total_Tax;

                    if (master.Total_Tax > 0)
                    {
                        recallSalesReportVM.RecalltotalTax = master.Total_Tax;
                    }
                    else
                    {
                        recallSalesReportVM.RecalltotalTax = 0;
                    }
                    recallSalesReportVM.RecalltotalafterTax = master.Total_Amount;

                    if (master.Customer_Id > 0)
                    {
                        var thissupp = db.Customers.Find(master.Customer_Id);
                        recallSalesReportVM.CustomerCode = thissupp.Customer_Code;
                        recallSalesReportVM.CustomerName = thissupp.Customer_Name;
                    }

                    counters++;
                    Recalllist.Add(recallSalesReportVM);
                }
                //return Recalllist;
            }

            if (allReVOrD.Count > 0)
            {
                foreach (var master in allReVOrD)
                {
                    RecallSalesReportVM recallSalesReportVM = new RecallSalesReportVM();
                    recallSalesReportVM.company = db.Companies.FirstOrDefault();
                    recallSalesReportVM.Counter = counters;
                    recallSalesReportVM.InvoiceCode = master.OrderNumber;
                    recallSalesReportVM.RecallDate = master.ReverseOrderDate;

                    var invoiceSales = db.Orders.Where(i => i.Id == master.Id).FirstOrDefault();
                    recallSalesReportVM.Invoice_Serial = invoiceSales.OrderNumber;
                    var employee = db.Employees.Where(e => e.Emp_Id == invoiceSales.EmployeeID).FirstOrDefault();
                    if (employee != null)
                    {
                        recallSalesReportVM.Emp_Name = employee.Emp_Name;
                    }
                    else
                    {
                        recallSalesReportVM.Emp_Name = "";
                    }


                    recallSalesReportVM.TotalOFRecallamount = master.Total_Amount - master.Total_Tax;

                    if (master.Total_Tax > 0)
                    {
                        recallSalesReportVM.RecalltotalTax = master.Total_Tax;
                    }
                    else
                    {
                        recallSalesReportVM.RecalltotalTax = 0;
                    }
                    recallSalesReportVM.RecalltotalafterTax = master.Total_Amount;

                    if (master.CustomerID > 0)
                    {
                        var thissupp = db.Customers.Find(master.CustomerID);
                        recallSalesReportVM.CustomerCode = thissupp.Customer_Code;
                        recallSalesReportVM.CustomerName = thissupp.Customer_Name;
                    }

                    counters++;
                    Recalllist.Add(recallSalesReportVM);
                }
                //return Recalllist;
            }

            if (allRecall.Count <= 0 && allRecallPos.Count <= 0 && allRecallRecipt.Count <= 0 && allReVOrD.Count <= 0)
            {

                RecallSalesReportVM recallSalesReportVM = new RecallSalesReportVM();
                recallSalesReportVM.company = db.Companies.FirstOrDefault();
                Recalllist.Add(recallSalesReportVM);
                //return Recalllist;

            }
            detasofreport.ReversList = Recalllist;
            return detasofreport;

        }

        /// <summary>
        /// For Creation classify sales
        /// </summary>
        /// <returns></returns>
        public static ClassifySalesInvoiceVM SalesforgettingreportClassify()
        {
            var db = new AccountingContainer();
            var thisclassify = new ClassifySalesInvoiceVM();
            thisclassify.branchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.SubLed_Type == "فرع"), "SubLed_Id", "SubLed_Name");
            thisclassify.ProductClassList = new SelectList(db.Categories.ToList(), "Cat_ID", "Cat_Descrip");
            thisclassify.ProductList = new SelectList(db.Products.ToList(), "Prod_ID", "Prod_Name");
            thisclassify.storeList = new SelectList(db.Stores.ToList(), "Store_ID", "Store_Name");
            thisclassify.CustomerList = new SelectList(db.Customers.ToList(), "Customer_Id", "Customer_Name");
            thisclassify.employeeList = new SelectList(db.Employees.Where(e => e.Reg_ID != null).ToList(), "Emp_Id", "Emp_Name");
            thisclassify.UsersList = new SelectList(db.Employees.Where(e => e.Reg_ID != null).ToList(), "Emp_Id", "Emp_Name");
            return thisclassify;
        }
        /// <summary>
        /// For Post classify sales
        /// </summary>
        /// <param name="detasofreport"></param>
        /// <returns></returns>
        public static ClassifySalesInvoiceVM CalssifiedSalesPrint(ClassifySalesInvoiceVM detasofreport)
        {
            var db = new AccountingContainer();
            SalesDatesVM getAllInvoice = new SalesDatesVM();
            detasofreport.List = new List<SalesReportVM>();
            getAllInvoice.from = detasofreport.from;
            getAllInvoice.to = detasofreport.to;
            if (detasofreport.employeeId != 0 && detasofreport.employeeId != null)
            {
                getAllInvoice.empId = detasofreport.employeeId.Value;
            }

            var salesList = allSalesReport(getAllInvoice).List;

            if (detasofreport.branchid != 0 && detasofreport.branchid != null)
            {
                salesList = salesList.Where(a => a.Branch == detasofreport.branchid).ToList();
            }
            if (detasofreport.storeID != 0 && detasofreport.storeID != null)
            {
                salesList = salesList.Where(a => a.Store == detasofreport.storeID).ToList();
            }
            if (detasofreport.customerId != 0 && detasofreport.customerId != null)
            {
                var Customer = db.Customers.Find(detasofreport.customerId);

                salesList = salesList.Where(a => a.CustomerCode == Customer.Customer_Code).ToList();
            }
            if (detasofreport.employeeId != 0 && detasofreport.employeeId != null)
            {
                salesList = salesList.Where(a => a.employee == detasofreport.employeeId).ToList();
            }
            if (detasofreport.ProductID != 0 && detasofreport.ProductID != null)
            {

                foreach (var item in salesList)
                {
                    var isExist = item.detail.Where(a => a.PrdouctId == detasofreport.ProductID).ToList();
                    if (isExist.Count() == 0)
                    {
                        item.Remove = true;
                    }
                    else
                    {
                        item.TotalClassFiyAmount = isExist.Sum(a => a.TotalAmount);
                        item.TotalClassFiyCost = isExist.Sum(a => a.TotalCost);
                    }
                }
            }
            if (detasofreport.productClassify != 0 && detasofreport.productClassify != null)
            {

                foreach (var item in salesList)
                {
                    var isExist = item.detail.Where(a => a.CategoryID == detasofreport.productClassify).ToList();
                    if (isExist.Count() == 0)
                    {
                        item.Remove = true;


                    }
                    else
                    {
                        item.TotalClassFiyAmount = isExist.Sum(a => a.TotalAmount);
                        item.TotalClassFiyCost = isExist.Sum(a => a.TotalCost);
                    }
                }
            }
            var z = salesList;
            var List = salesList.Where(a => a.Remove != true).ToList();
            if (List.Count == 0)
            {
                SalesReportVM vm = new SalesReportVM();
                vm.company = db.Companies.FirstOrDefault();
                List.Add(vm);
            }
            detasofreport.List = List;
            return detasofreport;

        }
        /// <summary>
        /// For classfied sales in all system by branchId, storeId, customerId,employeeId and productId
        /// </summary>
        /// <param name="detasofreport"></param>
        /// <returns></returns>
        public static List<SalesReportVM> CalssifiedSalesPrintForAllSystem(ClassifySalesInvoiceVM detasofreport)
        {
            var db = new AccountingContainer();
            SalesDatesVM getAllInvoice = new SalesDatesVM();
            getAllInvoice.from = detasofreport.from;
            getAllInvoice.to = detasofreport.to;
            getAllInvoice.empId = detasofreport.employeeId.Value;
            var salesList = allSalesReportForAllSystem(getAllInvoice);

            if (detasofreport.branchid != 0 && detasofreport.branchid != null)
            {
                salesList = salesList.Where(a => a.Branch == detasofreport.branchid).ToList();
            }
            if (detasofreport.storeID != 0 && detasofreport.storeID != null)
            {
                salesList = salesList.Where(a => a.Store == detasofreport.storeID).ToList();
            }
            if (detasofreport.customerId != 0 && detasofreport.customerId != null)
            {
                var Customer = db.Customers.Find(detasofreport.customerId);

                salesList = salesList.Where(a => a.CustomerCode == Customer.Customer_Code).ToList();
            }
            if (detasofreport.employeeId != 0 && detasofreport.employeeId != null)
            {
                salesList = salesList.Where(a => a.employee == detasofreport.employeeId).ToList();
            }
            if (detasofreport.ProductID != 0 && detasofreport.ProductID != null)
            {

                foreach (var item in salesList)
                {
                    var isExist = item.detail.Where(a => a.PrdouctId == detasofreport.ProductID).ToList();
                    if (isExist.Count() == 0)
                    {
                        item.Remove = true;
                    }
                }
            }
            if (detasofreport.productClassify != 0 && detasofreport.productClassify != null)
            {

                foreach (var item in salesList)
                {
                    var isExist = item.detail.Where(a => a.CategoryID == detasofreport.productClassify).ToList();
                    if (isExist.Count() == 0)
                    {
                        item.Remove = true;


                    }
                }
            }
            var z = salesList;
            var List = salesList.Where(a => a.Remove != true).ToList();
            if (List.Count == 0)
            {
                SalesReportVM vm = new SalesReportVM();
                vm.company = db.Companies.FirstOrDefault();
                List.Add(vm);
            }
            return List;

        }
        /// <summary>
        /// For Creation Sales by employee
        /// </summary>
        /// <returns></returns>
        public static ClassifySalesInvoiceVM SalesByEmployeeForCreation()
        {
            var db = new AccountingContainer();
            var thisclassify = new ClassifySalesInvoiceVM();

            thisclassify.employeeList = new SelectList(db.Employees.Where(e => e.Reg_ID != null).ToList(), "Emp_Id", "Emp_Name");
            thisclassify.UsersList = new SelectList(db.Employees.Where(e => e.Reg_ID != null).ToList(), "Emp_Id", "Emp_Name");
            return thisclassify;
        }
        /// <summary>
        /// To get sales by employee 
        /// </summary>
        /// <param name="detasofreport"></param>
        /// <returns></returns>
        public static ClassifySalesInvoiceVM allSalesByEmployeeReport(ClassifySalesInvoiceVM detasofreport)
        {
            var db = new AccountingContainer();
            var List = CalssifiedSalesPrint(detasofreport);

            return List;


        }
        public static ClassifySalesInvoiceVM allSalesInvoiceByEmployeeReport(ClassifySalesInvoiceVM detasofreport)
        {
            var db = new AccountingContainer();

            SalesDatesVM getAllInvoice = new SalesDatesVM();
            detasofreport.List = new List<SalesReportVM>();
            getAllInvoice.from = detasofreport.from;
            getAllInvoice.to = detasofreport.to;
            if (detasofreport.from == null)
            {
                detasofreport.from = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (detasofreport.to == null)
            {
                detasofreport.to = Convert.ToString(new DateTime(2200, 12, 30));

            }
            int counters = 1;
            decimal totalamount = 0;
            decimal totalrecall = 0;
            decimal totalofbefor = 0;
            var fromdate = Convert.ToDateTime(detasofreport.from);
            var todate = Convert.ToDateTime(detasofreport.to).AddHours(23).AddMinutes(59).AddSeconds(59);
            var invoicemaster = db.InvoiceMasters.Where(a => a.Invoice_Date >= fromdate && a.Invoice_Date <= todate && a.Emp_Id == detasofreport.employeeId && a.Invoice_State == TransactionState.Post).ToList();
            var MasterPOS = db.MasterPOSInvoices.Where(a => a.Invoice_Date >= fromdate && a.Invoice_Date <= todate && a.Employee_Id == detasofreport.employeeId && a.PosPaymentMethods.Count() == 0 && a.Invoice_State == TransactionState.Post).ToList();

            foreach (var master in invoicemaster)
            {
                SalesReportVM salesVM = new SalesReportVM();
                salesVM.company = db.Companies.FirstOrDefault();
                salesVM.Counter = counters;
                salesVM.InvoiceCode = master.Serial_Number;
                salesVM.employee = detasofreport.employeeId.Value;
                salesVM.InvoiceDate = master.Invoice_Date;
                var receipt = db.Receipts.Where(a => a.Rec_state == TransactionState.Post && a.Invoice_ID == master.Master_Id).ToList();
                if (receipt.Count() == 0)
                {
                    salesVM.PayedAmount = 0;
                }
                else
                {
                    salesVM.PayedAmount = receipt.Select(a => a.Rec_Amount).Sum();
                }
                salesVM.invoiceDeadLine = master.Due_Date.Value;
                salesVM.totaltax = master.Total_Tax;
                if (master.Branch_Id > 0)
                {
                    salesVM.branchName = db.SubLedgers.Find(master.Branch_Id.Value).SubLed_Name;
                    salesVM.Branch = master.Branch_Id.Value;
                }
                salesVM.quantity = master.InvoiceDetails.Select(a => a.Reverse_Quantity).Sum();
                if (master.TotaL_After_Fixed != 0 && master.TotaL_After_Fixed != null)
                {
                    salesVM.invoiceTotal = master.TotaL_After_Fixed.Value;
                }
                else
                {
                    salesVM.invoiceTotal = master.Total_Amount;
                }
                salesVM.TotalClassFiyAmount = salesVM.invoiceTotal - salesVM.totaltax;
                salesVM.totalafterfixed = master.Total_Amount - master.FixedDiscount.Value;
                var thisrecall = db.ReverseSalesInvoiceMasters.Where(e => e.InvoiceMaster_Id == master.Master_Id && e.Invoice_State == TransactionState.Post).ToList();

                if (thisrecall.Count > 0)
                {
                    salesVM.RecallInvoicetotal = thisrecall.Select(a => a.Total_Amount).Sum() - thisrecall.Select(a => a.Total_Tax).Sum();

                }
                else
                {
                    salesVM.RecallInvoicetotal = 0;
                }
                salesVM.TotalOFInvoicesBefor = (master.Total_Amount - master.Total_Tax) - salesVM.RecallInvoicetotal;
                totalamount = totalamount + salesVM.invoiceTotal;
                totalrecall = totalrecall + salesVM.RecallInvoicetotal;
                salesVM.TotalCost = master.Total_Cost;
                salesVM.TotalClassFiyCost = master.Total_Cost;
                totalofbefor = totalofbefor + salesVM.TotalOFInvoicesBefor;
                var thisCustomer = db.Customers.Find(master.Customer_Id);
                salesVM.CustomerCode = thisCustomer.Customer_Code;
                salesVM.CustomerName = thisCustomer.Customer_Name;
                salesVM.detail = new List<DetailInvoiceVM>();
                List<DetailInvoiceVM> detailInvoiceList = new List<DetailInvoiceVM> { };
                foreach (var detail in master.InvoiceDetails)
                {
                    DetailInvoiceVM detailInvoice = new DetailInvoiceVM();
                    detailInvoice.PrdouctId = detail.Product_ID;
                    detailInvoice.TotalCost = detail.Reverse_Quantity * (detail.Detail_Total_Cost / detail.Product_Quantity);
                    detailInvoice.TotalAmount = detail.Product_Price * detail.Reverse_Quantity;
                    detailInvoice.TotalTax = detail.Reverse_Total_Tax;
                    var category = db.Products.Find(detailInvoice.PrdouctId).Category_ID;
                    if (category == null)
                    {
                        detailInvoice.CategoryID = 0;
                    }
                    else
                    {
                        detailInvoice.CategoryID = category.Value;
                    }
                    detailInvoiceList.Add(detailInvoice);


                }
                salesVM.Store = master.Store_ID;
                if (master.Branch_Id == null)
                {
                    salesVM.Branch = 0;

                }
                else
                {
                    salesVM.Branch = master.Branch_Id.Value;
                }

                salesVM.employee = master.Emp_Id.Value;
                counters++;
                salesVM.detail.AddRange(detailInvoiceList);
                detasofreport.List.Add(salesVM);
            }

            foreach (var master in MasterPOS)
            {
                SalesReportVM salesVM = new SalesReportVM();
                var receipt = master.POSReceipts.Where(a => a.Rec_state == TransactionState.Post).ToList();
                if (receipt.Count() == 0)
                {
                    salesVM.PayedAmount = 0;
                }
                else
                {
                    salesVM.PayedAmount = receipt.Select(a => a.Rec_Amount).Sum();
                }
                salesVM.company = db.Companies.FirstOrDefault();
                salesVM.Counter = counters;
                salesVM.InvoiceCode = master.Serial_Number;
                salesVM.employee = detasofreport.employeeId.Value;
                salesVM.InvoiceDate = master.Invoice_Date.Value;
                salesVM.invoiceDeadLine = master.Invoice_Date.Value;
                salesVM.TotalCost = master.Total_Cost.Value;
                salesVM.totaltax = master.Total_Tax.Value;
                // salesVM.invoiceTotal = (master.Total_Amount.Value+master.AllowDiscount.Value);
                salesVM.invoiceTotal = master.Total_Amount.Value;
                salesVM.TotalClassFiyAmount = salesVM.invoiceTotal - salesVM.totaltax;
                salesVM.TotalClassFiyCost = master.Total_Cost.Value;
                salesVM.totalafterfixed = master.Total_Amount.Value;
                if (master.PointOfSale.BranchSubledger_Id > 0)
                {
                    salesVM.branchName = db.SubLedgers.Find(master.PointOfSale.BranchSubledger_Id.Value).SubLed_Name;
                    salesVM.Branch = master.PointOfSale.BranchSubledger_Id.Value;
                }
                salesVM.quantity = master.DetailPOSInvoices.Select(a => a.Reverse_Quantity).Sum();
                if (master.PosPaymentMethods.Count > 0)
                {
                    salesVM.paymentmethodName = db.PaymentMethods.Find(master.PosPaymentMethods.FirstOrDefault().PaymentMethod_Id).Payment_Type;
                }
                var thisrecall = db.ReversePOSInvoices.Where(e => e.MasterPOSInvoiceMasterPOSInvoiceId == master.MasterPOSInvoiceId && e.Invoice_State == TransactionState.Post).ToList();
                if (thisrecall.Count > 0)
                {
                    salesVM.RecallInvoicetotal = thisrecall.Select(a => a.Total_Amount).Sum() - thisrecall.Select(a => a.Total_Tax).Sum();
                }
                else
                {
                    salesVM.RecallInvoicetotal = 0;
                }
                salesVM.TotalOFInvoicesBefor = (master.Total_Amount.Value - master.Total_Tax.Value) - salesVM.RecallInvoicetotal;
                totalamount = totalamount + salesVM.invoiceTotal;
                totalrecall = totalrecall + salesVM.RecallInvoicetotal;
                totalofbefor = totalofbefor + salesVM.TotalOFInvoicesBefor;

                var thisCustomer = db.Customers.Find(master.Customer_Id);
                if (thisCustomer == null)
                {
                    salesVM.CustomerCode = null;
                    salesVM.CustomerName = null;
                }
                else
                {

                    salesVM.CustomerCode = thisCustomer.Customer_Code;
                    salesVM.CustomerName = thisCustomer.Customer_Name;
                }
                salesVM.detail = new List<DetailInvoiceVM>();
                foreach (var detail in master.DetailPOSInvoices)
                {
                    DetailInvoiceVM detailInvoice = new DetailInvoiceVM();
                    detailInvoice.PrdouctId = detail.Product_Id;
                    if (detail.Product_Quantity != 0)
                    {
                        detailInvoice.TotalCost = detail.Reverse_Quantity * (detail.Detail_Total_Cost / detail.Product_Quantity);


                    }

                    detailInvoice.TotalAmount = detail.Product_Price * detail.Reverse_Quantity;
                    detailInvoice.TotalTax = detail.Reverse_Total_Tax;
                    var category = db.Products.Find(detailInvoice.PrdouctId).Category_ID;
                    if (category == null)
                    {
                        detailInvoice.CategoryID = 0;
                    }
                    else
                    {
                        detailInvoice.CategoryID = category.Value;
                    }

                    salesVM.detail.Add(detailInvoice);

                }
                counters++;
                salesVM.Store = master.Store_ID.Value;
                if (master.PointOfSale.BranchSubledger_Id == null)
                {
                    salesVM.Branch = 0;

                }
                else
                {
                    salesVM.Branch = master.PointOfSale.BranchSubledger_Id.Value;
                }

                salesVM.employee = master.Employee_Id.Value;
                detasofreport.List.Add(salesVM);
            }
            if (detasofreport.List.Count() == 0)
            {
                SalesReportVM salesVM = new SalesReportVM();

                salesVM.company = db.Companies.FirstOrDefault();


                salesVM.employee = detasofreport.employeeId.Value;
                detasofreport.List.Add(salesVM);
            }
            return detasofreport;


        }
        /// <summary>
        /// To get sales by classfication 
        /// </summary>
        /// <param name="detasofreport"></param>
        /// <returns></returns>
        public static ClassifySalesInvoiceVM allSalesByEmployeeReportForAllSystem(ClassifySalesInvoiceVM detasofreport)
        {
            var db = new AccountingContainer();
            var List = CalssifiedSalesPrintForAllSystem(detasofreport);
            detasofreport.List = new List<SalesReportVM>();
            detasofreport.List = List;
            return detasofreport;


        }

        /// <summary>
        /// Report for all offer price 
        /// </summary>
        /// <param name="detasofreport"></param>
        /// <returns></returns>
        public static SalesDatesVM allOfferPriceReport(SalesDatesVM detasofreport)
        {
            var db = new AccountingContainer();
            detasofreport.OfferList = new List<OfferPriceReportVM>();
            var fromdate = Convert.ToDateTime(detasofreport.from);
            var todate = Convert.ToDateTime(detasofreport.to).AddHours(23).AddMinutes(59).AddSeconds(59);
            List<OfferPriceMaster> offerPriceMastersList = new List<OfferPriceMaster>();

            if (detasofreport.from == null || detasofreport.to == null)
            {
                if (detasofreport.OfferTransfer == false && detasofreport.OfferNotTransfer == false)
                {
                    offerPriceMastersList = db.OfferPriceMasters.ToList();
                }
                else if (detasofreport.OfferTransfer == true && detasofreport.OfferNotTransfer == false)
                {
                    offerPriceMastersList = db.OfferPriceMasters.Where(o => o.Offer_State == true).ToList();
                }
                else if (detasofreport.OfferTransfer == false && detasofreport.OfferNotTransfer == true)
                {
                    offerPriceMastersList = db.OfferPriceMasters.Where(o => o.Offer_State == false).ToList();
                }
                else
                {
                    offerPriceMastersList = db.OfferPriceMasters.ToList();
                }

            }
            else
            {
                if (detasofreport.OfferTransfer == false && detasofreport.OfferNotTransfer == false)
                {
                    offerPriceMastersList = db.OfferPriceMasters.Where(e => e.Offer_Date >= fromdate && e.Offer_Date <= todate).ToList();

                }
                else if (detasofreport.OfferTransfer == true && detasofreport.OfferNotTransfer == false)
                {
                    offerPriceMastersList = db.OfferPriceMasters.Where(o => o.Offer_State == true && o.Offer_Date >= fromdate && o.Offer_Date <= todate).ToList();
                }
                else if (detasofreport.OfferTransfer == false && detasofreport.OfferNotTransfer == true)
                {
                    offerPriceMastersList = db.OfferPriceMasters.Where(o => o.Offer_State == false && o.Offer_Date >= fromdate && o.Offer_Date <= todate).ToList();
                }
                else
                {
                    offerPriceMastersList = db.OfferPriceMasters.Where(e => e.Offer_Date >= fromdate && e.Offer_Date <= todate).ToList();
                }
            }

            List<OfferPriceReportVM> offerPriceReportVMsList = new List<OfferPriceReportVM>();
            var counters = 1;
            decimal totalamount = 0;
            decimal totalrecall = 0;
            decimal totalofbefor = 0;
            if (offerPriceMastersList.Count > 0)
            {
                foreach (var master in offerPriceMastersList)
                {
                    OfferPriceReportVM offerPriceVM = new OfferPriceReportVM();
                    offerPriceVM.company = db.Companies.FirstOrDefault();
                    offerPriceVM.Counter = counters;
                    offerPriceVM.OfferPriceCode = master.Serial_Number;
                    offerPriceVM.OfferPriceDate = master.Offer_Date;
                    offerPriceVM.OfferPriceDeadLine = Convert.ToDateTime(master.OfferDeadLine).Day + "-" + Convert.ToDateTime(master.OfferDeadLine).Month + "-" + Convert.ToDateTime(master.OfferDeadLine).Year;

                    offerPriceVM.OfferPriceTotal = master.Total_Amount;



                    var thisCustomer = db.Customers.Find(master.Customer_Id);
                    offerPriceVM.CustomerCode = thisCustomer.Customer_Code;
                    offerPriceVM.CustomerName = thisCustomer.Customer_Name;
                    counters++;
                    offerPriceReportVMsList.Add(offerPriceVM);

                }
                detasofreport.OfferList = offerPriceReportVMsList;
                return detasofreport;
            }
            else
            {
                OfferPriceReportVM offerPriceVM = new OfferPriceReportVM();
                offerPriceVM.company = db.Companies.FirstOrDefault();
                offerPriceReportVMsList.Add(offerPriceVM);
                detasofreport.OfferList = offerPriceReportVMsList;
                return detasofreport;

            }

        }
        /// <summary>
        /// Print all customers balance 
        /// </summary>
        /// <returns></returns>
        public static List<CustomerInfoVM> AllCustomerPrint()
        {
            var db = new AccountingContainer();
            var allCustomer = db.Customers.ToList();
            List<CustomerInfoVM> customerInfosList = new List<CustomerInfoVM>();
            foreach (var customer in allCustomer)
            {
                CustomerInfoVM customerInfoVM = new CustomerInfoVM();
                var thisaccount = db.Accounts.Where(e => e.Account_Code == customer.Customer_Code).FirstOrDefault();
                if (thisaccount.Account_Balance != null && thisaccount.Account_Balance > 0)
                {
                    customerInfoVM.Balance = @Math.Round((decimal)thisaccount.Account_Balance, 2, MidpointRounding.AwayFromZero).ToString();


                }
                else if (thisaccount.Account_Balance != null && thisaccount.Account_Balance < 0)
                {
                    decimal supafterneg = @Math.Round((decimal)thisaccount.Account_Balance, 2, MidpointRounding.AwayFromZero) * -1;
                    customerInfoVM.Balance = "(" + supafterneg.ToString() + ")";
                }
                else
                {
                    customerInfoVM.Balance = "0";
                }
                customerInfoVM.Email = customer.Customer_Mail;
                customerInfoVM.CustomerName = customer.Customer_Name;
                customerInfoVM.CustomerCode = customer.Customer_Code;
                customerInfoVM.CustomerPhone = customer.Customer_Phone;
                customerInfoVM.company = db.Companies.FirstOrDefault();
                customerInfoVM.customer = customer;
                customerInfosList.Add(customerInfoVM);
            }
            return customerInfosList;
        }

        /// <summary>
        /// Report to get customers balance with days before 30 day and after 30,60,90 and 120 days
        /// </summary>
        /// <param name="SGVM"></param>
        /// <returns></returns>
        public static List<CustomerReportWithDaysVM> getCustomerWithDays(SupplierGettingDateVM SGVM)
        {
            var db = new AccountingContainer();
            var allCustomers = db.Customers.ToList();
            DateTime thisdate = Convert.ToDateTime(SGVM.startDate);
            DateTime befor30 = Convert.ToDateTime(SGVM.startDate).AddMonths(-1);
            DateTime after30 = Convert.ToDateTime(SGVM.startDate).AddDays(29);
            DateTime after301 = after30.AddDays(1);
            DateTime after60 = after301.AddDays(29);
            DateTime after601 = after60.AddDays(1);
            DateTime after90 = after601.AddDays(29);
            DateTime after120 = after90.AddDays(1);

            List<CustomerReportWithDaysVM> CustomersList = new List<CustomerReportWithDaysVM>();
            foreach (var cust in allCustomers)
            {
                decimal befor30balance = 0;
                decimal after30balance = 0;
                decimal after60balance = 0;
                decimal after90balance = 0;
                decimal after120balance = 0;
                CustomerReportWithDaysVM SRD = new CustomerReportWithDaysVM();
                var accountCustomer = db.Accounts.Where(a => a.Account_Code == cust.Customer_Code).FirstOrDefault();
                SRD.company = db.Companies.FirstOrDefault();
                SRD.CustomerName = cust.Customer_Name;
                SRD.CustomerCode = cust.Customer_Code;
                if (accountCustomer.Account_Balance != null && accountCustomer.Account_Balance > 0)
                {
                    SRD.Balance = @Math.Round((decimal)accountCustomer.Account_Balance, 2, MidpointRounding.AwayFromZero).ToString();


                }
                else if (accountCustomer.Account_Balance != null && accountCustomer.Account_Balance < 0)
                {
                    decimal supafterneg = @Math.Round((decimal)accountCustomer.Account_Balance, 2, MidpointRounding.AwayFromZero) * -1;
                    SRD.Balance = "(" + supafterneg.ToString() + ")";
                }
                else
                {
                    SRD.Balance = "0";
                }
                //.AddHours(23).AddMinutes(59)
                DateTime firstdate = thisdate.AddHours(23).AddMinutes(59);
                DateTime seconddate = after30;
                DateTime thirddate = after60;
                DateTime fourthdate = after90;
                DateTime Fifthdate = after120;
                var thisdetailsCust = db.GLDetails.Where(e => e.Account_Child_ID == accountCustomer.Accounts_Id).ToList();
                List<GLMaster> glmasterlist = new List<GLMaster>();
                List<GLMaster> glmasterlistInvoice = new List<GLMaster>();
                foreach (var det in thisdetailsCust)
                {
                    GLMaster gl = new GLMaster();
                    gl = db.GLMasters.Find(det.GLMasterGLMaster_Id);
                    if (gl.GLCode.StartsWith("SAL-"))
                    {
                        glmasterlistInvoice.Add(gl);
                    }
                    else
                    {
                        glmasterlist.Add(gl);
                    }

                }
                List<GLMaster> glmasterlistInvoices = glmasterlistInvoice.Distinct().ToList();
                if (glmasterlistInvoices.Count > 0)
                {
                    foreach (var salesdetail in glmasterlistInvoices)
                    {
                        var invoiceMaster = db.InvoiceMasters.Where(e => e.Serial_Number == salesdetail.GLCode).ToList().FirstOrDefault();
                        DateTime thisDuedate = Convert.ToDateTime(invoiceMaster.Due_Date);
                        if (thisDuedate > thisdate)
                        {
                            var after30ofthis = thisdate.AddDays(29);
                            var after30by1 = after30ofthis.AddDays(1);
                            var after60ofthis = after30by1.AddDays(29);
                            var after60by1 = after60ofthis.AddDays(1);
                            var after90ofthis = after60by1.AddDays(29);
                            var after90by1 = after90ofthis.AddDays(1);
                            if (thisDuedate >= thisdate && thisDuedate <= after30ofthis)
                            {
                                var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == salesdetail.GLMaster_Id && e.Account_Child_ID == accountCustomer.Accounts_Id).ToList();

                                if (thisdetailBefor.Count != 0)
                                {
                                    foreach (var GLdetail in thisdetailBefor)
                                    {
                                        if (GLdetail.GLDetail_Debit > 0)
                                        {
                                            after30balance = after30balance + GLdetail.GLDetail_Debit;
                                        }
                                        else
                                        {
                                            after30balance = after30balance - GLdetail.GLDetail_Credit;
                                        }
                                    }
                                }
                                else
                                {

                                }

                                if (after30balance > 0)
                                {
                                    SRD.afterthiry = @Math.Round((decimal)after30balance, 2, MidpointRounding.AwayFromZero).ToString();


                                }
                                else if (after30balance < 0)
                                {
                                    decimal supafterneg = @Math.Round((decimal)after30balance, 2, MidpointRounding.AwayFromZero) * -1;
                                    SRD.afterthiry = "(" + supafterneg.ToString() + ")";
                                }
                                else
                                {
                                    SRD.afterthiry = "0";
                                }
                            }
                            else if (thisDuedate > after30ofthis && thisDuedate <= after60ofthis)
                            {
                                var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == salesdetail.GLMaster_Id && e.Account_Child_ID == accountCustomer.Accounts_Id).ToList();

                                if (thisdetailBefor.Count != 0)
                                {
                                    foreach (var GLdetail in thisdetailBefor)
                                    {
                                        if (GLdetail.GLDetail_Debit > 0)
                                        {
                                            after60balance = after60balance + GLdetail.GLDetail_Debit;
                                        }
                                        else
                                        {
                                            after60balance = after60balance - GLdetail.GLDetail_Credit;
                                        }
                                    }
                                }
                                else
                                {

                                }

                                if (after60balance > 0)
                                {
                                    SRD.aftersixty = @Math.Round((decimal)after60balance, 2, MidpointRounding.AwayFromZero).ToString();


                                }
                                else if (after60balance < 0)
                                {
                                    decimal supafterneg = @Math.Round((decimal)after60balance, 2, MidpointRounding.AwayFromZero) * -1;
                                    SRD.aftersixty = "(" + supafterneg.ToString() + ")";
                                }
                                else
                                {
                                    SRD.aftersixty = "0";
                                }
                            }
                            else if (thisDuedate > after60ofthis && thisDuedate <= after90ofthis)
                            {
                                var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == salesdetail.GLMaster_Id && e.Account_Child_ID == accountCustomer.Accounts_Id).ToList();

                                if (thisdetailBefor.Count != 0)
                                {
                                    foreach (var GLdetail in thisdetailBefor)
                                    {
                                        if (GLdetail.GLDetail_Debit > 0)
                                        {
                                            after90balance = after90balance + GLdetail.GLDetail_Debit;
                                        }
                                        else
                                        {
                                            after90balance = after90balance - GLdetail.GLDetail_Credit;
                                        }
                                    }
                                }
                                else
                                {

                                }

                                if (after90balance > 0)
                                {
                                    SRD.afterninty = @Math.Round((decimal)after90balance, 2, MidpointRounding.AwayFromZero).ToString();


                                }
                                else if (after90balance < 0)
                                {
                                    decimal supafterneg = @Math.Round((decimal)after90balance, 2, MidpointRounding.AwayFromZero) * -1;
                                    SRD.afterninty = "(" + supafterneg.ToString() + ")";
                                }
                                else
                                {
                                    SRD.afterninty = "0";
                                }
                            }
                            else if (thisDuedate > after90ofthis)
                            {
                                var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == salesdetail.GLMaster_Id && e.Account_Child_ID == accountCustomer.Accounts_Id).ToList();

                                if (thisdetailBefor.Count != 0)
                                {
                                    foreach (var GLdetail in thisdetailBefor)
                                    {
                                        if (GLdetail.GLDetail_Debit > 0)
                                        {
                                            after120balance = after120balance + GLdetail.GLDetail_Debit;
                                        }
                                        else
                                        {
                                            after120balance = after120balance - GLdetail.GLDetail_Credit;
                                        }
                                    }
                                }
                                else
                                {

                                }

                                if (after120balance > 0)
                                {
                                    SRD.afteronehunderd = @Math.Round((decimal)after120balance, 2, MidpointRounding.AwayFromZero).ToString();


                                }
                                else if (after120balance < 0)
                                {
                                    decimal supafterneg = @Math.Round((decimal)after120balance, 2, MidpointRounding.AwayFromZero) * -1;
                                    SRD.afteronehunderd = "(" + supafterneg.ToString() + ")";
                                }
                                else
                                {
                                    SRD.afteronehunderd = "0";
                                }
                            }

                        }
                        else
                        {
                            var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == salesdetail.GLMaster_Id && e.Account_Child_ID == accountCustomer.Accounts_Id).ToList();

                            if (thisdetailBefor.Count != 0)
                            {
                                foreach (var GLdetail in thisdetailBefor)
                                {
                                    if (GLdetail.GLDetail_Debit > 0)
                                    {
                                        befor30balance = befor30balance + GLdetail.GLDetail_Debit;
                                    }
                                    else
                                    {
                                        befor30balance = befor30balance - GLdetail.GLDetail_Credit;
                                    }
                                }
                            }
                            else
                            {

                            }

                            if (befor30balance > 0)
                            {
                                SRD.beforthiry = @Math.Round((decimal)befor30balance, 2, MidpointRounding.AwayFromZero).ToString();


                            }
                            else if (befor30balance < 0)
                            {
                                decimal supafterneg = @Math.Round((decimal)befor30balance, 2, MidpointRounding.AwayFromZero) * -1;
                                SRD.beforthiry = "(" + supafterneg.ToString() + ")";
                            }
                            else
                            {
                                SRD.beforthiry = "0";
                            }
                        }

                    }
                }
                //////////////////
                List<GLMaster> glmasterlists = glmasterlist.Distinct().ToList();
                var allGLMbefor30 = glmasterlists.Where(e => e.GL_Date < thisdate).ToList();
                if (allGLMbefor30.Count > 0)
                {
                    foreach (var detail in allGLMbefor30)
                    {
                        var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == detail.GLMaster_Id && e.Account_Child_ID == accountCustomer.Accounts_Id).ToList();

                        if (thisdetailBefor.Count != 0)
                        {
                            foreach (var GLdetail in thisdetailBefor)
                            {
                                if (GLdetail.GLDetail_Debit > 0)
                                {
                                    befor30balance = befor30balance + GLdetail.GLDetail_Debit;
                                }
                                else
                                {
                                    befor30balance = befor30balance - GLdetail.GLDetail_Credit;
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    if (befor30balance > 0)
                    {
                        SRD.beforthiry = @Math.Round((decimal)befor30balance, 2, MidpointRounding.AwayFromZero).ToString();


                    }
                    else if (befor30balance < 0)
                    {
                        decimal supafterneg = @Math.Round((decimal)befor30balance, 2, MidpointRounding.AwayFromZero) * -1;
                        SRD.beforthiry = "(" + supafterneg.ToString() + ")";
                    }
                    else
                    {
                        SRD.beforthiry = "0";
                    }
                }

                var allGLMafter30 = glmasterlists.Where(e => e.GL_Date >= thisdate && e.GL_Date <= seconddate).ToList();
                if (allGLMafter30.Count > 0)
                {
                    foreach (var detail in allGLMafter30)
                    {
                        var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == detail.GLMaster_Id && e.Account_Child_ID == accountCustomer.Accounts_Id).ToList();

                        if (thisdetailBefor.Count != 0)
                        {
                            foreach (var GLdetail in thisdetailBefor)
                            {
                                if (GLdetail.GLDetail_Debit > 0)
                                {
                                    after30balance = after30balance + GLdetail.GLDetail_Debit;
                                }
                                else
                                {
                                    after30balance = after30balance - GLdetail.GLDetail_Credit;
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    if (after30balance > 0)
                    {
                        SRD.afterthiry = @Math.Round((decimal)after30balance, 2, MidpointRounding.AwayFromZero).ToString();


                    }
                    else if (after30balance < 0)
                    {
                        decimal supafterneg = @Math.Round((decimal)after30balance, 2, MidpointRounding.AwayFromZero) * -1;
                        SRD.afterthiry = "(" + supafterneg.ToString() + ")";
                    }
                    else
                    {
                        SRD.afterthiry = "0";
                    }
                }

                var allGLMafter60 = glmasterlists.Where(e => e.GL_Date > after30 && e.GL_Date <= thirddate).ToList();
                if (allGLMafter60.Count > 0)
                {
                    foreach (var detail in allGLMafter60)
                    {
                        var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == detail.GLMaster_Id && e.Account_Child_ID == accountCustomer.Accounts_Id).ToList();

                        if (thisdetailBefor.Count != 0)
                        {
                            foreach (var GLdetail in thisdetailBefor)
                            {
                                if (GLdetail.GLDetail_Debit > 0)
                                {
                                    after60balance = after60balance + GLdetail.GLDetail_Debit;
                                }
                                else
                                {
                                    after60balance = after60balance - GLdetail.GLDetail_Credit;
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    if (after60balance > 0)
                    {
                        SRD.aftersixty = @Math.Round((decimal)after60balance, 2, MidpointRounding.AwayFromZero).ToString();


                    }
                    else if (after60balance < 0)
                    {
                        decimal supafterneg = @Math.Round((decimal)after60balance, 2, MidpointRounding.AwayFromZero) * -1;
                        SRD.aftersixty = "(" + supafterneg.ToString() + ")";
                    }
                    else
                    {
                        SRD.aftersixty = "0";
                    }
                }

                var allGLMafter90 = glmasterlists.Where(e => e.GL_Date > after60 && e.GL_Date <= fourthdate).ToList();
                if (allGLMafter90.Count > 0)
                {
                    foreach (var detail in allGLMafter60)
                    {
                        var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == detail.GLMaster_Id && e.Account_Child_ID == accountCustomer.Accounts_Id).ToList();

                        if (thisdetailBefor.Count != 0)
                        {
                            foreach (var GLdetail in thisdetailBefor)
                            {
                                if (GLdetail.GLDetail_Debit > 0)
                                {
                                    after90balance = after90balance + GLdetail.GLDetail_Debit;
                                }
                                else
                                {
                                    after90balance = after90balance - GLdetail.GLDetail_Credit;
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    if (after90balance > 0)
                    {
                        SRD.afterninty = @Math.Round((decimal)after90balance, 2, MidpointRounding.AwayFromZero).ToString();


                    }
                    else if (after90balance < 0)
                    {
                        decimal supafterneg = @Math.Round((decimal)after90balance, 2, MidpointRounding.AwayFromZero) * -1;
                        SRD.afterninty = "(" + supafterneg.ToString() + ")";
                    }
                    else
                    {
                        SRD.afterninty = "0";
                    }
                }

                var allGLMafter120 = glmasterlists.Where(e => e.GL_Date > after90).ToList();
                if (allGLMafter120.Count > 0)
                {
                    foreach (var detail in allGLMafter120)
                    {
                        var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == detail.GLMaster_Id && e.Account_Child_ID == accountCustomer.Accounts_Id).ToList();

                        if (thisdetailBefor.Count != 0)
                        {
                            foreach (var GLdetail in thisdetailBefor)
                            {
                                if (GLdetail.GLDetail_Debit > 0)
                                {
                                    after120balance = after120balance + GLdetail.GLDetail_Debit;
                                }
                                else
                                {
                                    after120balance = after120balance - GLdetail.GLDetail_Credit;
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    if (after120balance > 0)
                    {
                        SRD.afteronehunderd = @Math.Round((decimal)after120balance, 2, MidpointRounding.AwayFromZero).ToString();


                    }
                    else if (after120balance < 0)
                    {
                        decimal supafterneg = @Math.Round((decimal)after120balance, 2, MidpointRounding.AwayFromZero) * -1;
                        SRD.afteronehunderd = "(" + supafterneg.ToString() + ")";
                    }
                    else
                    {
                        SRD.afteronehunderd = "0";
                    }
                }
                if (SRD.Balance == "0")
                {

                }
                else
                {
                    CustomersList.Add(SRD);
                }


            }
            return CustomersList;
        }

        public static CustomerStatisticListVM GetStatisticListVM(CustomerStatisticListVM vm)
        {
            var db = new AccountingContainer();
            DateTime startdate = new DateTime();
            if (vm.StartDate == "" || vm.StartDate == null)
            {
                startdate = Convert.ToDateTime(DateTime.Today).AddHours(23).AddMinutes(59);
            }
            else
            {
                startdate = Convert.ToDateTime(vm.StartDate).AddHours(23).AddMinutes(59);
            }
            vm.StartDate = Convert.ToString(startdate);
            vm.company = db.Companies.FirstOrDefault();
            vm.List = new List<CustomerStatisticVM>();
            if (vm.cust_Id == null || vm.cust_Id == 0)
            {
                vm.List = MoreCustomerStatistic(startdate, vm.haveBallance);
            }
            else
            {
                vm.List = OneCustomerStatistic(startdate, vm.cust_Id.Value);
            }

            return vm;

        }
        public static List<CustomerStatisticVM> MoreCustomerStatistic(DateTime date, bool havebalance)
        {
            var db = new AccountingContainer();
            DateTime After30 = date.AddDays(-30);
            DateTime After60 = date.AddDays(-60);
            DateTime After90 = date.AddDays(-90);
            var dateminus = date.Date.AddDays(-1);
            var dateminus30 = After30.Date.AddDays(-1);
            var dateminus60 = After60.Date.AddDays(-1);

            var dateminusEnd = date.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            var dateminus30End = After30.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            var dateminus60End = After60.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            List<CustomerStatisticVM> staticList = new List<CustomerStatisticVM>();

            foreach (var customer in db.Customers.Where(a => a.Customer_State == Customer_State.نشط))
            {
                var statistic = new CustomerStatisticVM();
                var Account = db.Accounts.Where(a => a.Account_Code == customer.Customer_Code).FirstOrDefault();
                statistic.customer = customer;
                var gldetail = db.GLDetails.Where(a => a.GLMaster.GL_Date < new DateTime(2250, 12, 5) && a.Account_Child_ID == Account.Accounts_Id).ToList();
                if (gldetail.Count() == 0)
                {

                    staticList.Add(statistic);
                    continue;
                }
                statistic.CurrentBalance = gldetail.Select(a => a.GLDetail_Debit).Sum() - gldetail.Select(a => a.GLDetail_Credit).Sum();
                var bfore30date = date.AddDays(-30);

                var gldetailOndatedate = gldetail.Where(a => a.GLMaster.GL_Date <= date);

                statistic.Datebalance = gldetailOndatedate.Select(a => a.GLDetail_Debit).Sum() - gldetailOndatedate.Select(a => a.GLDetail_Credit).Sum();
                var gldetailbeforedate = gldetail.Where(a => a.GLMaster.GL_Date <= bfore30date);
                if (gldetailbeforedate.Count() == 0)
                {

                    staticList.Add(statistic);
                    continue;

                }
                if (statistic.Datebalance <= 0)
                {


                    continue;

                }
                statistic.Date30Befor = statistic.Datebalance - (gldetailbeforedate.Select(a => a.GLDetail_Debit).Sum() - gldetailbeforedate.Select(a => a.GLDetail_Credit).Sum());



                var gldetailafter30 = gldetail.Where(a => a.GLMaster.GL_Date >= After30.Date && a.GLMaster.GL_Date <= dateminusEnd).ToList();
                var credit = gldetailafter30.Select(a => a.GLDetail_Debit).Sum() * -1;

                statistic.Date30After = (gldetailafter30.Select(a => a.GLDetail_Debit).Sum() * 1);
                if (statistic.Date30After > statistic.Datebalance)
                {
                    statistic.Date30After = statistic.Datebalance;
                    staticList.Add(statistic);
                    continue;

                }

                var gldetailafter60 = gldetail.Where(a => a.GLMaster.GL_Date >= After60.Date && a.GLMaster.GL_Date <= dateminus30End).ToList();

                statistic.Date60After = (gldetailafter60.Select(a => a.GLDetail_Debit).Sum());
                if (statistic.Date60After + statistic.Date30After > statistic.Datebalance)
                {
                    statistic.Date60After = statistic.Datebalance - statistic.Date30After;
                    staticList.Add(statistic);
                    continue;

                }


                var gldetailafter90 = gldetail.Where(a => a.GLMaster.GL_Date >= After90.Date && a.GLMaster.GL_Date <= dateminus60End).ToList();

                statistic.Date90After = (gldetailafter90.Select(a => a.GLDetail_Debit).Sum());
                if (statistic.Date60After + statistic.Date30After + statistic.Date90After > statistic.Datebalance)
                {
                    statistic.Date90After = statistic.Datebalance - (statistic.Date30After + statistic.Date60After);
                    staticList.Add(statistic);
                    continue;

                }



                if (statistic.Date60After + statistic.Date30After + statistic.Date90After + statistic.Date120After < statistic.Datebalance)
                {
                    statistic.Date120After = statistic.Datebalance - (statistic.Date30After + statistic.Date60After + statistic.Date90After);


                }

                staticList.Add(statistic);

            }
            if (havebalance == true)
            {

                staticList = staticList.Where(a => Math.Round(a.CurrentBalance, 2, MidpointRounding.AwayFromZero) != 0).ToList();
            }

            return staticList;
        }


        public static List<CustomerStatisticVM> OneCustomerStatistic(DateTime date, int cust)
        {
            var db = new AccountingContainer();

            List<CustomerStatisticVM> staticList = new List<CustomerStatisticVM>();
            DateTime After30 = date.AddDays(-30);
            DateTime After60 = date.AddDays(-60);
            DateTime After90 = date.AddDays(-90);



            var statistic = new CustomerStatisticVM();
            var customer = db.Customers.Find(cust);
            var Account = db.Accounts.Where(a => a.Account_Code == customer.Customer_Code).FirstOrDefault();
            statistic.customer = customer;
            var gldetail = db.GLDetails.Where(a => a.GLMaster.GL_Date < new DateTime(2250, 12, 5) && a.Account_Child_ID == Account.Accounts_Id);
            if (gldetail.Count() == 0)
            {

                staticList.Add(statistic);
                return staticList;

            }
            statistic.CurrentBalance = gldetail.Select(a => a.GLDetail_Debit).Sum() - gldetail.Select(a => a.GLDetail_Credit).Sum();
            var bfore30date = date.AddDays(-30);
            var gldetailbeforedate = gldetail.Where(a => a.GLMaster.GL_Date <= bfore30date);

            var gldetailOndatedate = gldetail.Where(a => a.GLMaster.GL_Date <= date);

            statistic.Datebalance = gldetailOndatedate.Select(a => a.GLDetail_Debit).Sum() - gldetailOndatedate.Select(a => a.GLDetail_Credit).Sum();
            if (gldetailbeforedate.Count() == 0)
            {

                staticList.Add(statistic);
                return staticList;

            }
            statistic.Date30Befor = statistic.Datebalance - (gldetailbeforedate.Select(a => a.GLDetail_Debit).Sum() - gldetailbeforedate.Select(a => a.GLDetail_Credit).Sum());



            var dateminus = date.Date.AddDays(-1);
            var dateminus30 = After30.Date.AddDays(-1);
            var dateminus60 = After60.Date.AddDays(-1);

            var dateminusEnd = date.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            var dateminus30End = After30.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            var dateminus60End = After60.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);


            var gldetailafter30 = gldetail.Where(a => a.GLMaster.GL_Date >= After30.Date && a.GLMaster.GL_Date <= dateminusEnd).ToList();
            var credit = gldetailafter30.Select(a => a.GLDetail_Debit).Sum() * -1;

            statistic.Date30After = (gldetailafter30.Select(a => a.GLDetail_Debit).Sum() * 1);
            if (statistic.Date30After > statistic.Datebalance)
            {
                statistic.Date30After = statistic.Datebalance;
                staticList.Add(statistic);
                return staticList;

            }

            var gldetailafter60 = gldetail.Where(a => a.GLMaster.GL_Date >= After60.Date && a.GLMaster.GL_Date <= dateminus30End).ToList();

            statistic.Date60After = (gldetailafter60.Select(a => a.GLDetail_Debit).Sum());
            if (statistic.Date60After + statistic.Date30After > statistic.Datebalance)
            {
                statistic.Date60After = statistic.Datebalance - statistic.Date30After;
                staticList.Add(statistic);
                return staticList;

            }


            var gldetailafter90 = gldetail.Where(a => a.GLMaster.GL_Date >= After90.Date && a.GLMaster.GL_Date <= dateminus60End).ToList();

            statistic.Date90After = (gldetailafter90.Select(a => a.GLDetail_Debit).Sum());
            if (statistic.Date60After + statistic.Date30After + statistic.Date90After > statistic.Datebalance)
            {
                statistic.Date90After = statistic.Datebalance - (statistic.Date30After + statistic.Date60After);
                staticList.Add(statistic);
                return staticList;

            }



            if (statistic.Date60After + statistic.Date30After + statistic.Date90After + statistic.Date120After < statistic.Datebalance)
            {
                statistic.Date120After = statistic.Datebalance - (statistic.Date30After + statistic.Date60After + statistic.Date90After);


            }

            staticList.Add(statistic);


            return staticList;
        }



        public static CustomerStatisticListVM GetStatisticListSupplierVM(CustomerStatisticListVM vm)
        {
            var db = new AccountingContainer();
            DateTime startdate = new DateTime();
            if (vm.StartDate == "" || vm.StartDate == null)
            {
                startdate = Convert.ToDateTime(DateTime.Today).AddHours(23).AddMinutes(59);
            }
            else
            {
                startdate = Convert.ToDateTime(vm.StartDate).AddHours(23).AddMinutes(59);
            }

            vm.StartDate = Convert.ToString(startdate);
            vm.company = db.Companies.FirstOrDefault();
            vm.List = new List<CustomerStatisticVM>();
            if (vm.Supp_Id == null || vm.Supp_Id == 0)
            {
                vm.List = MoreSupplierStatistic(startdate, vm.haveBallance);
            }
            else
            {
                vm.List = OneSupplierStatistic(startdate, vm.Supp_Id.Value);
            }

            return vm;

        }
        public static List<CustomerStatisticVM> MoreSupplierStatistic(DateTime date, bool haveBalance)
        {
            var db = new AccountingContainer();
            List<CustomerStatisticVM> staticList = new List<CustomerStatisticVM>();
            DateTime After30 = date.AddDays(-30);
            DateTime After60 = date.AddDays(-60);
            DateTime After90 = date.AddDays(-90);
            var dateminus = date.Date.AddDays(-1);
            var dateminus30 = After30.Date.AddDays(-1);
            var dateminus60 = After60.Date.AddDays(-1);
            var dateminusEnd = date.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            var dateminus30End = After30.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            var dateminus60End = After60.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            foreach (var supplier in db.Suppliers)
            {
                var statistic = new CustomerStatisticVM();
                var Account = db.Accounts.Where(a => a.Account_Code == supplier.Supp_Code).FirstOrDefault();
                statistic.supplier = supplier;
                var gldetail = db.GLDetails.Where(a => a.GLMaster.GL_Date < new DateTime(2250, 12, 5) && a.Account_Child_ID == Account.Accounts_Id).ToList();
                if (gldetail.Count() == 0)
                {

                    staticList.Add(statistic);
                    continue;
                }
                statistic.CurrentBalance = gldetail.Select(a => a.GLDetail_Debit).Sum() - gldetail.Select(a => a.GLDetail_Credit).Sum();
                var bfore30date = date.AddDays(-30);
                var gldetailbeforedate = gldetail.Where(a => a.GLMaster.GL_Date <= bfore30date);
                var gldetailOndatedate = gldetail.Where(a => a.GLMaster.GL_Date <= date);

                statistic.Datebalance = gldetailOndatedate.Select(a => a.GLDetail_Debit).Sum() - gldetailOndatedate.Select(a => a.GLDetail_Credit).Sum();
                if (gldetailbeforedate.Count() == 0)
                {

                    staticList.Add(statistic);
                    continue;

                }


                if (statistic.Datebalance >= 0)
                {


                    continue;

                }
                statistic.Date30Befor = statistic.Datebalance - (gldetailbeforedate.Select(a => a.GLDetail_Debit).Sum() - gldetailbeforedate.Select(a => a.GLDetail_Credit).Sum());

                var gldetailafter30 = gldetail.Where(a => a.GLMaster.GL_Date >= After30.Date && a.GLMaster.GL_Date <= dateminusEnd).ToList();
                var credit = gldetailafter30.Select(a => a.GLDetail_Credit).Sum() * -1;

                statistic.Date30After = (gldetailafter30.Select(a => a.GLDetail_Credit).Sum() * -1);
                if (statistic.Date30After < statistic.Datebalance)
                {
                    statistic.Date30After = statistic.Datebalance;
                    staticList.Add(statistic);
                    continue;

                }

                var gldetailafter60 = gldetail.Where(a => a.GLMaster.GL_Date >= After60.Date && a.GLMaster.GL_Date <= dateminus30End).ToList();

                statistic.Date60After = (gldetailafter60.Select(a => a.GLDetail_Credit).Sum() * -1);
                if (statistic.Date60After + statistic.Date30After < statistic.Datebalance)
                {
                    statistic.Date60After = statistic.Datebalance - statistic.Date30After;
                    staticList.Add(statistic);
                    continue;

                }


                var gldetailafter90 = gldetail.Where(a => a.GLMaster.GL_Date >= After90.Date && a.GLMaster.GL_Date <= dateminus60End).ToList();

                statistic.Date90After = (gldetailafter90.Select(a => a.GLDetail_Credit).Sum() * -1);
                if (statistic.Date60After + statistic.Date30After + statistic.Date90After < statistic.Datebalance)
                {
                    statistic.Date90After = statistic.Datebalance - (statistic.Date30After + statistic.Date60After);
                    staticList.Add(statistic);
                    continue;

                }



                if (statistic.Date60After + statistic.Date30After + statistic.Date90After + statistic.Date120After > statistic.Datebalance)
                {
                    statistic.Date120After = statistic.Datebalance - (statistic.Date30After + statistic.Date60After + statistic.Date90After);


                }

                staticList.Add(statistic);

            }

            if (haveBalance == true)
            {

                staticList = staticList.Where(a => Math.Round(a.CurrentBalance, 2, MidpointRounding.AwayFromZero) != 0).ToList();
            }
            return staticList;
        }


        public static List<CustomerStatisticVM> OneSupplierStatistic(DateTime date, int supp)
        {
            var db = new AccountingContainer();
            DateTime After30 = date.AddDays(-30);
            DateTime After60 = date.AddDays(-60);
            DateTime After90 = date.AddDays(-90);


            var dateminusEnd = date.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            var dateminus30End = After30.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            var dateminus60End = After60.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);

            List<CustomerStatisticVM> staticList = new List<CustomerStatisticVM>();

            var statistic = new CustomerStatisticVM();
            var supplier = db.Suppliers.Find(supp);
            var Account = db.Accounts.Where(a => a.Account_Code == supplier.Supp_Code).FirstOrDefault();
            statistic.supplier = supplier;
            var gldetail = db.GLDetails.Where(a => a.GLMaster.GL_Date < new DateTime(2250, 12, 5) && a.Account_Child_ID == Account.Accounts_Id);
            if (gldetail.Count() == 0)
            {

                staticList.Add(statistic);
                return staticList;

            }
            statistic.CurrentBalance = gldetail.Select(a => a.GLDetail_Debit).Sum() - gldetail.Select(a => a.GLDetail_Credit).Sum();
            var bfore30date = date.AddDays(-30);
            var gldetailOndatedate = gldetail.Where(a => a.GLMaster.GL_Date <= date);

            statistic.Datebalance = gldetailOndatedate.Select(a => a.GLDetail_Debit).Sum() - gldetailOndatedate.Select(a => a.GLDetail_Credit).Sum();
            var gldetailbeforedate = gldetail.Where(a => a.GLMaster.GL_Date <= bfore30date);
            if (gldetailbeforedate.Count() == 0)
            {

                staticList.Add(statistic);
                return staticList;

            }




            statistic.Date30Befor = statistic.Datebalance - (gldetailbeforedate.Select(a => a.GLDetail_Debit).Sum() - gldetailbeforedate.Select(a => a.GLDetail_Credit).Sum());
            var dateminus = date.Date.AddDays(-1);
            var dateminus30 = After30.Date.AddDays(-1);
            var dateminus60 = After60.Date.AddDays(-1);
            var gldetailafter30 = gldetail.Where(a => a.GLMaster.GL_Date >= After30.Date && a.GLMaster.GL_Date <= dateminusEnd).ToList();
            var credit = gldetailafter30.Select(a => a.GLDetail_Credit).Sum() * -1;

            statistic.Date30After = (gldetailafter30.Select(a => a.GLDetail_Credit).Sum() * -1);
            if (statistic.Date30After < statistic.Datebalance)
            {
                statistic.Date30After = statistic.Datebalance;
                staticList.Add(statistic);
                return staticList;

            }

            var gldetailafter60 = gldetail.Where(a => a.GLMaster.GL_Date >= After60.Date && a.GLMaster.GL_Date <= dateminus30End).ToList();

            statistic.Date60After = (gldetailafter60.Select(a => a.GLDetail_Credit).Sum() * -1);
            if (statistic.Date60After + statistic.Date30After < statistic.Datebalance)
            {
                statistic.Date60After = statistic.Datebalance - statistic.Date30After;
                staticList.Add(statistic);
                return staticList;

            }


            var gldetailafter90 = gldetail.Where(a => a.GLMaster.GL_Date >= After90.Date && a.GLMaster.GL_Date <= dateminus60End).ToList();

            statistic.Date90After = (gldetailafter90.Select(a => a.GLDetail_Credit).Sum() * -1);
            if (statistic.Date60After + statistic.Date30After + statistic.Date90After < statistic.Datebalance)
            {
                statistic.Date90After = statistic.Datebalance - (statistic.Date30After + statistic.Date60After);
                staticList.Add(statistic);
                return staticList;

            }



            if (statistic.Date60After + statistic.Date30After + statistic.Date90After + statistic.Date120After > statistic.Datebalance)
            {
                statistic.Date120After = statistic.Datebalance - (statistic.Date30After + statistic.Date60After + statistic.Date90After);


            }

            staticList.Add(statistic);

            return staticList;
        }

        /// <summary>
        /// Print a transfer from store to maintanence
        /// </summary>
        /// <param name="SMVM"></param>
        /// <returns></returns>
        public static SubscriptionMaintainDateVM getSubscriptionTransfares(SubscriptionMaintainDateVM SMVM)
        {
            var db = new AccountingContainer();
            var subscriptionTrans = new List<TransferToMaintenance>();
            SMVM.List = new List<SubscriptionTransafareVM>();
            if (SMVM.From != null && SMVM.to != null)
            {
                DateTime fromdate = Convert.ToDateTime(SMVM.From).Date;
                DateTime Todate = Convert.ToDateTime(SMVM.to).AddHours(23).AddMinutes(59);
                subscriptionTrans = db.TransferToMaintenances.Where(e => e.Trans_Date >= fromdate && e.Trans_Date <= Todate && e.Invoice_State == TransactionState.Post).ToList();
            }
            else
            {
                subscriptionTrans = db.TransferToMaintenances.Where(e => e.Invoice_State == TransactionState.Post).ToList();
            }
            if (subscriptionTrans.Count > 0)
            {
                foreach (var trans in subscriptionTrans)
                {
                    SubscriptionTransafareVM STVM = new SubscriptionTransafareVM();
                    STVM.amount = trans.Total_Cost;
                    var thisaccount = db.ChartOfAccountings.Find(trans.Account_Id);
                    STVM.PayName = thisaccount.Account_Name;
                    var thisstore = db.Stores.Find(trans.Store_ID);
                    STVM.StoreName = thisstore.Store_Name;
                    var thissubledger = db.SubLedgers.Find(trans.Subledger_ID);
                    if (thissubledger != null)
                    {
                        STVM.SubledgerName = thissubledger.SubLed_Name;
                    }
                    STVM.TransCode = trans.Trans_Code;
                    STVM.company = db.Companies.FirstOrDefault();
                    SMVM.List.Add(STVM);
                }

            }
            else
            {
                SubscriptionTransafareVM STVM = new SubscriptionTransafareVM();
                STVM.company = db.Companies.FirstOrDefault();
                SMVM.List.Add(STVM);
            }
            return SMVM;
        }

        /// <summary>
        /// To print all subscription invoice
        /// </summary>
        /// <param name="SMVM"></param>
        /// <returns></returns>
        public static SubscriptionMaintainDateVM getSubscriptionInvoices(SubscriptionMaintainDateVM SMVM)
        {
            var db = new AccountingContainer();
            var subscriptionTrans = new List<Subscription>();
            SMVM.SubList = new List<SubscriptionInvoiceReportVM>();
            if (SMVM.From != null && SMVM.to != null)
            {
                var fromdate = Convert.ToDateTime(SMVM.From).Date;
                var todate = Convert.ToDateTime(SMVM.to).AddHours(23).AddMinutes(59);
                subscriptionTrans = db.Subscriptions.Where(e => e.Subscrip_State == TransactionState.Post && e.StartTime >= fromdate && e.StartTime <= todate).ToList();
            }
            else
            {
                subscriptionTrans = db.Subscriptions.Where(e => e.Subscrip_State == TransactionState.Post).ToList();
            }
            if (subscriptionTrans.Count > 0)
            {
                foreach (var sub in subscriptionTrans)
                {
                    decimal discountamount = 0;
                    SubscriptionInvoiceReportVM SIVM = new SubscriptionInvoiceReportVM();
                    SIVM.company = db.Companies.FirstOrDefault();
                    var thiscustomer = db.Customers.Find(sub.Customer_Id);
                    SIVM.CustomerCode = thiscustomer.Customer_Code;
                    SIVM.CustomerNamer = sub.Customer_Name;
                    SIVM.EndDate = sub.DeadLine.Day + "-" + sub.DeadLine.Month + "-" + sub.DeadLine.Year;
                    SIVM.StartDate = sub.StartTime.Day + "-" + sub.StartTime.Month + "-" + sub.StartTime.Year;
                    SIVM.SubCode = sub.Subscrip_Code;

                    SIVM.EstimatedAmount = sub.Estimate_Amount;
                    if (sub.Discount > 0)
                    {
                        discountamount = sub.Estimate_Amount * ((decimal)sub.Discount / 100);
                        SIVM.AfterDiscount = SIVM.EstimatedAmount - discountamount;
                    }
                    else
                    {
                        SIVM.AfterDiscount = SIVM.EstimatedAmount;
                    }
                    if (sub.Tax_ID > 0)
                    {
                        var tax = db.Taxes.Find(sub.Tax_ID);
                        decimal totaltax = SIVM.AfterDiscount * (tax.Tax_Percentage / 100);
                        SIVM.TaxAMount = totaltax;
                    }
                    else
                    {
                        SIVM.TaxAMount = 0;
                    }
                    SIVM.TotalAfterTax = sub.AmountAfterTax;

                    SMVM.SubList.Add(SIVM);

                }
            }
            else
            {
                SubscriptionInvoiceReportVM SIVM = new SubscriptionInvoiceReportVM();
                SIVM.company = db.Companies.FirstOrDefault();
                SMVM.SubList.Add(SIVM);
            }
            return SMVM;

        }

        /// <summary>
        /// Creation for subscription by employee
        /// </summary>
        /// <returns></returns>
        public static SubscriptionMaintainDateVM SubscriptionByEmployeeForCreation()
        {
            var db = new AccountingContainer();
            var thisclassify = new SubscriptionMaintainDateVM();

            thisclassify.employeeList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            return thisclassify;
        }

        /// <summary>
        /// Subscription by employee report
        /// </summary>
        /// <param name="SMVM"></param>
        /// <returns></returns>
        public static SubscriptionMaintainDateVM getSubscriptionInvoicesForEmployee(SubscriptionMaintainDateVM SMVM)
        {
            var db = new AccountingContainer();
            var subscriptionTrans = new List<Subscription>();
            SMVM.SubList = new List<SubscriptionInvoiceReportVM>();
            if (SMVM.From != null && SMVM.to != null && (SMVM.employeeId != 0 || SMVM.employeeId != null))
            {
                var fromdate = Convert.ToDateTime(SMVM.From).Date;
                var todate = Convert.ToDateTime(SMVM.to).AddHours(23).AddMinutes(59);
                subscriptionTrans = db.Subscriptions.Where(e => e.Subscrip_State == TransactionState.Post && e.StartTime >= fromdate && e.StartTime <= todate && e.Emp_Id == SMVM.employeeId).ToList();
            }
            else if (SMVM.From == null && SMVM.to == null && (SMVM.employeeId != 0 || SMVM.employeeId != null))
            {
                subscriptionTrans = db.Subscriptions.Where(e => e.Subscrip_State == TransactionState.Post && e.Emp_Id == SMVM.employeeId).ToList();
            }
            else
            {
                subscriptionTrans = db.Subscriptions.Where(e => e.Subscrip_State == TransactionState.Post).ToList();

            }
            if (subscriptionTrans.Count > 0)
            {
                foreach (var sub in subscriptionTrans)
                {
                    decimal discountamount = 0;
                    SubscriptionInvoiceReportVM SIVM = new SubscriptionInvoiceReportVM();
                    SIVM.company = db.Companies.FirstOrDefault();
                    var thiscustomer = db.Customers.Find(sub.Customer_Id);
                    SIVM.CustomerCode = thiscustomer.Customer_Code;
                    SIVM.CustomerNamer = sub.Customer_Name;
                    SIVM.EndDate = sub.DeadLine.Day + "-" + sub.DeadLine.Month + "-" + sub.DeadLine.Year;
                    SIVM.StartDate = sub.StartTime.Day + "-" + sub.StartTime.Month + "-" + sub.StartTime.Year;
                    SIVM.SubCode = sub.Subscrip_Code;
                    SIVM.empId = SMVM.employeeId;
                    SIVM.EstimatedAmount = sub.Estimate_Amount;
                    if (sub.Discount > 0)
                    {
                        discountamount = sub.Estimate_Amount * ((decimal)sub.Discount / 100);
                        SIVM.AfterDiscount = SIVM.EstimatedAmount - discountamount;
                    }
                    else
                    {
                        SIVM.AfterDiscount = SIVM.EstimatedAmount;
                    }
                    if (sub.Tax_ID > 0)
                    {
                        var tax = db.Taxes.Find(sub.Tax_ID);
                        decimal totaltax = SIVM.AfterDiscount * (tax.Tax_Percentage / 100);
                        SIVM.TaxAMount = totaltax;
                    }
                    else
                    {
                        SIVM.TaxAMount = 0;
                    }
                    SIVM.TotalAfterTax = sub.AmountAfterTax;

                    SMVM.SubList.Add(SIVM);

                }
            }
            else
            {
                SubscriptionInvoiceReportVM SIVM = new SubscriptionInvoiceReportVM();
                SIVM.company = db.Companies.FirstOrDefault();
                SIVM.empId = SMVM.employeeId;
                SMVM.SubList.Add(SIVM);
            }
            return SMVM;

        }

        /// <summary>
        /// Report to get tax balance
        /// </summary>
        /// <param name="taxID"></param>
        /// <returns></returns>

        public static List<TaxReportInAllVM> getTaxInfo(int taxID)
        {
            var db = new AccountingContainer();
            List<TaxReportInAllVM> infoofTax = new List<TaxReportInAllVM>();
            List<GLMaster> GLmasterList = new List<GLMaster>();
            var thistax = db.Taxes.Find(taxID);
            var thistaxincharofaccounting = db.ChartOfAccountings.Where(e => e.Account_Code == thistax.Tax_Code).FirstOrDefault();
            var thisGLDetails = db.GLDetails.Where(e => e.Account_Id == thistaxincharofaccounting.Account_Id).ToList();
            var company = db.Companies.ToList().FirstOrDefault();
            var thistaxs = db.Taxes.Find(taxID);
            if (thisGLDetails.Count > 0)
            {


                if (thistax.Tax_Code.StartsWith("020104001"))
                {




                    foreach (var detail in thisGLDetails)
                    {
                        TaxReportInAllVM TRIVM = new TaxReportInAllVM();
                        TRIVM.R_Amount = 0;
                        TRIVM.S_Amount = 0;
                        TRIVM.company = company;

                        TRIVM.TransactionCode = detail.GLMaster.GLCode;
                        if (detail.Account_Child_ID > 0)
                        {
                            TRIVM.CHildAccountName = detail.Account_CHild_Name;
                            var thisaccount = db.Accounts.Find(detail.Account_Child_ID);
                            TRIVM.CHildAccountCode = thisaccount.Account_Code;
                        }
                        TRIVM.dateofTransaction = detail.GLMaster.GL_Date.Day + "-" + detail.GLMaster.GL_Date.Month + "-" + detail.GLMaster.GL_Date.Year;
                        if (detail.Account_Id == thistaxincharofaccounting.Account_Id)
                        {
                            if (detail.GLDetail_Debit > 0)
                            {
                                TRIVM.S_Amount = TRIVM.S_Amount + 0;
                                TRIVM.R_Amount = TRIVM.R_Amount + detail.GLDetail_Debit;
                            }
                            else
                            {
                                TRIVM.S_Amount = TRIVM.S_Amount + detail.GLDetail_Credit;
                                TRIVM.R_Amount = TRIVM.R_Amount + 0;
                            }

                        }
                        TRIVM.Total = TRIVM.S_Amount - TRIVM.R_Amount;

                        TRIVM.taxName = thistaxs.Tax_Name;
                        infoofTax.Add(TRIVM);
                    }



                }
                else if (thistax.Tax_Code.StartsWith("010108001"))
                {


                    foreach (var detail in thisGLDetails)
                    {
                        TaxReportInAllVM TRIVM = new TaxReportInAllVM();
                        TRIVM.R_Amount = 0;
                        TRIVM.S_Amount = 0;
                        TRIVM.company = company;
                        TRIVM.TransactionCode = detail.GLMaster.GLCode;

                        if (detail.Account_Child_ID > 0)
                        {
                            TRIVM.CHildAccountName = detail.Account_CHild_Name;
                            var thisaccount = db.Accounts.Find(detail.Account_Child_ID);
                            TRIVM.CHildAccountCode = thisaccount.Account_Code;
                        }
                        TRIVM.dateofTransaction = detail.GLMaster.GL_Date.Day + "-" + detail.GLMaster.GL_Date.Month + "-" + detail.GLMaster.GL_Date.Year;
                        if (detail.Account_Id == thistaxincharofaccounting.Account_Id)
                        {
                            if (detail.GLDetail_Debit > 0)
                            {
                                TRIVM.S_Amount = TRIVM.S_Amount + 0;
                                TRIVM.R_Amount = TRIVM.R_Amount + detail.GLDetail_Debit;
                            }
                            else
                            {
                                TRIVM.S_Amount = TRIVM.S_Amount + detail.GLDetail_Credit;
                                TRIVM.R_Amount = TRIVM.R_Amount + 0;
                            }

                        }
                        TRIVM.Total = TRIVM.S_Amount - TRIVM.R_Amount;

                        TRIVM.taxName = thistaxs.Tax_Name;
                        infoofTax.Add(TRIVM);

                    }



                }

            }
            else
            {
                TaxReportInAllVM TRVM = new TaxReportInAllVM();

                TRVM.taxName = thistaxs.Tax_Name;
                TRVM.company = company;
                infoofTax.Add(TRVM);
            }
            return infoofTax;

        }



        /// <summary>
        /// this part belong HR partiation
        /// </summary>
        /// <returns></returns>
        public static EmployeeList GetAllEmployeeDateWork()
        {
            var db = new AccountingContainer();
            var EmpList = new EmployeeList();
            EmpList.EmpList = db.Employees.ToList();
            EmpList.company = db.Companies.FirstOrDefault();
            return EmpList;


        }

        public static EmployeeList HRNotification()
        {
            var db = new AccountingContainer();
            var EmpList = new EmployeeList();
            EmpList.EmpList = new List<Employee>();
           
            foreach (var emp in db.Employees.Where(a => (a.End_medical_Insurance != null|| a.End_medical_Insurance != null) && a.Active != false))
            {
                var End_Date_Residency = false;
                var End_medical_Insurance = false;
                if (emp.End_Date_Residency != null)
                {
                    End_Date_Residency=CheckDateIsSuccessForRidence(Convert.ToDateTime(emp.End_Date_Residency));
                }
               if(emp.End_medical_Insurance != null)
                {
                    End_medical_Insurance=CheckDateIsSuccessForHealth(Convert.ToDateTime(emp.End_medical_Insurance));
                }
               if(End_Date_Residency==true|| End_medical_Insurance == true)
                {
                    EmpList.EmpList.Add(emp);

                }
            }

            EmpList.company = db.Companies.FirstOrDefault();
            return EmpList;


        }

        public static bool  CheckDateIsSuccessForHealth (DateTime TimeDate)
        {
            var db = new AccountingContainer();

            var HRSetting = db.HRNotifications.FirstOrDefault();
         
            
                var date = DateTime.Now.AddDays(HRSetting.NoOfhealtyInsurance);
                var msg = false;
            
                    if (date >= TimeDate)
                    {

                        msg =true;


                    }

             
                if (msg ==true)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }

        public static bool CheckDateIsSuccessForRidence(DateTime TimeDate)
        {
            var db = new AccountingContainer();

            var HRSetting = db.HRNotifications.FirstOrDefault();


            var date = DateTime.Now.AddDays(HRSetting.NoOfResidence);
            var msg = false;

            if (date >= TimeDate)
            {

                msg = true;


            }


            if (msg == true)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        /// <summary>
        /// Point Of sale Report
        /// </summary>

        public static POSListVM getAllPOSInvoiceMasterGet()
        {
            var db = new AccountingContainer();
            var POS = new POSListVM();

            POS.POSList = new SelectList(db.PointOfSales, "POS_Id", "POS_Name");
            return POS;


        }
        /// <summary>
        /// Report for all post POS invoice
        /// </summary>
        /// <param name="pOSVM"></param>
        /// <returns></returns>
        public static POSListVM allPosInvoiceMaster(POSListVM pOSVM)
        {
            var db = new AccountingContainer();
            if (pOSVM.StartDate == null)
            {
                pOSVM.StartDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (pOSVM.End_Date == null)
            {
                pOSVM.End_Date = Convert.ToString(new DateTime(2200, 12, 30));

            }
            var star = Convert.ToDateTime(Convert.ToDateTime(pOSVM.StartDate).ToShortDateString());
            var End = Convert.ToDateTime(pOSVM.End_Date).AddHours(23).AddMinutes(59).AddSeconds(59);

            var detilPOSInvoice = db.MasterPOSInvoices.Where(e => e.Invoice_Date >= star && e.Invoice_Date <= End && e.Invoice_State == TransactionState.Post).ToList();
            if (pOSVM.Pos_Id != null && pOSVM.Pos_Id != 0)
            {
                detilPOSInvoice = detilPOSInvoice.Where(a => a.PointOfSalePOS_Id == pOSVM.Pos_Id).ToList();


            }
            pOSVM.AllInvoice = detilPOSInvoice;
            pOSVM.company = db.Companies.FirstOrDefault();

            return pOSVM;

        }
        /// <summary>
        /// Crreation of get all revers of POS invoices
        /// </summary>
        /// <returns></returns>
        public static ReversePOSListVM getAllRevesePOSInvoiceMasterGet()
        {
            var db = new AccountingContainer();
            var POS = new ReversePOSListVM();

            POS.POSList = new SelectList(db.PointOfSales, "POS_Id", "POS_Name");
            return POS;


        }
        /// <summary>
        /// Report of get all revers of POS invoices 
        /// </summary>
        /// <param name="pOSVM"></param>
        /// <returns></returns>
        public static ReversePOSListVM allReversePosInvoiceMaster(ReversePOSListVM pOSVM)
        {
            var db = new AccountingContainer();
            if (pOSVM.StartDate == null)
            {
                pOSVM.StartDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (pOSVM.End_Date == null)
            {
                pOSVM.End_Date = Convert.ToString(new DateTime(2200, 12, 30));

            }
            var star = Convert.ToDateTime(Convert.ToDateTime(pOSVM.StartDate).ToShortDateString());
            var End = Convert.ToDateTime(pOSVM.End_Date).AddHours(23).AddMinutes(59).AddSeconds(59);

            var detilPOSInvoice = db.ReversePOSInvoices.Where(e => e.Invoice_Date >= star && e.Invoice_Date <= End && e.Invoice_State == TransactionState.Post).ToList();
            if (pOSVM.Pos_Id != null && pOSVM.Pos_Id != 0)
            {
                detilPOSInvoice = detilPOSInvoice.Where(a => a.POS_ID == pOSVM.Pos_Id).ToList();


            }
            pOSVM.AllInvoice = detilPOSInvoice;
            pOSVM.company = db.Companies.FirstOrDefault();

            return pOSVM;

        }


        /////Report General
        /// <summary>
        /// creation of report balance of subldger
        /// </summary>
        /// <returns></returns>
        public static SubledgerVM getDataForMAkeReportOfSubledger(int ? subId)
        {

            var sub = new SubledgerVM();
            var db = new AccountingContainer();

            var newList = from subl in db.SubLedgers.Where(a=>a.isActive!=false)
                          select new
                          {

                              Id = subl.SubLed_Id,
                              Name = subl.SubLed_Name + "        " + subl.SubLed_Code
                          };


            sub.SubList = new SelectList(newList, "Id", "Name");
            if (subId != null && subId > 0)
            {
                sub.SubLedger_Id = subId.Value;
            }
            return sub;



        }
        public static SubledgerVM makeReportForSubledger(SubledgerVM subVM)
        {
            var db = new AccountingContainer();
            subVM.company = db.Companies.FirstOrDefault();
            subVM.subledger = db.SubLedgers.Find(subVM.SubLedger_Id);

            if (subVM.From_Date == null)
            {
                subVM.From_Date = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (subVM.To_Date == null)
            {
                subVM.To_Date = Convert.ToString(new DateTime(2200, 12, 30));

            }
            var star = Convert.ToDateTime(subVM.From_Date)/*.AddHours(23).AddMinutes(59).AddSeconds(59)*/;
            var End = Convert.ToDateTime(subVM.To_Date).AddHours(23).AddMinutes(59).AddSeconds(59);
            var detilGlList = db.GLDetails.Where(e => e.GLMaster.GL_Date >= star && e.GLMaster.GL_Date <= End && e.SubLedger_Id != 0 && e.SubLedger_Id != null).ToList();
            var code = db.SubLedgers.Find(subVM.SubLedger_Id).SubLed_Code;
            var SubLedger = db.SubLedgers.Where(a => a.SubLed_Code.StartsWith(code)).ToList();
            var List = new List<SubLedgerMovList> { };
            List.Add(getTheBalanceOfDateBeforeForSubLedger(SubLedger, star));
            foreach (var item in SubLedger)
            {
                var gl = getallMovmentWithSubledger(detilGlList, item.SubLed_Id);
                List.AddRange(gl);

            }

            subVM.DetailList = List.OrderBy(a => a.GLdate).ToList();
            return subVM;
        }
        public static SubLedgerMovList getTheBalanceOfDateBeforeForSubLedger(List<SubLedger> subledger, DateTime date)
        {
            var db = new AccountingContainer();
            SubLedgerMovList vm = new SubLedgerMovList();
            vm.Account_Name = "رصيد اول المدة";

            decimal totalCredit = 0;
            decimal totalDepit = 0;
            foreach (var sub in subledger)
            {
                var glDetail = db.GLDetails.Where(a => a.SubLedger_Id == sub.SubLed_Id && a.GLMaster.GL_Date < date).ToList();
                if (glDetail.Count() > 0)
                {
                    totalDepit = totalDepit + glDetail.Select(a => a.GLDetail_Debit).Sum();
                    totalCredit = totalCredit + glDetail.Select(a => a.GLDetail_Credit).Sum();



                }
            }

            vm.Credit = totalCredit;
            vm.Depite = totalDepit;
            vm.Amount = vm.Depite - vm.Credit;

            return vm;
        }
        public static List<SubLedgerMovList> getallMovmentWithSubledger(List<GLDetail> detailList, int SubLedger_Id)
        {

            var db = new AccountingContainer();
            var SubList = new List<SubLedgerMovList> { };


            var details = detailList.Where(a => a.SubLedger_Id == SubLedger_Id).ToList();
            if (details.FirstOrDefault() == null)
            {

            }
            else
            {
                var count = 1;
                SubList = (from detail in details
                           join Account in db.Accounts on detail.Account_Child_ID equals Account.Accounts_Id into AccountList
                           select new SubLedgerMovList
                           {
                               Account = detail.ChartOfAccounting,
                               IsRevenue = detail.ChartOfAccounting.Account_Code.StartsWith("04") ? true : false,
                               Account_Name = detail.ChartOfAccounting.Account_Name,
                               Child_Name = AccountList.Count() > 0 ? AccountList.FirstOrDefault().Account_Name : null,
                               GLdate = detail.GLMaster.GL_Date,

                               serialCode = detail.GLMaster.GLCode,
                               Description = detail.Description_Detail!=null ? detail.Description_Detail:detail.GLMaster.GL_Desc,
                               Credit = detail.GLDetail_Credit,
                               Depite = detail.GLDetail_Debit,
                               Amount = detail.GLDetail_Debit - detail.GLDetail_Credit,
                               GL_Type = detail.GLMaster.GLType_Id





                           }).ToList();


                //foreach (var detail in details)
                //{
                //    var MovList = new SubLedgerMovList();
                //    var Account = db.ChartOfAccountings.Find(detail.Account_Id);
                //    MovList.Account = Account;
                //    if (Account.Account_Code.StartsWith("04"))
                //    {
                //        MovList.IsRevenue = true;
                //    }
                //    else
                //    {

                //        MovList.IsRevenue = false;

                //    }
                //    MovList.Account_Name = db.ChartOfAccountings.Find(detail.Account_Id).Account_Name;
                //    if (detail.Account_Child_ID != 0 && detail.Account_Child_ID != null)
                //    {
                //        MovList.Child_Name = db.Accounts.Find(detail.Account_Child_ID).Account_Name;

                //    }
                //    else
                //    {
                //        MovList.Child_Name = "";
                //    }
                //    MovList.GLdate = detail.GLMaster.GL_Date;
                //    MovList.Count = count;
                //    MovList.serialCode = detail.GLMaster.GLCode;
                //    MovList.Description = detail.GLMaster.GL_Desc;
                //    MovList.Credit = detail.GLDetail_Credit;
                //    MovList.Depite = detail.GLDetail_Debit;
                //    MovList.Amount = detail.GLDetail_Debit - detail.GLDetail_Credit;
                //    MovList.GL_Type = detail.GLMaster.GLType_Id;
                //    SubList.Add(MovList);
                //}
                count++;

            }


            return SubList;
        }

        public static ProjectLossAndProfitVM getDataForReportForProjectProAndLoss()
        {
            var db = new AccountingContainer();

            var sub = new ProjectLossAndProfitVM();

            List<object> newList = new List<object>();
            var members1 = db.SubLedgers.Where(a => a.SubLed_Type == "مشروع").ToList();
            foreach (var member in members1)
                newList.Add(new
                {
                    Id = member.SubLed_Id,
                    Name = member.SubLed_Name
                });

            sub.SubList = new SelectList(newList, "Id", "Name");

            return sub;



        }
        public static ProjectLossAndProfitVM makeReportForProjectLoss(ProjectLossAndProfitVM subVM)
        {
            var db = new AccountingContainer();
            subVM.company = db.Companies.FirstOrDefault();
            subVM.subledger = db.SubLedgers.Find(subVM.SubLedger_Id);
            var Project = db.Projects.Where(a => a.Project_Name == subVM.subledger.SubLed_Name).FirstOrDefault();
            var cust = db.Customers.Find(Project.CustomerCustomer_Id);
            subVM.Custmer_Code = cust.Customer_Code;
            subVM.Custmer_Name = cust.Customer_Name;
            subVM.Project_Code = Project.Project_Code;


            if (subVM.From_Date == null)
            {
                subVM.From_Date = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (subVM.To_Date == null)
            {
                subVM.To_Date = Convert.ToString(new DateTime(2200, 12, 30));

            }
            var star = Convert.ToDateTime(Convert.ToDateTime(subVM.From_Date).ToShortDateString());
            var End = Convert.ToDateTime(subVM.To_Date).AddHours(23).AddMinutes(59).AddSeconds(59);
            var detilGlList = db.GLDetails.Where(e => e.GLMaster.GL_Date >= star && e.GLMaster.GL_Date <= End && e.SubLedger_Id != 0 && e.SubLedger_Id != null).ToList();
            var gl = getallMovmentWithSubledger(detilGlList, subVM.SubLedger_Id);
            subVM.Revenue = 0;
            subVM.Expences = 0;
            foreach (var item in gl)
            {
                if (item.IsRevenue == true)
                {
                    subVM.Revenue += item.Amount;
                }
                else
                {
                    subVM.Expences += item.Amount;
                }



            }
            subVM.ProfitOrLoss = subVM.Revenue + subVM.Expences;
            return subVM;
        }

        /// <summary>
        /// Creation of calculate the expences of project
        /// </summary>
        /// <returns></returns>
        public static ProjectExpenceVM GetDataForgetAllDataForProject()
        {

            var db = new AccountingContainer();

            var sub = new ProjectExpenceVM();

            List<object> newList = new List<object>();
            var members1 = db.SubLedgers.Where(a => a.SubLed_Type == "مشروع").ToList();
            foreach (var member in members1)
                newList.Add(new
                {
                    Id = member.SubLed_Id,
                    Name = member.SubLed_Name + "        " + member.SubLed_Code
                });

            sub.SubList = new SelectList(newList, "Id", "Name");

            return sub;

        }

        /// <summary>
        /// Report for get the expences of project 
        /// </summary>
        /// <param name="subVM"></param>
        /// <returns></returns>
        public static ProjectExpenceVM makeReportOfExpencesProject(ProjectExpenceVM subVM)
        {
            var db = new AccountingContainer();
            subVM.company = db.Companies.FirstOrDefault();
            subVM.subledger = db.SubLedgers.Find(subVM.SubLedger_Id);

            if (subVM.From_Date == null)
            {
                subVM.From_Date = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (subVM.To_Date == null)
            {
                subVM.To_Date = Convert.ToString(new DateTime(2200, 12, 30));

            }
            var star = Convert.ToDateTime(Convert.ToDateTime(subVM.From_Date).ToShortDateString());
            var End = Convert.ToDateTime(subVM.To_Date).AddHours(23).AddMinutes(59).AddSeconds(59);

            subVM.startCredit= getThCreditBeforeForSubledger(subVM.SubLedger_Id, star,"05");
            subVM.startDepit = getThDepitBeforeForSubledger(subVM.SubLedger_Id, star,"05");

            var detilGlList = db.GLDetails.Where(e => e.GLMaster.GL_Date >= star && e.GLMaster.GL_Date <= End && e.SubLedger_Id != 0 && e.SubLedger_Id != null).ToList();
            var gl = getallMovmentWithSubledger(detilGlList, subVM.SubLedger_Id);
            subVM.DetailList = gl.Where(a => a.IsRevenue == false).ToList();
            return subVM;
        }

        /// <summary>
        /// creation of report of calculate the invoice and receipt of project
        /// </summary>
        /// <returns></returns>
        public static ProjectDetailsVM GetDataForgetAllDataForProjectReceipt()
        {

            var db = new AccountingContainer();

            var projectDetailsVM = new ProjectDetailsVM();

            List<object> newList = new List<object>();
            var members1 = db.Projects.ToList();
            foreach (var member in members1)
                newList.Add(new
                {
                    Id = member.Project_Id,
                    Name = member.Project_Name
                });

            projectDetailsVM.ProjectList = new SelectList(newList, "Id", "Name");

            return projectDetailsVM;

        }
        /// <summary>
        /// Report for calculate the invoice and receipt of project 
        /// </summary>
        /// <param name="projectDetailsVM"></param>
        /// <returns></returns>
        public static ProjectDetailsVM makeReportOfCatchReceiptProject(ProjectDetailsVM projectDetailsVM)
        {
            var db = new AccountingContainer();
            var prjectVM = new ProjectDetailsVM();
            prjectVM.ProjectDetailsGLVMs = new List<ProjectDetailsGLVM>();

            prjectVM.company = db.Companies.FirstOrDefault();
            var project = db.Projects.Find(projectDetailsVM.ProjectId);
            prjectVM.ProjectId = project.Project_Id;
            prjectVM.ProjectName = project.Project_Name;
            prjectVM.ProjectCode = project.Project_Code;
            prjectVM.ProjectAMount = project.Estimate_Amount;
            prjectVM.Statu = project.Project_State;
            prjectVM.ProjectStartDate = project.StartTime.ToString();
            prjectVM.ProjectEndDate = project.DeadLine.ToString();
            var customer = db.Customers.Find(project.CustomerCustomer_Id);
            prjectVM.CustomerCode = customer.Customer_Code;
            prjectVM.CustomerName = customer.Customer_Name;

            if (projectDetailsVM.From_Date == null)
            {
                projectDetailsVM.From_Date = Convert.ToString(new DateTime(1995, 1, 1));
            }

            if (projectDetailsVM.To_Date == null)
            {
                projectDetailsVM.To_Date = Convert.ToString(new DateTime(2200, 12, 30));

            }
            prjectVM.To_Date = projectDetailsVM.To_Date;
            prjectVM.From_Date = projectDetailsVM.From_Date;
            var star = Convert.ToDateTime(Convert.ToDateTime(projectDetailsVM.From_Date).ToShortDateString());
            var End = Convert.ToDateTime(projectDetailsVM.To_Date).AddHours(23).AddMinutes(59).AddSeconds(59);

            var projectDetails = db.ProjectDetails.Where(p => p.ProjectProject_Id == project.Project_Id).ToList();
            prjectVM.ReceivedAmount = 0;
            foreach (var item in projectDetails)
            {
                prjectVM.ReceivedAmount += item.Amount;
            }

            prjectVM.projectReciptsList = db.ProjectRecipts.Where(p => p.ProjectCode == project.Project_Code && p.Recipt_Date >= star && p.Recipt_Date <= End && p.transActionState == TransactionState.Post).ToList();
            prjectVM.projectInvoiceMastersList = db.ProjectInvoiceMasters.Where(p => p.Project_Code == project.Project_Code && p.ProjectInv_Date >= star && p.ProjectInv_Date <= End && p.Project_State == TransactionState.Post).ToList();


            foreach (var item in prjectVM.projectReciptsList)
            {
                decimal debit = 0;
                decimal credit = 0;
                var glMaster = db.GLMasters.Where(g => g.GLCode == item.ReciptCode).ToList();
                foreach (var master in glMaster)
                {
                    var _ProjectDetailsGLVM = new ProjectDetailsGLVM();
                    var glDetail = db.GLDetails.Where(g => g.GLMasterGLMaster_Id == master.GLMaster_Id).ToList();
                    foreach (var detail in glDetail)
                    {
                        debit += detail.GLDetail_Debit;
                        credit += detail.GLDetail_Credit;

                    }
                    _ProjectDetailsGLVM.Balance_Debit = 0;
                    _ProjectDetailsGLVM.Balance_Credit = credit * -1;
                    _ProjectDetailsGLVM.GlCode = master.GLCode;
                    _ProjectDetailsGLVM.GlDescrib = master.GL_Desc;
                    _ProjectDetailsGLVM.GlDate = master.GL_Date;
                    _ProjectDetailsGLVM.GlType = master.GLType_Id;

                    prjectVM.ProjectDetailsGLVMs.Add(_ProjectDetailsGLVM);
                }

            }

            foreach (var item in prjectVM.projectInvoiceMastersList)
            {
                decimal debit = 0;
                decimal credit = 0;
                var glMaster = db.GLMasters.Where(g => g.GLCode == item.Serial_Number).ToList();
                foreach (var master in glMaster)
                {
                    var _ProjectDetailsGLVM = new ProjectDetailsGLVM();
                    var glDetail = db.GLDetails.Where(g => g.GLMasterGLMaster_Id == master.GLMaster_Id && g.Account_Name == "العملاء").ToList();
                    foreach (var detail in glDetail)
                    {
                        debit += detail.GLDetail_Debit;
                        credit += detail.GLDetail_Credit;

                    }
                    _ProjectDetailsGLVM.Balance_Debit = debit;
                    _ProjectDetailsGLVM.Balance_Credit = 0;
                    _ProjectDetailsGLVM.balance = debit;
                    _ProjectDetailsGLVM.GlCode = master.GLCode;
                    _ProjectDetailsGLVM.GlDescrib = master.GL_Desc;
                    _ProjectDetailsGLVM.GlDate = master.GL_Date;
                    _ProjectDetailsGLVM.GlType = master.GLType_Id;

                    prjectVM.ProjectDetailsGLVMs.Add(_ProjectDetailsGLVM);
                }
            }
            var z = prjectVM.ProjectDetailsGLVMs.ToList().OrderBy(a => a.GlDate);
            prjectVM.ProjectDetailsGLVMs = new List<ProjectDetailsGLVM> { };
            prjectVM.ProjectDetailsGLVMs.AddRange(z);
            return prjectVM;
        }
        /// <summary>
        /// creation for report of all sales
        /// </summary>
        /// <returns></returns>
        public static SalesOfProductVM prepareReportforAllSales()
        {
            var vm = new SalesOfProductVM();
            var db = new AccountingContainer();
            vm.InvoiceMaster = true;
            vm.SalesReciptMaster = true;
            vm.POSMaster = true;
            vm.OrderMaster = true;
            vm.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.SubLed_Type == "فرع"&&a.isActive!=false), "SubLed_Id", "SubLed_Name");
  
            vm.StoreList = new SelectList(db.Stores.Where(a=>a.isActive!=false).ToList(), "Store_ID", "Store_Name");
          
            vm.EmpList = new SelectList(db.Employees.Where(a => a.Active != false).ToList(), "Emp_Id", "Emp_Name");

            return vm;
        }
        /// <summary>
        /// Product report for all sales
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public static SalesOfProductVM makeSalesProductReportForAllSales(SalesOfProductVM vm)
        {
            var db = new AccountingContainer();
            vm.company = db.Companies.FirstOrDefault();
            decimal totalSales = 0;
            decimal totalProfit = 0;
            if (vm.POSMaster == vm.InvoiceMaster == vm.SalesReciptMaster ==vm.OrderMaster == false)
            {
                return null;
            }
            if (vm.startDate == null)
            {
                vm.startDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (vm.EndDate == null)
            {
                vm.EndDate = Convert.ToString(new DateTime(2200, 12, 30));

            }
            if (String.IsNullOrEmpty(vm.TimeFrom))
            {
                vm.TimeFrom = Convert.ToString(new TimeSpan(0, 0, 0, 0));
            }
            if (String.IsNullOrEmpty(vm.TimeTo))
            {
                vm.TimeTo = Convert.ToString(new TimeSpan(0, 0, 0, 0));

            }
            var From = TimeSpan.Parse(vm.TimeFrom);
            var To = TimeSpan.Parse(vm.TimeTo);
            var star = Convert.ToDateTime(Convert.ToDateTime(vm.startDate).Add(From));
            var End = Convert.ToDateTime(vm.EndDate).Add(To);
            //var star = Convert.ToDateTime(Convert.ToDateTime(vm.startDate).ToShortDateString());
            //var End = Convert.ToDateTime(vm.EndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
            vm.DetailList = new List<SalesOfProductDetailVM> { };
            if (vm.InvoiceMaster == true)
            {
                var invoiceMaster = makeSalesProductReportForInvoiceMaster(star, End,vm.emp_Id,vm.Branch_Id,vm.Store_Id);
                totalSales = invoiceMaster.TotalOfSale;
                totalProfit = invoiceMaster.TotalOfNetProfit;
                vm.DetailList.AddRange(invoiceMaster.DetailList);

            }
            if (vm.SalesReciptMaster == true)
            {
                var salesRecipt = makeSalesProductReportForSalesRecipt(star, End, vm.emp_Id, vm.Branch_Id, vm.Store_Id);
                totalSales += salesRecipt.TotalOfSale;
                totalProfit += salesRecipt.TotalOfNetProfit;
                vm.DetailList.AddRange(salesRecipt.DetailList);

            }
            if (vm.OrderMaster == true)
            {
                var order = makeSalesProductReportForOrder(star, End, vm.emp_Id, vm.Branch_Id, vm.Store_Id);
                totalSales += order.TotalOfSale;
                totalProfit += order.TotalOfNetProfit;
                vm.DetailList.AddRange(order.DetailList);

            }

            if (vm.POSMaster == true)
            {
                var MasterPOS = makeSalesProductReportForPOSInvoice(star, End, vm.emp_Id, vm.Branch_Id, vm.Store_Id);
                totalSales += MasterPOS.TotalOfSale;
                totalProfit += MasterPOS.TotalOfNetProfit;
                vm.DetailList.AddRange(MasterPOS.DetailList);

            }
            var ReportOrignal = new SalesOfProductVM();
            var Detailest = new List<SalesOfProductDetailVM> { };

            foreach (var pro in db.Products.ToList())
            {
                var Orgdetail = new SalesOfProductDetailVM();
                Orgdetail.tatalSaledPrice = vm.DetailList.Where(a => a.product.Prod_ID == pro.Prod_ID).Sum(a => a.tatalSaledPrice);
                Orgdetail.quantitySaled = vm.DetailList.Where(a => a.product.Prod_ID == pro.Prod_ID).Sum(a => a.quantitySaled);
                if (pro.ProductCompounds.Count() > 0&& Orgdetail.quantitySaled>0)
                {
                    Orgdetail.TotalCostOfProduct = getAllCostOfComplexProduct(pro, Orgdetail.quantitySaled);
                    Orgdetail.netProfit= vm.DetailList.Where(a => a.product.Prod_ID == pro.Prod_ID).Sum(a => a.tatalSaledPrice)- Orgdetail.TotalCostOfProduct;
                }
                else
                {
                    Orgdetail.TotalCostOfProduct = vm.DetailList.Where(a => a.product.Prod_ID == pro.Prod_ID).Sum(a => a.TotalCostOfProduct);
                    Orgdetail.netProfit = vm.DetailList.Where(a => a.product.Prod_ID == pro.Prod_ID).Sum(a => a.netProfit);
                }
                
            
                if (Orgdetail.quantitySaled != 0)
                {
                    Orgdetail.ProductCost = Orgdetail.TotalCostOfProduct / Orgdetail.quantitySaled;
                }
                else
                {
                    Orgdetail.ProductCost = 0;
                }
                Orgdetail.product = pro;
                if (totalSales > 0)
                {
                    Orgdetail.PrecentageTotalPrice = (Orgdetail.tatalSaledPrice / totalSales) * 100;
                }
                else
                {
                    Orgdetail.PrecentageTotalPrice = 0;
                }
                if (Orgdetail.tatalSaledPrice == 0)
                {
                    Orgdetail.PrecentageNetProfit = 0;
                }
                else
                {
                    if (Orgdetail.TotalCostOfProduct == 0)
                    {
                        Orgdetail.TotalCostOfProduct = 1;
                    }
                    Orgdetail.PrecentageNetProfit = (Orgdetail.netProfit / Orgdetail.TotalCostOfProduct) * 100;
                }




                Detailest.Add(Orgdetail);
            }
            ReportOrignal.DetailList = Detailest.Where(a => a.tatalSaledPrice > 0).ToList();
            ReportOrignal.TotalOfNetProfit = totalProfit;
            ReportOrignal.TotalOfSale = totalSales;
            ReportOrignal.company = vm.company;
            ReportOrignal.startDate = star.Add(From).ToString();
            ReportOrignal.EndDate = End.Add(To).ToString();
            

            return ReportOrignal;

        }

        public static decimal getAllCostOfComplexProduct(Product pro,decimal quantity)
        {
            decimal TotalCost = 0;
            var db = new AccountingContainer();
            foreach(var product in pro.ProductCompounds)
            {
                var rod = db.Products.Find(product.Product_Id);
                try
                {
                    var z = (rod.ProductBatches.Select(a => a.Batch_Cost).Sum() / rod.ProductBatches.Select(a => a.Product_Quantity).Sum());
                    TotalCost = TotalCost + (product.Quantity * (rod.ProductBatches.Select(a => a.Batch_Cost).Sum() / rod.ProductBatches.Select(a => a.Product_Quantity).Sum()) * quantity);
                }
                catch
                {
                 
                }
            }

            return TotalCost;
        }
        public static SalesClassffied prepareClassifiySales()
        {
            var vm = new SalesClassffied();
            var db = new AccountingContainer();
            vm.InvoiceMaster = true;
            vm.SalesReciptMaster = true;
            vm.POSMaster = true;
            vm.OrderMaster = true;
            vm.Pendding = true;
            vm.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.SubLed_Type == "فرع"), "SubLed_Id", "SubLed_Name");

            vm.StoreList = new SelectList(db.Stores.ToList(), "Store_ID", "Store_Name");

            vm.EmpList = new SelectList(db.Employees.ToList(), "Emp_Id", "Emp_Name");
            var newList = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                           select new
                           {
                               Id = cust.Customer_Id,
                               Name = cust.Customer_Name + "        " + cust.Customer_Phone

                           }).ToList();
            vm.CustomerList = new SelectList(newList, "Id", "Name");
            return vm;
        }
        public static SalesClassffied prepareClassifiySalesPOST(SalesClassffied vm)
        {
            var db = new AccountingContainer();
            vm.company = db.Companies.FirstOrDefault();
            decimal totalSales = 0;
            decimal totalProfit = 0;
            if (vm.POSMaster ==false&& vm.InvoiceMaster ==false &&vm.SalesReciptMaster ==false &&vm.OrderMaster == false)
            {
                return null;
            }
            if (vm.startDate == null)
            {
                vm.startDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (vm.EndDate == null)
            {
                vm.EndDate = Convert.ToString(new DateTime(2200, 12, 30));

            }
            var star = Convert.ToDateTime(Convert.ToDateTime(vm.startDate).ToShortDateString());
            var End = Convert.ToDateTime(vm.EndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
            var CustomerList = db.Customers.ToList();
            vm.classvmList = new List<classifiyVM> { };
            var reverse = classfiyReverse(star, End, vm.emp_Id, vm.Branch_Id, vm.CustomerId, vm.Store_Id, vm.Pendding);
            if (vm.InvoiceMaster == true)
            {
                var invoiceMaster = classfiyInvoiceMaster(star, End, vm.emp_Id, vm.Branch_Id, vm.CustomerId, vm.Store_Id,vm.Pendding);

                var reverseMaste = reverse.Where(a => a.InvoiceNo.StartsWith("REVS-")).ToList();

                vm.classvmList.AddRange(reverseMaste);
                vm.classvmList.AddRange(invoiceMaster);
             

            }
            vm.SalesReciptMasterList = new List<SalesReceiptMaster> { };
            if (vm.SalesReciptMaster == true)
            {
                var salesRecipt = classfiySalesReceiptMaster(star, End, vm.emp_Id, vm.Branch_Id, vm.CustomerId, vm.Store_Id,vm.Pendding);
                var reverseMaste = reverse.Where(a => a.InvoiceNo.StartsWith("REVSa-")).ToList();

                vm.classvmList.AddRange(reverseMaste);
                vm.classvmList.AddRange(salesRecipt);

            }
            vm.OrderMasterList = new List<Order> { };
            if (vm.OrderMaster == true)
            {
                var order = classfiyOrder(star, End, vm.emp_Id, vm.Branch_Id, vm.CustomerId, vm.Store_Id,false);
                var reverseMaste = reverse.Where(a => a.InvoiceNo.StartsWith("RORD-")).ToList();

                vm.classvmList.AddRange(reverseMaste);
                vm.classvmList.AddRange(order);

            }
            vm.POSMasterList = new List<MasterPOSInvoice> { };
            if (vm.POSMaster == true)
            {
                var MasterPOS = classfiyMasterPOSInvoice(star, End, vm.emp_Id, vm.Branch_Id,vm.CustomerId, vm.Store_Id,false);
                var reverseMaste = reverse.Where(a => a.InvoiceNo.StartsWith("REVPOS-")).ToList();

                vm.classvmList.AddRange(reverseMaste);

                vm.classvmList.AddRange(MasterPOS);

            }
           
            vm.classvmList = vm.classvmList.OrderBy(a => a.InvoiceDate).ToList();
            return vm;

        }
        public static List<classifiyVM> classfiyReverse(DateTime start, DateTime end, int emp, int branchs, int Customer, int store, bool pendding)
        {
            var db = new AccountingContainer();
  
            var detilReverseList = db.ReverseQRInvoices.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= end && e.Invoice_State != TransactionState.Void).ToList();

            if (emp != 0)
            {
         
                detilReverseList = detilReverseList.Where(a => a.MasterQRInvoice.Employee_Id == emp).ToList();
            }
            if (branchs != 0)
            {
             
                detilReverseList = detilReverseList.Where(a => a.MasterQRInvoice.Branch_Id == branchs).ToList();
            }
            if (store != 0)
            {
               
                detilReverseList = detilReverseList.Where(a => a.Store_ID == store).ToList();
            }
            if (pendding != true)
            {
                
                detilReverseList = detilReverseList.Where(a => a.Invoice_State != TransactionState.Pending).ToList();
            }
            if (Customer > 0)
            {
                detilReverseList = detilReverseList.Where(a => a.Customer_Id == Customer).ToList();
            }

            

            var ReverseList = (from invoice in detilReverseList
                               join customer in db.Customers on invoice.Customer_Id equals customer.Customer_Id
                               join storecard in db.StoreCards.Where(q => q.IncreaseQuantity == true) on invoice.Serial_Number equals storecard.Document_Serial into StoreCard
                               join Detail in db.ReverseQRDetails on invoice.Master_Id equals Detail.ReverseQRInvoiceMaster_Id into Details
                               join branch in db.SubLedgers on invoice.MasterQRInvoice.Branch_Id equals branch.SubLed_Id into BranchList


                               select new classifiyVM
                               {
                                   customerCode = customer.Customer_Code,
                                   customerName = customer.Customer_Name,
                                   InvoiceNo = invoice.Serial_Number,
                                   InvoiceDate = invoice.Invoice_Date,
                                   Quantity = Details.Select(g => g.Product_Quantity).ToList().Sum() * -1,
                                   BranchName = BranchList.Count() > 0 ? BranchList.FirstOrDefault().SubLed_Name : null,
                                   paymentMethodName = "",
                                   totalAmount = invoice.Total_Amount * -1,

                                   totalTax = invoice.Total_Tax * -1,
                                   totalCost = StoreCard.ToList().Count() > 0 ? StoreCard.ToList().Select(t => t.Quantity * t.CostPrice).Sum() * -1 : 0,

                               }).ToList();
        
            return ReverseList;
        }
        public static List<classifiyVM> classfiyInvoiceMaster(DateTime start, DateTime end, int emp, int branchs,int Customer, int store,bool pendding)
        {
            var db = new AccountingContainer();
            var detilGlList = db.InvoiceMasters.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= end ).ToList();
            if (emp != 0)
            {
                detilGlList = detilGlList.Where(a => a.Emp_Id == emp).ToList();
            }
            if (branchs != 0)
            {
                detilGlList = detilGlList.Where(a => a.Branch_Id == branchs).ToList();
            }
            if (store != 0)
            {
                detilGlList = detilGlList.Where(a => a.Store_ID == store).ToList();
            }
            if (pendding != true)
            {
                detilGlList = detilGlList.Where(a => a.Invoice_State != TransactionState.Pending).ToList();
            }
            if (Customer >0)
            {
                detilGlList = detilGlList.Where(a => a.Customer_Id == Customer).ToList();
            }

            var InvoiceMasterList = (from invoice in detilGlList
                                     join customer in db.Customers on invoice.Customer_Id equals customer.Customer_Id
                                     join storecard in db.StoreCards.Where(q => q.IncreaseQuantity == false) on invoice.Serial_Number equals storecard.Document_Serial into StoreCard
                                     join Detail in db.InvoiceDetails on invoice.Master_Id equals Detail.InvoiceMaster_Id into Details
                                     join branch in db.SubLedgers on invoice.Branch_Id equals branch.SubLed_Id into BranchList

                                     join reverse in db.ReverseSalesInvoiceMasters on invoice.Master_Id equals reverse.InvoiceMaster_Id into ReversList
                                     select new classifiyVM
                                     {
                                         customerCode = customer.Customer_Code,
                                         CustomerTaxNumber=customer.Customer_TaxNumber,
                                         customerName = customer.Customer_Name,
                                         InvoiceNo = invoice.Serial_Number,
                                         InvoiceDate = invoice.Invoice_Date,
                                         Quantity = Details.Select(g => g.Product_Quantity).ToList().Sum(),
                                         BranchName = BranchList.Count() > 0 ? BranchList.FirstOrDefault().SubLed_Name : null,
                                         paymentMethodName = "",
                                         totalAmount = invoice.Total_Amount,
                                         Status=invoice.Invoice_State,
                                         totalReverseCheck = ReversList.ToList().Count() > 0 ? ReversList.ToList().Select(t => t.Total_Amount).Sum() : 0,
                                         totalTax = invoice.Total_Tax,
                                         totalCost = StoreCard.ToList().Count() > 0 ? StoreCard.ToList().Select(t => t.Quantity * t.CostPrice).Sum() : 0,
                                      

                                     }).ToList();
            var Remove = InvoiceMasterList.Where(a => a.Status == TransactionState.Void && a.totalReverseCheck == 0).ToList();
             InvoiceMasterList.Except(Remove);

            return InvoiceMasterList;
        }
        public static List<classifiyVM> classfiySalesReceiptMaster(DateTime start, DateTime end, int emp, int branch,int Customer, int store, bool pendding)
        {
            var db = new AccountingContainer();
            var detilGlList = db.SalesReceiptMasters.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= end).ToList();
            if (emp != 0)
            {
                detilGlList = detilGlList.Where(a => a.Emp_Id == emp).ToList();
            }
            if (branch != 0)
            {
                detilGlList = detilGlList.Where(a => a.Branch_Id == branch).ToList();
            }
            if (store != 0)
            {
                detilGlList = detilGlList.Where(a => a.Store_ID == store).ToList();
            }
            if (pendding != true)
            {
                detilGlList = detilGlList.Where(a => a.Invoice_State != TransactionState.Pending).ToList();
            }
            if (Customer > 0)
            {
                detilGlList = detilGlList.Where(a => a.Customer_Id == Customer).ToList();
            }

            var InvoiceMasterList = (from invoice in detilGlList
                                     join customer in db.Customers on invoice.Customer_Id equals customer.Customer_Id into CustomerList
                                     join storecard in db.StoreCards.Where(q => q.IncreaseQuantity == false) on invoice.Serial_Number equals storecard.Document_Serial into StoreCard
                                     join Detail in db.SalesReceiptDetails on invoice.Master_Id equals Detail.Master_Id into Details
                                     join branchs in db.SubLedgers on invoice.Branch_Id equals branchs.SubLed_Id into BranchList
                                     join payment in db.PaymentMethods on invoice.PaymentMethod_Id equals payment.PaymentMethod_Id 

                                     join reverse in db.ReverseSalesReceiptMasters on invoice.Master_Id equals reverse.SalesRecipt_Id into ReversList
                                     select new classifiyVM
                                     {
                                         customerCode = CustomerList.Count() > 0 ? CustomerList.FirstOrDefault().Customer_Code : null,
                                         customerName = CustomerList.Count() > 0 ? CustomerList.FirstOrDefault().Customer_Name : null,
                                         CustomerTaxNumber = CustomerList.Count() > 0 ? CustomerList.FirstOrDefault().Customer_TaxNumber : null,
                                         InvoiceNo = invoice.Serial_Number,
                                         InvoiceDate = invoice.Invoice_Date,
                                         Quantity = Details.Select(g => g.Product_Quantity).ToList().Sum(),
                                         BranchName = BranchList.Count() > 0 ? BranchList.FirstOrDefault().SubLed_Name : null,
                                         paymentMethodName = payment.Payment_Type,
                                         totalAmount = invoice.Total_Amount,
                                         Status = invoice.Invoice_State,
                                         totalReverseCheck = ReversList.ToList().Count() > 0 ? ReversList.ToList().Select(t => t.Total_Amount).Sum() : 0,
                                         totalTax = invoice.Total_Tax,
                                         totalCost = StoreCard.ToList().Count() > 0 ? StoreCard.ToList().Select(t => t.Quantity * t.CostPrice).Sum() : 0,
                                         
                                     }).ToList();

            var Remove = InvoiceMasterList.Where(a => a.Status == TransactionState.Void && a.totalReverseCheck == 0).ToList();
            InvoiceMasterList.Except(Remove);


            return InvoiceMasterList;
        }

        public static List<classifiyVM> classfiyMasterPOSInvoice(DateTime start, DateTime end, int emp, int branch,int Customer, int store, bool pendding)
        {
            var db = new AccountingContainer();
            var detilGlList = db.MasterPOSInvoices.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= end ).ToList();
            if (emp != 0)
            {
                detilGlList = detilGlList.Where(a => a.PointOfSale.Employee_Id == emp).ToList();
            }
            if (branch != 0)
            {
                detilGlList = detilGlList.Where(a => a.PointOfSale.BranchSubledger_Id == branch).ToList();
            }
            if (store != 0)
            {
                detilGlList = detilGlList.Where(a => a.PointOfSale.Store_Id == store).ToList();
            }
            if (Customer > 0)
            {
                detilGlList = detilGlList.Where(a => a.Customer_Id == Customer).ToList();
            }

            var paymentoList = db.PaymentMethods.ToList();
            var InvoiceMasterList = (from invoice in detilGlList
                                     join customer in db.Customers on invoice.Customer_Id equals customer.Customer_Id into CustomerList
                                     join storecard in db.StoreCards.Where(q => q.IncreaseQuantity == false) on invoice.Serial_Number equals storecard.Document_Serial into StoreCard
                                     join Detail in db.DetailPOSInvoices on invoice.MasterPOSInvoiceId equals Detail.MasterPOSInvoiceId into Details
                                     join branchs in db.SubLedgers on invoice.PointOfSale.BranchSubledger_Id equals branchs.SubLed_Id into BranchList
                                   
                                     join paymentPOS in db.PosPaymentMethods on invoice.MasterPOSInvoiceId equals paymentPOS.MasterPOSInvoiceId into paymentList
                                     join reverse in db.ReversePOSInvoices on invoice.MasterPOSInvoiceId equals reverse.MasterPOSInvoiceMasterPOSInvoiceId into ReversList
                                     select new classifiyVM
                                     {
                                         customerCode = CustomerList.Count() > 0 ? CustomerList.FirstOrDefault().Customer_Code : null,
                                         customerName = CustomerList.Count() > 0 ? CustomerList.FirstOrDefault().Customer_Name : null,
                                         CustomerTaxNumber = CustomerList.Count() > 0 ? CustomerList.FirstOrDefault().Customer_TaxNumber : null,
                                         InvoiceNo = invoice.Serial_Number,
                                         InvoiceDate = invoice.Invoice_Date.Value,
                                         Quantity = Details.Select(g => g.Product_Quantity).ToList().Sum(),
                                         BranchName = BranchList.Count() > 0 ? BranchList.FirstOrDefault().SubLed_Name : null,
                                         paymentMethodName = paymentList.Count() > 0 ? paymentoList.Where(w=>w.PaymentMethod_Id== paymentList.FirstOrDefault().PaymentMethod_Id).FirstOrDefault().Payment_Type : null,
                                         totalAmount = invoice.Total_Amount.Value,
                                         Status = invoice.Invoice_State.Value,
                                         totalReverseCheck = ReversList.ToList().Count() > 0 ? ReversList.ToList().Select(t => t.Total_Amount).Sum() : 0,

                                         totalTax = invoice.Total_Tax.Value,
                                         totalCost = StoreCard.ToList().Count() > 0 ? StoreCard.ToList().Select(t => t.Quantity * t.CostPrice).Sum() : 0,
                                         
                                     }).ToList();



            var Remove = InvoiceMasterList.Where(a => a.Status == TransactionState.Void && a.totalReverseCheck == 0).ToList();
            InvoiceMasterList.Except(Remove);


            return InvoiceMasterList;
        }

        public static List<classifiyVM> classfiyOrder(DateTime start, DateTime end, int emp, int branch,int Customer, int store, bool pendding)
        {
            var db = new AccountingContainer();
            var detilGlList = db.Orders.Where(e => e.OrderDate >= start && e.OrderDate <= end ).ToList();
            if (emp != 0)
            {
                detilGlList = detilGlList.Where(a => a.EmployeeID == emp).ToList();
            }
            if (branch != 0)
            {
                detilGlList = detilGlList.Where(a => a.Branch_Id == branch).ToList();
            }
            if (store != 0)
            {
                detilGlList = detilGlList.Where(a => a.StoreId == store).ToList();
            }
            if (Customer > 0)
            {
                detilGlList = detilGlList.Where(a => a.CustomerID == Customer).ToList();
            }


            var InvoiceMasterList = (from invoice in detilGlList
                                     join customer in db.Customers on invoice.CustomerID equals customer.Customer_Id into CustomerList
                                     join storecard in db.StoreCards.Where(q => q.IncreaseQuantity == false) on invoice.OrderNumber equals storecard.Document_Serial into StoreCard
                                     join Detail in db.OrderDetails on invoice.Id equals Detail.OrderId into Details
                                     join branchs in db.SubLedgers on invoice.Branch_Id equals branchs.SubLed_Id into BranchList
                                     join payment in db.PaymentMethods on invoice.PaymentMethodeID equals payment.PaymentMethod_Id

                                     join reverse in db.ReverseOrders on invoice.Id equals reverse.OrderId into ReversList
                                     select new classifiyVM
                                     {
                                         customerCode = CustomerList.Count() > 0 ? CustomerList.FirstOrDefault().Customer_Code : null,
                                         customerName = CustomerList.Count() > 0 ? CustomerList.FirstOrDefault().Customer_Name : null,
                                         CustomerTaxNumber = CustomerList.Count() > 0 ? CustomerList.FirstOrDefault().Customer_TaxNumber : null,
                                         InvoiceNo = invoice.OrderNumber,
                                         InvoiceDate = invoice.OrderDate,
                                         Quantity = Details.Select(g => g.Quantity).ToList().Sum(),
                                         BranchName = BranchList.Count() > 0 ? BranchList.FirstOrDefault().SubLed_Name : null,
                                         paymentMethodName = payment.Payment_Type,
                                         totalAmount = invoice.Total_Amount,
                                         Status = invoice.Invoice_State,
                                         totalReverseCheck = ReversList.ToList().Count() > 0 ? ReversList.ToList().Select(t => t.Total_Amount).Sum() : 0,

                                         totalTax = invoice.Total_Tax,
                                         totalCost = StoreCard.ToList().Count() > 0 ? StoreCard.ToList().Select(t => t.Quantity * t.CostPrice).Sum() : 0,
         
                                     }).ToList();


            var Remove = InvoiceMasterList.Where(a => a.Status == TransactionState.Void && a.totalReverseCheck == 0).ToList();
            InvoiceMasterList.Except(Remove);
            return InvoiceMasterList;
        }
        /// <summary>
        /// Sales report product for sales invoice
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static SalesOfProductVM makeSalesProductReportForInvoiceMaster(DateTime start, DateTime end,int emp,int branch,int store)
        {
            var db = new AccountingContainer();
            var detilGlList = db.InvoiceDetails.Where(e => e.InvoiceMaster.Invoice_Date >= start && e.InvoiceMaster.Invoice_Date <= end && e.InvoiceMaster.Invoice_State == TransactionState.Post).ToList();
            if (emp != 0)
            {
                detilGlList = detilGlList.Where(a => a.InvoiceMaster.Emp_Id == emp).ToList();
            }
            if (branch != 0)
            {
                detilGlList = detilGlList.Where(a => a.InvoiceMaster.Branch_Id == branch).ToList();
            }
            if (store != 0)
            {
                detilGlList = detilGlList.Where(a => a.InvoiceMaster.Store_ID == store).ToList();
            }




            SalesOfProductVM vm = new SalesOfProductVM();
            vm.DetailList = new List<SalesOfProductDetailVM>();
            var ProList = db.Products.ToList();
            foreach (var pro in ProList)
            {
                SalesOfProductDetailVM detail = new SalesOfProductDetailVM();
                detail.product = pro;
                detail.quantitySaled = detilGlList.Where(a => a.Product_ID == pro.Prod_ID).Sum(a => a.Reverse_Quantity);
                if (detail.quantitySaled != 0)
                {
                    detail.tatalSaledPrice = detilGlList.Where(a => a.Product_ID == pro.Prod_ID).Sum(a => a.Reverse_Quantity * a.Product_Price);
                    vm.TotalOfSale += detail.tatalSaledPrice;
                    detail.TotalCostOfProduct = detilGlList.Where(a => a.Product_ID == pro.Prod_ID).Sum(a => (a.Detail_Total_Cost / a.Product_Quantity) * a.Reverse_Quantity);
                    detail.ProductCost = detail.TotalCostOfProduct / detail.quantitySaled;
                    detail.netProfit = detail.tatalSaledPrice - detail.TotalCostOfProduct;
                    vm.TotalOfNetProfit += detail.netProfit;
                }
                else
                {
                    detail.tatalSaledPrice = 0;
                    vm.TotalOfSale += 0; ;
                    detail.TotalCostOfProduct = 0;
                    detail.ProductCost = 0; ;
                    detail.netProfit = 0; ;
                    vm.TotalOfNetProfit += 0; ;

                }
                vm.DetailList.Add(detail);
            }

            return vm;
        }
        /// <summary>
        /// Sales report product for sales receipts
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static SalesOfProductVM makeSalesProductReportForSalesRecipt(DateTime start, DateTime end, int emp, int branch, int store)
        {
            var db = new AccountingContainer();
            var detilGlList = db.SalesReceiptDetails.Where(e => e.SalesReceiptMaster.Invoice_Date >= start && e.SalesReceiptMaster.Invoice_Date <= end && e.SalesReceiptMaster.Invoice_State == TransactionState.Post).ToList();
            if (emp != 0)
            {
                detilGlList = detilGlList.Where(a => a.SalesReceiptMaster.Emp_Id == emp).ToList();
            }
            if (branch != 0)
            {
                detilGlList = detilGlList.Where(a => a.SalesReceiptMaster.Branch_Id == branch).ToList();
            }
            if (store != 0)
            {
                detilGlList = detilGlList.Where(a => a.SalesReceiptMaster.Store_ID == store).ToList();
            }
            SalesOfProductVM vm = new SalesOfProductVM();
            vm.DetailList = new List<SalesOfProductDetailVM>();
            var ProList = db.Products.ToList();
            foreach (var pro in ProList)
            {
                SalesOfProductDetailVM detail = new SalesOfProductDetailVM();
                detail.product = pro;
                detail.quantitySaled = detilGlList.Where(a => a.Product_Id == pro.Prod_ID).Sum(a => a.Reverse_Quantity);
                if (detail.quantitySaled != 0)
                {
                    detail.tatalSaledPrice = detilGlList.Where(a => a.Product_Id == pro.Prod_ID).Sum(a => a.Reverse_Quantity * a.Product_Price);
                    vm.TotalOfSale += detail.tatalSaledPrice;
                    detail.TotalCostOfProduct = detilGlList.Where(a => a.Product_Id == pro.Prod_ID).Sum(a => (a.Detail_Total_Cost / a.Product_Quantity) * a.Reverse_Quantity);
                    detail.ProductCost = detail.TotalCostOfProduct / detail.quantitySaled;
                    detail.netProfit = detail.tatalSaledPrice - detail.TotalCostOfProduct;
                    vm.TotalOfNetProfit += detail.netProfit;
                }
                else
                {
                    detail.tatalSaledPrice = 0;
                    vm.TotalOfSale += 0; ;
                    detail.TotalCostOfProduct = 0;
                    detail.ProductCost = 0; ;
                    detail.netProfit = 0; ;
                    vm.TotalOfNetProfit += 0; ;

                }
                vm.DetailList.Add(detail);
            }

            return vm;
        }

        public static SalesOfProductVM makeSalesProductReportForOrder(DateTime start, DateTime end, int emp, int branch, int store)
        {
            var db = new AccountingContainer();
            var detilGlList = db.OrderDetails.Where(e => e.Order.OrderDate >= start && e.Order.OrderDate <= end && e.Order.Invoice_State == TransactionState.Post).ToList();
            if (emp != 0)
            {
                detilGlList = detilGlList.Where(a => a.Order.EmployeeID == emp).ToList();
            }

            if (branch != 0)
            {
                detilGlList = detilGlList.Where(a => a.Order.Branch_Id == branch).ToList();
            }
            if (store != 0)
            {
                detilGlList = detilGlList.Where(a => a.Order.StoreId == store).ToList();
            }
            SalesOfProductVM vm = new SalesOfProductVM();
            vm.DetailList = new List<SalesOfProductDetailVM>();
            var ProList = db.Products.ToList();
            foreach (var pro in ProList)
            {
                SalesOfProductDetailVM detail = new SalesOfProductDetailVM();
                detail.product = pro;
                detail.quantitySaled = detilGlList.Where(a => a.ProductID == pro.Prod_ID).Sum(a => a.Reverse_Quantity);
                if (detail.quantitySaled != 0)
                {
                    detail.tatalSaledPrice = detilGlList.Where(a => a.ProductID == pro.Prod_ID).Sum(a => a.Reverse_Quantity * a.Product_Price);
                    vm.TotalOfSale += detail.tatalSaledPrice;
                    detail.TotalCostOfProduct = detilGlList.Where(a => a.ProductID == pro.Prod_ID).Sum(a => (a.Detail_Total_Cost / a.Quantity) * a.Reverse_Quantity);
                    detail.ProductCost = detail.TotalCostOfProduct / detail.quantitySaled;
                    detail.netProfit = detail.tatalSaledPrice - detail.TotalCostOfProduct;
                    vm.TotalOfNetProfit += detail.netProfit;
                }
                else
                {
                    detail.tatalSaledPrice = 0;
                    vm.TotalOfSale += 0; ;
                    detail.TotalCostOfProduct = 0;
                    detail.ProductCost = 0; ;
                    detail.netProfit = 0; ;
                    vm.TotalOfNetProfit += 0; ;

                }
                vm.DetailList.Add(detail);
            }

            return vm;
        }

        /// <summary>
        /// Sales report product for POS
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static SalesOfProductVM makeSalesProductReportForPOSInvoice(DateTime start, DateTime end, int emp, int branch, int store)
        {
            var db = new AccountingContainer();
            var detilGlList = db.DetailPOSInvoices.Where(e => e.MasterPOSInvoice.Invoice_Date >= start && e.MasterPOSInvoice.Invoice_Date <= end && e.MasterPOSInvoice.Invoice_State == TransactionState.Post).ToList();
            if (emp != 0)
            {
                detilGlList = detilGlList.Where(a => a.MasterPOSInvoice.Employee_Id == emp).ToList();
            }

            if (branch != 0)
            {
                detilGlList = detilGlList.Where(a => a.MasterPOSInvoice.PointOfSale.BranchSubledger_Id == branch).ToList();
            }
            if (store != 0)
            {
                detilGlList = detilGlList.Where(a => a.MasterPOSInvoice.PointOfSale.Store_Id == store).ToList();
            }
            SalesOfProductVM vm = new SalesOfProductVM();
            vm.DetailList = new List<SalesOfProductDetailVM>();
            var ProList = db.Products.ToList();
            foreach (var pro in ProList)
            {
                SalesOfProductDetailVM detail = new SalesOfProductDetailVM();
                detail.product = pro;
                detail.quantitySaled = detilGlList.Where(a => a.Product_Id == pro.Prod_ID).Sum(a => a.Reverse_Quantity);
                if (detail.quantitySaled != 0)
                {
                    detail.tatalSaledPrice = detilGlList.Where(a => a.Product_Id == pro.Prod_ID).Sum(a => a.Reverse_Quantity * a.Product_Price);
                    vm.TotalOfSale += detail.tatalSaledPrice;
                    detail.TotalCostOfProduct = detilGlList.Where(a => a.Product_Id == pro.Prod_ID).Sum(a => a.ProductPriceCost * a.Reverse_Quantity);
                    detail.ProductCost = detail.TotalCostOfProduct / detail.quantitySaled;
                    detail.netProfit = detail.tatalSaledPrice - detail.TotalCostOfProduct;
                    vm.TotalOfNetProfit += detail.netProfit;
                }
                else
                {
                    detail.tatalSaledPrice = 0;
                    vm.TotalOfSale += 0; ;
                    detail.TotalCostOfProduct = 0;
                    detail.ProductCost = 0; ;
                    detail.netProfit = 0; ;
                    vm.TotalOfNetProfit += 0; ;

                }
                vm.DetailList.Add(detail);
            }

            return vm;
        }

        public static SalesCusomerReport prepareReportforCustomerSales()
        {
            var db = new AccountingContainer();
            var vm = new SalesCusomerReport();
            vm.customerList = new SelectList(db.Customers.ToList(), "Customer_Id", "Customer_Name");

            return vm;
        }
        /// <summary>
        /// Report for customer sales
        /// </summary>
        /// <param name="reportVM"></param>
        /// <returns></returns>
        public static SalesCusomerReport makecustomerSalesReport(SalesCusomerReport reportVM)
        {
            var db = new AccountingContainer();
            reportVM.company = db.Companies.FirstOrDefault();


            if (reportVM.startDate == null)
            {
                reportVM.startDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (reportVM.EndDate == null)
            {
                reportVM.EndDate = Convert.ToString(new DateTime(2200, 12, 30));

            }
            var start = Convert.ToDateTime(Convert.ToDateTime(reportVM.startDate).ToShortDateString());
            var End = Convert.ToDateTime(reportVM.EndDate).AddHours(23).AddMinutes(59).AddSeconds(59);

            List<int> customerList = new List<int>();
            if (reportVM.customer_id == 0 || reportVM.customer_id == null)
            {
                var cutomerInvoiceMaster = db.InvoiceMasters.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= End && e.Invoice_State == TransactionState.Post).ToList().Select(a => a.Customer_Id).ToList();
                var cutomersalesRecipt = db.SalesReceiptMasters.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= End && e.Invoice_State == TransactionState.Post && e.Customer_Id != null && e.Customer_Id != 0).ToList().Select(a => a.Customer_Id.Value).ToList();
                var MasterPOS = db.MasterPOSInvoices.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= End && e.Invoice_State == TransactionState.Post && e.Customer_Id != null && e.Customer_Id != 0).ToList().Select(a => a.Customer_Id.Value).ToList();
                var MasterOrder = db.Orders.Where(e => e.OrderDate >= start && e.OrderDate <= End && e.Invoice_State == TransactionState.Post && e.CustomerID != null && e.CustomerID != 0).ToList().Select(a => a.CustomerID.Value).ToList();
                customerList.AddRange(cutomerInvoiceMaster);
                customerList.AddRange(cutomersalesRecipt);
                customerList.AddRange(MasterPOS);
                customerList.AddRange(MasterOrder);

            }
            else
            {
                customerList.Add(reportVM.customer_id.Value);
            }

            var list = customerList.Distinct().OrderBy(a => a).ToList();
            var customer_Report = makeReportOfCustomerSales(list, start, End);
            customer_Report.startDate = reportVM.startDate;
            customer_Report.EndDate = reportVM.EndDate;
            return customer_Report;
        }

        /// <summary>
        /// Report for customer sales
        /// </summary>
        /// <param name="customerList"></param>
        /// <param name="start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        public static SalesCusomerReport makeReportOfCustomerSales(List<int> customerList, DateTime start, DateTime End)
        {
            var db = new AccountingContainer();
            SalesCusomerReport vm = new SalesCusomerReport();
            vm.company = db.Companies.FirstOrDefault();
            vm.CustomersSalesReport = new List<CustomerSalesMasterVM> { };
            var customerdetailList = new List<CustomerSalesMasterVM> { };
            foreach (var customerid in customerList)
            {
                var custdetail = new CustomerSalesMasterVM();
                custdetail.customer = db.Customers.Find(customerid);
                custdetail.cutomerBalance = 0;

                var invoicemasterchangeId = 0;
                var invoiceList = db.InvoiceDetails.Where(e => e.InvoiceMaster.Invoice_Date >= start && e.InvoiceMaster.Invoice_Date <= End && e.InvoiceMaster.Invoice_State == TransactionState.Post && e.InvoiceMaster.Customer_Id == customerid).ToList();
                custdetail.customerDetail = new List<InvoiceDetailOfCustomerVM>();
                foreach (var invoice in invoiceList)
                {
                    var detail = new InvoiceDetailOfCustomerVM();

                    if (invoicemasterchangeId != invoice.InvoiceMaster_Id)
                    {
                        var detail2 = new InvoiceDetailOfCustomerVM();
                        detail2.Invoice_date = invoice.InvoiceMaster.Invoice_Date;
                        detail2.TotalAmount = invoice.InvoiceMaster.InvoiceDetails.Sum(a => a.Reverse_Quantity * a.Product_Price) * ((invoice.InvoiceMaster.Master_Discount / 100) * -1) - invoice.InvoiceMaster.FixedDiscount.Value;
                        detail2.transtype = "فاتورة مبيعات اجل";
                        detail2.Invoice_Serial = invoice.InvoiceMaster.Serial_Number;

                        detail2.Description = "خصم";
                        if (detail2.TotalAmount != 0)
                        {
                            custdetail.customerDetail.Add(detail2);

                        }
                        invoicemasterchangeId = invoice.InvoiceMaster.Master_Id;
                    }
                    detail.Invoice_date = invoice.InvoiceMaster.Invoice_Date;

                    detail.Product = db.Products.Find(invoice.Product_ID);
                    detail.quantity = invoice.Reverse_Quantity;
                    detail.Price_unit = invoice.Product_Price;
                    detail.Invoice_Serial = invoice.InvoiceMaster.Serial_Number;
                    detail.transtype = "فاتورة مبيعات اجل";
                    detail.TotalAmount = invoice.Reverse_Quantity * invoice.Product_Price;
                    if (invoice.Product_Quantity == 0|| detail.Product.Prod_Type == Prod_Type.خدمة)
                    {
                        detail.totalCost = 0;
                    }
                    
                    else
                    {
                        detail.totalCost = (invoice.Detail_Total_Cost / invoice.Product_Quantity) * invoice.Reverse_Quantity;
                    }

                    detail.Description = invoice.InvoiceMaster.Description;
                    if (detail.TotalAmount != 0)
                    {
                        custdetail.customerDetail.Add(detail);
                    }



                }
                var SalesReciptId = 0;
                var SalesReciptList = db.SalesReceiptDetails.Where(e => e.SalesReceiptMaster.Invoice_Date >= start && e.SalesReceiptMaster.Invoice_Date <= End && e.SalesReceiptMaster.Invoice_State == TransactionState.Post && e.SalesReceiptMaster.Customer_Id == customerid).ToList();

                foreach (var invoice in SalesReciptList)
                {
                    var detail = new InvoiceDetailOfCustomerVM();

                    if (SalesReciptId != invoice.Master_Id)
                    {
                        var detail2 = new InvoiceDetailOfCustomerVM();
                        detail2.Invoice_date = invoice.SalesReceiptMaster.Invoice_Date;
                        detail2.TotalAmount = invoice.SalesReceiptMaster.SalesReceiptDetails.Sum(a => a.Reverse_Quantity * a.Product_Price) * ((invoice.SalesReceiptMaster.Master_Discount / 100) * -1) - invoice.SalesReceiptMaster.FixedDiscount.Value;
                        detail2.transtype = "فاتورة مبيعات نقدي";
                        detail2.Invoice_Serial = invoice.SalesReceiptMaster.Serial_Number;
                        detail2.Description = "خصم";
                        if (detail2.TotalAmount != 0)
                        {
                            custdetail.customerDetail.Add(detail2);

                        }
                        SalesReciptId = invoice.SalesReceiptMaster.Master_Id;
                    }
                    detail.Invoice_date = invoice.SalesReceiptMaster.Invoice_Date;

                    detail.Product = db.Products.Find(invoice.Product_Id);
                    detail.quantity = invoice.Reverse_Quantity;
                    detail.Price_unit = invoice.Product_Price;
                    detail.Invoice_Serial = invoice.SalesReceiptMaster.Serial_Number;
                    detail.transtype = "فاتورة مبيعات نقدي";
                    detail.TotalAmount = invoice.Reverse_Quantity * invoice.Product_Price;
                    if (invoice.Product_Quantity == 0|| detail.Product.Prod_Type == Prod_Type.خدمة)
                    {
                        detail.totalCost = 0;
                    }
                    else
                    {
                        detail.totalCost = (invoice.Detail_Total_Cost / invoice.Product_Quantity) * invoice.Reverse_Quantity;
                    }
                    detail.Description = invoice.SalesReceiptMaster.Description;

                    if (detail.TotalAmount != 0)
                    {
                        custdetail.customerDetail.Add(detail);
                    }



                }

                var OrderList = db.OrderDetails.Where(e => e.Order.OrderDate >= start && e.Order.OrderDate <= End && e.Order.Invoice_State == TransactionState.Post && e.Order.CustomerID == customerid).ToList();
                var OrderId = 0;
                foreach (var orderdetail in OrderList)
                {
                    var detail = new InvoiceDetailOfCustomerVM();

                    if (OrderId != orderdetail.OrderId)
                    {
                        var detail2 = new InvoiceDetailOfCustomerVM();
                        detail2.Invoice_date = orderdetail.Order.OrderDate;
                        detail2.TotalAmount = orderdetail.Order.OrderDetails.Sum(a => a.Reverse_Quantity * a.Product_Price) * ((orderdetail.Order.DicountInPrecentage / 100) * -1) ;
                        detail2.transtype = "فاتورة مبيعات كاشير";
                        detail2.Invoice_Serial = orderdetail.Order.OrderNumber;
                        detail2.Description = "خصم";
                        if (detail2.TotalAmount != 0)
                        {
                            custdetail.customerDetail.Add(detail2);

                        }
                        OrderId = orderdetail.Order.Id;
                    }
                    detail.Invoice_date = orderdetail.Order.OrderDate;

                    detail.Product = db.Products.Find(orderdetail.ProductID);
                    detail.quantity = orderdetail.Reverse_Quantity;
                    detail.Price_unit = orderdetail.Product_Price;
                    detail.Invoice_Serial = orderdetail.Order.OrderNumber;
                    detail.transtype = "فاتورة مبيعات كاشير";
                    detail.TotalAmount = orderdetail.Reverse_Quantity * orderdetail.Product_Price;
                    if (orderdetail.Quantity == 0 || detail.Product.Prod_Type == Prod_Type.خدمة|| detail.Product.Prod_Type == Prod_Type.مجمع)
                    {
                        detail.totalCost = 0;
                    }
                    else
                    {
                        detail.totalCost = (orderdetail.Detail_Total_Cost / orderdetail.Quantity) * orderdetail.Reverse_Quantity;
                    }
                    detail.Description = orderdetail.Order.Comment;

                    if (detail.TotalAmount != 0)
                    {
                        custdetail.customerDetail.Add(detail);
                    }



                }



                var POSMasterId = 0;
                var POSMasterList = db.DetailPOSInvoices.Where(e => e.MasterPOSInvoice.Invoice_Date >= start && e.MasterPOSInvoice.Invoice_Date <= End && e.MasterPOSInvoice.Invoice_State == TransactionState.Post && e.MasterPOSInvoice.Customer_Id == customerid).ToList();

                foreach (var invoice in POSMasterList)
                {
                    var detail = new InvoiceDetailOfCustomerVM();

                    if (POSMasterId != invoice.MasterPOSInvoiceId)
                    {
                        var detail2 = new InvoiceDetailOfCustomerVM();
                        detail2.Invoice_date = invoice.MasterPOSInvoice.Invoice_Date.Value;
                        detail2.TotalAmount = invoice.MasterPOSInvoice.DetailPOSInvoices.Sum(a => a.Reverse_Quantity * a.Product_Price) * ((invoice.MasterPOSInvoice.Master_Discount.Value / 100) * -1) - invoice.MasterPOSInvoice.AllowDiscount.Value;
                        detail2.transtype = "فاتورة نقطة بيع";
                        detail2.Invoice_Serial = invoice.MasterPOSInvoice.Serial_Number;
                        detail2.Description = "خصم";
                        if (detail2.TotalAmount != 0)
                        {
                            custdetail.customerDetail.Add(detail2);

                        }
                        SalesReciptId = invoice.MasterPOSInvoice.MasterPOSInvoiceId;
                    }
                    detail.Invoice_date = invoice.MasterPOSInvoice.Invoice_Date.Value;

                    detail.Product = db.Products.Find(invoice.Product_Id);
                    detail.quantity = invoice.Reverse_Quantity;
                    detail.Price_unit = invoice.Product_Price;
                    detail.Invoice_Serial = invoice.MasterPOSInvoice.Serial_Number;
                    detail.transtype = "فاتورة نقطة بيع";
                    detail.TotalAmount = invoice.Reverse_Quantity * invoice.Product_Price;
                    detail.Description = invoice.MasterPOSInvoice.Description;
                    if (invoice.Product_Quantity == 0 || detail.Product.Prod_Type == Prod_Type.خدمة)
                    {
                        detail.totalCost = 0;
                    }
                    else
                    {
                        detail.totalCost = (invoice.Detail_Total_Cost / invoice.Product_Quantity) * invoice.Reverse_Quantity;
                    }
                    if (detail.TotalAmount != 0)
                    {
                        custdetail.customerDetail.Add(detail);
                    }

                }
                var z = custdetail.customerDetail.ToList();
                custdetail.cutomerBalance = custdetail.customerDetail.Sum(a => a.TotalAmount);

                vm.CustomersSalesReport.Add(custdetail);
            }

            return vm;

        }
        public static BarCodePrint PrintBarcodevm( )
        {
            var bar = new BarCodePrint();
            var db = new AccountingContainer();
           
            if (db.PrintBarcodes.Count() != 0)
            {
                var print = db.PrintBarcodes.FirstOrDefault();
                bar.name = print.Name.Value;
                bar.Notes = print.Notes.Value;
               
                bar.Price = print.Price.Value;
                bar.PriceWithTax = print.PriceWithTax.Value;
                bar.Tax = print.Tax.Value;
                bar.DateOfExpire = print.DateOfExpire.Value;

            }
                return bar;
        }
        public static BarCodePrint PrintBarcode(BarCodePrint vm)
        {

            var db = new AccountingContainer();
            if (db.PrintBarcodes.Count() != 0)
            {
                var print = db.PrintBarcodes.FirstOrDefault();
                print.Name = vm.name;
                print.Notes = vm.Notes;
                print.Price = vm.Price ;
                print.PriceWithTax = vm.PriceWithTax ;
                print.DateOfExpire = vm.DateOfExpire;
                print.Tax= vm.Tax ;
                db.Entry(print).State = EntityState.Modified;
                db.SaveChanges();

            }
            else
            {
                var print =new PrintBarcode();
                print.Name = vm.name;
                print.Notes = vm.Notes;
                print.Price = vm.Price;
                print.PriceWithTax = vm.PriceWithTax;
                print.DateOfExpire = vm.DateOfExpire;
                print.Tax = vm.Tax;
                db.PrintBarcodes.Add(print);
                db.SaveChanges();

            }
         

            return vm;
        }

        public static BarCodePrint PrintBarcodeTemp(int Product_Id,int ? storeId)
        {

            var db = new AccountingContainer();

            var print = db.PrintBarcodes.FirstOrDefault();
            var vm = new BarCodePrint();
            if (print != null)
            {
                vm.name = print.Name.Value;
                vm.Notes = print.Notes.Value;
                vm.Price = print.Price.Value;
                vm.PriceWithTax = print.PriceWithTax.Value;
                vm.DateOfExpire = print.DateOfExpire.Value;
                vm.Tax = vm.Tax;
            }
            else
            {
                vm.name = false;
                vm.Notes = false;
                vm.Price = false;
                vm.PriceWithTax =false;
                vm.DateOfExpire = false;
                vm.Tax = false;

            }

          
            vm.product = db.Products.Find(Product_Id);
            if (storeId > 0)
            {
                var batch = db.ProductBatches.Where(a => a.Store_ID == storeId.Value && a.Product_ID == Product_Id).FirstOrDefault();
                if (batch != null&&batch.SalesPrice!=null)
                {
                    vm.product.Sale_Price = batch.SalesPrice;
                }
               

            }
            return vm;
        }
        public static BarCodePrint PrintBarcodeTest(BarCodePrint vm)
        {
            var db = new AccountingContainer();
            var pro = new Product();
            pro.Prod_Name = "لاب توب ";
            pro.Sale_Price = 5000;
            vm.product = pro;
            return vm;
        }




        public static PrintChartOfAccountVM PrintChartOfAccount()
        {
            var db = new AccountingContainer();
            ChartOfAccounting.MakeLedger();
            var account = new PrintChartOfAccount();

            var List = account.getAllBalanceSheetModel("0");

            var accountvm = new PrintChartOfAccountVM();
            accountvm.chart = List;
            accountvm.company = db.Companies.FirstOrDefault();
            return accountvm;

        }
        ///TaxReport
        public static TaxReportVM getTaxOfReportDateTax()
        {

            var taxReportVM = new TaxReportVM();
            return taxReportVM;


        }
        public static TaxReportVM CalculateTax(TaxReportVM vm)
        {
            var db = new AccountingContainer();
            if (vm.Date == null)
            {
                vm.Date = Convert.ToString(new DateTime(2200, 12, 30));

            }
            vm.company = db.Companies.FirstOrDefault();
            var End = Convert.ToDateTime(vm.Date).AddHours(23).AddMinutes(59).AddSeconds(59);
            vm.taxReportDetail = new List<TaxReportDetailVM>();
            foreach (var AccountSalTax in db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == 49).ToList())
            {
                var taxDetail = new TaxReportDetailVM();

                taxDetail.isPurchase = false;
                taxDetail.Percentage = db.Taxes.Where(a => a.Tax_Code == AccountSalTax.Account_Code).FirstOrDefault().Tax_Percentage;
                taxDetail.Tax_Name = AccountSalTax.Account_Name;
                taxDetail.TaxBallance = GetBallanceOfTax(AccountSalTax.Account_Id, End);
                taxDetail.SaleseBallance = taxDetail.TaxBallance * (100 / taxDetail.Percentage);
                taxDetail.Tax_Id = AccountSalTax.Account_Id;
                vm.TotalRevenueTax += taxDetail.TaxBallance;
                vm.TotalRevenue += taxDetail.SaleseBallance;
                vm.taxReportDetail.Add(taxDetail);

            }
            foreach (var AccountSalTax in db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == 36).ToList())
            {
                var taxDetail = new TaxReportDetailVM();
                taxDetail.isPurchase = true;
                taxDetail.Percentage = db.Taxes.Where(a => a.Tax_Code == AccountSalTax.Account_Code).FirstOrDefault().Tax_Percentage;
                taxDetail.Tax_Name = AccountSalTax.Account_Name;
                taxDetail.Tax_Id = AccountSalTax.Account_Id;
                taxDetail.TaxBallance = GetBallanceOfTax(AccountSalTax.Account_Id, End);
                taxDetail.PurchaseBallance = taxDetail.TaxBallance * (100 / taxDetail.Percentage);
                vm.TotalPurchaseTax += taxDetail.TaxBallance;
                vm.TotalPurchase += taxDetail.PurchaseBallance;

                vm.taxReportDetail.Add(taxDetail);

            }


            vm.DifferenceBetweenRevenueAndTax = vm.TotalPurchaseTax + vm.TotalRevenueTax;



            return vm;
        }

        public static decimal GetBallanceOfTax(int Account_Id, DateTime date)
        {
            var db = new AccountingContainer();
            decimal Credit = 0;
            decimal Depite = 0;
            try
            {
                Credit = db.GLDetails.Where(a => a.Account_Id == Account_Id && a.GLMaster.GL_Date <= date && a.GLDetail_Credit != null).Select(a => a.GLDetail_Credit).Sum();
                Depite = db.GLDetails.Where(a => a.Account_Id == Account_Id && a.GLMaster.GL_Date <= date && a.GLDetail_Debit != null).Select(a => a.GLDetail_Debit).Sum();
            }
            catch
            {
                return 0;
            }
            decimal ballance = 0;
            if (Credit > Depite)
            {
                ballance = Credit - Depite;
                ballance = ballance * -1;
            }
            else
            {
                ballance = Depite - Credit;
            }
            return ballance;

        }
        public static DailyPOSEmp prepareCustomersOfEmp()
        {
            var db = new AccountingContainer();
            DailyPOSEmp posinvoice = new DailyPOSEmp();

            
         
                var employee = db.Employees.Where(a=>a.Reg_ID!=null).ToList();
         
            posinvoice.EmployeeList = new SelectList(employee, "Emp_Id", "Emp_Name");
            return posinvoice;

        }
        public static DailyPOSEmp getCustomersOfEmp(DailyPOSEmp vm)
        {
            var db = new AccountingContainer();
            vm.CustomerList = new List<Customer>();
            vm.CustomerList = db.Customers.Where(a => a.Employee_Id == vm.Employee_Id).ToList();
            return vm;

        }
        public static DailyPOSEmp prepareForReportPOSDaily()
        {
            var db = new AccountingContainer();
            DailyPOSEmp posinvoice = new DailyPOSEmp();

            var empList = new List<Employee>();
            var EmpidList = db.PointOfSales.ToList().Select(a => a.Employee_Id);
            foreach (var emp in EmpidList.Distinct())
            {
                var employee = db.Employees.Find(emp);
                empList.Add(employee);
            }
            posinvoice.EmployeeList = new SelectList(empList, "Emp_Id", "Emp_Name");
            return posinvoice;

        }
        public static DailyPOSEmp PrepareForReportCachierDaily()
        {
            var db = new AccountingContainer();
            DailyPOSEmp posinvoice = new DailyPOSEmp();

            var empList = new List<Employee>();
            var EmpidList = db.Employees.Where(e => e.Reg_ID != null).ToList();
            //foreach (var emp in EmpidList.Distinct())
            //{
            //    var employee = db.Employees.Find(emp);
            //    empList.Add(employee);
            //}
            posinvoice.EmployeeList = new SelectList(EmpidList, "Emp_Id", "Emp_Name");
            return posinvoice;

        }
        public static DailyPOSEmp ReportPOSDaily(DailyPOSEmp posinvoice)
        {
            var db = new AccountingContainer();
            posinvoice.company = db.Companies.FirstOrDefault();

            posinvoice.PaymentDailyList = new List<Dailypayment>();
            posinvoice.Emp_Name = db.Employees.Find(posinvoice.Employee_Id).Emp_Name;

            if (posinvoice.StartDate == null)
            {
                posinvoice.StartDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (posinvoice.End_Date == null)
            {
                posinvoice.End_Date = Convert.ToString(new DateTime(2200, 12, 30));

            }
            var star = Convert.ToDateTime(Convert.ToDateTime(posinvoice.StartDate).ToShortDateString());
            var End = Convert.ToDateTime(posinvoice.End_Date).AddHours(23).AddMinutes(59).AddSeconds(59);
            posinvoice.POSInvoiceList = db.MasterPOSInvoices.Where(p => p.Invoice_Date >= star && p.Invoice_Date <= End && p.Invoice_State == TransactionState.Post && p.Employee_Id == posinvoice.Employee_Id&&p.Invoice_State==TransactionState.Post).ToList();
            var ListofPaymentDetail = db.PosPaymentMethods.Where(p => p.MasterPOSInvoice.Invoice_Date >= star && p.MasterPOSInvoice.Invoice_Date <= End && p.MasterPOSInvoice.Invoice_State == TransactionState.Post && p.MasterPOSInvoice.Employee_Id == posinvoice.Employee_Id).ToList();
            var paymentMethodIds = ListofPaymentDetail.Select(a => a.PaymentMethod_Id).Distinct();
            foreach (var payId in paymentMethodIds)
            {
                var dailypayment = new Dailypayment();

                dailypayment.payment_Id = payId;
                dailypayment.paymentName = db.PaymentMethods.Find(payId).Payment_Type;
                dailypayment.paymentValue = ListofPaymentDetail.Where(a => a.PaymentMethod_Id == payId).Select(a => a.Amount).Sum();

                posinvoice.PaymentDailyList.Add(dailypayment);
            }
            var UnpayedInvoice = posinvoice.POSInvoiceList.Where(c => c.PosPaymentMethods.Count() == 0).ToList();
            if (UnpayedInvoice.Count() > 0)
            {
                posinvoice.totalUnpayedInvoice = UnpayedInvoice.Select(a => a.Total_Amount.Value).Sum();

            }
            else
            {
                posinvoice.totalUnpayedInvoice = 0;
            }
            var Reverse = posinvoice.POSInvoiceList.SelectMany(a => a.ReversePOSInvoices).ToList();
            var RevesePaymentIds = Reverse.Select(a => a.PaymentMethod_Id).Distinct().ToList();
            var po = Reverse.Select(a => a.Total_Amount).ToList().Sum();
            var ac = posinvoice.PaymentDailyList.Select(a => a.paymentValue).ToList().Sum();
           
            foreach (var payId in RevesePaymentIds)
            {
                
                if (RevesePaymentIds.Contains(payId))
                {
                    var xz= Reverse.Where(p => p.PaymentMethod_Id== payId).Select(a => a.Total_Amount).ToList().Sum();
                    var pr = posinvoice.PaymentDailyList.Where(a => a.payment_Id == payId).FirstOrDefault().paymentValue;
                    posinvoice.PaymentDailyList.Where(a => a.payment_Id == payId).FirstOrDefault().paymentValue-= Reverse.Where(p => p.PaymentMethod_Id == payId).Select(a=>a.Total_Amount).ToList().Sum();

                }
                else
                {
                    var dailypayment = new Dailypayment();

                    dailypayment.payment_Id = payId;
                    dailypayment.paymentName = db.PaymentMethods.Find(payId).Payment_Type;
                    dailypayment.paymentValue  = Reverse.Select(a => a.Total_Amount).ToList().Sum()*-1;

                    posinvoice.PaymentDailyList.Add(dailypayment);

                }


            }

                return posinvoice;

        }

        public static DailyPOSEmp ReportCasherDaily(DailyPOSEmp posinvoice)
        {
            var db = new AccountingContainer();
            posinvoice.company = db.Companies.FirstOrDefault();

            posinvoice.PaymentDailyList = new List<Dailypayment>();
            posinvoice.Emp_Name = db.Employees.Find(posinvoice.Employee_Id).Emp_Name;

            if (posinvoice.StartDate == null)
            {
                posinvoice.StartDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (posinvoice.End_Date == null)
            {
                posinvoice.End_Date = Convert.ToString(new DateTime(2200, 12, 30));

            }
            // TimeSpan is 0 days, 4 hours, 0 minutes and 0 second.
            if (String.IsNullOrEmpty(posinvoice.TimeFrom))
            {
                posinvoice.TimeFrom = Convert.ToString(new TimeSpan(0, 0, 0, 0));
            }
            if (String.IsNullOrEmpty(posinvoice.TimeTo))
            {
                posinvoice.TimeTo = Convert.ToString(new TimeSpan(0, 0, 0, 0));

            }
            var From = TimeSpan.Parse(posinvoice.TimeFrom);
            var To = TimeSpan.Parse(posinvoice.TimeTo);
            var star = Convert.ToDateTime(Convert.ToDateTime(posinvoice.StartDate).Add(From));
            var End = Convert.ToDateTime(posinvoice.End_Date).Add(To);
           
            posinvoice.OrderList = db.Orders.Where(p => p.OrderDate >= star && p.OrderDate <= End && p.Invoice_State == TransactionState.Post && p.EmployeeID == posinvoice.Employee_Id && p.Invoice_State == TransactionState.Post).ToList();
            
            var paymentMethodIds = posinvoice.OrderList.Select(a => a.PaymentMethodeID).Distinct();
            foreach (var payId in paymentMethodIds)
            {
                var dailypayment = new Dailypayment();

                dailypayment.payment_Id = payId;
                dailypayment.paymentName = db.PaymentMethods.Find(payId).Payment_Type;
                dailypayment.paymentValue = posinvoice.OrderList.Where(a => a.PaymentMethodeID == payId).Select(a => a.TotalNewTaxs).Sum()+ posinvoice.OrderList.Where(a => a.PaymentMethodeID == payId).Select(a => a.Total_Amount).Sum();

                posinvoice.PaymentDailyList.Add(dailypayment);
            }
            var Reverse = posinvoice.OrderList.SelectMany(a => a.ReverseOrders).ToList();
            var RevesePaymentIds = Reverse.Select(a => a.PaymentMethodeID).Distinct().ToList();
            var po = Reverse.Select(a => a.Total_Amount).ToList().Sum();
            var ac = posinvoice.PaymentDailyList.Select(a => a.paymentValue).ToList().Sum();
            foreach (var payId in RevesePaymentIds)
            {

                if (RevesePaymentIds.Contains(payId))
                {
                    var xz = Reverse.Where(p => p.PaymentMethodeID == payId).Select(a => a.Total_Amount).ToList().Sum();
                    var pr = posinvoice.PaymentDailyList.Where(a => a.payment_Id == payId).FirstOrDefault().paymentValue;
                    posinvoice.PaymentDailyList.Where(a => a.payment_Id == payId).FirstOrDefault().paymentValue -= Reverse.Where(p => p.PaymentMethodeID == payId).Select(a => a.Total_Amount).ToList().Sum();

                }
                else
                {
                    var dailypayment = new Dailypayment();

                    dailypayment.payment_Id = payId;
                    dailypayment.paymentName = db.PaymentMethods.Find(payId).Payment_Type;
                    dailypayment.paymentValue = Reverse.Select(a => a.Total_Amount).ToList().Sum() * -1;

                    posinvoice.PaymentDailyList.Add(dailypayment);

                }


            }

            return posinvoice;

        }
        public static TaxSalesandPurchase getsaleTax(int taxid,string from,string to)
        {
            TaxSalesandPurchase tax = new TaxSalesandPurchase();
            tax.tax_Id = taxid;
            tax.to = to;
            tax.from = from;
            var model = Tax.MakeReportTaxSalesAndPurch(tax);
            return tax;

        }
        public static MRGL getListForLedgerget(string code)
        {
            var db = new AccountingContainer();
            MRGL LVM = new MRGL();

            //IQueryable<object> newList2 = ChartOfAccounting.getAlldataForGL();

            if (code != "" && code != null)
            {
                LVM.accountId = db.ChartOfAccountings.Where(a => a.Account_Code == code).FirstOrDefault().Account_Id;
            }

            //LVM.AccountList = new SelectList(newList2, "Id", "Name");

            var newList = from subled in db.ChartOfAccountings.Where(a=>a.Account_Level!=0)
                          select new
                          {

                              Id = subled.Account_Id,
                              Name = subled.Account_Name + "        " + subled.Account_Code


                          };
            LVM.AccountList = new SelectList(newList, "Id", "Name");

            return LVM;
        }
        public static MRGL getStatisticOAccount(int Account, string Start, string End)
        {
            MRGL mrgl = new MRGL();
            mrgl.FromDate = Start;
            mrgl.Todate = End;
            mrgl.accountId = Account;
            return makeReportForParent(mrgl);
        }

        public static MRGL getStatisticOAccountOfSubledger(int Account, int Subledger, string Start, string end)
        {
            MRGL mrgl = new MRGL();
            var db = new AccountingContainer();
            mrgl.FromDate = Start;
            mrgl.Todate = end;
            mrgl.accountId = Account;
            if (mrgl.FromDate == null)
            {
                mrgl.FromDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (mrgl.Todate == null)
            {
                mrgl.Todate = Convert.ToString(new DateTime(2200, 12, 30));

            }
            mrgl.Account = db.ChartOfAccountings.Find(Account);
    
     
            var End = Convert.ToDateTime(mrgl.Todate).AddHours(23).AddMinutes(59).AddSeconds(59);
        
            var start = Convert.ToDateTime(mrgl.FromDate);
            var ALLdetilGlForPeriod = db.GLDetails.Where(e => e.GLMaster.GL_Date >= start && e.GLMaster.GL_Date <= End && e.Account_Id==Account&&e.SubLedger_Id== Subledger).ToList();
            mrgl.AmountPeriod = ALLdetilGlForPeriod.Select(a => a.GLDetail_Debit).Sum() - ALLdetilGlForPeriod.Select(a => a.GLDetail_Credit).Sum();

            var ALLdetilGlForbefore = db.GLDetails.Where(e => e.GLMaster.GL_Date < start && e.Account_Id == Account && e.SubLedger_Id == Subledger).ToList();
            mrgl.AmountBefor = ALLdetilGlForbefore.Select(a => a.GLDetail_Debit).Sum() - ALLdetilGlForbefore.Select(a => a.GLDetail_Credit).Sum();

            return mrgl;
        }
        public static MRGL makeReportForParent(MRGL mRGL)
        {
            var db = new AccountingContainer();
            mRGL.details = new List<MRGLDetail>();
            mRGL.company = db.Companies.FirstOrDefault();
            mRGL.Account = db.ChartOfAccountings.Find(mRGL.accountId);

            if (mRGL.FromDate == null)
            {
                mRGL.FromDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (mRGL.Todate == null)
            {
                mRGL.Todate = Convert.ToString(new DateTime(2200, 12, 30));

            }
            var alldocument = new List<AllSectionPendingVM>();
            //var start = Convert.ToDateTime(mRGL.FromDate).AddHours(23).AddMinutes(59).AddSeconds(59); ;
            var End = Convert.ToDateTime(mRGL.Todate).AddHours(23).AddMinutes(59).AddSeconds(59);
            //var startForBforDate = Convert.ToDateTime(mRGL.FromDate).AddHours(23).AddMinutes(59).AddSeconds(59);
            var start = Convert.ToDateTime(mRGL.FromDate);
            var code = db.ChartOfAccountings.Find(mRGL.accountId).Account_Code;
            var detilGlList = db.GLDetails.Where(e => e.GLMaster.GL_Date >= start && e.GLMaster.GL_Date <= End &&e.ChartOfAccounting.Account_Code.StartsWith(code)).ToList();
            mRGL.AmountBefor = Tax.getTheBalanceOfDateBefore(mRGL.Account.Account_Code, start);
            if(((mRGL.Account.Account_Code.StartsWith("010120002")|| mRGL.Account.Account_Code.StartsWith("010120001") )&& mRGL.penddingDocument == true))
            {
                 alldocument = AllPendingDocumanets.GetAllPendingsForReport();
                mRGL.AmountBefor = mRGL.AmountBefor + (alldocument.Where(a => a.date < start && a.TresuryOrBankId == mRGL.accountId && a.treasuryCredit == false).Select(z => z.TotalAmount).Sum() -
                    alldocument.Where(a => a.date < start && a.TresuryOrBankId == mRGL.accountId && a.treasuryCredit == true).Select(z => z.TotalAmount).Sum());

                alldocument = alldocument.Where(e => e.date >= start && e.date <= End && e.TresuryOrBankId == mRGL.accountId).ToList();
            }
            //var alldata = db.GLDetails.Where(e => e.GLMaster.GL_Date >= start && e.GLMaster.GL_Date <= End && e.ChartOfAccounting.Account_Code == mRGL.Account.Account_Code).ToList();
            mRGL.details =
            (from detail in detilGlList
             join Account in db.Accounts on detail.Account_Child_ID equals Account.Accounts_Id into AccountList
             select new MRGLDetail
             {
                ChartAccount = detail.ChartOfAccounting,
              date = detail.GLMaster.GL_Date,
            DocumentNumber = detail.GLMaster.GLCode,
                 Description = detail.Description_Detail!=null? detail.Description_Detail:detail.GLMaster.GL_Desc,

                 Amount = detail.GLDetail_Debit - detail.GLDetail_Credit,
                 Account = AccountList.Count() > 0 ? AccountList.FirstOrDefault() : null,
                 AccountName = AccountList.Count() > 0 ? AccountList.FirstOrDefault().Account_Name : null,
                 


             }).ToList();
            //foreach (var detail in detilGlList)
            //{
            //    var detailMR = new MRGLDetail();
            //    detailMR.ChartAccount = detail.ChartOfAccounting;
            //    detailMR.date = detail.GLMaster.GL_Date;
            //    detailMR.DocumentNumber = detail.GLMaster.GLCode;
            //    //if(string.IsNullOrEmpty(detail.Description_Detail)|| detail.GLMaster.GLCode.StartsWith("SPRE-"))
            //    //{
            //    //    detailMR.Description = detail.GLMaster.GL_Desc;
            //    //}
            //    //else
            //    //{
                    
            //    //}


            //    detailMR.Description = detail.Description_Detail;
            //    detailMR.Amount = detail.GLDetail_Debit - detail.GLDetail_Credit;
            //    var AccountChild = detail.GLMaster.GLDetails.Where(a => a.Account_Child_ID > 0).FirstOrDefault();
            //    if (AccountChild != null)
            //    {
            //        if (detail.GLMaster.GLCode.StartsWith("GL-") || detail.GLMaster.GLCode.StartsWith("GLST-"))
            //        {

            //            detailMR.Account = null;
            //            detailMR.AccountName = "";
            //        }
            //        else
            //        {
            //            detailMR.Account = db.Accounts.Find(AccountChild.Account_Child_ID);
            //            detailMR.AccountName = detailMR.Account.Account_Name;

            //        }

            //    }


            //    mRGL.details.Add(detailMR);


            //}
            foreach (var detail in alldocument)
            {
                var detailMR = new MRGLDetail();
                detailMR.ChartAccount =mRGL.Account;
                detailMR.date = detail.date;
                detailMR.DocumentNumber = detail.Serial;

                detailMR.Description = detail.Description ;
                if (detail.treasuryCredit == true)
                {
                    detailMR.Amount = -detail.TotalAmount;
                }
                else
                {
                    detailMR.Amount = detail.TotalAmount;
                }
           
              


                mRGL.details.Add(detailMR);


            }
            mRGL.details = mRGL.details.OrderBy(a => a.date).ToList();
            return mRGL;
        
        }
        public static MRGL getListForLedgerPOSt(MRGL mRGL)
        {
            var db = new AccountingContainer();
            

            mRGL.details = new List<MRGLDetail>();
            mRGL.company = db.Companies.FirstOrDefault();
            mRGL.Account = db.ChartOfAccountings.Find(mRGL.accountId);
            if (mRGL.FromDate == null)
            {
                mRGL.FromDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (mRGL.Todate == null)
            {
                mRGL.Todate = Convert.ToString(new DateTime(2200, 12, 30));

            }
            DateTime start = Convert.ToDateTime(Convert.ToDateTime(mRGL.FromDate).ToShortDateString());
            DateTime End = Convert.ToDateTime(Convert.ToDateTime(mRGL.Todate).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);


            mRGL.AmountBefor = Tax.getTheBalanceOfDateBefore(mRGL.Account.Account_Code, start);
          
            var alldata = db.GLDetails.Where(e => e.GLMaster.GL_Date >= start && e.GLMaster.GL_Date <= End && e.ChartOfAccounting.Account_Code == mRGL.Account.Account_Code).ToList();
            foreach(var detail in alldata)
            {
                var detailMR = new MRGLDetail();
                detailMR.date = detail.GLMaster.GL_Date;
                detailMR.DocumentNumber = detail.GLMaster.GLCode;
                if(detail.Description_Detail==null|| detail.Description_Detail == "")
                {
                    detailMR.Description = detail.GLMaster.GL_Desc;
                }
                else
                {
                    detailMR.Description = detail.Description_Detail;
                }
                detailMR.Amount = detail.GLDetail_Debit - detail.GLDetail_Credit;
                var AccountChild = detail.GLMaster.GLDetails.Where(a => a.Account_Child_ID > 0).FirstOrDefault();
                if (AccountChild != null)
                {
                    if (detail.GLMaster.GLCode.StartsWith("GL-") || detail.GLMaster.GLCode.StartsWith("GLST-"))
                    {

                        detailMR.Account = null;
                        detailMR.AccountName = "";
                    }
                    else
                    {
                        detailMR.Account = db.Accounts.Find(AccountChild.Account_Child_ID);
                        detailMR.AccountName = detailMR.Account.Account_Name;

                    }

                }


                mRGL.details.Add(detailMR);


            }
            
            return mRGL;

        }

        public static ChildAccountGL getListForLedgerForChildget(string code)
        {
            var db = new AccountingContainer();
            ChildAccountGL LVM = new ChildAccountGL();

            //IQueryable<object> newList2 = ChartOfAccounting.getAlldataForGL();
           
            LVM.Account = db.Accounts.Where(a => a.Account_Code == code).FirstOrDefault();
            LVM.accountId = LVM.Account.Accounts_Id;
            //LVM.AccountList = new SelectList(newList2, "Id", "Name");

            return LVM;
        }
        public static ChildAccountGL getListForChildAccountPOSt(ChildAccountGL mRGL)
        {
            var db = new AccountingContainer();


            mRGL.details = new List<ChildAccountGLDetail>();
            mRGL.company = db.Companies.FirstOrDefault();
            mRGL.Account = db.Accounts.Find(mRGL.accountId);
           



            if (mRGL.FromDate == null)
            {
                mRGL.FromDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (mRGL.Todate == null)
            {
                mRGL.Todate = Convert.ToString(new DateTime(2200, 12, 30));

            }
            DateTime start = Convert.ToDateTime(Convert.ToDateTime(mRGL.FromDate).ToShortDateString());
            DateTime End = Convert.ToDateTime(Convert.ToDateTime(mRGL.Todate).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);
            int customerid = 0;
            int supplierid = 0;
            mRGL.AmountBefor = getTheBalanceOfDateBefore(mRGL.Account.Accounts_Id, start);
            mRGL.Creditbefor = getThCreditBefore(mRGL.Account.Accounts_Id, start);
            mRGL.Depitbefor = getTheDepitbefor(mRGL.Account.Accounts_Id, start);
      
            if (mRGL.Account.Account_Code.StartsWith("CUST"))
            {
                var customer = db.Customers.Where(a => a.Customer_Code == mRGL.Account.Account_Code).FirstOrDefault();
                 customerid = customer.Customer_Id;
        
                mRGL.statistic = OneCustomerStatistic(End, customerid).FirstOrDefault();
            }
            if (mRGL.Account.Account_Code.StartsWith("SUP"))
            {

                 supplierid = db.Suppliers.Where(a => a.Supp_Code == mRGL.Account.Account_Code).FirstOrDefault().Supp_Id;

                mRGL.statistic = OneSupplierStatistic(End, supplierid).FirstOrDefault();
            }
            var alldocument = new List<AllSectionPendingVM>();
            if (mRGL.penddingDoc == true)
            {
                 alldocument = AllPendingDocumanets.GetAllPendingsForReport();
                alldocument = alldocument.Where(a =>  ((a.ChildAccountId == mRGL.Account.Accounts_Id && a.ChildAccountId > 0) || (a.SupplierId == supplierid && a.SupplierId > 0) || (a.CustomerId == customerid && a.CustomerId > 0))).ToList();
                mRGL.Creditbefor = mRGL.Creditbefor + alldocument.Where(a => a.date < start && a.TotalAmountForChild < 0 ).Select(z => z.TotalAmountForChild).Sum();
                mRGL.Depitbefor = mRGL.Depitbefor + alldocument.Where(a => a.date < start && a.TotalAmountForChild > 0).Select(z => z.TotalAmountForChild).Sum();
                mRGL.AmountBefor = mRGL.Depitbefor + mRGL.Creditbefor;

                alldocument = alldocument.Where(e => e.date >= start && e.date <= End ).ToList();
            }

            var alldata = db.GLDetails.Where(e => e.GLMaster.GL_Date >= start && e.GLMaster.GL_Date <= End && e.Account_Child_ID == mRGL.Account.Accounts_Id).ToList();
            foreach (var detail in alldata)
            {
                var detailMR = new ChildAccountGLDetail();
                detailMR.date = detail.GLMaster.GL_Date;
                detailMR.DocumentNumber = detail.GLMaster.GLCode;
                detailMR.Number = detail.GLMaster.GLCode;
                if (string.IsNullOrEmpty(detail.Description_Detail) || detail.GLMaster.GLCode.StartsWith("SPRE-"))
                {
                    detailMR.Description = detail.GLMaster.GL_Desc;
                }
                else
                {
                    detailMR.Description = detail.Description_Detail;
                }


                detailMR.Amount = detail.GLDetail_Debit - detail.GLDetail_Credit;

                if (detail.GLMaster.GLCode.StartsWith("SAL-"))
                {
                    detailMR.dueDate = db.InvoiceMasters.Where(a => a.Serial_Number == detail.GLMaster.GLCode).FirstOrDefault().Due_Date;
                }
                if (detail.GLMaster.GLCode.StartsWith("PUR-"))
                {
                    detailMR.dueDate = db.PurInvoiceMasters.Where(a => a.Serial_Number == detail.GLMaster.GLCode).FirstOrDefault().Due_Date;
                }

                var invoiceSerial = db.MasterQRInvoices.Where(a => a.Serial_Number == detail.GLMaster.GLCode).FirstOrDefault();
                if (invoiceSerial != null && invoiceSerial.Number!=null)
                {
                    detailMR.Number = invoiceSerial.Number;
                }
                mRGL.details.Add(detailMR);


            }
            foreach (var detail in alldocument)
            {
                var detailMR = new ChildAccountGLDetail();
              
                detailMR.date = detail.date;
                detailMR.DocumentNumber = detail.Serial;

                detailMR.Description = detail.Description;
                detailMR.Amount = detail.TotalAmountForChild;
             
             
                mRGL.details.Add(detailMR);
            }
                return mRGL;

        }
        public static ChildAccountGL getListForLedgerForAsset()
        {
            var db = new AccountingContainer();
            ChildAccountGL LVM = new ChildAccountGL();
            var newList = from subled in db.FixedAssets
                          select new
                          {

                              Id = subled.Fasset_ID,
                              Name = subled.FAsset_Description + "        " + subled.FAsset_Code


                          };
            LVM.fixedAssetList = new SelectList(newList, "Id", "Name");
          

            return LVM;
        }
        public static ChildAccountGL getListForLedgerForAssetPOSt(ChildAccountGL mRGL)
        {
            var db = new AccountingContainer();


            mRGL.details = new List<ChildAccountGLDetail>();
            mRGL.company = db.Companies.FirstOrDefault();
            mRGL.fixedAsset = db.FixedAssets.Find(mRGL.fixedAsset_Id);
      



            if (mRGL.FromDate == null)
            {
                mRGL.FromDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (mRGL.Todate == null)
            {
                mRGL.Todate = Convert.ToString(new DateTime(2200, 12, 30));

            }
            DateTime start = Convert.ToDateTime(Convert.ToDateTime(mRGL.FromDate).ToShortDateString());
            DateTime End = Convert.ToDateTime(Convert.ToDateTime(mRGL.Todate).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);


            mRGL.AmountBefor = getTheBalanceOfDateBeforeAsset(mRGL.fixedAsset_Id, start);
            mRGL.Creditbefor = getThCreditBeforeAsset(mRGL.fixedAsset_Id, start);
            mRGL.Depitbefor = getTheDepitbeforAsset(mRGL.fixedAsset_Id, start);

            var alldata = db.GLDetails.Where(e => e.GLMaster.GL_Date >= start && e.GLMaster.GL_Date <= End && e.Asset_Id == mRGL.fixedAsset_Id&& (e.ChartOfAccounting.Account_Code.StartsWith("0102") || e.ChartOfAccounting.Account_Code.StartsWith("0202"))).ToList();
            foreach (var detail in alldata)
            {
                var detailMR = new ChildAccountGLDetail();
                detailMR.date = detail.GLMaster.GL_Date;
                detailMR.DocumentNumber = detail.GLMaster.GLCode;
                if (detail.Description_Detail == null || detail.Description_Detail == "")
                {
                    detailMR.Description = detail.GLMaster.GL_Desc;
                }
                else
                {
                    detailMR.Description = detail.Description_Detail;
                }
                detailMR.Amount = detail.GLDetail_Debit - detail.GLDetail_Credit;

                if (detail.GLMaster.GLCode.StartsWith("SAL-"))
                {
                    detailMR.dueDate = db.InvoiceMasters.Where(a => a.Serial_Number == detail.GLMaster.GLCode).FirstOrDefault().Due_Date;
                }
                if (detail.GLMaster.GLCode.StartsWith("PUR-"))
                {
                    detailMR.dueDate = db.PurInvoiceMasters.Where(a => a.Serial_Number == detail.GLMaster.GLCode).FirstOrDefault().Due_Date;
                }


                mRGL.details.Add(detailMR);


            }

            return mRGL;

        }
        public static decimal getTheBalanceOfDateBeforeAsset(int assetId, DateTime date)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.Asset_Id == assetId && a.GLMaster.GL_Date < date&&(a.ChartOfAccounting.Account_Code.StartsWith("0102")|| a.ChartOfAccounting.Account_Code.StartsWith("0202"))).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Debit).Sum() - glDetail.Select(a => a.GLDetail_Credit).Sum();

            }

            return 0;
        }
        public static decimal getTheBalanceOfDateBefore(int  Account_Id, DateTime date)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.Account_Child_ID == Account_Id && a.GLMaster.GL_Date < date).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Debit).Sum() - glDetail.Select(a => a.GLDetail_Credit).Sum();

            }

            return 0;
        }

        public static decimal getThCreditBeforeAsset(int assetId, DateTime date)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.Asset_Id == assetId && a.GLMaster.GL_Date < date&& (a.ChartOfAccounting.Account_Code.StartsWith("0102") || a.ChartOfAccounting.Account_Code.StartsWith("0202"))).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Credit).Sum() * -1;

            }

            return 0;
        }
        public static decimal getThCreditBefore(int Account_Id, DateTime date)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.Account_Child_ID == Account_Id && a.GLMaster.GL_Date < date).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Credit).Sum()*-1;

            }

            return 0;
        }
        public static decimal getThCreditBeforeForSubledger(int Subleger, DateTime date,string typeOfAccount)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.SubLedger_Id == Subleger && a.GLMaster.GL_Date < date && a.ChartOfAccounting.Account_Code.StartsWith(typeOfAccount)).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Credit).Sum() * -1;

            }

            return 0;
        }
        public static decimal getThDepitBeforeForSubledger(int Subleger, DateTime date, string typeOfAccount)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.SubLedger_Id == Subleger && a.GLMaster.GL_Date < date && a.ChartOfAccounting.Account_Code.StartsWith(typeOfAccount)).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Debit).Sum() ;

            }

            return 0;
        }



        public static decimal getTheDepitbefor(int Account_Id, DateTime date)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.Account_Child_ID == Account_Id && a.GLMaster.GL_Date < date).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Debit).Sum();

            }

            return 0;
        }

        public static decimal getTheDepitbeforAsset(int AssetId, DateTime date)
        {
            var db = new AccountingContainer();
            var glDetail = db.GLDetails.Where(a => a.Asset_Id == AssetId && a.GLMaster.GL_Date < date&& (a.ChartOfAccounting.Account_Code.StartsWith("0102") || a.ChartOfAccounting.Account_Code.StartsWith("0202"))).ToList();
            if (glDetail.Count() > 0)
            {

                return glDetail.Select(a => a.GLDetail_Debit).Sum();

            }

            return 0;
        }


        public static TrialBalanceNew NewTrialBalance(TrialBalanceNew Trial)
        {
            var db = new AccountingContainer();


            
          
            if (Trial.from == null)
            {
                Trial.from = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (Trial.to == null)
            {
                Trial.to = Convert.ToString(new DateTime(2200, 12, 30));

            }
            var StartDefaultDate = new DateTime(1995, 1, 1);
            DateTime start = Convert.ToDateTime(Convert.ToDateTime(Trial.from).ToShortDateString());
            DateTime End = Convert.ToDateTime(Convert.ToDateTime(Trial.to).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
            var GlDetail = db.GLDetails.Where(a => a.GLMaster.GL_Date <= End).ToList();
          
                if (Trial.OFFGL == false)
                {
                    var OffGL  = GlDetail.Where(a => a.GLMaster.GLCode.StartsWith("GLOF-")).ToList();

                         GlDetail = GlDetail.Except(OffGL).ToList();
                   

                }
           
            var AllAccountInthatPeriod = GlDetail.Select(a => a.Account_Id).Distinct().ToList();
            GlDetail= GlDetail.Where(e => e.GLMaster.GL_Date >= start && e.GLMaster.GL_Date <= End ).ToList();
            var asset= GlDetail.Select(a => a.Asset_Id).Distinct().ToList();
            var childAccount = GlDetail.Select(a => a.Account_Child_ID).Distinct().ToList();
            foreach (var Accountid in AllAccountInthatPeriod)
            {
                var detail = new TrialBalanceDetail();
                //if(Accountid==116)
                //{
                //    var a = 9;
                //    a++;
                //}
                detail.Account = db.ChartOfAccountings.Find(Accountid);
                if (Trial.OFFGL == false)
                {
                    detail.AmountBefor = Tax.getTheBalanceOfDateBeforewithoutOFF(detail.Account.Account_Code, start);
                }
                else
                {
                    detail.AmountBefor = Tax.getTheBalanceOfDateBefore(detail.Account.Account_Code, start);
                }
                detail.CeditMov = GlDetail.Where(a => a.Account_Id == Accountid).Select(a => a.GLDetail_Credit).ToList().Sum();
                detail.DepitMov = GlDetail.Where(a => a.Account_Id == Accountid).Select(a => a.GLDetail_Debit).ToList().Sum();
                detail.AmountOfPeriod = detail.DepitMov - detail.CeditMov;
                detail.LevelExpresion = new String('-', 1);

                detail.FinalAmount = detail.AmountBefor + detail.AmountOfPeriod;
                Trial.Detail.Add(detail);

            }
            if (Trial.ChildAccount == true)
            {
               var TrialAsset= AddingAsset(GlDetail, Trial.OFFGL,start);
                var AddtrialChild = AddingAssetChild(GlDetail, Trial.OFFGL,start);
                Trial.Detail.AddRange(TrialAsset);
                var FilterChild= AddtrialChild.Where(a => a.CeditMov != 0 || a.DepitMov != 0 || a.FinalAmount != 0 || a.AmountBefor != 0 ||a.ChildAccountd.Account_Id=="37"|| a.ChildAccountd.Account_Id == "45");
                Trial.Detail.AddRange(FilterChild);
            }
            var Trialo = Trial.Detail.Where(c => c.Asset == null && c.ChildAccountd == null).ToList();
            Trial.AmountBefor = Trialo.Select(z=>z.AmountBefor).Sum();
            Trial.CeditMov = Trialo.Select(z => z.CeditMov).Sum();
            Trial.DepitMov = Trialo.Select(z => z.DepitMov).Sum();
            Trial.AmountOfPeriod = Trialo.Select(z => z.AmountOfPeriod).Sum();
            Trial.FinalAmount = Trialo.Select(z => z.FinalAmount).Sum();
            PrintChartOfAccount printChartOfAccount = new PrintChartOfAccount();

            Trial.Detail = printChartOfAccount.AddToFatherAccout("0", Trial.Detail);

            if (Trial.FatherAccount == false)
            {

                Trial.Detail = Trial.Detail.Where(a => a.IshasAmount == true).ToList();
                Trial.Detail.Select(c => { c.LevelExpresion = "-"; return c; }).ToList();
            }

            return Trial;



        }

        public static List<TrialBalanceDetail> AddingAsset(List<GLDetail> gLDetails,bool offgl,DateTime start)
        {
            var TrilList = new List<TrialBalanceDetail>();
            var db = new AccountingContainer();
            gLDetails = gLDetails.Where(a => a.ChartOfAccounting.Account_Code.StartsWith("0102")).ToList();
            var AssetIds = db.FixedAssets.Select(a=>a.Fasset_ID);
            foreach (var assetId in AssetIds)
            {
                var detail = new TrialBalanceDetail();
                
               
                detail.Asset = db.FixedAssets.Find(assetId);
                detail.Account = db.ChartOfAccountings.Find(detail.Asset.FixedAssetTypeFAsset_TypeID);
                if (offgl == false)
                {
                    detail.AmountBefor = Tax.getTheBalanceAssetOfDateBeforewithoutOFF(assetId, start);
                }
                else
                {
                    detail.AmountBefor = Tax.getTheBalanceAssetOfDateBefore(assetId, start);
                }
                detail.CeditMov = gLDetails.Where(a => a.Asset_Id == assetId).Select(a => a.GLDetail_Credit).ToList().Sum();
                detail.DepitMov = gLDetails.Where(a => a.Asset_Id == assetId).Select(a => a.GLDetail_Debit).ToList().Sum();
                detail.AmountOfPeriod = detail.DepitMov - detail.CeditMov;
                detail.LevelExpresion = new String('-', 1);
                detail.IshasAmount = true;
                detail.FinalAmount = detail.AmountBefor + detail.AmountOfPeriod;
                TrilList.Add(detail);

            }
            return TrilList;
        }
        public static List<TrialBalanceDetail> AddingAssetChild(List<GLDetail> gLDetails,bool offgl,DateTime start)
        {
            var TrilList = new List<TrialBalanceDetail>();
            var db = new AccountingContainer();
            var childAccount = db.Accounts.Where(a=>a.Account_Code.StartsWith("CUST")||a.Account_Code.StartsWith("SUP")).Select(a => a.Accounts_Id).Distinct().ToList();
            foreach (var ChildId in childAccount)
            {
                var detail = new TrialBalanceDetail();
               

                detail.ChildAccountd = db.Accounts.Find(ChildId);
                detail.Account = db.ChartOfAccountings.Find(Convert.ToInt32(detail.ChildAccountd.Account_Id)); ;
                if (offgl == false)
                {
                    detail.AmountBefor = Tax.getTheBalanceChildOfDateBeforewithoutOFF(ChildId, start);
                }
                else
                {
                    detail.AmountBefor = Tax.getTheBalanceChildOfDateBefore(ChildId, start);
                }
                detail.CeditMov = gLDetails.Where(a => a.Account_Child_ID == ChildId).Select(a => a.GLDetail_Credit).ToList().Sum();
                detail.DepitMov = gLDetails.Where(a => a.Account_Child_ID == ChildId).Select(a => a.GLDetail_Debit).ToList().Sum();
                detail.AmountOfPeriod = detail.DepitMov - detail.CeditMov;
                detail.LevelExpresion = new String('-', 1);
                detail.IshasAmount = true;
                detail.FinalAmount = detail.AmountBefor + detail.AmountOfPeriod;
                TrilList.Add(detail);

            }
            var childAccountForEmployee = db.Accounts.Where(a => a.Account_Code.StartsWith("EMP")).Select(a => a.Accounts_Id).Distinct().ToList();
            foreach (var ChildId in childAccountForEmployee)
            {
                var detail = new TrialBalanceDetail();


                detail.ChildAccountd = db.Accounts.Find(ChildId);
                detail.Account = db.ChartOfAccountings.Find(Convert.ToInt32(50)); ;
                if (offgl == false)
                {
                    detail.AmountBefor = Tax.getTheBalanceChildOfDateBeforewithoutOFF(ChildId, start,50);
                }
                else
                {
                    detail.AmountBefor = Tax.getTheBalanceChildOfDateBefore(ChildId, start,50);
                }
                detail.CeditMov = gLDetails.Where(a => a.Account_Child_ID == ChildId&&a.Account_Id==50).Select(a => a.GLDetail_Credit).ToList().Sum();
                detail.DepitMov = gLDetails.Where(a => a.Account_Child_ID == ChildId&& a.Account_Id == 50).Select(a => a.GLDetail_Debit).ToList().Sum();
                detail.AmountOfPeriod = detail.DepitMov - detail.CeditMov;
                detail.LevelExpresion = new String('-', 1);
                detail.IshasAmount = true;
                detail.FinalAmount = detail.AmountBefor + detail.AmountOfPeriod;
                TrilList.Add(detail);


                var detail2 = new TrialBalanceDetail();


                detail2.ChildAccountd = db.Accounts.Find(ChildId);
                detail2.Account = db.ChartOfAccountings.Find(Convert.ToInt32(39)); ;
                if (offgl == false)
                {
                    detail2.AmountBefor = Tax.getTheBalanceChildOfDateBeforewithoutOFF(ChildId, start, 39);
                }
                else
                {
                    detail2.AmountBefor = Tax.getTheBalanceChildOfDateBefore(ChildId, start,39);
                }
                detail2.CeditMov = gLDetails.Where(a => a.Account_Child_ID == ChildId && a.Account_Id == 39).Select(a => a.GLDetail_Credit).ToList().Sum();
                detail2.DepitMov = gLDetails.Where(a => a.Account_Child_ID == ChildId && a.Account_Id == 39).Select(a => a.GLDetail_Debit).ToList().Sum();
                detail2.AmountOfPeriod = detail2.DepitMov - detail2.CeditMov;
                detail2.LevelExpresion = new String('-', 1);
                detail2.IshasAmount = true;
                detail2.FinalAmount = detail2.AmountBefor + detail2.AmountOfPeriod;
                TrilList.Add(detail2);

            }
            return TrilList;
        }


        public static StoreMovmentMaster getMovementVM()
        {

            var db = new AccountingContainer();
            StoreMovmentMaster store = new StoreMovmentMaster();
            store.storeList = new SelectList(db.Stores, "Store_ID", "Store_Name");
            return store;


        }
        public static StoreMovmentMaster getMovementVM(StoreMovmentMaster vm)
        {

            var db = new AccountingContainer();

            if (vm.endDate == null)
            {
                vm.endDate = Convert.ToString(new DateTime(2200, 12, 30));

            }
            vm.company = db.Companies.FirstOrDefault();
            vm.store = db.Stores.Find(vm.storeId);
            vm.MovmentDateilList = new List<StoreMovmentDateil>();

            var End = Convert.ToDateTime(Convert.ToDateTime(vm.endDate).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);
            var storeCardList = db.StoreCards.Where(a => a.Store_Id == vm.storeId && a.Date < End).ToList();
            foreach (var productid in storeCardList.Select(a => a.Product_Id).Distinct())
            {
                var storecard = storeCardList.Where(a => a.Product_Id == productid);
                var detail = new StoreMovmentDateil();
                detail.product = db.Products.Find(productid);
                detail.income = storecard.Where(a => a.IncreaseQuantity == true).Select(a => a.Quantity).Sum();
                detail.outgoing = storecard.Where(a => a.IncreaseQuantity == false).Select(a => a.Quantity).Sum();
                detail.balance = detail.income - detail.outgoing;
                vm.MovmentDateilList.Add(detail);
            }
            vm.MovmentDateilList = vm.MovmentDateilList.OrderBy(a => a.product.Prod_ID).ToList();
            return vm;

        }
        public static UnpaidInvoiceMasterVM GetAllUnPaidInvoice(string code)
        {
            var db = new AccountingContainer();
            UnpaidInvoiceMasterVM vm = new UnpaidInvoiceMasterVM();
            var Account = db.Accounts.Where(a => a.Account_Code == code).FirstOrDefault();
            vm.customer = db.Customers.Where(a => a.Customer_Code == code).FirstOrDefault();
            var gLDetails = db.GLDetails.Where(a => a.Account_Child_ID == Account.Accounts_Id);
            var Credit = gLDetails.Select(a => a.GLDetail_Credit).Sum();
            var Depit = gLDetails.Select(a => a.GLDetail_Debit).Sum();
            var balance = Depit - Credit;
            if (Math.Round( Depit,2,MidpointRounding.AwayFromZero) ==Math.Round( Credit,2,MidpointRounding.AwayFromZero))
            {
                return null;
            }
            else
            {
                vm.details = new List<UnpaidInvoiceVM>();
                gLDetails = gLDetails.Where(a=>a.GLDetail_Debit>0).OrderByDescending(a => a.GLMaster.GL_Date);
             foreach(var item in gLDetails)
                {
                    if (balance <= 0)
                    {

                        break;
                    }
                    if (item.GLMaster.GLCode.StartsWith("SAL-"))
                    {
                        var invoice = db.InvoiceMasters.Where(a => a.Serial_Number == item.GLMaster.GLCode).FirstOrDefault();
                        if(Math.Round(invoice.TotalAmountAfMat,2,MidpointRounding.AwayFromZero)!= 0&& Math.Round(invoice.TotalAmountAfMat, 2, MidpointRounding.AwayFromZero) <= Math.Round(invoice.Total_Amount, 2, MidpointRounding.AwayFromZero))
                        {
                            var detail = new UnpaidInvoiceVM();
                            var qrinvoice = MasterQRInvoice.GetQRMasterByGLCode(invoice.Serial_Number);
                            if (qrinvoice != null&&qrinvoice.Number!=null)
                            {
                                detail.serialNumber = qrinvoice.Number;
                            }
                            else
                            {
                                detail.serialNumber = item.GLMaster.GLCode;
                            }
                            detail.GLCode = item.GLMaster.GLCode;
                            detail.Date = invoice.Invoice_Date;
                            detail.DueDate = invoice.Due_Date.Value;
                            detail.invoiceValue = invoice.Total_Amount;
                            if ( balance>= invoice.TotalAmountAfMat)
                            {
                                detail.invoicePaid =  invoice.Total_Amount-invoice.TotalAmountAfMat;
                                detail.Remaining = invoice.TotalAmountAfMat;
                                balance -= invoice.TotalAmountAfMat;
                            }
                            else
                            {
                                detail.invoicePaid = (invoice.TotalAmountAfMat - balance);
                                detail.Remaining = invoice.TotalAmountAfMat - (invoice.TotalAmountAfMat - balance);
                                balance = 0;
                            }
                            vm.details.Add(detail);
                          
                            continue;
                        }
                        
                    }
                    else
                    {
                        var detail = new UnpaidInvoiceVM();
                       
                        detail.serialNumber = item.GLMaster.GLCode;
                        detail.Date = item.GLMaster.GL_Date;
                        detail.GLCode = item.GLMaster.GLCode;
                        detail.DueDate = null;
                        detail.invoiceValue = item.GLDetail_Debit;
                        if (balance >= item.GLDetail_Debit)
                        {
                            detail.invoicePaid = 0;
                            detail.Remaining = item.GLDetail_Debit;
                            balance -= item.GLDetail_Debit;
                        }
                        else
                        {
                            detail.invoicePaid = (item.GLDetail_Debit - balance);
                            detail.Remaining = item.GLDetail_Debit - (item.GLDetail_Debit - balance);
                            balance = 0;
                        }
                    
                        vm.details.Add(detail);
              
                    
                    }
                }
                vm.details = vm.details.OrderBy(a => a.Date).ToList();
                return vm;
            }
        }

    }
}



