using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accountingsystem.ViewModel
{
    public class SpentReciptVM
    {
        public int SpentReceipt_Id { get; set; }
        public int SpentReceiptOrderId { get; set; }

        public int AccountCredit_Id { get; set; }
      
        public SelectList AccCreditList { get; set; }
       
     
     
        public string SpentReceipt_Code { get; set; }
     
        public decimal SpentReceipt_Value { get; set; }
        public string SpentReceipt_Description { get; set; }
     
        public string SpentReceipt_date { get; set; }
        public Nullable<int> Company_Id { get; set; }
        public SelectList ComList { get; set; }
        public int Currency_Id { get; set; }
        public SelectList CurrencyList { get; set; }
        public TransactionState SpentReceipt_State { get; set; }
        public string Reference { get; set; }

        public Nullable<int> BranchSubledger_Id { get; set; }
        public SelectList BranchList { get; set; }


        public Nullable<int> Parent_AccountDepit_Id { get; set; }
        public SelectList ParentList { get; set; }

       
        public Nullable<int> AccountDebit_Id { get; set; }
  
        public SelectList AccDebitList { get; set; }

        public Nullable<int> Child_AccountDepit_Id { get; set; }
        public SelectList ChildList { get; set; }
       
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
 
       // [Required(ErrorMessage = "حساب الدائن")]
        public Nullable<int>  AccountsubCredit_Id { get; set; }
        public SelectList AccsubCreditList { get; set; }
    }
}