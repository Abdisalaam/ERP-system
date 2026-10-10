using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class TransferToProjectDetailVM
    {
        public int Details_Id { get; set; }
        public decimal Product_Quantity { get; set; }
        public string Product_Name { get; set; }
        public decimal Product_Price { get; set; }
        public decimal Total_Amount { get; set; }
        public int Product_Tax { get; set; }
        public decimal Detail_Total_Tax { get; set; }
        public decimal Detail_Total_Cost { get; set; }
        public Nullable<int> Batch_ID { get; set; }
        public int Master_ID { get; set; }
        public int Product_ID { get; set; }
        public SelectList BatchList { get; set; }
        public SelectList ProducttaxList { get; set; }

    }
}