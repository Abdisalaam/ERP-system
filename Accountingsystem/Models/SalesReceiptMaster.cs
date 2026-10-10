using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Accountingsystem.ViewModel;
using Accountingsystem.Utilites;
using Microsoft.Office.Interop.Excel;

namespace Accountingsystem.Models
{
    public partial class SalesReceiptMaster
    {
        /// <summary>
        /// check is that code is unique or not
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns>bool</returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.SalesReceiptMasters.ToList();
            foreach (var item in allinData)
            {
                if (item.Serial_Number == Serial || !Serial.StartsWith("SALRC-"))
                {
                    return true;
                }
                else
                {

                }

            }
            return false;
        }

        public static decimal changeSpecificValueinPOs(int detail_Id, decimal quantity, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.SalesReceiptDetailsTemps.Find(detail_Id);
            var master = detail.SalesReceiptMasterTemp;
            var batch = db.Products.Where(pro => pro.Prod_ID == detail.Product_Id).FirstOrDefault();

            //if (batch.Product_Quantity < quantity && batch.Product.Prod_Type == Prod_Type.منتج)
            //{
            //    return "الكمية اكبر من الموجود في المخزن";

            //}
            detail.Product_Quantity = quantity;
            //detail.Detail_Total_Cost = quantity * batch.Product_Cost;

            decimal totalprice = detail.Product_Price * detail.Product_Quantity;
            decimal discound = totalprice * (detail.Detail_Discount.Value / 100);
            decimal afterdec = totalprice - discound;
            detail.TotalBeforeDisc = totalprice;
            detail.TotalDiscount = discound;
            detail.Detail_Discount_Amount = detail.TotalDiscount;

         
            if (detail.Product_Tax == 0)
            {
                detail.Detail_Total_Tax = 0;
            }
            else
            {
                var prodtax = db.Taxes.Find(detail.Product_Tax);
                detail.Detail_Total_Tax = afterdec * (prodtax.Tax_Percentage / 100);
            }
            detail.Detail_Total_Cost= batch.Prod_Cost_Price.Value * quantity;
            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;
            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.SaveChanges();
            var ee = master.SalesReceiptDetailsTemps.Select(a => a.Product_Quantity*a.Product_Price*(a.Detail_Discount.Value/100)).Sum();
            var bb = master.SalesReceiptDetailsTemps.Select(a => a.Product_Quantity * a.Product_Price).Sum();
            decimal masdiscount =( ee / bb )  * 100;
            master.Master_Discount = masdiscount;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return masdiscount;

        }
        public static decimal changeSpecificPrice(int detail_Id, decimal Price, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.SalesReceiptDetailsTemps.Find(detail_Id);
            var master = detail.SalesReceiptMasterTemp;

            detail.Product_Price = Price;

            decimal totalprice = Price * detail.Product_Quantity;
            decimal discound = totalprice * (discount / 100);
            decimal afterdec = totalprice - discound;
            detail.TotalBeforeDisc = totalprice;
            detail.TotalDiscount = discound;
            detail.Detail_Discount_Amount = detail.TotalDiscount;
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
            var ee = master.SalesReceiptDetailsTemps.Select(a => a.Product_Price * a.Product_Quantity * (a.Detail_Discount.Value / 100)).Sum();
            var bb = master.SalesReceiptDetailsTemps.Select(a => a.Product_Price * a.Product_Quantity).Sum();
            decimal masdiscount = (ee / bb) * 100;
            master.Master_Discount = masdiscount;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return masdiscount;

        }
        public static decimal changeSpecificDiscount(int detail_Id, decimal discountdetail)
        {
            var db = new AccountingContainer();
            var detail = db.SalesReceiptDetailsTemps.Find(detail_Id);
            var master = detail.SalesReceiptMasterTemp;

            detail.Detail_Discount = discountdetail;

            decimal totalprice = detail.Product_Price * detail.Product_Quantity;
            decimal discound = totalprice * (discountdetail / 100);
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
            detail.Detail_Discount = discountdetail;
            detail.Detail_Discount_Amount = detail.TotalDiscount;
            detail.Detail_Discount_Amount = (detail.Product_Price * detail.Product_Quantity) * (detail.Detail_Discount / 100);
            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;
           
            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            var ee = master.SalesReceiptDetailsTemps.Select(a => a.Product_Price*a.Product_Quantity * (a.Detail_Discount.Value / 100)).Sum();
            var bb = master.SalesReceiptDetailsTemps.Select(a => a.Product_Price * a.Product_Quantity).Sum();
            decimal masdiscount = (ee / bb) * 100;
            return masdiscount;

        }
        public static string changeSpecificTax(int detail_Id, int product_Tax, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.SalesReceiptDetailsTemps.Find(detail_Id);
            var master = detail.SalesReceiptMasterTemp;




            decimal totalprice = detail.Product_Price * detail.Product_Quantity;
            decimal discound = 0;
            if (detail.Detail_Discount != null)
            {
                 discound = totalprice * (detail.Detail_Discount.Value / 100);
            }
            else
            {
                 discound = totalprice * (discount / 100);
            }
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

        public static decimal changeSpecificDiscountAmount(int detail_Id, decimal discountdetailAmount)
        {
            var db = new AccountingContainer();
            var detail = db.SalesReceiptDetailsTemps.Find(detail_Id);
            var master = detail.SalesReceiptMasterTemp;

            detail.Detail_Discount_Amount = discountdetailAmount;

            decimal totalprice = detail.Product_Price * detail.Product_Quantity;
            detail.Detail_Discount = (discountdetailAmount / totalprice) * 100;
            decimal discound = totalprice * (detail.Detail_Discount.Value / 100);
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

            detail.Detail_Discount_Amount = detail.TotalDiscount;
            detail.Detail_Discount_Amount = (detail.Product_Price * detail.Product_Quantity) * (detail.Detail_Discount / 100);
            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;

            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            var ee = master.SalesReceiptDetailsTemps.Select(a => a.Product_Price * a.Product_Quantity * (a.Detail_Discount.Value / 100)).Sum();
            var bb = master.SalesReceiptDetailsTemps.Select(a => a.Product_Price * a.Product_Quantity).Sum();
            decimal masdiscount = (ee / bb) * 100;
            return masdiscount;

        }
        public static decimal changeSpecificTotalAmount(int detail_Id, decimal totalAmount)
        {

            var db = new AccountingContainer();
            var detail = db.SalesReceiptDetailsTemps.Find(detail_Id);
            var master = detail.SalesReceiptMasterTemp;
            decimal tax = 0;
            if (detail.Product_Tax > 0)
            {
                tax = db.Taxes.Find(detail.Product_Tax).Tax_Percentage / 100;
            }

            var price = (totalAmount) / ((1 + tax) * (1 - (detail.Detail_Discount.Value / 100)) * detail.Product_Quantity);





            decimal totalprice = price * detail.Product_Quantity;
            decimal discound = totalprice * (detail.Detail_Discount.Value / 100);
            decimal afterdec = totalprice - discound;
            detail.Product_Price = price;
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

            detail.Detail_Discount_Amount = detail.TotalDiscount;
            detail.Detail_Discount_Amount = (detail.Product_Price * detail.Product_Quantity) * (detail.Detail_Discount / 100);
            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;

            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            var ee = master.SalesReceiptDetailsTemps.Select(a => a.Product_Price * a.Product_Quantity * (a.Detail_Discount.Value / 100)).Sum();
            var bb = master.SalesReceiptDetailsTemps.Select(a => a.Product_Price * a.Product_Quantity).Sum();
            decimal masdiscount = (ee / bb) * 100;
            return price;

        }
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
        /// get All SalesRecipt 
        /// </summary>
        /// <returns>List<SalesReceiptMaster></returns>

        public static List<SalesReceiptMaster> GetAllSalesReceipt(int? all)
        {

            var db = new AccountingContainer();
            var orders = db.SalesReceiptMasters.ToList();
            if (all == null)
            {
                orders.Reverse();
                orders = orders.Take(100).ToList();
            }
            return orders;
        }
        public static List<SalesReceiptMaster> GetAllInvoiceForThatUser(string UserId,int ?all)
        {

            var db = new AccountingContainer();
            var orders = db.SalesReceiptMasters.ToList();
            var emp = db.Employees.Where(a => a.Reg_ID == UserId).FirstOrDefault();
            if (emp != null)
            {
                orders = orders.Where(a => a.Emp_Id == emp.Emp_Id).ToList();
            }
            if (all == null)
            {
                orders.Reverse();
                orders = orders.Take(100).ToList();
            }
            return orders;
        }
        /// <summary>
        /// get Available Quantity of that batch by its id
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>

        public static int GetAvailbleProductQty(int batchId)
        {
            var db = new AccountingContainer();
            var batch = db.ProductBatches.Find(batchId);
            return Convert.ToInt32(batch.Product_Quantity);

        }
        /// <summary>
        /// get salesRecipt by its id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns>SalesReceiptMaster</returns>
        public static SalesReceiptMaster GetSalesReceiptMasterById(int invoiceId)
        {
            var db = new AccountingContainer();
            var detail = db.SalesReceiptMasters.Find(invoiceId);
            return detail;

        }
        /// <summary>
        /// prepare adding to list by store id
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public static SalesReceiptDetailsVM PrepareInvoiceMasterDetail(int storeId)
        {
            var db = new AccountingContainer();
            var SalesReciptVM = new SalesReceiptDetailsVM();
            //var batchs = db.ProductBatches.Where(batch => batch.Store_ID == storeId).ToList();
            //batchs = batchs.Where(a => Math.Round(a.Product_Quantity, 2, MidpointRounding.AwayFromZero) > 0).ToList();
            //var services = db.ProductBatches.Where(a => a.Store_ID == null || a.Store_ID == 0).ToList();
            //batchs.AddRange(services);
            //List<ProductBatchSelection> _ProductBatchSelection = new List<ProductBatchSelection>();
            //foreach (var item in batchs)
            //{
            //    var productBatchSelection = new ProductBatchSelection();

            //    productBatchSelection.Batch_Id = item.Batch_ID;
            //    productBatchSelection.ProductCost = item.Product_Cost;
            //    productBatchSelection.ProductName = item.Product.Prod_Name;
            //    productBatchSelection.Product_Id = item.Product_ID;
            //    productBatchSelection.Quantity = item.Product_Quantity;
            //    _ProductBatchSelection.Add(productBatchSelection);


            //}

            //List<object> newList2 = new List<object>();
            //var members2 = _ProductBatchSelection.Where(a => a.Quantity != 0 || a.storeID == 0).ToList();
            //foreach (var member in members2)
            //    newList2.Add(new
            //    {
            //        Id = member.Batch_Id,
            //        Name = member.ProductName
            //    });
            //SalesReciptVM.BatchList = new SelectList(newList2, "Id", "Name");
            List<object> newList = new List<object>();
            var thistaxinchart = db.ChartOfAccountings.Find(48);
            var members = db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false).ToList();
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.Tax_ID,
                    Name = member.Tax_Name + " % " + member.Tax_Percentage
                });
            SalesReciptVM.ProducttaxList = new SelectList(newList, "Id", "Name");

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
            //SalesReciptVM.ProductNotBatchList = new SelectList(productList, "Id", "Name");
          
            var newListpro = from prod in  db.Products.Where(a=> a.IsActive != false && a.Prod_Type!=Prod_Type.اولي) select new {
                Id = prod.Prod_ID,
                Name = prod.Prod_Name + "   " + prod.Bar_Code


            };
           

            SalesReciptVM.ProductNotBatchList = new SelectList(newListpro, "Id", "Name");
            //SalesReciptVM.ProductNotBatchList = new SelectList(db.Products.ToList(), "Prod_ID", "Prod_Name");
            SalesReciptVM.Product_Quantity = 1;
            
            return SalesReciptVM;
        }
        public static SalesReceiptDetailsTempVM PrepareListdetailOfInvoiceOffer(int masterid, int Offerid)
        {
            var db = new AccountingContainer();
            var offer = db.OfferPriceMasters.Find(Offerid);
            var invoicedetalTemp = new SalesReceiptDetailsTempVM();
            
            var Mastertemp = db.SalesReceiptMasterTemps.Find(masterid);
            var DetailList = new List<SalesReceiptDetailsTemp>();
            foreach (var item in offer.OfferPriceDetails.ToList())
            {
                var Detail = new SalesReceiptDetailsTemp();
                Detail.Batch_ID = item.Batch_ID.Value;

                Detail.Detail_Total_Cost = 0;
                Detail.Detail_Total_Tax = item.Detail_Total_Tax;
                Detail.Product_Id = item.Product_ID;
               
                Detail.Product_Price = item.Product_Price;
                Detail.Product_Quantity = item.Product_Quantity;
                Detail.Product_Tax =(int) item.Product_Tax;
                if (item.Detail_Discount != null)
                {
                    Detail.Detail_Discount = item.Detail_Discount;
                    Detail.Detail_Discount_Amount = item.Detail_Discount_Amount;
                }
                else
                {
                    Detail.Detail_Discount = offer.Master_Discount;
                    Detail.Detail_Discount_Amount = (item.Product_Price * item.Product_Quantity) * (offer.Master_Discount / 100);
                }
                Detail.Total_Amount = item.Total_Amount;
                Detail.Detail_Total_Cost = db.Products.Find(item.Product_ID).Prod_Cost_Price.Value * item.Product_Quantity;
                Detail.SalesReceiptMasterTempMaster_Id = Mastertemp.Master_Id;
                DetailList.Add(Detail);

            }
            if (Mastertemp.SalesReceiptDetailsTemps.Count != 0)
            {
                db.SalesReceiptDetailsTemps.RemoveRange(Mastertemp.SalesReceiptDetailsTemps);
                db.SaveChanges();
            }
            db.SalesReceiptDetailsTemps.AddRange(DetailList);
            db.SaveChanges();
            invoicedetalTemp.invoiceTemp= DetailList;
            if (offer.FixedDiscount != null)
            {
                invoicedetalTemp.AllowDiscount = offer.FixedDiscount.Value;
            }
            else
            {
                invoicedetalTemp.AllowDiscount = 0;
            }
            invoicedetalTemp.invoice_Id = masterid;
            return invoicedetalTemp;
        }
        public static bool AddmuchTax(List<string> taxList, int master_Id)
        {
            var db = new AccountingContainer();
            var Master = db.SalesReceiptMasterTemps.Find(master_Id);
            Master.TotalNewTaxs = 0;
            if (taxList != null)
            {
                foreach (var tax in taxList)
                {
                    var totalAmount = Master.SalesReceiptDetailsTemps.Select(a => a.Total_Amount).Sum();
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

        /// <summary>
        /// prepare for Create SalesRecipt 
        /// </summary>
        /// <returns></returns>
        public static SalesReceiptMasterVM AddNewRecieptSalesForCreation(int cust_Id,string userId,int? orderid)
        {
            var db = new AccountingContainer();
            var masterVM = new SalesReceiptMasterVM();
            String StartCodeOfrevenue = "04";
            if (orderid != null && orderid != 0)
            {
                masterVM.order_Id = orderid.Value;
            }
           
            var emp = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
            if (emp != null)
            {
                masterVM.Emp_Id = emp.Emp_Id;
            }
            //sort the Invoices and get the last insert Invoice sales Receipt.
            var listOfSale = db.SalesReceiptMasters.ToList();
            if (listOfSale.FirstOrDefault() == null)
            {
                masterVM.Serial_Number = "SALRC-001";
            }
            else
            {
                var lastSerial = db.SalesReceiptMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                //using string substring method to get the number of the last inserted
                masterVM.Serial_Number = "SALRC-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(6, lastSerial.Serial_Number.Length - 6)) + 1).ToString("D3");
            }
            var sub = db.SubLedgers.FirstOrDefault();
            if (sub.InvoiceDesc != null || sub.InvoiceNotes != null)
            {
                masterVM.Description = sub.InvoiceDesc;
                masterVM.Notes = sub.InvoiceNotes;
            }
            masterVM.Invoice_Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
            masterVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            masterVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            masterVM.OrderList = new SelectList(db.OfferPriceMasters.Where(e => e.Offer_State == false), "OfferMas_Id", "Serial_Number");
            masterVM.MyDepartmentList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2&&a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            masterVM.MyBuList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            masterVM.MySubList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            //List<object> newListo = new List<object>();
            //var members1 = db.Customers.Where(e => e.Customer_State == Customer_State.نشط).ToList();
            //foreach (var member in members1)
            //    newListo.Add(new
            //    {
            //        Id = member.Customer_Id,
            //        Name = member.Customer_Name + "        " + member.Customer_Phone
            //    });
            var newListo = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                           select new
                           {
                               Id = cust.Customer_Id,
                               Name = cust.Customer_Name + "        " + cust.Customer_Phone

                           }).ToList();
            masterVM.MyCustomerList = new SelectList(newListo, "Id", "Name");
            masterVM.MyCustomerList = new SelectList(newListo, "Id", "Name");
            masterVM.MyStoresList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            masterVM.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");

            masterVM.EmpList = new SelectList(db.Employees.Where(e => e.Reg_ID != null), "Emp_Id", "Emp_Name");
        
            var thistaxinchart = db.ChartOfAccountings.Find(48);
            var anewList = (from Tax in db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false)
                            select new
                            {

                                Id = Tax.Tax_ID,
                                Name = Tax.Tax_Name + "   " + Tax.Tax_Percentage + "%"

                            }).ToList();
            masterVM.TaxList = new SelectList(anewList, "Id", "Name");
         
            var newList = (from pay in  db.PaymentMethods.Where(a=>a.IsActive!=false) select new {
                Id = pay.PaymentMethod_Id,
                Name = pay.Payment_Type

            });
         
            masterVM.PaymentMethodList = new SelectList(newList, "Id", "Name");

           
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0&& a.isActive != false).ToList();
            masterVM.AccountList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfrevenue)), " Account_Id", "Account_Name");
            masterVM.Account_Id = db.ChartOfAccountings.Where(c => c.Account_Code == "040101001"&& c.isActive != false).FirstOrDefault().Account_Id;

            masterVM.Fixed_Discount = 0;
            masterVM.Invoice_State = TransactionState.Pending;
            if (db.Stores.Count()>=1)
            {
                masterVM.Store_ID = db.Stores.FirstOrDefault().Store_ID;
            }
           
            var salesRecieptTemp = new SalesReceiptMasterTemp();
            var id = (from s in db.SalesReceiptMasterTemps
                      select s).ToList().LastOrDefault();
            if (id == null)
            { salesRecieptTemp.Master_Id = 1; }
            else
            {
                salesRecieptTemp.Master_Id = id.Master_Id + 1;
            }
            salesRecieptTemp.AllowDiscount = 0;
            salesRecieptTemp.TotalNewTaxs = 0;
            db.SalesReceiptMasterTemps.Add(salesRecieptTemp);
            db.SaveChanges();
            masterVM.Master_Id = salesRecieptTemp.Master_Id;
            masterVM.Customer_Id = cust_Id;
            return masterVM;
        }
        /// <summary>
        /// add to list of sales Recipt and Check the validation
        /// </summary>
        /// <param name="invoiceMasterDetailVM"></param>
        /// <returns></returns>
        public static string AddInvoiceDetailsTemp(SalesReceiptDetailsVM invoiceMasterDetailVM)
        {

            if (invoiceMasterDetailVM.productNotBatched_ID > 0)
            {

                invoiceMasterDetailVM.Product_Quantity = 1;
                var db = new AccountingContainer();
                var invoiceMasterTemp = db.SalesReceiptMasterTemps.Find(invoiceMasterDetailVM.Master_Id);
                if (invoiceMasterDetailVM.Master_Id == 0)
                {
                    var k = db.SalesReceiptDetails.Find(invoiceMasterDetailVM.Details_Id).Master_Id;
                    invoiceMasterTemp = db.SalesReceiptMasterTemps.Find(k);

                }
                else
                {
                    invoiceMasterTemp = db.SalesReceiptMasterTemps.Find(invoiceMasterDetailVM.Master_Id);
                }

                var invoiceMasterDetailsTemp = new SalesReceiptDetailsTemp();

                var productsss = db.Products.Find(invoiceMasterDetailVM.productNotBatched_ID);
                invoiceMasterDetailsTemp.Batch_ID = 0;
                invoiceMasterDetailsTemp.Product_Id = productsss.Prod_ID;



                var dtails = invoiceMasterTemp.SalesReceiptDetailsTemps;
                var dublicateinTemp = dtails.Where(z => z.SalesReceiptMasterTempMaster_Id == invoiceMasterTemp.Master_Id && z.Product_Id == invoiceMasterDetailsTemp.Product_Id).ToList();
                if (dublicateinTemp.FirstOrDefault() != null)
                {
                    return Resources.OfferPriceMasterCreateOffer.ProductExisitInListModelMessage;
                }
                invoiceMasterDetailsTemp.Product_Price =InvoiceMaster.GetPriceSale(invoiceMasterDetailVM.Store_Id, invoiceMasterDetailVM.productNotBatched_ID, invoiceMasterDetailVM.CustomerID);


                invoiceMasterDetailsTemp.Product_Quantity = invoiceMasterDetailVM.Product_Quantity;
                invoiceMasterDetailsTemp.Comition_Id = Convert.ToInt32(invoiceMasterDetailVM.commition);


                if (productsss.Tax_Id != null)
                {
                    invoiceMasterDetailsTemp.Product_Tax = productsss.Tax_Id.Value;
                }
                else
                {
                    invoiceMasterDetailsTemp.Product_Tax = 0;
                }

                decimal TotalBeforeDiscount = invoiceMasterDetailsTemp.Product_Price * invoiceMasterDetailsTemp.Product_Quantity;
             
                decimal discount = TotalBeforeDiscount * (invoiceMasterDetailVM.Discount / 100);
                invoiceMasterDetailsTemp.Detail_Discount = invoiceMasterDetailVM.Discount;
                invoiceMasterDetailsTemp.Detail_Discount_Amount = discount;
                decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                invoiceMasterDetailsTemp.TotalBeforeDisc = TotalBeforeDiscount;
                invoiceMasterDetailsTemp.TotalDiscount = discount;
                if (productsss.Tax_Id == 0 || productsss.Tax_Id == null)
                {
                    invoiceMasterDetailsTemp.Detail_Total_Tax = 0;
                }
                else
                {
                    var a = db.Taxes.Find(invoiceMasterDetailsTemp.Product_Tax);
                    invoiceMasterDetailsTemp.Detail_Total_Tax = TotalAfterDiscount * (a.Tax_Percentage / 100);
                }
                invoiceMasterDetailsTemp.Detail_Total_Cost = productsss.Prod_Cost_Price.Value * invoiceMasterDetailVM.Product_Quantity;



                invoiceMasterTemp.SalesReceiptDetailsTemps.Add(invoiceMasterDetailsTemp);


                invoiceMasterDetailsTemp.Total_Amount = (TotalBeforeDiscount + invoiceMasterDetailsTemp.Detail_Total_Tax) - discount;
                if (invoiceMasterDetailVM.commition != 0 && invoiceMasterDetailVM.commition != null)
                {
                    var com = db.PaymentMethods.Find(invoiceMasterDetailVM.commition);
                    if (com.Percentage == 0 && com.Operatin_Cost == 0)
                    {
                        invoiceMasterDetailsTemp.comition = 0;

                    }
                    else
                    {
                        invoiceMasterDetailsTemp.comition = (com.Percentage / 100) * invoiceMasterDetailsTemp.Total_Amount;
                        //  invoiceMasterDetailsTemp.Total_Amount = invoiceMasterDetailsTemp.Total_Amount + invoiceMasterDetailsTemp.comition.Value;
                    }


                }
                db.Entry(invoiceMasterTemp).State = EntityState.Modified;
                db.SaveChanges();
                return Resources.OfferPriceMasterCreateOffer.ProductIsAddedToListModelMessage;
            }
            else
            {
                return Resources.OfferPriceMasterCreateOffer.ProductIsNotFoundModelMessage;
            }

        }
        /// <summary>
        /// get all Detail Temp by master id 
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns> SalesReceiptDetailsTempVM</returns>
        public static SalesReceiptDetailsTempVM GetAllTempSalesReceiptDetails(int masterVmId)
        {
            var db = new AccountingContainer();
            var invoicedetalTemp = new SalesReceiptDetailsTempVM();
            invoicedetalTemp.invoiceTemp = db.SalesReceiptDetailsTemps.Where(invoice => invoice.SalesReceiptMasterTempMaster_Id == masterVmId).ToList();

            invoicedetalTemp.invoice_Id = masterVmId;
            try
            {
                invoicedetalTemp.AllowDiscount = db.SalesReceiptMasterTemps.Find(masterVmId).AllowDiscount.Value;
            
            }
            catch
            {
                invoicedetalTemp.AllowDiscount = db.SalesReceiptMasters.Find(masterVmId).FixedDiscount.Value;
                
            }
            try { invoicedetalTemp.TotalNewTaxs = db.SalesReceiptMasterTemps.Find(masterVmId).TotalNewTaxs.Value; }
            catch { invoicedetalTemp.TotalNewTaxs = 0; }
         
            return invoicedetalTemp;
        }
        /// <summary>
        /// create Sales Recipt and save it in database
        /// </summary>
        /// <param name="saleMastVM"></param>
        /// <param name="UserId"></param>
        /// <returns>Tuple<string ,int></returns>
        public static Tuple<string ,int>  addnewSaleReciept(SalesReceiptMasterVM saleMastVM , String UserId)
        {
            
            var db = new AccountingContainer();
            var saleRecietMaster = new SalesReceiptMaster();
            var saleRecietMasterTemp = db.SalesReceiptMasterTemps.Find(saleMastVM.Master_Id);
            if (saleMastVM != null)
            {               
                var listOfSale = db.SalesReceiptMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                //using string substring method to get the number of the last inserted

               //var listOfSale = db.SalesReceiptMasters.ToList();
                if (listOfSale == null)
                {
                    saleRecietMaster.Serial_Number = "SALRC-001";
                }
                else
                {
                    var lastSerial = db.SalesReceiptMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                    //using string substring method to get the number of the last inserted
                    saleRecietMaster.Serial_Number = "SALRC-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(6, lastSerial.Serial_Number.Length - 6)) + 1).ToString("D3");
                }

                saleRecietMaster.Description = saleMastVM.Description;
                var comp = db.Companies.Find(saleMastVM.Company_Id);
                if (comp != null)
                {
                    var date = Convert.ToDateTime(saleMastVM.Invoice_Date);
                    if (date < comp.StartFinancialDate)
                    {
                        saleRecietMaster.Invoice_Date = DateTime.Now;
                    }
                    else
                    {
                        saleRecietMaster.Invoice_Date = Convert.ToDateTime(saleMastVM.Invoice_Date);
                    }
                }
                else
                {
                    saleRecietMaster.Invoice_Date = DateTime.Now;
                }
                saleRecietMaster.Invoice_Date = Convert.ToDateTime(saleMastVM.Invoice_Date);
                saleRecietMaster.Customer_Id = saleMastVM.Customer_Id;
                saleRecietMaster.Notes = saleMastVM.Notes;
                saleRecietMaster.Master_Discount = saleMastVM.Master_Discount;
                var CurrencyResult = db.Currencies.Find(saleMastVM.Invoice_Currency);
                if (CurrencyResult != null)
                {
                    saleRecietMaster.Invoice_Currency = CurrencyResult.Currency_Type;
                }
                else
                {
                    saleRecietMaster.Invoice_Currency = "الريال السعودى";
                }
                if (saleMastVM.Store_ID==0 || saleMastVM.Store_ID == null) {

                    saleMastVM.Store_ID = db.Stores.FirstOrDefault().Store_ID;

                }
                saleRecietMaster.Store_ID = saleMastVM.Store_ID;
                saleRecietMaster.Company_Id = saleMastVM.Company_Id;
                saleRecietMaster.PaymentMethod_Id = saleMastVM.PaymentMethod_Id;
                saleRecietMaster.Account_Id = saleMastVM.Account_Id;
                
                saleRecietMaster.Reference = saleMastVM.Reference;
                saleRecietMaster.ApprovalCode = saleMastVM.ApprovalCode;
                saleRecietMaster.Exchange_Rate = saleMastVM.Exchange_Rate;
                saleRecietMaster.Invoice_State = TransactionState.Pending;
                saleRecietMaster.Branch_Id = saleMastVM.Branch_Id;
                saleRecietMaster.Department_ID = saleMastVM.Department_ID;
                saleRecietMaster.BussinesUnit_ID = saleMastVM.BussinesUnit_ID;
                var usedOffer = db.OfferPriceMasters.Find(saleMastVM.order_Id);
                if (usedOffer == null)
                { }
                else
                {
                   // saleRecietMaster. = usedOffer.Serial_Number;
                    usedOffer.Offer_State = true;
                    db.Entry(usedOffer).State = EntityState.Modified;
                }
                var employee = db.Employees.Find(saleMastVM.Emp_Id);
                if (employee != null)
                {
                    saleRecietMaster.Emp_Id = employee.Emp_Id;
                }
                if (saleMastVM.SubLedger_ID != null)
                {

                    saleRecietMaster.SubLedger_ID = saleMastVM.SubLedger_ID;

                }
                else if (saleMastVM.BussinesUnit_ID != null)
                {

                    saleRecietMaster.SubLedger_ID = saleMastVM.BussinesUnit_ID;

                }
                else if (saleMastVM.Department_ID != null)
                {

                    saleRecietMaster.SubLedger_ID = saleMastVM.Department_ID;

                }
                else if (saleMastVM.Branch_Id != null)
                {

                    saleRecietMaster.SubLedger_ID = saleMastVM.Branch_Id;

                }
                else { saleRecietMaster.SubLedger_ID = 1; }

                if (saleMastVM.Files.FirstOrDefault() != null)
                {
                    foreach (var file in saleMastVM.Files)
                    {
                        var filedetails = new SalesReceiptFile();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.FileContent = data;
                        filedetails.FileType = file.ContentType;
                        if (data.Length < 1000000)
                        {
                            saleRecietMaster.SalesReceiptFiles.Add(filedetails);
                        }
                        
                    }
                }
                if (saleRecietMasterTemp.SalesReceiptDetailsTemps.ToList().Count < 1)
                {
                      return Tuple.Create("محتوريات الفاتورة فارغة", 0);
                }
                var qrMaster = new MasterQRInvoice();
                qrMaster.QrGuid = Guid.NewGuid();
                qrMaster.DetailQRInvoices = new List<DetailQRInvoice>();
                qrMaster.QRPaymentMethods = new List<QRPaymentMethod>();
                qrMaster.TaxQRInvoices = new List<TaxQRInvoice>();
                foreach (var item in saleRecietMasterTemp.SalesReceiptDetailsTemps.ToList())
                {
                    var details = new SalesReceiptDetails();
                    details.Details_Id = item.Details_Id;
                    details.Product_Quantity = item.Product_Quantity;
                    details.Product_Price = item.Product_Price;
                    details.Product_Tax = item.Product_Tax;
                    details.Detail_Discount = item.Detail_Discount;
                    details.Detail_Discount_Amount = item.Detail_Discount_Amount;
                    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    decimal discount = TotalBeforeDiscount * (item.Detail_Discount.Value / 100);
                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                    details.Batch_ID = item.Batch_ID;
                    if (details.Product_Tax == 0)
                    {
                        details.Detail_Total_Tax = 0;
                    }
                    else
                    {
                        var tax = Convert.ToDecimal(db.Taxes.Find(details.Product_Tax).Tax_Percentage);
                        details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                    }
                    // if the product in batch
                    if (item.Batch_ID > 0)
                    {
                        ProductBatches detailBatch = db.ProductBatches.Find(details.Batch_ID);
                        details.Product_Id = detailBatch.Product_ID;
                        // Checks for the products quantity if available or not
                        //if (item.Product_Quantity > detailBatch.Product_Quantity)
                        //    return "Not enough quantity of " + detailBatch.Product.Prod_Name;
                        // Calculate the Total_Cost

                        details.Detail_Total_Cost = item.Product_Quantity * detailBatch.Product_Cost;
                        
                        


                        var price = item.Product_Price * item.Product_Quantity;
                        details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                        saleRecietMaster.Total_Amount += details.Total_Amount;
                        saleRecietMaster.Total_Cost += details.Detail_Total_Cost;
                        saleRecietMaster.Total_Tax += details.Detail_Total_Tax;
                        details.Reverse_Total_Tax = details.Detail_Total_Tax;
                        details.Reverse_Quantity = details.Product_Quantity;

                        details.Reverse_Total_Amount = details.Total_Amount;
                        saleRecietMaster.SalesReceiptDetails.Add(details);
                        db.SalesReceiptDetailsTemps.Remove(item);//
                 
                    }
                    else
                    {
                        Product productss = db.Products.Find(item.Product_Id);
                        details.Product_Id = productss.Prod_ID;
                      
                        // Calculate the Total_Cost

                  
                        if (productss.Prod_Type == Prod_Type.خدمة)
                        {
                            details.Detail_Total_Cost = 0;
                        }
                        else
                        {


                            details.Detail_Total_Cost = item.Product_Quantity * productss.Prod_Cost_Price.Value;
                        }


                        var price = item.Product_Price * item.Product_Quantity;
                        details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                        saleRecietMaster.Total_Amount += details.Total_Amount;
                        saleRecietMaster.Total_Cost += details.Detail_Total_Cost;
                        saleRecietMaster.Total_Tax += details.Detail_Total_Tax;
                        details.Reverse_Total_Tax = details.Detail_Total_Tax;
                        details.Reverse_Quantity = details.Product_Quantity;

                        details.Reverse_Total_Amount = details.Total_Amount;
                        saleRecietMaster.SalesReceiptDetails.Add(details);
                        db.SalesReceiptDetailsTemps.Remove(item);
                    }
                  

                    ///// RR
                    ///
                    var invoiceQRDetail = new DetailQRInvoice();
                    invoiceQRDetail.Batch_ID = details.Batch_ID;
                    invoiceQRDetail.Product_Id = details.Product_Id;
                    invoiceQRDetail.Product_Quantity = details.Product_Quantity;
                    invoiceQRDetail.Product_Tax = details.Product_Tax;
                    invoiceQRDetail.Detail_Discount = item.Detail_Discount;
                    invoiceQRDetail.Detail_Discount_Amount = item.Detail_Discount_Amount;

                    invoiceQRDetail.Detail_Total_Cost = details.Detail_Total_Cost;

                    invoiceQRDetail.Total_Amount = details.Total_Amount;

                    invoiceQRDetail.Product_Price = details.Product_Price;
                    invoiceQRDetail.Detail_Total_Tax = details.Detail_Total_Tax;
                    invoiceQRDetail.ProductPriceCost = details.Product_Cost.Value;
                    qrMaster.DetailQRInvoices.Add(invoiceQRDetail);
                }
                saleRecietMaster.TotalNewTaxs = 0;
                if (saleMastVM.Taxes != null)
                {
                    foreach (var tax in saleMastVM.Taxes)
                    {
                        var taxo = new SalesReceiptTax();
                        taxo.Tax_Id = tax;

                        taxo.Amount = (db.Taxes.Find(Convert.ToInt32(tax)).Tax_Percentage / 100) * saleRecietMaster.SalesReceiptDetails.Select(a => a.Total_Amount).Sum();
                        saleRecietMaster.TotalNewTaxs += taxo.Amount;

                        saleRecietMaster.SalesReceiptTaxes.Add(taxo);

                        ////
                        var qrtax = new TaxQRInvoice();
                        qrtax.Tax_Id = tax;

                        qrtax.Amount = taxo.Amount;
                        qrMaster.TaxQRInvoices.Add(qrtax);


                    }
                }
            

                saleRecietMaster.FixedDiscount = saleMastVM.Fixed_Discount;
                var RealDisc = (saleRecietMaster.SalesReceiptDetails.Select(a => a.Detail_Discount_Amount.Value ).Sum()/ saleRecietMaster.SalesReceiptDetails.Select(h=>h.Product_Price * h.Product_Quantity).Sum()) * 100;
                if (Math.Round(RealDisc, 4, MidpointRounding.AwayFromZero) != Math.Round(saleRecietMaster.Master_Discount, 4, MidpointRounding.AwayFromZero))
                {
                    saleRecietMaster.Master_Discount = RealDisc;
                }
                saleRecietMaster.TotaL_After_Fixed = (saleRecietMaster.Total_Amount+ saleRecietMaster.TotalNewTaxs) - saleRecietMaster.FixedDiscount;
                var payment = db.PaymentMethods.Find(saleRecietMaster.PaymentMethod_Id);

                saleRecietMaster.Total_Payment_Method_Cost = (saleRecietMaster.TotaL_After_Fixed.Value * (payment.Percentage.Value / 100));
               
              
                if (payment.Operatin_Cost>0) {
                    saleRecietMaster.Total_Payment_Method_Cost = saleRecietMaster.Total_Payment_Method_Cost + payment.Operatin_Cost.Value;
                    
                }
                var paymentQR = new QRPaymentMethod();
                paymentQR.PaymentMethod_Id = saleRecietMaster.PaymentMethod_Id;
                paymentQR.Commution = 0;
                paymentQR.Amount = saleRecietMaster.TotaL_After_Fixed.Value;
                qrMaster.QRPaymentMethods.Add(paymentQR);


                //add QR Invoice
                qrMaster.AllowDiscount = saleRecietMaster.FixedDiscount.Value;
                qrMaster.ApprovalCode = saleRecietMaster.Reference;
                qrMaster.Company_Id = saleRecietMaster.Company_Id;
                qrMaster.Customer_Id = saleRecietMaster.Customer_Id;
                qrMaster.Description = saleRecietMaster.Description;
                qrMaster.Employee_Id = saleRecietMaster.Emp_Id.Value;
                qrMaster.Invoice_Date = saleRecietMaster.Invoice_Date;
                qrMaster.Invoice_State = saleRecietMaster.Invoice_State;
                qrMaster.InvoiceType_Id = 2;
                qrMaster.Master_Discount = saleRecietMaster.Master_Discount;

                qrMaster.Reference = saleRecietMaster.Reference;
                qrMaster.Serial_Number = saleRecietMaster.Serial_Number;
                qrMaster.Store_ID = saleRecietMaster.Store_ID;
                qrMaster.Subledger_Id = saleRecietMaster.SubLedger_ID;
                qrMaster.Notes = saleRecietMaster.Notes;
                qrMaster.TotalNewTaxs = saleRecietMaster.TotalNewTaxs.Value;
                qrMaster.Account_Id = saleRecietMaster.Account_Id;
                qrMaster.Total_Amount = saleRecietMaster.Total_Amount;
                qrMaster.InvoiceType = "فاتورة مبيعات نقدي";
                qrMaster.Total_Payment_Method_Cost = 0;
                qrMaster.Total_Tax = saleRecietMaster.Total_Tax;
                qrMaster.Branch_Id = saleRecietMaster.Branch_Id;
                qrMaster.Department_ID = saleRecietMaster.Department_ID;
                qrMaster.BussinesUnit_ID = saleRecietMaster.BussinesUnit_ID;


                qrMaster.Total_Cost = saleRecietMaster.Total_Cost;
                qrMaster.Notes = saleRecietMaster.Notes;
                qrMaster.Number = InvoiceSerial.generateSerialForInvoice();
                qrMaster.DateOfCreate = DateTime.Now;
                saleRecietMaster.Number = qrMaster.Number;
                saleRecietMaster.Serial_Number = "SALRC-" + System.Text.RegularExpressions.Regex.Match(saleRecietMaster.Number, @"\d+").Value;
                qrMaster.Serial_Number = saleRecietMaster.Serial_Number;
                db.MasterQRInvoices.Add(qrMaster);
                db.SaveChanges();
                saleRecietMaster.QRNumber = qrMaster.Id;
                
                db.SalesReceiptMasters.Add(saleRecietMaster);
                db.SalesReceiptMasterTemps.Remove(saleRecietMasterTemp);
                db.SaveChanges();
                db.Dispose();

                CreateTransactionBatch(saleRecietMaster , UserId);

                return Tuple.Create("تم انشاء الفاتورة بنجاح",saleRecietMaster .Master_Id);

            }
            return Tuple.Create("يوجد خطأ فى انشاء الفاتورة", 0); 
        }
        /// <summary>
        /// make Transaction batch when create sales Recipt
        /// </summary>
        /// <param name="saleRecietMaster"></param>
        /// <param name="UerId"></param>
        public static void CreateTransactionBatch(SalesReceiptMaster saleRecietMaster , String UerId)
        {
            var trans = new TransactionBatch();
            trans.Description = saleRecietMaster.Description;
            trans.Date = saleRecietMaster.Invoice_Date;
            trans.OperationName = "فاتورة مبيعات نقدية";
            // TODO: Edit UserId
            trans.UserId = UerId;
            trans.OperationId = saleRecietMaster.Master_Id;
            trans.Operation_Code = saleRecietMaster.Serial_Number;
            TransactionBatch.AddNewTransaction(trans);
        }
        /// <summary>
        /// get that file by its id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>SalesReceiptFile</returns>
        public static SalesReceiptFile getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.SalesReceiptFiles.Find(fileId);
            return file;
        }
        public static string deleteFile(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.SalesReceiptFiles.Find(fileId);
            db.SalesReceiptFiles.Remove(file);
            db.SaveChanges();
            return Resources.validate.DeleteSuccessful;
        }

        /// <summary>
        /// get List Of file by master id
        /// </summary>
        /// <param name="invoiceMasterId"></param>
        /// <returns></returns>
        public static List<SalesReceiptFile> GetAllFiles(int invoiceMasterId)
        {
            var db = new AccountingContainer();
            var files = db.SalesReceiptFiles.Where(emp => emp.Master_Id == invoiceMasterId).ToList();
            return files;
        }
        /// <summary>
        /// check if gl make or not to make gl
        /// </summary>
        /// <param name="salesinvoiceMasterVM"></param>
        /// <returns></returns>
        public static String UpdateInvoiceMaster(SalesReceiptMasterVM salesinvoiceMasterVM)
        {
            var db = new AccountingContainer();

            SalesReceiptMaster salesmaster = db.SalesReceiptMasters.Find(salesinvoiceMasterVM.Master_Id);
            OperationRun.Addnewoperation(salesmaster.Serial_Number);
            var operation = db.OperationRuns.Where(a => a.Code == salesmaster.Serial_Number).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(salesmaster.Serial_Number);
                return Resources.InvoiceMasterDetails.WaitingMSGAndTryAgain;
            }
            // Edit master invoice state to the new state
            if (salesmaster.Invoice_State == salesinvoiceMasterVM.Invoice_State)
            {
                OperationRun.DeleteOperation(salesmaster.Serial_Number);
                return Resources.InvoiceMasterDetails.ItsDoneWorking + " " + salesmaster.Invoice_State + " " + Resources.InvoiceMasterDetails.Before;
            }
            else
            {
                salesmaster.Invoice_State = salesinvoiceMasterVM.Invoice_State;
            }
          
            db.Entry(salesmaster).State = EntityState.Modified;

            // Create GL for the invoice 
           var msg= CreateInvoiceGL(salesmaster , db);
            db.SaveChanges();
            OperationRun.DeleteOperation(salesmaster.Serial_Number);
            db.Dispose();
            return msg;
        }
        /// <summary>
        /// make gl of this invoice making post or void and check if that quantity is available or not
        /// </summary>
        /// <param name="master"></param>
        /// <param name="db"></param>
        /// <returns>String</returns>
        public static String CreateInvoiceGL(SalesReceiptMaster master, AccountingContainer db)
        {
            var msg = "";
            //var db = new AccountingContainer();
            GLMaster glMaster = new GLMaster();
            glMaster.GLCode = master.Serial_Number;
            glMaster.GL_Date = master.Invoice_Date;
            glMaster.GL_Desc = master.Description;
            glMaster.Company_Id = master.Company_Id;
            glMaster.GLType_Id = "مبيعات نقدي";
            glMaster.Currency_Id = "الريال السعودى";
            glMaster.GLState = TransactionState.Pending;
            glMaster.Auto = true;
            List<ProuctCostUtilites> ProductCostList = new List<ProuctCostUtilites> { };
            GLDetail AccountReceivableGL = new GLDetail(),
                AccountCustomerGL = new GLDetail(),
                AccountCustomerGLDebit = new GLDetail(),
               revenueGL = new GLDetail(),

               inventoryGL = new GLDetail(),
               totalCostGL = new GLDetail(),
               bankcomution = new GLDetail(),
               discount = new GLDetail(),
               fixedDiscount = new GLDetail();
            fixedDiscount.Bu_Id = 0;

            AccountReceivableGL.Bu_Id =
            AccountCustomerGL.Bu_Id =
            AccountCustomerGLDebit.Bu_Id =
            revenueGL.Bu_Id =
            inventoryGL.Bu_Id =
            totalCostGL.Bu_Id =
             discount.Bu_Id =
            bankcomution.Bu_Id = 0;

            var bankAccount = db.PaymentMethods.Find(master.PaymentMethod_Id);

            var ChartOfAccounRecive = db.ChartOfAccountings.Find(bankAccount.Bank_Account_Id);
            AccountReceivableGL.Account_Id = ChartOfAccounRecive.Account_Id;
            AccountReceivableGL.Account_Name = ChartOfAccounRecive.Account_Name;

            var ChartOfAccounRevenue = db.ChartOfAccountings.Find(master.Account_Id);
            revenueGL.Account_Id = ChartOfAccounRevenue.Account_Id;
            revenueGL.Account_Name = ChartOfAccounRevenue.Account_Name;
            revenueGL.SubLedger_Id = master.SubLedger_ID;


            var store = db.Stores.Find(master.Store_ID);
            var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).First();
            inventoryGL.Account_Id = ChartOfAccounInventory.Account_Id;
            inventoryGL.Account_Name = ChartOfAccounInventory.Account_Name;

            var ChartOfAccounCost = db.ChartOfAccountings.Find(105);
            totalCostGL.Account_Id = ChartOfAccounCost.Account_Id;
            totalCostGL.Account_Name = ChartOfAccounCost.Account_Name;
            totalCostGL.SubLedger_Id = master.SubLedger_ID;

            var DiscountCost = db.ChartOfAccountings.Find(78);
            discount.Account_Id = DiscountCost.Account_Id;
            discount.Account_Name = DiscountCost.Account_Name;
            discount.SubLedger_Id = master.SubLedger_ID;

            var a = db.PaymentMethods.Find(master.PaymentMethod_Id).Expense_Account_Id;
            if (a != null)
            {
                var bank = db.ChartOfAccountings.Find(a);
                bankcomution.Account_Id = bank.Account_Id;
                bankcomution.Account_Name = bank.Account_Name;
                bankcomution.SubLedger_Id = master.SubLedger_ID;
            }

            AccountReceivableGL.Asset_Id =
           revenueGL.Asset_Id =

           inventoryGL.Asset_Id =
           AccountCustomerGL.Asset_Id =
           AccountCustomerGLDebit.Asset_Id =
            discount.Asset_Id =
           totalCostGL.Asset_Id = 0;
            var chartFixedDiscount = db.ChartOfAccountings.Find(184);
            fixedDiscount.Account_Id = chartFixedDiscount.Account_Id;
            fixedDiscount.Account_Name = chartFixedDiscount.Account_Name;
            fixedDiscount.SubLedger_Id = master.SubLedger_ID;

            var ChartOfAccounPay = db.ChartOfAccountings.Find(37);
            AccountCustomerGL.Account_Id = ChartOfAccounPay.Account_Id;
            AccountCustomerGL.Account_Name = ChartOfAccounPay.Account_Name;
            AccountCustomerGLDebit.Account_Id = ChartOfAccounPay.Account_Id;
            AccountCustomerGLDebit.Account_Name = ChartOfAccounPay.Account_Name;
            var RealDetailList = getMaterialOCompoundProduct(master.SalesReceiptDetails.ToList(), master.Store_ID);
           
            if (master.Customer_Id != null&&master.Customer_Id!=0)
            {
                var customer = db.Customers.Find(master.Customer_Id);
                var customerSubledger = db.Accounts.Where(k => k.Account_Name == customer.Customer_Name).FirstOrDefault();
                AccountCustomerGL.Account_Child_ID = customerSubledger.Accounts_Id;
                AccountCustomerGL.Account_CHild_Name = customerSubledger.Account_Name;
                AccountCustomerGLDebit.Account_Child_ID = customerSubledger.Accounts_Id;
                AccountCustomerGLDebit.Account_CHild_Name = customerSubledger.Account_Name;
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    switch (master.Invoice_State)
                    {
                        //Post Invoice
                        case TransactionState.Post:
                            master.Total_Cost = RealDetailList.Select(c => c.Detail_Total_Cost).Sum();
                            glMaster.GLState = TransactionState.Post;
                            var gl = db.GLMasters.Where(f=> f.GLCode == glMaster.GLCode).FirstOrDefault();
                            if (gl != null && gl.GLState == TransactionState.Post)
                            {
                                return Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade;
                            }
                            else if (gl != null && gl.GLState == TransactionState.Hold)
                            {
                                return CreateInvoiceGLHolding(master, db, gl, transaction);

                            }

                            // Customer GL
                            AccountCustomerGL.GLDetail_Credit = master.TotaL_After_Fixed.Value;
                            AccountCustomerGLDebit.GLDetail_Debit = master.TotaL_After_Fixed.Value;

                            // Revenue GL

                            // Taxes GL


                            var pay = db.PaymentMethods.Find(master.PaymentMethod_Id);
                            bankcomution.GLDetail_Debit = master.Total_Payment_Method_Cost;
                            AccountReceivableGL.GLDetail_Debit = master.TotaL_After_Fixed.Value - master.Total_Payment_Method_Cost;
                            fixedDiscount.GLDetail_Debit = master.FixedDiscount.Value;
                            foreach (var detail in master.SalesReceiptDetails)
                            {
                                var TotalBefore = detail.Product_Price * detail.Product_Quantity;
                                revenueGL.GLDetail_Credit += TotalBefore;
                            }
                            discount.GLDetail_Debit = revenueGL.GLDetail_Credit * (master.Master_Discount / 100);

                           
                            db.ProductBatches.Load();
                            var errorMessage = new List<string> { };
                            foreach (var detail in RealDetailList)
                            {
                                var thisproduct = db.Products.Find(detail.Product_Id);
                                if (thisproduct.Prod_Type == Prod_Type.منتج || thisproduct.Prod_Type == Prod_Type.اولي)
                                {
                                    ProductBatches detailBatch = db.ProductBatches.Where(p => p.Product_ID == detail.Product_Id && p.Store_ID == master.Store_ID).ToList().FirstOrDefault();
                                    if (detailBatch != null)
                                    {
                                        var productName = db.Products.Find(detail.Product_Id).Prod_Name;
                                        if (detail.Product_Quantity > detailBatch.Product_Quantity)
                                        {
                                           
                                            master.Invoice_State = TransactionState.Pending;
                                            //db.SaveChanges();
                                            errorMessage.Add(Resources.InvoiceMasterDetails.NotEnoughProductInTheInventory + " " + productName);

                                        }
                                    }
                                    else
                                    {
                                        var productName = db.Products.Find(detail.Product_Id).Prod_Name;
                                        master.Invoice_State = TransactionState.Pending;
                                        errorMessage.Add(Resources.InvoiceMasterDetails.NotEnoughProductInTheInventory + " " + productName);
                                    }
                                }
                            }
                            if (errorMessage.Count() != 0)
                            {


                                string combindedString = string.Join("\n", errorMessage.ToArray());
                                master.Invoice_State = TransactionState.Hold;
                                glMaster.GLState = TransactionState.Hold;
                                //return combindedString;
                            }
                            if (master.Invoice_State != TransactionState.Hold)
                            {
                                foreach (var detail in RealDetailList)
                                {
                                    var thisproduct = db.Products.Find(detail.Product_Id);
                                    if (thisproduct.Prod_Type == Prod_Type.منتج || thisproduct.Prod_Type == Prod_Type.اولي)
                                    {
                                        ProductBatches detailBatch = db.ProductBatches.Where(p => p.Product_ID == detail.Product_Id && p.Store_ID == master.Store_ID).ToList().FirstOrDefault();
                                        if (detailBatch != null)
                                        {


                                            var ProductCost = new ProuctCostUtilites();
                                            ProductCost.Product_ID = detailBatch.Product_ID;
                                            ProductCost.ProductCost = detailBatch.Product_Cost;

                                            detailBatch.Product_Quantity -= detail.Product_Quantity;
                                            detailBatch.Batch_Cost -= detailBatch.Product_Cost * detail.Product_Quantity;
                                            if (detailBatch.Product_Quantity == 0)
                                            {
                                                detailBatch.Product_Cost = 0;

                                            }
                                            else
                                            {
                                                detailBatch.Product_Cost = detailBatch.Batch_Cost / detailBatch.Product_Quantity;
                                            }

                                            db.Entry(detailBatch).State = EntityState.Modified;
                                            ProductCostList.Add(ProductCost);


                                        }
                                        else
                                        {

                                            var productName = db.Products.Find(detail.Product_Id).Prod_Name;
                                            master.Invoice_State = TransactionState.Pending;

                                            return Resources.InvoiceMasterDetails.NotEnoughProductInTheInventory + " " + productName;
                                        }
                                    }
                                }

                                db.ProductBatches.Load();
                                foreach (var detail in RealDetailList)
                                {
                                    var thisproduct = db.Products.Find(detail.Product_Id);
                                    if (thisproduct.Prod_Type == Prod_Type.منتج || thisproduct.Prod_Type == Prod_Type.اولي)
                                    {
                                        ProductBatches detailBatch = db.ProductBatches.Where(p => p.Product_ID == detail.Product_Id && p.Store_ID == master.Store_ID).ToList().FirstOrDefault();
                                        if (detailBatch.Store_ID != null)
                                        {
                                            if (detailBatch.Product_Cost == 0)
                                            {
                                                var LastBatch = ProductCostList.Where(c => c.Product_ID == detail.Product_Id).FirstOrDefault();
                                                detail.Product_Cost = LastBatch.ProductCost;
                                            }
                                            else
                                            {
                                                detail.Product_Cost = detailBatch.Product_Cost;
                                            }

                                            detail.Detail_Total_Cost = detail.Product_Quantity * detail.Product_Cost.Value;
                                            if (detail.Details_Id != 0)
                                            {
                                                db.Entry(detail).State = EntityState.Modified;
                                            }
                                            //make product card for Report
                                            var storecard = new StoreCard();
                                            storecard.CostPrice = detail.Product_Cost.Value;
                                            storecard.Date = DateTime.Now;
                                            storecard.Store_Id = master.Store_ID;
                                            storecard.Quantity = detail.Product_Quantity;
                                            storecard.SalePrice = detail.Product_Price;
                                            storecard.Product_Id = detail.Product_Id;
                                            storecard.IncreaseQuantity = false;
                                            storecard.TypeOfDocument = "فاتورة مبيعات نقدية";
                                            storecard.Document_Serial = master.Serial_Number;
                                            storecard.TotalCost = detail.Product_Quantity * detail.Product_Cost.Value;
                                            storecard.TotalAmount = detail.Total_Amount;
                                            storecard.Account_Id = AccountCustomerGL.Account_Child_ID;
                                            storecard.Subledger_Id = master.SubLedger_ID;
                                            db.StoreCards.Add(storecard);
                                            //Store.AddinginstorCard(storecard);
                                        }
                                    }
                                }
                                // Total Cost GL
                                totalCostGL.GLDetail_Debit = RealDetailList.Select(z => z.Detail_Total_Cost).ToList().Sum();
                                // Inventory GL
                                inventoryGL.GLDetail_Credit = RealDetailList.Select(z => z.Detail_Total_Cost).ToList().Sum();
                            }
                            if (master.SalesReceiptTaxes.FirstOrDefault() != null)
                            {
                                foreach (var tax in master.SalesReceiptTaxes)
                                {
                                    var gldetailTax = new GLDetail();
                                    var taxs = db.Taxes.Find(Convert.ToInt32(tax.Tax_Id));
                                    var AccountTax = db.ChartOfAccountings.Where(z => z.Account_Code == taxs.Tax_Code).FirstOrDefault();
                                    gldetailTax.Account_Id = AccountTax.Account_Id;
                                    gldetailTax.Account_Name = AccountTax.Account_Name;
                                    gldetailTax.GLDetail_Credit = tax.Amount;
                                    gldetailTax.GLDetail_Debit = 0;
                                    glMaster.GLDetails.Add(gldetailTax);


                                }



                            }
                           
                            break;


                        // Void Invoice
                        case TransactionState.Void:
                            glMaster.GLType_Id = "مبيعات نقدي عكسي";
                            glMaster.GLState = TransactionState.Void;
                            var glCheck = db.GLMasters.Where(s => s.GLCode == glMaster.GLCode && s.GLState == glMaster.GLState).FirstOrDefault();
                            if (glCheck != null)
                            {
                                return Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade;
                            }
                            var reverse = db.ReverseSalesReceiptMasters.Where(c => c.SalesRecipt_Id == master.Master_Id).ToList();
                            if (reverse.FirstOrDefault() != null)
                            {
                                msg = Resources.InvoiceMasterDetails.InvoiceCouldNotBeVoid;
                                SalesReceiptMaster salesmaster = db.SalesReceiptMasters.Find(master.Master_Id);
                                // Edit master invoice state to the new state
                                salesmaster.Invoice_State = TransactionState.Post;
                                db.Entry(salesmaster).State = EntityState.Modified;
                                db.SaveChanges();
                                return msg;
                            }
                            if (master.HasOff == true)
                            {

                                msg = Resources.InvoiceMasterDetails.InvoiceHasBeenIncluded + " ";
                                master.Invoice_State = TransactionState.Post;
                                db.SaveChanges();
                                return msg;
                            }
                            else
                            {
                                return MakeVoidForSales(master.Master_Id);
                            }
                           
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
                        if (master.Customer_Id != null&& master.Customer_Id != 0)
                        {
                            glMaster.GLDetails.Add(AccountCustomerGL);
                            glMaster.GLDetails.Add(AccountCustomerGLDebit);
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
                        if (fixedDiscount.GLDetail_Credit == 0 && fixedDiscount.GLDetail_Debit == 0)
                        {

                        }
                        else
                        {
                            glMaster.GLDetails.Add(fixedDiscount);
                        }
                        glMaster = addglDetailTax(glMaster, master.Master_Id);
                        db.GLMasters.Add(glMaster);
                        GLMaster.GetBalanceForAccount(glMaster);
                        GLMaster.GetBalanceForSubLedger(glMaster);
                        msg = Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade;
                        db.SaveChanges();
                        transaction.Commit();
                    }

                }

                catch (Exception)
                {
                    transaction.Rollback();
                    db.Dispose();
                }
            }
            if (master.Invoice_State == TransactionState.Post)
            {
                msg = Resources.validate.GLDone;
            }
            else if (master.Invoice_State == TransactionState.Hold)
            {

                 msg = Resources.validate.TheInvoiceHasBeenPartiallyApprovedUntilThequantitiesAreAvailableInTheStore;
            }
            else
            {
                msg = Resources.validate.GLReverse;
            }

            return msg;
        }
        public static string CreateInvoiceGLHolding(SalesReceiptMaster master, AccountingContainer db, GLMaster gl, DbContextTransaction transaction)
        {
            var RealDetailList = getMaterialOCompoundProduct(master.SalesReceiptDetails.ToList(), master.Store_ID);
            List<ProuctCostUtilites> ProductCostList = new List<ProuctCostUtilites> { };
           
            
            Accounts customerAccount = null;
            if (master.Customer_Id > 0)
            {
                var cust = db.Customers.Find(master.Customer_Id);
                 customerAccount = db.Accounts.Where(subled => subled.Account_Code == cust.Customer_Code).FirstOrDefault();
            }
            var inventoryGL = new GLDetail();
            var totalCostGL = new GLDetail();
            var store = db.Stores.Find(master.Store_ID);
            var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).First();
            inventoryGL.Account_Id = ChartOfAccounInventory.Account_Id;
            inventoryGL.Account_Name = ChartOfAccounInventory.Account_Name;

            var ChartOfAccounCost = db.ChartOfAccountings.Find(105);
            totalCostGL.Account_Id = ChartOfAccounCost.Account_Id;
            totalCostGL.Account_Name = ChartOfAccounCost.Account_Name;
            db.ProductBatches.Load();
            var errorMessage = new List<string> { };
            foreach (var detail in RealDetailList)
            {
                var thisproduct = db.Products.Find(detail.Product_Id);
                if (thisproduct.Prod_Type == Prod_Type.منتج || thisproduct.Prod_Type == Prod_Type.اولي)
                {
                    ProductBatches detailBatch = db.ProductBatches.Where(a => a.Product_ID == detail.Product_Id && a.Store_ID == master.Store_ID).ToList().FirstOrDefault();
                    try
                    {
                        if (detailBatch.Store_ID != null)
                        {
                            if (detail.Product_Quantity > detailBatch.Product_Quantity)
                            {
                                var productName = db.Products.Find(detail.Product_Id).Prod_Name;
                                master.Invoice_State = TransactionState.Hold;
                                //db.SaveChanges();
                                errorMessage.Add(Resources.InvoiceMasterDetails.NotEnoughProductInTheInventory + " " + productName);

                            }
                        }
                    }
                    catch
                    {
                        var productName = db.Products.Find(detail.Product_Id).Prod_Name;
                        master.Invoice_State = TransactionState.Hold;
                        //db.SaveChanges();
                        errorMessage.Add(Resources.InvoiceMasterDetails.NotEnoughProductInTheInventory + " " + productName);
                    }

                }
            }

            if (errorMessage.Count() != 0)
            {


                string combindedString = string.Join("\n", errorMessage.ToArray());
                return combindedString;
            }
            foreach (var detail in RealDetailList)
            {
                var thisproduct = db.Products.Find(detail.Product_Id);
                if (thisproduct.Prod_Type == Prod_Type.منتج || thisproduct.Prod_Type == Prod_Type.اولي)
                {
                    ProductBatches detailBatch = db.ProductBatches.Where(a => a.Product_ID == detail.Product_Id && a.Store_ID == master.Store_ID).ToList().LastOrDefault();
                    if (detailBatch.Store_ID != null)
                    {
                        var ProductCost = new ProuctCostUtilites();
                        ProductCost.Product_ID = detailBatch.Product_ID;
                        ProductCost.ProductCost = detailBatch.Product_Cost;


                        detailBatch.Product_Quantity -= detail.Product_Quantity;
                        detailBatch.Batch_Cost -= detailBatch.Product_Cost * detail.Product_Quantity;
                        if (detailBatch.Product_Quantity == 0)
                        {
                            detailBatch.Product_Cost = 0;
                        }
                        else
                        {
                            detailBatch.Product_Cost = detailBatch.Batch_Cost / detailBatch.Product_Quantity;

                        }
                        db.Entry(detailBatch).State = EntityState.Modified;
                        ProductCostList.Add(ProductCost);
                    }
                }
                else
                {

                }

            }

            db.ProductBatches.Load();

            foreach (var detail in RealDetailList)
            {
                var thisproduct = db.Products.Find(detail.Product_Id);
                if (thisproduct.Prod_Type == Prod_Type.منتج || thisproduct.Prod_Type == Prod_Type.اولي)
                {
                    ProductBatches detailBatch = db.ProductBatches.Where(a => a.Product_ID == detail.Product_Id && a.Store_ID == master.Store_ID).ToList().FirstOrDefault();
                    if (detailBatch.Store_ID != null)
                    {

                        if (detailBatch.Product_Cost == 0)
                        {
                            var LastBatch = ProductCostList.Where(a => a.Product_ID == detail.Product_Id).FirstOrDefault();
                            detail.Product_Cost = LastBatch.ProductCost;
                        }
                        else
                        {
                            detail.Product_Cost = detailBatch.Product_Cost;
                        }
                        detail.Detail_Total_Cost = detail.Product_Quantity * detail.Product_Cost.Value;

                        if (detail.Details_Id != 0)
                        {
                            db.Entry(detail).State = EntityState.Modified;
                        }




                        //make product card for Report
                        var storecard = new StoreCard();
                        storecard.CostPrice = detail.Product_Cost.Value;
                        storecard.Date = DateTime.Now;
                        storecard.Store_Id = master.Store_ID;
                        storecard.Quantity = detail.Product_Quantity;
                        storecard.SalePrice = detail.Product_Price;
                        storecard.Product_Id = detail.Product_Id;
                        storecard.IncreaseQuantity = false;
                        storecard.TypeOfDocument = "فاتورة مبيعات نقدي ";
                        storecard.Document_Serial = master.Serial_Number;
                        storecard.TotalCost = detail.Product_Quantity * detail.Product_Cost.Value;
                        storecard.TotalAmount = detail.Total_Amount;
                        if (customerAccount != null)
                        {
                            storecard.Account_Id = customerAccount.Accounts_Id;
                        }
                        storecard.Subledger_Id = master.SubLedger_ID;
                        db.StoreCards.Add(storecard);
                        //Store.AddinginstorCard(storecard);

                        inventoryGL.GLDetail_Credit += storecard.TotalCost.Value;
                        // Total Cost GL
                        totalCostGL.GLDetail_Debit += storecard.TotalCost.Value;

                    }
                }
            }
            inventoryGL.GLMasterGLMaster_Id = gl.GLMaster_Id;
            totalCostGL.GLMasterGLMaster_Id = gl.GLMaster_Id;
            totalCostGL.SubLedger_Id = master.SubLedger_ID;
            db.GLDetails.Add(inventoryGL);

            db.GLDetails.Add(totalCostGL);
            db.SaveChanges();
            if (gl.GLDetails.Where(a => a.Account_Id == 105).Count() >1 )
            {
                return Resources.validate.GLDone;
            }
            transaction.Commit();
            return Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade + " ";


        }
        public static List<SalesReceiptDetails> getMaterialOCompoundProduct(List<SalesReceiptDetails> Details, int Store_Id)
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
                List<SalesReceiptDetails> Details2 = new List<SalesReceiptDetails>();

                foreach (var item in ProList)
                {
                    var SubProductList = ProductAll.Where(a => a.Prod_ID == item.Product_Id).FirstOrDefault().ProductCompounds;
                    foreach (var subProduct in SubProductList)
                    {
                        var dublicate = Details.Where(a => a.Product_Id == subProduct.Product_Id).FirstOrDefault();
                        var batch = db.ProductBatches.Where(a => a.Product_ID == subProduct.Product_Id && a.Store_ID == Store_Id).FirstOrDefault();
                        if (dublicate == null)
                        {
                            var Detailnew = new SalesReceiptDetails();
                            Detailnew.Product_Id = subProduct.Product_Id;
                            Detailnew.Product_Quantity = subProduct.Quantity * item.Product_Quantity;
                            Detailnew.Reverse_Quantity = subProduct.Quantity * item.Product_Quantity;
                            if (batch != null)
                            {
                                Detailnew.Batch_ID = batch.Batch_ID;
                            }
                            if (batch != null && batch.Batch_Cost > 0)
                            {
                                Detailnew.Detail_Total_Cost = (batch.Batch_Cost / batch.Product_Quantity ) * Detailnew.Product_Quantity;
                                Detailnew.Product_Cost = (batch.Batch_Cost / batch.Product_Quantity );
                            }
                            else
                            {
                                Detailnew.Detail_Total_Cost = 0;
                                Detailnew.Product_Cost = 0;

                            }
                            Details.Add(Detailnew);

                        }
                        else
                        {


                            var Detailnew = new SalesReceiptDetails();
                            Detailnew.Product_Id = subProduct.Product_Id;
                            Detailnew.Product_Quantity = dublicate.Product_Quantity + (item.Product_Quantity * subProduct.Quantity);
                            Detailnew.Reverse_Quantity = Detailnew.Product_Quantity;
                            if (batch != null && batch != null)
                            {
                                Detailnew.Batch_ID = batch.Batch_ID;
                            }

                            if (batch.Batch_Cost > 0)
                            {
                                Detailnew.Detail_Total_Cost = (batch.Batch_Cost / batch.Product_Quantity) * Detailnew.Product_Quantity;
                                Detailnew.Product_Cost = (batch.Batch_Cost / batch.Product_Quantity);
                            }
                            else
                            {
                                Detailnew.Detail_Total_Cost = 0;
                                Detailnew.Product_Cost = 0;

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
            var invoiceDetail = db.SalesReceiptMasters.Find(invoiceId).SalesReceiptDetails;
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
                if (gLMaster.GLState == TransactionState.Post|| gLMaster.GLState == TransactionState.Hold)
                {
                    gldetail.GLDetail_Credit = invoiceDetail.Where(a => a.Product_Tax == tax).Select(a => a.Detail_Total_Tax).Sum();
                    gldetail.GLDetail_Debit = 0;
                }
                else
                {
                    gldetail.GLDetail_Credit = 0;
                    gldetail.GLDetail_Debit = invoiceDetail.Where(a => a.Product_Tax == tax).Select(a => a.Detail_Total_Tax).Sum();
                }

                gLMaster.GLDetails.Add(gldetail);
            }

            return gLMaster;
        }
        /// <summary>
        /// prepare Editing of sales to change on it
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns>SalesReceiptMasterVM</returns>
        public static SalesReceiptMasterVM fillForEditingInvoice(int invoice,int iscopy)
        {
            var db = new AccountingContainer();
            var salesReceipt = db.SalesReceiptMasters.Find(invoice);
            var revesre = db.ReverseSalesReceiptMasters.Where(a => a.SalesRecipt_Id == invoice).FirstOrDefault();
            if (revesre != null && iscopy != 1)
            {
                return null;
            }
            if (salesReceipt.Invoice_State != TransactionState.Pending&&iscopy!=1)
            {
                return null;
            }
            var salesVM = new SalesReceiptMasterVM();
          
            salesVM.isCopy = iscopy;
            String StartCodeOfrevenue = "04";
          
            var comp = db.Companies.FirstOrDefault(); ;
         
                salesVM.Company_Id = comp.Comp_Id;
           
            var cur = db.Currencies.FirstOrDefault();
            
                salesVM.Invoice_Currency = cur.Currency_Id ;
            if (salesReceipt.Customer_Id != null && salesReceipt.Customer_Id.Value > 0)
            {
                salesVM.Customer_Id = salesReceipt.Customer_Id.Value;
            }
            
          
               
           
            var stor = db.Stores.Where(Cu => Cu.Store_ID == salesReceipt.Store_ID);
            if (stor.Any())
            {
                salesVM.Store_ID= stor.FirstOrDefault().Store_ID;
            }
            var account = db.ChartOfAccountings.Where(Cu => Cu.Account_Id == salesReceipt.Account_Id);
            if (account.Any())
            {
                salesVM.Account_Id = account.FirstOrDefault().Account_Id;
            }
         
            var payment = db.PaymentMethods.Where(Cu => Cu.PaymentMethod_Id == salesReceipt.PaymentMethod_Id);
            if (payment.Any())
            {
                salesVM.PaymentMethod_Id = payment.FirstOrDefault().PaymentMethod_Id;
            }

            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0&&a.isActive != false).ToList();
            salesVM.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            if (salesReceipt.Branch_Id > 0)
            {
                salesVM.MyDepartmentList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == salesReceipt.Branch_Id && p.isActive != false && p.Subled_Level == 2), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                salesVM.MyDepartmentList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
                salesVM.MyBuList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            }
            if (salesReceipt.Department_ID > 0)
            {
                salesVM.MyBuList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == salesReceipt.Department_ID && p.isActive != false), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                salesVM.MyBuList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.SubLed_Type == "نشاط لقسم" && a.isActive != false), "SubLed_Id", "SubLed_Name");

            }

            //salesVM.MyDepartmentList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            //salesVM.MyBuList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            salesVM.MySubList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");

           // salesVM.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            salesVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            salesVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");
            
            var newList = (from custo in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                           select new
                           {
                               Id = custo.Customer_Id,
                               Name = custo.Customer_Name + "        " + custo.Customer_Phone

                           }).ToList();
            salesVM.MyCustomerList = new SelectList(newList, "Id", "Name");
            salesVM.MyStoresList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            salesVM.AccountList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfrevenue)), " Account_Id", "Account_Name");
            salesVM.EmpList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            var newListpay = (from pay in db.PaymentMethods.Where(a=>a.IsActive!=false)
                           select new
                           {
                               Id = pay.PaymentMethod_Id,
                               Name = pay.Payment_Type

                           });

            salesVM.PaymentMethodList = new SelectList(newListpay, "Id", "Name");
         
            salesVM.Master_Id = salesReceipt.Master_Id;
            salesVM.Serial_Number = salesReceipt.Serial_Number;
            salesVM.Description = salesReceipt.Description;
            salesVM.Notes = salesReceipt.Notes;
            salesVM.Master_Discount = salesReceipt.Master_Discount;
            salesVM.Invoice_State = salesReceipt.Invoice_State;
     
            salesVM.Exchange_Rate = salesReceipt.Exchange_Rate;
            salesVM.Reference = salesReceipt.Reference;
            salesVM.ApprovalCode = salesReceipt.ApprovalCode;
            salesVM.Department_ID = salesReceipt.Department_ID;
            salesVM.BussinesUnit_ID = salesReceipt.BussinesUnit_ID;
            salesVM.Fixed_Discount = salesReceipt.FixedDiscount.Value;
            if (salesReceipt.Emp_Id != 0 && salesReceipt.Emp_Id != null)
            {
                salesVM.Emp_Id = salesReceipt.Emp_Id.Value;
            }
           
            if (salesReceipt.Branch_Id!=null&& salesReceipt.Branch_Id != 0)
            {
                salesVM.Branch_Id = salesReceipt.Branch_Id.Value;
            }
            var salesTemp = new SalesReceiptMasterTemp();
         
            salesTemp.Serial_Number = salesReceipt.Serial_Number;
            salesTemp.Description = salesReceipt.Description;
            salesTemp.Invoice_Date = salesReceipt.Invoice_Date;
            salesTemp.Master_Discount = salesReceipt.Master_Discount;
            var CurrencyResult = db.Currencies.Find(salesVM.Invoice_Currency);
            if (CurrencyResult != null)
            {
                salesTemp.Invoice_Currency = CurrencyResult.Currency_Type;
            }
            else
            {
                salesTemp.Invoice_Currency = "الريال السعودى";
            }
     
            salesTemp.Total_Amount = salesReceipt.Total_Amount;
            salesTemp.Total_Tax = salesReceipt.Total_Tax;
            salesTemp.Total_Cost = salesReceipt.Total_Cost;
            salesTemp.Invoice_State= salesReceipt.Invoice_State;
            salesTemp.Exchange_Rate = salesReceipt.Exchange_Rate;
            salesTemp.Customer_Id = salesReceipt.Customer_Id;
            salesTemp.PaymentMethod_Id = salesReceipt.PaymentMethod_Id;
            salesTemp.Account_Id = salesReceipt.Account_Id;
            salesTemp.Branch_Id = salesReceipt.Branch_Id;
            salesTemp.Reference = salesReceipt.Reference;
            salesTemp.ApprovalCode= salesReceipt.ApprovalCode;
            salesTemp.Store_ID= salesReceipt.Store_ID;
            salesTemp.Company_Id = salesReceipt.Company_Id;
            var thistaxinchart = db.ChartOfAccountings.Find(48);
            var anewList = (from Tax in db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false)
                            select new
                            {

                                Id = Tax.Tax_ID,
                                Name = Tax.Tax_Name + "   " + Tax.Tax_Percentage + "%"

                            }).ToList();
            salesVM.TaxList = new SelectList(anewList, "Id", "Name");
            salesVM.Taxes = salesReceipt.SalesReceiptTaxes.Select(a => a.Tax_Id).ToList();
            var ThereORN = db.SalesReceiptMasterTemps.Find(salesReceipt.Master_Id);
            if (ThereORN != null)
            {

            }
            else
            {
                salesTemp.Master_Id = salesReceipt.Master_Id;

                db.SalesReceiptMasterTemps.Add(salesTemp);
            }
            salesTemp.TotalNewTaxs = salesReceipt.TotalNewTaxs;
            db.SaveChanges();
            if (salesVM.isCopy == 1)
            {
                var listOfSale = db.SalesReceiptMasters.ToList();
                if (listOfSale.FirstOrDefault() == null)
                {
                    salesVM.Serial_Number = "SALRC-001";
                }
                else
                {
                    var lastSerial = db.SalesReceiptMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                    //using string substring method to get the number of the last inserted
                    salesVM.Serial_Number = "SALRC-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(6, lastSerial.Serial_Number.Length - 6)) + 1).ToString("D3");
                }
            }
            if (iscopy == 1)
            {
                salesVM.Invoice_Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
            }
            else
            {
                salesVM.Invoice_Date = salesReceipt.Invoice_Date.ToString("dd-MM-yyyy   HH:mm:ss");
            }
            return salesVM;

        }
        /// <summary>
        /// get list of detail of sales Recipt by master id
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns></returns>
        public static List<SalesReceiptDetails> getAlldetailInInvoice(int masterId)
        {
            var db = new AccountingContainer();
            var detail = db.SalesReceiptDetails.Where(a => a.Master_Id == masterId).ToList();

            return detail;

        }
        /// <summary>
        /// get salesRecipt by its id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static SalesReceiptMaster GetpurchInvByID(int ID)
        {
            var db = new AccountingContainer();
            var invoiceMaster = db.SalesReceiptMasters.Where(c => c.Master_Id == ID);
            var invoice = new SalesReceiptMaster();

            if (invoiceMaster != null)
            {
                invoice = invoiceMaster.FirstOrDefault();
                return invoice;
            }
            return null;
        }
        /// <summary>
        /// prepare detail for editing in to order add to list
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns></returns>
        public static SalesReceiptDetailsVM PreparpurchMasterDetail2(int masterId)
        {

            var db = new AccountingContainer();
            var invoiceMasterDVM = new SalesReceiptDetailsVM();

            var thisdetails = GetAllTempPURCHInvoiceDetailsDetails(masterId).ToList().FirstOrDefault();

            var product = thisdetails.Product_Id;


            invoiceMasterDVM.Product_Quantity = thisdetails.Product_Quantity;
            //   invoiceMasterDVM.SelectedBtachtId = thisdetails.Product_ID;
            invoiceMasterDVM.Details_Id = thisdetails.Details_Id;
            invoiceMasterDVM.Product_Price = thisdetails.Product_Price;
            invoiceMasterDVM.Product_Tax = thisdetails.Product_Tax;
            invoiceMasterDVM.Detail_Total_Tax = thisdetails.Detail_Total_Tax;
            invoiceMasterDVM.Master_Id = masterId;
            var batchs = db.ProductBatches.Where(batch => batch.Product_ID == thisdetails.Product_Id).ToList();
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
            
            invoiceMasterDVM.BatchList = new SelectList(_ProductBatchSelection, "Batch_Id", "ProductName");
            List<object> newList = new List<object>();
            var thistaxinchart = db.ChartOfAccountings.Find(48);
            var members = db.Taxes.Where(e=>e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false).ToList();
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.Tax_ID,
                    Name = member.Tax_Name + "   " + member.Tax_Percentage + "%"
                });
            invoiceMasterDVM.ProducttaxList = new SelectList(newList, "Id", "Name");
            invoiceMasterDVM.Batch_ID = db.ProductBatches.Where(a => a.Product_ID == thisdetails.Product_Id).FirstOrDefault().Batch_ID;

            return invoiceMasterDVM;


        }
        /// <summary>
        /// get All Detail by its master id 
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns></returns>
        public static List<SalesReceiptDetails> GetAllTempPURCHInvoiceDetailsDetails(int masterVmId)
        {
            var db = new AccountingContainer();
            var DetailsTemp = db.SalesReceiptDetails.Where(invoice => invoice.Details_Id == masterVmId).ToList();
            return DetailsTemp;
        }
        /// <summary>
        /// get detail by its master id 
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns></returns>
        public static List<SalesReceiptDetails> GetAllTempPURCHInvoiceDetails2(int masterVmId)
        {
            var db = new AccountingContainer();
            var Details = db.SalesReceiptDetails.Where(invoice => invoice.Master_Id == masterVmId).ToList();
            return Details;
        }
        /// <summary>
        /// get all detail temp by salesRecipt master id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns></returns>
        public static List<SalesReceiptDetailsTemp> GetAllTempPurchInvoiceDetailsresult2(int masterVmId)
        {
            var db = new AccountingContainer();
            var InvoiceSalesTemp = db.SalesReceiptMasterTemps.Find(masterVmId);
            return InvoiceSalesTemp.SalesReceiptDetailsTemps.ToList();
        }
        /// <summary>
        /// add to list in salesRecipt
        /// </summary>
        /// <param name="Detail"></param>
        /// <returns></returns>
        public static string AddSalesDetailsTemp(SalesReceiptDetailsVM Detail)
        {
            var db = new AccountingContainer();
            var invoiceMasterTemp = db.SalesReceiptMasterTemps.Find(Detail.Master_Id);
            var batch = db.ProductBatches.Find(Detail.Batch_ID);
            var invoiceMasterDetailsTemp = new SalesReceiptDetailsTemp();
            invoiceMasterDetailsTemp.Batch_ID = batch.Batch_ID;
            invoiceMasterDetailsTemp.Product_Id = Detail.Batch_ID.Value;
            invoiceMasterDetailsTemp.Product_Price = Detail.Product_Price;
            invoiceMasterDetailsTemp.Product_Quantity = Detail.Product_Quantity;
            var dtails = invoiceMasterTemp.SalesReceiptDetailsTemps;
            var dublicateinTemp = dtails.Where(z => z.SalesReceiptMasterTempMaster_Id == invoiceMasterTemp.Master_Id && z.Product_Id == invoiceMasterDetailsTemp.Product_Id).ToList();
            if (dublicateinTemp.FirstOrDefault() != null)
            {
                return "هذا المنتج موجود بالفعل فى القائمة لا يمكن اضافته مرة اخرى";
            }
            var resultTax = db.Taxes.Find(Detail.Product_Tax);
            if (resultTax != null)
            {

                invoiceMasterDetailsTemp.Product_Tax = resultTax.Tax_ID;

            }
            var a = db.Taxes.Find(invoiceMasterDetailsTemp.Product_Tax);
            decimal TotalBeforeDiscount = Detail.Product_Price * Detail.Product_Quantity;
            decimal discount = TotalBeforeDiscount * (Detail.Discount / 100);
            decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
            invoiceMasterDetailsTemp.Detail_Total_Tax = TotalAfterDiscount * (a.Tax_Percentage / 100);
            invoiceMasterDetailsTemp.Total_Amount = (TotalBeforeDiscount + invoiceMasterDetailsTemp.Detail_Total_Tax) - discount;
            if (Detail.commition != 0 && Detail != null)
            {
                var com = db.PaymentMethods.Find(Detail.commition);
                if (com.Percentage == 0 && com.Operatin_Cost == 0)
                {
                    invoiceMasterDetailsTemp.comition = 0;

                }
                else
                {
                    invoiceMasterDetailsTemp.comition = (com.Percentage / 100) * invoiceMasterDetailsTemp.Total_Amount;
                    //  invoiceMasterDetailsTemp.Total_Amount = invoiceMasterDetailsTemp.Total_Amount + invoiceMasterDetailsTemp.comition.Value;
                }

            }
                invoiceMasterTemp.SalesReceiptDetailsTemps.Add(invoiceMasterDetailsTemp);

                db.Entry(invoiceMasterTemp).State = EntityState.Modified;
                db.SaveChanges();
                return "Details Has been added";
            }
        /// <summary>
        /// saving change in sales Recipt
        /// </summary>
        /// <param name="masterVM"></param>
        /// <returns>string</returns>


        public static string editinvoice(SalesReceiptMasterVM masterVM)
        {
            var db = new AccountingContainer();
            var invoice = db.SalesReceiptMasters.Find(masterVM.Master_Id);
            if (invoice.Invoice_State != TransactionState.Pending)
            {
                return Resources.validate.Error;
            }
            //1
            var qrMaster = new MasterQRInvoice();
            if (invoice.QRNumber != null)
            {
                qrMaster = db.MasterQRInvoices.Find(invoice.QRNumber.Value);
            }
            var mastertemp = db.SalesReceiptMasterTemps.Find(masterVM.Master_Id);
           
            invoice.Master_Id = masterVM.Master_Id;
            invoice.Serial_Number = masterVM.Serial_Number;
            invoice.Notes = masterVM.Notes;
            invoice.Description = masterVM.Description;
            invoice.Invoice_Date = Convert.ToDateTime(masterVM.Invoice_Date);
            invoice.Master_Discount = masterVM.Master_Discount;
           
            var currResult = db.Currencies.Where(a=>a.Currency_Id== masterVM.Invoice_Currency).FirstOrDefault();
            var oldDetail = db.SalesReceiptDetails.Where(a => a.Master_Id == invoice.Master_Id).ToList();
            var oldTaxes = db.SalesReceiptTaxes.Where(a => a.SalesReceiptMasterMaster_Id == invoice.Master_Id).ToList();

            //2
            List<DetailQRInvoice> oldQrDetail = new List<DetailQRInvoice>();
            List<TaxQRInvoice> oldQrTaxes = new List<TaxQRInvoice>();
            List<QRPaymentMethod> oldQrPaymentMethod = new List<QRPaymentMethod>();
            if (invoice.QRNumber != null)
            {
                oldQrDetail = db.DetailQRInvoices.Where(a => a.MasterQRInvoiceId == invoice.QRNumber).ToList();
                oldQrTaxes = db.TaxQRInvoices.Where(a => a.MasterQRInvoiceId == invoice.QRNumber).ToList();
                oldQrPaymentMethod = db.QRPaymentMethods.Where(a => a.MasterQRInvoiceId == invoice.QRNumber).ToList();
            }
            if (currResult != null)
            {
                invoice.Invoice_Currency = currResult.Currency_Type;

            }
            if (masterVM.SubLedger_ID != null)
            {

                invoice.SubLedger_ID = masterVM.SubLedger_ID;

            }
            else if (masterVM.BussinesUnit_ID != null)
            {

                invoice.SubLedger_ID = masterVM.BussinesUnit_ID;

            }
            else if (masterVM.Department_ID!= null)
            {

                invoice.SubLedger_ID = masterVM.Department_ID;

            }
            else if (masterVM.Branch_Id != null)
            {

                invoice.SubLedger_ID = masterVM.Branch_Id;

            }
            else { invoice.SubLedger_ID = 1; }

            var employee = db.Employees.Find(masterVM.Emp_Id);
            if (employee != null)
            {
                invoice.Emp_Id = employee.Emp_Id;
            }
            invoice.Invoice_State = TransactionState.Pending ;
            invoice.Exchange_Rate = masterVM.Exchange_Rate;
            invoice.Customer_Id= masterVM.Customer_Id;
            invoice.PaymentMethod_Id = masterVM.PaymentMethod_Id;
            invoice.Account_Id = masterVM.Account_Id;
            invoice.Branch_Id = masterVM.Branch_Id;
            invoice.Reference = masterVM.Reference;
            invoice.ApprovalCode = masterVM.ApprovalCode;
            if (masterVM.Store_ID == 0 || masterVM == null)
            {

                masterVM.Store_ID = db.Stores.FirstOrDefault().Store_ID;

            }
            invoice.Store_ID = masterVM.Store_ID;
            invoice.FixedDiscount = masterVM.Fixed_Discount;
            invoice.Department_ID = masterVM.Department_ID;
            invoice.BussinesUnit_ID=masterVM.BussinesUnit_ID;
            invoice.Company_Id = masterVM.Company_Id;
            invoice.Emp_Id = masterVM.Emp_Id;
            invoice.Total_Amount = 0;
            invoice.Total_Tax = 0;
            invoice.Total_Cost = 0;
            invoice.TotaL_After_Fixed = 0;

            decimal Sumtax = 0;
            decimal SumtAmount = 0;
            decimal SumTotalCost = 0;
            var detailList = db.SalesReceiptDetailsTemps.Where(e => e.SalesReceiptMasterTempMaster_Id == mastertemp.Master_Id).ToList();
            var ListDetailTemp = new List<SalesReceiptDetailsTemp>();
            invoice.SalesReceiptDetails = new List<SalesReceiptDetails>();

            qrMaster.DetailQRInvoices = new List<DetailQRInvoice>();
            qrMaster.QRPaymentMethods = new List<QRPaymentMethod>();
            qrMaster.TaxQRInvoices = new List<TaxQRInvoice>();

            foreach (var item in detailList)
            {
                var details = new SalesReceiptDetails();
                details.Details_Id = item.Details_Id;
                details.Product_Quantity = item.Product_Quantity;
                details.Product_Price = item.Product_Price;
                details.Product_Tax = item.Product_Tax;
                details.Detail_Discount = item.Detail_Discount;
                details.Detail_Discount_Amount = item.Detail_Discount_Amount;
                decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                decimal discount = TotalBeforeDiscount * (item.Detail_Discount.Value / 100);
                decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                details.Batch_ID = item.Batch_ID;
                if (item.Product_Tax==0)
                {
                    details.Detail_Total_Tax = 0;
                }
                else
                {
                    var tax = Convert.ToDecimal(db.Taxes.Find(details.Product_Tax).Tax_Percentage);
                    details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                }
                
                // if the product in batch
                if (item.Batch_ID > 0)
                {
                    ProductBatches detailBatch = db.ProductBatches.Find(details.Batch_ID);
                    details.Product_Id = detailBatch.Product_ID;
                    // Checks for the products quantity if available or not
                    //if (item.Product_Quantity > detailBatch.Product_Quantity)
                    //    return "Not enough quantity of " + detailBatch.Product.Prod_Name;
                    // Calculate the Total_Cost

                    details.Detail_Total_Cost = item.Product_Quantity * detailBatch.Product_Cost;
                   


                    var price = item.Product_Price * item.Product_Quantity;
                    details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                    invoice.Total_Amount += details.Total_Amount;
                    invoice.Total_Cost += details.Detail_Total_Cost;
                    invoice.Total_Tax += details.Detail_Total_Tax;
                    details.Reverse_Total_Tax = details.Detail_Total_Tax;
                    details.Reverse_Quantity = details.Product_Quantity;

                    details.Reverse_Total_Amount = details.Total_Amount;
                    invoice.SalesReceiptDetails.Add(details);
                    
                }
                else
                {
                    Product productss = db.Products.Find(item.Product_Id);
                    details.Product_Id = productss.Prod_ID;

                    // Calculate the Total_Cost

                    if (productss.Prod_Type == Prod_Type.خدمة)
                    {
                        details.Detail_Total_Cost = 0;
                    }
                    else
                    {


                        details.Detail_Total_Cost = item.Product_Quantity * productss.Prod_Cost_Price.Value;
                    }



                    var price = item.Product_Price * item.Product_Quantity;
                    details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                    invoice.Total_Amount += details.Total_Amount;
                    invoice.Total_Cost += details.Detail_Total_Cost;
                    invoice.Total_Tax += details.Detail_Total_Tax;
                    details.Reverse_Total_Tax = details.Detail_Total_Tax;
                    details.Reverse_Quantity = details.Product_Quantity;

                    details.Reverse_Total_Amount = details.Total_Amount;
                    invoice.SalesReceiptDetails.Add(details);
                
                }
                ListDetailTemp.Add(item);

                var invoiceQRDetail = new DetailQRInvoice();
                invoiceQRDetail.Batch_ID = details.Batch_ID;
                invoiceQRDetail.Product_Id = details.Product_Id;
                invoiceQRDetail.Product_Quantity = details.Product_Quantity;
                invoiceQRDetail.Product_Tax = details.Product_Tax;
                invoiceQRDetail.Detail_Discount = details.Detail_Discount;
                invoiceQRDetail.Detail_Discount_Amount = details.Detail_Discount_Amount;

                invoiceQRDetail.Detail_Total_Cost = details.Detail_Total_Cost;

                invoiceQRDetail.Total_Amount = details.Total_Amount;

                invoiceQRDetail.Product_Price = details.Product_Price;
                invoiceQRDetail.Detail_Total_Tax = details.Detail_Total_Tax;
                invoiceQRDetail.ProductPriceCost = details.Product_Cost.Value;
                qrMaster.DetailQRInvoices.Add(invoiceQRDetail);
            }


            if (masterVM.Files.FirstOrDefault() != null)
            {
                foreach (var file in masterVM.Files)
                {
                    var filedetails = new SalesReceiptFile();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.FileContent = data;
                    filedetails.FileType = file.ContentType;
                    if (data.Length < 1000000)
                    {
                        invoice.SalesReceiptFiles.Add(filedetails);
                    }
                    
                }
            }
            invoice.TotalNewTaxs = 0;

            if (masterVM.Taxes != null)
            {
                foreach (var tax in masterVM.Taxes)
                {
                    var taxo = new SalesReceiptTax();
                    taxo.Tax_Id = tax;

                    taxo.Amount = (db.Taxes.Find(Convert.ToInt32(tax)).Tax_Percentage / 100) * invoice.SalesReceiptDetails.Select(a => a.Total_Amount).Sum();
                    invoice.TotalNewTaxs += taxo.Amount;

                    invoice.SalesReceiptTaxes.Add(taxo);


                    var qrtax = new TaxQRInvoice();
                    qrtax.Tax_Id = tax;

                    qrtax.Amount = taxo.Amount;
                    qrMaster.TaxQRInvoices.Add(qrtax);

                }
            }
            invoice.TotaL_After_Fixed = (invoice.Total_Amount + invoice.TotalNewTaxs ) - invoice.FixedDiscount;
            var payment = db.PaymentMethods.Find(invoice.PaymentMethod_Id);
            invoice.Total_Payment_Method_Cost = (invoice.TotaL_After_Fixed.Value * (payment.Percentage.Value) / 100);
           
            if (payment.Operatin_Cost > 0)
            {
                invoice.Total_Payment_Method_Cost = invoice.Total_Payment_Method_Cost + payment.Operatin_Cost.Value;
               
            }
            var paymentQR = new QRPaymentMethod();
            paymentQR.PaymentMethod_Id = invoice.PaymentMethod_Id;
            paymentQR.Commution = invoice.Total_Payment_Method_Cost;
            paymentQR.Amount = invoice.TotaL_After_Fixed.Value;
            qrMaster.QRPaymentMethods.Add(paymentQR);

            var RealDisc = (invoice.SalesReceiptDetails.Select(a => a.Detail_Discount_Amount.Value).Sum() / invoice.SalesReceiptDetails.Select(h => h.Product_Price * h.Product_Quantity).Sum()) * 100;
            if (Math.Round(RealDisc, 4, MidpointRounding.AwayFromZero) != Math.Round(invoice.Master_Discount, 4, MidpointRounding.AwayFromZero))
            {
                invoice.Master_Discount = RealDisc;
            }
            //add QR Invoice
            qrMaster.AllowDiscount = invoice.FixedDiscount.Value;
            qrMaster.ApprovalCode = invoice.Reference;
            qrMaster.Company_Id = invoice.Company_Id;
            qrMaster.Customer_Id = invoice.Customer_Id;
            qrMaster.Description = invoice.Description;
            qrMaster.Employee_Id = invoice.Emp_Id.Value;
            qrMaster.Invoice_Date = invoice.Invoice_Date;
            qrMaster.Invoice_State = invoice.Invoice_State;
            qrMaster.InvoiceType_Id = 2;
            qrMaster.Master_Discount = invoice.Master_Discount;

            qrMaster.Reference = invoice.Reference;
            qrMaster.Serial_Number = invoice.Serial_Number;
            qrMaster.Store_ID = invoice.Store_ID;
            qrMaster.Subledger_Id = invoice.SubLedger_ID;
            qrMaster.Notes = invoice.Notes;
            qrMaster.TotalNewTaxs = invoice.TotalNewTaxs.Value;
            qrMaster.Account_Id = invoice.Account_Id;
            qrMaster.Total_Amount = invoice.Total_Amount;

            qrMaster.Total_Payment_Method_Cost = 0;
            qrMaster.Total_Tax = invoice.Total_Tax;
            qrMaster.Branch_Id = invoice.Branch_Id;
            qrMaster.Department_ID = invoice.Department_ID;
            qrMaster.BussinesUnit_ID = invoice.BussinesUnit_ID;


            qrMaster.Total_Cost = invoice.Total_Cost;
            qrMaster.Notes = invoice.Notes;

            if (invoice.QRNumber != null)
            {

                db.Entry(qrMaster).State = EntityState.Modified;
            }



            db.SalesReceiptDetailsTemps.RemoveRange(ListDetailTemp);
            
            db.SalesReceiptMasterTemps.Remove(mastertemp);
            db.SalesReceiptDetails.RemoveRange(oldDetail);
            if (oldTaxes.Count() != 0 )
            {
                db.SalesReceiptTaxes.RemoveRange(oldTaxes);
            }
            if (invoice.QRNumber != null)
            {

                db.TaxQRInvoices.RemoveRange(oldQrTaxes);
                db.DetailQRInvoices.RemoveRange(oldQrDetail);
                db.QRPaymentMethods.RemoveRange(oldQrPaymentMethod);
            }

            db.SaveChanges();

            return "success";
        }

        /// <summary>
        /// get DrobdownList of Customer for SalesRecipt
        /// </summary>
        /// <returns>SalesReceiptMasterVM</returns>
        public static IEnumerable<SelectListItem> GetCustomerList()
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> customers = new SelectList(db.Customers.Where(a=>a.Customer_State==Customer_State.نشط), "Customer_Id", "Customer_Name");
            return customers;
        
        }

        /// <summary>
        /// get the store by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Store</returns>
        public static Store GetBranchFroStoreInfo(int id)
        {
            var db = new AccountingContainer();
            var thistype = db.Stores.Find(id);
            return thistype;
        }
        /// <summary>
        /// delete Temp of sales detail temp(Remove from List by id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static decimal  delete_Temp_Record(int id)
        {
            var db = new AccountingContainer();
            var delete = db.SalesReceiptDetailsTemps.Find(id);
        
            db.SalesReceiptDetailsTemps.Remove(delete);
            db.SaveChanges();
            decimal discount = 0;
            var salestemp = db.SalesReceiptMasterTemps.Find(delete.SalesReceiptMasterTempMaster_Id);
            try
            {
                 discount = salestemp.SalesReceiptDetailsTemps.Select(a => a.Detail_Discount_Amount.Value).Sum() / salestemp.SalesReceiptDetailsTemps.Select(a => a.Product_Quantity * a.Product_Price).Sum();
            }
            catch (Exception ex)
            {
                return 0;
            }
            return discount*100;




        }

        /// <summary>
        /// delete salesRecipt in pending by master id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public static SalesReceiptMaster DeleteInvoiceMaster(int invoiceId)
        {
            var db = new AccountingContainer();
            SalesReceiptMaster invoice = db.SalesReceiptMasters.Find(invoiceId);

            if (invoice.Invoice_State != TransactionState.Pending)
                return null;
            db.SalesReceiptDetails.RemoveRange(invoice.SalesReceiptDetails);
            db.SalesReceiptFiles.RemoveRange(invoice.SalesReceiptFiles);
            db.SalesReceiptMasters.Remove(invoice);
            var DeletQR = db.MasterQRInvoices.Where(a => a.Serial_Number == invoice.Serial_Number).FirstOrDefault();
            db.DetailQRInvoices.RemoveRange(DeletQR.DetailQRInvoices);
            db.QRPaymentMethods.RemoveRange(DeletQR.QRPaymentMethods);
            db.TaxQRInvoices.RemoveRange(DeletQR.TaxQRInvoices);
            db.MasterQRInvoices.Remove(DeletQR);
            db.SaveChanges();
            db.Dispose();
            return invoice;
        }
        /// <summary>
        /// prepare detail of temp for Editin
        /// </summary>
        /// <param name="MasterId"></param>
        /// <returns>List<SalesReceiptDetailsTemp></returns>

        public static SalesReceiptDetailsTempVM GetAllDetailForEditing(int MasterId)
        {
            var db = new AccountingContainer();
            var invoicedetalTemp = new SalesReceiptDetailsTempVM();
            

            invoicedetalTemp.invoice_Id = MasterId;



            var detailTemp = db.SalesReceiptMasterTemps.Find(MasterId);
         
            db.SalesReceiptDetailsTemps.RemoveRange(detailTemp.SalesReceiptDetailsTemps);
            db.SaveChanges();
            var invoice = db.SalesReceiptMasters.Find(MasterId);
            invoicedetalTemp.AllowDiscount = invoice.FixedDiscount.Value;
            if (invoice.TotalNewTaxs == null)
            {
                invoice.TotalNewTaxs = 0;
            }
            invoicedetalTemp.TotalNewTaxs = invoice.TotalNewTaxs.Value;
            List<SalesReceiptDetailsTemp> Lista = new List<SalesReceiptDetailsTemp> { };
            foreach (var item in invoice.SalesReceiptDetails)
            {
                var invoiceTemp = new SalesReceiptDetailsTemp();
                invoiceTemp.Batch_ID = item.Batch_ID.Value;
                invoiceTemp.Detail_Total_Tax = item.Detail_Total_Tax;                
                invoiceTemp.Product_Id = item.Product_Id;             
                invoiceTemp.Product_Price = item.Product_Price;
                invoiceTemp.Product_Quantity = item.Product_Quantity;
                invoiceTemp.Detail_Total_Cost = item.Detail_Total_Cost;
                invoiceTemp.Product_Tax = item.Product_Tax;
                invoiceTemp.Total_Amount = item.Total_Amount;
                if (item.Detail_Discount==null)
                {
                    item.Detail_Discount = 0;
                    item.Detail_Discount_Amount = 0;
                }
                invoiceTemp.Detail_Discount = item.Detail_Discount;
                invoiceTemp.Detail_Discount_Amount = item.Detail_Discount_Amount;
                invoiceTemp.SalesReceiptMasterTempMaster_Id = MasterId;
                Lista.Add(invoiceTemp);

            }
            db.SalesReceiptDetailsTemps.AddRange(Lista);

            db.SaveChanges();
            invoicedetalTemp.invoiceTemp = detailTemp.SalesReceiptDetailsTemps.ToList();
            return invoicedetalTemp;
        }
        /// <summary>
        /// change detail temp by descounf
        /// </summary>
        /// <param name="discount"></param>
        /// <param name="Invoice_Id"></param>
        /// <returns></returns>
        public static string changeDetailOfInvoice(decimal discount, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.SalesReceiptMasterTemps.Find(Invoice_Id);
            var detail = db.SalesReceiptDetailsTemps.Where(a => a.SalesReceiptMasterTempMaster_Id == Invoice_Id).ToList();
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
        public static decimal updateDiscountDebendOnDiscountAmount(decimal discount, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.SalesReceiptMasterTemps.Find(Invoice_Id);
            var pecentage = (discount / invoiceTemp.SalesReceiptDetailsTemps.Sum(a => a.Product_Price * a.Product_Quantity)) * 100;
            var detail = db.SalesReceiptDetailsTemps.Where(a => a.SalesReceiptMasterTempMaster_Id == Invoice_Id).ToList();
            decimal total = 0;
            foreach (var item in detail)
            {
                item.TotalBeforeDisc = item.Product_Quantity * item.Product_Price;
                item.Detail_Discount = pecentage;

                item.TotalDiscount = item.TotalBeforeDisc * (pecentage / 100);
                item.Detail_Discount_Amount = item.TotalDiscount;
                decimal? beforeTax = 0;
                if (item.Product_Tax == 0)
                {
                    item.Detail_Total_Tax = 0;
                }
                else
                {
                    var tax = db.Taxes.Find(item.Product_Tax).Tax_Percentage;
                    beforeTax = item.TotalBeforeDisc - item.TotalDiscount;
                    item.Detail_Total_Tax = beforeTax.Value * (tax / 100);
                }


                item.Total_Amount = (item.TotalBeforeDisc.Value - item.TotalDiscount.Value) + item.Detail_Total_Tax;
                db.Entry(item).State = EntityState.Modified;
                total += item.Total_Amount;
            }
            invoiceTemp.Total_Amount = total;
            db.Entry(invoiceTemp).State = EntityState.Modified;
            db.SaveChanges();
            return pecentage;

        }
        public static decimal updateDiscountDebendOnNet(decimal net, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.SalesReceiptMasterTemps.Find(Invoice_Id);
            
           
            if (invoiceTemp.SalesReceiptDetailsTemps.Count() == 0 || invoiceTemp.SalesReceiptDetailsTemps.Select(a => a.Product_Tax).Distinct().Count() > 1)
            {
                try{
                    return invoiceTemp.Master_Discount.Value;
                }
                catch
                {
                    return 0;
                }
            }
            var detail = invoiceTemp.SalesReceiptDetailsTemps.ToList();
            var taxId = invoiceTemp.SalesReceiptDetailsTemps.FirstOrDefault().Product_Tax;
            decimal invoiceTax = 0;
            if (taxId > 0)
            {
                invoiceTax = db.Taxes.Find(taxId).Tax_Percentage/100;
            }
            decimal totalPrice = invoiceTemp.SalesReceiptDetailsTemps.Select(a => a.Product_Quantity * a.Product_Price).Sum();
            decimal discountTotal =( totalPrice+ (invoiceTax* totalPrice)-net)/(1+ invoiceTax);
            decimal percentage = (discountTotal / totalPrice) * 100;
            decimal total = 0;
            foreach (var item in detail)
            {
                item.TotalBeforeDisc = item.Product_Quantity * item.Product_Price;
               
                item.TotalDiscount = item.TotalBeforeDisc * (percentage / 100);
                item.Detail_Discount = percentage;
                item.Detail_Discount_Amount = item.TotalDiscount;
                decimal? beforeTax = 0;
                if (item.Product_Tax == 0)
                {
                    item.Detail_Total_Tax = 0;
                }
                else
                {
                    var tax = db.Taxes.Find(item.Product_Tax).Tax_Percentage;
                    beforeTax = item.TotalBeforeDisc - item.TotalDiscount;
                    item.Detail_Total_Tax = beforeTax.Value * (tax / 100);
                }


                item.Total_Amount = (item.TotalBeforeDisc.Value - item.TotalDiscount.Value) + item.Detail_Total_Tax;
                db.Entry(item).State = EntityState.Modified;
                total += item.Total_Amount;
            }
            invoiceTemp.Total_Amount = total;
            invoiceTemp.Master_Discount = percentage;
            db.Entry(invoiceTemp).State = EntityState.Modified;
            db.SaveChanges();
            return percentage;

        }
        /// <summary>
        /// change detail value of sales invoice by alldiscount
        /// </summary>
        /// <param name="AllowDiscount"></param>
        /// <param name="Invoice_Id"></param>
        /// <returns></returns>

        public static string changeDetailOfInvoiceForAllDiscount(decimal AllowDiscount, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.SalesReceiptMasterTemps.Find(Invoice_Id);
            invoiceTemp.AllowDiscount = AllowDiscount;
            db.Entry(invoiceTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "تم عمل الخصم المسموح به";

        }

        public static String Import(HttpPostedFileBase excelfile)
        {

            var data = getdataFillter(excelfile);
            List<SalesReceiptMaster> listCustomers = new List<SalesReceiptMaster>();
            if (data.Item2.Count() == 0 &&data.Item1.Count()>0)
            {
             
                foreach (var item in data.Item1)
                {
                   
                    var cust =AddInvoiceByExcelSheet(item);
                    listCustomers.Add(cust);

                }
                

                

                return "تم حفظ الملف بنجاح";

            }
            string combindedString = string.Join(" ,", data.Item2.ToArray());
            return "يوجد اخطاء فى الاسطر التالية" + combindedString;

        }
        /// <summary>
        /// Check File Validation in Excel File
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        public static Tuple<List<SalesReceiptMasterVM>, List<String>> getdataFillter(HttpPostedFileBase excelfile)
        {
            var vmList = new List<SalesReceiptMasterVM> { };
            var messageList = new List<String> { };
            var db = new AccountingContainer();
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return Tuple.Create(vmList, messageList);

            }

            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    // string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + excelfile.FileName);
                    var nameOfFile = "Sals" + DateTime.Now.ToBinary() + ".xlsx";
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + nameOfFile);

                    //if (System.IO.File.Exists(path))
                    //{


                    //    File.Delete(path);




                    //}

                    excelfile.SaveAs(path);

                    Application application = new Application();
                    Workbook workbook = application.Workbooks.Open(path);
                    Worksheet worksheet = workbook.ActiveSheet;
                    Range range = worksheet.UsedRange;
                    decimal tax = db.Taxes.FirstOrDefault().Tax_Percentage / 100;
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        try
                        {
                            SalesReceiptMasterVM vm = new SalesReceiptMasterVM();
                            // cust.Customer_Id = int.Parse(((Range)range.Cells[row, 1]).Text);
                            vm.Reference = ((Range)range.Cells[row, 1]).Text;
                            if (vm.Reference == "")
                            {
                                messageList.Add(row.ToString());
                                return Tuple.Create(vmList, messageList);
                            }
                            vm.Invoice_Date = ((Range)range.Cells[row, 2]).Text;
                            if (vm.Invoice_Date == null)
                            {
                                messageList.Add(row.ToString());
                                return Tuple.Create(vmList, messageList);
                            }
                            string Phone = ((Range)range.Cells[row, 4]).Text;
                            var cust = db.Customers.Where(a => a.Customer_Phone == Phone).FirstOrDefault();
                            if (cust == null)
                            {
                                var cusvm = new CustomerVM();
                                cusvm.Customer_Name = ((Range)range.Cells[row, 3]).Text;
                                cusvm.Customer_Phone = Phone;
                                cusvm.CustomerSelectSlice_Id = 1;
                                cusvm.Province = ((Range)range.Cells[row, 5]).Text;
                                //cusvm.Customer_Address = ((Range)range.Cells[row, 30]).Text;
                                cust = Customer.AddNewCustomerFromFile(cusvm);


                            }
                            else
                            {
                                vm.Customer_Id = cust.Customer_Id;
                            }
                            vm.Customer_Id = cust.Customer_Id;
                            string barcode = ((Range)range.Cells[row, 6]).Text;
                            if (barcode == null)
                            {
                                messageList.Add(row.ToString());
                                return Tuple.Create(vmList, messageList);
                            }
                            if (((Range)range.Cells[row, 7]).Text==null)
                            {
                                messageList.Add(row.ToString());
                                return Tuple.Create(vmList, messageList);
                            }
                            var prod = db.Products.Where(a => a.Bar_Code == barcode).FirstOrDefault();
                            int prodid = 0;
                            var quantity= Convert.ToDecimal(((Range)range.Cells[row, 8]).Text);
                            if (prod == null)
                            {
                                var prodvm = new ProductVM();
                                prodvm.Prod_Name = ((Range)range.Cells[row, 7]).Text;
                                prodvm.Bar_Code = barcode;
                                prodvm.Prod_Type = Prod_Type.خدمة;
                                if (quantity > 0)
                                {
                                    prodvm.Sale_Price = (Convert.ToDecimal(((Range)range.Cells[row, 9]).Text) / quantity) / (1 + tax);
                                }
                                prodvm.Prod_cost_Price = 1;
                                prodid = Product.CreateProdect(prodvm);


                            }
                            else
                            {
                                prodid = prod.Prod_ID;
                            }
                            var dublicateInvice = vmList.Where(a => a.Reference == vm.Reference).FirstOrDefault();
                            if (dublicateInvice != null)
                            {
                                var vmdetail = new SalesReceiptDetailsVM();

                                vmdetail.Product_Id = prodid;
                                vmdetail.Product_Quantity = quantity;
                                if (quantity > 0)
                                {
                                    vmdetail.Product_Price = (Convert.ToDecimal(((Range)range.Cells[row, 9]).Text) / quantity) / (1 + tax);
                                }
                                if (vmdetail.Product_Quantity > 0 && vmdetail.Product_Price > 0)
                                {
                                    dublicateInvice.myDetailsVM.Add(vmdetail);
                                }
                                  
                            }
                            else
                            {
                                vm.myDetailsVM = new List<SalesReceiptDetailsVM>();
                                var vmdetail = new SalesReceiptDetailsVM();

                                vmdetail.Product_Id = prodid;
                                vmdetail.Product_Quantity = quantity;
                                if (quantity > 0)
                                {
                                    vmdetail.Product_Price = (Convert.ToDecimal(((Range)range.Cells[row, 9]).Text) / quantity) / (1 + tax);
                                }
                                vm.myDetailsVM.Add(vmdetail);
                                if (vmdetail.Product_Quantity > 0 && vmdetail.Product_Price > 0)
                                {

                                    vmList.Add(vm);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            messageList.Add(row.ToString());
                            continue;
                        }
                    }

                    workbook.Save();
                    workbook.Close(path);
                    // File.Delete(path);

                }
            }
            return Tuple.Create(vmList, messageList);
        }



        public static SalesReceiptMaster AddInvoiceByExcelSheet(SalesReceiptMasterVM saleMastVM)
        {
            var db = new AccountingContainer();
            var saleRecietMaster = new SalesReceiptMaster();
          
            if (saleMastVM != null)
            {
                var listOfSale = db.SalesReceiptMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                //using string substring method to get the number of the last inserted

                //var listOfSale = db.SalesReceiptMasters.ToList();
                if (listOfSale == null)
                {
                    saleRecietMaster.Serial_Number = "SALRC-001";
                }
                else
                {
                    var lastSerial = db.SalesReceiptMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                    //using string substring method to get the number of the last inserted
                    saleRecietMaster.Serial_Number = "SALRC-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(6, lastSerial.Serial_Number.Length - 6)) + 1).ToString("D3");
                }

                saleRecietMaster.Description = saleMastVM.Description;
                var comp = db.Companies.Find(saleMastVM.Company_Id);
                if (comp != null)
                {
                    var date = Convert.ToDateTime(saleMastVM.Invoice_Date);
                    if (date < comp.StartFinancialDate)
                    {
                        saleRecietMaster.Invoice_Date = DateTime.Now;
                    }
                    else
                    {
                        saleRecietMaster.Invoice_Date = Convert.ToDateTime(saleMastVM.Invoice_Date);
                    }
                }
                else
                {
                    saleRecietMaster.Invoice_Date = DateTime.Now;
                }
                saleRecietMaster.Invoice_Date = Convert.ToDateTime(saleMastVM.Invoice_Date);
                saleRecietMaster.Customer_Id = saleMastVM.Customer_Id;
                saleRecietMaster.Notes = saleMastVM.Notes;
                saleRecietMaster.Master_Discount = saleMastVM.Master_Discount;
             
                    saleRecietMaster.Invoice_Currency = "الريال السعودى";
            
                saleRecietMaster.Store_ID = 1;
                saleRecietMaster.Company_Id = 1;
                saleRecietMaster.PaymentMethod_Id = 1;
                saleRecietMaster.Account_Id = 76;

                saleRecietMaster.Reference = saleMastVM.Reference;
                saleRecietMaster.ApprovalCode = saleMastVM.ApprovalCode;
                saleRecietMaster.Exchange_Rate = saleMastVM.Exchange_Rate;
                saleRecietMaster.Invoice_State = TransactionState.Pending;
                saleRecietMaster.Branch_Id = saleMastVM.Branch_Id;
                saleRecietMaster.Department_ID = saleMastVM.Department_ID;
                saleRecietMaster.BussinesUnit_ID = saleMastVM.BussinesUnit_ID;
               
              
                    saleRecietMaster.Emp_Id =1;
                
                
               saleRecietMaster.SubLedger_ID = 1; 

                
               
                var qrMaster = new MasterQRInvoice();
                qrMaster.QrGuid = Guid.NewGuid();
                qrMaster.DetailQRInvoices = new List<DetailQRInvoice>();
                qrMaster.QRPaymentMethods = new List<QRPaymentMethod>();
                qrMaster.TaxQRInvoices = new List<TaxQRInvoice>();
                foreach (var item in saleMastVM.myDetailsVM.ToList())
                {
                    var details = new SalesReceiptDetails();
                    details.Details_Id = item.Details_Id;
                    details.Product_Quantity = item.Product_Quantity;
                    details.Product_Price = item.Product_Price;
                    details.Detail_Discount = 0;
                    details.Detail_Discount_Amount = 0;
                    details.Product_Tax = 1;
                    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    decimal discount = TotalBeforeDiscount * (saleRecietMaster.Master_Discount / 100);
                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                    details.Batch_ID = item.Batch_ID;
                    if (details.Product_Tax == 0)
                    {
                        details.Detail_Total_Tax = 0;
                    }
                    else
                    {
                        var tax = Convert.ToDecimal(db.Taxes.Find(details.Product_Tax).Tax_Percentage);
                        details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                    }
                    // if the product in batch
                  
                   
                        Product productss = db.Products.Find(item.Product_Id);
                        details.Product_Id = productss.Prod_ID;

                        // Calculate the Total_Cost


                        if (productss.Prod_Type == Prod_Type.خدمة)
                        {
                            details.Detail_Total_Cost = 0;
                        }
                        else
                        {


                            details.Detail_Total_Cost = item.Product_Quantity * productss.Prod_Cost_Price.Value;
                        }


                        var price = item.Product_Price * item.Product_Quantity;
                        details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                        saleRecietMaster.Total_Amount += details.Total_Amount;
                        saleRecietMaster.Total_Cost += details.Detail_Total_Cost;
                        saleRecietMaster.Total_Tax += details.Detail_Total_Tax;
                        details.Reverse_Total_Tax = details.Detail_Total_Tax;
                        details.Reverse_Quantity = details.Product_Quantity;

                        details.Reverse_Total_Amount = details.Total_Amount;
                        saleRecietMaster.SalesReceiptDetails.Add(details);
                       
                    
                    ///// RR
                    ///
                    var invoiceQRDetail = new DetailQRInvoice();
                    invoiceQRDetail.Batch_ID = details.Batch_ID;
                    invoiceQRDetail.Product_Id = details.Product_Id;
                    invoiceQRDetail.Product_Quantity = details.Product_Quantity;
                    invoiceQRDetail.Product_Tax = details.Product_Tax;

                    invoiceQRDetail.Detail_Discount = 0;
                    invoiceQRDetail.Detail_Discount_Amount = 0;
                    invoiceQRDetail.Detail_Total_Cost = details.Detail_Total_Cost;

                    invoiceQRDetail.Total_Amount = details.Total_Amount;

                    invoiceQRDetail.Product_Price = details.Product_Price;
                    invoiceQRDetail.Detail_Total_Tax = details.Detail_Total_Tax;
                    invoiceQRDetail.ProductPriceCost = details.Product_Cost.Value;
                    qrMaster.DetailQRInvoices.Add(invoiceQRDetail);
                }
                saleRecietMaster.TotalNewTaxs = 0;
            


                saleRecietMaster.FixedDiscount = saleMastVM.Fixed_Discount;
                saleRecietMaster.TotaL_After_Fixed = (saleRecietMaster.Total_Amount + saleRecietMaster.TotalNewTaxs) - saleRecietMaster.FixedDiscount;
                var payment = db.PaymentMethods.Find(saleRecietMaster.PaymentMethod_Id);

                saleRecietMaster.Total_Payment_Method_Cost = (saleRecietMaster.TotaL_After_Fixed.Value * (payment.Percentage.Value / 100));


                if (payment.Operatin_Cost > 0)
                {
                    saleRecietMaster.Total_Payment_Method_Cost = saleRecietMaster.Total_Payment_Method_Cost + payment.Operatin_Cost.Value;

                }
                var paymentQR = new QRPaymentMethod();
                paymentQR.PaymentMethod_Id = saleRecietMaster.PaymentMethod_Id;
                paymentQR.Commution = 0;
                paymentQR.Amount = saleRecietMaster.TotaL_After_Fixed.Value;
                qrMaster.QRPaymentMethods.Add(paymentQR);


                //add QR Invoice
                qrMaster.AllowDiscount = saleRecietMaster.FixedDiscount.Value;
                qrMaster.ApprovalCode = saleRecietMaster.Reference;
                qrMaster.Company_Id = saleRecietMaster.Company_Id;
                qrMaster.Customer_Id = saleRecietMaster.Customer_Id;
                qrMaster.Description = saleRecietMaster.Description;
                qrMaster.Employee_Id = saleRecietMaster.Emp_Id.Value;
                qrMaster.Invoice_Date = saleRecietMaster.Invoice_Date;
                qrMaster.Invoice_State = saleRecietMaster.Invoice_State;
                qrMaster.InvoiceType_Id = 2;
                qrMaster.Master_Discount = saleRecietMaster.Master_Discount;

                qrMaster.Reference = saleRecietMaster.Reference;
                qrMaster.Serial_Number = saleRecietMaster.Serial_Number;
                qrMaster.Store_ID = saleRecietMaster.Store_ID;
                qrMaster.Subledger_Id = saleRecietMaster.SubLedger_ID;
                qrMaster.Notes = saleRecietMaster.Notes;
                qrMaster.TotalNewTaxs = saleRecietMaster.TotalNewTaxs.Value;
                qrMaster.Account_Id = saleRecietMaster.Account_Id;
                qrMaster.Total_Amount = saleRecietMaster.Total_Amount;
                qrMaster.InvoiceType = "فاتورة مبيعات نقدي";
                qrMaster.Total_Payment_Method_Cost = 0;
                qrMaster.Total_Tax = saleRecietMaster.Total_Tax;
                qrMaster.Branch_Id = saleRecietMaster.Branch_Id;
                qrMaster.Department_ID = saleRecietMaster.Department_ID;
                qrMaster.BussinesUnit_ID = saleRecietMaster.BussinesUnit_ID;


                qrMaster.Total_Cost = saleRecietMaster.Total_Cost;
                qrMaster.Notes = saleRecietMaster.Notes;
                db.MasterQRInvoices.Add(qrMaster);
                db.SaveChanges();
                saleRecietMaster.QRNumber = qrMaster.Id;

                db.SalesReceiptMasters.Add(saleRecietMaster);
           
                db.SaveChanges();
                db.Dispose();

                CreateTransactionBatch(saleRecietMaster, "0");

                return saleRecietMaster;

            }
            return null;
        }

        public static String MakeVoidForSales(int invoiceId)
        {
            var db = new AccountingContainer();
            var invoiceMaster = db.SalesReceiptMasters.Find(invoiceId);

            var Allreverse = db.ReverseSalesReceiptMasters.Where(a => a.SalesRecipt_Id == invoiceId).FirstOrDefault();
            if (Allreverse != null)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.ThierMoreInvoiceForThat;
            }
            else
            {

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

                var comp = db.Companies.FirstOrDefault();
                if (comp != null)
                {
                    var date = Convert.ToDateTime(DateTime.Now);
                    if (date < comp.StartFinancialDate)
                    {
                        master.Invoice_Date = DateTime.Now;
                    }
                    else
                    {
                        master.Invoice_Date = DateTime.Now;
                    }
                }
                else
                {
                    master.Invoice_Date = DateTime.Now;
                }




                if (master != null)
                {
                    master.Master_Id = invoiceMaster.Master_Id;
                    master.SalesRecipt_Id = invoiceMaster.Master_Id;
                    master.Master_Discount = invoiceMaster.Master_Discount;
                    master.SubLedger_ID = invoiceMaster.SubLedger_ID;
                    master.Branch_Id = invoiceMaster.Branch_Id;
                    master.BussinesUnit_ID = invoiceMaster.BussinesUnit_ID;
                    master.Department_ID = invoiceMaster.Department_ID;
                    master.Customer_Id = invoiceMaster.Customer_Id;


                    master.Company_Id = invoiceMaster.Company_Id;


                    master.Invoice_Currency = invoiceMaster.Invoice_Currency;


                    master.Store_ID = invoiceMaster.Store_ID;
                }



                master.Invoice_State = TransactionState.Post;

               





                decimal Sumtax = 0;
                decimal SumtAmount = 0;
                decimal SumTotalCost = 0;

                foreach (var item in invoiceMaster.SalesReceiptDetails.ToList())
                {
                    var details = new ReverseSalesReceiptDetails();
                    details.Product_Id = item.Product_Id;
                    details.Product_Quantity = item.Product_Quantity;

                    details.Product_Price = item.Product_Price;
                    details.Detail_Discount = item.Detail_Discount;
                    details.Detail_Discount_Amount = item.Detail_Discount_Amount;





                    item.Detail_Total_Cost = item.Detail_Total_Cost / item.Product_Quantity;

                    item.Reverse_Quantity = 0;

                    details.Product_Tax = item.Product_Tax;
                

                    var salesInvoiceDetail = db.SalesReceiptDetails.Where(a => a.Product_Id == item.Product_Id).FirstOrDefault();
                    var productBatch = db.ProductBatches.Where(a => a.Product_ID == salesInvoiceDetail.Product_Id && a.Store_ID == master.Store_ID).FirstOrDefault();


                    //var mySingelCost = item.Detail_Total_Cost / item.Product_Quantity;


                    details.Detail_Total_Cost = (item.Detail_Total_Cost * item.Product_Quantity);


                    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    decimal discount = TotalBeforeDiscount * (item.Detail_Discount.Value / 100);
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



                    var price = details.Product_Price * details.Product_Quantity;
                    details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                    SumtAmount += (decimal)details.Total_Amount;

                    Sumtax += (decimal)details.Detail_Total_Tax;
                    SumTotalCost += (decimal)details.Detail_Total_Cost;
                    master.Total_Tax = Sumtax;
                    master.Total_Amount = SumtAmount;
                    master.Total_Cost = SumTotalCost;
                 

                    master.ReverseSalesReceiptDetails.Add(details);
                    master.Invoice_State = TransactionState.Pending;


                    db.Entry(item).State = EntityState.Modified;




                  


                }

                master.Customer_Id = invoiceMaster.Customer_Id;
                master.Company_Id = invoiceMaster.Company_Id;
                master.PaymentMethod_Id = invoiceMaster.PaymentMethod_Id;
                master.Store_ID = invoiceMaster.Store_ID;
                master.Invoice_Currency = invoiceMaster.Invoice_Currency;
                master.Master_Discount = (master.ReverseSalesReceiptDetails.Select(a => a.Detail_Discount_Amount).Sum() / master.ReverseSalesReceiptDetails.Select(a => a.Product_Quantity * a.Product_Price).Sum()) * 100;
                master.Number = InvoiceSerial.generateSerialForReverse();
                db.ReverseSalesReceiptMasters.Add(master);

                db.SaveChanges();

                DateTime DateNow = DateTime.Now;




                var transBatch = new TransactionBatch();
                transBatch.Date = DateNow;
                transBatch.OperationName = "مردود مبيعات نقدي";
                transBatch.OperationId = master.Master_Id;
                transBatch.Operation_Code = master.Serial_Number;
                transBatch.Description = "عكس قيد مبيعات نقدي";
                transBatch.UserId = "noon";
                db.TransactionBatches.Add(transBatch);
                ReverseSalesReceiptMasterVM reverseVM = new ReverseSalesReceiptMasterVM();
                reverseVM.Master_Id = master.Master_Id;
                reverseVM.Invoice_State = TransactionState.Post;
                MakeSalesinQrSales(master);
                var msg = ReverseSalesReceiptMaster.UpdateInvoiceMaster(reverseVM);
                db.SaveChanges();
                db.Dispose();

                return msg;




            }



        }

        public static bool MakeSalesinQrSales(ReverseSalesReceiptMaster revmaster)
        {
            var db = new AccountingContainer();


            var qrmaster = new ReverseQRInvoice();
            var master = db.SalesReceiptMasters.Find(revmaster.SalesRecipt_Id);
            if (master.Account_Id > 0)
            {
                qrmaster.Account_Id = master.Account_Id;
            }
            else { master.Account_Id = 76; }

            qrmaster.ApprovalCode = "";
            qrmaster.Number = revmaster.Number;
            qrmaster.DateOfCreate = DateTime.Now;
            qrmaster.Company_Id = revmaster.Company_Id;
            qrmaster.Customer_Id = revmaster.Customer_Id;
            qrmaster.Description = revmaster.Description;
            qrmaster.Exchange_Rate = 0;
            qrmaster.HasOff = revmaster.HasOff;
            qrmaster.Invoice_Currency = revmaster.Invoice_Currency;
            qrmaster.Invoice_Date = revmaster.Invoice_Date;
            qrmaster.Invoice_State = revmaster.Invoice_State;
            qrmaster.MasterQRInvoiceId = master.QRNumber.Value;
            qrmaster.Master_Discount = revmaster.Master_Discount;
            qrmaster.PaymentMethod_Id = revmaster.PaymentMethod_Id;
            qrmaster.Reference = "";
            qrmaster.Serial_Number = revmaster.Serial_Number;
            qrmaster.Store_ID = revmaster.Store_ID;
            qrmaster.SubLedger_ID = master.SubLedger_ID;
            qrmaster.Total_Amount = revmaster.Total_Amount;
            qrmaster.Total_Cost = revmaster.Total_Cost;
            qrmaster.Total_Payment_Method_Cost = 0;
            qrmaster.Total_Tax = revmaster.Total_Tax;
            var qrmmaster = db.MasterQRInvoices.Find(qrmaster.MasterQRInvoiceId);
            qrmaster.ReversePOSDetails = new List<ReverseQRDetail>();
            foreach (var item in revmaster.ReverseSalesReceiptDetails)
            {
                var qrdetail = new ReverseQRDetail();
                if (item.Batch_ID != null)
                {
                    qrdetail.Batch_ID = item.Batch_ID;
                }
                qrdetail.DetailQRInvoiceDetail_Id = qrmmaster.DetailQRInvoices.Where(a => a.Product_Id == item.Product_Id).FirstOrDefault().Detail_Id;
                qrdetail.Detail_Discount = item.Detail_Discount;
                qrdetail.Detail_Discount_Amount = item.Detail_Discount_Amount;
                qrdetail.Detail_Total_Cost = item.Detail_Total_Cost;
                qrdetail.Detail_Total_Tax = item.Detail_Total_Tax;
                qrdetail.Product_Id = item.Product_Id;
                qrdetail.Product_Price = item.Product_Price;
                qrdetail.Product_Quantity = item.Product_Quantity;
                qrdetail.Product_Tax = Convert.ToInt32(item.Product_Tax);
                qrdetail.Total_Amount = item.Total_Amount;

                qrmaster.ReversePOSDetails.Add(qrdetail);

            }
            db.ReverseQRInvoices.Add(qrmaster);
            db.SaveChanges();

            return true;
        }
    }
}