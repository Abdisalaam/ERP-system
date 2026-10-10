using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class ProjectInvoiceDetailsVM
    {
        public int Detail_ID { get; set; }
    
        public string Product_Name { get; set; }
 
        public decimal Product_Price { get; set; }
      
        public int Tax_ID { get; set; }
     
        public SelectList tasList { get; set; }
     
        public decimal Detail_Total_Tax { get; set; }
     
        public decimal Product_Quantity { get; set; }
        public int ProjectInvoiceMasterPInvoiceMaster_ID { get; set; }
        public Nullable<int> masterID { get; set; }
        public decimal Discount { get; set; }
    }
}