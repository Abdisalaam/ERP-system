using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintMasterPOSInvoiceVM
    {
        public int MasterPOSInvoiceId { get; set; }
     
        public string Serial_Number { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public String Invoice_Date { get; set; }
        public string tax { get;set; }
       public int barcode { get; set; }
      public int customerId { get; set; }
        public string customer_Name { get; set; }
        public string customer_Tax { get; set; }
        
        public string pointOfSale  { get; set; }
       
      
        public string  Emp_Name { get; set; }
        public string Currency { get; set; }
        public decimal total { get; set; }

        public decimal Master_Discount { get; set; }
        public decimal PercentgeMasterDiscount { get; set; }
        public decimal totalAfterDiscount { get; set; }
        public decimal totalTax { get; set; }
        public decimal totalAfterTax { get; set; }

        public Company company { get; set; }
        public decimal AllowDiscount { get; set; }

       public decimal totalAfterAllDiscount { get; set; }
       public List<PrintDetailPODVM> POSInvoiceDetail { get; set; }
       public List<PrintPOSPaymentVM> POSPaymentDetail { get; set; }
        public decimal totalPayment { get; set; }
        public string customerPhone { get; set; }
        public string customerTel { get; set; }
        public string customerfax { get; set; }
        public string customerEmail { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeMobile { get; set; }
        public string PaymentMethod_Name { get; set; }
        public string POS_Name { get; set; }
        public string StoreName { get; set; }
        public int POSId { get; set; }
        public int storeId { get; set; }
        public int ? branchIdSub { get; set; }
        public int? branchId { get; set; }
        public List<MasterPOSTaxVM> Taxes { get; set; }
        public decimal NewTaxTotal { get; set; }

        public List<POSReceipt>ReceiptList  { get; set; }
    }
}