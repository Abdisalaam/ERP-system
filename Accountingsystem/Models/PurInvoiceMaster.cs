using Accountingsystem.ViewModel;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace Accountingsystem.Models
{


    public partial class PurInvoiceMaster
    {
        /// <summary>
        /// To check if the serial code is exist before or not 
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns></returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.PurInvoiceMasters.ToList();
            foreach (var item in allinData)
            {

                if (item.Serial_Number == Serial || !Serial.StartsWith("PUR-"))
                {
                    return true;
                }

                else
                { }

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
        /// Get list of all purchase invoice
        /// </summary>
        /// <returns></returns>
        public static List<PurInvoiceMaster> GetAllInvoicesProducts(int ?all)
        {
            var db = new AccountingContainer();
            var orders = db.PurInvoiceMasters.Where(e => e.Store_ID > 0).ToList();
            if (all == null)
            {
                orders.Reverse();
                orders = orders.Take(100).ToList();
            }
            return orders;
        }

        /// <summary>
        /// Get list of all purchase assets invoice
        /// </summary>
        /// <returns></returns>
        public static List<PurInvoiceMaster> GetAllInvoicesAssets(int? all)
        {
            var db = new AccountingContainer();
            var orders = db.PurInvoiceMasters.Where(e => e.Store_ID == 0).ToList();
            if (all == null)
            {
                orders.Reverse();
                orders = orders.Take(100).ToList();
            }
            return orders;
        }
        /// <summary>
        /// Get all invoices and receipt in status pendding
        /// </summary>
        /// <returns></returns>
        public static List<PurchaseSectionPendingVM> GetAllPendings()
        {
            var db = new AccountingContainer();
            List<PurchaseSectionPendingVM> sectionList = new List<PurchaseSectionPendingVM>();

            var purchoinv = db.PurInvoiceMasters.Where(e => e.Invoice_State == TransactionState.Pending).ToList();
            if (purchoinv != null)
            {
                foreach (var purchaseinv in purchoinv)
                {
                    PurchaseSectionPendingVM thisdetaisl = new PurchaseSectionPendingVM();
                    thisdetaisl.MasterID = purchaseinv.Master_Id;
                    thisdetaisl.Serial = purchaseinv.Serial_Number;
                    thisdetaisl.Description = purchaseinv.Description;
                    thisdetaisl.TotalAmount = purchaseinv.Total_Cost;
                    sectionList.Add(thisdetaisl);
                }

            }
            var purchaseRecall = db.RecalPurchInvoiceMasters.Where(e => e.PI_State == TransactionState.Pending).ToList();
            if (purchaseRecall != null)
            {
                foreach (var recal in purchaseRecall)
                {
                    PurchaseSectionPendingVM thisdetaisl = new PurchaseSectionPendingVM();
                    thisdetaisl.MasterID = recal.Master_Id;
                    thisdetaisl.Serial = recal.Recall_MPurch_Serial;
                    thisdetaisl.Description = recal.Doc_Type;
                    thisdetaisl.TotalAmount = recal.Inv_TotalCost;
                    sectionList.Add(thisdetaisl);
                }
            }
            var Payments = db.Payments.Where(e => e.Pay_State == TransactionState.Pending).ToList();
            if (Payments != null)
            {
                foreach (var pay in Payments)
                {
                    PurchaseSectionPendingVM thisdetaisl = new PurchaseSectionPendingVM();
                    thisdetaisl.MasterID = pay.Pay_ID;
                    thisdetaisl.Serial = pay.Pay_Number;
                    thisdetaisl.Description = pay.Rec_Description;
                    thisdetaisl.TotalAmount = pay.Pay_Amount;
                    sectionList.Add(thisdetaisl);
                }
            }
            return sectionList;
        }
        /// <summary>
        /// Get all invoices that cost distribute it 
        /// </summary>
        /// <returns></returns>
        public static List<PurInvoiceMaster> GetAllInvoicesCostDistribution()
        {
            var db = new AccountingContainer();
            List<PurInvoiceMaster> masterList = new List<PurInvoiceMaster> { };
            var list = db.PurInvoiceMasters.Where(f => f.TotalAmountAfMat > 0 && f.Cost_distribute == false && f.Invoice_State == TransactionState.Post).ToList();
            foreach (var invoice in list)
            {
                var reverse = db.RecalPurchInvoiceMasters.Where(a => a.PurInvoiceMasterMaster_Id == invoice.Master_Id && a.PI_State == TransactionState.Post).ToList();

                decimal TotalReferse = 0;
                foreach (var reverseInvoice in reverse)
                {
                    TotalReferse += reverseInvoice.Inv_TotalCost;

                }
                if (TotalReferse < invoice.Total_Cost)
                {
                    masterList.Add(invoice);

                }
            }
            //var members = db.PurInvoiceMasters.Where(t => t.Cost_distribute == false && t.Invoice_State == TransactionState.Post).ToList();
            return masterList;
        }

        /// <summary>
        /// Get invoice by id and calculate the payment after pay it 
        /// </summary>
        /// <param name="detailId"></param>
        /// <returns></returns>
        public static PurInvoiceMaster GetInvoiceDetailsById(int detailId)
        {
            var db = new AccountingContainer();
            var detail = db.PurInvoiceMasters.Find(detailId);
            if (detail.TotaL_After_Fixed == null)
            {
                detail.FixedDiscount = 0;
                detail.TotaL_After_Fixed = detail.TotalAmountAfMat;
            }
            detail.Payed_Amount = (detail.TotaL_After_Fixed - detail.TotalAmountAfMat);
            db.Entry(detail).State = EntityState.Modified;
            db.SaveChanges();

            return detail;

        }

        /// <summary>
        /// Get invoice in table temp by master id 
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns></returns>
        public static PurchaseDetailsTempVM GetAllTempInvoiceDetails(int masterVmId)
        {
            var db = new AccountingContainer();
            var _PurchaseInvoiceDetailsTemp = new PurchaseDetailsTempVM();
            _PurchaseInvoiceDetailsTemp.invoiceTemp = db.PurchaseDetailsTemps.Where(invoice => invoice.PurInvoiceMasterTempMaster_Id == masterVmId).ToList();
            _PurchaseInvoiceDetailsTemp.invoice_Id = masterVmId;
            var thisinv = db.PurInvoiceMasterTemps.Find(masterVmId);
            if (thisinv.AllowDiscount > 0 && thisinv.AllowDiscount != null)
            {
                _PurchaseInvoiceDetailsTemp.AllowDiscount = (decimal)thisinv.AllowDiscount;
            }
            else
            {
                _PurchaseInvoiceDetailsTemp.AllowDiscount = 0;
            }
            return _PurchaseInvoiceDetailsTemp;
        }
        public static PurchaseDetailsTempVM GetAllTempInvoiceDetailsForOrder(string code,int masterid)
        {
            var db = new AccountingContainer();
            PurchaseDetailsTempVM vm = new PurchaseDetailsTempVM();
            var purchtemp = db.PurInvoiceMasterTemps.Find(masterid);
            db.PurchaseDetailsTemps.RemoveRange(purchtemp.PurchaseDetailsTemps);
            db.SaveChanges();
            var purchase = db.PurchaseOrderMasters.Where(a=>a.Serial_Number==code).FirstOrDefault();
            var thisAcutauldetail = purchase.PurchaseOrderDetails.ToList();
            if (purchase.FixedDiscount == null)
            {
                purchase.FixedDiscount = 0;
            }
            vm.AllowDiscount = purchase.FixedDiscount.Value;
            
            var PurchList = new List<PurchaseDetailsTemp> { };

            if (thisAcutauldetail.Count > 0)
            {
                foreach (var detail in thisAcutauldetail)
                {
                    var purchasedetiltemp = new PurchaseDetailsTemp();
                    purchasedetiltemp.Product_Quantity = detail.Product_Quantity;
                    purchasedetiltemp.Product_Price = detail.Product_Price;
                    purchasedetiltemp.Total_Amount = detail.Total_Amount;
                    if (detail.Product_Tax == null)
                    {

                    }
                    else
                    {
                        purchasedetiltemp.Product_Tax = (int)detail.Product_Tax;
                    }
                    purchasedetiltemp.Detail_Total_Tax = detail.Detail_Total_Tax;
                    purchasedetiltemp.Detail_Total_Cost = detail.Detail_Total_Cost;
                    purchasedetiltemp.Product_ID = detail.Product_ID;
                    purchasedetiltemp.PurInvoiceMasterTempMaster_Id = purchtemp.Master_Id;
                    
                        var thisporduct = db.Products.Find(detail.Product_ID);
                      
                        purchasedetiltemp.Product_Code = thisporduct.Prod_Code;
                    

                    PurchList.Add(purchasedetiltemp);

                }

            }
            else
            {
            }
            db.PurchaseDetailsTemps.AddRange(PurchList);
            db.SaveChanges();
            vm.invoiceTemp = db.PurInvoiceMasterTemps.Find(masterid).PurchaseDetailsTemps.ToList();

            return vm;
        }

        /// <summary>
        /// Method to prepare invoice details
        /// </summary>
        /// <returns></returns>
        public static PurchaseDetailsVM PrepareInvoiceMasterDetail()
        {
            var db = new AccountingContainer();
            var PurchaseDetailsVm = new PurchaseDetailsVM();
         
            var newListpro = (from prod in  db.Products.Where(a => a.IsActive != false&&a.ProductCompounds.Count()<=1 && a.Prod_Type != Prod_Type.خدمة) select new {

                Id = prod.Prod_ID,
                Name = prod.Prod_Name + "   " + prod.Bar_Code

            })
            ;
        

            PurchaseDetailsVm.MyProductList = new SelectList(newListpro, "Id", "Name");
            var thistaxinchart = db.ChartOfAccountings.Find(35);
       
            var newList = from Taxes in  db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false) select new {

                Id = Taxes.Tax_ID,
                Name = Taxes.Tax_Name + "   " + Taxes.Tax_Percentage + "%"


            }
            
            
            
            ;
            
            PurchaseDetailsVm.MyProduct_TaxList = new SelectList(newList, "Id", "Name");
            return PurchaseDetailsVm;

        }

        /// <summary>
        /// To get price of product by productId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Product GetProductPrice(int id)
        {
            var db = new AccountingContainer();
            var pro = db.Products.Find(id);
            return pro;
        }

        /// <summary>
        /// To add product details to list
        /// </summary>
        /// <param name="PrchaseInvoiceMasterDetail"></param>
        /// <returns></returns>
        public static string AddInvoiceDetailsTemp(PurchaseDetailsVM PrchaseInvoiceMasterDetail)
        {
            var db = new AccountingContainer();
            var purchaseInvoiceMasterTemp = db.PurInvoiceMasterTemps.Find(PrchaseInvoiceMasterDetail.InvoiceMaster_Id);


            var purchaseInvoiceMasterDetailsTemp = new PurchaseDetailsTemp();
            purchaseInvoiceMasterDetailsTemp.Product_ID = PrchaseInvoiceMasterDetail.SelectedProductId;
            
            var details = purchaseInvoiceMasterTemp.PurchaseDetailsTemps;
            var duplecatTemp = details.Where(p => p.PurInvoiceMasterTempMaster_Id == purchaseInvoiceMasterTemp.Master_Id && p.Product_ID == PrchaseInvoiceMasterDetail.SelectedProductId).ToList();
            if (duplecatTemp.FirstOrDefault() != null)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.thesameProduct;
            }

            else
            {
                var Product = db.Products.Find(PrchaseInvoiceMasterDetail.SelectedProductId);
                purchaseInvoiceMasterDetailsTemp.Product_Price = Product.Prod_Cost_Price.Value;

                purchaseInvoiceMasterDetailsTemp.Product_Tax = Product.Tax_Id_Purch;
                purchaseInvoiceMasterDetailsTemp.Product_Quantity = PrchaseInvoiceMasterDetail.Product_Quantity;
                if (purchaseInvoiceMasterDetailsTemp.Product_Quantity == null || purchaseInvoiceMasterDetailsTemp.Product_Quantity <= 0)
                {
                    purchaseInvoiceMasterDetailsTemp.Product_Quantity = 1;
                    PrchaseInvoiceMasterDetail.Product_Quantity = 1;


                }
                var thisproduct = db.Products.Find(PrchaseInvoiceMasterDetail.SelectedProductId);
                if (thisproduct == null)
                { }
                else
                {
                    purchaseInvoiceMasterDetailsTemp.ItemDesc = thisproduct.Prod_Name;
                    purchaseInvoiceMasterDetailsTemp.Product_Code = thisproduct.Prod_Code;
                }
                var resultTax = db.Taxes.Find(Product.Tax_Id_Purch);
                if (resultTax != null)
                {
                    purchaseInvoiceMasterDetailsTemp.Product_Tax = resultTax.Tax_ID;
                }
                else
                {
                    purchaseInvoiceMasterDetailsTemp.Product_Tax = 0;
                }
                var a = db.Taxes.Find(Product.Tax_Id_Purch);

                purchaseInvoiceMasterDetailsTemp.Total_Amount = (Product.Prod_Cost_Price.Value * PrchaseInvoiceMasterDetail.Product_Quantity);
                decimal TotalBeforeDiscount = Product.Prod_Cost_Price.Value * PrchaseInvoiceMasterDetail.Product_Quantity;
                var dicount = (PrchaseInvoiceMasterDetail.Discount / 100) * purchaseInvoiceMasterDetailsTemp.Total_Amount;

                purchaseInvoiceMasterDetailsTemp.Total_Amount = purchaseInvoiceMasterDetailsTemp.Total_Amount - dicount;
                purchaseInvoiceMasterDetailsTemp.TotalBeforeDisc = TotalBeforeDiscount;
                purchaseInvoiceMasterDetailsTemp.TotalDiscount = dicount;

                if (a == null)
                {
                    purchaseInvoiceMasterDetailsTemp.Detail_Total_Tax = 0;
                }
                else
                {
                    purchaseInvoiceMasterDetailsTemp.Detail_Total_Tax = (purchaseInvoiceMasterDetailsTemp.Total_Amount) * (Convert.ToDecimal(a.Tax_Percentage) / 100);
                }

                purchaseInvoiceMasterDetailsTemp.Detail_Total_Cost = (purchaseInvoiceMasterDetailsTemp.Total_Amount) + (purchaseInvoiceMasterDetailsTemp.Detail_Total_Tax);
                purchaseInvoiceMasterTemp.PurchaseDetailsTemps.Add(purchaseInvoiceMasterDetailsTemp);
                db.Entry(purchaseInvoiceMasterTemp).State = EntityState.Modified;
                //var existProductId = purchaseInvoiceMasterTemp.PurchaseDetailsTemps.FirstOrDefault().Product_ID;
                db.SaveChanges();

                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Addedsuccessfully;
            }

        }
        //GET: Inventory purchase Invoice
        /// <summary>
        /// Get creation for purchase invoice
        /// </summary>
        /// <returns></returns>
        public static PurMasterVM AddNewForCreation(int? supId)
        {
            var db = new AccountingContainer();
            var masterVM = new PurMasterVM();


            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0).ToList();

            masterVM.PaymentMethodList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("01012") && accCode.isActive != false), "Account_Id", "Account_Name");
            masterVM.SelectedSupplierId = supId;
            masterVM.MySupplierList = new SelectList(db.Suppliers.Where(c => c.Supplier_state == Customer_State.نشط), "Supp_Id", "Supp_Name");
            masterVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            masterVM.MyBussinesUnitList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            // masterVM.MyProductsList = new SelectList(db.Products, "Prod_ID", "Prod_Name");
            masterVM.MyStoresList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            //masterVM.BU_List = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            masterVM.MyEmpList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            //masterVM.MyBranchList = new SelectList(db.Branches, "Branch_Id", "Branch_Name");
            //masterVM.AccountsList = new SelectList(db.ChartOfAccountings, "Account_Id", "Account_Name");
            masterVM.Invoice_State = TransactionState.Pending;


            var lastSerial = db.PurInvoiceMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                masterVM.Serial_Number = "PUR-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                masterVM.Serial_Number = "PUR-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
            }

            var purchaseMasterTemp = new PurInvoiceMasterTemp();
            var lastID = (from s in db.PurInvoiceMasterTemps
                          select s.Master_Id).ToList().LastOrDefault();
            purchaseMasterTemp.Master_Id = lastID + 1;
            purchaseMasterTemp.AllowDiscount = 0;
            db.PurInvoiceMasterTemps.Add(purchaseMasterTemp);

            db.SaveChanges();
            if (masterVM.Master_Id == purchaseMasterTemp.Master_Id)
            { masterVM.Master_Id = purchaseMasterTemp.Master_Id + 2; }
            else
            {
                masterVM.Master_Id = purchaseMasterTemp.Master_Id;
            }
            return masterVM;
        }
        //GET: assetPurchaseInvoice
        /// <summary>
        /// To get creation of purchase assets invoice
        /// </summary>
        /// <returns></returns>
        public static PurMasterVM AssetAddNewForCreation()
        {
            var db = new AccountingContainer();
            var masterVM = new PurMasterVM();

            ///////////////
            //masterVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            masterVM.MySupplierList = new SelectList(db.Suppliers.Where(a=>a.Supplier_state!=Customer_State.غير_نشط), "Supp_Id", "Supp_Name");
            masterVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            masterVM.MyBussinesUnitList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            masterVM.MyProductsList = new SelectList(db.FixedAssetInformations, "Id", "Fasset_Code");
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false).ToList();

            masterVM.PaymentMethodList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("010120001") || accCode.Account_Code.StartsWith("010120002")), "Account_Id", "Account_Name");
            masterVM.MyAssetList = new SelectList(db.FixedAssetInformations, "Id", "FAsset_Description");
            masterVM.SubledgerList = new SelectList(db.SubLedgers.Where(a=>a.isActive!=false), "SubLed_Id", "SubLed_Name");
            masterVM.AccountsList = new SelectList(db.ChartOfAccountings.Where(a=>a.isActive!=false), "Account_Id", "Account_Name");
            //masterVM.BU_List = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            //masterVM.MyEmpList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");

            // masterVM.MyBranchList = new SelectList(db.Branches, "Branch_Id", "Branch_Name");
            masterVM.Invoice_State = TransactionState.Pending;


            //int lastCode = (from s in db.InvoiceMasters
            //                select s.Master_Id).Count();
            //masterVM.Serial_Number = (lastCode + 1).ToString();

            var lastSerial = db.PurInvoiceMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                masterVM.Serial_Number = "PUR-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                masterVM.Serial_Number = "PUR-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
            }

            var purchaseMasterTemp = new PurInvoiceMasterTemp();
            var lastID = (from s in db.PurInvoiceMasterTemps
                          select s.Master_Id).ToList().LastOrDefault();
            purchaseMasterTemp.Master_Id = lastID + 1;
            purchaseMasterTemp.AllowDiscount = 0;
            db.PurInvoiceMasterTemps.Add(purchaseMasterTemp);




            db.SaveChanges();
            if (masterVM.Master_Id == purchaseMasterTemp.Master_Id)
            { masterVM.Master_Id = purchaseMasterTemp.Master_Id + 2; }
            else
            {
                masterVM.Master_Id = purchaseMasterTemp.Master_Id;
            }

            return masterVM;
        }
        /// <summary>
        /// To check for seriial number of supplier is exist or not
        /// </summary>
        /// <param name="supId"></param>
        /// <param name="serialNum"></param>
        /// <returns></returns>
        public static int CheckTheSerialNum(int supId, string serialNum)
        {
            var db = new AccountingContainer();
            var result = db.PurInvoiceMasters.Where(sup => sup.Supplier_Id == supId && sup.Serial_Number == serialNum.Trim());
            if (result.Any())
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }
        //POST: inventory Purchase invoice 
        /// <summary>
        /// Post for create purchase invoice 
        /// </summary>
        /// <param name="masterVM"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static Tuple<string, int> AddNewInvoice(PurMasterVM masterVM, String UserId)
        {


            var db = new AccountingContainer();
            var master = new PurInvoiceMaster();

            var PurchaseMasterTemp = db.PurInvoiceMasterTemps.Find(masterVM.Master_Id);
            var SelectedInvoice_SupplierSerial = db.PurInvoiceMasters.Where(i => masterVM.SelectedSupplierId == master.Supplier_Id && masterVM.Serial_Number == master.Serial_Number);
            if (SelectedInvoice_SupplierSerial.Count() == 0)
            {
                // master.Serial_Number = masterVM.Serial_Number;
                var lastSerial = db.PurInvoiceMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                if (lastSerial == null)
                {
                    master.Serial_Number = "PUR-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    master.Serial_Number = "PUR-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
                }
                master.Description = masterVM.Description;
                master.Purch_Order_Code = masterVM.Purch_Order_Code;

                master.Payment_Policy = masterVM.Payment_Policy;
                var comp = db.Companies.FirstOrDefault();
                if (comp != null)
                {
                    var date = Convert.ToDateTime(masterVM.Invoice_Date);
                    if (date < comp.StartFinancialDate)
                    {
                        master.Invoice_Date = DateTime.Now;
                    }
                    else
                    {
                        master.Invoice_Date = Convert.ToDateTime(masterVM.Invoice_Date);
                    }
                }
                else
                {
                    master.Invoice_Date = DateTime.Now;
                }
                //  master.Invoice_Date = DateTime.Now;
                master.Due_Date = DateTime.Now.AddDays(masterVM.Payment_Policy);
                master.GL_Date = master.Invoice_Date;
                master.Exchange_Rate = masterVM.Exchange_Rate;
                master.Subledger_Id = masterVM.SelectedSubledger_Id;
                master.Purch_Order_Code = masterVM.Purch_Order_Code;
                master.Supp_Inv_Serial = masterVM.Supp_Inv_Serial;
                var resultSupplier = db.Suppliers.Find(masterVM.SelectedSupplierId);
                if (resultSupplier != null)
                {
                    master.Supplier_Id = resultSupplier.Supp_Id;
                }
                else
                {
                    master.Supplier_Id = 0;
                }

                var resultCompany = db.Companies.FirstOrDefault();
                if (resultCompany != null)
                {
                    master.Company_Id = resultCompany.Comp_Id;
                }
                master.Company_Id = resultCompany.Comp_Id;
                var resultCurrency = db.Currencies.Find(masterVM.SelectedCurrencyId);
                if (resultCurrency != null)
                {
                    master.Invoice_Currency = resultCurrency.Currency_Type;
                }

                var resultAccountRevenu = db.ChartOfAccountings.Find(masterVM.PaymentMethod_Id);
                if (resultAccountRevenu != null)
                {
                    master.PaymentMethod_Id = resultAccountRevenu.Account_Id;

                }
                else
                {
                    master.PaymentMethod_Id = 0;
                }

                var resultBU = db.BussinesUnits.Find(masterVM.SelectedBussinesUnitId);
                if (resultBU != null)
                {
                    master.BU_Id = resultBU.BU_Id;
                }
                if(masterVM.SelectedStoreId==0|| masterVM.SelectedStoreId == null)
                {
                    masterVM.SelectedStoreId = db.Stores.FirstOrDefault().Store_ID;
                }
                var resultStore = db.Stores.Find(masterVM.SelectedStoreId);
                if (resultStore != null)
                {
                    master.Store_ID = resultStore.Store_ID;
                    master.Store_Name = resultStore.Store_Name;
                }

                var resultBranch = db.Branches.FirstOrDefault();
                if (resultBranch != null)
                {
                    master.Branch_Id = resultBranch.Branch_Id;
                }


                var resultEmp = db.Employees.Find(masterVM.SelectedEmp_Id);
                if (resultEmp != null)
                {
                    master.Emp_Id = resultEmp.Emp_Id;
                }
                if (masterVM.Payment_Policy == null)
                {
                    master.Due_Date = Convert.ToDateTime(masterVM.Invoice_Date);
                }
                else
                {
                    master.Due_Date = Convert.ToDateTime(masterVM.Invoice_Date).AddMonths((int)masterVM.Payment_Policy);
                }
                /* DateTime.Now.AddMonths(SupscripeVM.Durationsubscription)*/
                master.Store_ID = masterVM.SelectedStoreId;
                master.Master_Discount = masterVM.Master_Discount;
                master.FixedDiscount = masterVM.Fixed_Discount;
                master.Invoice_State = masterVM.Invoice_State;
                master.Tax_Type = masterVM.Tax_Type;
                //master.Serial_Number = masterVM.Serial_Number;
                master.Document_Type = Document_Type.Purchase_Invoice; //////////// Purchase Invoice
                master.Invoice_State = TransactionState.Pending;
                master.Subledger_Id = masterVM.SelectedSubledger_Id;

                if (masterVM.Files.FirstOrDefault() != null)
                {
                    foreach (var file in masterVM.Files)
                    {
                        var filedetails = new PurchaseFileDetail();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.FileContent = data;
                        filedetails.FileType = file.ContentType;
                        if (data.Length < 1000000)
                        {
                            master.PurchaseFileDetails.Add(filedetails);
                        }
                        
                    }
                }

                if (PurchaseMasterTemp.PurchaseDetailsTemps.ToList().Count() == 0) {


                    return Tuple.Create("لا يوجد محتوي لهذه الفاتورة ", 0);


                }

                foreach (var item in PurchaseMasterTemp.PurchaseDetailsTemps.ToList())
                {
                    var details = new PurchaseDetails();
                    // details.Details_Id = item.Details_Id;
                    details.Product_ID = item.Product_ID;
                    details.Product_Quantity = item.Product_Quantity;
                    details.Reverse_Quantity = details.Product_Quantity;
                    details.Product_Price = item.Product_Price;
                    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    decimal discount = TotalBeforeDiscount * (master.Master_Discount / 100);
                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                    decimal tax = 0;
                    if (item.Product_Tax != 0&& item.Product_Tax!=null)
                    {
                        tax = Convert.ToDecimal(db.Taxes.Find(item.Product_Tax).Tax_Percentage);
                    }
                    else
                    {
                        tax = 0;
                    }

                    details.ItemDesc = item.ItemDesc;
                    details.Item_Code = item.Product_Code;
                    details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                    details.Total_Amount = TotalBeforeDiscount;
                    details.Detail_Total_Cost = TotalAfterDiscount + details.Detail_Total_Tax;
                    details.Reverse_Total_Tax = details.Detail_Total_Tax;
                    details.Reverse_Tota_Cost = details.Detail_Total_Cost;

                    master.Total_Amount += details.Total_Amount;
                    master.Total_Tax += details.Detail_Total_Tax;
                    master.Total_Cost += details.Detail_Total_Cost;


                    details.Product_Tax = Convert.ToInt32(item.Product_Tax);
                    var thistax = db.Taxes.Find(details.Product_Tax);
                    if (thistax == null)
                    {
                        details.Tax_Name = "";
                    }
                    else
                    {
                        details.Tax_Name = thistax.Tax_Name;
                    }
                    var thisProduct = db.Products.Find(item.Product_ID);
                    if (thisProduct != null)
                    {
                        details.ItemDesc = thisProduct.Prod_Name;
                    }
                    else
                    {
                        var thisAsset = db.FixedAssets.Find(item.Asset_Id);
                        if (thisAsset == null)
                        {
                            details.ItemDesc = "";
                        }
                        else
                        {
                            details.ItemDesc = thisAsset.FAsset_Description;
                            details.Asset_Id = item.Asset_Id;
                        }
                    }


                    master.InvoiceDetails.Add(details);
                    db.PurchaseDetailsTemps.Remove(item);

                }
                // The total amount after matching is initialized to the total amount
                master.TotaL_After_Fixed = master.Total_Cost - master.FixedDiscount;
                master.TotalAmountAfMat = master.TotaL_After_Fixed.Value;
                master.Payed_Amount = 0;
                master.BU_Id = 0;
                master.Subledger_Id = 0;
                if (masterVM.SelectedSupplierId!=null)
                {
                    master.Supplier_Id = masterVM.SelectedSupplierId.Value;
                }
               
                //Creates Object of Type Transaction
                var trans = new TransactionBatch();
                trans.Description = masterVM.Description;
                trans.Date = Convert.ToDateTime(masterVM.Invoice_Date);
                trans.OperationName = "فاتورة مشتريات";
                trans.UserId = UserId;///////////////////// User Id Coming from another entity "Identity"

                db.PurInvoiceMasters.Add(master);
                db.PurInvoiceMasterTemps.Remove(PurchaseMasterTemp);
                db.SaveChanges();
                db.Dispose();
                trans.OperationId = master.Master_Id;
                trans.Operation_Code = master.Serial_Number;
                TransactionBatch.AddNewTransaction(trans);

                return Tuple.Create(Resources.validate.CreateSucc, master.Master_Id);
            }

            return Tuple.Create(Resources.validate.Error, 0);

        }

        /// <summary>
        /// Prepare to details of product for assets
        /// </summary>
        /// <returns></returns>
        public static PurchaseDetailsVM PrepareInvoiceMasterDetailForAsset()
        {
            var db = new AccountingContainer();
            var PurchaseDetailsVm = new PurchaseDetailsVM();
            PurchaseDetailsVm.MyAssetList = new SelectList(db.FixedAssetInformations.Where(a=>a.IsActive != false), "Id", "FAsset_Description");
            var thistaxinchart = db.ChartOfAccountings.Find(35);
            List<object> newList = new List<object>();
            var members = db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false).ToList();
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.Tax_ID,
                    Name = member.Tax_Name + "   " + member.Tax_Percentage + "%"
                });
            PurchaseDetailsVm.MyProduct_TaxList = new SelectList(newList, "Id", "Name");            
            return PurchaseDetailsVm;

        }

        /// <summary>
        /// Add product details to list 
        /// </summary>
        /// <param name="PrchaseInvoiceMasterDetail"></param>
        /// <returns></returns>
        public static string AddInvoiceDetailsTempForAsset(PurchaseDetailsVM PrchaseInvoiceMasterDetail)
        {
            var db = new AccountingContainer();
            if (PrchaseInvoiceMasterDetail.AssetId == null || PrchaseInvoiceMasterDetail.AssetId == 0)
            {
                return "من فضلك اختر الاصل";
            }
            if (PrchaseInvoiceMasterDetail.Product_Price == null || PrchaseInvoiceMasterDetail.Product_Price <= 0)
            {
                return "من فضلك ادخل السعر ";
            }
            if (PrchaseInvoiceMasterDetail.Product_Quantity == null || PrchaseInvoiceMasterDetail.Product_Quantity <= 0)
            {
                return "من فضلك ادخل الكمية ";
            }
            var purchaseInvoiceMasterTemp = db.PurInvoiceMasterTemps.Find(PrchaseInvoiceMasterDetail.InvoiceMaster_Id);
            var purchaseInvoiceMasterDetailsTemp = new PurchaseDetailsTemp();
            purchaseInvoiceMasterDetailsTemp.Asset_Id = PrchaseInvoiceMasterDetail.AssetId;

            var thisasst = db.FixedAssetInformations.Find(PrchaseInvoiceMasterDetail.AssetId);
            var details = purchaseInvoiceMasterTemp.PurchaseDetailsTemps;
            var duplecatTemp = details.Where(p => p.PurInvoiceMasterTempMaster_Id == purchaseInvoiceMasterTemp.Master_Id && p.Asset_Id == PrchaseInvoiceMasterDetail.AssetId).ToList();
            if (duplecatTemp.FirstOrDefault() != null)
            {
                return "هذا الاصل موجود بالفعل فى القائمة لايمكن اضافته مرة اخرى";
            }
            else
            {
                purchaseInvoiceMasterDetailsTemp.ItemDesc = thisasst.FAsset_Description;
                purchaseInvoiceMasterDetailsTemp.Product_Code = thisasst.FAsset_Code;
                purchaseInvoiceMasterDetailsTemp.Product_Price = PrchaseInvoiceMasterDetail.Product_Price;

                purchaseInvoiceMasterDetailsTemp.Product_Quantity = PrchaseInvoiceMasterDetail.Product_Quantity;

                if (PrchaseInvoiceMasterDetail.SelectProduct_Tax == 0 || PrchaseInvoiceMasterDetail.SelectProduct_Tax == null)
                {
                    purchaseInvoiceMasterDetailsTemp.Product_Tax = 0;
                }
                else
                {
                    purchaseInvoiceMasterDetailsTemp.Product_Tax = (int)PrchaseInvoiceMasterDetail.SelectProduct_Tax;
                }

                var a = db.Taxes.Find(PrchaseInvoiceMasterDetail.SelectProduct_Tax);
                purchaseInvoiceMasterDetailsTemp.Total_Amount = (PrchaseInvoiceMasterDetail.Product_Price * PrchaseInvoiceMasterDetail.Product_Quantity);
                decimal TotalBeforeDiscount = PrchaseInvoiceMasterDetail.Product_Price * PrchaseInvoiceMasterDetail.Product_Quantity;

                var dicount = (PrchaseInvoiceMasterDetail.Discount / 100) * purchaseInvoiceMasterDetailsTemp.Total_Amount;

                purchaseInvoiceMasterDetailsTemp.Total_Amount = purchaseInvoiceMasterDetailsTemp.Total_Amount - dicount;

                purchaseInvoiceMasterDetailsTemp.TotalBeforeDisc = TotalBeforeDiscount;
                purchaseInvoiceMasterDetailsTemp.TotalDiscount = dicount;

                if (a == null)
                {
                    purchaseInvoiceMasterDetailsTemp.Detail_Total_Tax = 0;
                }
                else
                {
                    purchaseInvoiceMasterDetailsTemp.Detail_Total_Tax = (purchaseInvoiceMasterDetailsTemp.Total_Amount) * (Convert.ToDecimal(a.Tax_Percentage) / 100);
                }

                purchaseInvoiceMasterDetailsTemp.Detail_Total_Cost = (purchaseInvoiceMasterDetailsTemp.Total_Amount) + (purchaseInvoiceMasterDetailsTemp.Detail_Total_Tax);
                purchaseInvoiceMasterTemp.PurchaseDetailsTemps.Add(purchaseInvoiceMasterDetailsTemp);

                db.Entry(purchaseInvoiceMasterTemp).State = EntityState.Modified;
                db.SaveChanges();

                return "تم الاضافة  بنجاح ";
            }


        }
        //POST: asset Purchase invoice 
        /// <summary>
        /// get creation for create purchase assets
        /// </summary>
        /// <param name="masterVM"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static int AssetOrItemAddNewInvoice(PurMasterVM masterVM, String UserId)
        {
            var db = new AccountingContainer();
            var master = new PurInvoiceMaster();
            var PurchaseMasterTemp = db.PurInvoiceMasterTemps.Find(masterVM.Master_Id);
            if (masterVM != null)
            {
                //TODO: Acquire serial no. from VM
                var lastSerial = db.PurInvoiceMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                if (lastSerial == null)
                {
                    master.Serial_Number = "PUR-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    master.Serial_Number = "PUR-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
                }
                //TODO: Acquire patch no. from VM
                //master.Batch_Number = masterVM.Batch_Number;
                master.Description = masterVM.Description;
                master.Payment_Policy = masterVM.Payment_Policy;
                master.Invoice_Date = Convert.ToDateTime( masterVM.Invoice_Date);
                master.Due_Date = master.Invoice_Date.AddDays(masterVM.Payment_Policy);
                master.GL_Date = master.Invoice_Date;
                master.Exchange_Rate = masterVM.Exchange_Rate;
                master.Supp_Inv_Serial = masterVM.Supp_Inv_Serial;
                master.Purch_Order_Code = masterVM.Purch_Order_Code;
                master.FixedDiscount = masterVM.Fixed_Discount;
                master.PaymentMethod_Id = masterVM.PaymentMethod_Id;
                master.Payed_Amount = 0;
                //master.Branch_Id = masterVM.SelectedBranch_Id;

                var resultSubledger = db.SubLedgers.Find(masterVM.SelectedSubledger_Id);
                if (resultSubledger != null)
                {
                    master.Subledger_Id = resultSubledger.SubLed_Id;
                }

                var resultSupplier = db.Suppliers.Find(masterVM.SelectedSupplierId);
                if (resultSupplier != null)
                {
                    master.Supplier_Id = resultSupplier.Supp_Id;
                    master.Subledger_Id = resultSupplier.Supp_Id;
                }
                var resultCompany = db.Companies.FirstOrDefault();
                if (resultCompany != null)
                {
                    master.Company_Id = resultCompany.Comp_Id;
                }

                var resultCurrency = db.Currencies.Find(masterVM.SelectedCurrencyId);
                if (resultCurrency != null)
                {
                    master.Invoice_Currency = resultCurrency.Currency_Name;
                }


                var ChartOfAccount = db.ChartOfAccountings.Find(masterVM.SelectedAccount_Id);
                if (ChartOfAccount != null)
                {
                    master.Account_Id = ChartOfAccount.Account_Id;
                }
                master.Store_ID = 0;
                master.Master_Discount = masterVM.Master_Discount;
                master.Document_Type = Document_Type.Purchase_Invoice;      /////////// Purchase Invoice
                master.Invoice_State = TransactionState.Pending;
                master.Tax_Type = masterVM.Tax_Type;


                if (masterVM.Files.FirstOrDefault() != null)
                {
                    foreach (var file in masterVM.Files)
                    {
                        var filedetails = new PurchaseFileDetail();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.FileContent = data;
                        filedetails.FileType = file.ContentType;
                        if (data.Length < 1000000)
                        {
                            master.PurchaseFileDetails.Add(filedetails);
                        }
                       
                    }
                }

                if (PurchaseMasterTemp.PurchaseDetailsTemps.Count > 0)
                {
                    foreach (var item in PurchaseMasterTemp.PurchaseDetailsTemps.ToList())
                    {
                        var details = new PurchaseDetails();
                        details.Details_Id = item.Details_Id;
                        details.Asset_Id = item.Asset_Id;

                        details.Item_Code = item.Product_Code;
                        details.Product_Quantity = item.Product_Quantity;
                        details.Product_Price = item.Product_Price;
                        decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                        decimal discount = TotalBeforeDiscount * (master.Master_Discount / 100);
                        decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                        decimal tax = 0;
                        if (item.Product_Tax != 0)
                        {
                            tax = Convert.ToDecimal(db.Taxes.Find(item.Product_Tax).Tax_Percentage);
                        }

                        details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                        details.Total_Amount = TotalBeforeDiscount;
                        details.Detail_Total_Cost = TotalAfterDiscount + details.Detail_Total_Tax;
                        // Deduct the discount from the detail Total_Amount 

                        details.Product_Tax = Convert.ToInt32(item.Product_Tax);

                        var thistax = db.Taxes.Find(details.Product_Tax);
                        if (thistax != null)
                        {
                            details.Tax_Name = thistax.Tax_Name;
                        }
                        else
                        {
                            details.Tax_Name = "";
                        }
                        var thisasset = db.FixedAssetInformations.Find(item.Asset_Id);
                        details.ItemDesc = thisasset.FAsset_Description;
                        details.InvoiceMaster_Id = masterVM.Master_Id;

                        master.BU_Id = 0;
                        master.Subledger_Id = 0;
                        master.Payed_Amount = 0;
                        master.Total_Tax += details.Detail_Total_Tax;
                        master.Total_Cost += details.Detail_Total_Cost;
                        master.Total_Amount += details.Total_Amount;
                        master.TotalAmountAfMat = master.Total_Cost;
                        if (details.Asset_Id > 0)
                        {

                            // CreateFixedAfterInvoice(master, details);
                        }
                        master.InvoiceDetails.Add(details);
                        db.PurchaseDetailsTemps.Remove(item);

                    }

                    master.TotaL_After_Fixed = master.Total_Cost - master.FixedDiscount;
                    master.TotalAmountAfMat = master.TotaL_After_Fixed.Value;
                    //Creates Object of Type Transaction
                    var trans = new TransactionBatch();
                    trans.Description = masterVM.Description;
                    trans.Date = Convert.ToDateTime(masterVM.Invoice_Date);
                    trans.OperationName = "فاتورة مشتريات";
                    //trans.OprationId = masterVM.Master_Id;/********/
                    trans.UserId = UserId;///////////////////// User Id Coming from another entity "Identity"
                    trans.Operation_Code = master.Serial_Number;
                    TransactionBatch.AddNewTransaction(trans);
                    master.Payed_Amount = 0;

                    db.PurInvoiceMasters.Add(master);
                    db.PurInvoiceMasterTemps.Remove(PurchaseMasterTemp);
                    db.SaveChanges();
                    db.Dispose();

                    return master.Master_Id;
                }
                else
                {
                    return 0;
                }

            }
            return 0;
        }
        //To create fixed asset after create asset purchase invoice
        public static List<FixedAsset> CreateFixedAfterInvoice(PurInvoiceMaster purin, PurchaseDetails purde)
        {
            var db = new AccountingContainer();

            // FixedAsset fix = new FixedAsset();

            var fixid = db.FixedAssetInformations.Find(purde.Asset_Id);
            var NewAssets = new FixedAsset();

            decimal thistax = 0;
            if (purde.Detail_Total_Tax == 0)
            {


            }
            else
            {
                thistax = purde.Detail_Total_Tax / purde.Product_Quantity;

            }



          
            fixid.IsActive = false;
            db.Entry(fixid).State = EntityState.Modified;
            db.SaveChanges();
            var FixedList = new List <FixedAsset>();
            for (int i = 1; i <= purde.Product_Quantity; i++)
            {
                var fixedfromPurchase = new FixedAsset();
                fixedfromPurchase.FAsset_BuyDate = purin.Due_Date;
                if (purin.Tax_Type == false)
                {
                    fixedfromPurchase.FAsset_PurchasePrice = Convert.ToDouble(purde.Total_Amount / purde.Product_Quantity) - (Convert.ToDouble(purde.Total_Amount / purde.Product_Quantity) * (Convert.ToDouble(purin.Master_Discount / 100)));
                }
                else
                { fixedfromPurchase.FAsset_PurchasePrice = Convert.ToDouble(purde.Total_Amount / purde.Product_Quantity) - (Convert.ToDouble(purde.Total_Amount / purde.Product_Quantity) * (Convert.ToDouble(purin.Master_Discount / 100))); }


                fixedfromPurchase.BookValue = fixedfromPurchase.FAsset_PurchasePrice;
                fixedfromPurchase.Currency = purin.Invoice_Currency;
                fixedfromPurchase.Complex = 0;
                fixedfromPurchase.FAsset_AgeMonth = fixid.FAsset_AgeMonth;
                fixedfromPurchase.FixedAssetSecCategoryFAsset_CatID = fixid.FixedAssetSecCategoryFAsset_CatID;
                fixedfromPurchase.FixedAssetTypeFAsset_TypeID = fixid.FixedAssetTypeFAsset_TypeID;
                fixedfromPurchase.BussinesUnitBU_Id = 0;
                //fixedfromPurchase.Currency = purin.Invoice_Currency;
                fixedfromPurchase.FAssetCat_name = fixid.FAssetCat_name;
                fixedfromPurchase.CompanyComp_Id = fixid.CompanyComp_Id;
                fixedfromPurchase.FAsset_Description = fixid.FAsset_Description;
                fixedfromPurchase.FAsset_Type = fixid.FAsset_Type;
                fixedfromPurchase.FAsset_Employee_ID = fixid.FAsset_Employee_ID;
                fixedfromPurchase.FAsset_WorkingDate = fixid.FAsset_WorkingDate.Date;
                fixedfromPurchase.JunckValue = fixid.JunckValue;
                fixedfromPurchase.SubLedger_ID = fixid.SubLedger_ID;
                var lastCode = db.FixedAssets.OrderByDescending(c => c.Fasset_ID).FirstOrDefault();
                if (lastCode == null)
                {
                    fixedfromPurchase.FAsset_Code = "AS-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    fixedfromPurchase.FAsset_Code = "AS-" + (Convert.ToInt32(lastCode.FAsset_Code.Substring(3, lastCode.FAsset_Code.Length - 3)) + 1).ToString("D3");
                }
                if (fixedfromPurchase.FAsset_AgeMonth > 0)
                {
                    var endDate = fixedfromPurchase.FAsset_WorkingDate.AddMonths(fixedfromPurchase.FAsset_AgeMonth);
                    fixedfromPurchase.FAsset_AgeDay = (endDate - fixedfromPurchase.FAsset_WorkingDate).Days;
                    fixedfromPurchase.PriceOfAssetDaily = Convert.ToDecimal((fixedfromPurchase.BookValue.Value - fixedfromPurchase.JunckValue) / fixedfromPurchase.FAsset_AgeDay.Value);
                }
                fixedfromPurchase.PurchaseId = purin.Master_Id;

                db.FixedAssets.Add(fixedfromPurchase);
                db.SaveChanges();
                var filedetails = new FixedAssetFileDetail();
                var fileget = db.FixedAssetFileDetailInfoes.Where(e => e.FixedAssetInformationId1 == fixid.Id).ToList().FirstOrDefault();
                if (fileget == null)
                {
                }
                else
                {
                    filedetails.FileName = fileget.FileName;
                    filedetails.File_Type = fileget.File_Type;
                    filedetails.ContentLength = fileget.ContentLength;

                    filedetails.ProImg = fileget.ProImg;
                    filedetails.FixedAssetFasset_ID = fixedfromPurchase.Fasset_ID;
                    if (filedetails.ProImg.Length < 1000000)
                    {
                        fixedfromPurchase.FixedAssetFileDetails.Add(filedetails);
                    }
                   
                }


                db.SaveChanges();
                FixedList.Add(fixedfromPurchase);

            }
            db.Dispose();
            return FixedList;
            //}
            //else
            //{



            //    for (int i = 1; i <= purde.Product_Quantity; i++)
            //    {
            //        var fixedfromPurchase = new FixedAsset();
            //        fixedfromPurchase.FAsset_BuyDate = purin.Due_Date;
            //        fixedfromPurchase.FAsset_PurchasePrice = Convert.ToDouble(purde.Product_Price);
            //        fixedfromPurchase.BookValue = Convert.ToDouble(fixedfromPurchase.FAsset_PurchasePrice);
            //        fixedfromPurchase.Currency = purin.Invoice_Currency;
            //        fixedfromPurchase.Complex = 0;
            //        fixedfromPurchase.FAsset_AgeMonth = fixid.FAsset_AgeMonth;
            //        fixedfromPurchase.FixedAssetSecCategoryFAsset_CatID = fixid.FixedAssetSecCategoryFAsset_CatID;
            //        fixedfromPurchase.FixedAssetTypeFAsset_TypeID = fixid.FixedAssetTypeFAsset_TypeID;
            //        fixedfromPurchase.BussinesUnitBU_Id = fixid.BussinesUnitBU_Id;
            //        fixedfromPurchase.Currency = fixid.Currency;
            //        fixedfromPurchase.FAssetCat_name = fixid.FAssetCat_name;
            //        fixedfromPurchase.CompanyComp_Id = fixid.CompanyComp_Id;
            //        fixedfromPurchase.FAsset_Description = fixid.FAsset_Description;
            //        fixedfromPurchase.FAsset_Type = fixid.FAsset_Type;
            //        fixedfromPurchase.FAsset_Employee_ID = fixid.FAsset_Employee_ID;
            //        fixedfromPurchase.FAsset_WorkingDate = fixid.FAsset_WorkingDate;
            //        fixedfromPurchase.JunckValue = fixid.JunckValue;
            //        var lastCode = db.FixedAssets.OrderByDescending(c => c.Fasset_ID).FirstOrDefault();
            //        if (lastCode == null)
            //        {
            //            fixedfromPurchase.FAsset_Code = "AS-001";
            //        }
            //        else
            //        {
            //            //using string substring method to get the number of the last inserted
            //            fixedfromPurchase.FAsset_Code = "AS-" + (Convert.ToInt32(lastCode.FAsset_Code.Substring(3, lastCode.FAsset_Code.Length - 3)) + 1).ToString("D3");
            //        }

            //        db.FixedAssets.Add(fixedfromPurchase);
            //        db.SaveChanges();
            //        var filedetails = new FixedAssetFileDetail();
            //        var fileget = db.FixedAssetFileDetails.Where(e => e.FixedAssetFasset_ID == fixid.Fasset_ID).ToList().FirstOrDefault();
            //        if (fileget == null)
            //        { }
            //        else
            //        {
            //            filedetails.FileName = fileget.FileName;
            //            filedetails.File_Type = fileget.File_Type;
            //            filedetails.ContentLength = fileget.ContentLength;

            //            filedetails.ProImg = fileget.ProImg;
            //            filedetails.FixedAssetFasset_ID = fixedfromPurchase.Fasset_ID;
            //            fixedfromPurchase.FixedAssetFileDetails.Add(filedetails);
            //        }


            //        db.SaveChanges();


            //    }
            //    db.Dispose();
            //}
            //db.Dispose();
        }
        // Gets the ViewModel used for editing the InvoiceMaster 
        public static PurMasterVM GetEditViewModel(int id)
        {
            PurInvoiceMaster invoice = GetInvoiceDetailsById(id);
            PurMasterVM invoiceMasterVM = new PurMasterVM();
            invoiceMasterVM.Invoice_State = invoice.Invoice_State;
            return invoiceMasterVM;
        }
        public static List<PurchaseDetails> getMaterialOCompoundProduct(List<PurchaseDetails> Details, int Store_Id)
        {
            var db = new AccountingContainer();
            var ProductAll = db.Products.ToList();
            var project = ProductAll.Where(a => a.Prod_Type == Prod_Type.مجمع).Select(a => a.Prod_ID).ToList();
            var ProList = Details.Where(s => project.Contains(s.Product_ID.Value)).ToList();
            if (ProList.Count() == 0)
            {

            }
            else
            {
                List<PurchaseDetails> Details2 = new List<PurchaseDetails>();

                foreach (var item in ProList)
                {
                    var SubProductList = ProductAll.Where(a => a.Prod_ID == item.Product_ID).FirstOrDefault().ProductCompounds;
                    foreach (var subProduct in SubProductList)
                    {
                        var dublicate = Details.Where(a => a.Product_ID == subProduct.Product_Id).FirstOrDefault();
                      
                        if (dublicate == null)
                        {
                            var Detailnew = new PurchaseDetails();
                            Detailnew.Product_ID = subProduct.Product_Id;
                            Detailnew.Product_Quantity = subProduct.Quantity * item.Product_Quantity;
                            Detailnew.Reverse_Quantity = subProduct.Quantity * item.Product_Quantity;
                            Detailnew.Total_Amount = item.Total_Amount;
                            Detailnew.Detail_Total_Cost = item.Detail_Total_Cost;
                            Detailnew.Detail_Total_Tax = item.Detail_Total_Tax;
                            Detailnew.Product_Price = item.Total_Amount / Detailnew.Reverse_Quantity;
                            Detailnew.InvoiceMaster_Id = Details.FirstOrDefault().InvoiceMaster_Id;
                            Details.Add(Detailnew);

                        }
                        else
                        {


                            var Detailnew = new PurchaseDetails();
                            Detailnew.Product_ID= subProduct.Product_Id;
                            Detailnew.Product_Quantity = dublicate.Product_Quantity + (item.Product_Quantity * subProduct.Quantity);
                            Detailnew.Reverse_Quantity = Detailnew.Product_Quantity;
                            Detailnew.Total_Amount = dublicate.Total_Amount+item.Total_Amount;
                            Detailnew.Detail_Total_Tax = dublicate.Detail_Total_Tax + item.Detail_Total_Tax;
                            Detailnew.Detail_Total_Cost = dublicate.Detail_Total_Cost + item.Detail_Total_Cost;
                            Detailnew.Product_Price = item.Total_Amount / Detailnew.Reverse_Quantity;
                            Detailnew.InvoiceMaster_Id = Details.FirstOrDefault().InvoiceMaster_Id;
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
        ///  Updates inventory invoice to create GLs of purchase
        /// </summary>
        /// <param name="invoiceMasterVM"></param>
        /// <returns></returns>
        public static String UpdateInvoiceMaster(PurMasterVM invoiceMasterVM)
        {
            string msg = "";
            var db = new AccountingContainer();

            PurInvoiceMaster invoice = db.PurInvoiceMasters.Find(invoiceMasterVM.Master_Id);
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
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {


                    #region Create GL
                    // Add GLHeader 
                    GLMaster glHeader = new GLMaster();
                    var realDetail= getMaterialOCompoundProduct(invoice.InvoiceDetails.ToList(), invoice.Store_ID.Value);
                    glHeader.GL_Date = invoice.Invoice_Date;
                    glHeader.GL_Desc = invoice.Description;
                    glHeader.Company_Id = invoice.Company_Id;
                    glHeader.GLType_Id = "مشتريات";
                    glHeader.Currency_Id = invoice.Invoice_Currency;
                    glHeader.Auto = true;
                    // Initialize GLState to Pending when first created
                    glHeader.GLState = TransactionState.Pending;
                    glHeader.GLCode = invoice.Serial_Number;
                    glHeader.Account_Child_ID = invoice.Supplier_Id;
                    // Create five GlDetail
                    GLDetail AccountsPayableCredit = new GLDetail(),
                        AccountsPayableDebit = new GLDetail(),
                        PaymentGL = new GLDetail(),
                        taxesGL = new GLDetail(),
                        inventoryGL = new GLDetail(),
                          fixedDiscount = new GLDetail();


                    AccountsPayableCredit.Bu_Id = 0;
                    AccountsPayableDebit.Bu_Id = 0;
                    inventoryGL.Bu_Id = 0;
                    fixedDiscount.Bu_Id = 0;
                    PaymentGL.Bu_Id = 0;

                    var ChartOfAccounPay = db.ChartOfAccountings.Find(45);
                    AccountsPayableCredit.Account_Id = ChartOfAccounPay.Account_Id;
                    AccountsPayableCredit.Account_Name = ChartOfAccounPay.Account_Name;
                    AccountsPayableDebit.Account_Id = ChartOfAccounPay.Account_Id;
                    AccountsPayableDebit.Account_Name = ChartOfAccounPay.Account_Name;

                    var chartFixedDiscount = db.ChartOfAccountings.Find(233);
                    fixedDiscount.Account_Id = chartFixedDiscount.Account_Id;
                    fixedDiscount.Account_Name = chartFixedDiscount.Account_Name;


                    var taxofDetail = invoice.InvoiceDetails;
                    foreach (var Taxo in taxofDetail)
                    {
                        if (Taxo.Product_Tax == 0 || Taxo.Product_Tax == null)
                        {
                            invoice.Tax_Type = false;
                        }
                        else
                        {

                            var tax = db.Taxes.Find(Taxo.Product_Tax);
                            var ChartOfAccounTax = db.ChartOfAccountings.Where(e => e.Account_Code == tax.Tax_Code).ToList().FirstOrDefault();
                            taxesGL.Account_Id = ChartOfAccounTax.Account_Id;
                            taxesGL.Account_Name = ChartOfAccounTax.Account_Name;

                        }


                    }


                    var ChartOfAccounInventory = db.Stores.Find(invoice.Store_ID).Store_Code;
                    var storeChart = db.ChartOfAccountings.Where(a => a.Account_Code == ChartOfAccounInventory).FirstOrDefault();
                    inventoryGL.Account_Id = storeChart.Account_Id;
                    inventoryGL.Account_Name = storeChart.Account_Name;

                    if (invoice.PaymentMethod_Id > 0)
                    {
                        var ChartOfAccounPayMentMethod = db.ChartOfAccountings.Find(invoice.PaymentMethod_Id);
                        PaymentGL.Account_Id = ChartOfAccounPayMentMethod.Account_Id;
                        PaymentGL.Account_Name = ChartOfAccounPayMentMethod.Account_Name;
                        PaymentGL.GLMasterGLMaster_Id = glHeader.GLMaster_Id;
                    }


                    AccountsPayableCredit.Asset_Id =
                    AccountsPayableDebit.Asset_Id =
                    taxesGL.Asset_Id =
                    PaymentGL.Asset_Id =
                    inventoryGL.Asset_Id =
                    fixedDiscount.Asset_Id = 0;


                    if (invoice.Supplier_Id > 0)
                    {
                        var sup = db.Suppliers.Find(invoice.Supplier_Id);
                        Accounts supplSubledger = db.Accounts.Where(e => e.Account_Code == sup.Supp_Code).ToList().FirstOrDefault();
                        AccountsPayableCredit.Account_Child_ID = supplSubledger.Accounts_Id;
                        AccountsPayableCredit.Account_CHild_Name = supplSubledger.Account_Name;
                        AccountsPayableDebit.Account_Child_ID = supplSubledger.Accounts_Id;
                        AccountsPayableDebit.Account_CHild_Name = supplSubledger.Account_Name;
                    }



                    SubLedger fixedSubledger = db.SubLedgers.Where(e => e.SubLed_Code == "0").ToList().FirstOrDefault();
                    fixedDiscount.SubLedger_Id = fixedSubledger.SubLed_Id;

                    switch (invoice.Invoice_State)
                    {
                        //Post Invoice
                        //Creates Automatic GLs
                        case TransactionState.Post:
                            glHeader.GLState = TransactionState.Post;
                            var gl = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                            if (gl != null)
                            {
                                OperationRun.DeleteOperation(invoice.Serial_Number);
                              
                                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                            }

                            if (invoice.Total_Tax == 0)
                            {
                                // Supplier GL
                                AccountsPayableCredit.GLDetail_Credit = invoice.TotaL_After_Fixed.Value;
                                // Supplier GL
                                AccountsPayableDebit.GLDetail_Debit = invoice.TotaL_After_Fixed.Value;
                                // PaymentGL GL
                                PaymentGL.GLDetail_Credit = invoice.TotaL_After_Fixed.Value;
                                // Fixed discoud GL
                                fixedDiscount.GLDetail_Credit = invoice.FixedDiscount.Value;
                                // Inventory GL
                                inventoryGL.GLDetail_Debit = invoice.Total_Amount - ((invoice.Master_Discount / 100) * invoice.Total_Amount);
                            }
                            else
                            {
                                // Supplier GL
                                AccountsPayableCredit.GLDetail_Credit = invoice.TotaL_After_Fixed.Value;
                                // Supplier GL
                                AccountsPayableDebit.GLDetail_Debit = invoice.TotaL_After_Fixed.Value;
                                // PaymentGL GL
                                PaymentGL.GLDetail_Credit = invoice.TotaL_After_Fixed.Value;
                                // Fixed discoud GL
                                fixedDiscount.GLDetail_Credit = invoice.FixedDiscount.Value;
                                // Taxes GL
                                taxesGL.GLDetail_Debit = invoice.Total_Tax;
                                // Inventory GL
                                inventoryGL.GLDetail_Debit = invoice.Total_Amount - ((invoice.Master_Discount / 100) * invoice.Total_Amount);
                                glHeader.GLDetails.Add(taxesGL);
                            }

                            if (invoice.Supplier_Id > 0 && (invoice.PaymentMethod_Id == null || invoice.PaymentMethod_Id == 0))
                            {
                                glHeader.GLDetails.Add(AccountsPayableCredit);

                            }
                            else if (invoice.Supplier_Id > 0 && invoice.PaymentMethod_Id > 0)
                            {
                                glHeader.GLDetails.Add(AccountsPayableCredit);
                                glHeader.GLDetails.Add(AccountsPayableDebit);
                                glHeader.GLDetails.Add(PaymentGL);
                            }
                            else if (invoice.PaymentMethod_Id > 0 && invoice.Supplier_Id == 0)
                            {
                                glHeader.GLDetails.Add(PaymentGL);
                            }


                            glHeader.GLDetails.Add(inventoryGL);
                            if (fixedDiscount.GLDetail_Credit != 0 || fixedDiscount.GLDetail_Debit != 0)
                            {
                                glHeader.GLDetails.Add(fixedDiscount);
                            }

                            // Add a batch for each detail
                            foreach (var detail in realDetail)
                            {
                                var master = db.PurInvoiceMasters.Find(detail.InvoiceMaster_Id);
                                var batchs = db.ProductBatches.Where(e => e.Product_ID == detail.Product_ID && e.Store_ID == master.Store_ID).ToList().LastOrDefault();
                                if (batchs == null)
                                {
                                    ProductBatches batch = new ProductBatches();
                                    batch.Batch_Date = DateTime.Now;
                                    // TODO: acquire expire date from invoice
                                    batch.Batch_ExpireDate = DateTime.Now;

                                    //batch.Product_Cost = detail.Product_Price;
                                    var discount = detail.Product_Price * (master.Master_Discount / 100);
                                    batch.Product_Cost = detail.Product_Price - discount;
                                    batch.Product_ID = (int)detail.Product_ID;
                                    batch.Product_Quantity = detail.Product_Quantity;
                                    batch.PurInvoice_ID = detail.InvoiceMaster_Id;
                                    batch.Store_ID = (int)invoice.Store_ID;
                                    batch.Batch_Cost = batch.Product_Cost * batch.Product_Quantity; /*detail.Detail_Total_Cost*/;

                                    db.ProductBatches.Add(batch);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    if (batchs.Product_Cost == detail.Product_Price)
                                    {
                                        batchs.Product_Quantity += detail.Product_Quantity;
                                        var discount = detail.Product_Price * (master.Master_Discount / 100);

                                        var secnddisc = (detail.Product_Price * detail.Product_Quantity) - (discount * detail.Product_Quantity);

                                        batchs.Batch_Cost = batchs.Batch_Cost + secnddisc;
                                        batchs.Product_Cost = batchs.Batch_Cost / batchs.Product_Quantity;
                                        batchs.PurInvoice_ID = detail.InvoiceMaster_Id;
                                        db.Entry(batchs).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        batchs.Product_Quantity += detail.Product_Quantity;
                                        var discount = detail.Product_Price * (master.Master_Discount / 100);

                                        var secnddisc = (detail.Product_Price * detail.Product_Quantity) - (discount * detail.Product_Quantity);

                                        batchs.Batch_Cost = batchs.Batch_Cost + secnddisc;
                                        batchs.Product_Cost = batchs.Batch_Cost / batchs.Product_Quantity;
                                        batchs.PurInvoice_ID = detail.InvoiceMaster_Id;
                                        db.Entry(batchs).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }

                                }
                                var storecard = new StoreCard();
                                storecard.CostPrice = (detail.Detail_Total_Cost - detail.Detail_Total_Tax) / detail.Product_Quantity;
                                storecard.Date = DateTime.Now;
                                storecard.Store_Id = invoice.Store_ID.Value;
                                storecard.Quantity = detail.Product_Quantity;
                                storecard.SalePrice = 0;
                                storecard.Product_Id = detail.Product_ID.Value;
                                storecard.IncreaseQuantity = true;
                                storecard.TypeOfDocument = " فاتورة مشتريات مخزنية";
                                storecard.Document_Serial = master.Serial_Number;
                                storecard.TotalCost = detail.Detail_Total_Cost - detail.Detail_Total_Tax;
                                storecard.TotalAmount = 0;
                                storecard.Account_Id = AccountsPayableCredit.Account_Child_ID;
                                storecard.Subledger_Id = Convert.ToInt32(master.Subledger_Id);
                                Store.AddinginstorCard(storecard);

                            }

                            break;
                        // Void Invoice
                        //Creates Automatic GLs (same as GLs Created When Posted but Reversed)
                        case TransactionState.Void:
                            if (invoice.Cost_distribute == true)
                            {
                                invoice.Invoice_State = TransactionState.Post;
                                //db.SaveChanges();
                             return "تم توزيع التكلفة على هذه الفاتورة";
                            }
                            glHeader.GLType_Id = "مشتريات عكسي";
                            glHeader.GLState = TransactionState.Void;
                            var glcheck = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                            if (glcheck != null)
                            {
                                OperationRun.DeleteOperation(invoice.Serial_Number);
                                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                            }
                            var errorMessage = new List<string> { };
                            foreach (var detail in realDetail)
                            {
                                var thisproduct = db.Products.Find(detail.Product_ID);
                                if (thisproduct.Prod_Type == Prod_Type.منتج|| thisproduct.Prod_Type == Prod_Type.اولي)
                                {
                                    ProductBatches detailBatch = db.ProductBatches.Where(a => a.Product_ID == detail.Product_ID && a.Store_ID == invoice.Store_ID).ToList().FirstOrDefault();
                                    if (detailBatch.Store_ID != null)
                                    {
                                        if (detail.Product_Quantity > detailBatch.Product_Quantity)
                                        {
                                            var productName = db.Products.Find(detail.Product_ID).Prod_Name;
                                            invoice.Invoice_State = TransactionState.Post;
                                            //db.SaveChanges();
                                            errorMessage.Add(Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.Thereisnotenoughproduct + productName);

                                        }
                                    }
                                }
                            }
                            if (errorMessage.Count() != 0)
                            {


                                string combindedString = string.Join("\n", errorMessage.ToArray());
                                OperationRun.DeleteOperation(invoice.Serial_Number);
                                return combindedString;
                            }
                            var reverse = db.RecalPurchInvoiceMasters.Where(c => c.PurInvoiceMasterMaster_Id == invoice.Master_Id).ToList();
                            if (reverse.FirstOrDefault() != null)
                            {
                                msg = Resources.validate.InvoiceDepend;
                                OperationRun.DeleteOperation(invoice.Serial_Number);
                                return msg;
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

                                if (invoice.Total_Tax == 0)
                                {
                                    // Supplier GL
                                    AccountsPayableCredit.GLDetail_Debit = invoice.TotaL_After_Fixed.Value;
                                    // Supplier GL
                                    AccountsPayableDebit.GLDetail_Credit = invoice.TotaL_After_Fixed.Value;
                                    // PaymentGL GL
                                    PaymentGL.GLDetail_Debit = invoice.TotaL_After_Fixed.Value;
                                    // Fixed discoud GL
                                    fixedDiscount.GLDetail_Debit = invoice.FixedDiscount.Value;
                                    // Inventory GL
                                    inventoryGL.GLDetail_Credit = invoice.Total_Amount - ((invoice.Master_Discount / 100) * invoice.Total_Amount);
                                }
                                else
                                {
                                    // Supplier GL
                                    AccountsPayableCredit.GLDetail_Debit = invoice.TotaL_After_Fixed.Value;
                                    // Supplier GL
                                    AccountsPayableDebit.GLDetail_Credit = invoice.TotaL_After_Fixed.Value;
                                    // PaymentGL GL
                                    PaymentGL.GLDetail_Debit = invoice.TotaL_After_Fixed.Value;
                                    // Fixed discoud GL
                                    fixedDiscount.GLDetail_Debit = invoice.FixedDiscount.Value;
                                    // Taxes GL
                                    taxesGL.GLDetail_Credit = invoice.Total_Tax;
                                    // Inventory GL
                                    inventoryGL.GLDetail_Credit = invoice.Total_Amount - ((invoice.Master_Discount / 100) * invoice.Total_Amount);
                                    glHeader.GLDetails.Add(taxesGL);
                                }
                                if (invoice.Supplier_Id > 0 && (invoice.PaymentMethod_Id == null || invoice.PaymentMethod_Id == 0))
                                {
                                    glHeader.GLDetails.Add(AccountsPayableCredit);

                                }
                                else if (invoice.Supplier_Id > 0 && invoice.PaymentMethod_Id > 0)
                                {
                                    glHeader.GLDetails.Add(AccountsPayableCredit);
                                    glHeader.GLDetails.Add(AccountsPayableDebit);
                                    glHeader.GLDetails.Add(PaymentGL);
                                }
                                else if (invoice.PaymentMethod_Id > 0 && invoice.Supplier_Id == 0)
                                {
                                    glHeader.GLDetails.Add(PaymentGL);
                                }
                                glHeader.GLDetails.Add(inventoryGL);
                                if (fixedDiscount.GLDetail_Credit != 0 || fixedDiscount.GLDetail_Debit != 0)
                                {
                                    glHeader.GLDetails.Add(fixedDiscount);
                                }
                                // Remove all batches related to this invoice

                                foreach (var detail in realDetail)
                                {
                                    var batches = db.ProductBatches.Where(batch => batch.PurInvoice_ID == invoice.Master_Id).ToList();

                                    var master = db.PurInvoiceMasters.Find(detail.InvoiceMaster_Id);
                                    var batchs = db.ProductBatches.Where(e => e.Product_ID == detail.Product_ID && e.Store_ID == master.Store_ID).ToList().LastOrDefault();
                                    if (batchs.Product_Quantity <= 0)
                                    {
                                        OperationRun.DeleteOperation(invoice.Serial_Number);
                                        return "لا يوجد كمية كافية لعمل قيد الفاتورة";
                                    }
                                    else
                                    {
                                        batchs.Product_Quantity -= detail.Product_Quantity;
                                        var discount = detail.Product_Price * (master.Master_Discount / 100);

                                        var secnddisc = (detail.Product_Price * detail.Product_Quantity) - (discount * detail.Product_Quantity);

                                        batchs.Batch_Cost = batchs.Batch_Cost - secnddisc;
                                        if (batchs.Product_Quantity == 0)
                                        {
                                            batchs.Product_Cost = 0;
                                        }
                                        else
                                        {
                                            batchs.Product_Cost = batchs.Batch_Cost / batchs.Product_Quantity;
                                        }

                                        batchs.PurInvoice_ID = detail.InvoiceMaster_Id;
                                        db.Entry(batchs).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
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
                                    storecard.Store_Id = invoice.Store_ID.Value;
                                    storecard.Quantity = detail.Product_Quantity;
                                    storecard.SalePrice = 0;
                                    storecard.Product_Id = detail.Product_ID.Value;
                                    storecard.IncreaseQuantity = false;
                                    storecard.TypeOfDocument = " استبعاد فاتورة مشتريات مخزنية";
                                    storecard.Document_Serial = master.Serial_Number;
                                    storecard.TotalCost = detail.Detail_Total_Cost - detail.Detail_Total_Tax;
                                    storecard.TotalAmount = 0;
                                    storecard.Account_Id = AccountsPayableCredit.Account_Child_ID;
                                    storecard.Subledger_Id = Convert.ToInt32(master.Subledger_Id);
                                    Store.AddinginstorCard(storecard);

                                }
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion
                    if (msg == "")
                    {
                        db.GLMasters.Add(glHeader);
                        GLMaster.GetBalanceForAccount(glHeader);
                        GLMaster.GetBalanceForSubLedger(glHeader);
                        msg = Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.GldoneSuccessful;
                        db.Entry(invoice).State = EntityState.Modified;
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
            if (invoice.Invoice_State == TransactionState.Post)
            {
                msg = Resources.validate.GLDone;
            }
            else
            {
                msg = Resources.validate.GLReverse;
            }
            OperationRun.DeleteOperation(invoice.Serial_Number);
            return msg;
        }
        // Updates regular invoice to create GLs
        public static String UpdateRegularInvoiceMaster(PurMasterVM invoiceMasterVM)
        {
            String msg = "";
            var db = new AccountingContainer();

            PurInvoiceMaster invoice = db.PurInvoiceMasters.Find(invoiceMasterVM.Master_Id);
            OperationRun.Addnewoperation(invoice.Serial_Number);
            var operation = db.OperationRuns.Where(a => a.Code == invoice.Serial_Number).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(invoice.Serial_Number);
                return "يرجي الانتظار ومحاولة الاعتماد مرة اخرى ";
            }
            if (invoice.Invoice_State == invoiceMasterVM.Invoice_State)
            {
                OperationRun.DeleteOperation(invoice.Serial_Number);
                return "لقد تم عمل" + " " + invoice.Invoice_State + " " + "من قبل";
            }
            else
            {
                invoice.Invoice_State = invoiceMasterVM.Invoice_State;
            }
            if (invoice.Invoice_State == TransactionState.Void&& itCanVoid(invoice.Master_Id)==false)
            {
                return "يوجد اصول لهذه الفاتورة تم عمل لهم اهلاك";
            }
            #region Create GL
            // Add GLHeader 
            GLMaster glHeader = new GLMaster();
            glHeader.GL_Date = invoice.Invoice_Date;
            if (invoice.Description == null)
            {
                glHeader.GL_Desc = "شراء اصل ثابت";
            }
            else
            {
                glHeader.GL_Desc = invoice.Description;
            }
            glHeader.Company_Id = invoice.Company_Id;
            glHeader.GLType_Id = "فاتورة مشتريات اصول";
            glHeader.Currency_Id = invoice.Invoice_Currency;
            glHeader.GLState = TransactionState.Pending;
            glHeader.GLCode = invoice.Serial_Number;
            glHeader.Account_Child_ID = invoice.Supplier_Id;
            glHeader.Auto = true;
            //add GL Detail
            // Create five GlDetail
            GLDetail AccountsPayable = new GLDetail(),
                 taxesGL = new GLDetail(),
            fixedDiscount = new GLDetail(),
            SupplierDepit = new GLDetail(),
            PaymentGL = new GLDetail();
            // Initialize common values
            // TODO: Fix the following values
            //var BusId = db.BussinesUnits.FirstOrDefault().BU_Id;
            AccountsPayable.Bu_Id = 0;
            fixedDiscount.Bu_Id = 0;

            var chartFixedDiscount = db.ChartOfAccountings.Find(233);
            fixedDiscount.Account_Id = chartFixedDiscount.Account_Id;
            fixedDiscount.Account_Name = chartFixedDiscount.Account_Name;
            fixedDiscount.Asset_Id = 0;
            var ChartOfAccounPay = db.ChartOfAccountings.Find(45);
            AccountsPayable.Account_Id = ChartOfAccounPay.Account_Id;
            AccountsPayable.Account_Name = ChartOfAccounPay.Account_Name;
            AccountsPayable.Description_Detail = "فاتورة مشتريات";


            if (invoice.PaymentMethod_Id > 0)
            {
                var ChartOfAccounPayMentMethod = db.ChartOfAccountings.Find(invoice.PaymentMethod_Id);
                PaymentGL.Account_Id = ChartOfAccounPayMentMethod.Account_Id;
                PaymentGL.Account_Name = ChartOfAccounPayMentMethod.Account_Name;
                PaymentGL.GLMasterGLMaster_Id = glHeader.GLMaster_Id;
                var ChartOfAccounPay2 = db.ChartOfAccountings.Find(45);
                SupplierDepit.Account_Id = ChartOfAccounPay.Account_Id;
                SupplierDepit.Account_Name = ChartOfAccounPay.Account_Name;
                SupplierDepit.Description_Detail = "فاتورة مشتريات";
            }





            var supps = db.Suppliers.Find(invoice.Supplier_Id);

            var SubldgerId = db.Accounts.Where(e => e.Account_Code == supps.Supp_Code).ToList().FirstOrDefault();
            AccountsPayable.Account_Child_ID = SubldgerId.Accounts_Id; //Convert.ToInt32( SubldgerId);
                                                                       //Convert.ToInt32(SubldgerId);
            AccountsPayable.Account_CHild_Name = SubldgerId.Account_Name;
            SupplierDepit.Account_Child_ID= SubldgerId.Accounts_Id;
            SupplierDepit.Account_CHild_Name = SubldgerId.Account_Name;
            //var ChartOfAccounTax = db.ChartOfAccountings.Find(36);
            //taxesGL.Account_Id = ChartOfAccounTax.Account_Id;
            //taxesGL.Account_Name = ChartOfAccounTax.Account_Name;
            //taxesGL.Description_Detail = "فاتورة مشتريات";
            var taxofDetail = invoice.InvoiceDetails;
            foreach (var Taxo in taxofDetail)
            {
                if (Taxo.Product_Tax == 0 || Taxo.Product_Tax == null)
                {
                    invoice.Tax_Type = false;
                }
                else
                {

                    var tax = db.Taxes.Find(Taxo.Product_Tax);
                    var ChartOfAccounTax = db.ChartOfAccountings.Where(e => e.Account_Code == tax.Tax_Code).ToList().FirstOrDefault();
                    taxesGL.Account_Id = ChartOfAccounTax.Account_Id;
                    taxesGL.Account_Name = ChartOfAccounTax.Account_Name;
                    taxesGL.Description_Detail = "فاتورة مشتريات";
                }


            }
            switch (invoice.Invoice_State)
            {
                //Post Invoice
                case TransactionState.Post:
                    glHeader.GLState = TransactionState.Post;
                    var gl = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                    if (gl != null)
                    {
                        OperationRun.DeleteOperation(invoice.Serial_Number);
                        return " تم عمل قيد الفاتورة بنجاح ";
                    }

                    // Supplier GL

                    if (invoice.Total_Tax == 0)

                    {
                        if (invoice.FixedDiscount > 0)
                        {
                            AccountsPayable.GLDetail_Credit = invoice.Total_Cost - (decimal)invoice.FixedDiscount;
                            fixedDiscount.GLDetail_Credit = (decimal)invoice.FixedDiscount;
                            glHeader.GLDetails.Add(fixedDiscount);
                        }
                        else
                        {
                            AccountsPayable.GLDetail_Credit = invoice.Total_Cost;
                        }
                        
                        
                    }
                    else
                    { // Taxes GL
                        if (invoice.FixedDiscount > 0)
                        {
                            AccountsPayable.GLDetail_Credit = invoice.Total_Cost - (decimal)invoice.FixedDiscount;
                            fixedDiscount.GLDetail_Credit = (decimal)invoice.FixedDiscount;
                            glHeader.GLDetails.Add(fixedDiscount);
                        }
                        else
                        {
                            AccountsPayable.GLDetail_Credit = invoice.Total_Cost;
                        }
                        taxesGL.GLDetail_Debit = invoice.Total_Tax;
                        glHeader.GLDetails.Add(taxesGL);
                    }
                    if (invoice.PaymentMethod_Id > 0)
                    {
                        SupplierDepit.GLDetail_Debit = invoice.Total_Cost;
                        SupplierDepit.GLDetail_Credit = 0;
                        PaymentGL.GLDetail_Credit = invoice.Total_Cost;
                        PaymentGL.GLDetail_Debit = 0;
                        glHeader.GLDetails.Add(SupplierDepit);
                        glHeader.GLDetails.Add(PaymentGL);

                    }
                    // Inventory GL


                    glHeader.GLDetails.Add(AccountsPayable);







                    if (invoice.InvoiceDetails.FirstOrDefault().Asset_Id != null)
                    {
                        foreach (PurchaseDetails item in invoice.InvoiceDetails)
                        {
                            
                           var ListOfAsset= CreateFixedAfterInvoice(invoice, item);
                            var thisasset = db.FixedAssetInformations.Find(item.Asset_Id);
                            foreach (var asset in ListOfAsset)
                            {
                                GLDetail expenseAccountGL = new GLDetail();
                                expenseAccountGL.Account_Child_ID = 0;
                                expenseAccountGL.Bu_Id = 0;
                               
                                AccountsPayable.Asset_Id = 0;
                            
                                var ChartOfAccountExpense = db.ChartOfAccountings.Where(a => a.Account_Name == thisasset.FAsset_Type).FirstOrDefault();
                                expenseAccountGL.Account_Id = ChartOfAccountExpense.Account_Id;
                                expenseAccountGL.Account_Name = ChartOfAccountExpense.Account_Name;
                                expenseAccountGL.GLDetail_Debit = (item.Total_Amount - ((invoice.Master_Discount / 100) * item.Total_Amount))/item.Product_Quantity;

                                expenseAccountGL.Description_Detail = "فاتورة مشتريات";

                                taxesGL.Asset_Id = 0;
                                expenseAccountGL.Asset_Id = asset.Fasset_ID;
                                glHeader.GLDetails.Add(expenseAccountGL);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }

                    break;
                // Void Invoice
                case TransactionState.Void:
                    glHeader.GLState = TransactionState.Void;
                    var glcheck = db.GLMasters.Where(a => a.GLCode == glHeader.GLCode && a.GLState == glHeader.GLState).FirstOrDefault();
                    if (glcheck != null)
                    {
                        OperationRun.DeleteOperation(invoice.Serial_Number);
                        return " تم عمل قيد الفاتورة بنجاح";
                    }

                    if (invoice.HasOff == true)

                    {
                        msg = "قيد الاغلاق قد شمل الفاتورة  لن يتم حفظ التغيير ";
                        invoice.Invoice_State = TransactionState.Post;
                        db.SaveChanges();

                    }
                    else
                    {

                        if (invoice.Total_Tax == 0)

                        {
                            if (invoice.FixedDiscount > 0)
                            {
                                AccountsPayable.GLDetail_Debit = invoice.Total_Cost - (decimal)invoice.FixedDiscount;
                                fixedDiscount.GLDetail_Debit = (decimal)invoice.FixedDiscount;
                                glHeader.GLDetails.Add(fixedDiscount);
                            }
                            else
                            {
                                AccountsPayable.GLDetail_Debit = invoice.Total_Cost;
                            }


                        }
                        else
                        { // Taxes GL
                            if (invoice.FixedDiscount > 0)
                            {
                                AccountsPayable.GLDetail_Debit = invoice.Total_Cost - (decimal)invoice.FixedDiscount;
                                fixedDiscount.GLDetail_Debit = (decimal)invoice.FixedDiscount;
                                glHeader.GLDetails.Add(fixedDiscount);
                            }
                            else
                            {
                                AccountsPayable.GLDetail_Debit = invoice.Total_Cost;
                            }
                            taxesGL.GLDetail_Credit = invoice.Total_Tax;
                            glHeader.GLDetails.Add(taxesGL);
                        }
                        // Inventory GL
                        if (invoice.InvoiceDetails.FirstOrDefault().Asset_Id != null)
                        {
                            //foreach (PurchaseDetails item in invoice.InvoiceDetails)
                            //{
                              
                            //    expenseAccountGL.Account_Child_ID = 0;
                            //    expenseAccountGL.Bu_Id = 0;
                             
                            //    AccountsPayable.Asset_Id = 0;
                            //    var thisasset = db.FixedAssetInformations.Find(item.Asset_Id);

                         
                           
                            //    //CreateFixedAfterInvoice(invoice, item);
                            //}
                            foreach(var asset in db.FixedAssets.Where(a => a.PurchaseId == invoice.Master_Id).ToList())
                            {
                                GLDetail expenseAccountGL = new GLDetail();
                                var ChartOfAccountExpense = db.ChartOfAccountings.Where(a => a.Account_Name == asset.FAsset_Type).FirstOrDefault();
                                expenseAccountGL.Account_Id = ChartOfAccountExpense.Account_Id;
                                expenseAccountGL.Account_Name = ChartOfAccountExpense.Account_Name;
                                expenseAccountGL.GLDetail_Credit = Convert.ToDecimal( asset.FAsset_PurchasePrice.Value);

                                expenseAccountGL.Description_Detail = "فاتورة مشتريات";

                                taxesGL.Asset_Id = 0;
                                expenseAccountGL.Asset_Id = asset.Fasset_ID;
                                glHeader.GLDetails.Add(expenseAccountGL);
                                asset.FAsset_Dispose = true;
                                db.Entry(asset).State = EntityState.Modified;

                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            break;
                        }
                        if (invoice.PaymentMethod_Id > 0)
                        {
                            SupplierDepit.GLDetail_Credit = invoice.Total_Cost;
                            SupplierDepit.GLDetail_Debit = 0;
                            PaymentGL.GLDetail_Debit = invoice.Total_Cost;
                            PaymentGL.GLDetail_Credit = 0;
                            glHeader.GLDetails.Add(SupplierDepit);
                            glHeader.GLDetails.Add(PaymentGL);

                        }
                    }
                    break;
                default:
                    break;
            }
            if (msg == "")
            {
                // Add all GL details to the master

                #endregion
                glHeader.GLDetails.Add(AccountsPayable);


                //glHeader.GLDetails.Add(expenseAccountGL);
                db.GLMasters.Add(glHeader);
                GLMaster.GetBalanceForAccount(glHeader);
                GLMaster.GetBalanceForSubLedger(glHeader);
                msg = "تم عمل قيد الفاتورة بنجاح";
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
            }
            OperationRun.DeleteOperation(invoice.Serial_Number);
            return msg;
        }

        /// <summary>
        /// To delete purchase invoice by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeletePurchaseInvoice(int id)
        {
            var db = new AccountingContainer();
            var selectedPrchaseInv = db.PurInvoiceMasters.Find(id);
            if (db.GLMasters.Where(a => a.GLCode == selectedPrchaseInv.Serial_Number).FirstOrDefault() != null)
                return false;
            if (selectedPrchaseInv.Invoice_State != TransactionState.Pending)
                return false;
            db.PurchaseFileDetails.RemoveRange(selectedPrchaseInv.PurchaseFileDetails);
            db.PurchaseDetails.RemoveRange(selectedPrchaseInv.InvoiceDetails);
            db.PurInvoiceMasters.Remove(selectedPrchaseInv);


            db.SaveChanges();
            db.Dispose();
            return true;
        }

        public static PurInvoiceMaster GetpurchInvByID(int ID)
        {
            var db = new AccountingContainer();
            var purchinv = db.PurInvoiceMasters.Where(c => c.Master_Id == ID);
            var purch = new PurInvoiceMaster();
            /// here  we  was  going  to  add the disposed  Asset  but not now
            if (purchinv != null)
            {
                purch = purchinv.FirstOrDefault();
                return purch;
            }
            return null;
        }

        /// <summary>
        /// To prepare details for product
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="discount"></param>
        /// <returns></returns>
        public static PurchaseDetailsVM PreparpurchMasterDetail2(int serial, decimal discount)
        {

            var db = new AccountingContainer();
            var invoiceMasterDVM = new PurchaseDetailsVM();

            var thisdetails = GetAllTempPURCHInvoiceDetailsDetails(serial).ToList().FirstOrDefault();

            invoiceMasterDVM.Product_Quantity = thisdetails.Product_Quantity;
            if (thisdetails.Product_ID == null || invoiceMasterDVM.Product_ID == 0)
            {
                invoiceMasterDVM.Product_ID = 0;
            }
            else { invoiceMasterDVM.Product_ID = (int)thisdetails.Product_ID;
                invoiceMasterDVM.ItemDesc = thisdetails.ItemDesc;
            }
            if (thisdetails.Asset_Id == null || thisdetails.Asset_Id == 0)
            {
                invoiceMasterDVM.AssetId = 0;
            }

            else { invoiceMasterDVM.AssetId = (int)thisdetails.Asset_Id;
                invoiceMasterDVM.ItemDesc = thisdetails.ItemDesc;
            }
            invoiceMasterDVM.Discount = discount;
            invoiceMasterDVM.Details_Id = thisdetails.Details_Id;
            invoiceMasterDVM.Product_Price = thisdetails.Product_Price;
            invoiceMasterDVM.SelectProduct_Tax = (int)thisdetails.Product_Tax;
            invoiceMasterDVM.Detail_Total_Tax = thisdetails.Detail_Total_Tax;
            invoiceMasterDVM.SelectProduct_Tax = (int)thisdetails.Product_Tax;

            List<object> newList = new List<object>();
            var thistaxofchart = db.ChartOfAccountings.Find(35);
            var members = db.Taxes.Where(e => e.Tax_Code.Contains(thistaxofchart.Account_Code)).Where(a => a.IsActive != false).ToList();
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.Tax_ID,
                    Name = member.Tax_Name + "   " + member.Tax_Percentage + "%"
                });
            invoiceMasterDVM.MyProduct_TaxList = new SelectList(newList, "Id", "Name");
            invoiceMasterDVM.InvoiceMaster_Id = serial;
            List<object> newListpro = new List<object>();
            var membersPro = db.Products.Where(a => a.Prod_Type != Prod_Type.خدمة&&a.Prod_Type!=Prod_Type.مجمع);
            foreach (var member in membersPro)
                newListpro.Add(new
                {
                    Id = member.Prod_ID,
                    Name = member.Prod_Name + "   " + member.Bar_Code
                });
       
            invoiceMasterDVM.MyProductList = new SelectList(newListpro, "Id", "Name");
            var query = db.FixedAssets.DistinctBy(p => new { p.FAsset_Description });
            invoiceMasterDVM.MyAssetList = new SelectList(query, "Fasset_ID", "FAsset_Description");
            //invoiceMasterDVM.MyAssetList = new SelectList(db.FixedAssetInformations, "Id", "FAsset_Description");

            return invoiceMasterDVM;

        }
        /// <summary>
        /// To get list of details by master id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns></returns>
        public static List<PurchaseDetails> GetAllTempPURCHInvoiceDetailsDetails(int masterVmId)
        {
            var db = new AccountingContainer();
            var DetailsTemp = db.PurchaseDetails.Where(invoice => invoice.InvoiceMaster_Id == masterVmId).ToList();
            return DetailsTemp;
        }

        public static List<PurchaseDetails> GetAllTempPURCHInvoiceDetails2(int masterVmId)
        {
            var db = new AccountingContainer();
            var DetailsTemp = db.PurchaseDetails.Where(invoice => invoice.InvoiceMaster_Id == masterVmId).ToList();
            return DetailsTemp;
        }
        /// <summary>
        /// To get list of details temp
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns></returns>
        public static List<PurchaseDetailsTemp> GetAllTempPurchInvoiceDetailsresult2(int masterVmId)
        {
            var db = new AccountingContainer();
            var PurchaseTemp = db.PurInvoiceMasterTemps.Find(masterVmId);
            return PurchaseTemp.PurchaseDetailsTemps.ToList();
        }
        /// <summary>
        /// To add product details to list
        /// </summary>
        /// <param name="Detail"></param>
        /// <returns></returns>
        public static string AddPurchDetailsTemp(PurchaseDetailsVM Detail)
        {
            var db = new AccountingContainer();
            if ((Detail.Product_ID == null || Detail.Product_ID == 0))
            {
                if(Detail.AssetId == 0 || Detail.AssetId == null)
                {
                    return "من فضلك اختر الصنف";
                }
            }
           
               
           
            if (Detail.Product_Price == null || Detail.Product_Price <= 0)
            {
                return "من فضلك ادخل السعر ";
            }
            if (Detail.Product_Quantity == null || Detail.Product_Quantity <= 0)
            {
                return "من فضلك ادخل الكمية ";
            }

            var invoiceMasterTemp = db.PurInvoiceMasterTemps.Find(Detail.InvoiceMaster_Id);

            var invoiceMasterDetailsTemp = new PurchaseDetailsTemp();

            var details = invoiceMasterTemp.PurchaseDetailsTemps;
            var duplecatTemp =new List<PurchaseDetailsTemp>();
            if (Detail.Product_ID == null)
            {
                duplecatTemp = details.Where(p => p.PurInvoiceMasterTempMaster_Id == invoiceMasterTemp.Master_Id && p.Asset_Id == Detail.AssetId).ToList();

            }
            else
            {
                duplecatTemp = details.Where(p => p.PurInvoiceMasterTempMaster_Id == invoiceMasterTemp.Master_Id && p.Product_ID == Detail.Product_ID).ToList();

            }

            if (duplecatTemp.FirstOrDefault() != null)
            {
                return "هذا المنتج موجود بالفعل فى القائمة لايمكن اضافته مرة اخرى";
            }
            else
            {
                var resultTax = db.Taxes.Find(Detail.SelectProduct_Tax);
                if (resultTax != null)
                {

                    invoiceMasterDetailsTemp.Product_Tax = resultTax.Tax_ID;

                }
                var a = db.Taxes.Find(Detail.SelectProduct_Tax);
                if (Detail.Discount != null || Detail.Discount != 0)
                { invoiceMasterTemp.Master_Discount = Detail.Discount; }
                else
                {
                }

                invoiceMasterDetailsTemp.Product_ID = Detail.Product_ID;
                var thisproduct = db.Products.Find(Detail.Product_ID);
                if (thisproduct != null)
                {
                    invoiceMasterDetailsTemp.ItemDesc = thisproduct.Prod_Name;
                    invoiceMasterDetailsTemp.Product_Code = thisproduct.Prod_Code;
                }
                else
                {
                    var thisasset = db.FixedAssets.Find(Detail.AssetId);
                    if (thisasset != null)
                    {
                        invoiceMasterDetailsTemp.ItemDesc = thisasset.FAsset_Description;
                        invoiceMasterDetailsTemp.Product_Code = thisasset.FAsset_Code;
                        invoiceMasterDetailsTemp.Asset_Id = Detail.AssetId;
                    }
                    else
                    {
                        return "هذا الصنف غير موجود";
                    }
                }

                invoiceMasterDetailsTemp.Product_Quantity = (short)Detail.Product_Quantity;
                invoiceMasterDetailsTemp.Product_Price = Detail.Product_Price;
                invoiceMasterDetailsTemp.Detail_ID_Edit = Detail.Details_Id;
                invoiceMasterDetailsTemp.Total_Amount = invoiceMasterDetailsTemp.Product_Price * invoiceMasterDetailsTemp.Product_Quantity;
                var dicount = (invoiceMasterTemp.Master_Discount / 100) * invoiceMasterDetailsTemp.Total_Amount;
                if (dicount == 0 || dicount == null)
                {

                }
                else
                {
                    invoiceMasterDetailsTemp.Total_Amount = (decimal)invoiceMasterDetailsTemp.Total_Amount - (decimal)dicount;
                }



              
                if (resultTax == null)
                {
                    invoiceMasterDetailsTemp.Detail_Total_Cost = (invoiceMasterDetailsTemp.Total_Amount);
                    invoiceMasterDetailsTemp.Detail_Total_Tax = 0;
                }
                else
                {
                    invoiceMasterDetailsTemp.Detail_Total_Cost = (invoiceMasterDetailsTemp.Total_Amount) + (invoiceMasterDetailsTemp.Detail_Total_Tax);
                    invoiceMasterDetailsTemp.Detail_Total_Tax = (invoiceMasterDetailsTemp.Total_Amount) * (Convert.ToDecimal(a.Tax_Percentage) / 100);
                }
                

                invoiceMasterTemp.PurchaseDetailsTemps.Add(invoiceMasterDetailsTemp);
                db.Entry(invoiceMasterTemp).State = EntityState.Modified;

                db.SaveChanges();
                return "تم الاضافة  بنجاح ";
            }
           
        }
        /// <summary>
        /// To fill data for editing purchase invoice by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PurMasterVM FillTheViewModel(int id,int isCopy)
        {
            var db = new AccountingContainer();
            var editModel = GetpurchInvByID(id);
            PurMasterVM PVM = new PurMasterVM();
            PVM.Supp_Inv_Serial = editModel.Supp_Inv_Serial;
            PVM.Master_Id = editModel.Master_Id;
            PVM.Serial_Number = editModel.Serial_Number;
            PVM.isCopy = isCopy;
                PVM.SelectedSupplierId = editModel.Supplier_Id;
            

            PVM.MySupplierList = new SelectList(db.Suppliers.Where(a=>a.Supplier_state!=Customer_State.غير_نشط), "Supp_Id", "Supp_Name");
            if (isCopy == 1)
            {
                PVM.Invoice_Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
            }
            else
            {
                PVM.Invoice_Date = editModel.Invoice_Date.ToString("dd-MM-yyyy   HH:mm:ss");
            }
           
            PVM.Description = editModel.Description;
            PVM.GL_Date = editModel.GL_Date.ToString();
         

            var cur = db.Currencies.Where(c => c.Currency_Type == editModel.Invoice_Currency);
            if (cur.Any())
            {
                PVM.SelectedCurrencyId = cur.FirstOrDefault().Currency_Id;
            }

            var PaymentId = db.ChartOfAccountings.Where(c => c.Account_Id == editModel.PaymentMethod_Id);
            if (PaymentId.Any())
            {
                PVM.PaymentMethod_Id = PaymentId.FirstOrDefault().Account_Id;
            }
            PVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            var com = db.Companies.Where(c => c.Comp_Id == editModel.Company_Id);
            if (com.Any())
            {
                PVM.SelectedCompany_Id = com.FirstOrDefault().Comp_Id;
            }
            PVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            var bu = db.BussinesUnits.Where(c => c.BU_Id == editModel.BU_Id);
            if (bu.Any())
            {
                PVM.SelectedBU_Id = bu.FirstOrDefault().BU_Id;
            }
            PVM.MyBussinesUnitList = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            var store = db.Stores.Where(c => c.Store_ID == editModel.Store_ID).ToList().FirstOrDefault();
            if (store != null)
            {
                PVM.SelectedStoreId = store.Store_ID; }
            else
            {
                PVM.SelectedStoreId = 0;
            }


            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false).ToList();

            PVM.PaymentMethodList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("01012")), "Account_Id", "Account_Name");

            PVM.MyStoresList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            if (editModel.Payment_Policy == null)
            {
                PVM.Payment_Policy = 0;
            }
            else
            {
                PVM.Payment_Policy = editModel.Payment_Policy.Value;
            }

            //PVM.Exchange_Rate = (decimal)editModel.Exchange_Rate;
            PVM.Master_Discount = editModel.Master_Discount;
            if (editModel.FixedDiscount > 0)
            {
                PVM.Fixed_Discount = (decimal)editModel.FixedDiscount;
            }
            else
            {
                PVM.Fixed_Discount = 0;
            }
            
            PVM.Tax_Type = editModel.Tax_Type;
            PVM.Document_Type = editModel.Document_Type;
            PVM.Due_Date = Convert.ToDateTime(editModel.Due_Date).Day + "-" + Convert.ToDateTime(editModel.Due_Date).Month + "-" + Convert.ToDateTime(editModel.Due_Date).Year;
            // PVM.InvoiceDetails= editModel.InvoiceDetails.ToList();
            PVM.Payment_Policy = editModel.Payment_Policy.Value;
            var purchMasterTemp = new PurInvoiceMasterTemp();
            purchMasterTemp.Account_Id = editModel.Account_Id;
            purchMasterTemp.BU_Id = PVM.SelectedBU_Id;
            purchMasterTemp.Company_Id = PVM.SelectedCompany_Id;
            purchMasterTemp.Description = editModel.Description;
            purchMasterTemp.Document_Type = editModel.Document_Type;
            //purchMasterTemp.Due_Date =Convert.ToDateTime(Convert.ToDateTime(editModel.Due_Date).Day+"-"+ Convert.ToDateTime(editModel.Due_Date).Month+"-"+ Convert.ToDateTime(editModel.Due_Date).Year);
            purchMasterTemp.Exchange_Rate = editModel.Exchange_Rate;
            purchMasterTemp.GL_Date = editModel.GL_Date;
            purchMasterTemp.Invoice_Currency = editModel.Invoice_Currency;
            purchMasterTemp.Invoice_Date = editModel.Invoice_Date;
            purchMasterTemp.Invoice_State = editModel.Invoice_State;
            purchMasterTemp.Master_Discount = editModel.Master_Discount;

            purchMasterTemp.Serial_Number = editModel.Serial_Number;
            purchMasterTemp.Store_ID = PVM.SelectedStoreId;
            purchMasterTemp.Subledger_Id = editModel.Supplier_Id;
            purchMasterTemp.Supplier_Id = editModel.Supplier_Id;
            purchMasterTemp.Tax_Type = editModel.Tax_Type;
            purchMasterTemp.TotalAmountAfMat = editModel.TotalAmountAfMat;
            purchMasterTemp.Total_Amount = editModel.Total_Amount;
            purchMasterTemp.Total_Cost = editModel.Total_Cost;
            purchMasterTemp.Total_Tax = editModel.Total_Tax;
            purchMasterTemp.Payment_Policy = editModel.Payment_Policy;
            var ThereORN = db.PurInvoiceMasterTemps.Find(editModel.Master_Id);
            if (ThereORN != null)
            {

            }
            else
            {
                purchMasterTemp.Master_Id = editModel.Master_Id;

                db.PurInvoiceMasterTemps.Add(purchMasterTemp);
            }
            if (PVM.isCopy == 1)
            {
                var lastSerial = db.PurInvoiceMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                if (lastSerial == null)
                {
                    PVM.Serial_Number = "PUR-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    PVM.Serial_Number = "PUR-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
                }
            }

            db.SaveChanges();


            return PVM;
        }
        /// <summary>
        /// To edit purchase invoice 
        /// </summary>
        /// <param name="mastertemps"></param>
        /// <returns></returns>
        public static Tuple<string,int> editinvoice(PurMasterVM mastertemps)
        {
            var db = new AccountingContainer();
            var purchinvoice = db.PurInvoiceMasters.Find(mastertemps.Master_Id);
            var mastertempe = db.PurInvoiceMasterTemps.ToList().LastOrDefault();
            purchinvoice.Company_Id = db.Companies.FirstOrDefault().Comp_Id;
            purchinvoice.Supp_Inv_Serial = mastertemps.Supp_Inv_Serial;
            purchinvoice.Serial_Number = purchinvoice.Serial_Number;
            purchinvoice.FixedDiscount = mastertemps.Fixed_Discount;
            purchinvoice.Description = mastertemps.Description;
            purchinvoice.PaymentMethod_Id = mastertemps.PaymentMethod_Id;
            purchinvoice.Payment_Policy = mastertemps.Payment_Policy;
            purchinvoice.Invoice_Date = Convert.ToDateTime(mastertemps.Invoice_Date);
            purchinvoice.Due_Date = purchinvoice.Invoice_Date.AddMonths(purchinvoice.Payment_Policy.Value);
            
            purchinvoice.Master_Discount = (decimal)mastertemps.Master_Discount;
            if (mastertemps.Files.FirstOrDefault() != null)
            {
                foreach (var file in mastertemps.Files)
                {
                    var filedetails = new PurchaseFileDetail();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.FileContent = data;
                    filedetails.FileType = file.ContentType;
                    if (data.Length < 1000000)
                    {
                        purchinvoice.PurchaseFileDetails.Add(filedetails);
                    }

                   
                }
            }

            var cur = db.Currencies.FirstOrDefault(); 
         
                purchinvoice.Invoice_Currency = cur.Currency_Type;
          

            //purchinvoice.Total_Amount = (decimal)mastertempe.Total_Amount;
            //purchinvoice.Total_Tax = (decimal)mastertempe.Total_Tax;
            // purchinvoice.TotalAmountAfMat = (decimal)mastertempe.TotalAmountAfMat;
            purchinvoice.Document_Type = 0;
            purchinvoice.Invoice_State = TransactionState.Pending;

            var resultSupplier = db.Suppliers.Find(mastertemps.SelectedSupplierId);
            if (resultSupplier != null)
            {
                purchinvoice.Supplier_Id = resultSupplier.Supp_Id;
            }
            else
            {
                purchinvoice.Supplier_Id = 0;
            }
            if (mastertemps.SelectedStoreId == 0 || mastertemps.SelectedStoreId == null)
            {
             

            }
            else
            {
                purchinvoice.Store_ID = mastertemps.SelectedStoreId;
            }


            purchinvoice.Subledger_Id = 0;
            purchinvoice.Account_Id = mastertemps.SelectedAccount_Id;
            purchinvoice.BU_Id = 0;
            purchinvoice.Tax_Type = (bool)mastertemps.Tax_Type;
            purchinvoice.Total_Amount = 0;
            purchinvoice.Total_Tax = 0;
            purchinvoice.Total_Cost = 0;
            purchinvoice.Payed_Amount = 0;
            //if (purchinvoice.InvoiceDetails == null || purchinvoice.InvoiceDetails.Count == 0)
            //{

            //}
            var masterTempp = db.PurInvoiceMasterTemps.Find(mastertemps.Master_Id);
            List<PurchaseDetailsTemp> Listtemp = new List<PurchaseDetailsTemp>();
            if (masterTempp.PurchaseDetailsTemps.Count()==0)
            {
                return Tuple.Create("محتويات الفاتورة فارغة لم يتم حفظها  ", 0);
            }
         
            foreach (var item in masterTempp.PurchaseDetailsTemps)
            {
                if (item.Asset_Id != null && item.Asset_Id != 0)
                {
                    var deletedDetails = db.PurchaseDetails.Where(r => r.InvoiceMaster_Id == item.PurInvoiceMasterTempMaster_Id).ToList();
                    db.PurchaseDetails.RemoveRange(deletedDetails);
                    var details = new PurchaseDetails();
                    // details.Details_Id = item.Details_Id;
                    details.Asset_Id = item.Asset_Id;                
                    details.ItemDesc = db.FixedAssetInformations.Where(a => a.FAsset_Code == item.Product_Code).FirstOrDefault().FAsset_Description;
                    details.Product_Quantity = item.Product_Quantity;
                    details.Product_Price = item.Product_Price;
                    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    decimal discount = TotalBeforeDiscount * (purchinvoice.Master_Discount / 100);
                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                    if (item.Product_Tax == 0)
                    {
                        details.Detail_Total_Tax = 0;
                    }
                    else
                    {
                        var tax = Convert.ToDecimal(db.Taxes.Find(item.Product_Tax).Tax_Percentage);
                        details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                    }
                    
                    details.Total_Amount = TotalBeforeDiscount;
                    details.Detail_Total_Cost = TotalAfterDiscount + details.Detail_Total_Tax;
                    purchinvoice.Total_Amount += details.Total_Amount;
                    purchinvoice.Total_Tax += details.Detail_Total_Tax;
                    purchinvoice.Total_Cost += details.Detail_Total_Cost;
                    
                    details.Item_Code = item.Product_Code;
                    // Deduct the discount from the detail Total_Amount 

                    // details.Total_Amount = item.Total_Amount - (item.Total_Amount * (masterVM.Master_Discount / 100));

                    // details.Total_Amount = item.Total_Amount; //- (item.Total_Amount * (masterVM.Master_Discount / 100));
                    details.Product_Tax = Convert.ToInt32(item.Product_Tax);
                    var thistax = db.Taxes.Find(details.Product_Tax);
                    if (thistax == null)
                    {
                        details.Tax_Name = "";
                    }
                    else
                    {
                        details.Tax_Name = thistax.Tax_Name;
                    }
                    //var thisProduct = db.Products.Find(item.Product_ID);
                    //if (thisProduct != null)
                    //{
                    //    details.ItemDesc = thisProduct.Prod_Name;
                    //}
                    //else
                    //{
                    //    var thisAsset = db.FixedAssets.Find(item.Asset_Id);
                    //    if (thisAsset == null)
                    //    {
                    //        details.ItemDesc = "";
                    //    }
                    //    else
                    //    {
                    //        details.ItemDesc = thisAsset.FAsset_Description;
                    //    }
                    //}

                   
                   
                    purchinvoice.InvoiceDetails.Add(details);
                    Listtemp.Add(item);
                }
                else
                {
                    var deletedDetails = db.PurchaseDetails.Where(r => r.InvoiceMaster_Id == item.PurInvoiceMasterTempMaster_Id).ToList();
                    db.PurchaseDetails.RemoveRange(deletedDetails);
                    var details = new PurchaseDetails();
                    // details.Details_Id = item.Details_Id;
                    details.Product_ID = item.Product_ID;
                    details.Product_Quantity = item.Product_Quantity;
                    details.Reverse_Quantity = details.Product_Quantity;
                    details.Product_Price = item.Product_Price;
                    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    decimal discount = TotalBeforeDiscount * (purchinvoice.Master_Discount / 100);

                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                    if(item.Product_Tax >0)
                    {
                        var tax = Convert.ToDecimal(db.Taxes.Find(item.Product_Tax).Tax_Percentage);
                        details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                    }
                    else
                    {
                        details.Detail_Total_Tax = 0;
                    }
                    
                    
                    details.Total_Amount = TotalBeforeDiscount;
                    details.Detail_Total_Cost = TotalAfterDiscount + details.Detail_Total_Tax;
                    details.Reverse_Total_Tax = item.Detail_Total_Tax;
                    details.Reverse_Tota_Cost = details.Detail_Total_Cost;
                    purchinvoice.Total_Amount += details.Total_Amount;
                    purchinvoice.Total_Tax += details.Detail_Total_Tax;
                    purchinvoice.Total_Cost += details.Detail_Total_Cost;

                    details.ItemDesc = item.ItemDesc;
                    details.Item_Code = item.Product_Code;
                    // Deduct the discount from the detail Total_Amount 

                    // details.Total_Amount = item.Total_Amount - (item.Total_Amount * (masterVM.Master_Discount / 100));

                    // details.Total_Amount = item.Total_Amount; //- (item.Total_Amount * (masterVM.Master_Discount / 100));
                    details.Product_Tax = Convert.ToInt32(item.Product_Tax);
                    var thistax = db.Taxes.Find(details.Product_Tax);
                    if (thistax == null)
                    {
                        details.Tax_Name = "";
                    }
                    else
                    {
                        details.Tax_Name = thistax.Tax_Name;
                    }
                    var thisProduct = db.Products.Find(item.Product_ID);
                    if (thisProduct != null)
                    {
                        details.ItemDesc = thisProduct.Prod_Name;
                    }
                    else
                    {
                        var thisAsset = db.FixedAssets.Find(item.Asset_Id);
                        if (thisAsset == null)
                        {
                            details.ItemDesc = "";
                        }
                        else
                        {
                            details.ItemDesc = thisAsset.FAsset_Description;
                        }
                    }

                    
                    
                    purchinvoice.InvoiceDetails.Add(details);
                    Listtemp.Add(item);
                }

              
                

            }
            foreach (var deleteditem in Listtemp)
            {
                db.PurchaseDetailsTemps.Remove(deleteditem);
            }
           
            purchinvoice.Payed_Amount.Value.Equals(0);
 
            purchinvoice.TotaL_After_Fixed = purchinvoice.Total_Cost - purchinvoice.FixedDiscount;
            purchinvoice.TotalAmountAfMat = purchinvoice.TotaL_After_Fixed.Value;
            purchinvoice.Payed_Amount = (decimal)0;
            db.Entry(purchinvoice).State = EntityState.Modified;
            db.PurInvoiceMasterTemps.Remove(mastertempe);
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

            return Tuple.Create("تم الحفظ بنجاح", purchinvoice.Master_Id);
        }
        /// <summary>
        /// To make drop down list for supplier list
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetSupplierList()
        {
            var db = new AccountingContainer();
            PurMasterVM vm = new PurMasterVM();

            var supplierList = from supp in db.Suppliers.Where(a=>a.Supplier_state==Customer_State.نشط)
                       select new SelectListItem
                       {
                           Text = supp.Supp_Name ,
                           Value = supp.Supp_Id.ToString()
                       };
          
            return supplierList;
           
        }
        /// <summary>
        ///  To make drop down list for currency list
        /// </summary>
        /// <returns></returns>
        public static PurMasterVM GetCurrencyList()
        {
            var db = new AccountingContainer();
            PurMasterVM vm = new PurMasterVM();

            vm.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");

            return vm;
        }
        /// <summary>
        /// To calculate due date from invoice date
        /// </summary>
        /// <param name="pur"></param>
        /// <returns></returns>
        public static PurMasterVM GetDueDate(PurMasterVM pur)
        {
            var db = new AccountingContainer();
            PurMasterVM vm = new PurMasterVM();
            var thiss = Convert.ToDateTime(pur.Invoice_Date).AddDays((int)pur.Payment_Policy);
            vm.Due_Date = thiss.ToString();
            return vm;
        }
        public static PurMasterVM GetDueDate1(string date, int polcy)
        {
            var db = new AccountingContainer();
            PurMasterVM vm = new PurMasterVM();
            var thiss = Convert.ToDateTime(date).AddDays(polcy);
            vm.Due_Date = thiss.ToString();
            return vm;
        }

        /// <summary>
        /// To git  list from product in popup view 
        /// </summary>
        /// <returns></returns>
    
        public static IEnumerable<SelectListItem> GetProudectList()
        {
            var db = new AccountingContainer();
            var prod = from prodc in db.Products
                       select new SelectListItem
                       {
                           Text = prodc.Prod_Name + " " + prodc.Bar_Code,
                           Value = prodc.Prod_ID.ToString()
                       };
           
          
            return prod;
        }

        /// <summary>
        /// To get list from assets in popup view
        /// </summary>
        /// <returns></returns>
        public static PurchaseDetailsVM GetAssetctList()
        {
            var db = new AccountingContainer();
            PurchaseDetailsVM vm = new PurchaseDetailsVM();
            vm.MyAssetList = new SelectList(db.FixedAssetInformations, "Id", "FAsset_Description");
            return vm;
        }

        /// <summary>
        /// To get prepare for transfer from purchase order by id
        /// </summary>
        /// <param name="offerMasterId"></param>
        /// <returns></returns>
        public static PurMasterVM AddNewOfferForCreation(int offerMasterId)
        {

            var db = new AccountingContainer();

            var order = db.PurchaseOrderMasters.Find(offerMasterId);
            var purmaster = new PurMasterVM();
            purmaster.order_Id = offerMasterId;
            purmaster.Description = order.Description;
            purmaster.Master_Discount = order.Master_Discount;
            purmaster.Purch_Order_Code = order.Serial_Number;
            var currencyResult = db.Currencies.Where(a => a.Currency_Type == order.Invoice_Currency).FirstOrDefault();
            if (currencyResult != null)
            {
                purmaster.SelectedCurrencyId = currencyResult.Currency_Id;
            }
            var lastSerial = db.PurInvoiceMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                purmaster.Serial_Number = "PUR-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                purmaster.Serial_Number = "PUR-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
            }
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false).ToList();

            purmaster.PaymentMethodList = new SelectList(lista.Where(accCode => accCode.Account_Code.StartsWith("010120001") || accCode.Account_Code.StartsWith("010120002")), "Account_Id", "Account_Name");
            purmaster.Document_Type = order.Document_Type;
            purmaster.Invoice_State = TransactionState.Pending;
            //purmaster.SelectedSupplierId = order.;
            purmaster.SelectedStoreId = order.Store_Id;
            purmaster.Company_Id = order.Company_Id;
            if (order.BU_Id != null)
            { purmaster.SelectedBussinesUnitId = order.BU_Id.Value; }
            else
            { purmaster.SelectedBussinesUnitId = 0; }

            purmaster.Reference = order.Reference;
            purmaster.SelectedEmp_Id = order.Emp_Id;
            purmaster.SelectedBranch_Id = order.Branch_Id;
            if (order.Payment_Policy == null)
            {
                order.Payment_Policy = 0;
            }
            else
            {
                purmaster.Payment_Policy = order.Payment_Policy.Value;
            }
            purmaster.SelectedSupplierId = order.Supplier_Id;
            purmaster.Invoice_Date = Convert.ToString(DateTime.Now);
            purmaster.Due_Date = Convert.ToString(DateTime.Now.AddDays(order.Payment_Policy.Value));


            //List<PurchaseDetailsVM> _myDetailsVM = new List<PurchaseDetailsVM>();
            //foreach (var item in order.PurchaseOrderDetails)
            //{

            //    var detailPur = new PurchaseDetailsVM ();
            //    detailPur.Product_ID= item.Product_ID;
            //    detailPur.Product_Quantity = item.Product_Quantity;
            //    detailPur.Product_Price = item.Product_Price;
            //    detailPur.Product_ID = item.Product_ID;
            //    detailPur.SelectProduct_Tax = Convert.ToInt32(item.Detail_Total_Tax);
            //    detailPur.Total_Amount = item.Total_Amount;

            //    purmaster.Total_Amount += item.Total_Amount;
            //    _myDetailsVM.Add(detailPur);

            //}
            purmaster.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            purmaster.MySupplierList = new SelectList(db.Suppliers, "Supp_Id", "Supp_Name");
            purmaster.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            purmaster.MyBussinesUnitList = new SelectList(db.SubLedgers.Where(a=>a.isActive!=false), "BU_Id", "BU_Name");
            purmaster.MyProductsList = new SelectList(db.FixedAssets, "Fasset_ID", "Fasset_Code");
            purmaster.MyStoresList = new SelectList(db.Stores.Where(a=>a.isActive!=false), "Store_ID", "Store_Name");

            purmaster.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.isActive != false), "SubLed_Code", "SubLed_Name");
            purmaster.AccountsList = new SelectList(db.ChartOfAccountings.Where(a => a.isActive != false), "Account_Id", "Account_Name");
            purmaster.BU_List = new SelectList(db.BussinesUnits, "BU_Id", "BU_Name");
            purmaster.MyBranchList = new SelectList(db.Branches.Where(a => a.IsActive != false), "Branch_Id", "Branch_Name");
            purmaster.MyEmpList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            var purchaseMasterTemp = new PurInvoiceMasterTemp();
            var lastID = (from s in db.PurInvoiceMasterTemps
                          select s.Master_Id).ToList().LastOrDefault();
            purchaseMasterTemp.Master_Id = lastID + 1;
            purchaseMasterTemp.AllowDiscount = 0;
            db.PurInvoiceMasterTemps.Add(purchaseMasterTemp);

            db.SaveChanges();
            if (purmaster.Master_Id == purchaseMasterTemp.Master_Id)
            { purmaster.Master_Id = purchaseMasterTemp.Master_Id + 2; }
            else
            {
                purmaster.Master_Id = purchaseMasterTemp.Master_Id;
            }
            //    purmaster.myDetailsVM = _myDetailsVM;
            return purmaster;
        }

        /// <summary>
        /// To create a purchase invoice from purchase order 
        /// </summary>
        /// <param name="PurVM"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static String AddNewOfferForCreation(PurMasterVM PurVM , String UserId)
        {
            var db = new AccountingContainer();
            var detail = db.PurchaseOrderDetails.Where(a => a.InvoiceMaster_Id == PurVM.order_Id).ToList();
            var purchaseMaster = new PurInvoiceMaster();
            purchaseMaster.Branch_Id = PurVM.SelectedBranch_Id;
            purchaseMaster.Description = PurVM.Description;
            var lastSerial = db.PurInvoiceMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                purchaseMaster.Serial_Number = "PUR-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                purchaseMaster.Serial_Number = "PUR-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
            }
            purchaseMaster.Invoice_Date = DateTime.Now;
            purchaseMaster.Due_Date = purchaseMaster.Invoice_Date.AddDays(PurVM.Payment_Policy);
            purchaseMaster.GL_Date = DateTime.Now;
            purchaseMaster.Master_Discount = PurVM.Master_Discount;
            var currResult = db.Currencies.Find(Convert.ToInt32(PurVM.SelectedCurrencyId));
            if (currResult != null)
            {
                purchaseMaster.Invoice_Currency = currResult.Currency_Type;

            }
            purchaseMaster.Tax_Type = PurVM.Tax_Type;
            purchaseMaster.Total_Amount = PurVM.Total_Amount;
            purchaseMaster.Total_Tax = PurVM.Total_Tax;
            purchaseMaster.Document_Type = PurVM.Document_Type;
            purchaseMaster.Invoice_State = TransactionState.Pending;
            purchaseMaster.Supplier_Id = PurVM.SelectedSupplierId.Value;
            purchaseMaster.Store_ID = PurVM.SelectedStoreId;
            purchaseMaster.Company_Id = PurVM.SelectedCompany_Id;
            purchaseMaster.TotalAmountAfMat = PurVM.Total_Amount;
            purchaseMaster.Subledger_Id = PurVM.SelectedSupplierId;
            purchaseMaster.BU_Id = PurVM.SelectedBussinesUnitId;
            purchaseMaster.Tax_Type = PurVM.Tax_Type;
            purchaseMaster.Reference = PurVM.Reference;
            purchaseMaster.Payment_Policy = PurVM.Payment_Policy;
            purchaseMaster.Emp_Id = PurVM.SelectedEmp_Id;
            if (PurVM.Files.FirstOrDefault() != null)
            {
                foreach (var file in PurVM.Files)
                {
                    var filedetails = new PurchaseFileDetail();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.FileContent = data;
                    filedetails.FileType = file.ContentType;
                    if (data.Length < 1000000)
                    {
                        purchaseMaster.PurchaseFileDetails.Add(filedetails);
                    }
                   
                }
            }

            foreach (var item in detail)
            {

                var detailPur = new PurchaseDetails();
                detailPur.Product_ID = item.Product_ID;
                detailPur.Product_Quantity = item.Product_Quantity;
                detailPur.Product_Price = item.Product_Price;
                var product = db.Products.Find(item.Product_ID);
                detailPur.ItemDesc = product.Prod_Name;
                detailPur.Item_Code = product.Prod_Code;


                decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                decimal discount = TotalBeforeDiscount * (purchaseMaster.Master_Discount / 100);
                decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                decimal tax = 0;

                if (item.Product_Tax == null || item.Product_Tax == null)
                {
                    detailPur.Product_Tax = 0;
                    detailPur.Detail_Total_Tax = 0;

                }
                else
                {
                    detailPur.Product_Tax = Convert.ToInt32(item.Product_Tax);
                    tax = Convert.ToDecimal(db.Taxes.Find(detailPur.Product_Tax).Tax_Percentage);
                    detailPur.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                    detailPur.Tax_Name = db.Taxes.Find(detailPur.Product_Tax).Tax_Name;
                }


              
                detailPur.Detail_Total_Cost = (TotalBeforeDiscount + detailPur.Detail_Total_Tax) - discount;

                detailPur.Total_Amount = TotalBeforeDiscount ;

                detailPur.Reverse_Quantity = detailPur.Product_Quantity;
                detailPur.Reverse_Total_Tax = detailPur.Detail_Total_Tax;
                detailPur.Reverse_Tota_Cost = detailPur.Detail_Total_Cost;
                purchaseMaster.Total_Cost += detailPur.Detail_Total_Cost;
                purchaseMaster.Total_Amount += detailPur.Total_Amount;
                purchaseMaster.Total_Tax += detailPur.Detail_Total_Tax;
                purchaseMaster.Company_Id = db.Companies.FirstOrDefault().Comp_Id;
                purchaseMaster.InvoiceDetails.Add(detailPur);

            }
            if (purchaseMaster.Total_Tax == 0)
            {
                purchaseMaster.Tax_Type = false;
            }
            var trans = new TransactionBatch();
            trans.Description = PurVM.Description;
            trans.Date = Convert.ToDateTime(PurVM.Invoice_Date);
            trans.OperationName = "فاتورة مشتريات من امر شراء";
            trans.UserId = UserId;
            trans.Operation_Code = PurVM.Serial_Number;
            purchaseMaster.Serial_Number = PurVM.Serial_Number;
            purchaseMaster.Purch_Order_Code = PurVM.Purch_Order_Code;
            purchaseMaster.Payed_Amount = 0;
            purchaseMaster.TotalAmountAfMat = purchaseMaster.Total_Cost;
            db.PurInvoiceMasters.Add(purchaseMaster);
            db.SaveChanges();
            db.Dispose();
            trans.OperationId = purchaseMaster.Master_Id;
            TransactionBatch.AddNewTransaction(trans);
            return "تم انشاء الفاتورة بنجاح";
        }
        /// <summary>
        /// To get product details by order id
        /// </summary>
        /// <param name="Order"></param>
        /// <returns></returns>
        public static List<PurchaseOrderDetail> getDetailFromOrder(int Order)
        {
            var db = new AccountingContainer();
            var detail = db.PurchaseOrderDetails.Where(a => a.InvoiceMaster_Id == Order).ToList();
            return detail;
        }
        /// <summary>
        /// To delete product by id from list of details
        /// </summary>
        /// <param name="purch"></param>
        /// <returns></returns>
        public static PurchaseDetailsTemp DeleteInvoiceFromTemp(int purch)
        {
            var db = new AccountingContainer();
            PurchaseDetailsTemp vm = db.PurchaseDetailsTemps.Find(purch);
            db.PurchaseDetailsTemps.Remove(vm);
            db.SaveChanges();
            db.Dispose();
            return vm;
        }
        public static String MakeDeadLineDate(int pay, String Startdate)
        {
            var time = Convert.ToDateTime(Startdate);
            if (time == null)
            {
                return null;
            }
            else
            {
                String dateLine = time.AddMonths(pay).ToString("dd-MM-yyyy");
                return dateLine;
            }



        }
        /// <summary>
        /// TO get file from entity of files 
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public static PurchaseFileDetail getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.PurchaseFileDetails.Find(fileId);
            return file;
        }
        public static string deleteFile(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.PurchaseFileDetails.Find(fileId);
            db.PurchaseFileDetails.Remove(file);
            db.SaveChanges();
            return "تم حذف الملف ";
        }
        /// <summary>
        /// To get list of files by invoice id
        /// </summary>
        /// <param name="invoiceMasterId"></param>
        /// <returns></returns>
        public static List<PurchaseFileDetail> GetAllFiles(int invoiceMasterId)
        {
            var db = new AccountingContainer();
            var files = db.PurchaseFileDetails.Where(emp => emp.PurInvoiceMasterMaster_Id == invoiceMasterId).ToList();
            return files;
        }

        /// <summary>
        /// To get list of details of product when edit invoice
        /// </summary>
        /// <param name="MasterVMID"></param>
        /// <returns></returns>
        public static PurchaseDetailsTempVM GetOneListofDetailsFirst(int MasterVMID)
        {
            var db = new AccountingContainer();
            PurchaseDetailsTempVM vm = new PurchaseDetailsTempVM();
            
            var thiadetailtemp = db.PurchaseDetailsTemps.ToList();

            if (thiadetailtemp.Count > 0)
            {
                db.PurchaseDetailsTemps.RemoveRange(thiadetailtemp);
                db.SaveChanges();
            }
            var purchase = db.PurInvoiceMasters.Find(MasterVMID);
            var thisAcutauldetail = purchase.InvoiceDetails.ToList();
            if (purchase.FixedDiscount == null)
            {
                purchase.FixedDiscount = 0;
            }
            vm.AllowDiscount = purchase.FixedDiscount.Value;
            vm.invoice_Id = MasterVMID;
            var PurchList = new List<PurchaseDetailsTemp> { };

            if (thisAcutauldetail.Count > 0)
            {
                foreach (var detail in thisAcutauldetail)
                {
                    var purchasedetiltemp = new PurchaseDetailsTemp();
                    purchasedetiltemp.Product_Quantity = detail.Product_Quantity;
                    purchasedetiltemp.Product_Price = detail.Product_Price;
                    purchasedetiltemp.Total_Amount = detail.Total_Amount;
                    if (detail.Product_Tax == null)
                    {

                    }
                    else
                    {
                        purchasedetiltemp.Product_Tax = (int)detail.Product_Tax;
                    }
                    purchasedetiltemp.Detail_Total_Tax = detail.Detail_Total_Tax;
                    purchasedetiltemp.Detail_Total_Cost = detail.Detail_Total_Cost;
                    purchasedetiltemp.Product_ID = detail.Product_ID;
                    purchasedetiltemp.PurInvoiceMasterTempMaster_Id = detail.InvoiceMaster_Id;
                    if (detail.Asset_Id > 0)
                    {
                        var thisasst = db.FixedAssetInformations.Find(detail.Asset_Id);
                        purchasedetiltemp.ItemDesc = detail.ItemDesc;
                        purchasedetiltemp.Product_Code = detail.Item_Code;
                        purchasedetiltemp.Asset_Id = detail.Asset_Id;
                    }
                    else
                    {
                        var thisporduct = db.Products.Find(detail.Product_ID);
                        purchasedetiltemp.ItemDesc = detail.ItemDesc;
                        purchasedetiltemp.Product_Code = thisporduct.Prod_Code;
                    }
                    
                    PurchList.Add(purchasedetiltemp);

                }

            }
            else
            {
            }
            db.PurchaseDetailsTemps.AddRange(PurchList);
            db.SaveChanges();
           vm.invoiceTemp = db.PurInvoiceMasterTemps.Find(MasterVMID).PurchaseDetailsTemps.ToList();

            return vm;

        }

        public static List<PurchaseDetailsTemp> GetOneListofDetailssecond(int mastervmID)
        {
            var db = new AccountingContainer();

            var finshedtemp = db.PurchaseDetailsTemps.Where(e => e.PurInvoiceMasterTempMaster_Id == mastervmID).ToList();
            return finshedtemp;
        }
        public static string DeleteFromORderTemp(int DetailID, int masterID)
        {
            var db = new AccountingContainer();
            var allDetails = db.PurchaseDetailsTemps.Where(e => e.PurInvoiceMasterTempMaster_Id == masterID).ToList();
            if (allDetails.Count > 1)
            {
                var finshedtemp = db.PurchaseDetailsTemps.Find(DetailID);
                db.PurchaseDetailsTemps.Remove(finshedtemp);
                db.SaveChanges();
                db.Dispose();
                return "تم حذف المنتج";
            }
            else
            {
                return "يجب وجود منتج واحد علي الاقل في امر الشراء";
            }

        }

        /// <summary>
        /// To change discound in view when change from user 
        /// </summary>
        /// <param name="discount"></param>
        /// <param name="Invoice_Id"></param>
        /// <returns></returns>
        public static string changeDetailOfInvoice(decimal discount, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.PurInvoiceMasterTemps.Find(Invoice_Id);
            var detail = db.PurchaseDetailsTemps.Where(a => a.PurInvoiceMasterTempMaster_Id == Invoice_Id).ToList();
            decimal total = 0;
            foreach (var item in detail)
            {
                item.TotalDiscount = (item.Product_Quantity * item.Product_Price) * (discount / 100);
                item.Total_Amount = (item.Product_Quantity*item.Product_Price) -(decimal) item.TotalDiscount;
                var beforeTax = (item.Product_Quantity * item.Product_Price) - item.TotalDiscount;
                if (item.Product_Tax == 0 || item.Product_Tax==null)
                {

                }
                else
                {
                    var tax = db.Taxes.Find(item.Product_Tax).Tax_Percentage;
                    item.Detail_Total_Tax = beforeTax.Value * (tax / 100);
                }     
                item.Detail_Total_Cost = ((item.Product_Quantity * item.Product_Price) - item.TotalDiscount.Value) + item.Detail_Total_Tax;
                db.Entry(item).State = EntityState.Modified;
                total += item.Detail_Total_Cost;
            }
            invoiceTemp.Total_Cost = total;
            db.Entry(invoiceTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "تم تعديل الخصم بنجاح";

        }
        /// <summary>
        /// To change allow discound automatic in view when change from user 
        /// </summary>
        /// <param name="AllowDiscount"></param>
        /// <param name="Invoice_Id"></param>
        /// <returns></returns>
        public static string changeDetailOfInvoiceForAllDiscount(decimal AllowDiscount, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.PurInvoiceMasterTemps.Find(Invoice_Id);
            invoiceTemp.AllowDiscount = AllowDiscount;
            db.Entry(invoiceTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "تم عمل الخصم المسموح به";

        }
        public static bool itCanVoid(int PuchaseId)
        {
            var db = new AccountingContainer();
            var pur = db.PurInvoiceMasters.Find(PuchaseId);
            foreach (var asset in db.FixedAssets.Where(a=>a.PurchaseId== PuchaseId).ToList() )
            {
                if (asset.FixedAssetDetails.Count() > 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}