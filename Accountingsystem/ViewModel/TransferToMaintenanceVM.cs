using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Accountingsystem.Models;

namespace Accountingsystem.ViewModel
{
    public class TransferToMaintenanceVM
    {
        public int Master_Id { get; set; }
       
        public string Trans_Code { get; set; }
 

        public string Describtion { get; set; }
    

        public TransactionState Invoice_State { get; set; }
       
        public int Account_Id { get; set; }
 

        public string Reference { get; set; }
     

        public int Company_Id { get; set; }
    

        public decimal Total_Amount { get; set; }
     

        public Nullable<decimal> Total_Tax { get; set; }
    

        public decimal Total_Cost { get; set; }
   

        public string Trans_Date { get; set; }

        public int Store_ID { get; set; }
     
        public SelectList MyCompanyList { get; set; }
        public SelectList MyStoresList { get; set; }
     
        public SelectList AccountList { get; set; }


     

        public Nullable<int> Department_ID { get; set; }
        public SelectList MyDepartList { get; set; }

 

        public Nullable<int> Subledger_ID { get; set; }
        public SelectList supList { get; set; }


        public Nullable<int> Transfer_BU { get; set; }
        public SelectList BUList { get; set; }
     

        public Nullable<int> Branch_Id { get; set; }
        public SelectList BranchList { get; set; }

    }
}