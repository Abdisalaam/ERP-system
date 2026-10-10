using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class ReverseSalesReceiptMaster
    {
        /// <summary>
        /// check that code is unique or not 
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns>bool</returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.ReverseSalesReceiptMasters.ToList();
            foreach (var item in allinData)
            {
                if (item.Serial_Number == Serial || !Serial.StartsWith("REVSa-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }
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
        /// getList for All Reverse invoice 
        /// </summary>
        /// <returns>List<ReverseSalesReceiptMaster></returns>
        public static List<ReverseSalesReceiptMaster> GetAllRevInvoices(int ?all)
        {
            var db = new AccountingContainer();
            var orders = db.ReverseSalesReceiptMasters.ToList();
            if (all == null)
            {
                orders.Reverse();
                orders = orders.Take(100).ToList();
            }
            return orders;
        }
        /// <summary>
        /// get Detail for that invoice by master id
        /// </summary>
        /// <param name="detailId"></param>
        /// <returns>ReverseSalesReceiptMaster</returns>
        public static ReverseSalesReceiptMaster GetrevInvoiceDetailsById(int detailId)
        {
            var db = new AccountingContainer();
            var detail = db.ReverseSalesReceiptMasters.Find(detailId);
            return detail;

        }
        /// <summary>
        /// fill fore Createion anew Reverse invoice 
        /// </summary>
        /// <returns></returns>
        public static ReverseSalesReceiptMasterVM AddNewForCreation()
        {

            var db = new AccountingContainer();
            var masterVM = new ReverseSalesReceiptMasterVM();
            masterVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            masterVM.MyCustomerList = new SelectList(db.Customers.Where(a => a.Customer_State != Customer_State.غير_نشط), "Customer_Id", "Customer_Name");
            masterVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            var lastCodebalance = db.ReverseSalesReceiptMasters.OrderByDescending(c => c.Master_Id).Where(e => e.Serial_Number != null).FirstOrDefault();
            if (lastCodebalance == null)
            {
                masterVM.Serial_Number = "REVSa-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                masterVM.Serial_Number = "REVSa-" + (Convert.ToInt32(lastCodebalance.Serial_Number.Substring(6, lastCodebalance.Serial_Number.Length - 6)) + 1).ToString("D3");
            }
            //  masterVM.myBUnit = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            masterVM.MyProductsList = new SelectList(db.Products, "Prod_ID", "Prod_Name");
            masterVM.mystoreList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            var InvoiceList = db.SalesReceiptMasters.Where(i => i.Invoice_State != TransactionState.Void);
            //masterVM.purchInvList = new SelectList(db.InvoiceMasters.Where(i=>i.Invoice_State == TransactionState.Post), "Master_Id", "Serial_Number");
            List<object> newListInvoice = new List<object>();

            foreach (var member in InvoiceList)
                newListInvoice.Add(new
                {
                    Id = member.Master_Id,
                    Name = member.Serial_Number + " " + member.Number
                });
            masterVM.purchInvList = new SelectList(newListInvoice, "Id", "Name");

           // masterVM.purchInvList = new SelectList(db.SalesReceiptMasters.Where(s=>s.Invoice_State == TransactionState.Post), "Master_Id", "Serial_Number");
            List<object> newList = new List<object>();
            var members = db.PaymentMethods.Where(a => a.IsActive != false).ToList();
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.PaymentMethod_Id,
                    Name = member.Payment_Type 
                });
            masterVM.PaymentMethodList = new SelectList(newList, "Id", "Name");
            masterVM.Invoice_State = TransactionState.Pending;

            var reversMasterTemp = new ReverseSalesReceiptMasterTemp1();
            var id = (from s in db.ReverseSalesReceiptMasterTemp1
                      select s).ToList().LastOrDefault();
            if (id == null)
            { reversMasterTemp.Master_Id = 1; }
            else
            {
                reversMasterTemp.Master_Id = id.Master_Id + 1;
            }
            masterVM.Invoice_Date = Convert.ToString( DateTime.Now);
            db.ReverseSalesReceiptMasterTemp1.Add(reversMasterTemp);
            db.SaveChanges();
            masterVM.Master_Id = reversMasterTemp.Master_Id;
            return masterVM;
        }

        /// <summary>
        /// prepare for adding to List for Reverse invoice 
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="tempSalesMasterId"></param>
        /// <returns></returns>
        public static ReverseSalesReceiptDetailsVM PrepareReverseMasterDetail(int serial, string tempSalesMasterId)
        {

            var db = new AccountingContainer();
            var SalesinvoiceMasterVM = new ReverseSalesReceiptDetailsVM();
            string[] words = tempSalesMasterId.Split(' ');
            int first = Convert.ToInt32(words.FirstOrDefault());
            int sec = Convert.ToInt32(words.LastOrDefault());
            var thisdetails = GetAllTempReverseSalesInvoiceDetails(serial).Where(e=>e.Details_Id==sec).ToList().FirstOrDefault();

            SalesinvoiceMasterVM.Product_Quantity = thisdetails.Reverse_Quantity;

            SalesinvoiceMasterVM.Product_Id = (int)thisdetails.Product_Id;



            SalesinvoiceMasterVM.Details_Id = thisdetails.Details_Id;
            SalesinvoiceMasterVM.Product_Price = thisdetails.Product_Price;
            SalesinvoiceMasterVM.Product_Tax = thisdetails.Product_Tax;
            SalesinvoiceMasterVM.SalesReceiptMasterTempMaster_Id = first;
            List<object> newList = new List<object>();
            var members = db.Taxes.ToList().Where(a => a.IsActive != false);
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
        /// get detail for Sales Recipt to make Reverse 
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>List<SalesReceiptDetails></returns>
        public static List<SalesReceiptDetails> GetAllTempReverseSalesInvoiceDetails(int masterVmId)
        {
            var db = new AccountingContainer();

            var ReversDetailsTemp = db.SalesReceiptDetails.Where(invoice => invoice.Master_Id == masterVmId && invoice.Reverse_Quantity != 0).ToList();
            return ReversDetailsTemp;



        }
        /// <summary>
        /// get list for Reverse temp by using its id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>List<ReverseSalesReceiptDetailsTemp></returns>
        public static List<ReverseSalesReceiptDetailsTemp> GetAllTempReverseSalesInvoiceDetailsresult(int masterVmId,int delete)
        {
            var db = new AccountingContainer();
            var ReversDetailsTemp = db.ReverseSalesReceiptMasterTemp1.Find(masterVmId);
            if (delete == 1)
            {
                db.ReverseSalesReceiptDetailsTemps.RemoveRange(ReversDetailsTemp.SalesReceiptDetailsTemps);
                db.SaveChanges();
            }
            return ReversDetailsTemp.SalesReceiptDetailsTemps.ToList();
        }
        /// <summary>
        /// add to list and check validation on it
        /// </summary>
        /// <param name="ReverseDetail"></param>
        /// <returns>string</returns>
        public static string AddReversSalesDetailsTemp(ReverseSalesReceiptDetailsVM ReverseDetail)
        {
           

            if (ReverseDetail.Product_Quantity <= 0 || ReverseDetail.Product_Quantity == null)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Addquantity;
            }
            var db = new AccountingContainer();

            var salesinvMasterTemp = db.ReverseSalesReceiptMasterTemp1.Find(ReverseDetail.Master_Id);
            var SalesRecipt = db.SalesReceiptMasters.Find(ReverseDetail.SalesReceiptMasterTempMaster_Id);
            var detailsales = SalesRecipt.SalesReceiptDetails.Where(a => a.Product_Id == ReverseDetail.Product_Id).FirstOrDefault();
            var SalesinMasterDTemp = new ReverseSalesReceiptDetailsTemp();
            SalesinMasterDTemp.Product_Id = ReverseDetail.Product_Id;
            SalesinMasterDTemp.Product_Quantity = ReverseDetail.Product_Quantity;
            SalesinMasterDTemp.Product_Price = detailsales.Product_Price;
            SalesinMasterDTemp.Product_Tax = detailsales.Product_Tax;
            var dtails = salesinvMasterTemp.SalesReceiptDetailsTemps;
            var dublicateinTemp = dtails.Where(z => z.SalesReceiptMasterTempMaster_Id == salesinvMasterTemp.Master_Id && z.Product_Id == SalesinMasterDTemp.Product_Id).ToList();
            if (dublicateinTemp.FirstOrDefault() != null)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.thesameProduct;
            }
            var invoicedetail = SalesRecipt.SalesReceiptDetails.Where(a => a.Product_Id == SalesinMasterDTemp.Product_Id).ToList().FirstOrDefault();
            if (invoicedetail.Reverse_Quantity < ReverseDetail.Product_Quantity)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.QuantityIsBigger;

            }
            else
            {
                //var batch = db.ProductBatches.Where(a => a.Product_ID == ReverseDetail.Product_Id).FirstOrDefault();
                //SalesinMasterDTemp.Batch_ID = batch.Batch_ID;
                decimal TotalBeforeDiscount = detailsales.Product_Price * ReverseDetail.Product_Quantity;
                decimal discount = 0;
                if (detailsales.Detail_Discount != null)
                {
                     discount = TotalBeforeDiscount * (detailsales.Detail_Discount.Value / 100);
                    SalesinMasterDTemp.Detail_Discount = detailsales.Detail_Discount.Value;
                }
                else
                {
                     discount = TotalBeforeDiscount * (SalesRecipt.Master_Discount / 100);
                    SalesinMasterDTemp.Detail_Discount = SalesRecipt.Master_Discount;
                }
                detailsales.Detail_Discount_Amount = discount;
                decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                if (detailsales.Product_Tax == 0)
                {
                    SalesinMasterDTemp.Detail_Total_Tax = 0;
                }
                else
                {
                    var Tax = db.Taxes.Find(Convert.ToInt32(detailsales.Product_Tax));
                    SalesinMasterDTemp.Detail_Total_Tax = TotalAfterDiscount * (Tax.Tax_Percentage / 100);
                }
               
               
                
                SalesinMasterDTemp.Total_Amount = (TotalBeforeDiscount + SalesinMasterDTemp.Detail_Total_Tax) - discount;
                SalesinMasterDTemp.Commition_Id = Convert.ToInt32(ReverseDetail.payment);
                var payment = db.PaymentMethods.Find(ReverseDetail.payment);
                if (ReverseDetail.payment != 0 && ReverseDetail.payment != null)
                {
                    var com = db.PaymentMethods.Find(ReverseDetail.payment);
                    if (com.Percentage == 0 && com.Operatin_Cost == 0)
                    {
                        SalesinMasterDTemp.Commition = 0;

                    }
                    else
                    {
                        SalesinMasterDTemp.Commition = (com.Percentage / 100) * SalesinMasterDTemp.Total_Amount;
                        //  invoiceMasterDetailsTemp.Total_Amount = invoiceMasterDetailsTemp.Total_Amount + invoiceMasterDetailsTemp.comition.Value;
                    }


                }
                salesinvMasterTemp.SalesReceiptDetailsTemps.Add(SalesinMasterDTemp);
                db.Entry(salesinvMasterTemp).State = EntityState.Modified;
                db.SaveChanges();

                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Addedsuccessfully;
            }
        }
        public static string deleteItemFomDetails(int id)
        {
            var db = new AccountingContainer();
            var thisitem = db.ReverseSalesReceiptDetailsTemps.Find(Convert.ToInt32(id));
            db.ReverseSalesReceiptDetailsTemps.Remove(thisitem);
            db.SaveChanges();
            db.Dispose();
            return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.deleteFromList;
        }
        /// <summary>
        /// get that file by its id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>ReverseSalesReceiptFile</returns>
        public static ReverseSalesReceiptFile getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.ReverseSalesReceiptFiles.Find(fileId);
            return file;
        }
        public static string deleteFile(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.ReverseSalesReceiptFiles.Find(fileId);
            db.ReverseSalesReceiptFiles.Remove(file);
            db.SaveChanges();
                 return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.deleteFile;
        }

        /// <summary>
        /// get list of Reverse file by its master id
        /// </summary>
        /// <param name="ReverseSalesMasterId"></param>
        /// <returns> List<ReverseSalesReceiptFile></returns>
        public static List<ReverseSalesReceiptFile> GetAllFiles(int ReverseSalesMasterId)
        {
            var db = new AccountingContainer();
            var files = db.ReverseSalesReceiptFiles.Where(emp => emp.ReverseSalesReceiptMasterMaster_Id == ReverseSalesMasterId).ToList();
            return files;
        }
        /// <summary>
        /// Create anew Reverse Sales and saving in db
        /// </summary>
        /// <param name="mastersalesVM"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static Tuple<String,int> AddNewRecallsalesInvoice(ReverseSalesReceiptMasterVM mastersalesVM , String UserId)
        {
            var db = new AccountingContainer();


            var revinvoiceMasterTemp = db.ReverseSalesReceiptMasterTemp1.Find(mastersalesVM.Master_Id);
            var master = new ReverseSalesReceiptMaster();

          
                var lastCodebalance = db.ReverseSalesReceiptMasters.OrderByDescending(c => c.Master_Id).Where(e => e.Serial_Number != null).FirstOrDefault();
                if (lastCodebalance == null)
                {
                    master.Serial_Number = "REVSa-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    master.Serial_Number = "REVSa-" + (Convert.ToInt32(lastCodebalance.Serial_Number.Substring(6, lastCodebalance.Serial_Number.Length - 6)) + 1).ToString("D3");
                }
                    var comp = db.Companies.Find(mastersalesVM.Company_Id);
                    if (comp != null)
                    {
                        var date = Convert.ToDateTime(mastersalesVM.Invoice_Date);
                        if (date < comp.StartFinancialDate)
                        {
                            master.Invoice_Date = DateTime.Now;
                        }
                        else
                        {
                            master.Invoice_Date = Convert.ToDateTime(mastersalesVM.Invoice_Date);
                        }
                    }
                    else
                    {
                        master.Invoice_Date = DateTime.Now;
                    }
                // master.Invoice_Date = Convert.ToDateTime(mastersalesVM.Invoice_Date);
                // master.Due_Date = Convert.ToDateTime(mastersalesVM.Due_Date);
                
                var Allreverse = db.ReverseSalesReceiptMasters.Where(a => a.SalesRecipt_Id == mastersalesVM.salesRecipt_Id && a.Invoice_State == TransactionState.Pending).FirstOrDefault();
                if (Allreverse != null)
                {
                    return Tuple.Create( Allreverse.Serial_Number + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.ThierMoreInvoiceForThat, 0);
                }
                else
                {
                    var resultInv = db.SalesReceiptMasters.Find(mastersalesVM.salesRecipt_Id);
                    if (resultInv != null)
                    {
                        master.Master_Id = resultInv.Master_Id;
                        master.Branch_Id = resultInv.Branch_Id;
                        master.Department_ID = resultInv.Department_ID;
                        master.SubLedger_ID = resultInv.SubLedger_ID;
                        master.BussinesUnit_ID = resultInv.BussinesUnit_ID;
                    master.Customer_Id = resultInv.Customer_Id;
                    master.Company_Id = resultInv.Company_Id;
                    master.Invoice_Currency = resultInv.Invoice_Currency;
                    master.Store_ID = resultInv.Store_ID;
                    master.SalesRecipt_Id = resultInv.Master_Id;

                }
               
                master.Invoice_Date = Convert.ToDateTime(mastersalesVM.Invoice_Date);
                    master.Invoice_State = TransactionState.Pending;
                   
                    master.Account_Id = db.SalesReceiptMasters.Find(mastersalesVM.salesRecipt_Id).Account_Id;
                    master.Branch_Id = db.SalesReceiptMasters.Find(mastersalesVM.salesRecipt_Id).Branch_Id;
                    if (mastersalesVM.Files.FirstOrDefault() != null)
                    {
                        foreach (var file in mastersalesVM.Files)
                        {
                            var filedetails = new ReverseSalesReceiptFile();
                            var data = new byte[file.ContentLength];
                            filedetails.FileName = file.FileName;
                            filedetails.ContentLength = file.ContentLength;
                            file.InputStream.Read(data, 0, file.ContentLength);
                            filedetails.FileContent = data;
                            filedetails.FileType = file.ContentType;
                        if (data.Length < 1000000)
                        {
                            master.ReverseSalesReceiptFiles.Add(filedetails);
                        }
                       
                        }
                    }
                    decimal Sumtax = 0;
                    decimal SumtAmount = 0;
                    decimal SumTotalCost = 0;
                if (revinvoiceMasterTemp.SalesReceiptDetailsTemps.ToList().FirstOrDefault() != null)
                {
                    foreach (var item in revinvoiceMasterTemp.SalesReceiptDetailsTemps.ToList())
                    {
                        var details = new ReverseSalesReceiptDetails();
                        details.Details_Id = item.Details_Id;
                        details.Product_Quantity = item.Product_Quantity;
                        details.Product_Price = item.Product_Price;
                        details.Product_Tax = item.Product_Tax;
                        details.Detail_Discount = item.Detail_Discount;
                        details.Detail_Discount_Amount =( item.Product_Quantity * item.Product_Price)*(item.Detail_Discount/100);

                        details.Total_Amount += details.Detail_Total_Tax;
                        details.Batch_ID = item.Batch_ID;
                        ProductBatches detailBatch = db.ProductBatches.Find(details.Batch_ID);
                        details.Product_Id = item.Product_Id;

                        var detail = resultInv.SalesReceiptDetails.ToList();

                        foreach (var productCost in detail)
                        {
                            if (item.Product_Id == productCost.Product_Id)
                            {

                                item.Detail_Total_Cost = productCost.Detail_Total_Cost / productCost.Product_Quantity;
                            }



                        }

                        decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                        decimal discount = TotalBeforeDiscount * (item.Detail_Discount.Value / 100);
                        decimal TotalAfterDiscount = TotalBeforeDiscount - discount;


                        details.Detail_Total_Cost = (item.Detail_Total_Cost * details.Product_Quantity);
                        if (item.Product_Tax == 0)
                        {
                            details.Detail_Total_Tax = 0;
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


                        db.ReverseSalesReceiptDetailsTemps.Remove(item);



                        master.ReverseSalesReceiptDetails.Add(details);


                    }
                    if (mastersalesVM.PaymentMethod_Id!=null&& mastersalesVM.PaymentMethod_Id != 0)
                    {
                        master.PaymentMethod_Id = mastersalesVM.PaymentMethod_Id.Value;
                        var payment = db.PaymentMethods.Find(master.PaymentMethod_Id);
                        master.Total_Payment_Method_Cost = (master.Total_Amount * (payment.Percentage.Value / 100));
                    }
                  else if(resultInv.Customer_Id!=0&& resultInv.Customer_Id != null)
                    {
                        master.PaymentMethod_Id = 0;
                        
                        master.Total_Payment_Method_Cost = 0;
                    }
                    else
                    {
                        master.PaymentMethod_Id = db.SalesReceiptMasters.Find(master.SalesRecipt_Id).PaymentMethod_Id;
                        var payment = db.PaymentMethods.Find(master.PaymentMethod_Id);
                        master.Total_Payment_Method_Cost = (master.Total_Amount * (payment.Percentage.Value / 100));
                    }
                    try
                    {
                        master.Master_Discount = (master.ReverseSalesReceiptDetails.Select(a => a.Detail_Discount_Amount).Sum() / master.ReverseSalesReceiptDetails.Select(a => a.Product_Price*a.Product_Quantity).Sum()) * 100;
                    }
                    catch
                    {
                        master.Master_Discount = db.SalesReceiptMasters.Find(mastersalesVM.salesRecipt_Id).Master_Discount;
                    }
                    master.Invoice_State = TransactionState.Pending;
                    master.Number = InvoiceSerial.generateSerialForReverse();
                    master.Serial_Number = "REVSa-" + System.Text.RegularExpressions.Regex.Match(master.Number, @"\d+").Value;
                    db.ReverseSalesReceiptMasterTemp1.Remove(revinvoiceMasterTemp);
                    DateTime DateNow = DateTime.Now;
                    db.ReverseSalesReceiptMasters.Add(master);
                    db.SaveChanges();
                    SalesReceiptMaster.MakeSalesinQrSales(master);
                    var transBatch = new TransactionBatch();
                    transBatch.Date = DateNow;
                    transBatch.OperationName = "مردود مبيعات نقدية";
                    transBatch.OperationId = master.Master_Id;
                    transBatch.Operation_Code = master.Serial_Number;
                    transBatch.Description = "مردود مبيعات نقدية";
                    transBatch.UserId = UserId;
                    db.TransactionBatches.Add(transBatch);
                    db.SaveChanges();
                    db.Dispose();

                    return Tuple.Create(Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Therefundinvoicehasbeencreatedsuccessfully, master.Master_Id);
                  
                }
                return Tuple.Create(Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.EmptyInvoiceModelMessage, 0);
              
                }


            
           
        }
        /// <summary>
        /// get sales Recipt by its id
        /// </summary>
        /// <param name="invId"></param>
        /// <returns></returns>
        public static SalesReceiptMaster GetrevsalesInvoice(int invId)
        {
            return SalesReceiptMaster.GetSalesReceiptMasterById(invId);

        }
        /// <summary>
        /// make gl of that invoice by status and id
        /// </summary>
        /// <param name="RevsalesinvoiceMasterVM"></param>
        /// <returns></returns>
        public static string UpdateInvoiceMaster(ReverseSalesReceiptMasterVM RevsalesinvoiceMasterVM)
        {
            var db = new AccountingContainer();

            ReverseSalesReceiptMaster resalesmaster = db.ReverseSalesReceiptMasters.Find(RevsalesinvoiceMasterVM.Master_Id);
            OperationRun.Addnewoperation(resalesmaster.Serial_Number);
            var operation = db.OperationRuns.Where(a => a.Code == resalesmaster.Serial_Number).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(resalesmaster.Serial_Number);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }
            if (resalesmaster.Invoice_State == RevsalesinvoiceMasterVM.Invoice_State)
            {
                OperationRun.DeleteOperation(resalesmaster.Serial_Number);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + resalesmaster.Invoice_State + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            else
            {
                resalesmaster.Invoice_State = RevsalesinvoiceMasterVM.Invoice_State;
            }
            // Edit master invoice state to the new state
          
            
        

            // Create GL for the invoice 
            var msg = "";
            var revQr = db.ReverseQRInvoices.Where(a => a.Serial_Number == resalesmaster.Serial_Number).FirstOrDefault();
            if (revQr != null)
            {
                revQr.Invoice_State = resalesmaster.Invoice_State;
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Entry(resalesmaster).State = EntityState.Modified;
                    if (revQr != null)
                    {
                        db.Entry(revQr).State = EntityState.Modified;
                    }
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
        /// make gl and check quantity and closed in void status
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        public static String CreateInvoiceGL(ReverseSalesReceiptMaster master)
        {
            var msg = "";
            var db = new AccountingContainer();
            GLMaster glMaster = new GLMaster();
            glMaster.GLCode = master.Serial_Number;
            glMaster.GL_Date = master.Invoice_Date;
            SalesReceiptMaster invoiceMaster = db.SalesReceiptMasters.Find(master.SalesRecipt_Id);
            var detailInvoice = invoiceMaster.SalesReceiptDetails.ToList();
            glMaster.GL_Desc = invoiceMaster.Description;
            glMaster.Company_Id = master.Company_Id;
            glMaster.GLType_Id = "مردود مبيعات نقدية";
            glMaster.Currency_Id = "الريال السعودى";
            glMaster.GLState = TransactionState.Pending;
            glMaster.Auto = true;
            GLDetail AccountReceivableGL = new GLDetail(),
               revenueGL = new GLDetail(),
               
               inventoryGL = new GLDetail(),
               totalCostGL = new GLDetail(),
        bankcomution = new GLDetail(),
          AccountCustomerGL = new GLDetail(),
                AccountCustomerGLDebit = new GLDetail(),
      discount = new GLDetail();
            //var BusId = db.BussinesUnits.FirstOrDefault().BU_Id;
            AccountReceivableGL.Bu_Id =
            revenueGL.Bu_Id =
            inventoryGL.Bu_Id =
            totalCostGL.Bu_Id =
             discount.Bu_Id =
            bankcomution.Bu_Id = 0;
            if (master.PaymentMethod_Id != 0 && master.PaymentMethod_Id != null)
            {
                var bankAccount = db.PaymentMethods.Find(master.PaymentMethod_Id);

                var ChartOfAccounRecive = db.ChartOfAccountings.Find(bankAccount.Bank_Account_Id);
                AccountReceivableGL.Account_Id = ChartOfAccounRecive.Account_Id;
                AccountReceivableGL.Account_Name = ChartOfAccounRecive.Account_Name;
            }
            else if(master.Customer_Id!=null&&master.Customer_Id!=0) {

                var ChartOfAccounRecive = db.ChartOfAccountings.Find(37);
                AccountReceivableGL.Account_Id = ChartOfAccounRecive.Account_Id;
                AccountReceivableGL.Account_Name = ChartOfAccounRecive.Account_Name;
                var cust = db.Customers.Find(master.Customer_Id).Customer_Code;
                var cusAccount = db.Accounts.Where(g => g.Account_Code == cust).FirstOrDefault();
                AccountReceivableGL.Account_Child_ID = cusAccount.Accounts_Id;
                AccountReceivableGL.Account_CHild_Name = cusAccount.Account_Name;
            }
            else
            {
                var salesRecipt = db.SalesReceiptMasters.Find(master.SalesRecipt_Id);
                var bankAccount = db.PaymentMethods.Find(salesRecipt.PaymentMethod_Id);

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

            if (master != null && master.PaymentMethod_Id != 0)
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
            if (master.Customer_Id != 0&&master.Customer_Id != null&& master.PaymentMethod_Id!=0)
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
            var RealDetails = getMaterialOCompoundProduct(master.ReverseSalesReceiptDetails.ToList(), master.Store_ID);
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
                    AccountCustomerGL.GLDetail_Credit = master.Total_Amount;

                    AccountCustomerGLDebit.GLDetail_Debit = master.Total_Amount;
                    // Revenue GL

                    // Taxes GL
                    
                    // Inventory GL
                 
                    var pay = db.PaymentMethods.Find(master.PaymentMethod_Id);
                    bankcomution.GLDetail_Credit =0;
                    AccountReceivableGL.GLDetail_Credit = master.Total_Amount ;

                    foreach (var detail in master.ReverseSalesReceiptDetails)
                    {
                        var TotalBefore = detail.Product_Price * detail.Product_Quantity;
                        revenueGL.GLDetail_Debit += TotalBefore;
                    }
                    discount.GLDetail_Credit= revenueGL.GLDetail_Debit * (master.Master_Discount.Value / 100);

                    db.ProductBatches.Load();
                    foreach (var detail in master.ReverseSalesReceiptDetails)
                    {
                        //تعديل فى قيم الفاتورة بناءا على المرتجع
                        var record = detailInvoice.Where(m => m.Product_Id == detail.Product_Id).FirstOrDefault();
                        record.Reverse_Quantity = record.Reverse_Quantity - detail.Product_Quantity;
                        record.Reverse_Total_Tax = record.Reverse_Total_Tax - detail.Detail_Total_Tax;
                        record.Reverse_Total_Amount = record.Reverse_Total_Amount - detail.Total_Amount;
                        db.Entry(record).State = EntityState.Modified;
                        db.SaveChanges();
                       
                    }
                    foreach (var detail in RealDetails)
                    {
                        var thisproduct = db.Products.Find(detail.Product_Id);
                        if (thisproduct.Prod_Type == Prod_Type.اولي || thisproduct.Prod_Type == Prod_Type.منتج)
                        {


                            var storecard = new StoreCard();

                            ProductBatches detailBatch = db.ProductBatches.Where(p => p.Product_ID == detail.Product_Id && p.Store_ID == master.Store_ID).FirstOrDefault();
                            //detailBatch.Product_Quantity += detail.Product_Quantity;
                            //detailBatch.Batch_Cost += detailBatch.Product_Cost * detail.Product_Quantity;
                            if (detailBatch == null)
                            {
                                var batch = new ProductBatches();
                                batch.Store_ID = master.Store_ID;
                                batch.Product_ID = thisproduct.Prod_ID;
                                batch.IsActive = true;
                                db.ProductBatches.Add(batch);
                                db.SaveChanges();
                                detailBatch = batch;
                            }

                            if (detailBatch.Store_ID != null)
                            {
                                var storeCardOLd = db.StoreCards.Where(a => a.Product_Id == detail.Product_Id && a.Document_Serial == invoiceMaster.Serial_Number).FirstOrDefault();
                                if (storeCardOLd == null)
                                {
                                    if (detailBatch.Product_Quantity > 0)
                                    {
                                        storecard.CostPrice = detailBatch.Batch_Cost / detailBatch.Product_Quantity;
                                    }
                                    else
                                    {
                                        storecard.CostPrice = thisproduct.Prod_Cost_Price.Value;
                                    }
                                    detailBatch.Product_Quantity += detail.Product_Quantity;

                                    detailBatch.Batch_Cost += detail.Product_Quantity * storecard.CostPrice;
                                    detailBatch.Product_Cost = storecard.CostPrice;

                                    storecard.TotalCost = detail.Product_Quantity * storecard.CostPrice;
                                }
                                else
                                {
                                    detailBatch.Product_Quantity += detail.Product_Quantity;

                                    detailBatch.Batch_Cost += detail.Product_Quantity * storeCardOLd.CostPrice;
                                    detailBatch.Product_Cost = (detailBatch.Batch_Cost / detailBatch.Product_Quantity);
                                    storecard.CostPrice = storeCardOLd.CostPrice;
                                    storecard.TotalCost = detail.Product_Quantity * storeCardOLd.CostPrice;
                                }
                                db.Entry(detailBatch).State = EntityState.Modified;





                                storecard.Date = DateTime.Now;
                                storecard.Store_Id = master.Store_ID;
                                storecard.Quantity = detail.Product_Quantity;
                                storecard.SalePrice = detail.Product_Price;
                                storecard.Product_Id = detail.Product_Id;
                                storecard.IncreaseQuantity = true;
                                storecard.TypeOfDocument = "فاتورة مردود فورية";
                                storecard.Document_Serial = master.Serial_Number;

                                storecard.TotalAmount = detail.Total_Amount;
                                storecard.Account_Id = AccountReceivableGL.Account_Child_ID;
                                storecard.Subledger_Id = Convert.ToInt32(master.SubLedger_ID);
                                Store.AddinginstorCard(storecard);
                                master.Total_Cost += storecard.TotalCost.Value;

                            }
                        }
                    }
                    // Total Cost GL
                    totalCostGL.GLDetail_Credit = master.Total_Cost;
                    inventoryGL.GLDetail_Debit = master.Total_Cost;
                    break;
                // Void Invoice
                case TransactionState.Void:
                    glMaster.GLType_Id = "مردود مبيعات نقدية عكسي";
                    glMaster.GLState = TransactionState.Void;
                    var glcheck = db.GLMasters.Where(s => s.GLCode == glMaster.GLCode && s.GLState == glMaster.GLState).FirstOrDefault();
                    if (glcheck != null)
                    {
                        return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                    }
                    var errorMessage = new List<string> { };
                    foreach (var detail in RealDetails)
                    {
                        var thisproduct = db.Products.Find(detail.Product_Id);

                        ProductBatches detailBatch = db.ProductBatches.Where(c=> c.Product_ID == detail.Product_Id && c.Store_ID == master.Store_ID).ToList().FirstOrDefault();
                        if (detailBatch!=null&& detailBatch.Store_ID != null)
                        {
                            if (detail.Product_Quantity > detailBatch.Product_Quantity)
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
                        AccountCustomerGL.GLDetail_Credit = master.Total_Amount;

                        AccountCustomerGLDebit.GLDetail_Debit = master.Total_Amount;

                        // Inventory GL
                      
                        var pay2 = db.PaymentMethods.Find(master.PaymentMethod_Id);

                        bankcomution.GLDetail_Debit = master.Total_Payment_Method_Cost;
                        AccountReceivableGL.GLDetail_Debit = master.Total_Amount;
                        foreach (var detail in master.ReverseSalesReceiptDetails)
                        {
                            var TotalBefore = detail.Product_Price * detail.Product_Quantity;
                            revenueGL.GLDetail_Credit += TotalBefore;
                        }
                        discount.GLDetail_Debit = revenueGL.GLDetail_Credit * (master.Master_Discount.Value / 100);
                        // Update store quantity
                        foreach (var detail in master.ReverseSalesReceiptDetails)
                        {
                            var record = detailInvoice.Where(m => m.Product_Id == detail.Product_Id).FirstOrDefault();
                            record.Reverse_Quantity = record.Reverse_Quantity + detail.Product_Quantity;
                            record.Reverse_Total_Tax = record.Reverse_Total_Tax + detail.Detail_Total_Tax;
                            record.Reverse_Total_Amount = record.Reverse_Total_Amount + detail.Total_Amount;
                            db.Entry(record).State = EntityState.Modified;
                            db.SaveChanges();


                        }
                        foreach (var detail in RealDetails)
                        {
                            var thisproduct = db.Products.Find(detail.Product_Id);
                            if (thisproduct.Prod_Type == Prod_Type.اولي || thisproduct.Prod_Type == Prod_Type.منتج)
                            {

                                ProductBatches detailBatch = db.ProductBatches.Where(p => p.Product_ID == detail.Product_Id && p.Store_ID == master.Store_ID).FirstOrDefault();
                                //detailBatch.Product_Quantity -= detail.Product_Quantity;
                                //detailBatch.Batch_Cost -= detailBatch.Product_Cost * detail.Product_Quantity;
                                var storeCardOLd = db.StoreCards.Where(a => a.Product_Id == detail.Product_Id && a.Document_Serial == invoiceMaster.Serial_Number).FirstOrDefault();
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
                                storecard.TypeOfDocument = "استبعاد فاتورة مردود فورى";
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
                glMaster.GLDetails.Add(AccountReceivableGL);
                glMaster.GLDetails.Add(revenueGL);
                if (inventoryGL.GLDetail_Credit != inventoryGL.GLDetail_Debit)
                {
                    glMaster.GLDetails.Add(inventoryGL);
                    glMaster.GLDetails.Add(totalCostGL);
                }
                
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
                if (master.Customer_Id != 0 && master.Customer_Id != null&&master.PaymentMethod_Id!=0)
                {
                    glMaster.GLDetails.Add(AccountCustomerGL);
                    glMaster.GLDetails.Add(AccountCustomerGLDebit);
                }
                glMaster = addglDetailTax(glMaster, master.Master_Id);
                db.GLMasters.Add(glMaster);
                GLMaster.GetBalanceForAccount(glMaster);
                GLMaster.GetBalanceForSubLedger(glMaster);
                msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                db.SaveChanges();
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
        public static List<ReverseSalesReceiptDetails> getMaterialOCompoundProduct(List<ReverseSalesReceiptDetails> Details, int Store_Id)
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
                            var Detailnew = new ReverseSalesReceiptDetails();
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


                            var Detailnew = new ReverseSalesReceiptDetails();
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
        
        public static GLMaster addglDetailTax(GLMaster gLMaster, int invoiceId)
        {
            var db = new AccountingContainer();
            var invoiceDetail = db.ReverseSalesReceiptMasters.Find(invoiceId).ReverseSalesReceiptDetails;
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
        /// <summary>
        /// delete that Revers invoice by its id if that status is pendding
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        /// 
        public static List<GLDetail> addglextraTax(GLMaster gLMaster, ReverseSalesReceiptMaster Reverse)
        {
            var db = new AccountingContainer();
            var invoice = db.SalesReceiptMasters.Find(Reverse.SalesRecipt_Id);
            var invoiceExtra = invoice.SalesReceiptTaxes;
            var gldetails = new List<GLDetail>();
          
            if (invoiceExtra.Count() == 0)
            {
                return null;
            }
            foreach (var tax in invoiceExtra)
            {
                var gldetail = new GLDetail();

                var thistax = db.Taxes.Find(Convert.ToInt32( tax.Tax_Id));
                var taxChart = db.ChartOfAccountings.Where(a => a.Account_Code == thistax.Tax_Code).FirstOrDefault();

                gldetail.Account_Id = taxChart.Account_Id;
                gldetail.Account_Name = taxChart.Account_Name;
                if (gLMaster.GLState == TransactionState.Post)
                {
                    gldetail.GLDetail_Credit = Reverse.Total_Amount * (thistax.Tax_Percentage / 100);
                    gldetail.GLDetail_Debit = 0;
                }
                else
                {


                    gldetail.GLDetail_Credit = 0;
                    gldetail.GLDetail_Debit = Reverse.Total_Amount * (thistax.Tax_Percentage / 100);
                }

                gldetails.Add(gldetail);
            }

            return gldetails;
        }
        public static String DeleteInvoiceMaster(int invoiceId)
        {
            var db = new AccountingContainer();
            ReverseSalesReceiptMaster invoice = db.ReverseSalesReceiptMasters.Find(invoiceId);

            if (invoice.Invoice_State != TransactionState.Pending)
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.NotDeleted;
            db.ReverseSalesReceiptDetails.RemoveRange(invoice.ReverseSalesReceiptDetails);
            db.ReverseSalesReceiptFiles.RemoveRange(invoice.ReverseSalesReceiptFiles);
            db.ReverseSalesReceiptMasters.Remove(invoice);
            var DeletQR = db.ReverseQRInvoices.Where(a => a.Serial_Number == invoice.Serial_Number).FirstOrDefault();
            db.ReverseQRDetails.RemoveRange(DeletQR.ReversePOSDetails);

            db.ReverseQRInvoices.Remove(DeletQR);
            db.SaveChanges();
            db.Dispose();
            return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.deleteDone;
        }

    }
}