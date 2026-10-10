using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class SubscriptionMaintainDateVM
    {
        public string From { get; set; }
        public string to { get; set; }
        public int employeeId { get; set; }

        public SelectList employeeList { get; set; }
        public List<SubscriptionInvoiceReportVM> SubList { get; set; }

        public List<SubscriptionTransafareVM> List { get; set; }
    }
}