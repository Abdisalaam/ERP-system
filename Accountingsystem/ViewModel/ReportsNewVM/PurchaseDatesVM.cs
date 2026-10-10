using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class PurchaseDatesVM
    {
        public string from { get; set; }
        public string to { get; set; }
        public List<PurchaseReportVM> List { get; set; }
        public List<RecallPurchaseReportVM> RecallList { get; set; }
        public Company company { get; set; } 
    }
}