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
    public partial class RecalPurchInvoiceMaster
    {/// <summary>
     /// check that code is unique 
     /// </summary>
     /// <param name="Serial"></param>
     /// <returns>bool</returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.RecalPurchInvoiceMasters.ToList();
            foreach (var item in allinData)
            {
                if (item.Recall_MPurch_Serial == Serial || !Serial.StartsWith("REVP-"))
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
        //this  Method  is  to GetList  of  All Recall Purchase invoice  Created
        public static List<RecalPurchInvoiceMaster> GetAllInvoices(int ?all)
        {
            var db = new AccountingContainer();
            var orders = db.RecalPurchInvoiceMasters.ToList();
            if (all == null)
            {
                orders.Reverse();
                orders = orders.Take(100).ToList();
            }
            return orders;
        }
        //this  Method  is  to Get   Recall Purchase invoice  Created by its ID
        public static RecalPurchInvoiceMaster GetInvoiceDetailsById(int detailId)
        {
            var db = new AccountingContainer();
            var detail = db.RecalPurchInvoiceMasters.Find(detailId);
            return detail;

        }
        // this  Method  IS  used  in  the  GetMethod  of  the  controller  to  make  lists 
        //from  other Tables  and  choose from it  the  name  and  transfare  its  id to  the  selectedID  Created  in the  ViewModel  
        public static RecalPurchInvoiceMasterVM AddNewForCreation()
        {
            var db = new AccountingContainer();
            var masterVM = new RecalPurchInvoiceMasterVM();
            var lastmasterID = (from s in db.RecalPurchInvoiceMasters
                                select s.Master_Id).ToList().LastOrDefault();


            var lista = new List<ChartOfAccounting> { };
            foreach (var k in db.ChartOfAccountings.Where(a => a.isActive != false))
            {
                var parent = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == k.Account_Id);
                var o = parent.FirstOrDefault();
                if (o == null)
                {
                    lista.Add(db.ChartOfAccountings.Find(k.Account_Id));
                }
            }
            masterVM.PaymentMethodList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("010120001") || accCode.Account_Code.StartsWith("010120002")), "Account_Id", "Account_Name");

            masterVM.CompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            masterVM.SupplierList = new SelectList(db.Suppliers.Where(a => a.Supplier_state != Customer_State.غير_نشط), "Supp_Id", "Supp_Name");
            masterVM.CurrenyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            masterVM.Recall_MPurch_Serial = Convert.ToString(lastmasterID + 1);
            List<object> newListpro = new List<object>();
            var membersPro = db.Products.Where(a => a.Prod_Type != Prod_Type.خدمة);
            foreach (var member in membersPro)
                newListpro.Add(new
                {
                    Id = member.Prod_ID,
                    Name = member.Prod_Name + "   " + member.Bar_Code
                });

            masterVM.MyProductsList = new SelectList(newListpro, "Id", "Name");
       
            masterVM.mystoreList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            masterVM.purchInvList = new SelectList(db.PurInvoiceMasters.Where(e => e.Store_ID != 0 && e.Invoice_State == TransactionState.Post), "Master_Id", "Serial_Number");

            masterVM.PI_State = TransactionState.Pending;
            var lastSerial = db.RecalPurchInvoiceMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                masterVM.Recall_MPurch_Serial = "REVP-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                masterVM.Recall_MPurch_Serial = "REVP-" + (Convert.ToInt32(lastSerial.Recall_MPurch_Serial.Substring(5, lastSerial.Recall_MPurch_Serial.Length - 5)) + 1).ToString("D3");
            }
            var recallMasterTemp = new RecallPurchInvoicMasterTemp();
          
            db.RecallPurchInvoicMasterTemps.Add(recallMasterTemp);
            db.SaveChanges();
            masterVM.Master_Id = recallMasterTemp.Master_Id;
            return masterVM;
        }
        /// <summary>
        /// prepare detail vm by id of master
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="tempPurchaseMasterId"></param>
        /// <returns>RecalPurchInvoiceDetailsVM</returns>
        public static RecalPurchInvoiceDetailsVM PreparerecallMasterDetail(int serial ,string tempPurchaseMasterId )
        {

            var db = new AccountingContainer();
            var invoiceMasterVM = new RecalPurchInvoiceDetailsVM();
            string[] words = tempPurchaseMasterId.Split(' ');
            int first = Convert.ToInt32(words.FirstOrDefault());
            int sec = Convert.ToInt32(words.LastOrDefault());
            var thisdetails = GetAllTemprecallInvoiceDetails(serial).Where(e=>e.Details_Id==sec).ToList().FirstOrDefault();
            var thismaster = db.PurInvoiceMasters.Find(serial);

            invoiceMasterVM.Product_Quantity = thisdetails.Reverse_Quantity;
            if (thisdetails.Product_ID > 0)
            {
                invoiceMasterVM.Product_ID = (int)thisdetails.Product_ID;
                invoiceMasterVM.Item_Name = thisdetails.ItemDesc;
               
                invoiceMasterVM.Asset_Id = 0;
            }
            else
            {
                invoiceMasterVM.Asset_Id = thisdetails.Asset_Id;
                invoiceMasterVM.Product_ID = 0;
            }
            //var s = tempPurchaseMasterId.FirstOrDefault();
            invoiceMasterVM.Details_Id = thisdetails.Details_Id;
            invoiceMasterVM.Product_Price = thisdetails.Product_Price;
            invoiceMasterVM.Product_Tax = (int)thisdetails.Product_Tax;
            var thistax = db.Taxes.Find(thisdetails.Product_Tax);
            if (thistax != null)
            {
                invoiceMasterVM.Tax_Name = thistax.Tax_Name;
                invoiceMasterVM.tax_amount = Convert.ToDecimal(thistax.Tax_Percentage);
            }          
            invoiceMasterVM.dicount = thismaster.Master_Discount;
            invoiceMasterVM.Detail_Total_Cost = thisdetails.Reverse_Tota_Cost;
           invoiceMasterVM.RecalPurchInvoiceMasterMaster_Id = first;
            
            return invoiceMasterVM;
        }
        /// <summary>
        /// get all detail of purchase by id of purchase invoice
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>ist<PurchaseDetails></returns>
        public static List<PurchaseDetails> GetAllTemprecallInvoiceDetails(int masterVmId)
        {
            var db = new AccountingContainer();
            var masterVM = new RecalPurchInvoiceMasterVM();
           
            var recallDetailsTemp = db.PurchaseDetails.Where(invoice => invoice.InvoiceMaster_Id == masterVmId && invoice.Reverse_Quantity > 0).ToList();


            return recallDetailsTemp;
            

         
        }
        /// <summary>
        /// get all detail temp for Recall purchase invoice
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns> List<RecallPurchInvDetailsTemp> </returns>
        public static List<RecallPurchInvDetailsTemp> GetAllTemprecallInvoiceDetailsresult(int masterVmId,int delete)
        {
            var db = new AccountingContainer();
            var RecallPurchaseTemp = db.RecallPurchInvoicMasterTemps.Find(masterVmId);
            if (delete == 1)
            {
                db.RecallPurchInvDetailsTemps.RemoveRange(RecallPurchaseTemp.RecallPurchInvDetailsTemps);
                db.SaveChanges();
            }
            return RecallPurchaseTemp.RecallPurchInvDetailsTemps.ToList();
        }
        /// <summary>
        /// prepare detail of recall to make detail of that Recall
        /// </summary>
        /// <param name="recallDetail"></param>
        /// <returns>string</returns>
        public static string AddRecallDetailsTemp(RecalPurchInvoiceDetailsVM recallDetail)
        {
            var db = new AccountingContainer();       
         
            if (recallDetail.Product_Quantity == null || recallDetail.Product_Quantity <= 0)
            {
                 return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Addquantity;
            }
            var invoiceMasterTemp = db.RecallPurchInvoicMasterTemps.Find(recallDetail.RecalPurchInvoiceMasterMaster_Id);
            var invoiceMasterDetailsTemp = new RecallPurchInvDetailsTemp();
            var purchace = db.PurInvoiceMasters.Find(recallDetail.selectedPurchaseInvoice);
            var details = invoiceMasterTemp.RecallPurchInvDetailsTemps;
            var duplecatTemp = details.Where(p => p.RecallPurchInvoicMasterTempMaster_Id == invoiceMasterTemp.Master_Id && p.Product_Id == recallDetail.Product_ID).ToList();
            if (duplecatTemp.FirstOrDefault() != null)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.thesameProduct;
            }
            var quantity = purchace.InvoiceDetails.Where(p=>p.Product_ID == recallDetail.Product_ID).ToList().FirstOrDefault();
            if (recallDetail.Product_Quantity > quantity.Reverse_Quantity)
            {
                 return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.QuantityIsBigger;
            }
            else
            {
                if (recallDetail.Product_ID > 0)
                {
                    var thisproduct = db.Products.Find(recallDetail.Product_ID);
                    if (thisproduct == null)
                    { return null; }
                    else
                    {
                        invoiceMasterDetailsTemp.Product_Id = recallDetail.Product_ID;
                        invoiceMasterDetailsTemp.Product_Code = thisproduct.Prod_Code;
                        invoiceMasterDetailsTemp.Product_Name = thisproduct.Prod_Name;
                        invoiceMasterDetailsTemp.Asset_Id = 0;

                    }

                }
                else
                {
                    invoiceMasterDetailsTemp.Product_Id = 0;
                    invoiceMasterDetailsTemp.Asset_Id = recallDetail.Asset_Id;
                }


                invoiceMasterDetailsTemp.Product_Id = recallDetail.Product_ID;
                invoiceMasterDetailsTemp.Product_Quantity = recallDetail.Product_Quantity;
                invoiceMasterDetailsTemp.Product_Price = recallDetail.Product_Price;
                invoiceMasterDetailsTemp.Product_Tax = (int)recallDetail.Product_Tax;
                invoiceMasterDetailsTemp.Total_Amount = invoiceMasterDetailsTemp.Product_Price * invoiceMasterDetailsTemp.Product_Quantity;
                var dicount = (recallDetail.dicount / 100) * invoiceMasterDetailsTemp.Total_Amount;
                if (dicount == 0 || dicount == null)
                {

                }
                else
                {
                    invoiceMasterDetailsTemp.Total_Amount = invoiceMasterDetailsTemp.Total_Amount - (decimal)dicount;
                }
                var thistax = db.Taxes.Find(recallDetail.Product_Tax);
                if (thistax == null)
                { invoiceMasterDetailsTemp.Detail_total_Tax = 0; }
                else
                { invoiceMasterDetailsTemp.Detail_total_Tax = (invoiceMasterDetailsTemp.Total_Amount) * (Convert.ToDecimal(thistax.Tax_Percentage) / 100); }

                invoiceMasterDetailsTemp.Detail_Total_Cost = (invoiceMasterDetailsTemp.Total_Amount) + (invoiceMasterDetailsTemp.Detail_total_Tax);
                invoiceMasterTemp.RecallPurchInvDetailsTemps.Add(invoiceMasterDetailsTemp);
                db.Entry(invoiceMasterTemp).State = EntityState.Modified;
                db.SaveChanges();

                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Addedsuccessfully;
            }

           
        }
        /// <summary>
        /// get that file by its id 
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>RecallPurchFileDetails</returns>
        public static RecallPurchFileDetails getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.RecallPurchFileDetails.Find(fileId);
            return file;
        }
        /// <summary>
        /// get All file of Recall purchase by id of master
        /// </summary>
        /// <param name="recallMasterId"></param>
        /// <returns>  List<RecallPurchFileDetails></returns>
        public static List<RecallPurchFileDetails> GetAllFiles(int recallMasterId)
        {
            var db = new AccountingContainer();
            var files = db.RecallPurchFileDetails.Where(emp => emp.RecalPurchInvoiceMasterMaster_Id == recallMasterId).ToList();
            return files;
        }
        //this  method  is   to  get   the  RecallInvoice by its  ID
        public static int GetallSupplierPurchInv(int id)
        {
            var db = new AccountingContainer();
            var supp = db.PurInvoiceMasters.Find(id);
            return supp.Supplier_Id;
        }
        // this  Method  is  to create  new  recallInvoice  that  is  taken  its  Values  from  the  ViewModel
        //then  Make  A  batchNumber  for  this transaction
        public static bool AddNewRecallInvoice(RecalPurchInvoiceMasterVM masterVM)
        {
            var db = new AccountingContainer();
            var master = new RecalPurchInvoiceMaster();
        
            if (masterVM != null)
            {
                master.Master_Id = masterVM.Master_Id;
                var lastSerial = db.RecalPurchInvoiceMasters.ToList().LastOrDefault();
                master.Recall_MPurch_Serial = "REVP-" + (Convert.ToInt32(lastSerial.Recall_MPurch_Serial.Substring(5, lastSerial.Recall_MPurch_Serial.Length - 5)) + 1).ToString("D3");
              
               

                var comp = db.Companies.Find(masterVM.CompanyComp_Id);
                if (comp != null)
                {
                    var date = Convert.ToDateTime(masterVM.Register_Date);
                    if (date < comp.StartFinancialDate)
                    {
                        master.Register_Date = DateTime.Now;
                    }
                    else
                    {
                        master.Register_Date = Convert.ToDateTime(masterVM.Register_Date);
                    }
                }
                else
                {
                    master.Register_Date = DateTime.Now;
                }
                master.Due_Date = Convert.ToDateTime(masterVM.Due_Date);

                var resultInv = db.PurInvoiceMasters.Find(masterVM.selectedPurInvoiceMasterMaster_Id);
                if (resultInv != null)
                {
                    master.PurInvoiceMasterMaster_Id = resultInv.Master_Id;
                }
                var resultsupplier = db.Suppliers.Find(masterVM.SupplierSuppId);
                if (resultsupplier != null)
                {
                    master.SupplierSuppId = resultsupplier.Supp_Id;
                    
                }
                var resultCompany = db.Companies.Find(masterVM.CompanyComp_Id);
                if (resultCompany != null)
                {
                    master.CompanyComp_Id = resultCompany.Comp_Id;
                }
                var resultCurrency = db.Currencies.Find(masterVM.SelectedCurrencyId);
                if (resultCurrency != null)
                {
                    master.PI_Currency = resultCurrency.Currency_Type;
                }
                var resultstore = db.Stores.Find(masterVM.selectedStoreStore_Id);
                if (resultstore != null)
                {
                    master.StoreStore_ID = resultstore.Store_ID;
                    // var sub = db.SubLedgers.Where(p => p.SubLed_Name == resultstore.Store_Name).ToList().FirstOrDefault();

                    // master.SubLedger = sub.SubLed_Code;

                }
                //  master.StoreStore_ID = masterVM.selectedStoreStore_Id;
                master.Exchange_Rate = resultInv.Exchange_Rate;

                master.Master_Discount = (decimal)masterVM.Master_Discount;
                master.PI_State = TransactionState.Pending;
                var products = db.Products;
                if (masterVM.myDetailsVM.Count() == 0)
                {
                    return false;
                }
                foreach (var item in masterVM.myDetailsVM)
                {
                    var details = new RecalPurchInvoiceDetails();
                    details.Product_ID = item.Product_ID;
                    details.Product_Quantity = item.Product_Quantity;
                    details.Product_Price = item.Product_Price;
                    // Deduct the discount from the detail Total_Amount 
                    details.Total_Amount = item.Total_Amount;
                    details.Detail_Total_Tax = item.Detail_Total_Tax;
                    details.Product_Tax = item.Product_Tax;
                    details.Detail_Total_Cost = details.Detail_Total_Cost;
                    // Get the batch related to this detail
                    var productBatch = db.ProductBatches.Where(batch => batch.PurInvoice_ID == item.Details_Id).First();
                    details.Batch_ID = productBatch.Batch_ID;

                    master.Doc_Type = Document_Type.Credit_Note.ToString();
                    master.Inv_TotalAmount += details.Total_Amount;
                    master.Inv_TotalTax += details.Detail_Total_Tax;
                    master.Inv_TotalCost += details.Detail_Total_Cost;
                    master.RecalPurchInvoiceDetails.Add(details);

                }

                if (masterVM.Files != null)
                {
                    foreach (var file in masterVM.Files)
                    {
                        var filedetails = new RecallPurchFileDetails();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLenght = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.ProImg = data;
                        filedetails.File_Type = file.ContentType;
                        if (data.Length < 1000000)
                        {
                            master.RecallPurchFileDetails.Add(filedetails);
                        }

                       
                    }
                }

                //master.Inv_TotalAmount -= master.Inv_TotalAmount * (master.Master_Discount / 100);
                db.RecalPurchInvoiceMasters.Add(master);
                db.SaveChanges();
                var Datenow = DateTime.Now;
                var transBatch = new TransactionBatch();
                transBatch.Date = Datenow;
                transBatch.OperationName = " مردود مشتريات";
                transBatch.OperationId = master.Master_Id;
                transBatch.Description = "مردود مشتريات";
                transBatch.UserId = "";
                db.TransactionBatches.Add(transBatch);

                db.SaveChanges();
                db.Dispose();

                return true;
            }

            return false;
        }
        /// <summary>
        /// make Recall invoice and save  in db and check if there is any pending recall for that purchase invoice 
        /// </summary>
        /// <param name="masterVM"></param>
        /// <param name="UserId"></param>
        /// <returns>Tuple<string,int></returns>
        public static Tuple< string,int> AddNewInvoice(RecalPurchInvoiceMasterVM masterVM , String UserId)
        {
            var db = new AccountingContainer();
            var master = new RecalPurchInvoiceMaster();
            var invoiceMasterTemp = db.RecallPurchInvoicMasterTemps.Find(masterVM.Master_Id);
            var AllReversMaster = db.RecalPurchInvoiceMasters.Where(r => r.PurInvoiceMasterMaster_Id == masterVM.selectedPurInvoiceMasterMaster_Id && r.PI_State == TransactionState.Pending).FirstOrDefault();
            if (AllReversMaster != null)
            {
                return Tuple.Create( " "+AllReversMaster.Recall_MPurch_Serial +" "+ "يوجد فاتورة مردود لهذه الفاتورة فى قائمة الانتظار ",0);
            }
            if (masterVM != null)
            {
                master.Master_Id = masterVM.Master_Id;
                var lastSerial = db.RecalPurchInvoiceMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                if (lastSerial == null)
                {
                    master.Recall_MPurch_Serial = "REVP-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    master.Recall_MPurch_Serial = "REVP-" + (Convert.ToInt32(lastSerial.Recall_MPurch_Serial.Substring(5, lastSerial.Recall_MPurch_Serial.Length - 5)) + 1).ToString("D3");
                }
                
                master.Register_Date = Convert.ToDateTime( masterVM.Due_Date);
                master.Due_Date = Convert.ToDateTime(masterVM.Due_Date);
                var resultInv = db.PurInvoiceMasters.Find(masterVM.selectedPurInvoiceMasterMaster_Id);
                if (resultInv != null)
                {
                    master.PurInvoiceMasterMaster_Id = resultInv.Master_Id;
                }
                var resultsupplier = db.Suppliers.Find(resultInv.Supplier_Id);
                if (resultsupplier != null)
                {
                    master.SupplierSuppId = resultsupplier.Supp_Id;
                }
                else
                {
                    master.SupplierSuppId = 0;
                }
                var resultAccountRevenu = db.ChartOfAccountings.Find(masterVM.SelectedPaymentMethodId);
                if (resultAccountRevenu != null)
                {
                    master.PaymentMethod_Id = resultAccountRevenu.Account_Id;

                }
                else
                {
                    master.PaymentMethod_Id = 0;
                }

                var resultCompany = db.Companies.FirstOrDefault();
                if (resultCompany != null)
                {
                    master.CompanyComp_Id = resultCompany.Comp_Id;
                }
                var resultCurrency = db.Currencies.FirstOrDefault();
                if (resultCurrency != null)
                {
                    master.PI_Currency = resultCurrency.Currency_Type;
                }
                if (resultInv.InvoiceDetails.FirstOrDefault().Asset_Id > 0)
                {
                    master.StoreStore_ID = 0;
                }
                else {
                    
                    if (resultInv.Store_ID != null&& resultInv.Store_ID!=0)
                    {
                        master.StoreStore_ID = resultInv.Store_ID.Value;
                    }
                }


                //  master.StoreStore_ID = masterVM.selectedStoreStore_Id;
                master.Exchange_Rate = resultInv.Exchange_Rate;
                master.Master_Discount = resultInv.Master_Discount;
                master.PI_State = TransactionState.Pending;
                master.Doc_Type = "مردود مشتريات";
                var products = db.Products.ToList();
                if (masterVM.Files.FirstOrDefault() != null)
                {
                    foreach (var file in masterVM.Files)
                    {
                        var filedetails = new RecallPurchFileDetails();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLenght = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.ProImg = data;
                        filedetails.File_Type = file.ContentType;
                        master.RecallPurchFileDetails.Add(filedetails);
                    }
                }
                // loop over details in the VM and add them to the master
                if (invoiceMasterTemp.RecallPurchInvDetailsTemps.ToList().Count() != 0)
                {
                    foreach (var item in invoiceMasterTemp.RecallPurchInvDetailsTemps.ToList())
                    {

                        // Create new detail and map the values from detailVM to it
                        // TODO: Use AutoMapper
                        var details = new RecalPurchInvoiceDetails();
                        details.Details_Id = item.Details_Id;
                        details.Product_Quantity = item.Product_Quantity;
                        details.Product_Price = (decimal)item.Product_Price;
                        // Deduct the discount from the detail Total_Amount 
                        details.Total_Amount = (decimal)item.Total_Amount;
                        details.Detail_Total_Tax = item.Detail_total_Tax;
                        details.Product_Tax = (int)item.Product_Tax;
                        var thistax = db.Taxes.Find(details.Product_Tax);
                        if (thistax != null)
                        {
                            details.Tax_Name = thistax.Tax_Name;
                        }
                        var thisproduct = db.Products.Find(item.Product_Id);
                        if (thisproduct != null)
                        {
                            details.Product_Name = thisproduct.Prod_Name;
                            details.Product_Code = thisproduct.Prod_Code;
                           
                            details.Product_ID = thisproduct.Prod_ID;
                        }
                        else
                        {
                            details.Product_Name = "";
                        }
                        details.Batch_ID = item.Btach_ID;
                        // TEMP: Fix relations
                        //ProductBatches detailBatch = db.ProductBatches.Where(e => e.Product_ID == item.Product_Id && e.Store_ID == resultInv.Store_ID).ToList().FirstOrDefault();
                        //if (detailBatch == null)
                        //{
                        //    details.Asset_Id = item.Asset_Id;
                        //    details.Batch_ID = 0;
                        //    details.Product_ID = 0;
                        //}
                        //else
                        //{
                        //    details.Product_ID = detailBatch.Product_ID;
                        //    details.Batch_ID = detailBatch.Batch_ID;
                        //    details.Asset_Id = 0;
                        //    if (item.Product_Quantity > detailBatch.Product_Quantity)
                        //        return Tuple.Create("لا توجد كمية كافية من منتج " + detailBatch.Product.Prod_Name,0);


                        //}
                        //details.Product_ID = detailBatch.Product_ID;
                        // Checks for the products quantity if available or not

                        // Calculate the Total_Cost
                        details.Detail_Total_Cost = item.Detail_Total_Cost;
                        // Add this detail Total_Amount to the Masters's Total_Amount
                        master.Inv_TotalAmount += details.Total_Amount;
                        // Add this detail Tax to the Masters's Total_Tax
                        master.Inv_TotalTax += details.Detail_Total_Tax;
                        // Add this detail Total Cost to the Masters's Total_Tax
                        master.Inv_TotalCost += details.Detail_Total_Cost;

                        // Add the detail to the master
                        master.RecalPurchInvoiceDetails.Add(details);
                        db.RecallPurchInvDetailsTemps.Remove(item);
                    }
                
                // The total amount after matching is initialized to the total amount

                db.RecalPurchInvoiceMasters.Add(master);

                db.RecallPurchInvoicMasterTemps.Remove(invoiceMasterTemp);
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


                var Datenow = DateTime.Now;
                var transBatch = new TransactionBatch();
                transBatch.Date = Datenow;
                transBatch.OperationName = " مردود مشتريات";
                transBatch.OperationId = master.Master_Id;
                transBatch.Operation_Code = master.Recall_MPurch_Serial;
                transBatch.Description = "مردود مشتريات";
                transBatch.UserId = UserId;
                db.TransactionBatches.Add(transBatch);
                db.SaveChanges();
                db.Dispose();
                return Tuple.Create(Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Therefundinvoicehasbeencreatedsuccessfully, master.Master_Id);
            }
               
                return Tuple.Create(Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.EmptyInvoiceModelMessage, 0);

                
            }    
       
            return Tuple.Create(Resources.validate.Error,0);
}
        public static string DeleteInvoiceFromTemp(int purch)
        {
            var db = new AccountingContainer();
            RecallPurchInvDetailsTemp vm = db.RecallPurchInvDetailsTemps.Find(purch);
            db.RecallPurchInvDetailsTemps.Remove(vm);
            db.SaveChanges();
            db.Dispose();
            return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.deleteFromList; 
        }
        // Gets the ViewModel used for editing the InvoiceMaster 
        public static RecalPurchInvoiceMasterVM GetEditViewModel(int id)
        {
            RecalPurchInvoiceMaster invoice = GetInvoiceDetailsById(id);
            RecalPurchInvoiceMasterVM invoiceMasterVM = new RecalPurchInvoiceMasterVM();
            invoiceMasterVM.PI_State = invoice.PI_State;
            return invoiceMasterVM;
        }
        



        //this  method  is  to  update the  State  of  the RecallInvoice   and   create  its  GL 
        //where  it  have  Post State  and  its  Gl  and  Voide  State  and  its  Gl
        public static string UpdateInvoiceMaster(RecalPurchInvoiceMasterVM invoiceMasterVM)
        {
            var msgOff = "";
            var db = new AccountingContainer();
        
            RecalPurchInvoiceMaster invoice = db.RecalPurchInvoiceMasters.Find(invoiceMasterVM.Master_Id);
            OperationRun.Addnewoperation(invoice.Recall_MPurch_Serial);
            var operation = db.OperationRuns.Where(a => a.Code == invoice.Recall_MPurch_Serial).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(invoice.Recall_MPurch_Serial);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }

            if (invoice.PI_State == invoiceMasterVM.PI_State)
            {
                OperationRun.DeleteOperation(invoice.Recall_MPurch_Serial);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + invoice.PI_State + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            else
            {
                invoice.PI_State = invoiceMasterVM.PI_State;
            }

            var Purchase = db.PurInvoiceMasters.Find(invoice.PurInvoiceMasterMaster_Id);
            var detailsPurchase = Purchase.InvoiceDetails.ToList();
            db.Entry(invoice).State = EntityState.Modified;

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    #region Create GL
                    // Add GLHeader 
                    GLMaster glmaster = new GLMaster();
                    var realDetail = getMaterialOCompoundProduct(invoice.RecalPurchInvoiceDetails.ToList(), invoice.StoreStore_ID);
                    glmaster.GLCode = invoice.Recall_MPurch_Serial;
                    glmaster.GL_Date = invoice.Register_Date;
                    glmaster.GL_Desc = "فاتورة مردود مشتريات";
                    glmaster.Company_Id = invoice.CompanyComp_Id;
                    glmaster.GLType_Id = "مردود مشتريات";
                    glmaster.Currency_Id = invoice.PI_Currency;
                    glmaster.Auto = true;
                    // glmaster.GL_State = TransactionState.Pending;
                    //add GL Detail
                    GLDetail AccountsPayable = new GLDetail(),
                         AccountsPayableDebit = new GLDetail(),
                        PaymentGL = new GLDetail(),
                    taxesGL = new GLDetail(),
                    inventoryGL = new GLDetail();
                    //var PurinvoicePay = db.PurInvoiceMasters.Find(invoice.PurInvoiceMasterMaster_Id).PaymentMethod_Id;

                    if (invoice.PaymentMethod_Id != null && invoice.PaymentMethod_Id != 0)
                    {
                        PaymentGL.Account_Id = invoice.PaymentMethod_Id.Value;
                        PaymentGL.Account_Name = db.ChartOfAccountings.Find(invoice.PaymentMethod_Id.Value).Account_Name;
                    }


                    taxesGL.SubLedger_Id = 0;
                    taxesGL.Description_Detail = "مردود مشتريات";
                    inventoryGL.SubLedger_Id = 0;
                    inventoryGL.Description_Detail = "مردود مشتريات";
                    // Initialize common values
                    // TODO: Fix the following values
                    //var BusId = db.BussinesUnits.FirstOrDefault().BU_Id;
                    AccountsPayable.Bu_Id = 0;
                    inventoryGL.Bu_Id = 0;
                    var ChartOfAccounPay = db.ChartOfAccountings.Find(45);
                    AccountsPayable.Account_Id = ChartOfAccounPay.Account_Id;
                    AccountsPayable.Account_Name = ChartOfAccounPay.Account_Name;
                    AccountsPayableDebit.Account_Id = ChartOfAccounPay.Account_Id;
                    AccountsPayableDebit.Account_Name = ChartOfAccounPay.Account_Name;

                    if (invoice.SupplierSuppId > 0||invoice.PaymentMethod_Id==0)
                    {
                        var supplier = db.Suppliers.Find(invoice.SupplierSuppId);
                        if (supplier == null)
                        {
                            supplier= db.Suppliers.Find(Purchase.Supplier_Id);

                        }
                        var accountSup = db.Accounts.Where(a => a.Account_Code == supplier.Supp_Code).FirstOrDefault();
                        AccountsPayable.Account_Child_ID = accountSup.Accounts_Id;
                        AccountsPayable.Account_CHild_Name = accountSup.Account_Name;
                        AccountsPayableDebit.Account_Child_ID = accountSup.Accounts_Id;
                        AccountsPayableDebit.Account_CHild_Name = accountSup.Account_Name;
                    }


                    AccountsPayable.Account_Id = 45;
                    AccountsPayable.Account_Name = db.ChartOfAccountings.Find(45).Account_Name;

                    if (invoice.Inv_TotalTax == 0 || invoice.Inv_TotalTax == null)
                    {
                    }
                    else
                    {
                        foreach (var taxs in invoice.RecalPurchInvoiceDetails.Where(a => a.Product_Tax != 0 && a.Product_Tax != null).ToList())

                        {
                            var taxofDetail = taxs.Product_Tax;
                            var tax = db.Taxes.Find(taxofDetail);
                            var ChartOfAccounTax = db.ChartOfAccountings.Where(e => e.Account_Code == tax.Tax_Code).ToList().FirstOrDefault();
                            taxesGL.Account_Id = ChartOfAccounTax.Account_Id;
                            taxesGL.Account_Name = ChartOfAccounTax.Account_Name;
                        }

                    }


                    var ChartOfAccounInventory = db.Stores.Find(Purchase.Store_ID).Store_Code;
                    var storeChart = db.ChartOfAccountings.Where(a => a.Account_Code == ChartOfAccounInventory).FirstOrDefault();
                    inventoryGL.Account_Id = storeChart.Account_Id;
                    inventoryGL.Account_Name = storeChart.Account_Name;


                    AccountsPayable.Asset_Id =
                    taxesGL.Asset_Id =
                    inventoryGL.Asset_Id = 0;


                    switch (invoice.PI_State)
                    {
                        //Post Invoice
                        case TransactionState.Post:
                            glmaster.GLState = TransactionState.Post;
                            var gl = db.GLMasters.Where(a => a.GLCode == glmaster.GLCode && a.GLState == glmaster.GLState).FirstOrDefault();
                            if (gl != null)
                            {
                                OperationRun.DeleteOperation(invoice.Recall_MPurch_Serial);
                                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                            }
                            if ((invoice.SupplierSuppId > 0 ||Purchase.Supplier_Id>0)&& (invoice.PaymentMethod_Id == null || invoice.PaymentMethod_Id == 0))
                            {
                                AccountsPayableDebit.GLDetail_Debit = invoice.Inv_TotalCost;
                                AccountsPayableDebit.GLDetail_Credit = 0;
                                glmaster.GLDetails.Add(AccountsPayableDebit);

                            }
                            else if (invoice.SupplierSuppId > 0 && invoice.PaymentMethod_Id > 0)
                            {
                                AccountsPayableDebit.GLDetail_Debit = invoice.Inv_TotalCost;
                                AccountsPayable.GLDetail_Credit = invoice.Inv_TotalCost;
                                PaymentGL.GLDetail_Debit = invoice.Inv_TotalCost;

                                AccountsPayableDebit.GLDetail_Credit = 0;
                                AccountsPayable.GLDetail_Debit = 0;
                                PaymentGL.GLDetail_Credit = 0;

                                glmaster.GLDetails.Add(AccountsPayable);
                                glmaster.GLDetails.Add(AccountsPayableDebit);
                                glmaster.GLDetails.Add(PaymentGL);
                            }
                            else if (invoice.SupplierSuppId > 0 && invoice.PaymentMethod_Id == 0)
                            {
                                PaymentGL.GLDetail_Debit = invoice.Inv_TotalCost;
                                PaymentGL.GLDetail_Credit = 0;
                                glmaster.GLDetails.Add(PaymentGL);
                            }
                            // Supplier GL

                            if (invoice.Inv_TotalTax == 0)
                            { }
                            else
                            {  // Taxes GL
                                taxesGL.GLDetail_Credit = invoice.Inv_TotalTax;
                            }

                            // Inventory GL
                            inventoryGL.GLDetail_Credit = invoice.Inv_TotalAmount;

                            // Update batch quantity
                            db.ProductBatches.Load();
                            var errorMessage = new List<string> { };
                            foreach (var detail in realDetail)
                            {
                                var thisproduct = db.Products.Where(a=>a.Prod_ID==detail.Product_ID).FirstOrDefault();
                                if (thisproduct.Prod_Type == Prod_Type.منتج|| thisproduct.Prod_Type == Prod_Type.اولي)
                                {
                                    ProductBatches detailBatch = db.ProductBatches.Where(a => a.Product_ID == thisproduct.Prod_ID && a.Store_ID == Purchase.Store_ID).ToList().FirstOrDefault();
                                    if (detailBatch.Store_ID != null)
                                    {
                                        if (detail.Product_Quantity > detailBatch.Product_Quantity)
                                        {
                                            var productName = db.Products.Find(thisproduct.Prod_ID).Prod_Name;
                                            invoice.PI_State = TransactionState.Pending;
                                            //db.SaveChanges();
                                            errorMessage.Add(Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Thereisnotenoughproduct + productName);

                                        }
                                    }
                                }
                            }
                            if (errorMessage.Count() != 0)
                            {


                                string combindedString = string.Join("\n", errorMessage.ToArray());
                                OperationRun.DeleteOperation(invoice.Recall_MPurch_Serial);
                                return combindedString;
                            }


                            foreach (var detail in invoice.RecalPurchInvoiceDetails)
                            {
                                var thisproduct = db.Products.Where(a => a.Prod_ID == detail.Product_ID).FirstOrDefault();
                                var reverseDetails = detailsPurchase.Where(p => p.Product_ID == thisproduct.Prod_ID).FirstOrDefault();
                                reverseDetails.Reverse_Quantity = reverseDetails.Reverse_Quantity - detail.Product_Quantity;
                                reverseDetails.Reverse_Total_Tax = reverseDetails.Reverse_Total_Tax - detail.Detail_Total_Tax;
                                reverseDetails.Reverse_Tota_Cost = reverseDetails.Reverse_Tota_Cost - detail.Detail_Total_Cost;
                                db.Entry(reverseDetails).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                                foreach (var detail in realDetail)
                            {
                                var thisproduct = db.Products.Where(a => a.Prod_ID == detail.Product_ID).FirstOrDefault();
                                ProductBatches detailBatch = db.ProductBatches.Where(a=>a.Store_ID==Purchase.Store_ID&&a.Product_ID== thisproduct.Prod_ID).FirstOrDefault();
                                if (detailBatch == null)
                                {
                                    string msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                                    OperationRun.DeleteOperation(invoice.Recall_MPurch_Serial);
                                    return msg;
                                }
                                else if (detailBatch.Product_Quantity == 0)
                                {
                                    string msg = "كمية المخزن تساوي 0";
                                    OperationRun.DeleteOperation(invoice.Recall_MPurch_Serial);
                                    return msg;
                                }
                                else if (detailBatch.Product_Quantity < detail.Product_Quantity)
                                {
                                    string msg = "هذه الكمية اكبر من كمية المخزن";
                                    OperationRun.DeleteOperation(invoice.Recall_MPurch_Serial);
                                    return msg;
                                }
                                else
                                {
                                    detailBatch.Product_Quantity -= detail.Product_Quantity;
                                    // detailBatch.Batch_Cost -= detail.Product_Price * detail.Product_Quantity;
                                    detailBatch.Batch_Cost -= detail.Total_Amount;
                                    if (detailBatch.Product_Quantity == 0 )
                                    {
                                        detailBatch.Product_Cost = 0;
                                        detailBatch.Batch_Cost = 0;
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
                                        storecard.CostPrice = (detail.Detail_Total_Cost - detail.Detail_Total_Tax) / detail.Product_Quantity;
                                    }

                                    storecard.Date = DateTime.Now;
                                    storecard.Store_Id = invoice.StoreStore_ID;
                                    storecard.Quantity = detail.Product_Quantity;
                                    storecard.SalePrice = 0;
                                    storecard.Product_Id = detail.Product_ID;
                                    storecard.IncreaseQuantity = false;
                                    storecard.TypeOfDocument = " فاتورة مردود مشتريات مخزنية";
                                    storecard.Document_Serial = invoice.Recall_MPurch_Serial;
                                    storecard.TotalCost = detail.Detail_Total_Cost - detail.Detail_Total_Tax;
                                    storecard.TotalAmount = 0;
                                    storecard.Account_Id = AccountsPayable.Account_Child_ID;
                                    storecard.Subledger_Id = 0;
                                    Store.AddinginstorCard(storecard);
                                }



                            }


                            break;
                        // Void Invoice
                        case TransactionState.Void:
                            glmaster.GLType_Id = "مردود مشتريات عكسي";
                            glmaster.GLState = TransactionState.Void;
                            var glcheck = db.GLMasters.Where(a => a.GLCode == glmaster.GLCode && a.GLState == glmaster.GLState).FirstOrDefault();
                            if (glcheck != null)
                            {
                                OperationRun.DeleteOperation(invoice.Recall_MPurch_Serial);
                                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                            }

                            if (invoice.HasOff == true)

                            {
                                msgOff = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.UnderCloseing;
                                invoice.PI_State = TransactionState.Post;
                                db.SaveChanges();
                                OperationRun.DeleteOperation(invoice.Recall_MPurch_Serial);
                                return msgOff;

                            }
                            else
                            {
                                if ((invoice.SupplierSuppId > 0|| Purchase.Supplier_Id>0) && (invoice.PaymentMethod_Id == null || invoice.PaymentMethod_Id == 0))
                                {
                                    AccountsPayableDebit.GLDetail_Credit = invoice.Inv_TotalCost;

                                    AccountsPayableDebit.GLDetail_Debit = 0;
                                    glmaster.GLDetails.Add(AccountsPayableDebit);

                                }
                                else if (invoice.SupplierSuppId > 0 && invoice.PaymentMethod_Id > 0)
                                {
                                    AccountsPayableDebit.GLDetail_Credit = invoice.Inv_TotalCost;
                                    AccountsPayable.GLDetail_Debit = invoice.Inv_TotalCost;
                                    PaymentGL.GLDetail_Credit = invoice.Inv_TotalCost;

                                    AccountsPayableDebit.GLDetail_Debit = 0;
                                    AccountsPayable.GLDetail_Credit = 0;
                                    PaymentGL.GLDetail_Debit = 0;

                                    glmaster.GLDetails.Add(AccountsPayable);
                                    glmaster.GLDetails.Add(AccountsPayableDebit);
                                    glmaster.GLDetails.Add(PaymentGL);
                                }
                                else if (invoice.PaymentMethod_Id > 0 && invoice.SupplierSuppId == 0)
                                {
                                    PaymentGL.GLDetail_Credit = invoice.Inv_TotalCost;
                                    PaymentGL.GLDetail_Debit = 0;
                                    glmaster.GLDetails.Add(PaymentGL);
                                }
                                // Supplier GL

                                // Taxes GL
                                if (invoice.Inv_TotalTax == 0)
                                { }
                                else
                                { taxesGL.GLDetail_Debit = invoice.Inv_TotalTax; }

                                // Inventory GL
                                inventoryGL.GLDetail_Debit = invoice.Inv_TotalAmount;

                                // Update batch quantity
                                db.ProductBatches.Load();

                                foreach (var detail in invoice.RecalPurchInvoiceDetails)
                                {
                                    var reverseDetails = detailsPurchase.Where(p => p.Product_ID == detail.Product_ID).FirstOrDefault();
                                    reverseDetails.Reverse_Quantity = reverseDetails.Reverse_Quantity + detail.Product_Quantity;
                                    reverseDetails.Reverse_Total_Tax = reverseDetails.Reverse_Total_Tax + detail.Detail_Total_Tax;
                                    reverseDetails.Reverse_Tota_Cost = reverseDetails.Reverse_Tota_Cost + detail.Detail_Total_Cost;
                                    db.Entry(reverseDetails).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                foreach (var detail in realDetail)
                                {
                                    ProductBatches detailBatch = db.ProductBatches.Where(a=>a.Product_ID==detail.Product_ID&&a.Store_ID==invoice.StoreStore_ID).FirstOrDefault(); 
                                    detailBatch.Product_Quantity += detail.Product_Quantity;
                                    detailBatch.Batch_Cost += detail.Detail_Total_Cost - detail.Detail_Total_Tax;
                                    detailBatch.Product_Cost = (detailBatch.Batch_Cost / detailBatch.Product_Quantity);
                                    db.Entry(detailBatch).State = EntityState.Modified;



                                    var storecard = new StoreCard();
                                    storecard.CostPrice = (detail.Detail_Total_Cost - detail.Detail_Total_Tax) / detail.Product_Quantity;
                                    storecard.Date = DateTime.Now;
                                    storecard.Store_Id = invoice.StoreStore_ID;
                                    storecard.Quantity = detail.Product_Quantity;
                                    storecard.SalePrice = 0;
                                    storecard.Product_Id = detail.Product_ID;
                                    storecard.IncreaseQuantity = true;
                                    storecard.TypeOfDocument = " استبعاد فاتورة مردورد مشتريات";
                                    storecard.Document_Serial = invoice.Recall_MPurch_Serial;
                                    storecard.TotalCost = detail.Detail_Total_Cost - detail.Detail_Total_Tax;
                                    storecard.TotalAmount = 0;
                                    storecard.Account_Id = AccountsPayable.Account_Child_ID;
                                    storecard.Subledger_Id = 0;
                                    Store.AddinginstorCard(storecard);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    if (msgOff == "")
                    {

                        // Add all GL details to the master

                        if (invoice.Inv_TotalTax == 0)
                        { }
                        else
                        { glmaster.GLDetails.Add(taxesGL); }

                        glmaster.GLDetails.Add(inventoryGL);
                        #endregion

                        db.GLMasters.Add(glmaster);

                        GLMaster.GetBalanceForAccount(glmaster);
                        GLMaster.GetBalanceForSubLedger(glmaster);
                        db.SaveChanges();
                        msgOff = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                        transaction.Commit();
                    }
                }
                catch
                {
                    transaction.Rollback();
                    db.Dispose();
                }
            }
            OperationRun.DeleteOperation(invoice.Recall_MPurch_Serial);
            if (invoice.PI_State == TransactionState.Post)
            {
                msgOff = Resources.validate.GLDone;
            }
            else
            {
                msgOff = Resources.validate.GLReverse;
            }
            return msgOff;
        }
        public static List<RecalPurchInvoiceDetails> getMaterialOCompoundProduct(List<RecalPurchInvoiceDetails> Details, int Store_Id)
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
                List<RecalPurchInvoiceDetails> Details2 = new List<RecalPurchInvoiceDetails>();

                foreach (var item in ProList)
                {
                    var SubProductList = ProductAll.Where(a => a.Prod_ID == item.Product_ID).FirstOrDefault().ProductCompounds;
                    foreach (var subProduct in SubProductList)
                    {
                        var dublicate = Details.Where(a => a.Product_ID == subProduct.Product_Id).FirstOrDefault();

                        if (dublicate == null)
                        {
                            var Detailnew = new RecalPurchInvoiceDetails();
                            Detailnew.Product_ID = subProduct.Product_Id;
                            Detailnew.Product_Quantity = subProduct.Quantity * item.Product_Quantity;
                           
                            Detailnew.Total_Amount = item.Total_Amount;
                            Detailnew.Detail_Total_Cost = item.Detail_Total_Cost;
                            Detailnew.Detail_Total_Tax = item.Detail_Total_Tax;
                            Detailnew.Product_Price = item.Total_Amount / Detailnew.Product_Quantity;
                            Detailnew.RecalPurchInvoiceMasterMaster_Id = Details.FirstOrDefault().RecalPurchInvoiceMasterMaster_Id;
                            Details.Add(Detailnew);

                        }
                        else
                        {


                            var Detailnew = new RecalPurchInvoiceDetails();
                            Detailnew.Product_ID = subProduct.Product_Id;
                            Detailnew.Product_Quantity = dublicate.Product_Quantity + (item.Product_Quantity * subProduct.Quantity);
                           
                            Detailnew.Total_Amount = dublicate.Total_Amount + item.Total_Amount;
                            Detailnew.Detail_Total_Tax = dublicate.Detail_Total_Tax + item.Detail_Total_Tax;
                            Detailnew.Detail_Total_Cost = dublicate.Detail_Total_Cost + item.Detail_Total_Cost;
                            Detailnew.Product_Price = item.Total_Amount / Detailnew.Product_Quantity;
                            Detailnew.RecalPurchInvoiceMasterMaster_Id = Details.FirstOrDefault().RecalPurchInvoiceMasterMaster_Id;
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
        /// <summary>
        /// gt id of product from purchase detail purchase
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> getallProducts(int id)
        {
            var db = new AccountingContainer();
            var masterVM = new RecalPurchInvoiceMasterVM();
            IEnumerable<SelectListItem> products = new SelectList(db.PurchaseDetails.Where(p => p.InvoiceMaster_Id == id), "InvoiceMaster_Id", "Product_ID");
            return products;
        }
        //this method  make  list of  prices from  the invoice  as  it  take  invoice id   from the parameter
        public static IEnumerable<SelectListItem> getallprices(int id)
        {
            var db = new AccountingContainer();
            var masterVM = new RecalPurchInvoiceMasterVM();
            IEnumerable<SelectListItem> products = new SelectList(db.PurchaseDetails.Where(p => p.InvoiceMaster_Id == id), "InvoiceMaster_Id", "Product_Price");
            return products;
        }
        //this method  is  to get  the  quantity  of  invoice  product  from  the  parameter  id  as  it  is  the  id  of  the  masterInvoice
        public static SelectListItem GetQuantity(int TypeId)
        {
            var db = new AccountingContainer();
            var Quantity = new SelectList(db.PurchaseDetails.Where(p => p.InvoiceMaster_Id == TypeId), "InvoiceMaster_Id", "Product_Quantity").ToList().FirstOrDefault();
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return Quantity;
        }
        //this method   is   the   get   the  pruchaseinvoice  as  an  object   by its   parameter  Id
        public static PurInvoiceMaster GetPurchInvoice(int invId)
        {
            return PurInvoiceMaster.GetInvoiceDetailsById(invId);
        }
        /// <summary>
        /// get quantity of store and prod of productBatch
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="purchid"></param>
        /// <param name="storeid"></param>
        /// <returns>decimal</returns>
        public static decimal GetStoreQuantity(int proid, int purchid, int storeid)
        {
            var db = new AccountingContainer();
            var SQuantity = db.ProductBatches.Where(e => e.PurInvoice_ID == purchid && e.Product_ID == proid && e.Store_ID == storeid).ToList().LastOrDefault();
            decimal quantity = SQuantity.Product_Quantity;
            return quantity;
        }
        /// <summary>
        /// delete Recall Purchase by using its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteRecallPurInvoice(int id)
        {
            var db = new AccountingContainer();
            RecalPurchInvoiceMaster recallInvoice = db.RecalPurchInvoiceMasters.Find(id);

            if (recallInvoice.PI_State != TransactionState.Pending)
                return "لم يتم الحذف";

            db.RecallPurchFileDetails.RemoveRange(recallInvoice.RecallPurchFileDetails);
            db.RecalPurchInvoiceDetails.RemoveRange(recallInvoice.RecalPurchInvoiceDetails);
            db.RecalPurchInvoiceMasters.Remove(recallInvoice);

            db.SaveChanges();
            db.Dispose();
            return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.deleteDone;
        }

    }

}