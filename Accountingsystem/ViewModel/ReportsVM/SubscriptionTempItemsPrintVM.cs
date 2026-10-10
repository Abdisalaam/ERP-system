using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class SubscriptionTempItemsPrintVM
    {
        public string Product_Name { get; set; }

        public decimal Product_Quantity { get; set; }
        public decimal Product_Price { get; set; }
        public decimal Total { get; set; }
    }
}