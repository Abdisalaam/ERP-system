using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class SalesCusomerReport
    {
        public List<CustomerSalesMasterVM>CustomersSalesReport {get;set;}
        public string startDate { get; set; }
        public string EndDate { get; set; }
        public SelectList customerList { get; set; }
        public Nullable<int> customer_id { get; set; }
        public Company company { get; set; }
    }
}