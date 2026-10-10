using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintReversePOSInvoiceVM
    {
        public int Master_Id { get; set; }
        public string Serial_Number { get; set; }
        public string Invoice_Date { get; set; }
        public string Due_Date { get; set; }
        public decimal Master_Discount { get; set; }
        public string Invoice_Currency { get; set; }
        public string Invoice_Code { get; set; }
        public string Customer_Name { get; set; }
        public decimal Total_Amount { get; set; }
       
        public String PaymentMethod_Name { get; set; }
        public decimal Commition { get; set; }

        public List<PrintPOSDetailInvoiceVM> MyDetails { get; set; }
        public decimal totalTax { get; set; }
        public decimal totalBeforeDiscount { get; set; }
        public decimal Masterdiscount { get; set; }
        public decimal totalAfterdiscount { get; set; }
        public Company company { get; set; }
        public string customerPhone { get; set; }
        public string customerTel { get; set; }
        public string customerfax { get; set; }
        public string customerEmail { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeMobile { get; set; }
        public string POS_Name { get; set; }
        public string org_Serial { get; set; }
        public string customer_Tax { get; set; }
        public string Description { get; set; }
    }
}