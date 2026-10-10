using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintInvoiceMasterVM
    {
        public int Master_Id { get; set; }
        public int customerId { get; set; }
        public int? QRNumber { get; set; }
      
        public string references { get; set; }
        public string Serial_Number { get; set; }

        public int? branchId { get; set; }
        public string Invoice_Date { get; set; }
        public string Due_Date { get; set; }
        public decimal Master_Discount { get; set; }
        public string Invoice_Currency { get; set; }
        //public int customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public decimal Total_Amount { get; set; }
        public Company company { get; set; }

        public decimal PaymentOfinvoice { get; set; }
        public decimal afterMatching { get; set; }
        public decimal totalTax { get; set; }
        public decimal totalBeforeDiscount { get; set; }
        public decimal Masterdiscount { get; set; }
        public decimal totalAfterdiscount { get; set; }
        public decimal fixed_Discount { get; set; }
        public decimal total_After_Fixed { get; set; }
        public List<PrintInvoiceMasterDetailsVM> MyDetails { get; set; }
        public List<InvoiceTaxesPrintVM> Taxes { get; set; }
        public decimal NewTaxTotal { get; set; }

        ///new attibutes

        public string customerPhone { get; set; }
        public string customerAddress { get; set; }
        public string customerTel { get; set; }
        public string customerfax {get;set;}
        public string customerEmail { get; set; }
        public string  EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeMobile { get; set; }
       public string CustomerTax { get; set; }
 
        public string TaxName { get; set; }
        public int storeId { get; set; }

        public string StoreName { get; set; }
        public string ValidateDays { get; set; }
        public string Describtion { get; set; }
        public string Notes { get; set; }
        



    }
}