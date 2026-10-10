using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;

using System.Linq;

using System.Web.Mvc;



namespace Accountingsystem.Models
{
    public partial class InventoryTransfer_Hd
    {

        // use this method to  Get List of InventoryTransfer_Hd from Data base.
        public static List<InventoryTransfer_Hd> getAllInvTransferes()
        {
            var db = new AccountingContainer();
            var Transferes = db.InventoryTransfer_Hd.ToList();
            return Transferes;
        }


        // use this method to  Get one Element GetInventoryById from Data base. 
        /// <summary>
        /// GetInventoryById
        /// </summary>
        /// <param name="id"></param>
        /// <returns>InventoryTransfer_Hd</returns>
        public static InventoryTransfer_Hd GetInventoryById(int Id)
        {
            var db = new AccountingContainer();
            var Invens = db.InventoryTransfer_Hd.Find(Id);
            return Invens;

        }
        /// <summary>
        /// get product quantity for that batch
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public static decimal GetAvailbleProductQty(int batchId)
        {
            var db = new AccountingContainer();
            var batch = db.ProductBatches.Find(batchId);
            return batch.Product_Quantity;

        }
        /// <summary>
        /// get vm detail for tansfer and filter the batch  fot that store 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="Transfer"></param>
        /// <returns></returns>
        public static InventorytransferHdDetailsVM PrepareInventoryeH_dDetail(int storeId ,int Transfer)
        {
            var db = new AccountingContainer();
            var InventoryH_dDetailsVM = new InventorytransferHdDetailsVM();
            var newListpro = from prod in db.ProductBatches.Where(batch => batch.Store_ID == storeId&&batch.Product_Quantity>0&&batch.Product.IsActive!=false)select new{
                Id = prod.Batch_ID,
                Name = prod.Product.Prod_Name + "   " + prod.Product.Bar_Code

            };
            
            //List<ProductBatchSelection> _ProductBatchSelection = new List<ProductBatchSelection>();
            //foreach (var item in batchs)
            //{
            //    var productBatchSelection = new ProductBatchSelection();

            //    productBatchSelection.Batch_Id = item.Batch_ID;
            //    productBatchSelection.ProductCost = item.Product_Cost;
            //    productBatchSelection.ProductName = item.Product.Prod_Name;
            //    productBatchSelection.Product_Code = db.Products.Find(item.Product_ID).Bar_Code;
            //    productBatchSelection.Product_Id = item.Product_ID;
            //    _ProductBatchSelection.Add(productBatchSelection);


            //}
       
            
            //foreach (var member in _ProductBatchSelection)
            //    newListpro.Add(new
            //    {
            //        Id = member.Batch_Id,
            //        Name = member.ProductName + "   " + member.Product_Code
            //    });

            InventoryH_dDetailsVM.MyBatchsList = new SelectList(newListpro, "Id", "Name");
            InventoryH_dDetailsVM.MyProductsList = new SelectList(newListpro, "Id", "Name");
            //InventoryH_dDetailsVM.MyBatchsList = new SelectList(_ProductBatchSelection, "Batch_Id", "ProductName");
            //InventoryH_dDetailsVM.MyProductsList = new SelectList(_ProductBatchSelection, "Product_Id", "ProductName");
            
            return InventoryH_dDetailsVM;

        }

        public static List<InventoryTransfer_DetailTemp> GetAllTempInventoryeDetails(int tempInventoryH_dId,int delete)
        {
            var db = new AccountingContainer();
            if (delete == 1)
            {
                db.InventoryTransfer_DetailTemp.RemoveRange(db.InventoryTransfer_DetailTemp.Where(Inventory => Inventory.InventoryTransfer_Temp_Id == tempInventoryH_dId));
                db.SaveChanges();
            }
            var InventoryTransfer_DetailTemp = db.InventoryTransfer_DetailTemp.Where(Inventory => Inventory.InventoryTransfer_Temp_Id == tempInventoryH_dId).ToList();
            return InventoryTransfer_DetailTemp;
        }

        /// <summary>
        /// for details for transfer by transfer _Id
        /// </summary>
        /// <param name="tempInventoryH_dId"></param>
        /// <returns></returns>
        public static List<InventoryTransfer_Detail> GetAllTempInventoryeDetailresult(int tempInventoryH_dId)
        {
          var db = new AccountingContainer();
          var Transfertemp = db.InventoryTransfer_Detail.Where(iv=>iv.InventoryTransfer_Id== tempInventoryH_dId).ToList();
           return Transfertemp;
        }

      
        // spacific List of Inventory Transfer copany and BussinesUnits
        // Currencies and Currencies And Inventory Source store and Inventory Destnation store 
        // (return Data another Tabel )
        public static InventoryTransfer_HdVM CreateTransferForCreation()
        {
            var db = new AccountingContainer();
            var InvTransVM = new InventoryTransfer_HdVM();
            InvTransVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id ", "Comp_Name");
            InvTransVM.MyBusiness_UnitIdList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            InvTransVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            InvTransVM.MySoursStoreList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID ", "Store_Name");
            InvTransVM.MyDestStoreList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            InvTransVM.MyProductsList = new SelectList(db.Products, "Prod_ID", "Prod_Name");
            InvTransVM.Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
            InvTransVM.Tranfer_State = TransactionState.Pending;


          


            //sort the Transfer and get the last insert Transfer.
            var lastTransfer = db.InventoryTransfer_Hd.OrderByDescending(c => c.TransferHd_Id).FirstOrDefault();
            if (lastTransfer == null)
            {
                InvTransVM.Inv_Code = "TRS-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                InvTransVM.Inv_Code = "TRS-" + (Convert.ToInt32(lastTransfer.Inv_Code.Substring(4, lastTransfer.Inv_Code.Length - 4)) + 1).ToString("D3");
            }


            var InventoryTransfer_HdTemp = new InventoryTransfer_HdTemp();
            var id = (from s in db.InventoryTransfer_HdTemp
                      select s.TransferHd_Id).ToList().LastOrDefault();
            if (id==0)
            { InventoryTransfer_HdTemp.TransferHd_Id =  1; }
            else
            { InventoryTransfer_HdTemp.TransferHd_Id = id + 1; }
           

            db.InventoryTransfer_HdTemp.Add(InventoryTransfer_HdTemp);
            db.SaveChanges();
            InvTransVM.TransferHd_Id = InventoryTransfer_HdTemp.TransferHd_Id;

            return InvTransVM;
        }

        //  use this method to  Create New InvTransfere and save it in DataBase.
        // after spacific data  another Tabel put this data in the model Invintory Transfer
        // and add data thih  table put table  Invintory Transfer
        /// <summary>
        ///  method to  Create New InvTransfere
        /// </summary>
        /// <returns>bool</returns>
        public static int CreateNewInvTransfere(InventoryTransfer_HdVM InvTransfereMaster , String UserId)
        {
            var db = new AccountingContainer();
            InventoryTransfer_Hd Master = new InventoryTransfer_Hd();
            if(InvTransfereMaster.SelectedDestStore_Id== InvTransfereMaster.SelectedSourceStore_Id)
            {
                return 0;
            }
            var transFaerMasterTem = db.InventoryTransfer_HdTemp.Find(InvTransfereMaster.TransferHd_Id);
            Master.TransferHd_Id = InvTransfereMaster.TransferHd_Id;


            var comp = db.Companies.Find(InvTransfereMaster.SelectedCompanyId);
            if (comp != null)
            {
                var date = Convert.ToDateTime(InvTransfereMaster.Date);
                if (date < comp.StartFinancialDate)
                {
                    Master.Date = DateTime.Now;
                }
                else
                {
                    Master.Date = Convert.ToDateTime(InvTransfereMaster.Date);
                }
            }
            else
            {
                Master.Date = DateTime.Now;
            }

           
            var lastTransfer = db.InventoryTransfer_Hd.OrderByDescending(c => c.TransferHd_Id).FirstOrDefault();
            if (lastTransfer == null)
            {
                Master.Inv_Code = "TRS-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                Master.Inv_Code = "TRS-" + (Convert.ToInt32(lastTransfer.Inv_Code.Substring(4, lastTransfer.Inv_Code.Length - 4)) + 1).ToString("D3");
            }

            Master.InvTransfere_Notes = InvTransfereMaster.InvTransfere_Notes;
            Master.Total_Cost = 0;// InvTransfereMaster.Total_Cost;

            Master.Tranfer_State = TransactionState.Pending;


            if (InvTransfereMaster != null)
            {
                // create selectList from Database to entity Companies.
                var resultCompany = db.Companies.Find(InvTransfereMaster.SelectedCompanyId);
                {
                    Master.Comop_ID = resultCompany.Comp_Id;
                }          

                // create selectList from Database to entity Stores.
                var resultSourStore = db.Stores.Find(InvTransfereMaster.SelectedSourceStore_Id);
                {
                    Master.SourceStore_Id = resultSourStore.Store_ID;
                }

                // create selectList from Database to entity Stores.
                var resultDeststore = db.Stores.Find(InvTransfereMaster.SelectedDestStore_Id);
                {
                    Master.DestStore_Id = resultDeststore.Store_ID;
                }

        

                // Add other table Details Inventory Transfer in the model Inventory Transfer 
                // More than once data Details Inventory Transfer in the model Inventory Transfer
                //used  ( foreach )
                db.ProductBatches.Load();
                if (transFaerMasterTem.InventoryTransfer_DetailTemp.Count()==0)
                {
                    return 0;
                }
                foreach (var item in transFaerMasterTem.InventoryTransfer_DetailTemp.ToList())
                {
                    var details = new InventoryTransfer_Detail();

                    details.TransferDetail_Id = item.TransferDetail_Id;
                    //details.InventoryTransfer_Hd = item.TransHd_Id;
                    details.Product_ID = item.Product_ID;
                    details.Product_Qty = item.Product_Qty;
                    details.InventoryTransfer_Id = InvTransfereMaster.TransferHd_Id;
                    details.Batch_ID = item.Batch_ID;
                    ProductBatches curBatch = db.ProductBatches.Find(item.Batch_ID);
                    details.Detail_TotalCoct = curBatch.Product_Cost * details.Product_Qty;
                    details.Product_ID = curBatch.Product_ID;
                    Master.Total_Cost += details.Detail_TotalCoct;
                    Master.InventoryTransfer_Detail.Add(details);

                    db.InventoryTransfer_DetailTemp.Remove(item);
                    db.SaveChanges();
                }


                // this is Batch number any transaction  Inventory Transfer 
                var traBatch = new TransactionBatch();

                traBatch.OperationName = "تحويل من مخزن الى مخزن";
                traBatch.Description = null;
                traBatch.Date = Convert.ToDateTime(InvTransfereMaster.Date);
                traBatch.UserId = UserId;
                traBatch.Operation_Code = Master.Inv_Code;
                db.InventoryTransfer_Hd.Add(Master);
                db.InventoryTransfer_HdTemp.Remove(transFaerMasterTem);
                db.SaveChanges();
                traBatch.OperationId = Master.TransferHd_Id;
                TransactionBatch.AddNewTransaction(traBatch);
                db.Dispose();
                return Master.TransferHd_Id;
            }
            return 0;
        }
        // use this method For Edite InvoiceTransfer and save it DataBase.
        //  Update Invoice Transfer on the data from this model Invoice Transfer
        // and Update Gl Header and Upadte GLDetail SoursStoreGL Cridet DestStoreGL Depite
        /// <summary>
        /// method For Edite InvoiceTransfer
        /// </summary>
        /// <param name="invoiceTransVM"></param>
        /// <returns>bool</returns>
        public static string UpdateInvoiceTransfer(InventoryTransfer_HdVM invoiceTransVM)
        {
           
            var msg = "";
            var db = new AccountingContainer();
            var InvoisTransfe = db.InventoryTransfer_Hd.Find(invoiceTransVM.TransferHd_Id);
            OperationRun.Addnewoperation(InvoisTransfe.Inv_Code);
            var operation = db.OperationRuns.Where(a => a.Code == InvoisTransfe.Inv_Code).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(InvoisTransfe.Inv_Code);
                return Resources.InvoiceMasterDetails.WaitingMSGAndTryAgain;
            }
            if (InvoisTransfe.Tranfer_State == invoiceTransVM.Tranfer_State)
            {
                OperationRun.DeleteOperation(InvoisTransfe.Inv_Code);
                return Resources.InvoiceMasterDetails.ItsDoneWorking + " " + InvoisTransfe.Tranfer_State + " " + Resources.InvoiceMasterDetails.Before;
            }
            else
            {
                InvoisTransfe.Tranfer_State = invoiceTransVM.Tranfer_State;
            }

            // Used this State Spacific State Gl start Pending 
            
            db.Entry(InvoisTransfe).State = EntityState.Modified;
            // InvoisTransfe.Tranfer_State = TransactionState.Pending;


            // Used this GLHeader Create GL
            GLMaster glHeader = new GLMaster();

            glHeader.GL_Date = InvoisTransfe.Date;
            glHeader.GLCode = InvoisTransfe.Inv_Code;
            if (InvoisTransfe.InvTransfere_Notes == null)
            {
                glHeader.GL_Desc ="";
            }
            else
            {
                glHeader.GL_Desc = InvoisTransfe.InvTransfere_Notes;
            }
            var detailList = getMaterialOCompoundProduct(InvoisTransfe.InventoryTransfer_Detail.ToList(), InvoisTransfe.SourceStore_Id);
            glHeader.Company_Id = InvoisTransfe.Comop_ID;
            glHeader.GLType_Id = "تحويل من مخزن الى مخزن";
            glHeader.Currency_Id = "";
            glHeader.GLState = TransactionState.Pending;
            glHeader.Auto = true;

            // this GL Details Start SoursStoreGL Secound DestStoreGL
            GLDetail SoursStoreGL = new GLDetail(),
              DestStoreGL = new GLDetail();

            // Enter the data Source store  
            SoursStoreGL.Bu_Id = 1;
            SoursStoreGL.Asset_Id = null;

            // Used ChartOfAccountRecive  Spacefic Account this CustomerAdvanced. 
         
            SoursStoreGL.GLMasterGLMaster_Id = glHeader.GLMaster_Id;


            DestStoreGL.Bu_Id = 1;
            // Used ChartOfAccountRecive  Spacefic Account this AccountReceivableGL . 
           

            DestStoreGL.GLMasterGLMaster_Id = glHeader.GLMaster_Id;


            var SourceStore = db.Stores.Find(InvoisTransfe.SourceStore_Id);
            var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == SourceStore.Store_Code).First();
            SoursStoreGL.Account_Id = ChartOfAccounInventory.Account_Id;
            SoursStoreGL.Account_Name = ChartOfAccounInventory.Account_Name;

            var DestStore = db.Stores.Find(InvoisTransfe.DestStore_Id);
            var DestorInventory = db.ChartOfAccountings.Where(account => account.Account_Code == DestStore.Store_Code).First();
            DestStoreGL.Account_Id = DestorInventory.Account_Id;
            DestStoreGL.Account_Name = DestorInventory.Account_Name;

            // This State GL Start Pending Secound Post thierd Voiod 
            // Update the state Gl frome depite to credite 
            // and  The opposite gl from cridet to depite 
            switch (InvoisTransfe.Tranfer_State)
            {
                //Post GL
                case TransactionState.Post:

                    // From Account SoursStoreGL to DestStoreGL .


                    // Update batch quantity

                    glHeader.GLState = TransactionState.Post;
                    var gl = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                    if (gl != null)
                    {
                        OperationRun.DeleteOperation(InvoisTransfe.Inv_Code);
                        return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                    }

                    db.ProductBatches.Load();
                    var errorMessage = new List<string> { };
                    foreach (var detail in detailList)
                    {

                        ProductBatches detailBatch = db.ProductBatches.Find(detail.Batch_ID);

                        if (detail.Product_Qty > detailBatch.Product_Quantity)
                        {
                            var productName = db.Products.Find(detail.Product_ID).Prod_Name;
                            InvoisTransfe.Tranfer_State = TransactionState.Pending;
                            db.SaveChanges();
                            errorMessage.Add(Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Thereisnotenoughproduct + productName);

                        }
                    }
                    if (errorMessage.Count() != 0)
                    {


                        string combindedString = string.Join("\n", errorMessage.ToArray());
                        OperationRun.DeleteOperation(InvoisTransfe.Inv_Code);
                        return combindedString;
                    }
                    // this ProductBatches Cost distribution
                    foreach (var detail in detailList)
                    {

                        ProductBatches detailBatch = db.ProductBatches.Find(detail.Batch_ID);

                      
                       
                        if (detailBatch.Product_Quantity >= detail.Product_Qty)
                        {
                            // Decrease the quantity and cost of the batch and create a new batch for the destination store
                            detailBatch.Product_Quantity -= detail.Product_Qty;
                            detailBatch.Batch_Cost -= detail.Detail_TotalCoct ;
                            if (detailBatch.Product_Quantity == 0)
                            {
                                detailBatch.Product_Cost = 0;
                                detailBatch.Batch_Cost = 0;

                            }
                            else
                            {
                                detailBatch.Product_Cost = detailBatch.Batch_Cost / detailBatch.Product_Quantity;
                            }
                            var batchAbdisalaam = new ProductBatches();

                            batchAbdisalaam.Product_ID = detailBatch.Product_ID;
                            batchAbdisalaam.Store_ID = InvoisTransfe.DestStore_Id;
                            batchAbdisalaam.Product_Quantity = detail.Product_Qty;
                            batchAbdisalaam.Product_Cost = detailBatch.Product_Cost;
                            batchAbdisalaam.Batch_Cost = detailBatch.Product_Cost * detailBatch.Product_Quantity;
                            batchAbdisalaam.Batch_Date = detailBatch.Batch_Date;
                            batchAbdisalaam.Batch_ExpireDate = detailBatch.Batch_ExpireDate;
                            batchAbdisalaam.PurInvoice_ID = batchAbdisalaam.PurInvoice_ID;
                            db.Entry(detailBatch).State = EntityState.Modified;
                            ProductBatches productBatch = new ProductBatches();

                            var allproductpatches = db.ProductBatches.Where(p => p.Product_ID == detailBatch.Product_ID && p.Store_ID == InvoisTransfe.DestStore_Id).FirstOrDefault();
                           
                           
                            if (allproductpatches != null)
                            {

                                allproductpatches.Product_Quantity += detail.Product_Qty;
                                //  allproductpatches.Product_Cost += detail.Detail_TotalCoct;
                                // adding  the  newbatchcost  on  the  newbacth cost
                                allproductpatches.Batch_Cost += detail.Detail_TotalCoct;
                                // this line  is  to  make  the  avarage  after  adding  the  new  batchcost  on  the  onld  one
                                allproductpatches.Product_Cost =allproductpatches.Batch_Cost / allproductpatches.Product_Quantity;
                                db.Entry(allproductpatches).State = EntityState.Modified;
                               
                            }
                            else
                            {

                                productBatch.Batch_Date = detailBatch.Batch_Date;
                                productBatch.Batch_ExpireDate = detailBatch.Batch_ExpireDate;
                                productBatch.Product_Cost = detail.Detail_TotalCoct/detail.Product_Qty;
                                productBatch.Product_Quantity = detail.Product_Qty;
                               
                                productBatch.Batch_Cost =detail.Detail_TotalCoct;
                                productBatch.Product_ID = detailBatch.Product_ID;
                                productBatch.PurInvoice_ID = detailBatch.PurInvoice_ID;
                                productBatch.Store_ID = InvoisTransfe.DestStore_Id;
                                //SoursStoreGL.GLDetail_Credit += detail.Detail_TotalCoct;
                                //DestStoreGL.GLDetail_Debit += detail.Detail_TotalCoct;
                                db.ProductBatches.Add(productBatch);
                               
                            }
                        }


                        var source = new StoreCard();
                        if (detail.Product_Qty == 0)
                        {
                            source.CostPrice = 0;
                        }
                        else
                        {
                            source.CostPrice = detail.Detail_TotalCoct / detail.Product_Qty;
                        }
                        
                        source.Date = DateTime.Now;
                        source.Store_Id = InvoisTransfe.SourceStore_Id;
                        source.Quantity = detail.Product_Qty;
                        source.SalePrice = 0;
                        source.Product_Id = detail.Product_ID;
                        source.IncreaseQuantity = false;
                        source.TypeOfDocument = "تحويل من مخزن لمخزن";
                        source.Document_Serial = InvoisTransfe.Inv_Code;
                        source.TotalCost = detail.Detail_TotalCoct;
                        source.TotalAmount = 0;
                        source.Account_Id = 0;
                        source.Subledger_Id = 0;
                        Store.AddinginstorCard(source);


                        var desStore = new StoreCard();
                        desStore.CostPrice = detail.Detail_TotalCoct / detail.Product_Qty;
                        desStore.Date = DateTime.Now;
                        desStore.Store_Id = InvoisTransfe.DestStore_Id;
                        desStore.Quantity = detail.Product_Qty;
                        desStore.SalePrice = 0;
                        desStore.Product_Id = detail.Product_ID;
                        desStore.IncreaseQuantity = true;
                        desStore.TypeOfDocument = "تحويل من مخزن لمخزن";
                        desStore.Document_Serial = InvoisTransfe.Inv_Code;
                        desStore.TotalCost = detail.Detail_TotalCoct;
                        desStore.TotalAmount = 0;
                        desStore.Account_Id = 0;
                        desStore.Subledger_Id = 0;
                        Store.AddinginstorCard(desStore);


                        DestStoreGL.GLDetail_Debit += detail.Detail_TotalCoct;
                        SoursStoreGL.GLDetail_Credit += detail.Detail_TotalCoct;
                    }
               
                    break;
                // Void Invoice
                case TransactionState.Void:

                    glHeader.GLState = TransactionState.Void;
                    var glcheck = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                    if (glcheck != null)
                    {
                        OperationRun.DeleteOperation(InvoisTransfe.Inv_Code);
                        return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;

                    }

                        if (InvoisTransfe.HasOff == true)

                    {
                        msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.UnderCloseing;
                        InvoisTransfe.Tranfer_State = TransactionState.Post;
                       ;
                        db.SaveChanges();
                        OperationRun.DeleteOperation(InvoisTransfe.Inv_Code);
                        return msg;
                    }

                    else
                    {
                        // From Account DestStoreGL to SoursStoreGL .
                  

                        // Update batch quantity
                        db.ProductBatches.Load();
                        var errorMessageo = new List<string> { };
                        foreach (var detail in detailList)
                        {
                            ProductBatches sourceBatch = db.ProductBatches.Find(detail.Batch_ID);
                            var Source = db.ProductBatches.Where(a => a.Store_ID == InvoisTransfe.SourceStore_Id && a.Product_ID == detail.Product_ID).FirstOrDefault();
                            var dest = db.ProductBatches.Where(a => a.Store_ID == InvoisTransfe.DestStore_Id && a.Product_ID == detail.Product_ID).FirstOrDefault();

                            if (detail.Product_Qty > dest.Product_Quantity)
                            {
                                var productName = db.Products.Find(detail.Product_ID).Prod_Name;
                                InvoisTransfe.Tranfer_State = TransactionState.Pending;
                                db.SaveChanges();
                                errorMessageo.Add(Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Thereisnotenoughproduct + productName);

                            }



                        }
                        if (errorMessageo.Count() != 0)
                        {


                            string combindedString = string.Join("\n", errorMessageo.ToArray());
                            OperationRun.DeleteOperation(InvoisTransfe.Inv_Code);
                            return combindedString;
                        }

                        foreach (var detail in detailList)
                        {
                            ProductBatches sourceBatch = db.ProductBatches.Find(detail.Batch_ID);

                            if (sourceBatch.Store_ID == InvoisTransfe.DestStore_Id)
                            {
                                // Change the store id of the batch to the source store
                                sourceBatch.Store_ID = InvoisTransfe.SourceStore_Id;
                                db.Entry(sourceBatch).State = EntityState.Modified;
                            }
                            else
                            {
                                
                                var Source =db.ProductBatches.Where(a=>a.Store_ID== InvoisTransfe.SourceStore_Id&&a.Product_ID==detail.Product_ID).FirstOrDefault();
                                var dest = db.ProductBatches.Where(a => a.Store_ID == InvoisTransfe.DestStore_Id && a.Product_ID == detail.Product_ID).FirstOrDefault();
                                Source.Product_Quantity = Source.Product_Quantity + detail.Product_Qty;
                                Source.Batch_Cost = Source.Batch_Cost + detail.Detail_TotalCoct;
                                Source.Product_Cost = Source.Batch_Cost / Source.Product_Quantity;
                                db.Entry(Source).State = EntityState.Modified;
                                
                                dest.Product_Quantity = dest.Product_Quantity - detail.Product_Qty;
                                if (dest.Product_Quantity == 0)
                                {
                                    dest.Batch_Cost = 0;
                                    dest.Product_Cost = 0;
                                }
                                else
                                {
                                    
                                    dest.Batch_Cost = dest.Batch_Cost- detail.Detail_TotalCoct;
                                    dest.Product_Cost = dest.Batch_Cost / dest.Product_Quantity;
                                }
                                db.Entry(dest).State = EntityState.Modified;

                                var source = new StoreCard();
                                source.CostPrice = detail.Detail_TotalCoct / detail.Product_Qty;
                                source.Date = DateTime.Now;
                                source.Store_Id = InvoisTransfe.SourceStore_Id;
                                source.Quantity = detail.Product_Qty;
                                source.SalePrice = 0;
                                source.Product_Id = detail.Product_ID;
                                source.IncreaseQuantity = true;
                                source.TypeOfDocument = "  استبعاد تحويل من مخزن لمخزن";
                                source.Document_Serial = InvoisTransfe.Inv_Code;
                                source.TotalCost = detail.Detail_TotalCoct;
                                source.TotalAmount = 0;
                                source.Account_Id = 0;
                                source.Subledger_Id = 0;
                                Store.AddinginstorCard(source);


                                var desStore = new StoreCard();
                                if (detail.Product_Qty==0)
                                {
                                    desStore.CostPrice = 0;
                                }
                                else
                                {
                                    desStore.CostPrice = detail.Detail_TotalCoct / detail.Product_Qty;
                                }
                                
                                desStore.Date = DateTime.Now;
                                desStore.Store_Id = InvoisTransfe.DestStore_Id;
                                desStore.Quantity = detail.Product_Qty;
                                desStore.SalePrice = 0;
                                desStore.Product_Id = detail.Product_ID;
                                desStore.IncreaseQuantity = false;
                                desStore.TypeOfDocument = " استبعاد تحويل من مخزن لمخزن";
                                desStore.Document_Serial = InvoisTransfe.Inv_Code;
                                desStore.TotalCost = detail.Detail_TotalCoct;
                                desStore.TotalAmount = 0;
                                desStore.Account_Id = 0;
                                desStore.Subledger_Id = 0;
                                Store.AddinginstorCard(desStore);
                                DestStoreGL.GLDetail_Credit += detail.Detail_TotalCoct;
                                SoursStoreGL.GLDetail_Debit += detail.Detail_TotalCoct;
                                db.SaveChanges();
                            }

                        }
                    }
                    break;
                default:
                    break;
            }

            if (msg == "")
            {
                glHeader.GLDetails.Add(DestStoreGL);
                glHeader.GLDetails.Add(SoursStoreGL);


                // add gl details in the gl header 
                // sorce store 
                // destnation store 
                db.GLMasters.Add(glHeader);

                GLMaster.GetBalanceForAccount(glHeader);
                GLMaster.GetBalanceForSubLedger(glHeader);
                db.SaveChanges();
                msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                db.Dispose();
            }
            OperationRun.DeleteOperation(InvoisTransfe.Inv_Code);
            if (InvoisTransfe.Tranfer_State == TransactionState.Post)
            {
                msg = Resources.validate.GLDone;
            }
            else
            {
                msg = Resources.validate.GLReverse;
            }
            return msg;

        }

        public static List<InventoryTransfer_Detail> getMaterialOCompoundProduct(List<InventoryTransfer_Detail> Details, int Store_Id)
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
                List<InventoryTransfer_Detail> Details2 = new List<InventoryTransfer_Detail>();

                foreach (var item in ProList)
                {
                    var SubProductList = ProductAll.Where(a => a.Prod_ID == item.Product_ID).FirstOrDefault().ProductCompounds;
                    foreach (var subProduct in SubProductList)
                    {
                        var dublicate = Details.Where(a => a.Product_ID == subProduct.Product_Id).FirstOrDefault();
                        var batch = db.ProductBatches.Where(a => a.Product_ID == subProduct.Product_Id && a.Store_ID == Store_Id).FirstOrDefault();
                        if (batch.Product.Prod_Type == Prod_Type.خدمة)
                        {
                            continue;
                        }
                        else if (dublicate == null)
                        {
                            var Detailnew = new InventoryTransfer_Detail();
                            Detailnew.Product_ID = subProduct.Product_Id;
                            Detailnew.Product_Qty = subProduct.Quantity * item.Product_Qty;
                            Detailnew.Batch_ID = batch.Batch_ID;
                            if (batch != null)
                            {
                                Detailnew.Batch_ID = batch.Batch_ID;
                            }
                            if (batch != null && batch.Product_Quantity > 0)
                            {
                                Detailnew.Detail_TotalCoct = (batch.Batch_Cost / batch.Product_Quantity) * Detailnew.Product_Qty;
                              
                            }
                            else
                            {
                                Detailnew.Detail_TotalCoct = 0;
                         

                            }
                            Details.Add(Detailnew);

                        }
                        else
                        {


                            var Detailnew = new InventoryTransfer_Detail();
                            Detailnew.Batch_ID = batch.Batch_ID;
                            Detailnew.Product_ID = subProduct.Product_Id;
                            Detailnew.Product_Qty = dublicate.Product_Qty + (item.Product_Qty * subProduct.Quantity);
                        
                            if (batch != null)
                            {
                                Detailnew.Batch_ID = batch.Batch_ID;
                            }
                            if (batch != null && batch.Product_Quantity > 0)
                            {
                                Detailnew.Detail_TotalCoct = (batch.Batch_Cost / batch.Product_Quantity) * Detailnew.Product_Qty;
                            
                            }
                            else
                            {
                                Detailnew.Detail_TotalCoct = 0;
                           

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


        // List of proudectthat contain in that store
        public static SelectList getStoreProducts(int storeId)
        {
            var db = new AccountingContainer();
            var store = db.Stores.Find(storeId);

            List<Product> MyProduct = new List<Product>();

            


            SelectList Products = new SelectList(MyProduct, "Prod_ID", "Prod_Name");
            return Products;
        }
        /// <summary>
        /// delete that transfer if is status equal Pending
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteInvTransfer(int id)
        {
            var db = new AccountingContainer();
            var transferMaster = db.InventoryTransfer_Hd.Find(id);

            if (transferMaster.Tranfer_State != TransactionState.Pending)
                return;
            db.InventoryTransfer_Detail.RemoveRange(transferMaster.InventoryTransfer_Detail);
            db.InventoryTransfer_Hd.Remove(transferMaster);

            db.SaveChanges();
            db.Dispose();

        }

        /// <summary>
        /// fill For Transfer and prepare for creation
        /// </summary>
        /// <param name="TransId"></param>
        /// <returns></returns>
        public static InventoryTransfer_HdVM fillTransfer(int TransId)
        {
            var db = new AccountingContainer();
            var transVM = new InventoryTransfer_HdVM();
            var Transfer = db.InventoryTransfer_Hd.Find(TransId);
            transVM.TransferHd_Id = Transfer.TransferHd_Id;
            var comp = db.Companies.Where(c => c.Comp_Id == Transfer.Comop_ID);
            if (comp.Any())
            {
                transVM.SelectedCompanyId = comp.First().Comp_Id;
            }

            var BU = db.BussinesUnits.Where(b => b.BU_Id == Transfer.Business_UnitId);
            if (BU.Any())
            {
                transVM.SelectedBusiness_UnitId = BU.First().BU_Id;
            }

            var stor = db.Stores.Where(Cu => Cu.Store_ID == Transfer.SourceStore_Id);
            if (stor.Any())
            {
                transVM.SelectedSourceStore_Id = stor.First().Store_ID;
            }

            var stors = db.Stores.Where(st => st.Store_ID == Transfer.DestStore_Id);
            if (stors.Any())
            {
                transVM.SelectedDestStore_Id = stors.First().Store_ID;
            }

            var currency = db.Currencies.Where(Cur => Cur.Currency_Type == Transfer.Inv_Currency);
            if (currency.Any())
            {
               transVM.SelectedCurrencyId = currency.First().Currency_Id;
            }


            transVM.TransferHd_Id = Transfer.TransferHd_Id;
            
            transVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id ", "Comp_Name");
            transVM.MyBusiness_UnitIdList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            transVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            transVM.MySoursStoreList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID ", "Store_Name");
            transVM.MyDestStoreList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            transVM.MyProductsList = new SelectList(db.Products, "Prod_ID", "Prod_Name");

           
        
            transVM.InvTransfere_Notes = Transfer.InvTransfere_Notes;
            transVM.Date = Convert.ToString(Transfer.Date);
            transVM.Total_Cost = Transfer.Total_Cost.Value;
            transVM.Tranfer_State = Transfer.Tranfer_State;
            var invTemp = new InventoryTransfer_HdTemp();
           
            invTemp.Total_Cost = transVM.Total_Cost;
            var ThereORN = db.InventoryTransfer_HdTemp.Find(Transfer.TransferHd_Id);
            if (ThereORN != null)
            {

            }
            else
            {
                invTemp.TransferHd_Id = transVM.TransferHd_Id;

                db.InventoryTransfer_HdTemp.Add(invTemp);
            }

            db.SaveChanges();

            return transVM;
        }

        //for create details temp and check the validate of every one created
        public static string AddTransDetailsTemp(InventorytransferHdDetailsVM Detail)
        {
            var db = new AccountingContainer();
            if (Detail.Batch_ID == null || Detail.Batch_ID == 0)
            {
                return Resources.validate.ChooseProduct;
            }

            if (Detail.Product_Qty == null || Detail.Product_Qty <= 0)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Addquantity;
            }
            var transMasterTemp = db.InventoryTransfer_HdTemp.Find(Detail.InventoryTransfer_Id);
            var inventoDetailsTemp = new InventoryTransfer_DetailTemp();
            var batch = db.ProductBatches.Find(Detail.Batch_ID);
            inventoDetailsTemp.Batch_ID = batch.Batch_ID;
            inventoDetailsTemp.Product_ID = batch.Product_ID;
          
            var details = transMasterTemp.InventoryTransfer_DetailTemp;
            var duplecatTemp = details.Where(p => p.InventoryTransfer_Temp_Id == transMasterTemp.TransferHd_Id && p.Product_ID == inventoDetailsTemp.Product_ID).ToList();
            if (duplecatTemp.FirstOrDefault() != null)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.thesameProduct;
            }
            var batchQuantity = db.ProductBatches.Find(Detail.Batch_ID).Product_Quantity;
            //if (batchQuantity< Detail.Product_Qty)
            //{
            //    return Resources.validate.TheQuantityIsbiggerfromStor;
            //}
            //else
            //{
              
              
                inventoDetailsTemp.Product_Qty = Detail.Product_Qty;
                inventoDetailsTemp.Detail_TotalCoct = batch.Product_Cost * Detail.Product_Qty;
                inventoDetailsTemp.InventoryTransfer_Temp_Id = Detail.InventoryTransfer_Id;
                transMasterTemp.InventoryTransfer_DetailTemp.Add(inventoDetailsTemp);
                db.Entry(transMasterTemp).State = EntityState.Modified;
                db.SaveChanges();
                return Resources.validate.SuccessfullAdd;
            //}
          
        }
        /// <summary>
        /// that used to delete that emp by its id
        /// </summary>
        /// <param name="DetailID"></param>
        /// <returns></returns>
        public static string DeleteTemp(int DetailID)
        {
            var db = new AccountingContainer();
            var detailsTemp = db.InventoryTransfer_DetailTemp.Find(DetailID);
            db.InventoryTransfer_DetailTemp.Remove(detailsTemp);
            db.SaveChanges();
            db.Dispose();
            return Resources.validate.DeleteSuccessful;
        }

        /// <summary>
        /// Prepare detail For Creaton and make quantity in vm ==o if product not contain in store
        /// </summary>
        /// <param name="Stor"></param>
        /// <param name="tempInventory"></param>
        /// <returns></returns>
        public static InventoryTransfer_DetailVM PreparerTransfer(int Stor, int tempInventory)
        {

            var db = new AccountingContainer();
            var InventoryVM = new InventoryTransfer_DetailVM();

            var thisdetails = GetAllTempInventoryeDetails(Stor,0).ToList().FirstOrDefault();

            InventoryVM.Product_Qty = thisdetails.Product_Qty;
            if (thisdetails.Product_Qty > 0)
            {
                InventoryVM.Product_Qty = (int)thisdetails.Product_Qty;
                InventoryVM.TransHd_Id = 0;
            }
            else
            {
                InventoryVM.TransHd_Id = thisdetails.InventoryTransfer_Temp_Id;
                InventoryVM.Product_ID = 0;
            }
            InventoryVM.TransferDetail_Id = thisdetails.TransferDetail_Id;
            return InventoryVM;
        }
       

        /// <summary>
        /// for get details for edit
        /// </summary>
        /// <param name="Transfer"></param>
        /// <returns></returns>
        public static InventorytransferHdDetailsVM PrepareInventoryeH_dDetail2(int Transfer)
        {
            var db = new AccountingContainer();
            var InventoryH_dDetailsVM = new InventorytransferHdDetailsVM();
            
            var thisdetails = GetAllTempInventoryeDetailresult(Transfer).ToList().FirstOrDefault();
            var invtmaster = db.InventoryTransfer_Hd.Find(thisdetails.InventoryTransfer_Id);
            InventoryH_dDetailsVM.Product_Qty = thisdetails.Product_Qty;
            InventoryH_dDetailsVM.Detail_TotalCoct = thisdetails.Detail_TotalCoct;
            InventoryH_dDetailsVM.InventoryTransfer_Id = thisdetails.InventoryTransfer_Id;
            InventoryH_dDetailsVM.Product_ID = thisdetails.Product_ID;
            InventoryH_dDetailsVM.Batch_ID = thisdetails.Batch_ID;
            InventoryH_dDetailsVM.TransferDetail_Id = thisdetails.TransferDetail_Id;
            var pro = db.Products.Where(c => c.Prod_ID == thisdetails.Product_ID);
            if (pro.Any())
            {
               // InventoryH_dDetailsVM. = pro.FirstOrDefault().Prod_ID;
            }
            InventoryH_dDetailsVM.MyProductsList = new SelectList(db.Products, "Product_Id", "ProductName");
           
            var branch = db.ProductBatches.Where(c => c.Batch_ID == thisdetails.Batch_ID&&c.Store_ID== invtmaster.SourceStore_Id);
            if (branch.Any())
            {
              //  InventoryH_dDetailsVM.SelectedBtachtId = (int)thisdetails.Batch_ID;
            }

            InventoryH_dDetailsVM.MyBatchsList = new SelectList(db.ProductBatches, "Batch_Id", "ProductName");
            return InventoryH_dDetailsVM;

        }


        /// <summary>
        /// for prepar details 
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns></returns>
        public static InventorytransferHdDetailsVM PreparTransferDetail2(int transfId)
        {

            var db = new AccountingContainer();
            var transferDVM = new InventorytransferHdDetailsVM();

            var thisdetails = GetAllTempTransferDetailsDetails(transfId).ToList().FirstOrDefault();

            var product = thisdetails.Product_ID;


            transferDVM.Product_Qty = thisdetails.Product_Qty;
           
            //transferDVM.TransferDetail_Id = thisdetails.TransferDetail_Id;    
            transferDVM.InventoryTransfer_Id = thisdetails.InventoryTransfer_Id;
            var batchs = db.ProductBatches.Where(batch => batch.Product_ID == thisdetails.Product_ID).ToList();

            List<ProductBatchSelection> _ProductBatchSelection = new List<ProductBatchSelection>();
            foreach (var item in batchs)
            {
                var productBatchSelection = new ProductBatchSelection();

                productBatchSelection.Batch_Id = item.Batch_ID;
                productBatchSelection.ProductCost = item.Product_Cost;
                productBatchSelection.ProductName = item.Product.Prod_Name;
                productBatchSelection.Product_Id = item.Product_ID;
                _ProductBatchSelection.Add(productBatchSelection);

            }

            transferDVM.MyBatchsList = new SelectList(_ProductBatchSelection, "Batch_Id", "ProductName");   
            

            var prod = db.Products.Where(c => c.Prod_ID == product);
            if (prod.Any())
            {
                transferDVM.Batch_ID = prod.FirstOrDefault().Prod_ID;
            }
            return transferDVM;


        }


        /// <summary>
        /// get all Detail for that inventory Id
        /// </summary>
        /// <param name="transfVmId"></param>
        /// <returns></returns>
        public static List<InventoryTransfer_Detail> GetAllTempTransferDetailsDetails(int transfVmId)
        {
            var db = new AccountingContainer();
            var DetailsTemp = db.InventoryTransfer_Detail.Where(trans => trans.TransferDetail_Id == transfVmId).ToList();
            return DetailsTemp;
        }
        /// <summary>
        /// get all Detail for that inventory Id
        /// </summary>
        /// <param name="transfVmId"></param>
        /// <returns></returns>
        public static List<InventoryTransfer_Detail> GetAllTempTransferDetails2(int transfVmId)
        {
            var db = new AccountingContainer();
            var DetailsTemp = db.InventoryTransfer_Detail.Where(transf => transf.InventoryTransfer_Id == transfVmId).ToList();
            return DetailsTemp;
        }
        /// <summary>
        /// get all tranfet detail temp bt using master _id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns></returns>
        public static List<InventoryTransfer_DetailTemp> GetAllTempTransfDetailsresult2(int masterVmId)
        {
            var db = new AccountingContainer();
            var TransferTemp = db.InventoryTransfer_HdTemp.Find(masterVmId);
            return TransferTemp.InventoryTransfer_DetailTemp.ToList();
        }

        //public static string AddInventoryDetailsTemp2(InventorytransferHdDetailsVM InventoryH_dDetail)
        //{
        //    var db = new AccountingContainer();
        //    var InventoryH_dTemp = db.InventoryTransfer_HdTemp.Find(InventoryH_dDetail.InventoryTransfer_Id);
        //    var InventoryH_dDetailsTemp = new InventoryTransfer_DetailTemp();

        //    var batch = db.ProductBatches.Where(p => p.Product_ID == InventoryH_dDetail.Batch_ID).FirstOrDefault();
        //    // var batch = db.ProductBatches.Find(InventoryH_dDetail.SelectedProductId);

        //    // InventoryH_dDetail.Product_Qty;
        //    InventoryH_dDetailsTemp.Batch_ID = batch.Batch_ID;
        //    InventoryH_dDetailsTemp.Product_ID = batch.Product_ID;
        //    InventoryH_dDetailsTemp.Product_Qty = InventoryH_dDetail.Product_Qty;
        //    InventoryH_dDetailsTemp.Detail_TotalCoct = batch.Product_Cost * InventoryH_dDetail.Product_Qty;
        //    InventoryH_dTemp.InventoryTransfer_DetailTemp.Add(InventoryH_dDetailsTemp);
        //    db.Entry(InventoryH_dTemp).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return "Details Has been added";
        //}


            /// <summary>
            /// file the master to make Editing 
            /// </summary>
            /// <param name="transfId"></param>
            /// <returns></returns>
        public static InventoryTransfer_HdVM fillForEditingTransfer(int transfId)
        {
            var db = new AccountingContainer();
            var transferMaster = db.InventoryTransfer_Hd.Find(transfId);
            var TransfVM = new InventoryTransfer_HdVM();
         
            var comp = db.Companies.Where(c => c.Comp_Id == transferMaster.Comop_ID);
            if (comp.Any())
            {
                TransfVM.SelectedCompanyId = comp.FirstOrDefault().Comp_Id;
            }

            var SourceStore = db.Stores.Where(Cu => Cu.Store_ID == transferMaster.SourceStore_Id);
            if (SourceStore.Any())
            {
                TransfVM.SelectedSourceStore_Id = SourceStore.FirstOrDefault().Store_ID;
            }


            var DestinationStore = db.Stores.Where(Cu => Cu.Store_ID == transferMaster.DestStore_Id);
            if (DestinationStore.Any())
            {
                TransfVM.SelectedDestStore_Id = DestinationStore.FirstOrDefault().Store_ID;
            }

            TransfVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
        
            TransfVM.MySoursStoreList = new SelectList(db.Stores, "Store_ID", "Store_Name");
            TransfVM.MyDestStoreList = new SelectList(db.Stores, "Store_ID", "Store_Name");


            TransfVM.Inv_Code = transferMaster.Inv_Code;
            TransfVM.InvTransfere_Notes = transferMaster.InvTransfere_Notes;
            TransfVM.Date = transferMaster.Date.ToString("dd-MM-yyyy   HH:mm:ss") ;
            TransfVM.Tranfer_State = TransactionState.Pending;
            TransfVM.TransferHd_Id = transferMaster.TransferHd_Id;
           

  

            //   InvoiceVM.myDetailsVM = invoiceMast.InvoiceDetails.ToList();
            var invoiceTemp = new InventoryTransfer_HdTemp();
            invoiceTemp.Comop_ID = TransfVM.SelectedCompanyId;
     
  
            invoiceTemp.Date = transferMaster.Date;        
          
            invoiceTemp.Tranfer_State = transferMaster.Tranfer_State;

            invoiceTemp.DestStore_Id = TransfVM.SelectedDestStore_Id;
         
            invoiceTemp.SourceStore_Id = TransfVM.SelectedSourceStore_Id;
          
            invoiceTemp.Total_Cost = transferMaster.Total_Cost;
           
            var ThereORN = db.InventoryTransfer_HdTemp.Find(transferMaster.TransferHd_Id);
            if (ThereORN != null)
            {

            }
            else
            {
                invoiceTemp.TransferHd_Id = transferMaster.TransferHd_Id;

                db.InventoryTransfer_HdTemp.Add(invoiceTemp);
            }

      
            db.SaveChanges();
            return TransfVM;
        }

        /// <summary>
        /// save changes when posting Editing in inventory Transfer
        /// </summary>
        /// <param name="transfVM"></param>
        /// <returns></returns>
        public static int EditTransfer(InventoryTransfer_HdVM transfVM)
        {
            var db = new AccountingContainer();
            decimal total = 0;
            var transf = db.InventoryTransfer_Hd.Find(transfVM.TransferHd_Id);
            var mastertemp = db.InventoryTransfer_HdTemp.Find(transfVM.TransferHd_Id);
            transf.Comop_ID = (int)transfVM.SelectedCompanyId;
           
            transf.InvTransfere_Notes = transfVM.InvTransfere_Notes;
            transf.Date = Convert.ToDateTime(transfVM.Date);

            var resultSourStore = db.Stores.Find(transfVM.SelectedSourceStore_Id);
            {
                transf.SourceStore_Id = resultSourStore.Store_ID;
            }

            // create selectList from Database to entity Stores.
            var resultDeststore = db.Stores.Find(transfVM.SelectedDestStore_Id);
            {
                transf.DestStore_Id = resultDeststore.Store_ID;
            }

            transf.Tranfer_State = TransactionState.Pending;
            var oldDetail =db.InventoryTransfer_Detail.Where(a => a.InventoryTransfer_Id == transf.TransferHd_Id).ToList();

            var details = db.InventoryTransfer_DetailTemp.Where(e => e.InventoryTransfer_Temp_Id == mastertemp.TransferHd_Id).ToList();
            List<InventoryTransfer_DetailTemp> Deletetemps = new List<InventoryTransfer_DetailTemp>();
            if (details.Count() == 0)
            {
                return 0;
            }
            foreach (var item in details)
            {
                
                var detail = new InventoryTransfer_Detail();

                detail.TransferDetail_Id = item.TransferDetail_Id;
                
                detail.Product_ID = item.Product_ID;
                detail.Product_Qty = item.Product_Qty;
                detail.InventoryTransfer_Id = transf.TransferHd_Id;
                detail.Batch_ID = item.Batch_ID;
                ProductBatches curBatch = db.ProductBatches.Find(detail.Batch_ID);
                detail.Detail_TotalCoct = curBatch.Product_Cost * detail.Product_Qty;
                detail.Product_ID = curBatch.Product_ID;
                total += detail.Detail_TotalCoct;
                transf.InventoryTransfer_Detail.Add(detail);

                db.InventoryTransfer_DetailTemp.Remove(item);
                db.SaveChanges();
            }
            transf.Total_Cost = total;
            db.InventoryTransfer_DetailTemp.RemoveRange(Deletetemps);
            db.Entry(transf).State = EntityState.Modified;
            db.SaveChanges();
            db.InventoryTransfer_HdTemp.Remove(mastertemp);
            db.InventoryTransfer_Detail.RemoveRange(oldDetail);

            try { db.SaveChanges(); }
            catch (Exception e)
            {
               var msg= e.Message;
            }
            db.Dispose();

            return transf.TransferHd_Id;
        }
        /// <summary>
        /// prepare put all data in detail of inventory to detai Temp to make Edit on it
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns></returns>
        public static List<InventoryTransfer_DetailTemp> GetAllTempTransfDetailsresultForEdit(int masterVmId)
        {
            var db = new AccountingContainer();
            var detailTemp = db.InventoryTransfer_HdTemp.Find(masterVmId);
            db.InventoryTransfer_DetailTemp.RemoveRange(detailTemp.InventoryTransfer_DetailTemp);
            db.SaveChanges();
            var invoice = db.InventoryTransfer_Hd.Find(masterVmId);
            List<InventoryTransfer_DetailTemp> Lista = new List<InventoryTransfer_DetailTemp> { };
            foreach (var item in invoice.InventoryTransfer_Detail)
            {
                var invoiceTemp = new InventoryTransfer_DetailTemp();
                invoiceTemp.Batch_ID = item.Batch_ID.Value;
                invoiceTemp.Product_ID = item.Product_ID;
                invoiceTemp.Product_Qty = item.Product_Qty;
                invoiceTemp.Detail_TotalCoct = item.Detail_TotalCoct;
                invoiceTemp.InventoryTransfer_Temp_Id = masterVmId;
                
                Lista.Add(invoiceTemp);

            }
            db.InventoryTransfer_DetailTemp.AddRange(Lista);

            db.SaveChanges();

            return detailTemp.InventoryTransfer_DetailTemp.ToList();
        }


        public static IEnumerable<SelectListItem> getAllExceptStore(int storeId)
        {
            var db = new AccountingContainer();
            var store = db.Stores.Where(p => p.Store_ID != storeId).ToList();
            IEnumerable<SelectListItem> Bussines = new SelectList(store, "Store_ID", "Store_Name");
            
            return Bussines;
        }
    }
}
