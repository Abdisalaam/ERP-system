using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class SubscriptionNonPaidVM
    {
        public string startDate { get; set; }

        public string EndDate { get; set; }
    

        public List<Subscription> SubscribList { get; set; }
        public Company company { get; set; }
        public Customer customer { get; set; }
    }
}