using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class StoreProductsVM
    {
        public int Batch_ID { get; set; }
        public int Product_ID { get; set; }
        public decimal SalesProduct { get; set; }
        public decimal Product_Quantity { get; set; }
        public decimal Product_Cost { get; set; }
        public Nullable<System.DateTime> Batch_Date { get; set; }
        public Nullable<System.DateTime> Batch_ExpireDate { get; set; }
        public decimal Batch_Cost { get; set; }
        public Nullable<int> PurInvoice_ID { get; set; }
        public Nullable<int> Store_ID { get; set; }
        public string ProductCOde { get; set; }
        public string ProductName { get; set; }
    }
}