using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class SubscriptionInvoiceReportVM
    {
        public string CustomerNamer { get; set; }
        public string CustomerCode { get; set; }
        public string SubCode { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal EstimatedAmount { get; set; }
        public decimal AfterDiscount { get; set; }
        public decimal TaxAMount { get; set; }
        public decimal TotalAfterTax { get; set; }
        public Company company { get; set; }
        public int empId { get; set; }

    }
}