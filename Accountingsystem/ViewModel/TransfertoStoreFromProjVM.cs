using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class TransfertoStoreFromProjVM
    {
        public int Master_Id { get; set; }

        public string Trans_Code { get; set; }
        public string Describtion { get; set; }
        public int Project_Id { get; set; }
    
        public int Account_Id { get; set; }
        public int Branch_Id { get; set; }
        public string Reference { get; set; }
        public int Company_Id { get; set; }
        public decimal Total_Amount { get; set; }
        public Nullable<decimal> Total_Tax { get; set; }
        public decimal Total_Cost { get; set; }
        public int Store_Id { get; set; }
        public string Trans_Date { get; set; }
        public int Details_Id { get; set; }
        public int Product_Quantity { get; set; }
        public decimal Product_Price { get; set; }

        public int Product_Tax { get; set; }
        public decimal Detail_Total_Tax { get; set; }
        public decimal Detail_Total_Cost { get; set; }
        public Nullable<int> Batch_ID { get; set; }
     
        public int Product_ID { get; set; }

    }
}