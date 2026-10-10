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
    public class CatchReceiptVM
    {
      
       
        public int CatchReceipt_Id { get; set; }
    
        public int AccountCredit_Id { get; set; }
        public SelectList AccCreditList { get; set; }

   
        public int AccountDebit_Id { get; set; }
        public SelectList AccDebitList { get; set; }

        public string CatchReceipt_Code { get; set; }

        public decimal CatchReceipt_Value { get; set; }
        public string CatchReceipt_Description { get; set; }

        public string CatchReceipt_date { get; set; }
        public Nullable<int> Company_Id { get; set; }
        public SelectList ComList { get; set; }
        public int Currency_Id { get; set; }
        public SelectList CurrencyList { get; set; }
        public TransactionState CatchReceipt_State { get; set; }
        public string Reference { get; set; }
       
        public Nullable<int> BranchSubledger_Id { get; set; }
        public SelectList BranchList { get; set; }
        public Nullable<int> Child_AccountCredit_Id { get; set; }
        public SelectList ChildList { get; set; }


        public Nullable<int> SubChild_AccountCredit_Id { get; set; }
        public SelectList SubChildList { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
 

        public int AccountsubDebit_Id { get; set; }
        public SelectList AccsubDebitList { get; set; }
    }
}