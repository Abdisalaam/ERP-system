using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class SupplierReportWithDaysVM
    {
        public Company company { get; set; }
        public string Balance { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string beforthiry { get; set; }
        public string afterthiry { get; set; }
        public string aftersixty { get; set; }
        public string afterninty { get; set; }
        public string afteronehunderd { get; set; } 
    }
}