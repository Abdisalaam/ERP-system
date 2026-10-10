using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.ReportsVM
{
    public class ProjectPaymentPrintVM
    {

        public string Expense_Name { get; set; }
        public decimal Pay_Amount { get; set; }
        public string ProjectPay_Code { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Revenue_Name { get; set; }
        public Nullable<decimal> Total_Tax { get; set; }
        public string taxName { get; set; }
        public string projectName { get; set; }
        public string Supplier_Name { get; set; }
        public int Supplier_Id { get; set; }
        public int? branchId { get; set; }
        public Company company { get; set; }
    }
}