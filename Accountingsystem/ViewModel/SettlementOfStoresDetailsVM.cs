using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class SettlementOfStoresDetailsVM
    {
        public int Settlement_DetailsId { get; set; }
        public decimal Product_Quantity { get; set; }
        public decimal Product_QuantityReal { get; set; }
        public decimal Product_Cost { get; set; }
        public decimal Product_Cost_InBatch { get; set; }
        public string Product_Name { get; set; }
        public int ProductId_In { get; set; }
        public int ProductId_Out { get; set; }
        public int SettlementOfStoresId { get; set; }
        public decimal Total_Cost_Detail { get; set; }
    }
}