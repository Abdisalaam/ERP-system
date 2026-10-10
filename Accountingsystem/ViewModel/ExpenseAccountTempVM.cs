using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class ExpenseAccountTempVM
    {
        public int Id { get; set; }
        public int Expense_ID { get; set; }
        public bool Tax_Bool { get; set; }
        public Nullable<int> AccountExp_ID { get; set; }
        public Nullable<int> AccountTreasury_ID { get; set; }
        public Nullable<int> SubLedger_ID { get; set; }
        public string ExpDesc { get; set; }
        public decimal Amount { get; set; }
        public int BussinesSubledger_Id { get; set; }
        public int DepartSubledger_Id { get; set; }
        public int BranchSubledger_Id { get; set; }
        
        public int taxid { get; set; }
        public decimal totalDebit { get; set; }
        public decimal totalCredit { get; set; }
        public Nullable<int> RandoumVAlue { get; set; }
        public int childAccount { get; set; }
    }
}