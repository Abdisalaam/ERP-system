using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class InventoryTranferDetailsVm
    {
        public int TransferDetail_Id { get; set; }
        public decimal Product_Qty { get; set; }
        public decimal Detail_TotalCoct { get; set; }
        public decimal TotalAmount { get; set; }
        public int Product_ID { get; set; }
        public decimal product_Cost_Price { get; set; }
    }
}