using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class CustomerStatisticListVM
    {
        public Company company { get; set; }
        public string StartDate { get; set; }
        public bool haveBallance { get; set; }
        public int? cust_Id { get; set; }
        public int? Supp_Id { get; set; }
        public List<CustomerStatisticVM> List { get; set; }
    }
}