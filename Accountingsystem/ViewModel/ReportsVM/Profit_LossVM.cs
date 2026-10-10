using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class Profit_LossVM
    {
        public string Date { get; set; }
        public Decimal Total_Profit { get; set; }
        public Decimal Total_Loss { get; set; }
        public Decimal Difference { get; set; }
        public List<ChartOfAccounting> Account_Revenue { get; set; }

        public List<ChartOfAccounting> Account_Expense { get; set; }

    }
}