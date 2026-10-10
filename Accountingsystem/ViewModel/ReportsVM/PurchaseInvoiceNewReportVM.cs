using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class PurchaseInvoiceNewReportVM
    {
        public int Master_Id { get; set; }
        public string Serial_Number { get; set; }
        public string Invoice_Date { get; set; }
        public string Due_Date { get; set; }
        public decimal Master_Discount { get; set; }
        public string Invoice_Currency { get; set; }
        //public int customer_Id { get; set; }
        public string supplier_Name { get; set; }
        public int supplierId { get; set; }
        public decimal Total_Amount { get; set; }
        public Company company { get; set; }
        public string Description { get; set; }

        public decimal PaymentOfinvoice { get; set; }
        public decimal afterMatching { get; set; }
        public decimal totalTax { get; set; }
        public decimal totalBeforeDiscount { get; set; }
        public decimal Masterdiscount { get; set; }
        public decimal totalAfterdiscount { get; set; }
        public decimal fixed_Discount { get; set; }
        public decimal total_After_Fixed { get; set; }
        public decimal total_cost { get; set; }
        public List<PurchaseInvoiceDetailNewRepVM> MyDetails { get; set; }

        ///new attibutes

        public string supplierPhone { get; set; }
        public string supplierTel { get; set; }
        public string supplierfax { get; set; }
        public string supplierEmail { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeMobile { get; set; }


        public string TaxName { get; set; }

        public string StoreName { get; set; }
        public int storeId { get; set; }
        public int ValidateDays { get; set; }
        public Nullable<int> PaymentMethod_Id { get; set; }
        public string SupPurchaseNo { get; set; }
    }
}