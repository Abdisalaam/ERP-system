using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class LedgerFromDetailVM
    {
        public decimal Balance_Debit { get; set; }
        public decimal Balance_Credit { get; set; }
        public string Statment_debit { get; set; }
        public string Credit_Date { get; set; }
        public string Debit_Date { get; set; }
        public string Statment_credit { get; set; }
        public string Credit_COde { get; set; }
        public string Debit_COde { get; set; }
        public string Credit_Desc { get; set; }
        public string Debit_Desc { get; set; }
        public decimal totalBalance { get; set; }
        public string childName { get; set; }
    }
}