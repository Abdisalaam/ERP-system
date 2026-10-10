using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class ProductBatchSelection
    {

        public int Product_Id { get; set; }

        public int Batch_Id { get; set; }

  
        public string ProductName { get; set; }


        public decimal ProductCost { get; set; }
        public int pro_ID { get; set; }
        public SelectList productList { get; set; }
        public SelectList secondProductList { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityExact { get; set; }
        public int storeID { get; set; }
        public string Product_Code { get; set; }

    }
}