using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintExpensesVM
    {
        public int Expense_Id { get; set; }
        public string Expense_Code { get; set; }
        public int AccountExpense_Id { get; set; }
        public string Expense_Description { get; set; }
        public decimal Expense_Value { get; set; }
        public string Currency_Id { get; set; }
        public DateTime expensesDate { get; set; }
        public int AccountTreasure_Id { get; set; }
        public Company company { get; set; }
        public string Description { get; set; }
        public string todaydate { get; set; }
        public List<PrintExpenseTempVM> ExpDetails { get; set; }
    }
}