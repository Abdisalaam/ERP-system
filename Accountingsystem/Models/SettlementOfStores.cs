using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class SettlementOfStores
    {
        /// <summary>
        /// check that code is unique or not
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns></returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.SettlementOfStores.ToList();
            foreach (var item in allinData)
            {
                if (item.Settlement_Code == Serial || !Serial.StartsWith("SETSTO-"))
                {
                    return true;
                }

                else
                {

                }

            }
            return false;
        }
        // Returns a list of all settlement store
        public static List<SettlementOfStores> GetAllISettlementStore()
        {
            var db = new AccountingContainer();
            var settlementOfStores = db.SettlementOfStores.ToList();
            return settlementOfStores;
        }
        // Gets an settlement store by its id
        public static SettlementOfStores GetSettlemementsById(int settlementId)
        {
            var db = new AccountingContainer();
            var detail = db.SettlementOfStores.Find(settlementId);
            return detail;

        }

        /// <summary>
        /// prepare detail to add to list
        /// </summary>
        /// <param name="prodID"></param>
        /// <param name="storID"></param>
        /// <returns>SettlementOfStoresDetailsVM</returns>
        public static SettlementOfStoresDetailsVM PrepareBatchQuantityInStore(int prodID, int storID)
        {
            var prepareVM = new SettlementOfStoresDetailsVM();
            var db = new AccountingContainer();
            var thisBatch = db.ProductBatches.Where(s => s.Product_ID == prodID && s.Store_ID == storID).FirstOrDefault();
            var thisproduct = db.Products.Where(e => e.Prod_ID == prodID).ToList().FirstOrDefault();
            prepareVM.Product_Quantity = thisBatch.Product_Quantity;
            prepareVM.Product_QuantityReal = thisBatch.Product_Quantity;
            prepareVM.Product_Name = thisproduct.Prod_Name;
            prepareVM.ProductId_In = thisBatch.Product_ID;
            prepareVM.Product_Cost = thisBatch.Product_Cost;
            prepareVM.Product_Cost_InBatch = thisBatch.Product_Cost;
            prepareVM.SettlementOfStoresId = storID;
            return prepareVM;
        }
        /// <summary>
        /// prepare prouduct that has no quantity in the store
        /// </summary>
        /// <param name="prodID"></param>
        /// <param name="storID"></param>
        /// <returns>SettlementOfStoresDetailsVM</returns>
        public static SettlementOfStoresDetailsVM PrepareProductOutStore(int prodID, int storID)
        {
            var prepare = new SettlementOfStoresDetailsVM();
            var db = new AccountingContainer();
            var thisProduct = db.Products.Find(prodID);
            prepare.Product_Quantity = 0;
            prepare.Product_QuantityReal = 0;
            prepare.Product_Name = thisProduct.Prod_Name;
            prepare.SettlementOfStoresId = storID;
            prepare.ProductId_Out = prodID;
            prepare.Product_Cost = thisProduct.Prod_Cost_Price.Value;
            return prepare;
        }
        /// <summary>
        /// get list of detail temp by its master id
        /// </summary>
        /// <param name="settlementId"></param>
        /// <returns></returns>
        public static List<SettlementOfStoresDetailsTemp> GetAllTempSettlementDetails(int settlementId)
        {
            var db = new AccountingContainer();

            var SettlementDetailsTemp = db.SettlementOfStoresDetailsTemps.Where(settement => settement.SettlementOfStoresTempId == settlementId).ToList();
            return SettlementDetailsTemp;
        }
        /// <summary>
        /// add to list in settlement and check the validation
        /// </summary>
        /// <param name="ofStoresDetailsVM"></param>
        /// <returns>string</returns>
        public static string AddDetailsToTemp(SettlementOfStoresDetailsVM ofStoresDetailsVM)
        {
            var db = new AccountingContainer();
            SettlementOfStoresDetailsTemp ofStoresDetailsTemp = new SettlementOfStoresDetailsTemp();
            var settlementTemp = db.SettlementOfStoresTemps.Find(ofStoresDetailsVM.SettlementOfStoresId);

            if (ofStoresDetailsVM.Product_Quantity < 0 )
            {
                return Resources.validate.Quantityshoudbiggerthan;
            }
            if (ofStoresDetailsVM.ProductId_In == 0&& ofStoresDetailsVM.ProductId_Out==0)
            {
                return "اختر المنتج";
            }

            //var batch = db.ProductBatches.Find(ofStoresDetailsVM.ProductId_In);
            var batch = db.ProductBatches.Where(a=>a.Product_ID == ofStoresDetailsVM.ProductId_In && a.Store_ID == settlementTemp.StoreId).ToList().FirstOrDefault();
            var product = db.Products.Find(ofStoresDetailsVM.ProductId_Out);

            ofStoresDetailsTemp.Product_Quantity = ofStoresDetailsVM.Product_Quantity;

           

            if (batch != null)
            {
                if (ofStoresDetailsVM.Product_Quantity == batch.Product_Quantity)
                {
                    return Resources.validate.Quantityisthesameinstore;
                }
                ofStoresDetailsTemp.Product_Id = batch.Product_ID;          
                ofStoresDetailsTemp.Product_Cost = batch.Product_Cost;
                ofStoresDetailsTemp.Real_Quantity = batch.Product_Quantity;
                ofStoresDetailsTemp.Total_Cost_Detail = ofStoresDetailsVM.Product_Quantity * batch.Product_Cost;
                
            }
            else if(product != null)
            {
                if (ofStoresDetailsVM.Product_Quantity <= 0)
                {
                    return "الكمية تساوى صفر من فضلك ادخل الكمية المراد ادخالها فى المخزن";
                }

                ofStoresDetailsTemp.Product_Id = product.Prod_ID;
                ofStoresDetailsTemp.Product_Cost = product.Prod_Cost_Price.Value;
                ofStoresDetailsTemp.Total_Cost_Detail = ofStoresDetailsVM.Product_Quantity * product.Prod_Cost_Price.Value;
            }

            var dtails = settlementTemp.SettlementOfStoresDetailsTemps;
            var dublicateinTemp = dtails.Where(z => z.SettlementOfStoresTempId == settlementTemp.SettlementId && z.Product_Id == ofStoresDetailsTemp.Product_Id).ToList();
            if (dublicateinTemp.FirstOrDefault() != null)
            {
                return Resources.OfferPriceMasterCreateOffer.ProductExisitInListModelMessage;
            }

            settlementTemp.SettlementOfStoresDetailsTemps.Add(ofStoresDetailsTemp);
            db.Entry(settlementTemp).State = EntityState.Modified;
            db.SaveChanges();
            return Resources.OfferPriceMasterCreateOffer.ProductIsAddedToListModelMessage;
        }
        /// <summary>
        /// prepare for Create Stellment by using Store id
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public static SettlementOfStoresVM AddForCreation(int storeId,int ? productInId,int ? ProductoutId)
        {
            var db = new AccountingContainer();
            SettlementOfStoresVM settlementVM = new SettlementOfStoresVM();
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.FirstOrDefault() == null);
            settlementVM.Expences_List = new SelectList(lista.Where(a => a.Account_Code.StartsWith("05")), "Account_Id", "Account_Name");
            settlementVM.Expence_Id = 151;
            var lastSerial = db.SettlementOfStores.OrderByDescending(c => c.SettlementId).FirstOrDefault();
            if (lastSerial == null)
            {
                settlementVM.Settlement_Code = "SETSTO-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                settlementVM.Settlement_Code = "SETSTO-" + (Convert.ToInt32(lastSerial.Settlement_Code.Substring(7, lastSerial.Settlement_Code.Length - 7)) + 1).ToString("D3");
            }
            if (productInId > 0)
            {
                settlementVM.ProductId_In = productInId;
            }
            if (ProductoutId > 0)
            {
                settlementVM.ProductId_Out = ProductoutId;
            }
            var productBatch = db.ProductBatches.Where(a => a.Store_ID == storeId && a.Product_Quantity != 0&&(a.Product.Prod_Type==Prod_Type.منتج|| a.Product.Prod_Type == Prod_Type.اولي)).ToList();
            var productMinus = productBatch.Select(a => a.Product_ID ).Distinct();
            List<Product> AllProductList = db.Products.Where(a => a.Prod_Type == Prod_Type.منتج|| a.Prod_Type == Prod_Type.اولي).ToList();
            List<Product> MinusList = new List<Product>();

            foreach (var item in productMinus)
            {
                Product prod = new Product();
                prod = db.Products.Find(item);
                MinusList.Add(prod);
                AllProductList.Remove(prod);
                
            }
           
            var newListproIn = from Minus in MinusList.Where(a => a.IsActive != false)
                               select new
                               {
                                   Id = Minus.Prod_ID,
                                   Name = Minus.Prod_Name + "   " + Minus.Bar_Code

                               };
           
           
            var newListproout = from product in AllProductList.Where(a=>a.IsActive!=false)
                               select new
                               {
                                   Id = product.Prod_ID,
                                   Name = product.Prod_Name + "   " + product.Bar_Code

                               };

            

          
            settlementVM.MyProductListInStore = new SelectList(newListproIn, "Id", "Name");
            settlementVM.MyProductListOutStore = new SelectList(newListproout, "Id", "Name");
            settlementVM.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            settlementVM.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            settlementVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            settlementVM.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            settlementVM.StoreId = storeId;
            settlementVM.Settlement_State = TransactionState.Pending;

            var settlementStoreTemp = new SettlementOfStoresTemp();
            var id = (from s in db.SettlementOfStoresTemps
                      select s).ToList().LastOrDefault();
            if (id == null)
            { settlementStoreTemp.SettlementId = 1; }
            else
            { settlementStoreTemp.SettlementId = id.SettlementId + 1; }
            settlementStoreTemp.StoreId = storeId;

            db.SettlementOfStoresTemps.Add(settlementStoreTemp);
            db.SaveChanges();
            settlementVM.SettlementId = settlementStoreTemp.SettlementId;
           
            return settlementVM;
        }
        /// <summary>
        /// add new settlement for Create and sae it in db
        /// </summary>
        /// <param name="settlementOfStoresVM"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>

        public static Tuple<string ,int>  AddForCreate(SettlementOfStoresVM settlementOfStoresVM , String UserId)
        {
            var db = new AccountingContainer();

            var master = new SettlementOfStores();
            var settlementOfTemp = db.SettlementOfStoresTemps.Find(settlementOfStoresVM.SettlementId);
            if (settlementOfStoresVM != null)
            {
                master.Settlement_State = TransactionState.Pending;
                master.Date =Convert.ToDateTime( settlementOfStoresVM.Date);
                var lastSerial = db.SettlementOfStores.OrderByDescending(c => c.SettlementId).FirstOrDefault();
                if (lastSerial == null)
                {
                    master.Settlement_Code = "SETSTO-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    master.Settlement_Code = "SETSTO-" + (Convert.ToInt32(lastSerial.Settlement_Code.Substring(7, lastSerial.Settlement_Code.Length - 7)) + 1).ToString("D3");
                }
                if (settlementOfStoresVM.Subledger_Id != null && settlementOfStoresVM.Subledger_Id != 0)
                {

                    master.Subledger_Id = settlementOfStoresVM.Subledger_Id;

                }
                else if (settlementOfStoresVM.BussinesSubledger_Id != null && settlementOfStoresVM.BussinesSubledger_Id != 0)
                {

                    master.Subledger_Id = settlementOfStoresVM.BussinesSubledger_Id;

                }
                else if (settlementOfStoresVM.DepartSubledger_Id != null && settlementOfStoresVM.DepartSubledger_Id != 0)
                {

                    master.Subledger_Id = settlementOfStoresVM.DepartSubledger_Id;

                }
                else if (settlementOfStoresVM.BranchSubledger_Id != null && settlementOfStoresVM.BranchSubledger_Id != 0)
                {

                    master.Subledger_Id = settlementOfStoresVM.BranchSubledger_Id;

                }
                else { master.Subledger_Id = 1; }
                master.BranchSubledger_Id = settlementOfStoresVM.BranchSubledger_Id;
                master.DepartSubledger_Id = settlementOfStoresVM.DepartSubledger_Id;
                master.BussinesSubledger_Id = settlementOfStoresVM.BussinesSubledger_Id;
                master.Expense_Id= settlementOfStoresVM.Expence_Id;


                master.StoreId = settlementOfStoresVM.StoreId;
                if (settlementOfTemp.SettlementOfStoresDetailsTemps.ToList().Count==0)
                {
                    return Tuple.Create("محتويات الفاتورة فارغة", 0);
                }
                // loop over details in the VM and add them to the master
                foreach (var item in settlementOfTemp.SettlementOfStoresDetailsTemps.ToList())
                {

                    // Create new detail and map the values from detailVM to it
                    // TODO: Use AutoMapper
                    var details = new SettlementOfStoresDetails();
                    details.Settlement_DetailsId = item.Settlement_DetailsId;
                    details.Product_Quantity = item.Product_Quantity;
                    details.Real_Quantity = item.Real_Quantity;
                    details.Product_Cost = item.Product_Cost;
                    details.Product_Id = item.Product_Id;
                    
                    var detailBatch = db.ProductBatches.Where(p=>p.Product_ID ==  item.Product_Id && p.Store_ID == master.StoreId).ToList().FirstOrDefault();

                    var thisporduct = db.Products.Find(item.Product_Id);
                    decimal quantityDif = 0;
                    bool increase = true;
                    
                        if (detailBatch != null)
                        {
                            if (item.Product_Quantity > detailBatch.Product_Quantity)
                            {
                                 quantityDif = item.Product_Quantity - detailBatch.Product_Quantity;
                            increase = true;
                                

                            }
                            else 
                            {
                                 quantityDif = detailBatch.Product_Quantity - item.Product_Quantity;
                            increase = false;

                            }
                        if (detailBatch.Product_Cost != 0)
                        {
                            details.Total_Cost_Detail = quantityDif * detailBatch.Product_Cost;
                        }
                        else
                        {
                            details.Total_Cost_Detail = quantityDif * thisporduct.Prod_Cost_Price.Value;
                        }
                        
                    }
                    
                        else if (thisporduct != null)
                        {
                            details.Total_Cost_Detail = item.Product_Quantity * thisporduct.Prod_Cost_Price.Value;
                        quantityDif = item.Product_Quantity;


                        }
                    if (details.Product_Cost == 0)
                    {
                        details.Product_Cost = details.Total_Cost_Detail / quantityDif;
                    }
                    if (increase == true)
                    {
                        master.Total_Cost_Debit += details.Total_Cost_Detail;
                    }
                    else
                    {
                        master.Total_Cost_Credit += details.Total_Cost_Detail;
                    }
                    details.Product_Cost = details.Total_Cost_Detail / quantityDif;
                    master.SettlementOfStoresDetails.Add(details);
                    db.SettlementOfStoresDetailsTemps.Remove(item);
                }
                if(master.Total_Cost_Debit> master.Total_Cost_Credit)
                {
                    master.Total_Cost_Debit = master.Total_Cost_Debit - master.Total_Cost_Credit;
                    master.Total_Cost_Credit = 0;
                }
                else
                {
                    master.Total_Cost_Credit = master.Total_Cost_Credit - master.Total_Cost_Debit;
                    master.Total_Cost_Debit = 0;
                }
                db.SettlementOfStores.Add(master);
                db.SettlementOfStoresTemps.Remove(settlementOfTemp);
                db.SaveChanges();
                var traBatch = new TransactionBatch();

                traBatch.OperationName = "تسوية مخازن";
                traBatch.Description = "تسوية مخازن";
                traBatch.Date = Convert.ToDateTime(master.Date);
                traBatch.UserId = UserId;
                traBatch.OperationId = master.SettlementId;
                traBatch.Operation_Code = master.Settlement_Code;
                TransactionBatch.AddNewTransaction(traBatch);
                db.Dispose();
                return Tuple.Create(Resources.validate.CreateSucc, master.SettlementId);
            }

            return Tuple.Create(Resources.validate.Error,0);
        }
        /// <summary>
        /// delete temp by using id of Detail and Remove from list
        /// </summary>
        /// <param name="DetailID"></param>
        /// <returns></returns>
        public static string DeleteTemp(int DetailID)
        {
            var db = new AccountingContainer();
            var SettlmentTemp = db.SettlementOfStoresDetailsTemps.Find(DetailID);
            db.SettlementOfStoresDetailsTemps.Remove(SettlmentTemp);
            db.SaveChanges();
            db.Dispose();
            return Resources.validate.DeleteSuccessful;
        }
        /// <summary>
        ///  check status before making gl 
        /// </summary>
        /// <param name="settlementOfStoresVM"></param>
        /// <returns>String</returns>

        public static String UpdateSettlementOfStores(SettlementOfStoresVM settlementOfStoresVM)
        {
            var db = new AccountingContainer();

            SettlementOfStores master = db.SettlementOfStores.Find(settlementOfStoresVM.SettlementId);
            OperationRun.Addnewoperation(master.Settlement_Code);
            var operation = db.OperationRuns.Where(a => a.Code == master.Settlement_Code).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(master.Settlement_Code);
                return Resources.InvoiceMasterDetails.WaitingMSGAndTryAgain;
            }
            // Edit master settlement state to the new state
            if (master.Settlement_State == settlementOfStoresVM.Settlement_State)
            {
                OperationRun.DeleteOperation(master.Settlement_Code);
                return Resources.InvoiceMasterDetails.ItsDoneWorking + " " + master.Settlement_State + " " + Resources.InvoiceMasterDetails.Before;
            }
            else
            {
                master.Settlement_State = settlementOfStoresVM.Settlement_State;
            }
            var Mssg = "";
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
               

                    // Create GL for the invoice 
                    Mssg = CreateInvoiceGL(master);
                    db.Entry(master).State = EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    db.Dispose();
                }
            }
            OperationRun.DeleteOperation(master.Settlement_Code);
            db.Dispose();
            if (master.Settlement_State == TransactionState.Post)
            {
                Mssg = Resources.validate.GLDone;
            }
            else
            {
                Mssg =Resources.validate.Error;
            }
            return Mssg;
        }
        // Creats Gl master and details given the master settlement and AccountingContainer and check if it offgl or not 
        public static String CreateInvoiceGL(SettlementOfStores master)
        {
            var db = new AccountingContainer();
            var msg = "";
            // Create GL Master and initialize it from the master invoice
            GLMaster glMaster = new GLMaster();
            if (master.Date == null)
            {
                glMaster.GL_Date = DateTime.Now;
            }
            else
            {
                glMaster.GL_Date = master.Date.Value;
            }
            
            glMaster.GL_Desc = "تسوية مخازن";
            glMaster.Company_Id = db.Companies.FirstOrDefault().Comp_Id;
            glMaster.GLCode = master.Settlement_Code;
            glMaster.Auto = true;
            // TODO: Edit GLType
            glMaster.GLType_Id = "تسوية مخازن";
            // TODO: Edit currency
            glMaster.Currency_Id = db.Currencies.FirstOrDefault().Currency_Type;

            // Initialize GLState to Pending when first created
            glMaster.GLState = TransactionState.Pending;

            // Create five GlDetail
            GLDetail inventoryCreditGL = new GLDetail(),
                inventoryDebitGL = new GLDetail(),
                 GainANdLoseCredit = new GLDetail(),
                GainANdLoseDebit = new GLDetail();

            // Initialize common values
            // TODO: Fix the following values
            //var BusId = db.BussinesUnits.FirstOrDefault().BU_Id;

            inventoryCreditGL.Bu_Id = 0;
            inventoryDebitGL.Bu_Id = 0;
            GainANdLoseCredit.Bu_Id = 0;
            GainANdLoseDebit.Bu_Id = 0;
            ChartOfAccounting ChartOfAccountGainOrLoss = null;
            if (master.Expense_Id > 0)
            {
                ChartOfAccountGainOrLoss = db.ChartOfAccountings.Find(master.Expense_Id);
            }
            else
            {
                ChartOfAccountGainOrLoss = db.ChartOfAccountings.Find(151);
            }
         
            GainANdLoseCredit.Account_Id = ChartOfAccountGainOrLoss.Account_Id;
            GainANdLoseCredit.Account_Name = ChartOfAccountGainOrLoss.Account_Name;
            GainANdLoseDebit.Account_Id = ChartOfAccountGainOrLoss.Account_Id;
            GainANdLoseDebit.Account_Name = ChartOfAccountGainOrLoss.Account_Name;
            
            var store = db.Stores.Find(master.StoreId);
            var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).First();
            inventoryCreditGL.Account_Id = ChartOfAccounInventory.Account_Id;
            inventoryCreditGL.Account_Name = ChartOfAccounInventory.Account_Name;
            inventoryDebitGL.Account_Id = ChartOfAccounInventory.Account_Id;
            inventoryDebitGL.Account_Name = ChartOfAccounInventory.Account_Name;

            inventoryCreditGL.Asset_Id =
            inventoryDebitGL.Asset_Id =
            GainANdLoseCredit.Asset_Id = 
            GainANdLoseDebit.Asset_Id = 0;

            inventoryCreditGL.SubLedger_Id = 0;
            inventoryDebitGL.SubLedger_Id = 0;
            GainANdLoseDebit.SubLedger_Id = master.Subledger_Id;
            GainANdLoseCredit.SubLedger_Id = master.Subledger_Id;
            // store has no subledger only account
            //inventoryGL.SubLedger_Id = int.Parse(master.Store.Store_Code);

            // Check for new master invoice state whether Post or Void 
            switch (master.Settlement_State)
            {
                //Post Invoice
                case TransactionState.Post:
                    glMaster.GLState = TransactionState.Post;
                    var gl = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode && a.GLState == glMaster.GLState).FirstOrDefault();
                    if (gl != null)
                    {
                        return Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade;
                    }

                    inventoryCreditGL.GLDetail_Credit = master.Total_Cost_Credit;
                    GainANdLoseDebit.GLDetail_Debit = master.Total_Cost_Credit;

                    inventoryDebitGL.GLDetail_Debit = master.Total_Cost_Debit;
                    GainANdLoseCredit.GLDetail_Credit = master.Total_Cost_Debit;
                    GainANdLoseCredit.SubLedger_Id = master.Subledger_Id;
                    
                    // Update store quantity
                    db.ProductBatches.Load();
                    foreach (var detail in master.SettlementOfStoresDetails)
                    {
                        ProductBatches detailBatch = db.ProductBatches.Where(a => a.Product_ID == detail.Product_Id && a.Store_ID == master.StoreId).ToList().FirstOrDefault();
                        var _product = db.Products.Where(p => p.Prod_ID == detail.Product_Id).ToList().FirstOrDefault();
                        
                        if (detailBatch != null)
                        {
                            if (detailBatch.Store_ID != null)
                            { //make product card for Report
                                
                                if (detail.Product_Quantity == detailBatch.Product_Quantity)
                                {
                                    master.Settlement_State = TransactionState.Pending;
                                    return Resources.validate.Quantityisthesameinstore+ _product.Prod_Name;

                                }
                            }
                        }
                    } 
                    foreach (var detail in master.SettlementOfStoresDetails) 
                    {
                        ProductBatches detailBatch = db.ProductBatches.Where(a=>a.Product_ID == detail.Product_Id && a.Store_ID == master.StoreId).ToList().FirstOrDefault();
                        var _product = db.Products.Where(p => p.Prod_ID == detail.Product_Id).ToList().FirstOrDefault();
                        ProductBatches productBatches = new ProductBatches();
                        if (detailBatch != null)
                        {
                            if (detailBatch.Store_ID != null)
                            { //make product card for Report
                                var storecard = new StoreCard();
                                
                               
                                storecard.Date = DateTime.Now;
                                storecard.Store_Id = master.StoreId;
                                decimal quantityDiff = 0;
                                if (detail.Product_Quantity < detailBatch.Product_Quantity)
                                {
                                     quantityDiff = detailBatch.Product_Quantity-detail.Product_Quantity ;
                                }
                                else
                                {
                                     quantityDiff = detail.Product_Quantity- detailBatch.Product_Quantity ;
                                }
                                if (quantityDiff == 0)
                                {
                                    storecard.CostPrice = 0;
                                }
                                else
                                {
                                    storecard.CostPrice = detail.Total_Cost_Detail / quantityDiff;
                                }
                                storecard.Quantity = quantityDiff;
                                storecard.SalePrice = 0;
                                storecard.Product_Id = detail.Product_Id;
                                if (detailBatch.Product_Quantity > detail.Product_Quantity)
                                {
                                    storecard.IncreaseQuantity = false;
                                }
                                else
                                {
                                    storecard.IncreaseQuantity = true;
                                }
                               
                                storecard.TypeOfDocument = "تسوية مخازن";
                                storecard.Document_Serial = master.Settlement_Code;
                                storecard.TotalCost = detail.Total_Cost_Detail;
                                storecard.TotalAmount = 0;
                                storecard.Account_Id = 0;
                                storecard.Subledger_Id = 0;
                                Store.AddinginstorCard(storecard);



                                detailBatch.Product_Quantity = detail.Product_Quantity;
                                if (storecard.IncreaseQuantity == true)
                                {
                                    detailBatch.Batch_Cost += detail.Total_Cost_Detail;
                                }
                                else
                                {
                                    detailBatch.Batch_Cost -= detail.Total_Cost_Detail;
                                }
                                if (detailBatch.Product_Quantity != 0)
                                {
                                    detailBatch.Product_Cost = detailBatch.Batch_Cost / detailBatch.Product_Quantity;
                                }
                                else
                                {
                                    detailBatch.Product_Cost = 0;
                                }
                                db.Entry(detailBatch).State = EntityState.Modified;
                                db.SaveChanges();

                            }
                        }
                        else if (_product != null)
                        {
                            var storecard = new StoreCard();
                            if (detail.Product_Quantity == 0)
                            {
                                storecard.CostPrice = 0;
                            }
                            else
                            {
                                storecard.CostPrice = detail.Total_Cost_Detail / detail.Product_Quantity;
                            }
                       
                            storecard.Date = DateTime.Now;
                            storecard.Store_Id = master.StoreId;
                            storecard.Quantity = detail.Product_Quantity;
                            storecard.SalePrice = 0;
                            storecard.Product_Id = detail.Product_Id;
                           
                                storecard.IncreaseQuantity = true;
                            
                           

                            storecard.TypeOfDocument = "تسوية مخازن";
                            storecard.Document_Serial = master.Settlement_Code;
                            storecard.TotalCost = detail.Total_Cost_Detail;
                            storecard.TotalAmount = 0;
                            storecard.Account_Id = 0;
                            storecard.Subledger_Id = 0;
                            Store.AddinginstorCard(storecard);



                            productBatches.Product_ID = detail.Product_Id;
                            productBatches.Store_ID = master.StoreId;
                            productBatches.Product_Quantity = detail.Product_Quantity;
                            productBatches.Batch_Cost = _product.Prod_Cost_Price.Value * detail.Product_Quantity;
                            productBatches.Product_Cost = productBatches.Batch_Cost / productBatches.Product_Quantity;
                            db.ProductBatches.Add(productBatches);
                            db.SaveChanges();
                        }
                       

                    }
     
                    break;
                // Void Invoice
                case TransactionState.Void:
                   glMaster.GLState = TransactionState.Void;
                    var glcheck = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode && a.GLState == glMaster.GLState).FirstOrDefault();
                    if (glcheck != null)
                    {
                        return Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade;
                    }

                    if (master.HasOff == true)
                    {
                        msg = Resources.InvoiceMasterDetails.InvoiceHasBeenIncluded;
                        master.Settlement_State = TransactionState.Post;
                        db.SaveChanges();
                        return msg;
                    }
                    else
                    {

                        inventoryCreditGL.GLDetail_Debit = master.Total_Cost_Credit;
                        GainANdLoseDebit.GLDetail_Credit = master.Total_Cost_Credit;

                        inventoryDebitGL.GLDetail_Credit = master.Total_Cost_Debit;
                        GainANdLoseCredit.GLDetail_Debit = master.Total_Cost_Debit;

                        foreach (var detail in master.SettlementOfStoresDetails)
                        {
                            ProductBatches detailBatch = db.ProductBatches.Where(a => a.Product_ID == detail.Product_Id && a.Store_ID == master.StoreId).ToList().FirstOrDefault();
                            var _product = db.Products.Where(p => p.Prod_ID == detail.Product_Id).ToList().FirstOrDefault();
                            ProductBatches productBatches = new ProductBatches();
                            decimal diffQuantity = 0;
                           
                            if (detailBatch.Store_ID != null)
                            {
                                if (detail.Product_Quantity < detail.Real_Quantity)
                                {

                                    diffQuantity  = detail.Real_Quantity - detail.Product_Quantity;
                                    detailBatch.Product_Quantity += diffQuantity;
                                    detailBatch.Batch_Cost += detail.Product_Cost* diffQuantity;
                                    
                                }
                                else
                                {
                                    diffQuantity = detail.Product_Quantity - detail.Real_Quantity;
                                    detailBatch.Product_Quantity -= diffQuantity;
                                    detailBatch.Batch_Cost -= detail.Product_Cost * diffQuantity;
                                }
                                if (detailBatch.Product_Quantity==0)
                                {
                                    detailBatch.Product_Cost = 0;
                                        detailBatch.Batch_Cost = 0;
                                }
                                else
                                {
                                    detailBatch.Product_Cost = detailBatch.Batch_Cost / detailBatch.Product_Quantity;
                                }

                                
                                db.Entry(detailBatch).State = EntityState.Modified;
                            }
                        }
                     
                    }
                    var storCardVoid = db.StoreCards.Where(a => a.Document_Serial == master.Settlement_Code).ToList();
                    foreach (var item in storCardVoid)
                    {
                        var storecard = new StoreCard();

                        storecard.CostPrice = item.CostPrice;
                        storecard.Date = DateTime.Now;
                        storecard.Store_Id = item.Store_Id;
                        storecard.Quantity = item.Quantity;
                        storecard.SalePrice = 0;
                        storecard.Product_Id = item.Product_Id;
                        if (item.IncreaseQuantity == true)
                        {
                            storecard.IncreaseQuantity = false;
                        }
                        else
                        {
                            storecard.IncreaseQuantity = true;
                        }
                        storecard.TypeOfDocument = "استبعاد تسوية مخزن";
                        storecard.Document_Serial = item.Document_Serial;
                        storecard.TotalCost = item.TotalCost;
                        storecard.TotalAmount = 0;
                        storecard.Account_Id = 0;
                        storecard.Subledger_Id = 0;
                        Store.AddinginstorCard(storecard);




                    }


                    break;
                default:
                    break;
            }

            if (msg == "")
            {

                msg = Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade;
                // Add all GL details to the master
                if (master.Total_Cost_Credit == 0)
                {

                }
                else
                {
                    glMaster.GLDetails.Add(inventoryCreditGL);
                    glMaster.GLDetails.Add(GainANdLoseDebit);
                   
                }
                if (master.Total_Cost_Debit == 0)
                {

                }
                else
                {
                    glMaster.GLDetails.Add(inventoryDebitGL);
                    glMaster.GLDetails.Add(GainANdLoseCredit);
                }
                
           
                db.GLMasters.Add(glMaster);
                GLMaster.GetBalanceForAccount(glMaster);
                GLMaster.GetBalanceForSubLedger(glMaster);
                db.SaveChanges();
            }

            return msg;
        }

        // Deletes the master settlement and its details given the id
        public static String DeleteSettlementOfStore(int settlementId)
        {
            var db = new AccountingContainer();
            SettlementOfStores settlement = db.SettlementOfStores.Find(settlementId);

            if (settlement.Settlement_State != TransactionState.Pending)
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.NotDeleted;
            db.SettlementOfStoresDetails.RemoveRange(settlement.SettlementOfStoresDetails);
            db.SettlementOfStores.Remove(settlement);

            db.SaveChanges();
            db.Dispose();
            return Resources.validate.DeleteSuccessful;
        }

        // get all detail of settlement by using its id
        public static List<SettlementOfStoresDetails> GetAllSettlementDetails(int settlementId)
        {
            var db = new AccountingContainer();
            var Details = db.SettlementOfStoresDetails.Where(s => s.Settlement_DetailsId == settlementId).ToList();
            return Details;
        }
        // get all detail of settlement by using its id
        public static List<SettlementOfStoresDetails> GetAllSettlementDetails2(int settlementId)
        {
            var db = new AccountingContainer();
            var Details = db.SettlementOfStoresDetails.Where(s => s.SettlementOfStoresId == settlementId).ToList();
            return Details;
        }
        /// <summary>
        /// prepare the detail for Editing in Settlement
        /// </summary>
        /// <param name="prodID"></param>
        /// <param name="storID"></param>
        /// <param name="serial"></param>
        /// <returns>SettlementOfStoresDetailsVM </returns>
        public static SettlementOfStoresDetailsVM PrepareBatchQuantityInStoreForEdit(int prodID, int storID , int serial)
        {
            var prepareVM = new SettlementOfStoresDetailsVM();
            var db = new AccountingContainer();
            var thisdetails = GetAllSettlementDetails(serial).ToList().FirstOrDefault();

            //var thisBatch = db.ProductBatches.Where(s => s.Product_ID == prodID && s.Store_ID == storID).FirstOrDefault();
            var thisproduct = db.Products.Where(e => e.Prod_ID == prodID).ToList().FirstOrDefault();
            prepareVM.Product_Quantity = thisdetails.Product_Quantity;
            prepareVM.Product_QuantityReal = thisdetails.Product_Quantity;
            prepareVM.Product_Name = thisproduct.Prod_Name;
            prepareVM.ProductId_In = thisdetails.Product_Id;
            prepareVM.Product_Cost = thisdetails.Product_Cost;
            prepareVM.Product_Cost_InBatch = thisdetails.Product_Cost;
            prepareVM.SettlementOfStoresId = storID;
            return prepareVM;
        }
        /// <summary>
        /// prepare detail for Editing depend on parameter
        /// </summary>
        /// <param name="prodID"></param>
        /// <param name="storID"></param>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static SettlementOfStoresDetailsVM PrepareProductOutStoreForEdit(int prodID, int storID , int serial)
        {
            var prepare = new SettlementOfStoresDetailsVM();
            var db = new AccountingContainer();
            var thisdetails = GetAllSettlementDetails(serial).ToList().FirstOrDefault();
            var thisProduct = db.Products.Find(prodID);
            prepare.Product_Quantity = thisdetails.Product_Quantity;
            prepare.Product_QuantityReal = thisdetails.Product_Quantity;
            prepare.Product_Name = thisProduct.Prod_Name;
            prepare.SettlementOfStoresId = storID;
            prepare.ProductId_Out = thisdetails.Product_Id;
            prepare.Product_Cost = thisdetails.Product_Cost;
            return prepare;
        }
        /// <summary>
        /// get List of detail debend on thiere master id
        /// </summary>
        /// <param name="settlementId"></param>
        /// <returns></returns>

        public static List<SettlementOfStoresDetailsTemp> GetAllTempDetailsresult2(int settlementId)
        {
            var db = new AccountingContainer();
            var detailTemp = db.SettlementOfStoresTemps.Find(settlementId);
            db.SettlementOfStoresDetailsTemps.RemoveRange(detailTemp.SettlementOfStoresDetailsTemps);
            db.SaveChanges();
            var settlement = db.SettlementOfStores.Find(settlementId);
            List<SettlementOfStoresDetailsTemp> Lista = new List<SettlementOfStoresDetailsTemp> { };
            foreach (var item in settlement.SettlementOfStoresDetails)
            {
                var settlementOfStoresDetailsTemp = new SettlementOfStoresDetailsTemp();
                settlementOfStoresDetailsTemp.Product_Id = item.Product_Id;
                settlementOfStoresDetailsTemp.Product_Quantity = item.Product_Quantity;
                settlementOfStoresDetailsTemp.Real_Quantity = item.Real_Quantity;
                settlementOfStoresDetailsTemp.Product_Cost = item.Product_Cost;
                settlementOfStoresDetailsTemp.Total_Cost_Detail = item.Total_Cost_Detail;
                settlementOfStoresDetailsTemp.SettlementOfStoresTempId = settlementId;
             
                Lista.Add(settlementOfStoresDetailsTemp);

            }
            db.SettlementOfStoresDetailsTemps.AddRange(Lista);

            db.SaveChanges();

            return detailTemp.SettlementOfStoresDetailsTemps.ToList();
        }
        /// <summary>
        /// prepare detail for Editing for adding to list
        /// </summary>
        /// <param name="ofStoresDetailsVM"></param>
        /// <returns></returns>

        public static string AddDetailsToTempForEdit(SettlementOfStoresDetailsVM ofStoresDetailsVM)
        {
            var db = new AccountingContainer();
            SettlementOfStoresDetailsTemp ofStoresDetailsTemp = new SettlementOfStoresDetailsTemp();
            var settlementTemp = db.SettlementOfStoresTemps.Find(ofStoresDetailsVM.SettlementOfStoresId);

            if (ofStoresDetailsVM.Product_Quantity < 0)
            {
                return "ادخل الكمية من فضلك";
            }

            //var batch = db.ProductBatches.Find(ofStoresDetailsVM.ProductId_In);
            var batch = db.ProductBatches.Where(a => a.Product_ID == ofStoresDetailsVM.ProductId_In && a.Store_ID == settlementTemp.StoreId).ToList().FirstOrDefault();
            var product = db.Products.Find(ofStoresDetailsVM.ProductId_Out);

            ofStoresDetailsTemp.Product_Quantity = ofStoresDetailsVM.Product_Quantity;

            if (batch != null)
            {
                if (ofStoresDetailsVM.Product_Quantity == batch.Product_Quantity)
                {
                    return "الكمية تساوى الكمية من فضلك ادخل الكمية المراد ادخالها فى المخزن";
                }
                ofStoresDetailsTemp.Product_Id = batch.Product_ID;
                ofStoresDetailsTemp.Product_Cost = batch.Product_Cost;
                ofStoresDetailsTemp.Real_Quantity = batch.Product_Quantity;
                ofStoresDetailsTemp.Total_Cost_Detail = ofStoresDetailsVM.Product_Quantity * batch.Product_Cost;

            }
            else if (product != null)
            {
                ofStoresDetailsTemp.Product_Id = product.Prod_ID;
                ofStoresDetailsTemp.Product_Cost = product.Prod_Cost_Price.Value;
                ofStoresDetailsTemp.Total_Cost_Detail = ofStoresDetailsVM.Product_Quantity * product.Prod_Cost_Price.Value;
            }

            var dtails = settlementTemp.SettlementOfStoresDetailsTemps;
            var dublicateinTemp = dtails.Where(z => z.SettlementOfStoresTempId == settlementTemp.SettlementId && z.Product_Id == ofStoresDetailsTemp.Product_Id).ToList();
            if (dublicateinTemp.FirstOrDefault() != null)
            {
                return "هذا المنتج موجود بالفعل فى القائمة لا يمكن اضافته مرة اخرى";
            }
            ofStoresDetailsTemp.SettlementOfStoresTempId = ofStoresDetailsVM.SettlementOfStoresId;

            settlementTemp.SettlementOfStoresDetailsTemps.Add(ofStoresDetailsTemp);
            db.Entry(settlementTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "تمت الاضافة بنجاح";
        }

        /// <summary>
        /// prepare master to ediiting  by its id
        /// </summary>
        /// <param name="settmentId"></param>
        /// <returns>SettlementOfStoresVM</returns>
        public static SettlementOfStoresVM EditForCreation(int settmentId)
        {
            var db = new AccountingContainer();
            var editModel = db.SettlementOfStores.Find(settmentId);
            SettlementOfStoresVM settlementVM = new SettlementOfStoresVM();
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.FirstOrDefault() == null);
            settlementVM.Expences_List = new SelectList(lista.Where(a => a.Account_Code.StartsWith("05")), "Account_Id", "Account_Name");
            if (editModel.Expense_Id > 0)
            {
                settlementVM.Expence_Id = editModel.Expense_Id.Value;
            }
            else
            {
                settlementVM.Expence_Id = 151;
            }
            settlementVM.SettlementId = editModel.SettlementId;
            settlementVM.Settlement_Code = editModel.Settlement_Code;

            var productBatch = db.ProductBatches.Where(a => a.Store_ID == editModel.StoreId && a.Product_Quantity != 0 && a.Product_Cost != 0).ToList();
            var productMinus = productBatch.Select(a => a.Product_ID).Distinct();
            List<Product> AllProductList = db.Products.ToList();
            List<Product> MinusList = new List<Product>();

            foreach (var item in productMinus)
            {
                Product prod = new Product();
                prod = db.Products.Find(item);
                MinusList.Add(prod);
                AllProductList.Remove(prod);

            }
            //settlementVM.MyProductListInStore = new SelectList(MinusList.Where(a => a.Prod_Type == Prod_Type.منتج), "Prod_ID", "Prod_Name");
            //settlementVM.MyProductListOutStore = new SelectList(AllProductList.Where(a => a.Prod_Type == Prod_Type.منتج && a.Prod_Cost_Price != 0), "Prod_ID", "Prod_Name");
            List<object> newListproIn = new List<object>();
            foreach (var member in MinusList)
                newListproIn.Add(new
                {
                    Id = member.Prod_ID,
                    Name = member.Prod_Name + "   " + member.Bar_Code
                });
            List<object> newListproout = new List<object>();

            foreach (var member in AllProductList)
                newListproout.Add(new
                {
                    Id = member.Prod_ID,
                    Name = member.Prod_Name + "   " + member.Bar_Code
                });


            settlementVM.MyProductListInStore = new SelectList(newListproIn, "Id", "Name");
            settlementVM.MyProductListOutStore = new SelectList(newListproout, "Id", "Name");
            //settlementVM.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            //settlementVM.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            //settlementVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            settlementVM.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            settlementVM.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            if (editModel.BranchSubledger_Id > 0)
            {
                settlementVM.DepartSubList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == editModel.BranchSubledger_Id && p.isActive != false && p.Subled_Level == 2), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                settlementVM.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
                settlementVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            }
            if (editModel.DepartSubledger_Id > 0)
            {
                settlementVM.BussinessList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == editModel.DepartSubledger_Id && p.isActive != false), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                settlementVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.SubLed_Type == "نشاط لقسم" && a.isActive != false), "SubLed_Id", "SubLed_Name");

            }
            settlementVM.StoreId = editModel.StoreId;
            settlementVM.Settlement_State = TransactionState.Pending;
            settlementVM.BranchSubledger_Id = editModel.BranchSubledger_Id;
            settlementVM.DepartSubledger_Id = editModel.DepartSubledger_Id;
            settlementVM.BussinesSubledger_Id = editModel.BussinesSubledger_Id;
            var settlementStoreTemp = new SettlementOfStoresTemp();

            var ThereORN = db.SettlementOfStoresTemps.Find(editModel.SettlementId);
            if (ThereORN != null)
            {

            }
            else
            {
                settlementStoreTemp.SettlementId = editModel.SettlementId;
                settlementStoreTemp.StoreId = editModel.StoreId;
                db.SettlementOfStoresTemps.Add(settlementStoreTemp);
            }
            db.SaveChanges();
            settlementVM.Date = editModel.Date.ToString();
            return settlementVM;
        }
        /// <summary>
        /// save change in Editing and save in db 
        /// </summary>
        /// <param name="settlementOfStoresVM"></param>
        /// <param name="UserId"></param>
        /// <returns> Tuple< string,int></returns>
        public static Tuple< string,int> Edit(SettlementOfStoresVM settlementOfStoresVM ,String UserId)
        {
            var db = new AccountingContainer();

            var master = db.SettlementOfStores.Find(settlementOfStoresVM.SettlementId);
            var settlementOfTemp = db.SettlementOfStoresTemps.Find(settlementOfStoresVM.SettlementId);
            if (settlementOfStoresVM != null)
            {
                master.Settlement_State = TransactionState.Pending;
                //master.Date = DateTime.Now;
                // master.Settlement_Code = settlementOfStoresVM.Settlement_Code;
                master.Expense_Id = settlementOfStoresVM.Expence_Id;
                master.StoreId = settlementOfStoresVM.StoreId;
                master.Total_Cost_Debit = 0;
                master.Total_Cost_Credit = 0;
                // loop over details in the VM and add them to the master

                var detailListTemp = db.SettlementOfStoresDetailsTemps.Where(s => s.SettlementOfStoresTempId == settlementOfTemp.SettlementId).ToList();
                var oldDetail = db.SettlementOfStoresDetails.Where(s => s.SettlementOfStoresId == master.SettlementId).ToList();
                if (settlementOfStoresVM.Subledger_Id != null && settlementOfStoresVM.Subledger_Id != 0)
                {

                    master.Subledger_Id = settlementOfStoresVM.Subledger_Id;

                }
                else if (settlementOfStoresVM.BussinesSubledger_Id != null && settlementOfStoresVM.BussinesSubledger_Id != 0)
                {

                    master.Subledger_Id = settlementOfStoresVM.BussinesSubledger_Id;

                }
                else if (settlementOfStoresVM.DepartSubledger_Id != null && settlementOfStoresVM.DepartSubledger_Id != 0)
                {

                    master.Subledger_Id = settlementOfStoresVM.DepartSubledger_Id;

                }
                else if (settlementOfStoresVM.BranchSubledger_Id != null && settlementOfStoresVM.BranchSubledger_Id != 0)
                {

                    master.Subledger_Id = settlementOfStoresVM.BranchSubledger_Id;

                }
                else { master.Subledger_Id = 1; }
                master.BranchSubledger_Id = settlementOfStoresVM.BranchSubledger_Id;
                master.DepartSubledger_Id = settlementOfStoresVM.DepartSubledger_Id;
                master.BussinesSubledger_Id = settlementOfStoresVM.BussinesSubledger_Id;
                List<SettlementOfStoresDetailsTemp> ListDetail = new List<SettlementOfStoresDetailsTemp>();
                if (detailListTemp.Count()==0) {
                    return Tuple.Create("محتويات التسوية فارغة", 0);

                }
                foreach (var item in detailListTemp)
                {

                    // Create new detail and map the values from detailVM to it
                    // TODO: Use AutoMapper
                    var details = new SettlementOfStoresDetails();
                    details.Settlement_DetailsId = item.Settlement_DetailsId;
                    details.Product_Quantity = item.Product_Quantity;
                    details.Real_Quantity = item.Real_Quantity;
                    details.Product_Cost = item.Product_Cost;
                    details.Product_Id = item.Product_Id;

                    var detailBatch = db.ProductBatches.Where(p => p.Product_ID == item.Product_Id && p.Store_ID == master.StoreId&&p.Product_Quantity!=0).ToList().FirstOrDefault();
                    var thisporduct = db.Products.Find(item.Product_Id);

                    if (detailBatch != null)
                    {
                        if (item.Product_Quantity > detailBatch.Product_Quantity)
                        {
                            var quantityDif = item.Product_Quantity - detailBatch.Product_Quantity;
                            details.Total_Cost_Detail = quantityDif * detailBatch.Product_Cost;
                            master.Total_Cost_Debit += details.Total_Cost_Detail;

                        }
                        else
                        {
                            var quantityDif = detailBatch.Product_Quantity - item.Product_Quantity;
                            details.Total_Cost_Detail = quantityDif * detailBatch.Product_Cost;
                            master.Total_Cost_Credit += details.Total_Cost_Detail;

                        }


                    }
                    else if (thisporduct != null)
                    {
                        details.Total_Cost_Detail = item.Product_Quantity * thisporduct.Prod_Cost_Price.Value;
                        master.Total_Cost_Debit += details.Total_Cost_Detail;
                    }

                    master.SettlementOfStoresDetails.Add(details);
                    ListDetail.Add(item);
                }
                master.Date = Convert.ToDateTime(settlementOfStoresVM.Date);
                db.SettlementOfStoresDetailsTemps.RemoveRange(ListDetail);
                db.Entry(master).State = EntityState.Modified;
                db.SettlementOfStoresTemps.Remove(settlementOfTemp);
                db.SettlementOfStoresDetails.RemoveRange(oldDetail);
                db.SaveChanges();

                var traBatch = new TransactionBatch();

                traBatch.OperationName = "تعديل تسوية مخازن";
                traBatch.Description = "تعديل تسوية مخازن";
                traBatch.Date = Convert.ToDateTime(master.Date);
                traBatch.UserId = UserId;
                traBatch.OperationId = master.SettlementId;
                traBatch.Operation_Code = master.Settlement_Code;
                TransactionBatch.AddNewTransaction(traBatch);
                db.Dispose();
                return Tuple.Create( Resources.validate.Editsucc,master.SettlementId);
            }

            return Tuple.Create( Resources.validate.Error,0);
        }
    }
}