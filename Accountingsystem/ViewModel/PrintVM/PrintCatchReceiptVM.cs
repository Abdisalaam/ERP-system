using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintCatchReceiptVM
    {
        public int CatchReceipt_Id { get; set; }
        public string CatchReceipt_Code { get; set; }
        public string CatchReceipt_Date { get; set; }

        public int CatchReceipt_Currency { get; set; }
        //public int customer_Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int AccountCredit_Id { get; set; }
        public Nullable<int> Child_AccountCredit_Id { get; set; }
        public Nullable<int> SubChild_AccountCredit_Id { get; set; }
    }
}