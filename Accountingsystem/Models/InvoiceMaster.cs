using Accountingsystem.Utilites;
using Accountingsystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using System.Web.Mvc;

namespace Accountingsystem.Models
{
    public partial class InvoiceMaster
    {
        /// <summary>
        /// make unique Code for Each Invoice that Create
        /// </summary>
        /// <param name="Serial"></param>
        /// <returns></returns>
        public static bool containCode(string Serial)
        {
            var db = new AccountingContainer();
            var allinData = db.InvoiceMasters.ToList();
            foreach (var item in allinData)
            {
                if (item.Serial_Number == Serial || !Serial.StartsWith("SAL-"))
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
            var detail = db.InvoiceDetailsTemps.Find(detail_Id);
            var master = detail.InvoiceMasterTemp;
            var batch = db.Products.Where(pro => pro.Prod_ID == detail.Product_ID).FirstOrDefault();

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
            detail.Detail_Total_Cost = batch.Prod_Cost_Price.Value * quantity;
            master.Total_Amount -= detail.Total_Amount;
            detail.Total_Amount = (totalprice + detail.Detail_Total_Tax) - discound;
            master.Total_Amount += detail.Total_Amount;
            db.Entry(detail).State = EntityState.Modified;
            db.SaveChanges();
            var ee = master.InvoiceDetailsTemps.Select(a => a.Product_Quantity * a.Product_Price * (a.Detail_Discount.Value / 100)).Sum();
            var bb = master.InvoiceDetailsTemps.Select(a => a.Product_Quantity * a.Product_Price).Sum();
            decimal masdiscount = (ee / bb) * 100;
            master.Master_Discount = masdiscount;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return masdiscount;

        }
        public static decimal changeSpecificPrice(int detail_Id, decimal Price, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.InvoiceDetailsTemps.Find(detail_Id);
            var master = detail.InvoiceMasterTemp;

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
            var ee = master.InvoiceDetailsTemps.Select(a => a.Product_Price*a.Product_Quantity * (a.Detail_Discount.Value / 100)).Sum();
            var bb = master.InvoiceDetailsTemps.Select(a => a.Product_Price * a.Product_Quantity).Sum();
            decimal masdiscount = (ee / bb) * 100;
            master.Master_Discount = masdiscount;
            db.Entry(master).State = EntityState.Modified;


            db.SaveChanges();
            return masdiscount;

        }
        public static decimal changeSpecificDiscount(int detail_Id, decimal discountdetail)
        {
            var db = new AccountingContainer();
            var detail = db.InvoiceDetailsTemps.Find(detail_Id);
            var master = detail.InvoiceMasterTemp;

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
            var ee = master.InvoiceDetailsTemps.Select(a => a.Product_Price * a.Product_Quantity * (a.Detail_Discount.Value / 100)).Sum();
            var bb = master.InvoiceDetailsTemps.Select(a => a.Product_Price * a.Product_Quantity).Sum();
            decimal masdiscount = (ee / bb) * 100;
            return masdiscount;

        }
        public static decimal changeSpecificDiscountAmount(int detail_Id, decimal discountdetailAmount)
        {
            var db = new AccountingContainer();
            var detail = db.InvoiceDetailsTemps.Find(detail_Id);
            var master = detail.InvoiceMasterTemp;

            detail.Detail_Discount_Amount = discountdetailAmount;
           
            decimal totalprice = detail.Product_Price * detail.Product_Quantity;
            detail.Detail_Discount = (discountdetailAmount / totalprice)*100;
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
            var ee = master.InvoiceDetailsTemps.Select(a => a.Product_Price * a.Product_Quantity * (a.Detail_Discount.Value / 100)).Sum();
            var bb = master.InvoiceDetailsTemps.Select(a => a.Product_Price * a.Product_Quantity).Sum();
            decimal masdiscount = (ee / bb) * 100;
            return masdiscount;

        }
        public static decimal changeSpecificTotalAmount(int detail_Id, decimal totalAmount)
        {
        
            var db = new AccountingContainer();
            var detail = db.InvoiceDetailsTemps.Find(detail_Id);
            var master = detail.InvoiceMasterTemp;
            decimal tax = 0;
            if (detail.Product_Tax > 0)
            {
                 tax = db.Taxes.Find(detail.Product_Tax).Tax_Percentage / 100;
            }

            var price=(totalAmount)/((1+ tax)*(1-(detail.Detail_Discount.Value/100))*detail.Product_Quantity);



            

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
            var ee = master.InvoiceDetailsTemps.Select(a => a.Product_Price * a.Product_Quantity * (a.Detail_Discount.Value / 100)).Sum();
            var bb = master.InvoiceDetailsTemps.Select(a => a.Product_Price * a.Product_Quantity).Sum();
            decimal masdiscount = (ee / bb) * 100;
            return price;

        }
        public static string changeSpecificTax(int detail_Id, int product_Tax, decimal discount)
        {
            var db = new AccountingContainer();
            var detail = db.InvoiceDetailsTemps.Find(detail_Id);
            var master = detail.InvoiceMasterTemp;




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

        // Returns a list of all invoice masters

        public static List<InvoiceMaster> GetAllInvoices(int? all)
        {
            var db = new AccountingContainer();
            var orders = db.InvoiceMasters.ToList();
            if (all == null)
            {
                orders.Reverse();
                orders = orders.Take(100).ToList();
            }

            return orders;
        }
        public static List<InvoiceMaster> GetAllInvoiceForThatUser(string UserId, int? all)
        {
            var db = new AccountingContainer();
            var orders = db.InvoiceMasters.ToList();
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
        // Gets an invoice master by its id
        public static int GetAvailbleProductQty(int batchId)
        {
            var db = new AccountingContainer();
            var batch = db.ProductBatches.Find(batchId);
            return Convert.ToInt32(batch.Product_Quantity);

        }

        /// <summary>
        /// get That invoice by using its id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public static InvoiceMaster GetInvoiceDetailsById(int invoiceId)
        {
            var db = new AccountingContainer();
            var detail = db.InvoiceMasters.Find(invoiceId);
            return detail;

        }


        /// <summary>
        /// prepare Detail For Creation and Filttering the product in The Store Choseing
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public static InvoiceMasterDetailsVM PrepareInvoiceMasterDetail(int storeId)
        {
            var db = new AccountingContainer();
            var invoiceMasterDetailsVM = new InvoiceMasterDetailsVM();


            List<object> newList2 = new List<object>();
            var thistaxinchart = db.ChartOfAccountings.Find(48);
            var members2 = db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false).ToList();
            foreach (var member1 in members2)
                newList2.Add(new
                {
                    Id = member1.Tax_ID,
                    Name = member1.Tax_Name + "  % " + member1.Tax_Percentage
                });
            invoiceMasterDetailsVM.ProducttaxList = new SelectList(newList2, "Id", "Name");




            var newListpro = (from prod in db.Products.Where(a => a.IsActive != false && a.Prod_Type != Prod_Type.اولي)
                              select new
                              {
                                  Id = prod.Prod_ID,
                                  Name = prod.Prod_Name + "   " + prod.Bar_Code


                              });


            invoiceMasterDetailsVM.ProductNotBatchList = new SelectList(newListpro, "Id", "Name");
            //invoiceMasterDetailsVM.ProductNotBatchList = new SelectList(db.Products.ToList(), "Prod_ID", "Prod_Name");
            invoiceMasterDetailsVM.Product_Quantity = 1;
            //invoiceMasterDetailsVM.Product_Tax = db.Taxes.Where(a => a.Tax_Code.StartsWith("02")).Where(a => a.IsActive != false).FirstOrDefault().Tax_ID;
            return invoiceMasterDetailsVM;

        }

        /// <summary>
        /// Preparing Detail For OfferPrice to Create Invoice
        /// </summary>
        /// <param name="masterID"></param>
        /// <returns></returns>
        public static OfferPriceDetailVM PrepareOfferDetail(int masterID)
        {
            var db = new AccountingContainer();
            var invoiceMasterDetailsVM = new OfferPriceDetailVM();
            var OfferDetail = db.OfferPriceDetails.Where(e => e.OfferPriceMasterMaster_Id == masterID).ToList();

            var name = "";
            foreach (var item in OfferDetail)
            {

                var proname = db.Products.Find(item.Product_ID);
                name = proname.Prod_Name;

            }
            invoiceMasterDetailsVM.OfferPriceMasterMaster_Id = masterID;
            var thistaxinchart = db.ChartOfAccountings.Find(48);
            invoiceMasterDetailsVM.taxList = new SelectList(db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)), "Tax_ID", "Tax_Name");

            invoiceMasterDetailsVM.ProductList = new SelectList(db.OfferPriceDetails.Where(e => e.OfferPriceMasterMaster_Id == masterID), "Product_ID", "Product_Name");
            return invoiceMasterDetailsVM;

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
        // Gets the VM used for populating the Create view
        public static MasterVM AddNewForCreation(int orderid, string userId, int Customer_Id = 0)
        {
            var db = new AccountingContainer();
            var masterVM = new MasterVM();
            String StartCodeOfrevenue = "04";
            var emp = db.Employees.Where(a => a.Reg_ID == userId).FirstOrDefault();
            if (emp != null)
            {
                masterVM.Emp_Id = emp.Emp_Id;
            }
            masterVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            masterVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");

            //List<object> newList = new List<object>();
            //var members1 = db.Customers.Where(e => e.Customer_State == Customer_State.نشط).ToList();
            //foreach (var member in members1)
            //    newList.Add(new
            //    {
            //        Id = member.Customer_Id,
            //        Name = member.Customer_Name + "        " + member.Customer_Phone
            //    });


            masterVM.MyStoresList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");

            var newList = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                           select new
                           {
                               Id = cust.Customer_Id,
                               Name = cust.Customer_Name + "        " + cust.Customer_Phone

                           }).ToList();
            masterVM.MyCustomerList = new SelectList(newList, "Id", "Name");
            var thistaxinchart = db.ChartOfAccountings.Find(48);

            var anewList = (from Tax in db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false)
                            select new
                            {

                                Id = Tax.Tax_ID,
                                Name = Tax.Tax_Name + "   " + Tax.Tax_Percentage + "%"

                            }).ToList();
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false).ToList();

            masterVM.TaxList = new SelectList(anewList, "Id", "Name");
            masterVM.EmpList = new SelectList(db.Employees.Where(e => e.Reg_ID != null), "Emp_Id", "Emp_Name");

            masterVM.DepList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            masterVM.BUList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            masterVM.SubList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            masterVM.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");

            masterVM.OrderList = new SelectList(db.OfferPriceMasters.Where(e => e.Offer_State == false), "OfferMas_Id", "Serial_Number");
            masterVM.Document_Type = Document_Type.Sales_Invoice;
            masterVM.Invoice_Date = DateTime.Now.ToString();
            masterVM.Due_Date = DateTime.Now.ToString();

            masterVM.SelectedCustomerId = Customer_Id;
            //sort the Invoices and get the last insert Invoice sales.
            var lastSerial = db.InvoiceMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
            if (lastSerial == null)
            {
                masterVM.Serial_Number = "SAL-001";
            }
            else
            {
                //using string substring method to get the number of the last inserted
                masterVM.Serial_Number = "SAL-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
            }
            var sub = db.SubLedgers.FirstOrDefault();
            if (sub.InvoiceDesc != null || sub.InvoiceNotes != null)
            {
                masterVM.Description = sub.InvoiceDesc;
                masterVM.Notes = sub.InvoiceNotes;
            }


            masterVM.AccountList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfrevenue)), " Account_Id", "Account_Name");
            masterVM.RevenueAccount_Id = db.ChartOfAccountings.Where(c => c.Account_Code == "040101001").FirstOrDefault().Account_Id;
            // masterVM.MyStoresList = new SelectList(db.ChartOfAccountings.Where(a => a.Account_Code.StartsWith("010109")), "Account_Id", "Account_Name");

            // Initialize the Invoice state to be Pending
            masterVM.Invoice_State = TransactionState.Pending;
            masterVM.Fixed_Discount = 0;

            if (orderid != 0 && orderid != null)
            {
                masterVM.order_Id = orderid;
            }
            if (db.Stores.Count() >= 1)
            {
                masterVM.SelectedStoreId = db.Stores.FirstOrDefault().Store_ID;
            }
            var invoiceMasterTemp = new InvoiceMasterTemp();
            var id = (from s in db.InvoiceMasterTemps
                      select s).ToList().LastOrDefault();
            if (id == null)
            { invoiceMasterTemp.Master_Id = 1; }
            else
            { invoiceMasterTemp.Master_Id = id.Master_Id + 1; }
            invoiceMasterTemp.AllowDiscount = 0;
            invoiceMasterTemp.TotalNewTaxs = 0;
            db.InvoiceMasterTemps.Add(invoiceMasterTemp);
            db.SaveChanges();
            masterVM.Master_Id = invoiceMasterTemp.Master_Id;

            return masterVM;
        }
        // Gets the VM used for populating the Create model and its made to create invoice 
        // the call comes from The Offer that has been made for the client
        //
        public static InvoiceDetailsTempVM GetAllTempInvoiceDetails(int masterVmId)
        {
            var db = new AccountingContainer();
            var invoicedetalTemp = new InvoiceDetailsTempVM();
            invoicedetalTemp.invoiceTemp = db.InvoiceDetailsTemps.Where(invoice => invoice.InvoiceMaster_Id == masterVmId).ToList();
            invoicedetalTemp.invoice_Id = masterVmId;
            try
            {
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
        public static InvoiceDetailsTempVM PrepareListdetailOfInvoiceOffer(int masterid, int Offerid)
        {
            var db = new AccountingContainer();
            var offer = db.OfferPriceMasters.Find(Offerid);
            var invoicedetalTemp = new InvoiceDetailsTempVM();

            var Mastertemp = db.InvoiceMasterTemps.Find(masterid);


            var DetailList = new List<InvoiceDetailsTemp>();
            foreach (var item in offer.OfferPriceDetails.ToList())
            {
                var Detail = new InvoiceDetailsTemp();
                Detail.Batch_ID = item.Batch_ID.Value;

                Detail.Detail_Total_Cost = 0;
                Detail.Detail_Total_Tax = item.Detail_Total_Tax;
                Detail.Product_ID = item.Product_ID;
                var pro = db.Products.Find(Detail.Product_ID);
                Detail.Product_Code = pro.Prod_Code;
                Detail.Product_Name = pro.Prod_Name;
                Detail.Detail_Total_Cost = pro.Prod_Cost_Price.Value * item.Product_Quantity;
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
                Detail.Product_Price = item.Product_Price;
                Detail.Product_Quantity = item.Product_Quantity;
                Detail.Product_Tax = item.Product_Tax;
                Detail.Total_Amount = item.Total_Amount;
                Detail.InvoiceMaster_Id = Mastertemp.Master_Id;
                DetailList.Add(Detail);

            }
            if (Mastertemp.InvoiceDetailsTemps.Count != 0)
            {
                db.InvoiceDetailsTemps.RemoveRange(Mastertemp.InvoiceDetailsTemps);
                db.SaveChanges();
            }
            db.InvoiceDetailsTemps.AddRange(DetailList);

            db.SaveChanges();
            invoicedetalTemp.invoiceTemp = DetailList;
            if (offer.FixedDiscount != null)
            {
                invoicedetalTemp.AllowDiscount = offer.FixedDiscount.Value
                    ;
            }
            else
            {
                invoicedetalTemp.AllowDiscount = 0;
            }

            invoicedetalTemp.invoice_Id = masterid;
            return invoicedetalTemp;
        }
        /// <summary>
        /// For add detials for create and Check Validation For Each place
        /// </summary>
        /// <param name="invoiceMasterDetail"></param>
        /// <returns></returns>
        public static string AddInvoiceDetailsTemp(InvoiceMasterDetailsVM invoiceMasterDetail)
        {
            if (invoiceMasterDetail.productNotBatched_ID > 0)
            {

                if (invoiceMasterDetail.Product_Quantity <= 0)
                {
                    invoiceMasterDetail.Product_Quantity = 1;
                }








                var db = new AccountingContainer();

                var invoiceMasterTemp = db.InvoiceMasterTemps.Find(invoiceMasterDetail.InvoiceMaster_Id);

                var invoiceMasterDetailsTemp = new InvoiceDetailsTemp();
                var productsss = db.Products.Find(invoiceMasterDetail.productNotBatched_ID);
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
                    invoiceMasterDetailsTemp.Product_Price = GetPriceSale(invoiceMasterDetail.store, invoiceMasterDetail.productNotBatched_ID, invoiceMasterDetail.CustomerID);


                    invoiceMasterDetailsTemp.Product_Quantity = invoiceMasterDetail.Product_Quantity;
                    var thisProduct = db.Products.Find(invoiceMasterDetailsTemp.Product_ID);
                    if (thisProduct != null)
                    {
                        invoiceMasterDetailsTemp.Product_Name = thisProduct.Prod_Name;
                        invoiceMasterDetailsTemp.Product_Code = thisProduct.Prod_Code;
                    }

                    decimal TotalBeforeDiscount = invoiceMasterDetailsTemp.Product_Price * invoiceMasterDetail.Product_Quantity;
                    decimal discount = TotalBeforeDiscount * (invoiceMasterDetail.Master_Discount / 100);
                    invoiceMasterDetailsTemp.Detail_Discount = invoiceMasterDetail.Master_Discount;
                    invoiceMasterDetailsTemp.Detail_Discount_Amount = discount;
                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;
                    invoiceMasterDetailsTemp.TotalBeforeDisc = TotalBeforeDiscount;
                    invoiceMasterDetailsTemp.TotalDiscount = discount;

                    invoiceMasterDetailsTemp.Detail_Total_Cost = (decimal)productsss.Prod_Cost_Price * invoiceMasterDetail.Product_Quantity;
                    if (productsss.Tax_Id == null || productsss.Tax_Id == 0)
                    {
                        invoiceMasterDetailsTemp.Product_Tax = 0;
                        invoiceMasterDetailsTemp.Detail_Total_Tax = 0;
                    }
                    else
                    {
                        invoiceMasterDetailsTemp.Product_Tax = productsss.Tax_Id.Value;
                        var a = db.Taxes.Find(invoiceMasterDetailsTemp.Product_Tax);

                        invoiceMasterDetailsTemp.Detail_Total_Tax = TotalAfterDiscount * (a.Tax_Percentage / 100);
                    }


                    invoiceMasterDetailsTemp.Total_Amount = (TotalBeforeDiscount + invoiceMasterDetailsTemp.Detail_Total_Tax) - discount;
                    invoiceMasterTemp.InvoiceDetailsTemps.Add(invoiceMasterDetailsTemp);

                    db.Entry(invoiceMasterTemp).State = EntityState.Modified;
                    db.SaveChanges();

                    return Resources.validate.SuccessfullAdd;

                }
            }
            else
            {
                return Resources.OfferPriceMasterCreateOffer.ProductIsNotFoundModelMessage;
            }

        }


        public static decimal GetPriceSale(int storeId, int ProductId, int Customerid)
        {
            var db = new AccountingContainer();
            var FirstSlice = db.Slices.FirstOrDefault().Slice_Id;
            var CustomerSlice = FirstSlice;
            if (Customerid != 0)
            {
                CustomerSlice = db.Customers.Find(Customerid).SlicesSlice_Id;
            }
            var product = db.Products.Find(ProductId);
            if (FirstSlice == CustomerSlice || product.SlicesProducts.Where(a => a.SlicesSlice_Id == CustomerSlice).FirstOrDefault() == null)
            {
                if (storeId == 0)
                {
                    storeId = 1;
                }
                var batch = product.ProductBatches.Where(a => a.Store_ID == storeId).FirstOrDefault();
                if (batch != null && batch.SalesPrice > 0)
                {

                    return batch.SalesPrice.Value;

                }
                else
                {
                    return product.Sale_Price.Value;
                }


            }
            else
            {
                return product.SlicesProducts.Where(a => a.SlicesSlice_Id == CustomerSlice).FirstOrDefault().Slice_Price;

            }


        }
        /// <summary>
        /// For add detials for Editing and Check Validation For Each place
        /// </summary>
        /// <param name="invoiceMasterDetail"></param>
        /// <returns></returns>
        public static string AddInvoiceDetailsTempOffer(InvoiceMasterDetailsVM invoiceMasterDetail)
        {
            var db = new AccountingContainer();
            var invoiceMasterTemp = db.InvoiceMasterTemps.Find(invoiceMasterDetail.InvoiceMaster_Id);
            var invoiceMasterDetailsTemp = new InvoiceDetailsTemp();
            var batch = db.ProductBatches.Where(e => e.Product_ID == invoiceMasterDetail.productID && e.Store_ID == invoiceMasterDetail.store).ToList().FirstOrDefault();
            invoiceMasterDetailsTemp.Batch_ID = batch.Batch_ID;
            invoiceMasterDetailsTemp.Product_ID = batch.Product_ID;
            var dtails = invoiceMasterTemp.InvoiceDetailsTemps;
            var dublicateinTemp = dtails.Where(z => z.InvoiceMaster_Id == invoiceMasterTemp.Master_Id && z.Product_ID == invoiceMasterDetailsTemp.Product_ID).ToList();
            if (dublicateinTemp.FirstOrDefault() != null)
            {
                return "هذا المنتج موجود بالفعل فى القائمة لا يمكن اضافته مرة اخرى";
            }
            else
            {
                invoiceMasterDetailsTemp.Product_Price = invoiceMasterDetail.Product_Price;
                invoiceMasterDetailsTemp.Product_Quantity = invoiceMasterDetail.Product_Quantity;
                var thisProduct = db.Products.Find(invoiceMasterDetailsTemp.Product_ID);
                if (thisProduct != null)
                {
                    invoiceMasterDetailsTemp.Product_Name = thisProduct.Prod_Name;
                    invoiceMasterDetailsTemp.Product_Code = thisProduct.Prod_Code;
                }
                var resultTrea = db.Taxes.Find(invoiceMasterDetail.Product_Tax);
                if (resultTrea != null)
                {
                    invoiceMasterDetailsTemp.Product_Tax = resultTrea.Tax_ID;
                }
                var a = db.Taxes.Find(invoiceMasterDetailsTemp.Product_Tax);
                invoiceMasterDetailsTemp.Detail_Total_Tax = (invoiceMasterDetail.Product_Price * invoiceMasterDetail.Product_Quantity) * (Convert.ToDecimal(a.Tax_Percentage) / 100);
                invoiceMasterDetailsTemp.Detail_Total_Cost = batch.Product_Cost * invoiceMasterDetail.Product_Quantity;
                invoiceMasterDetailsTemp.Total_Amount = (invoiceMasterDetail.Product_Price * invoiceMasterDetail.Product_Quantity);
                invoiceMasterTemp.InvoiceDetailsTemps.Add(invoiceMasterDetailsTemp);

                db.Entry(invoiceMasterTemp).State = EntityState.Modified;
                db.SaveChanges();

                return "تمت الاضافة بنجاح";
            }
        }
        //when click on convert to invoice From OfferPrice
        //Prepare that form of invoice that come From offerPrice
        //public static MasterVM AddNewForCreation(int offerId)
        //{
        //    var db = new AccountingContainer();
        //    var offr = db.OfferPriceMasters.Find(offerId);

        //    var msvm = new MasterVM();
        //    String StartCodeOfrevenue = "04";
        //    msvm.Notes = offr.Notes;
        //    msvm.Description = offr.Description;
        //    msvm.Master_Id = offr.OfferMas_Id;
        //    var currency = db.Currencies.Find(offr.Currency_Id);
        //    var k = db.Customers.Find(offr.Customer_Id);
        //    msvm.Customer_Id = k.Customer_Id;
        //    msvm.Company_Id = offr.Company_Id;
        //    msvm.Master_Discount = offr.Master_Discount;
        //    msvm.SelectedStoreId = (int)offr.Store_ID;
        //    msvm.Invoice_Date = offr.Offer_Date.ToString();
        //    msvm.Branch_Id = offr.Branch_Id;

        //    msvm.Reference = offr.Reference;
        //    msvm.RevenueAccount_Id = offr.RevenueAccount_Id;
        //    if(offr.Emp_Id!=0&& offr.Emp_Id != null)
        //    {
        //        msvm.Emp_Id = offr.Emp_Id.Value;
        //    }
        //    else
        //    {
        //        msvm.Emp_Id = db.Employees.FirstOrDefault().Emp_Id;
        //    }

        //  //  msvm.Payment_Policy = offr.Payment_Policy.Value;
        //    msvm.Tax_ID = offr.Tax_ID;
        //    msvm.order_Id = offr.OfferMas_Id;
        //    msvm.AccountList = new SelectList(db.ChartOfAccountings.Where(a => a.Account_Code.StartsWith(StartCodeOfrevenue)), " Account_Id", "Account_Name");
        //    msvm.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
        //    msvm.DepList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
        //    msvm.MyBussinesUnitList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
        //    msvm.SubList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى"), "SubLed_Id", "SubLed_Name");
        //    msvm.EmpList = new SelectList(db.Employees.Where(e => e.Reg_ID != null), "Emp_Id", "Emp_Name");
        //    msvm.MyCustomerList = new SelectList(db.Customers.Where(e=>e.Customer_State==Customer_State.نشط).ToList(), "Customer_Id", "Customer_Name");

        //    msvm.OfferCode = offr.Serial_Number;
        //    var lastSerial = db.InvoiceMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
        //    if (lastSerial == null)
        //    {
        //        msvm.Serial_Number = "SAL-001";
        //    }
        //    else
        //    {
        //        //using string substring method to get the number of the last inserted
        //        msvm.Serial_Number = "SAL-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
        //    }
        //    msvm.SelectedCompany_Id = offr.Company_Id;
        //    msvm.SelectedCustomerId = offr.Customer_Id;

        //    msvm.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
        //    msvm.SelectedCurrencyId = offr.Currency_Id;
        //    msvm.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");

        //    msvm.MyStoresList = new SelectList(db.Stores, "Store_ID", "Store_Name");

        //    // Initialize the Invoice state to be Pending
        //    msvm.Invoice_State = TransactionState.Pending;
        //    msvm.Document_Type = Document_Type.Sales_Invoice;
        //    msvm.Branch_Id = offr.BranchSubledger_Id;
        //    msvm.Department_ID = offr.DepartSubledger_Id;
        //    msvm.BussinesUnit_ID = offr.BussinesSubledger_Id;
        //    //  msvm.myDetailsVM = _myDetailsVM;
        //    return msvm;
        //}

        /// <summary>
        /// get the discount from OfferPrice and Make form for that invoice 
        /// </summary>
        /// <param name="offerId"></param>
        /// <param name="discount"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static int AddNewOfferInvoice(int offerId, decimal discount, String UserId)
        {
            var model = AddNewForCreation(offerId, UserId);
            var masterId = AddNewInvoiceFromOffer(model, discount, UserId);
            return masterId;
        }
        /// <summary>
        /// To do :Check equ if master, res, store are not null get the data
        /// and Create Invoice Master From OfferPrice
        /// </summary>
        /// <param name="invoiceMasterDetail"></param>
        /// <returns></returns>
        public static int AddNewInvoiceFromOffer(MasterVM masterVM, decimal fixeddiscount, String UserId)
        {
            var db = new AccountingContainer();
            var detail = db.OfferPriceDetails.Where(a => a.OfferPriceMasterMaster_Id == masterVM.order_Id).ToList();
            var offermaster = db.OfferPriceMasters.Find(masterVM.order_Id);
            var master = new InvoiceMaster();

            if (masterVM != null)
            {
                // sort the Invoices and get the last insert Invoice sales.
                var lastSerial = db.InvoiceMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                if (lastSerial == null)
                {
                    master.Serial_Number = "SAL-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    master.Serial_Number = "SAL-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
                }

                master.Description = masterVM.Description;
                master.Invoice_Date = DateTime.Now;
                master.Due_Date = DateTime.Now.AddMonths(masterVM.Payment_Policy);
                master.Notes = masterVM.Notes;
                master.Customer_Id = masterVM.SelectedCustomerId;
                master.Company_Id = masterVM.SelectedCompany_Id;
                master.Offer_Price_Code = masterVM.OfferCode;
                var resultCurrency = db.Currencies.Find(masterVM.SelectedCurrencyId);
                if (resultCurrency != null)
                {
                    master.Invoice_Currency = resultCurrency.Currency_Type;
                }

                var resultStore = db.Stores.Find(masterVM.SelectedStoreId);
                if (resultStore != null)
                {
                    master.Store_ID = resultStore.Store_ID;
                }

                master.Master_Discount = masterVM.Master_Discount;

                master.Document_Type = masterVM.Document_Type;
                master.Invoice_State = TransactionState.Pending;
                master.RevenueAccount_Id = masterVM.RevenueAccount_Id;
                master.Branch_Id = masterVM.Branch_Id;
                master.Department_ID = masterVM.Department_ID;
                master.BussinesUnit_ID = masterVM.BussinesUnit_ID;
                master.Reference = masterVM.Reference;
                master.Emp_Id = masterVM.Emp_Id;
                master.Tax_ID = masterVM.Tax_ID;
                master.Payment_Policy = masterVM.Payment_Policy;
                if (masterVM.SubLedger_ID != null && masterVM.SubLedger_ID != 0)
                {

                    master.SubLedger_ID = masterVM.SubLedger_ID;

                }
                else if (masterVM.BussinesUnit_ID != null && masterVM.BussinesUnit_ID != 0)
                {

                    master.SubLedger_ID = masterVM.BussinesUnit_ID;

                }
                else if (masterVM.Department_ID != null && masterVM.Department_ID != 0)
                {

                    master.SubLedger_ID = master.Department_ID;

                }
                else if (masterVM.Branch_Id != null && masterVM.Branch_Id != 0)
                {

                    master.SubLedger_ID = master.Branch_Id;

                }
                else { master.SubLedger_ID = 1; }

                master.FixedDiscount = fixeddiscount;

                List<InvoiceDetails> _myDetails = new List<InvoiceDetails>();


                foreach (var item in detail)
                {

                    // Create new detail and map the values from detailVM to it
                    // TODO: Use AutoMapper
                    var details = new InvoiceDetails();
                    details.Details_Id = item.OfferDet_Id;
                    details.Product_Quantity = item.Product_Quantity;
                    details.Reverse_Quantity = item.Product_Quantity;

                    details.Product_Price = item.Product_Price;
                    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    decimal discount = TotalBeforeDiscount * (master.Master_Discount / 100);
                    decimal TotalAfterDiscount = TotalBeforeDiscount - discount;


                    details.Product_Tax = Convert.ToInt32(item.Product_Tax);


                    details.Batch_ID = item.Batch_ID;
                    master.Tax_ID = (int)item.Product_Tax;


                    // TEMP: Fix relations
                    if (item.Batch_ID > 0)
                    {
                        ProductBatches detailBatch = db.ProductBatches.Find(details.Batch_ID);

                        details.Product_ID = detailBatch.Product_ID;
                        var battchh = db.ProductBatches.Find(item.Batch_ID);

                        var tax = Convert.ToDecimal(db.Taxes.Find(details.Product_Tax).Tax_Percentage);
                        details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                        details.Reverse_Total_Tax = details.Detail_Total_Tax;


                        details.Batch_ID = item.Batch_ID;
                        var price = item.Product_Quantity * item.Product_Price;
                        details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                        details.Reverse_Total_Amount = details.Total_Amount;
                        master.Total_Amount += details.Total_Amount;
                        master.Total_Cost += details.Detail_Total_Cost;

                        master.Total_Tax += details.Detail_Total_Tax;
                        master.InvoiceDetails.Add(details);

                    }
                    else
                    {
                        Product productss = db.Products.Find(item.Product_ID);

                        details.Product_ID = productss.Prod_ID;
                        var tax = Convert.ToDecimal(db.Taxes.Find(details.Product_Tax).Tax_Percentage);
                        details.Detail_Total_Tax = TotalAfterDiscount * (tax / 100);
                        details.Reverse_Total_Tax = details.Detail_Total_Tax;


                        details.Batch_ID = item.Batch_ID;
                        var price = item.Product_Quantity * item.Product_Price;
                        details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                        details.Reverse_Total_Amount = details.Total_Amount;
                        master.Total_Amount += details.Total_Amount;

                        master.Total_Tax += details.Detail_Total_Tax;

                        master.InvoiceDetails.Add(details);
                    }
                }

                master.TotaL_After_Fixed = master.Total_Amount - master.FixedDiscount;
                master.TotalAmountAfMat = master.TotaL_After_Fixed.Value;
                offermaster.Offer_State = true;
                db.Entry(offermaster).State = EntityState.Modified;
                db.InvoiceMasters.Add(master);
                db.SaveChanges();
                db.Dispose();

                CreateTransactionBatch(master, UserId);

                return master.Master_Id;
            }
            return 0;
        }
        /// <summary>
        /// get All Detail From OfferPrice For That Id
        /// </summary>
        /// <param name="Order"></param>
        /// <returns></returns>
        public static List<OfferPriceDetail> getDetailFromOrder(int Order)
        {
            var db = new AccountingContainer();
            var detail = db.OfferPriceDetails.Where(a => a.OfferPriceMasterMaster_Id == Order).ToList();
            return detail;
        }
        /// <summary>
        /// Get that file by its id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public static SalesFileDetail getFileById(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.SalesFileDetails.Find(fileId);
            return file;
        }
        public static string deleteFile(int fileId)
        {
            var db = new AccountingContainer();
            var file = db.SalesFileDetails.Find(fileId);
            db.SalesFileDetails.Remove(file);
            db.SaveChanges();
            return "تم حذف الملف ";
        }


        /// <summary>
        /// get All Files That Belonge that invoice
        /// </summary>
        /// <param name="invoiceMasterId"></param>
        /// <returns></returns>
        public static List<SalesFileDetail> GetAllFiles(int invoiceMasterId)
        {
            var db = new AccountingContainer();
            var files = db.SalesFileDetails.Where(emp => emp.InvoiceMasterMaster_Id == invoiceMasterId).ToList();
            return files;
        }
        public static InvoiceMaster GetInvoiceBySerial(string Serial)
        {
            var db = new AccountingContainer();
            var Invoice = db.InvoiceMasters.Where(emp => emp.Serial_Number == Serial).FirstOrDefault();
            return Invoice;
        }
        // Creates a new Invoice master and details given a masterVM
        /// <summary>
        /// To do :Check Equ
        /// Create invoice and make Unique Code for That Invoice
        /// </summary>
        /// <param name="masterVM"></param>
        /// <returns></returns>
        public static int AddNewInvoice(MasterVM masterVM, String UserId)
        {
            var db = new AccountingContainer();
            var master = new InvoiceMaster();
            var invoiceMasterTemp = db.InvoiceMasterTemps.Find(masterVM.Master_Id);
            if (masterVM != null)
            {

                // sort the Invoices and get the last insert Invoice sales.
                var lastSerial = db.InvoiceMasters.OrderByDescending(c => c.Master_Id).FirstOrDefault();
                if (lastSerial == null)
                {
                    master.Serial_Number = "SAL-001";
                }
                else
                {
                    //using string substring method to get the number of the last inserted
                    master.Serial_Number = "SAL-" + (Convert.ToInt32(lastSerial.Serial_Number.Substring(4, lastSerial.Serial_Number.Length - 4)) + 1).ToString("D3");
                }
                master.Description = masterVM.Description;
                var comp = db.Companies.Find(masterVM.SelectedCompany_Id);
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
                master.Due_Date = Convert.ToDateTime(masterVM.Invoice_Date).AddDays(masterVM.Payment_Policy);
                master.Notes = masterVM.Notes;
                // master.GL_Date = DateTime.Now;
                master.Customer_Id = masterVM.SelectedCustomerId;
                master.Company_Id = masterVM.SelectedCompany_Id;
                master.Branch_Id = masterVM.Branch_Id;
                var resultCurrency = db.Currencies.FirstOrDefault();
                if (resultCurrency != null)
                {
                    master.Invoice_Currency = resultCurrency.Currency_Type;
                }


                if (masterVM.SelectedStoreId == null || masterVM.SelectedStoreId == 0)
                {
                    masterVM.SelectedStoreId = db.Stores.FirstOrDefault().Store_ID;
                }
                master.Store_ID = masterVM.SelectedStoreId;
                var usedOffer = db.OfferPriceMasters.Find(masterVM.order_Id);
                if (usedOffer == null)
                { }
                else
                {
                    master.Offer_Price_Code = usedOffer.Serial_Number;
                    usedOffer.Offer_State = true;
                    db.Entry(usedOffer).State = EntityState.Modified;
                }



                master.Master_Discount = masterVM.Master_Discount;
                //master.Invoice_State = masterVM.Invoice_State;
                master.Document_Type = masterVM.Document_Type;
                master.Reference = masterVM.Reference;
                master.RevenueAccount_Id = masterVM.RevenueAccount_Id;
                master.Payment_Policy = masterVM.Payment_Policy;
                master.Emp_Id = masterVM.Emp_Id;
                master.Branch_Id = masterVM.Branch_Id;
                master.Department_ID = masterVM.Department_ID;
                master.BussinesUnit_ID = masterVM.BussinesUnit_ID;
                master.Invoice_State = TransactionState.Pending;

                if (masterVM.SubLedger_ID != null && masterVM.SubLedger_ID != 0)
                {

                    master.SubLedger_ID = masterVM.SubLedger_ID;

                }
                else if (masterVM.BussinesUnit_ID != null && masterVM.BussinesUnit_ID != 0)
                {

                    master.SubLedger_ID = masterVM.BussinesUnit_ID;

                }
                else if (masterVM.Department_ID != null && masterVM.Department_ID != 0)
                {

                    master.SubLedger_ID = masterVM.Department_ID;

                }
                else if (masterVM.Branch_Id != null && masterVM.Branch_Id != 0)
                {

                    master.SubLedger_ID = masterVM.Branch_Id;

                }
                else { master.SubLedger_ID = 1; }


                if (masterVM.Files.FirstOrDefault() != null)
                {
                    foreach (var file in masterVM.Files)
                    {
                        var filedetails = new SalesFileDetail();
                        var data = new byte[file.ContentLength];
                        filedetails.FileName = file.FileName;
                        filedetails.ContentLength = file.ContentLength;
                        file.InputStream.Read(data, 0, file.ContentLength);
                        filedetails.FileContent = data;
                        filedetails.FileType = file.ContentType;
                        if (data.Length < 1000000)
                        {
                            master.SalesFileDetails.Add(filedetails);
                        }

                    }
                }

                master.FixedDiscount = masterVM.Fixed_Discount;
                // loop over details in the VM and add them to the master
                if (invoiceMasterTemp.InvoiceDetailsTemps.ToList().Count == 0)
                {
                    return 0;
                }
                var qrMaster = new MasterQRInvoice();
                qrMaster.QrGuid = Guid.NewGuid();
                qrMaster.DetailQRInvoices = new List<DetailQRInvoice>();

                qrMaster.TaxQRInvoices = new List<TaxQRInvoice>();
                foreach (var item in invoiceMasterTemp.InvoiceDetailsTemps.ToList())
                {

                    // Create new detail and map the values from detailVM to it
                    // TODO: Use AutoMapper
                    var details = new InvoiceDetails();
                    details.Details_Id = item.Details_Id;
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
                    master.Tax_ID = (int)item.Product_Tax;
                    if (details.Product_Tax == 0)
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


                        details.Product_Cost = battchh.Product_Cost;

                        details.Detail_Total_Cost = item.Product_Quantity * detailBatch.Product_Cost;
                        details.Batch_ID = item.Batch_ID;
                        var price = item.Product_Quantity * item.Product_Price;
                        details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                        master.Total_Amount += details.Total_Amount;
                        master.Total_Cost += details.Detail_Total_Cost;

                        master.Total_Tax += details.Detail_Total_Tax;
                        details.Reverse_Total_Tax = details.Detail_Total_Tax;
                        details.Reverse_Quantity = details.Product_Quantity;
                        details.Reverse_Total_Amount = details.Total_Amount;
                        master.InvoiceDetails.Add(details);
                        db.InvoiceDetailsTemps.Remove(item);
                    }
                    else
                    {
                        Product productss = db.Products.Find(item.Product_ID);

                        details.Product_ID = productss.Prod_ID;
                        if (productss.Prod_Type == Prod_Type.خدمة)
                        {
                            details.Detail_Total_Cost = 0;
                        }
                        else
                        {


                            details.Detail_Total_Cost = item.Detail_Total_Cost;
                        }
                        details.Batch_ID = item.Batch_ID;
                        var price = item.Product_Quantity * item.Product_Price;
                        details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                        master.Total_Amount += details.Total_Amount;
                        master.Total_Cost += details.Detail_Total_Cost;

                        master.Total_Tax += details.Detail_Total_Tax;
                        details.Reverse_Total_Tax = details.Detail_Total_Tax;
                        details.Reverse_Quantity = details.Product_Quantity;
                        details.Reverse_Total_Amount = details.Total_Amount;
                        master.InvoiceDetails.Add(details);
                        db.InvoiceDetailsTemps.Remove(item);
                        //qr


                    }
                    var invoiceQRDetail = new DetailQRInvoice();
                    invoiceQRDetail.Batch_ID = details.Batch_ID;
                    invoiceQRDetail.Product_Id = details.Product_ID;
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
                master.TotalNewTaxs = 0;
                if (masterVM.Taxes != null)
                {
                    foreach (var tax in masterVM.Taxes)
                    {
                        var taxo = new InvoiceTax();
                        taxo.Tax_Id = tax;

                        taxo.Amount = (db.Taxes.Find(Convert.ToInt32(tax)).Tax_Percentage / 100) * master.InvoiceDetails.Select(a => a.Total_Amount).Sum();
                        master.TotalNewTaxs += taxo.Amount;

                        master.InvoiceTaxes.Add(taxo);
                        //qr Taxes
                        var qrtax = new TaxQRInvoice();
                        qrtax.Tax_Id = tax;

                        qrtax.Amount = taxo.Amount;
                        qrMaster.TaxQRInvoices.Add(qrtax);

                    }
                }

                // The total amount after matching is initialized to the total amount
                master.TotaL_After_Fixed = (master.Total_Amount + master.TotalNewTaxs) - master.FixedDiscount;
                master.TotalAmountAfMat = master.TotaL_After_Fixed.Value;


                //add QR Invoice
                qrMaster.AllowDiscount = master.FixedDiscount.Value;
                qrMaster.ApprovalCode = master.Reference;
                qrMaster.Company_Id = master.Company_Id;
                qrMaster.Customer_Id = master.Customer_Id;
                qrMaster.Description = master.Description;
                qrMaster.Employee_Id = master.Emp_Id.Value;
                qrMaster.Invoice_Date = master.Invoice_Date;
                qrMaster.Invoice_State = master.Invoice_State;
                qrMaster.InvoiceType_Id = 1;
                qrMaster.Master_Discount = master.Master_Discount;

                qrMaster.Reference = master.Reference;
                qrMaster.Serial_Number = master.Serial_Number;
                qrMaster.Store_ID = master.Store_ID;
                qrMaster.Subledger_Id = master.SubLedger_ID;
                qrMaster.Notes = master.Notes;
                qrMaster.TotalNewTaxs = master.TotalNewTaxs.Value;
                qrMaster.Account_Id = master.RevenueAccount_Id;
                qrMaster.Total_Amount = master.Total_Amount;
                qrMaster.InvoiceType = "فاتورة مبيعات اجل";
                qrMaster.Total_Payment_Method_Cost = 0;
                qrMaster.Total_Tax = master.Total_Tax;
                qrMaster.Branch_Id = master.Branch_Id;
                qrMaster.Department_ID = master.Department_ID;
                qrMaster.BussinesUnit_ID = master.BussinesUnit_ID;


                qrMaster.Total_Cost = master.Total_Cost;
                qrMaster.Notes = master.Notes;
                qrMaster.Number = InvoiceSerial.generateSerialForInvoice();
                qrMaster.DateOfCreate = DateTime.Now;
                master.Serial_Number = "SAL-" + System.Text.RegularExpressions.Regex.Match(qrMaster.Number, @"\d+").Value;
                qrMaster.Serial_Number = master.Serial_Number;
                db.MasterQRInvoices.Add(qrMaster);
              
                master.Number = qrMaster.Number;
                db.SaveChanges();
                master.QRNumber = qrMaster.Id;
              
                db.InvoiceMasters.Add(master);

                db.InvoiceMasterTemps.Remove(invoiceMasterTemp);
                db.SaveChanges();
                db.Dispose();

                CreateTransactionBatch(master, UserId);

                return master.Master_Id;
            }
            return 0;
        }



        // Gets the ViewModel used for editing the InvoiceMaster 
        public static MasterVM GetEditViewModel(int id)
        {
            InvoiceMaster invoice = GetInvoiceDetailsById(id);
            MasterVM invoiceMasterVM = new MasterVM();
            invoiceMasterVM.Invoice_State = invoice.Invoice_State;
            return invoiceMasterVM;
        }
        // Updates the invoice state (Post, Void) and Generates GLs for it depending on the new state
        public static String UpdateInvoiceMaster(MasterVM invoiceMasterVM)
        {
            var db = new AccountingContainer();

            InvoiceMaster master = db.InvoiceMasters.Find(invoiceMasterVM.Master_Id);
            OperationRun.Addnewoperation(master.Serial_Number);
            var operation = db.OperationRuns.Where(a => a.Code == master.Serial_Number).Count();
            if (operation != 1)
            {
                OperationRun.DeleteOperation(master.Serial_Number);
                return Resources.InvoiceMasterDetails.WaitingMSGAndTryAgain;
            }
            if (master.Invoice_State == invoiceMasterVM.Invoice_State)
            {
                OperationRun.DeleteOperation(master.Serial_Number);
                return Resources.InvoiceMasterDetails.ItsDoneWorking + " " + master.Invoice_State + " " + Resources.InvoiceMasterDetails.Before;
            }
            else
            {
                master.Invoice_State = invoiceMasterVM.Invoice_State;
            }


            checkproducts(invoiceMasterVM.Master_Id);



            // Create GL for the invoice 
            var Mssg = CreateInvoiceGL(master, db);
            db.Entry(master).State = EntityState.Modified;

            db.SaveChanges();
            OperationRun.DeleteOperation(master.Serial_Number);
            db.Dispose();
            return Mssg;
        }
        /// <summary>
        /// check that product is Exist in that Store and Invoice Or not
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string checkproducts(int id)
        {
            var db = new AccountingContainer();
            InvoiceMaster master = db.InvoiceMasters.Find(id);
            // Edit master invoice state to the new state


            var thisstore = db.Stores.Find(master.Store_ID);
            var thisPatch = db.ProductBatches.Where(e => e.Store_ID == thisstore.Store_ID).ToList();

            foreach (var detail in master.InvoiceDetails)
            {
                foreach (var batchitem in thisPatch)
                {
                    if (batchitem.Product_ID == detail.Product_ID)
                    {
                        detail.Batch_ID = batchitem.Batch_ID;
                        db.Entry(detail).State = EntityState.Modified;
                        //db.SaveChanges();
                    }
                    else
                    {

                    }
                }

            }
            return "";
        }
        // Deletes the master invoice and its details given the id
        public static InvoiceMaster DeleteInvoiceMaster(int invoiceId)
        {
            var db = new AccountingContainer();
            InvoiceMaster invoice = db.InvoiceMasters.Find(invoiceId);

            if (invoice.Invoice_State != TransactionState.Pending)
                return null;
            db.InvoiceDetails.RemoveRange(invoice.InvoiceDetails);
            db.SalesFileDetails.RemoveRange(invoice.SalesFileDetails);
            db.InvoiceTaxes.RemoveRange(invoice.InvoiceTaxes);
            db.InvoiceMasters.Remove(invoice);
            var DeletQR = db.MasterQRInvoices.Where(a => a.Serial_Number == invoice.Serial_Number).FirstOrDefault();
            db.DetailQRInvoices.RemoveRange(DeletQR.DetailQRInvoices);
            db.QRPaymentMethods.RemoveRange(DeletQR.QRPaymentMethods);
            db.TaxQRInvoices.RemoveRange(DeletQR.TaxQRInvoices);
            db.MasterQRInvoices.Remove(DeletQR);


            db.SaveChanges();
            db.Dispose();
            return invoice;
        }

        // Gets the Products available in a store
        public static SelectList getStoreProducts(int storeId)
        {
            var db = new AccountingContainer();
            var store = db.Stores.Find(storeId);

            List<Product> MyProduct = new List<Product>();

            //foreach (var StoreProduct in store.StoreProducts)
            //{
            //    MyProduct.Add(StoreProduct.Product);
            //}
            //var products = store.StoreProducts.Join(db.Products,
            //    storeProduct => storeProduct.Product_ID,
            //    product => product.Prod_ID,
            //    (sp, p) => new { Product = p });

            SelectList Products = new SelectList(MyProduct, "Prod_ID", "Prod_Name");
            return Products;
        }
        // Creates new TransactionBatch using the master invoice instance
        public static void CreateTransactionBatch(InvoiceMaster master, String UserId)
        {
            var trans = new TransactionBatch();
            trans.Description = master.Description;
            trans.Date = master.Invoice_Date;
            trans.Operation_Code = master.Serial_Number;
            trans.OperationName = "مبيعات اجل";
            // TODO: Edit UserId
            trans.UserId = UserId;
            trans.OperationId = master.Master_Id;
            TransactionBatch.AddNewTransaction(trans);
        }
        // Creats Gl master and details given the master invoice and AccountingContainer
        public static String CreateInvoiceGL(InvoiceMaster master, AccountingContainer db)
        {
            //var db = new AccountingContainer();
            var msg = "";
            // Create GL Master and initialize it from the master invoice
            GLMaster glMaster = new GLMaster();
            glMaster.GL_Date = master.Invoice_Date;
            glMaster.GL_Desc = master.Description;
            glMaster.Company_Id = master.Company_Id;
            glMaster.GLCode = master.Serial_Number;
            glMaster.Auto = true;
            // TODO: Edit GLType

            glMaster.GLType_Id = "مبيعات أجل";
            // TODO: Edit currency
            glMaster.Currency_Id = "الريال السعودى";
            List<ProuctCostUtilites> ProductCostList = new List<ProuctCostUtilites> { };
            // Initialize GLState to Pending when first created
            glMaster.GLState = TransactionState.Pending;

            // Create five GlDetail
            GLDetail AccountReceivableGL = new GLDetail(),
                revenueGL = new GLDetail(),

                inventoryGL = new GLDetail(),
                totalCostGL = new GLDetail(),
            discount = new GLDetail(),
            fixedDiscount = new GLDetail();
            // Initialize common values
            // TODO: Fix the following values
            //var BusId = db.BussinesUnits.FirstOrDefault().BU_Id;
            AccountReceivableGL.Bu_Id = 0;
            revenueGL.Bu_Id = 0;
            inventoryGL.Bu_Id = 0;
            totalCostGL.Bu_Id = 0;
            discount.Bu_Id = 0;
            fixedDiscount.Bu_Id = 0;
            var ChartOfAccounRecive = db.ChartOfAccountings.Find(37);
            AccountReceivableGL.Account_Id = ChartOfAccounRecive.Account_Id;
            AccountReceivableGL.Account_Name = ChartOfAccounRecive.Account_Name;
            if (master.RevenueAccount_Id > 0)
            {

            }
            else { master.RevenueAccount_Id = 76; }
            var ChartOfAccounRevenue = db.ChartOfAccountings.Find(master.RevenueAccount_Id);
            revenueGL.Account_Id = ChartOfAccounRevenue.Account_Id;
            revenueGL.Account_Name = ChartOfAccounRevenue.Account_Name;
            //we  neeed  menatin here because  of  (OfferPricee Taxe  is  not  COming  As  ID  ...it  is  coming  as Value Total Tax)
            // var taxId = db.InvoiceDetails.FirstOrDefault().Product_Tax;
            // var tax = db.Taxes.Find(49).Tax_Code;





            var DiscountCost = db.ChartOfAccountings.Find(78);
            discount.Account_Id = DiscountCost.Account_Id;
            discount.Account_Name = DiscountCost.Account_Name;
            discount.SubLedger_Id = master.SubLedger_ID;



            var store = db.Stores.Find(master.Store_ID);
            var ChartOfAccounInventory = db.ChartOfAccountings.Where(account => account.Account_Code == store.Store_Code).First();
            inventoryGL.Account_Id = ChartOfAccounInventory.Account_Id;
            inventoryGL.Account_Name = ChartOfAccounInventory.Account_Name;

            var ChartOfAccounCost = db.ChartOfAccountings.Find(105);
            totalCostGL.Account_Id = ChartOfAccounCost.Account_Id;
            totalCostGL.Account_Name = ChartOfAccounCost.Account_Name;


            var chartFixedDiscount = db.ChartOfAccountings.Find(184);
            fixedDiscount.Account_Id = chartFixedDiscount.Account_Id;
            fixedDiscount.Account_Name = chartFixedDiscount.Account_Name;
            //if (master.Customer.Customer_Code.Contains('.'))
            //{
            //   var read = master.Customer.Customer_Code.Replace('.', ' ');
            //    read.Trim();
            //    AccountReceivableGL.SubLedger_Id =
            //    revenueGL.SubLedger_Id =
            //    taxesGL.SubLedger_Id =
            //    totalCostGL.SubLedger_Id = int.Parse(read);

            //}

            AccountReceivableGL.Asset_Id =
            revenueGL.Asset_Id =

            inventoryGL.Asset_Id =
            discount.Asset_Id =
            totalCostGL.Asset_Id = 0;
            var cust = db.Customers.Find(master.Customer_Id);
            Accounts customerAccount = db.Accounts.Where(subled => subled.Account_Code == cust.Customer_Code).FirstOrDefault();
            AccountReceivableGL.Account_Child_ID = customerAccount.Accounts_Id;
            revenueGL.Account_Child_ID = 0;
            revenueGL.SubLedger_Id = master.SubLedger_ID;

            totalCostGL.Account_Child_ID = 0;
            totalCostGL.SubLedger_Id = master.SubLedger_ID;
            fixedDiscount.SubLedger_Id = master.SubLedger_ID;
            AccountReceivableGL.Account_CHild_Name = customerAccount.Account_Name;
            var RealDetailList = getMaterialOCompoundProduct(master.InvoiceDetails.ToList(), master.Store_ID);

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // store has no subledger only account
                    //inventoryGL.SubLedger_Id = int.Parse(master.Store.Store_Code);

                    // Check for new master invoice state whether Post or Void 
                    switch (master.Invoice_State)
                    {
                        
                        //Post Invoice
                        case TransactionState.Post:
                            master.Total_Cost = RealDetailList.Select(a => a.Detail_Total_Cost).Sum();
                            glMaster.GLState = TransactionState.Post;
                            var gl = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode ).FirstOrDefault();
                    
                            if (gl != null&&gl.GLState==TransactionState.Post)
                            {
                                return Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade;
                            }
                            else if (gl != null && gl.GLState == TransactionState.Hold)
                            {
                                return CreateInvoiceGLHolding(master, db, gl,transaction);

                            }
                            // Customer GL
                            AccountReceivableGL.GLDetail_Debit = master.TotaL_After_Fixed.Value;
                            // Revenue GL
                            //revenueGL.GLDetail_Credit = master.Total_Amount - master.Total_Tax;
                            foreach (var detail in master.InvoiceDetails)
                            {
                                var TotalBefore = detail.Product_Price * detail.Product_Quantity;
                                revenueGL.GLDetail_Credit += TotalBefore;
                            }
                            discount.GLDetail_Debit = revenueGL.GLDetail_Credit * (master.Master_Discount / 100);
                            // Taxes GL



                            //For AssetDiscountGL
                            fixedDiscount.GLDetail_Debit = master.FixedDiscount.Value;

                            // Update store quantity
                            db.ProductBatches.Load();
                            var errorMessage = new List<string> { };
                            foreach (var detail in RealDetailList)
                            {
                                var thisproduct = db.Products.Find(detail.Product_ID);
                                if (thisproduct.Prod_Type == Prod_Type.منتج || thisproduct.Prod_Type == Prod_Type.اولي)
                                {
                                    ProductBatches detailBatch = db.ProductBatches.Where(a => a.Product_ID == detail.Product_ID && a.Store_ID == master.Store_ID).ToList().FirstOrDefault();
                                    try
                                    {
                                        if (detailBatch.Store_ID != null)
                                        {
                                            if (detail.Product_Quantity > detailBatch.Product_Quantity)
                                            {
                                                var productName = db.Products.Find(detail.Product_ID).Prod_Name;
                                                master.Invoice_State = TransactionState.Hold;
                                                //db.SaveChanges();
                                                errorMessage.Add(Resources.InvoiceMasterDetails.NotEnoughProductInTheInventory + " " + productName);

                                            }
                                        }
                                    }
                                    catch
                                    {
                                        var productName = db.Products.Find(detail.Product_ID).Prod_Name;
                                        master.Invoice_State = TransactionState.Hold;
                                        glMaster.GLState = TransactionState.Hold;
                                        //db.SaveChanges();
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
                                    var thisproduct = db.Products.Find(detail.Product_ID);
                                    if (thisproduct.Prod_Type == Prod_Type.منتج || thisproduct.Prod_Type == Prod_Type.اولي)
                                    {
                                        ProductBatches detailBatch = db.ProductBatches.Where(a => a.Product_ID == detail.Product_ID && a.Store_ID == master.Store_ID).ToList().LastOrDefault();
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
                                    var thisproduct = db.Products.Find(detail.Product_ID);
                                    if (thisproduct.Prod_Type == Prod_Type.منتج || thisproduct.Prod_Type == Prod_Type.اولي)
                                    {
                                        ProductBatches detailBatch = db.ProductBatches.Where(a => a.Product_ID == detail.Product_ID && a.Store_ID == master.Store_ID).ToList().FirstOrDefault();
                                        if (detailBatch.Store_ID != null)
                                        {

                                            if (detailBatch.Product_Cost == 0)
                                            {
                                                var LastBatch = ProductCostList.Where(a => a.Product_ID == detail.Product_ID).FirstOrDefault();
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
                                            storecard.Product_Id = detail.Product_ID;
                                            storecard.IncreaseQuantity = false;
                                            storecard.TypeOfDocument = "فاتورة مبيعات أجل ";
                                            storecard.Document_Serial = master.Serial_Number;
                                            storecard.TotalCost = detail.Product_Quantity * detail.Product_Cost.Value;
                                            storecard.TotalAmount = detail.Total_Amount;
                                            storecard.Account_Id = customerAccount.Accounts_Id;
                                            storecard.Subledger_Id = master.SubLedger_ID;
                                            db.StoreCards.Add(storecard);
                                            //Store.AddinginstorCard(storecard);

                                            inventoryGL.GLDetail_Credit += storecard.TotalCost.Value;
                                            // Total Cost GL
                                            totalCostGL.GLDetail_Debit += storecard.TotalCost.Value;

                                        }
                                    }
                                }
                            
                            }

                            //db.Entry(master).State = EntityState.Modified;
                            // Inventory GL
                           
                            if (master.InvoiceTaxes.FirstOrDefault() != null)
                            {
                                foreach (var tax in master.InvoiceTaxes)
                                {
                                    var gldetailTax = new GLDetail();
                                    var taxs = db.Taxes.Find(Convert.ToInt32(tax.Tax_Id));
                                    var AccountTax = db.ChartOfAccountings.Where(a => a.Account_Code == taxs.Tax_Code).FirstOrDefault();
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
                            glMaster.GLType_Id = "مبيعات أجل عكسي";
                            glMaster.GLState = TransactionState.Void;
                            var glcheck = db.GLMasters.Where(a => a.GLCode == glMaster.GLCode && a.GLState == glMaster.GLState).FirstOrDefault();
                            if (glcheck != null)
                            {
                                return Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade;
                            }

                            var reverse = db.ReverseSalesInvoiceMasters.Where(a => a.InvoiceMaster_Id == master.Master_Id).ToList();
                            if (reverse.FirstOrDefault() != null)
                            {
                                msg = Resources.InvoiceMasterDetails.InvoiceCouldNotBeVoid;
                                InvoiceMaster salesmaster = db.InvoiceMasters.Find(master.Master_Id);
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
                                //db.SaveChanges();
                                return msg;

                            }
                            else
                            {
                                return MakeVoidForSales(master.Master_Id);
                            }
                            break;
                        default:
                            break;
                    }

                    if (msg == "")
                    {

                        msg = Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade + " ";
                    }
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
                        if (fixedDiscount.GLDetail_Credit != 0 || fixedDiscount.GLDetail_Debit != 0)
                        {
                            glMaster.GLDetails.Add(fixedDiscount);
                        }
                        if (inventoryGL.GLDetail_Credit != 0 || inventoryGL.GLDetail_Debit != 0)
                        {
                            glMaster.GLDetails.Add(inventoryGL);
                            glMaster.GLDetails.Add(totalCostGL);
                        }
                        glMaster = addglDetailTax(glMaster, master.Master_Id);
                        db.GLMasters.Add(glMaster);

                        GLMaster.GetBalanceForAccount(glMaster);
                        GLMaster.GetBalanceForSubLedger(glMaster);
                        db.SaveChanges();

                        transaction.Commit();
                    
                }
                catch (Exception exp)
                {
                    transaction.Rollback();
                    db.Dispose();
                }
                //db.SaveChanges();
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

        public static string CreateInvoiceGLHolding(InvoiceMaster master, AccountingContainer db,GLMaster gl,DbContextTransaction transaction)
        {
            var RealDetailList = getMaterialOCompoundProduct(master.InvoiceDetails.ToList(), master.Store_ID);
            List<ProuctCostUtilites> ProductCostList = new List<ProuctCostUtilites> { };
            var cust = db.Customers.Find(master.Customer_Id);
            if (gl.GLDetails.Where(a => a.Account_Id == 105).FirstOrDefault() != null)
            {
                return Resources.validate.GLDone;
            }
            Accounts customerAccount = db.Accounts.Where(subled => subled.Account_Code == cust.Customer_Code).FirstOrDefault();
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
                var thisproduct = db.Products.Find(detail.Product_ID);
                if (thisproduct.Prod_Type == Prod_Type.منتج || thisproduct.Prod_Type == Prod_Type.اولي)
                {
                    ProductBatches detailBatch = db.ProductBatches.Where(a => a.Product_ID == detail.Product_ID && a.Store_ID == master.Store_ID).ToList().FirstOrDefault();
                    try
                    {
                        if (detailBatch.Store_ID != null)
                        {
                            if (detail.Product_Quantity > detailBatch.Product_Quantity)
                            {
                                var productName = db.Products.Find(detail.Product_ID).Prod_Name;
                                master.Invoice_State = TransactionState.Hold;
                                //db.SaveChanges();
                                errorMessage.Add(Resources.InvoiceMasterDetails.NotEnoughProductInTheInventory + " " + productName);
                            
                            }
                        }
                    }
                    catch
                    {
                        var productName = db.Products.Find(detail.Product_ID).Prod_Name;
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
                var thisproduct = db.Products.Find(detail.Product_ID);
                if (thisproduct.Prod_Type == Prod_Type.منتج || thisproduct.Prod_Type == Prod_Type.اولي)
                {
                    ProductBatches detailBatch = db.ProductBatches.Where(a => a.Product_ID == detail.Product_ID && a.Store_ID == master.Store_ID).ToList().LastOrDefault();
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
                var thisproduct = db.Products.Find(detail.Product_ID);
                if (thisproduct.Prod_Type == Prod_Type.منتج || thisproduct.Prod_Type == Prod_Type.اولي)
                {
                    ProductBatches detailBatch = db.ProductBatches.Where(a => a.Product_ID == detail.Product_ID && a.Store_ID == master.Store_ID).ToList().FirstOrDefault();
                    if (detailBatch.Store_ID != null)
                    {

                        if (detailBatch.Product_Cost == 0)
                        {
                            var LastBatch = ProductCostList.Where(a => a.Product_ID == detail.Product_ID).FirstOrDefault();
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
                        storecard.Product_Id = detail.Product_ID;
                        storecard.IncreaseQuantity = false;
                        storecard.TypeOfDocument = "فاتورة مبيعات أجل ";
                        storecard.Document_Serial = master.Serial_Number;
                        storecard.TotalCost = detail.Product_Quantity * detail.Product_Cost.Value;
                        storecard.TotalAmount = detail.Total_Amount;
                        storecard.Account_Id = customerAccount.Accounts_Id;
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
            transaction.Commit();
            return Resources.InvoiceMasterDetails.RecordHasBeenSuccessfullyMade + " ";
            

        }

        public static List<InvoiceDetails> getMaterialOCompoundProduct(List<InvoiceDetails> Details, int Store_Id)
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
                List<InvoiceDetails> Details2 = new List<InvoiceDetails>();

                foreach (var item in ProList)
                {
                    var SubProductList = ProductAll.Where(a => a.Prod_ID == item.Product_ID).FirstOrDefault().ProductCompounds;
                    foreach (var subProduct in SubProductList)
                    {
                        var dublicate = Details.Where(a => a.Product_ID == subProduct.Product_Id).FirstOrDefault();
                        var batch = db.ProductBatches.Where(a => a.Product_ID == subProduct.Product_Id && a.Store_ID == Store_Id).FirstOrDefault();
                        if (dublicate == null)
                        {
                            var Detailnew = new InvoiceDetails();
                            Detailnew.Product_ID = subProduct.Product_Id;
                            Detailnew.Product_Quantity = subProduct.Quantity * item.Product_Quantity;
                            Detailnew.Reverse_Quantity = subProduct.Quantity * item.Product_Quantity;
                            if (batch != null)
                            {
                                Detailnew.Batch_ID = batch.Batch_ID;
                            }
                            if (batch != null && batch.Product_Quantity > 0)
                            {
                                Detailnew.Detail_Total_Cost = (batch.Batch_Cost / batch.Product_Quantity) * Detailnew.Product_Quantity;
                                Detailnew.Product_Cost = (batch.Batch_Cost / batch.Product_Quantity);
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


                            var Detailnew = new InvoiceDetails();
                            Detailnew.Product_ID = subProduct.Product_Id;
                            Detailnew.Product_Quantity = dublicate.Product_Quantity + (item.Product_Quantity * subProduct.Quantity);
                            Detailnew.Reverse_Quantity = Detailnew.Product_Quantity;
                            if (batch != null)
                            {
                                Detailnew.Batch_ID = batch.Batch_ID;
                            }
                            if (batch != null && batch.Product_Quantity > 0)
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
                        var Delete = Details.Where(a => a.Product_ID == subProduct.Master_Product_Id).FirstOrDefault();

                        Details.Remove(Delete);

                    }





                }

            }

            return Details;
        }
        public static GLMaster addglDetailTax(GLMaster gLMaster, int invoiceId)
        {
            var db = new AccountingContainer();
            var invoiceDetail = db.InvoiceMasters.Find(invoiceId).InvoiceDetails;
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
                if (gLMaster.GLState == TransactionState.Post||gLMaster.GLState==TransactionState.Hold)
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
        /// prepare invoice For Editing and -
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public static MasterVM fillForEditingInvoice(int invoice, int isCopy)
        {
            var db = new AccountingContainer();
            var invoiceMast = db.InvoiceMasters.Find(invoice);
            var revesre = db.ReverseSalesInvoiceMasters.Where(a => a.InvoiceMaster_Id == invoice).FirstOrDefault();
            if (revesre != null && isCopy != 1)
            {
                return null;
            }
            if (invoiceMast.Invoice_State != TransactionState.Pending && isCopy != 1)
            {
                return null;
            }
            var InvoiceVM = new MasterVM();
            InvoiceVM.isCopy = isCopy;

            String StartCodeOfrevenue = "04";

            InvoiceVM.Notes = invoiceMast.Notes;

            var comp = db.Companies.Where(c => c.Comp_Id == invoiceMast.Company_Id);
            if (comp.Any())
            {
                InvoiceVM.SelectedCompany_Id = comp.FirstOrDefault().Comp_Id;
            }

            var cur = db.Currencies.Where(b => b.Currency_Type == invoiceMast.Invoice_Currency);
            if (cur.Any())
            {
                InvoiceVM.SelectedCurrencyId = cur.FirstOrDefault().Currency_Id;
            }
            var cust = db.Customers.Where(b => b.Customer_Id == invoiceMast.Customer_Id);
            if (cust.Any())
            {
                InvoiceVM.SelectedCustomerId = cust.FirstOrDefault().Customer_Id;
            }
            var stor = db.Stores.Where(Cu => Cu.Store_ID == invoiceMast.Store_ID);
            if (stor.Any())
            {
                InvoiceVM.SelectedStoreId = stor.FirstOrDefault().Store_ID;
            }

            //var branch = db.Branches.Where(Cu => Cu.Branch_Id == invoiceMast.Branch_Id);
            //if (branch.Any())
            //{
            //    InvoiceVM.Branch_Id = branch.FirstOrDefault().Branch_Id;
            //}
            var Emp = db.Employees.Where(Cu => Cu.Emp_Id == invoiceMast.Emp_Id);
            if (Emp.Any())
            {
                InvoiceVM.Emp_Id = Emp.FirstOrDefault().Emp_Id;
            }

            //var lista = new List<ChartOfAccounting> { };
            //foreach (var k in db.ChartOfAccountings)
            //{
            //    var parent = db.ChartOfAccountings.Where(a => a.ChartOfAccountingAccount_Id == k.Account_Id);
            //    var o = parent.FirstOrDefault();
            //    if (o == null)
            //    {
            //        lista.Add(db.ChartOfAccountings.Find(k.Account_Id));

            //    }


            //}
            var newList = (from custo in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                           select new
                           {
                               Id = custo.Customer_Id,
                               Name = custo.Customer_Name + "        " + custo.Customer_Phone

                           }).ToList();
            InvoiceVM.MyCustomerList = new SelectList(newList, "Id", "Name");
            var thistaxinchart = db.ChartOfAccountings.Find(48);
            //List<object> anewList = new List<object>();
            var anewList = (from Tax in db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).Where(a => a.IsActive != false)
                            select new
                            {

                                Id = Tax.Tax_ID,
                                Name = Tax.Tax_Name + "   " + Tax.Tax_Percentage + "%"

                            }).ToList();
            var lista = db.ChartOfAccountings.Where(a => a.ChartOfAccountings.Count() == 0 && a.isActive != false).ToList();
            InvoiceVM.AccountList = new SelectList(lista.Where(a => a.Account_Code.StartsWith(StartCodeOfrevenue)), " Account_Id", "Account_Name");
            InvoiceVM.MyCompanyList = new SelectList(db.Companies, "Comp_Id", "Comp_Name");
            InvoiceVM.MyCurrencyList = new SelectList(db.Currencies, "Currency_Id", "Currency_Type");


            InvoiceVM.TaxList = new SelectList(anewList, "Id", "Name");
            InvoiceVM.Taxes = invoiceMast.InvoiceTaxes.Select(a => a.Tax_Id).ToList();

            InvoiceVM.MyCustomerList = new SelectList(newList, "Id", "Name");
            InvoiceVM.MyStoresList = new SelectList(db.Stores.Where(a => a.isActive != false), "Store_ID", "Store_Name");
            InvoiceVM.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            if (invoiceMast.Branch_Id > 0)
            {
                InvoiceVM.DepList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == invoiceMast.Branch_Id && p.isActive != false && p.Subled_Level == 2), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                InvoiceVM.DepList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.isActive != false && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
                InvoiceVM.BUList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.isActive != false && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            }
            if (invoiceMast.Department_ID > 0)
            {
                InvoiceVM.BUList = new SelectList(db.SubLedgers.Where(p => p.SubLedgerSubLed_Id == invoiceMast.Department_ID && p.isActive != false), "SubLed_Id", "SubLed_Name");
            }
            else
            {
                InvoiceVM.BUList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.SubLed_Type == "نشاط لقسم" && a.isActive != false), "SubLed_Id", "SubLed_Name");

            }
            //InvoiceVM.BranchList = new SelectList(db.SubLedgers.Where(s => s.Subled_Level == 1), "SubLed_Id", "SubLed_Name");
            InvoiceVM.EmpList = new SelectList(db.Employees.Where(e => e.Reg_ID != null), "Emp_Id", "Emp_Name");
            //InvoiceVM.DepList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 2 && a.SubLed_Type == "قسم لفرع"), "SubLed_Id", "SubLed_Name");
            //InvoiceVM.BUList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 3 && a.SubLed_Type == "نشاط لقسم"), "SubLed_Id", "SubLed_Name");
            InvoiceVM.SubList = new SelectList(db.SubLedgers.Where(a => a.SubLed_Type == "مركز تكلفة يدوى" && a.isActive != false), "SubLed_Id", "SubLed_Name");
            InvoiceVM.BranchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            InvoiceVM.OrderList = new SelectList(db.OfferPriceMasters.Where(e => e.Offer_State == false), "OfferMas_Id", "Serial_Number");
            InvoiceVM.Document_Type = Document_Type.Sales_Invoice;
            InvoiceVM.Description = invoiceMast.Description;
            InvoiceVM.Due_Date = Convert.ToString(invoiceMast.Due_Date);
            InvoiceVM.Tax_ID = invoiceMast.Tax_ID;
            InvoiceVM.GL_Date = Convert.ToString(invoiceMast.GL_Date);
            InvoiceVM.Invoice_Date = Convert.ToString(invoiceMast.Invoice_Date);
            InvoiceVM.Invoice_State = TransactionState.Pending;
            InvoiceVM.Master_Discount = invoiceMast.Master_Discount;
            InvoiceVM.Master_Id = invoiceMast.Master_Id;
            InvoiceVM.Serial_Number = invoiceMast.Serial_Number;
            InvoiceVM.Fixed_Discount = invoiceMast.FixedDiscount.Value;
            InvoiceVM.Reference = invoiceMast.Reference;
            InvoiceVM.RevenueAccount_Id = invoiceMast.RevenueAccount_Id;
            InvoiceVM.Payment_Policy = invoiceMast.Payment_Policy.Value;
            InvoiceVM.Emp_Id = invoiceMast.Emp_Id.Value;
            InvoiceVM.Branch_Id = invoiceMast.Branch_Id;
            InvoiceVM.Department_ID = invoiceMast.Department_ID;
            InvoiceVM.BussinesUnit_ID = invoiceMast.BussinesUnit_ID;
            InvoiceVM.Fixed_Discount = invoiceMast.FixedDiscount.Value;
            //   InvoiceVM.myDetailsVM = invoiceMast.InvoiceDetails.ToList();
            var invoiceTemp = new InvoiceMasterTemp();
            invoiceTemp.Company_Id = InvoiceVM.SelectedCompany_Id;
            invoiceTemp.Customer_Id = InvoiceVM.SelectedCustomerId;
            invoiceTemp.Invoice_Currency = Convert.ToString(InvoiceVM.SelectedCurrencyId);
            invoiceTemp.Description = InvoiceVM.Description;
            invoiceTemp.Document_Type = (int)invoiceMast.Document_Type;
            invoiceTemp.Due_Date = invoiceMast.Due_Date;
            invoiceTemp.GL_Date = invoiceMast.GL_Date;
            invoiceTemp.Invoice_Date = invoiceMast.Invoice_Date;
            invoiceTemp.Invoice_State = (int)invoiceMast.Invoice_State;
            invoiceTemp.Master_Discount = invoiceMast.Master_Discount;


            invoiceTemp.Reference = invoiceMast.Reference;
            invoiceTemp.RevenueAccount_Id = invoiceMast.RevenueAccount_Id;
            invoiceTemp.Payment_Policy = invoiceMast.Payment_Policy.Value;
            invoiceTemp.Emp_Id = invoiceMast.Emp_Id;
            invoiceTemp.Branch_Id = invoiceMast.Branch_Id;


            invoiceTemp.Serial_Number = invoiceMast.Serial_Number;
            invoiceTemp.Store_ID = InvoiceVM.SelectedStoreId;
            invoiceTemp.TotalAmountAfMat = invoiceMast.TotalAmountAfMat;
            invoiceTemp.TotalNewTaxs = invoiceMast.TotalNewTaxs;
            invoiceTemp.Total_Amount = invoiceMast.Total_Amount;
            invoiceTemp.Total_Cost = invoiceMast.Total_Cost;
            invoiceTemp.Total_Tax = invoiceMast.Total_Tax;
            var ThereORN = db.InvoiceMasterTemps.Find(invoiceMast.Master_Id);
            if (ThereORN != null)
            {

            }
            else
            {
                invoiceTemp.Master_Id = invoiceMast.Master_Id;
                invoiceTemp.AllowDiscount = invoiceMast.FixedDiscount.Value;

                db.InvoiceMasterTemps.Add(invoiceTemp);
            }
            if (isCopy == 1)
            {
                InvoiceVM.Invoice_Date = DateTime.Now.ToString("dd-MM-yyyy   HH:mm:ss");
            }
            else
            {
                InvoiceVM.Invoice_Date = invoiceMast.Invoice_Date.ToString("dd-MM-yyyy   HH:mm:ss");
            }

            InvoiceVM.Due_Date = invoiceMast.Due_Date.Value.ToString("dd-MM-yyyy   HH:mm:ss");

            db.SaveChanges();

            return InvoiceVM;


        }

        /// <summary>
        /// get detail of invoice by using the id of master
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns></returns>
        public static List<InvoiceDetails> getAlldetailInInvoice(int masterId)
        {
            var db = new AccountingContainer();
            var detail = db.InvoiceDetails.Where(a => a.InvoiceMaster_Id == masterId).ToList();

            return detail;



        }
        /// <summary>
        /// get that invoice by using its id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static InvoiceMaster GetpurchInvByID(int ID)
        {
            var db = new AccountingContainer();
            var invoiceMaster = db.InvoiceMasters.Where(c => c.Master_Id == ID);
            var invoice = new InvoiceMaster();

            if (invoiceMaster != null)
            {
                invoice = invoiceMaster.FirstOrDefault();
                return invoice;
            }
            return null;
        }
        /// <summary>
        /// get the data of detail in detail form meaning prepare for 
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns></returns>
        public static InvoiceMasterDetailsVM PreparpurchMasterDetail2(int masterId)
        {

            var db = new AccountingContainer();
            var invoiceMasterDVM = new InvoiceMasterDetailsVM();

            var thisdetails = GetAllTempPURCHInvoiceDetailsDetails(masterId).ToList().FirstOrDefault();

            var product = thisdetails.Product_ID;


            invoiceMasterDVM.Product_Quantity = thisdetails.Product_Quantity;
            //   invoiceMasterDVM.SelectedBtachtId = thisdetails.Product_ID;
            invoiceMasterDVM.Details_Id = thisdetails.Details_Id;
            invoiceMasterDVM.Product_Price = thisdetails.Product_Price;
            invoiceMasterDVM.Product_Tax = thisdetails.Product_Tax;
            invoiceMasterDVM.SelectedBtachtId = db.ProductBatches.Where(a => a.Product_ID == thisdetails.Product_ID).FirstOrDefault().Batch_ID;
            invoiceMasterDVM.Detail_Total_Tax = thisdetails.Detail_Total_Tax;
            invoiceMasterDVM.InvoiceMaster_Id = masterId;
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

            invoiceMasterDVM.MyBatchsList = new SelectList(_ProductBatchSelection, "Batch_Id", "ProductName");
            List<object> newList = new List<object>();
            var thistaxinchart = db.ChartOfAccountings.Find(48);
            var members = db.Taxes.Where(e => e.Tax_Code.Contains(thistaxinchart.Account_Code)).ToList();
            foreach (var member in members)
                newList.Add(new
                {
                    Id = member.Tax_ID,
                    Name = member.Tax_Name + "   " + member.Tax_Percentage + "%"
                });
            invoiceMasterDVM.ProducttaxList = new SelectList(newList, "Id", "Name");

            return invoiceMasterDVM;


        }
        /// <summary>
        /// get All Detail of invoice master by its id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>List<InvoiceDetails> </returns>
        public static List<InvoiceDetails> GetAllTempPURCHInvoiceDetailsDetails(int masterVmId)
        {
            var db = new AccountingContainer();
            var DetailsTemp = db.InvoiceDetails.Where(invoice => invoice.Details_Id == masterVmId).ToList();
            return DetailsTemp;
        }
        /// <summary>
        /// get All Detail of invoice master by its id
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>List<InvoiceDetails> </returns>
        public static List<InvoiceDetails> GetAllTempPURCHInvoiceDetails2(int masterVmId)
        {
            var db = new AccountingContainer();
            var DetailsTemp = db.InvoiceDetails.Where(invoice => invoice.InvoiceMaster_Id == masterVmId).ToList();
            return DetailsTemp;
        }
        /// <summary>
        /// get all invoice detail temp by the id of invoice master temp
        /// </summary>
        /// <param name="masterVmId"></param>
        /// <returns>List<InvoiceDetailsTemp></returns>
        public static List<InvoiceDetailsTemp> GetAllTempPurchInvoiceDetailsresult2(int masterVmId)
        {
            var db = new AccountingContainer();
            var InvoiceSalesTemp = db.InvoiceMasterTemps.Find(masterVmId);
            return InvoiceSalesTemp.InvoiceDetailsTemps.ToList();
        }
        /// <summary>
        /// delete invoice detail temp by its id 
        /// </summary>
        /// <param name="DetailID"></param>
        /// <returns>string msg </returns>
        public static decimal DeleteTemp(int DetailID)
        {
            var db = new AccountingContainer();
            var delete = db.InvoiceDetailsTemps.Find(DetailID);

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
        /// add to to invoice detail temp 
        /// </summary>
        /// <param name="Detail"></param>
        /// <returns>String msg</returns>
        public static string AddSalesDetailsTemp(InvoiceMasterDetailsVM Detail)
        {
            var db = new AccountingContainer();
            var invoiceMasterTemp = db.InvoiceMasterTemps.Find(Detail.InvoiceMaster_Id);
            var batch = db.ProductBatches.Find(Detail.SelectedBtachtId);
            var invoiceMasterDetailsTemp = new InvoiceDetailsTemp();


            invoiceMasterDetailsTemp.Batch_ID = batch.Batch_ID;
            invoiceMasterDetailsTemp.Product_ID = Detail.SelectedBtachtId;
            invoiceMasterDetailsTemp.Product_Price = Detail.Product_Price;
            invoiceMasterDetailsTemp.Product_Quantity = Detail.Product_Quantity;

            var resultTax = db.Taxes.Find(Detail.Product_Tax);
            if (resultTax != null)
            {

                invoiceMasterDetailsTemp.Product_Tax = resultTax.Tax_ID;

            }
            var a = db.Taxes.Find(invoiceMasterDetailsTemp.Product_Tax);
            invoiceMasterDetailsTemp.Detail_Total_Tax = (Detail.Product_Price * Detail.Product_Quantity) * (Convert.ToDecimal(a.Tax_Percentage) / 100);
            invoiceMasterDetailsTemp.Detail_Total_Cost = batch.Product_Cost * Detail.Product_Quantity;
            invoiceMasterDetailsTemp.Total_Amount = (Detail.Product_Price * Detail.Product_Quantity);
            invoiceMasterTemp.InvoiceDetailsTemps.Add(invoiceMasterDetailsTemp);

            db.Entry(invoiceMasterTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "Details Has been added";
        }
        /// <summary>
        /// make Edit for invoice master by change old valuue to updated value
        /// </summary>
        /// <param name="masterVM"></param>
        /// <returns>string msg</returns>
        public static string editinvoice(MasterVM masterVM)
        {
            var db = new AccountingContainer();
            var invoice = db.InvoiceMasters.Find(masterVM.Master_Id);
            //1
            var qrMaster = new MasterQRInvoice();
            if (invoice.QRNumber != null)
            {
                qrMaster = db.MasterQRInvoices.Find(invoice.QRNumber.Value);
            }
            if (invoice.Invoice_State != TransactionState.Pending)
            {
                return Resources.validate.Error;
            }

            var mastertemp = db.InvoiceMasterTemps.Find(masterVM.Master_Id);
            invoice.Company_Id = Convert.ToInt32(masterVM.SelectedCompany_Id);
            invoice.Serial_Number = masterVM.Serial_Number;
            invoice.Description = masterVM.Description;
            invoice.Notes = masterVM.Notes;
            invoice.Invoice_Date = Convert.ToDateTime(masterVM.Invoice_Date);

            invoice.Due_Date = Convert.ToDateTime(invoice.Invoice_Date).AddMonths(masterVM.Payment_Policy);

            invoice.Master_Discount = (decimal)masterVM.Master_Discount;
            var currResult = db.Currencies.Find(masterVM.SelectedCurrencyId);
            if (currResult != null)
            {
                invoice.Invoice_Currency = currResult.Currency_Type;

            }


            invoice.Reference = masterVM.Reference;
            invoice.RevenueAccount_Id = masterVM.RevenueAccount_Id;
            invoice.Payment_Policy = masterVM.Payment_Policy;
            invoice.InvoiceDetails = new List<InvoiceDetails> { };
            invoice.Emp_Id = masterVM.Emp_Id;
            if (masterVM.SubLedger_ID != null)
            {

                invoice.SubLedger_ID = masterVM.SubLedger_ID;

            }
            else if (masterVM.BussinesUnit_ID != null)
            {

                invoice.SubLedger_ID = masterVM.BussinesUnit_ID;

            }
            else if (masterVM.Department_ID != null)
            {

                invoice.SubLedger_ID = masterVM.Department_ID;

            }
            else if (masterVM.Branch_Id != null)
            {

                invoice.SubLedger_ID = masterVM.Branch_Id;

            }
            else { invoice.SubLedger_ID = 1; }
            invoice.Document_Type = 0;
            invoice.Invoice_State = TransactionState.Pending;
            invoice.Customer_Id = (int)masterVM.SelectedCustomerId;
            if (masterVM.SelectedStoreId == 0 || masterVM.SelectedStoreId == null)
            {
                masterVM.SelectedStoreId = db.Stores.FirstOrDefault().Store_ID;
            }
            invoice.Store_ID = masterVM.SelectedStoreId;
            invoice.Department_ID = masterVM.Department_ID;
            invoice.BussinesUnit_ID = masterVM.BussinesUnit_ID;
            invoice.Branch_Id = masterVM.Branch_Id;
            invoice.FixedDiscount = masterVM.Fixed_Discount;
            List<InvoiceDetailsTemp> InvoicedtailListRemove = new List<InvoiceDetailsTemp>();
            var oldDetail = db.InvoiceDetails.Where(a => a.InvoiceMaster_Id == invoice.Master_Id).ToList();

            //2
            List<DetailQRInvoice> oldQrDetail = new List<DetailQRInvoice>();
            List<TaxQRInvoice> oldQrTaxes = new List<TaxQRInvoice>();
            if (invoice.QRNumber != null)
            {
                oldQrDetail = db.DetailQRInvoices.Where(a => a.MasterQRInvoiceId == invoice.QRNumber).ToList();
                oldQrTaxes = db.TaxQRInvoices.Where(a => a.MasterQRInvoiceId == invoice.QRNumber).ToList();
            }
            var oldTaxes = db.InvoiceTaxes.Where(a => a.InvoiceMasterMaster_Id == invoice.Master_Id).ToList();

            //  invoice.TotalAmountAfMat = (decimal)mastertemp.TotalAmountAfMat;
            invoice.Tax_ID = masterVM.Tax_ID;

            var detailListTemp = db.InvoiceDetailsTemps.Where(e => e.InvoiceMaster_Id == mastertemp.Master_Id).ToList();
            invoice.Total_Amount = 0;
            invoice.Total_Cost = 0;
            invoice.Total_Tax = 0;
            invoice.InvoiceDetails = new List<InvoiceDetails> { };
            qrMaster.DetailQRInvoices = new List<DetailQRInvoice>();

            qrMaster.TaxQRInvoices = new List<TaxQRInvoice>();

            foreach (var item in detailListTemp)
            {

                // Create new detail and map the values from detailVM to it
                // TODO: Use AutoMapper
                var details = new InvoiceDetails();
                details.Details_Id = item.Details_Id;
                details.Product_Quantity = item.Product_Quantity;
                details.Product_Price = item.Product_Price;

                decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                details.Detail_Discount = item.Detail_Discount;
                details.Detail_Discount_Amount = item.Detail_Discount_Amount;
                decimal discount = TotalBeforeDiscount * (details.Detail_Discount.Value / 100);
                decimal TotalAfterDiscount = TotalBeforeDiscount - discount;

                details.Product_Tax = Convert.ToInt32(item.Product_Tax);

                details.Batch_ID = item.Batch_ID;
                invoice.Tax_ID = (int)item.Product_Tax;
                if (details.Product_Tax == 0)
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

                    // Calculate the Total_Cost


                    details.Product_Cost = battchh.Product_Cost;

                    details.Detail_Total_Cost = item.Product_Quantity * detailBatch.Product_Cost;
                    details.Batch_ID = item.Batch_ID;
                    var price = item.Product_Quantity * item.Product_Price;
                    details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                    invoice.Total_Amount += details.Total_Amount;
                    invoice.Total_Cost += details.Detail_Total_Cost;
                    invoice.Total_Tax += details.Detail_Total_Tax;
                    details.Reverse_Total_Tax = details.Detail_Total_Tax;
                    details.Reverse_Quantity = details.Product_Quantity;
                    details.Reverse_Total_Amount = details.Total_Amount;
                    invoice.InvoiceDetails.Add(details);
                    db.InvoiceDetailsTemps.Remove(item);
                }
                else
                {
                    Product productss = db.Products.Find(item.Product_ID);
                    details.Product_ID = productss.Prod_ID;
                    // Calculate the Total_Cost

                    if (productss.Prod_Type == Prod_Type.خدمة)
                    {
                        details.Detail_Total_Cost = 0;
                    }
                    else
                    {


                        details.Detail_Total_Cost = item.Detail_Total_Cost;
                    }
                    details.Batch_ID = item.Batch_ID;
                    var price = item.Product_Quantity * item.Product_Price;
                    details.Total_Amount = (price + details.Detail_Total_Tax) - discount;
                    invoice.Total_Amount += details.Total_Amount;
                    invoice.Total_Cost += details.Detail_Total_Cost;
                    invoice.Total_Tax += details.Detail_Total_Tax;
                    details.Reverse_Total_Tax = details.Detail_Total_Tax;
                    details.Reverse_Quantity = details.Product_Quantity;
                    details.Reverse_Total_Amount = details.Total_Amount;
                    invoice.InvoiceDetails.Add(details);
                    db.InvoiceDetailsTemps.Remove(item);
                }

                var invoiceQRDetail = new DetailQRInvoice();
                invoiceQRDetail.Batch_ID = details.Batch_ID;
                invoiceQRDetail.Product_Id = details.Product_ID;
                invoiceQRDetail.Product_Quantity = details.Product_Quantity;
                invoiceQRDetail.Product_Tax = details.Product_Tax;


                invoiceQRDetail.Detail_Total_Cost = details.Detail_Total_Cost;

                invoiceQRDetail.Total_Amount = details.Total_Amount;

                invoiceQRDetail.Product_Price = details.Product_Price;
                invoiceQRDetail.Detail_Total_Tax = details.Detail_Total_Tax;
                invoiceQRDetail.Detail_Discount = item.Detail_Discount;
                invoiceQRDetail.Detail_Discount_Amount = item.Detail_Discount_Amount;
                invoiceQRDetail.ProductPriceCost = details.Product_Cost.Value;
                qrMaster.DetailQRInvoices.Add(invoiceQRDetail);
            }
            invoice.TotalNewTaxs = 0;
            invoice.InvoiceTaxes = new List<InvoiceTax>();
            if (masterVM.Files.FirstOrDefault() != null)
            {
                foreach (var file in masterVM.Files)
                {
                    var filedetails = new SalesFileDetail();
                    var data = new byte[file.ContentLength];
                    filedetails.FileName = file.FileName;
                    filedetails.ContentLength = file.ContentLength;
                    file.InputStream.Read(data, 0, file.ContentLength);
                    filedetails.FileContent = data;
                    filedetails.FileType = file.ContentType;
                    if (data.Length < 1000000)
                    {
                        invoice.SalesFileDetails.Add(filedetails);
                    }

                }
            }

            if (masterVM.Taxes != null)
            {
                foreach (var tax in masterVM.Taxes)
                {
                    var taxo = new InvoiceTax();
                    taxo.Tax_Id = tax;

                    taxo.Amount = (db.Taxes.Find(Convert.ToInt32(tax)).Tax_Percentage / 100) * invoice.InvoiceDetails.Select(a => a.Total_Amount).Sum();
                    invoice.TotalNewTaxs += taxo.Amount;

                    invoice.InvoiceTaxes.Add(taxo);
                    //3
                    var qrtax = new TaxQRInvoice();
                    qrtax.Tax_Id = tax;

                    qrtax.Amount = taxo.Amount;
                    qrMaster.TaxQRInvoices.Add(qrtax);

                }
            }

            // The total amount after matching is initialized to the total amount
            invoice.TotaL_After_Fixed = (invoice.Total_Amount + invoice.TotalNewTaxs) - invoice.FixedDiscount;

            invoice.TotalAmountAfMat = invoice.TotaL_After_Fixed.Value;
            db.InvoiceDetailsTemps.RemoveRange(InvoicedtailListRemove);
            invoice.Due_Date = invoice.Invoice_Date.AddDays(invoice.Payment_Policy.Value);
            invoice.Customer_Id = masterVM.SelectedCustomerId;
            //4



            //add QR Invoice
            qrMaster.AllowDiscount = invoice.FixedDiscount.Value;
            qrMaster.ApprovalCode = invoice.Reference;
            qrMaster.Company_Id = invoice.Company_Id;
            qrMaster.Customer_Id = invoice.Customer_Id;
            qrMaster.Description = invoice.Description;
            qrMaster.Employee_Id = invoice.Emp_Id.Value;
            qrMaster.Invoice_Date = invoice.Invoice_Date;
            qrMaster.Invoice_State = invoice.Invoice_State;
            qrMaster.InvoiceType_Id = 1;
            qrMaster.Master_Discount = invoice.Master_Discount;

            qrMaster.Reference = invoice.Reference;
            qrMaster.Serial_Number = invoice.Serial_Number;
            qrMaster.Store_ID = invoice.Store_ID;
            qrMaster.Subledger_Id = invoice.SubLedger_ID;
            qrMaster.Notes = invoice.Notes;
            qrMaster.TotalNewTaxs = invoice.TotalNewTaxs.Value;
            qrMaster.Account_Id = invoice.RevenueAccount_Id;
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

            db.Entry(invoice).State = EntityState.Modified;
            db.InvoiceMasterTemps.Remove(mastertemp);
            db.InvoiceDetails.RemoveRange(oldDetail);
            db.InvoiceTaxes.RemoveRange(oldTaxes);
            //5
            if (invoice.QRNumber != null)
            {

                db.TaxQRInvoices.RemoveRange(oldQrTaxes);
                db.DetailQRInvoices.RemoveRange(oldQrDetail);
            }

            db.SaveChanges();

            return "success";
        }
        /// <summary>
        /// make drobdownList for all customer that status is equal Active
        /// </summary>
        /// <returns>MasterVM</returns>
        //public static MasterVM GetCustomerList()
        //{
        //    var db = new AccountingContainer();    
        //    MasterVM vm = new MasterVM();

        //    List<object> newList = new List<object>();
        //    var members1 = db.Customers.Where(e => e.Customer_State == Customer_State.نشط).ToList();
        //    foreach (var member in members1)
        //        newList.Add(new
        //        {
        //            Id = member.Customer_Id,
        //            Name = member.Customer_Name + "        " + member.Customer_Phone
        //        });
        //    vm.MyCustomerList = new SelectList(newList, "Id", "Name");




        //    return vm;
        //}
        public static IEnumerable<SelectListItem> GetCustomerList()
        {
            var db = new AccountingContainer();
            IEnumerable<SelectListItem> customers = new SelectList(db.Customers.Where(a => a.Customer_State == Customer_State.نشط), "Customer_Id", "Customer_Name");
            return customers;

        }
        /// <summary>
        /// by usinng ProId and customer get the salary of that Product and its tax and put it in Slice Helper
        /// </summary>
        /// <param name="proId"></param>
        /// <param name="customer"></param>
        /// <returns>SliceHelper</returns>
        public static SliceHelper getProductPrice(int proId, int customer)
        {
            var db = new AccountingContainer();

            // var data = db.ProductBatches.Find(proId).Product_ID;
            var product = db.Products.Find(proId);
            var slice = new SliceHelper();
            if (customer == null || customer == 0)
            {
                slice.price = product.Sale_Price.Value;
                if (product.Tax_Id != null)
                {
                    slice.Tax = product.Tax_Id.Value;
                }
                return slice;
            }
            else
            {
                var cus = db.Customers.Find(customer).SlicesSlice_Id;
                var priceSlice = db.SlicesProducts.Where(a => a.SlicesSlice_Id == cus && a.Product_ID == proId).FirstOrDefault();
                if (priceSlice == null)
                {
                    slice.price = product.Sale_Price.Value;
                    if (product.Tax_Id != null)
                    {
                        slice.Tax = product.Tax_Id.Value;
                    }

                }
                else
                {
                    if (priceSlice.Slice_Price == 0)
                    {
                        slice.price = product.Sale_Price.Value;
                    }
                    else
                    {
                        slice.price = priceSlice.Slice_Price;
                    }
                    //slice.price = priceSlice.Slice_Price;
                    if (product.Tax_Id != null)
                    {
                        slice.Tax = product.Tax_Id.Value;
                    }
                }



                return slice;
            }
        }
        /// <summary>
        /// by usinng ProId and customer get the salary of that Product and its tax and put it in Slice Helper
        /// </summary>
        /// <param name="proId"></param>
        /// <param name="customer"></param>
        /// <returns>SliceHelper</returns>
        public static SliceHelper getProductPricenotPatched(int proId, int customer)
        {
            var db = new AccountingContainer();

            //var data = db.ProductBatches.Find(proId).Product_ID;
            var product = db.Products.Find(proId);
            var slice = new SliceHelper();
            if (customer == null || customer == 0)
            {
                slice.price = product.Sale_Price.Value;
                if (product.Tax_Id != null)
                {
                    slice.Tax = product.Tax_Id.Value;
                }
                return slice;
            }
            else
            {
                var cus = db.Customers.Find(customer).SlicesSlice_Id;
                var priceSlice = db.SlicesProducts.Where(a => a.SlicesSlice_Id == cus && a.Product_ID == product.Prod_ID).FirstOrDefault();
                if (priceSlice == null)
                {
                    slice.price = product.Sale_Price.Value;
                    if (product.Tax_Id != null)
                    {
                        slice.Tax = product.Tax_Id.Value;
                    }

                }
                else
                {
                    if (priceSlice.Slice_Price == 0)
                    {
                        slice.price = product.Sale_Price.Value;
                    }
                    else
                    {
                        slice.price = priceSlice.Slice_Price;
                    }

                    if (product.Tax_Id != null)
                    {
                        slice.Tax = product.Tax_Id.Value;
                    }
                }

                if (slice.Tax == null || slice.Tax == 0)
                {
                    slice.Tax = db.Taxes.Where(a => a.Tax_Code.StartsWith("02")).FirstOrDefault().Tax_ID;
                }


                return slice;
            }
        }
        /// <summary>
        /// get offer Price detail by his master id and product id
        /// </summary>
        /// <param name="proId"></param>
        /// <param name="masterid"></param>
        /// <returns>OfferPriceDetail</returns>
        public static OfferPriceDetail getProductInfo(int proId, int masterid)
        {
            var db = new AccountingContainer();
            var data = db.OfferPriceDetails.Where(e => e.Product_ID == proId && e.OfferPriceMasterMaster_Id == masterid).ToList().FirstOrDefault();

            return data;
        }
        /// <summary>
        /// get all information of store by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>store </returns>
        public static Store GetBranchFroStoreInfo(int id)
        {
            var db = new AccountingContainer();
            var thistype = db.Stores.Find(id);
            return thistype;
        }
        /// <summary>
        /// calculate by adding type id to start time
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="typeid"></param>
        /// <returns>string</returns>
        public static string getdate(string startdate, int typeid)
        {


            DateTime time = Convert.ToDateTime(startdate);
            var vm = new MasterVM();
            //vm.Due_Date = DateTime.Now.AddMonths(typemodel.Payment_Policy).ToString();
            vm.Due_Date = time.AddDays(typeid).ToString("dd-MM-yyyy");
            return vm.Due_Date;
        }
        /// <summary>
        /// calculate by adding type id to start time
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="typeid"></param>
        /// <returns>InvoiceMaster</returns>
        public static InvoiceMaster AddDate(MasterVM MstVm)
        {
            var db = new AccountingContainer();

            var date = db.InvoiceMasters.Find(MstVm.Master_Id);
            date.Due_Date = DateTime.Now.AddMonths(MstVm.Payment_Policy);

            db.Entry(date).State = EntityState.Modified;

            db.SaveChanges();
            db.Dispose();
            return date;
        }
        /// <summary>
        /// get all customer that belonge that slice of that Emolyee
        /// </summary>
        /// <param name="EmpID"></param>
        /// <returns></returns>

        public static List<Customer> GetCustomerUsingEmployee(int EmpID)
        {
            var db = new AccountingContainer();
            var thisEMployee = db.Sales_Settings.Where(e => e.Employee_ID == EmpID).ToList().FirstOrDefault();
            var slice = db.Sales_Setting_Slice.Where(r => r.Sales_SettingsId == thisEMployee.Id).ToList();

            List<Customer> CustList = new List<Customer>();
            if (slice != null || slice.Count != 0)
            {
                foreach (var sli in slice)
                {
                    var newCust = db.Customers.Where(s => s.SlicesSlice_Id == sli.Slice_ID).ToList();
                    if (newCust != null)
                    {
                        foreach (var cust in newCust)
                        {
                            CustList.Add(cust);
                        }
                    }
                    else
                    {
                    }


                }
                return CustList;
            }
            else
            {
                var allCust = db.Customers.ToList();
                return allCust;
            }


        }
        /// <summary>
        /// check if thr price of that product is smalles or Equal cost
        /// </summary>
        /// <param name="Batchid"></param>
        /// <param name="price"></param>
        /// <returns>string</returns>

        public static String CreateAlertIfPriceLowPriceCostForPOS(int Batchid, decimal price)
        {

            var db = new AccountingContainer();
            var prodpatchCost = db.ProductBatches.Find(Batchid).Product_Cost;
            if (price < prodpatchCost)
            {

                return "هذا السعر اقل من سعر التكلفة";
            }

            return null;




        }
        public static String CreateAlertIfPriceLowPriceCostForSales(int Batchid, decimal price)
        {

            var db = new AccountingContainer();

            decimal prodpatchCost = 0;
            try
            {
                prodpatchCost = db.Products.Find(Batchid).Prod_Cost_Price.Value;
            }
            catch
            {
                prodpatchCost = 0;
            }

            if (price < prodpatchCost)
            {

                return "هذا السعر اقل من سعر التكلفة";
            }

            return null;




        }

        /// <summary>
        /// calculate the net value of invoice 
        /// </summary>
        /// <param name="total"></param>
        /// <param name="FixedDiscount"></param>
        /// <returns>decimal</returns>
        public static decimal getTheNetTotal(decimal total, decimal FixedDiscount)
        {

            decimal net = total - FixedDiscount;
            return net;



        }
        /// <summary>
        /// Put all invoice detail for invoice to invoice detail Temp
        /// </summary>
        /// <param name="MasterId"></param>
        /// <returns>List<InvoiceDetailsTemp></returns>
        public static InvoiceDetailsTempVM GetAllDetailForEditing(int MasterId)
        {
            var db = new AccountingContainer();
            var invoicedetalTemp = new InvoiceDetailsTempVM();


            invoicedetalTemp.invoice_Id = MasterId;



            var detailTemp = db.InvoiceMasterTemps.Find(MasterId);

            db.InvoiceDetailsTemps.RemoveRange(detailTemp.InvoiceDetailsTemps);
            db.SaveChanges();
            var invoice = db.InvoiceMasters.Find(MasterId);
            if (invoice.FixedDiscount == null)
            {
                invoice.FixedDiscount = 0;
            }
            invoicedetalTemp.AllowDiscount = invoice.FixedDiscount.Value;
            if (invoice.TotalNewTaxs == null)
            {
                invoice.TotalNewTaxs = 0;
            }
            invoicedetalTemp.TotalNewTaxs = invoice.TotalNewTaxs.Value;
            List<InvoiceDetailsTemp> Lista = new List<InvoiceDetailsTemp> { };
            foreach (var item in invoice.InvoiceDetails)
            {
                var invoiceTemp = new InvoiceDetailsTemp();
                var pro = db.Products.Find(item.Product_ID);
                invoiceTemp.Batch_ID = item.Batch_ID.Value;
                invoiceTemp.Detail_Total_Tax = item.Detail_Total_Tax;
                invoiceTemp.Product_ID = item.Product_ID;
                invoiceTemp.Product_Price = item.Product_Price;
                invoiceTemp.Product_Code = pro.Prod_Code;
                if (item.Detail_Discount != null)
                {
                    invoiceTemp.Detail_Discount = item.Detail_Discount;
                    invoiceTemp.Detail_Discount_Amount = item.Detail_Discount_Amount;
                }
                else
                {
                    invoiceTemp.Detail_Discount = invoice.Master_Discount;
                    invoiceTemp.Detail_Discount_Amount = (item.Product_Price * item.Product_Quantity) * (invoice.Master_Discount / 100);
                }
                invoiceTemp.Product_Name = pro.Prod_Name;
                invoiceTemp.Product_Quantity = item.Product_Quantity;
                invoiceTemp.Detail_Total_Cost = pro.Prod_Cost_Price.Value * item.Product_Quantity;
                invoiceTemp.Product_Tax = item.Product_Tax;
                invoiceTemp.Detail_Total_Tax = item.Detail_Total_Tax;
                invoiceTemp.Total_Amount = item.Total_Amount;
                invoiceTemp.InvoiceMaster_Id = MasterId;
                Lista.Add(invoiceTemp);

            }
            db.InvoiceDetailsTemps.AddRange(Lista);

            db.SaveChanges();
            invoicedetalTemp.invoiceTemp = detailTemp.InvoiceDetailsTemps.ToList();
            return invoicedetalTemp;
        }
        /// <summary>
        /// change invoice value depend on discount
        /// </summary>
        /// <param name="discount"></param>
        /// <param name="Invoice_Id"></param>
        /// <returns>string</returns>
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

        public static decimal updateDiscountDebendOnDiscountAmount(decimal discount, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.InvoiceMasterTemps.Find(Invoice_Id);
            var pecentage = (discount / invoiceTemp.InvoiceDetailsTemps.Sum(a => a.Product_Price * a.Product_Quantity)) * 100;
            var detail = db.InvoiceDetailsTemps.Where(a => a.InvoiceMaster_Id == Invoice_Id).ToList();
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

        public static string changeDetailOfInvoicebyAmount(decimal discount, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.InvoiceMasterTemps.Find(Invoice_Id);
            var detail = db.InvoiceDetailsTemps.Where(a => a.InvoiceMaster_Id == Invoice_Id).ToList();
            decimal total = 0;
            foreach (var item in detail)
            {
                item.TotalBeforeDisc = item.Product_Quantity * item.Product_Price;
                item.TotalDiscount = item.TotalBeforeDisc * (discount / 100);
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
            return "تم تعديل الخصم بنجاح";

        }
        /// <summary>
        /// change invoice value depend on allow discount
        /// </summary>
        /// <param name="discount"></param>
        /// <param name="Invoice_Id"></param>
        /// <returns>string</returns>
        public static string changeDetailOfInvoiceForAllDiscount(decimal AllowDiscount, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.InvoiceMasterTemps.Find(Invoice_Id);
            invoiceTemp.AllowDiscount = AllowDiscount;
            db.Entry(invoiceTemp).State = EntityState.Modified;
            db.SaveChanges();
            return "تم عمل الخصم المسموح به";

        }
        public static IEnumerable<SelectListItem> GetAllCustomerForEmp(int EmpId)
        {
            var db = new AccountingContainer();
            var emp = db.Employees.Find(EmpId);
            if (emp == null || emp.ViewAllCustomer == true)
            {
                var newList = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط)
                               select new
                               {
                                   Id = cust.Customer_Id,
                                   Name = cust.Customer_Name + "        " + cust.Customer_Phone

                               }).ToList();
                return new SelectList(newList, "Id", "Name");
            }
            else
            {
                var newList = (from cust in db.Customers.Where(a => a.Customer_State == Customer_State.نشط && (a.Employee_Id == EmpId || a.Employee_Id == null))
                               select new
                               {
                                   Id = cust.Customer_Id,
                                   Name = cust.Customer_Name + "        " + cust.Customer_Phone

                               }).ToList();
                return new SelectList(newList, "Id", "Name");
            }


        }
        public static decimal updateDiscountDebendOnNet(decimal net, int Invoice_Id)
        {
            var db = new AccountingContainer();
            var invoiceTemp = db.InvoiceMasterTemps.Find(Invoice_Id);


            if (invoiceTemp.InvoiceDetailsTemps.Count() == 0 || invoiceTemp.InvoiceDetailsTemps.Select(a => a.Product_Tax).Distinct().Count() > 1)
            {
                try
                {
                    return invoiceTemp.Master_Discount.Value;
                }
                catch
                {
                    return 0;
                }
            }
            var detail = invoiceTemp.InvoiceDetailsTemps.ToList();
            var taxId = invoiceTemp.InvoiceDetailsTemps.FirstOrDefault().Product_Tax;
            decimal invoiceTax = 0;
            if (taxId > 0)
            {
                invoiceTax = db.Taxes.Find(taxId).Tax_Percentage / 100;
            }
            decimal totalPrice = invoiceTemp.InvoiceDetailsTemps.Select(a => a.Product_Quantity * a.Product_Price).Sum();
            decimal discountTotal = (totalPrice + (invoiceTax * totalPrice) - net) / (1 + invoiceTax);
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

        public static String MakeVoidForSales(int invoiceId)
        {
            var db = new AccountingContainer();
            var invoiceMaster = db.InvoiceMasters.Find(invoiceId);

            var Allreverse = db.ReverseSalesInvoiceMasters.Where(a => a.InvoiceMaster_Id == invoiceId).FirstOrDefault();
            if (Allreverse != null)
            {
                return Resources.ReverseSalesInvoiceMasterAddNewRevInvoiceDetail.ThierMoreInvoiceForThat;
            }
            else
            {

                var master = new ReverseSalesInvoiceMaster();


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
                    master.InvoiceMaster_Id = invoiceMaster.Master_Id;
                    master.Master_Discount = invoiceMaster.Master_Discount;
                    master.SubLedger = invoiceMaster.SubLedger_ID.ToString();
                    master.Branch_ID = invoiceMaster.Branch_Id;
                    master.BussinesUnit_ID = invoiceMaster.BussinesUnit_ID;
                    master.Department_ID = invoiceMaster.Department_ID;
                    master.Customer_Id = invoiceMaster.Customer_Id;


                    master.Company_Id = invoiceMaster.Company_Id;


                    master.Invoice_Currency = invoiceMaster.Invoice_Currency;


                    master.Store_ID = invoiceMaster.Store_ID;
                }



                master.Invoice_State = TransactionState.Post;

                master.GL_Date = DateTime.Now;





                decimal Sumtax = 0;
                decimal SumtAmount = 0;
                decimal SumTotalCost = 0;

                foreach (var item in invoiceMaster.InvoiceDetails.ToList())
                {
                    var details = new ReveInvoiceDetails();
                    details.Product_ID = item.Product_ID;
                    details.Product_Quantity = item.Product_Quantity;

                    details.Product_Price = item.Product_Price;
                    details.Detail_Discount = item.Detail_Discount;
                    details.Detail_Discount_Amount = item.Detail_Discount_Amount;





                    item.Detail_Total_Cost = item.Detail_Total_Cost / item.Product_Quantity;

                    item.Reverse_Quantity = 0;

                    details.Product_Tax = item.Product_Tax;
                    details.Batch_ID = item.Batch_ID;

                    var salesInvoiceDetail = db.InvoiceDetails.Where(a => a.Product_ID == item.Product_ID).FirstOrDefault();
                    var productBatch = db.ProductBatches.Where(a => a.Product_ID == salesInvoiceDetail.Product_ID && a.Store_ID == master.Store_ID).FirstOrDefault();


                    //var mySingelCost = item.Detail_Total_Cost / item.Product_Quantity;


                    details.Detail_Total_Cost = (item.Detail_Total_Cost * item.Product_Quantity);


                    decimal TotalBeforeDiscount = item.Product_Price * item.Product_Quantity;
                    if (item.Detail_Discount == null)
                    {
                        item.Detail_Discount = master.Master_Discount;
                    }
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
                    master.Document_Type = invoiceMaster.Document_Type;

                    master.ReveInvoiceDetails.Add(details);
                    master.Invoice_State = TransactionState.Pending;


                    db.Entry(item).State = EntityState.Modified;




                    master.ReveInvoiceDetails.Add(details);


                }

                master.Customer_Id = invoiceMaster.Customer_Id;
                master.Company_Id = invoiceMaster.Company_Id;
                master.Document_Type = invoiceMaster.Document_Type;
                master.Store_ID = invoiceMaster.Store_ID;
                master.Invoice_Currency = invoiceMaster.Invoice_Currency;
                master.Master_Discount = (master.ReveInvoiceDetails.Select(a => a.Detail_Discount_Amount).Sum() / master.ReveInvoiceDetails.Select(a => a.Product_Quantity * a.Product_Price).Sum()) * 100;
                master.Number = InvoiceSerial.generateSerialForReverse();
                db.ReverseSalesInvoiceMasters.Add(master);

                db.SaveChanges();

                DateTime DateNow = DateTime.Now;




                var transBatch = new TransactionBatch();
                transBatch.Date = DateNow;
                transBatch.OperationName = "مردود مبيعات";
                transBatch.OperationId = master.Master_Id;
                transBatch.Operation_Code = master.Serial_Number;
                transBatch.Description = "عكس قيد مبيعات اجل";
                transBatch.UserId = "noon";
                db.TransactionBatches.Add(transBatch);
                ReverseSalesInvoiceVM reverseVM = new ReverseSalesInvoiceVM();
                reverseVM.Master_Id = master.Master_Id;
                reverseVM.Invoice_State = TransactionState.Post;
                MakeSalesinQrSales(master);
                var msg=ReverseSalesInvoiceMaster.UpdateInvoiceMaster(reverseVM);
                db.SaveChanges();
                db.Dispose();

                return msg;




            }



        }

        public static bool MakeSalesinQrSales(ReverseSalesInvoiceMaster revmaster)
        {
            var db = new AccountingContainer();
          
            
            var qrmaster = new ReverseQRInvoice();
            var master = db.InvoiceMasters.Find(revmaster.InvoiceMaster_Id);
            if (master.RevenueAccount_Id > 0)
            {
                qrmaster.Account_Id = master.RevenueAccount_Id;
            }
            else { master.RevenueAccount_Id = 76; }

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
            qrmaster.PaymentMethod_Id = 0;
            qrmaster.Reference = "";
            qrmaster.Serial_Number = revmaster.Serial_Number;
            qrmaster.Store_ID = revmaster.Store_ID;
            qrmaster.SubLedger_ID =master.SubLedger_ID;
            qrmaster.Total_Amount = revmaster.Total_Amount;
            qrmaster.Total_Cost = revmaster.Total_Cost;
            qrmaster.Total_Payment_Method_Cost = 0;
            qrmaster.Total_Tax = revmaster.Total_Tax;
            var qrmmaster = db.MasterQRInvoices.Find(qrmaster.MasterQRInvoiceId);
            qrmaster.ReversePOSDetails = new List<ReverseQRDetail>();
            foreach(var item in revmaster.ReveInvoiceDetails)
            {
                var qrdetail = new ReverseQRDetail();
                if (item.Batch_ID != null)
                {
                    qrdetail.Batch_ID = item.Batch_ID.Value;
                }
                qrdetail.DetailQRInvoiceDetail_Id = qrmmaster.DetailQRInvoices.Where(a=>a.Product_Id==item.Product_ID).FirstOrDefault().Detail_Id;
                qrdetail.Detail_Discount = item.Detail_Discount;
                qrdetail.Detail_Discount_Amount = item.Detail_Discount_Amount;
                qrdetail.Detail_Total_Cost = item.Detail_Total_Cost;
                qrdetail.Detail_Total_Tax = item.Detail_Total_Tax;
                qrdetail.Product_Id = item.Product_ID;
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


