using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class SupplierInfosVM
    {
        public Company company { get; set; }
        public Supplier sup { get; set; }
        public string Balance { get; set; }
        public string  Email { get; set; }
        public string SupplierPhone { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
    }

}