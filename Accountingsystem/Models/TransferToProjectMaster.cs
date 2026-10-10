using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Accountingsystem.ViewModel;
namespace Accountingsystem.Models
{
    public partial class TransferToProjectMaster
    {/// <summary>
     ///  check that code is unique 
     /// </summary>
     /// <param name="Serial"></param>
     /// <returns>bool</returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.TransferToProjectMasters.ToList();
            foreach (var item in allinData)
            {
                if (item.Trans_Code == Serial)
                {
                    return true;
                }
                else if (!Serial.StartsWith("TTPRO-"))
                {
                    return true;
                }
                else if (!Serial.StartsWith("TTSTO-"))
                {
                    return true;
                }
                else
                {
                    return false;
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
        /// get all transfer to project invoice 
        /// </summary>
        /// <returns>List<TransferToProjectMaster></returns>
        public static List<TransferToProjectMaster> GetAllSalesReceipt()
        {

            var db = new AccountingContainer();
            var orders = db.TransferToProjectMasters.Where(e => e.Trans_Code.StartsWith("TTPRO")).ToList();
            return orders;
        }
        /// <summary>
        /// get all transfer to store 
        /// </summary>
        /// <returns>List<TransferToProjectMaster></returns>
        public static List<TransferToProjectMaster> GetAlltransFromProject()
        {

            var db = new AccountingContainer();
            var orders = db.TransferToProjectMasters.Where(e => e.Trans_Code.StartsWith("TTSTO")).ToList();
            return orders;
        }
        /// <summary>
        /// get all quantity for that batch of that product
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns>decimal</returns>
        public static decimal GetAvailbleProductQty(int batchId)
        {
            var db = new AccountingContainer();
            var batch = db.ProductBatches.Find(batchId);
            return batch.Product_Quantity;

        }
        /// <summary>
        /// get that invoice by its id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns>TransferToProjectMaster</returns>
        public static TransferToProjectMaster GetTransferProjectById(int invoiceId)
        {
            var db = new AccountingContainer();
            var detail = db.TransferToProjectMasters.Find(invoiceId);
            return detail;

        }
        /// <summary>
        /// prepare detail for add to list
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns>TransferToProjectDetailVM</returns>
        public static TransferToProjectDetailVM PrepareInvoiceMasterDetail(int storeId)
        {
            var db = new AccountingContainer();
            var TransVM = new TransferToProjectDetailVM();
            var newListproout = from prod in db.Products.Where(batch =>batch.IsActive!=false)
                         select new
                         {

                             Id = prod.Prod_ID,
                             Name = prod.Prod_Name + "   " + prod.Bar_Code


                         };
            
            
            //List<ProductBatchSelection> _ProductBatchSelection = new List<ProductBatchSelection>();
            //foreach (var item in batchs)
            //{
            //    var productBatchSelection = new ProductBatchSelection();

            //    productBatchSelection.Batch_Id = item.Batch_ID;
            //    productBatchSelection.ProductCost = item.Product_Cost;
            //    productBatchSelection.Product_Code = item.Product.Bar_Code;
            //    productBatchSelection.ProductName = item.Product.Prod_Name;
            //    productBatchSelection.Product_Id = item.Product_ID;
            //    _ProductBatchSelection.Add(productBatchSelection);


            //}
            //List<object> newListproout = new List<object>();

            //foreach (var member in _ProductBatchSelection)
            //    newListproout.Add(new
            //    {
            //        Id = member.Batch_Id,
            //        Name = member.ProductName + "   " + member.Product_Code
            //    });


            TransVM.BatchList = new SelectList(newListproout, "Id", "Name");
            
            TransVM.ProducttaxList = new SelectList(db.Taxes, "Tax_ID", "Tax_Name");

            return TransVM;

        }
        /// <summary>
        /// prepare master for create anew transfer to project
        /// </summary>
        /// <returns>TransferToProjectMasterVM</returns>
        public static TransferToProjectMasterVM AddNewTransferForCreation()
        {
            var db = new AccountingContainer();
            var masterVM = new TransferToProjectMasterVM();
            String StartCodeOfExpense = "05";
            // TODO: get serial no. from database
            //int lastCode = (from s in db.TransferToProjectMasters
            //                select s).Count();
            //masterVM.Trans_Code = (lastCode + 1).ToString();


            var lastCode = db.TransferToProjectMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
            if (lastCode == null)
            {
                masterVM.Trans_Code = "TTPRO-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                masterVM.Trans_Code = "TTPRO-" + (Convert.ToInt32(lastCode.Trans_Code.Substring(6, lastCode.Trans_Code.Length - 6)) + 1).ToString("D3");
            }

            masterVM.Trans_Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");

            masterVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            var ProjectName = db.Projects.Where(a => a.Project_State != ProjectState.Finished).Select(a => a.Project_Name);
            var ProList = db.SubLedgers.Where(s => ProjectName.Contains(s.SubLed_Name)).ToList();
            masterVM.ProjList = new SelectList(ProList.Where(a => a.SubLed_Type == "مشروع" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            masterVM.MyStoresList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            List<object> newList = new List<object>();
            var members = db.PaymentMethods.ToList();


            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false);
           

            masterVM.AccountList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfExpense)), " Account_Id", "Account_Name");
            masterVM.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            masterVM.DepList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            masterVM.BUList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            masterVM.SubList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");

            masterVM.Invoice_State = TransactionState.Pending;
            var TransTemp = new TransferToProjectTemp();
            var id = (from s in db.TransferToProjectTemps
                      select s).ToList().LastOrDefault();
            if (id == null)
            { TransTemp.Master_Id = 1; }
            else
            {
                TransTemp.Master_Id = id.Master_Id + 1;
            }
           
            db.TransferToProjectTemps.Add(TransTemp);
            db.SaveChanges();
            masterVM.Master_Id = TransTemp.Master_Id;
            return masterVM;
        }
        /// <summary>
        /// prepare master for create anew transfer to Store
        /// </summary>
        /// <returns>TransferToProjectMasterVM</returns>
        public static TransferToProjectMasterVM AddNewTransferForCreationtoStore()
        {
            var db = new AccountingContainer();
            var masterVM = new TransferToProjectMasterVM();
            String StartCodeOfExpense = "05";
            // TODO: get serial no. from database
            //int lastCode = (from s in db.TransferToProjectMasters
            //                select s).Count();
            //masterVM.Trans_Code = (lastCode + 1).ToString();


            var lastCode = db.TransferToProjectMasters.OrderByDescending(c => c.Master_Id).Where(e=>e.Trans_Code.StartsWith("TTSTO")==true).FirstOrDefault();
            if (lastCode == null)
            {
                masterVM.Trans_Code = "TTSTO-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                masterVM.Trans_Code = "TTSTO-" + (Convert.ToInt32(lastCode.Trans_Code.Substring(6, lastCode.Trans_Code.Length - 6)) + 1).ToString("D3");
            }

            masterVM.Trans_Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");

            masterVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            var projectend = db.Projects.Where(a => a.Project_State == ProjectState.Finished).ToList();
            var project = db.Projects.Where(a => a.Project_State != ProjectState.Finished).Select(a=>a.Project_Name).ToList();
            var ProList = db.SubLedgers.Where(s => project.Contains(s.SubLed_Name)).ToList();
            masterVM.ProjList = new SelectList(ProList.Where(a => a.SubLed_Type == "مشروع" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            masterVM.MyStoresList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
        


            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false);
            

            masterVM.AccountList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfExpense)), " Account_Id", "Account_Name");
            masterVM.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            masterVM.DepList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            masterVM.BUList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            masterVM.SubList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");

            masterVM.Invoice_State = TransactionState.Pending;
            var TransTemp = new TransferToProjectTemp();
            var id = (from s in db.TransferToProjectTemps
                      select s).ToList().LastOrDefault();
            var lastTransfare = db.TransferToProjectMasters.ToList().LastOrDefault();
            if (lastTransfare == null )
            {
                return masterVM;
            }
            if (id == null)
            { TransTemp.Master_Id = 1;
                TransTemp.Project_Id = lastTransfare.Master_Id+1;
            }
            else
            {
                TransTemp.Master_Id = id.Master_Id + 1;
                TransTemp.Project_Id = lastTransfare.Master_Id+1;
            }

            db.TransferToProjectTemps.Add(TransTemp);
            db.SaveChanges();
            masterVM.Master_Id = TransTemp.Master_Id;
            return masterVM;
        }
        /// <summary>
        /// add to list of transfer and check validation
        /// </summary>
        /// <param name="invoiceMasterDetailVM"></param>
        /// <returns>string</returns>
        public static string AddInvoiceDetailsTemp(TransferToProjectDetailVM invoiceMasterDetailVM)
        {
            var db = new AccountingContainer();
            var invoiceMasterDetailsTemp = new TransferToProjectDetailTemp();
            if (invoiceMasterDetailVM.Batch_ID == null || invoiceMasterDetailVM.Batch_ID == 0)
            {
                return Resources.validate.ChooseProduct;
            }

            if (invoiceMasterDetailVM.Product_Quantity == null || invoiceMasterDetailVM.Product_Quantity == 0)
            {
                return Resources.InvoiceMasterAddNewInvoice.Quantity;
            }
            var invoiceMasterTemp = db.TransferToProjectTemps.Find(invoiceMasterDetailVM.Master_ID);
            var batch = db.Products.Find(invoiceMasterDetailVM.Batch_ID);
            invoiceMasterDetailsTemp.Batch_ID = 0;
            invoiceMasterDetailsTemp.Product_Id = batch.Prod_ID;
            var dtails = invoiceMasterTemp.TransferToProjectDetailTemps;
            var dublicateinTemp = dtails.Where(z => z.Master_ID == invoiceMasterTemp.Master_Id && z.Product_Id == invoiceMasterDetailsTemp.Product_Id).ToList();
            if (dublicateinTemp.FirstOrDefault() != null)
            {
                return Resources.validate.productSame;
            }
            
                if (invoiceMasterDetailVM.Master_ID == 0)
                {
                    var k = db.TransferToProjectDetails.Find(invoiceMasterDetailVM.Details_Id).Master_ID;
                    invoiceMasterTemp = db.TransferToProjectTemps.Find(k);

                }
                else
                {
                    invoiceMasterTemp = db.TransferToProjectTemps.Find(invoiceMasterDetailVM.Master_ID);
                }




                invoiceMasterDetailsTemp.Product_Price = batch.Prod_Cost_Price.Value;
                invoiceMasterDetailsTemp.Product_Quantity = invoiceMasterDetailVM.Product_Quantity;

                var resultTrea = db.Taxes.Find(invoiceMasterDetailVM.Product_Tax);
                if (resultTrea != null)
                {

                    invoiceMasterDetailsTemp.Product_Tax = resultTrea.Tax_ID;

                }
                var a = db.Taxes.Find(invoiceMasterDetailsTemp.Product_Tax);
                

                invoiceMasterDetailsTemp.Detail_Total_Cost = invoiceMasterDetailsTemp.Product_Price * invoiceMasterDetailVM.Product_Quantity;


                invoiceMasterTemp.TransferToProjectDetailTemps.Add(invoiceMasterDetailsTemp);

                db.Entry(invoiceMasterTemp).State = EntityState.Modified;
                db.SaveChanges();

                return Resources.validate.SuccessfullAdd;
            
           
        }
        /// <summary>
        /// get all detail of Transfer by its id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns></returns>
        public static List<TransferToProjectDetailTemp> GetAllTempTransferDetails(int masterVmId)
        {
            var db = new AccountingContainer();

            var TransDetailsTemp = db.TransferToProjectDetailTemps.Where(invoice => invoice.Master_ID == masterVmId).ToList();
            return TransDetailsTemp;
        }
        /// <summary>
        /// add a new transfer invoice in db and make anew code for that 
        /// </summary>
        /// <param name="TransVM"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static Tuple< String,int> addnewSaleReciept(TransferToProjectMasterVM TransVM , String UserId)
        {
            var db = new AccountingContainer();
            var TransMaster = new TransferToProjectMaster();
            var TransMasterTemp = db.TransferToProjectTemps.Find(TransVM.Master_Id);
            if (TransVM != null)
            {
                //int lastCode = (from s in db.TransferToProjectTemps
                //                select s.Master_Id).Count();
                //TransMaster.Trans_Code = (lastCode + 1).ToString();
                var lastCode = db.TransferToProjectMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                if (lastCode == null)
                {
                    TransMaster.Trans_Code = "TTPRO-001";
                }
                else
                {
                    var lastSerial = db.TransferToProjectMasters.Where(a=>a.Trans_Code.StartsWith("TTPRO-")).OrderByDescending(c => c.Master_Id).FirstOrDefault().Trans_Code;
                    //using string substring method to get the number of the last inserted
                    TransMaster.Trans_Code = "TTPRO-" + (Convert.ToInt32(lastSerial.Substring(6, lastSerial.Length - 6)) + 1).ToString("D3");
                }
                TransMaster.Describtion = TransVM.Describtion;
                var comp = db.Companies.Find(TransVM.Company_Id);
                if (comp != null)
                {
                    var date = Convert.ToDateTime(TransVM.Trans_Date);
                    if (date < comp.StartFinancialDate)
                    {
                        TransMaster.Trans_Date = DateTime.Now;
                    }
                    else
                    {
                        TransMaster.Trans_Date = Convert.ToDateTime(TransVM.Trans_Date);
                    }
                }
                else
                {
                    TransMaster.Trans_Date = DateTime.Now;
                }
               
                TransMaster.Project_Id = TransVM.Project_Id;
                TransMaster.Invoice_State = TransactionState.Pending;
                TransMaster.HasTransfered = false;
                TransMaster.Account_Id = TransVM.Account_Id;
                //if (TransVM.Branch_Id == null || TransVM.Branch_Id == 0)
                //{

                //}
                //else
                //{
                //    TransMaster.Branch_Id = TransVM.Branch_Id.Value;
                //}
                
                TransMaster.Reference = TransVM.Reference;
                TransMaster.Company_Id = TransVM.Company_Id;
                TransMaster.Store_ID = TransVM.Store_Id;

                //if (TransVM.SubLedger_ID != null)
                //{

                //    TransMaster.SubLedger_ID = TransVM.SubLedger_ID;

                //}
                //else if (TransVM.BussinesUnit_ID != null && TransVM.BussinesUnit_ID != 0)
                //{

                //    TransMaster.SubLedger_ID = TransVM.BussinesUnit_ID;
                //    TransMaster.BussinesUnit_ID = TransVM.BussinesUnit_ID;
                //    TransMaster.Department_ID = TransVM.Department_ID;
                //    TransMaster.Branch_Id = TransVM.Branch_Id;

                //}
                //else if (TransVM.Department_ID != null && TransVM.Department_ID != 0)
                //{
                //    TransMaster.SubLedger_ID = TransVM.Department_ID;
                //    TransMaster.Department_ID = TransVM.Department_ID;
                //    TransMaster.Branch_Id = TransVM.Branch_Id;
                //}
                //else if (TransVM.Branch_Id != null && TransVM.Branch_Id != 0)
                //{

                //    TransMaster.SubLedger_ID = TransVM.Branch_Id;
                //    TransMaster.Branch_Id = TransVM.Branch_Id;
                //}
                //else { TransMaster.SubLedger_ID = 1; }


                if (TransVM.Files.FirstOrDefault() != null)
                {
                    foreach (var file in TransVM.Files)
                    {
                        var filedetails = new TransferToprojectFile1();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.FileContent = data;
                        filedetails.FileType = file.ContentType;
                        if (data.Length < 1000000)
                        {
                            TransMaster.TransferToprojectFile1.Add(filedetails);
                        }
                        
                    }
                }
                if (TransMasterTemp.TransferToProjectDetailTemps.ToList().Count()==0)
                {
                    return Tuple.Create("لايوجد تفاصيل للتحويل", 0);
                }
                foreach (var item in TransMasterTemp.TransferToProjectDetailTemps.ToList())
                {
                    var details = new TransferToProjectDetail();
                    details.Details_Id = item.Details_Id;
                    details.Product_Quantity = item.Product_Quantity;
                    details.Reverse_quantity = item.Product_Quantity;
                    //  details.Total_Amount = item.Total_Amount - (item.Total_Amount * (saleMastVM.Master_Discount / 100));
                    details.Detail_Total_Tax = item.Detail_Total_Tax;
                    details.Product_Tax = item.Product_Tax;
                    details.Total_Amount += details.Detail_Total_Tax;
                
                    details.Batch_ID = item.Batch_ID;
                  
                    details.Product_ID = item.Product_Id;
                    // Checks for the products quantity if available or not

                    // Calculate the Total_Cost
                  
                    details.Detail_Total_Cost = item.Detail_Total_Cost;
                    details.Reverse_Total = details.Detail_Total_Cost;
                    details.Product_Price = item.Product_Price;
                    //if (details.Detail_Total_Cost==0) {
                    //    return Tuple.Create("يوجد خطأ فى الادخال", 0);

                    //}

                    TransMaster.Total_Cost += details.Detail_Total_Cost;
                    TransMaster.Total_Amount = TransMaster.Total_Cost;
                    TransMaster.TransferToProjectDetails.Add(details);
                    db.TransferToProjectDetailTemps.Remove(item);

                }
                db.TransferToProjectMasters.Add(TransMaster);
                db.TransferToProjectTemps.Remove(TransMasterTemp);
                db.SaveChanges();
                db.Dispose();
                CreateTransactionBatch(TransMaster , UserId);
                return Tuple.Create( "تم الحفظ بنجاح",TransMaster.Master_Id);
            }
            return  Tuple.Create("خطأ اثناء الحفظ",0);
        }
        /// <summary>
        /// make a gl of that transfer to project
        /// </summary>
        /// <param name="salesinvoiceMasterVM"></param>
        /// <returns>String</returns>
        public static String UpdateInvoiceMaster(TransferToProjectMasterVM salesinvoiceMasterVM)
        {
            var db = new AccountingContainer();

            TransferToProjectMaster salesmaster = db.TransferToProjectMasters.Find(salesinvoiceMasterVM.Master_Id);
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
                // Edit master invoice state to the new state
                salesmaster.Invoice_State = salesinvoiceMasterVM.Invoice_State;
                String msg = "";
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Entry(salesmaster).State = EntityState.Modified;

                        // Create GL for the invoice 
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
        /// <summary>
        /// make agl of transfer to store
        /// </summary>
        /// <param name="salesinvoiceMasterVM"></param>
        /// <returns>String</returns>

        public static String UpdateInvoiceMasterTrans(TransferToProjectMasterVM salesinvoiceMasterVM)
        {
            var db = new AccountingContainer();

            TransferToProjectMaster salesmaster = db.TransferToProjectMasters.Find(salesinvoiceMasterVM.Master_Id);
            // Edit master invoice state to the new state
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
                salesmaster.Invoice_State = salesinvoiceMasterVM.Invoice_State;
                String msg = "";
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        db.Entry(salesmaster).State = EntityState.Modified;

                        // Create GL for the invoice 
                         msg = CreateInvoiceGLfortrans(salesmaster);
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
        /// <summary>
        /// make agl for transfer to project and change status of it 
        /// </summary>
        /// <param name="master"></param>
        /// <returns>CreateInvoiceGL</returns>
        public static String CreateInvoiceGL(TransferToProjectMaster master)
        {
            String msg = "";
            var db = new AccountingContainer();
            GLMaster glMaster = new GLMaster();
            glMaster.GL_Date = master.Trans_Date;
            glMaster.GL_Desc = master.Describtion;
            glMaster.Company_Id = master.Company_Id;
            glMaster.GLType_Id = "تحويل من مخزن الى مشروع";
            glMaster.Currency_Id = "الريال السعودى";
            glMaster.GLCode = master.Trans_Code;
            glMaster.GLState = TransactionState.Pending;
            glMaster.Auto = true;
            GLDetail inventoryGL = new GLDetail(),
             ExpenseGL = new GLDetail();
            //var BusId = db.BussinesUnits.FirstOrDefault().BU_Id;
            inventoryGL.Bu_Id = 0;
            ExpenseGL.Bu_Id = 0;
            ExpenseGL.Account_Id = db.ChartOfAccountings.Find(master.Account_Id).Account_Id;
            ExpenseGL.Account_Name = db.ChartOfAccountings.Find(master.Account_Id).Account_Name;

            var store = db.Stores.Find(master.Store_ID);
            var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).First();
            inventoryGL.Account_Id = ChartOfAccounInventory.Account_Id;
            inventoryGL.Account_Name = ChartOfAccounInventory.Account_Name;

            inventoryGL.Asset_Id =
                ExpenseGL.Asset_Id = 0;

            inventoryGL.SubLedger_Id = 0;
            ExpenseGL.SubLedger_Id = master.Project_Id;
            var detailList = getMaterialOCompoundProduct(master.TransferToProjectDetails.ToList(), master.Store_ID);
            switch (master.Invoice_State)
            {

                case TransactionState.Post:
                    glMaster.GLState = TransactionState.Post;
                    var gl = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode && a.GLState == glMaster.GLState).FirstOrDefault();
                    if (gl != null)
                    {
                        return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                    }
                    var errorMessage = new List<string> { };
                    foreach (var detail in detailList)
                    {
                        var thisproduct = db.Products.Find(detail.Product_ID);

                        ProductBatches detailBatch = db.ProductBatches.Where(c => c.Product_ID == detail.Product_ID && c.Store_ID == master.Store_ID).ToList().FirstOrDefault();
                        if (detailBatch == null)
                        {
                            var productName = db.Products.Find(detail.Product_ID).Prod_Name;
                            master.Invoice_State = TransactionState.Pending;
                            //db.SaveChanges();
                            errorMessage.Add(Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Thereisnotenoughproduct+" " + productName);
                        }
                        else if (detailBatch.Store_ID != null)
                        {
                            if (detail.Product_Quantity > detailBatch.Product_Quantity)
                            {
                                var productName = db.Products.Find(detail.Product_ID).Prod_Name;
                                master.Invoice_State = TransactionState.Pending;
                                //db.SaveChanges();
                                errorMessage.Add(Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Thereisnotenoughproduct+" " + productName);

                            }
                        }

                    }
                    if (errorMessage.Count() != 0)
                    {


                        string combindedString = string.Join("\n", errorMessage.ToArray());
                        return combindedString;
                    }
                  
                    db.ProductBatches.Load();
                    foreach (var detail in detailList)
                    {
                        var product= db.Products.Find(detail.Product_ID);
                        ProductBatches detailBatch = product.ProductBatches.Where(a=>a.Store_ID== master.Store_ID).FirstOrDefault();
                        var costPrice = detailBatch.Batch_Cost / detailBatch.Product_Quantity;
                        detailBatch.Product_Quantity -= detail.Product_Quantity;
                        detailBatch.Batch_Cost -= costPrice * detail.Product_Quantity;
                        if (detailBatch.Product_Quantity == 0)
                        {
                            detailBatch.Product_Cost = 0;
                        }
                        else
                        {
                            detailBatch.Product_Cost = detailBatch.Batch_Cost / detailBatch.Product_Quantity;
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
                        storecard.SalePrice = 0;
                        storecard.Product_Id = detail.Product_ID;
                        storecard.IncreaseQuantity = false;
                        storecard.TypeOfDocument = " تحويل من مخزن لمشروع";
                        storecard.Document_Serial = master.Trans_Code;
                        storecard.TotalCost = costPrice * detail.Product_Quantity;
                        storecard.TotalAmount = 0;
                        storecard.Account_Id =0;
                        storecard.Subledger_Id = Convert.ToInt32(master.SubLedger_ID);
                        Store.AddinginstorCard(storecard);
                        ExpenseGL.GLDetail_Debit += costPrice * detail.Product_Quantity;
                        inventoryGL.GLDetail_Credit += costPrice * detail.Product_Quantity;

                    }
                    break;
                case TransactionState.Void:
                    glMaster.GLState = TransactionState.Void;
                    var glCheck = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode && a.GLState == glMaster.GLState).FirstOrDefault();
                    if (glCheck != null)
                    {
                        return Resources.validate.GLDone;
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
                       
                        foreach (var detail in detailList)
                        {
                            ProductBatches detailBatch = db.ProductBatches.Find(detail.Batch_ID);
                            detailBatch.Product_Quantity += detail.Product_Quantity;
                            detailBatch.Batch_Cost +=detail.Detail_Total_Cost;
                            db.Entry(detailBatch).State = EntityState.Modified;

                            var storecard = new StoreCard();
                            storecard.CostPrice = detail.Detail_Total_Cost / detail.Product_Quantity;
                            storecard.Date = DateTime.Now;
                            storecard.Store_Id = master.Store_ID;
                            storecard.Quantity = detail.Product_Quantity;
                            storecard.SalePrice = 0;
                            storecard.Product_Id = detail.Product_ID;
                            storecard.IncreaseQuantity = true;
                            storecard.TypeOfDocument = " استبعاد تحويل من مخزن لمشروع";
                            storecard.Document_Serial = master.Trans_Code;
                            storecard.TotalCost = detail.Detail_Total_Cost;
                            storecard.TotalAmount = 0;
                            storecard.Account_Id = 0;
                            storecard.Subledger_Id = Convert.ToInt32(master.SubLedger_ID);
                            Store.AddinginstorCard(storecard);
                            ExpenseGL.GLDetail_Credit += detail.Detail_Total_Cost;
                            inventoryGL.GLDetail_Debit += detail.Detail_Total_Cost;

                        }
                    }
                    break;
                default:
                    break;
            }
            if (msg == "")
            {
                glMaster.GLDetails.Add(ExpenseGL);
                glMaster.GLDetails.Add(inventoryGL);
                db.GLMasters.Add(glMaster);
                GLMaster.GetBalanceForAccount(glMaster);
                GLMaster.GetBalanceForSubLedger(glMaster);
                db.SaveChanges();
                msg = Resources.validate.GLDone;
            }

            return msg;
        }
        /// <summary>
        ///  make agl for transfer to store and change status of it 
        /// </summary>
        /// <param name="master"></param>
        /// <returns>String</returns>
        public static String CreateInvoiceGLfortrans(TransferToProjectMaster master)
        {
            String msg = "";
            var db = new AccountingContainer();
            GLMaster glMaster = new GLMaster();
            glMaster.GL_Date = master.Trans_Date;
            glMaster.GL_Desc = master.Describtion;
            glMaster.Company_Id = master.Company_Id;
            glMaster.GLType_Id = "تحويل من مشروع الى مخزن";
            glMaster.Currency_Id = "الريال السعودى";
            glMaster.GLCode = master.Trans_Code;
            glMaster.GLState = TransactionState.Pending;
            glMaster.Auto = true;
            GLDetail inventoryGL = new GLDetail(),
             ExpenseGL = new GLDetail();
            //var BusId = db.BussinesUnits.FirstOrDefault().BU_Id;
            inventoryGL.Bu_Id = 0;
            ExpenseGL.Bu_Id = 0;
            ExpenseGL.Account_Id = db.ChartOfAccountings.Find(master.Account_Id).Account_Id;
            ExpenseGL.Account_Name = db.ChartOfAccountings.Find(master.Account_Id).Account_Name;

            var store = db.Stores.Find(master.Store_ID);
            var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).First();
            inventoryGL.Account_Id = ChartOfAccounInventory.Account_Id;
            inventoryGL.Account_Name = ChartOfAccounInventory.Account_Name;
            var detailList = getMaterialOCompoundProduct(master.TransferToProjectDetails.ToList(), master.Store_ID);
            inventoryGL.Asset_Id =
                ExpenseGL.Asset_Id = 0;

            inventoryGL.SubLedger_Id = 0;
            ExpenseGL.SubLedger_Id = master.Project_Id;
            switch (master.Invoice_State)
            {

                case TransactionState.Post:
                    var thisMasterProject = db.TransferToProjectDetails.Where(e => e.TransferToProjectMaster.Trans_Code.StartsWith("TTPRO") && e.TransferToProjectMaster.Project_Id == master.Project_Id && e.TransferToProjectMaster.Invoice_State == TransactionState.Post).ToList();

                    foreach (var item in master.TransferToProjectDetails)
                    {

                        var edited = thisMasterProject.Where(e => e.Product_ID == item.Product_ID && e.Product_Quantity >= item.Product_Quantity).ToList().FirstOrDefault();

                        if (edited == null)

                        {
                            var prod = db.Products.Find(item.Product_ID).Prod_Name;
                            master.Invoice_State = TransactionState.Pending;
                            db.SaveChanges();
                         
                            return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Thereisnotenoughproduct + prod  ;
                        }
                       
                            
                       


                    }
                    foreach (var item in master.TransferToProjectDetails)
                    {

                        var edited = thisMasterProject.Where(e => e.Product_ID == item.Product_ID && e.Product_Quantity >= item.Product_Quantity).ToList().FirstOrDefault();

                        

                        edited.Reverse_quantity -= item.Product_Quantity;
                        edited.Reverse_Total -= item.Product_Price * item.Product_Quantity;
                        db.Entry(edited).State = EntityState.Modified;
                        


                    }
                    glMaster.GLState = TransactionState.Post;
                    var gl = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode && a.GLState == glMaster.GLState).FirstOrDefault();
                    if (gl != null)
                    {
                        return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                    }

                  
                    db.ProductBatches.Load();
                    foreach (var detail in detailList)
                    {
                        //ProductBatches detailBatch = db.ProductBatches.Find(detail.Batch_ID);
                        ProductBatches detailBatch = db.ProductBatches.Where(p=>p.Product_ID == detail.Product_ID && p.Store_ID == master.Store_ID).FirstOrDefault();
                        if (detailBatch != null)
                        {
                            detailBatch.Product_Quantity += detail.Product_Quantity;
                            detailBatch.Batch_Cost += detail.Total_Amount;
                            detailBatch.Product_Cost = detailBatch.Batch_Cost / detailBatch.Product_Quantity;
                            db.Entry(detailBatch).State = EntityState.Modified;
                        }
                        else
                        {
                            ProductBatches newbatch = new ProductBatches();
                            newbatch.Batch_Date = DateTime.Now;
                            newbatch.Product_ID = detail.Product_ID;
                            newbatch.Store_ID = master.Store_ID;
                            newbatch.Product_Quantity = detail.Product_Quantity;
                            newbatch.Product_Cost = detail.Product_Price;
                            newbatch.Batch_Cost= detail.Product_Price* detail.Product_Quantity;
                            db.ProductBatches.Add(newbatch);



                        }
                        var storecard = new StoreCard();
                        storecard.CostPrice = detail.Detail_Total_Cost / detail.Product_Quantity;
                        storecard.Date = DateTime.Now;
                        storecard.Store_Id = master.Store_ID;
                        storecard.Quantity = detail.Product_Quantity;
                        storecard.SalePrice = 0;
                        storecard.Product_Id = detail.Product_ID;
                        storecard.IncreaseQuantity = true;
                        storecard.TypeOfDocument = "تحويل من مشروع لمخزن";
                        storecard.Document_Serial = master.Trans_Code;
                        storecard.TotalCost = detail.Detail_Total_Cost;
                        storecard.TotalAmount = 0;
                        storecard.Account_Id = 0;
                        storecard.Subledger_Id = Convert.ToInt32(master.SubLedger_ID);
                        Store.AddinginstorCard(storecard);
                        ExpenseGL.GLDetail_Credit += detail.Detail_Total_Cost;
                        inventoryGL.GLDetail_Debit += detail.Detail_Total_Cost;
                    }

                   
                
                    break;
                case TransactionState.Void:
                    glMaster.GLState = TransactionState.Void;
                    var glcheck = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode && a.GLState == glMaster.GLState).FirstOrDefault();
                    if (glcheck != null)
                    {
                        return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                    }
                    var errorMessage = new List<string> { };
                    foreach (var detail in detailList)
                    {
                        var thisproduct = db.Products.Find(detail.Product_ID);

                        ProductBatches detailBatch = db.ProductBatches.Where(c => c.Product_ID == detail.Product_ID && c.Store_ID == master.Store_ID).ToList().FirstOrDefault();
                        if (detailBatch.Store_ID != null)
                        {
                            if (detail.Product_Quantity > detailBatch.Product_Quantity)
                            {
                                var productName = db.Products.Find(detail.Product_ID).Prod_Name;
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

                       
                        foreach (var detail in detailList)
                        {
                            ProductBatches detailBatch = db.ProductBatches.Where(p => p.Product_ID == detail.Product_ID && p.Store_ID == master.Store_ID).FirstOrDefault();
                            detailBatch.Product_Quantity -= detail.Product_Quantity;
                            detailBatch.Batch_Cost -= detail.Total_Amount;
                            if (detailBatch.Product_Quantity == 0)
                            {
                                detailBatch.Product_Cost = 0;
                            }
                            else
                            {
                                detailBatch.Product_Cost = detailBatch.Batch_Cost / detailBatch.Product_Quantity;
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
                            storecard.SalePrice = 0;
                            storecard.Product_Id = detail.Product_ID;
                            storecard.IncreaseQuantity = false;
                            storecard.TypeOfDocument = "استبعاد تحويل من مشروع لمخزن";
                            storecard.Document_Serial = master.Trans_Code;
                            storecard.TotalCost = detail.Detail_Total_Cost;
                            storecard.TotalAmount = 0;
                            storecard.Account_Id = 0;
                            storecard.Subledger_Id = Convert.ToInt32(master.SubLedger_ID);
                            Store.AddinginstorCard(storecard);
                            ExpenseGL.GLDetail_Debit += detail.Detail_Total_Cost;
                            inventoryGL.GLDetail_Credit += detail.Detail_Total_Cost;
                        }
                        var thisMasterProjectVoid = db.TransferToProjectMasters.Where(e => e.Trans_Code.StartsWith("TTPRO") && e.Project_Id == master.Project_Id && e.HasTransfered == true).ToList().FirstOrDefault();
                        foreach (var item in master.TransferToProjectDetails)
                        {
                            var edited = thisMasterProjectVoid.TransferToProjectDetails.Where(e => e.Product_ID == item.Product_ID &&  e.Master_ID == thisMasterProjectVoid.Master_Id).ToList().FirstOrDefault();
                            if (item.Product_Quantity < 0)
                            {

                            }
                            else
                            {
                                edited.Product_Quantity += item.Product_Quantity;
                                edited.Detail_Total_Cost = item.Product_Price * edited.Product_Quantity;
                                db.Entry(edited).State = EntityState.Modified;
                            }
                            if (edited.Product_Quantity > 0)
                            {
                                thisMasterProjectVoid.HasTransfered = false;
                                db.Entry(thisMasterProjectVoid).State = EntityState.Modified;
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
                glMaster.GLDetails.Add(ExpenseGL);
                glMaster.GLDetails.Add(inventoryGL);
                db.GLMasters.Add(glMaster);
                GLMaster.GetBalanceForAccount(glMaster);
                GLMaster.GetBalanceForSubLedger(glMaster);

                db.SaveChanges();
             
                db.Dispose();
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
        /// <summary>
        /// make atransaction batch for create operation
        /// </summary>
        /// <param name="master"></param>
        /// <param name="UserId"></param>
        public static List<TransferToProjectDetail> getMaterialOCompoundProduct(List<TransferToProjectDetail> Details, int Store_Id)
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
                List<TransferToProjectDetail> Details2 = new List<TransferToProjectDetail>();

                foreach (var item in ProList)
                {
                    var SubProductList = ProductAll.Where(a => a.Prod_ID == item.Product_ID).FirstOrDefault().ProductCompounds;
                    foreach (var subProduct in SubProductList)
                    {
                        var dublicate = Details.Where(a => a.Product_ID == subProduct.Product_Id).FirstOrDefault();
                        var batch = db.ProductBatches.Where(a => a.Product_ID == subProduct.Product_Id && a.Store_ID == Store_Id).FirstOrDefault();
                        if (dublicate == null)
                        {
                            var Detailnew = new TransferToProjectDetail();
                            Detailnew.Product_ID = subProduct.Product_Id;
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


                            var Detailnew = new TransferToProjectDetail();
                            Detailnew.Batch_ID = batch.Batch_ID;
                            Detailnew.Product_ID = subProduct.Product_Id;
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
                        var Delete = Details.Where(a => a.Product_ID == subProduct.Master_Product_Id).FirstOrDefault();

                        Details.Remove(Delete);

                    }





                }

            }

            return Details;
        }
        public static void CreateTransactionBatch(TransferToProjectMaster master , String UserId)
        {
            var trans = new TransactionBatch();
            trans.Description = master.Describtion;
            trans.Date = master.Trans_Date;
            trans.OperationName = "تحويل من مخزن الى مشروع";
            // TODO: Edit UserId
            trans.UserId = UserId;
            trans.OperationId = master.Master_Id;
            trans.Operation_Code = master.Trans_Code;
            TransactionBatch.AddNewTransaction(trans);
        }
        /// <summary>
        /// fill for Edit Transfer by putting the data in view 
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns>TransferToProjectMasterVM</returns>
        public static TransferToProjectMasterVM fillForEditingTransfer(int invoice)
        {
            var db = new AccountingContainer();
            var Transfer = db.TransferToProjectMasters.Find(invoice);
            var TransferVM = new TransferToProjectMasterVM();
            String startCodeExpenses = "05";
            var comp = db.Companies.Where(c => c.Comp_Id == Transfer.Company_Id);
            if (comp.Any())
            {
                TransferVM.Company_Id = comp.FirstOrDefault().Comp_Id;
            }
            var stor = db.Stores.Where(Cu => Cu.Store_ID == Transfer.Store_ID);
            if (stor.Any())
            {
                TransferVM.Store_Id = stor.FirstOrDefault().Store_ID;
            }
            var account = db.ChartOfAccountings.Where(Cu => Cu.Account_Id == Transfer.Account_Id);
            if (account.Any())
            {
                TransferVM.Account_Id = account.FirstOrDefault().Account_Id;
            }
       

            var project = db.SubLedgers.Where(Cu => Cu.SubLed_Id == Transfer.Project_Id);
            if (project.Any())
            {
                TransferVM.Project_Id = project.FirstOrDefault().SubLed_Id;
            }
            TransferVM.Branch_Id = Transfer.Branch_Id;
            TransferVM.Department_ID = Transfer.Department_ID;
            TransferVM.BussinesUnit_ID = Transfer.BussinesUnit_ID;
            TransferVM.SubLedger_ID = Transfer.SubLedger_ID;
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() ==0 && a.isActive != false);
            
            TransferVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            TransferVM.MyStoresList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            TransferVM.AccountList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(startCodeExpenses)), " Account_Id", "Account_Name");
            TransferVM.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            TransferVM.DepList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            TransferVM.BUList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            TransferVM.SubList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            var ProjectName = db.Projects.Where(a => a.Project_State != ProjectState.Finished).Select(a => a.Project_Name);
            var ProList = db.SubLedgers.Where(s => ProjectName.Contains(s.SubLed_Name)).ToList();
            TransferVM.ProjList = new SelectList(ProList.Where(a => a.SubLed_Type == "مشروع" && a.isActive != false), "SubLed_Id", "SubLed_Name");

            TransferVM.Master_Id = Transfer.Master_Id;
            TransferVM.Trans_Code = Transfer.Trans_Code;
            TransferVM.Trans_Date =Transfer.Trans_Date.Day+"-"+Transfer.Trans_Date.Month+"-"+Transfer.Trans_Date.Year;
            TransferVM.Invoice_State = Transfer.Invoice_State;
            TransferVM.Describtion = Transfer.Describtion;
            TransferVM.Reference = Transfer.Reference;
            var TransTemp = new TransferToProjectTemp();
            TransTemp.Account_Id = Transfer.Account_Id;
            TransTemp.Branch_Id = Transfer.Branch_Id;
            TransTemp.Company_Id = Transfer.Company_Id;
            TransTemp.Describtion = Transfer.Describtion;
            TransTemp.Invoice_State = Transfer.Invoice_State;
            TransTemp.Project_Id = Transfer.Project_Id;
            TransTemp.Store_Id = Transfer.Store_ID;
            TransTemp.Trans_Code = Transfer.Trans_Code;
            TransTemp.Trans_Date = Transfer.Trans_Date;
            
            var ThereORN = db.TransferToProjectTemps.Find(Transfer.Master_Id);
            if (ThereORN != null)
            {

            }
            else
            {
                TransTemp.Master_Id = Transfer.Master_Id;

                db.TransferToProjectTemps.Add(TransTemp);
            }
            db.SaveChanges();
            var PrepareDetail = GetAllDetailForEditing(invoice);
            return TransferVM;


        }
        /// <summary>
        /// get all detail by master id
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns>List<TransferToProjectDetail></returns>
        public static List<TransferToProjectDetail> getAlldetailInInvoice(int masterId)
        {
            var db = new AccountingContainer();
            var detail = db.TransferToProjectDetails.Where(a => a.Master_ID == masterId).ToList();

            return detail;


        }
        /// <summary>
        /// get invoice transfer by its master id 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>TransferToProjectMaster</returns>
        public static TransferToProjectMaster GetTransferByID(int ID)
        {
            var db = new AccountingContainer();
            var Trans = db.TransferToProjectMasters.Where(c => c.Master_Id == ID);
            var newTrans = new TransferToProjectMaster();

            if (Trans != null)
            {
                newTrans = Trans.FirstOrDefault();
                return newTrans;
            }
            return null;
        }
        /// <summary>
        /// prepare detail for adding to list 
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns>TransferToProjectDetailVM</returns>
        public static TransferToProjectDetailVM PrepareTransDetail2(int masterId)
        {

            var db = new AccountingContainer();
            var TransDVM = new TransferToProjectDetailVM();

            var thisdetails = GetAllTempTransferDetailsDetails(masterId).ToList().FirstOrDefault();

            var product = thisdetails.Product_ID;


            TransDVM.Product_Quantity = thisdetails.Product_Quantity;
            //   invoiceMasterDVM.SelectedBtachtId = thisdetails.Product_ID;
            TransDVM.Details_Id = thisdetails.Details_Id;
            TransDVM.Product_Price = thisdetails.Product_Price;
            TransDVM.Product_Tax = thisdetails.Product_Tax;
            TransDVM.Detail_Total_Tax = thisdetails.Detail_Total_Tax;
            TransDVM.Master_ID = masterId;
            var batchs = db.ProductBatches.Where(batch => batch.Product_ID == thisdetails.Product_ID&& batch.Product.IsActive != false).ToList();
            List<ProductBatchSelection> _ProductBatchSelection = new List<ProductBatchSelection>();
            foreach (var item in batchs)
            {
                var productBatchSelection = new ProductBatchSelection();

                productBatchSelection.Batch_Id = item.Batch_ID;
                productBatchSelection.ProductCost = item.Product_Cost;
                productBatchSelection.ProductName = item.Product.Prod_Name;
                productBatchSelection.Product_Code = item.Product.Bar_Code;
                productBatchSelection.Product_Id = item.Product_ID;
                _ProductBatchSelection.Add(productBatchSelection);

            }
            List<object> newListproout = new List<object>();

            foreach (var member in _ProductBatchSelection)
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
        /// <summary>
        /// get all project transfer detail by its master id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>List<TransferToProjectDetail></returns>
        public static List<TransferToProjectDetail> GetAllTempTransferDetailsDetails(int masterVmId)
        {
            var db = new AccountingContainer();
            var DetailsTemp = db.TransferToProjectDetails.Where(invoice => invoice.Details_Id == masterVmId).ToList();
            return DetailsTemp;
        }
        /// <summary>
        /// get all project transfer detail by its master id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>List<TransferToProjectDetail></returns>
        public static List<TransferToProjectDetail> GetAllTempTransferDetails2(int masterVmId)
        {
            var db = new AccountingContainer();
            var Details = db.TransferToProjectDetails.Where(invoice => invoice.Master_ID == masterVmId).ToList();
            return Details;
        }
        /// <summary>
        /// get all project transfer detail by its master id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>List<TransferToProjectDetailTemp></returns>

        public static List<TransferToProjectDetailTemp> GetAllTempTransDetailsresult2(int masterVmId)
        {
            var db = new AccountingContainer();
            var TransTemp = db.TransferToProjectTemps.Find(masterVmId);
            return TransTemp.TransferToProjectDetailTemps.ToList();
        }
        /// <summary>
        /// add to list by detail vm
        /// </summary>
        /// <param name="Detail"></param>
        /// <returns>string</returns>
        public static string AddTransDetailsTemp(TransferToProjectDetailVM Detail)
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
            var TransMasterTemp = db.TransferToProjectTemps.Find(Detail.Master_ID);
            var batch = db.ProductBatches.Find(Detail.Batch_ID);
            var TransDetailsTemp = new TransferToProjectDetailTemp();
            TransDetailsTemp.Batch_ID = batch.Batch_ID;
            TransDetailsTemp.Product_Id = Detail.Batch_ID.Value;
            TransDetailsTemp.Product_Price = Detail.Product_Price;
            TransDetailsTemp.Product_Quantity = Detail.Product_Quantity;               
            TransDetailsTemp.Detail_Total_Cost = batch.Product_Cost * Detail.Product_Quantity;        
            TransMasterTemp.TransferToProjectDetailTemps.Add(TransDetailsTemp);
            db.Entry(TransMasterTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "Details Has been added";
        }
        /// <summary>
        /// get all project transfer detail by its master id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>List<TransferToProjectDetailTemp></returns>
        public static List<TransferToProjectDetailTemp> GetAllDetailForEditing(int MasterId)
        {
            var db = new AccountingContainer();
            var detailTemp = db.TransferToProjectTemps.Find(MasterId);
            db.TransferToProjectDetailTemps.RemoveRange(detailTemp.TransferToProjectDetailTemps);
            db.SaveChanges();
            var invoice = db.TransferToProjectMasters.Find(MasterId);
            List<TransferToProjectDetailTemp> Lista = new List<TransferToProjectDetailTemp> { };
            foreach (var item in invoice.TransferToProjectDetails)
            {
                var invoiceTemp = new TransferToProjectDetailTemp();
                invoiceTemp.Batch_ID = item.Batch_ID.Value;
                invoiceTemp.Detail_Total_Tax = item.Detail_Total_Tax;
                invoiceTemp.Product_Id = item.Product_ID;
                invoiceTemp.Product_Price = item.Product_Price;
                invoiceTemp.Product_Quantity = item.Product_Quantity;
                invoiceTemp.Product_Tax = item.Product_Tax;
                invoiceTemp.Detail_Total_Cost = item.Detail_Total_Cost;
                invoiceTemp.Detail_Total_Tax = item.Detail_Total_Tax;
                
                invoiceTemp.Total_Amount = item.Total_Amount;
                invoiceTemp.Master_ID= MasterId;
                Lista.Add(invoiceTemp);

            }
            db.TransferToProjectDetailTemps.AddRange(Lista);

            db.SaveChanges();

            return detailTemp.TransferToProjectDetailTemps.ToList();
        }
        /// <summary>
        /// post the editing in db and save changes transfer to project
        /// </summary>
        /// <param name="masterVM"></param>
        /// <returns> Tuple< string,int> </returns>
        public static Tuple< string,int> editinvoice(TransferToProjectMasterVM masterVM)
        {
            var db = new AccountingContainer();
            var Trans = db.TransferToProjectMasters.Find(masterVM.Master_Id);
            var Transtemp = db.TransferToProjectTemps.Find(masterVM.Master_Id);
            Trans.Master_Id = masterVM.Master_Id;
            Trans.Account_Id = masterVM.Account_Id;
          
            Trans.Company_Id = masterVM.Company_Id;
            Trans.Describtion = masterVM.Describtion;
            Trans.Invoice_State = TransactionState.Pending;
            Trans.Project_Id = masterVM.Project_Id;
            Trans.Reference = masterVM.Reference;
            Trans.Store_ID = masterVM.Store_Id;
            var comp = db.Companies.Find(masterVM.Company_Id);
            if (comp != null)
            {
                var date = Convert.ToDateTime(masterVM.Trans_Date);
                if (date < comp.StartFinancialDate)
                {
                    Trans.Trans_Date = DateTime.Now;
                }
                else
                {
                    Trans.Trans_Date = Convert.ToDateTime(masterVM.Trans_Date);
                }
            }
            else
            {
                Trans.Trans_Date = DateTime.Now;
            }
           

         

            decimal k = 0;
            if (masterVM.Files.FirstOrDefault() != null)
            {
                foreach (var file in masterVM.Files)
                {
                    var filedetails = new TransferToprojectFile1();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.FileContent = data;
                    filedetails.FileType = file.ContentType;
                    if (data.Length < 1000000)
                    {
                        Trans.TransferToprojectFile1.Add(filedetails);
                    }
                   

                }
            }
            var oldDetail = db.TransferToProjectDetails.Where(a => a.Master_ID == Trans.Master_Id).ToList();
            var detailListTemp = db.TransferToProjectDetailTemps.Where(e => e.Master_ID == Transtemp.Master_Id).ToList();
            List<TransferToProjectDetailTemp> DeleteList = new List<TransferToProjectDetailTemp>();
            if (detailListTemp.Count() == 0)
            {
                return Tuple.Create("تفاصيل التحويل غير مكتملة", 0);
            }
            foreach (var detail in detailListTemp)
            {
                var item = new TransferToProjectDetail();
                item.Product_ID = detail.Product_Id;
             
   
                item.Product_Price = detail.Product_Price;
                item.Product_Quantity = detail.Product_Quantity;                          
             
                item.Product_ID = detail.Product_Id;
                
                // Calculate the Total_Cost
                item.Detail_Total_Cost = item.Product_Price * item.Product_Quantity;
                item.Batch_ID = detail.Batch_ID;
                item.Reverse_quantity = detail.Product_Quantity;
                item.Reverse_Total= detail.Product_Price * detail.Product_Quantity;
                k += item.Detail_Total_Cost;
                Trans.Total_Cost =k;
                Trans.TransferToProjectDetails.Add(item);
                DeleteList.Add(detail);
                
            }
            db.TransferToProjectDetailTemps.RemoveRange(DeleteList);
            db.Entry(Trans).State = EntityState.Modified;
      
            db.TransferToProjectTemps.Remove(Transtemp);
            db.TransferToProjectDetails.RemoveRange(oldDetail);
            db.SaveChanges();

            return Tuple.Create( "تم التعديل بنجاح", Trans.Master_Id);
        }
        /// <summary>
        /// post the editing in db and save changes transfer to store
        /// </summary>
        /// <param name="masterVM"></param>
        /// <returns> Tuple< string,int> </returns>
        public static Tuple<string,int> editinvoicetostore(TransferToProjectMasterVM masterVM)
        {
            var db = new AccountingContainer();
            var Trans = db.TransferToProjectMasters.Find(masterVM.Master_Id);
            var Transtemp = db.TransferToProjectTemps.Find(masterVM.Master_Id);
            Trans.Master_Id = masterVM.Master_Id;
            Trans.Account_Id = masterVM.Account_Id;
            //Trans.Branch_Id = masterVM.Branch_Id.Value;
            Trans.Company_Id = masterVM.Company_Id;
            Trans.Describtion = masterVM.Describtion;
            Trans.Invoice_State = TransactionState.Pending;
            Trans.Project_Id = masterVM.Project_Id;
            Trans.Reference = masterVM.Reference;
            Trans.Store_ID = masterVM.Store_Id;


            var comp = db.Companies.Find(masterVM.Company_Id);
            if (comp != null)
            {
                var date = Convert.ToDateTime(masterVM.Trans_Date);
                if (date < comp.StartFinancialDate)
                {
                    Trans.Trans_Date = DateTime.Now;
                }
                else
                {
                    Trans.Trans_Date = Convert.ToDateTime(masterVM.Trans_Date);
                }
            }
            else
            {
                Trans.Trans_Date = DateTime.Now;
            }
          
            decimal k = 0;
            var oldDetail = db.TransferToProjectDetails.Where(a => a.Master_ID == Trans.Master_Id).ToList();
            var detailListTemp = db.TransferToProjectDetailTemps.Where(e => e.Master_ID == Transtemp.Master_Id).ToList();
            List<TransferToProjectDetailTemp> DeleteList = new List<TransferToProjectDetailTemp>();
            if (masterVM.Files.FirstOrDefault() != null)
            {
                foreach (var file in masterVM.Files)
                {
                    var filedetails = new TransferToprojectFile1();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.FileContent = data;
                    filedetails.FileType = file.ContentType;
                    if (data.Length < 1000000)
                    {
                        Trans.TransferToprojectFile1.Add(filedetails);
                    }
                    
                    

                }
            }
            if (detailListTemp.Count == 0)
            {
                return Tuple.Create("لا يوجد تفاصيل للفاتورة", 0);
            }
            foreach (var detail in detailListTemp)
            {
                var item = new TransferToProjectDetail();
                item.Product_ID = detail.Product_Id;
                var productPrice = db.TransferToProjectDetails.Where(a => a.Master_ID == Trans.Master_Id&&a.Product_ID==detail.Product_Id).FirstOrDefault();
                var thisporject = db.TransferToProjectMasters.Where(e => e.Project_Id == masterVM.Project_Id && e.Trans_Code.Contains("TTPRO") && e.Store_ID == masterVM.Store_Id).ToList().FirstOrDefault();
                decimal thispojectQuant = 0;
                foreach (var ttproDetail in thisporject.TransferToProjectDetails.Where(e=>e.Product_ID==item.Product_ID).ToList())
                {

                    thispojectQuant += ttproDetail.Product_Quantity;
                }
                item.Product_Price = productPrice.Product_Price;
                item.Product_Quantity = detail.Product_Quantity;
               //  ProductBatches detailBatch = db.ProductBatches.Find(detail.Batch_ID);
                item.Product_ID = productPrice.Product_ID;
                var productName = db.Products.Find(productPrice.Product_ID).Prod_Name;
                if (detail.Product_Quantity > thispojectQuant)
                    return Tuple.Create("لا يوجد كمية كافية من المنتج", 0);
                // Calculate the Total_Cost
                item.Detail_Total_Cost = item.Product_Price * item.Product_Quantity;
                item.Batch_ID = detail.Batch_ID;
                k += item.Detail_Total_Cost;
                Trans.Total_Cost = k;
                Trans.TransferToProjectDetails.Add(item);
                DeleteList.Add(detail);
                //db.TransferToProjectDetailTemps.Remove(detail);
                //db.SaveChanges();
            }
            db.TransferToProjectDetailTemps.RemoveRange(DeleteList);
            db.Entry(Trans).State = EntityState.Modified;
            db.TransferToProjectTemps.Remove(Transtemp);
            db.TransferToProjectDetails.RemoveRange(oldDetail);
            db.SaveChanges();

            return Tuple.Create("تم حفظ التعديل بنجاح", Trans.Master_Id);
        }
        /// <summary>
        /// get file by its id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>TransferToprojectFile1</returns>
        public static TransferToprojectFile1 getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.TransferToprojectFile1.Find(fileId);
            return file;
        }
        /// <summary>
        /// get all file by its master id
        /// </summary>
        /// <param name="invoiceMasterId"></param>
        /// <returns>List<TransferToprojectFile1></returns>
        public static List<TransferToprojectFile1> GetAllFiles(int invoiceMasterId)
        {
            var db = new AccountingContainer();
            var files = db.TransferToprojectFile1.Where(emp => emp.TransferToProjectMasterMaster_Id == invoiceMasterId).ToList();
            return files;
        }
        /// <summary>
        /// get temp by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TransfareFromPorjectToStoretemp</returns>
        public static TransfareFromPorjectToStoretemp gettempbyID(int id)
        {
            var db = new AccountingContainer();
            var detail = db.TransfareFromPorjectToStoretemps.Find(id);
            return detail;
        }
        /// <summary>
        /// get all temp belong to master by master id
        /// </summary>
        /// <returns> List<TransfareFromPorjectToStoretemp></returns>
        public static List<TransfareFromPorjectToStoretemp> getAllSavedData()
        {
            var db = new AccountingContainer();
            var alllist = db.TransfareFromPorjectToStoretemps.ToList();
            return alllist;
        }
        /// <summary>
        /// get list of Project dropdownList
        /// </summary>
        /// <returns>TransferToProjectMasterVM</returns>
        public static TransferToProjectMasterVM GetListOFProjects()
        {
            var db = new AccountingContainer();
            TransferToProjectMasterVM TVM = new TransferToProjectMasterVM();
            TVM.ProjList = new SelectList(db.TransferToProjectMasters, "Project_Id", "Describtion");
            return TVM;

        }
        /// <summary>
        /// get all temp belong to master by master id
        /// </summary>
        /// <returns> List<TransfareFromPorjectToStoretemp></returns>
        public static List<TransfareFromPorjectToStoretemp> GetListOFProjectsintemp(int masterID)
        {
            var db = new AccountingContainer();
            var thisList = db.TransfareFromPorjectToStoretemps.Where(e => e.Master_ID == masterID).ToList();
            return thisList;

        }
        /// <summary>
        /// get detail of project transfer by its id
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns>List<TransferToProjectDetail></returns>
        public static List<TransferToProjectDetail> getListProjectTransDetail(int projectID)
        {
            var db = new AccountingContainer();
            List<TransferToProjectDetail> listDetails = new List<TransferToProjectDetail> { };
            var thisTrans = db.TransferToProjectDetails.Where(e => e.TransferToProjectMaster.Project_Id == projectID && e.TransferToProjectMaster.Trans_Code.Contains("TTPRO-")&& e.TransferToProjectMaster.Invoice_State==TransactionState.Post&&e.Reverse_quantity>0).ToList();

            
            return thisTrans;
        }
        public static TransferToProjectDetailVM PrepareTransDetailtoBatch(int DetailID)
        {
            var db = new AccountingContainer();
            var thisTrans = db.TransferToProjectDetails.Where(e => e.Details_Id == DetailID).ToList().FirstOrDefault();
            TransferToProjectDetailVM TVM = new TransferToProjectDetailVM();
            TVM.Details_Id = thisTrans.Details_Id;
            TVM.Batch_ID = thisTrans.Batch_ID;
            TVM.Product_ID = thisTrans.Product_ID;
            TVM.Product_Name = db.Products.Find(TVM.Product_ID).Prod_Name;
            TVM.Product_Quantity = thisTrans.Reverse_quantity.Value;
            TVM.Product_Tax = thisTrans.Product_Tax;
            TVM.Product_Price = thisTrans.Product_Price;
            TVM.Total_Amount = TVM.Product_Price * TVM.Product_Quantity;
            TVM.Master_ID = thisTrans.Master_ID;

            return TVM;
        }
        /// <summary>
        /// add to temp and check validation
        /// </summary>
        /// <param name="TVM"></param>
        /// <returns></returns>
        public static string SaveDataToPermenantTemp(TransferToProjectDetailVM TVM)
        {
            var db = new AccountingContainer();
            TransfareFromPorjectToStoretemp TFTemp = new TransfareFromPorjectToStoretemp();
           

            if (TVM.Product_Quantity == null || TVM.Product_Quantity == 0)
            {
                return  Resources.InvoiceMasterAddNewInvoice.Quantity;
            }
            var detailquantity = db.TransferToProjectDetails.Find(TVM.Details_Id).Reverse_quantity;
            if (detailquantity < TVM.Product_Quantity)
            {
                return Resources.validate.quantitynotSuitable;
            }

            TFTemp.Master_ID = TVM.Master_ID;
            TFTemp.ProductID = TVM.Product_ID;
            TFTemp.Batch_ID = Convert.ToInt32(TVM.Batch_ID);
            TFTemp.Total_Amount = TVM.Total_Amount;
            TFTemp.Product_Price = TVM.Product_Price;
            TFTemp.Product_Quantity = TVM.Product_Quantity;
            TFTemp.Detail_ID = TVM.Details_Id;
         
            TFTemp.State = TransactionState.Pending;
            db.TransfareFromPorjectToStoretemps.Add(TFTemp);
            db.SaveChanges();                   
            return Resources.validate.SuccessfullAdd;
        }
        /// <summary>
        /// add new invoice of transfer to store
        /// </summary>
        /// <param name="TMS"></param>
        /// <param name="UserId"></param>
        /// <returns>Tuple< string,int></returns>
        public static Tuple< string,int> saveTransfaretoprogectMaster(TransferToProjectMasterVM TMS , String UserId)
        {
            
               var db = new AccountingContainer();
            var newmaster = new TransferToProjectMaster();
            if (TMS.Project_Id == null || TMS.Project_Id == 0)
            {
                return Tuple.Create( "من فضلك اختر المشروع",0);
            }
            if (TMS.Files.FirstOrDefault() != null)
            {
                foreach (var file in TMS.Files)
                {
                    var filedetails = new TransferToprojectFile1();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.FileContent = data;
                    filedetails.FileType = file.ContentType;
                    if (data.Length < 1000000)
                    {
                        newmaster.TransferToprojectFile1.Add(filedetails);
                    }
                   
                    
                }
            }
            newmaster.Trans_Code = TMS.Trans_Code;
            var lastCode = db.TransferToProjectMasters.OrderByDescending(c => c.Master_Id).Where(e => e.Trans_Code.StartsWith("TTSTO") == true).FirstOrDefault();
            if (lastCode == null)
            {
                newmaster.Trans_Code = "TTSTO-001";
            }
            else
            {
                var lastSerial = db.TransferToProjectMasters.Where(a=>a.Trans_Code.StartsWith("TTSTO-")).OrderByDescending(c => c.Master_Id).FirstOrDefault();
                //using string substring method to get the number of the last inserted
                newmaster.Trans_Code = "TTSTO-" + (Convert.ToInt32(lastSerial.Trans_Code.Substring(6, lastSerial.Trans_Code.Length - 6)) + 1).ToString("D3");
            }

            newmaster.Describtion = TMS.Describtion;
            newmaster.Project_Id = TMS.Project_Id;

           

            newmaster.Invoice_State = TransactionState.Pending;
            newmaster.Account_Id = TMS.Account_Id;
            if (TMS.Branch_Id == null || TMS.Branch_Id == 0)
            {

            }
            else
            {
                newmaster.Branch_Id = TMS.Branch_Id.Value;

            }
            newmaster.Reference = TMS.Reference;
            newmaster.Company_Id = TMS.Company_Id;
            newmaster.Store_ID = TMS.Store_Id;
            //newmaster.Trans_Date = Convert.ToDateTime(TMS.Trans_Date);
            var comp = db.Companies.Find(TMS.Company_Id);
            if (comp != null)
            {
                var date = Convert.ToDateTime(TMS.Trans_Date);
                if (date < comp.StartFinancialDate)
                {
                    newmaster.Trans_Date = DateTime.Now;
                }
                else
                {
                    newmaster.Trans_Date = Convert.ToDateTime(TMS.Trans_Date);
                }
            }
            else
            {
                newmaster.Trans_Date = DateTime.Now;
            }

            var thismaster = db.TransferToProjectMasters.Where(e => e.Trans_Code == TMS.Trans_Code).ToList().LastOrDefault();
            var thispermenttemp = db.TransfareFromPorjectToStoretemps.Where(t => t.Master_ID == TMS.Master_Id).ToList();
            decimal total = 0;
            foreach (var item in thispermenttemp)
            {
                
                var newdetail = new TransferToProjectDetail();
                newdetail.Product_Quantity = item.Product_Quantity;
                newdetail.Product_Price = (decimal)item.Product_Price;
                newdetail.Total_Amount = (decimal)(item.Product_Price * item.Product_Quantity);
                //var thisporduct = db.Products.Find(item.ProductID);
                var oldTransferDetail = db.TransferToProjectDetails.Where(t => t.Product_ID == item.ProductID).FirstOrDefault();
                newdetail.Detail_Total_Cost = (decimal)oldTransferDetail.Product_Price * item.Product_Quantity;
                newdetail.Batch_ID = item.Batch_ID;
                newdetail.Product_ID = item.ProductID;

                newmaster.TransferToProjectDetails.Add(newdetail);
                total += (decimal)item.Total_Amount;
                
                
               
            }
            newmaster.Total_Amount = total;
            db.TransferToProjectMasters.Add(newmaster);
            db.SaveChanges();
            var trans = new TransactionBatch();
            trans.Description = newmaster.Describtion;
            trans.Date = newmaster.Trans_Date;
            trans.OperationName = "تحويل من مشروع الى مخزن";
            // TODO: Edit UserId
            trans.UserId = UserId;
            trans.OperationId = newmaster.Master_Id;
            trans.Operation_Code = newmaster.Trans_Code;
            TransactionBatch.AddNewTransaction(trans);
            db.Dispose();
            return Tuple.Create("تم التحويل", newmaster.Master_Id);
        }
        /// <summary>
        /// delete from temp by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteFromTempofPermenant(int id)
        {
            var db = new AccountingContainer();
            var thisdata = db.TransferToProjectDetailTemps.Find(id);
            db.TransferToProjectDetailTemps.Remove(thisdata);
            db.SaveChanges();
            db.Dispose();
            return Resources.validate.DeleteSuccessful;
        }
        /// <summary>
        /// delete invoice by using its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>string</returns>
        public static string DeleteFromTempProjectToStore(int id)
        {
            var db = new AccountingContainer();
            var thisdata = db.TransfareFromPorjectToStoretemps.Find(id);
            db.TransfareFromPorjectToStoretemps.Remove(thisdata); 
            db.SaveChanges();
            db.Dispose();
            return Resources.validate.DeleteSuccessful;
        }
        /// <summary>
        /// make gl for transer to store
        /// </summary>
        /// <param name="TFPSs"></param>
        /// <returns>bool</returns>
        public static bool UpdateInvoicePermtem(TransfareFromPorjectToStoretemp TFPSs)
        {
            var db = new AccountingContainer();

            TransfareFromPorjectToStoretemp salesmaster = db.TransfareFromPorjectToStoretemps.Where(e => e.Detail_ID == TFPSs.Detail_ID).FirstOrDefault();
            // Edit master invoice state to the new state
            salesmaster.State = TFPSs.State;
            db.Entry(salesmaster).State = EntityState.Modified;

            // Create GL for the invoice 
            CreateInvoiceGLStore(salesmaster);
            db.SaveChanges();
            db.Dispose();
            return true;
        }
        /// <summary>
        /// make gl for transfer to store
        /// </summary>
        /// <param name="TFPSs"></param>
        /// <returns></returns>
        public static int CreateInvoiceGLStore(TransfareFromPorjectToStoretemp TFPSs)
        {
            var db = new AccountingContainer();
            var TFPS = db.TransfareFromPorjectToStoretemps.Where(s => s.Detail_ID == TFPSs.Detail_ID).ToList().FirstOrDefault();
            var thismaster = db.TransferToProjectMasters.Where(e => e.Master_Id == TFPS.Master_ID).ToList().FirstOrDefault();
            GLMaster glMaster = new GLMaster();
            glMaster.GL_Date = DateTime.Now;
            glMaster.GL_Desc = "تحويل من مشروع الي مخزن";
            glMaster.Company_Id = thismaster.Company_Id;
            glMaster.GLType_Id = "تحويل من مشروع الي مخزن";
            glMaster.Currency_Id = "الريال السعودى";
            glMaster.GLCode = thismaster.Trans_Code;
            glMaster.GLState = TransactionState.Pending;
            glMaster.Auto = true;
            GLDetail inventoryGL = new GLDetail(),
            ExpenseGL = new GLDetail();
            //var BusId = db.BussinesUnits.FirstOrDefault().BU_Id;        
            var store = db.Stores.Find(thismaster.Store_ID);
            var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).FirstOrDefault();
            inventoryGL.Account_Id = ChartOfAccounInventory.Account_Id;
            inventoryGL.Account_Name = ChartOfAccounInventory.Account_Name;
            inventoryGL.Bu_Id = 0;
            ExpenseGL.Bu_Id = 0;
            ExpenseGL.Account_Id = db.ChartOfAccountings.Find(thismaster.Account_Id).Account_Id;
            ExpenseGL.Account_Name = db.ChartOfAccountings.Find(thismaster.Account_Id).Account_Name;
            inventoryGL.Asset_Id =
            ExpenseGL.Asset_Id = 0;
            inventoryGL.SubLedger_Id = 0;
            ExpenseGL.SubLedger_Id = thismaster.Project_Id;
            switch (TFPS.State)
            {
                case TransactionState.Post:
                    glMaster.GLState = TransactionState.Post;
                    var gl = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode && a.GLState == glMaster.GLState).FirstOrDefault();
                    if (gl != null)
                    {
                        return 5;
                    }

                    ExpenseGL. GLDetail_Credit = (decimal)TFPS.Total_Amount;
                    inventoryGL.GLDetail_Debit = (decimal)TFPS.Total_Amount;
                    db.ProductBatches.Load();
                    ProductBatches detailBatch = db.ProductBatches.Find(TFPS.Batch_ID);
                    detailBatch.Product_Quantity += TFPS.Product_Quantity;

                    detailBatch.Batch_Cost += TFPS.Total_Amount.Value;
                    detailBatch.Product_Cost = TFPS.Total_Amount.Value/ TFPS.Product_Quantity;


                    db.Entry(detailBatch).State = EntityState.Modified;
                    db.SaveChanges();
                    break;
                case TransactionState.Void:
                    glMaster.GLState = TransactionState.Void;
                    var glCheck = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode && a.GLState == glMaster.GLState).FirstOrDefault();
                    if (glCheck != null)
                    {
                        return 5;
                    }
                    ExpenseGL.GLDetail_Debit = (decimal)TFPS.Total_Amount;
                    inventoryGL.GLDetail_Credit = (decimal)TFPS.Total_Amount;
                    ProductBatches detailBatchs = db.ProductBatches.Find(TFPS.Batch_ID);
                    detailBatchs.Product_Quantity -= TFPS.Product_Quantity;
                    detailBatchs.Batch_Cost -= TFPS.Total_Amount.Value;
                    detailBatchs.Product_Cost = TFPS.Total_Amount.Value / TFPS.Product_Quantity;
                    db.Entry(detailBatchs).State = EntityState.Modified;
                    db.SaveChanges();
                    break;
                default:
                    break;
            }
            glMaster.GLDetails.Add(ExpenseGL);
            glMaster.GLDetails.Add(inventoryGL);
            db.GLMasters.Add(glMaster);
            GLMaster.GetBalanceForAccount(glMaster);
            GLMaster.GetBalanceForSubLedger(glMaster);
            db.SaveChanges();
            return 9;
        }

        public static TransferToProjectDetailTemp DeleteInvoiceFromTemp(int purch)
        {
            var db = new AccountingContainer();
            TransferToProjectDetailTemp vm = db.TransferToProjectDetailTemps.Find(purch);
            db.TransferToProjectDetailTemps.Remove(vm);
            db.SaveChanges();
            db.Dispose();
            return vm;
        }

        /// <summary>
        /// prepare detail for adding to list
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns>TransferToProjectDetailVM</returns>

        public static TransferToProjectDetailVM PrepareTransDetailToStore(int masterId)
        {

            var db = new AccountingContainer();
            var TransDVM = new TransferToProjectDetailVM();

            var thisdetails = GetAllTempTransferDetailsDetails(masterId).ToList().FirstOrDefault();

            var product = thisdetails.Product_ID;


            TransDVM.Product_Quantity = thisdetails.Product_Quantity;
            //   invoiceMasterDVM.SelectedBtachtId = thisdetails.Product_ID;
            TransDVM.Details_Id = thisdetails.Details_Id;
            TransDVM.Product_Price = thisdetails.Product_Price;
            TransDVM.Product_Tax = thisdetails.Product_Tax;
            TransDVM.Detail_Total_Tax = thisdetails.Detail_Total_Tax;
            TransDVM.Master_ID = masterId;
            var batchs = db.ProductBatches.Where(batch => batch.Product_ID == thisdetails.Product_ID).ToList();
            List<ProductBatchSelection> _ProductBatchSelection = new List<ProductBatchSelection>();
            foreach (var item in batchs)
            {
                var productBatchSelection = new ProductBatchSelection();

                productBatchSelection.Batch_Id = item.Batch_ID;
                productBatchSelection.ProductCost = item.Product_Cost;
                productBatchSelection.ProductName = item.Product.Prod_Name;
                productBatchSelection.Product_Code = item.Product.Bar_Code;
                productBatchSelection.Product_Id = item.Product_ID;
                _ProductBatchSelection.Add(productBatchSelection);

            }
            List<object> newListproout = new List<object>();

            foreach (var member in _ProductBatchSelection)
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
        /// <summary>
        /// get all detail bt its master id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns> List<TransferToProjectDetail></returns>

        public static List<TransferToProjectDetail> GetAllTempTransferDetailsToStore(int masterVmId)
        {
            var db = new AccountingContainer();
            var Details = db.TransferToProjectDetails.Where(invoice => invoice.Master_ID == masterVmId).ToList();
            return Details;
        }
        /// <summary>
        /// get all detail bt its master id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns> List<TransferToProjectDetailTemp></returns>

        public static List<TransferToProjectDetailTemp> GetAllTempTransDetailsresultToStore(int masterVmId)
        {
            var db = new AccountingContainer();
            var TransTemp = db.TransferToProjectTemps.Find(masterVmId);
            return TransTemp.TransferToProjectDetailTemps.ToList();
        }
        /// <summary>
        /// fill for create anew transfer of invoice
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>

        public static TransferToProjectMasterVM fillForEditingTransferToStore(int invoice)
        {
            var db = new AccountingContainer();
            var Transfer = db.TransferToProjectMasters.Find(invoice);
            var TransferVM = new TransferToProjectMasterVM();
            String startCodeExpenses = "05";
            var comp = db.Companies.Where(c => c.Comp_Id == Transfer.Company_Id);
            if (comp.Any())
            {
                TransferVM.Company_Id = comp.FirstOrDefault().Comp_Id;
            }
            var stor = db.Stores.Where(Cu => Cu.Store_ID == Transfer.Store_ID);
            if (stor.Any())
            {
                TransferVM.Store_Id = stor.FirstOrDefault().Store_ID;
            }
            var account = db.ChartOfAccountings.Where(Cu => Cu.Account_Id == Transfer.Account_Id);
            if (account.Any())
            {
                TransferVM.Account_Id = account.FirstOrDefault().Account_Id;
            }
            var branch = db.Branches.Where(Cu => Cu.Branch_Id == Transfer.Branch_Id);
            if (branch.Any())
            {
                TransferVM.Branch_Id = branch.FirstOrDefault().Branch_Id;
            }
            var project = db.SubLedgers.Where(Cu => Cu.SubLed_Id == Transfer.Project_Id);
            if (project.Any())
            {
                TransferVM.Project_Id = project.FirstOrDefault().SubLed_Id;
            }
            var lista = new List<ChartOfAccounting> { };
            foreach (var k in db.ChartOfAccountings)
            {
                var parent = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == k.Account_Id);
                var o = parent.FirstOrDefault();
                if (o == null)
                {
                    lista.Add(db.ChartOfAccountings.Find(k.Account_Id));

                }
            }
            TransferVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            TransferVM.MyStoresList = new SelectList(db.Stores, "Store_ID", "Store_Name");
            TransferVM.AccountList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(startCodeExpenses)), " Account_Id", "Account_Name");
            TransferVM.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            TransferVM.DepList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            TransferVM.BUList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            TransferVM.SubList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى"), "SubLed_Id", "SubLed_Name");

            TransferVM.ProjList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مشروع"), "SubLed_Id", "SubLed_Name");
            TransferVM.Master_Id = Transfer.Master_Id;
            TransferVM.Trans_Code = Transfer.Trans_Code;
            TransferVM.Trans_Date = Convert.ToString(Transfer.Trans_Date);
            TransferVM.Invoice_State = Transfer.Invoice_State;
            TransferVM.Describtion = Transfer.Describtion;
            TransferVM.Reference = Transfer.Reference;
            TransferVM.Branch_Id = Transfer.Branch_Id;
            TransferVM.Department_ID = Transfer.Department_ID;
            TransferVM.BussinesUnit_ID = Transfer.BussinesUnit_ID;
            TransferVM.SubLedger_ID = Transfer.SubLedger_ID;
            var TransTemp = new TransferToProjectTemp();
            TransTemp.Account_Id = Transfer.Account_Id;
            TransTemp.Branch_Id = Transfer.Branch_Id;
            TransTemp.Company_Id = Transfer.Company_Id;
            TransTemp.Describtion = Transfer.Describtion;
            TransTemp.Invoice_State = Transfer.Invoice_State;
            TransTemp.Project_Id = Transfer.Project_Id;
            TransTemp.Store_Id = Transfer.Store_ID;
            TransTemp.Trans_Code = Transfer.Trans_Code;
            TransTemp.Trans_Date = Transfer.Trans_Date;
            var ThereORN = db.TransferToProjectTemps.Find(Transfer.Master_Id);
            if (ThereORN != null)
            {

            }
            else
            {
                TransTemp.Master_Id = Transfer.Master_Id;

                db.TransferToProjectTemps.Add(TransTemp);
            }
            db.SaveChanges();
            return TransferVM;
        }
        /// <summary>
        /// post editing of transfer and change in db
        /// </summary>
        /// <param name="masterVM"></param>
        /// <returns></returns>

        public static string editinvoiceToStore(TransferToProjectMasterVM masterVM)
        {
            var db = new AccountingContainer();
            var Trans = db.TransferToProjectMasters.Find(masterVM.Master_Id);
            var Transtemp = db.TransferToProjectTemps.Find(masterVM.Master_Id);
            Trans.Master_Id = masterVM.Master_Id;
            Trans.Account_Id = masterVM.Account_Id;
            Trans.Branch_Id = masterVM.Branch_Id.Value;
            Trans.Department_ID = masterVM.Department_ID;
            Trans.BussinesUnit_ID = masterVM.BussinesUnit_ID;
            Trans.SubLedger_ID = masterVM.SubLedger_ID;

            Trans.Company_Id = masterVM.Company_Id;
            Trans.Describtion = masterVM.Describtion;
            Trans.Invoice_State = TransactionState.Pending;
            Trans.Project_Id = masterVM.Project_Id;
            Trans.Reference = masterVM.Reference;
            Trans.Store_ID = masterVM.Store_Id;
            Trans.Trans_Date = DateTime.Now;
            decimal k = 0;
            var oldDetail = db.TransferToProjectDetails.Where(a => a.Master_ID == Trans.Master_Id).ToList();
            var detailListTemp = db.TransferToProjectDetailTemps.Where(e => e.Master_ID == Transtemp.Master_Id).ToList();
            List<TransferToProjectDetailTemp> DeleteList = new List<TransferToProjectDetailTemp>();
            foreach (var detail in detailListTemp)
            {
                var item = new TransferToProjectDetail();
                item.Product_ID = detail.Product_Id;
                var productPrice = db.ProductBatches.Where(a => a.Product_ID == detail.Product_Id).FirstOrDefault();

                item.Product_Price = productPrice.Product_Cost;
                item.Product_Quantity = detail.Product_Quantity;
                ProductBatches detailBatch = db.ProductBatches.Find(detail.Batch_ID);
                item.Product_ID = detailBatch.Product_ID;
                if (detail.Product_Quantity > detailBatch.Product_Quantity)
                    return "Not enough quantity of " + detailBatch.Product.Prod_Name;
                // Calculate the Total_Cost
                item.Detail_Total_Cost = item.Product_Price * item.Product_Quantity;
                item.Batch_ID = detail.Batch_ID;
                k += item.Detail_Total_Cost;
                Trans.Total_Cost = k;
                Trans.TransferToProjectDetails.Add(item);
                DeleteList.Add(detail);
                //db.TransferToProjectDetailTemps.Remove(detail);
                //db.SaveChanges();
            }
            db.TransferToProjectDetailTemps.RemoveRange(DeleteList);
            db.Entry(Trans).State = EntityState.Modified;
            db.TransferToProjectTemps.Remove(Transtemp);
            db.TransferToProjectDetails.RemoveRange(oldDetail);
            db.SaveChanges();

            return "success";
        }
        /// <summary>
        /// delete transfer using its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public static String DeleteTransferToProject(int id)
        {
            var db = new AccountingContainer();
            TransferToProjectMaster invoice = db.TransferToProjectMasters.Find(id);

            if (invoice.Invoice_State != TransactionState.Pending)
                return "التحويل  فى حالة التأثير فى الحسابات لايمكنك الحذف";
            db.TransferToProjectDetails.RemoveRange(invoice.TransferToProjectDetails);
            db.TransferToprojectFile1.RemoveRange(invoice.TransferToprojectFile1);
            db.TransferToProjectMasters.Remove(invoice);

            db.SaveChanges();
            db.Dispose();
            return Resources.validate.DeleteSuccessful;
        }
        /// <summary>
        /// get all department depend on branch
        /// </summary>
        /// <param name="Branch_Id"></param>
        /// <returns>IEnumerable<SelectListItem></returns>

        public static IEnumerable<SelectListItem> GetDepartment(int Branch_Id)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> Department = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == Branch_Id && p.Subled_Level == 2), "SubLed_Id", "SubLed_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return Department;
        }

        // use this method to make ListItem from Provinces Cities and get Data from DataBase.
        public static IEnumerable<SelectListItem> GetBussines(int Department_Id)
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> Bussines = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == Department_Id), "SubLed_Id", "SubLed_Name");
            // = db.Provinces.Where(p => p.CountryId == countryId).ToList();
            return Bussines;
        }
    }
}