using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class ReversePOSInvoice
    {
        /// <summary>
        /// chek that date is suitable
        /// </summary>
        /// <returns></returns>
        public static bool Checkdate(string date)
        {
            var datetime = Convert.ToDateTime(date);
            var db = new AccountingContainer();
            var Comstartdate = db.Companies.FirstOrDefault().StartFinancialDate;
            if (Comstartdate != null)
            {
                if (datetime < Comstartdate.Value)
                {
                    return false;
                }


            }

            return true;


        }
        /// <summary>
        /// Make index depend on user id
        /// </summary>
        /// <param name="user_Id"></param>
        /// <returns></returns>
        public static List<ReversePOSInvoice> getAllDataForReverseInvoice(string user_Id,int?all)
        {
            var db = new AccountingContainer();
            var List = new List<ReversePOSInvoice> { };
            var Emp = db.Employees.Where(a => a.Reg_ID == user_Id).FirstOrDefault();

            if (Emp != null&&Emp.SystemRoleId!=6)
            {

                var Point = db.PointOfSales.Where(a => a.Employee_Id == Emp.Emp_Id).FirstOrDefault();


                if (Point != null)
                {
                    List = db.ReversePOSInvoices.Where(a => a.POS_ID == Point.POS_Id).ToList();

                }
                else
                {

                    List = db.ReversePOSInvoices.ToList();
                }
            }
            else
            {
                  List = db.ReversePOSInvoices.ToList();
            }
            if (all == null)
            {
                List.Reverse();
                List = List.Take(100).ToList();
            }


            return List;



        }
        /// <summary>
        /// detail of Reverse invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ReversePOSInvoice getinvoiceByid(int id)
        {
            var db = new AccountingContainer();
            var invoice = db.ReversePOSInvoices.Find(id);
            return invoice;



        }
        /// <summary>
        /// get data for Revese invoice by its id
        /// </summary>
        /// <param name="invId"></param>
        /// <returns>MasterPOSInvoice</returns>
        public static MasterPOSInvoice GetDataForReverseInvoice(int invId)
        {
            return MasterPOSInvoice.getinvoiceByid(invId);

        }
        /// <summary>
        /// Prepare for Creation of that invoice
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>ReversePOSInvoiceVM</returns>
        public static ReversePOSInvoiceVM AddNewForCreation(String userId)
        {
     
            var db = new AccountingContainer();
            var masterVM = new ReversePOSInvoiceVM();
            var Emp = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
            if (Emp != null) {

                var Point = db.PointOfSales.Where(a => a.Employee_Id == Emp.Emp_Id).FirstOrDefault();
                if (Point != null&&Emp.SystemRoleId!=6)
                {

                  
                    masterVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
                    masterVM.MyCustomerList = new SelectList(db.Customers, "Customer_Id", "Customer_Name");
                    masterVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
                    masterVM.PointOfSaleList = new SelectList(db.PointOfSales.Where(a => a.Employee_Id == Emp.Emp_Id).ToList(), "POS_Id", "POS_Name");
                    
                    var lastCodebalance = db.ReversePOSInvoices.OrderByDescending(c => c.Master_Id).Where(e => e.Serial_Number != null).FirstOrDefault();
                    if (lastCodebalance == null)
                    {
                        masterVM.Serial_Number = "REVPOS-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        masterVM.Serial_Number = "REVPOS-" + (Convert.ToInt32(lastCodebalance.Serial_Number.Substring(7, lastCodebalance.Serial_Number.Length - 7)) + 1).ToString("D3");
                    }
                    //  masterVM.myBUnit = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");

                    var invoices = db.MasterPOSInvoices.Where(s => s.Invoice_State == TransactionState.Post && s.PointOfSalePOS_Id == Point.POS_Id).ToList();
                    //masterVM.purchInvList = new SelectList(invoices, "MasterPOSInvoiceId", "Serial_Number");

                    List<object> newListInvoice = new List<object>();
                   
                    foreach (var member in invoices)
                        newListInvoice.Add(new
                        {
                            Id = member.MasterPOSInvoiceId,
                            Name = member.Serial_Number+" "+member.Number
                        });
                    masterVM.purchInvList = new SelectList(newListInvoice, "Id", "Name");



                    List<object> newList = new List<object>();
                    var members = db.POSDetailPayments.Where(a=>a.PointOfSalePOS_Id==Point.POS_Id).ToList();
                    foreach (var member in members)
                        newList.Add(new
                        {
                            Id = member.PaymentMethod_Id,
                            Name = member.PaymentMethod_Name 
                        });
                    masterVM.PaymentMethodList = new SelectList(newList, "Id", "Name");
                    masterVM.Invoice_State = TransactionState.Pending;

                    var reversMasterTemp = new ReversePOSInvoiceTemp();
                    var id = (from s in db.ReversePOSInvoiceTemps
                              select s).ToList().LastOrDefault();
                    if (id == null)
                    { reversMasterTemp.Master_Id = 1; }
                    else
                    {
                        reversMasterTemp.Master_Id = id.Master_Id + 1;
                    }
                    masterVM.Invoice_Date = Convert.ToString(DateTime.Now);
                    db.ReversePOSInvoiceTemps.Add(reversMasterTemp);
                    db.SaveChanges();
                    masterVM.Master_Id = reversMasterTemp.Master_Id;
                    return masterVM;


                }
                else if(Emp.SystemRoleId == 6)
                {

                    masterVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
                    masterVM.MyCustomerList = new SelectList(db.Customers, "Customer_Id", "Customer_Name");
                    masterVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
                    masterVM.PointOfSaleList = new SelectList(db.PointOfSales.ToList(), "POS_Id", "POS_Name");

                    var lastCodebalance = db.ReversePOSInvoices.OrderByDescending(c => c.Master_Id).Where(e => e.Serial_Number != null).FirstOrDefault();
                    if (lastCodebalance == null)
                    {
                        masterVM.Serial_Number = "REVPOS-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        masterVM.Serial_Number = "REVPOS-" + (Convert.ToInt32(lastCodebalance.Serial_Number.Substring(7, lastCodebalance.Serial_Number.Length - 7)) + 1).ToString("D3");
                    }
                    //  masterVM.myBUnit = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");

                    var invoices = db.MasterPOSInvoices.Where(s => s.Invoice_State == TransactionState.Post).ToList();
                    masterVM.purchInvList = new SelectList(invoices, "MasterPOSInvoiceId", "Serial_Number");
                    List<object> newList = new List<object>();
                    var members = db.POSDetailPayments.ToList();
                    foreach (var member in members)
                        newList.Add(new
                        {
                            Id = member.PaymentMethod_Id,
                            Name = member.PaymentMethod_Name
                        });
                    masterVM.PaymentMethodList = new SelectList(newList, "Id", "Name");
                    masterVM.Invoice_State = TransactionState.Pending;

                    var reversMasterTemp = new ReversePOSInvoiceTemp();
                    var id = (from s in db.ReversePOSInvoiceTemps
                              select s).ToList().LastOrDefault();
                    if (id == null)
                    { reversMasterTemp.Master_Id = 1; }
                    else
                    {
                        reversMasterTemp.Master_Id = id.Master_Id + 1;
                    }
                    masterVM.Invoice_Date = Convert.ToString(DateTime.Now);
                    db.ReversePOSInvoiceTemps.Add(reversMasterTemp);
                    db.SaveChanges();
                    masterVM.Master_Id = reversMasterTemp.Master_Id;
                    return masterVM;

                }

            }

            return null;
        }

        /// <summary>
        /// get the detail of POS invoice where quantity not equal zero
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>List<DetailPOSInvoice></returns>
        public static List<DetailPOSInvoice>getDetailOfInvoice(int masterVmId)
        {
            var db = new AccountingContainer();

            var invoiceDetail = db.DetailPOSInvoices.Where(invoice => invoice.MasterPOSInvoiceId == masterVmId && invoice.Reverse_Quantity != 0).ToList();
            return invoiceDetail;



        }
        /// <summary>
        /// prepare detail of Revese POS 
        /// </summary>
        /// <param name="Detail_Id"></param>
        /// <returns>ReversePOSDetailVM</returns>

        public static ReversePOSDetailVM PrepareDetailForMakingReverse (int Detail_Id)
        {

            var db = new AccountingContainer();
            var SalesinvoiceMasterVM = new ReversePOSDetailVM();
          
            var thisdetails =db.DetailPOSInvoices.Find(Detail_Id);

            SalesinvoiceMasterVM.Product_Quantity = thisdetails.Reverse_Quantity;

            SalesinvoiceMasterVM.Product_Id = (int)thisdetails.Product_Id;
            SalesinvoiceMasterVM.Details_Id = (int)thisdetails.Detail_Id;
          

            SalesinvoiceMasterVM.Batch_ID = (int)thisdetails.Batch_ID.Value;
            SalesinvoiceMasterVM.SalesReceiptMasterTempMaster_Id = thisdetails.MasterPOSInvoiceId;

            SalesinvoiceMasterVM.Product_Price = thisdetails.Product_Price;
            SalesinvoiceMasterVM.Product_Tax = thisdetails.Product_Tax;
           
            List<object> newList = new List<object>();
            var members = db.Taxes.Where(a=>a.Tax_Code.StartsWith("02")).Where(a => a.IsActive != false);
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.Tax_ID,
                    Name = member.Tax_Name + "   " + member.Tax_Percentage + "%"
                });

            SalesinvoiceMasterVM.TaxList = new SelectList(newList, "Id", "Name");

            return SalesinvoiceMasterVM;


        }
        /// <summary>
        /// get all detail of Reverse invoice by master id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns></returns>
        public static List<ReversePOSDetailTemp> GetTempOfInvoice(int masterVmId,int delete)
        {
            var db = new AccountingContainer();
            var ReversDetailsTemp = db.ReversePOSInvoiceTemps.Find(masterVmId);
            if(delete == 1)
            {
                db.ReversePOSDetailTemps.RemoveRange(ReversDetailsTemp.ReversePOSDetailTemps);
                db.SaveChanges();
            }
            return ReversDetailsTemp.ReversePOSDetailTemps.ToList();
        }
        /// <summary>
        /// add to list and check the validation
        /// </summary>
        /// <param name="ReverseDetail"></param>
        /// <returns></returns>
        public static string AddReversSalesDetailsTemp(ReversePOSDetailVM ReverseDetail)
        {
           

            if (ReverseDetail.Product_Quantity <= 0 || ReverseDetail.Product_Quantity == null)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Addquantity;
            }
            var db = new AccountingContainer();

            var salesinvMasterTemp = db.ReversePOSInvoiceTemps.Find(ReverseDetail.Master_Id);
            var SalesRecipt = db.MasterPOSInvoices.Find(ReverseDetail.SalesReceiptMasterTempMaster_Id);
            var invoicedetail = SalesRecipt.DetailPOSInvoices.Where(a => a.Product_Id == ReverseDetail.Product_Id).ToList().FirstOrDefault();
            var SalesinMasterDTemp = new ReversePOSDetailTemp();
            SalesinMasterDTemp.Product_Id = ReverseDetail.Product_Id;
            SalesinMasterDTemp.Product_Quantity = ReverseDetail.Product_Quantity;
            SalesinMasterDTemp.Product_Price = invoicedetail.Product_Price;
            SalesinMasterDTemp.Product_Tax = invoicedetail.Product_Tax;
            var dtails = salesinvMasterTemp.ReversePOSDetailTemps;
            var dublicateinTemp = dtails.Where(z => z.ReversePOSInvoiceTempMaster_Id == salesinvMasterTemp.Master_Id && z.Product_Id == SalesinMasterDTemp.Product_Id).ToList();
            if (dublicateinTemp.FirstOrDefault() != null)
            {
              return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.thesameProduct;
            }
            
            if (invoicedetail.Reverse_Quantity < ReverseDetail.Product_Quantity)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.QuantityIsBigger;

            }
            else
            {
                var batch = db.ProductBatches.Where(a => a.Product_ID == ReverseDetail.Product_Id).FirstOrDefault();
                SalesinMasterDTemp.Batch_ID = batch.Batch_ID;
                
                decimal TotalBeforeDiscount = invoicedetail.Product_Price * ReverseDetail.Product_Quantity;
                decimal discount = 0;
                if (invoicedetail.Detail_Discount != null)
                {
                    discount = TotalBeforeDiscount * (invoicedetail.Detail_Discount.Value / 100);
                    SalesinMasterDTemp.Detail_Discount = invoicedetail.Detail_Discount;

                }
                else
                {
                    SalesinMasterDTemp.Detail_Discount = SalesRecipt.Master_Discount;
                    discount = TotalBeforeDiscount * (SalesRecipt.Master_Discount.Value / 100);
                }
                SalesinMasterDTemp.Detail_Discount_Amount = discount;
                
                decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                if (invoicedetail.Product_Tax == 0)
                {
                    SalesinMasterDTemp.Detail_Total_Tax = 0;
                }
                else
                {
                    var Tax = db.Taxes.Find(Convert.ToInt32(invoicedetail.Product_Tax));
                    SalesinMasterDTemp.Detail_Total_Tax = TotalAfterDiscount * (Tax.Tax_Percentage / 100);
                }
                
                SalesinMasterDTemp.Total_Amount = (TotalBeforeDiscount + SalesinMasterDTemp.Detail_Total_Tax) - discount;
               
              
                salesinvMasterTemp.ReversePOSDetailTemps.Add(SalesinMasterDTemp);
                db.Entry(salesinvMasterTemp).State = EntityState.Modified;
                db.SaveChanges();

                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Addedsuccessfully;
            }
        }
        /// <summary>
        /// add Reverse Pos in data base
        /// </summary>
        /// <param name="mastersalesVM"></param>
        /// <param name="UserId"></param>
        /// <returns>String</returns>
        public static int AddNewRecallsalesInvoice(ReversePOSInvoiceVM mastersalesVM , String UserId)
        {
            var db = new AccountingContainer();


            var revinvoiceMasterTemp = db.ReversePOSInvoiceTemps.Find(mastersalesVM.Master_Id);
            var master = new ReversePOSInvoice();


            var lastCodebalance = db.ReversePOSInvoices.OrderByDescending(c => c.Master_Id).Where(e => e.Serial_Number != null).FirstOrDefault();
            if (lastCodebalance == null)
            {
                master.Serial_Number = "REVPOS-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                master.Serial_Number = "REVPOS-" + (Convert.ToInt32(lastCodebalance.Serial_Number.Substring(7, lastCodebalance.Serial_Number.Length - 7)) + 1).ToString("D3");
            }

            master.Invoice_Date = Convert.ToDateTime(mastersalesVM.Invoice_Date);
            // master.Due_Date = Convert.ToDateTime(mastersalesVM.Due_Date);

            var Allreverse = db.ReversePOSInvoices.Where(a => a.MasterPOSInvoiceMasterPOSInvoiceId== mastersalesVM.salesRecipt_Id && a.Invoice_State == TransactionState.Pending).FirstOrDefault();
            if (Allreverse != null)
            {
                return 0;
            }
            else
            {
                var resultInv = db.MasterPOSInvoices.Find(mastersalesVM.salesRecipt_Id);
                if (resultInv != null)
                {
                    
                   
                  
                    master.SubLedger_ID = resultInv.Subledger_Id;
                    
                    master.Customer_Id = resultInv.Customer_Id;
                    master.Company_Id = resultInv.Company_Id.Value;
                    master.Invoice_Currency = resultInv.Invoice_Currency;
                    master.Store_ID = resultInv.Store_ID.Value;
                    master.POS_ID = resultInv.PointOfSalePOS_Id;
                }
               
                master.Invoice_Date = Convert.ToDateTime(mastersalesVM.Invoice_Date);
                master.Invoice_State = TransactionState.Pending;
                master.Master_Discount = db.MasterPOSInvoices.Find(mastersalesVM.salesRecipt_Id).Master_Discount;
                master.Account_Id = db.MasterPOSInvoices.Find(mastersalesVM.salesRecipt_Id).Account_Id.Value;
                //   master.PaymentMethod_Id = db.SalesReceiptMasters.Find(mastersalesVM.salesRecipt_Id).PaymentMethod_Id;
                master.MasterPOSInvoiceMasterPOSInvoiceId = mastersalesVM.salesRecipt_Id;
               
                
                decimal Sumtax = 0;
                decimal SumtAmount = 0;
                decimal SumTotalCost = 0;
                List<ReversePOSDetailTemp> List = new List<ReversePOSDetailTemp> { };
                if (revinvoiceMasterTemp.ReversePOSDetailTemps.ToList().FirstOrDefault() != null)
                {
                    foreach (var item in revinvoiceMasterTemp.ReversePOSDetailTemps.ToList())
                    {
                        var details = new ReversePOSDetail();
                        details.Details_Id = item.Details_Id;
                        details.Product_Quantity = item.Product_Quantity;
                        details.Product_Price = item.Product_Price;
                        details.Product_Tax = item.Product_Tax;
                        details.Detail_Discount = item.Detail_Discount;
                        details.Detail_Discount_Amount = item.Detail_Discount_Amount;

                        details.Total_Amount += details.Detail_Total_Tax;
                        try
                        {

                            details.Batch_ID = db.ProductBatches.Where(a => a.Store_ID == master.Store_ID && a.Product_ID == item.Product_Id).FirstOrDefault().Batch_ID;
                        }
                        catch
                        {

                            details.Batch_ID = db.ProductBatches.Where(a => a.Product_ID == item.Product_Id).FirstOrDefault().Batch_ID;

                        }
                        ProductBatches detailBatch = db.ProductBatches.Find(details.Batch_ID);
                        details.Product_Id = detailBatch.Product_ID;

                        var detail = resultInv.DetailPOSInvoices.ToList();

                        foreach (var productCost in detail)
                        {
                            if (item.Product_Id == productCost.Product_Id)
                            {

                                item.Detail_Total_Cost = productCost.Detail_Total_Cost / productCost.Product_Quantity;
                            }



                        }



                        details.Detail_Total_Cost = (item.Detail_Total_Cost * details.Product_Quantity);

                        decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                        decimal discount = TotalBeforeDiscount * (item.Detail_Discount.Value / 100);
                        details.Detail_Discount = item.Detail_Discount;
                        details.Detail_Discount_Amount = item.Detail_Discount_Amount;

                        decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                        if (item.Product_Tax == 0)
                        {

                        }
                        else
                        {
                            var tax = Convert.ToDecimal(db.Taxes.Find(item.Product_Tax).Tax_Percentage);
                            details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                        }

                        var price = details.Product_Price * details.Product_Quantity;
                        details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                        SumtAmount += (decimal)details.Total_Amount;

                        Sumtax += (decimal)details.Detail_Total_Tax;
                        SumTotalCost += (decimal)details.Detail_Total_Cost;
                        master.Total_Tax = Sumtax;
                        master.Total_Amount = SumtAmount;
                        master.Total_Cost = SumTotalCost;



                        List.Add(item);


                        master.ReversePOSDetails.Add(details);


                    }
                    master.PaymentMethod_Id = mastersalesVM.PaymentMethod_Id;
                    if (mastersalesVM.PaymentMethod_Id > 0)
                    {
                        var payment = db.PaymentMethods.Find(master.PaymentMethod_Id);
                        master.Total_Payment_Method_Cost = (master.Total_Amount * (payment.Percentage.Value / 100));
                    }
                    //master.Total_Amount = master.Total_Amount + (master.Total_Amount * (payment.Percentage.Value) / 100);
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            master.Number = InvoiceSerial.generateSerialForReverse();
                            master.Serial_Number = "REVPOS-" + System.Text.RegularExpressions.Regex.Match(master.Number, @"\d+").Value;
                       
                            db.ReversePOSInvoices.Add(master);
                            db.ReversePOSDetailTemps.RemoveRange(List);
                            db.ReversePOSInvoiceTemps.Remove(revinvoiceMasterTemp);


                            DateTime DateNow = DateTime.Now;
                            master.Invoice_State = TransactionState.Post;
                            master.Master_Discount = (master.ReversePOSDetails.Select(a => a.Detail_Discount_Amount).Sum() / master.ReversePOSDetails.Select(a => a.Product_Quantity * a.Product_Price).Sum()) * 100;
                        
                        
                            db.ReversePOSInvoices.Add(master);
                            db.SaveChanges();
                            var gl = CreateInvoiceGL(master);
                            var transBatch = new TransactionBatch();
                            MasterPOSInvoice.MakeSalesinQrSales(master);
                            transBatch.Date = DateNow;
                            transBatch.OperationName = "مردود مبيعات نقطة بيع";
                            transBatch.OperationId = master.Master_Id;
                            transBatch.Operation_Code = master.Serial_Number;
                            transBatch.Description = "مردود مبيعات نقطة بيع";
                            transBatch.UserId = UserId;
                            db.TransactionBatches.Add(transBatch);
                            transaction.Commit();
                            db.Dispose();
                        }
                        catch
                        {
                            transaction.Rollback();
                            db.Dispose();
                        }

                        return master.Master_Id;
                    }
               
                }
            }


            return 0;

        }
        /// <summary>
        /// prepare for making gl
        /// </summary>
        /// <param name="RevsalesinvoiceMasterVM"></param>
        /// <returns>string</returns>
        public static string UpdateInvoiceMaster(ReversePOSInvoiceVM RevsalesinvoiceMasterVM)
        {
            var db = new AccountingContainer();

            ReversePOSInvoice resalesmaster = db.ReversePOSInvoices.Find(RevsalesinvoiceMasterVM.Master_Id);
            OperationRun.Addnewoperation(resalesmaster.Serial_Number);
            var operation = db.OperationRuns.Where(a => a.Code == resalesmaster.Serial_Number).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(resalesmaster.Serial_Number);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }
            // Edit master invoice state to the new state

            if (resalesmaster.Invoice_State == RevsalesinvoiceMasterVM.Invoice_State)
            {
                OperationRun.DeleteOperation(resalesmaster.Serial_Number);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + resalesmaster.Invoice_State + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            else
            {
                resalesmaster.Invoice_State = RevsalesinvoiceMasterVM.Invoice_State;
            }
          
            var msg = "";
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Entry(resalesmaster).State = EntityState.Modified;
                    // Create GL for the invoice 
                    msg = CreateInvoiceGL(resalesmaster);
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    db.Dispose();
                }
            }
            OperationRun.DeleteOperation(resalesmaster.Serial_Number);
            db.Dispose();
            return msg;
        }
        /// <summary>
        /// make gl for that invoice what ever Post or void
        /// </summary>
        /// <param name="master"></param>
        /// <returns>String</returns>
        public static String CreateInvoiceGL(ReversePOSInvoice master)
        {
            var msg = "";
            var db = new AccountingContainer();
            GLMaster glMaster = new GLMaster();
            glMaster.GLCode = master.Serial_Number;
            glMaster.GL_Date = master.Invoice_Date;
            MasterPOSInvoice invoiceMaster = db.MasterPOSInvoices.Find(master.MasterPOSInvoiceMasterPOSInvoiceId);
            var detailInvoice = invoiceMaster.DetailPOSInvoices.ToList();
            glMaster.GL_Desc = invoiceMaster.Description;
            glMaster.Company_Id = master.Company_Id;
            glMaster.GLType_Id = "مردود مبيعات سريعة";
            glMaster.Currency_Id = "الريال السعودى";
            glMaster.GLState = TransactionState.Pending;
            glMaster.Auto = true;

            GLDetail AccountReceivableGL = new GLDetail(),
               revenueGL = new GLDetail(),

               inventoryGL = new GLDetail(),
               totalCostGL = new GLDetail(),
                 AccountCustomerGL = new GLDetail(),
                AccountCustomerGLDebit = new GLDetail(),
        bankcomution = new GLDetail(),
      discount = new GLDetail();
            //var BusId = db.BussinesUnits.FirstOrDefault().BU_Id;
            AccountReceivableGL.Bu_Id =
            revenueGL.Bu_Id =
            inventoryGL.Bu_Id =
            totalCostGL.Bu_Id =
             discount.Bu_Id =
            bankcomution.Bu_Id = 0;
            if (master.PaymentMethod_Id > 0) { 
                var bankAccount = db.PaymentMethods.Find(master.PaymentMethod_Id);

            var ChartOfAccounRecive = db.ChartOfAccountings.Find(bankAccount.Bank_Account_Id);
            AccountReceivableGL.Account_Id = ChartOfAccounRecive.Account_Id;
            AccountReceivableGL.Account_Name = ChartOfAccounRecive.Account_Name;
        }
            var ChartOfAccounRevenue = db.ChartOfAccountings.Find(77);
            revenueGL.Account_Id = ChartOfAccounRevenue.Account_Id;
            revenueGL.Account_Name = ChartOfAccounRevenue.Account_Name;

            

            var store = db.Stores.Find(master.Store_ID);
            var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).First();
            inventoryGL.Account_Id = ChartOfAccounInventory.Account_Id;
            inventoryGL.Account_Name = ChartOfAccounInventory.Account_Name;

            var ChartOfAccounCost = db.ChartOfAccountings.Find(105);
            totalCostGL.Account_Id = ChartOfAccounCost.Account_Id;
            totalCostGL.Account_Name = ChartOfAccounCost.Account_Name;

            var DiscountCost = db.ChartOfAccountings.Find(78);
            discount.Account_Id = DiscountCost.Account_Id;
            discount.Account_Name = DiscountCost.Account_Name;


            discount.SubLedger_Id =
            revenueGL.SubLedger_Id =
            bankcomution.SubLedger_Id =
            totalCostGL.SubLedger_Id = master.SubLedger_ID;
            if (master.PaymentMethod_Id > 0)
            {

                var a = db.PaymentMethods.Find(master.PaymentMethod_Id).Expense_Account_Id;
                if (a != null)
                {
                    var bank = db.ChartOfAccountings.Find(a);
                    bankcomution.Account_Id = bank.Account_Id;
                    bankcomution.Account_Name = bank.Account_Name;
                }
            }
            AccountReceivableGL.Asset_Id =
           revenueGL.Asset_Id =
          
           inventoryGL.Asset_Id =
            discount.Asset_Id =
           totalCostGL.Asset_Id = 0;
            if (master.Customer_Id != null)
            {
                var ChartOfAccounPay = db.ChartOfAccountings.Find(37);
                AccountCustomerGL.Account_Id = ChartOfAccounPay.Account_Id;
                AccountCustomerGL.Account_Name = ChartOfAccounPay.Account_Name;
                AccountCustomerGLDebit.Account_Id = ChartOfAccounPay.Account_Id;
                AccountCustomerGLDebit.Account_Name = ChartOfAccounPay.Account_Name;
                var customer = db.Customers.Find(master.Customer_Id);
                var customerSubledger = db.Accounts.Where(k => k.Account_Name == customer.Customer_Name).FirstOrDefault();
                AccountCustomerGL.Account_Child_ID = customerSubledger.Accounts_Id;
                AccountCustomerGL.Account_CHild_Name = customerSubledger.Account_Name;
                AccountCustomerGLDebit.Account_Child_ID = customerSubledger.Accounts_Id;
                AccountCustomerGLDebit.Account_CHild_Name = customerSubledger.Account_Name;
            }
            var RealDetails = getMaterialOCompoundProduct(master.ReversePOSDetails.ToList(), master.Store_ID);
            master.Total_Cost = 0;
            switch (master.Invoice_State)
            {
                //Post Invoice
                case TransactionState.Post:
                    // Customer GL
                    glMaster.GLState = TransactionState.Post;
                    var gl = db.GLMasters.Where(y => y.GLCode == glMaster.GLCode && y.GLState == glMaster.GLState).FirstOrDefault();
                    if (gl != null)
                    {
                        return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                    }

                    // Revenue GL

                    // Taxes GL
                    if (master.PaymentMethod_Id > 0)
                    {
                        

                        AccountCustomerGLDebit.GLDetail_Debit = master.Total_Amount;
                        var pay = db.PaymentMethods.Find(master.PaymentMethod_Id);
                        //bankcomution.GLDetail_Credit = master.Total_Payment_Method_Cost;
                        bankcomution.GLDetail_Credit = 0;
                        AccountReceivableGL.GLDetail_Credit = master.Total_Amount;
                    }
                    AccountCustomerGL.GLDetail_Credit = master.Total_Amount;
                    // Inventory GL

                    

                    foreach (var detail in master.ReversePOSDetails)
                    {
                        var TotalBefore = detail.Product_Price * detail.Product_Quantity;
                        revenueGL.GLDetail_Debit += TotalBefore;
                    }
                    discount.GLDetail_Credit = revenueGL.GLDetail_Debit * (master.Master_Discount.Value / 100);

                    db.ProductBatches.Load();
                    foreach (var detail in master.ReversePOSDetails)
                    {
                        //تعديل فى قيم الفاتورة بناءا على المرتجع
                        var record = detailInvoice.Where(m => m.Product_Id == detail.Product_Id).FirstOrDefault();
                        record.Reverse_Quantity = record.Reverse_Quantity - detail.Product_Quantity;
                        record.Reverse_Total_Tax = record.Reverse_Total_Tax - detail.Detail_Total_Tax;
                        record.Reverse_Total_Amount = record.Reverse_Total_Amount - detail.Total_Amount;
                        db.Entry(record).State = EntityState.Modified;
                        db.SaveChanges();

                        
                       
                     
                    }
                    foreach(var detail in RealDetails)
                    {
                        ProductBatches detailBatch = db.ProductBatches.Find(detail.Batch_ID);
                        if (detailBatch.Product.Prod_Type == Prod_Type.اولي || detailBatch.Product.Prod_Type == Prod_Type.منتج)
                        {
                            var storeCardOLd = db.StoreCards.Where(z => z.Product_Id == detail.Product_Id && z.Document_Serial == invoiceMaster.Serial_Number).FirstOrDefault();
                            detailBatch.Product_Quantity += detail.Product_Quantity;

                            detailBatch.Batch_Cost += detail.Product_Quantity * storeCardOLd.CostPrice;
                            detailBatch.Product_Cost = (detailBatch.Batch_Cost / detailBatch.Product_Quantity);

                            db.Entry(detailBatch).State = EntityState.Modified;


                            var storecard = new StoreCard();
                            storecard.CostPrice = storeCardOLd.CostPrice;
                            storecard.Date = DateTime.Now;
                            storecard.Store_Id = master.Store_ID;
                            storecard.Quantity = detail.Product_Quantity;
                            storecard.SalePrice = detail.Product_Price;
                            storecard.Product_Id = detail.Product_Id;
                            storecard.IncreaseQuantity = true;
                            storecard.TypeOfDocument = " فاتورة مردود نقطة بيع";
                            storecard.Document_Serial = master.Serial_Number;
                            storecard.TotalCost = detail.Product_Quantity * storeCardOLd.CostPrice;
                          
                            storecard.TotalAmount = detail.Total_Amount;
                            storecard.Account_Id = AccountReceivableGL.Account_Child_ID;
                            storecard.Subledger_Id = Convert.ToInt32(master.SubLedger_ID);
                            Store.AddinginstorCard(storecard);
                            master.Total_Cost += storecard.TotalCost.Value;
                        }
                    }
                    // Total Cost GL
                    inventoryGL.GLDetail_Debit = master.Total_Cost;
                    totalCostGL.GLDetail_Credit = master.Total_Cost;
                    break;
                // Void Invoice
                case TransactionState.Void:
                    glMaster.GLType_Id = "مردود مبيعات سريعة عكسي  ";
                    glMaster.GLState = TransactionState.Void;
                    var glChck = db.GLMasters.Where(y => y.GLCode == glMaster.GLCode && y.GLState == glMaster.GLState).FirstOrDefault();
                    if (glChck != null)
                    {
                        return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                    }
                    var errorMessage = new List<string> { };
                    foreach (var detail in RealDetails)
                    {
                        var thisproduct = db.Products.Find(detail.Product_Id);
                        if (thisproduct.Prod_Type==Prod_Type.اولي||thisproduct.Prod_Type == Prod_Type.منتج)
                        {
                            ProductBatches detailBatch = db.ProductBatches.Where(c => c.Product_ID == detail.Product_Id && c.Store_ID == master.Store_ID).ToList().FirstOrDefault();
                            
                                if (detail.Product_Quantity > detailBatch.Product_Quantity&& detailBatch.Product.Prod_Type==Prod_Type.منتج)
                                {
                                    var productName = db.Products.Find(detail.Product_Id).Prod_Name;
                                    master.Invoice_State = TransactionState.Post;
                                //db.SaveChanges();
                                errorMessage.Add(Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Thereisnotenoughproduct + productName);

                            }
                            }
                        
                    }
                    if (errorMessage.Count() != 0)
                    {


                        string combindedString = string.Join("\n", errorMessage.ToArray());
                        return combindedString;
                    }
                    if (master.HasOff == true)

                    {
                        msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.UnderCloseing;
                        master.Invoice_State = TransactionState.Post;
                        db.SaveChanges();
                        return msg;

                    }
                    else
                    {
                        // Customer GL

                        // Revenue GL

                        // Taxes GL
                        if (master.PaymentMethod_Id > 0)
                        {
                            AccountCustomerGL.GLDetail_Credit = master.Total_Amount;
                            var pay2 = db.PaymentMethods.Find(master.PaymentMethod_Id);

                            //bankcomution.GLDetail_Debit = master.Total_Payment_Method_Cost;
                            bankcomution.GLDetail_Debit = 0;
                            AccountReceivableGL.GLDetail_Debit = master.Total_Amount;
                        }
                        AccountCustomerGLDebit.GLDetail_Debit = master.Total_Amount;
                        
                        // Inventory GL
                  
                       
                        foreach (var detail in master.ReversePOSDetails)
                        {
                            var TotalBefore = detail.Product_Price * detail.Product_Quantity;
                            revenueGL.GLDetail_Credit += TotalBefore;
                        }
                        discount.GLDetail_Debit = revenueGL.GLDetail_Credit * (master.Master_Discount.Value / 100);
                        // Update store quantity
                        foreach (var detail in master.ReversePOSDetails)
                        {
                            var record = detailInvoice.Where(m => m.Product_Id == detail.Product_Id).FirstOrDefault();
                            record.Reverse_Quantity = record.Reverse_Quantity + detail.Product_Quantity;
                            record.Reverse_Total_Tax = record.Reverse_Total_Tax + detail.Detail_Total_Tax;
                            record.Reverse_Total_Amount = record.Reverse_Total_Amount + detail.Total_Amount;
                            db.Entry(record).State = EntityState.Modified;
                            db.SaveChanges();


                            
                            

                        }
                        foreach(var detail in RealDetails)
                        {
                            ProductBatches detailBatch = db.ProductBatches.Find(detail.Batch_ID);
                            //detailBatch.Product_Quantity -= detail.Product_Quantity;
                            //detailBatch.Batch_Cost -= detailBatch.Product_Cost * detail.Product_Quantity;
                            if (detailBatch.Product.Prod_Type == Prod_Type.اولي | detailBatch.Product.Prod_Type == Prod_Type.منتج)
                            {
                                var storeCardOLd = db.StoreCards.Where(x => x.Product_Id == detail.Product_Id && x.Document_Serial == invoiceMaster.Serial_Number).FirstOrDefault();
                                detailBatch.Product_Quantity -= detail.Product_Quantity;
                                detailBatch.Batch_Cost -= detail.Product_Quantity * storeCardOLd.CostPrice;
                                if (detailBatch.Product_Quantity == 0)
                                {
                                    detailBatch.Product_Cost = 0;
                                }
                                else
                                {
                                    detailBatch.Product_Cost = (detailBatch.Batch_Cost / detailBatch.Product_Quantity);
                                }
                                db.Entry(detailBatch).State = EntityState.Modified;


                                var storecard = new StoreCard();
                                if (detail.Product_Quantity == 0)
                                {
                                    storecard.CostPrice = 0;
                                }
                                else
                                {
                                    storecard.CostPrice = detail.Detail_Total_Cost / detail.Product_Quantity;
                                }

                                storecard.Date = DateTime.Now;
                                storecard.Store_Id = master.Store_ID;
                                storecard.Quantity = detail.Product_Quantity;
                                storecard.SalePrice = detail.Product_Price;
                                storecard.Product_Id = detail.Product_Id;
                                storecard.IncreaseQuantity = false;
                                storecard.TypeOfDocument = " استبعاد مردود نقطة بيع ";
                                storecard.Document_Serial = master.Serial_Number;
                                storecard.TotalCost = detail.Product_Quantity * storeCardOLd.CostPrice;
                                storecard.CostPrice = storeCardOLd.CostPrice;
                                storecard.TotalAmount = detail.Total_Amount;
                                storecard.Account_Id = AccountReceivableGL.Account_Child_ID;
                                storecard.Subledger_Id = Convert.ToInt32(master.SubLedger_ID);
                                Store.AddinginstorCard(storecard);
                                master.Total_Cost += storecard.TotalCost.Value;
                            }
                        }
                        // Total Cost GL
                        totalCostGL.GLDetail_Debit = master.Total_Cost;
                        inventoryGL.GLDetail_Credit = master.Total_Cost;
                    }
                    break;
                default:
                    break;
            }
            if (msg == "")
            {
                // Add all GL details to the master
                if (master.PaymentMethod_Id > 0)
                {
                    glMaster.GLDetails.Add(AccountReceivableGL);
                }
                glMaster.GLDetails.Add(revenueGL);
                
                glMaster.GLDetails.Add(inventoryGL);
                glMaster.GLDetails.Add(totalCostGL);
                if (bankcomution.GLDetail_Credit == 0 && bankcomution.GLDetail_Debit == 0)
                {

                }
                else
                {
                    glMaster.GLDetails.Add(bankcomution);

                }
                if (discount.GLDetail_Credit == 0 && discount.GLDetail_Debit == 0)
                {

                }
                else
                {
                    glMaster.GLDetails.Add(discount);

                }
                if (master.Customer_Id > 0)
                {
                    if (AccountCustomerGL.GLDetail_Debit + AccountCustomerGL.GLDetail_Credit > 0)
                    {
                        glMaster.GLDetails.Add(AccountCustomerGL);
                    }
                    if (AccountCustomerGLDebit.GLDetail_Debit + AccountCustomerGLDebit.GLDetail_Credit > 0)
                    {
                        glMaster.GLDetails.Add(AccountCustomerGLDebit);
                    }
                }
                glMaster = addglDetailTax(glMaster, master);
                db.GLMasters.Add(glMaster);
                db.SaveChanges();
                GLMaster.GetBalanceForAccount(glMaster);
                GLMaster.GetBalanceForSubLedger(glMaster);
                msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
               
            }
            if (master.Invoice_State == TransactionState.Post)
            {
                msg = Resources.validate.GLDone;
            }
            else
            {
                msg = Resources.validate.GLReverse;
            }
            return msg;
        }
        public static List<ReversePOSDetail> getMaterialOCompoundProduct(List<ReversePOSDetail> Details, int Store_Id)
        {
            var db = new AccountingContainer();
            var ProductAll = db.Products.ToList();
            var project = ProductAll.Where(a => a.Prod_Type == Prod_Type.مجمع).Select(a => a.Prod_ID).ToList();
            var ProList = Details.Where(s => project.Contains(s.Product_Id)).ToList();
            if (ProList.Count() == 0)
            {

            }
            else
            {

                foreach (var item in ProList)
                {
                    var SubProductList = ProductAll.Where(a => a.Prod_ID == item.Product_Id).FirstOrDefault().ProductCompounds;
                    foreach (var subProduct in SubProductList)
                    {
                        var dublicate = Details.Where(a => a.Product_Id == subProduct.Product_Id).FirstOrDefault();
                        var batch = db.ProductBatches.Where(a => a.Product_ID == subProduct.Product_Id && a.Store_ID == Store_Id).FirstOrDefault();
                        if (dublicate == null)
                        {
                            var Detailnew = new ReversePOSDetail();
                            Detailnew.Product_Id = subProduct.Product_Id;
                            Detailnew.Product_Quantity = subProduct.Quantity * item.Product_Quantity;

                            Detailnew.Batch_ID = batch.Batch_ID;

                            if (batch.Batch_Cost > 0)
                            {
                                Detailnew.Detail_Total_Cost = (batch.Batch_Cost / batch.Product_Quantity) * Detailnew.Product_Quantity;

                            }
                            else
                            {
                                Detailnew.Detail_Total_Cost = 0;


                            }
                            Details.Add(Detailnew);

                        }
                        else
                        {


                            var Detailnew = new ReversePOSDetail();
                            Detailnew.Product_Id = subProduct.Product_Id;
                            Detailnew.Product_Quantity = dublicate.Product_Quantity + (item.Product_Quantity * subProduct.Quantity);

                            Detailnew.Batch_ID = batch.Batch_ID;

                            if (batch.Batch_Cost > 0)
                            {
                                Detailnew.Detail_Total_Cost = (batch.Batch_Cost / batch.Product_Quantity) * Detailnew.Product_Quantity;

                            }
                            else
                            {
                                Detailnew.Detail_Total_Cost = 0;


                            }
                            Details.Remove(dublicate);
                            Details.Add(Detailnew);

                        }
                        var Delete = Details.Where(a => a.Product_Id == subProduct.Master_Product_Id).FirstOrDefault();

                        Details.Remove(Delete);

                    }





                }

            }

            return Details;
        }
        public static string deleteItemFomDetails(int id)
        {
            var db = new AccountingContainer();
            var thisitem = db.ReversePOSDetailTemps.Find(Convert.ToInt32(id));
            db.ReversePOSDetailTemps.Remove(thisitem);
            db.SaveChanges();
            db.Dispose();
             return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.deleteFromList;
        }
        
        public static GLMaster addglDetailTax(GLMaster gLMaster, ReversePOSInvoice invoice)
        {
            var db = new AccountingContainer();
            var invoiceDetail = invoice.ReversePOSDetails;
            var numberofdifferentTax = invoiceDetail.Where(a => a.Product_Tax != 0).Select(a => a.Product_Tax).Distinct();
            if (numberofdifferentTax.Count() == 0)
            {
                return gLMaster;
            }
            foreach (var tax in numberofdifferentTax)
            {
                var gldetail = new GLDetail();
                var thistax = db.Taxes.Find(tax).Tax_Code;
                var taxChart = db.ChartOfAccountings.Where(a => a.Account_Code == thistax).FirstOrDefault();

                gldetail.Account_Id = taxChart.Account_Id;
                gldetail.Account_Name = taxChart.Account_Name;
                if (gLMaster.GLState == TransactionState.Post)
                {
                    gldetail.GLDetail_Credit = 0;
                    gldetail.GLDetail_Debit = invoiceDetail.Where(a => a.Product_Tax == tax).Select(a => a.Detail_Total_Tax).Sum();
                }
                else
                {


                    gldetail.GLDetail_Credit = invoiceDetail.Where(a => a.Product_Tax == tax).Select(a => a.Detail_Total_Tax).Sum();
                    gldetail.GLDetail_Debit = 0;
                }

                gLMaster.GLDetails.Add(gldetail);
            }

            return gLMaster;
        }
    }
}