using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Accountingsystem.Models;
using System.ComponentModel;


namespace Accountingsystem.ViewModel.ReportsVM
{
    public class SubscriptionStopVM
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CustomerName { get; set; }
        public SelectList MySubTypeList { get; set; }
        public SubType_State SubType_State { get; set; }
        public List<Subscription> StopSubscription { get; set; }
        public List<SubscriptionTemp> StopSubscriptionTemp { get; set; }
        public Company company { get; set; }
        public int Branch_Id { get; set; }
        public SelectList BranchList { get; set; }
    }
}