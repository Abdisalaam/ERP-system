using Accountingsystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel.PrintVM
{
    public class catchreciptPrintVM
    {
        public int Receipt_Id { get; set; }
        public string Recepit_Code { get; set; }
        public string Receipt_Date { get; set; }

        public string Receipt_Currency { get; set; }
       
        public string Customer_Name { get; set; }
        public string Supplier_Name { get; set; }
        public string Employee_Name { get; set; }
        public string SubledgerName { get; set; }
        public string AccountDeptitId { get; set; }
        public decimal Amount { get; set; }
        public Company company { get; set; }

        public string Description { get; set; }
    }
}