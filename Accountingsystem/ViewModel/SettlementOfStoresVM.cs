using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Accountingsystem.Models;
namespace Accountingsystem.ViewModel
{
    public class SettlementOfStoresVM
    {
        public int SettlementId { get; set; }
      
      
        public Nullable<int> ProductId_In { get; set; }
        public SelectList MyProductListInStore { get; set; }
    
        public Nullable<int> ProductId_Out { get; set; }
        public SelectList MyProductListOutStore { get; set; }
        public int StoreId { get; set; }
        public int Expence_Id { get; set; }

        public string Settlement_Code { get; set; }
        public string Date { get; set; }
        public decimal Total_Cost { get; set; }

        public Nullable<int> DepartSubledger_Id { get; set; }
      
        public Nullable<int> BussinesSubledger_Id { get; set; }
        public Nullable<int> Subledger_Id { get; set; }
 
        public Nullable<int> BranchSubledger_Id { get; set; }
        public SelectList SubledgerList { get; set; }
        public SelectList BranchSubList { get; set; }
        public SelectList DepartSubList { get; set; }
        public SelectList BussinessList { get; set; }
        public SelectList Expences_List { get; set; }
        public TransactionState Settlement_State { get; set; }
    }
}