using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Accountingsystem.ViewModel;
using System.Data.Entity;


namespace Accountingsystem.Models
{
    public partial class TransferToMaintenance
    {
        public static bool ContainCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.TransferToMaintenances.ToList();
            if (!Serial.StartsWith("TMain-"))
            {
                return true;
            }
            else
            {

                foreach (var item in allinData)
                {
                    if (item.Trans_Code == Serial || !Serial.StartsWith("TMain-"))
                    {
                        return true;
                    }
                    else
                    {

                    }

                }
            }
            return false;
        }
        public static string delete_Temp_Record(int id)
        {
            var db = new AccountingContainer();
            var delete = db.TransferToMaintenanceDetailsTemps.Find(id);
            db.TransferToMaintenanceDetailsTemps.Remove(delete);
            db.SaveChanges();
            db.Dispose();

            return "تم حذف المنتج بنجاح";



        }

        public static List<TransferToMaintenance> GetAllTransferToMaintenance()
        {
            var db = new AccountingContainer();
            var trans = db.TransferToMaintenances.ToList();
            return trans;
        }
        public static TransferToMaintenance GetAllTransferToMaintenanceById(int id)
        {
            var db = new AccountingContainer();
            var transfer = db.TransferToMaintenances.Find(id);
            return transfer;
        }
        public static int GetAvailbleProductQty(int batchId)
        {
            var db = new AccountingContainer();
            var batch = db.ProductBatches.Find(batchId);
            return Convert.ToInt32(batch.Product_Quantity);

        }
        public static TransferToMaintenanceDetailsVM PreperInvoiceMasterDetail(string storIds)
        {
            var db = new AccountingContainer();
            var TransferVM = new TransferToMaintenanceDetailsVM();
            int storId = Convert.ToInt32(storIds);
            var newListproout = from prod in db.ProductBatches.Where(b => b.Store_ID == storId&&b.Product_Quantity>0 && b.Product.IsActive != false)
                                select new
                                {
                                    Id = prod.Batch_ID,
                                    Name = prod.Product.Prod_Name + "   " + prod.Product.Bar_Code


                                };
            //List<ProductBatchSelection> _prodectPatch = new List<ProductBatchSelection>();
            //foreach (var item in batch)
            //{
            //    var ProductBatchSelectio = new ProductBatchSelection();
            //    ProductBatchSelectio.Batch_Id = item.Batch_ID;
            //    ProductBatchSelectio.ProductCost = item.Product_Cost;
            //    ProductBatchSelectio.ProductName = item.Product.Prod_Name;
            //    ProductBatchSelectio.Product_Code = item.Product.Bar_Code;
            //    ProductBatchSelectio.Product_Id = item.Product_ID;
            //    _prodectPatch.Add(ProductBatchSelectio);
            //}
            //List<object> newListproout = new List<object>();

            //foreach (var member in _prodectPatch)
            //    newListproout.Add(new
            //    {
            //        Id = member.Batch_Id,
            //        Name = member.ProductName + "   " + member.Product_Code
            //    });


            TransferVM.BatchList = new SelectList(newListproout, "Id", "Name");
       
            TransferVM.ProducttaxList = new SelectList(db.Taxes, "Tax_ID", "Tax_Name");
            return TransferVM;
        }
        public static TransferToMaintenanceVM CreateForCreation()
        {
            var db = new AccountingContainer();
            var masterVM = new TransferToMaintenanceVM();
            String StartCodeOfExpense = "05";

            var lastCode = db.TransferToMaintenances.OrderByDescending(c => c.Master_Id).FirstOrDefault();
            if (lastCode == null)
            {
                masterVM.Trans_Code = "TMain-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                masterVM.Trans_Code = "TMain-" + (Convert.ToInt32(lastCode.Trans_Code.Substring(6, lastCode.Trans_Code.Length - 6)) + 1).ToString("D3");
            }
           
            masterVM.Trans_Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
            masterVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            masterVM.MyStoresList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");



           
            

            var lista = db.ChartOfAccountings.Where(a=>a.ChartOfAccountings.Count()==0&&a.isActive!=false).ToList();

            //foreach (var s in db.ChartOfAccountings)
            //{
            //    {
            //        var Parent = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == s.Account_Id);
            //        var o = Parent.FirstOrDefault();
            //        if( o ==null)
            //        {
            //            lista.Add(db.ChartOfAccountings.Find(s.Account_Id));
            //        }
            //    }
            //}
            masterVM.AccountList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfExpense)), "Account_Id", "Account_Name");


            masterVM.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "فرع لقسم رئيسي")), "SubLed_Id", "SubLed_Name");
            masterVM.MyDepartList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            masterVM.BUList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            masterVM.supList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفه يدوي" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            masterVM.Invoice_State = TransactionState.Pending;


            var transtemp = new TransferToMaintenanceTemp();
           
            var id = (from s in db.TransferToMaintenanceTemps
                      select s).ToList().LastOrDefault();
            if (id == null)
            { transtemp.Master_Id = 1; }
            else
            { transtemp.Master_Id = id.Master_Id + 1; }
           
            db.TransferToMaintenanceTemps.Add(transtemp);
            db.SaveChanges();
            masterVM.Master_Id = transtemp.Master_Id;
            return masterVM;
        }
        public static string AddInvoiceDetailTemp(TransferToMaintenanceDetailsVM invoiceMasterDetailVM)
        {
            var db = new AccountingContainer();
            if (invoiceMasterDetailVM.Batch_ID == null || invoiceMasterDetailVM.Batch_ID == 0)
            {
                return Resources.validate.ChooseProduct;
            }
       
            if (invoiceMasterDetailVM.Product_Quantity == null || invoiceMasterDetailVM.Product_Quantity == 0)
            {
                return Resources.validate.Quantityshoudbiggerthan;
            }
            var invoiceMasterTemp = db.TransferToMaintenanceTemps.Find(invoiceMasterDetailVM.TransferToMaintenanceMaster_Id);

            var details = invoiceMasterTemp.TransferToMaintenanceDetailsTemps;
            var duplecatTemp = details.Where(p => p.TransferToMaintenanceTempMaster_Id == invoiceMasterTemp.Master_Id && p.Batch_ID == invoiceMasterDetailVM.Batch_ID).ToList();
            if (duplecatTemp.FirstOrDefault() != null)
            {
                   return Resources.validate.productSame; ;
            }
            var batchQuantity = db.ProductBatches.Find(invoiceMasterDetailVM.Batch_ID);
            if (batchQuantity.Product_Quantity < invoiceMasterDetailVM.Product_Quantity&& batchQuantity.Product.Prod_Type==Prod_Type.منتج)
            {
                return Resources.validate.TheQuantityIsbiggerfromStor;
            }


            if (invoiceMasterDetailVM.TransferToMaintenanceMaster_Id == 0)
            {
                var k = db.TransferToMaintenanceDetails.Find(invoiceMasterDetailVM.Details_Id).TransferToMaintenanceMaster_Id;
                invoiceMasterTemp = db.TransferToMaintenanceTemps.Find(k);
            }
            else
            {
                invoiceMasterTemp = db.TransferToMaintenanceTemps.Find(invoiceMasterDetailVM.TransferToMaintenanceMaster_Id);
            }

            var invoiceMasterDetailsTemp = new TransferToMaintenanceDetailsTemp();
            var batch = db.ProductBatches.Find(invoiceMasterDetailVM.Batch_ID);
            invoiceMasterDetailsTemp.Batch_ID = batch.Batch_ID;
            invoiceMasterDetailsTemp.Product_Id = batch.Product.Prod_ID;
            invoiceMasterDetailsTemp.Product_Price = batch.Product_Cost;
            invoiceMasterDetailsTemp.Product_Quantity = invoiceMasterDetailVM.Product_Quantity;

            //var resultTrea = db.Taxes.Find(invoiceMasterDetailVM.Product_Tax);
            //if(resultTrea == null)
            //{
            //    invoiceMasterDetailsTemp.Product_Tax = resultTrea.Tax_ID;
            //}


            //var a = db.Taxes.Find(invoiceMasterDetailsTemp.Product_Tax);

            invoiceMasterDetailsTemp.Detail_Total_Cost = batch.Product_Cost * invoiceMasterDetailVM.Product_Quantity;
            invoiceMasterTemp.TransferToMaintenanceDetailsTemps.Add(invoiceMasterDetailsTemp);

            db.Entry(invoiceMasterTemp).State = EntityState.Modified;
            db.SaveChanges();
            return Resources.validate.SuccessfullAdd;
        }
        public static List<TransferToMaintenanceDetailsTemp> GetAllTransferDetails(int MasterVmId)
        {
            var db = new AccountingContainer();
            var details = db.TransferToMaintenanceDetailsTemps.Where(a => a.TransferToMaintenanceTempMaster_Id == MasterVmId).ToList();
            return details;
        }
        public static Tuple< String,int> CreateNewTransfer(TransferToMaintenanceVM transVm , String UserId)
        {
            var db = new AccountingContainer();
            var transfer = new TransferToMaintenance();
            var transtemp = db.TransferToMaintenanceTemps.Find(transVm.Master_Id);
            if (transVm != null)
            {
                var lastCode = db.TransferToMaintenances.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                if (lastCode == null)
                {
                    transfer.Trans_Code = "TMain-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    transfer.Trans_Code = "TMain-" + (Convert.ToInt32(lastCode.Trans_Code.Substring(6, lastCode.Trans_Code.Length - 6)) + 1).ToString("D3");
                }
                transfer.Describtion = transVm.Describtion;
                transfer.Reference = transVm.Reference;
                var comp = db.Companies.Find(transVm.Company_Id);
                if (comp != null)
                {
                    var date = Convert.ToDateTime(transVm.Trans_Date);
                    if (date < comp.StartFinancialDate)
                    {
                        transfer.Trans_Date = DateTime.Now;
                    }
                    else
                    {
                        transfer.Trans_Date = Convert.ToDateTime(transVm.Trans_Date);
                    }
                }
                else
                {
                    transfer.Trans_Date = DateTime.Now;
                }
               
                //transfer.Trans_Code = transVm.Trans_Code;
                transfer.Invoice_State = TransactionState.Pending;
                //transfer.Company_Id = transVm.Company_Id;
                //transfer.Branch_Id = transVm.Branch_Id;
               if( transVm.Store_ID==0|| transVm.Store_ID == null)
                {
                    transVm.Store_ID=db.Stores.FirstOrDefault().Store_ID;
                }
                transfer.Store_ID = transVm.Store_ID;
                //transfer.Sup_ID =(Int32)transVm.Subledger_ID;


                var com = db.Companies.Find(transVm.Company_Id);
                if(com != null)
                {
                    transfer.Company_Id = com.Comp_Id;
                }

                var Chart = db.ChartOfAccountings.Find(transVm.Account_Id);
                if (Chart != null)
                {
                        transfer.Account_Id = Chart.Account_Id;
                }

                var subResult = db.SubLedgers.Find(transVm.Subledger_ID);
                if (subResult != null)
                {
                    transfer.Subledger_ID = subResult.SubLed_Id;
                }
                else
                {
                    if (transVm.Transfer_BU != null && transVm.Transfer_BU != 0)
                    {
                        transfer.Subledger_ID = (int)transVm.Transfer_BU;
                        transfer.Transfer_BU = transVm.Transfer_BU;
                    }
                    else if ((transVm.Transfer_BU == null || transVm.Transfer_BU == 0) && (transVm.Department_ID != null && transVm.Department_ID != 0))
                    {
                        transfer.Subledger_ID = (int)transVm.Department_ID;
                        transfer.Department_ID = transVm.Department_ID;
                    }
                    else if ((transVm.Transfer_BU == null || transVm.Transfer_BU == 0) && (transVm.Department_ID == null || transVm.Department_ID == 0) && (transVm.Branch_Id != null && transVm.Branch_Id != 0))
                    {
                        transfer.Subledger_ID = (int)transVm.Branch_Id;
                        transfer.Branch_Id = transVm.Branch_Id;
                    }
                    else
                    {
                        var thissub = db.SubLedgers.Where(e => e.Subled_Level == 0).FirstOrDefault();
                        transfer.Subledger_ID = (int)thissub.SubLed_Id;
                    }
                }
                //if (transVm.Subledger_ID != null)
                //{
                //    transfer.Subledger_ID = transVm.Subledger_ID;
                //}
                //else if (transVm.Transfer_BU != null)
                //{
                //    transfer.Subledger_ID = transVm.Transfer_BU;
                //    transfer.Transfer_BU = transVm.Transfer_BU;
                //}
                //else if(transVm.Department_ID != null)
                //{
                //    transfer.Subledger_ID = transVm.Department_ID;
                //    transfer.Department_ID = transVm.Department_ID;
                //}
                //else if(transVm.Branch_Id != null)
                //{
                //    transfer.Subledger_ID = transVm.Branch_Id;
                //    transfer.Branch_Id = transVm.Branch_Id;
                //}
                //else
                //{
                //    transVm.Subledger_ID = 1;
                //}
                if (transtemp.TransferToMaintenanceDetailsTemps.ToList().Count() == 0)
                {
                    return Tuple.Create("لا يوجد تفاصيل للتحويل", 0);
                }
                foreach (var sasa in transtemp.TransferToMaintenanceDetailsTemps.ToList())
                {
                    var details = new TransferToMaintenanceDetails();
                    details.Details_Id = sasa.Details_Id;
                    details.Product_Quantity = sasa.Product_Quantity;
                    //details.Product_Tax = sasa.Product_Tax;
                    //details.Detail_Total_Tax = sasa.Detail_Total_Tax;
                    //details.Total_Amount += sasa.Detail_Total_Tax;
                    
                    
                    ProductBatches pro = db.ProductBatches.Find(sasa.Batch_ID);
                    details.Batch_ID = sasa.Batch_ID;
                    details.Product_Id = pro.Product_ID;
                    //if (sasa.Product_Quantity > pro.Product_Quantity)
                    //    return Tuple.Create( "Not enough quantity of " + pro.Product.Prod_Name,0);
                    details.Detail_Total_Cost = pro.Product_Cost * details.Product_Quantity;
                    details.Product_Price = pro.Product_Cost;

                    transfer.Total_Cost += details.Detail_Total_Cost;
                    transfer.Total_Amount = transfer.Total_Cost;
                    transfer.TransferToMaintenanceDetails.Add(details);
                    db.TransferToMaintenanceDetailsTemps.Remove(sasa);
                }

                db.TransferToMaintenances.Add(transfer);
                db.TransferToMaintenanceTemps.Remove(transtemp);
                db.SaveChanges();
                var traBatch = new TransactionBatch();

                traBatch.OperationName = "تحويل من مخزن الى صيانة";
                traBatch.Description = transVm.Describtion;
                traBatch.Date = Convert.ToDateTime(transVm.Trans_Date);
                traBatch.UserId = UserId;               
                traBatch.OperationId = transfer.Master_Id;
                traBatch.Operation_Code = transfer.Trans_Code;
                TransactionBatch.AddNewTransaction(traBatch);
                db.Dispose();
                return Tuple.Create(Resources.validate.CreateSucc, transfer.Master_Id);
            }
            return Tuple.Create(Resources.validate.Error, 0);
        }
        public static String UpdateInvoiceMaster(TransferToMaintenanceVM salesinvoiceMasterVM)
        {
            var db = new AccountingContainer();

            TransferToMaintenance salesmaster = db.TransferToMaintenances.Find(salesinvoiceMasterVM.Master_Id);
            OperationRun.Addnewoperation(salesmaster.Trans_Code);
            var operation = db.OperationRuns.Where(a => a.Code == salesmaster.Trans_Code).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(salesmaster.Trans_Code);
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.PleaseWait;
            }
            if (salesmaster.Invoice_State == salesinvoiceMasterVM.Invoice_State)
            {
                OperationRun.DeleteOperation(salesmaster.Trans_Code);

                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.hasbeendoneby + " " + salesmaster.Invoice_State + " " + Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.previously;
            }
            else
            {
                String msg = "";
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        salesmaster.Invoice_State = salesinvoiceMasterVM.Invoice_State;
                        db.Entry(salesmaster).State = EntityState.Modified;
                         msg = CreateInvoiceGL(salesmaster);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        db.Dispose();
                    }
                    }
                OperationRun.DeleteOperation(salesmaster.Trans_Code);
                db.Dispose();
                return msg;
            }
           

        }
        public static String CreateInvoiceGL(TransferToMaintenance Transfer)
        {
            var msg = "";
            var db = new AccountingContainer();
            GLMaster glMaster = new GLMaster();
            glMaster.GL_Date = Transfer.Trans_Date;
            glMaster.GLCode = Transfer.Trans_Code;
            glMaster.GL_Desc = Transfer.Describtion;
            glMaster.Company_Id =(Int32)Transfer.Company_Id;
            glMaster.GLType_Id = "تحويل من مخزن لأشتراك";
            glMaster.Currency_Id = "الريال السعودى";
            glMaster.GLState = TransactionState.Pending;
            glMaster.Auto = true;

            GLDetail inventoryGL = new GLDetail(),
             MaintainGl = new GLDetail();
            //var BusId = db.BussinesUnits.FirstOrDefault().BU_Id;
            inventoryGL.Bu_Id = 0;
            MaintainGl.Bu_Id = 0;
            MaintainGl.Account_Id = db.ChartOfAccountings.Find(Transfer.Account_Id).Account_Id;
            MaintainGl.Account_Name = db.ChartOfAccountings.Find(Transfer.Account_Id).Account_Name;
            var detaiList = getMaterialOCompoundProduct(Transfer.TransferToMaintenanceDetails.ToList(), Transfer.Store_ID);

            var stor = db.Stores.Find(Transfer.Store_ID);
            var Chart = db.ChartOfAccountings.Where(a => a.Account_Code == stor.Store_Code).First();
            inventoryGL.Account_Id = Chart.Account_Id;
            inventoryGL.Account_Name = Chart.Account_Name;

            inventoryGL.Asset_Id =
                MaintainGl.Asset_Id = 0;

            inventoryGL.SubLedger_Id =0;
            MaintainGl.SubLedger_Id = Transfer.Subledger_ID;

            switch (Transfer.Invoice_State)
            {
                case TransactionState.Post:
                    glMaster.GLState = TransactionState.Post;
                    var gl = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode && a.GLState == glMaster.GLState).FirstOrDefault();
                    if (gl != null)
                    {
                        return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                    }

                    var errorMessage = new List<string> { };
                    foreach (var detail in detaiList)
                    {
                        var thisproduct = db.Products.Find(detail.Product_Id);

                        ProductBatches detailBatch = db.ProductBatches.Where(c => c.Product_ID == detail.Product_Id && c.Store_ID == Transfer.Store_ID).ToList().FirstOrDefault();
                        if (detailBatch.Store_ID != null)
                        {
                            if (detail.Product_Quantity > detailBatch.Product_Quantity)
                            {
                                var productName = db.Products.Find(detail.Product_Id).Prod_Name;
                                Transfer.Invoice_State = TransactionState.Pending;
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
              
                    db.ProductBatches.Load();
                    foreach (var detail in detaiList)
                    {

                        {
                            ProductBatches patsh = db.ProductBatches.Find(detail.Batch_ID);
                           
                            patsh.Product_Quantity -= detail.Product_Quantity;
                            patsh.Batch_Cost -=detail.Detail_Total_Cost;
                            if (patsh.Product_Quantity == 0)
                            {
                                patsh.Product_Cost = 0;

                            }
                            else {
                                patsh.Product_Cost = patsh.Batch_Cost / patsh.Product_Quantity;
                            }

                            db.Entry(patsh).State = EntityState.Modified;

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
                            storecard.Store_Id = Transfer.Store_ID;
                            storecard.Quantity = detail.Product_Quantity;
                            storecard.SalePrice = 0;
                            storecard.Product_Id = detail.Product_Id;
                            storecard.IncreaseQuantity = false;
                            storecard.TypeOfDocument = " تحويل من مخزن لأشتراك";
                            storecard.Document_Serial = Transfer.Trans_Code;
                            storecard.TotalCost = detail.Detail_Total_Cost;
                            storecard.TotalAmount = 0;
                            storecard.Account_Id = 0;
                            storecard.Subledger_Id = Convert.ToInt32(Transfer.Subledger_ID);
                            Store.AddinginstorCard(storecard);
                            MaintainGl.GLDetail_Debit += detail.Detail_Total_Cost;
                            inventoryGL.GLDetail_Credit += detail.Detail_Total_Cost;
                        }
                    }
                    break;
                       case TransactionState.Void:
                    {
                        glMaster.GLState = TransactionState.Void;
                        var glcheck = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode && a.GLState == glMaster.GLState).FirstOrDefault();
                        if (glcheck != null)
                        {
                            return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                        }

                        if (Transfer.HasOff == true)

                        {
                            msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.UnderCloseing;
                            Transfer.Invoice_State = TransactionState.Post;
                            db.SaveChanges();
                            return msg;
                        }
                        else
                        {
                          
                            foreach (var details in detaiList)
                            {
                                var ProBatsh = db.ProductBatches.Find(details.Batch_ID);
                                ProBatsh.Product_Quantity += details.Product_Quantity;
                                ProBatsh.Batch_Cost += details.Detail_Total_Cost;
                                ProBatsh.Product_Cost = ProBatsh.Batch_Cost / ProBatsh.Product_Quantity;
                                db.Entry(ProBatsh).State = EntityState.Modified;

                                var storecard = new StoreCard();
                                storecard.CostPrice = details.Detail_Total_Cost / details.Product_Quantity;
                                storecard.Date = DateTime.Now;
                                storecard.Store_Id = Transfer.Store_ID;
                                storecard.Quantity = details.Product_Quantity;
                                storecard.SalePrice = 0;
                                storecard.Product_Id = details.Product_Id;
                                storecard.IncreaseQuantity = true;
                                storecard.TypeOfDocument = "  استبعاد تحويل من مخزن الى اشتراك";
                                storecard.Document_Serial = Transfer.Trans_Code;
                                storecard.TotalCost = details.Detail_Total_Cost;
                                storecard.TotalAmount = 0;
                                storecard.Account_Id = 0;
                                storecard.Subledger_Id = Convert.ToInt32(Transfer.Subledger_ID);
                                Store.AddinginstorCard(storecard);
                                inventoryGL.GLDetail_Debit += details.Detail_Total_Cost;
                                MaintainGl.GLDetail_Credit += details.Detail_Total_Cost;

                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            if (msg == "")
            {
                glMaster.GLDetails.Add(MaintainGl);
                glMaster.GLDetails.Add(inventoryGL);
                db.GLMasters.Add(glMaster);
                msg = "تم عمل قيد الايصال بنجاح";
                GLMaster.GetBalanceForAccount(glMaster);
                GLMaster.GetBalanceForSubLedger(glMaster);
                db.SaveChanges();
            }
            if (Transfer.Invoice_State == TransactionState.Post)
            {
                msg = Resources.validate.GLDone;
            }
            else
            {
                msg = Resources.validate.GLReverse;
            }
            return msg;
           
        }

        public static List<TransferToMaintenanceDetails> getMaterialOCompoundProduct(List<TransferToMaintenanceDetails> Details, int Store_Id)
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
                List<TransferToMaintenanceDetails> Details2 = new List<TransferToMaintenanceDetails>();

                foreach (var item in ProList)
                {
                    var SubProductList = ProductAll.Where(a => a.Prod_ID == item.Product_Id).FirstOrDefault().ProductCompounds;
                    foreach (var subProduct in SubProductList)
                    {
                        var dublicate = Details.Where(a => a.Product_Id == subProduct.Product_Id).FirstOrDefault();
                        var batch = db.ProductBatches.Where(a => a.Product_ID == subProduct.Product_Id && a.Store_ID == Store_Id).FirstOrDefault();
                        if (dublicate == null)
                        {
                            var Detailnew = new TransferToMaintenanceDetails();
                            Detailnew.Product_Id = subProduct.Product_Id;
                            Detailnew.Product_Quantity = subProduct.Quantity * item.Product_Quantity;
                            Detailnew.Batch_ID = batch.Batch_ID;
                            if (batch != null)
                            {
                                Detailnew.Batch_ID = batch.Batch_ID;
                            }
                            if (batch != null && batch.Product_Quantity > 0)
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


                            var Detailnew = new TransferToMaintenanceDetails();
                            Detailnew.Batch_ID = batch.Batch_ID;
                            Detailnew.Product_Id = subProduct.Product_Id;
                            Detailnew.Product_Quantity = dublicate.Product_Quantity + (item.Product_Quantity * subProduct.Quantity);

                            if (batch != null)
                            {
                                Detailnew.Batch_ID = batch.Batch_ID;
                            }
                            if (batch != null && batch.Product_Quantity > 0)
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

        public static IEnumerable<SelectListItem> GetDepartment(int Branch)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> Department = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == Branch && p.Subled_Level == 2), "SubLed_Id", "SubLed_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return Department;
        }
        public static IEnumerable<SelectListItem> GetBesniessUnet(int BuId)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> Besniss = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == BuId && p.Subled_Level == 3), "SubLed_Id", "SubLed_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return Besniss;
        }
        public static TransferToMaintenanceVM fillForCreation(int Trnsfer)
        {
            var db = new AccountingContainer();
            var transfe = db.TransferToMaintenances.Find(Trnsfer);
            var transferVM = new TransferToMaintenanceVM();
            String startCodeMaintenance = "05";


            var copm = db.Companies.Where(a => a.Comp_Id == transfe.Company_Id);
            if(copm.Any())
            {
                transferVM.Company_Id = copm.FirstOrDefault().Comp_Id;
            }

            var Chart = db.ChartOfAccountings.Where(a => a.Account_Id == transfe.Account_Id);
            if(Chart.Any())
            {
                transferVM.Account_Id = Chart.FirstOrDefault().Account_Id;
            }

            var stor = db.Stores.Where(a => a.Store_ID == transfe.Store_ID);
            if (stor.Any())
            {
                transferVM.Store_ID = stor.FirstOrDefault().Store_ID;
            }


            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0).ToList();

            
            transferVM.AccountList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(startCodeMaintenance)), "Account_Id", "Account_Name");
            transferVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            transferVM.MyStoresList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");


            transferVM.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false), "SubLed_Id", "SubLed_Name");
            transferVM.MyDepartList = new SelectList(db.SubLedgers, "SubLed_Id", "SubLed_Name");

            transferVM.BUList = new SelectList(db.SubLedgers, "SubLed_Id", "SubLed_Name");
            transferVM.supList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفه يدوي"), "SubLed_Id", "SubLed_Name");


            transferVM.Master_Id = transfe.Master_Id;
            transferVM.Trans_Code = transfe.Trans_Code;
            transferVM.Describtion = transfe.Describtion;
            transferVM.Reference = transfe.Reference;
            transferVM.Trans_Date = transfe.Trans_Date.Day + "-" + transfe.Trans_Date.Month + "-" + transfe.Trans_Date.Year;
            transferVM.Total_Amount = transfe.Total_Amount;

            transferVM.Invoice_State = TransactionState.Pending;

            transferVM.Department_ID = transfe.Department_ID;
            transferVM.Transfer_BU = transfe.Transfer_BU;

            transferVM.Subledger_ID = transfe.Subledger_ID;
            transferVM.Branch_Id = transfe.Branch_Id;


            var transtemp = new TransferToMaintenanceTemp();
            transtemp.Trans_Code = transfe.Trans_Code;
            transtemp.Describtion = transfe.Describtion;
            transtemp.Trans_Date = transfe.Trans_Date;
            transtemp.Account_Id = transfe.Account_Id;
            transtemp.Store_Id = transfe.Store_ID;
            transtemp.Invoice_State = transfe.Invoice_State;
            transtemp.Total_Amount = transfe.Total_Amount;
            transtemp.Total_Cost = transfe.Total_Cost;
            transtemp.Total_Tax = transfe.Total_Tax;

            var trans = db.TransferToMaintenanceTemps.Find(transfe.Master_Id);

            if(trans != null)
            {
             
            }
            else
            {
                transtemp.Master_Id = transfe.Master_Id;
                db.TransferToMaintenanceTemps.Add(transtemp);
            }
            db.SaveChanges();
            return transferVM;
        }



        public static List<TransferToMaintenanceDetails> getAlldataMaintainans(int Id)
        {
            var db = new AccountingContainer();
            var Main = db.TransferToMaintenanceDetails.Where(a => a.TransferToMaintenanceMaster_Id == Id).ToList();
            return Main;
        }
        public static TransferToMaintenanceDetailsVM PreperTransferMain2(int masterId)
        {
            var db = new AccountingContainer();
            var TransDVM = new TransferToMaintenanceDetailsVM();

            var thisdetails = GetAllTempTransferDetailsDetails(masterId).ToList().FirstOrDefault();

            var product = thisdetails.Product_Id;

            TransDVM.Product_Quantity = thisdetails.Product_Quantity;

            TransDVM.Details_Id = thisdetails.Details_Id;
            TransDVM.Product_Price = thisdetails.Product_Price;
            //TransDVM.Product_Tax = thisdetails.Product_Tax;
            //TransDVM.Detail_Total_Tax = thisdetails.Detail_Total_Tax;
            TransDVM.TransferToMaintenanceMaster_Id = masterId;

            var batchs = db.ProductBatches.Where(batch => batch.Product_ID == thisdetails.Product_Id&& batch.Product.IsActive != false).ToList();

            List<ProductBatchSelection> _prodectPatch = new List<ProductBatchSelection>();
            foreach (var item in batchs)
            {
                var ProductBatchSelectio = new ProductBatchSelection();
                ProductBatchSelectio.Batch_Id = item.Batch_ID;
                ProductBatchSelectio.ProductCost = item.Product_Cost;
                ProductBatchSelectio.ProductName = item.Product.Prod_Name;
                ProductBatchSelectio.Product_Code = item.Product.Bar_Code;
                ProductBatchSelectio.Product_Id = item.Product_ID;
                _prodectPatch.Add(ProductBatchSelectio);
            }
            List<object> newListproout = new List<object>();

            foreach (var member in _prodectPatch)
                newListproout.Add(new
                {
                    Id = member.Batch_Id,
                    Name = member.ProductName + "   " + member.Product_Code
                });


            TransDVM.BatchList = new SelectList(newListproout, "Id", "Name");
     


            var prod = db.Products.Where(c => c.Prod_ID == product);
            if (prod.Any())
            {
                TransDVM.Batch_ID = prod.FirstOrDefault().Prod_ID;
            }

            return TransDVM;

        }
        public static List<TransferToMaintenanceDetails> GetAllTempTransferDetailsDetails(int masterVmId)
        {
            var db = new AccountingContainer();
            var DetailsTemp = db.TransferToMaintenanceDetails.Where(invoice => invoice.Details_Id == masterVmId).ToList();
            return DetailsTemp;

        }
        public static List<TransferToMaintenanceDetails> GetAllTempTransferDetails2(int masterVmId)
        {
            var db = new AccountingContainer();
            var Details = db.TransferToMaintenanceDetails.Where(invoice => invoice.TransferToMaintenanceMaster_Id == masterVmId).ToList();
            return Details;
        }
        public static List<TransferToMaintenanceDetailsTemp> GetAllTempTransDetailsresult(int masterVmId)
        {
            var db = new AccountingContainer();
            var TransTemp = db.TransferToMaintenanceTemps.Find(masterVmId);
            return TransTemp.TransferToMaintenanceDetailsTemps.ToList();
        }
        public static string AddTransDetailsTemp(TransferToMaintenanceDetailsVM Detail)
        {
            var db = new AccountingContainer();
            if (Detail.Batch_ID == null || Detail.Batch_ID == 0)
            {
                return "من فضلك اختر الصنف";
            }

            if (Detail.Product_Quantity == null || Detail.Product_Quantity == 0)
            {
                return "من فضلك ادخل الكمية ";
            }
            var TransMasterTemp = db.TransferToMaintenanceTemps.Find(Detail.TransferToMaintenanceMaster_Id);
            var batch = db.ProductBatches.Find(Detail.Batch_ID);

            var TransDetailsTemp = new TransferToMaintenanceDetailsTemp();
            TransDetailsTemp.Batch_ID = batch.Batch_ID;
            TransDetailsTemp.Product_Id = Detail.Batch_ID.Value;
            TransDetailsTemp.Product_Price = Detail.Product_Price;
            TransDetailsTemp.Product_Quantity = Detail.Product_Quantity;
            TransDetailsTemp.Detail_Total_Cost = batch.Product_Cost * Detail.Product_Quantity;
            TransMasterTemp.TransferToMaintenanceDetailsTemps.Add(TransDetailsTemp);
            db.Entry(TransMasterTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "تم الاضافة بنجاح";
        }
        public static TransferToMaintenance EditMaintanens(TransferToMaintenanceVM MasterVM)
        {
            var db = new AccountingContainer();
            var Trans = db.TransferToMaintenances.Find(MasterVM.Master_Id);
            var Transtemp = db.TransferToMaintenanceTemps.Find(MasterVM.Master_Id);
            

            Trans.Master_Id = MasterVM.Master_Id;
            Trans.Account_Id = MasterVM.Account_Id;
            
            decimal d= Trans.Total_Amount;
            Trans.Company_Id = MasterVM.Company_Id;
            Trans.Describtion = MasterVM.Describtion;
            Trans.Invoice_State = TransactionState.Pending;       
            Trans.Reference = MasterVM.Reference;
            Trans.Store_ID = MasterVM.Store_ID;

            var subResult = db.SubLedgers.Find(MasterVM.Subledger_ID);
            if (subResult != null)
            {
                Trans.Subledger_ID = subResult.SubLed_Id;
            }
            else
            {
                if (MasterVM.Transfer_BU != null && MasterVM.Transfer_BU != 0)
                {
                    Trans.Subledger_ID = (int)MasterVM.Transfer_BU;
                    Trans.Transfer_BU = MasterVM.Transfer_BU;
                }
                else if ((MasterVM.Transfer_BU == null || MasterVM.Transfer_BU == 0) && (MasterVM.Department_ID != null && MasterVM.Department_ID != 0))
                {
                    Trans.Subledger_ID = (int)MasterVM.Department_ID;
                    Trans.Department_ID = MasterVM.Department_ID;
                }
                else if ((MasterVM.Transfer_BU == null || MasterVM.Transfer_BU == 0) && (MasterVM.Department_ID == null || MasterVM.Department_ID == 0) && (MasterVM.Branch_Id != null && MasterVM.Branch_Id != 0))
                {
                    Trans.Subledger_ID = (int)MasterVM.Branch_Id;
                    Trans.Branch_Id = MasterVM.Branch_Id;
                }
                else
                {
                    var thissub = db.SubLedgers.Where(e => e.Subled_Level == 0).FirstOrDefault();
                    Trans.Subledger_ID = (int)thissub.SubLed_Id;
                }
            }

            Trans.Trans_Date = Convert.ToDateTime(MasterVM.Trans_Date);
            decimal k = 0;

            var oldDetail = db.TransferToMaintenanceDetails.Where(a => a.TransferToMaintenanceMaster_Id == Trans.Master_Id).ToList();
            var detailListTemp = db.TransferToMaintenanceDetailsTemps.Where(e => e.TransferToMaintenanceTempMaster_Id == Transtemp.Master_Id).ToList();
            List<TransferToMaintenanceDetailsTemp> ListTempRemove = new List<TransferToMaintenanceDetailsTemp> { };
            if (detailListTemp.Count()==0)
            {
                return null;
            }
            foreach (var detail in detailListTemp)
            {

                var item = new TransferToMaintenanceDetails();
                item.Product_Id = detail.Product_Id;
                //var productPrice = db.ProductBatches.Where(a => a.Product_ID == detail.Product_Id).FirstOrDefault();

                
                item.Product_Price = detail.Product_Price;
                item.Product_Quantity = detail.Product_Quantity;
                ProductBatches detailBatch = db.ProductBatches.Find(detail.Batch_ID);
                item.Product_Id = detailBatch.Product_ID;
                item.Batch_ID = detail.Batch_ID;
                if (detail.Product_Quantity > detailBatch.Product_Quantity)
                {
                    return null;

                }
                // Calculate the Total_Cost
                item.Detail_Total_Cost = detail.Product_Price * detail.Product_Quantity;
                Trans.Total_Amount = detail.Detail_Total_Cost;
                d = item.Detail_Total_Cost;
                k += item.Detail_Total_Cost;
                Trans.Total_Cost = k;

                Trans.TransferToMaintenanceDetails.Add(item);
                ListTempRemove.Add(detail);
                //db.TransferToMaintenanceDetailsTemps.Remove(detail);
                //db.SaveChanges();
            }
            db.Entry(Trans).State = EntityState.Modified;
            db.TransferToMaintenanceTemps.Remove(Transtemp);
            db.TransferToMaintenanceDetails.RemoveRange(oldDetail);
            db.TransferToMaintenanceDetailsTemps.RemoveRange(ListTempRemove);
            db.SaveChanges();

            return Trans;
        }
        public static List<TransferToMaintenanceDetailsTemp> GetAllDetailForEditing(int MasterId)
        {
            var db = new AccountingContainer();
            var detailTemp = db.TransferToMaintenanceTemps.Find(MasterId);
            db.TransferToMaintenanceDetailsTemps.RemoveRange(detailTemp.TransferToMaintenanceDetailsTemps);
            db.SaveChanges();
            var invoice = db.TransferToMaintenances.Find(MasterId);
            List<TransferToMaintenanceDetailsTemp> Lista = new List<TransferToMaintenanceDetailsTemp> { };
            foreach (var item in invoice.TransferToMaintenanceDetails)
            {
                var invoiceTemp = new TransferToMaintenanceDetailsTemp();
                invoiceTemp.Batch_ID = item.Batch_ID.Value;
                invoiceTemp.Detail_Total_Tax = item.Detail_Total_Tax;
                invoiceTemp.Product_Id = item.Product_Id;
                invoiceTemp.Product_Price = item.Product_Price;
                invoiceTemp.Product_Quantity = item.Product_Quantity;
                invoiceTemp.Product_Tax = item.Product_Tax;
                invoiceTemp.Total_Amount = item.Total_Amount;
                invoiceTemp.Detail_Total_Cost = item.Detail_Total_Cost;
            
                invoiceTemp.TransferToMaintenanceTempMaster_Id = MasterId;
                Lista.Add(invoiceTemp);

            }
            db.TransferToMaintenanceDetailsTemps.AddRange(Lista);

            db.SaveChanges();

            return detailTemp.TransferToMaintenanceDetailsTemps.ToList();
        }

        public static string DeleteMaintaince(int id)
        {
            var db = new AccountingContainer();
            TransferToMaintenance transfer = db.TransferToMaintenances.Find(id);

            if (transfer.Invoice_State != TransactionState.Pending)
                return "لم يتم الحذف";

            db.TransferToMaintenanceDetails.RemoveRange(transfer.TransferToMaintenanceDetails);
            db.TransferToMaintenances.Remove(transfer);

            db.SaveChanges();
            db.Dispose();
            return "تم الحذف بنجاح";
        }
    }
}