using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Accountingsystem.ViewModel
{
    public class SalesReceiptDetailsVM
    {
        public int Details_Id { get; set; }
        public int Store_Id { get; set; }

        public decimal Product_Quantity { get; set; }
     
        public decimal Product_Price { get; set; }

        public int CustomerID { get; set; }
        public decimal Total_Amount { get; set; }
  
        public int Product_Tax { get; set; }
    
        public decimal Detail_Total_Tax { get; set; }

        public decimal Detail_Total_Cost { get; set; }

        public Nullable<int> Batch_ID { get; set; }
        public int Master_Id { get; set; }

        public int Product_Id { get; set; }
        public SelectList ProducttaxList { get; set; }
        public SelectList BatchList { get; set; }
        public decimal Discount { get; set; }
         public decimal commition { get; set; }

        public int productNotBatched_ID { get; set; }
        public SelectList ProductNotBatchList { get; set; }
    }
}