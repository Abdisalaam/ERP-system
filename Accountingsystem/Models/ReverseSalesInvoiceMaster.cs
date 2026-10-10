using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class ReverseSalesInvoiceMaster
    {
        /// <summary>
        /// check that code is unique or not 
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns>bool</returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.ReverseSalesInvoiceMasters.ToList();
            foreach (var item in allinData)
            {
                if (item.Serial_Number == Serial || !Serial.StartsWith("REVS-"))
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
        /// get list of all Reverse invoice master
        /// </summary>
        /// <returns>List<ReverseSalesInvoiceMaster></returns>
        public static List<ReverseSalesInvoiceMaster> GetAllRevInvoices(int ?all)
        {
            var db = new AccountingContainer();
            var orders = db.ReverseSalesInvoiceMasters.ToList();
            if (all == null)
            {
                orders.Reverse();
                orders = orders.Take(100).ToList();
            }
            return orders;
        }
        //this method  is  used  for  details of  an invoice  that it  get  its 
        //detail by  ID  that  is  passed  to it  from the index
        public static ReverseSalesInvoiceMaster GetrevInvoiceDetailsById(int detailId)
        {
            var db = new AccountingContainer();
            var detail = db.ReverseSalesInvoiceMasters.Find(detailId);
            return detail;

        }
        //this  method is  used  to show  lists  of(Company,supplier,currency,serial of  purchase invoice,products,store) 
        //and  show  the  state of  invoice  to be  state  at  the  beggining
        public static ReverseSalesInvoiceVM AddNewForCreation()
        {

            var db = new AccountingContainer();
            var masterVM = new ReverseSalesInvoiceVM();
            masterVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            masterVM.MyCustomerList = new SelectList(db.Customers.Where(a => a.Customer_State != Customer_State.غير_نشط), "Customer_Id", "Customer_Name");
            masterVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            var lastCodebalance = db.ReverseSalesInvoiceMasters.OrderByDescending(c => c.Master_Id).Where(e => e.Serial_Number != null).FirstOrDefault();
            if (lastCodebalance == null)
            {
                masterVM.Serial_Number = "REVS-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                masterVM.Serial_Number = "REVS-" + (Convert.ToInt32(lastCodebalance.Serial_Number.Substring(5, lastCodebalance.Serial_Number.Length - 5)) + 1).ToString("D3");
            }
            //  masterVM.myBUnit = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            masterVM.MyProductsList = new SelectList(db.Products, "Prod_ID", "Prod_Name");
            masterVM.mystoreList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            var InvoiceList = db.InvoiceMasters.Where(i => i.Invoice_State != TransactionState.Void);
            //masterVM.purchInvList = new SelectList(db.InvoiceMasters.Where(i=>i.Invoice_State == TransactionState.Post), "Master_Id", "Serial_Number");
            List<object> newListInvoice = new List<object>();

            foreach (var member in InvoiceList)
                newListInvoice.Add(new
                {
                    Id = member.Master_Id,
                    Name = member.Serial_Number + " " + member.Number
                });
            masterVM.purchInvList = new SelectList(newListInvoice, "Id", "Name");

            masterVM.Invoice_State = TransactionState.Pending;
            masterVM.Invoice_Date =Convert.ToString( DateTime.Now);
              var reversMasterTemp = new ReverseSalesInvoiceMasterTemp();

            db.ReverseSalesInvoiceMasterTemps.Add(reversMasterTemp);
            db.SaveChanges();
            masterVM.Master_Id = reversMasterTemp.Master_Id;
            return masterVM;
        }
        /// <summary>
        /// prepare detail of Reverse Invoice 
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="tempSalesMasterId"></param>
        /// <returns>ReveInvoiceDetailsVM</returns>
        public static ReveInvoiceDetailsVM PrepareReverseMasterDetail(int serial, string tempSalesMasterId)
        {
            var db = new AccountingContainer();
            var SalesinvoiceMasterVM = new ReveInvoiceDetailsVM();
            string[] words = tempSalesMasterId.Split(' ');
            int first = Convert.ToInt32(words.FirstOrDefault());
            int sec = Convert.ToInt32(words.LastOrDefault());
            var thisdetails = db.InvoiceDetails.Where(e=>e.Details_Id==sec).ToList().FirstOrDefault();

            SalesinvoiceMasterVM.Product_Quantity = thisdetails.Reverse_Quantity;
         
            SalesinvoiceMasterVM.Product_ID = (int)thisdetails.Product_ID;
            var thisproduct = db.Products.Find(SalesinvoiceMasterVM.Product_ID);
            SalesinvoiceMasterVM.Product_Name = thisproduct.Prod_Name;
            SalesinvoiceMasterVM.Product_Code = thisproduct.Prod_Code;
            SalesinvoiceMasterVM.Details_Id = thisdetails.Details_Id;
            SalesinvoiceMasterVM.Product_Price = thisdetails.Product_Price;
            SalesinvoiceMasterVM.Product_Tax = thisdetails.Product_Tax;
            SalesinvoiceMasterVM.InvoiceMaster_Id = first;
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
        public static string deleteItemFomDetails(int id)
        {
            var db = new AccountingContainer();
            var thisitem = db.ReveInvoiceDetailsTemps.Find(Convert.ToInt32(id));
            db.ReveInvoiceDetailsTemps.Remove(thisitem);
            db.SaveChanges();
            db.Dispose();
            return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.deleteFromList;
        }
        /// <summary>
        /// get all Reverse detail temp by master id 
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>List<InvoiceDetails></returns>
        public static List<InvoiceDetails> GetAllTempReverseSalesInvoiceDetails(int masterVmId,int Rev_Id)
        {
            var db = new AccountingContainer();

            var ReversDetailsTemp = db.InvoiceDetails.Where(invoice => invoice.InvoiceMaster_Id == masterVmId&&invoice.Reverse_Quantity!=0).ToList();
            var Master = db.ReverseSalesInvoiceMasterTemps.Find(Rev_Id);
            
            return ReversDetailsTemp;
          


        }
        /// <summary>
        /// get all Reverse detail temp by master id 
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns> List<ReveInvoiceDetailsTemp></returns>
        public static List<ReveInvoiceDetailsTemp> GetAllTempReverseSalesInvoiceDetailsresult(int masterVmId,int delete)
        {
            var db = new AccountingContainer();
            var ReversDetailsTemp = db.ReverseSalesInvoiceMasterTemps.Find(masterVmId);
            if (delete == 1)
            {
                db.ReveInvoiceDetailsTemps.RemoveRange(ReversDetailsTemp.ReveInvoiceDetailsTemps);
                db.SaveChanges();
            }

            return ReversDetailsTemp.ReveInvoiceDetailsTemps.ToList();
        }
        /// <summary>
        /// add to list and check value that inter
        /// </summary>
        /// <param name="ReverseDetail"></param>
        /// <returns></returns>
        public static string AddReversSalesDetailsTemp(ReveInvoiceDetailsVM ReverseDetail)
        {
            
           
          
            if (ReverseDetail.Product_Quantity <= 0 || ReverseDetail.Product_Quantity == null)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Addquantity;
            }
            var db = new AccountingContainer();
            var salesinvMasterTemp = db.ReverseSalesInvoiceMasterTemps.Find(ReverseDetail.InvoiceMaster_Id);
            var SalesinMasterDTemp = new ReveInvoiceDetailsTemp();
            SalesinMasterDTemp.Product_ID = ReverseDetail.Product_ID;
            var invoice = db.InvoiceMasters.Find(ReverseDetail.invoiceId);
            
            var thisProduct = db.Products.Find(SalesinMasterDTemp.Product_ID);
            var batch = db.ProductBatches.Where(a => a.Product_ID == thisProduct.Prod_ID).FirstOrDefault();
            if (thisProduct != null)
            {
                SalesinMasterDTemp.Product_Name = thisProduct.Prod_Name;
                SalesinMasterDTemp.Product_Code = thisProduct.Prod_Code;
            }
            else
            {
                return "هذا الصنف غير موجود ";
            }
            var dtails = salesinvMasterTemp.ReveInvoiceDetailsTemps;
            var dublicateinTemp = dtails.Where(z => z.ReverseSalesInvoiceMasterTempMaster_Id == salesinvMasterTemp.Master_Id && z.Product_ID == thisProduct.Prod_ID).ToList();
            if (dublicateinTemp.FirstOrDefault() != null)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.thesameProduct;
            }
           var invoicedetail = invoice.InvoiceDetails.Where(a => a.Product_ID == thisProduct.Prod_ID).ToList().FirstOrDefault();
           if(invoicedetail.Reverse_Quantity< ReverseDetail.Product_Quantity)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.QuantityIsBigger;

            }
            else { 
            



                SalesinMasterDTemp.Product_Quantity = ReverseDetail.Product_Quantity;
                SalesinMasterDTemp.Product_Price = invoicedetail.Product_Price;
                SalesinMasterDTemp.Product_Tax = invoicedetail.Product_Tax;
               
                decimal TotalBeforeDiscount = SalesinMasterDTemp.Product_Price * SalesinMasterDTemp.Product_Quantity;
                decimal discount = 0;
                if (invoicedetail.Detail_Discount != null)
                {
                    discount = TotalBeforeDiscount * (invoicedetail.Detail_Discount.Value / 100);
                    SalesinMasterDTemp.Detail_Discount = invoicedetail.Detail_Discount;

                }
                else
                {
                    SalesinMasterDTemp.Detail_Discount = invoice.Master_Discount;
                    discount = TotalBeforeDiscount * (invoice.Master_Discount / 100);
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
                salesinvMasterTemp.ReveInvoiceDetailsTemps.Add(SalesinMasterDTemp);
                db.Entry(salesinvMasterTemp).State = EntityState.Modified;
                db.SaveChanges();

                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Addedsuccessfully;
            } }
        /// <summary>
        /// get the file by using its id 
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>ReversSalesFileDetail</returns>
        public static ReversSalesFileDetail getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.ReversSalesFileDetails.Find(fileId);
            return file;
        }
        /// <summary>
        /// get all files that belong to that Reverse invoice 
        /// </summary>
        /// <param name="ReverseSalesMasterId"></param>
        /// <returns></returns>
        public static List<ReversSalesFileDetail> GetAllFiles(int ReverseSalesMasterId)
        {
            var db = new AccountingContainer();
            var files = db.ReversSalesFileDetails.Where(emp => emp.ReverseSalesInvoiceMaster_Id == ReverseSalesMasterId).ToList();
            return files;
        }
        public static string deleteFile(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.ReversSalesFileDetails.Find(fileId);
            db.ReversSalesFileDetails.Remove(file);
            db.SaveChanges();
            return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.deleteFile;
        }

        /// <summary>
        /// add new invoice and check the vale and save it in db
        /// </summary>
        /// <param name="mastersalesVM"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static Tuple< String,int> AddNewRecallsalesInvoice(ReverseSalesInvoiceVM mastersalesVM , String UserId)
        {
            var db = new AccountingContainer();
            var msg = "";
            var Allreverse = db.ReverseSalesInvoiceMasters.Where(a => a.InvoiceMaster_Id == mastersalesVM.selectedrevInvoiceMasterMaster_Id && a.Invoice_State == TransactionState.Pending).FirstOrDefault();
            if (Allreverse != null)
            {
                return Tuple.Create( Allreverse.Serial_Number + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.ThierMoreInvoiceForThat,0);
            }
            else { 
            var revinvoiceMasterTemp = db.ReverseSalesInvoiceMasterTemps.Find(mastersalesVM.Master_Id);
            var master = new ReverseSalesInvoiceMaster();

                if (mastersalesVM != null)
                {
                    var lastCodebalance = db.ReverseSalesInvoiceMasters.OrderByDescending(c => c.Master_Id).Where(e => e.Serial_Number != null).FirstOrDefault();
                    if (lastCodebalance == null)
                    {
                        master.Serial_Number = "REVS-001";
                    }
                    else
                    {
                        //using string substring method to get the number of the last inserted
                        master.Serial_Number = "REVS-" + (Convert.ToInt32(lastCodebalance.Serial_Number.Substring(5, lastCodebalance.Serial_Number.Length - 5)) + 1).ToString("D3");
                    }

                    var comp = db.Companies.Find(mastersalesVM.SelectedCompany_Id);
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
                    
                    master.Master_Discount = db.InvoiceMasters.Find(mastersalesVM.selectedrevInvoiceMasterMaster_Id).Master_Discount;
                    var InvoiceDetail = db.InvoiceMasters.Find(mastersalesVM.selectedrevInvoiceMasterMaster_Id).InvoiceDetails;
                    var resultInv = db.InvoiceMasters.Find(mastersalesVM.selectedrevInvoiceMasterMaster_Id);
                    if (resultInv != null)
                    {
                        master.Master_Id = resultInv.Master_Id;
                        master.InvoiceMaster_Id = resultInv.Master_Id;
                        master.Master_Discount = resultInv.Master_Discount;
                        master.SubLedger = resultInv.SubLedger_ID.ToString();
                        master.Branch_ID = resultInv.Branch_Id;
                        master.BussinesUnit_ID = resultInv.BussinesUnit_ID;
                        master.Department_ID = resultInv.Department_ID;
                    }
                    var resultcustomer = db.Customers.Find(mastersalesVM.SelectedCustomerId);
                    if (resultcustomer != null)
                    {
                        master.Customer_Id = resultcustomer.Customer_Id;
                    }
                    var resultCompany = db.Companies.Find(mastersalesVM.SelectedCompany_Id);
                    if (resultCompany != null)
                    {
                        master.Company_Id = resultCompany.Comp_Id;
                    }
                    var resultCurrency = db.Currencies.Find(mastersalesVM.SelectedCurrencyId);
                    if (resultCurrency != null)
                    {
                        master.Invoice_Currency = resultCurrency.Currency_Type;
                    }
                    var resultstore = db.Stores.Find(mastersalesVM.selectedStoreStore_Id);
                    if (resultstore != null)
                    {
                        master.Store_ID = resultstore.Store_ID;
                    }
                    master.Invoice_State = TransactionState.Pending;

                    master.GL_Date = DateTime.Now;
                    if (mastersalesVM.Files.FirstOrDefault() != null)
                    {
                        foreach (var file in mastersalesVM.Files)
                        {
                            var filedetails = new ReversSalesFileDetail();
                            var data = new byte[file.ContentLength];
                            filedetails.FileName = file.FileName;
                            filedetails.ContentLength = file.ContentLength;
                            file.InputStream.Read(data, 0, file.ContentLength);
                             filedetails.FileContent = data;
                     

                            filedetails.FileType = file.ContentType;
                            if (data.Length < 1000000)
                            {
                                master.ReversSalesFileDetails.Add(filedetails);
                            }
                           
                        }
                    }




                    decimal Sumtax = 0;
                    decimal SumtAmount = 0;
                    decimal SumTotalCost = 0;
                    if (revinvoiceMasterTemp.ReveInvoiceDetailsTemps.ToList().FirstOrDefault() != null)
                    {
                        foreach (var item in revinvoiceMasterTemp.ReveInvoiceDetailsTemps.ToList())
                        {
                            var details = new ReveInvoiceDetails();
                            details.Product_ID = item.Product_ID;
                            details.Product_Quantity = item.Product_Quantity;

                            details.Product_Price = item.Product_Price;
                            details.Detail_Discount = item.Detail_Discount;
                            details.Detail_Discount_Amount = item.Detail_Discount_Amount;

                            var detail = resultInv.InvoiceDetails.ToList();

                            foreach (var productCost in detail)
                            {
                                if (item.Product_ID == productCost.Product_ID)
                                {

                                    item.Detail_Total_Cost = productCost.Detail_Total_Cost / productCost.Product_Quantity;
                                }



                            }


                            details.Product_Tax = item.Product_Tax;
                            details.Batch_ID = item.Batch_ID;

                            var salesInvoiceDetail = db.InvoiceDetails.Where(a => a.Product_ID == item.Product_ID).FirstOrDefault();
                            var productBatch = db.ProductBatches.Where(a => a.Product_ID == salesInvoiceDetail.Product_ID && a.Store_ID == master.Store_ID).FirstOrDefault();


                            //var mySingelCost = item.Detail_Total_Cost / item.Product_Quantity;


                            details.Detail_Total_Cost = (item.Detail_Total_Cost * item.Product_Quantity);

                           
                            decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                            decimal discount = TotalBeforeDiscount * (item.Detail_Discount.Value / 100);
                            decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                            if (item.Product_Tax == 0)
                            {
                                details.Detail_Total_Tax = 0;
                            }
                            else {
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
                            master.Document_Type = mastersalesVM.Document_Type;

                            master.ReveInvoiceDetails.Add(details);
                            master.Invoice_State = TransactionState.Pending;







                            master.ReveInvoiceDetails.Add(details);

                          
                        }
                        var thisinvoice = db.InvoiceMasters.Find(mastersalesVM.selectedrevInvoiceMasterMaster_Id);
                        master.Customer_Id = thisinvoice.Customer_Id;
                        master.Company_Id = thisinvoice.Company_Id;
                        master.Document_Type = thisinvoice.Document_Type;
                        master.Store_ID = thisinvoice.Store_ID;
                        master.Invoice_Currency = thisinvoice.Invoice_Currency;
                        master.Master_Discount = (master.ReveInvoiceDetails.Select(a => a.Detail_Discount_Amount).Sum() / master.ReveInvoiceDetails.Select(a => a.Product_Quantity * a.Product_Price).Sum()) * 100;
                       
                        master.Number = InvoiceSerial.generateSerialForReverse();
                        master.Serial_Number = "REVS-" + System.Text.RegularExpressions.Regex.Match(master.Number, @"\d+").Value;
                       
                        db.ReverseSalesInvoiceMasters.Add(master);
                        InvoiceMaster.MakeSalesinQrSales(master);
                        try
                        { db.SaveChanges(); }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    var x = validationError.PropertyName;
                                    var y = validationError.ErrorMessage;
                                }
                            }
                        }
                        DateTime DateNow = DateTime.Now;


                        db.SaveChanges();

                        var transBatch = new TransactionBatch();
                        transBatch.Date = DateNow;
                        transBatch.OperationName = "مردود مبيعات";
                        transBatch.OperationId = master.Master_Id;
                        transBatch.Operation_Code = master.Serial_Number;
                        transBatch.Description = mastersalesVM.Description;
                        transBatch.UserId = UserId;
                        db.TransactionBatches.Add(transBatch);
                        db.ReveInvoiceDetailsTemps.RemoveRange(revinvoiceMasterTemp.ReveInvoiceDetailsTemps);
                        db.ReverseSalesInvoiceMasterTemps.Remove(revinvoiceMasterTemp);
                        db.SaveChanges();
                        db.Dispose();

                        return Tuple.Create( Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Therefundinvoicehasbeencreatedsuccessfully, master.Master_Id);
                    }
                    return Tuple.Create("محتويات الفاتورة فارغة", 0);
                    
                }
            }

            return Tuple.Create("تأكد من البيانات المدخلة", 0);
       
        }
        // Gets the ViewModel used for editing the InvoiceMaster 
        //it just  edit  the  state  of  the invoice 
        //public static RecalPurchInvoiceMasterVM GetEditViewModel(int id)
        //{
        //    RecalPurchInvoiceMaster invoice = GetInvoiceDetailsById(id);
        //    RecalPurchInvoiceMasterVM invoiceMasterVM = new RecalPurchInvoiceMasterVM();
        //    invoiceMasterVM.PI_State = invoice.PI_State;
        //    return invoiceMasterVM;
        //}
        /* here  we  got  the  recall  purchase invoice  by its  ID  and  update  its state 
        //and  made  Automatic  GL  for  each  state  that  will be  updated  to*/

        public static String UpdateInvoiceMaster(ReverseSalesInvoiceVM invoiceMasterVM)
        {
            var msg = "";
            var db = new AccountingContainer();
           
            ReverseSalesInvoiceMaster invoice = db.ReverseSalesInvoiceMasters.Find(invoiceMasterVM.Master_Id);
            OperationRun.Addnewoperation(invoice.Serial_Number);
            var operation = db.OperationRuns.Where(a => a.Code == invoice.Serial_Number).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(invoice.Serial_Number);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }
            if (invoice.Invoice_State == invoiceMasterVM.Invoice_State)
            {
                OperationRun.DeleteOperation(invoice.Serial_Number);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + invoice.Invoice_State + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            else
            {
                invoice.Invoice_State = invoiceMasterVM.Invoice_State;
            }
            
            InvoiceMaster invoiceMaster = db.InvoiceMasters.Find(invoice.InvoiceMaster_Id.Value);
            var detailInvoice = invoiceMaster.InvoiceDetails.ToList();
            invoice.Invoice_State = invoiceMasterVM.Invoice_State;
            var revQr = db.ReverseQRInvoices.Where(a => a.Serial_Number == invoice.Serial_Number).FirstOrDefault();
            if (revQr != null)
            {
                revQr.Invoice_State = invoice.Invoice_State;
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Entry(invoice).State = EntityState.Modified;
                    if (revQr != null)
                    {
                        db.Entry(revQr).State = EntityState.Modified;
                    }
                    #region Create GL
                    // Add GLHeader 
                    GLMaster glMaster = new GLMaster();

                    glMaster.GL_Date = invoice.Invoice_Date;
                    glMaster.GL_Desc = invoice.Description;
                    glMaster.Company_Id = invoice.Company_Id;
                    glMaster.GLType_Id = "مردود مبيعات اجل";
                    glMaster.Currency_Id = invoice.Invoice_Currency;
                    glMaster.Auto = true;
                    glMaster.GLCode = invoice.Serial_Number;
                    // glmaster.GL_State = ransactionState.Pending;

                    // Create five GlDetail
                    GLDetail AccountReceivableGL = new GLDetail(),
                        revenueGL = new GLDetail(),

                        inventoryGL = new GLDetail(),
                        totalCostGL = new GLDetail(),
                    discount = new GLDetail();

                    // Initialize common values
                    // TODO: Fix the following values
                    //var BusId = db.BussinesUnits.FirstOrDefault().BU_Id;
                    AccountReceivableGL.Bu_Id = 0;
                    revenueGL.Bu_Id = 0;
                    inventoryGL.Bu_Id = 0;
                    totalCostGL.Bu_Id = 0;
                    discount.Bu_Id = 0;
                    var ChartOfAccounRecive = db.ChartOfAccountings.Find(37);
                    AccountReceivableGL.Account_Id = ChartOfAccounRecive.Account_Id;
                    AccountReceivableGL.Account_Name = ChartOfAccounRecive.Account_Name;



                    var ChartOfAccounRevenue = db.ChartOfAccountings.Find(77);
                    revenueGL.Account_Id = ChartOfAccounRevenue.Account_Id;
                    revenueGL.Account_Name = ChartOfAccounRevenue.Account_Name;

                    var RealDetails = getMaterialOCompoundProduct(invoice.ReveInvoiceDetails.ToList(), invoice.Store_ID);


                    var store = db.Stores.Find(invoice.Store_ID);
                    var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).First();
                    inventoryGL.Account_Id = ChartOfAccounInventory.Account_Id;
                    inventoryGL.Account_Name = ChartOfAccounInventory.Account_Name;

                    var ChartOfAccounCost = db.ChartOfAccountings.Find(105);
                    totalCostGL.Account_Id = ChartOfAccounCost.Account_Id;
                    totalCostGL.Account_Name = ChartOfAccounCost.Account_Name;


                    var DiscountCost = db.ChartOfAccountings.Find(78);
                    discount.Account_Id = DiscountCost.Account_Id;
                    discount.Account_Name = DiscountCost.Account_Name;

                    AccountReceivableGL.Asset_Id =
                    revenueGL.Asset_Id =

                    inventoryGL.Asset_Id =
                    totalCostGL.Asset_Id = 0;
                    AccountReceivableGL.Description_Detail =
                   revenueGL.Description_Detail =

                   inventoryGL.Description_Detail =
                   totalCostGL.Description_Detail = "مردود مبيعات";
                    //Customer customer = db.Customers.Find(invoice.Customer_Id);
                    //SubLedger customerSubledger = db.SubLedgers.Where(subled => subled.SubLed_Code == customer.Customer_Code).First();
                    //AccountReceivableGL.SubLedger_Id =
                    //revenueGL.SubLedger_Id =
                    //taxesGL.SubLedger_Id =
                    //totalCostGL.SubLedger_Id = customerSubledger.SubLed_Id;




                    var Customerr = db.Customers.Find(invoice.Customer_Id);
                    var Cust = db.Accounts.Where(C => C.Account_Code == Customerr.Customer_Code).FirstOrDefault();
                    AccountReceivableGL.Account_Child_ID = Cust.Accounts_Id;
                    AccountReceivableGL.Account_CHild_Name = Cust.Account_Name;
                    revenueGL.Account_Child_ID = 0;

                    totalCostGL.Account_Child_ID = 0;

                    revenueGL.SubLedger_Id = Convert.ToInt32(invoice.SubLedger);
                    totalCostGL.SubLedger_Id = Convert.ToInt32(invoice.SubLedger);
                    discount.SubLedger_Id = Convert.ToInt32(invoice.SubLedger);
                    invoice.Total_Cost = 0;
                    switch (invoice.Invoice_State)
                    {
                        //Post Invoice
                        case TransactionState.Post:
                            glMaster.GLState = TransactionState.Post;
                            var gl = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode && a.GLState == glMaster.GLState).FirstOrDefault();
                            if (gl != null)
                            {
                                OperationRun.DeleteOperation(invoice.Serial_Number);
                                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                            }

                            // Customer GL
                            AccountReceivableGL.GLDetail_Credit = invoice.Total_Amount;
                            // Revenue GL
                            foreach (var detail in invoice.ReveInvoiceDetails)
                            {
                                var TotalBefore = detail.Product_Price * detail.Product_Quantity;
                                revenueGL.GLDetail_Debit += TotalBefore;
                            }
                            discount.GLDetail_Credit = revenueGL.GLDetail_Debit * (invoice.Master_Discount.Value / 100);
                            // Taxes GL

                            // Inventory GL
                          
                            // Update batch quantity
                            db.ProductBatches.Load();
                            foreach (var detail in invoice.ReveInvoiceDetails)
                            {


                                //تعديل فى قيم الفاتورة بناءا على المرتجع
                                var record = detailInvoice.Where(a => a.Product_ID == detail.Product_ID).FirstOrDefault();
                                record.Reverse_Quantity = record.Reverse_Quantity - detail.Product_Quantity;
                                record.Reverse_Total_Tax = record.Reverse_Total_Tax - detail.Detail_Total_Tax;
                                record.Reverse_Total_Amount = record.Reverse_Total_Amount - detail.Total_Amount;
                                db.Entry(record).State = EntityState.Modified;
                                db.SaveChanges();
                                
                            }
                            foreach (var detail in RealDetails)
                            {
                                var thisproduct = db.Products.Find(detail.Product_ID);
                                if (thisproduct.Prod_Type == Prod_Type.منتج|| thisproduct.Prod_Type == Prod_Type.اولي)
                                {
                                    var storecard = new StoreCard();

                                    ProductBatches detailBatch = db.ProductBatches.Where(p => p.Product_ID == detail.Product_ID && p.Store_ID == invoice.Store_ID).FirstOrDefault();
                                    //detailBatch.Product_Quantity += detail.Product_Quantity;
                                    //detailBatch.Batch_Cost += detailBatch.Product_Cost * detail.Product_Quantity;
                                    if (detailBatch == null)
                                    {
                                        var batch = new ProductBatches();
                                        batch.Store_ID = invoice.Store_ID;
                                        batch.Product_ID = thisproduct.Prod_ID;
                                        batch.IsActive = true;
                                        db.ProductBatches.Add(batch);
                                        db.SaveChanges();
                                        detailBatch = batch;
                                    }

                                    if (detailBatch.Store_ID != null)
                                    {
                                        var storeCardOLd = db.StoreCards.Where(a => a.Product_Id == detail.Product_ID && a.Document_Serial == invoiceMaster.Serial_Number).FirstOrDefault();
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
                                         
                                            storecard.TotalCost = detail.Product_Quantity* storecard.CostPrice;
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
                                        storecard.Store_Id = invoice.Store_ID;
                                        storecard.Quantity = detail.Product_Quantity;
                                        storecard.SalePrice = detail.Product_Price;
                                        storecard.Product_Id = detail.Product_ID;
                                        storecard.IncreaseQuantity = true;
                                        storecard.TypeOfDocument = "فاتورة مردود مخزنية";
                                        storecard.Document_Serial = invoice.Serial_Number;
                                     
                                        storecard.TotalAmount = detail.Total_Amount;
                                        storecard.Account_Id = AccountReceivableGL.Account_Child_ID;
                                        storecard.Subledger_Id = Convert.ToInt32(invoice.SubLedger);
                                        Store.AddinginstorCard(storecard);
                                        invoice.Total_Cost += storecard.TotalCost.Value;

                                    }
                                }


                            }

                                // Total Cost GL
                                totalCostGL.GLDetail_Credit = invoice.Total_Cost;
                            inventoryGL.GLDetail_Debit = invoice.Total_Cost;

                            break;
                        // Void Invoice
                        case TransactionState.Void:
                            glMaster.GLType_Id = "مردود مبيعات اجل عكسي";
                            // Customer GL
                            glMaster.GLState = TransactionState.Void;
                            var glCheck = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode && a.GLState == glMaster.GLState).FirstOrDefault();
                            if (glCheck != null)
                            {
                                OperationRun.DeleteOperation(invoice.Serial_Number);
                                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                            }
                            var errorMessage = new List<string> { };
                            foreach (var detail in RealDetails)
                            {
                                var thisproduct = db.Products.Find(detail.Product_ID);

                                ProductBatches detailBatch = db.ProductBatches.Where(a => a.Product_ID == detail.Product_ID && a.Store_ID == invoice.Store_ID).ToList().FirstOrDefault();
                                if (detailBatch!=null&&detailBatch.Store_ID != null)
                                {
                                    if (detail.Product_Quantity > detailBatch.Product_Quantity)
                                    {
                                        var productName = db.Products.Find(detail.Product_ID).Prod_Name;
                                        invoice.Invoice_State = TransactionState.Post;
                                        db.SaveChanges();
                                        errorMessage.Add(Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Thereisnotenoughproduct + productName);

                                    }
                                }

                            }
                            if (errorMessage.Count() != 0)
                            {


                                string combindedString = string.Join("\n", errorMessage.ToArray());
                                OperationRun.DeleteOperation(invoice.Serial_Number);
                                return combindedString;
                            }
                            if (invoice.HasOff == true)

                            {
                                msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.UnderCloseing;
                                invoice.Invoice_State = TransactionState.Post;
                                db.SaveChanges();
                                OperationRun.DeleteOperation(invoice.Serial_Number);
                                return msg;

                            }
                            else
                            {


                                AccountReceivableGL.GLDetail_Debit = invoice.Total_Amount;
                                // Revenue GL
                                foreach (var detail in invoice.ReveInvoiceDetails)
                                {
                                    var TotalBefore = detail.Product_Price * detail.Product_Quantity;
                                    revenueGL.GLDetail_Credit += TotalBefore;
                                }
                                discount.GLDetail_Debit = revenueGL.GLDetail_Credit * (invoice.Master_Discount.Value / 100);
                                // Taxes GL

                                // Inventory GL
                                inventoryGL.GLDetail_Credit = RealDetails.Select(a=>a.Detail_Total_Cost).ToList().Sum();
                                // Update batch quantity
                                db.ProductBatches.Load();
                                foreach (var detail in invoice.ReveInvoiceDetails)
                                { //تعديل فى قيم الفاتورة بناءا على المرتجع
                                    var record = detailInvoice.Where(a => a.Product_ID == detail.Product_ID).FirstOrDefault();
                                    record.Reverse_Quantity = record.Reverse_Quantity + detail.Product_Quantity;
                                    record.Reverse_Total_Tax = record.Reverse_Total_Tax + detail.Detail_Total_Tax;
                                    record.Reverse_Total_Amount = record.Reverse_Total_Amount + detail.Total_Amount;
                                    db.Entry(record).State = EntityState.Modified;
                                    db.SaveChanges();


                                    
                                }
                                // Total Cost GL
                                foreach(var detail in RealDetails)
                                {
                                    var thisproduct = db.Products.Find(detail.Product_ID);
                                    if (thisproduct.Prod_Type == Prod_Type.منتج|| thisproduct.Prod_Type == Prod_Type.اولي )
                                    {

                                        ProductBatches detailBatch = db.ProductBatches.Where(p => p.Product_ID == detail.Product_ID && p.Store_ID == invoice.Store_ID).FirstOrDefault();
                                        if (detailBatch.Store_ID != null)
                                        {
                                            var storeCardOLd = db.StoreCards.Where(a => a.Product_Id == detail.Product_ID && a.Document_Serial == invoiceMaster.Serial_Number).FirstOrDefault();
                                            //detailBatch.Product_Quantity -= detail.Product_Quantity;
                                            //detailBatch.Batch_Cost -= detailBatch.Product_Cost * detail.Product_Quantity;
                                            detailBatch.Product_Quantity -= detail.Product_Quantity;
                                            detailBatch.Batch_Cost -= detail.Product_Quantity* storeCardOLd.CostPrice;
                                            if (Math.Round(detailBatch.Product_Quantity, 2, MidpointRounding.AwayFromZero) == 0)
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
                                            storecard.Store_Id = invoice.Store_ID;
                                            storecard.Quantity = detail.Product_Quantity;
                                            storecard.SalePrice = detail.Product_Price;
                                            storecard.Product_Id = detail.Product_ID;
                                            storecard.IncreaseQuantity = false;
                                            storecard.TypeOfDocument = "استبعاد فاتورة مردود مخزنية";
                                            storecard.Document_Serial = invoice.Serial_Number;
                                            storecard.TotalCost = detail.Product_Quantity * storeCardOLd.CostPrice;
                                            storecard.CostPrice = storeCardOLd.CostPrice;
                                            storecard.TotalAmount = detail.Total_Amount;
                                            storecard.Account_Id = AccountReceivableGL.Account_Child_ID;
                                            storecard.Subledger_Id = Convert.ToInt32(invoice.SubLedger);
                                            
                                            Store.AddinginstorCard(storecard);
                                            invoice.Total_Cost += storecard.TotalCost.Value;

                                        }
                                    }
                                }
                                totalCostGL.GLDetail_Debit = RealDetails.Select(a => a.Detail_Total_Cost).ToList().Sum();
                            }
                            break;
                        default:
                            break;
                    }
                    if (msg == "")
                    {

                        msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                        // Add all GL details to the master
                        glMaster.GLDetails.Add(AccountReceivableGL);
                        glMaster.GLDetails.Add(revenueGL);


                        if (discount.GLDetail_Credit == 0 && discount.GLDetail_Debit == 0)
                        {

                        }
                        else
                        {
                            glMaster.GLDetails.Add(discount);

                        }
                        if (inventoryGL.GLDetail_Credit != 0 || inventoryGL.GLDetail_Debit != 0)
                        {
                            glMaster.GLDetails.Add(inventoryGL);
                            glMaster.GLDetails.Add(totalCostGL);
                        }
                        db.GLMasters.Add(glMaster);
                        #endregion
                        glMaster = addglDetailTax(glMaster, invoice.ReveInvoiceDetails.ToList());
                        GLMaster.GetBalanceForAccount(glMaster);
                        GLMaster.GetBalanceForSubLedger(glMaster);

                        db.SaveChanges();

                        transaction.Commit();
                    }
                 
                }
            
                catch
                {
                    transaction.Rollback();
                    db.Dispose();
                }
            }
            OperationRun.DeleteOperation(invoice.Serial_Number);
            if (invoice.Invoice_State == TransactionState.Post)
            {
                msg = Resources.validate.GLDone;
            }
            else
            {
                msg = Resources.validate.GLReverse;
            }
            return msg;
        }
        public static List<ReveInvoiceDetails> getMaterialOCompoundProduct(List<ReveInvoiceDetails> Details, int Store_Id)
        {
            var db = new AccountingContainer();
            var ProductAll = db.Products.ToList();
            var project = ProductAll.Where(a => a.Prod_Type == Prod_Type.مجمع).Select(a => a.Prod_ID).ToList();
            var ProList = Details.Where(s => project.Contains(s.Product_ID)).ToList();
            if (ProList.Count() == 0)
            {

            }
            else
            {
                
                foreach (var item in ProList)
                {
                    var SubProductList = ProductAll.Where(a => a.Prod_ID == item.Product_ID).FirstOrDefault().ProductCompounds;
                    foreach (var subProduct in SubProductList)
                    {
                        var dublicate = Details.Where(a => a.Product_ID == subProduct.Product_Id).FirstOrDefault();
                        var batch = db.ProductBatches.Where(a => a.Product_ID == subProduct.Product_Id && a.Store_ID == Store_Id).FirstOrDefault();
                        if (dublicate == null)
                        {
                            var Detailnew = new ReveInvoiceDetails();
                            Detailnew.Product_ID = subProduct.Product_Id;
                            Detailnew.Product_Quantity = subProduct.Quantity * item.Product_Quantity;
                           
                            Detailnew.Batch_ID = batch.Batch_ID;

                            if (batch.Batch_Cost > 0)
                            {
                                Detailnew.Detail_Total_Cost = ( batch.Batch_Cost / batch.Product_Quantity ) * Detailnew.Product_Quantity;
                             
                            }
                            else
                            {
                                Detailnew.Detail_Total_Cost = 0;
                               

                            }
                            Details.Add(Detailnew);

                        }
                        else
                        {


                            var Detailnew = new ReveInvoiceDetails();
                            Detailnew.Product_ID = subProduct.Product_Id;
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
                        var Delete = Details.Where(a => a.Product_ID == subProduct.Master_Product_Id).FirstOrDefault();

                        Details.Remove(Delete);

                    }





                }

            }

            return Details;
        }
        public static GLMaster addglDetailTax(GLMaster gLMaster, List<ReveInvoiceDetails> invoiceDetail)
        {
            var db = new AccountingContainer();
          
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
        // public static bool addAssetInvoice

        /* here  we  get  the  purchase invoice  details  and   bass  its  value  to  the  
        //recallPurchase  invoice   and   make  the  recall on  its  values*/
        public static InvoiceMaster GetrevsalesInvoice(int invId)
        {
            return InvoiceMaster.GetInvoiceDetailsById(invId);

        }
        /// <summary>
        /// delete invoice by using its id 
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns>string</returns>
        public static string DeleteInvoiceMaster(int invoiceId)
        {
            var db = new AccountingContainer();
            ReverseSalesInvoiceMaster invoice = db.ReverseSalesInvoiceMasters.Find(invoiceId);

            if (invoice.Invoice_State != TransactionState.Pending)
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.NotDeleted;
            db.ReveInvoiceDetails.RemoveRange(invoice.ReveInvoiceDetails);
            db.ReversSalesFileDetails.RemoveRange(invoice.ReversSalesFileDetails);
            db.ReverseSalesInvoiceMasters.Remove(invoice);
            var DeletQR = db.ReverseQRInvoices.Where(a => a.Serial_Number == invoice.Serial_Number).FirstOrDefault();
            db.ReverseQRDetails.RemoveRange(DeletQR.ReversePOSDetails);
       
            db.ReverseQRInvoices.Remove(DeletQR);
            db.SaveChanges();
            db.Dispose();
            return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.deleteDone;
        }
    }
}
