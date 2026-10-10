using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Accountingsystem.ViewModel
{
    public class ExpenseVM
    {
        public int Expense_Id { get; set; }

      
        public string Expense_Code { get; set; }

       
 
        public Nullable<int> AccountExpense_Id { get; set; }
       
   
        public Nullable<int> AccountTreasure_Id { get; set; }
   
        public int BU_Id { get; set; }
       
        public decimal Expense_Value { get; set; }
     
        public string Expense_Description { get; set; }

        //  [DataType(DataType.Date)]
        public string Expense_date { get; set; }
  
        public int Company_Id { get; set; }
        public TransactionState Expense_State { get; set; }
    
        public int Currency_Id { get; set; }

        public String Refference{ get; set; }
     
       
       
     
        public SelectList CurrencyList { get; set; }
        public SelectList CompanyList { get; set; }
        public SelectList Account_List { get; set; }
        public SelectList BU_List { get; set; }

        public Nullable<int> Asset_Id { get; set; }
        public SelectList AssetList { get; set; }
        public Nullable<decimal> Exchange_Rate { get; set; }
        public SelectList Treasure_List { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
        //مركز التكلفة الجديد

        public Nullable<int> DepartSubledger_Id { get; set; }
   
        public Nullable<int> BussinesSubledger_Id { get; set; }
        public Nullable<int> Subledger_Id { get; set; }
 
        public Nullable<int> BranchSubledger_Id { get; set; }
        public SelectList SubledgerList{ get; set; }
        public SelectList BranchSubList { get; set; }
        public SelectList DepartSubList { get; set; }
        public SelectList BussinessList { get; set; }
        public Nullable<int> RandomValue { get; set; }
        public Nullable<int> selecteTax_ID { get; set; }
        public SelectList taxList { get; set; }
        public int ChildAccount { get; set; }
        public SelectList ChildAcountList { get; set; }
    }
}