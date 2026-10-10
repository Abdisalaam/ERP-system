using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class ProductListVM
    {
        public  List<Product> Products { get; set; }
        public  Company company { get; set; }
        public int ? all { get; set; }

    }
}