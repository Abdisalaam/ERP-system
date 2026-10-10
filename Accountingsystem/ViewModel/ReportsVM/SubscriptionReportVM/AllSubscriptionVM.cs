using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM.SubscriptionReportVM
{
    public class AllSubscriptionVM
    {
        public List<SubscriptionVM> AllSUb { get; set; }
        public List<Subscription> Sublist { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<int> subtype_ID { get; set; }
        public int Subledger_Id { get; set; }
        public Company company { get; set; }
       
    }
}