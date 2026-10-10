using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accountingsystem.Models;

namespace Accountingsystem.ViewModel
{
    public class TransferToMaintenanceTempVM
    {
        public int Master_Id { get; set; }
        public string Trans_Code { get; set; }
        public string Describtion { get; set; }
        public Nullable<int> Account_Id { get; set; }
        public Nullable<int> Branch_Id { get; set; }
        public Nullable<int> Company_Id { get; set; }
        public Nullable<decimal> Total_Amount { get; set; }
        public Nullable<decimal> Total_Tax { get; set; }
        public Nullable<decimal> Total_Cost { get; set; }
        public Nullable<int> Store_Id { get; set; }
        public Nullable<System.DateTime> Trans_Date { get; set; }
        public Nullable<TransactionState> Invoice_State { get; set; }
        public int Sup_ID { get; set; }

    }
}