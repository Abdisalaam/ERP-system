using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class RecalPurchInvoiceDetailsVM
    {
    
        public int Details_Id { get; set; }

        public decimal Product_Quantity { get; set; }
 
        public decimal Product_Price { get; set; }
     
        public decimal Total_Amount { get; set; }
        public decimal dicount { get; set; }
        public decimal tax_amount { get; set; }
        public Nullable<int> Asset_Id { get; set; }
   
        public decimal Detail_Total_Tax { get; set; }
  
        public int RecalPurchInvoiceMasterMaster_Id { get; set; }
  
        public decimal Detail_Total_Cost { get; set; }
  
        public Nullable<decimal> Product_Tax { get; set; }
 
        public int Product_ID { get; set; }
        public int store_Quantity { get; set; }
        public int BatchID { get; set; }
        public SelectList PurchaseList { get; set; }
       
        public string Tax_Name { get; set; }

        public string Item_COde { get; set; }
     
        public string Item_Name { get; set; }
        public int selectedPurchaseInvoice { get; set; }

    }
}