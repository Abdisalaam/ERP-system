using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class PurchaseDetailsVM
    {
        public int Details_Id { get; set; }
        public Nullable<int> Product_ID { get; set; }

    
        
        public decimal Product_Quantity { get; set; }


        
        public decimal Product_Price { get; set; }

      
        public decimal Total_Amount { get; set; }

 
        public int InvoiceMaster_Id { get; set; }

     

        public Nullable<int> SelectProduct_Tax { get; set; }
        public SelectList MyProduct_TaxList { get; set; }

 
        public decimal Detail_Total_Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal tax_amount { get; set; }
  
        public Nullable<int> AssetId { get; set; }
        public SelectList MyAssetList { get; set; }

        public int? SelectedProductId { get; set; }
        public SelectList MyProductList { get; set; }
        public string ItemDesc { get; set; }

    }
}