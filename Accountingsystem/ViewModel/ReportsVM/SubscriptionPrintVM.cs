using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class SubscriptionPrintVM
    {
        public string Sub_Code { get; set; }
        public int? branchId { get; set; }
        public int? QRNumber { get; set; }
        public string deadline { get; set; }
        public string starttime { get; set; }
        public decimal EstimatedAmount { get; set; }
        public string customerName { get; set; }
        public string customerCode { get; set; }
        public string EmployeeName { get; set; }
        public string customerphone { get; set; }
        public string customer_tel { get; set; }
        public string Customer_Email { get; set; }
        public int CustomerID { get; set; }
        public string customer_Tax { get; set; }
        public string customer_fax { get; set; }
        public string taxName { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal totaltax { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAMountAFterTax { get; set; }
        public string subtype { get; set; }
        public int subduration { get; set; }
        public Nullable<decimal> Master_Discount { get; set; }
        public Nullable<decimal> fixed_Discount { get; set; }
        public Nullable<decimal> total_After_Fixed { get; set; }
        public Nullable<decimal> totalAfterdiscount { get; set; }

        public Nullable<DateTime> sub_Date { get; set; }

        public int CompanyID { get; set; }
        public string description { get; set; }
        public Company company { get; set; }
        public decimal afterAddingItem { get; set; }
        public List<SubscriptionTempItemsPrintVM> details{ get; set; }
    }
}