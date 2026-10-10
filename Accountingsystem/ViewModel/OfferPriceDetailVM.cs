using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class OfferPriceDetailVM
    {
        public int OfferDet_Id { get; set; }

       
        public int Product_ID { get; set; }

      
        public decimal Product_Quantity { get; set; }
  
        public decimal Product_Price { get; set; }
       
        public int OfferPriceMasterMaster_Id { get; set; }
    
        public string Product_Unit { get; set; }
   
        public decimal Total_Amount { get; set; }
   
        public decimal Detail_Total_Tax { get; set; }
    
        public int Batch_ID { get; set; }
    
        public int product_tax { get; set; }
        public SelectList ProductList { get; set; }
        public SelectList taxList { get; set; }
        public decimal Discount { get; set; }


    }
}