using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintReveInvoiceMasterVM
    {
        public int Master_Id { get; set; }
        public string Serial_Number { get; set; }
        public string Invoice_Date { get; set; }
        public string Due_Date { get; set; }
        public decimal Master_Discount { get; set; }
        public string Invoice_Currency { get; set; }
        //public int customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public decimal Total_Amount { get; set; }

        public String Invoice_Code { get; set; }
        public List<PrintReveInvoiceDetailsVM> MyDetails { get; set; }
        public decimal totalTax { get; set; }
        public decimal totalBeforeDiscount { get; set; }
        public decimal Masterdiscount { get; set; }
        public decimal totalAfterdiscount { get; set; }
        public Company company { get; set; }
    }
}