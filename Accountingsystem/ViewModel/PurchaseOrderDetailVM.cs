using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class PurchaseOrderDetailVM
    {
        public int Id { get; set; }

        public decimal Product_Quantity { get; set; }
  
        public decimal Product_Price { get; set; }
        
        public decimal Total_Amount { get; set; }
   
        public int Product_Tax { get; set; }
        public decimal Detail_Total_Tax { get; set; }
        public decimal Detail_Total_Cost { get; set; }
   
        public Nullable<int> Product_ID { get; set; }
        public Nullable<int> Asset_Id { get; set; }
        public int InvoiceMaster_Id { get; set; }
        public SelectList MyProductList { get; set; }
        public SelectList MyProduct_TaxList{ get; set; }
        public decimal Master_Discount { get; set; }
    }
}