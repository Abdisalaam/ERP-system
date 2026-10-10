using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class PrintReceiptVM
    {
        public int Receipt_Id { get; set; }
        public int? branchId { get; set; }
        public string Recepit_Code { get; set; }
        public string Receipt_Date { get; set; }
        public Company company { get; set; }
        public string Receipt_Currency { get; set; }
        public string Customer_Name { get; set; }
        public decimal Amount { get; set; }
        public int accountId { get; set; }
        public string Description { get; set; }
        public int EmpId { get; set; }
       public string customer_Code{ get; set; }
        public Customer customer { get; set; }
    }
}