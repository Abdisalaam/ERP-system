using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsVM.SubscriptionReportVM
{
    public class SubscriptionViewPageVM
    {
        public Nullable <int> subTypeID{get; set;}
        public SelectList SubTypeList { get; set; }
        public SelectList branchList { get; set; }
        public Nullable<int> branchId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}