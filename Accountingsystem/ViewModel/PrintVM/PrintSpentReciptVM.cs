using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintSpentReciptVM
    {
        public int Receipt_Id { get; set; }
        public string Recepit_Code { get; set; }
        public string Receipt_Date { get; set; }

        public string Receipt_Currency { get; set; }
        //public int customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public decimal Amount { get; set; }
        public string AccountCreditId { get; set; }
        public Company company { get; set; }
        public string Description { get; set; }
        public int Emp_Id { get; set; }
    }
}