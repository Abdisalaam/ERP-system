using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintPurInvoiceMasterVM
    {
        public int Master_Id { get; set; }
        public string Serial_Number { get; set; }
        public string Invoice_Date { get; set; }
        public string Due_Date { get; set; }
        public decimal Total_Cost { get; set; }
   
        public decimal Master_Discount { get; set; }
        public string Invoice_Currency { get; set; }
        public decimal Total_Amount { get; set; }
        public string Supllier_Name { get; set; }
       
        public string CompanyName { get; set; }
        public string Comp_Address { get; set; }
        public string Comp_phone { get; set; }
        public string Comp_Email { get; set; }
        public decimal TotalAmountAfterMatch { get; set; }
        public decimal PayedAmount { get; set; }
        public decimal TotalTax { get; set; }
        public List<PrintPurInvoiceDetailsVM> MyDetails { get; set; }

        public decimal totalTax { get; set; }
        public decimal totalBeforeDiscount { get; set; }
        public decimal Masterdiscount { get; set; }
        public decimal totalAfterdiscount { get; set; }
        public Company company { get; set; }


    }
}