using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintExpenseTempVM
    {

        public string AccountExpName { get; set; }

        public string AccountTreasuryName { get; set; }

        public string subledgerName { get; set; }
        public string ExpDesc { get; set; }
        public Nullable<int> expid { get; set; }
        public Nullable<int> treasuID { get; set; } 
        public decimal AccountExp_Debit { get; set; }
        public decimal AccountTreat_Credit { get; set; }
        public int counter { get; set; }
        public decimal total_Debit { get; set; }
        public decimal total_Credit {get; set;}
    }
}