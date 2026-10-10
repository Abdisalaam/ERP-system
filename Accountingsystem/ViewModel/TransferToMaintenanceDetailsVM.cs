using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class TransferToMaintenanceDetailsVM
    {
        //public int Details_Id { get; set; }
        //public int Product_Quantity { get; set; }
        //public int PAcctual_Quantity { get; set; }
        //public decimal Product_Price { get; set; }
        //public decimal Total_Amount { get; set; }
        //public int Product_Tax { get; set; }
        //public decimal Detail_Total_Tax { get; set; }
        //public decimal Detail_Total_Cost { get; set; }
        //public Nullable<int> Batch_ID { get; set; }
        //public int TransferToMaintenanceMaster_Id { get; set; }
        //public int Product_ID { get; set; }
   
        //public string product_Name { get; set; }
        public int Details_Id { get; set; }
        public decimal Product_Quantity { get; set; }
        public decimal Product_Price { get; set; }
        public decimal Total_Amount { get; set; }
        public int Product_Tax { get; set; }
        public decimal Detail_Total_Tax { get; set; }
        public decimal Detail_Total_Cost { get; set; }
        public Nullable<int> Batch_ID { get; set; }
        public int TransferToMaintenanceMaster_Id { get; set; }
        public int ProductProd_ID { get; set; }
        public SelectList BatchList { get; set; }
        public SelectList ProducttaxList { get; set; }

        public Nullable<int> Department_ID { get; set; }
        public Nullable<int> Subledger_ID { get; set; }
        public Nullable<int> Transfer_BU { get; set; }
        public Nullable<int> Branch_Id { get; set; }

    }
}