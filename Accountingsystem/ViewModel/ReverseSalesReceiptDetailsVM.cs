using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class ReverseSalesReceiptDetailsVM
    {
        public int Details_Id { get; set; }
        public decimal Product_Quantity { get; set; }
        public decimal Product_Price { get; set; }
        public decimal Total_Amount { get; set; }
        public int Product_Tax { get; set; }
        public decimal Detail_Total_Tax { get; set; }
        public decimal Detail_Total_Cost { get; set; }
        public int Batch_ID { get; set; }
        public int Product_Id { get; set; }
        public int SalesReceiptMasterTempMaster_Id { get; set; }
        public SelectList TaxList { get; set; }
        public int payment { get; set; }
        public decimal Commition { get; set; }
        public int Master_Id {get;set;}

    }
}