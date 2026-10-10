using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class TransfaretoProjectStroeDetailVM
    {
        public decimal Product_Quantity { get; set; }
        public decimal Product_Price { get; set; }
        public decimal TotalCost { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string product_COde { get; set; }
       
    }
}