using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accountingsystem.ViewModel;
using System.Data.Entity;
using System.Web.Mvc;
using System.Data.Entity.Validation;

namespace Accountingsystem.Models
{
    public partial class OfferPriceMaster
    {
        /// <summary>
        /// create unique code for each OfferPrice
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns></returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.OfferPriceMasters.ToList();
            foreach (var item in allinData)
            {
                if (item.Serial_Number == Serial || !Serial.StartsWith("OFR-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }
        public static string changeSpecificValueinPOs(int detail_Id, decimal quantity, decimal discount)
        {
            var db = new AccountingContainer();
            if (quantity < 0)
            {
                quantity = quantity * -1;
            }
            if (quantity == 0)
            {
                quantity = 1;
            }
            var detail = db.InvoiceDetailsTemps.Find(detail_Id);
            var master = detail.InvoiceMasterTemp;
            var batch = db.Products.Where(pro => pro.Prod_ID == detail.Product_ID).FirstOrDefault();
            
            detail.Product_Quantity = quantity;         

            decimal totalprice = detail.Product_Price * detail.Product_Quantity;
            decimal discound = totalprice * (discount / 100);
            decimal afterdec = totalprice - discound;
            detail.TotalBeforeDisc = totalprice;
            detail.TotalDiscount = discound;
            if (detail.Product_Tax == 0)
            {
                detail.Detail_Total_Tax = 0;
            }
            else
            {
                var prodtax = db.Taxes.Find(detail.Product_Tax);
                detail.Detail_Total_Tax = afterdec * (prodtax.Tax_Percentage / 100);
            }
            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;
            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return "";

        }
        public static string changeSpecificPrice(int detail_Id, decimal Price, decimal discount)
        {
            var db = new AccountingContainer();
            if (Price < 0)
            {
                Price = Price * -1;
            }
          
            var detail = db.InvoiceDetailsTemps.Find(detail_Id);
            var master = detail.InvoiceMasterTemp;

            detail.Product_Price = Price;

            decimal totalprice = Price * detail.Product_Quantity;
            decimal discound = totalprice * (discount / 100);
            decimal afterdec = totalprice - discound;
            detail.TotalBeforeDisc = totalprice;
            detail.TotalDiscount = discound;
            if (detail.Product_Tax == 0)
            {
                detail.Detail_Total_Tax = 0;
            }
            else
            {
                var prodtax = db.Taxes.Find(detail.Product_Tax);
                detail.Detail_Total_Tax = afterdec * (prodtax.Tax_Percentage / 100);
            }
            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;
            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return "";

        }
        public static bool AddmuchTax(List<string> taxList, int master_Id)
        {
            var db = new AccountingContainer();
            var Master = db.InvoiceMasterTemps.Find(master_Id);
            Master.TotalNewTaxs = 0;
            if (taxList != null)
            {
                foreach (var tax in taxList)
                {
                    var totalAmount = Master.InvoiceDetailsTemps.Select(a => a.Total_Amount).Sum();
                    var percntage = (db.Taxes.Find(Convert.ToInt32(tax)).Tax_Percentage / 100);

                    Master.TotalNewTaxs += totalAmount * percntage;
                }

            }
            else
            {
                Master.TotalNewTaxs = 0;
            }
            db.Entry(Master).State = EntityState.Modified;
            db.SaveChanges();
            return true;

        }
        public static string changeSpecificTax(int detail_Id, int product_Tax, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.InvoiceDetailsTemps.Find(detail_Id);
            var master = detail.InvoiceMasterTemp;




            decimal totalprice = detail.Product_Price * detail.Product_Quantity;
            decimal discound = totalprice * (discount / 100);
            decimal afterdec = totalprice - discound;
            detail.TotalBeforeDisc = totalprice;
            detail.TotalDiscount = discound;
            if (product_Tax == 0)
            {
                detail.Detail_Total_Tax = 0;
                detail.Product_Tax = 0;
            }
            else
            {
                var prodtax = db.Taxes.Find(product_Tax);
                detail.Product_Tax = product_Tax;
                detail.Detail_Total_Tax = afterdec * (prodtax.Tax_Percentage / 100);

            }

            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;
            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return "";

        }

        //get all offer price in data base
        public static List<OfferPriceMaster> getALLOfferMasr(ApplicationUser user,int ?all)
        {
            var db = new AccountingContainer();
            var oferMast = db.OfferPriceMasters.Where(e => e.Offer_State == false).ToList();
            var Emp = db.Employees.Where(a => a.Reg_ID == user.Id).FirstOrDefault();

            if (user.Roles.FirstOrDefault().RoleId == "11")
            {
                try
                {
                    oferMast = oferMast.Where(a => a.Emp_Id == Emp.Emp_Id).ToList();
                }
                catch
                {
                    oferMast = new List<OfferPriceMaster>();
                }
            }
            if (all == null)
            {
                oferMast.Reverse();
                oferMast = oferMast.Take(100).ToList();
            }


            return oferMast;
        }
        /// <summary>
        /// get All Offer price that statuse is true
        /// </summary>
        /// <returns></returns>
        public static List<OfferPriceMaster> getALLOfferMasrTrueState(ApplicationUser user,int ?all)
        {
            var db = new AccountingContainer();
            var oferMast = db.OfferPriceMasters.Where(e=>e.Offer_State==true).ToList();
            var Emp = db.Employees.Where(a => a.Reg_ID == user.Id).FirstOrDefault();
            if (user.Roles.FirstOrDefault().RoleId == "11" && Emp != null)
            {
                oferMast = oferMast.Where(a => a.Emp_Id == Emp.Emp_Id).ToList();
            }
            if (all == null)
            {
                oferMast.Reverse();
                oferMast = oferMast.Take(100).ToList();
            }

            return oferMast;
           
        }
        //get offer detail by id that user enter that
        public static OfferPriceMaster getDetailById(int id)
        {
            var db = new AccountingContainer();
            var ofermast = db.OfferPriceMasters.Find(id);
            if (ofermast.FixedDiscount == null)
            {
                ofermast.FixedDiscount = 0;
            }
            return ofermast;
        }
        //get view to user to fill it
        public static OfferPriceMasterVM AddNewForCreation(string userId)
        {
            var db = new AccountingContainer();
            String StartCodeOfrevenue = "04";
            var OffMast = new OfferPriceMasterVM();
            var emp = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
            if (emp != null)
            {
                OffMast.Emp_Id = emp.Emp_Id;
            }
          
            var lastSerial = db.OfferPriceMasters.OrderByDescending(c => c.OfferMas_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                OffMast.Serial_Number = "OFR-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                OffMast.Serial_Number = "OFR-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
            }


            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false).ToList();
            // Get SelectLists used to populate drop-down lists
            OffMast.CompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            OffMast.CurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            var newList = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                           select new
                           {
                               Id = cust.Customer_Id,
                               Name = cust.Customer_Name + "        " + cust.Customer_Phone

                           }).ToList();
            OffMast.CustomerList = new SelectList(newList, "Id", "Name");
           
            OffMast.StoreList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            OffMast.AccountList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfrevenue)), " Account_Id", "Account_Name");
            OffMast.RevenueAccount_Id = db.ChartOfAccountings.Where(c => c.Account_Code == "040101001" && c.isActive != false).FirstOrDefault().Account_Id;
            OffMast.Payment_Policy = 0;
            OffMast.Offer_Date = DateTime.Now.ToString();
            OffMast.OfferDeadLine = DateTime.Now.ToString();
            OffMast.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            OffMast.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false &&a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            OffMast.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            OffMast.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            OffMast.EmpList = new SelectList(db.Employees.Where(e => e.Reg_ID != null), "Emp_Id", "Emp_Name");
            // Initialize the Invoice state to be Pending
            var thistaxinchart = db.ChartOfAccountings.Find(48);

            var anewList = (from Tax in db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false)
                            select new
                            {

                                Id = Tax.Tax_ID,
                                Name = Tax.Tax_Name + "   " + Tax.Tax_Percentage + "%"

                            }).ToList();


            OffMast.TaxList = new SelectList(anewList, "Id", "Name");
            var invoiceMasterTemp = new InvoiceMasterTemp();
            var id = (from s in db.InvoiceMasterTemps
                      select s).ToList().LastOrDefault();
            if (id == null)
            { invoiceMasterTemp.Master_Id = 1; }
            else
            {
                invoiceMasterTemp.Master_Id = id.Master_Id + 1;
            }
            invoiceMasterTemp.AllowDiscount = 0;
            if (db.Stores.Count() >= 1)
            {
                OffMast.SelectedStore_ID = db.Stores.FirstOrDefault().Store_ID;
            }
            db.InvoiceMasterTemps.Add(invoiceMasterTemp);
            var sub = db.SubLedgers.FirstOrDefault();
            if (sub.OfferDesc != null || sub.OfferNotes != null)
            {
                OffMast.Description = sub.OfferDesc;
                OffMast.Notes = sub.OfferNotes;
            }
            db.SaveChanges();

            OffMast.OfferMas_Id = invoiceMasterTemp.Master_Id;
            return OffMast;
        }
        /// <summary>
        /// add to list in invoice detail temp
        /// </summary>
        /// <param name="invoiceMasterDetail"></param>
        /// <returns></returns>
        public static string AddInvoiceDetailsTemp(InvoiceMasterDetailsVM invoiceMasterDetail)
        {
            var db = new AccountingContainer();
            if (invoiceMasterDetail.productNotBatched_ID > 0)
            {              
               
                
                invoiceMasterDetail.Product_Quantity = 1;
                var invoiceMasterTemp = db.InvoiceMasterTemps.Find(invoiceMasterDetail.InvoiceMaster_Id);

                var invoiceMasterDetailsTemp = new InvoiceDetailsTemp();
                var productsss = db.Products.Find(invoiceMasterDetail.productNotBatched_ID);
                invoiceMasterDetailsTemp.Product_Price =InvoiceMaster.GetPriceSale(invoiceMasterDetail.store, invoiceMasterDetail.productNotBatched_ID, invoiceMasterDetail.CustomerID);

                invoiceMasterDetailsTemp.Batch_ID = 0;
                invoiceMasterDetailsTemp.Product_ID = productsss.Prod_ID;

                var dtails = invoiceMasterTemp.InvoiceDetailsTemps;
                var dublicateinTemp = dtails.Where(z => z.InvoiceMaster_Id == invoiceMasterTemp.Master_Id && z.Product_ID == invoiceMasterDetailsTemp.Product_ID).ToList();
                if (dublicateinTemp.FirstOrDefault() != null)
                {
                    return Resources.OfferPriceMasterCreateOffer.ProductExisitInListModelMessage;
                }
                else
                {
                   
                    invoiceMasterDetailsTemp.Product_Quantity = invoiceMasterDetail.Product_Quantity;
                    var thisProduct = db.Products.Find(invoiceMasterDetailsTemp.Product_ID);
                    if (thisProduct != null)
                    {
                        invoiceMasterDetailsTemp.Product_Name = thisProduct.Prod_Name;
                        invoiceMasterDetailsTemp.Product_Code = thisProduct.Prod_Code;
                    }
                    
                       
                    
                    decimal TotalBeforeDiscount = invoiceMasterDetailsTemp.Product_Price * invoiceMasterDetail.Product_Quantity;
                    decimal discount = TotalBeforeDiscount * (invoiceMasterDetail.Master_Discount / 100);
                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                    invoiceMasterDetailsTemp.TotalBeforeDisc = TotalBeforeDiscount;
                    invoiceMasterDetailsTemp.TotalDiscount = discount;
                    if (productsss.Tax_Id==0|| productsss.Tax_Id == null)
                    {
                        invoiceMasterDetailsTemp.Detail_Total_Tax = 0;
                        invoiceMasterDetailsTemp.Product_Tax = 0;
                    }
                    else
                    {
                        var a = db.Taxes.Find(productsss.Tax_Id);
                        invoiceMasterDetailsTemp.Product_Tax = productsss.Tax_Id.Value;
                        invoiceMasterDetailsTemp.Detail_Total_Tax = TotalAfterDiscount * (a.Tax_Percentage / 100);
                    }
                   
                    
                    
                    invoiceMasterDetailsTemp.Detail_Total_Cost = (decimal)productsss.Prod_Cost_Price * invoiceMasterDetail.Product_Quantity;
                    invoiceMasterDetailsTemp.Detail_Discount = invoiceMasterDetail.Master_Discount;
                    invoiceMasterDetailsTemp.Detail_Discount_Amount = discount;

                    invoiceMasterDetailsTemp.Total_Amount = (TotalBeforeDiscount + invoiceMasterDetailsTemp.Detail_Total_Tax) - discount;
                    invoiceMasterTemp.InvoiceDetailsTemps.Add(invoiceMasterDetailsTemp);

                    db.Entry(invoiceMasterTemp).State = EntityState.Modified;
                    db.SaveChanges();

                    return Resources.OfferPriceMasterCreateOffer.ProductIsAddedToListModelMessage;

                }
            }
            else
            {
                return Resources.OfferPriceMasterCreateOffer.ProductIsNotFoundModelMessage;
            }


        }
        /// <summary>
        /// Prepare detail for create and fillter the product to choosing store
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="masterVmId"></param>
        /// <returns></returns>
        public static InvoiceMasterDetailsVM PrepareInvoiceMasterDetail(int storeId, int masterVmId)
        {
            var db = new AccountingContainer();
            var invoiceMasterDetailsVM = new InvoiceMasterDetailsVM();
           // var batchs = db.ProductBatches.Where(batch => batch.Store_ID == storeId || batch.Store_ID == null).ToList();
           // batchs = batchs.Where(a => Math.Round(a.Product_Quantity,2,MidpointRounding.AwayFromZero) > 0).ToList();
           // var services = db.ProductBatches.Where(a => a.Store_ID == null || a.Store_ID == 0).ToList();
           //batchs.AddRange(services);
            //List<ProductBatchSelection> _ProductBatchSelection = new List<ProductBatchSelection>();
            //foreach (var item in batchs)
            //{
            //    var productBatchSelection = new ProductBatchSelection();

            //    productBatchSelection.Batch_Id = item.Batch_ID;
            //    productBatchSelection.ProductCost = item.Product_Cost;
            //    productBatchSelection.ProductName = item.Product.Prod_Name;
            //    productBatchSelection.Product_Id = item.Product_ID;
                
            //    _ProductBatchSelection.Add(productBatchSelection);

            //}

            //invoiceMasterDetailsVM.MyBatchsList = new SelectList(_ProductBatchSelection, "Batch_Id", "ProductName");
            List<object> newList = new List<object>();
            var thistaxinchart = db.ChartOfAccountings.Find(48);
            var members = db.Taxes.Where(e=>e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false).ToList();
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.Tax_ID,
                    Name = member.Tax_Name + "   " + member.Tax_Percentage + "%"
                });
            invoiceMasterDetailsVM.ProducttaxList = new SelectList(newList, "Id", "Name");
            //// this  code  it  to make  list  of  Products  not  in Batch
            //List<object> productList = new List<object>();
            //var products = db.Products.ToList();
            //foreach (var pro in products)
            //{
            //    var thisbatch = batchs.Where(e => e.Product_ID == pro.Prod_ID).ToList();
            //    if (thisbatch.Count > 0)
            //    {

            //    }
            //    else
            //    {
            //        productList.Add(new
            //        {
            //            Id = pro.Prod_ID,
            //            Name = pro.Prod_Name
            //        });

            //    }
            //}
            //invoiceMasterDetailsVM.ProductNotBatchList = new SelectList(productList, "Id", "Name");
          
            var newListpro = (from prod in db.Products.Where(a=>a.IsActive!=false&&a.Prod_Type!=Prod_Type.اولي)
                              select new
                              {
                                  Id = prod.Prod_ID,
                                  Name = prod.Prod_Name + "   " + prod.Bar_Code

                              }).ToList();
            

            invoiceMasterDetailsVM.ProductNotBatchList = new SelectList(newListpro, "Id", "Name");
            
            return invoiceMasterDetailsVM;

        }
        /// <summary>
        /// get all invoice detail temp by its master id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns></returns>
        public static InvoiceDetailsTempVM GetAllTempInvoiceDetails(int masterVmId)
        {
            var db = new AccountingContainer();
            var invoicedetalTemp = new InvoiceDetailsTempVM();
            invoicedetalTemp.invoiceTemp = db.InvoiceDetailsTemps.Where(invoice => invoice.InvoiceMaster_Id == masterVmId).ToList();
            invoicedetalTemp.invoice_Id = masterVmId;
         
            try
            {
                var x = db.InvoiceMasterTemps.Find(masterVmId);
                invoicedetalTemp.AllowDiscount = db.InvoiceMasterTemps.Find(masterVmId).AllowDiscount.Value;


            }
            catch { invoicedetalTemp.AllowDiscount = 0; }
            try
            {
                invoicedetalTemp.TotalNewTaxs = db.InvoiceMasterTemps.Find(masterVmId).TotalNewTaxs.Value;


            }
            catch { invoicedetalTemp.TotalNewTaxs = 0; }
            return invoicedetalTemp;
        }
        //take the view that has been filled and save it in database
        public static int AddNewOffer(OfferPriceMasterVM offerMasVM , String UserId)
        {
            var db = new AccountingContainer();
            var OfferMaster = new OfferPriceMaster();
            var invoiceMasterTemp = db.InvoiceMasterTemps.Find(offerMasVM.OfferMas_Id);
            if (offerMasVM != null)
            {

                var lastSerial = db.OfferPriceMasters.OrderByDescending(c => c.OfferMas_Id).FirstOrDefault();
                if (lastSerial == null)
                {
                    OfferMaster.Serial_Number = "OFR-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    OfferMaster.Serial_Number = "OFR-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
                }

                OfferMaster.Description = offerMasVM.Description;
                OfferMaster.Payment_Policy = offerMasVM.Payment_Policy;
                var comp = db.Companies.Find(offerMasVM.SelectedCompany_Id);
                if (comp != null)
                {
                    var date = Convert.ToDateTime(offerMasVM.Offer_Date);
                    if (date < comp.StartFinancialDate)
                    {
                        OfferMaster.Offer_Date = DateTime.Now;
                    }
                    else
                    {
                        OfferMaster.Offer_Date = Convert.ToDateTime(offerMasVM.Offer_Date);
                    }
                }
                else
                {
                    OfferMaster.Offer_Date = DateTime.Now;
                }
              //OfferMaster.OfferDeadLine = Convert.ToDateTime(offerMasVM.OfferDeadLine);
                OfferMaster.OfferDeadLine = OfferMaster.Offer_Date.AddDays(offerMasVM.Payment_Policy.Value);
                OfferMaster.Reference = offerMasVM.Reference;
                OfferMaster.RevenueAccount_Id = offerMasVM.RevenueAccount_Id;
                OfferMaster.Customer_Name = db.Customers.Find(offerMasVM.SelectedCustomer_Id).Customer_Name;
                OfferMaster.Emp_Id = offerMasVM.Emp_Id;
                OfferMaster.Branch_Id = offerMasVM.Branch_Id;
                OfferMaster.Notes = offerMasVM.Notes;

                var resultCurrency = db.Currencies.Find(offerMasVM.SelectedCurrency_Id);
                if (resultCurrency != null)
                {
                    OfferMaster.Currency_Id = resultCurrency.Currency_Id;
                }
                OfferMaster.Customer_Id = offerMasVM.SelectedCustomer_Id;
                var resultComp = db.Companies.Find(offerMasVM.SelectedCompany_Id);
                if (resultComp != null)
                {
                    OfferMaster.Company_Id = resultComp.Comp_Id;
                }
                var resultCus = db.Customers.Find(offerMasVM.SelectedCustomer_Id);
                if (resultCus != null)
                {
                    OfferMaster.Customer_Id = resultCus.Customer_Id;
                }
                if (offerMasVM.SelectedStore_ID == 0 || offerMasVM.SelectedStore_ID == null)
                {
                    offerMasVM.SelectedStore_ID = db.Stores.FirstOrDefault().Store_ID;
                }
               
                    OfferMaster.Store_ID= offerMasVM.SelectedStore_ID;
               
                OfferMaster.Master_Discount = offerMasVM.Master_Discount;
                if (offerMasVM.Subledger_Id != null && offerMasVM.Subledger_Id != 0)
                {

                    OfferMaster.Branch_Id = offerMasVM.Subledger_Id;

                }
                else if (offerMasVM.BussinesSubledger_Id != null && offerMasVM.BussinesSubledger_Id != 0)
                {

                    OfferMaster.Branch_Id = offerMasVM.BussinesSubledger_Id;

                }
                else if (offerMasVM.DepartSubledger_Id != null && offerMasVM.DepartSubledger_Id != 0)
                {

                    OfferMaster.Branch_Id = offerMasVM.DepartSubledger_Id;

                }
                else if (offerMasVM.BranchSubledger_Id != null && offerMasVM.BranchSubledger_Id != 0)
                {

                    OfferMaster.Branch_Id = offerMasVM.BranchSubledger_Id;

                }
                else { OfferMaster.Branch_Id = 1; }
                // OfferMaster.Total_Tax = offerMasVM.Total_Tax;
                // loop over details in the VM and add them to the master
                //foreach (var item in invoiceMasterTemp.InvoiceDetailsTemps.ToList())
                //{
                //    var details = new OfferPriceDetail();
                //    var productthis = db.Products.Find(item.Product_ID);
                //    details.Product_Name = productthis.Prod_Name;
                //    details.Batch_ID = item.Batch_ID;
                //    details.Product_Quantity = item.Product_Quantity;
                //    details.Product_Price = item.Product_Price;                    
                //    details.Product_ID = item.Product_ID;
                //    details.Product_Tax = item.Product_Tax;
                //    var tax = Convert.ToDecimal(db.Taxes.Find(item.Product_Tax).Tax_Percentage);
                //    OfferMaster.Tax_ID =Convert.ToInt32( item.Product_Tax);
                //    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                //    decimal discount = TotalBeforeDiscount * (OfferMaster.Master_Discount / 100);
                //    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;

                //    details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                //    var cost = item.Product_Price * item.Product_Quantity;

                //    details.Total_Amount = TotalAfterDiscount + details.Detail_Total_Tax;
                //    OfferMaster.Total_Amount += details.Total_Amount;


                //    OfferMaster.Total_Tax += details.Detail_Total_Tax;
                //    OfferMaster.OfferPriceDetails.Add(details);
                //    db.InvoiceDetailsTemps.Remove(item);

                //}
                foreach (var item in invoiceMasterTemp.InvoiceDetailsTemps.ToList())
                {

                    // Create new detail and map the values from detailVM to it
                    // TODO: Use AutoMapper
                    var details = new OfferPriceDetail();
                    details.OfferDet_Id = item.Details_Id;
                    details.Product_Quantity = item.Product_Quantity;
                    details.Product_Price = item.Product_Price;
                    details.Detail_Discount = item.Detail_Discount;
                    details.Detail_Discount_Amount = item.Detail_Discount_Amount;
                    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    decimal discount = TotalBeforeDiscount * (details.Detail_Discount.Value / 100);
                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;

                    // details.Total_Amount = item.Total_Amount; //- (item.Total_Amount * (masterVM.Master_Discount / 100));
                    details.Product_Tax = Convert.ToInt32(item.Product_Tax);
                    // details.InvoiceMaster_Id = masterVM.Master_Id;       

                    details.Batch_ID = item.Batch_ID;
                    OfferMaster.Tax_ID = (int)item.Product_Tax;
                    if (details.Product_Tax == 0)
                    {
                        details.Detail_Total_Tax =0;
                    }
                    else
                    {
                        var tax = Convert.ToDecimal(db.Taxes.Find(details.Product_Tax).Tax_Percentage);
                        details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                    }
                    

                    // TEMP: Fix relations
                    if (item.Batch_ID > 0)
                    {
                        ProductBatches detailBatch = db.ProductBatches.Find(details.Batch_ID);

                        details.Product_ID = detailBatch.Product_ID;
                        var battchh = db.ProductBatches.Find(item.Batch_ID);
                        // Checks for the products quantity if available or not
                        //if (item.Product_Quantity > detailBatch.Product_Quantity&&battchh.Store_ID!=null)
                        //    return "Not enough quantity of " + detailBatch.Product.Prod_Name;
                        // Calculate the Total_Cost
                        
                        //details.Detail_Total_Cost = item.Detail_Total_Cost;
                        details.Batch_ID = item.Batch_ID;
                        var price = item.Product_Quantity * item.Product_Price;
                        details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                        OfferMaster.Total_Amount += details.Total_Amount;
                        //OfferMaster.Total_Cost += details.Detail_Total_Cost;
                      
                        OfferMaster.OfferPriceDetails.Add(details);
                        db.InvoiceDetailsTemps.Remove(item);
                    }
                    else
                    {
                        Product productss = db.Products.Find(item.Product_ID);

                        details.Product_ID = productss.Prod_ID;
                        
                        //details.Detail_Total_Cost = item.Detail_Total_Cost;
                        details.Batch_ID = item.Batch_ID;
                        var price = item.Product_Quantity * item.Product_Price;
                        details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                        OfferMaster.Total_Amount += details.Total_Amount;
                        //OfferMaster.Total_Cost += details.Detail_Total_Cost;
                        //master.Total_Amount += details.Total_Amount;
                        OfferMaster.Total_Tax += details.Detail_Total_Tax;

                        OfferMaster.OfferPriceDetails.Add(details);
                        db.InvoiceDetailsTemps.Remove(item);
                    }


                }

                OfferMaster.TotalNewTaxs = 0;
                if (offerMasVM.Taxes != null)
                {
                    foreach (var tax in offerMasVM.Taxes)
                    {
                        var taxo = new OfferTax();
                        taxo.Tax_Id = tax;

                        taxo.Amount = (db.Taxes.Find(Convert.ToInt32(tax)).Tax_Percentage / 100) * OfferMaster.OfferPriceDetails.Select(a => a.Total_Amount).Sum();
                        OfferMaster.TotalNewTaxs += taxo.Amount;

                        OfferMaster.OfferTaxes.Add(taxo);


                    }
                }
                OfferMaster.FixedDiscount = offerMasVM.Fixed_Discount;
                // The total amount after matching is initialized to the total amount
              
                OfferMaster.TotaL_After_Fixed = (OfferMaster.Total_Amount + OfferMaster.TotalNewTaxs) - OfferMaster.FixedDiscount;
                OfferMaster.BranchSubledger_Id = offerMasVM.BranchSubledger_Id;
                OfferMaster.DepartSubledger_Id = offerMasVM.DepartSubledger_Id;
                OfferMaster.BussinesSubledger_Id = offerMasVM.BussinesSubledger_Id;
                db.OfferPriceMasters.Add(OfferMaster);
                db.InvoiceMasterTemps.Remove(invoiceMasterTemp);


                var traBatch = new TransactionBatch();

                traBatch.OperationName = "عرض سعر";
                traBatch.Description = offerMasVM.Description;
                traBatch.Date = Convert.ToDateTime(offerMasVM.Offer_Date);
                traBatch.UserId = UserId;
                traBatch.Operation_Code = offerMasVM.Serial_Number;

                traBatch.OperationId = OfferMaster.OfferMas_Id;
                db.SaveChanges();
                db.Dispose();



                return OfferMaster.OfferMas_Id;
            }
            return 0;
        }
        //delete the offer from data basehas no invoices
        //where that offer 


        public static int deleteOffer(int id)
        {
            var db = new AccountingContainer();
            var offermas = db.OfferPriceMasters.Find(id);


            var offerdetail = offermas.OfferPriceDetails;
            if (offermas.OfferTaxes.Any())
            {
                db.OfferTaxes.RemoveRange(offermas.OfferTaxes);
            }
            var thisinvoice = db.InvoiceMasters.Where(e => e.Offer_Price_Code == offermas.Serial_Number).ToList();
            List<InvoiceMaster> thislistofinvoice = new List<InvoiceMaster>();
            if (thisinvoice.Count > 0)
            {
                foreach (var item in thisinvoice)
                {
                    if (item.Invoice_State == TransactionState.Post || item.Invoice_State == TransactionState.Pending)
                    {
                        thislistofinvoice.Add(item);
                    }

                }
                if (thislistofinvoice.Count > 0)
                {
                    return 0;
                }
                else
                {
                    db.OfferPriceDetails.RemoveRange(offerdetail);

                    db.OfferPriceMasters.Remove(offermas);
                    db.SaveChanges();
                    db.Dispose();
                    return 1;
                }
            }
            else
            {
                db.OfferPriceDetails.RemoveRange(offerdetail);



                db.OfferPriceMasters.Remove(offermas);
                db.SaveChanges();
                db.Dispose();
                return 1;
            }


        }
        /// <summary>
        ///  get all stor product drobdown List
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>

        public static SelectList getStoreProducts(int storeId)
        {
            var db = new AccountingContainer();
            var store = db.Stores.Find(storeId);

            List<Product> MyProduct = new List<Product>();

        

            SelectList Products = new SelectList(MyProduct, "Prod_ID", "Prod_Name");

            return Products;
        }

        /// <summary>
        /// make invoice master depend on Offer Price id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MasterVM addinvoiceDependOnInvoice(int id)
        {
            var db = new AccountingContainer();
            var offr = db.OfferPriceMasters.Find(id);
            var msvm = new MasterVM();
            msvm.Description = offr.Description;
            msvm.Invoice_Currency = db.Currencies.Find(offr.Currency_Id).Currency_ISO;
            msvm.Customer_Id = db.Customers.Find(offr.Customer_Id).Customer_Id;        
            msvm.Company_Id = offr.Company_Id;
            msvm.Master_Discount = offr.Master_Discount;
            msvm.Tax_ID = offr.Tax_ID;
            foreach (var item in offr.OfferPriceDetails)
            {
                var invoice = new DetailsVM();
                invoice.Product_Quantity = item.Product_Quantity;
                invoice.Product_Price = item.Product_Price;
                invoice.Product_ID = item.Product_ID;
                invoice.Batch_ID = (int)item.Batch_ID;
                msvm.myDetailsVM.Add(invoice);
            }
            return msvm;
        }


        //////////this  folowing models  are for  editting///////
        public static OfferPriceDetailVM PreparOfferPriceDetailVMEdit(int serial)
        {

            var db = new AccountingContainer();
            var OfferpricedetailVM = new OfferPriceDetailVM();

            var thisdetails = GetAllofferDetails(serial).ToList().FirstOrDefault();

            OfferpricedetailVM.Product_Quantity = thisdetails.Product_Quantity;
            OfferpricedetailVM.Product_Price = thisdetails.Product_Price;
            OfferpricedetailVM.Total_Amount = thisdetails.Total_Amount;
            OfferpricedetailVM.Detail_Total_Tax = thisdetails.Detail_Total_Tax;
            //tax  of  product is  not  in the  entitys
            //total cost too
            OfferpricedetailVM.Product_ID = thisdetails.Product_ID;
            OfferpricedetailVM.Batch_ID = (int)thisdetails.Batch_ID;
            OfferpricedetailVM.product_tax = (int)thisdetails.Product_Tax;
            List<object> newListpro = new List<object>();
            var membersPro = db.Products.ToList();
            foreach (var member in membersPro)
                newListpro.Add(new
                {
                    Id = member.Prod_ID,
                    Name = member.Prod_Name + "   " + member.Bar_Code
                });

            OfferpricedetailVM.ProductList = new SelectList(newListpro, "Id", "Name");
           
            List<object> newList = new List<object>();
            var thistaxinchart = db.ChartOfAccountings.Find(48);
            var members = db.Taxes.Where(e=>e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false).ToList();
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.Tax_ID,
                    Name = member.Tax_Name + "   " + member.Tax_Percentage + "%"
                });
            OfferpricedetailVM.taxList = new SelectList(newList, "Id", "Name");

            return OfferpricedetailVM;

        }
        /// <summary>
        /// get all detail of ofer Price  by using offer master id
        /// </summary>
        /// <param name="offermasterID"></param>
        /// <returns>List<OfferPriceDetail></returns>
        public static List<OfferPriceDetail> GetAllofferDetails(int offermasterID)
        {
            var db = new AccountingContainer();
            var Details = db.OfferPriceDetails.Where(invoice => invoice.OfferDet_Id == offermasterID).ToList();
            return Details;
        }
        /// <summary>
        /// get all detail of ofer Price  by using offer master id
        /// </summary>
        /// <param name="offermasterID"></param>
        /// <returns>List<OfferPriceDetail></returns>
        public static List<OfferPriceDetail> GetAllofferDetails2(int offermasterID)
        {
            var db = new AccountingContainer();
            var Details = db.OfferPriceDetails.Where(invoice => invoice.OfferPriceMasterMaster_Id == offermasterID).ToList();
            return Details;
        }
        /// <summary>
        /// prepare Offer Price detail fpr Editing by put all detail of offer price in invoice detail temp
        /// </summary>
        /// <param name="offermasterID"></param>
        /// <returns>List<InvoiceDetailsTemp></returns>
        public static InvoiceDetailsTempVM GetAllTempDetailsresult2(int offermasterID)
        {
            var db = new AccountingContainer();
            var invoicedetalTemp = new InvoiceDetailsTempVM();
            
            var detailTemp = db.InvoiceMasterTemps.Find(offermasterID);
            db.InvoiceDetailsTemps.RemoveRange(detailTemp.InvoiceDetailsTemps);
            db.SaveChanges();
           
            var OfferPrice = db.OfferPriceMasters.Find(offermasterID);
            if (OfferPrice.FixedDiscount != null)
            {
                invoicedetalTemp.AllowDiscount = OfferPrice.FixedDiscount.Value;
            }
            else
            {
                invoicedetalTemp.AllowDiscount = 0;
            }
            if (OfferPrice.TotalNewTaxs != 0 && OfferPrice.TotalNewTaxs != null)
            {
                invoicedetalTemp.TotalNewTaxs = OfferPrice.OfferTaxes.Select(a=>a.Amount).Sum();
            }
            List<InvoiceDetailsTemp> Lista = new List<InvoiceDetailsTemp> { };
            foreach (var item in OfferPrice.OfferPriceDetails)
            {
                var OfferDetailTemp = new InvoiceDetailsTemp();
                var pro = db.Products.Find(item.Product_ID);
                OfferDetailTemp.Batch_ID = item.Batch_ID.Value;
                OfferDetailTemp.Detail_Total_Tax = item.Detail_Total_Tax;
                OfferDetailTemp.Product_Code = pro.Prod_Code;
                OfferDetailTemp.Product_ID = item.Product_ID;
                OfferDetailTemp.Product_Name = pro.Prod_Name;
                OfferDetailTemp.Product_Price = item.Product_Price;
                OfferDetailTemp.Product_Quantity = item.Product_Quantity;
                if (item.Detail_Discount != null)
                {
                    OfferDetailTemp.Detail_Discount = item.Detail_Discount;
                    OfferDetailTemp.Detail_Discount_Amount = item.Detail_Discount_Amount;
                }
                else
                {
                    OfferDetailTemp.Detail_Discount = item.OfferPriceMaster.Master_Discount;
                    OfferDetailTemp.Detail_Discount_Amount = (item.Product_Quantity*item.Product_Price) * (item.OfferPriceMaster.Master_Discount/100);

                }
                OfferDetailTemp.Product_Tax = item.Product_Tax;
                OfferDetailTemp.Total_Amount = item.Total_Amount;
                OfferDetailTemp.InvoiceMaster_Id = offermasterID;
                Lista.Add(OfferDetailTemp);

            }
            invoicedetalTemp.invoiceTemp = Lista;
            invoicedetalTemp.invoice_Id = offermasterID;
            db.InvoiceDetailsTemps.AddRange(Lista);

            db.SaveChanges();
           
            return invoicedetalTemp;
        }
        /// <summary>
        /// add to List to invoice detail temp and check to validation of input
        /// </summary>
        /// <param name="Detail"></param>
        /// <returns>string</returns>
        public static string AddofferDetailsTemp(OfferPriceDetailVM Detail)
        {
            var db = new AccountingContainer();
            var offerMasterTemp = db.InvoiceMasterTemps.Find(Detail.OfferPriceMasterMaster_Id);
            var offerDetailsTemp = new InvoiceDetailsTemp();
            var batch = db.ProductBatches.Find(Detail.Product_ID);
            offerDetailsTemp.Total_Amount = Detail.Product_Quantity * Detail.Product_Price;
           
             var a = db.Taxes.Find(Convert.ToInt32(Detail.product_tax));
            decimal TotalBeforeDiscount = Detail.Product_Price * Detail.Product_Quantity;
            decimal discount = TotalBeforeDiscount * (Detail.Discount / 100);
            decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
            if (a != null)
            {
              
                offerDetailsTemp.Detail_Total_Tax = TotalAfterDiscount * (a.Tax_Percentage / 100);
            }
           else { offerDetailsTemp.Detail_Total_Tax = 0; }
            offerDetailsTemp.Product_ID = Detail.Product_ID;

            offerDetailsTemp.Product_Quantity = (short)Detail.Product_Quantity;
            offerDetailsTemp.Product_Price = Detail.Product_Price;
            offerDetailsTemp.Product_Tax = Detail.product_tax;
            var dtails = offerMasterTemp.InvoiceDetailsTemps;
            var dublicateinTemp = dtails.Where(z => z.InvoiceMaster_Id == offerDetailsTemp.InvoiceMaster_Id && z.Product_ID == offerDetailsTemp.Product_ID).ToList();
            if (dublicateinTemp.FirstOrDefault() != null)
            {
                return "هذا المنتج موجود بالفعل فى القائمة لا يمكن اضافته مرة اخرى";
            }
            //it  must  have  product  tax  here


            offerDetailsTemp.Total_Amount = (TotalBeforeDiscount + offerDetailsTemp.Detail_Total_Tax) - discount;


            offerDetailsTemp.InvoiceMaster_Id = Detail.OfferPriceMasterMaster_Id;
            offerMasterTemp.InvoiceDetailsTemps.Add(offerDetailsTemp);
            db.Entry(offerMasterTemp).State = EntityState.Modified;
          

            db.SaveChanges();
            return "تمت الاضافة بنجاح";
        }
        /// <summary>
        /// fill for Editing for OfferPrice by using its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>OfferPriceMasterVM</returns>
        public static OfferPriceMasterVM FillTheOfferViewModel(int id , int isCopy)
        {
            var db = new AccountingContainer();
            var editModel = getDetailById(id) ;
           OfferPriceMasterVM PVM = new OfferPriceMasterVM();
            if (editModel.FixedDiscount != null)
            {
                PVM.Fixed_Discount = editModel.FixedDiscount.Value;
            }
            else
            {
                PVM.Fixed_Discount = 0;
            }
            

            PVM.OfferMas_Id = editModel.OfferMas_Id;
            PVM.isCopy = isCopy;
            PVM.Serial_Number = editModel.Serial_Number;
            PVM.Notes = editModel.Notes;
            //var cust = db.Customers.Where(s => s.Customer_Id == editModel.Customer_Id);
            //if (cust.Any())
            //{
            //    PVM.SelectedCustomer_Id = cust.FirstOrDefault().Customer_Id;
            //}
           
            var newList = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                           select new
                           {
                               Id = cust.Customer_Id,
                               Name = cust.Customer_Name + "        " + cust.Customer_Phone

                           }).ToList();
            PVM.CustomerList = new SelectList(newList, "Id", "Name");
            if (isCopy == 1)
            {
                PVM.Offer_Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
            }
            else
            {
                PVM.Offer_Date = editModel.Offer_Date.ToString("dd-MM-yyyy   HH:mm:ss");
            }
            var thistaxinchart = db.ChartOfAccountings.Find(48);
            //List<object> anewList = new List<object>();
            var anewList = (from Tax in db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false)
                            select new
                            {

                                Id = Tax.Tax_ID,
                                Name = Tax.Tax_Name + "   " + Tax.Tax_Percentage + "%"

                            }).ToList();
            PVM.TaxList = new SelectList(anewList, "Id", "Name");
            PVM.Taxes = editModel.OfferTaxes.Select(a => a.Tax_Id).ToList();

            PVM.OfferDeadLine = editModel.OfferDeadLine.ToString("dd-MM-yyyy   HH:mm:ss");
            PVM.Reference = editModel.Reference;
            PVM.RevenueAccount_Id = editModel.RevenueAccount_Id;
            PVM.Description = editModel.Description;
            PVM.Payment_Policy = editModel.Payment_Policy;
            PVM.Emp_Id = editModel.Emp_Id.Value;
            PVM.SelectedCustomer_Id = editModel.Customer_Id;
            
          
            PVM.SelectedCurrency_Id = editModel.Currency_Id;
            PVM.SelectedCompany_Id = editModel.Company_Id;
           
    
            var store = db.Stores.Where(c => c.Store_ID == editModel.Store_ID);
            if (store.Any())
            {
                PVM.SelectedStore_ID = store.FirstOrDefault().Store_ID;
            }
            PVM.StoreList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            if (editModel.Tax_ID == null)
            {
                PVM.Exchange_Rate = 0;
            }
            else
            {
                PVM.Exchange_Rate = (decimal)editModel.Tax_ID;
            }
           
            PVM.Master_Discount = editModel.Master_Discount;
            String StartCodeOfrevenue = "04";
            var acc = db.ChartOfAccountings.Where(c => c.Account_Code == editModel.RevenueAccount_Id.ToString());
            if (acc.Any())
            {
                PVM.RevenueAccount_Id =Convert.ToInt32(acc.FirstOrDefault().Account_Code);
            }


            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false).ToList();
            PVM.AccountList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfrevenue)), " Account_Id", "Account_Name");
            var emp = db.Employees.Where(c => c.Emp_Id == editModel.Emp_Id);
            if (emp.Any())
            {
                PVM.Emp_Id = emp.FirstOrDefault().Emp_Id;
            }
            PVM.EmpList = new SelectList(db.Employees.Where(e => e.Reg_ID != null), "Emp_Id", "Emp_Name");
           
            
            PVM.BranchSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false&&(a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            if (editModel.BranchSubledger_Id > 0)
            {
                PVM.DepartSubList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == editModel.BranchSubledger_Id && p.isActive != false && p.Subled_Level == 2), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                PVM.DepartSubList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
                PVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            }
            if (editModel.DepartSubledger_Id > 0)
            {
                PVM.BussinessList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == editModel.DepartSubledger_Id&& p.isActive != false ), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                PVM.BussinessList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");

            }
            PVM.SubledgerList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى"&&a.isActive != false ), "SubLed_Id", "SubLed_Name");
            PVM.BranchSubledger_Id = editModel.BranchSubledger_Id;
            PVM.DepartSubledger_Id= editModel.DepartSubledger_Id;
            PVM.BussinesSubledger_Id = editModel.BussinesSubledger_Id;
            // PVM.InvoiceDetails= editModel.InvoiceDetails.ToList();
            var offerMasterTemp = new InvoiceMasterTemp();
         
            offerMasterTemp.Company_Id = PVM.SelectedCompany_Id;
            offerMasterTemp.Description = editModel.Description;
            offerMasterTemp.Due_Date =Convert.ToDateTime(editModel.Offer_Date);
            offerMasterTemp.Exchange_Rate = editModel.Tax_ID;
            offerMasterTemp.GL_Date = Convert.ToDateTime(editModel.OfferDeadLine);
            offerMasterTemp.Master_Discount = editModel.Master_Discount;
            offerMasterTemp.Serial_Number = editModel.Serial_Number;
            offerMasterTemp.Store_ID = PVM.SelectedStore_ID;
            offerMasterTemp.Total_Amount = editModel.Total_Amount;
            if (editModel.FixedDiscount != null)
            {
                offerMasterTemp.AllowDiscount = editModel.FixedDiscount;
            }
            else
            {
                offerMasterTemp.AllowDiscount = 0;
            }
            offerMasterTemp.Total_Tax = editModel.Total_Tax;
            var ThereORN = db.InvoiceMasterTemps.Find(editModel.OfferMas_Id);
            if (ThereORN != null)
            {
                ThereORN.AllowDiscount = offerMasterTemp.AllowDiscount;
                db.Entry(ThereORN).State = EntityState.Modified;
            }
            else
            {
                offerMasterTemp.Master_Id = editModel.OfferMas_Id;
                

                db.InvoiceMasterTemps.Add(offerMasterTemp);
            }

            db.SaveChanges();

            PVM.RevenueAccount_Id = editModel.RevenueAccount_Id;
            return PVM;
        }
        /// <summary>
        /// saving Edite in data base og offer Price
        /// </summary>
        /// <param name="mastertemps"></param>
        /// <returns>string</returns>
        public static int editOffer(OfferPriceMasterVM mastertemps)
        {
            try
            {
                var db = new AccountingContainer();
                var offerMaster = db.OfferPriceMasters.Find(mastertemps.OfferMas_Id);
                var mastertempe = db.InvoiceMasterTemps.Find(mastertemps.OfferMas_Id);
                offerMaster.Company_Id = Convert.ToInt32(mastertemps.SelectedCompany_Id);
                if (mastertemps.SelectedCustomer_Id == 0 || mastertemps.SelectedCustomer_Id == null)
                {

                }
                else
                {
                    offerMaster.Customer_Id = Convert.ToInt32(mastertemps.SelectedCustomer_Id);
                }
                offerMaster.Serial_Number = mastertemps.Serial_Number;
                offerMaster.OfferMas_Id = mastertemps.OfferMas_Id;
                offerMaster.Description = mastertemps.Description;
                offerMaster.Notes = mastertemps.Notes;
                var comp = db.Companies.Find(mastertemps.SelectedCompany_Id);
                if (comp != null)
                {
                    var date = Convert.ToDateTime(mastertemps.Offer_Date);
                    if (date < comp.StartFinancialDate)
                    {
                        offerMaster.Offer_Date = DateTime.Now;
                    }
                    else
                    {
                        offerMaster.Offer_Date = Convert.ToDateTime(mastertemps.Offer_Date);
                    }
                }
                else
                {
                    offerMaster.Offer_Date = DateTime.Now;
                }
                offerMaster.OfferDeadLine = offerMaster.Offer_Date.AddDays(offerMaster.Payment_Policy.Value);
                offerMaster.Serial_Number = mastertemps.Serial_Number;
                
                offerMaster.Customer_Name = db.Customers.Find(offerMaster.Customer_Id).Customer_Name;
                offerMaster.Master_Discount = (decimal)mastertemps.Master_Discount;
                if (mastertemps.Subledger_Id != null && mastertemps.Subledger_Id != 0)
                {

                    offerMaster.Branch_Id = mastertemps.Subledger_Id;

                }
                else if (mastertemps.BussinesSubledger_Id != null && mastertemps.BussinesSubledger_Id != 0)
                {

                    offerMaster.Branch_Id = mastertemps.BussinesSubledger_Id;

                }
                else if (mastertemps.DepartSubledger_Id != null && mastertemps.DepartSubledger_Id != 0)
                {

                    offerMaster.Branch_Id = mastertemps.DepartSubledger_Id;

                }
                else if (mastertemps.BranchSubledger_Id != null && mastertemps.BranchSubledger_Id != 0)
                {

                    offerMaster.Branch_Id = mastertemps.BranchSubledger_Id;

                }
                else { offerMaster.Branch_Id = 1; }
                var cur = db.Currencies.Where(c => c.Currency_Id == mastertemps.SelectedCurrency_Id);
                if (cur.Any())
                {
                    offerMaster.Currency_Id = cur.FirstOrDefault().Currency_Id;
                }
               
              if(mastertemps.SelectedStore_ID==0|| mastertemps.SelectedStore_ID == null)
                {
                    mastertemps.SelectedStore_ID = db.Stores.FirstOrDefault().Store_ID;
                }
                offerMaster.Store_ID = mastertemps.SelectedStore_ID;
              
                offerMaster.RevenueAccount_Id = mastertemps.RevenueAccount_Id; 
                offerMaster.Reference = mastertemps.Reference;
                offerMaster.Payment_Policy = mastertemps.Payment_Policy;
                offerMaster.Emp_Id = mastertemps.Emp_Id;
                if (mastertemps.Branch_Id == null || mastertemps.Branch_Id == 0)
                {
                    mastertemps.Branch_Id = 0;
                }
                else
                {
                    offerMaster.Branch_Id = mastertemps.Branch_Id;
                }
                
                decimal Sumtax = 0;
                decimal SumtAmount = 0;
                var detailListTemp = db.InvoiceDetailsTemps.Where(e => e.InvoiceMaster_Id == mastertempe.Master_Id).ToList();
                var oldDetail = db.OfferPriceDetails.Where(a => a.OfferPriceMasterMaster_Id == offerMaster.OfferMas_Id).ToList();
                var oldTaxes = db.OfferTaxes.Where(a => a.OfferPriceMasterOfferMas_Id == offerMaster.OfferMas_Id).ToList();
                List<InvoiceDetailsTemp> ListDetail = new List<InvoiceDetailsTemp>();
                offerMaster.Total_Amount = 0;
                offerMaster.Total_Tax = 0;
                offerMaster.TotalNewTaxs = 0;
                offerMaster.OfferTaxes = new List<OfferTax>();
                foreach (var item in detailListTemp)
                {

                    // Create new detail and map the values from detailVM to it
                    // TODO: Use AutoMapper
                    var details = new OfferPriceDetail();
                    details.OfferDet_Id = item.Details_Id;
                    details.Product_Quantity = item.Product_Quantity;
                    details.Product_Price = item.Product_Price;
                    details.Detail_Discount = item.Detail_Discount;
                    details.Detail_Discount_Amount = item.Detail_Discount_Amount;
                    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    decimal discount = TotalBeforeDiscount * (item.Detail_Discount.Value / 100);
                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;

                    // details.Total_Amount = item.Total_Amount; //- (item.Total_Amount * (masterVM.Master_Discount / 100));
                    details.Product_Tax = Convert.ToInt32(item.Product_Tax);
                    // details.InvoiceMaster_Id = masterVM.Master_Id;       

                    details.Batch_ID = item.Batch_ID;
                    offerMaster.Tax_ID = (int)item.Product_Tax;
                    if (item.Product_Tax == 0)
                    {
                        details.Detail_Total_Tax = 0;
                    }
                    else
                    {
                        var tax = Convert.ToDecimal(db.Taxes.Find(details.Product_Tax).Tax_Percentage);
                        details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                    }
                   
                    // TEMP: Fix relations
                    if (item.Batch_ID > 0)
                    {
                        ProductBatches detailBatch = db.ProductBatches.Find(details.Batch_ID);

                        details.Product_ID = detailBatch.Product_ID;
                        var battchh = db.ProductBatches.Find(item.Batch_ID);
                  
                        
                        //details.Detail_Total_Cost = item.Detail_Total_Cost;
                        details.Batch_ID = item.Batch_ID;
                        var price = item.Product_Quantity * item.Product_Price;
                        details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                        offerMaster.Total_Amount += details.Total_Amount;
                        //OfferMaster.Total_Cost += details.Detail_Total_Cost;
                        offerMaster.Total_Tax += details.Detail_Total_Tax;
                        offerMaster.OfferPriceDetails.Add(details);
                        db.InvoiceDetailsTemps.Remove(item);
                    }
                    else
                    {
                        Product productss = db.Products.Find(item.Product_ID);

                        details.Product_ID = productss.Prod_ID;
                        
                        //details.Detail_Total_Cost = item.Detail_Total_Cost;
                        details.Batch_ID = item.Batch_ID;
                        var price = item.Product_Quantity * item.Product_Price;
                        details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                        offerMaster.Total_Amount += details.Total_Amount;
                       
                        offerMaster.Total_Tax += details.Detail_Total_Tax;

                        offerMaster.OfferPriceDetails.Add(details);
                        db.InvoiceDetailsTemps.Remove(item);
                    }


                }
                if (mastertemps.Taxes != null)
                {
                    foreach (var tax in mastertemps.Taxes)
                    {
                        var taxo = new OfferTax();
                        taxo.Tax_Id = tax;
                        var b = offerMaster.OfferPriceDetails.Select(z => z.Total_Amount).Sum();
                        taxo.Amount = (db.Taxes.Find(Convert.ToInt32(tax)).Tax_Percentage / 100) * detailListTemp.Select(a => a.Total_Amount).Sum();
                        offerMaster.TotalNewTaxs += taxo.Amount;

                        offerMaster.OfferTaxes.Add(taxo);


                    }
                }
                offerMaster.FixedDiscount = mastertemps.Fixed_Discount;
                offerMaster.TotaL_After_Fixed = (offerMaster.Total_Amount + offerMaster.TotalNewTaxs) - offerMaster.FixedDiscount;
                offerMaster.BranchSubledger_Id = mastertemps.BranchSubledger_Id;
                offerMaster.DepartSubledger_Id = mastertemps.DepartSubledger_Id;
                offerMaster.BussinesSubledger_Id = mastertemps.BussinesSubledger_Id;
                db.InvoiceDetailsTemps.RemoveRange(ListDetail);
                db.Entry(offerMaster).State = EntityState.Modified;
                db.InvoiceMasterTemps.Remove(mastertempe);
                db.OfferPriceDetails.RemoveRange(oldDetail);
                db.OfferTaxes.RemoveRange(oldTaxes);
                try
                { db.SaveChanges();
                    db.Dispose();
                }
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

                return offerMaster.OfferMas_Id;
            }
            catch
            {
                var db = new AccountingContainer();
                
                var mastertempe = db.InvoiceMasterTemps.Where(e => e.Master_Id == mastertemps.OfferMas_Id).ToList().LastOrDefault();
                var mastertempeD = db.InvoiceDetailsTemps.Where(e => e.InvoiceMaster_Id == mastertemps.OfferMas_Id).ToList();
                db.InvoiceMasterTemps.Remove(mastertempe);
                foreach (var item in mastertempeD)
                {
                    db.InvoiceDetailsTemps.Remove(item);
                    db.SaveChanges();
                }
                db.Dispose();
                return 0;
            }
           
        }

        /// <summary>
        /// delete offer price detail temp and remove it from List
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static decimal deleteItemFomDetails(int id)
        {
            var db = new AccountingContainer();
            var delete = db.InvoiceDetailsTemps.Find(id);

            db.InvoiceDetailsTemps.Remove(delete);
            db.SaveChanges();
            decimal discount = 0;
            var salestemp = db.InvoiceMasterTemps.Find(delete.InvoiceMaster_Id);
            try
            {
                discount = salestemp.InvoiceDetailsTemps.Select(a => a.Detail_Discount_Amount.Value).Sum() / salestemp.InvoiceDetailsTemps.Select(a => a.Product_Quantity * a.Product_Price).Sum();
            }
            catch (Exception ex)
            {
                return 0;
            }
            return discount * 100;
        }
        /// <summary>
        /// get allcustomer for dropdownList that Active
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetCustomerList()
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> customers = new SelectList(db.Customers, "Customer_Id", "Customer_Name");           
            return customers;
        }
        public static IEnumerable<SelectListItem> GetProductList()
        {
            var db = new AccountingContainer();
            var prod = from prodc in db.Products.Where(z=>z.IsActive!=false)
                       select new SelectListItem
                       {
                           Text = prodc.Prod_Name + " " + prodc.Bar_Code,
                           Value = prodc.Prod_ID.ToString()
                       };
            IEnumerable<SelectListItem> products = prod;
            var a = prod.ToList();
            return products;
        }

        public static OfferPriceMasterVM GetCustomerListbYid(int id)
        {
            var db = new AccountingContainer();

            OfferPriceMasterVM vm = new OfferPriceMasterVM();
            vm.SelectedCustomer_Id = db.OfferPriceMasters.Find(id).Customer_Id;

            vm.CustomerList = new SelectList(db.Customers.Where(e => e.Customer_State == Customer_State.نشط).ToList(), "Customer_Id", "Customer_Name");

            return vm;
        }
        /// <summary>
        /// calculate the dead Line of OfferPrice
        /// </summary>
        /// <param name="pay"></param>
        /// <param name="Startdate"></param>
        /// <returns></returns>
        public static String MakeDeadLineDate(int pay,String Startdate)
        { 
                var time = Convert.ToDateTime(Startdate);                   
                if (time == null)
                {
                return null;
                }
                else
                {
                    String dateLine = time.AddDays(pay).ToString("dd-MM-yyyy");
                    return dateLine;
                }         
           
             

        }
        /// <summary>
        /// change the detail of invoice depend on change in discount 
        /// </summary>
        /// <param name="discount"></param>
        /// <param name="Invoice_Id"></param>
        /// <returns></returns>
        public static string changeDetailOfInvoice(decimal discount, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.InvoiceMasterTemps.Find(Invoice_Id);
            var detail = db.InvoiceDetailsTemps.Where(a => a.InvoiceMaster_Id == Invoice_Id).ToList();
            decimal total = 0;
            foreach (var item in detail)
            {
                item.TotalBeforeDisc = item.Product_Quantity * item.Product_Price;
                item.TotalDiscount = item.TotalBeforeDisc * (discount / 100);
                item.Detail_Discount = discount;
                item.Detail_Discount_Amount = item.TotalDiscount;
                var beforeTax = item.TotalBeforeDisc - item.TotalDiscount;
                if (item.Product_Tax == 0)
                {
                    item.Detail_Total_Tax = 0;
                }
                else
                {
                    var tax = db.Taxes.Find(item.Product_Tax).Tax_Percentage;

                    item.Detail_Total_Tax = beforeTax.Value * (tax / 100);
                }

                item.Total_Amount = (item.TotalBeforeDisc.Value - item.TotalDiscount.Value) + item.Detail_Total_Tax;
                db.Entry(item).State = EntityState.Modified;
                total += item.Total_Amount;
            }
            invoiceTemp.Total_Amount = total;
            db.Entry(invoiceTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "تم تعديل الخصم بنجاح";

        }
    }
}