using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrentProjectInvoiceMasterVM
    {
        public string Project_Code { get; set; }
        public int customerId { get; set; }
        public int? QRNumber { get; set; }
        public int? branchId { get; set; }
        public string Project_Name { get; set; }
        public string StartTime { get; set; }
        public decimal TotalAmount { get; set; }
        public string Customer_Name { get; set; }
        public List<PrintProjectInvoiceDetailsVM> MyDetails { get; set; }
        public Company company { get; set; }
        public decimal TotalTax { get; set; }
        public String Curriency { get; set; }
        public decimal TotalBeforeTax { get; set; }
        public string invDate{get;set;}
        public string CustomerPhone { get; set; }
        public string CustomerTel { get; set; }
        public string Customerfax { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerTax { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeMobile { get; set; }
        public decimal MasterDiscount { get; set; }
        public decimal MasterDiscountAmount { get; set; }
        
        public decimal FixedDiscount { get; set; }
        public string taxName { get; set; }
    }
}