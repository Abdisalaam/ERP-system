using Accountingsystem.ViewModel;
using Accountingsystem.ViewModel.PrintVM;
using Accountingsystem.ViewModel.ReportsVM;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using Accountingsystem.ViewModel.ReportsVM.SubscriptionReportVM;
using System.Data.Entity;
using Accountingsystem.Utilites;

namespace Accountingsystem.Models
{
    public static class Reports
    {

        /// <summary>
        /// Print a POS invoice 
        /// </summary>
        /// <param name="id"> </param>
        /// <returns></returns>
        public static PrintMasterPOSInvoiceVM PrintPOSInvoice(int id)
        {
            var db = new AccountingContainer();
            PrintMasterPOSInvoiceVM master = new PrintMasterPOSInvoiceVM();
            decimal totalBeforeDiscount = 0;
            decimal Masterdiscount = 0;
            decimal totalAfterdiscount = 0;


            var list = db.MasterPOSInvoices.Find(id);
            master.ReceiptList = new List<POSReceipt>();
            master.ReceiptList = list.POSReceipts.Where(a=>a.Rec_state==TransactionState.Post).ToList();
            if (list.QRNumber != null)
            {
                master.barcode = list.QRNumber.Value;
            }
            
            master.Currency = list.Invoice_Currency;
            master.Invoice_Date = Convert.ToString(list.Invoice_Date);
            master.Description = list.Description;
            if (list.Customer_Id == null)
            {
                master.customerId = 0;
            }
            else
            {
                master.customerId = list.Customer_Id.Value;
            }
            if (list.PointOfSale.BranchSubledger_Id != 0 && list.PointOfSale.BranchSubledger_Id != null)
            {
              var branchName = db.SubLedgers.Find( list.PointOfSale.BranchSubledger_Id.Value).SubLed_Name;
                master.branchId = db.Branches.Where(a => a.Branch_Name == branchName).FirstOrDefault().Branch_Id;
            }
            else
            {
                master.branchId = 0;
            }
            master.branchIdSub = list.PointOfSale.BranchSubledger_Id;
            if (list.Number == null)
            {
                master.Serial_Number = list.Serial_Number;
            }
            else
            {
                master.Serial_Number = list.Number;
            }


            master.company = db.Companies.Find(list.Company_Id);


            var result = db.Customers.Find(list.Customer_Id);
            if (result != null)
            {
                master.customer_Name = result.Customer_Name;
                master.customerEmail = result.Customer_Mail;
                master.customerfax = result.Customer_Fax;
                master.customerPhone = result.Customer_Phone;
                master.customerTel = result.Customer_Mobile;
                master.customer_Tax = result.Customer_TaxNumber;

            }
            if (list.Employee_Id != null || list.Employee_Id > 0)
            {
                var thisEMployee = db.Employees.Find(list.Employee_Id);
                master.EmployeeEmail = thisEMployee.Emp_Mail;
                master.EmployeeMobile = thisEMployee.Emp_Phone;
                master.EmployeeName = thisEMployee.Emp_Name;
            }
            if (list.DetailPOSInvoices.FirstOrDefault().Product_Tax != null && list.DetailPOSInvoices.FirstOrDefault().Product_Tax > 0)
            {
                var thistax = db.Taxes.Find(list.DetailPOSInvoices.FirstOrDefault().Product_Tax);
                master.tax = thistax.Tax_Name + "%" + thistax.Tax_Percentage;
            }
            var Point = db.PointOfSales.Find(list.PointOfSalePOS_Id);
            master.pointOfSale = Point.POS_Name;
            master.POSId = Point.POS_Id;
            master.Notes = list.Notes;
            master.StoreName = db.Stores.Find(Point.Store_Id).Store_Name;
            master.storeId = db.Stores.Find(Point.Store_Id).Store_ID;
            List<PrintDetailPODVM> vmlist = new List<PrintDetailPODVM> { };

            foreach (var item in list.DetailPOSInvoices)
            {
                decimal detailAfterDiscount = 0;
                var invoice = new PrintDetailPODVM();
                var product = db.Products.Find(item.Product_Id);
                invoice.Product_Name = product.Prod_Name;
                invoice.Product_Description = product.Prod_Descrip;
                invoice.Product_Id = item.Product_Id;

                invoice.Detail_Discount = item.Detail_Discount;
                invoice.Detail_Discount_Amount = item.Detail_Discount_Amount;
                if (item.Detail_Discount_Amount != null)
                {
                    invoice.discount = item.Detail_Discount_Amount.Value;
                }
                else
                {
                    invoice.discount =0;
                }
                invoice.Product_Code = product.Prod_Code;
                invoice.Product_Model = product.Prod_Mark;
                invoice.Product_Price = item.Product_Price;
                invoice.Product_Quantity = item.Product_Quantity;
                invoice.Total_Amount_Detail = item.Total_Amount;
                invoice.amountBeforeTax = invoice.Product_Quantity * invoice.Product_Price;
                totalBeforeDiscount += invoice.amountBeforeTax;
                //invoice.discount = invoice.amountBeforeTax * (master.Master_Discount / 100);
                Masterdiscount += invoice.discount;
                detailAfterDiscount = invoice.amountBeforeTax - invoice.discount;
                totalAfterdiscount += detailAfterDiscount;
                invoice.Total_Tax = item.Detail_Total_Tax;
                master.totalTax += invoice.Total_Tax;
                invoice.MeasureUnit = db.ProductBatches.Find(item.Batch_ID).Product.Prod_Unet;
                invoice.tax = db.Taxes.Find(item.Product_Tax);
                var thisporduct = db.Products.Find(item.Product_Id);
                invoice.productcode = thisporduct.Prod_Code;
                vmlist.Add(invoice);

            }
            List<PrintPOSPaymentVM> Paylist = new List<PrintPOSPaymentVM> { };
            foreach (var item in list.PosPaymentMethods)
            {
                var Pay = new PrintPOSPaymentVM();
                Pay.Payment_Name = db.PaymentMethods.Find(item.PaymentMethod_Id).Payment_Type;
                Pay.Amount = item.Amount;
                master.totalPayment += Pay.Amount;
                Paylist.Add(Pay);
            }

            master.Taxes = new List<MasterPOSTaxVM> { };
            foreach (var item in list.TaxPOSInvoices)
            {
                var invoiceTax = new MasterPOSTaxVM();
                var taxo = db.Taxes.Find(Convert.ToInt32(item.Tax_Id));
                invoiceTax.TaxName = taxo.Tax_Name + "%" + taxo.Tax_Percentage;
                invoiceTax.Amount = item.Amount;
                master.Taxes.Add(invoiceTax);

            }
            if (list.TotalNewTaxs == null)
            {
                master.NewTaxTotal = 0;
            }
            else
            {
                master.NewTaxTotal = list.TotalNewTaxs.Value;
            }
            master.total = totalBeforeDiscount;
            master.Master_Discount = (list.Master_Discount.Value / 100) * totalBeforeDiscount;
            master.totalAfterDiscount = totalAfterdiscount;
            master.AllowDiscount = list.AllowDiscount.Value;
            master.totalAfterAllDiscount = list.Total_Amount.Value;
            master.POSInvoiceDetail = vmlist.ToList();
            master.POSPaymentDetail = Paylist.ToList();
            master.PercentgeMasterDiscount = list.Master_Discount.Value;
            return master;
        }

        /// <summary>
        /// Print a reverse POS invoice 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrintReversePOSInvoiceVM PrintReversePOSInvoice(int id)
        {
            var db = new AccountingContainer();
            PrintReversePOSInvoiceVM master = new PrintReversePOSInvoiceVM();

            decimal totalBeforeDiscount = 0;
            decimal Masterdiscount = 0;
            decimal totalAfterdiscount = 0;
            decimal totalTax = 0;



            var list = db.ReversePOSInvoices.Find(id);
            master.Master_Id = list.Master_Id;
            master.Description = list.Description;
            master.Invoice_Currency = list.Invoice_Currency;
            master.Invoice_Date = Convert.ToString(list.Invoice_Date);
            
            master.Master_Discount = list.Master_Discount.Value;
           
            var invoice2 = db.MasterPOSInvoices.Find(list.MasterPOSInvoiceMasterPOSInvoiceId);
            if (invoice2.Number == null)
            {
                master.Invoice_Code = invoice2.Serial_Number;
            }
            else
            {
                master.Invoice_Code = invoice2.Number;
            }
            if (list.Number == null)
            {
                master.Serial_Number = list.Serial_Number;
            }
            else
            {
                master.Serial_Number = list.Number;
            }


            master.Total_Amount = list.Total_Amount;
            master.Commition = list.Total_Payment_Method_Cost;
            master.PaymentMethod_Name = db.PaymentMethods.Find(list.PaymentMethod_Id).Bank_Account_Name;




            var result = db.Customers.Find(list.Customer_Id);
            if (result != null)
            {
                master.Customer_Name = result.Customer_Name;
                master.customerEmail = result.Customer_Mail;
                master.customerfax = result.Customer_Fax;
                master.customerPhone = result.Customer_Phone;
                master.customerTel = result.Customer_Mobile;

            }

            var thisEMployee = db.Employees.Find(list.MasterPOSInvoice.PointOfSale.Employee_Id);
            master.EmployeeEmail = thisEMployee.Emp_Mail;
            master.EmployeeMobile = thisEMployee.Emp_Phone;
            master.EmployeeName = thisEMployee.Emp_Name;

            master.company = db.Companies.Find(list.Company_Id);

            List<PrintPOSDetailInvoiceVM> vmlist = new List<PrintPOSDetailInvoiceVM> { };

            foreach (var item in list.ReversePOSDetails)
            {
                decimal detailAfterDiscount = 0;
                var invoice = new PrintPOSDetailInvoiceVM();
                var prod_Id = db.ProductBatches.Find(item.Batch_ID).Product_ID;
                var prod = db.Products.Find(prod_Id);
                invoice.Product_Code = prod.Prod_Code;
                invoice.Product_Model = prod.Prod_Mark;
                invoice.Product_Name = prod.Prod_Name;
                invoice.Product_Code = prod.Prod_Code;
                invoice.Product_Price = item.Product_Price;
                invoice.Product_Quantity = item.Product_Quantity;
                invoice.Total_Amount_Detail = item.Total_Amount;
                invoice.amountBeforeTax = invoice.Product_Quantity * invoice.Product_Price;
                totalBeforeDiscount += invoice.amountBeforeTax;
                invoice.discount = invoice.amountBeforeTax * (master.Master_Discount / 100);

                Masterdiscount += invoice.discount;
                detailAfterDiscount = invoice.amountBeforeTax - invoice.discount;
                totalAfterdiscount += detailAfterDiscount;
                invoice.Total_Tax = item.Detail_Total_Tax;
                totalTax += invoice.Total_Tax;
                invoice.MeasureUnit = db.ProductBatches.Find(item.Batch_ID).Product.Prod_Unet;
                invoice.tax = db.Taxes.Find(item.Product_Tax);


                invoice.Total_Tax = item.Detail_Total_Tax;


                vmlist.Add(invoice);

            }
            master.totalBeforeDiscount = totalBeforeDiscount;
            master.Masterdiscount = Masterdiscount;
            master.totalAfterdiscount = totalAfterdiscount;
            master.totalTax = totalTax;
            master.PaymentMethod_Name = db.PaymentMethods.Find(list.PaymentMethod_Id).Payment_Type;
            master.MyDetails = vmlist.ToList();
            master.POS_Name = list.MasterPOSInvoice.PointOfSale.POS_Name;
            master.org_Serial = list.MasterPOSInvoice.Serial_Number;
            return master;
        }

        /// <summary>
        /// For print sales invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrintInvoiceMasterVM PrintSalesInvoice(int id)
        {
            var db = new AccountingContainer();
            PrintInvoiceMasterVM master = new PrintInvoiceMasterVM();
            decimal totalBeforeDiscount = 0;
            decimal Masterdiscount = 0;
            decimal totalAfterdiscount = 0;


            var list = db.InvoiceMasters.Find(id);

            if (list == null)
            {
                master.customerId = 0;
            }
            else
            {
                master.customerId = list.Customer_Id;
            }
            master.Master_Id = list.Master_Id;
            master.QRNumber = list.QRNumber;
            master.branchId = list.Branch_Id;
            master.Notes = list.Notes;
            master.Invoice_Currency = list.Invoice_Currency;
            master.Invoice_Date = Convert.ToString(list.Invoice_Date);
            master.Due_Date = Convert.ToString(list.Due_Date);
            master.Master_Discount = list.Master_Discount;
            if (list.Number == null)
            {
                master.Serial_Number = list.Serial_Number;
            }
            else
            {
                master.Serial_Number = list.Number;
            }
            master.Total_Amount = list.Total_Amount;
            master.PaymentOfinvoice = list.TotaL_After_Fixed.Value - list.TotalAmountAfMat;
            master.company = db.Companies.Find(list.Company_Id);
            master.afterMatching = list.TotalAmountAfMat;
            master.totalTax = list.Total_Tax;
            master.fixed_Discount = list.FixedDiscount.Value;
            master.total_After_Fixed = list.TotaL_After_Fixed.Value;
            master.Describtion = list.Description;
            master.references = list.Reference;
            var result = db.Customers.Find(list.Customer_Id);
            if (result != null)
            {
                master.Customer_Name = result.Customer_Name;
                master.customerEmail = result.Customer_Mail;
                master.customerfax = result.Customer_Fax;
                master.customerPhone = result.Customer_Phone;
                master.customerTel = result.Customer_Mobile;
                master.CustomerTax = result.Customer_TaxNumber;
                master.customerAddress = result.Customer_Address;
            }
            if (list.Emp_Id != null || list.Emp_Id > 0)
            {
                var thisEMployee = db.Employees.Find(list.Emp_Id);
                master.EmployeeEmail = thisEMployee.Emp_Mail;
                master.EmployeeMobile = thisEMployee.Emp_Phone;
                master.EmployeeName = thisEMployee.Emp_Name;
            }
            if (list.Tax_ID == null || list.Tax_ID == 0)
            {
                
            }
            else
            {
                var thistax = db.Taxes.Find(list.Tax_ID);
                master.TaxName = thistax.Tax_Name + "%" + thistax.Tax_Percentage;
            }
            if (list.Store_ID != null && list.Store_ID > 0)
            {
                var thisstore = db.Stores.Find(list.Store_ID);
                master.StoreName = thisstore.Store_Name;
                master.storeId = thisstore.Store_ID;
            }
            List<PrintInvoiceMasterDetailsVM> vmlist = new List<PrintInvoiceMasterDetailsVM> { };

            foreach (var item in list.InvoiceDetails)
            {
                decimal detailAfterDiscount = 0;
                var product = db.Products.Find(item.Product_ID);
                var invoice = new PrintInvoiceMasterDetailsVM();
                invoice.Product_Name = product.Prod_Name;
                invoice.Product_Description = product.Prod_Descrip;
                invoice.Product_Model = product.Prod_Mark;
                invoice.Product_Id = item.Product_ID;
                invoice.Product_Price = item.Product_Price;
                invoice.Product_Quantity = item.Product_Quantity;                
                invoice.Total_Amount_Detail = item.Total_Amount;
                invoice.amountBeforeTax = invoice.Product_Quantity * invoice.Product_Price;
                totalBeforeDiscount += invoice.amountBeforeTax;
                if (item.Detail_Discount != null)
                {
                    invoice.Detail_Discount_Amount = item.Detail_Discount_Amount.Value;
                    invoice.Detail_Discount = item.Detail_Discount.Value;
                }
                else
                {
                    invoice.Detail_Discount_Amount = invoice.amountBeforeTax * (master.Master_Discount / 100);
                    invoice.Detail_Discount = master.Master_Discount;

                }
                invoice.discount = invoice.Detail_Discount_Amount;
                Masterdiscount += invoice.discount;
                detailAfterDiscount = invoice.amountBeforeTax - invoice.discount;
                totalAfterdiscount += detailAfterDiscount;
                invoice.Total_Tax = item.Detail_Total_Tax;
                invoice.MeasureUnit = db.Products.Find(item.Product_ID).Prod_Unet;
                invoice.tax = db.Taxes.Find(item.Product_Tax);
               
                invoice.productcode = product.Prod_Code;
                vmlist.Add(invoice);

            }
            master.Taxes = new List<InvoiceTaxesPrintVM> { };
            foreach (var item in list.InvoiceTaxes)
            {
                var invoiceTax = new InvoiceTaxesPrintVM();
                var taxo = db.Taxes.Find(Convert.ToInt32(item.Tax_Id));
                invoiceTax.TaxName = taxo.Tax_Name+"%"+taxo.Tax_Percentage;
                invoiceTax.Amount = item.Amount;
                master.Taxes.Add(invoiceTax);

            }

            master.totalBeforeDiscount = totalBeforeDiscount;
            master.Masterdiscount = Masterdiscount;
            master.totalAfterdiscount = totalAfterdiscount;
            if (list.TotalNewTaxs == null)
            {
                list.TotalNewTaxs = 0;
            }
            master.NewTaxTotal = list.TotalNewTaxs.Value;
            master.MyDetails = vmlist.ToList();
            return master;
        }
        public static MaintenanceOrderPrintVM PrintMaintenanceOrder(int id)
        {
            var db = new AccountingContainer();
            MaintenanceOrderPrintVM master = new MaintenanceOrderPrintVM();
            decimal totalBeforeDiscount = 0;
            decimal Masterdiscount = 0;
            decimal totalAfterdiscount = 0;


            var list = db.MaintenanceOrderOperationals.Find(id);
            master.Id = list.Id;
           
            master.Order_Code = list.Order_Code;

            master.customer =db.Customers.Find(list.Customer_Id);
            master.employee = db.Employees.Find(list.Emplyee_id);
            master.Describtion = list.Describtion;
            master.Date = list.Date;
            master.guaranteeDuration = list.guaranteeDuration;
            master.Masterdiscount = list.Master_Discount.Value;
            master.RequiredDescribtion = list.RequiredDescribtion;
            master.SerialNumber = list.SerialNumber;
            master.Status =db.MaintenanceStatus.Find( list.Status);
         

            master.company = db.Companies.FirstOrDefault();
          
            master.totalTax = list.Total_Tax.Value;
            master.fixed_Discount = list.AllowDiscount.Value;
            master.total_After_Fixed = list.Total_Amount.Value;
          
           
            
           
            List<MaintenanceOrderDetailPrintVM> vmlist = new List<MaintenanceOrderDetailPrintVM> { };

            foreach (var item in list.MaintenanceOrderOperationalDetails)
            {
                decimal detailAfterDiscount = 0;
                var product = db.Products.Find(item.Product_Id);
                var invoice = new MaintenanceOrderDetailPrintVM();
                invoice.Product_Name = product.Prod_Name;
                invoice.Product_Description = product.Prod_Descrip;
                invoice.Product_Model = product.Prod_Mark;
                invoice.Product_Id = item.Product_Id;
                invoice.Product_Price = item.Product_Price;
                invoice.Product_Quantity = item.Product_Quantity;
                invoice.Total_Amount_Detail = item.Total_Amount;
                invoice.amountBeforeTax = invoice.Product_Quantity * invoice.Product_Price;
                totalBeforeDiscount += invoice.amountBeforeTax;
                invoice.discount = invoice.amountBeforeTax * (master.Masterdiscount / 100);
                Masterdiscount += invoice.discount;
                detailAfterDiscount = invoice.amountBeforeTax - invoice.discount;
                totalAfterdiscount += detailAfterDiscount;
                invoice.Total_Tax = item.Detail_Total_Tax;
                invoice.MeasureUnit = db.Products.Find(item.Product_Id).Prod_Unet;
                invoice.tax = db.Taxes.Find(item.Product_Tax);

                invoice.productcode = product.Prod_Code;
                vmlist.Add(invoice);

            }
           

            master.totalBeforeDiscount = totalBeforeDiscount;
            master.Masterdiscount = Masterdiscount;
            master.totalAfterdiscount = totalAfterdiscount;
           
            master.MyDetails = vmlist.ToList();
            return master;
        }
        /// <summary>
        ///  For print offer price sales invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrintInvoiceMasterVM PrintOfferPrice(int id)
        {
            var db = new AccountingContainer();
            PrintInvoiceMasterVM master = new PrintInvoiceMasterVM();
            decimal totalBeforeDiscount = 0;
            decimal Masterdiscount = 0;
            decimal totalAfterdiscount = 0;

            var list = db.OfferPriceMasters.Find(id);
            master.Master_Id = list.OfferMas_Id;
            master.Notes = list.Notes;
            master.Invoice_Currency = list.Currency_Id.ToString();
            master.Invoice_Date = Convert.ToString(list.Offer_Date);
            master.Due_Date = Convert.ToString(list.OfferDeadLine);         
            master.Master_Discount = list.Master_Discount;
            master.Serial_Number = list.Serial_Number;
            master.Total_Amount = list.Total_Amount;
            //master.PaymentOfinvoice = list.Total_Amount;
            master.company = db.Companies.Find(list.Company_Id);
            //master.afterMatching = list.TotalAmountAfMat;
            master.totalTax = list.Total_Tax;
            var result = db.Customers.Find(list.Customer_Id);
            if (result != null)
            {
                master.Customer_Name = list.Customer_Name;
            }

            List<PrintInvoiceMasterDetailsVM> vmlist = new List<PrintInvoiceMasterDetailsVM> { };

            foreach (var item in list.OfferPriceDetails)
            {
                decimal detailAfterDiscount = 0;
                var invoice = new PrintInvoiceMasterDetailsVM();
                var product = db.Products.Find(item.Product_ID);
                invoice.Product_Name = product.Prod_Name;
                invoice.Product_Description = product.Prod_Descrip;

                invoice.Product_Model = product.Prod_Mark;
                invoice.Product_Price = item.Product_Price;
                invoice.Product_Quantity = item.Product_Quantity;
                invoice.Total_Amount_Detail = item.Total_Amount;
                invoice.amountBeforeTax = invoice.Product_Quantity * invoice.Product_Price;
                totalBeforeDiscount += invoice.amountBeforeTax;
                invoice.discount = invoice.amountBeforeTax * (master.Master_Discount / 100);
                Masterdiscount += invoice.discount;
                detailAfterDiscount = invoice.amountBeforeTax - invoice.discount;
                totalAfterdiscount += detailAfterDiscount;
                invoice.Total_Tax = item.Detail_Total_Tax;
                if (item.Batch_ID > 0)
                {
                    invoice.MeasureUnit = db.ProductBatches.Find(item.Batch_ID).Product.Prod_Unet;
                }
                else
                {
                    invoice.MeasureUnit = db.Products.Find(item.Product_ID).Prod_Unet;
                }
                invoice.tax = db.Taxes.Find(item.Product_Tax);

                vmlist.Add(invoice);

            }
            master.totalBeforeDiscount = totalBeforeDiscount;
            master.Masterdiscount = Masterdiscount;
            master.totalAfterdiscount = totalAfterdiscount;

            master.MyDetails = vmlist.ToList();
            return master;
        }

        /// <summary>
        /// For print new offerPirce SalesInvoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrintInvoiceMasterVM PrintOfferPriceNew(int id)
        {
            var db = new AccountingContainer();
            PrintInvoiceMasterVM master = new PrintInvoiceMasterVM();
            decimal totalBeforeDiscount = 0;
            decimal Masterdiscount = 0;
            decimal totalAfterdiscount = 0;

            var list = db.OfferPriceMasters.Find(id);
            master.branchId = list.BranchSubledger_Id;
            master.Notes = list.Notes;
            master.Master_Id = list.OfferMas_Id;
            master.Invoice_Currency = list.Currency_Id.ToString();
            master.Invoice_Date = Convert.ToString(list.Offer_Date);
            master.Due_Date = Convert.ToString(list.OfferDeadLine);
            master.Master_Discount = list.Master_Discount;
            master.Serial_Number = list.Serial_Number;
            master.Total_Amount = list.Total_Amount;
            master.Describtion = list.Description;
      
            master.company = db.Companies.Find(list.Company_Id);
 
            master.totalTax = list.Total_Tax;
            var result = db.Customers.Find(list.Customer_Id);
            if (result != null)
            {
                master.Customer_Name = list.Customer_Name;
                master.customerEmail = result.Customer_Mail;
                master.customerfax = result.Customer_Fax;
                master.customerPhone = result.Customer_Phone;
                master.customerTel = result.Customer_Mobile;
                master.CustomerTax = result.Customer_TaxNumber;
            }
            if (list.Emp_Id != null && list.Emp_Id > 0)
            {
                var thisEMployee = db.Employees.Find(list.Emp_Id);
                master.EmployeeEmail = thisEMployee.Emp_Mail;
                master.EmployeeMobile = thisEMployee.Emp_Phone;
                master.EmployeeName = thisEMployee.Emp_Name;
            }
            else
            {
                master.EmployeeEmail = "";
                master.EmployeeMobile = "";
                master.EmployeeName = "";
            }
            if (list.Tax_ID == null || list.Tax_ID == 0)
            {
               
            }
            else
            {
                var thistax = db.Taxes.Find(list.Tax_ID);
                master.TaxName = thistax.Tax_Name + " " + thistax.Tax_Percentage + "%";
            }
            if (list.Payment_Policy > 0 && list.Payment_Policy != null)
            {
                master.ValidateDays = list.Payment_Policy.ToString();
            }
            if (list.Store_ID != null && list.Store_ID > 0)
            {
                var thisstore = db.Stores.Find(list.Store_ID);
                master.StoreName = thisstore.Store_Name;
                master.storeId = thisstore.Store_ID;
            }
         
            List<PrintInvoiceMasterDetailsVM> vmlist = new List<PrintInvoiceMasterDetailsVM> { };

            foreach (var item in list.OfferPriceDetails)
            {
                decimal detailAfterDiscount = 0;
                var invoice = new PrintInvoiceMasterDetailsVM();
                var product = db.Products.Find(item.Product_ID);
                invoice.Product_Id = item.Product_ID;
                invoice.Product_Name = product.Prod_Name;
                invoice.Product_Description = product.Prod_Descrip;
                invoice.Product_Price = item.Product_Price;
                invoice.Product_Quantity = item.Product_Quantity;
                invoice.Total_Amount_Detail = item.Total_Amount;
                if (item.Detail_Discount != null)
                {
                    invoice.Detail_Discount = item.Detail_Discount.Value;
                    invoice.Detail_Discount_Amount = item.Detail_Discount_Amount.Value;
                }
                else
                {
                    invoice.Detail_Discount = list.Master_Discount;
                    invoice.Detail_Discount_Amount = (item.Product_Price * item.Product_Quantity) * (list.Master_Discount / 100);
                }
                invoice.amountBeforeTax = invoice.Product_Quantity * invoice.Product_Price;
                totalBeforeDiscount += invoice.amountBeforeTax;
                invoice.discount = invoice.amountBeforeTax * (master.Master_Discount / 100);
                Masterdiscount += invoice.discount;
                detailAfterDiscount = invoice.amountBeforeTax - invoice.discount;
                totalAfterdiscount += detailAfterDiscount;
                invoice.Total_Tax = item.Detail_Total_Tax;
                if (item.Batch_ID > 0)
                {
                    invoice.MeasureUnit = db.ProductBatches.Find(item.Batch_ID).Product.Prod_Unet;
                }
                else
                {
                    invoice.MeasureUnit = db.Products.Find(item.Product_ID).Prod_Unet;
                }
              
                invoice.tax = db.Taxes.Find(item.Product_Tax);
                var thisporduct = db.Products.Find(item.Product_ID);
                invoice.productcode = thisporduct.Prod_Code;

                vmlist.Add(invoice);

            }
            master.totalBeforeDiscount = totalBeforeDiscount;
            master.Masterdiscount = Masterdiscount;
            master.totalAfterdiscount = totalAfterdiscount;
            try
            {
                master.fixed_Discount = list.FixedDiscount.Value;
            }
            catch
            {
                master.fixed_Discount = 0;
            }
            master.MyDetails = vmlist.ToList();
            return master;
        }

        /// <summary>
        /// For print recipt for customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrintReceiptVM PrintReceipt(int id)
        {
            var db = new AccountingContainer();
            PrintReceiptVM receiptVM = new PrintReceiptVM();

            var receipt = db.Receipts.Find(id);
            receiptVM.Receipt_Id = receipt.Rec_ID;
            receiptVM.Recepit_Code = receipt.Rec_Number;
            receiptVM.Receipt_Currency = receipt.RecCurrency_ID;
            receiptVM.Receipt_Date = receipt.Rec_Date.ToString();
            if (receipt.Emp_Id != null)
            {
                receiptVM.EmpId = receipt.Emp_Id.Value;
            }
            var invoice = db.InvoiceMasters.Find(receipt.Invoice_ID);
            receiptVM.branchId = invoice.Branch_Id;
            receiptVM.Amount = receipt.Rec_Amount;
           
            receiptVM.Description = receipt.Rec_Description;
            receiptVM.company = db.Companies.Find(receipt.RecComp_ID);
            receiptVM.customer = db.Customers.Find(receipt.Customer_Id);
            
            if (receiptVM.customer != null)
            {
                receiptVM.Customer_Name = receiptVM.customer.Customer_Name;
                receiptVM.customer_Code = receiptVM.customer.Customer_Code;
            }
            var resultAccount = db.PaymentMethods.Find(receipt.RecBank_Account);
            if (resultAccount != null)
            {
                receiptVM.accountId = receipt.RecBank_Account;
            }


            return receiptVM;
        }

        public static PrintReceiptVM PrintPOSReceipt(int id)
        {
            var db = new AccountingContainer();
            PrintReceiptVM receiptVM = new PrintReceiptVM();

            var receipt = db.POSReceipts.Find(id);
            receiptVM.Receipt_Id = receipt.Rec_ID;
            receiptVM.Recepit_Code = receipt.Rec_Number;
            receiptVM.Receipt_Currency = receipt.RecCurrency_ID;
            receiptVM.Receipt_Date = receipt.Rec_Date.ToString();
            if (receipt.Emp_Id != null)
            {
                receiptVM.EmpId = receipt.Emp_Id.Value;
            }
            var invoice = db.MasterPOSInvoices.Find(receipt.MasterPOSInvoiceMasterPOSInvoiceId);
            receiptVM.branchId = invoice.PointOfSale.BranchSubledger_Id;
            receiptVM.Amount = receipt.Rec_Amount;

            receiptVM.Description = receipt.Rec_Description;
            receiptVM.company = db.Companies.Find(receipt.RecComp_ID);
            receiptVM.customer = db.Customers.Find(receipt.Customer_Id);

            if (receiptVM.customer != null)
            {
                receiptVM.Customer_Name = receiptVM.customer.Customer_Name;
                receiptVM.customer_Code = receiptVM.customer.Customer_Code;
            }
            var resultAccount = db.PaymentMethods.Find(receipt.RecBank_Account);
            if (resultAccount != null)
            {
                receiptVM.accountId = receipt.RecBank_Account;
            }


            return receiptVM;
        }

        /// <summary>
        /// For print payment for supllier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrintPaymentVM PrintPayment(int id)
        {
            var db = new AccountingContainer();
            PrintPaymentVM paymentVM = new PrintPaymentVM();

            var payment = db.Payments.Find(id);
            paymentVM.Payment_Id = payment.Pay_ID;
            paymentVM.Payment_Code = payment.Pay_Number;
            paymentVM.Payment_Currency = payment.Pay_Currency_ID;
            paymentVM.Payment_Date = payment.Pay_Date.ToString();
            paymentVM.Amount = payment.Pay_Amount;
            if (payment.Emp_Id == null)
            {
                paymentVM.EmpId = 0;
            }
            else
            {
                paymentVM.EmpId = payment.Emp_Id.Value;
            }
            paymentVM.Description = payment.Rec_Description;
            paymentVM.company = db.Companies.Find(payment.PayComp_ID);
            var result = db.Suppliers.Find(payment.Supplier_Id);
            if (result != null)
            {
                paymentVM.supp = result;
            }
            var resultAccount = db.ChartOfAccountings.Find(payment.PayBank_Account);
            if (resultAccount != null)
            {
                paymentVM.accountId = payment.PayBank_Account;
            }


            return paymentVM;
        }

        public static CostDistributionPrintVM PrintCostDestribution(int id)
        {
            var db = new AccountingContainer();
            var costMaster = db.CostDistributions.Find(id);
            CostDistributionPrintVM vm = new CostDistributionPrintVM();
            vm.company = db.Companies.FirstOrDefault();
            vm.id = id;
            vm.Cost_Distr_Code = costMaster.Cost_Distr_Code;
            vm.purchase = db.PurInvoiceMasters.Where(a => a.Serial_Number == costMaster.Invoice_Serial).FirstOrDefault();
            vm.subLedger = db.SubLedgers.Find(costMaster.Subledger_ID);
            vm.supplier = db.Suppliers.Find(vm.purchase.Supplier_Id);
            vm.total = 0;
            vm.detail = new List<CostDistributionDetailPrintVM>();
            foreach (var detail in costMaster.CostDistributionDetails)
            {
                var detailPurchase = PurInvoiceMaster.getMaterialOCompoundProduct(vm.purchase.InvoiceDetails.ToList(), vm.purchase.Store_ID.Value).Where(a => a.Product_ID == detail.Product_ID).FirstOrDefault();
                var CosDetai = new CostDistributionDetailPrintVM();
                CosDetai.product = db.Products.Find(detail.Product_ID);
                CosDetai.quantity = detailPurchase.Product_Quantity;
                CosDetai.Costbefor = detailPurchase.Total_Amount-(detailPurchase.Total_Amount*(vm.purchase.Master_Discount/100));
                CosDetai.IncreasingCost = detail.Product_AdditionalCost;
                CosDetai.CostAfter = CosDetai.IncreasingCost + CosDetai.Costbefor;
                CosDetai.CostOneBefor = CosDetai.Costbefor / CosDetai.quantity;
                CosDetai.CostOneAfter = CosDetai.CostAfter / CosDetai.quantity;
                CosDetai.Percenatge = CosDetai.IncreasingCost / CosDetai.Costbefor;
                vm.total += CosDetai.IncreasingCost;

                vm.detail.Add(CosDetai);
            }
            return vm;


        }



        /// <summary>
        /// For print purchase invoice 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrintPurInvoiceMasterVM PrintPurInvoice(int id)
        {
            var db = new AccountingContainer();
            decimal totalBeforeDiscount = 0;
            decimal Masterdiscount = 0;
            decimal totalAfterdiscount = 0;
            decimal totalTax = 0;
            PrintPurInvoiceMasterVM PurMaster = new PrintPurInvoiceMasterVM();
            var list = db.PurInvoiceMasters.Find(id);
            var thiscompany = db.Companies.Find(list.Company_Id);
            PurMaster.Master_Id = list.Master_Id;
            PurMaster.Serial_Number = list.Serial_Number;

            PurMaster.Invoice_Date = Convert.ToString(Convert.ToDateTime(list.Invoice_Date).Day + "-" + Convert.ToDateTime(list.Invoice_Date).Month + "-" + Convert.ToDateTime(list.Invoice_Date).Year);
            PurMaster.Master_Discount = list.Master_Discount;

            PurMaster.Invoice_Currency = list.Invoice_Currency;
            PurMaster.TotalTax = list.Total_Tax;
            PurMaster.Total_Amount = list.Total_Amount;
            PurMaster.Total_Cost = list.Total_Cost;
            PurMaster.TotalAmountAfterMatch = list.TotalAmountAfMat;
            if (list.Payed_Amount.Value == 0)
            {
                PurMaster.PayedAmount = 0;
            }
            else
            {
                PurMaster.PayedAmount = list.Payed_Amount.Value;
            }

            PurMaster.company = db.Companies.Find(list.Company_Id);
            PurMaster.Due_Date = Convert.ToString(Convert.ToDateTime(list.Due_Date).Day + "-" + Convert.ToDateTime(list.Due_Date).Month + "-" + Convert.ToDateTime(list.Due_Date).Year);
          

            var supplier = db.Suppliers.Find(list.Supplier_Id);
            {
                if (supplier != null)
                {
                    PurMaster.Supllier_Name = supplier.Supp_Name;
                }
            }

            List<PrintPurInvoiceDetailsVM> Details = new List<PrintPurInvoiceDetailsVM> { };

            foreach (var item in list.InvoiceDetails)
            {
                var PurDetails = new PrintPurInvoiceDetailsVM();
                decimal detailAfterDiscount = 0;
                PurDetails.Details_Id = item.Details_Id;

                PurDetails.Product_Quantity = item.Product_Quantity;
                PurDetails.Product_Price = item.Product_Price;
                PurDetails.Total_Amount = item.Total_Amount;
                PurDetails.Detail_Total_Tax = item.Detail_Total_Tax;
                PurDetails.Details_total_Cost = item.Detail_Total_Cost;
                PurDetails.amountBeforeTax = PurDetails.Product_Quantity * PurDetails.Product_Price;
                totalBeforeDiscount += PurDetails.amountBeforeTax;
                PurDetails.discount = PurDetails.amountBeforeTax * (PurMaster.Master_Discount / 100);
                Masterdiscount += PurDetails.discount;
                detailAfterDiscount = PurDetails.amountBeforeTax - PurDetails.discount;
                totalAfterdiscount += detailAfterDiscount;

                totalTax += PurDetails.Detail_Total_Tax;
               var MeasureUnits = db.Products.Find(item.Product_ID);
                if (MeasureUnits != null)
                {
                    PurDetails.MeasureUnit = db.Products.Find(item.Product_ID).Prod_Unet;
                }
                else
                {
                    PurDetails.MeasureUnit = "";

                }
                PurDetails.tax = db.Taxes.Find(item.Product_Tax);


                PurDetails.productCode = item.Item_Code;
                PurDetails.ProductName = item.ItemDesc;
                PurDetails.Product_Description = db.Products.Find(item.Product_ID).Prod_Descrip;
                Details.Add(PurDetails);
            }
            PurMaster.MyDetails = Details.ToList();
            PurMaster.totalBeforeDiscount = totalBeforeDiscount;
            PurMaster.Masterdiscount = Masterdiscount;
            PurMaster.totalAfterdiscount = totalAfterdiscount;
            PurMaster.totalTax = totalTax;

            return PurMaster;
        }
        public static PrintOrderVM PrintOrder(int id)
        {
            var db = new AccountingContainer();
            PrintOrderVM master = new PrintOrderVM();
            decimal totalBeforeDiscount = 0;
            decimal Masterdiscount = 0;
            decimal totalAfterdiscount = 0;

            var list = db.Orders.Find(id);
            master.Master_Id = list.Id;
            //var a = Int32.Parse(list.Invoice_Currency);
            master.QRNumber = list.QRNumber;

            master.Invoice_Date = Convert.ToString(list.OrderDate);
            master.company = db.Companies.FirstOrDefault();
            master.Master_Discount = list.DicountInPrecentage;
            if (list.Number == null)
            {
                master.Serial_Number = list.OrderNumber;
            }
            else
            {
                master.Serial_Number = list.Number;
            }
            master.Notes = list.Comment;
            master.Total_Amount = list.Total_Amount;
            var branchEmp = db.Employees.Find(list.EmployeeID).BranchSubledger_Id;
            if(branchEmp!=0&& branchEmp != null)
            {

                var sub = db.SubLedgers.Find(branchEmp);
            
                if (sub != null)
                {
                    master.branchId = sub.SubLed_Id;
                }
            }


            if (list.CustomerID == null)
            {
                master.customerId = 0;
            }
            else
            {
                master.customerId = list.CustomerID.Value;
            }
            master.totalTax = list.Total_Tax;
            master.PaymentMethod_Name = db.PaymentMethods.Find(list.PaymentMethodeID).Payment_Type;

            master.storeid = db.Stores.FirstOrDefault().Store_ID;

            var result = db.Customers.Find(list.CustomerID);
            if (result != null)
            {
                master.Customer_Name = result.Customer_Name;
                master.customerEmail = result.Customer_Mail;
                master.customerfax = result.Customer_Fax;
                master.customerPhone = result.Customer_Phone;
                master.customerTel = result.Customer_Mobile;
                master.CustomerTax = result.Customer_TaxNumber;

            }
            if (list.EmployeeID != null || list.EmployeeID > 0)
            {
                var thisEMployee = db.Employees.Find(list.EmployeeID);
                master.EmployeeEmail = thisEMployee.Emp_Mail;
                master.EmployeeMobile = thisEMployee.Emp_Phone;
                master.EmployeeName = thisEMployee.Emp_Name;
            }


            var thisstore = db.Stores.Find(master.storeid);
            master.StoreName = thisstore.Store_Name;


            master.TotalAfterFixed = list.Total_Amount + list.TotalNewTaxs;

            List<PrintOrderDetail> vmlist = new List<PrintOrderDetail> { };

            foreach (var item in list.OrderDetails)
            {
                decimal detailAfterDiscount = 0;
                var invoice = new PrintOrderDetail();
                var product = db.Products.Find(item.ProductID);
                invoice.Product_Name = product.Prod_Name;
                invoice.Product_Code = product.Prod_Code;
                invoice.Product_Description = product.Prod_Descrip;
                invoice.Product_Model = product.Prod_Mark;
                invoice.Product_Id = item.ProductID;
                invoice.Product_Price = item.Product_Price;
                invoice.Product_Quantity = item.Quantity;
                invoice.Total_Amount_Detail = item.Total_Amount;
                invoice.amountBeforeTax = invoice.Product_Quantity * invoice.Product_Price;
                totalBeforeDiscount += invoice.amountBeforeTax;

                invoice.discount = invoice.amountBeforeTax * (master.Master_Discount / 100);

                Masterdiscount += invoice.discount;
                detailAfterDiscount = invoice.amountBeforeTax - invoice.discount;
                totalAfterdiscount += detailAfterDiscount;
                if (item.TaxId != null && item.TaxId != 0)
                {
                    var thistax = db.Taxes.Find(item.TaxId);
                    master.TaxName = thistax.Tax_Name + "%" + thistax.Tax_Percentage;
                }

                invoice.Total_Tax = item.Detail_Total_Tax;
                invoice.MeasureUnit = db.Products.Find(item.ProductID).Prod_Unet;
                invoice.tax = db.Taxes.Find(item.TaxId);
                vmlist.Add(invoice);

            }

            if (list.TotalNewTaxs == null)
            {
                master.NewTaxTotal = 0;
            }
            else
            {
                master.Taxes = db.Taxes.Find(list.SelectedExtraTaxId);
                master.NewTaxTotal = list.TotalNewTaxs;
            }
            master.totalBeforeDiscount = totalBeforeDiscount;
            master.Masterdiscount = Masterdiscount;
            master.totalAfterdiscount = totalAfterdiscount;




            master.MyDetails = vmlist.ToList();
            return master;
        }
        public static GLMaster PrintExpencesNew(int id)
        {
            var db = new AccountingContainer();
            var GL = MakeGLExpences(id);
            return GL;
        }
        public static GLMaster MakeGLExpences(int id)
        {
            
            var db = new AccountingContainer();
            var payment = db.Expenses.Find(id);
       
                
            
                GLMaster glHeader = new GLMaster();
                // Used this GLHeader Create GL
                glHeader.GL_Date = Convert.ToDateTime(payment.Expense_date);
                glHeader.GL_Desc = payment.Expense_Description;
                glHeader.Company_Id = db.Companies.ToList().FirstOrDefault().Comp_Id;
                glHeader.GLType_Id = "مصروف";
                glHeader.Currency_Id = "الريال السعودى";
                glHeader.GLCode = payment.Expense_Code;
                glHeader.Auto = true;
            

                // this GL Details Start advansedrec Secound BankGL
                GLDetail ExpenseAccount = new GLDetail(),
                    TaxAccount = new GLDetail(),
                    RevenueAccount = new GLDetail(),
                    supplierCredit = new GLDetail(),
                    supplierDepit = new GLDetail();




                RevenueAccount.Bu_Id = 0;
                RevenueAccount.Asset_Id = null;



                // Used ChartOfAccountRecive  Spacefic Account this Bank or treasury or tax. 

                if (payment.Asset_Id > 0)
                {

                    var ChartOfAccounPay = db.ChartOfAccountings.Find(45);
                    supplierCredit.Account_Id = ChartOfAccounPay.Account_Id;
                    supplierCredit.Account_Name = ChartOfAccounPay.Account_Name;
                    Accounts supplSubledger = db.Accounts.Find(payment.Asset_Id);
                    supplierCredit.Account_Child_ID = supplSubledger.Accounts_Id;
                    supplierCredit.Account_CHild_Name = supplSubledger.Account_Name;
                    supplierCredit.GLMasterGLMaster_Id = glHeader.GLMaster_Id;


                    supplierDepit.Account_Id = ChartOfAccounPay.Account_Id;
                    supplierDepit.Account_Name = ChartOfAccounPay.Account_Name;

                    supplierDepit.Account_Child_ID = supplSubledger.Accounts_Id;
                    supplierDepit.Account_CHild_Name = supplSubledger.Account_Name;
                    supplierDepit.GLMasterGLMaster_Id = glHeader.GLMaster_Id;
                }

                if (payment.AccountTreasure_Id > 0)
                {
                    var paymento = db.PaymentMethods.Find(payment.AccountTreasure_Id);
                    var ChartOfAccounPay = db.ChartOfAccountings.Find(paymento.Bank_Account_Id);
                    RevenueAccount.Account_Id = ChartOfAccounPay.Account_Id;
                    RevenueAccount.Account_Name = ChartOfAccounPay.Account_Name;

                    RevenueAccount.GLMasterGLMaster_Id = glHeader.GLMaster_Id;
                }




                if (payment.Tax_ID != null && payment.Tax_ID > 0)
                {
                    var thistax = db.Taxes.Find(payment.Tax_ID).Tax_Code;
                    if (thistax != null)
                    {
                        var taxChart = db.ChartOfAccountings.Where(a => a.Account_Code == thistax).FirstOrDefault();
                        TaxAccount.Account_Id = taxChart.Account_Id;
                        TaxAccount.Account_Name = taxChart.Account_Name;
                        TaxAccount.GLMasterGLMaster_Id = glHeader.GLMaster_Id;


                        TaxAccount.Bu_Id = 0;
                        TaxAccount.Asset_Id = null;
                    }
                }




                ExpenseAccount.Bu_Id = 1;
                ExpenseAccount.Asset_Id = null;

                // Used ChartOfAccountRecive  Spacefic Account this advansedPayment . 
                var ChartOfAccounadvansedrec = db.ChartOfAccountings.Find(payment.AccountExpense_Id);
                ExpenseAccount.Account_Id = ChartOfAccounadvansedrec.Account_Id;
                ExpenseAccount.Account_Name = ChartOfAccounadvansedrec.Account_Name;
                ExpenseAccount.GLMasterGLMaster_Id = glHeader.GLMaster_Id;

                //var sup = db.Projects.Find(project.Project_Id);
                //SubLedger supplSubledger = db.SubLedgers.Where(subled => subled.SubLed_Code == sup.Project_Code).FirstOrDefault();
                //ExpenseAccount.SubLedger_Id = supplSubledger.SubLed_Id;

                //var sup = db.SubLedgers.Find(project.Project_Id);
                ExpenseAccount.SubLedger_Id = payment.Subledger_Id;

                RevenueAccount.SubLedger_Id = 0;

                // This State GL Start Pending Secound Post thierd Voiod 
        
                        // From Account ExpenseAccount to RevenueAccounts.
                        //ExpenseAccount.GLDetail_Debit = payment.Pay_Amount;
                      

                        RevenueAccount.GLDetail_Credit = payment.Expense_Value;
                        if (payment.Tax_ID > 0)
                        {
                            var thistaxs = db.Taxes.Find(payment.Tax_ID);
                            var TaxAmount = (payment.Expense_Value / (1 + (thistaxs.Tax_Percentage / 100))) * (thistaxs.Tax_Percentage / 100);
                            TaxAccount.GLDetail_Debit = TaxAmount;
                            ExpenseAccount.GLDetail_Debit = payment.Expense_Value - TaxAmount;

                        }
                        else
                        {
                            ExpenseAccount.GLDetail_Debit = payment.Expense_Value;
                        }
                        if (payment.AccountTreasure_Id > 0 && payment.Asset_Id > 0)
                        {
                            supplierCredit.GLDetail_Credit = payment.Expense_Value;
                            supplierDepit.GLDetail_Debit = payment.Expense_Value;

                        }
                        else if (payment.Asset_Id > 0)
                        {
                            supplierCredit.GLDetail_Credit = payment.Expense_Value;
                        }

             

                    if (payment.Tax_ID > 0)
                    {
                        glHeader.GLDetails.Add(TaxAccount);
                    }
                    glHeader.GLDetails.Add(ExpenseAccount);
                    //if (TaxAccount.GLDetail_Credit == 0 && TaxAccount.GLDetail_Credit == 0)
                    //{

                    //}
                    //else { glHeader.GLDetails.Add(TaxAccount); }
                    if (payment.AccountTreasure_Id > 0 && payment.Asset_Id > 0)
                    {
                        glHeader.GLDetails.Add(supplierCredit);
                        glHeader.GLDetails.Add(supplierDepit);
                    }
                    else if (payment.AccountTreasure_Id == 0 && payment.Asset_Id > 0)
                    {
                        glHeader.GLDetails.Add(supplierCredit);
                    }
                    if (RevenueAccount.Account_Id > 0)
                    {
                        glHeader.GLDetails.Add(RevenueAccount);
                    }
                    return glHeader;
                
                
            
            

        }
        public static PrintRevOrderVM PrintReverseOrder(int id)
        {
            var db = new AccountingContainer();
            PrintRevOrderVM master = new PrintRevOrderVM();

            decimal totalBeforeDiscount = 0;
            decimal Masterdiscount = 0;
            decimal totalAfterdiscount = 0;
            decimal totalTax = 0;



            var list = db.ReverseOrders.Find(id);
            master.Master_Id = list.Id;

            master.Invoice_Currency = "الريال السعودي";
            master.Invoice_Date = Convert.ToString(list.ReverseOrderDate);
          
            master.Master_Discount = list.DicountInPrecentage;

            master.Invoice_Code = list.Order.OrderNumber;

          
            if (list.Order.Number == null)
            {
                master.Invoice_Code = list.Order.OrderNumber;
            }
            else
            {
                master.Invoice_Code = list.Order.Number;
            }
            if (list.Number == null)
            {
                master.Serial_Number = list.OrderNumber;
            }
            else
            {
                master.Serial_Number = list.Number;
            }



            master.Total_Amount = list.Total_Amount;
            master.storeId = db.Stores.FirstOrDefault().Store_ID;

            master.PaymentMethod_Name = db.PaymentMethods.Find(list.PaymentMethodeID).Payment_Type;
            if (list.Order.CustomerID == null || list.Order.CustomerID == 0)
            {
                master.Customer_Name = "";
            }
            else
            {
                var result = db.Customers.Find(list.Order.CustomerID);

                master.Customer_Name = db.Customers.Find(list.Order.CustomerID).Customer_Name;
            }
            master.company = db.Companies.FirstOrDefault();

            List<PrintRevOrderDetail> vmlist = new List<PrintRevOrderDetail> { };

            foreach (var item in list.ReverseOrderDetails)
            {
                decimal detailAfterDiscount = 0;
                var invoice = new PrintRevOrderDetail();
                var prod_Id = db.ProductBatches.Find(item.PatchId).Product_ID;
                var prod = db.Products.Find(prod_Id);
                invoice.Product_Description = prod.Prod_Descrip;
                invoice.Product_Code = prod.Prod_Code;
                invoice.Product_Name = prod.Prod_Name;
                invoice.Product_Model = prod.Prod_Mark;
                invoice.Product_Price = item.Product_Price;
                invoice.Product_Quantity = item.Quantity;
                invoice.Total_Amount_Detail = item.Total_Amount;
                invoice.amountBeforeTax = invoice.Product_Quantity * invoice.Product_Price;
                totalBeforeDiscount += invoice.amountBeforeTax;
                invoice.discount = invoice.amountBeforeTax * (master.Master_Discount / 100);

                Masterdiscount += invoice.discount;
                detailAfterDiscount = invoice.amountBeforeTax - invoice.discount;
                totalAfterdiscount += detailAfterDiscount;
                invoice.Total_Tax = item.Detail_Total_Tax;
                totalTax += invoice.Total_Tax;
                invoice.MeasureUnit = db.ProductBatches.Find(item.PatchId).Product.Prod_Unet;
                invoice.tax = db.Taxes.Find(item.TaxId);


                invoice.Total_Tax = item.Detail_Total_Tax;


                vmlist.Add(invoice);

            }
            master.totalBeforeDiscount = totalBeforeDiscount;
            master.Masterdiscount = Masterdiscount;
            master.totalAfterdiscount = totalAfterdiscount;
            master.totalTax = totalTax;

            master.MyDetails = vmlist.ToList();
            return master;
        }
        /// <summary>
        /// Print SalesRecipt invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrintSalesReciptMasterVM PrintSalesRecipt(int id)
        {
            var db = new AccountingContainer();
            PrintSalesReciptMasterVM master = new PrintSalesReciptMasterVM();
            decimal totalBeforeDiscount = 0;
            decimal Masterdiscount = 0;
            decimal totalAfterdiscount = 0;

            var list = db.SalesReceiptMasters.Find(id);
            if (list.Customer_Id == null)
            {
                master.customerId = 0;
            }
            else
            {
                master.customerId = list.Customer_Id.Value;
            }
            master.branchId = list.Branch_Id;
            master.Master_Id = list.Master_Id;
            master.QRNumber = list.QRNumber;
            //var a = Int32.Parse(list.Invoice_Currency);
            master.Invoice_Currency = list.Invoice_Currency;
            master.Invoice_Date = Convert.ToString(list.Invoice_Date);
            master.company = db.Companies.Find(list.Company_Id);
            master.Master_Discount = list.Master_Discount;
            if (list.Number == null)
            {
                master.Serial_Number = list.Serial_Number;
            }
            else
            {
                master.Serial_Number = list.Number;
            }
            master.Notes = list.Notes;
            master.Total_Amount = list.Total_Amount;
            master.Commition = list.Total_Payment_Method_Cost;
            master.totalTax = list.Total_Tax;
            master.PaymentMethod_Name = db.PaymentMethods.Find(list.PaymentMethod_Id).Payment_Type;
            master.Description = list.Description;
            master.storeid = list.Store_ID;
            if (list.Branch_Id != 0 && list.Branch_Id != null)
            {
                var branchName = db.SubLedgers.Find(list.Branch_Id.Value).SubLed_Name;
                //master.Branch_Id = db.Branches.Where(a => a.Branch_Name == branchName).FirstOrDefault().Branch_Id;
            }
            var result = db.Customers.Find(list.Customer_Id);
            if (result != null)
            {
                master.Customer_Name = result.Customer_Name;
                master.customerEmail = result.Customer_Mail;
                master.customerfax = result.Customer_Fax;
                master.customerPhone = result.Customer_Phone;
                master.customerTel = result.Customer_Mobile;
                master.CustomerTax = result.Customer_TaxNumber;

            }
            if (list.Emp_Id != null || list.Emp_Id > 0)
            {
                var thisEMployee = db.Employees.Find(list.Emp_Id);
                master.EmployeeEmail = thisEMployee.Emp_Mail;
                master.EmployeeMobile = thisEMployee.Emp_Phone;
                master.EmployeeName = thisEMployee.Emp_Name;
            }

            if (list.Store_ID != null && list.Store_ID > 0)
            {
                var thisstore = db.Stores.Find(list.Store_ID);
                master.StoreName = thisstore.Store_Name;
            }
            master.Fixed_Discount = list.FixedDiscount.Value;
            master.TotalAfterFixed = list.TotaL_After_Fixed.Value;

            List<PrintSalesReciptDetailVM> vmlist = new List<PrintSalesReciptDetailVM> { };

            foreach (var item in list.SalesReceiptDetails)
            {
                decimal detailAfterDiscount = 0;
                var invoice = new PrintSalesReciptDetailVM();
                var product = db.Products.Find(item.Product_Id);
                invoice.Product_Name = product.Prod_Name;
                invoice.Product_Code = product.Prod_Code;
                invoice.Product_Description = product.Prod_Descrip;
                invoice.Product_Model = product.Prod_Mark;
                invoice.Product_Id = item.Product_Id;
                invoice.discount = item.Detail_Discount_Amount;
                invoice.discountPercentage = item.Detail_Discount;
                invoice.Product_Price = item.Product_Price;
                invoice.Product_Quantity = item.Product_Quantity;
                invoice.Total_Amount_Detail = item.Total_Amount;
                invoice.amountBeforeTax = invoice.Product_Quantity * invoice.Product_Price;
                totalBeforeDiscount += invoice.amountBeforeTax;
                if (item.Detail_Discount !=null)
                {
                    invoice.discount = invoice.amountBeforeTax * (item.Detail_Discount / 100);
                }
                else
                {
                    invoice.discount = invoice.amountBeforeTax * (master.Master_Discount / 100);
                }
                Masterdiscount += invoice.discount.Value;
                detailAfterDiscount = invoice.amountBeforeTax - invoice.discount.Value;
                totalAfterdiscount += detailAfterDiscount;
                if (item.Product_Tax != null && item.Product_Tax != 0)
                {
                    var thistax = db.Taxes.Find(item.Product_Tax);
                    master.TaxName = thistax.Tax_Name + "%" + thistax.Tax_Percentage;
                }

                invoice.Total_Tax = item.Detail_Total_Tax;
                invoice.MeasureUnit = db.Products.Find(item.Product_Id).Prod_Unet;
                invoice.tax = db.Taxes.Find(item.Product_Tax);
                vmlist.Add(invoice);

            }
            master.Taxes = new List<SalesReciptTaxVM> { };
            foreach (var item in list.SalesReceiptTaxes)
            {
                var invoiceTax = new SalesReciptTaxVM();
                var taxo = db.Taxes.Find(Convert.ToInt32(item.Tax_Id));
                invoiceTax.TaxName = taxo.Tax_Name + "%" + taxo.Tax_Percentage;
                invoiceTax.Amount = item.Amount;
                master.Taxes.Add(invoiceTax);

            }
            if (list.TotalNewTaxs == null)
            {
                master.NewTaxTotal = 0;
            }
            else
            {
                master.NewTaxTotal = list.TotalNewTaxs.Value;
            }
            master.totalBeforeDiscount = totalBeforeDiscount;
            master.Masterdiscount = Masterdiscount;
            master.totalAfterdiscount = totalAfterdiscount;
          
               
           
           
            master.MyDetails = vmlist.ToList();
            return master;
        }

        /// <summary>
        /// Print Reverse InvoiceMaster
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrintReveInvoiceMasterVM PrintReverseSalesInvoice(int id)
        {
            var db = new AccountingContainer();
            PrintReveInvoiceMasterVM master = new PrintReveInvoiceMasterVM();

            decimal totalBeforeDiscount = 0;
            decimal Masterdiscount = 0;
            decimal totalAfterdiscount = 0;
            decimal totalTax = 0;

            var list = db.ReverseSalesInvoiceMasters.Find(id);
            master.Master_Id = list.Master_Id;
            master.Invoice_Currency = list.Invoice_Currency;
            master.Invoice_Date = Convert.ToString(list.Invoice_Date);
            master.Due_Date = Convert.ToString(list.Due_Date);
            
           
            master.Master_Discount = list.Master_Discount.Value;
            var invoiceo = db.InvoiceMasters.Find(list.InvoiceMaster_Id);
            if (invoiceo.Number == null)
            {
                master.Invoice_Code = invoiceo.Serial_Number;
            }
            else
            {
                master.Invoice_Code = invoiceo.Number;
            }
            if (list.Number == null)
            {
                master.Serial_Number = list.Serial_Number;
            }
            else
            {
                master.Serial_Number = list.Number;
            }
            master.Total_Amount = list.Total_Amount;
            var result = db.Customers.Find(list.Customer_Id);
            if (result != null)
            {
                master.Customer_Name = db.Customers.Find(list.Customer_Id).Customer_Name;
            }
            master.company = db.Companies.Find(list.Company_Id);
            List<PrintReveInvoiceDetailsVM> vmlist = new List<PrintReveInvoiceDetailsVM> { };

            foreach (var item in list.ReveInvoiceDetails)
            {
                decimal detailAfterDiscount = 0;
                var invoice = new PrintReveInvoiceDetailsVM();
                var pro = db.Products.Find(item.Product_ID);
                invoice.Product_Code = pro.Prod_Code;
                invoice.Product_Name = pro.Prod_Name;
                invoice.Product_Description = pro.Prod_Descrip;
                invoice.Product_Model = pro.Prod_Mark;
                invoice.Product_Price = item.Product_Price;
                invoice.Product_Quantity = item.Product_Quantity;
                invoice.Total_Amount_Detail = item.Total_Amount;
                invoice.amountBeforeTax = invoice.Product_Quantity * invoice.Product_Price;
                totalBeforeDiscount += invoice.amountBeforeTax;
                invoice.discount = invoice.amountBeforeTax * (master.Master_Discount / 100);
                Masterdiscount += invoice.discount;
                detailAfterDiscount = invoice.amountBeforeTax - invoice.discount;
                totalAfterdiscount += detailAfterDiscount;
                invoice.Total_Tax = item.Detail_Total_Tax;
                totalTax += invoice.Total_Tax;
                invoice.MeasureUnit = db.Products.Find(item.Product_ID).Prod_Unet;
                invoice.tax = db.Taxes.Find(item.Product_Tax);



                vmlist.Add(invoice);

            }

            master.totalBeforeDiscount = totalBeforeDiscount;
            master.Masterdiscount = Masterdiscount;
            master.totalAfterdiscount = totalAfterdiscount;
            master.totalTax = totalTax;


            master.MyDetails = vmlist.ToList();
            return master;
        }
        /// <summary>
        /// Print Reverse SalesRecipt 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrintRevSalesReciptMasterVM PrintReverseSalesRecipt(int id)
        {
            var db = new AccountingContainer();
            PrintRevSalesReciptMasterVM master = new PrintRevSalesReciptMasterVM();

            decimal totalBeforeDiscount = 0;
            decimal Masterdiscount = 0;
            decimal totalAfterdiscount = 0;
            decimal totalTax = 0;



            var list = db.ReverseSalesReceiptMasters.Find(id);
            master.Master_Id = list.Master_Id;

            master.Invoice_Currency = "الريال السعودي";
            master.Invoice_Date = Convert.ToString(list.Invoice_Date);
        
            master.Master_Discount = list.Master_Discount.Value;
            
            var invoiceo = db.SalesReceiptMasters.Find(list.SalesRecipt_Id);
            if (invoiceo.Number == null)
            {
                master.Invoice_Code = invoiceo.Serial_Number;
            }
            else
            {
                master.Invoice_Code = invoiceo.Number;
            }
            if (list.Number == null)
            {
                master.Serial_Number = list.Serial_Number;
            }
            else
            {
                master.Serial_Number = list.Number;
            }
            master.Total_Amount = list.Total_Amount;
            master.storeId = list.Store_ID;
            master.Commition = list.Total_Payment_Method_Cost;
            master.PaymentMethod_Name = db.PaymentMethods.Find(list.PaymentMethod_Id).Payment_Type;
            if (list.Customer_Id == null || list.Customer_Id == 0)
            {
                master.Customer_Name = "";
            }
            else
            {
                var result = db.Customers.Find(list.Customer_Id);

                master.Customer_Name = db.Customers.Find(list.Customer_Id).Customer_Name;
            }
            master.company = db.Companies.Find(list.Company_Id);

            List<PrintRevSalesReciptDetailVM> vmlist = new List<PrintRevSalesReciptDetailVM> { };

            foreach (var item in list.ReverseSalesReceiptDetails)
            {
                decimal detailAfterDiscount = 0;
                var invoice = new PrintRevSalesReciptDetailVM();
                var prod_Id = db.ProductBatches.Find(item.Batch_ID).Product_ID;
                var prod = db.Products.Find(prod_Id);
                invoice.Product_Description = prod.Prod_Descrip;
                invoice.Product_Code = prod.Prod_Code;
                invoice.Product_Name = prod.Prod_Name;
                invoice.Product_Model = prod.Prod_Mark;
                invoice.Product_Price = item.Product_Price;
                invoice.Product_Quantity = item.Product_Quantity;
                invoice.Total_Amount_Detail = item.Total_Amount;
                invoice.amountBeforeTax = invoice.Product_Quantity * invoice.Product_Price;
                totalBeforeDiscount += invoice.amountBeforeTax;
                invoice.discount = invoice.amountBeforeTax * (master.Master_Discount / 100);

                Masterdiscount += invoice.discount;
                detailAfterDiscount = invoice.amountBeforeTax - invoice.discount;
                totalAfterdiscount += detailAfterDiscount;
                invoice.Total_Tax = item.Detail_Total_Tax;
                totalTax += invoice.Total_Tax;
                invoice.MeasureUnit = db.ProductBatches.Find(item.Batch_ID).Product.Prod_Unet;
                invoice.tax = db.Taxes.Find(item.Product_Tax);


                invoice.Total_Tax = item.Detail_Total_Tax;


                vmlist.Add(invoice);

            }
            master.totalBeforeDiscount = totalBeforeDiscount;
            master.Masterdiscount = Masterdiscount;
            master.totalAfterdiscount = totalAfterdiscount;
            master.totalTax = totalTax;

            master.MyDetails = vmlist.ToList();
            return master;
        }

        /// <summary>
        /// For print Purchase asset 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PurInvoiceMasterAccetVM PrintPurInvoiceAsset(int id)
        {
            var db = new AccountingContainer();
            PurInvoiceMasterAccetVM assetVM = new PurInvoiceMasterAccetVM();
            var Asseet = db.PurInvoiceMasters.Find(id);
            assetVM.Master_Id = Asseet.Master_Id;
            assetVM.Serial_Number = Asseet.Serial_Number;
            assetVM.Invoice_Date = Convert.ToString(Asseet.Invoice_Date);
            assetVM.Due_Date = Convert.ToString(Asseet.Due_Date);
            assetVM.Invoice_Currency = Asseet.Invoice_Currency;
            assetVM.Total_Amount = Asseet.Total_Amount;
            var supp = db.Suppliers.Find(Asseet.Supplier_Id);
            {
                if (supp != null)
                    assetVM.suuplier_Name = supp.Supp_Name;
            }

            List<PurInvoiceMasterAccetDetailsVM> AsstDe = new List<PurInvoiceMasterAccetDetailsVM> { };

            foreach (var item in Asseet.InvoiceDetails)
            {
                var AssetDetaills = new PurInvoiceMasterAccetDetailsVM();
                AssetDetaills.Details_Id = item.Details_Id;
                AssetDetaills.Product_Quantity = item.Product_Quantity;

                AssetDetaills.Product_Price = item.Product_Price;

                AssetDetaills.Total_Amount = item.Total_Amount;
                AssetDetaills.Asset_Name = db.FixedAssets.Find(item.Asset_Id.Value).FAsset_Description;

                AsstDe.Add(AssetDetaills);
            }

            assetVM.MyAsset = AsstDe.ToList();
            return assetVM;
        }

        /// <summary>
        /// For all revers of purchase invoice 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrintRecalPurchInvoiceMasterVM GetAllRecallPurshace(int id)
        {
            var db = new AccountingContainer();
            PrintRecalPurchInvoiceMasterVM RecalVM = new PrintRecalPurchInvoiceMasterVM();
            decimal totalBeforeDiscount = 0;
            decimal Masterdiscount = 0;
            decimal totalAfterdiscount = 0;
            decimal totalTax = 0;

            var RecalList = db.RecalPurchInvoiceMasters.Find(id);
            RecalVM.invoice_Code = db.PurInvoiceMasters.Find(RecalList.PurInvoiceMasterMaster_Id).Serial_Number;
            RecalVM.SuppId = RecalList.SupplierSuppId;
            RecalVM.Master_Id = RecalList.Master_Id;
            RecalVM.Register_Date = Convert.ToString(RecalList.Register_Date);

            var Purchuse = db.PurInvoiceMasters.Find(RecalList.PurInvoiceMasterMaster_Id);
            RecalVM.store_name = db.Stores.Find(Purchuse.Store_ID).Store_Name;
            var result = db.Suppliers.Find(Purchuse.Supplier_Id);
            if (result != null)
            {
                RecalVM.Supplier_Name = result.Supp_Name;
                RecalVM.supplierEmail = result.Supp_Mail;
                RecalVM.supplierfax = result.Supp_Fax;
                RecalVM.supplierPhone = result.Supp_Mobile;
                RecalVM.supplierTel = result.Supp_Phone;

            }
            if (Purchuse.Emp_Id != null || Purchuse.Emp_Id > 0)
            {
                var thisEMployee = db.Employees.Find(Purchuse.Emp_Id);
                RecalVM.EmployeeEmail = thisEMployee.Emp_Mail;
                RecalVM.EmployeeMobile = thisEMployee.Emp_Phone;
                RecalVM.EmployeeName = thisEMployee.Emp_Name;
            }
            RecalVM.Due_Date = Convert.ToString(RecalList.Due_Date);

            RecalVM.Recall_MPurch_Serial = RecalList.Recall_MPurch_Serial;

            RecalVM.Inv_TotalAmount = RecalList.Inv_TotalAmount;

            RecalVM.PI_Currency = RecalList.PI_Currency;
            RecalVM.Master_Discount = RecalList.Master_Discount;


            RecalVM.Inv_TotalTax = RecalList.Inv_TotalTax;
            var RecallSupplier = db.Suppliers.Find(RecalList.SupplierSuppId);
            {
                if (RecallSupplier != null)
                    RecalVM.Supplier_Name = RecallSupplier.Supp_Name;
            }
            RecalVM.company = db.Companies.Find(RecalList.CompanyComp_Id);
            List<PrintRecalPurchInvoiceDetailsVM> Details = new List<PrintRecalPurchInvoiceDetailsVM> { };
           
            
            
            foreach (var item in RecalList.RecalPurchInvoiceDetails)
            {
                var RecalDetails = new PrintRecalPurchInvoiceDetailsVM { };
                decimal detailAfterDiscount = 0;
                var prod = db.Products.Find(item.Product_ID);
                RecalDetails.Details_Id = item.Details_Id;
                RecalDetails.Product_Model = prod.Prod_Mark;
                RecalDetails.Product_Code = prod.Prod_Code;
                RecalDetails.Product_Quantity = item.Product_Quantity;
                RecalDetails.Product_Describtion = prod.Prod_Descrip;
                RecalDetails.product_Name = item.Product_Name;
                RecalDetails.Product_Price = Convert.ToDecimal(item.Product_Price);

                RecalDetails.Total_Amount = item.Total_Amount;
                RecalDetails.amountBeforeTax = RecalDetails.Product_Quantity * RecalDetails.Product_Price;
                totalBeforeDiscount += RecalDetails.amountBeforeTax;
                RecalDetails.discount = RecalDetails.amountBeforeTax * (RecalVM.Master_Discount / 100);
                Masterdiscount += RecalDetails.discount;
                detailAfterDiscount = RecalDetails.amountBeforeTax - RecalDetails.discount;
                totalAfterdiscount += detailAfterDiscount;
                RecalDetails.Total_Tax = item.Detail_Total_Tax;
                totalTax += RecalDetails.Total_Tax;
                RecalDetails.MeasureUnit = db.Products.Find(item.Product_ID).Prod_Unet;
                RecalDetails.tax = db.Taxes.Find(item.Product_Tax);





                Details.Add(RecalDetails);
            }

            RecalVM.totalBeforeDiscount = totalBeforeDiscount;
            RecalVM.Masterdiscount = Masterdiscount;
            RecalVM.totalAfterdiscount = totalAfterdiscount;
            RecalVM.totalTax = totalTax;

            RecalVM.MyDetailsRecall = Details.ToList();
            return RecalVM;
        }

        /// <summary>
        /// for report profits and loss
        /// </summary>
        /// <returns></returns>
        public static Profit_LossVM ProfitsAndLoss()
        {
            var db = new AccountingContainer();
            Profit_LossVM prof = new Profit_LossVM();

            prof.Account_Revenue = db.ChartOfAccountings.Where(c=>c.Account_Code.StartsWith("040")).ToList();           
            prof.Account_Expense = db.ChartOfAccountings.Where(c => c.Account_Code.StartsWith("050")).ToList();

            prof.Total_Loss = db.ChartOfAccountings.Where(c => c.Account_Code == "05").FirstOrDefault().Account_Balance.Value;
            prof.Total_Profit = db.ChartOfAccountings.Where(c => c.Account_Code == "04").FirstOrDefault().Account_Balance.Value;


            prof.Difference = prof.Total_Profit + prof.Total_Loss;
            return prof;
        }
        // For Customer balance detail
        public static CustomerBalanceDetailsVM CustomerBalanceDetails(int CustId)
        {
            var db = new AccountingContainer();
            CustomerBalanceDetailsVM customerBalance = new CustomerBalanceDetailsVM();

            var customer = db.Customers.Find(CustId);
            var Account = db.Accounts.Where(a => a.Account_Code == customer.Customer_Code).ToList();
            var AcountId = Account.FirstOrDefault().Accounts_Id;
            var AcountCode = Account.FirstOrDefault().Account_Code;
            if (customer.Customer_Code == AcountCode)
            {

                customerBalance.Customer_Name = customer.Customer_Name;
                var invoices = db.InvoiceMasters.Where(c => c.Customer_Id == CustId && c.Invoice_State == TransactionState.Post).ToList();
                if (invoices != null)
                {
                    customerBalance.InvoiceList = invoices;
                }
                var subscriptions = db.Subscriptions.Where(c => c.Customer_Id == CustId).ToList();
                if (subscriptions != null)
                {
                    customerBalance.Subscriptionslist = subscriptions;
                }
                var reverses = db.ReverseSalesInvoiceMasters.Where(c => c.Customer_Id == CustId && c.Invoice_State == TransactionState.Post).ToList();
                if (reverses != null)
                {
                    customerBalance.ReversInvoiceList = reverses;
                }
                var salesReceipts = db.SalesReceiptMasters.Where(c => c.Customer_Id == CustId && c.Invoice_State == TransactionState.Post).ToList();
                if (salesReceipts != null)
                {
                    customerBalance.SalesReceiptList = salesReceipts;
                }
                var reverseSales = db.ReverseSalesReceiptMasters.Where(c => c.Customer_Id == CustId && c.Invoice_State == TransactionState.Post).ToList();
                if (reverseSales != null)
                {
                    customerBalance.ReversSalesReceiptList = reverseSales;
                }
                var receipts = db.Receipts.Where(c => c.Customer_Id == CustId && c.Rec_state == TransactionState.Post).ToList();
                if (receipts != null)
                {
                    customerBalance.ReceiptsList = receipts;
                }
                var catchReceipts = db.CatchReceipts.Where(c => c.Child_AccountCredit_Id == CustId && c.CatchReceipt_State == TransactionState.Post).ToList();
                if (catchReceipts != null)
                {
                    customerBalance.CatchReceiptList = catchReceipts;
                }
                var spentRecipts = db.SpentRecipts.Where(c => c.Child_AccountDebit_Id == CustId && c.SpentReceipt_State == TransactionState.Post).ToList();
                if (spentRecipts != null)
                {
                    customerBalance.SpentReceiptList = spentRecipts;
                }
                var customerAccount = db.GLMasters.Where(a => a.Auto == false || a.GLType_Id.StartsWith("قيد افتتاحى")).ToList();

                customerBalance.Customer_Account = makeListForCustomerGL(customerAccount, AcountId);

                var Total_Balance = db.Accounts.Where(a => a.Account_Code == customer.Customer_Code).FirstOrDefault();
                customerBalance.Total_Balance = Total_Balance.Account_Balance.Value;
            }
            else
            {
                return customerBalance;
            }

            return customerBalance;
        }

        /// <summary>
        /// For customer details debite and credit report 
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns></returns>
        public static CustomerTransactionGettingDateVM getDetailsofCustomer(int CustID)
        {
            var db = new AccountingContainer();
            CustomerTransactionGettingDateVM custDetails = new CustomerTransactionGettingDateVM();
            custDetails.CustomerID = CustID;
            return custDetails;
        }
        /// <summary>
        /// this method  will take  ID  and  Start Date of  transaction of  Customer and  will  search  in GL  in  this  Transaction  and get  the  Details
        /// of this  Gls ,adding  all Blanace befor  this  date  in  the  (BeforDateBalance) watching  if it  is  Debit  or  Credit  and  then add   Debeit  if Balance  is  
        /// debit and  subtract  Credit  and  vis  vrsa
        /// 
        /// </summary>
        /// <param ID="ID-ofCustomer"> DateOFStartofnly="StartDate"</param>
        /// <returns>
        /// List of  GlDetails  with  custome Attributs
        /// </returns>
        public static CustomerTransactionFirstDataVM CustomerTransactionDetails(CustomerTransactionGettingDateVM CVM)
        {
            var db = new AccountingContainer();
            var thisCustomer = new CustomerTransactionFirstDataVM();
            if (CVM.Date == null)
            {
                CVM.Date = Convert.ToString(new DateTime(1995, 1, 1));

            }
            thisCustomer.CustomerDetails = new List<CustomerTransactionVM>();
            thisCustomer.FromDate = CVM.Date;
            var startDategot = Convert.ToDateTime(thisCustomer.FromDate);
            decimal totalBalanceBeforDate = 0;
            decimal CreditBalance = 0;
            decimal debitBalance = 0;
            var cust = db.Customers.Find(CVM.CustomerID);
            thisCustomer.AccountName = cust.Customer_Name;
            thisCustomer.AccountCode = cust.Customer_Code;
            var accountCustomer = db.Accounts.Where(a => a.Account_Code == cust.Customer_Code).FirstOrDefault();
            thisCustomer.company = db.Companies.FirstOrDefault();
            //we  will add  all details here  and  then start getting  which  we  want
            List<GLDetail> thisTransList = new List<GLDetail>();

            // var allGlDetails = db.GLDetails.Where(e => e.Account_CHild_Name == cust.Customer_Name).ToList();
            //  var allGLDetailsBeforStartDate = allGlDetails.Where()
            var thisdetailBefor = db.GLDetails.Where(e => e.GLMaster.GL_Date < startDategot && e.Account_Child_ID == accountCustomer.Accounts_Id).ToList();
           

                if (thisdetailBefor.Count != 0)
                {
                    foreach (var thisdetail in thisdetailBefor)
                    {
                        CreditBalance += thisdetail.GLDetail_Credit;
                        debitBalance += thisdetail.GLDetail_Debit;

                    }
                    if (debitBalance > CreditBalance)
                    {
                        thisCustomer.BalanceNameBefor = "مدين";


                        totalBalanceBeforDate =Math.Round( debitBalance,2,MidpointRounding.AwayFromZero) -Math.Round( CreditBalance,2,MidpointRounding.AwayFromZero);
                        thisCustomer.totalbeforDate = totalBalanceBeforDate;

                    }
                    else if (debitBalance < CreditBalance)
                    {
                        thisCustomer.BalanceNameBefor = "دائن";
                        totalBalanceBeforDate =Math.Round( CreditBalance,2,MidpointRounding.AwayFromZero) -Math.Round( debitBalance,2,MidpointRounding.AwayFromZero);
                        thisCustomer.totalbeforDate = totalBalanceBeforDate;
                    }
                    else
                    {
                        thisCustomer.BalanceNameBefor = null;
                        thisCustomer.totalbeforDate = 0;
                        totalBalanceBeforDate = 0;
                    }
                    if (thisCustomer.totalbeforDate < 0)
                    {
                        thisCustomer.totalbeforDate = thisCustomer.totalbeforDate * -1;
                        totalBalanceBeforDate = thisCustomer.totalbeforDate;
                    }


                }
            
            var GLdetialList = db.GLDetails.Where(e => e.GLMaster.GL_Date >= startDategot&& e.Account_Child_ID == accountCustomer.Accounts_Id).ToList();
      
                    foreach (var detail in GLdetialList)
                    {
                        try
                        {
                            if (detail.GLDetail_Debit > 0)
                            {
                                var thiscustdeb = new CustomerTransactionVM();
                                var thiscustcred = new CustomerTransactionVM();

                                var master = db.GLMasters.Find(detail.GLMasterGLMaster_Id);
                                if (master.GLCode.StartsWith("SAL-"))
                                {
                                    var thisinvoice = db.InvoiceMasters.Where(e => e.Serial_Number == master.GLCode).FirstOrDefault();
                                    thiscustdeb.Duedebit = thisinvoice.Due_Date.ToString();
                                    thiscustdeb.Discription = thisinvoice.Description;
                                }
                                if (master.GLCode.StartsWith("SALRC-"))
                                {
                                    var thisinvoice = db.SalesReceiptMasters.Where(e => e.Serial_Number == master.GLCode).FirstOrDefault();
                                    thiscustcred.Duecredit = null;
                                    thiscustdeb.Discription = thisinvoice.Description;
                                }
                                if (master.GLCode.StartsWith("POS-"))
                                {
                                    var thisinvoice = db.MasterPOSInvoices.Where(e => e.Serial_Number == master.GLCode).FirstOrDefault();
                                    thiscustcred.Duecredit = null;
                                    thiscustdeb.Discription = thisinvoice.Description;
                                }
                                if (master.GLCode.StartsWith("REC-"))
                                {
                                    var thisinvoice = db.Receipts.Where(e => e.Rec_Number == master.GLCode).FirstOrDefault();
                                    thiscustcred.Duecredit = null;
                                    thiscustdeb.Discription = thisinvoice.Rec_Description;
                                }
                                else
                                {
                                    thiscustdeb.Discription = detail.GLMaster.GL_Desc;
                                }

                                var detinMaster = db.GLDetails.Where(t => t.GLMasterGLMaster_Id == master.GLMaster_Id && t.GLDetail_Credit > 0).FirstOrDefault();
                                var thischart = db.ChartOfAccountings.Where(u => u.Account_Id == detinMaster.Account_Id).FirstOrDefault();

                                thiscustdeb.Balance_Debit = detail.GLDetail_Debit;
                                thiscustdeb.Debit_Date = master.GL_Date.ToString();
                                thiscustdeb.Balance_Credit = 0;
                                //thiscustdeb.Credit_Date = null;
                                //thiscustdeb.Duecredit = null;
                                thiscustdeb.Debit_COde = master.GLCode;
                                thiscustdeb.Debit_Desc = master.GLType_Id;
                                if (thisCustomer.BalanceNameBefor == "دائن")
                                {
                                    thiscustdeb.totalBalance = totalBalanceBeforDate - thiscustdeb.Balance_Debit;
                                    totalBalanceBeforDate = thiscustdeb.totalBalance;
                                }
                                else
                                {
                                    thiscustdeb.totalBalance = totalBalanceBeforDate + thiscustdeb.Balance_Debit;
                                    totalBalanceBeforDate = thiscustdeb.totalBalance;
                                }

                                //totalBalanceBeforDate += newLedgerdeb.Balance_Debit;

                                thisCustomer.CustomerDetails.Add(thiscustdeb);
                            }
                            else if (detail.GLDetail_Credit > 0)
                            {
                                var thiscustdeb = new CustomerTransactionVM();
                                var thiscustcred = new CustomerTransactionVM();
                                var master = db.GLMasters.Find(detail.GLMasterGLMaster_Id);
                                if (master.GLCode.StartsWith("SAL-"))
                                {
                                    var thisinvoice = db.InvoiceMasters.Where(e => e.Serial_Number == master.GLCode).FirstOrDefault();
                                    thiscustcred.Duecredit = thisinvoice.Due_Date.ToString();
                                    thiscustcred.Discription = thisinvoice.Description;
                                }

                                if (master.GLCode.StartsWith("SALRC-"))
                                {
                                    var thisinvoice = db.SalesReceiptMasters.Where(e => e.Serial_Number == master.GLCode).FirstOrDefault();
                                    thiscustcred.Duecredit = null;
                                    thiscustcred.Discription = thisinvoice.Description;
                                }
                                if (master.GLCode.StartsWith("POS-"))
                                {
                                    var thisinvoice = db.MasterPOSInvoices.Where(e => e.Serial_Number == master.GLCode).FirstOrDefault();
                                    thiscustcred.Duecredit = null;
                                    thiscustcred.Discription = thisinvoice.Description;
                                }
                                if (master.GLCode.StartsWith("REC-"))
                                {
                                    var thisinvoice = db.Receipts.Where(e => e.Rec_Number == master.GLCode).FirstOrDefault();
                                    thiscustcred.Duecredit = null;
                                    thiscustcred.Discription = thisinvoice.Rec_Description;
                                }
                                else
                                {
                                    thiscustcred.Discription = detail.GLMaster.GL_Desc;
                                }
                                var detinMaster = db.GLDetails.Where(t => t.GLMasterGLMaster_Id == master.GLMaster_Id && t.GLDetail_Debit > 0).FirstOrDefault();
                                var thischart = db.ChartOfAccountings.Where(u => u.Account_Id == detinMaster.Account_Id).FirstOrDefault();

                                thiscustcred.Balance_Debit = 0;
                                thiscustcred.Balance_Credit = detail.GLDetail_Credit;
                                thiscustcred.Credit_Date = master.GL_Date.ToString();
                                //thiscustcred.Duedebit = null;
                                //thiscustcred.Debit_Date = null;
                                thiscustcred.Credit_COde = master.GLCode;
                                thiscustcred.Credit_Desc = master.GLType_Id;
                                
                                // newledgerCred.totalBalance = totalBalanceBeforDate - newledgerCred.Balance_Credit;
                                if (thisCustomer.BalanceNameBefor == "دائن")
                                {
                                    thiscustcred.totalBalance = totalBalanceBeforDate +thiscustcred.Balance_Credit;
                                    totalBalanceBeforDate = thiscustcred.totalBalance;
                                }
                                else
                                {
                                    thiscustcred.totalBalance =totalBalanceBeforDate -thiscustcred.Balance_Credit;
                                    totalBalanceBeforDate = thiscustcred.totalBalance;
                                }
                                //totalBalanceBeforDate -= newledgerCred.Balance_Credit;
                                thisCustomer.CustomerDetails.Add(thiscustcred);
                            }


                        }
                        catch (Exception e)
                        {
                            var thiscustcred = new CustomerTransactionVM();
                            thiscustcred.Duedebit = "";
                            thiscustcred.Balance_Debit = 0;
                            thiscustcred.Balance_Credit = 0;
                            thiscustcred.Credit_Date = "";
                            thiscustcred.Duecredit = "";
                            thiscustcred.Debit_Date = "";
                            thiscustcred.Credit_COde = "";
                            thiscustcred.Credit_Desc = "";
                            thiscustcred.totalBalance = 0;
                            totalBalanceBeforDate = 0;
                            thisCustomer.CustomerDetails.Add(thiscustcred);
                            return thisCustomer;
                        }
                    

                


            }
            return thisCustomer;



        }
        /// <summary>
        /// this  method is  the  same  like(CustomerTransactionDetails) i used  the  same VM  Nvermind  what  is  naming  convention becuase  they  two  are  the  same  but 
        /// the  diffrance  is  supplier  and  Customer  so  no  need  to  invcrease  another  2  VM   with no  reason
        /// </summary>
        /// <param name="CVM"></param>
        /// <returns></returns>
        public static CustomerTransactionFirstDataVM SupplierTransactionDetails(CustomerTransactionGettingDateVM CVM)
        {
            var db = new AccountingContainer();
            var thisCustomer = new CustomerTransactionFirstDataVM(); 
            if (CVM.Date == null)
            {
                CVM.Date = Convert.ToString(new DateTime(1995, 1, 1));


            }


            thisCustomer.CustomerDetails = new List<CustomerTransactionVM>();
            thisCustomer.FromDate = CVM.Date;
            var startDategot = Convert.ToDateTime(thisCustomer.FromDate);
            decimal totalBalanceBeforDate = 0;
            decimal CreditBalance = 0;
            decimal debitBalance = 0;
            var cust = db.Suppliers.Find(CVM.CustomerID);
            thisCustomer.AccountName = cust.Supp_Name;
            var accountSupplier = db.Accounts.Where(a => a.Account_Code == cust.Supp_Code).FirstOrDefault();

            thisCustomer.company = db.Companies.FirstOrDefault();
            //we  will add  all details here  and  then start getting  which  we  want
            List<GLDetail> thisTransList = new List<GLDetail>();

            // var allGlDetails = db.GLDetails.Where(e => e.Account_CHild_Name == cust.Customer_Name).ToList();
            //  var allGLDetailsBeforStartDate = allGlDetails.Where()
            var thisdetailBefor = db.GLDetails.Where(e => e.GLMaster.GL_Date < startDategot&& e.Account_Child_ID == accountSupplier.Accounts_Id).ToList();
            

                if (thisdetailBefor.Count != 0)
                {
                    foreach (var thisdetail in thisdetailBefor)
                    {
                        CreditBalance += thisdetail.GLDetail_Credit;
                        debitBalance += thisdetail.GLDetail_Debit;

                    }
                    if (debitBalance > CreditBalance)
                    {
                        thisCustomer.BalanceNameBefor = "مدين";


                        totalBalanceBeforDate = debitBalance - CreditBalance;
                        thisCustomer.totalbeforDate = totalBalanceBeforDate;

                    }
                    else if (debitBalance < CreditBalance)
                    {
                        thisCustomer.BalanceNameBefor = "دائن";
                        totalBalanceBeforDate = CreditBalance - debitBalance;
                        thisCustomer.totalbeforDate = totalBalanceBeforDate;
                    }
                    else
                    {
                        thisCustomer.BalanceNameBefor = null;
                        thisCustomer.totalbeforDate = 0;
                        totalBalanceBeforDate = 0;
                    }
                    if (thisCustomer.totalbeforDate < 0)
                    {
                        thisCustomer.totalbeforDate = thisCustomer.totalbeforDate * -1;
                        totalBalanceBeforDate = thisCustomer.totalbeforDate;
                    }


                }
            
            var GLdetialList = db.GLDetails.Where(e => e.GLMaster.GL_Date >= startDategot&& e.Account_Child_ID == accountSupplier.Accounts_Id).ToList();
            
                    foreach (var detail in GLdetialList)
                    {
                        try
                        {
                            if (detail.GLDetail_Debit > 0)
                            {
                                var thiscustdeb = new CustomerTransactionVM();
                                var thiscustcred = new CustomerTransactionVM();

                                var master = db.GLMasters.Find(detail.GLMasterGLMaster_Id);
                                if (master.GLCode.StartsWith("PUR"))
                                {
                                    var thisinvoice = db.PurInvoiceMasters.Where(e => e.Serial_Number == master.GLCode).FirstOrDefault();
                                    thiscustdeb.Duedebit = thisinvoice.Due_Date.ToString();
                                    thiscustdeb.Discription = thisinvoice.Description;
                                }
                                if (master.GLCode.StartsWith("GLST"))
                                {
                                    var thisinvoice = db.GLMasters.Where(e => e.GLCode == master.GLCode).FirstOrDefault();
                                    thiscustdeb.Discription = thisinvoice.GL_Desc;
                                }
                                if (master.GLCode.StartsWith("SPRE"))
                                {
                                    var thisinvoice = db.SpentRecipts.Where(e => e.SpentReceipt_Code == master.GLCode).FirstOrDefault();
                                    thiscustdeb.Discription = thisinvoice.SpentReceipt_Description;
                                }
                                if (master.GLCode.StartsWith("PAY"))
                                {
                                    var thisinvoice = db.Payments.Where(e => e.Pay_Number == master.GLCode).FirstOrDefault();
                                    thiscustdeb.Discription = thisinvoice.Rec_Description;
                                }
                                else
                                {
                                    thiscustcred.Discription = master.GL_Desc;
                                }
                                var detinMaster = db.GLDetails.Where(t => t.GLMasterGLMaster_Id == master.GLMaster_Id && t.GLDetail_Credit > 0).FirstOrDefault();
                                var thischart = db.ChartOfAccountings.Where(u => u.Account_Id == detinMaster.Account_Id).FirstOrDefault();

                                thiscustdeb.Balance_Debit = detail.GLDetail_Debit;
                                thiscustdeb.Debit_Date = master.GL_Date.ToString();
                                thiscustdeb.Balance_Credit = 0;
                                thiscustdeb.Credit_Date = null;
                                thiscustdeb.Duecredit = null;
                                thiscustdeb.Debit_COde = master.GLCode;
                                thiscustdeb.Debit_Desc = master.GLType_Id;
                               
                                if (thisCustomer.BalanceNameBefor == "دائن")
                                {
                                    thiscustdeb.totalBalance = totalBalanceBeforDate - thiscustdeb.Balance_Debit;
                                    totalBalanceBeforDate = thiscustdeb.totalBalance;
                                }
                                else
                                {
                                    thiscustdeb.totalBalance = totalBalanceBeforDate + thiscustdeb.Balance_Debit;
                                    totalBalanceBeforDate = thiscustdeb.totalBalance;
                                }

                                thisCustomer.CustomerDetails.Add(thiscustdeb);
                            }
                            else if (detail.GLDetail_Credit > 0)
                            {
                                var thiscustdeb = new CustomerTransactionVM();
                                var thiscustcred = new CustomerTransactionVM();
                                var master = db.GLMasters.Find(detail.GLMasterGLMaster_Id);
                                if (master.GLCode.StartsWith("PUR"))
                                {
                                    var thisinvoice = db.PurInvoiceMasters.Where(e => e.Serial_Number == master.GLCode).FirstOrDefault();
                                    thiscustcred.Duecredit = thisinvoice.Due_Date.ToString();
                                    thiscustcred.Discription = thisinvoice.Description;
                                }
                                if (master.GLCode.StartsWith("GLST"))
                                {
                                    var thisinvoice = db.GLMasters.Where(e => e.GLCode == master.GLCode).FirstOrDefault();
                                    thiscustcred.Discription = thisinvoice.GL_Desc;
                                }
                                if (master.GLCode.StartsWith("SPRE"))
                                {
                                    var thisinvoice = db.SpentRecipts.Where(e => e.SpentReceipt_Code == master.GLCode).FirstOrDefault();
                                    thiscustcred.Discription = thisinvoice.SpentReceipt_Description;
                                }
                                if (master.GLCode.StartsWith("PAY"))
                                {
                                    var thisinvoice = db.Payments.Where(e => e.Pay_Number == master.GLCode).FirstOrDefault();
                                    thiscustcred.Discription = thisinvoice.Rec_Description;
                                }
                                else
                                {
                                    thiscustcred.Discription = master.GL_Desc;
                                }
                                var detinMaster = db.GLDetails.Where(t => t.GLMasterGLMaster_Id == master.GLMaster_Id && t.GLDetail_Debit > 0).FirstOrDefault();
                                var thischart = db.ChartOfAccountings.Where(u => u.Account_Id == detinMaster.Account_Id).FirstOrDefault();

                                thiscustcred.Balance_Debit = 0;
                                thiscustcred.Balance_Credit = detail.GLDetail_Credit;
                                thiscustcred.Credit_Date = master.GL_Date.ToString();
                                thiscustcred.Duedebit = null;
                                thiscustcred.Debit_Date = null;
                                thiscustcred.Credit_COde = master.GLCode;
                                thiscustcred.Credit_Desc = master.GLType_Id;
                              
                                // newledgerCred.totalBalance = totalBalanceBeforDate - newledgerCred.Balance_Credit;
                                if (thisCustomer.BalanceNameBefor == "دائن")
                                {
                                    thiscustcred.totalBalance = totalBalanceBeforDate + thiscustcred.Balance_Credit;
                                    totalBalanceBeforDate = thiscustcred.totalBalance;
                                }
                                else
                                {
                                    thiscustcred.totalBalance = totalBalanceBeforDate - thiscustcred.Balance_Credit;
                                    totalBalanceBeforDate = thiscustcred.totalBalance;
                                }
                                //totalBalanceBeforDate -= newledgerCred.Balance_Credit;
                                thisCustomer.CustomerDetails.Add(thiscustcred);
                            }


                        }
                        catch (Exception e)
                        {
                            var thiscustcred = new CustomerTransactionVM();
                            thiscustcred.Duedebit = "";
                            thiscustcred.Balance_Debit = 0;
                            thiscustcred.Balance_Credit = 0;
                            thiscustcred.Credit_Date = "";
                            thiscustcred.Duecredit = "";
                            thiscustcred.Debit_Date = "";
                            thiscustcred.Credit_COde = "";
                            thiscustcred.Credit_Desc = "";
                            thiscustcred.totalBalance = 0;
                            totalBalanceBeforDate = 0;
                            thisCustomer.CustomerDetails.Add(thiscustcred);
                            return thisCustomer;
                        }
                    }

                


            
            return thisCustomer;



        }
        /// <summary>
        /// for report  Income Statement
        /// </summary>
        /// <param name="incomeVM"></param>
        /// <returns></returns>
        public static IncomeStatementVM IncomeStatement(IncomeStatementVM incomeVM)
        {
            var db = new AccountingContainer();
            IncomeStatementVM income = new IncomeStatementVM();
            income.To_Date = incomeVM.To_Date;
            income.From_Date = incomeVM.From_Date;
            income.WithOutOFFGLs= incomeVM.WithOutOFFGLs;
            decimal TotalexpCredit = 0;
            decimal Totalexpdepit = 0;
            decimal TotalRevCredit = 0;
            decimal TotalRevdepit = 0;
            if (income.From_Date == null)
            {
                income.From_Date =Convert.ToString( new DateTime(1995, 1, 1));


            }

            if (income.To_Date==null)
            {
                income.To_Date = Convert.ToString(new DateTime(2200, 12, 30));

            }
            var start = Convert.ToDateTime(Convert.ToDateTime(income.From_Date).ToShortDateString());
            var end = Convert.ToDateTime(Convert.ToDateTime(income.To_Date).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);


            List<GLDetail> myGLDetail = new List<GLDetail> { };

            var GlDetail = db.GLDetails.Where(a => a.GLMaster.GL_Date <= end);
          
            var date = GlDetail.Where(e => e.GLMaster.GL_Date >= start && e.GLMaster.GL_Date <= end).ToList();
            

            if (income.WithOutOFFGLs == false)
            {
                var OffGL = GlDetail.Where(a => a.GLMaster.GLCode.StartsWith("GLOF-")).ToList();

                date = date.Except(OffGL).ToList();


            }
            income.Account_Revenue = makeListForRevenu(date);
            income.Account_Expense = makeListForExp(date);
            income.ExpenseSubLedger = income.Account_Expense.Where(a => a.Account_Id == 105).ToList();
            income.Account_Expense.Remove(income.ExpenseSubLedger.FirstOrDefault());
            if (income.ExpenseSubLedger.FirstOrDefault() != null)
            {
                income.salesCost = income.ExpenseSubLedger.FirstOrDefault().difference;
            }
            else { income.salesCost = 0; }
           //calculate Expenses 
                TotalexpCredit = income.Account_Expense.Select(a=>a.GLDetail_Credit).Sum();
                Totalexpdepit = income.Account_Expense.Select(a => a.GLDetail_Debit).Sum();
            
            income.Total_Expense = Totalexpdepit - TotalexpCredit;
           
                TotalRevCredit = income.Account_Revenue.Select(a => a.GLDetail_Credit).Sum();
                TotalRevdepit = income.Account_Revenue.Select(a => a.GLDetail_Debit).Sum();
            
            income.Total_Revenue = TotalRevdepit - TotalRevCredit;

            income.Difference = income.Total_Revenue + income.Total_Expense + income.salesCost;
            income.collectofRevenue = income.Total_Revenue + income.salesCost;
            income.company = db.Companies.FirstOrDefault();
         
            return income;
        }
        public static TrialBalncePostVM getDatesToMake()
        {
            var dates = new TrialBalncePostVM();
            return dates;
        }
        /// <summary>
        /// For traill balance report
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static TrialBalncePostVM MakeTrailBalance(TrialBalncePostVM date)
        {
           
            var Trial = new TrialBalncePostVM();
           
            if (date.Start_Date == null)
            {
                date.Start_Date = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (date.End_Date == null)
            {
                date.End_Date = Convert.ToString(new DateTime(2200, 12, 30));

            }
            DateTime start = Convert.ToDateTime(Convert.ToDateTime(date.Start_Date).ToShortDateString());
            var End = Convert.ToDateTime(Convert.ToDateTime(date.End_Date).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);
            


            Trial = makeTrialOthermethod(date);
           
            return Trial;

        }
        /// <summary>
        /// For get total balance for every account.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static TrialBalncePostVM makeTrialOthermethod(TrialBalncePostVM date)
        {

            var db = new AccountingContainer();
            decimal starcredit = 0;
            decimal stardepit = 0;
            decimal movmentcredit = 0;
            decimal movmenrdepit = 0;
            decimal purecredit = 0;
            decimal puredepit = 0;
            decimal endcredit = 0;
            decimal enddepit = 0;
            decimal pureFinalDepit = 0;
            decimal pureFinalCredit = 0;


            var post = new TrialBalncePostVM();

            var ListaOfBallanceTrial = new List<BalanceTrialVMAttribute> { };

            var star = Convert.ToDateTime(Convert.ToDateTime(date.Start_Date).ToShortDateString());
            var End = Convert.ToDateTime(Convert.ToDateTime(date.End_Date).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);
            var details = db.GLDetails.ToList();
            var alldata = details.Where(e => e.GLMaster.GL_Date >= star && e.GLMaster.GL_Date <= End).ToList();
                var glmovment = alldata.Where(e => e.GLMaster.GLType_Id != "قيد افتتاحى" && e.GLMaster.GLType_Id != "رصيد افتتاحي اصناف").ToList();

            var StartBalanceList = alldata.Where(e => e.GLMaster.GLType_Id == "قيد افتتاحى" || e.GLMaster.GLType_Id == "رصيد افتتاحي اصناف").ToList();
            var glOffa = new List<GLDetail>();
            try
            {
                 glOffa = alldata.Where(e => e.GLMaster.GLCode.StartsWith("GLOF-")).ToList();
            }
            catch
            {
                glOffa= new List<GLDetail>();
            }
            if (glOffa.Count() > 0)
            {
                if (date.WithOutOFFGLs == false)
                {
                    var glOff = glmovment.Where(a => a.GLMaster.GLCode.StartsWith("GLOF-")).ToList();
                    
                    glmovment = glmovment.Except(glOff).ToList();
                    var glstartOff = StartBalanceList.Where(a => a.GLMaster.GLCode.StartsWith("GLOF-")).ToList();
                    StartBalanceList = StartBalanceList.Except(glstartOff).ToList();


                }
            }
           
            var glDatailMovment = makeListForBalanceTrail(glmovment);
            

            var glDatailstartbalance = makeListForBalanceTrail(StartBalanceList);
            var TotalList = glmovment.Concat(StartBalanceList).ToList();
            var total_Detail = makeListForBalanceTrail(TotalList);
            foreach (var item in total_Detail)
            {
                var colum = new BalanceTrialVMAttribute();
                colum.Account_Name = item.Account_Name;
                colum.Account = db.ChartOfAccountings.Where(a => a.Account_Code == item.Account_Code).FirstOrDefault();
                colum.Account_Code = item.Account_Code;
                colum.Account_Id = item.Account_Id;
                var startbalanceForThatAccount = glDatailstartbalance.Where(a => a.Account_Id == item.Account_Id).ToList();
                if (startbalanceForThatAccount.FirstOrDefault() == null)
                {
                    colum.startBallanceDebit = 0;
                    colum.startBallanceCredit = 0;
                }
                else
                {
                    colum.startBallanceDebit = startbalanceForThatAccount.Select(a=>a.GLDetail_Debit).Sum();
                    colum.startBallanceCredit = startbalanceForThatAccount.Select(a => a.GLDetail_Credit).Sum();
                    //foreach (var startballance in startbalanceForThatAccount)
                    //{
                    //    colum.startBallanceDebit += startballance.GLDetail_Debit;
                    //    colum.startBallanceCredit += startballance.GLDetail_Credit;
                    //}
                }
                var MovmentbalanceForThatAccount = glDatailMovment.Where(a => a.Account_Id == item.Account_Id).ToList();
                if (MovmentbalanceForThatAccount.FirstOrDefault() == null)
                {
                    colum.MovmentBallanceCredit = 0;
                    colum.MovmentBallanceDebit = 0;
                }
                else
                {
                    colum.MovmentBallanceDebit = MovmentbalanceForThatAccount.Select(a=>a.GLDetail_Debit).Sum();
                    colum.MovmentBallanceCredit = MovmentbalanceForThatAccount.Select(a => a.GLDetail_Credit).Sum();
                    //foreach (var startballance in MovmentbalanceForThatAccount)
                    //{
                    //    colum.MovmentBallanceDebit += startballance.GLDetail_Debit;
                    //    colum.MovmentBallanceCredit += startballance.GLDetail_Credit;
                    //}
                }
                var endballance = total_Detail.Where(a => a.Account_Id == item.Account_Id).ToList();
                if (endballance.FirstOrDefault() == null)
                {
                    colum.Enddepit = 0;
                    colum.EndCredit = 0;
                }
                else
                {
                    colum.Enddepit = endballance.Select(a=>a.GLDetail_Debit).Sum();
                    colum.EndCredit = endballance.Select(a => a.GLDetail_Credit).Sum();
                    //foreach (var startballance in endballance)
                    //{
                    //    colum.Enddepit += startballance.GLDetail_Debit;
                    //    colum.EndCredit += startballance.GLDetail_Credit;
                    //}
                }
                if (colum.MovmentBallanceDebit >= colum.MovmentBallanceCredit)
                {
                    colum.puredepit = colum.MovmentBallanceDebit - colum.MovmentBallanceCredit;
                    colum.pureCredit = 0;
                }
                else
                {
                    colum.pureCredit = colum.MovmentBallanceCredit - colum.MovmentBallanceDebit;
                    colum.puredepit = 0;

                }
                if (colum.Enddepit >= colum.EndCredit)
                {
                    colum.pureFinaldepit = colum.Enddepit - colum.EndCredit;
                    colum.pureFinalCredit = 0;
                }
                else
                {
                    colum.pureFinalCredit = colum.EndCredit - colum.Enddepit;
                    colum.pureFinaldepit = 0;

                }

                ListaOfBallanceTrial.Add(colum);
                starcredit += colum.startBallanceCredit;

                stardepit += colum.startBallanceDebit;
                movmentcredit += colum.MovmentBallanceCredit;
                movmenrdepit += colum.MovmentBallanceDebit;
                purecredit += colum.pureCredit;
                puredepit += colum.puredepit;
                endcredit += colum.EndCredit;
                enddepit += colum.Enddepit;
                pureFinalDepit += colum.pureFinaldepit;
                pureFinalCredit += colum.pureFinalCredit;

            }
            var asset = ListaOfBallanceTrial.Where(a=>a.Account_Code.StartsWith("01")).OrderByDescending(a=>a.Account.ChartOfAccountingAccount_Id).ToList();
            var enemy = ListaOfBallanceTrial.Where(a => a.Account_Code.StartsWith("02")).OrderByDescending(a => a.Account.ChartOfAccountingAccount_Id).ToList();
            var layaplity = ListaOfBallanceTrial.Where(a => a.Account_Code.StartsWith("03")).OrderByDescending(a => a.Account.ChartOfAccountingAccount_Id).ToList();
            var revenu = ListaOfBallanceTrial.Where(a => a.Account_Code.StartsWith("04")).OrderByDescending(a => a.Account.ChartOfAccountingAccount_Id).ToList();
            var exp = ListaOfBallanceTrial.Where(a => a.Account_Code.StartsWith("05")).OrderByDescending(a => a.Account.ChartOfAccountingAccount_Id).ToList();
            var wasset = ListaOfBallanceTrial.Where(a => a.Account_Code.StartsWith("06")).OrderByDescending(a => a.Account.ChartOfAccountingAccount_Id).ToList();

            asset.AddRange(enemy);
            asset.AddRange(layaplity);
            asset.AddRange(revenu);
            asset.AddRange(exp);
            asset.AddRange(wasset);
            post.ballanceAttrribute = asset;
            post.startBallanceCreditSUM = starcredit;
            post.startBallanceDebitSUM = stardepit;
            post.MovmentBallanceCreditSUM = movmentcredit;
            post.MovmentBallanceDebitSUM = movmenrdepit;
            post.pureCreditSUM = purecredit;
            post.puredepitSUM = puredepit;
            post.EndCreditSUM = endcredit;
            post.EnddepitSUM = enddepit;
            post.Start_Date = date.Start_Date;
            post.End_Date = date.End_Date;
            post.PureEndCreditSUM = pureFinalCredit;
            post.PureEnddepitSUM = pureFinalDepit;
            post.company = db.Companies.FirstOrDefault();
            //GLMaster.makeEvaluationOfStore(post);
            //post = makeTrialOthermethodForMakeEvaluationGL(date);


            return post;
        }
        /// <summary>
        /// Return a list of balance account 
        /// </summary>
        /// <param name="glMasterList"></param>
        /// <returns></returns>
        public static List<GLDetailVM> makeListForBalanceTrail(List<GLDetail> glDetail)
        {
            var db = new AccountingContainer();
            
            var glDetailVM = new List<GLDetailVM>();
            
            var accountidList = glDetail.Select(a => a.Account_Id).Distinct().ToList();
            foreach (var account in accountidList)
            {
                var accountone = db.ChartOfAccountings.Find(account);
                var GldetialVm = new GLDetailVM();
                var glAccountList = glDetail.Where(a => a.Account_Id == account).ToList();
                

                    GldetialVm.Account_Id = account;
                    GldetialVm.GLDetail_Credit = glAccountList.Select(z=>z.GLDetail_Credit).Sum();
                    GldetialVm.GLDetail_Debit = glAccountList.Select(z => z.GLDetail_Debit).Sum();
                    GldetialVm.Account_Name = accountone.Account_Name;
                    GldetialVm.Account_Code = accountone.Account_Code;
                
                glDetailVM.Add(GldetialVm);
            
            }

            return glDetailVM;
        }
        /// <summary>
        /// Return a list for balance expenese
        /// </summary>
        /// <param name="glMasterList"></param>
        /// <returns></returns>
        public static List<GLDetailVM> makeListForExp(List<GLDetail> glDetail)
        {
            var db = new AccountingContainer();
            
            var glDetailVM = new List<GLDetailVM>();
           
            var incomList = glDetail.Where(a => a.ChartOfAccounting.Account_Code.StartsWith("05")).ToList();
            var accountidList = incomList.Select(a => a.Account_Id).Distinct();
            foreach (var account in accountidList)
            {
                //if (account == 111)
                //{
                //    var r = 12;
                //}
                var GldetialVm = new GLDetailVM();
                var glAccountList = glDetail.Where(a => a.Account_Id == account).ToList();
                var account1 = db.ChartOfAccountings.Find(account);
               

                    GldetialVm.Account_Id = account;
                    GldetialVm.GLDetail_Credit = glAccountList.Select(a=>a.GLDetail_Credit).Sum();
                    GldetialVm.GLDetail_Debit = glAccountList.Select(a => a.GLDetail_Debit).Sum();
                    GldetialVm.Account_Name = account1.Account_Name;
                    GldetialVm.Account_Code = account1.Account_Code;
                
                GldetialVm.difference = GldetialVm.GLDetail_Debit - GldetialVm.GLDetail_Credit;
                glDetailVM.Add(GldetialVm);

            }

            return glDetailVM;
        }
        /// <summary>
        /// Return a list for balance revenue
        /// </summary>
        /// <param name="glMasterList"></param>
        /// <returns></returns>
        public static List<GLDetailVM> makeListForRevenu(List<GLDetail> glDetail)
        {
            var db = new AccountingContainer();
            
            var glDetailVM = new List<GLDetailVM>();
            
            var incomList = glDetail.Where(a => a.ChartOfAccounting.Account_Code.StartsWith("04")).ToList();
            var accountidList = incomList.Select(a => a.Account_Id).Distinct();
            foreach (var account in accountidList)
            {
                var GldetialVm = new GLDetailVM();
                var account1 = db.ChartOfAccountings.Find(account);
                var glAccountList = glDetail.Where(a => a.Account_Id == account).ToList();
                

                    GldetialVm.Account_Id = account;
                    GldetialVm.GLDetail_Credit = glAccountList.Select(a=>a.GLDetail_Credit).Sum();
                    GldetialVm.GLDetail_Debit = glAccountList.Select(a=>a.GLDetail_Debit).Sum();
                    GldetialVm.Account_Name = account1.Account_Name;
                    GldetialVm.Account_Code = account1.Account_Code;
                
                GldetialVm.difference = GldetialVm.GLDetail_Debit - GldetialVm.GLDetail_Credit;
                glDetailVM.Add(GldetialVm);

            }

            return glDetailVM;
        }

        /// <summary>
        /// for creation mean prepation for a method
        /// </summary>
        /// <returns></returns>
        public static ReportMasterGeneralBudgetVM CreateForeReport()
        {
            var db = new AccountingContainer();

            var reportBadget = new ReportMasterGeneralBudgetVM();
            return reportBadget;
        }

        /// <summary>
        /// This method used an another method (makeTrialOthermethod) to return a budget 
        /// </summary>
        /// <param name="vmDate"></param>
        /// <returns></returns>
        public static ReportMasterGeneralBudgetVM ReportGeneralBadget(ReportMasterGeneralBudgetVM vmDate)
        {
            var db = new AccountingContainer();

            ReportMasterGeneralBudgetVM Badget = new ReportMasterGeneralBudgetVM();
            TrialBalncePostVM trialba = new TrialBalncePostVM();
            TrialBalncePostVM trialba2 = new TrialBalncePostVM();

            trialba.Start_Date = vmDate.StartDate;
            trialba.End_Date = vmDate.EndDate;
            if (trialba.Start_Date == null)
            {
                trialba.Start_Date = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (trialba.End_Date == null)
            {
                trialba.End_Date = Convert.ToString(new DateTime(2200, 12, 30));

            }
            if (vmDate.WithOutOFFGLs==true)
            {
                trialba.WithOutOFFGLs = true;

            }
            trialba2 = Reports.makeTrialOthermethod(trialba);
            var gl= trialba2.ballanceAttrribute.ToList();
            var chart = db.ChartOfAccountings.ToList();
            foreach(var account in chart)
            {
                account.BalanceSheetValue = 0;

                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();

            }

            GetBalanceForBallanceSheet(gl);

            PrintChartOfAccount chartList = new PrintChartOfAccount();
           var Chart= chartList.getAllBalanceSheet("0");
            Badget.AssetList = Chart.Where(a => a.Account_Code.StartsWith("01")).Where(a => a.Account_Balance != 0||a.Total!=0).ToList();
            Badget.Libality = Chart.Where(a => a.Account_Code.StartsWith("02")).Where(a => a.Account_Balance != 0 || a.Total != 0).ToList();
            Badget.PropertyRights= Chart.Where(a => a.Account_Code.StartsWith("03")).Where(a=>a.Account_Balance!=0 || a.Total != 0).ToList();
            IncomeStatementVM income = new IncomeStatementVM();
            income.From_Date= vmDate.StartDate;
            income.To_Date = vmDate.EndDate;
            if (vmDate.WithOutOFFGLs == true)
            {
                income.WithOutOFFGLs = true;

            }
            var ProfitAndLoss= Reports.IncomeStatement(income);
            Badget.Profit = ProfitAndLoss.Difference;
            var LibalitySum = Chart.Where(a => a.Account_Code == "02").FirstOrDefault().Total;
            var PropertRightSum = Chart.Where(a => a.Account_Code == "03").FirstOrDefault().Total;
            Badget.LibalityAndPropertyRightsAndProfit = PropertRightSum + LibalitySum+ Badget.Profit;
            Badget.company = db.Companies.FirstOrDefault();
            Badget.StartDate = trialba.Start_Date;
            Badget.EndDate = trialba.End_Date;
            db.SaveChanges();
            //db.Dispose();
            return Badget;
        }      

        /// <summary>
        /// report for get balance sheet 
        /// </summary>
        /// <param name="GL"></param>
        public static void GetBalanceForBallanceSheet(List<BalanceTrialVMAttribute> GL)
        {
            var db = new AccountingContainer();
            foreach (var detail in GL)
            {

                var minusCredit = detail.pureFinalCredit * -1;
                var balance = minusCredit + detail.pureFinaldepit;
                var account = db.ChartOfAccountings.Find(detail.Account_Id);

                account.BalanceSheetValue = account.BalanceSheetValue + balance;
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                var parent = account.ChartOfAccountingAccount_Id;
                while (parent != null)
                {
                    var parentDetail = db.ChartOfAccountings.Find(parent);
                    parentDetail.BalanceSheetValue = parentDetail.BalanceSheetValue + balance;
                    db.Entry(parentDetail).State = EntityState.Modified;
                    db.SaveChanges();
                    parent = parentDetail.ChartOfAccountingAccount_Id;
                }



            }
            db.SaveChanges();
            db.Dispose();
        }

        public static List<GLDetailVM> makeListForCustomerGL(List<GLMaster> glMasterList , int id)
        {
            var db = new AccountingContainer();
            var glDetail = new List<GLDetail>();
            var glDetailVM = new List<GLDetailVM>();
            foreach (var glMaster in glMasterList)
            {
                var List = glMaster.GLDetails.ToList();
                glDetail.AddRange(List);

            }
            var incomList = glDetail.Where(a => a.ChartOfAccounting.Account_Code.StartsWith("010109") && a.Account_Child_ID == id).ToList();
            var accountidList = incomList.Select(a => a.Account_Id).Distinct();
            foreach (var account in accountidList)
            {
                var GldetialVm = new GLDetailVM();
                var glAccountList = glDetail.Where(a => a.Account_Id == account && a.Account_Child_ID == id).ToList();
                foreach (var accountbalance in glAccountList)
                {

                    GldetialVm.Account_Id = accountbalance.Account_Id;
                    GldetialVm.GLDetail_Credit += accountbalance.GLDetail_Credit;
                    GldetialVm.GLDetail_Debit += accountbalance.GLDetail_Debit;
                    GldetialVm.Account_Name = accountbalance.Account_Name;
                    GldetialVm.Account_Code = db.ChartOfAccountings.Find(accountbalance.Account_Id).Account_Code;
                }
                glDetailVM.Add(GldetialVm);

            }

            return glDetailVM;
        }

        /// <summary>
        /// Method for creation of report general ledger
        /// </summary>
        /// <returns></returns>
        public static LedgerPickerVM getListForLedger()
        {
            var db = new AccountingContainer();
            LedgerPickerVM LVM = new LedgerPickerVM();
          
            IQueryable<object> newList2 = ChartOfAccounting.getAlldataForGL();

            

            LVM.SubAccountList = new SelectList(newList2, "Id", "Name");
         
            return LVM;
        }

        /// <summary>
        /// Report for general ledger 
        /// </summary>
        /// <param name="LVM"></param>
        /// <returns></returns>
        public static LedgerListVM AddingDataInLedger(LedgerPickerVM LVM)
        {
            var db = new AccountingContainer();
            var LedgerData = new LedgerListVM();

            LedgerData.LedgDetails = new List<LedgerFromDetailVM>();
            LedgerData.company = db.Companies.FirstOrDefault();
            if (LVM.FromDate == null)
            {
                LVM.FromDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (LVM.Todate == null)
            {
                LVM.Todate = Convert.ToString(new DateTime(2200, 12, 30));

            }
            DateTime start = Convert.ToDateTime(Convert.ToDateTime(LVM.FromDate).ToShortDateString());
            DateTime End = Convert.ToDateTime(Convert.ToDateTime(LVM.Todate).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);
            var start2 = start.ToShortDateString();
            LedgerData.FromDate = LVM.FromDate.ToString();
            LedgerData.ToDate = LVM.Todate.ToString();
            decimal totalBalanceBeforDate = 0;
            decimal CreditBalance = 0;
            decimal debitBalance = 0;
            if (LVM.Master_ID == null && LVM.SubAccount_ID == null)
            {
                var newledgerCred = new LedgerFromDetailVM();
                newledgerCred.Statment_credit = "";
                newledgerCred.Balance_Debit = 0;
                newledgerCred.Balance_Credit = 0;
                newledgerCred.Credit_Date = "";
                newledgerCred.Statment_debit = "";
                newledgerCred.Debit_Date = "";
                newledgerCred.totalBalance = 0;
                LedgerData.LedgDetails.Add(newledgerCred);
                return LedgerData;
            }
            if (LVM.Master_ID != 0 && LVM.Master_ID != null)
            {
                // add  the  Nameof the  account here
                var thisAccount = db.ChartOfAccountings.Find(LVM.Master_ID);
                LedgerData.AccountName = thisAccount.Account_Name;
                //we  got  theChilds  of  this  accounts
                var allMasters = db.ChartOfAccountings.Where(e => e.ChartOfAccountingAccount_Id == LVM.Master_ID).ToList();
                //see  if  the parent  dont  have  childs or  not
                if (allMasters.Count == 0)
                {
                    var parentBeforDate = db.GLDetails.Where(e => e.GLMaster.GL_Date <= start && e.Account_Id == thisAccount.Account_Id).ToList();
                    var thisdetailBefor = parentBeforDate;
                  
                        //here  we  get DetailsOF MasterBeforDate and Loop on them
                       
                        if (thisdetailBefor.Count != 0)
                        {
                        CreditBalance = thisdetailBefor.Select(a=>a.GLDetail_Credit).Sum();
                        debitBalance = thisdetailBefor.Select(a => a.GLDetail_Debit).Sum();
                        
                            if (debitBalance > CreditBalance)
                            {
                                LedgerData.BalanceNameBefor = "مدين";


                                totalBalanceBeforDate = debitBalance - CreditBalance;
                                LedgerData.totalbeforDate = totalBalanceBeforDate;

                            }
                            else if (debitBalance < CreditBalance)
                            {
                                LedgerData.BalanceNameBefor = "دائن";
                                totalBalanceBeforDate = CreditBalance - debitBalance;
                                LedgerData.totalbeforDate = totalBalanceBeforDate;
                            }
                            else
                            {
                                LedgerData.BalanceNameBefor = null;
                                LedgerData.totalbeforDate = 0;
                                totalBalanceBeforDate = 0;
                            }
                            if (LedgerData.totalbeforDate < 0)
                            {
                                LedgerData.totalbeforDate = LedgerData.totalbeforDate * -1;
                                totalBalanceBeforDate = LedgerData.totalbeforDate;
                            }

                        }

                    

                    //var parentAccountInDate = db.GLMasters.Where(e => e.GL_Date.Day >= start.Day&&e.GL_Date.Month>=start.Month&&e.GL_Date.Year>=start.Year && e.GL_Date.Day <= End.Day&&e.GL_Date.Month<=End.Month&&e.GL_Date.Year<=End.Year).ToList();
                    var parentAccountInDate = db.GLDetails.Where(e => e.GLMaster.GL_Date >= start && e.GLMaster.GL_Date <= End.Date && e.Account_Id == thisAccount.Account_Id).ToList();
                    var GLdetialList1 = parentAccountInDate;
                    
                        if (GLdetialList1.Count == 0)
                        {
                        }
                        else
                        {
                            foreach (var detail in GLdetialList1)
                            {
                                try
                                {
                                    if (detail.GLDetail_Debit > 0)
                                    {
                                        var newLedgerdeb = new LedgerFromDetailVM();
                                        var newledgerCred = new LedgerFromDetailVM();
                                        var master = db.GLMasters.Find(detail.GLMasterGLMaster_Id);

                                        var detinMaster = db.GLDetails.Where(t => t.GLMasterGLMaster_Id == master.GLMaster_Id && t.GLDetail_Credit > 0).FirstOrDefault();
                                        var thischart = db.ChartOfAccountings.Where(u => u.Account_Id == detinMaster.Account_Id).FirstOrDefault();
                                        newLedgerdeb.Statment_debit = "الي" + " " + thischart.Account_Name;
                                        newLedgerdeb.Balance_Debit = detail.GLDetail_Debit;
                                        newLedgerdeb.Debit_Date = master.GL_Date.ToString();
                                        newLedgerdeb.Balance_Credit = 0;
                                        newLedgerdeb.Credit_Date = null;
                                        newLedgerdeb.Statment_credit = null;
                                        newLedgerdeb.Debit_COde = master.GLCode;
                                        if (detail.Description_Detail !=null )
                                        {
                                            newLedgerdeb.Debit_Desc = detail.Description_Detail;
                                        }
                                        else
                                        {
                                            newLedgerdeb.Debit_Desc = master.GL_Desc;
                                        }
                                 
                                       
                                        if (detail.Account_Child_ID > 0)
                                        {
                                            if (string.IsNullOrEmpty(newLedgerdeb.childName))
                                            {
                                                var thischildname = db.Accounts.Where(e => e.Accounts_Id == detail.Account_Child_ID).ToList();
                                                if (thischildname.Count > 0)
                                                {
                                                    newLedgerdeb.childName = thischildname.FirstOrDefault().Account_Name;
                                                }
                                            }
                                            else
                                            {
                                            }

                                        }

                                        if (LedgerData.BalanceNameBefor == "دائن")
                                        {
                                            newLedgerdeb.totalBalance = totalBalanceBeforDate - newLedgerdeb.Balance_Debit;
                                            totalBalanceBeforDate = newLedgerdeb.totalBalance;
                                        }
                                        else
                                        {
                                            newLedgerdeb.totalBalance = totalBalanceBeforDate + newLedgerdeb.Balance_Debit;
                                            totalBalanceBeforDate = newLedgerdeb.totalBalance;
                                        }

                                        //totalBalanceBeforDate += newLedgerdeb.Balance_Debit;

                                        LedgerData.LedgDetails.Add(newLedgerdeb);
                                    }
                                    else if (detail.GLDetail_Credit > 0)
                                    {
                                        var newLedgerdeb = new LedgerFromDetailVM();
                                        var newledgerCred = new LedgerFromDetailVM();
                                        var master = db.GLMasters.Find(detail.GLMasterGLMaster_Id);

                                        var detinMaster = db.GLDetails.Where(t => t.GLMasterGLMaster_Id == master.GLMaster_Id && t.GLDetail_Debit > 0).FirstOrDefault();
                                        var thischart = db.ChartOfAccountings.Where(u => u.Account_Id == detinMaster.Account_Id).FirstOrDefault();
                                        newledgerCred.Statment_credit = "من" + " " + thischart.Account_Name;
                                        newledgerCred.Balance_Debit = 0;
                                        newledgerCred.Balance_Credit = detail.GLDetail_Credit;
                                        newledgerCred.Credit_Date = master.GL_Date.ToString();
                                        newledgerCred.Statment_debit = null;
                                        newledgerCred.Debit_Date = null;
                                        newledgerCred.Credit_COde = master.GLCode;
                                        if (detail.Description_Detail != null)
                                        {
                                            newledgerCred.Credit_Desc = detail.Description_Detail;
                                        }
                                        else
                                        {
                                            newledgerCred.Credit_Desc = master.GL_Desc;
                                        }
                                        
                                        if (detail.Account_Child_ID > 0)
                                        {
                                            if (string.IsNullOrEmpty(newLedgerdeb.childName))
                                            {

                                                var thischildname = db.Accounts.Where(e => e.Accounts_Id == detail.Account_Child_ID).ToList();
                                                if (thischildname.Count > 0)
                                                {
                                                    newledgerCred.childName = thischildname.FirstOrDefault().Account_Name;
                                                }

                                            }
                                            else
                                            {
                                            }

                                        }

                                        // newledgerCred.totalBalance = totalBalanceBeforDate - newledgerCred.Balance_Credit;
                                        if (LedgerData.BalanceNameBefor == "دائن")
                                        {
                                            newledgerCred.totalBalance = totalBalanceBeforDate + newledgerCred.Balance_Credit;
                                            totalBalanceBeforDate = newledgerCred.totalBalance;
                                        }
                                        else
                                        {
                                            newledgerCred.totalBalance = totalBalanceBeforDate - newledgerCred.Balance_Credit;
                                            totalBalanceBeforDate = newledgerCred.totalBalance;
                                        }
                                        //totalBalanceBeforDate -= newledgerCred.Balance_Credit;
                                        LedgerData.LedgDetails.Add(newledgerCred);
                                    }


                                }
                                catch (Exception e)
                                {
                                    var newledgerCred = new LedgerFromDetailVM();
                                    newledgerCred.Statment_credit = "";
                                    newledgerCred.Balance_Debit = 0;
                                    newledgerCred.Balance_Credit = 0;
                                    newledgerCred.Credit_Date = "";
                                    newledgerCred.Statment_debit = "";
                                    newledgerCred.Debit_Date = "";
                                    newledgerCred.Credit_COde = "";
                                    newledgerCred.Credit_Desc = "";
                                    newledgerCred.totalBalance = 0;
                                    totalBalanceBeforDate = 0;
                                    LedgerData.LedgDetails.Add(newledgerCred);
                                    return LedgerData;
                                }
                            }

                        }


                    
                    return LedgerData;
                }
                else
                {
                    //here  we  got balancebeforDate 
                    var parentBeforDate = db.GLMasters.Where(e => e.GL_Date <= start).ToList();
                    foreach (var glMasterBefor in parentBeforDate)
                    {
                        //here  we  get DetailsOF MasterBeforDate and Loop on them
                        foreach (var childDetail in allMasters)
                        {
                            var thisdetailBefor = db.GLDetails.Where(e => e.GLMasterGLMaster_Id == glMasterBefor.GLMaster_Id && e.Account_Id == childDetail.Account_Id).ToList();
                            if (thisdetailBefor.Count != 0)
                            {
                                foreach (var thisdetail in thisdetailBefor)
                                {
                                    CreditBalance += thisdetail.GLDetail_Credit;
                                    debitBalance += thisdetail.GLDetail_Debit;

                                }
                                if (debitBalance > CreditBalance)
                                {
                                    LedgerData.BalanceNameBefor = "مدين";
                                    totalBalanceBeforDate = debitBalance - CreditBalance;
                                    LedgerData.totalbeforDate += totalBalanceBeforDate;
                                }
                                else if (debitBalance < CreditBalance)
                                {
                                    LedgerData.BalanceNameBefor = "دائن";
                                    totalBalanceBeforDate = CreditBalance - debitBalance;
                                    LedgerData.totalbeforDate += totalBalanceBeforDate;
                                }
                                else
                                {
                                    LedgerData.BalanceNameBefor = null;
                                    LedgerData.totalbeforDate = 0;
                                    totalBalanceBeforDate = 0;
                                }

                            }
                        }


                    }
                    var parentAccountInDate = db.GLMasters.Where(e => e.GL_Date >= start && e.GL_Date <= End).ToList();
                    foreach (var GLdateID in parentAccountInDate)
                    {
                        var GLdetialList2 = db.GLDetails.Where(t => t.Account_Id == thisAccount.Account_Id && t.GLMasterGLMaster_Id == GLdateID.GLMaster_Id).ToList();
                        if (GLdetialList2.Count == 0)
                        {
                        }
                        else
                        {
                            foreach (var detail in GLdetialList2)
                            {
                                try
                                {
                                    if (detail.GLDetail_Debit > 0)
                                    {
                                        var newLedgerdeb = new LedgerFromDetailVM();
                                        var newledgerCred = new LedgerFromDetailVM();
                                        var master = db.GLMasters.Find(detail.GLMasterGLMaster_Id);

                                        var detinMaster = db.GLDetails.Where(t => t.GLMasterGLMaster_Id == master.GLMaster_Id && t.GLDetail_Credit > 0).FirstOrDefault();
                                        var thischart = db.ChartOfAccountings.Where(u => u.Account_Id == detinMaster.Account_Id).FirstOrDefault();
                                        newLedgerdeb.Statment_debit = "الي" + " " + thischart.Account_Name;
                                        newLedgerdeb.Balance_Debit = detail.GLDetail_Debit;
                                        newLedgerdeb.Debit_Date = master.GL_Date.ToString();
                                        newLedgerdeb.Balance_Credit = 0;
                                        newLedgerdeb.Credit_Date = null;
                                        newLedgerdeb.Statment_credit = null;
                                        newLedgerdeb.Debit_COde = master.GLCode;
                                        
                                        if (detail.Description_Detail != null)
                                        {
                                            newLedgerdeb.Debit_Desc = detail.Description_Detail;
                                        }
                                        else
                                        {
                                            newLedgerdeb.Debit_Desc = master.GL_Desc;
                                        }
                                        if (detail.Account_Child_ID > 0)
                                        {
                                            if (string.IsNullOrEmpty(newLedgerdeb.childName))
                                            {
                                                var thischildname = db.Accounts.Where(e => e.Accounts_Id == detail.Account_Child_ID).ToList();
                                                if (thischildname.Count > 0)
                                                {
                                                    newLedgerdeb.childName = thischildname.FirstOrDefault().Account_Name;
                                                }
                                            }
                                            else
                                            {
                                            }

                                        }
                                        if (LedgerData.BalanceNameBefor == "دائن")
                                        {
                                            newLedgerdeb.totalBalance = totalBalanceBeforDate - newLedgerdeb.Balance_Debit;
                                            totalBalanceBeforDate = newLedgerdeb.totalBalance;
                                        }
                                        else
                                        {
                                            newLedgerdeb.totalBalance = totalBalanceBeforDate + newLedgerdeb.Balance_Debit;
                                            totalBalanceBeforDate = newLedgerdeb.totalBalance;
                                        }

                                        //  totalBalanceBeforDate += newLedgerdeb.Balance_Debit;

                                        LedgerData.LedgDetails.Add(newLedgerdeb);
                                    }
                                    else if (detail.GLDetail_Credit > 0)
                                    {
                                        var newLedgerdeb = new LedgerFromDetailVM();
                                        var newledgerCred = new LedgerFromDetailVM();
                                        var master = db.GLMasters.Find(detail.GLMasterGLMaster_Id);

                                        var detinMaster = db.GLDetails.Where(t => t.GLMasterGLMaster_Id == master.GLMaster_Id && t.GLDetail_Debit > 0).FirstOrDefault();
                                        var thischart = db.ChartOfAccountings.Where(u => u.Account_Id == detinMaster.Account_Id).FirstOrDefault();
                                        newledgerCred.Statment_credit = "من" + " " + thischart.Account_Name;
                                        newledgerCred.Balance_Debit = 0;
                                        newledgerCred.Balance_Credit = detail.GLDetail_Credit;
                                        newledgerCred.Credit_Date = master.GL_Date.ToString();
                                        newledgerCred.Statment_debit = null;
                                        newledgerCred.Debit_Date = null;
                                        newledgerCred.Credit_COde = master.GLCode;
                                        newledgerCred.Credit_Desc = master.GL_Desc;
                                        if (detail.Description_Detail != null)
                                        {
                                            newledgerCred.Credit_Desc = detail.Description_Detail;
                                        }
                                        else
                                        {
                                            newledgerCred.Credit_Desc = master.GL_Desc;
                                        }
                                        if (detail.Account_Child_ID > 0)
                                        {
                                            if (string.IsNullOrEmpty(newLedgerdeb.childName))
                                            {
                                                var thischildname = db.Accounts.Where(e => e.Accounts_Id == detail.Account_Child_ID).ToList();
                                                if (thischildname.Count > 0)
                                                {
                                                    newledgerCred.childName = thischildname.FirstOrDefault().Account_Name;
                                                }
                                            }
                                            else
                                            {
                                            }

                                        }
                                        if (LedgerData.BalanceNameBefor == "دائن")
                                        {
                                            newledgerCred.totalBalance = totalBalanceBeforDate + newledgerCred.Balance_Credit;
                                            totalBalanceBeforDate = newledgerCred.totalBalance;
                                        }
                                        else
                                        {
                                            newledgerCred.totalBalance = totalBalanceBeforDate - newledgerCred.Balance_Credit;
                                            totalBalanceBeforDate = newledgerCred.totalBalance;
                                        }
                                        //  totalBalanceBeforDate -= newledgerCred.Balance_Credit;
                                        LedgerData.LedgDetails.Add(newledgerCred);
                                    }

                                    //   return LedgerData;
                                }
                                catch (Exception e)
                                {
                                    var newledgerCred = new LedgerFromDetailVM();
                                    newledgerCred.Statment_credit = "";
                                    newledgerCred.Balance_Debit = 0;
                                    newledgerCred.Balance_Credit = 0;
                                    newledgerCred.Credit_Date = "";
                                    newledgerCred.Statment_debit = "";
                                    newledgerCred.Debit_Date = "";
                                    newledgerCred.Credit_COde = "";
                                    newledgerCred.Credit_Desc = "";
                                    newledgerCred.totalBalance = 0;
                                    totalBalanceBeforDate = 0;
                                    LedgerData.LedgDetails.Add(newledgerCred);
                                    return LedgerData;
                                }
                            }

                        }


                    }
                    return LedgerData;
                }

            }
            else
            {
                var parentBeforDate = db.GLDetails.Where(e => e.GLMaster.GL_Date < start && e.Account_Id == LVM.SubAccount_ID).ToList();
                //we  got  theChilds  of  this  accounts
                var thisAccount = db.ChartOfAccountings.Find(LVM.SubAccount_ID);
                LedgerData.AccountName = thisAccount.Account_Name;
                List<GLDetail> Specific = new List<GLDetail> { };
                var alldetailsbefordate = parentBeforDate;
                
                    foreach (var account in alldetailsbefordate)
                    {
                        if (account.Account_Id == LVM.SubAccount_ID)
                        {
                            Specific.Add(account);
                        }


                    }
                
                if (Specific.Count != 0)
                {
                    foreach (var thisdetail in Specific)
                    {
                        CreditBalance += thisdetail.GLDetail_Credit;
                        debitBalance += thisdetail.GLDetail_Debit;

                    }
                    if (debitBalance > CreditBalance)
                    {
                        LedgerData.BalanceNameBefor = "مدين";
                        totalBalanceBeforDate = debitBalance - CreditBalance;
                        LedgerData.totalbeforDate += totalBalanceBeforDate;
                    }
                    else if (debitBalance < CreditBalance)
                    {
                        LedgerData.BalanceNameBefor = "دائن";
                        totalBalanceBeforDate = CreditBalance - debitBalance;
                        LedgerData.totalbeforDate += totalBalanceBeforDate;
                    }
                }

                else
                {
                    LedgerData.BalanceNameBefor = null;
                    LedgerData.totalbeforDate = 0;
                    totalBalanceBeforDate = 0;
                }

                //  }
                //}


            }
            var CHildAccountInDate = db.GLDetails.Where(e => (e.GLMaster.GL_Date >= start) && (e.GLMaster.GL_Date <= End)&&e.Account_Id== LVM.SubAccount_ID).ToList();
           var GLdetialList = CHildAccountInDate;
            
                if (GLdetialList.Count == 0)
                {
                }
                else
                {
                    foreach (var detail in GLdetialList)
                    {
                        try
                        {
                            if (detail.GLDetail_Debit > 0)
                            {
                                var newLedgerdeb = new LedgerFromDetailVM();
                                var newledgerCred = new LedgerFromDetailVM();
                                var master = db.GLMasters.Find(detail.GLMasterGLMaster_Id);

                                var detinMaster = db.GLDetails.Where(t => t.GLMasterGLMaster_Id == master.GLMaster_Id && t.GLDetail_Credit > 0).FirstOrDefault();
                                var thischart = db.ChartOfAccountings.Where(u => u.Account_Id == detinMaster.Account_Id).FirstOrDefault();
                                newLedgerdeb.Statment_debit = "الي" + " " + thischart.Account_Name;
                                newLedgerdeb.Balance_Debit = detail.GLDetail_Debit;
                                newLedgerdeb.Debit_Date = master.GL_Date.ToString();
                                newLedgerdeb.Balance_Credit = 0;
                                newLedgerdeb.Credit_Date = null;
                                newLedgerdeb.Statment_credit = null;
                                newLedgerdeb.Debit_COde = master.GLCode;
                                newLedgerdeb.Debit_Desc = master.GL_Desc;
                                if (detail.Description_Detail != null)
                                {
                                    newLedgerdeb.Debit_Desc = detail.Description_Detail;
                                }
                                else
                                {
                                    newLedgerdeb.Debit_Desc = master.GL_Desc;
                                }

                                if (detail.Account_Child_ID > 0)
                                {
                                    if (string.IsNullOrEmpty(newLedgerdeb.childName))
                                    {
                                        var thischildname = db.Accounts.Where(e => e.Accounts_Id == detail.Account_Child_ID).ToList();
                                        if (thischildname.Count > 0)
                                        {
                                            newLedgerdeb.childName = thischildname.FirstOrDefault().Account_Name;
                                        }
                                    }
                                    else
                                    {
                                    }

                                }
                                if (LedgerData.BalanceNameBefor == "دائن")
                                {
                                    newLedgerdeb.totalBalance = totalBalanceBeforDate - newLedgerdeb.Balance_Debit;
                                    totalBalanceBeforDate = newLedgerdeb.totalBalance;
                                }
                                else
                                {
                                    newLedgerdeb.totalBalance = totalBalanceBeforDate + newLedgerdeb.Balance_Debit;
                                    totalBalanceBeforDate = newLedgerdeb.totalBalance;
                                }

                                //  totalBalanceBeforDate += newLedgerdeb.Balance_Debit;

                                LedgerData.LedgDetails.Add(newLedgerdeb);
                            }
                            else if (detail.GLDetail_Credit > 0)
                            {
                                var newLedgerdeb = new LedgerFromDetailVM();
                                var newledgerCred = new LedgerFromDetailVM();
                                var master = db.GLMasters.Find(detail.GLMasterGLMaster_Id);

                                var detinMaster = db.GLDetails.Where(t => t.GLMasterGLMaster_Id == master.GLMaster_Id && t.GLDetail_Debit > 0).FirstOrDefault();
                                var thischart = db.ChartOfAccountings.Where(u => u.Account_Id == detinMaster.Account_Id).FirstOrDefault();
                                newledgerCred.Statment_credit = "من" + " " + thischart.Account_Name;
                                newledgerCred.Balance_Debit = 0;
                                newledgerCred.Balance_Credit = detail.GLDetail_Credit;
                                newledgerCred.Credit_Date = master.GL_Date.ToString();
                                newledgerCred.Statment_debit = null;
                                newledgerCred.Debit_Date = null;
                                newledgerCred.Credit_COde = master.GLCode;
                                newledgerCred.Credit_Desc = master.GL_Desc;
                                if (detail.Description_Detail != null)
                                {
                                    newledgerCred.Credit_Desc = detail.Description_Detail;
                                }
                                else
                                {
                                    newledgerCred.Credit_Desc = master.GL_Desc;
                                }
                                if (detail.Account_Child_ID > 0)
                                {
                                    if (string.IsNullOrEmpty(newLedgerdeb.childName))
                                    {
                                        var thischildname = db.Accounts.Where(e => e.Accounts_Id == detail.Account_Child_ID).ToList();
                                        if (thischildname.Count > 0)
                                        {
                                            newledgerCred.childName = thischildname.FirstOrDefault().Account_Name;
                                        }
                                    }
                                    else
                                    {
                                    }

                                }
                                if (LedgerData.BalanceNameBefor == "دائن")
                                {
                                    newledgerCred.totalBalance = totalBalanceBeforDate + newledgerCred.Balance_Credit;
                                    totalBalanceBeforDate = newledgerCred.totalBalance;
                                }
                                else
                                {
                                    newledgerCred.totalBalance = totalBalanceBeforDate - newledgerCred.Balance_Credit;
                                    totalBalanceBeforDate = newledgerCred.totalBalance;
                                }
                                //  totalBalanceBeforDate -= newledgerCred.Balance_Credit;
                                LedgerData.LedgDetails.Add(newledgerCred);
                            }


                        }
                        catch (Exception e)
                        {
                            var newledgerCred = new LedgerFromDetailVM();
                            newledgerCred.Statment_credit = "";
                            newledgerCred.Balance_Debit = 0;
                            newledgerCred.Balance_Credit = 0;
                            newledgerCred.Credit_Date = "";
                            newledgerCred.Statment_debit = "";
                            newledgerCred.Debit_Date = "";
                            newledgerCred.Credit_COde = "";
                            newledgerCred.Credit_Desc = "";
                            newledgerCred.totalBalance = 0;
                            totalBalanceBeforDate = 0;
                            LedgerData.LedgDetails.Add(newledgerCred);
                            return LedgerData;
                        }
                    }

                


            }
            if (LedgerData.LedgDetails.Count() == 0)
            {
                LedgerData.totalinDate = LedgerData.totalbeforDate;
                LedgerData.BalanceNameAfter = LedgerData.BalanceNameBefor;

            }
            return LedgerData;
        }

        public static SupplierBalanceDetailsVM SupplierPalanceDetails(int SupID)
        {
            var db = new AccountingContainer();
            SupplierBalanceDetailsVM supplierPalance = new SupplierBalanceDetailsVM();
            supplierPalance.company = db.Companies.FirstOrDefault();
            var Supplierreport = db.Suppliers.Find(SupID);

            var Account = db.Accounts.Where(a => a.Account_Code == Supplierreport.Supp_Code).ToList();
            var AcountId = Account.FirstOrDefault().Accounts_Id;
            var AcountCode = Account.FirstOrDefault().Account_Code;

            if (Supplierreport.Supp_Code == AcountCode)
            {
                supplierPalance.Supplier_Name = Supplierreport.Supp_Name;

                var PurMaster = db.PurInvoiceMasters.Where(P => P.Supplier_Id == SupID && P.Invoice_State == TransactionState.Post).ToList();
                if (PurMaster != null)
                {
                    supplierPalance.PusMaster = PurMaster;
                }


                var Recal = db.RecalPurchInvoiceMasters.Where(R => R.SupplierSuppId == SupID && R.PI_State == TransactionState.Post).ToList();
                if (Recal != null)
                {
                    supplierPalance.RecallMaster = Recal;
                }

                var Paym = db.Payments.Where(Pay => Pay.Supplier_Id == SupID && Pay.Pay_State == TransactionState.Post).ToList();
                if (Paym != null)
                {
                    supplierPalance.PayList = Paym;
                }

                var spent = db.SpentRecipts.Where(S => S.Child_AccountDebit_Id == SupID && S.SpentReceipt_State == TransactionState.Post).ToList();
                if (spent != null)
                {
                    supplierPalance.SpentPaymentList = spent;
                }

                var catches = db.CatchReceipts.Where(C => C.Child_AccountCredit_Id == SupID && C.CatchReceipt_State == TransactionState.Post).ToList();
                if (catches != null)
                {
                    supplierPalance.CatchPaymentList = catches;
                }

                var SupplierAccount = db.GLMasters.Where(a => a.Auto == false || a.GLType_Id.StartsWith("قيد افتتاحى")).ToList();
                supplierPalance.GLList = makeListForSupplierGL(SupplierAccount, AcountId);


                var Total_Balanc = db.Accounts.Where(a => a.Account_Code == Supplierreport.Supp_Code).FirstOrDefault();

                supplierPalance.Total_Balance = Total_Balanc.Account_Balance.Value;
            }

            else
            {
                return supplierPalance;
            }
            return supplierPalance;
        }

        /// <summary>
        /// This method for get all gl for supplier
        /// </summary>
        /// <param name="glMasterList"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<GLDetailVM> makeListForSupplierGL(List<GLMaster> glMasterList, int id)
        {
            var db = new AccountingContainer();
            var glDetail = new List<GLDetail>();
            var glDetailVM = new List<GLDetailVM>();
            foreach (var glMaster in glMasterList)
            {
                var List = glMaster.GLDetails.ToList();
                glDetail.AddRange(List);

            }
            var incomList = glDetail.Where(a => a.ChartOfAccounting.Account_Code.StartsWith("020101") && a.Account_Child_ID == id).ToList();
            var accountidList = incomList.Select(a => a.Account_Id).Distinct();
            foreach (var account in accountidList)
            {
                var GldetialVm = new GLDetailVM();
                var glAccountList = glDetail.Where(a => a.Account_Id == account && a.Account_Child_ID == id).ToList();
                foreach (var accountbalance in glAccountList)
                {

                    GldetialVm.Account_Id = accountbalance.Account_Id;
                    GldetialVm.GLDetail_Credit += accountbalance.GLDetail_Credit;
                    GldetialVm.GLDetail_Debit += accountbalance.GLDetail_Debit;
                    GldetialVm.Account_Name = accountbalance.Account_Name;
                    GldetialVm.Account_Code = db.ChartOfAccountings.Find(accountbalance.Account_Id).Account_Code;
                }
                glDetailVM.Add(GldetialVm);

            }

            return glDetailVM;
        }

        /// <summary>
        /// This for creation method of report subscription by team
        /// </summary>
        /// <returns></returns>
        public static SubscriptionTeamVM SubscriptionTeam()
        {
            var db = new AccountingContainer();

            SubscriptionTeamVM sub = new SubscriptionTeamVM();
            sub.MySupTeamList = new SelectList(db.EmployeeTeams, "Team_ID", "Team_Name");
            return sub;
        }
        /// <summary>
        /// This method report subscription by team 
        /// </summary>
        /// <param name="Teamsub"></param>
        /// <returns></returns>
        public static SubscriptionTeamVM ReportSubscriptionteam(SubscriptionTeamVM Teamsub)
        {
            var db = new AccountingContainer();

            SubscriptionTeamVM subscriptionTeam = new SubscriptionTeamVM();
            if (Teamsub.startDate == null)
            {
                Teamsub.startDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (Teamsub.EndDate == null)
            {
                Teamsub.EndDate = Convert.ToString(new DateTime(2200, 12, 30));

            }
            subscriptionTeam.company = db.Companies.FirstOrDefault();
            var start = Convert.ToDateTime(Teamsub.startDate);
            var Endtime = Convert.ToDateTime(Teamsub.EndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
            var team = Teamsub.SupTeam;
            //var DateTeam = db.Subscriptions.Where(a => a.StartTime == start && a.Emp_Team == team).ToList();

            subscriptionTeam.Subscrib = db.Subscriptions.Where(S => S.StartTime >= start && S.Emp_Team == team && S.StartTime <= Endtime&&S.IsCanceled!=true).ToList();
            subscriptionTeam.startDate = Teamsub.startDate;
            subscriptionTeam.EndDate = Teamsub.EndDate;
            subscriptionTeam.SupTeamName = db.EmployeeTeams.Find(Teamsub.SupTeam).Team_Name;
            return subscriptionTeam;
        }
        /////////////////////////////////Subscription___Reports///////////////////////////
        /// <summary>
        /// Method for get all subscription 
        /// </summary>
        /// <returns></returns>
        public static AllSubscriptionVM GetAllSubscriptions()
        {
            var db = new AccountingContainer();
            var allsubs = db.Subscriptions.ToList();
            var AllSUbVM = new AllSubscriptionVM();

            AllSUbVM.AllSUb = new List<SubscriptionVM>();
            AllSUbVM.company = db.Companies.FirstOrDefault();
            if (allsubs == null || allsubs.Count == 0)
            {
                var subvm = new SubscriptionVM();
                subvm.Subscrip_Id = 0;
                subvm.Subscrip_Code = "";
                subvm.Subscrip_Description = "";
                subvm.DeadLine = "";
                subvm.StartTime = "";
                subvm.Estimate_Amount = 0;
                subvm.Customer_Id = 0;
                subvm.Comp_Id = 0;
                subvm.Subscrip_BU = 0;
                subvm.Subscrip_State = 0;
                subvm.SelectedSubscrip_Type = 0;
                subvm.Subscrip_Pranch = 0;
                subvm.SelectEmp_Team = 0;
                subvm.SubType_State = 0;
                subvm.SelecltPayment_Method = 0;
                subvm.tel_customer = "";
                subvm.sub_Stop = null;
                subvm.Subscrip_DateEnd = null;
                subvm.Customer_Name = "";
                subvm.Check_Number = "";
                subvm.AMountAFterTax = 0;
                subvm.Subledger_ID = 0;
                AllSUbVM.AllSUb.Add(subvm);
            }
            else
            {
                foreach (var sub in allsubs)
                {
                    var subvm = new SubscriptionVM();
                    subvm.Subscrip_Id = sub.Subscrip_Id;
                    subvm.Subscrip_Code = sub.Subscrip_Code;
                    subvm.Subscrip_Description = sub.Subscrip_Description;
                    subvm.DeadLine = sub.DeadLine.ToString();
                    subvm.StartTime = sub.StartTime.ToString();
                    subvm.Estimate_Amount = sub.Estimate_Amount;
                    subvm.Customer_Id = sub.Customer_Id.Value;
                    subvm.Comp_Id = sub.Subscrip_Comp;
                    subvm.Subscrip_BU = sub.Subscrip_BU;
                    subvm.Subscrip_State = sub.Subscrip_State;
                    subvm.SelectedSubscrip_Type = sub.Subscrip_Type;
                    subvm.Subscrip_Pranch = sub.Subscrip_Pranch;
                    if (sub.Emp_Team == null)
                    { subvm.SelectEmp_Team = 0; }
                    else
                    {
                        subvm.SelectEmp_Team = (int)sub.Emp_Team;
                    }
                  
                    subvm.SubType_State = sub.SubType_State;
                    subvm.SelecltPayment_Method = sub.Payment_Method;
                    subvm.tel_customer = sub.tel_customer;
                    subvm.sub_Stop = sub.Subscrip_Stop;
                    subvm.Subscrip_DateEnd = sub.Subscrip_DateEnd;
                    subvm.Customer_Name = sub.Customer_Name;
                    subvm.Check_Number = sub.Check_Number;
                    subvm.AMountAFterTax = sub.AmountAfterTax;
                    subvm.Subledger_ID = sub.Subledger_ID;
                    subvm.Sub_Type_ID = sub.Sub_Type_ID;
                    AllSUbVM.AllSUb.Add(subvm);
                }
            }

            return AllSUbVM;
        }

        /// <summary>
        /// Method for creation to get subscreption by type
        /// </summary>
        /// <returns></returns>
        public static SubscriptionViewPageVM GetTypleForCreateReport()
        {
            var db = new AccountingContainer();
            SubscriptionViewPageVM LVM = new SubscriptionViewPageVM();
            LVM.SubTypeList = new SelectList(db.Subscription_Type.ToList(), "Subscription_Type_ID", "Subscrip_Name");
            LVM.branchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && (a.SubLed_Type == "فرع" || a.SubLed_Type == "قسم رئيسى لشركة")), "SubLed_Id", "SubLed_Name");
            return LVM;
        }

        /// <summary>
        /// To get subscription by type
        /// </summary>
        /// <param name="LVM"></param>
        /// <returns></returns>
        public static AllSubscriptionVM getSubscriptionBytype(SubscriptionViewPageVM LVM)
        {
            var db = new AccountingContainer();
            var exactSub = new AllSubscriptionVM();
            exactSub.Sublist = new List<Subscription>();
            exactSub.company = db.Companies.FirstOrDefault();
            exactSub.EndDate = LVM.EndDate;
            exactSub.StartDate = LVM.StartDate;
            exactSub.subtype_ID = LVM.subTypeID;
            if (LVM.branchId > 0)
            {
                exactSub.Subledger_Id = LVM.branchId.Value;
            }
            List<Subscription> allsubs = new List<Subscription>();
         
            if (LVM.StartDate == null)
                {
                LVM.StartDate = Convert.ToString(new DateTime(1995, 1, 1));
                }

                if (LVM.EndDate == null)
                {
                LVM.EndDate = Convert.ToString(new DateTime(2200, 12, 30));
                }
            exactSub.StartDate = LVM.StartDate;
            exactSub.EndDate = LVM.EndDate;
            var start = Convert.ToDateTime(exactSub.StartDate);
                var Endtime = Convert.ToDateTime(exactSub.EndDate).AddHours(23).AddMinutes(59).AddSeconds(59);
            allsubs = db.Subscriptions.Where(e => e.Sub_Date >= start && e.Sub_Date <= Endtime).ToList();
            if (LVM.subTypeID > 0)
            {
                allsubs = allsubs.Where(e => e.Sub_Type_ID == LVM.subTypeID).ToList();
            }
            if (LVM.branchId > 0)
            {
                allsubs = allsubs.Where(e => e.Subscrip_Pranch == LVM.branchId).ToList();
            }

            exactSub.Sublist = allsubs;




            return exactSub;
        }

        /// <summary>
        /// Report for get subscription non paid
        /// </summary>
        /// <param name="LVM"></param>
        /// <returns></returns>
        public static SubscriptionNonPaidVM GetSubscriptionNonPaid(SubscriptionNonPaidVM LVM)
        {
            var db = new AccountingContainer();
            var exactSub = new SubscriptionNonPaidVM();
      
            exactSub.company = db.Companies.FirstOrDefault();
            exactSub.EndDate = LVM.EndDate;
            exactSub.startDate = LVM.startDate;
          
    
            
        

                if (LVM.EndDate == null)
                {
                    exactSub.EndDate = Convert.ToString(new DateTime(1995, 1, 1));
                }

                
                var Endtime = Convert.ToDateTime(exactSub.EndDate).AddHours(23).AddMinutes(59).AddSeconds(59);

                exactSub.SubscribList = db.Subscriptions.Where(e => (e.Sub_Pay_State == Sub_Pay_State.دفع_جزئي || e.Sub_Pay_State == Sub_Pay_State.لم_يتم_الدفع ) &&  e.DeadLine >= Endtime ).ToList();
            

            return exactSub;
        }
        public static List<CustomerVM> getAllCustomerSub()
        {
            var db = new AccountingContainer();
            var Customer = db.Customers.Join(db.Subscriptions, cust => cust.Customer_Id, sub => sub.Customer_Id.Value, (cust, sub) => new  CustomerVM
            { _Customer_Id = cust.Customer_Id, Customer_Name = cust.Customer_Name, Customer_Code = cust.Customer_Code,Customer_Phone=cust.Customer_Phone,Customer_Mobile=cust.Customer_Mobile}).Distinct().ToList();
         
            return Customer;




        }
        
        public static SubscriptionStopVM CreateSubscriptionForCreation()
        {
            var db = new AccountingContainer();
            SubscriptionStopVM Stop = new SubscriptionStopVM();
            Stop.BranchList  = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && a.isActive != false && (a.SubLed_Type == "فرع " || a.SubLed_Type == "قسم رئيسي للشركه")), "SubLed_Id", "SubLed_Name");
            return Stop;
        }

        /// <summary>
        /// This method for get subscription status
        /// </summary>
        /// <param name="stopVM"></param>
        /// <returns></returns>
        public static SubscriptionStopVM subscriptionStop(SubscriptionStopVM stopVM)
        {
            var db = new AccountingContainer();

            SubscriptionStopVM subscriptionVm = new SubscriptionStopVM();
            subscriptionVm.company = db.Companies.FirstOrDefault();

         
                stopVM.StartDate = Convert.ToString(new DateTime(1995, 1, 1));
            

            if (stopVM.EndDate == null)
            {
                stopVM.EndDate = Convert.ToString(new DateTime(2200, 12, 30));
            }

            var start = Convert.ToDateTime(stopVM.StartDate);
            var stopdate = Convert.ToDateTime(stopVM.EndDate).AddHours(23).AddMinutes(59).AddSeconds(59);

            if (stopVM.SubType_State == SubType_State.مشترك)
            {
                subscriptionVm.StopSubscription = db.Subscriptions.Where(S => S.SubType_State == SubType_State.مشترك && S.StartTime >= start && S.StartTime <= stopdate && S.IsCanceled == false && S.Subscrip_State == TransactionState.Post).ToList();

            }



            else if (stopVM.SubType_State == SubType_State.على_وشك_الانتهاء)
            {
                subscriptionVm.StopSubscription = db.Subscriptions.Where(S => S.SubType_State == SubType_State.على_وشك_الانتهاء && S.StartTime >= start && S.StartTime <= stopdate && S.IsCanceled == false && S.Subscrip_State == TransactionState.Post).ToList();

            }

            else if (stopVM.SubType_State == SubType_State.ايقاف_مؤقت)
            {
                subscriptionVm.StopSubscription = db.Subscriptions.Where(S => S.SubType_State == SubType_State.ايقاف_مؤقت && S.StartTime >= start && S.StartTime <= stopdate && S.IsCanceled == false && S.Subscrip_State == TransactionState.Post).ToList();

            }
            else 
            {
                subscriptionVm.StopSubscription = db.Subscriptions.Where(S => S.SubType_State == SubType_State.منتهى && S.StartTime >= start && S.StartTime <= stopdate && S.IsCanceled == false && S.Subscrip_State == TransactionState.Post&&S.NewSubCode==null).ToList();

            }
            if (stopVM.Branch_Id > 0)
            {
                subscriptionVm.StopSubscription = subscriptionVm.StopSubscription.Where(a => a.Subscrip_Pranch == stopVM.Branch_Id).ToList();
            }
            subscriptionVm.StartDate = stopVM.StartDate;
            subscriptionVm.EndDate = stopVM.EndDate;
            subscriptionVm.SubType_State = stopVM.SubType_State;
            return subscriptionVm;
        }
        /// <summary>
        /// make SubLedger Report 
        /// </summary>
        /// <returns></returns>
        public static SubLedgerReportVM getDataToMakeReportOfSubLedger()
        {
            var db = new AccountingContainer();

            SubLedgerReportVM subRep = new SubLedgerReportVM();
       
            var newList = from subl in db.SubLedgers.Where(a=>a.isActive!=false) select new {

                Id = subl.SubLed_Id,
                Name = subl.SubLed_Name + "        " + subl.SubLed_Code
            };
            

            subRep.SubList = new SelectList(newList, "Id", "Name");
            return subRep;
        }
        /// <summary>
        /// Report for subldger balance
        /// </summary>
        /// <param name="subRep"></param>
        /// <returns></returns>
        public static SubLedgerReportVM postDataToMakeReportOfSubLedger(SubLedgerReportVM subRep)
        {
            var db = new AccountingContainer();
            if (subRep.From_Date == null)
            {
                subRep.From_Date = Convert.ToString(new DateTime(1995, 1, 1));

            }

            if (subRep.To_Date == null)
            {
                subRep.To_Date = Convert.ToString(new DateTime(2200, 12, 30));

            }

            //var startForBforDate = Convert.ToDateTime(subRep.From_Date).AddHours(23).AddMinutes(59).AddSeconds(59);
            var star = Convert.ToDateTime(subRep.From_Date);            
            var End = Convert.ToDateTime(subRep.To_Date).AddHours(23).AddMinutes(59).AddSeconds(59);
            var detilGlList = db.GLDetails.Where(e => e.GLMaster.GL_Date >= star&& e.GLMaster.GL_Date <= End&&e.SubLedger_Id!=0&&e.SubLedger_Id!=0).ToList();

            var code = db.SubLedgers.Find(subRep.SubLedger_Id).SubLed_Code;
            var SubLedger = db.SubLedgers.Where(a => a.SubLed_Code.StartsWith(code)).ToList();
            var startBalance = getTheBalanceOfDateBeforeForSubLedger(SubLedger, star);
            subRep.Account_Sudledger = new List<GLDetailVM>();
            subRep.Account_Sudledger.Add(startBalance);
          
            subRep.Account_Sudledger.AddRange(makeListForSubLedger(detilGlList, SubLedger));
            subRep.TotalCredit = subRep.Account_Sudledger.Select(a=>a.GLDetail_Credit).Sum();
            subRep.TotalDepit = subRep.Account_Sudledger.Select(a => a.GLDetail_Debit).Sum();


            
            subRep.Difference = subRep.TotalDepit - subRep.TotalCredit;
            subRep.SubName = db.SubLedgers.Find(subRep.SubLedger_Id).SubLed_Name;
            subRep.Su_Code = db.SubLedgers.Find(subRep.SubLedger_Id).SubLed_Code;
            subRep.company = db.Companies.FirstOrDefault();
            return subRep;



        }
        public static GLDetailVM getTheBalanceOfDateBeforeForSubLedger(List<SubLedger> subledger, DateTime date)
        {
            var db = new AccountingContainer();
            GLDetailVM vm = new GLDetailVM();
            vm.Subledger_Name = "رصيد اول المدة";
            
            decimal totalCredit = 0;
            decimal totalDepit = 0;
            foreach (var sub in subledger)
            {
                var glDetail = db.GLDetails.Where(a => a.SubLedger_Id==sub.SubLed_Id && a.GLMaster.GL_Date < date).ToList();
                if (glDetail.Count() > 0)
                {
                    totalDepit = totalDepit + glDetail.Select(a => a.GLDetail_Debit).Sum();
                    totalCredit = totalCredit + glDetail.Select(a => a.GLDetail_Credit).Sum();


                    
                }
            }
           
                vm.GLDetail_Credit = totalCredit;
                vm.GLDetail_Debit = totalDepit;

            
            return vm;
        }
        /// <summary>
        /// Make a list of subldger balance
        /// </summary>
        /// <param name="glMasterList"></param>
        /// <param name="SubList"></param>
        /// <returns></returns>
        public static List<GLDetailVM> makeListForSubLedger(List<GLDetail> glDetail, List<SubLedger> SubList)
        {
            var db = new AccountingContainer();
            
            var glDetailVM = new List<GLDetailVM>();
            var deletDetailVM = new List<GLDetailVM>();
            
            
            foreach (var subLed in SubList)
            {

                var GldetialVm = new GLDetailVM();

                var glAccountList = glDetail.Where(a => a.SubLedger_Id == subLed.SubLed_Id).ToList();
                if (glAccountList.FirstOrDefault() != null)
                {
                    GldetialVm.GLDetail_Credit = glAccountList.Select(a=>a.GLDetail_Credit).Sum();
                    GldetialVm.GLDetail_Debit = glAccountList.Select(a => a.GLDetail_Debit).Sum();
                    GldetialVm.SubLedger_Id = subLed.SubLed_Id;
                    GldetialVm.Subledger_Name = db.SubLedgers.Find(subLed.SubLed_Id).SubLed_Name;
                    GldetialVm.Subledger_Code = db.SubLedgers.Find(subLed.SubLed_Id).SubLed_Code;
          

                    glDetailVM.Add(GldetialVm);
                }

            }

            return glDetailVM;
        }
        /// <summary>
        /// For print catch receipt 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static catchreciptPrintVM PrintCatchRecipt(int id)
        {
            var db = new AccountingContainer();
            catchreciptPrintVM catchRecipt = new catchreciptPrintVM();

            var receipt = db.CatchReceipts.Find(id);
            catchRecipt.Receipt_Id = receipt.CatchReceipt_Id;
            catchRecipt.AccountDeptitId = db.PaymentMethods.Find(receipt.AccountDebit_Id).Payment_Type;
            catchRecipt.Recepit_Code = receipt.CatchReceipt_Code;
            catchRecipt.Receipt_Currency = db.Currencies.Find(receipt.Currency_Id).Currency_Type;
            catchRecipt.Receipt_Date = receipt.CatchReceipt_date.ToString();
            catchRecipt.Amount = receipt.CatchReceipt_Value;
            catchRecipt.Description = receipt.CatchReceipt_Description;
            catchRecipt.company = db.Companies.Find(receipt.Company_Id);
            if (receipt.Child_AccountCredit_Id == null || receipt.Child_AccountCredit_Id == 0)
            {
                catchRecipt.Customer_Name = db.ChartOfAccountings.Find(receipt.AccountCredit_Id).Account_Name;
            }
            else
            {

                var account = db.Accounts.Find(receipt.Child_AccountCredit_Id);
                if (account == null)
                {

                    catchRecipt.Customer_Name = db.ChartOfAccountings.Find(receipt.Child_AccountCredit_Id).Account_Name;

                }
                else
                {
                    catchRecipt.Customer_Name = account.Account_Name;
                }
            }
            return catchRecipt;
        }

        /// <summary>
        /// For print spent receipt 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrintSpentReciptVM PrintSpentRecipt(int id)
        {
            var db = new AccountingContainer();
            PrintSpentReciptVM catchRecipt = new PrintSpentReciptVM();

            var receipt = db.SpentRecipts.Find(id);
            catchRecipt.AccountCreditId = db.ChartOfAccountings.Find(receipt.AccountCredit_Id).Account_Name;
            catchRecipt.Receipt_Id = receipt.SpentReceipt_Id;
            catchRecipt.Recepit_Code = receipt.SpentReceipt_Code;
            catchRecipt.Receipt_Currency = db.Currencies.Find(receipt.Currency_Id).Currency_Type;
            catchRecipt.Receipt_Date = receipt.CatchReceipt_date.ToString();
            catchRecipt.Amount = receipt.SpentReceipt_Value;
            catchRecipt.Description = receipt.SpentReceipt_Description;
            if (receipt.Emp_Id != null && receipt.Emp_Id != 0)
            {
                catchRecipt.Emp_Id = receipt.Emp_Id.Value;
            }
             
            if (receipt.Child_AccountDebit_Id == null || receipt.Child_AccountDebit_Id == 0)
            {
                catchRecipt.Customer_Name = db.ChartOfAccountings.Find(receipt.AccountDebit_Id).Account_Name;
            }
            else
            {
                var account = db.Accounts.Find(receipt.Child_AccountDebit_Id);
                if (account == null)
                {

                    catchRecipt.Customer_Name = db.ChartOfAccountings.Find(receipt.Child_AccountDebit_Id).Account_Name;

                }
                else
                {
                    catchRecipt.Customer_Name = account.Account_Name;
                }

            }
            catchRecipt.company = db.Companies.Find(receipt.Company_Id);
            return catchRecipt;
        }
        /// <summary>
        /// For print GL master 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static GlMasterDetaislVM PrintGlMaster(int id)
        {
            var db = new AccountingContainer();
            GlMasterDetaislVM GlMasterVM = new GlMasterDetaislVM();
            var glMaster = db.GLMasters.Find(id);

            GlMasterVM.Company_Id = glMaster.Company_Id;

            GlMasterVM.GLCode = glMaster.GLCode;
            GlMasterVM.GL_Date = glMaster.GL_Date;
            GlMasterVM.Currency_Id = glMaster.Currency_Id;
            decimal TotalCredit = 0;
            decimal TotalDebit = 0;
            List<printGlDetailsVM> Listdetails = new List<printGlDetailsVM> { };
            foreach (var item in glMaster.GLDetails)
            {
                var Details = new printGlDetailsVM();
                Details.GLDetail_Id = item.GLDetail_Id;
                Details.GLDetail_Debit = item.GLDetail_Debit;
                Details.GLDetail_Credit = item.GLDetail_Credit;
                TotalCredit += item.GLDetail_Credit;

                TotalDebit += item.GLDetail_Debit;

                Listdetails.Add(Details);
            }
            GlMasterVM.totaCredit = TotalCredit;
            GlMasterVM.company = db.Companies.Find(glMaster.Company_Id);
            GlMasterVM.totaDebit = TotalDebit;
            GlMasterVM.myDetauls = Listdetails.ToList();
            return GlMasterVM;
        }

        /// <summary>
        /// For print expence balance
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrintExpensesVM printExpen(int id)
        {
            var db = new AccountingContainer();
            PrintExpensesVM expensesVm = new PrintExpensesVM();

            var expens = db.Expenses.Find(id);
            expensesVm.AccountExpense_Id = expens.AccountExpense_Id;
            expensesVm.Expense_Description = expens.Expense_Description;
            expensesVm.Expense_Value = expens.Expense_Value;
            expensesVm.expensesDate = expens.Expense_date;
            expensesVm.Expense_Code = expens.Expense_Code;
            expensesVm.Description = expens.Expense_Description;
            expensesVm.AccountTreasure_Id = expens.AccountTreasure_Id;
            expensesVm.company = db.Companies.Find(expens.Company_Id);
            expensesVm.todaydate = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
            var result = db.Currencies.Find(expens.Currency_Id);
            if (result != null)
            {
                expensesVm.Currency_Id = Convert.ToString(expens.Currency_Id);
            }
            expensesVm.ExpDetails = new List<PrintExpenseTempVM>();
            var counters = 0;
            decimal totaldebs = 0;
            decimal totalcreds = 0;
            var thisexptemp = db.ExpenseAccountTemps.Where(e => e.RandoumVAlue == expens.RandomValue).ToList();
            if (thisexptemp.Count > 0)
            {
                foreach (var detail in thisexptemp)
                {
                    totalcreds = totalcreds + detail.AccountTreat_Credit;
                    totaldebs = totaldebs + detail.AccountExp_Debit;
                    PrintExpenseTempVM pxtvm = new PrintExpenseTempVM();

                    pxtvm.total_Credit = totalcreds;
                    pxtvm.total_Debit = totaldebs;
                    counters++;
                    pxtvm.counter = counters;
                    if (detail.AccountExp_ID > 0 && detail.Tax_Bool == false)
                    {
                        var thisexpacc = db.ChartOfAccountings.Where(e => e.Account_Id == detail.AccountExp_ID).ToList().FirstOrDefault();
                        pxtvm.AccountExpName = thisexpacc.Account_Name;
                        pxtvm.AccountExp_Debit = detail.AccountExp_Debit;
                        pxtvm.AccountTreat_Credit = detail.AccountTreat_Credit;
                        pxtvm.ExpDesc = detail.ExpDesc;
                        var thissubledger = db.SubLedgers.Find(detail.SubLedger_ID);
                        pxtvm.subledgerName = thissubledger.SubLed_Name;
                        pxtvm.treasuID = 0;
                        pxtvm.expid = detail.AccountExp_ID;
                        expensesVm.ExpDetails.Add(pxtvm);
                    }
                    else if (detail.AccountExp_ID > 0 && detail.Tax_Bool == true)
                    {
                        var thisexpacc = db.Taxes.Where(e => e.Tax_ID == detail.AccountExp_ID).ToList().FirstOrDefault();
                        pxtvm.AccountExpName = thisexpacc.Tax_Name;
                        pxtvm.AccountExp_Debit = detail.AccountExp_Debit;
                        pxtvm.AccountTreat_Credit = detail.AccountTreat_Credit;
                        pxtvm.ExpDesc = detail.ExpDesc;
                       
                        pxtvm.treasuID = 0;
                        pxtvm.expid = detail.AccountExp_ID;
                        expensesVm.ExpDetails.Add(pxtvm);
                    }
                    else
                    {
                        var thisexpacc = ChartOfAccounting.getDataForThatAccount((int)detail.AccountTreasury_ID).Account_Name;
                        pxtvm.AccountTreasuryName = thisexpacc;
                        pxtvm.AccountExp_Debit = detail.AccountExp_Debit;
                        pxtvm.AccountTreat_Credit = detail.AccountTreat_Credit;
                        pxtvm.ExpDesc = detail.ExpDesc;
                        pxtvm.treasuID = detail.AccountTreasury_ID;
                        pxtvm.expid = 0;
                        expensesVm.ExpDetails.Add(pxtvm);
                    }

                }
            }


            return expensesVm;
        }
        /// <summary>
        /// Print a transfer mentainans by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrintTransferToMentainansVM TransferToMentainans(int id)
        {
            var db = new AccountingContainer();
            PrintTransferToMentainansVM MentainansVm = new PrintTransferToMentainansVM();
            var transfer = db.TransferToMaintenances.Find(id);
            MentainansVm.Company_Id = Convert.ToString(transfer.Company_Id);
            MentainansVm.Total_Amount = transfer.Total_Amount;
            MentainansVm.Trans_Date = Convert.ToString(transfer.Trans_Date);
            MentainansVm.Trans_Code = transfer.Trans_Code;
            MentainansVm.describtion = transfer.Describtion;
            MentainansVm.ExpenseAccountName = db.ChartOfAccountings.Find(transfer.Account_Id).Account_Name;
            var storeId = db.Stores.Find(transfer.Store_ID);
            if (storeId != null)
            {
                MentainansVm.StoreId = storeId.Store_ID;
                MentainansVm.StoreName = storeId.Store_Name;
            }

            List<PrintTransferMentainansDetailsVM> ListDetails = new List<PrintTransferMentainansDetailsVM> { };
            foreach (var item in transfer.TransferToMaintenanceDetails)
            {
                var Details = new PrintTransferMentainansDetailsVM();

                Details.Details_Id = item.Details_Id;
                Details.Product_Quantity = item.Product_Quantity;
                Details.Product_Price = item.Product_Price;
                Details.Total_Amount = item.Total_Amount;
                Details.Detail_Total_Cost = item.Detail_Total_Cost;
                Details.ProductProd_ID = item.Product_Id;

                ListDetails.Add(Details);
            }
            MentainansVm.MyDetails = ListDetails.ToList();
            MentainansVm.company = db.Companies.Find(transfer.Company_Id);
            return MentainansVm;
        }
        /// <summary>
        /// print a transfer from store to anothor 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static InventoryTransferToInventoryVm TansferToInventory(int id)
        {
            var db = new AccountingContainer();
            InventoryTransferToInventoryVm InvebtoryTransferVM = new InventoryTransferToInventoryVm();
            var Inventory = db.InventoryTransfer_Hd.Find(id);
            InvebtoryTransferVM.Date = Convert.ToString(Inventory.Date);
            InvebtoryTransferVM.store_Id = db.Stores.Find(Inventory.SourceStore_Id).Store_Name;
            InvebtoryTransferVM.CurrencyId = Inventory.Inv_Currency;
            InvebtoryTransferVM.SelectedCompanyId = Convert.ToString(Inventory.Comop_ID);
            InvebtoryTransferVM.Inv_Code = Inventory.Inv_Code;
            InvebtoryTransferVM.company = db.Companies.Find(Inventory.Comop_ID);
            InvebtoryTransferVM.StoreTo= db.Stores.Find(Inventory.DestStore_Id).Store_Name;
            InvebtoryTransferVM.Decribtion = Inventory.InvTransfere_Notes;
            decimal TotalAmount = 0;
            List<InventoryTranferDetailsVm> ListInventory = new List<InventoryTranferDetailsVm> { };
            foreach (var item in Inventory.InventoryTransfer_Detail)
            {
                var Transfer = new InventoryTranferDetailsVm();
                Transfer.TransferDetail_Id = item.TransferDetail_Id;
                Transfer.Product_Qty = item.Product_Qty;
                Transfer.Detail_TotalCoct = item.Detail_TotalCoct;
                
                Transfer.Product_ID =item.Product_ID;

                TotalAmount+= item.Detail_TotalCoct;

                ListInventory.Add(Transfer);
            }
            InvebtoryTransferVM.TotalAmount = TotalAmount;
            InvebtoryTransferVM.MyDetails = ListInventory.ToList();
            return InvebtoryTransferVM;
        }
        /// <summary>
        /// Print a project invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrentProjectInvoiceMasterVM ProjectInvoice(int id)
        {
            var db = new AccountingContainer();
            PrentProjectInvoiceMasterVM ProjectMasterVM = new PrentProjectInvoiceMasterVM();
            var project = db.ProjectInvoiceMasters.Find(id);
           
            if (project.Number == null)
            {
                ProjectMasterVM.Project_Code = project.Serial_Number;
            }
            else
            {
                ProjectMasterVM.Project_Code = project.Number;
            }
            ProjectMasterVM.QRNumber = project.QRNumber;
            var projects= db.Projects.Find(project.Project_Id);
            ProjectMasterVM.Project_Name = projects.Project_Name;
            ProjectMasterVM.StartTime = Convert.ToString(project.StartTime);
            ProjectMasterVM.branchId = projects.BranchSubledger_Id;
            ProjectMasterVM.TotalAmount = project.Estimate_Amount;
            var a = project.Customer_Name;
            ProjectMasterVM.Curriency = "الريال السعودى";
            ProjectMasterVM.Customer_Name =db.Customers.Find( project.CustomerCustomer_Id).Customer_Name;
            ProjectMasterVM.CustomerTax = db.Customers.Find(project.CustomerCustomer_Id).Customer_TaxNumber;
            ProjectMasterVM.company = db.Companies.FirstOrDefault();
            if (project.CustomerCustomer_Id == null)
            {
                ProjectMasterVM.customerId = 0;
            }
            else
            {
                ProjectMasterVM.customerId = project.CustomerCustomer_Id;
            }
            if (project.FixedDiscount > 0)
            {
                ProjectMasterVM.FixedDiscount = (decimal)project.FixedDiscount;
            }
            else
            {
                ProjectMasterVM.FixedDiscount = 0;
            }
            if (project.ProjectInv_Date != null)
            {
                ProjectMasterVM.invDate = Convert.ToDateTime(project.ProjectInv_Date).Day + "-" + Convert.ToDateTime(project.ProjectInv_Date).Month + "-" + Convert.ToDateTime(project.ProjectInv_Date).Year;
            }
            else
            {
                ProjectMasterVM.invDate  = Convert.ToString(project.StartTime); ;
            }

            decimal totalTax = 0;
            decimal totalBeforTax = 0;
            decimal TotalAfterTax = 0;
            List<PrintProjectInvoiceDetailsVM> ListProject = new List<PrintProjectInvoiceDetailsVM> { };
            foreach (var item in project.ProjectInvoiceDetails)
            {
                var details = new PrintProjectInvoiceDetailsVM();
                details.Detail_ID = item.Detail_ID;
                details.Product_Name = item.Product_Name;
                details.Price = item.Product_Price;
                details.Product_Quantity = item.Product_Quantity;
                details.TotalBeforTax = item.Product_Quantity * item.Product_Price;
                if (item.Tax_ID > 0)
                {
                    details.tax = db.Taxes.Find(item.Tax_ID);
                }

                details.Detail_Total_Tax = item.Detail_Total_Tax;
                if (project.MasterDiscount > 0)
                {
                    details.DetailDiscount = (project.MasterDiscount.Value / 100) * details.TotalBeforTax;
                    details.totalamount = item.Detail_Total_Tax + (details.TotalBeforTax - details.DetailDiscount);
                    details.TotalBeforTax = (item.Product_Quantity * item.Product_Price)-details.DetailDiscount;
                }
                else
                {
                    details.totalamount = item.Detail_Total_Tax + details.TotalBeforTax;
                }
              
                if (item.Tax_ID > 0 && item.Tax_ID != null)
                {
                    var thistax = db.Taxes.Find(item.Tax_ID);
                    ProjectMasterVM.taxName = thistax.Tax_Name;
                }
                else
                {

                }
                totalTax += item.Detail_Total_Tax;
                totalBeforTax += details.TotalBeforTax;
                TotalAfterTax += details.totalamount;
                ListProject.Add(details);
            }
            ProjectMasterVM.TotalTax = totalTax;
            ProjectMasterVM.TotalBeforeTax = totalBeforTax;
            ProjectMasterVM.TotalAmount = TotalAfterTax;
            ProjectMasterVM.MyDetails = ListProject.ToList();
            if (project.MasterDiscount > 0)
            {
                ProjectMasterVM.MasterDiscountAmount = (project.MasterDiscount.Value/100) * ProjectMasterVM.TotalBeforeTax;
                ProjectMasterVM.MasterDiscount = project.MasterDiscount.Value;
            }
            else
            {
                ProjectMasterVM.MasterDiscount = 0;
            }
            return ProjectMasterVM;
        }

        /// <summary>
        /// Print a subscription invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PrintSubscriptionInvoiceVM SupscriptionInvoic(int id)
        {
            var db = new AccountingContainer();
            PrintSubscriptionInvoiceVM SubscripIncoicevm = new PrintSubscriptionInvoiceVM();
            var Incoice = db.SubscriptionInvoices.Find(id);
            SubscripIncoicevm.Customer_Name = Incoice.Customer_Name;
            SubscripIncoicevm.Subscrip_Code = Incoice.Subscrip_Code;
            SubscripIncoicevm.company = db.Companies.FirstOrDefault();
            SubscripIncoicevm.DeadLine = Convert.ToString(Incoice.DeadLine);

            SubscripIncoicevm.StartTime = Convert.ToString(Incoice.StartTime);

            SubscripIncoicevm.Estimate_Amount = Incoice.Estimate_Amount;

            SubscripIncoicevm.Serial_Number = Incoice.Serial_Number;

            return SubscripIncoicevm;
        }
        /// <summary>
        /// Report for get all assets 
        /// </summary>
        /// <returns></returns>
        public static AssetsReportVM AllAssetReport()
        {
            var db = new AccountingContainer();
            var allassets = db.FixedAssets.Where(e=>e.FAsset_Dispose == false).ToList();
            AssetsReportVM assetreport = new AssetsReportVM();
            List<FixedAssetOpeningBalanceVM> thisList = new List<FixedAssetOpeningBalanceVM>();
            List<FixedAssetTypeForReportVM> thisType = new List<FixedAssetTypeForReportVM>();
            foreach (var asset in allassets)
            {
                var thisasset = new FixedAssetOpeningBalanceVM();
                var typeName = new FixedAssetTypeForReportVM();
                thisasset.FAsset_Descriptions = asset.FAsset_Description;
                thisasset.FAsset_PurchasePrices = asset.FAsset_PurchasePrice;
                thisasset.BookVlaue = (double)asset.BookValue;
                thisasset.Complex = (double)asset.Complex;
                thisasset.FAsset_Types = asset.FAsset_Type;
                typeName.FixedAssetType = thisasset.FAsset_Types;
                thisList.Add(thisasset);
                var expectedtype = thisType.Where(e => e.FixedAssetType == asset.FAsset_Type).ToList();
                if (expectedtype.Count == 0)
                {
                    thisType.Add(typeName);
                }
            }
            assetreport.AssetDetails = thisList.ToList();
            assetreport.AssetType = thisType.ToList();
            assetreport.company = db.Companies.FirstOrDefault();
            return assetreport;

        }
        public static EmployeeSalaryMounthVM Get_DateToMake_ReportOFEmployee()
        {
            var empSal = new EmployeeSalaryMounthVM();
            return empSal;



        }
        public static EmployeeSalaryMounthVM Post_DateToMake_ReportOFEmployee(EmployeeSalaryMounthVM EmpSalVM)
        {
            var db = new AccountingContainer();
            var Date = Convert.ToDateTime(EmpSalVM.Date);
            var parentBeforDate = db.EmployeeSalaryMonths.Where(e => e.Date.Month == Date.Month && e.Date.Year == Date.Year).ToList();
            foreach (var emp in parentBeforDate)
            {
                EmpSalVM.EmpNAme = emp.Emp_Name;
                EmpSalVM.EmpCode = emp.Emp_Code;
                EmpSalVM.NameOfSalaryCommponent = db.TypeOfDetailSalaries.Select(a => a.SalaryDetail_Name).Distinct().ToList();
                var SalType_Id = db.TypeOfDetailSalaries.Select(a => a.Id).Distinct().ToList();
                foreach (var item in SalType_Id)
                {
                    foreach (var salComp in emp.EmpSal_Detail)
                    {
                        var Account = db.Salary_Components.Where(a => a.Account_Id == item).ToList();
                        if (Account.FirstOrDefault() == null)
                        {



                        }


                    }
                }

            }

            return EmpSalVM;
        }
        public static EmpLoyeeLoanVM getLoanOfEmployee()
        {
            var db = new AccountingContainer();
            var AccountList = db.Accounts.Where(a => a.Account_Code.StartsWith("EMP")).ToList();
            EmpLoyeeLoanVM loanVM = new EmpLoyeeLoanVM();
            loanVM.company = db.Companies.FirstOrDefault();
            loanVM.Accounts = AccountList;
            return loanVM;


        }

        // report for list from purchase invoice with out taxs

        public static AllPurchaseInvoicesWithTaxVM GetAllPurchaseInvoicesWithTaxForCreation()
        {
            var models = new AllPurchaseInvoicesWithTaxVM();
            return models;
        }

        public static AllPurchaseInvoicesWithTaxVM GetAllPurchaseInvoicesWithTax(AllPurchaseInvoicesWithTaxVM allPurchaseInvoicesWithTaxVM)
        {
            var db = new AccountingContainer();
            var purchaseWithTax = new AllPurchaseInvoicesWithTaxVM();
            if (allPurchaseInvoicesWithTaxVM.startDate == null)
            {
                allPurchaseInvoicesWithTaxVM.startDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (allPurchaseInvoicesWithTaxVM.EndDate == null) 
            {
                allPurchaseInvoicesWithTaxVM.EndDate = Convert.ToString(new DateTime(2200, 12, 30));

            }
            purchaseWithTax.startDate = allPurchaseInvoicesWithTaxVM.startDate;
            purchaseWithTax.EndDate = allPurchaseInvoicesWithTaxVM.EndDate;
            purchaseWithTax.company = db.Companies.FirstOrDefault();
            var start = Convert.ToDateTime(Convert.ToDateTime(allPurchaseInvoicesWithTaxVM.startDate).ToShortDateString());
            var end = Convert.ToDateTime(Convert.ToDateTime(allPurchaseInvoicesWithTaxVM.EndDate).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);
           
            var PurMaster = db.PurInvoiceMasters.Where(e => e.Invoice_Date >= start  && e.Invoice_Date <= end).ToList();
 

            purchaseWithTax.PurchaseMasterWithTax = PurMaster.Where(a => a.Total_Tax != 0 && a.Invoice_State == TransactionState.Post && a.InvoiceDetails.FirstOrDefault().Asset_Id == null).ToList();
           
            foreach (var item in purchaseWithTax.PurchaseMasterWithTax)
            {
                var totalAfterDisc = item.Total_Amount - (item.Total_Amount * (item.Master_Discount / 100));
                purchaseWithTax.totalBeforeTax += totalAfterDisc;
                purchaseWithTax.totalTax += item.Total_Tax;
                purchaseWithTax.totalAfterTax += item.Total_Cost;
            }

            return purchaseWithTax;  
        }

        public static AllPurchaseInvoicesWithTaxVM GetAllPurchaseInvoicesWithoutTaxForCreation()
        {
            var models = new AllPurchaseInvoicesWithTaxVM();
            return models;
        }

        public static AllPurchaseInvoicesWithTaxVM GetAllPurchaseInvoicesWithoutTax(AllPurchaseInvoicesWithTaxVM allPurchaseInvoicesWithTaxVM)
        {
            var db = new AccountingContainer();
            var purchaseWithTax = new AllPurchaseInvoicesWithTaxVM();

            
            if (allPurchaseInvoicesWithTaxVM.startDate == null)
            {
                allPurchaseInvoicesWithTaxVM.startDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (allPurchaseInvoicesWithTaxVM.EndDate == null)
            {
                allPurchaseInvoicesWithTaxVM.EndDate = Convert.ToString(new DateTime(2200, 12, 30));

            }

            purchaseWithTax.startDate = allPurchaseInvoicesWithTaxVM.startDate;
            purchaseWithTax.EndDate = allPurchaseInvoicesWithTaxVM.EndDate;
            var start = Convert.ToDateTime(Convert.ToDateTime(allPurchaseInvoicesWithTaxVM.startDate).ToShortDateString());
            var end = Convert.ToDateTime(Convert.ToDateTime(allPurchaseInvoicesWithTaxVM.EndDate).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59); 
            var PurMaster = db.PurInvoiceMasters.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= end ).ToList();

            purchaseWithTax.PurchaseMasterWithTax = PurMaster.Where(a => a.Total_Tax == 0 && a.Invoice_State == TransactionState.Post && a.InvoiceDetails.FirstOrDefault().Asset_Id == null).ToList();

            foreach (var item in purchaseWithTax.PurchaseMasterWithTax)
            {
                var totalAfterDisc = item.Total_Amount - (item.Total_Amount * (item.Master_Discount / 100));
                purchaseWithTax.totalBeforeTax += totalAfterDisc;
                //purchaseWithTax.totalTax += item.Total_Tax;
                //purchaseWithTax.totalAfterTax += item.Total_Cost;
            }
            purchaseWithTax.company = db.Companies.FirstOrDefault();
            return purchaseWithTax;
        }

        public static AllSalseInvoicesWithOutTaxVM GetAllInvoiceSalseWithoutTaxForCreation()
        {
            var models = new AllSalseInvoicesWithOutTaxVM();
            return models;
        }

        public static AllSalseInvoicesWithOutTaxVM GetAllInvoiceSalseWithoutTax(AllSalseInvoicesWithOutTaxVM allSalesInvoicesWithTaxVM)
        {
            var db = new AccountingContainer();
            var salesInvoice = new AllSalseInvoicesWithOutTaxVM();
            if (allSalesInvoicesWithTaxVM.startDate == null)
            {
                allSalesInvoicesWithTaxVM.startDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (allSalesInvoicesWithTaxVM.EndDate == null)
            {
                allSalesInvoicesWithTaxVM.EndDate = Convert.ToString(new DateTime(2200, 12, 30));

            }

            salesInvoice.startDate = allSalesInvoicesWithTaxVM.startDate;
            salesInvoice.EndDate = allSalesInvoicesWithTaxVM.EndDate;

            var start = Convert.ToDateTime(Convert.ToDateTime(allSalesInvoicesWithTaxVM.startDate).ToShortDateString());
            var end = Convert.ToDateTime(Convert.ToDateTime(allSalesInvoicesWithTaxVM.EndDate).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59); 
            var salesInvoiceList = db.InvoiceMasters.Where(e => e.Invoice_Date >= start   && e.Invoice_Date <= end ).ToList();
            var salesRecipt=db.SalesReceiptMasters.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= end).ToList();
            salesInvoice.SalesInvoiceMasterWithOut = salesInvoiceList.Where(a => a.Total_Tax != 0 && a.Invoice_State == TransactionState.Post).ToList();
            salesInvoice.SalesReciptWithOut = salesRecipt.Where(a => a.Total_Tax != 0 && a.Invoice_State == TransactionState.Post).ToList();

            foreach (var item in salesInvoice.SalesInvoiceMasterWithOut)
            {
                var totalAfterDisc = item.Total_Amount - item.Total_Tax;
                salesInvoice.totalBeforeTaxSales += totalAfterDisc;
                salesInvoice.totalTaxSales += item.Total_Tax;
                salesInvoice.totalAfterTaxSales += item.Total_Amount;
            }
            foreach (var item in salesInvoice.SalesReciptWithOut)
            {
                var totalAfterDisc = item.Total_Amount - item.Total_Tax;
                salesInvoice.totalBeforeTaxSalesReceipt += totalAfterDisc;
                salesInvoice.totalTaxSalesReceipt += item.Total_Tax;
                salesInvoice.totalAfterTaxSalesReceipt += item.Total_Amount;
            }

            salesInvoice.totalBeforeTax = salesInvoice.totalBeforeTaxSales + salesInvoice.totalBeforeTaxSalesReceipt;
            salesInvoice.totalTax = salesInvoice.totalTaxSales + salesInvoice.totalTaxSalesReceipt;
            salesInvoice.totalAfterTax = salesInvoice.totalAfterTaxSales + salesInvoice.totalAfterTaxSalesReceipt;
            salesInvoice.company = db.Companies.FirstOrDefault();

            return salesInvoice;
        }
        public static SalaryListaVM PrintAllEmployee(int id)
        {
            var db = new AccountingContainer();
            var EmplSalDate = db.EmployeeSalaryMonths.Find(id).Date;
            var AllEmployeeInSamDate = db.EmployeeSalaryMonths.Where(a => a.Date.Month == EmplSalDate.Month && a.Date.Year == EmplSalDate.Year).ToList();
            List<EmployeeSallaryVM> EmpSalList = new List<EmployeeSallaryVM>();
            SalaryListaVM salListVm = new SalaryListaVM();
            foreach (var item in AllEmployeeInSamDate.Where(a=>a.Date_Take_Sal!=null))
            {
                EmployeeSallaryVM empSal = new EmployeeSallaryVM();
                empSal.Currency = "ريال سعودى";
                empSal.Date = Convert.ToString(item.Date);
                var emp = db.Accounts.Where(a => a.Account_Code == item.Emp_Code).FirstOrDefault();
                empSal.documentSerial = db.GLDetails.Where(a => a.GLMaster.GLCode.StartsWith( "EMPT-")&&a.Account_Child_ID==emp.Accounts_Id&& a.GLMaster.GL_Date.Month == item.Date_Take_Sal.Value.Month && a.GLMaster.GL_Date.Year == item.Date_Take_Sal.Value.Year&& a.GLMaster.GL_Date.Day == item.Date_Take_Sal.Value.Day).FirstOrDefault().GLMaster.GLCode;

                empSal.Employee_Name = item.Emp_Name;
                empSal.Employee_Code= item.Emp_Code;
                empSal.SallaryAmount = item.TotalSalary;
                empSal.LoanOfEmp = item.Trims_Pay.Value;
                var Account = db.ChartOfAccountings.Find(item.AcountSal);
                if (Account != null)
                {

                    empSal.Account = Account.Account_Name;
                    EmpSalList.Add(empSal);

                }
            }
            salListVm.company = db.Companies.FirstOrDefault();
            salListVm.ListSallary = EmpSalList;

            return salListVm;

        }

        public static SalaryListaVM PrintAllEmployeeLoan(int id)
        {
            var db = new AccountingContainer();
            var EmplSalDate = db.EmployeeSalaryMonths.Find(id).Date;
            var AllEmployeeInSamDate = db.EmployeeSalaryMonths.Where(a => a.Date.Month == EmplSalDate.Month && a.Date.Year == EmplSalDate.Year).ToList();
            List<EmployeeSallaryVM> EmpSalList = new List<EmployeeSallaryVM>();
            SalaryListaVM salListVm = new SalaryListaVM();
            foreach (var item in AllEmployeeInSamDate.Where(a=>a.Trims_Pay!=null))
            {
                EmployeeSallaryVM empSal = new EmployeeSallaryVM();
                empSal.Currency = "ريال سعودى";
                empSal.Date = Convert.ToString(item.Date);
                empSal.CompnyName = db.Companies.FirstOrDefault().Comp_Name;
                empSal.Employee_Name = item.Emp_Name;
                empSal.SallaryAmount = item.Trims_Pay.Value;


                var Account = db.ChartOfAccountings.Find(item.AcountSal);
                if (Account != null)
                {

                    empSal.Account = Account.Account_Name;

                    if (empSal.SallaryAmount != 0)
                    {

                        EmpSalList.Add(empSal);
                    }


                    

                }
            }
            salListVm.LoanOfEmployee = EmpSalList;
            salListVm.company = db.Companies.FirstOrDefault();
            return salListVm;

        }

        ////subscriptionReportAfterPost
        public static SubscriptionPrintVM SubscriptionPrint(int subID)
        {
            var db = new AccountingContainer();

            var subVM = new SubscriptionPrintVM();
            var thissub = db.Subscriptions.Find(subID);
            var type = db.Subscription_Type.Find(thissub.Sub_Type_ID);
            subVM.subtype = type.Subscrip_Name;
            subVM.branchId = thissub.Subscrip_Pranch;
            subVM.QRNumber = thissub.QRNumber;
           
            if (thissub.Number == null)
            {
                subVM.Sub_Code = thissub.Subscrip_Code;
            }
            else
            {
                subVM.Sub_Code = thissub.Number;
            }
            subVM.deadline = thissub.DeadLine.ToString("yyyy/MM/dd ");
            subVM.starttime = thissub.StartTime.ToString("yyyy/MM/dd ");
          
            var thiscustomer = db.Customers.Find(thissub.Customer_Id);

            var employee = db.Employees.Find(thissub.Emp_Id);
            if (employee != null)
            {
                subVM.EmployeeName = employee.Emp_Name;
            }
            subVM.customerName = thiscustomer.Customer_Name;
            subVM.customer_Tax = thiscustomer.Customer_TaxNumber;
            subVM.customer_tel = thiscustomer.Customer_Mobile;
            subVM.customer_fax = thiscustomer.Customer_Fax;
            subVM.customerphone = thiscustomer.Customer_Phone;
            subVM.Customer_Email = thiscustomer.Customer_Mail;
            subVM.EstimatedAmount = thissub.Estimate_Amount;
            subVM.customerCode = thiscustomer.Customer_Code;
            subVM.CustomerID = thiscustomer.Customer_Id;
            try
            {
                subVM.description = db.Subscription_Type.Find(thissub.Sub_Type_ID).Subscrip_Description;
            }
            catch
            {

            }
            subVM.sub_Date = thissub.Sub_Date;
            subVM.subduration = thissub.Subscrip_Type;
            // subVM.subtype = thissub.Subscrip_Description;
            subVM.CompanyID = thissub.Subscrip_Comp;
            //subVM.TotalDiscount = thissub.Estimate_Amount * (thissub.Discount.Value / 100);
            //var TotalAfterDiscount = thissub.Estimate_Amount - subVM.TotalDiscount;
            subVM.company = db.Companies.Find(thissub.Subscrip_Comp);
            //var thussubtype = db.Subscription_Type.Find(thissub.Sub_Type_ID);
            //if (thussubtype != null)
            //{
            //    subVM.subduration = thussubtype.Durationsubscription;
            //}

            //var thistax = db.Taxes.Find(thissub.Tax_ID);
            //if (thistax != null)
            //{
            //    subVM.totaltax = TotalAfterDiscount * (thistax.Tax_Percentage / 100);
            //    subVM.taxName = thistax.Tax_Name;
            //    subVM.TaxPercentage = thistax.Tax_Percentage;
            //}
            //else
            //{
            //    subVM.totaltax = 0;
            //    subVM.TaxPercentage = 0;
            //    subVM.taxName = "";
            //}
            //subVM.TotalDiscount = subVM.TotalDiscount + thissub.DiscountAllow.Value;
            //subVM.TotalAMountAFterTax = thissub.AmountAfterTax;
            subVM.Master_Discount = (decimal)thissub.Discount;
            subVM.TotalDiscount = (decimal)(subVM.EstimatedAmount * (subVM.Master_Discount / 100));
            subVM.totalAfterdiscount = thissub.Estimate_Amount - subVM.TotalDiscount;
            if (thissub.Tax_ID > 0)
            {
                var thistax = db.Taxes.Find(thissub.Tax_ID);
                subVM.taxName = thistax.Tax_Name;
                subVM.totaltax = (decimal)subVM.totalAfterdiscount * (thistax.Tax_Percentage / 100);
                subVM.TotalAMountAFterTax = (decimal)subVM.totalAfterdiscount + subVM.totaltax;
                subVM.TaxPercentage = thistax.Tax_Percentage;
            }
            else
            {
                subVM.TotalAMountAFterTax = (decimal)subVM.totalAfterdiscount;
            }
            if (thissub.DiscountAllow > 0)
            {
                subVM.fixed_Discount = (decimal)thissub.DiscountAllow;
                subVM.total_After_Fixed = (decimal)(subVM.TotalAMountAFterTax - thissub.DiscountAllow);

            }
            return subVM;
        }
        //this  Method  is  to get all Informationof  the  GL master  and its  Detail in  List and  Show  in  Report
        public static GlMasterAndDetailListVM ReportOfEachGLMaster(int GMasterID)
        {
            var db = new AccountingContainer();
            int counter = 1;
            var thisGLmaster = db.GLMasters.Find(GMasterID);
            GlMasterAndDetailListVM GLMDL = new GlMasterAndDetailListVM();
            GLMDL.GLDetailList = new List<GLDetailIistReportVM>();
            GLMDL.GlCode = thisGLmaster.GLCode;
            GLMDL.GlDate = thisGLmaster.GL_Date.Day + "-" + thisGLmaster.GL_Date.Month + "-" + thisGLmaster.GL_Date.Year;
            GLMDL.GLDescr = thisGLmaster.GL_Desc;
            GLMDL.GLType = thisGLmaster.GLType_Id;
            GLMDL.DateNowforReport= DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
            var thiscompay = db.Companies.Find(thisGLmaster.Company_Id);
            GLMDL.CompanyName = thiscompay.Comp_Name;
            foreach (var detail in thisGLmaster.GLDetails)
            {
                GLDetailIistReportVM Gldetail = new GLDetailIistReportVM();
                Gldetail.detailcounter = counter;
                var thisaccountcode = db.ChartOfAccountings.Find(detail.Account_Id);
                Gldetail.AccountCode = thisaccountcode.Account_Code;
                Gldetail.AccountName = thisaccountcode.Account_Name;
                Gldetail.childName = detail.Account_CHild_Name;
                Gldetail.decribtion = detail.Description_Detail;
                if (detail.GLDetail_Credit > 0)
                {
                    Gldetail.credit = Math.Round(detail.GLDetail_Credit,2,MidpointRounding.AwayFromZero).ToString();
                }
                else
                {
                    Gldetail.credit = "";
                }
                if (detail.GLDetail_Debit > 0)
                {
                    Gldetail.Debit = Math.Round(detail.GLDetail_Debit, 2,MidpointRounding.AwayFromZero).ToString();
                }
                else
                {
                    Gldetail.Debit = "";
                }
                Gldetail.CurrencyName = thisGLmaster.Currency_Id;
                
                if (detail.SubLedger_Id != null && detail.SubLedger_Id != 0)
                {
                    var thissubcode = db.SubLedgers.Find(detail.SubLedger_Id);
                    Gldetail.subledgerCode = thissubcode.SubLed_Code;
                    Gldetail.subledgerName = thissubcode.SubLed_Name;
                }
                else
                {
                    Gldetail.subledgerCode = ""; 
                }
                GLMDL.DebitTotal +=detail.GLDetail_Debit;
                GLMDL.CreditTotal +=detail.GLDetail_Credit;
                GLMDL.GLDetailList.Add(Gldetail);
                counter++;
            }

            return GLMDL;
        }
        public static SubInvoiceReportVM reportofSubInvoice(int id)
        {


            var db = new AccountingContainer();
            var thissubInvoic = db.SubscriptionInvoices.Find(id);
            var thissub = db.Subscriptions.Where(e => e.Subscrip_Code == thissubInvoic.Subscrip_Code).ToList().FirstOrDefault();

            SubInvoiceReportVM subVM = new SubInvoiceReportVM();
            subVM.branchId = thissub.Subscrip_Pranch;
            subVM.description = thissubInvoic.Note;
            subVM.EstimatedAmount = thissubInvoic.Estimate_Amount;
            subVM.InvoiceDate = Convert.ToDateTime(thissubInvoic.InvoiceDate).Day + "-" + Convert.ToDateTime(thissubInvoic.InvoiceDate).Month + "-" + Convert.ToDateTime(thissubInvoic.InvoiceDate).Year;
            subVM.PayedAmount = thissubInvoic.Pay_Amount;

            var CustomeData = db.Customers.Find(thissubInvoic.Customer_Id);
            if (CustomeData != null)
            {
                subVM.CustomerName = CustomeData.Customer_Name;
                subVM.CustomerCode = CustomeData.Customer_Code;
                //subVM.AmountRequired = CustomeData.Customer_Balance.Value;
                subVM.AmountRequired = Accounts.RefreshTheBallanceOfCustomer(CustomeData.Customer_Code);


                //subVM.AmountStill = subVM.AmountRequired - subVM.PayedAmount;
            }


            //subVM.CustomerName = thissubInvoic.Customer_Name;
            var thismethodAccoun = db.PaymentMethods.Find(thissubInvoic.Payment_Method);
            if (thissubInvoic.Transaction_State == TransactionState.Void)
            {
                subVM.from = db.ChartOfAccountings.Find(thismethodAccoun.Bank_Account_Id).Account_Name;
                subVM.to = thissubInvoic.Customer_Name;
            }
            else
            {
                subVM.to = db.ChartOfAccountings.Find(thismethodAccoun.Bank_Account_Id).Account_Name;
                subVM.from = thissubInvoic.Customer_Name;
            }
    
            subVM.Company = db.Companies.FirstOrDefault();
            subVM.currency = db.Currencies.FirstOrDefault();
            subVM.InvoiceCode = thissubInvoic.Serial_Number;
            subVM.SubScription_Type = db.Subscription_Type.Find(thissub.Sub_Type_ID).Subscrip_Name;
            subVM.SubscriptionCode = thissub.Subscrip_Code;
            subVM.SubscriptionAmount = thissub.AmountAfterTax;
            subVM.startSub = thissub.StartTime;

            subVM.EndSub = thissub.DeadLine;

            try
            {
                var pay = db.PaymentMethods.Find(thissub.Payment_Method.Value);
                subVM.PaymentMethod = pay.Payment_Type + " " + pay.Bank_Account_Name;
            }
            catch
            {
                subVM.PaymentMethod = "";
            }

            return subVM;

        }
        public static StoreReportVM ReportofStor(int id)
        {
            var db = new AccountingContainer();
            var thisstore = db.Stores.Find(id);
            StoreReportVM stRVM = new StoreReportVM();
            stRVM.ListofProducts = new List<StoreProductsVM>();
            stRVM.Store_Address = thisstore.Store_Address;
            stRVM.Store_Code = thisstore.Store_Code;
            stRVM.Store_Name = thisstore.Store_Name;
            if (thisstore.Total_Batch_Cost == null)
            {
                stRVM.Total_Batch_Cost = 0;
            }
            else
            {
                stRVM.Total_Batch_Cost = (decimal)thisstore.Total_Batch_Cost;
            }
            stRVM.Company = db.Companies.FirstOrDefault();
            foreach (var distinctBatch in thisstore.ProductBatches.Distinct(new Models.ProductBatches.ProductComparer()))
            {
                StoreProductsVM productJObject = new StoreProductsVM();

                var batches = thisstore.ProductBatches.Where(batch => batch.Product_ID == distinctBatch.Product_ID);

                var Product_Quantity = batches.Sum(batch => batch.Product_Quantity);
                var Batch_Cost = batches.FirstOrDefault().Batch_Cost;
                var thispor = db.Products.Find(distinctBatch.Product_ID);
                if (Product_Quantity == 0)
                {

                    productJObject.Batch_ID = distinctBatch.Batch_ID;
                    productJObject.Product_ID = distinctBatch.Product_ID;
                    productJObject.Product_Quantity = Product_Quantity;
                    productJObject.Product_Cost = 0;
                    productJObject.Batch_Cost = Batch_Cost;
                    productJObject.ProductCOde = thispor.Prod_Code;
                    productJObject.ProductName = thispor.Prod_Name;
                }
                else
                {
                    var Product_Avg_Cost = decimal.Round(batches.Sum(batch => batch.Product_Cost * batch.Product_Quantity) / Product_Quantity);

                    productJObject.Batch_ID = distinctBatch.Batch_ID;
                    productJObject.Product_ID = distinctBatch.Product_ID;
                    productJObject.Product_Quantity = Product_Quantity;
                    productJObject.Product_Cost = Product_Avg_Cost;
                    productJObject.Batch_Cost = Batch_Cost;
                    productJObject.ProductCOde = thispor.Prod_Code;
                    productJObject.ProductName = thispor.Prod_Name;
                }

                productJObject.SalesProduct = Product.getQuantitySaleProductForStore(distinctBatch.Product_ID, id);
                stRVM.ListofProducts.Add(productJObject);



            }
            return stRVM;
        }

        public static EmployeesrecivableVM EmployeeRecivableForCreation()
        {
            var db = new AccountingContainer();
            EmployeesrecivableVM ERVM = new EmployeesrecivableVM();
            ERVM.EmployeesList = new SelectList(db.Employees, "Emp_Id", "Emp_Name");
            return ERVM;
        }
        public static EmployeesrecivableVM EmployeeRecivableFor(EmployeesrecivableVM ERVM)
        {
            var db = new AccountingContainer();
            EmployeesrecivableVM ERVMs = new EmployeesrecivableVM();
            ERVMs.RecivableDetails = new List<EmployeeRecivableDetailsVM>();
            var thisEMployee = db.Employees.Find(ERVM.EmployeeID);
            var thisAccount = db.Accounts.Where(e => e.Account_Code == thisEMployee.Emp_Code).ToList().FirstOrDefault();
            var thiscompany = db.Companies.FirstOrDefault();
            ERVMs.compnay = thiscompany;
            var start = Convert.ToDateTime(Convert.ToDateTime(ERVM.StartDate).ToShortDateString());
            var end = Convert.ToDateTime(Convert.ToDateTime(ERVM.EndDate).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);
            var thisGLMasters = new List<GLMaster>();
            var thiGlDetailsofNoDate = new List<GLDetail>();
            if (ERVM.StartDate != null && ERVM.EndDate != null)
            {
                thisGLMasters = db.GLMasters.Where(e => e.GL_Date >= start && e.GL_Date <= end).ToList();
            }
            else if (ERVM.StartDate != null && ERVM.EndDate == null)
            {
                thisGLMasters = db.GLMasters.Where(e => e.GL_Date >= start).ToList();
            }
            else if (ERVM.StartDate == null && ERVM.EndDate != null)
            {
                thisGLMasters = db.GLMasters.Where(e => e.GL_Date <= end).ToList();
            }
            else
            {
                int thiaaccountid = Convert.ToInt32(thisAccount.Accounts_Id);
                thiGlDetailsofNoDate = db.GLDetails.Where(e => e.Account_Id == 39 && e.Account_Child_ID == (int)thiaaccountid && e.Account_CHild_Name == thisAccount.Account_Name).ToList();
            }
            ERVMs.EmployeeName = thisAccount.Account_Name;
            ERVMs.EmployeeCode = thisAccount.Account_Code;
            if (thisGLMasters.Count > 0)
            {
                foreach (var detail in thisGLMasters)
                {

                    int thiaaccountid = Convert.ToInt32(thisAccount.Accounts_Id);
                    thiGlDetailsofNoDate = db.GLDetails.Where(e => e.Account_Id == 39 && e.Account_Child_ID == (int)thiaaccountid && e.Account_CHild_Name == thisAccount.Account_Name && e.GLMasterGLMaster_Id == detail.GLMaster_Id).ToList();
                    foreach (var subdetail in thiGlDetailsofNoDate)
                    {
                        EmployeeRecivableDetailsVM ERDVM = new EmployeeRecivableDetailsVM();
                        ERDVM.GLDate = detail.GL_Date.Day + "-" + detail.GL_Date.Month + "-" + detail.GL_Date.Year;
                        ERDVM.Credit = subdetail.GLDetail_Credit;
                        ERDVM.Debit = subdetail.GLDetail_Debit;
                        ERVMs.RecivableDetails.Add(ERDVM);
                    }

                }
            }
            else
            {
                foreach (var subdetail in thiGlDetailsofNoDate)
                {
                    var thisglM = db.GLMasters.Find(subdetail.GLMasterGLMaster_Id);
                    EmployeeRecivableDetailsVM ERDVM = new EmployeeRecivableDetailsVM();
                    ERDVM.GLDate = thisglM.GL_Date.Day + "-" + thisglM.GL_Date.Month + "-" + thisglM.GL_Date.Year;
                    ERDVM.Credit = subdetail.GLDetail_Credit;
                    ERDVM.Debit = subdetail.GLDetail_Debit;
                    ERVMs.RecivableDetails.Add(ERDVM);
                }
            }
            return ERVMs;
        }

        public static WarehousesReportVM MakeWareHouseGet()
        {

            var Stores = new WarehousesReportVM();
            return Stores;

        }
        public static WarehousesReportVM MakeWareHousePost(WarehousesReportVM Stores)
        {

            var db = new AccountingContainer();

            if (Stores.StartDate == null)
            {
                Stores.StartDate = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (Stores.EndDate == null)
            {
                Stores.EndDate = Convert.ToString(new DateTime(2200, 12, 30));

            }
            ReportMasterGeneralBudgetVM mastergeneral = new ReportMasterGeneralBudgetVM();

            mastergeneral.StartDate = Stores.StartDate;
            mastergeneral.EndDate = Stores.EndDate;
            mastergeneral = Reports.ReportGeneralBadget(mastergeneral);
            Stores.Stores = db.ChartOfAccountings.Where(a => a.Account_Code.StartsWith("01010500") && a.BalanceSheetValue != 0).ToList();
            Stores.company = db.Companies.FirstOrDefault();
            return Stores;

        }

        public static AllSalseInvoicesWithOutTaxVM GetAllInvoiceSalseWithBranchForCreation()
        {
            var db = new AccountingContainer();
            var models = new AllSalseInvoicesWithOutTaxVM();
            models.branchList = new SelectList(db.SubLedgers.Where(a => a.Subled_Level == 1 && (a.SubLed_Type == "فرع")), "SubLed_Id", "SubLed_Name");
            return models;
        }

        public static TransfareProjectStoreVM trnasfarePrint(int transID)
        {
            var db = new AccountingContainer();
            TransfareProjectStoreVM TPSVM = new TransfareProjectStoreVM();
            TPSVM.Details = new List<TransfaretoProjectStroeDetailVM>();
            TPSVM.company = db.Companies.FirstOrDefault();
          
            var thistrans = db.TransferToProjectMasters.Find(transID);
            TPSVM.Status = thistrans.Invoice_State;
            if (thistrans.Account_Id > 0)
            {
                var thisaccount = db.ChartOfAccountings.Find(thistrans.Account_Id);
                TPSVM.AccountID = thisaccount.Account_Name;

            }
            TPSVM.TransCOde = thistrans.Trans_Code;
            TPSVM.Description = thistrans.Describtion;
            var thisproj = db.SubLedgers.Find(thistrans.Project_Id);
            var thisproj1 = db.Projects.Where(e => e.Project_Name.Contains(thisproj.SubLed_Name)).ToList().FirstOrDefault();
            if (thisproj != null)
            {
                TPSVM.ProjectID = thisproj1.Project_Name;
            }

            var thisstore = db.Stores.Find(thistrans.Store_ID);
            TPSVM.storeName = thisstore.Store_Name;
            TPSVM.totalAmount = thistrans.Total_Amount;
            TPSVM.transDate = thistrans.Trans_Date.Day + "-" + thistrans.Trans_Date.Month + "-" + thistrans.Trans_Date.Year;
            foreach (var detail in thistrans.TransferToProjectDetails)
            {
                TransfaretoProjectStroeDetailVM TPSD = new TransfaretoProjectStroeDetailVM();
                TPSD.Product_Quantity = detail.Product_Quantity;
                TPSD.ProductID = detail.Product_ID;
                var thisprod = db.Products.Find(detail.Product_ID);
                TPSD.ProductName = thisprod.Prod_Name;
                TPSD.product_COde = thisprod.Prod_Code;
                TPSD.TotalCost = detail.Detail_Total_Cost;
                TPSD.Product_Price = detail.Product_Price;
                TPSVM.Details.Add(TPSD);

            }
            return TPSVM;
        }



        public static AllSalseInvoicesWithOutTaxVM GetAllInvoiceSalseWithBranch(AllSalseInvoicesWithOutTaxVM allSalesInvoicesWithTaxVM)
        {
            var db = new AccountingContainer();
            var salesInvoice = new AllSalseInvoicesWithOutTaxVM();
            if (allSalesInvoicesWithTaxVM.startDate == null)
            {
                allSalesInvoicesWithTaxVM.startDate = Convert.ToString(new DateTime(1995, 1, 1));
            }

            if (allSalesInvoicesWithTaxVM.EndDate == null)
            {
                allSalesInvoicesWithTaxVM.EndDate = Convert.ToString(new DateTime(2200, 12, 30));
            }

            salesInvoice.startDate = allSalesInvoicesWithTaxVM.startDate;
            salesInvoice.EndDate = allSalesInvoicesWithTaxVM.EndDate;

            var start = Convert.ToDateTime(Convert.ToDateTime(allSalesInvoicesWithTaxVM.startDate).ToShortDateString());
            var end = Convert.ToDateTime(Convert.ToDateTime(allSalesInvoicesWithTaxVM.EndDate).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);
            
         
            var resultBreanch = db.SubLedgers.Find(allSalesInvoicesWithTaxVM.branchId);
            if (resultBreanch != null)
            {
                salesInvoice.branchId = resultBreanch.SubLed_Id;
            }

            var salesInvoiceList = db.InvoiceMasters.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= end).ToList();
            var salesRecipt = db.SalesReceiptMasters.Where(e => e.Invoice_Date >= start && e.Invoice_Date <= end).ToList();
            salesInvoice.SalesInvoiceMasterWithOut = salesInvoiceList.Where(a => a.Branch_Id == salesInvoice.branchId && a.Invoice_State == TransactionState.Post).ToList();
            salesInvoice.SalesReciptWithOut = salesRecipt.Where(a => a.Branch_Id == salesInvoice.branchId && a.Invoice_State == TransactionState.Post).ToList();

            foreach (var item in salesInvoice.SalesInvoiceMasterWithOut)
            {
                var totalAfterDisc = item.Total_Amount - item.Total_Tax;
                salesInvoice.totalBeforeTaxSales += totalAfterDisc;
                salesInvoice.totalTaxSales += item.Total_Tax;
                salesInvoice.totalAfterTaxSales += item.Total_Amount;
            }
            foreach (var item in salesInvoice.SalesReciptWithOut)
            {
                var totalAfterDisc = item.Total_Amount - item.Total_Tax;
                salesInvoice.totalBeforeTaxSalesReceipt += totalAfterDisc;
                salesInvoice.totalTaxSalesReceipt += item.Total_Tax;
                salesInvoice.totalAfterTaxSalesReceipt += item.Total_Amount;
            }

            salesInvoice.totalBeforeTax = salesInvoice.totalBeforeTaxSales + salesInvoice.totalBeforeTaxSalesReceipt;
            salesInvoice.totalTax = salesInvoice.totalTaxSales + salesInvoice.totalTaxSalesReceipt;
            salesInvoice.totalAfterTax = salesInvoice.totalAfterTaxSales + salesInvoice.totalAfterTaxSalesReceipt;
            salesInvoice.company = db.Companies.FirstOrDefault();

            return salesInvoice;
        }


        public static ProjectPaymentPrintVM printProPayment(int ProPayID)
        {
            var db = new AccountingContainer();
            var thisProPay = db.ProjectPayments.Find(ProPayID);
            ProjectPaymentPrintVM PYVM = new ProjectPaymentPrintVM();
            PYVM.company = db.Companies.FirstOrDefault();
            PYVM.Expense_Name = thisProPay.Expense_Name;
            PYVM.Pay_Amount = thisProPay.Pay_Amount;
            PYVM.ProjectPay_Code = thisProPay.ProjectPay_Code;
            var thisprojectinsub = db.SubLedgers.Find(thisProPay.Project_Id);

            PYVM.projectName = thisprojectinsub.SubLed_Name;
            var Project = db.Projects.Where(a => a.Project_Name == PYVM.projectName).FirstOrDefault();
            PYVM.branchId = Project.BranchSubledger_Id;
            PYVM.Description = thisProPay.Description;
            PYVM.Date = thisProPay.Date.Day + "-" + thisProPay.Date.Month + "-" + thisProPay.Date.Year;

            if (thisProPay.Supplier_ID != null && thisProPay.Supplier_ID != 0)
            {
                PYVM.Supplier_Id = thisProPay.Supplier_ID.Value;

                PYVM.Supplier_Name = db.Suppliers.Find(PYVM.Supplier_Id).Supp_Name;
            }
            if (thisProPay.Revenue_Id != 0)
            {
                PYVM.Revenue_Name = db.ChartOfAccountings.Find(thisProPay.Revenue_Id).Account_Name;
            }
            if (thisProPay.Tax_Id != null || thisProPay.Tax_Id > 0)
            {
                var thistax = db.Taxes.Find(thisProPay.Tax_Id);
                PYVM.taxName = thistax.Tax_Name + thistax.Tax_Percentage + "%";
                PYVM.Total_Tax = thisProPay.Total_Tax;
            }
            else
            {
                PYVM.taxName = null;
                PYVM.Total_Tax = 0;
            }
            return PYVM;

        }

        public static ProjectReciptPrintVM printProRecipt(int proRectID)
        {
            var db = new AccountingContainer();
            var thisrecipt = db.ProjectRecipts.Find(proRectID);
            ProjectReciptPrintVM PRVM = new ProjectReciptPrintVM();
            PRVM.company = db.Companies.FirstOrDefault();
          
          
           var customer = db.Customers.Where(a => a.Customer_Id == thisrecipt.Customer_ID).FirstOrDefault();
            PRVM.Customer_Name = customer.Customer_Name;
            PRVM.Customer_Code = customer.Customer_Code;

            var thispyamethod = db.PaymentMethods.Find(thisrecipt.Payment_Method);
            PRVM.Payment_Method = thispyamethod.Payment_Type;
            PRVM.Pay_Amount = thisrecipt.Pay_Amount;
            PRVM.ProjectCode = thisrecipt.ProjectCode;
            PRVM.Project_Amount = thisrecipt.Project_Amount;
            if (thisrecipt.Project_End != null)
            {
                PRVM.Project_End = Convert.ToDateTime(thisrecipt.Project_End).Day + "-" + Convert.ToDateTime(thisrecipt.Project_End).Month + "-" + Convert.ToDateTime(thisrecipt.Project_End).Year;

            }
            var thisproject = db.Projects.Where(e => e.Project_Code == thisrecipt.ProjectCode).ToList().FirstOrDefault();
            PRVM.branchId = thisproject.BranchSubledger_Id;
            PRVM.projectName = thisproject.Project_Name;
            PRVM.Project_start = Convert.ToDateTime(thisrecipt.Project_start).Day + "-" + Convert.ToDateTime(thisrecipt.Project_start).Month + "-" + Convert.ToDateTime(thisrecipt.Project_start).Year;
            PRVM.ReciptCode = thisrecipt.ReciptCode;
            PRVM.Recipt_Date = Convert.ToDateTime(thisrecipt.Recipt_Date).Day + "-" + Convert.ToDateTime(thisrecipt.Recipt_Date).Month + "-" + Convert.ToDateTime(thisrecipt.Recipt_Date).Year;
            PRVM.Recipt_Description = thisrecipt.Recipt_Description;
            return PRVM;


        }

        public static SubscriptionPrintVM printNonPaidSubs(int subID)
        {
            var db = new AccountingContainer();

            var subVM = new SubscriptionPrintVM();
            var thissub = db.SubscriptionTemps.Find(subID);
            var type = db.Subscription_Type.Find(thissub.Sub_Type_ID);
            subVM.subtype = type.Subscrip_Name;

            subVM.Sub_Code = thissub.Subscrip_Code;
            
            subVM.deadline = thissub.DeadLine.ToString("yyyy/MM/dd ");
            subVM.starttime = thissub.StartTime.ToString("yyyy/MM/dd ");
            subVM.customerName = thissub.Customer_Name;
            var thiscustomer = db.Customers.Find(thissub.Customer_Id);

            var employee = db.Employees.Find(thissub.Emp_Id);
            if (employee != null)
            {
                subVM.EmployeeName = employee.Emp_Name;
            }
            subVM.branchId = thissub.Subscrip_Pranch;
            subVM.customer_Tax = thiscustomer.Customer_TaxNumber;
            subVM.customer_tel = thiscustomer.Customer_Mobile;
            subVM.customer_fax = thiscustomer.Customer_Fax;
            subVM.customerphone = thiscustomer.Customer_Phone;
            subVM.Customer_Email = thiscustomer.Customer_Mail;
            subVM.customerCode = thiscustomer.Customer_Code;
            subVM.EstimatedAmount = thissub.Estimate_Amount;
            try
            {
                subVM.description = db.Subscription_Type.Find(thissub.Sub_Type_ID).Subscrip_Description;
            }
            catch
            {

            }
            var thisubItems = db.SubscriptionItemsTemps.Where(e => e.SubTempID == thissub.Id).ToList();
            subVM.sub_Date = thissub.Sub_Date;
            subVM.subduration = thissub.Subscrip_Type;
            // subVM.subtype = thissub.Subscrip_Description;
            subVM.CompanyID = thissub.Subscrip_Comp;
            //subVM.TotalDiscount = thissub.Estimate_Amount * (thissub.Discount.Value / 100);
            //var TotalAfterDiscount = thissub.Estimate_Amount - subVM.TotalDiscount;
            subVM.company = db.Companies.Find(thissub.Subscrip_Comp);
            //var thussubtype = db.Subscription_Type.Find(thissub.Sub_Type_ID);
            //if (thussubtype != null)
            //{
            //    subVM.subduration = thussubtype.Durationsubscription;
            //}

            //var thistax = db.Taxes.Find(thissub.Tax_ID);
            //if (thistax != null)
            //{
            //    subVM.totaltax = TotalAfterDiscount * (thistax.Tax_Percentage / 100);
            //    subVM.taxName = thistax.Tax_Name;
            //    subVM.TaxPercentage = thistax.Tax_Percentage;
            //}
            //else
            //{
            //    subVM.totaltax = 0;
            //    subVM.TaxPercentage = 0;
            //    subVM.taxName = "";
            //}
            //subVM.TotalDiscount = subVM.TotalDiscount + thissub.DiscountAllow.Value;
            //subVM.TotalAMountAFterTax = thissub.AmountAfterTax;
            subVM.Master_Discount = (decimal)thissub.Discount;
            subVM.TotalDiscount = (decimal)(subVM.EstimatedAmount * (subVM.Master_Discount / 100));
            subVM.totalAfterdiscount = thissub.Estimate_Amount - subVM.TotalDiscount;
            if (thissub.Tax_ID > 0)
            {
                var thistax = db.Taxes.Find(thissub.Tax_ID);
                subVM.taxName = thistax.Tax_Name;
                subVM.totaltax = (decimal)subVM.totalAfterdiscount * (thistax.Tax_Percentage / 100);
                subVM.TotalAMountAFterTax = (decimal)subVM.totalAfterdiscount + subVM.totaltax;
                subVM.TaxPercentage = thistax.Tax_Percentage;
            }
            else
            {
                subVM.TotalAMountAFterTax = (decimal)subVM.totalAfterdiscount;
            }
            if (thissub.DiscountAllow > 0)
            {
                subVM.fixed_Discount = (decimal)thissub.DiscountAllow;
                subVM.total_After_Fixed = (decimal)(subVM.TotalAMountAFterTax - thissub.DiscountAllow);

            }
            decimal allitemsTotal = 0;
            subVM.details = new List<SubscriptionTempItemsPrintVM>();
            if (thisubItems.Count > 0)
            {
              
                foreach (var item in thisubItems)
                {
                    var detail = new SubscriptionTempItemsPrintVM();
                    detail.Product_Name = item.Item_Name;
                    detail.Product_Price = item.Item_price;
                    detail.Product_Quantity = item.Item_Quantity;
                    detail.Total = (item.Item_Quantity) * (item.Item_price);
                    allitemsTotal = allitemsTotal + detail.Total;
                    subVM.details.Add(detail);
                }
            }
            if (subVM.total_After_Fixed != null)
            {
                subVM.afterAddingItem = allitemsTotal + subVM.total_After_Fixed.Value;
            }
            else
            {
                subVM.total_After_Fixed = subVM.TotalAMountAFterTax;
            }
            return subVM;

        }
        public static List<SubscriptionTempPrintVM> printNonPaidSubss()
        {
            var db = new AccountingContainer();
            var alltemps = db.SubscriptionTemps.Where(s=>s.IsCanceled == false && (s.SubType_State == SubType_State.على_وشك_الانتهاء || s.SubType_State == SubType_State.منتهى)).ToList();
            List<SubscriptionTempPrintVM> listoftemps = new List<SubscriptionTempPrintVM>();
            if (alltemps.Count > 0)
            {
                foreach (var temp in alltemps)
                {
                    var thissub = db.SubscriptionTemps.Find(temp.Id);
                    var thisubItems = db.SubscriptionItemsTemps.Where(e => e.SubTempID == thissub.Id).ToList();
                    decimal allitemsTotal = 0;
                    SubscriptionTempPrintVM subvm = new SubscriptionTempPrintVM();
                    subvm.MyDetails = new List<SubscriptionTempItemsPrintVM>();
                    subvm.company = db.Companies.FirstOrDefault();
                    var thissuctomer = db.Customers.Find(thissub.Customer_Id);
                    subvm.customerEmail = thissuctomer.Customer_Mail;
                    subvm.customerfax = thissuctomer.Customer_Fax;
                    subvm.customerPhone = thissuctomer.Customer_Phone;
                    subvm.customerTel = thissub.tel_customer;
                    subvm.Customer_Name = thissub.Customer_Name;
                    subvm.DayEnd = thissub.DeadLine.Day + "-" + thissub.DeadLine.Month + "-" + thissub.DeadLine.Year;
                    subvm.startTime = thissub.StartTime.Day + "-" + thissub.StartTime.Month + "-" + thissub.StartTime.Year;
                    subvm.SubCOde = thissub.Subscrip_Code;
                    subvm.Description = thissub.Subscrip_Description;
                    if (thisubItems.Count > 0)
                    {
                        foreach (var item in thisubItems)
                        {
                            var detail = new SubscriptionTempItemsPrintVM();
                            detail.Product_Name = item.Item_Name;
                            detail.Product_Price = item.Item_price;
                            detail.Product_Quantity = item.Item_Quantity;
                            detail.Total = (item.Item_Quantity) * (item.Item_price);
                            allitemsTotal = allitemsTotal + detail.Total;
                            subvm.MyDetails.Add(detail);
                        }
                    }
                    subvm.EstimatedAMount = thissub.Estimate_Amount;

                    subvm.Master_Discount = (decimal)thissub.Discount;
                    subvm.totalDiscounts = (subvm.EstimatedAMount * (subvm.Master_Discount / 100));
                    subvm.totalAfterdiscount = thissub.Estimate_Amount - subvm.totalDiscounts;
                    if (thissub.Tax_ID > 0)
                    {
                        var thistax = db.Taxes.Find(thissub.Tax_ID);
                        subvm.TaxName = thistax.Tax_Name;
                        subvm.totalTax = subvm.totalAfterdiscount * (thistax.Tax_Percentage / 100);
                        subvm.totalAftertax = subvm.totalAfterdiscount + subvm.totalTax;
                    }
                    else
                    {
                        subvm.totalAftertax = subvm.totalAfterdiscount;
                    }
                    if (thissub.DiscountAllow > 0)
                    {
                        subvm.fixed_Discount = (decimal)thissub.DiscountAllow;
                        subvm.total_After_Fixed = (decimal)(subvm.totalAftertax - thissub.DiscountAllow);

                    }
                    subvm.totalafterAddingItems = thissub.AmountAfterTax;
                    listoftemps.Add(subvm);
                }
                return listoftemps;
            }
            else
            {
                return listoftemps;
            }

        }

        public static DoingOfEmployeeVM DoingOfEmployee(DoingOfEmployeeVM doingOfEmployeeVM )
        {
            var db = new AccountingContainer();
            var doingOfEmpVM = new DoingOfEmployeeVM();

            if (doingOfEmployeeVM.Start_Date == null)
            {
                doingOfEmployeeVM.Start_Date = Convert.ToString(new DateTime(1995, 1, 1));


            }

            if (doingOfEmployeeVM.End_Date == null)
            {
                doingOfEmployeeVM.End_Date = Convert.ToString(new DateTime(2200, 12, 30));

            }

            var Emp = db.Employees.Find(doingOfEmployeeVM.Emp_Id);

            doingOfEmpVM.End_Date = doingOfEmployeeVM.Start_Date.ToString();
            doingOfEmpVM.Start_Date = doingOfEmployeeVM.End_Date.ToString();
            DateTime start = Convert.ToDateTime(Convert.ToDateTime(doingOfEmployeeVM.Start_Date).ToShortDateString());
            DateTime End = Convert.ToDateTime(Convert.ToDateTime(doingOfEmployeeVM.End_Date).ToString("dd/MM/yyyy")).AddHours(23).AddMinutes(59).AddSeconds(59);
            doingOfEmpVM.UserId = Emp.Reg_ID;
          
            doingOfEmpVM.Emp_Name = Emp.Emp_Name;
            doingOfEmpVM.Emp_Code = Emp.Emp_Code;
            var AllData = db.TransactionBatches.Where(e => e.Date >= start && e.Date <= End && e.UserId == Emp.Reg_ID).ToList();
            doingOfEmpVM.company = db.Companies.Find(Emp.Company_ID);
            doingOfEmpVM.TransactionBatches = AllData;
         
         
            return doingOfEmpVM;
        }
        public static PurchaseInvoiceNewReportVM PrintPurchInvoiceNew(int id)
        {
            var db = new AccountingContainer();
            PurchaseInvoiceNewReportVM master = new PurchaseInvoiceNewReportVM();
            decimal totalBeforeDiscount = 0;
            decimal Masterdiscount = 0;
            decimal totalAfterdiscount = 0;


            var list = db.PurInvoiceMasters.Find(id);
            master.Master_Id = list.Master_Id;
            master.supplierId = list.Supplier_Id;
            master.Invoice_Currency = list.Invoice_Currency;
            master.Invoice_Date = Convert.ToString(list.Invoice_Date);
            master.Due_Date = Convert.ToString(list.Due_Date);
            master.Master_Discount = list.Master_Discount;
            master.Serial_Number = list.Serial_Number;
            master.Total_Amount = list.Total_Amount;
            master.Description = list.Description;
            if (list.PaymentMethod_Id == null || list.PaymentMethod_Id == 0)
            {

            }
            else
            {
                master.PaymentMethod_Id = list.PaymentMethod_Id.Value;
            }
     
            master.ValidateDays = list.Payment_Policy.Value;
            //master.PaymentOfinvoice = list.TotaL_After_Fixed.Value - list.TotalAmountAfMat;
            master.company = db.Companies.Find(list.Company_Id);
            master.afterMatching = list.TotalAmountAfMat;
            master.totalTax = list.Total_Tax;
            if (list.Supplier_Id!=null&&list.Supplier_Id!=0)
            {


            }
            if (list.FixedDiscount > 0)
            {
                master.fixed_Discount = (decimal)list.FixedDiscount;
            }
            else
            {
                master.fixed_Discount = 0;
            }
            if (list.TotaL_After_Fixed > 0)
            {
                master.total_After_Fixed = (decimal)list.TotaL_After_Fixed;
            }
            else
            {
                master.total_After_Fixed = 0;
            }
          

            var result = db.Suppliers.Find(list.Supplier_Id);
            if (result != null)
            {
                master.supplier_Name = result.Supp_Name;
                master.supplierEmail = result.Supp_Mail;
                master.supplierfax = result.Tax_Number;
                master.supplierPhone = result.Supp_Mobile;
                master.supplierTel = result.Supp_Phone;
                master.SupPurchaseNo = list.Supp_Inv_Serial;
            }
            if (list.Emp_Id != null || list.Emp_Id > 0)
            {
                var thisEMployee = db.Employees.Find(list.Emp_Id);
                master.EmployeeEmail = thisEMployee.Emp_Mail;
                master.EmployeeMobile = thisEMployee.Emp_Phone;
                master.EmployeeName = thisEMployee.Emp_Name;
            }
            //if (list.Tax_ID != null || list.Tax_ID > 0)
            //{
            //    var thistax = db.Taxes.Find(list.Tax_ID);
            //    master.TaxName = thistax.Tax_Name + " " + thistax.Tax_Percentage + "%";
            //}
            if (list.Store_ID != null && list.Store_ID > 0)
            {
                var thisstore = db.Stores.Find(list.Store_ID);
                master.StoreName = thisstore.Store_Name;
                master.storeId = thisstore.Store_ID;
            }
            List<PurchaseInvoiceDetailNewRepVM> vmlist = new List<PurchaseInvoiceDetailNewRepVM> { };

            foreach (var item in list.InvoiceDetails)
            {
                decimal detailAfterDiscount = 0;
                var invoice = new PurchaseInvoiceDetailNewRepVM();
               
                if (item.Product_ID > 0)
                {
                    var pod = db.Products.Find(item.Product_ID);
                    invoice.Product_Name = pod.Prod_Name;
                    invoice.Product_Description = pod.Prod_Descrip;
                    invoice.Product_Code = pod.Prod_Code;
                    invoice.Product_Model = pod.Prod_Mark;

                    invoice.Product_Id = (int)item.Product_ID;
                }
                else
                {
                    invoice.Product_Name = db.FixedAssetInformations.Find(item.Asset_Id).FAsset_Description;
                    invoice.Product_Id = (int)item.Asset_Id;
                }
              
                
         
                invoice.Product_Price = item.Product_Price;
                invoice.Product_Quantity = item.Product_Quantity;
                //invoice.Total_Amount_Detail = item.Total_Amount;
                if (item.Product_Tax != null && item.Product_Tax > 0)
                {
                    var thistax = db.Taxes.Find(item.Product_Tax);
                    master.TaxName = thistax.Tax_Name + " " + thistax.Tax_Percentage + "%";
                }
                invoice.amountBeforeTax = invoice.Product_Quantity * invoice.Product_Price;
                totalBeforeDiscount += invoice.amountBeforeTax;
                invoice.discount = invoice.amountBeforeTax * (master.Master_Discount / 100);
                Masterdiscount += invoice.discount;
                detailAfterDiscount = invoice.amountBeforeTax - invoice.discount;
                totalAfterdiscount += detailAfterDiscount;
                invoice.Total_Tax = item.Detail_Total_Tax;
                if (item.Product_ID > 0)
                {
                    invoice.MeasureUnit = db.Products.Find(item.Product_ID).Prod_Unet;
                }
                else
                {
                    invoice.MeasureUnit = " ";
                }
              
                invoice.tax = db.Taxes.Find(item.Product_Tax);
                if (item.Product_ID > 0)
                {
                    var thisporduct = db.Products.Find(item.Product_ID);

                    invoice.productcode = thisporduct.Prod_Code;
                }
              
              
                else if(item.Asset_Id>0)
                {
                   var thisasst = db.FixedAssetInformations.Find(item.Asset_Id);
                    invoice.productcode = thisasst.FAsset_Code;

                }
              
                vmlist.Add(invoice);

            }
            master.totalBeforeDiscount = totalBeforeDiscount;
            master.Masterdiscount = Masterdiscount;
            master.totalAfterdiscount = totalAfterdiscount;
            master.total_cost = list.Total_Cost;
            master.MyDetails = vmlist.ToList();
            return master;
        }
        public static PurchaseOrderVM PrintPurchOrder(int id)
        {
            var db = new AccountingContainer();
            PurchaseOrderVM master = new PurchaseOrderVM();
            decimal totalBeforeDiscount = 0;
            decimal Masterdiscount = 0;
            decimal totalAfterdiscount = 0;


            var list = db.PurchaseOrderMasters.Find(id);
            master.Master_Id = list.Master_Id;
            master.Invoice_Currency = list.Invoice_Currency;
            master.Invoice_Date = Convert.ToString(list.OrderDate);
            master.SupId = list.Supplier_Id;
            master.Master_Discount = list.Master_Discount;
            master.Serial_Number = list.Serial_Number;
            master.Total_Amount = list.Total_Amount;
            master.Description = list.Description;
            if (list.PaymentMethod_Id == null || list.PaymentMethod_Id == 0)
            {

            }
            else
            {
                master.PaymentMethod_Id = list.PaymentMethod_Id.Value;
            }
            if (list.Payment_Policy == null)
            {
                list.Payment_Policy = 0;
            }
            master.ValidateDays = list.Payment_Policy.Value;
            //master.PaymentOfinvoice = list.TotaL_After_Fixed.Value - list.TotalAmountAfMat;
            master.company = db.Companies.Find(list.Company_Id);
           
            master.totalTax = list.Total_Tax;
            if (list.Supplier_Id != null && list.Supplier_Id != 0)
            {


            }
            if (list.FixedDiscount > 0)
            {
                master.fixed_Discount = (decimal)list.FixedDiscount;
            }
            else
            {
                master.fixed_Discount = 0;
            }
            if (list.TotaL_After_Fixed > 0)
            {
                master.total_After_Fixed = (decimal)list.TotaL_After_Fixed;
            }
            else
            {
                master.total_After_Fixed = 0;
            }


            var result = db.Suppliers.Find(list.Supplier_Id);
            if (result != null)
            {
                master.supplier_Name = result.Supp_Name;
                master.supplierEmail = result.Supp_Mail;
                master.supplierfax = result.Tax_Number;
                master.supplierPhone = result.Supp_Mobile;
                master.supplierTel = result.Supp_Phone;
               
            }
            if (list.Emp_Id != null || list.Emp_Id > 0)
            {
                var thisEMployee = db.Employees.Find(list.Emp_Id);
                master.EmployeeEmail = thisEMployee.Emp_Mail;
                master.EmployeeMobile = thisEMployee.Emp_Phone;
                master.EmployeeName = thisEMployee.Emp_Name;
            }
           
            if (list.Store_Id != null && list.Store_Id > 0)
            {
                var thisstore = db.Stores.Find(list.Store_Id);
                master.StoreName = thisstore.Store_Name;
                master.storeId = thisstore.Store_ID;
            }
            List<PurchasesOrderDetailVM> vmlist = new List<PurchasesOrderDetailVM> { };

            foreach (var item in list.PurchaseOrderDetails)
            {
                decimal detailAfterDiscount = 0;
                var invoice = new PurchasesOrderDetailVM();
                if (item.Product_ID > 0)
                {
                    var pod = db.Products.Find(item.Product_ID);
                    invoice.Product_Name = pod.Prod_Name;
                    invoice.Product_Description =pod.Prod_Descrip;
                    invoice.Product_Code = pod.Prod_Code;
                    invoice.Product_Model = pod.Prod_Mark;
                  
                    invoice.Product_Id = (int)item.Product_ID;
                }
                else
                {
                    invoice.Product_Name = db.FixedAssetInformations.Find(item.Asset_Id).FAsset_Description;
                    invoice.Product_Id = (int)item.Asset_Id;
                }



                invoice.Product_Price = item.Product_Price;
                invoice.Product_Quantity = item.Product_Quantity;
                //invoice.Total_Amount_Detail = item.Total_Amount;
                if (item.Product_Tax != null && item.Product_Tax > 0)
                {
                    var thistax = db.Taxes.Find(item.Product_Tax);
                    master.TaxName = thistax.Tax_Name + " " + thistax.Tax_Percentage + "%";
                }
                invoice.amountBeforeTax = invoice.Product_Quantity * invoice.Product_Price;
                totalBeforeDiscount += invoice.amountBeforeTax;
                invoice.discount = invoice.amountBeforeTax * (master.Master_Discount / 100);
                Masterdiscount += invoice.discount;
                detailAfterDiscount = invoice.amountBeforeTax - invoice.discount;
                totalAfterdiscount += detailAfterDiscount;
                invoice.Total_Tax = item.Detail_Total_Tax;
                if (item.Product_ID > 0)
                {
                    invoice.MeasureUnit = db.Products.Find(item.Product_ID).Prod_Unet;
                }
                else
                {
                    invoice.MeasureUnit = " ";
                }

                invoice.tax = db.Taxes.Find(item.Product_Tax);
                if (item.Product_ID > 0)
                {
                    var thisporduct = db.Products.Find(item.Product_ID);

                    invoice.productcode = thisporduct.Prod_Code;
                }


                else if (item.Asset_Id > 0)
                {
                    var thisasst = db.FixedAssetInformations.Find(item.Asset_Id);
                    invoice.productcode = thisasst.FAsset_Code;

                }

                vmlist.Add(invoice);

            }
            master.totalBeforeDiscount = totalBeforeDiscount;
            master.Masterdiscount = Masterdiscount;
            master.totalAfterdiscount = totalAfterdiscount;
            master.total_cost = list.Total_Cost;
            master.MyDetails = vmlist.ToList();
            return master;
        }


        public static LedgerPickerVM GetLedgerTreasuryVM(int treasuryId)
        {
            var db = new AccountingContainer();
            LedgerPickerVM LVM = new LedgerPickerVM();
            var treasury = db.Treasuries.Find(treasuryId);
            var treauryAccount = db.ChartOfAccountings.Where(c => c.Account_Code == treasury.Trea_Code).FirstOrDefault();
            LVM.SubAccount_ID = treauryAccount.Account_Id;
            return LVM;
        }
        public static LedgerListVM TreasuryReport(LedgerPickerVM ledgerPickerVM)
        {
            var db = new AccountingContainer();
            var LedgerData = new LedgerListVM();

            LedgerData = AddingDataInLedger(ledgerPickerVM);
            return LedgerData;
        }

        public static LedgerPickerVM GetLedgerBankVM(int bankId)
        {
            var db = new AccountingContainer();
            LedgerPickerVM LVM = new LedgerPickerVM();
            var bank = db.Banks.Find(bankId);
            var bankAccount = db.ChartOfAccountings.Where(c => c.Account_Code == bank.Bank_Code).FirstOrDefault();
            LVM.SubAccount_ID = bankAccount.Account_Id;
            return LVM;
        }
        public static LedgerListVM BankReport(LedgerPickerVM ledgerPickerVM)
        {
            var db = new AccountingContainer();
            var LedgerData = new LedgerListVM();

            LedgerData = AddingDataInLedger(ledgerPickerVM);
            return LedgerData;
        }

        public static CustomerTransactionFirstDataVM EmployeeTransactionDetails(CustomerTransactionGettingDateVM CVM)
        {
            var db = new AccountingContainer();
            var thisCustomer = new CustomerTransactionFirstDataVM();
            if (CVM.Date == null)
            {
                CVM.Date = Convert.ToString(new DateTime(1995, 1, 1));


            }


            thisCustomer.CustomerDetails = new List<CustomerTransactionVM>();
            thisCustomer.FromDate = CVM.Date;
            var startDategot = Convert.ToDateTime(thisCustomer.FromDate);
            decimal totalBalanceBeforDate = 0;
            decimal CreditBalance = 0;
            decimal debitBalance = 0;
            var cust = db.Employees.Find(CVM.CustomerID);
            thisCustomer.AccountName = cust.Emp_Name;
            var accountSupplier = db.Accounts.Where(a => a.Account_Code == cust.Emp_Code).FirstOrDefault();

            thisCustomer.company = db.Companies.FirstOrDefault();
            //we  will add  all details here  and  then start getting  which  we  want
            List<GLDetail> thisTransList = new List<GLDetail>();

            // var allGlDetails = db.GLDetails.Where(e => e.Account_CHild_Name == cust.Customer_Name).ToList();
            //  var allGLDetailsBeforStartDate = allGlDetails.Where()
            var allGLMBefor_startDate = db.GLDetails.Where(e => e.GLMaster.GL_Date < startDategot && e.Account_Child_ID == accountSupplier.Accounts_Id).ToList();
            var thisdetailBefor = allGLMBefor_startDate;
           

                //var gldetailvm = new GLDetail();
                //gldetailvm = exactDetails;
                //thisTransList.Add(gldetailvm);
               

                if (thisdetailBefor.Count != 0)
                {
                   
                    CreditBalance = thisdetailBefor.Select(a => a.GLDetail_Credit).Sum(); 
                    debitBalance = thisdetailBefor.Select(a => a.GLDetail_Debit).Sum();

              
                    if (debitBalance > CreditBalance)
                    {
                        thisCustomer.BalanceNameBefor = "مدين";


                        totalBalanceBeforDate = debitBalance - CreditBalance;
                        thisCustomer.totalbeforDate = totalBalanceBeforDate;

                    }
                    else if (debitBalance < CreditBalance)
                    {
                        thisCustomer.BalanceNameBefor = "دائن";
                        totalBalanceBeforDate = CreditBalance - debitBalance;
                        thisCustomer.totalbeforDate = totalBalanceBeforDate;
                    }
                    else
                    {
                        thisCustomer.BalanceNameBefor = null;
                        thisCustomer.totalbeforDate = 0;
                        totalBalanceBeforDate = 0;
                    }
                    if (thisCustomer.totalbeforDate < 0)
                    {
                        thisCustomer.totalbeforDate = thisCustomer.totalbeforDate * -1;
                        totalBalanceBeforDate = thisCustomer.totalbeforDate;
                    }


                }
            
            var allGlMinDate = db.GLDetails.Where(e => e.GLMaster.GL_Date >= startDategot&& e.Account_Child_ID == accountSupplier.Accounts_Id).ToList();
           var  GLdetialList = allGlMinDate;


        foreach (var detail in GLdetialList)
                    {
              
                        try
                        {
                            if (detail.GLDetail_Debit > 0)
                            {
                                var thiscustdeb = new CustomerTransactionVM();
                                var thiscustcred = new CustomerTransactionVM();
                           
                            var master = db.GLMasters.Find(detail.GLMasterGLMaster_Id);
                              thiscustcred.Debit_Desc = master.GLType_Id;
                              thiscustdeb.Debit_Desc= master.GLType_Id;
                        if (master.GLCode.StartsWith("PUR"))
                                {
                                    var thisinvoice = db.PurInvoiceMasters.Where(e => e.Serial_Number == master.GLCode).FirstOrDefault();
                                    thiscustdeb.Duedebit = thisinvoice.Due_Date.ToString();
                                    thiscustdeb.Discription = thisinvoice.Description;
                                }
                                if (master.GLCode.StartsWith("GLST"))
                                {
                                    var thisinvoice = db.GLMasters.Where(e => e.GLCode == master.GLCode).FirstOrDefault();
                                    thiscustdeb.Discription = thisinvoice.GL_Desc;
                                }
                                if (master.GLCode.StartsWith("SPRE"))
                                {
                                    var thisinvoice = db.SpentRecipts.Where(e => e.SpentReceipt_Code == master.GLCode).FirstOrDefault();
                            if (thisinvoice == null)
                            {
                                thiscustdeb.Discription = "";
                            }
                            else
                            {
                                thiscustdeb.Discription = thisinvoice.SpentReceipt_Description;
                            }
                                }
                                if (master.GLCode.StartsWith("PAY"))
                                {
                                    var thisinvoice = db.Payments.Where(e => e.Pay_Number == master.GLCode).FirstOrDefault();
                                    thiscustdeb.Discription = thisinvoice.Rec_Description;
                                }
                                     else
                                    {
                            thiscustcred.Credit_COde = master.GLCode;
                         
                            thiscustdeb.Discription = detail.Description_Detail+ "  " + master.GL_Desc;
                        }
                                var detinMaster = db.GLDetails.Where(t => t.GLMasterGLMaster_Id == master.GLMaster_Id && t.GLDetail_Credit > 0).FirstOrDefault();
                                var thischart = db.ChartOfAccountings.Where(u => u.Account_Id == detinMaster.Account_Id).FirstOrDefault();

                                thiscustdeb.Balance_Debit = detail.GLDetail_Debit;
                                thiscustdeb.Debit_Date = master.GL_Date.ToString();
                                thiscustdeb.Balance_Credit = 0;
                                thiscustdeb.Credit_Date = null;
                                thiscustdeb.Duecredit = null;
                                thiscustdeb.Debit_COde = master.GLCode;
                                

                                if (thisCustomer.BalanceNameBefor == "دائن")
                                {
                                    thiscustdeb.totalBalance = totalBalanceBeforDate - thiscustdeb.Balance_Debit;
                                    totalBalanceBeforDate = thiscustdeb.totalBalance;
                                }
                                else
                                {
                                    thiscustdeb.totalBalance = totalBalanceBeforDate + thiscustdeb.Balance_Debit;
                                    totalBalanceBeforDate = thiscustdeb.totalBalance;
                                }

                                thisCustomer.CustomerDetails.Add(thiscustdeb);
                            }
                            else if (detail.GLDetail_Credit > 0)
                            {
                                var thiscustdeb = new CustomerTransactionVM();
                                var thiscustcred = new CustomerTransactionVM();
                                var master = db.GLMasters.Find(detail.GLMasterGLMaster_Id);
                                if (master.GLCode.StartsWith("PUR"))
                                {
                                    var thisinvoice = db.PurInvoiceMasters.Where(e => e.Serial_Number == master.GLCode).FirstOrDefault();
                                    thiscustcred.Duecredit = thisinvoice.Due_Date.ToString();
                                    thiscustcred.Discription = thisinvoice.Description;
                                }
                                if (master.GLCode.StartsWith("GLST"))
                                {
                                    var thisinvoice = db.GLMasters.Where(e => e.GLCode == master.GLCode).FirstOrDefault();
                                    thiscustcred.Discription = thisinvoice.GL_Desc;
                                }
                                if (master.GLCode.StartsWith("SPRE"))
                                {
                                    var thisinvoice = db.SpentRecipts.Where(e => e.SpentReceipt_Code == master.GLCode).FirstOrDefault();
                                    thiscustcred.Discription = thisinvoice.SpentReceipt_Description;
                                }
                                if (master.GLCode.StartsWith("PAY"))
                                {
                                    var thisinvoice = db.Payments.Where(e => e.Pay_Number == master.GLCode).FirstOrDefault();
                                    thiscustcred.Discription = thisinvoice.Rec_Description;
                                }
                                var detinMaster = db.GLDetails.Where(t => t.GLMasterGLMaster_Id == master.GLMaster_Id && t.GLDetail_Debit > 0).FirstOrDefault();
                                var thischart = db.ChartOfAccountings.Where(u => u.Account_Id == detinMaster.Account_Id).FirstOrDefault();

                                thiscustcred.Balance_Debit = 0;
                                thiscustcred.Balance_Credit = detail.GLDetail_Credit;
                                thiscustcred.Credit_Date = master.GL_Date.ToString();
                                thiscustcred.Duedebit = null;
                                thiscustcred.Debit_Date = null;
                                thiscustcred.Credit_COde = master.GLCode;
                                thiscustcred.Credit_Desc = master.GLType_Id;
                                // newledgerCred.totalBalance = totalBalanceBeforDate - newledgerCred.Balance_Credit;
                                if (thisCustomer.BalanceNameBefor == "دائن")
                                {
                                    thiscustcred.totalBalance = totalBalanceBeforDate + thiscustcred.Balance_Credit;
                                    totalBalanceBeforDate = thiscustcred.totalBalance;
                                }
                                else
                                {
                                    thiscustcred.totalBalance = totalBalanceBeforDate - thiscustcred.Balance_Credit;
                                    totalBalanceBeforDate = thiscustcred.totalBalance;
                                }
                                //totalBalanceBeforDate -= newledgerCred.Balance_Credit;
                                thisCustomer.CustomerDetails.Add(thiscustcred);
                            }


                        }
                        catch (Exception)
                        {
                            
                            var thiscustcred = new CustomerTransactionVM();
                            thiscustcred.Duedebit = "";
                            thiscustcred.Balance_Debit = 0;
                            thiscustcred.Balance_Credit = 0;
                            thiscustcred.Credit_Date = "";
                            thiscustcred.Duecredit = "";
                            thiscustcred.Debit_Date = "";
                            thiscustcred.Credit_COde = "";
                            thiscustcred.Credit_Desc = "";
                            thiscustcred.totalBalance = 0;
                            totalBalanceBeforDate = 0;
                            thisCustomer.CustomerDetails.Add(thiscustcred);
                            return thisCustomer;
                        }
                    }

                
            
            return thisCustomer;
        }

        public static List<SalaryReportForBankVM> GetSalaryReportForBank(List<EmployeeSalaryMonth> SalaryList)
        {
            var db = new AccountingContainer();
            List<SalaryReportForBankVM> banlList = new List<SalaryReportForBankVM>();
            var type = db.TypeOfDetailSalaries.Where(a => a.Sal_Type == SalaryTypeValue.يقلل_من_اجمالى_الراتب).Select(a=>a.Id).ToList();
           foreach (var sal in SalaryList)
            {
                SalaryReportForBankVM bankemp = new SalaryReportForBankVM();
                bankemp.SalaryDate = sal.Date;
                var distict = sal.EmpSal_Detail.Select(a => a.Account_Id).Intersect(type).ToList();
                if (distict.Count() > 0)
                {
                    foreach(var decrease in distict)
                    {
                        bankemp.decraseSalary+= sal.EmpSal_Detail.Where(a => a.Account_Id == decrease).Select(a => a.AcountValue).Sum();

                    }


                }
                else
                {
                    bankemp.decraseSalary = 0;
                }



                bankemp.emp = db.Employees.Where(a => a.Emp_Code == sal.Emp_Code).FirstOrDefault();
               
                if (sal.Trims_Pay == null)
                {
                    sal.Trims_Pay = 0;
                }

                bankemp.TotalSalary = sal.TotalSalary - sal.Trims_Pay.Value;
               
                if(sal.EmpSal_Detail.Where(a => a.Account_Id == 2).FirstOrDefault() == null)
                {
                    bankemp.ResidentSalary = 0;
                }
                else
                {
                    bankemp.ResidentSalary = sal.EmpSal_Detail.Where(a => a.Account_Id == 2).Select(a => a.AcountValue).Sum();
                }
                if (sal.EmpSal_Detail.Where(a => a.Account_Id == 1).FirstOrDefault() == null)
                {
                    bankemp.basicSalary = 0;
                }
                else
                {
                    bankemp.basicSalary = sal.EmpSal_Detail.Where(a => a.Account_Id == 1).Select(a => a.AcountValue).Sum();
                }
                bankemp.basicSalary = sal.EmpSal_Detail.Where(a => a.Account_Id == 1).Select(a => a.AcountValue).Sum();
                bankemp.increaseSalary = (sal.TotalSalary+sal.Trims_Pay.Value+bankemp.decraseSalary) - (bankemp.basicSalary + bankemp.ResidentSalary);

                banlList.Add(bankemp);

            }
            return banlList;
        }


    }

}
