using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class ProjectBalancesVM
    {
        public Company company { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectName { get; set; }
        public decimal Customer_ballance { get; set; }
        public ProjectState Statu { get; set; }
        public decimal ProjectAMount { get; set; }
        public decimal PastPayedAmount { get; set; }
        public decimal PorjectInvAMount { get; set; }
        public decimal PayedAMount { get; set; }
        public decimal Balance { get; set; }

    }
}