using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class IncomeStatementVM
    {
        public string From_Date { get; set; }
        public string To_Date { get; set; }
        public Boolean WithOutOFFGLs { get; set; }
        public Decimal Total_Revenue { get; set; }
        public Decimal Total_Expense { get; set; }
        public Decimal Difference { get; set; }
        public List<GLDetailVM> Account_Revenue { get; set; }
        public Decimal collectofRevenue { get; set; }
        public List<GLDetailVM> ExpenseSubLedger { get; set; }
        public decimal salesCost { get; set; }
        public Company company { get; set; }
        public List<GLDetailVM> Account_Expense { get; set; }
    }
}