using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class ReveInvoiceDetailsVM
    {
        public int Details_Id { get; set; }

     
        public decimal Product_Quantity { get; set; }

     
        public decimal Product_Price { get; set; }


      
        public decimal Total_Amount { get; set; }


    
        public int InvoiceMaster_Id { get; set; }


     
        public decimal Product_Tax { get; set; }

      
        public decimal Detail_Total_Tax { get; set; }


     
        public decimal Detail_Total_Cost { get; set; }
        public int Product_ID { get; set; }
        public SelectList TaxList { get; set; }
        public string Product_Name { get; set; }
    
        public string Product_Code { get; set; }
        public int invoiceId{ get; set; }
    }
}