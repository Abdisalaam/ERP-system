using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintTransferMentainansDetailsVM
    {
        public int Details_Id { get; set; }
        public decimal Product_Quantity { get; set; }
        public decimal Product_Price { get; set; }
        public decimal Total_Amount { get; set; }
        public decimal Detail_Total_Tax { get; set; }
        public int Product_Tax { get; set; }
        public decimal Detail_Total_Cost { get; set; }
        public int ProductProd_ID { get; set; }

    }
}