using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintSubscriptionInvoiceVM
    {
        public string Serial_Number { get; set; }
        public string Subscrip_Code { get; set; }
        public string DeadLine { get; set; }
        public string StartTime { get; set; }
        public decimal Estimate_Amount { get; set; }
        public string Customer_Name { get; set; }
        public Company company { get; set; }
    }
}