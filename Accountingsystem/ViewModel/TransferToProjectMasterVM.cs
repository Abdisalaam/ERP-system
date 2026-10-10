using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class TransferToProjectMasterVM
    {
        public int Master_Id { get; set; }
        
        public string Trans_Code { get; set; }
        public string Describtion { get; set; }
   
        public int Project_Id { get; set; }
        public TransactionState Invoice_State { get; set; }
      
        public int Account_Id { get; set; }
       
        public Nullable<int> Branch_Id { get; set; }
        public string Reference { get; set; }
        public int Company_Id { get; set; }
        public decimal Total_Amount { get; set; }
        public Nullable<decimal> Total_Tax { get; set; }
        public decimal Total_Cost { get; set; }


        public int Store_Id { get; set; }
     
        public string Trans_Date { get; set; }
        public SelectList MyCompanyList { get; set; }
   
        public SelectList ProjList { get; set; }
  
        public SelectList MyStoresList { get; set; }
        public SelectList BranchList { get; set; }

        public SelectList AccountList { get; set; }
        public List<TransferToProjectDetail> myDetailsVM { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        public Nullable<int> Department_ID { get; set; }
        public SelectList DepList { get; set; }
        public Nullable<int> BussinesUnit_ID { get; set; }
        public SelectList BUList { get; set; }
        public Nullable<int> SubLedger_ID { get; set; }
        public SelectList SubList { get; set; }
    }
}