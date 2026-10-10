using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class SubscriptionTransafareVM
    {
        public string TransCode { get; set; }
        public string PayName { get; set; }
        public string StoreName { get; set; }
        public decimal amount { get; set; }
        public string SubledgerName { get; set; }
        public Company company { get; set; }
    }
}