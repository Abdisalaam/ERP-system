using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class SubInvoiceReportVM
    {
        public string InvoiceDate { get; set; }
        public decimal PayedAmount { get; set; }
        public decimal AmountRequired { get; set; }
        public decimal AmountStill { get; set; }
        public int ? branchId { get; set; }
        public decimal EstimatedAmount { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string description { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public Company Company { get; set; }
        public string InvoiceCode { get; set; }
        public String SubScription_Type { get; set; }
        public DateTime startSub { get; set; }
        public Nullable <DateTime> EndSub { get; set; }
        public String PaymentMethod { get; set; }
        public string SubscriptionCode { get; set; }
        public decimal SubscriptionAmount { get; set; }
        public Currency currency  { get; set; }
    }
}