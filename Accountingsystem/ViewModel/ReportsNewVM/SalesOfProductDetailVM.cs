using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsNewVM
{
    public class SalesOfProductDetailVM
    {
        
        

        public Product product { get; set; }
        public decimal quantitySaled { get; set; }
        public decimal tatalSaledPrice { get; set; }
        public decimal PrecentageTotalPrice { get; set; }

       public decimal TotalCostOfProduct { get; set; }
        public decimal ProductCost { get; set; }
        public decimal netProfit { get; set; }
        public decimal PrecentageNetProfit { get; set; }
     



    }
}